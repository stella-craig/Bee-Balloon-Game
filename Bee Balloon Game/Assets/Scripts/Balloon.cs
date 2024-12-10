using UnityEngine;

public class Balloon : MonoBehaviour
{
    public int scoreValue = 1; // Points for popping the balloon
    public GameObject popEffect; // Particle effect prefab
    public AudioClip popSound; // Sound effect for popping
    private AudioSource audioSource; // For playing sounds
    private bool isPopped = false; // Prevent multiple triggers

    void Awake()
    {
        // Ensure AudioSource is initialized early
        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.volume = 1f;
        audioSource.spatialBlend = 0f; // 2D sound
    }

    void Start()
    {
        if (popSound == null)
        {
            Debug.LogWarning("PopSound is not assigned in the Balloon script. Please check the Inspector.");
        }

        Debug.Log($"AudioSource initialized: {audioSource != null}");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPopped) return; // Prevent multiple triggers
        isPopped = true;

        if (collision.CompareTag("Bee"))
        {
            if (audioSource != null && popSound != null)
            {
                Debug.Log($"Playing sound: {popSound.name}");
                audioSource.PlayOneShot(popSound);
                Debug.Log("Sound played successfully.");
            }
            else
            {
                Debug.LogError($"AudioSource or PopSound is missing. AudioSource: {audioSource}, PopSound: {popSound}");
            }

            if (popEffect != null)
            {
                Instantiate(popEffect, transform.position, Quaternion.identity);
            }

            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(scoreValue);
            }

            Destroy(gameObject);
        }
    }
}
