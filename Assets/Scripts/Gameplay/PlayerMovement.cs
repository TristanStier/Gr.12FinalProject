using System.Numerics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // References
    private Rigidbody2D mRigidBody;
    [SerializeField] private float mSpeed = 7;
    public bool mCanMove = true;
    private Animator mAnimator;

    private void Awake()
    {
        // Setting References
        mRigidBody = GetComponent<Rigidbody2D>();
        mAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Movement
        float lHorizontalInput = Input.GetAxis("Horizontal");

        if(mCanMove == true)
        {
            mRigidBody.velocity = new UnityEngine.Vector2(lHorizontalInput * mSpeed, mRigidBody.velocity.y);
        }

        // Flipping Character
        if(lHorizontalInput > 0.01f && mCanMove == true)
        {
            transform.localScale = new UnityEngine.Vector3(1, 1, 1);
        }
        else if(lHorizontalInput < -0.01f  && mCanMove == true)
        {
            transform.localScale = new UnityEngine.Vector3(-1, 1, 1);
        }

        mAnimator.SetBool("running?", mRigidBody.velocity.x != 0);
    }
}
