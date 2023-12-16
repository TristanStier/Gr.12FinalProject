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
        public string Role { get; set; }
        public string Content { get; set; }
    }

    public sealed class OpenAiRequest
    {
        public string Model { get; set; }
        public List<OpenAiChatMessage> Messages { get; set; }
        public float? Temperature { get; set; } = 1f;
        public int? MaxTokens { get; set; }

        /*
        public int N { get; set; } = 1;
        public bool Stream { get; set; }
        public string Stop { get; set; }
        public float? PresencePenalty { get; set; } = 0f;
        public float? FrequencyPenalty { get; set; } = 0f;
        public Dictionary<string, string> LogitBias { get; set; }
        public string User { get; set; }
        public string SystemFingerprint { get; set; }
        */
    }
    
     public struct OpenAiResponse
    {
        public ApiError Error { get; set; }
        public string Warning { get; set; }
        public string Model { get; set; }
        public string Id { get; set; }
        public string Object { get; set; }
        public long Created { get; set; }
        public List<OpenAiChatChoice> Choices { get; set; }
        public Usage Usage { get; set; }
        public string SystemFingerprint { get; set; }
    }
    
    public struct OpenAiChatChoice
    {
        public OpenAiChatMessage Message { get; set; }
        public OpenAiChatMessage Delta { get; set; }
        public int? Index { get; set; }
        public string FinishReason { get; set; }
    }
   

    public class NpcOpenAI : MonoBehaviour, IConversation
    {
        [SerializeField] private string mCharacterDescription = "";
        [SerializeField] private string mNpcName = "";
        private string mSystemPrompt = "Your playing the role of a character in a city. You must never break character in any conversation ever. Your answers must be very short and concise to flow naturally with the conversation. When asked very specific questions about yourself I give you the freedom to make up your own answer. You must have emotion in your conversation and answer based off emotion. If someone is being mean to you, be mean back and be less likely to help them etc. I want you to act as human as possible with human emotions. You must respond with a casual tone and can choose to end a conversation at any point if you want given the past exchanges. You must make the correct judgement based off your personality however try and converse when possible. If you choose to end the conversation and go on with your day, please end your reply with the phrase “[End]” . If you choose to continue the conversation, end your reply with [Continue] outside of quotation marks. Please include your name followed by a \": \" before every response.";
        private string mInteractionsSummary = "";
        private OpenAIApi mOpenAI = new OpenAIApi();
        private List<ChatMessage> mMessages = new List<ChatMessage>();
        private OpenAI.NpcOpenAI mNpcAI = null;
        public int mId;
        private PlayerInteraction mPlayerInteraction = null;
        public bool mInteracting = false;

        void Start()
        {
            mId = (int)UnityEngine.Random.Range(-1000000000, 1000000000);
        }

        public async void SendRequest(string pPrompt)
        {
            print("hello");
            var newMessage = new ChatMessage()
            {
                Role = "user",
                Content = mPlayerInteraction.getName() + " says: " + pPrompt
            };

            mMessages.Add(newMessage);

            string s = JsonConvert.SerializeObject(new CreateChatCompletionRequest()
            {
                Model = "gpt-4",
                Messages = mMessages,
                MaxTokens = 120
            });
            
            print(s);
            using (var request = UnityWebRequest.Put("https://api.openai.com/v1/chat/completions", Encoding.UTF8.GetBytes(s)))
            {
                request.method = "POST";
                request.SetRequestHeader("OpenAI-Organization", "org-UHjJR7lHAqlaPyuoqEhC86jm");
                request.SetRequestHeader("Content-Type", ContentType.ApplicationJson);
                request.SetRequestHeader("Authorization", "Bearer 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");

                var asyncOperation = request.SendWebRequest();

                while (!asyncOperation.isDone) await Task.Yield();

                print(request.downloadHandler.text);
                //
                OpenAiResponse data = JsonConvert.DeserializeObject<OpenAiResponse>(request.downloadHandler.text);
            }

 
        /*
            var completionResponse = await mOpenAI.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-4",
                Messages = mMessages,
                MaxTokens = 120
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;  

                mMessages.Add(message);
                string messageString = message.Content.ToString();
                var matchResult = Regex.Match(messageString, @"^([\w\-]+)");
                var firstWord = matchResult.Value;
                var withoutFirstWord = messageString.Substring(firstWord.Length+2);
                string messageToDisplay = withoutFirstWord.Substring(0, withoutFirstWord.LastIndexOf(" ")<0?0:withoutFirstWord.LastIndexOf(" "));
                ChatBubble.Create(this.gameObject.transform, new UnityEngine.Vector3(1.3f, 2.2f), messageToDisplay, 6f);
                string lastWord = message.Content.ToString().Split(' ').Last();
                
                if(mPlayerInteraction != null)
                {
                    if(lastWord == "[End]")
                    {
                        mPlayerInteraction.endConversation();
                        endConversation();
                    }
                }
                
                if(mNpcAI != null)
                {
                    if(lastWord == "[End]")
                    {
                        mNpcAI.endConversation();
                        endConversation();
                    }
                    else
                    {
                        mNpcAI.SendRequest(messageToDisplay);
                    }
                }
            }
            else
            {
                Debug.LogWarning("No text was generated from this prompt.");
            }
            */
        }

        private async void summarizeConversation(List<ChatMessage> iMessages)
        {
            // // INDIVIDUAL INTERACTIONS MEMORY CODE
            // var summarizePrompt = new ChatMessage()
            // {
            //     Role = "user",
            //     Content = "You can break character here and return to your default settings. I want you to summarize your past conversation given the context of your past interactions. Here is a summary/memory of your past interactions outside of your current conversation: " + mInteractionsSummary + ". I now want you to summarize your current conversation given the context of your previous interactions into a summary that will act as a memory for your conversation. It should be written in first person in the format of: \" I met a man named ..., we talking a little bit about this and he left.\". Make sure you only summarize the key talking points and important ideas into a single short and concise summary. Don't include anything else in the summary. It is important to put [Interaction (respective summary number): ] before each summary so I can add it into a list of memories and keep it in chronological order for future reference. You also should express your point of view on the topic in the summary. The summary is only for the past conversation and it must include important specifics rather than general idea so you can reference them in the future. If you ever make up an answer after being asked a specific question about yourself, make sure to include it in the summary and indicate that you said it about yourself."
            // };

            // BIG SUMMARY MEMORY CODE
            var summarizePrompt = new ChatMessage()
            {
                Role = "user",
                Content = "You can break character here and return to your default settings. I want you to summarize your past interactions and your current conversation into one big summary to serve as a memeroy for future encounters. Here is a summary/memory of your past interactions outside of your current conversation: " + mInteractionsSummary + ". I now want you to summarize your current conversation given the previous messages and then mix it in with your past interactions outside of the last converstaion to create 1 large recolection of memories. It should be written in first person in the format of: \" I met a man named ..., we talking a little bit about this and he left.\". Make sure you only summarize the key talking points and important ideas into a single short and concise summary. Don't include anything else in the summary other than your paste interactions with the converstaion you juts had."
            };

            iMessages.Add(summarizePrompt);

            var completionResponse = await mOpenAI.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-4",
                Messages = iMessages,
                MaxTokens = 150
            });
            
            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                mInteractionsSummary = message.Content.ToString(); // Big memories
                // mInteractionsSummary += "\n" + message.Content.ToString(); // Individual memories            
                print(mInteractionsSummary);
            }
        }

        private void OnTriggerEnter2D(Collider2D iCollision)
        {
            if(iCollision.gameObject.tag == "Player")
            {
                mPlayerInteraction = iCollision.gameObject.GetComponent<PlayerInteraction>();
            }
            if(iCollision.gameObject.tag == "NPC")
            {
                mNpcAI = iCollision.gameObject.GetComponent<OpenAI.NpcOpenAI>();

                if(mId>mNpcAI.mId)
                {
                    beginConversation();
                    mNpcAI.beginConversation();
                    mNpcAI.SendRequest("Hello");
                    ChatBubble.Create(this.gameObject.transform, new UnityEngine.Vector3(1.3f, 2.2f), "Hello", 6f);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D iCollision)
        {
            if(iCollision.gameObject.tag == "Player")
            {
                mPlayerInteraction = null;
            }
            if(iCollision.gameObject.tag == "NPC")
            {
                mNpcAI = null;
            }
        }

        public void beginConversation()
        {
            mInteracting = true;
            gameObject.GetComponent<NPCMovement>().mCanMove = false;
            this.gameObject.GetComponent<Rigidbody2D>().velocity = new UnityEngine.Vector2(0, 0);

            var lNewMessage = new ChatMessage()
            {
                Role = "system",
                Content = "Your name is: " + mNpcName + ". You must play a character living in a city. Here is a description of your character: " + mCharacterDescription + "\n" + "Here are specific requirements you must follow at all costs when conversing: " + mSystemPrompt + "Here is a summary of your last interactions, this is essentially your memory: " + mInteractionsSummary + ". You run into someone in the city and you start chatting."
            };

            mMessages.Add(lNewMessage);
        }

        public void endConversation()
        {
            mInteracting = false;
            gameObject.GetComponent<NPCMovement>().mCanMove = true;
            // summarizeConversation(mMessages);
            mMessages.Clear();
        }

        public string getName()
        {
            return name;
        }
    }
}