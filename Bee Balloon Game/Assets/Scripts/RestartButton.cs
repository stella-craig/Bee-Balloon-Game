using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    public void RestartGame()
    {
        Debug.Log("Restart button clicked!");
        Debug.Log($"Current Scene: {SceneManager.GetActiveScene().name}");

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
