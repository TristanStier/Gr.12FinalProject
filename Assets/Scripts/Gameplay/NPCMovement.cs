using UnityEngine;

public class NPCMovement : MonoBehaviour
{  
    private Rigidbody2D mRigidBody;
    private BoxCollider2D mBoxCollider;
    private int mXPos;
    private int mTargetXPos;
    public bool mCanMove = true;
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
        mXPos = (int)gameObject.transform.position.x;

        if(mTargetXPos>mXPos && mCanMove == true)
        {
            mRigidBody.velocity = new UnityEngine.Vector2(mSpeed, mRigidBody.velocity.y);
        }
        else if(mTargetXPos<mXPos && mCanMove == true)
        {
            mRigidBody.velocity = new UnityEngine.Vector2(-mSpeed, mRigidBody.velocity.y);  
        }
        else if(mCanMove == true)
        {
            FindNewPatrolPoint();
        }
    }

    private void FindNewPatrolPoint()
    {
        mTargetXPos = (int)Random.Range(-11.0f, 11.0f);
    }
}
