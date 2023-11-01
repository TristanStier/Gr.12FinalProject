using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcOpenAI : MonoBehaviour
{
    public bool interacting = false;

    public void testPrint(string message)
    {
        ChatBubble.Create(this.gameObject.transform, new UnityEngine.Vector3(1.3f, 1.3f), message, 4f);
    }
}