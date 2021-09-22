using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// Handles player physics and movement.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [HideInInspector] public bool inputEnabled = true;

    // ground check
    public Transform groundCheck;
    [SerializeField] private LayerMask whatIsGround;
    const float groundedRadius = .1f;
    const float particleBuffer = .1f;
    private float particleTimer = 0f;
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
    ParticleSystem particles;
    AudioSource jumpSound;

    // death event
    public UnityEvent onDeath;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        particles = groundCheck.GetComponent<ParticleSystem>();
        jumpSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!inputEnabled) return;

        Move(direction);
        particleTimer += Time.deltaTime;

        // death check
        if(transform.position.y < -200)
        {
            onDeath.Invoke();
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (!inputEnabled) return;

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
                    // dust cloud
                    if(particleTimer > particleBuffer)
                    {
                        particles.Play();
                        particleTimer = 0;
                    }
                    
                }
            }
        }
        animator.SetBool("grounded", grounded);
    }

    private void Move(float move)
    {
        moving = move != 0 ? true : false;
        Vector3 targetVelocity = new Vector2(move * speed * (1 + TripManager.Instance.tripFactor), rb.velocity.y);
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
            particles.Play();
            particleTimer = 0;
            //jumpSound.Play();
        }
        else if(!grounded && pressed == 0 && rb.velocity.y > 0)
        {
            //terminate the jump
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
    }

    
    #endregion
}
