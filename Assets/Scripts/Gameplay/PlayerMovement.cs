using System.Numerics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // References
    private Rigidbody2D mRigidBody;
    [SerializeField] private float mSpeed = 10;
    [SerializeField] private float mJumpHeight = 5;
    private BoxCollider2D mBoxCollider;
    [SerializeField] private LayerMask mGroundLayer;
    public bool mCanMove = true;

    private void Awake()
    {
        // Setting References
        mRigidBody = GetComponent<Rigidbody2D>();
        mBoxCollider = GetComponent<BoxCollider2D>();
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

        // Jumping
        if(Input.GetKey(KeyCode.Space) && isGrounded() && mCanMove == true)
        {
            Jump();
        }
    }

    private void Jump()
    {
        mRigidBody.velocity = new UnityEngine.Vector2(mRigidBody.velocity.x, mJumpHeight);
    }

    private bool isGrounded()
    {
        RaycastHit2D lRaycastHit = Physics2D.BoxCast(mBoxCollider.bounds.center, mBoxCollider.bounds.size, 0, UnityEngine.Vector2.down, 0.1f, mGroundLayer);
        return lRaycastHit.collider != null;
    }
}
