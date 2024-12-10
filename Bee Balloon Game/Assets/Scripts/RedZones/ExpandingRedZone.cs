using UnityEngine;

public class ExpandingRedZone : MonoBehaviour
{
    public float expandSpeed = 2f;    // Speed of expansion and shrinking
    public Vector2 maxScale = new Vector2(8f, 8f); // Maximum scale
    public Vector2 minScale = new Vector2(0.5f, 0.5f); // Minimum scale

    private bool expanding = true;

    private void Update()
    {
        // Calculate new scale based on expanding or shrinking
        Vector3 targetScale = expanding ? new Vector3(maxScale.x, maxScale.y, 1f) : new Vector3(minScale.x, minScale.y, 1f);
        transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, expandSpeed * Time.deltaTime);

        // Check if the target scale is reached
        if (Vector3.Distance(transform.localScale, targetScale) < 0.01f)
        {
            expanding = !expanding; // Switch direction
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bee"))
        {
            Debug.Log("Expanding RedZone hit! Losing a life.");
            GameManager.Instance.LoseLife();
        }
    }
}
