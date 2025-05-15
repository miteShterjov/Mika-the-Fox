using System;
using System.Collections;
using NUnit.Framework;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement Settings")]
    [SerializeField] private float moveSpeed = 5f; // Normal speed
    [SerializeField] private float climbSpeed = 3f;
    [SerializeField] private float sprintSpeed = 10f; // Sprint speed
    [SerializeField] private float jumpForce = 8f; // Jump force
    [SerializeField] private int maxJumpCount = 2; // Number of jumps allowed (double jump)
    [SerializeField] private Transform groundCheck; // Position to check for ground
    [SerializeField] private LayerMask groundLayer; // Layer mask for ground detection
    [SerializeField] private float groundCheckRadius = 0.5f; // Radius for ground check
    [SerializeField] private float staminaRefreshRate = 10f; // Rate at which stamina regenerates
    [SerializeField] private float staminaDrainRate = 20f; // Rate at which stamina drains while sprinting

    private Rigidbody2D rb; // Reference to the Rigidbody2D component
    private Animator animator; // Reference to the Animator component
    private float horizontalInput; // Horizontal input value
    private int jumpCount; // Current jump count
    private bool isFacingRight = true; // Track if the player is facing right
    private float originalMoveSpeed; // Store the original move speed
    private bool isSprinting; // Track if the player is sprinting
    private float verticalInput; // Vertical input value for climbing
    private bool isOnStairs; // Track if the player is climbing
    private bool isCrouching; // Track if the player is crouching

    public bool IsFacingRight { get => isFacingRight; set => isFacingRight = value; }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        jumpCount = maxJumpCount;
        originalMoveSpeed = moveSpeed;
    }

    void Update()
    {
        // Handle player movement
        OnMove(new InputAction.CallbackContext());
        if (isOnStairs) OnClimb(new InputAction.CallbackContext());

        if (horizontalInput > 0 && !IsFacingRight)
        {
            Flip();
        }
        else if (horizontalInput < 0 && IsFacingRight)
        {
            Flip();
        }

        // Update player animation based on movement
        UpdatePlayerAnimation();

        // Handle player sprinting, stamina, and speed
        if (isSprinting && Player.Instance.Stamina > 0)
        {
            moveSpeed = sprintSpeed; // Set sprint speed
            DrainStamina(); // Drain stamina while sprinting 

            if (Player.Instance.Stamina <= 0)
            {
                Player.Instance.Stamina = 0; // Prevent stamina from going negative
                isSprinting = false; // Stop sprinting if stamina is depleted
                moveSpeed = originalMoveSpeed; // Reset to original speed
            }
        }
        else if (Player.Instance.Stamina < Player.Instance.MaxStamina)
        {
            RefreshStamina(); // Regenerate stamina when not sprinting
        }
    }


    public void Move(InputAction.CallbackContext context)
    {
        horizontalInput = context.ReadValue<Vector2>().x;
        print("Horizontal Input: " + horizontalInput);
        verticalInput = context.ReadValue<Vector2>().y;
        print("Vertical Input: " + verticalInput);
    }

    public void OnClimb(InputAction.CallbackContext context)
    {
        if (!isOnStairs) return; // Only allow climbing if on stairs

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, verticalInput * climbSpeed); // Climb up or down
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (IsGrounded()) jumpCount = maxJumpCount; // Reset jump count when grounded

        if (context.performed && jumpCount > 0 && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpCount--;
        }
        else if (context.performed && jumpCount > 0 && !IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpCount--;
        }
        else if (context.canceled && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
    }

    public void Sprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Player.Instance.Stamina > 0) isSprinting = true; // Start sprinting if stamina is available
        }
        if (context.canceled)
        {
            isSprinting = false; // Stop sprinting
            moveSpeed = originalMoveSpeed; // Reset to original speed
        }
    }

    public void Crouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isCrouching = true; // Start crouching
            moveSpeed = originalMoveSpeed * 0.2f; // Reduce speed while crouching

            if (Input.GetButtonDown("Jump"))
            {
                StartCoroutine(DropThroughPlatform());  // Drop through platform if crouching and jump is pressed
            }
        }
        if (context.canceled)
        {
             isCrouching = false;
             moveSpeed = originalMoveSpeed;
        }
    }
    private void RefreshStamina()
    {
        Player.Instance.Stamina += Time.deltaTime * staminaRefreshRate; // Regenerate stamina over time
        if (Player.Instance.Stamina > Player.Instance.MaxStamina) Player.Instance.Stamina = Player.Instance.MaxStamina;
    }

    private void DrainStamina()
    {
        Player.Instance.Stamina -= Time.deltaTime * staminaDrainRate; // Drain stamina while sprinting
        if (Player.Instance.Stamina < 0) Player.Instance.Stamina = 0;
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void Flip()
    {
        IsFacingRight = !IsFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Stairs"))
        {
            isOnStairs = true; // Set climbing state when entering stairs
            rb.gravityScale = 0f; // Disable gravity while climbing
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Stairs"))
        {
            isOnStairs = false; // Reset climbing state when exiting stairs
            rb.gravityScale = 1f; // Enable gravity when not climbing
        }
    }

    private void UpdatePlayerAnimation()
    {
        animator.SetFloat("moveSpeed", Mathf.Abs(horizontalInput));
        if (!IsGrounded() && !isOnStairs && rb.linearVelocity.y > 0) animator.SetTrigger("Jump");
        if (!IsGrounded() && rb.linearVelocity.y < 0) animator.SetTrigger("Fall");
        if (IsGrounded()) animator.SetTrigger("Idle");
        animator.SetBool("IsClimbing", (isOnStairs && verticalInput != 0));
        animator.SetBool("IsCrouching", (isCrouching && IsGrounded()));
    }

    private IEnumerator DropThroughPlatform()
    {
        // Disable collision between player and one-way platforms
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Platforms"), true);
        yield return new WaitForSeconds(0.4f); // Duration of falling through
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Platforms"), false);
    }
}
