using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement Settings")]
    [SerializeField] private float moveSpeed = 5f; // Normal speed
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
        OnMove(new InputAction.CallbackContext());

        if (horizontalInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (horizontalInput < 0 && isFacingRight)
        {
            Flip();
        }

        UpdatePlayerAnimation();
        if (Player.Instance.Stamina != Player.Instance.MaxStamina) RefreshStamina();
        if (moveSpeed != originalMoveSpeed) DrainStamina();
    }

    private void UpdatePlayerAnimation()
    {
        animator.SetFloat("moveSpeed", Mathf.Abs(horizontalInput));
        if (!IsGrounded() && rb.linearVelocity.y > 0) animator.SetTrigger("Jump");
        if (!IsGrounded() && rb.linearVelocity.y < 0) animator.SetTrigger("Fall");
        if (IsGrounded()) animator.SetTrigger("Idle");
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalInput = context.ReadValue<Vector2>().x;
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
        if (Player.Instance.Stamina <= 0) return; // Prevent sprinting if stamina is depleted

        if (context.performed && Player.Instance.Stamina != 0 && IsGrounded())
        {
            moveSpeed = sprintSpeed;
        }
        if (context.canceled && Player.Instance.Stamina <= 0)
        {
            moveSpeed = originalMoveSpeed; // Reset to normal speed
            return;
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
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
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
