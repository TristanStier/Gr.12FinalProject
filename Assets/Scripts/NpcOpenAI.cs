using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OpenAI
{
    public class NpcOpenAI : MonoBehaviour, IConversation
    {
        [SerializeField] private string mCharacterDescription = "";
        private List<string> pastInteractions = new List<string>();
        private string mSystemPrompt = "You must never break character in any conversation ever. Your answers must be very short and concise to flow naturally with the conversation. When asked very specific questions I give you the freedom to make up your own answer. You must respond with a casual tone and can choose to end a conversation at any point if you want given the past exchanges. You must make the correct judgement based off your personality however try and converse when possible. If you choose to end the conversation and go on with your day, please end your reply with the phrase “[End]” . If you choose to continue the conversation, end your reply with [Continue] outside of quotation marks.";
        private OpenAIApi openai = new OpenAIApi();
        private List<ChatMessage> messages = new List<ChatMessage>();

        private PlayerInteraction playerInteraction = null;
        public bool interacting = false;
        
        void Start()
        {
            var newMessage = new ChatMessage()
            {
                Role = "system",
                Content = "You must play a character living in a city. Here is a description of your character: " + mCharacterDescription + "\n" + "Here are specific requirements you must follow at all costs when conversing: " + mSystemPrompt
            };

            messages.Add(newMessage);
        }

        public async void SendRequest(string pPrompt)
        {
            var newMessage = new ChatMessage()
            {
                Role = "user",
                Content = pPrompt
            };

            messages.Add(newMessage);

            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-4",
                Messages = messages,
                MaxTokens = 120
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;    
                messages.Add(message);
                string messageString = message.Content.ToString();
                string messageToDisplay = messageString.Substring(0, messageString.LastIndexOf(" ")<0?0:messageString.LastIndexOf(" "));
                ChatBubble.Create(this.gameObject.transform, new UnityEngine.Vector3(1.3f, 1.5f), messageToDisplay, 5f);
                
                string lastWord = message.Content.ToString().Split(' ').Last();
                if(lastWord == "[End]")
                {
                    playerInteraction.endConversation();
                    endConversation();
                }
            }
            else
            {
                Debug.LogWarning("No text was generated from this prompt.");
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.tag == "Player")
            {
                playerInteraction = collision.gameObject.GetComponent<PlayerInteraction>();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if(collision.gameObject.tag == "Player")
            {
                playerInteraction = null;
            }
        }

        public void beginConversation()
        {
            interacting = true;
            this.gameObject.GetComponent<Rigidbody2D>().velocity = new UnityEngine.Vector2(0, 0);
        }

        public void endConversation()
        {
            interacting = false;
        }
    }
}