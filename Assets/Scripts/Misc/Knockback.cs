using UnityEngine;

public class Knockback : MonoBehaviour
{
    // this fcker needs a lot of work, for now it does shit
    // this class handles the knockback effect when the player is hit by an enemy
    // but sadly is a sorry excuse for a knockback system
    // it is not working as intended and needs a lot of work
    // its ment to be used as eye candy for the player
    private Rigidbody2D rb;
    private PlayerController playerController;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (gameObject.CompareTag("Player"))
        {
            playerController = GetComponent<PlayerController>();
        }   

   }

    public void ApplyHitKnockback(Vector2 damageSource, float knockbackForce)
    {
        //player stopes moving
        rb.linearVelocity = Vector2.zero;
        // Calculate the direction of the knockback
        Vector2 knockbackDirection = (Vector2)transform.position - damageSource;
        knockbackDirection.Normalize(); // Normalize the direction vector
        //then apply the knockback force pushing the player away from the damage source backwards
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
    }
}
