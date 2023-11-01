using System.Numerics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // References
    private Rigidbody2D body;
    private Animator anim;
    [SerializeField] private float speed = 10;
    [SerializeField] private float jumpHeight = 5;
    private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask groundLayer;
    public bool canMove = true;

    private void Awake()
    {
        // Setting References
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        // Movement
        float horizontalInput = Input.GetAxis("Horizontal");

        if(canMove == true)
        {
            body.velocity = new UnityEngine.Vector2(horizontalInput * speed, body.velocity.y);
        }

        // Flipping Character
        if(horizontalInput > 0.01f && canMove == true)
        {
            transform.localScale = new UnityEngine.Vector3(1, 1, 1);
        }
        else if(horizontalInput < -0.01f  && canMove == true)
        {
            transform.localScale = new UnityEngine.Vector3(-1, 1, 1);
        }

        // Jumping
        if(Input.GetKey(KeyCode.Space) && isGrounded() && canMove == true)
        {
            Jump();
        }
    }

    private void Jump()
    {
        body.velocity = new UnityEngine.Vector2(body.velocity.x, jumpHeight);
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, UnityEngine.Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
}
