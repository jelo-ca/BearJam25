using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    public float volume;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] music;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void PlayMenuMusic()
    {
        audioSource.clip = music[0];
        audioSource.Play();

    }

    public void PlayGameMusic()
    {
        if(audioSource.clip != music[1])
        {
            audioSource.clip = music[1];
            audioSource.Play();
        }
    }
}
