using UnityEngine;

public class PauseButtonToggle : MonoBehaviour
{
    public GameObject pauseIcon; // Reference to the PauseIcon GameObject
    public GameObject playIcon;  // Reference to the PlayIcon GameObject

    private bool isPaused = false; // Tracks the pause state

    private void Start()
    {
        // Ensure the initial state is correct
        if (pauseIcon != null) pauseIcon.SetActive(true);  // Pause icon starts active
        if (playIcon != null) playIcon.SetActive(false);  // Play icon starts inactive
    }

    public void TogglePause()
    {
        // Toggle the paused state
        isPaused = !isPaused;

        // Toggle the visibility of the icons
        if (pauseIcon != null) pauseIcon.SetActive(!isPaused);
        if (playIcon != null) playIcon.SetActive(isPaused);

        // Pause or unpause the game
        Time.timeScale = isPaused ? 0 : 1;

        // Debug logs to confirm state
        Debug.Log(isPaused ? "Game Paused" : "Game Resumed");
    }
}
