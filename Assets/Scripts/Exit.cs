using UnityEngine;

public class Exit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SFXManager.instance.PlayExit();
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Player")) GameManager.instance.NextLevel();
    }
}
