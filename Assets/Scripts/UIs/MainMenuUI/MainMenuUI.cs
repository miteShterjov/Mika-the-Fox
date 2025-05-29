using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(1); // Load the first level scene
    }

    public void QuitGame()
    {
        Application.Quit(); // Quit the application
        print("Quiting the game");
    }
}
