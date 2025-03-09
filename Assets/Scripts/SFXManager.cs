using UnityEngine;
using UnityEngine.SceneManagement;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

    [SerializeField] AudioSource footstepSource;

    public float volume;

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

    public void PlayFootsteps()
    {
        if (!footstepSource.isPlaying)
        {
            footstepSource.Play();
        }
    }

    public void StopFootsteps()
    {
        if (footstepSource.isPlaying)
        {
            footstepSource.Stop();
        }
    }
}