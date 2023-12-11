using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{  
    private Rigidbody2D mRigidBody;
    private BoxCollider2D mBoxCollider;
    private float mXPos;
    private float mTargetXPos;
    [SerializeField] private float mSpeed = 1;

    void Start()
    {
        // Setting References
        mRigidBody = GetComponent<Rigidbody2D>();
        mBoxCollider = GetComponent<BoxCollider2D>();
        FindNewPatrolPoint();
    }

    void Update()
    {
        mXPos = gameObject.transform.position.x;

        if((int)mTargetXPos>(int)mXPos)
        {
            mRigidBody.velocity = new UnityEngine.Vector2(mSpeed, mRigidBody.velocity.y);
        }
        else if((int)mTargetXPos<(int)mXPos)
        {
            mRigidBody.velocity = new UnityEngine.Vector2(-mSpeed, mRigidBody.velocity.y);  
        }
        else
        {
            Invoke("FindNewPatrolPoint", 3);
            print("waiting");
        }

        print("Target: " + (int)mTargetXPos + ", Current: " + (int)mXPos);
    }

    private void FindNewPatrolPoint()
    {
        mTargetXPos = Random.Range(-11.0f, 11.0f);
        print("done");
    }
}
