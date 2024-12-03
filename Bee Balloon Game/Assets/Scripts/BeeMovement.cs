using UnityEngine;

public class BeeMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 50f; // Speed of the bee's movement
    public Vector2 boundsMin = new Vector2(-8f, -4.5f); // Minimum x and y boundaries
    public Vector2 boundsMax = new Vector2(8f, 4.5f);  // Maximum x and y boundaries

    void Update()
    {
        // Follow the mouse position
        FollowMouse();
    }

    private void FollowMouse()
    {
        // Get mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Clamp the position to keep the bee within the bounds
        mousePosition.x = Mathf.Clamp(mousePosition.x, boundsMin.x, boundsMax.x);
        mousePosition.y = Mathf.Clamp(mousePosition.y, boundsMin.y, boundsMax.y);

        // Keep z position unchanged
        mousePosition.z = transform.position.z;

        // Smoothly move the bee towards the mouse position
        transform.position = Vector3.Lerp(transform.position, mousePosition, speed * Time.deltaTime);
    }
}
