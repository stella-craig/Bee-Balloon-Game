using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Player stats
    public int score = 0;
    public int lives = 3;
    public Text scoreText;
    public Text livesText;
    public Text levelText;
    private float timePlayed = 0f; // Tracks time spent in the game

    //class to store player stats
    [Serializable]
    public class PlayerData
    {
        public int playerID;
        public int score;
        public int lives;
        public float timePlayed;
        public int level;
        public string feedback;
    }

    // Timer
    public Text timerText; // Reference to the Timer UI Text element
    private float timer = 120f; // 2 minutes (120 seconds)

    //For respawn text
    public Text countdownText; // Reference to the countdown text element when respawning

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

    private PlayerData playerStats;

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
        playerStats = new PlayerData(); // Initialize the player stats object
    }

    private void Start()
    {
        ValidatePrefabs();
        SpawnBee();
        LoadZone(currentLevel);
        UpdateUI();
        UpdateProgressBar();
        UpdateTimerUI();
    }

    private void Update()
    {
        // Update the countdown timer
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            timePlayed += Time.deltaTime; // Add time to the total played time
            UpdateTimerUI();

            if (timer <= 0)
            {
                timer = 0; // Ensure timer doesn't go negative
                EndGame();
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
            EndGame();
        }
    }

    private IEnumerator PauseAfterRespawn()
    {
        if (beeMovement != null) beeMovement.enabled = false;

        // Show the countdown on the screen
        for (int i = 3; i > 0; i--)
        {
            if (countdownText != null)
            {
                countdownText.text = i.ToString(); // Update the countdown text
                countdownText.gameObject.SetActive(true); // Ensure the text is visible
            }

            yield return new WaitForSeconds(1f); // Wait for 1 second
        }

        // Hide the countdown text
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }

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
        if (progressBar != null)
        {
            // Hardcoded progress for each level based on the thresholds
            if (score < 92) // Level 1
            {
                progressBar.size = Mathf.Clamp01((float)score / 92);
            }
            else if (score < 159) // Level 2
            {
                progressBar.size = Mathf.Clamp01((float)(score - 92) / (159 - 92));
            }
            else if (score < 209) // Level 3
            {
                progressBar.size = Mathf.Clamp01((float)(score - 159) / (209 - 159));
            }
            else if (score <= 273) // Level 4
            {
                progressBar.size = Mathf.Clamp01((float)(score - 209) / (273 - 209));
            }
            else
            {
                // Cap the progress bar at 100% if the score exceeds all thresholds
                progressBar.size = 1.0f;
            }
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

            // Destroy the current zone and load the next one
            if (activeZone != null) Destroy(activeZone);
            LoadZone(currentLevel);

            RespawnBee(); // Respawn the bee at the center
            StartCoroutine(PauseAfterRespawn()); // Show countdown and pause movement

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



    /// Debug method to instantly set the score and transition to the corresponding level.
    public void DebugSetScore(int targetScore)
    {
        // Set the score to the target value
        score = targetScore;

        // Determine the corresponding level based on the target score
        if (targetScore < 92) // Level 1
        {
            currentLevel = 0;
        }
        else if (targetScore < 159) // Level 2
        {
            currentLevel = 1;
        }
        else if (targetScore < 209) // Level 3
        {
            currentLevel = 2;
        }
        else if (targetScore <= 273) // Level 4
        {
            currentLevel = 3;
        }
        else
        {
            Debug.LogWarning("Target score exceeds the maximum threshold.");
            return; // Do nothing if the target score is out of range
        }

        Debug.Log($"Debug: Set score to {score}, transitioned to Level {currentLevel + 1}");

        // Destroy the current active zone if it exists
        if (activeZone != null)
        {
            Destroy(activeZone);
        }

        // Load the appropriate level zone
        LoadZone(currentLevel);

        // Respawn the bee and update the UI
        RespawnBee();
        UpdateUI();
        UpdateProgressBar();
    }
    public void RestartGame()
    {
        // Store feedback using PlayerPrefs (instead of relying on GameManager across scenes)
        PlayerPrefs.SetString("PlayerFeedback", "Player feedback here"); // Replace with the actual feedback from EndScreenManager
        PlayerPrefs.SetInt("PlayerScore", score);
        PlayerPrefs.SetInt("PlayerLives", lives);
        PlayerPrefs.SetFloat("PlayerTimePlayed", timePlayed);
        PlayerPrefs.SetInt("PlayerLevel", currentLevel);

        // Emit player stats as JSON and log
        string playerDataJson = GetPlayerStatsAsJson();
        Debug.Log("Emitting Player Stats as JSON: " + playerDataJson);

        // Reset all game state variables
        score = 0;
        lives = 3;
        currentLevel = 0;
        timer = 120f;

        // Destroy the active zone and bee
        if (activeZone != null)
        {
            Destroy(activeZone);
        }
        if (bee != null)
        {
            Destroy(bee);
        }

        // Reset the progress bar
        if (progressBar != null)
        {
            progressBar.size = 0;
        }

        // Destroy any lingering cameras
        DestroyPersistingCamera();

        // Reload StartScene and ensure GameManager resets
        SceneManager.LoadScene("StartScene");
    }



    private void DestroyPersistingCamera()
    {
        GameObject mainCamera = GameObject.FindWithTag("MainCamera");
        if (mainCamera != null && mainCamera.transform.parent == null) // Ensure it's not nested
        {
            Destroy(mainCamera);
        }
    }

    public void EndGame()
    {
        // Save data in PlayerPrefs
        PlayerPrefs.SetInt("FinalScore", score);
        PlayerPrefs.SetFloat("TimePlayed", timePlayed);
        PlayerPrefs.SetInt("FinalLevel", currentLevel + 1);

        Debug.Log("Game Ended! Transitioning to End Screen.");

        if (lives == 0)
        {
            SceneManager.LoadScene("DeathScene");
        }
        else
        {
            SceneManager.LoadScene("VictoryScene");
        }
    }


    public string GetTimePlayed()
    {
        int minutes = Mathf.FloorToInt(timePlayed / 60);
        int seconds = Mathf.FloorToInt(timePlayed % 60);
        return $"{minutes:D2}:{seconds:D2}"; // Format as "MM:SS"
    }


    // Add method to emit player data as JSON, including feedback
    public string GetPlayerStatsAsJson()
    {
        PlayerData playerData = new PlayerData
        {
            playerID = UnityEngine.Random.Range(1000, 9999), // Use random ID for now or implement real logic
            score = score,
            lives = lives,
            timePlayed = Time.timeSinceLevelLoad, // Time since the level loaded
            level = currentLevel + 1,
        };

        string json = JsonUtility.ToJson(playerData);
        Debug.Log("Player Data JSON: " + json);

        return json;
    }


    public void SetPlayerFeedback(string feedback)
    {
        // Store the feedback in the PlayerData object
        playerStats.feedback = feedback;

        // Log the feedback for verification
        Debug.Log("Player feedback set: " + feedback);
    }





}
