using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager Instance { get; private set; }

    // Player stats
    public int score = 0;
    public int lives = 3;
    public Text scoreText;
    public Text livesText;
    public Text levelText;

    private void Awake()
    {
        // Ensure only one instance of GameManager exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        UpdateUI();
    }

    // Method to add score
    public void AddScore(int value)
    {
        score += value;
        UpdateUI();
    }

    // Method to decrease lives
    public void LoseLife()
    {
        lives--;
        UpdateUI();

        if (lives <= 0)
        {
            GameOver();
        }
    }

    // Update the UI
    private void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
        if (livesText != null) livesText.text = "Lives: " + lives;
        if (levelText != null) levelText.text = "Level: " + SceneManager.GetActiveScene().name;
    }

    // Restart the game on game over
    private void GameOver()
    {
        Debug.Log("Game Over!");
        SceneManager.LoadScene("GameOver"); // Create a GameOver scene for this
    }

    // Transition to the next level
    public void NextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("You completed all levels!");
            SceneManager.LoadScene("EndScene"); // Create an EndScene for this
        }
    }
}
