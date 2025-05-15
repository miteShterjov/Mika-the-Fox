using UnityEngine;

public class CherryPickUp : MonoBehaviour, IPickUp
{
    [Header("Cherry PickUp Settings")]
    [SerializeField] private float healthRestoreAmount = 10f; // Amount of health restored by the cherry
    [SerializeField] private float destroyDelay = 0.8f; // Delay before destroying the cherry

    private Animator animator; // Reference to the Animator component

    void Awake()
    {
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    public void ApplyPickUpEffect(Player player)
    {
        if (player.Health == player.MaxHealth) return; // Do nothing if health is already full

        player.Health += healthRestoreAmount; // Restore health to the player
        if (player.Health > player.MaxHealth) player.Health = player.MaxHealth; // Clamp health to max value

        animator.SetTrigger("PickedUp"); // Trigger the pick-up animation
        Destroy(gameObject, destroyDelay); // Destroy the cherry after a delay
    }
}
