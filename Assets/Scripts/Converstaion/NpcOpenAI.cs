using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

using System.Text;
using System.Threading.Tasks;

namespace OpenAI
{

    public struct OpenAiChatMessage
    {
        public string role { get; set; }
        public string content { get; set; }
    }

    public sealed class OpenAiRequest
    {
        public string model { get; set; }
        public List<OpenAiChatMessage> messages { get; set; }
        public float? temperature { get; set; } = 1f;
        public float? top_p { get; set; } = 1f;
        public int? max_tokens { get; set; }
        public float? presence_penalty { get; set; } = 0f;
        public float? frequency_penalty { get; set; } = 0f;
    }
    
     public struct OpenAiResponse
    {
        public ApiError error { get; set; }
        public string warning { get; set; }
        public string model { get; set; }
        public string id { get; set; }
        public long created { get; set; }
        public List<OpenAiChatChoice> choices { get; set; }
        public Usage usage { get; set; }
        public string system_fingerprint { get; set; }
    }
    
    public struct OpenAiChatChoice
    {
        public OpenAiChatMessage message { get; set; }
        public OpenAiChatMessage delta { get; set; }
        public int? index { get; set; }
        public string finish_reason { get; set; }
    }
   

    public class NpcOpenAI : MonoBehaviour, IConversation
    {
        [SerializeField] private string mCharacterDescription = "";
        [SerializeField] private int mImportance = 8;
        private string mSystemPrompt = "Role-play as a city-dweller with brief, clear responses. Use proper English without metaphors. Remain in character, limit interactions with strangers, and invent personal details as needed. Respond emotionally—reciprocate attitudes. Casual tone encouraged; may exit conversations at discretion with [End] or continue with [Continue]. Please include your name followed by a \": \" before every response. ";
        private string mInteractionsSummary = "";
        private OpenAIApi mOpenAI = new OpenAIApi();
        private List<OpenAiChatMessage> mMessages = new List<OpenAiChatMessage>();
        private IConversation mOther = null;
  
        public async void say(string pPrompt, string senderName)
        {
            var newMessage = new OpenAiChatMessage()
            {
                role = "user",
                content = senderName + " says: " + pPrompt
            };

            mMessages.Add(newMessage);

            string lJson = JsonConvert.SerializeObject(new OpenAiRequest()
            {
                model = "gpt-4",
                messages = mMessages,
                max_tokens = 120
            });
            
            using (var request = UnityWebRequest.Put("https://api.openai.com/v1/chat/completions", Encoding.UTF8.GetBytes(lJson)))
            {
                request.method = "POST";
                request.SetRequestHeader("OpenAI-Organization", "org-UHjJR7lHAqlaPyuoqEhC86jm");
                request.SetRequestHeader("Content-Type", ContentType.ApplicationJson);
                request.SetRequestHeader("Authorization", "Bearer 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");

                var asyncOperation = request.SendWebRequest();

                while (!asyncOperation.isDone) await Task.Yield();
                await Task.Delay(4000);

                OpenAiResponse lResponse = JsonConvert.DeserializeObject<OpenAiResponse>(request.downloadHandler.text);

                if (lResponse.choices != null && lResponse.choices.Count > 0)
                {
                    var message = lResponse.choices[0].message;  
                    print(message);
                    mMessages.Add(message);
                    string messageString = message.content.ToString();
                    var matchResult = Regex.Match(messageString, @"^([\w\-]+)");
                    var firstWord = matchResult.Value;
                    var withoutFirstWord = messageString.Substring(firstWord.Length+2);
                    string messageToDisplay = withoutFirstWord.Substring(0, withoutFirstWord.LastIndexOf(" ")<0?0:withoutFirstWord.LastIndexOf(" "));
                    ChatBubble.Create(gameObject.transform, new UnityEngine.Vector3(1.3f, 2.2f), messageToDisplay, 5f);
                    string lastWord = message.content.ToString().Split(' ').Last();
                    
                    if(lastWord == "[End]")
                    {
                        mOther.endConversation();
                        endConversation();
                    }
                    else
                    {
                        mOther.say(messageToDisplay, name);
                    }
                }
                else
                {
                    Debug.LogWarning("No text was generated from this prompt.");
                }
            }
        }

        private async void summarizeConversation(List<OpenAiChatMessage> iMessages)
        {
            var summarizePrompt = new OpenAiChatMessage()
            {
                role = "user",
                content = "Summarize past interactions and blend with the current conversation into a first-person concise memory recall without breaking character. Include key points and important ideas only. Past summary provided: " + mInteractionsSummary + ". Apply the format: 'I met a person named..., we discussed..., and then..."
            };

            iMessages.Add(summarizePrompt);

            string lJson = JsonConvert.SerializeObject(new OpenAiRequest()
            {
                model = "gpt-4",
                messages = iMessages,
                max_tokens = 120
            });
            
            using (var request = UnityWebRequest.Put("https://api.openai.com/v1/chat/completions", Encoding.UTF8.GetBytes(lJson)))
            {
                request.method = "POST";
                request.SetRequestHeader("OpenAI-Organization", "org-UHjJR7lHAqlaPyuoqEhC86jm");
                request.SetRequestHeader("Content-Type", ContentType.ApplicationJson);
                request.SetRequestHeader("Authorization", "Bearer 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");

                var asyncOperation = request.SendWebRequest();

                while (!asyncOperation.isDone) await Task.Yield();

                OpenAiResponse lResponse = JsonConvert.DeserializeObject<OpenAiResponse>(request.downloadHandler.text);

                if (lResponse.choices != null && lResponse.choices.Count > 0)
                {
                    var message = lResponse.choices[0].message;  
                    mInteractionsSummary = message.content.ToString();
                    print(mInteractionsSummary);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D iCollision)
        {
            if (mOther == null)
            {
               IConversation lOther = iCollision.gameObject.GetComponent<IConversation>();
               if (lOther != null)
               {
                    if (lOther.letsTalk(this))
                    {
                        mOther = lOther;
                        beginConversation();
                        mOther.beginConversation();
                        mOther.say("Hello", name);
                        ChatBubble.Create(this.gameObject.transform, new UnityEngine.Vector3(1.3f, 2.2f), "Hello", 6f);
                    }
               }
            }
        }

        public void beginConversation()
        {
            gameObject.GetComponent<NPCMovement>().mCanMove = false;
            gameObject.GetComponent<Rigidbody2D>().velocity = new UnityEngine.Vector2(0, 0);

            var lNewMessage = new OpenAiChatMessage()
            {
                role = "system",
                content = "Your name is: " + name + ". Here is a description of your character: " + mCharacterDescription + "\n" + "Here are specific requirements you must follow at all costs when conversing: " + mSystemPrompt + "Here is a summary of your last interactions, this is essentially your memory: " + mInteractionsSummary + ". You run into someone in the city and you start chatting."
            };

            mMessages.Add(lNewMessage);
        }

        public void endConversation()
        {
            gameObject.GetComponent<NPCMovement>().mCanMove = true;
            summarizeConversation(mMessages);
            mMessages.Clear();
            mOther = null;
        }
        
        public bool letsTalk(IConversation iOther)
        {
            if (mOther == null)
            {
                if(UnityEngine.Random.Range(0, 100) <= iOther.getImportance())
                {
                    mOther = iOther;
                    return true;
                }
            }
            return false;
        }

        public int getImportance()
        {
            return mImportance;
        }
    }
}