using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private NpcOpenAI npc = null;

    // private void OnTriggerStay2D(Collider2D collision)
    // {
    //     if(Input.GetKey(KeyCode.E) == true && collision.gameObject.tag == "NPC")
    //     {
    //         collision.gameObject.GetComponent<NpcOpenAI>().testPrint(); // Some reason I can't change the name to NPCOpenAi so as of now the naming isnt consistent
    //     }
    // }

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

    public void sendMessage(TMP_InputField message)
    {
        if(npc != null)
        {
            npc.testPrint(message.text);
        }
    }
}
