using UnityEngine;

public class Honey : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.localScale /= 2;
            Destroy(gameObject);
        }
    }
}
