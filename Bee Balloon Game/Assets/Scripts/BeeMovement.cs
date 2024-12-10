using UnityEngine;

public class BeeMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 1f; // Speed of the bee's movement
    public Vector2 boundsMin = new Vector2(-8f, -4.5f); // Minimum x and y boundaries
    public Vector2 boundsMax = new Vector2(8f, 4.5f);  // Maximum x and y boundaries

    private PolygonCollider2D boundary;

    private void Start()
    {
        // Find the initial boundary in the scene
        FindBoundary();
    }

    private void Update()
    {
        // Follow the mouse position
        FollowMouse();

        // Constrain the bee's position within the boundary
        if (boundary != null)
        {
            Vector3 clampedPosition = boundary.ClosestPoint(transform.position);
            transform.position = new Vector3(clampedPosition.x, clampedPosition.y, transform.position.z);
        }
        else
        {
            Debug.LogWarning("Boundary is not set! Bee's movement is not constrained.");
        }
    }

    private void FollowMouse()
    {
        // Get mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Clamp the position to keep the bee within the specified bounds
        mousePosition.x = Mathf.Clamp(mousePosition.x, boundsMin.x, boundsMax.x);
        mousePosition.y = Mathf.Clamp(mousePosition.y, boundsMin.y, boundsMax.y);

        // Preserve the Z position of the bee
        mousePosition.z = transform.position.z;

        // Smoothly move the bee towards the mouse position
        transform.position = Vector3.Lerp(transform.position, mousePosition, speed * Time.deltaTime);
    }

    public void FindBoundary()
    {
        // Find the active PolygonCollider2D in the scene
        boundary = FindObjectOfType<PolygonCollider2D>();
        if (boundary == null)
        {
            Debug.LogWarning("No PolygonCollider2D found in the scene! Ensure a boundary exists.");
        }
    }
}
