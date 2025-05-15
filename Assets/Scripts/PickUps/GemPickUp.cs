using UnityEngine;

public class GemPickUp : MonoBehaviour, IPickUp
{
    [SerializeField] private float destroyDelay = 0.8f; // Delay before destroying the gem
    Animator animator; // Reference to the Animator component
    private void Awake()
    {
        animator = GetComponent<Animator>(); // Get the Animator component
    }
    public void ApplyPickUpEffect(Player player)
    {
        player.GemsCollected += 1; // Increment the gem count
        animator.SetTrigger("PickedUp"); // Trigger the pick-up animation
        Destroy(gameObject, destroyDelay); // Destroy the gem object
    }
}
