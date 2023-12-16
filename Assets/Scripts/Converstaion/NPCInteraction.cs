using System;
using System.Collections;
using System.Collections.Generic;
using OpenAI;
using Unity.Mathematics;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    private OpenAI.NpcOpenAI mOtherOpenAI = null;
    private NPCInteraction mOtherInteractionAI = null;
    private bool mInteract = false;
    private OpenAI.NpcOpenAI mMyAI = null;
    [SerializeField] private float mPercentToInteract = 50;
    public int id;

    void Start()
    {
        mMyAI = GetComponent<NpcOpenAI>();
        id = (int)UnityEngine.Random.Range(-1000000000, 1000000000);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "NPC")
        {
            mOtherOpenAI = collision.gameObject.GetComponent<OpenAI.NpcOpenAI>();
        }
    }
}