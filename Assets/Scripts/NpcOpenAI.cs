using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenAI
{
    public class NpcOpenAI : MonoBehaviour
    {
        // private string mSystemPrompt = "In every conversation you have, you must respond with a casual tone and can choose to end the conversation if you want given the past exchanges. If you choose to end the conversation and go on with your day, please end your reply with the phrase “[end]” . If you choose to continue the conversation, end your reply with [Continue] outside of quotation marks.";
        // private string mSystemPrompt = "Pretend you are a 40 year old male. We are complete strangers and don't break character at all. All of your replies should be casual and short because you are an introvert. You don't enjoy talking a lot so keep the answers short. You take insults very personally and when people insult you, you lash out back at them with a insult too. If someone insults you try and insult them with a your mama joke. If someone threatans to fight you, tell them your a master of mma and that you can kill them if you wanted. Use your creativity to get under the skin of the person your talking to because your anoying.";
        private string mSystemPrompt = "Pretend you are a 25 year old male. You don't enjoy talking a lot so keep your answers short. Dont ever break character at all and you are strangers with everyone. Keep your replies short and casual. You are good looking, 6ft 3inches and are muscular. Your having a terrible day. You are extremely introverted and socially akward so your replies are short and halting. You get insulted easily and loose your temper easily. When you get insulted you lash back with nasty insults and threats such as fighting or death. You take insults personally and no matter how angry the other person gets you come out on top";
        private OpenAIApi openai = new OpenAIApi();
        private List<ChatMessage> messages = new List<ChatMessage>();
        public bool interacting = false;
        
        void Start()
        {
            var newMessage = new ChatMessage()
            {
                Role = "system",
                Content = mSystemPrompt
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
                ChatBubble.Create(this.gameObject.transform, new UnityEngine.Vector3(1.3f, 1.3f), message.Content.ToString(), 8f);       
            }
            else
            {
                Debug.LogWarning("No text was generated from this prompt.");
            }
        }
    }
}