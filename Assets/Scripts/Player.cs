using NUnit.Framework.Constraints;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] int speed = 10;
    [SerializeField] int jumpForce = 10;
    [SerializeField] int jumpCount = 1;
    int maxJumpCount;

    public float coyoteTime = 0.2f;
    public float coyoteTimeCounter = 0f;

    public float jumpBufferTime = 0.2f;
    public float jumpBufferCounter;

    public SpriteRenderer spriteRenderer;

    public bool isJumping;
    public Transform GroundCheckPos;
    public Vector2 GroundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask GroundLayer;
    bool facingRight;
    Animator playerAnim;

    public int RefreshCount = 5;

    [SerializeField] AudioClip jumpSFX;
    [SerializeField] AudioClip deadSFX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxJumpCount = jumpCount;
        rb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();  
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    rb.velocity = Vector2.up * jumpForce;
        //}

        //Debug.Log(rb.linearVelocity);

        if (!GameManager.Instance.Win)
        {
            if (isGrounded())
            {
                coyoteTimeCounter = coyoteTime;
                //isJumping = false;
            }
            else if (!isGrounded())
            {
                coyoteTimeCounter -= Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                jumpBufferCounter = jumpBufferTime;
                //isJumping = true;
                //jumpCount--;
                if (coyoteTimeCounter < 0 & jumpCount > 0)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                    SFXManager.Instance.PlaySound(jumpSFX, transform, 1f);
                    jumpCount = 0;
                }
            }
            else
            {
                jumpBufferCounter -= Time.deltaTime;
                //isJumping = false;
            }

            //if (jumpBufferCounter > 0f & coyoteTimeCounter > 0f & !isJumping)

            //if (jumpBufferCounter > 0f & coyoteTimeCounter > 0f & !isJumping)
            //{
            //    if (isJumping)
            //    //isJumping = true;
            //    Debug.Log("jump");
            //    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            //    isJumping = true;

            //    jumpBufferCounter = 0f;
            //    coyoteTimeCounter = 0f;
            //}
            //else if (jumpBufferCounter > 0f & coyoteTimeCounter > 0f & isJumping & jumpCount > 0)
            //{
            //    Debug.Log("double jump");
            //    jumpCount--;
            //    isJumping = false;
            //    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            //}

            if (isJumping)
            {
                playerAnim.SetBool("IsJumping", true);
            }
            else if (!isJumping)
            {
                playerAnim.SetBool("IsJumping", false);
            }

            if (jumpBufferCounter > 0f) //could be better, if you have time come revisit //moving while jumping increases jumpheight
            {
                if (coyoteTimeCounter > 0f && !isJumping)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                    isJumping = true;
                    SFXManager.Instance.PlaySound(jumpSFX, transform, 1f);

                    jumpBufferCounter = 0f;
                    coyoteTimeCounter = 0f;
                }
                else if (isJumping)
                {
                    if (jumpCount > 0)
                    {
                        jumpCount--;
                        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                        SFXManager.Instance.PlaySound(jumpSFX, transform, 1f);
                        jumpBufferCounter = 0f;
                    }
                }
            }
        }
    }
    private void FixedUpdate()
    {
        if (!GameManager.Instance.Win)
        {
            var horizontalInput = Input.GetAxisRaw("Horizontal");

            rb.linearVelocity = new Vector2(horizontalInput * speed, rb.linearVelocity.y);
            //transform.Translate(Vector2.right * horizontalInput * speed * Time.deltaTime);

            if (horizontalInput != 0)
            {
                playerAnim.SetBool("IsWalking", true);
            }
            else
            {
                playerAnim.SetBool("IsWalking", false);
            }

            if (horizontalInput < 0 && !facingRight)
            {
                Flip();
            }
            else if (horizontalInput > 0 && facingRight)
            {
                Flip();
            }

            if (rb.linearVelocity.y < 0)
            {
                rb.linearVelocity += Vector2.up * Physics2D.gravity.y * 1.25f * Time.deltaTime;
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            playerAnim.SetBool("IsWalking", false);
            playerAnim.SetBool("IsJumping", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            GameManager.Instance.Restart();
        }
    }

    private bool isGrounded()
    {
        if (Physics2D.OverlapBox(GroundCheckPos.position, GroundCheckSize, 0, GroundLayer) & rb.linearVelocity.y <= 0) //kinda ducttape rn
        {
            jumpCount = maxJumpCount;
            isJumping = false;
            return true;
        }
        return false;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!isGrounded()) //stopping effector from applying force when jumping
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(GroundCheckPos.position, GroundCheckSize);
    }
}
