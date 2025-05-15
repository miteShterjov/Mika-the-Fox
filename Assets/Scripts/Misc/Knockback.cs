using UnityEngine;

public class Knockback : MonoBehaviour
{
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
