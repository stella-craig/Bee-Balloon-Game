using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DrawBoundary : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private PolygonCollider2D polygonCollider;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        polygonCollider = GetComponentInParent<PolygonCollider2D>();

        DrawLine();
    }

    private void DrawLine()
    {
        Vector2[] points = polygonCollider.points;

        lineRenderer.positionCount = points.Length + 1; // Add one to close the loop
        for (int i = 0; i < points.Length; i++)
        {
            lineRenderer.SetPosition(i, polygonCollider.transform.TransformPoint(points[i]));
        }
        // Close the loop
        lineRenderer.SetPosition(points.Length, polygonCollider.transform.TransformPoint(points[0]));
    }
}
