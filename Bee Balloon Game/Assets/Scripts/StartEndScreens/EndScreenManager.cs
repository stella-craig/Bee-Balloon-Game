using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreenManager : MonoBehaviour
{
    public Text finalScoreText;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            finalScoreText.text = "Final Score: " + GameManager.Instance.score;
        }
    }

    public void RestartGame()
    {
        Debug.Log("Restart button clicked!");
        Debug.Log($"Current Scene: {SceneManager.GetActiveScene().name}");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.RestartGame();
        }
        else
        {
            Debug.LogError("GameManager instance is missing. Unable to restart.");
        }

        if (Application.CanStreamedLevelBeLoaded("StartScene"))
        {
            Debug.Log("StartScene found. Loading...");
            SceneManager.LoadScene("StartScene");
        }
        else
        {
            Debug.LogError("StartScene could not be loaded. Check scene name or build settings.");
        }
    }
}
