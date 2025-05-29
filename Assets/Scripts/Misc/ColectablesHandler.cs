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

    // This method checks if we will have loot or not, roll the dice
    // and returns true or false, by default set at 40% chance
    private bool DoWeHaveLoot()
    {
        return Random.Range(0, 100) < dropRate;
    }

    // This method returns a random collectable prefab from the array
    // it uses the Random.Range method to get a random index from the array
    // and returns the collectable prefab at that index
    // this method is called when the player stomps the enemy
    // and the loot is maybe spawned
    private GameObject GetFatLoot()
    {
        int randomIndex = Random.Range(0, collectables.Length); // Get a random index from the collectables array
        return collectables[randomIndex]; // Return the collectable prefab
    }

    // This method is called when the player stomps the enemy
    // and the loot is maybe spawned
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
