using UnityEngine;

public class DestroyOverTime : MonoBehaviour
{
    [SerializeField] private float destroyTime = 0.5f; // Time in seconds before the object is destroyed

    void Start()
    {
        Destroy(gameObject, destroyTime); // Schedule the object for destruction
    }
}
