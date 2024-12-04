using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Player stats
    public int score = 0;
    public int lives = 3;
    public Text scoreText;
    public Text livesText;
    public Text levelText;

    // Final score for end screens
    public int finalScore;

    // Level and progress tracking
    public Scrollbar progressBar; // Assign your Scrollbar in the Inspector
    public int[] balloonsPerLevel = { 67, 130, 200 }; // Total balloons for each level
    private int currentLevel = 0;
    private int levelStartScore = 0;

    [Header("Level References")]
    public GameObject bee; // Drag the Bee GameObject here in the Inspector
    public GameObject branchesZone; // Level 1 reference
    public GameObject fishZone;     // Level 2 reference

    private BeeMovement beeMovement;

    private void Awake()
    {
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
        if (bee != null)
        {
            beeMovement = bee.GetComponent<BeeMovement>();
        }

        // Activate only the first level at the start
        if (branchesZone != null) branchesZone.SetActive(true);
        if (fishZone != null) fishZone.SetActive(false);

        UpdateUI();
        UpdateProgressBar();
    }

    public void AddScore(int value)
    {
        score += value;
        UpdateUI();
        UpdateProgressBar();

        if (score >= levelStartScore + balloonsPerLevel[currentLevel])
        {
            NextLevel();
        }
    }

    public void LoseLife()
    {
        lives--;
        UpdateUI();

        if (lives > 0)
        {
            RespawnBee();
            StartCoroutine(PauseAfterRespawn());
        }
        else
        {
            finalScore = score; // Store the final score
            SceneManager.LoadScene("DeathScene");
        }
    }

    private IEnumerator PauseAfterRespawn()
    {
        if (beeMovement != null)
        {
            beeMovement.enabled = false;
        }

        yield return new WaitForSeconds(3f);

        if (beeMovement != null)
        {
            beeMovement.enabled = true;
        }
    }

    private void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
        if (livesText != null) livesText.text = "Lives: " + lives;
        if (levelText != null) levelText.text = "Level: " + (currentLevel + 1);
    }

    private void UpdateProgressBar()
    {
        if (progressBar != null)
        {
            int balloonsThisLevel = balloonsPerLevel[currentLevel];
            progressBar.size = (float)(score - levelStartScore) / balloonsThisLevel;
        }
    }

    private void NextLevel()
    {
        if (currentLevel + 1 < balloonsPerLevel.Length)
        {
            currentLevel++;
            levelStartScore = score;
            UpdateUI();
            UpdateProgressBar();
            StartCoroutine(LevelTransitionRoutine());
        }
        else
        {
            finalScore = score; // Store the final score
            SceneManager.LoadScene("VictoryScene"); 
        }
    }

    private IEnumerator LevelTransitionRoutine()
    {
        RespawnBee();

        if (beeMovement != null)
        {
            beeMovement.enabled = false;
        }

        TransitionToNextLevel();

        yield return new WaitForSeconds(3f);

        if (beeMovement != null)
        {
            beeMovement.enabled = true;
        }
    }

    private void TransitionToNextLevel()
    {
        Debug.Log($"Transitioning to Level {currentLevel + 1}");

        // Handle level transitions explicitly
        if (currentLevel == 1)
        {
            if (branchesZone != null) branchesZone.SetActive(false);
            if (fishZone != null) fishZone.SetActive(true);
            Debug.Log("Transitioned to FishZone (Level 2).");
        }
        else
        {
            Debug.LogError("Transition logic for this level is not set up!");
        }
    }

    private void RespawnBee()
    {
        if (bee != null)
        {
            bee.transform.position = Vector3.zero; // Reset bee to center
            Debug.Log("Bee respawned at the center.");
        }
        else
        {
            Debug.LogError("Bee reference is null!");
        }
    }
}
