using UnityEngine;

public class ColectablesHandler : MonoBehaviour
{
    public static ColectablesHandler Instance; // Singleton instance of the class
    
    // this class handles the colectables in the game that may spawn when hte player stomps the enemy
    [SerializeField] private GameObject[] collectables; // Array of collectable prefabs
    [SerializeField][Range(0,100)] private int dropRate = 40; // Drop rate to drop collectables

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Set the singleton instance
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instance
        }
    }

    private bool DoWeHaveLoot()
    {
        return Random.Range(0, 100) < dropRate; // 50% chance to have loot
    }

    private GameObject GetFatLoot()
    {
        int randomIndex = Random.Range(0, collectables.Length); // Get a random index from the collectables array
        return collectables[randomIndex]; // Return the collectable prefab
    }

    public void SpawnPlayerCollectables(Vector3 position)
    {
        if (DoWeHaveLoot())
        {
            print("Loot spawned!"); // Print message if loot is spawned
            GameObject loot = GetFatLoot(); // Get a random collectable prefab
            Instantiate(loot, position, Quaternion.identity); // Spawn the collectable at the given position
        }
        else print("No loot this time!"); // Print message if no loot is spawned
    }
}
