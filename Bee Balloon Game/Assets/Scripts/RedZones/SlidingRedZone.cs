using UnityEngine;

public class EasingSpike : MonoBehaviour
{
    public float speed = 1f; // Speed of the movement
    public float pauseTime = 0.5f; // Time to pause at full height or fully hidden

    private Vector3 initialScale; // The initial scale of the spike
    private bool isGrowing = true; // Whether the spike is growing or shrinking
    private float timer = 0f; // Pause timer
    private Collider2D spikeCollider;

    private void Start()
    {
        initialScale = transform.localScale;
        transform.localScale = new Vector3(initialScale.x, 0, initialScale.z); // Start hidden
        spikeCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }

        if (isGrowing)
        {
            // Increase the Y scale
            transform.localScale = new Vector3(
                initialScale.x,
                Mathf.MoveTowards(transform.localScale.y, initialScale.y, speed * Time.deltaTime),
                initialScale.z
            );

            // Check if fully grown
            if (Mathf.Approximately(transform.localScale.y, initialScale.y))
            {
                isGrowing = false; // Start shrinking
                timer = pauseTime; // Pause at full height
            }
        }
        else
        {
            // Decrease the Y scale
            transform.localScale = new Vector3(
                initialScale.x,
                Mathf.MoveTowards(transform.localScale.y, 0, speed * Time.deltaTime),
                initialScale.z
            );

            // Check if fully hidden
            if (Mathf.Approximately(transform.localScale.y, 0))
            {
                isGrowing = true; // Start growing again
                timer = pauseTime; // Pause at fully hidden
            }
        }

        // Enable/disable the collider based on visibility
        spikeCollider.enabled = transform.localScale.y > 0;
    }
}
