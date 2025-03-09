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
    }

    public void ResetLevel()
    {
        if (cam != null) cam.orthographicSize = 5; // Reset camera zoom
        SpawnPlayer(); // Respawn player
    }
}
