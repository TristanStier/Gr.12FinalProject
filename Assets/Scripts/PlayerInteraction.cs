using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(Input.GetKey(KeyCode.E) == true && collision.gameObject.tag == "NPC")
        {
            collision.gameObject.GetComponent<NpcOpenAI>().testPrint(); // Some reason I can't change the name to NPCOpenAi so as of now the naming isnt consistent
        }
    }
}
