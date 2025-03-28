using UnityEngine;
using UnityEngine.SceneManagement;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

    [SerializeField] AudioSource footstepSource;
    [SerializeField] AudioSource honeyjamSource;
    [SerializeField] AudioSource exitSource;
    [SerializeField] AudioSource jumpSource;
    [SerializeField] AudioSource squishSource;

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

    public void PlayHoneyjam()
    {
        honeyjamSource.Play();
    }

    public void PlayExit()
    {
        exitSource.Play();
    }

    public void PlayJump()
    {
        jumpSource.Play();
    }

    public void PlaySquish()
    {
        squishSource.Play();
    }
}