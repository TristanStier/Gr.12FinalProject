using UnityEngine;

public class NPCMovement : MonoBehaviour
{  
    private Rigidbody2D mRigidBody;
    private BoxCollider2D mBoxCollider;
    private int mXPos;
    private int mTargetXPos;
    [HideInInspector] public bool mCanMove = true;
    [SerializeField] private float mSpeed = 2;
    [SerializeField] private bool mShouldWander = true;
    [SerializeField] private float mWanderMaxX = 110;
    [SerializeField] private float mWanderMinX = -110;
    private Animator mAnimator;

    void Start()
    {
        // Setting References
        mRigidBody = GetComponent<Rigidbody2D>();
        mBoxCollider = GetComponent<BoxCollider2D>();
        mAnimator = GetComponent<Animator>();
        FindNewPatrolPoint();
    }

    void Update()
    {
        if(mShouldWander)
        {
            mXPos = (int)gameObject.transform.position.x;

            if(mTargetXPos>mXPos && mCanMove == true)
            {
                mRigidBody.velocity = new UnityEngine.Vector2(mSpeed, mRigidBody.velocity.y);
                transform.localScale = new UnityEngine.Vector3(1, 1, 1);
            }
            else if(mTargetXPos<mXPos && mCanMove == true)
            {
                mRigidBody.velocity = new UnityEngine.Vector2(-mSpeed, mRigidBody.velocity.y); 
                transform.localScale = new UnityEngine.Vector3(-1, 1, 1); 
            }
            else if(mCanMove == true)
            {
                FindNewPatrolPoint();
            }

            mAnimator.SetBool("running?", mCanMove);
        }
    }

    private void FindNewPatrolPoint()
    {
        mTargetXPos = (int)Random.Range(mWanderMinX, mWanderMaxX);
    }
}
