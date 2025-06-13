using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public GameObject[] enemyPrefabs;

    [Header("Boss Music")]
    [SerializeField] private AudioClip bossMusicClip; 
    private AudioSource bossMusicSource;

    [Header("Attributes")]
    [SerializeField] public int baseEnemies = 8;
    [SerializeField] public float enemiesPerSecond = 0.5f;
    [SerializeField] public float timeBetweenWaves = 5f;
    [SerializeField] public float difficultyScalingFactor = 0.75f;

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private bool isSpawning = false;
    private int totalNumberOfWaves = 10; // Total number of waves to spawn

    private bool bossMusicPlaying = false; // Track if boss music is playing

    private void Awake()
    {
        onEnemyDestroy.AddListener(EnemyDestroyed);
        bossMusicSource = gameObject.AddComponent<AudioSource>();
        bossMusicSource.loop = true;
        bossMusicSource.playOnAwake = false;
        bossMusicSource.volume = 0.25f; // Set boss music volume to 0.25
    }

    private void Start()
    {
        StartCoroutine(StartWave());
    }

    private void Update()
    {
        if (!isSpawning)
            return;

        timeSinceLastSpawn += Time.deltaTime;
        enemiesPerSecond += Time.deltaTime * 0.05f;

        if (timeSinceLastSpawn >= 1f / enemiesPerSecond && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            timeSinceLastSpawn = 0f;
        }

        if (enemiesAlive == 0 && enemiesLeftToSpawn == 0)
        {
            EndWave();
        }

        if (currentWave > totalNumberOfWaves)
        {
            Debug.Log("Am terminat TD!");
            PlayerController.minigamesCompleted++;
            SceneManager.LoadScene("MainScene1");
        }
    }

    private void SpawnEnemy()
    {
        GameObject prefabToSpawn;

        if (currentWave % 10 == 0) // Spawn boss every 10th wave
        {
            prefabToSpawn = enemyPrefabs[3]; // Final Boss

            // Start boss music if not already playing
            if (!bossMusicPlaying && bossMusicClip != null)
            {
                bossMusicSource.clip = bossMusicClip;
                bossMusicSource.Play();
                bossMusicPlaying = true;
            }
        }
        else if (currentWave > 5 && Random.value < 0.2f)
        {
            prefabToSpawn = enemyPrefabs[1]; // Tank
        }
        else if (Random.value < 0.3f)
        {
            prefabToSpawn = enemyPrefabs[2]; // Fast Enemy
        }
        else
        {
            prefabToSpawn = enemyPrefabs[0]; // Normal Enemy
        }

        Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity);
    }

    private void EnemyDestroyed()
    {
        enemiesAlive--;
    }

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave();
    }

    private void EndWave()
    {
        // Stop boss music if this was a boss wave
        if (currentWave % 10 == 0 && bossMusicPlaying)
        {
            bossMusicSource.Stop();
            bossMusicPlaying = false;
        }

        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;
        StartCoroutine(StartWave());
    }

    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies + Mathf.Pow(currentWave, difficultyScalingFactor));
    }
}
