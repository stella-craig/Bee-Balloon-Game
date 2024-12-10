


using UnityEngine;

public class SpinningRedZone : MonoBehaviour
{
    public Transform rotationCenter; // The center point of rotation
    public float rotationSpeed = 30f; // Speed of rotation

    private void Update()
    {
        if (rotationCenter != null)
        {
            // Rotate around the specified center point
            transform.RotateAround(rotationCenter.position, Vector3.forward, rotationSpeed * Time.deltaTime);
        }
        else
        {
            Debug.LogWarning("Rotation center is not assigned!");
        }
    }
}
