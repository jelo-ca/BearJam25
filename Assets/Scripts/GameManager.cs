using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] GameObject playerPrefab;

    private GameObject player;
    private Transform spawnPoint;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        spawnPoint = GameObject.FindWithTag("SpawnPoint").transform;

        SpawnPlayer();
    }

    void Update()
    {
        
    }

    private void SpawnPlayer()
    {
        if (player == null) Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        else
        {
            player.transform.position = spawnPoint.position;
        }
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ResetLevel()
    {
        //Reset Level Size


        //Reset Spawn Player
        SpawnPlayer();
    }
}
