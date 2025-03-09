using UnityEngine;

public class Jam : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //cam.orthographicSize /= 2;
            collision.gameObject.GetComponent<PlayerController>().Grow();
            Destroy(gameObject);
        }
    }
}
