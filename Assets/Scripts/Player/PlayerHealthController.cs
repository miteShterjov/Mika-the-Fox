using System;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    private bool isInvincible = false;
    private float invincibilityDuration = 1f; // Duration of invincibility after taking damage

    private Knockback knockback;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        knockback = GetComponent<Knockback>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (Player.Instance.transform.position.y > other.transform.position.y) return; // Ignore damage if player is above the enemy
            if (isInvincible) return; // Ignore damage if invincible
            DoDamageToPlayer(other.gameObject);
        }

        if (other.CompareTag("PickUp"))
        {
            other.gameObject.GetComponent<IPickUp>().ApplyPickUpEffect(Player.Instance);
        }
    }

    private void DoDamageToPlayer(GameObject other)
    {
        Player.Instance.Health -= other.GetComponent<Enemy>().Damage;
        isInvincible = true;
        //knockback.ApplyHitKnockback(other.transform.position, other.GetComponent<Enemy>().Damage); - needs work

        if (Player.Instance.Health <= 0) PlayerDieAndRespawn();
    }

    private void PlayerDieAndRespawn()
    {
        float respawnDelay = 1f; // Delay before respawning
        Player.Instance.gameObject.SetActive(false);
        Invoke("RespawnPlayer", respawnDelay);
    }

    private void RespawnPlayer()
    {
        Player.Instance.transform.position = Player.Instance.RespawnPoint;
        Player.Instance.gameObject.SetActive(true);
        Player.Instance.Health = Player.Instance.MaxHealth;
    }
}
