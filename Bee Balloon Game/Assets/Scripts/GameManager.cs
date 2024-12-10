using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

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
    public Scrollbar progressBar;
    public int[] balloonsPerLevel = { 92, 67, 50, 64 };
    private int currentLevel = 0;

    [Header("Zone Prefabs")]
    public GameObject beePrefab;
    public GameObject[] zonePrefabs;

    private GameObject activeZone;
    private GameObject bee;
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
        SpawnBee();
        LoadZone(currentLevel);
        UpdateUI();
        UpdateProgressBar();
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
            finalScore = score;
            SceneManager.LoadScene("DeathScene");
        }
    }

    private IEnumerator PauseAfterRespawn()
    {
        if (beeMovement != null) beeMovement.enabled = false;

        yield return new WaitForSeconds(3f);

        if (beeMovement != null) beeMovement.enabled = true;
    }

    private void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
        if (livesText != null) livesText.text = "Lives: " + lives;
        if (levelText != null) levelText.text = "Level: " + (currentLevel + 1);
    }

    private void UpdateProgressBar()
    {
        if (progressBar != null && currentLevel < balloonsPerLevel.Length)
        {
            int balloonsThisLevel = balloonsPerLevel[currentLevel];
            progressBar.size = (float)(score - GetLevelThreshold(currentLevel - 1)) / balloonsThisLevel;
        }
    }

    private void RespawnBee()
    {
        if (bee != null)
        {
            bee.transform.position = Vector3.zero;
        }
        else
        {
            Debug.LogError("Bee reference is null!");
        }
    }

    public void AddScore(int value)
    {
        score += value;
        UpdateUI();
        UpdateProgressBar();

        if (score >= GetLevelThreshold(currentLevel))
        {
            HandleLevelTransition();
        }
    }

    private int GetLevelThreshold(int level)
    {
        switch (level)
        {
            case 0: return 92;
            case 1: return 159;
            case 2: return 209;
            case 3: return 273;
            default: return int.MaxValue;
        }
    }

    private void HandleLevelTransition()
    {
        if (currentLevel < balloonsPerLevel.Length - 1)
        {
            currentLevel++;
            Debug.Log($"Transitioning to Level {currentLevel + 1}");

            if (activeZone != null) Destroy(activeZone);

            LoadZone(currentLevel);

            RespawnBee();
            UpdateUI();
            UpdateProgressBar();
        }
        else
        {
            CompleteGame();
        }
    }

    private void LoadZone(int level)
    {
        if (level < zonePrefabs.Length)
        {
            activeZone = Instantiate(zonePrefabs[level]);
            Debug.Log($"Zone {level + 1} loaded.");

            // Notify BeeMovement to find the new boundary
            if (bee != null)
            {
                BeeMovement beeMovement = bee.GetComponent<BeeMovement>();
                if (beeMovement != null)
                {
                    beeMovement.FindBoundary();
                }
            }
        }
        else
        {
            Debug.LogError($"Zone prefab for level {level + 1} not found!");
        }
    }

    private void CompleteGame()
    {
        finalScore = score;
        Debug.Log("Game completed! Transitioning to VictoryScene.");
        progressBar.size = 1.0f;
        SceneManager.LoadScene("VictoryScene");
    }

    private void SpawnBee()
    {
        if (beePrefab != null)
        {
            bee = Instantiate(beePrefab, Vector3.zero, Quaternion.identity);
            beeMovement = bee.GetComponent<BeeMovement>();
        }
        else
        {
            Debug.LogError("Bee prefab not assigned!");
        }
    }
}
