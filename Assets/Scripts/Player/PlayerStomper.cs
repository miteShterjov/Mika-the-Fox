using UnityEngine;

public class PlayerStomper : MonoBehaviour
{
    [SerializeField] private GameObject stompEffectPrefab; // Prefab for the stomp effect

    private PlayerHealthController playerHealthController; // Reference to the PlayerHealthController cuz of the PlayerBounce()

    void Awake()
    {
        playerHealthController = GetComponentInParent<PlayerHealthController>();
    }

    // This method handles when the player stomps on an enemy
    // It checks if the player is above the enemy and destroys the enemy if true
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (transform.position.y > collision.transform.position.y)
            {
                // Destroy the enemy if the player stomps on it
                Destroy(collision.gameObject);
                Instantiate(stompEffectPrefab, collision.transform.position, Quaternion.identity);
                playerHealthController.PlayerBounce(); // Bounce the player
                ColectablesHandler.Instance.SpawnPlayerCollectables(collision.transform.position); // Spawn collectables
            }
        }
    }
}
