using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private GameObject playerPrefab;
    private GameObject player;
    private Transform spawnPoint;
    private Camera cam;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to scene load event
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializeLevel();

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (MusicManager.instance != null)
            {
                MusicManager.instance.PlayMenuMusic();
            }
            else
            {
                Debug.LogWarning("MusicManager instance is null!");
            }
        }
    }

    private void InitializeLevel()
    {
        player = GameObject.FindWithTag("Player");
        UpdateSpawnPoint();
        UpdateCamera();

        SpawnPlayer();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Ensure references are updated when a new scene loads
        UpdateSpawnPoint();
        UpdateCamera();
        SpawnPlayer();
    }

    private void UpdateSpawnPoint()
    {
        GameObject sp = GameObject.FindWithTag("SpawnPoint");
        if (sp != null) spawnPoint = sp.transform;
    }

    private void UpdateCamera()
    {
        GameObject mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        if (mainCam != null) cam = mainCam.GetComponent<Camera>();
    }

    private void SpawnPlayer()
    {
        if (spawnPoint == null) return; // Prevent null reference errors

        if (player != null) Destroy(player.gameObject);
        player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    public void NextLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        MusicManager.instance.PlayGameMusic();
        Time.timeScale = 1;
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
        MusicManager.instance.PlayGameMusic();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
        MusicManager.instance.PlayMenuMusic();
    }
}
