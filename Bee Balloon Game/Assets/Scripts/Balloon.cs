using UnityEngine;

public class Balloon : MonoBehaviour
{
    // Reference to the GameManager or LevelManager script to track score
    public int scoreValue = 1; // Points awarded for popping this balloon

    // Particle system to show pop effect
    public GameObject popEffect;

    // Sound effect for popping
    public AudioClip popSound;
    private AudioSource audioSource;

    void Start()
    {
        // Get or add an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object is the bee
        if (collision.CompareTag("Bee"))
        {
            // Play pop sound
            if (popSound != null)
            {
                audioSource.PlayOneShot(popSound);
            }

            // Instantiate pop effect
            if (popEffect != null)
            {
                Instantiate(popEffect, transform.position, Quaternion.identity);
            }

            // Update score
            GameManager.Instance.AddScore(scoreValue);

            // Destroy the balloon
            Destroy(gameObject);
        }
    }
}
