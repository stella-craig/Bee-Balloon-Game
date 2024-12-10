using UnityEngine;

public class VisibilityBounds : MonoBehaviour
{
    private PolygonCollider2D boundaryCollider;

    private void Awake()
    {
        // Get the boundary's collider
        boundaryCollider = GetComponent<PolygonCollider2D>();

        if (boundaryCollider == null)
        {
            Debug.LogError("PolygonCollider2D is missing on the boundary object!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Show the red zone when it enters the boundary
        if (other.CompareTag("RedZone"))
        {
            SetVisibility(other, true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Hide the red zone when it exits the boundary
        if (other.CompareTag("RedZone"))
        {
            SetVisibility(other, false);
        }
    }

    private void SetVisibility(Collider2D other, bool visible)
    {
        Renderer[] renderers = other.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = visible;
        }
    }
}
