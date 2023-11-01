using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    private NpcOpenAI npc = null;
    public bool interacting = false;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button sendButton;
    
    private void Update()
    {
        if(Input.GetKey(KeyCode.E) == true && interacting == false && npc != null)
        {
            print("interacting");
            interacting = true;
            npc.interacting = true;
            this.gameObject.GetComponent<PlayerMovement>().canMove = false;
            this.gameObject.GetComponent<Rigidbody2D>().velocity = new UnityEngine.Vector2(0, 0);
            npc.gameObject.GetComponent<Rigidbody2D>().velocity = new UnityEngine.Vector2(0, 0);
        }
        
        if(Input.GetKey(KeyCode.Escape) == true && interacting == true && npc != null)
        {
            print("not interacting");
            interacting = false;
            npc.interacting = false;
            this.gameObject.GetComponent<PlayerMovement>().canMove = true;
        }

        if(interacting == true)
        {
            inputField.gameObject.SetActive(true);
            sendButton.gameObject.SetActive(true);
        }
        else
        {
            inputField.gameObject.SetActive(false);
            sendButton.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "NPC")
        {
            npc = collision.gameObject.GetComponent<NpcOpenAI>();
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
            npc.testPrint(inputField.text);
        }
    }
}
