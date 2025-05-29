using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] private bool isPaused = false; // Start with the game unpaused

    void Start()
    {
        pauseMenuUI.SetActive(isPaused); // Ensure the pause menu is hidden at the start    
    }

    void Update()
    {
        // Check for the pause key (Escape) to toggle pause state
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause(); // Call the method to toggle pause state
        }
    }

    public void ResumeButton()
    {
        PauseUnpause(); // Call the method to toggle pause state
    }

    public void LevelsButton()
    {
        // Load the levels menu scene
        //UnityEngine.SceneManagement.SceneManager.LoadScene("levelsMenu");
        print("Loading levels menu");
    }

    public void QuitButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0); // Load the main menu scene

    }

    private void PauseUnpause()
    {
        isPaused = !isPaused; // Toggle the pause state
        pauseMenuUI.SetActive(isPaused); // Show or hide the pause menu
        Time.timeScale = isPaused ? 0f : 1f; // Pause or resume the game
    }
}
