using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour, IConversation
{
    private OpenAI.NpcOpenAI npc = null;
    public bool interacting = false;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button sendButton;
    
    private void Start()
    {
        inputField.gameObject.SetActive(false);
        sendButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.E) == true && interacting == false && npc != null)
        {
            beginConversation();
            npc.beginConversation();
        }
        
        if(Input.GetKey(KeyCode.Escape) == true && interacting == true && npc != null)
        {
            endConversation();
            npc.endConversation();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "NPC")
        {
            npc = collision.gameObject.GetComponent<OpenAI.NpcOpenAI>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "NPC")
        {
            npc = null;
        }
    }

    public void sendMessage()
    {
        if(npc != null && interacting == true)
        {
            npc.SendRequest(inputField.text);
        }
    }

    public void beginConversation()
    {
        print("Player: Conversation Started");
        interacting = true;
        this.gameObject.GetComponent<PlayerMovement>().canMove = false;
        this.gameObject.GetComponent<Rigidbody2D>().velocity = new UnityEngine.Vector2(0, 0);
        inputField.gameObject.SetActive(true);
        sendButton.gameObject.SetActive(true);
    }

    public void endConversation()
    {
        print("Player: Conversation Ended");
        interacting = false;
        this.gameObject.GetComponent<PlayerMovement>().canMove = true;
        inputField.gameObject.SetActive(false);
        sendButton.gameObject.SetActive(false);
    }
}
