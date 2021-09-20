using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // ground check
    public Transform groundCheck;
    [SerializeField] private LayerMask whatIsGround;
    const float groundedRadius = .2f;
    private bool grounded;

    float jumpForce = 40000f;
    float speed = 250f;
    [Range(0, .3f)] [SerializeField] float movementSmoothing = .05f;

    private Vector3 velocity = Vector3.zero;
    private float direction = 0;

    private bool flipped = false;
    private bool moving = false;
    
    // need an animator reference to set parameters
    Animator animator;
    Rigidbody2D rb;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
    }

    private void Update()
    {
        Move(direction);
    }

    private void FixedUpdate()
    {
        bool wasGrounded = grounded;
        grounded = false;
        animator.SetFloat("y_move", rb.velocity.y);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                grounded = true;
                if (!wasGrounded)
                {
                    // this is just a hack to prevent the player from slowing down when landing
                    rb.velocity = new Vector2(direction * speed, 0);
                }
            }
        }
        animator.SetBool("grounded", grounded);
    }

    private void Move(float move)
    {
        moving = move != 0 ? true : false;
        Vector3 targetVelocity = new Vector2(move * speed, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothing);
        animator.SetBool("moving", moving);
        if (move > 0 && flipped)
        {
            Flip();
        }
        else if (move < 0 && !flipped)
        {
            Flip();
        }
    }

    private void Flip()
    {
        flipped = !flipped;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    #region InputHandlers
    public void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 inputVec = ctx.ReadValue<Vector2>();
        direction = inputVec.x;
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        float pressed = ctx.ReadValue<float>();
        if(grounded && pressed > 0)
        {
            grounded = false;
            rb.AddForce(new Vector2(0f, jumpForce));
        }
        else if(!grounded && pressed == 0 && rb.velocity.y > 0)
        {
            //terminate the jump
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
    }

    
    #endregion
}
