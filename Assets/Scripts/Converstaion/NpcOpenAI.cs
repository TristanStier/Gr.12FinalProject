using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OpenAI
{
    public class NpcOpenAI : MonoBehaviour, IConversation
    {
        [SerializeField] private string mCharacterDescription = "";
        [SerializeField] private string npcName = "";
        private string mSystemPrompt = "Your playing the role of a character in a city. You must never break character in any conversation ever. Your answers must be very short and concise to flow naturally with the conversation. When asked very specific questions I give you the freedom to make up your own answer. You must respond with a casual tone and can choose to end a conversation at any point if you want given the past exchanges. You must make the correct judgement based off your personality however try and converse when possible. If you choose to end the conversation and go on with your day, please end your reply with the phrase “[End]” . If you choose to continue the conversation, end your reply with [Continue] outside of quotation marks. Please include your name followed by a \": \" before every response.";
        private string interactionsSummary = "";
        private OpenAIApi openai = new OpenAIApi();
        private List<ChatMessage> messages = new List<ChatMessage>();

        private PlayerInteraction playerInteraction = null;
        public bool interacting = false;

        public async void SendRequest(string pPrompt)
        {
            var newMessage = new ChatMessage()
            {
                Role = "user",
                Content = playerInteraction.getName() + " says: " + pPrompt
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
                ChatBubble.Create(this.gameObject.transform, new UnityEngine.Vector3(1.3f, 2f), messageToDisplay, 6f);
                
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

        private async void summarizeConversation(string name, List<ChatMessage> messages)
        {
            var summarizePrompt = new ChatMessage()
            {
                Role = "user",
                Content = "You can break character here and return to your default settings. I want you to summarize your past interactions and your current conversation into one big summary to serve as a memeroy for future encounters. Here is a summary/memory of your past interactions outside of your current conversation: " + interactionsSummary + ". I now want you to summarize your current conversation given the previous messages and then mix it in with your past interactions outside of the last converstaion to create 1 large recolection of memories. It should be written in first person in the format of: \" I met a man named ..., we talking a little bit about this and he left.\". Make sure you only summarize the key talking points and important ideas into a single short and concise summary. Don't include anything else in the summary other than your paste interactions with the converstaion you juts had."
            };

            messages.Add(summarizePrompt);

            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-4",
                Messages = messages,
                MaxTokens = 1000
            });
            
            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                interactionsSummary = message.Content.ToString();
                print(interactionsSummary);
                messages.Clear();
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

            var newMessage = new ChatMessage()
            {
                Role = "system",
                Content = "Your name is: " + npcName + ". You must play a character living in a city. Here is a description of your character: " + mCharacterDescription + "\n" + "Here are specific requirements you must follow at all costs when conversing: " + mSystemPrompt + "Here is a summary of your last interactions, this is essentially your memory: " + interactionsSummary + ". You run into someone in the city and you start chatting."
            };

            messages.Add(newMessage);
        }

        public void endConversation()
        {
            interacting = false;

            summarizeConversation(playerInteraction.getName(), messages);
        }

        public string getName()
        {
            return name;
        }
    }
}