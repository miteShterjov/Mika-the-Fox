using UnityEngine;

public class PlayerStomper : MonoBehaviour
{
    [SerializeField] private float delay = 0.5f; // Delay before destroying the enemy
    [SerializeField] private GameObject stompEffectPrefab; // Prefab for the stomp effect
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (transform.position.y > collision.transform.position.y)
            {
                // Destroy the enemy if the player stomps on it
                Destroy(collision.gameObject);
                Instantiate(stompEffectPrefab, collision.transform.position, Quaternion.identity);
                
            }
        }
    }
}
