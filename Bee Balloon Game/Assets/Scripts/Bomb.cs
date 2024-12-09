using UnityEngine;

public class Bomb : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bee"))
        {
            Debug.Log("Bomb hit! Losing a life.");
            GameManager.Instance.LoseLife();
        }
    }
}
