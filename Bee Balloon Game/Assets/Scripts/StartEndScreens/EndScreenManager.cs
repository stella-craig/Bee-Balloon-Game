using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreenManager : MonoBehaviour
{
    public Text finalScoreText;
    public Text finalTimeText;
    public InputField feedbackInputField; // To collect player feedback
    public Button submitFeedbackButton;

    private string playerFeedback = ""; // Store feedback locally

    private void Start()
    {
        // Load data from PlayerPrefs
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0);
        float timePlayed = PlayerPrefs.GetFloat("TimePlayed", 0f);
        int finalLevel = PlayerPrefs.GetInt("FinalLevel", 1);

        // Update UI elements
        finalScoreText.text = $"Final Score: {finalScore}";
        finalTimeText.text = $"Time Played: {FormatTime(timePlayed)}";

        // Set up feedback submission
        submitFeedbackButton.onClick.AddListener(SubmitFeedback);
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return $"{minutes:D2}:{seconds:D2}";
    }

    public void SubmitFeedback()
    {
        playerFeedback = feedbackInputField.text;
        Debug.Log($"Player Feedback Submitted: {playerFeedback}");
    }

    public void RestartGame()
    {
        // Emit all data, including feedback
        EmitPlayerData();

        Debug.Log("Restarting game...");
        SceneManager.LoadScene("StartScene");
    }

    private void EmitPlayerData()
    {
        // Collect data and log it as JSON
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0);
        float timePlayed = PlayerPrefs.GetFloat("TimePlayed", 0f);
        int finalLevel = PlayerPrefs.GetInt("FinalLevel", 1);

        string playerDataJson = $"{{\"score\": {finalScore}, \"timePlayed\": \"{FormatTime(timePlayed)}\", \"level\": {finalLevel}, \"feedback\": \"{playerFeedback}\"}}";

        Debug.Log("Player Data and Feedback JSON: " + playerDataJson);

        // Optionally: Send this JSON to a database or log it somewhere
    }
}
