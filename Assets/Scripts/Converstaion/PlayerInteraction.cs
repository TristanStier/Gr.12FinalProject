using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour, IConversation
{
    private OpenAI.NpcOpenAI mNpcAI = null;
    public bool mInteracting = false;
    [SerializeField] private string mPlayerName = "Tristan";
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
        if(Input.GetKey(KeyCode.E) == true && mInteracting == false && mNpcAI != null)
        {
            beginConversation();
            mNpcAI.beginConversation();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "NPC")
        {
            mNpcAI = collision.gameObject.GetComponent<OpenAI.NpcOpenAI>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "NPC")
        {
            mNpcAI = null;
        }
    }

    public void sendMessage()
    {
        if(mNpcAI != null && mInteracting == true)
        {
            mNpcAI.SendRequest(mInputField.text);
        }
    }

    public void beginConversation()
    {
        mInteracting = true;
        this.gameObject.GetComponent<PlayerMovement>().canMove = false;
        this.gameObject.GetComponent<Rigidbody2D>().velocity = new UnityEngine.Vector2(0, 0);
        mInputField.gameObject.SetActive(true);
        mSendButton.gameObject.SetActive(true);
    }

    public void endConversation()
    {
        mInteracting = false;
        this.gameObject.GetComponent<PlayerMovement>().canMove = true;
        mInputField.gameObject.SetActive(false);
        mSendButton.gameObject.SetActive(false);
    }

    public string getName()
    {
        return mPlayerName;
    }

    public void changeName()
    {
        if(mNameField != null)
        {
            mPlayerName = mNameField.text;
        }
    }
}
