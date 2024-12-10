using UnityEngine;

public class RedZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bee"))
        {
            Debug.Log("RedZone hit! Losing a life.");
            GameManager.Instance.LoseLife();
        }
    }
}
