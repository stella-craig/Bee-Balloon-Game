using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
using System.Collections;

public class FirebaseInitializer : MonoBehaviour
{
    private FirebaseApp app;
    private FirebaseFirestore firestore;

    private void Start()
    {
        // Initialize Firebase on start
        InitializeFirebase();
    }

    void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            firestore = FirebaseFirestore.GetInstance(app);
            Debug.Log("Firebase initialized successfully.");
        });
    }

    // Function to send player data to Firestore
    public void SendPlayerData(string playerId, int score, string timePlayed)
    {
        DocumentReference playerRef = firestore.Collection("players").Document(playerId);
        playerRef.SetAsync(new { score = score, timePlayed = timePlayed }).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Player data saved to Firestore!");
            }
            else
            {
                Debug.LogError("Error saving player data: " + task.Exception);
            }
        });
    }
}
