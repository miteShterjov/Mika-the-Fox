using System;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    [SerializeField] private float bounceForce = 15f; // Force applied to the player when bouncing
    [SerializeField] private Transform groundChecker;
    private bool isInvincible = false; // Flag to check if player is invincible
    private float invincibilityDuration = 1f; // Duration of invincibility after taking damage
    private Knockback knockback;
    private Animator animator; // Animator to control player animations
    private SpriteRenderer spriteRenderer; // SpriteRenderer to control player sprite
    private Rigidbody2D rb; // Rigidbody2D to control player physics

    private void Awake()
    {
        knockback = GetComponent<Knockback>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isInvincible)
        {
            // Flash the player sprite to indicate invincibility
            spriteRenderer.color = new Color(1, 1, 1, Mathf.PingPong(Time.time * 5, 1));
            animator.SetBool("IsHurt", true);
            invincibilityDuration -= Time.deltaTime;
            if (invincibilityDuration <= 0)
            {
                isInvincible = false;
                invincibilityDuration = 1f; // Reset duration
                spriteRenderer.color = Color.white; // Reset color
                animator.SetBool("IsHurt", false); // Reset animation
            }
        }
    }

    // This method handles when the player collides with the enviorment
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (groundChecker.transform.position.y > other.transform.position.y) return; // Ignore collision if player is above enemy
            if (isInvincible) return; // Ignore damage if invincible
            DoDamageToPlayer(other.gameObject);
        }

        if (other.CompareTag("PickUp"))
        {
            other.gameObject.GetComponent<IPickUp>().ApplyPickUpEffect(Player.Instance);
        }
    }

    // player takes damage from enemy tag
    private void DoDamageToPlayer(GameObject other)
    {
        Player.Instance.Health -= other.GetComponent<Enemy>().Damage;
        isInvincible = true;
        //knockback.ApplyHitKnockback(other.transform.position, other.GetComponent<Enemy>().Damage); - needs work

        if (Player.Instance.Health <= 0) PlayerDieAndRespawn();
    }

    // This method handles the player respawn logic, player dies and respawns after a delay
    private void PlayerDieAndRespawn()
    {
        float respawnDelay = 1f; // Delay before respawning
        Player.Instance.gameObject.SetActive(false);
        Invoke("RespawnPlayer", respawnDelay);
    }

    // This method handles the player respawn logic, player respawns at the last checkpoint
    private void RespawnPlayer()
    {
        Player.Instance.transform.position = Player.Instance.RespawnPoint;
        Player.Instance.gameObject.SetActive(true);
        Player.Instance.Health = Player.Instance.MaxHealth;
    }

    // This method handles the player bounce logic, player bounces when colliding with enemy tag
    public void PlayerBounce()
    {
        rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
    }
}
