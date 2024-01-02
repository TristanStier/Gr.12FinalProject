using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour, IConversation
{
    public static AnimatorController sApearance;

    private List<OpenAI.NpcOpenAI> mNpcAIList = new List<OpenAI.NpcOpenAI>();
    private OpenAI.NpcOpenAI mNpcAI = null;
    [SerializeField] private TMP_InputField mInputField;
    [SerializeField] private Button mSendButton;
    [SerializeField] private TMP_InputField mNameField;
    [SerializeField] private Button mChangeNameButton;
    
    private void Start()
    {
        mInputField.gameObject.SetActive(false);
        mSendButton.gameObject.SetActive(false);

        Animator lAnimator = GetComponent<Animator>();
        if(sApearance != null)
        {
            lAnimator.runtimeAnimatorController = sApearance;
        }
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.E) == true && mNpcAIList.Count>0)
        {
            OpenAI.NpcOpenAI lNpcAI = getClosestAI();
            if (lNpcAI.letsTalk(this))
            {
                mNpcAI = lNpcAI;
                beginConversation();
                mNpcAI.beginConversation();
                
                if(mNpcAI.gameObject.transform.position.x > transform.position.x)
                {
                    transform.localScale = new UnityEngine.Vector3(1, 1, 1);
                    mNpcAI.transform.localScale = new UnityEngine.Vector3(-1, 1, 1);
                }
                else
                {
                    transform.localScale = new UnityEngine.Vector3(-1, 1, 1);
                    mNpcAI.transform.localScale = new UnityEngine.Vector3(1, 1, 1);
                }
            }
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
        if(mNpcAI != null)
        {
            mNpcAI.say(mInputField.text, name);
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
        this.gameObject.GetComponent<PlayerMovement>().mCanMove = false;
        this.gameObject.GetComponent<Rigidbody2D>().velocity = new UnityEngine.Vector2(0, 0);
        mInputField.gameObject.SetActive(true);
        mSendButton.gameObject.SetActive(true);
    }

    public void say(string pPrompt, string senderName)
    {
        
    }

    public void endConversation()
    {
        mNpcAI = null;
        this.gameObject.GetComponent<PlayerMovement>().mCanMove = true;
        mInputField.gameObject.SetActive(false);
        mSendButton.gameObject.SetActive(false);
    }

    public bool letsTalk(IConversation iOther)
    {
        return false;
    }

    public int getImportance()
    {
        return 100;
    }

    public void changeName()
    {
        if(mNameField != null)
        {
            name = mNameField.text;
        }
    }
}
