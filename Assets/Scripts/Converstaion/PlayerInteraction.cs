using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour, IConversation
{
    private List<OpenAI.NpcOpenAI> mNpcAIList = new List<OpenAI.NpcOpenAI>();
    private OpenAI.NpcOpenAI mNpcAI = null;
    public bool mInteracting = false;
    [SerializeField] private TMP_InputField mInputField;
    [SerializeField] private Button mSendButton;
    [SerializeField] private TMP_InputField mNameField;
    [SerializeField] private Button mChangeNameButton;
    
    private void Start()
    {
        mInputField.gameObject.SetActive(false);
        mSendButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.E) == true && mInteracting == false && mNpcAIList.Count>0)
        {
            beginConversation();
            mNpcAI = getClosestAI();
            mNpcAI.beginConversation();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "NPC")
        {
            mNpcAIList.Add(collision.gameObject.GetComponent<OpenAI.NpcOpenAI>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "NPC")
        {
            mNpcAIList.Remove(collision.gameObject.GetComponent<OpenAI.NpcOpenAI>());
        }
    }

    public void sendMessage()
    {
        if(mNpcAIList.Count>0 && mInteracting == true)
        {
            mNpcAI.SendRequest(mInputField.text, name);
        }
    }

    private OpenAI.NpcOpenAI getClosestAI()
    {
        if(mNpcAIList.Count == 1)
        {
            return mNpcAIList[0];
        }
        else
        {
            float lLowestDist = Mathf.Infinity;
            OpenAI.NpcOpenAI lClosestNPC = mNpcAIList[0];

            for(int i=0; i<mNpcAIList.Count; i++)
            {
    
                float lDistance = Vector3.Distance(mNpcAIList[i].transform.position, transform.position);
    
                if (lDistance<lLowestDist)
                {
                    lLowestDist = lDistance;
                    lClosestNPC = mNpcAIList[i];
                }
            }

            return lClosestNPC;
        }
    }

    public void beginConversation()
    {
        mInteracting = true;
        this.gameObject.GetComponent<PlayerMovement>().mCanMove = false;
        this.gameObject.GetComponent<Rigidbody2D>().velocity = new UnityEngine.Vector2(0, 0);
        mInputField.gameObject.SetActive(true);
        mSendButton.gameObject.SetActive(true);
    }

    public void endConversation()
    {
        mInteracting = false;
        this.gameObject.GetComponent<PlayerMovement>().mCanMove = true;
        mInputField.gameObject.SetActive(false);
        mSendButton.gameObject.SetActive(false);
    }

    public void changeName()
    {
        if(mNameField != null)
        {
            name = mNameField.text;
        }
    }
}
