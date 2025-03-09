using UnityEngine;
using UnityEngine.Video;

public class FinalExit : MonoBehaviour
{

    public VideoPlayer videoPlayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SFXManager.instance.PlayExit();
        videoPlayer.Play();
        if (collision.gameObject.CompareTag("Player")) GameManager.instance.EndGame();
    }
}
