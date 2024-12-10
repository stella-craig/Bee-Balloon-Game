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

    // Timer
    public Text timerText; // Reference to the Timer UI Text element
    private float timer = 120f; // 2 minutes (120 seconds)

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
        ValidatePrefabs();
        SpawnBee();
        LoadZone(currentLevel);
        UpdateUI();
        UpdateProgressBar();
        UpdateTimerUI(); // Initialize the timer UI
    }

    private void Update()
    {
        // Update the countdown timer
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            UpdateTimerUI();

            if (timer <= 0)
            {
                timer = 0; // Ensure timer doesn't go negative
                SceneManager.LoadScene("DeathScene"); // Load DeathScene when time runs out
            }
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timer / 60);
            int seconds = Mathf.FloorToInt(timer % 60);
            timerText.text = $"{minutes:D2}:{seconds:D2}";
        }
    }

    private void ValidatePrefabs()
    {
        for (int i = 0; i < zonePrefabs.Length; i++)
        {
            if (zonePrefabs[i] == null)
            {
                Debug.LogError($"Zone prefab for Level {i + 1} is not assigned in the Inspector!");
            }
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
            Debug.Log("Final level reached.");
            if (activeZone != null) Destroy(activeZone);

            CompleteGame();
        }
    }

    private void LoadZone(int level)
    {
        if (level < zonePrefabs.Length)
        {
            if (zonePrefabs[level] == null)
            {
                Debug.LogError($"Zone prefab for Level {level + 1} is not assigned in the Inspector!");
                return;
            }

            activeZone = Instantiate(zonePrefabs[level]);
            Debug.Log($"Zone {level + 1} loaded: {zonePrefabs[level].name}");

            // Notify BeeMovement to find the new boundary
            if (bee != null)
            {
                BeeMovement beeMovement = bee.GetComponent<BeeMovement>();
                if (beeMovement != null)
                {
                    beeMovement.FindBoundary();
                    Debug.Log("BeeMovement boundary updated.");
                }
                else
                {
                    Debug.LogError("BeeMovement script is missing on the bee!");
                }
            }
        }
        else
        {
            Debug.LogError($"Zone prefab for level {level + 1} is out of range!");
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
