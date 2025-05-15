using UnityEngine;

public class AcornPickUp : MonoBehaviour, IPickUp
{
    [Header("Acorn PickUp Settings")]
    [SerializeField] private float staminaRestoreAmount = 10f; // Amount of health restored by the cherry
    [SerializeField] private float destroyDelay = 0.8f; // Delay before destroying the cherry

    private Animator animator; // Reference to the Animator component

    void Awake()
    {
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    public void ApplyPickUpEffect(Player player)
    {
        if (player.Stamina == player.MaxStamina) return; // Do nothing if health is already full

        player.Stamina += staminaRestoreAmount; // Restore health to the player
        if (player.Stamina > player.MaxStamina) player.Stamina = player.MaxStamina; // Clamp health to max value

        animator.SetTrigger("PickedUp"); // Trigger the pick-up animation
        Destroy(gameObject, destroyDelay); // Destroy the cherry after a delay
    }
}
