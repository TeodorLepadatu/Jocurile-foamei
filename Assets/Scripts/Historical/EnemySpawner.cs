using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// Spawns enemy waves, handles boss music, and manages wave progression for Tower Defence.
/// The next wave starts immediately after all enemies from the previous wave are defeated,
/// except the final boss wave, which ends only when the boss is defeated.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Boss Music")]
    [SerializeField] private AudioClip bossMusicClip;
    private AudioSource bossMusicSource;

    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 5;
    [SerializeField] private float enemiesPerSecond = 0.5f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;
    [SerializeField] private int totalNumberOfWaves = 10;

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private bool isSpawning;
    private bool bossMusicPlaying;

    private void Awake()
    {
        onEnemyDestroy.RemoveAllListeners();
        onEnemyDestroy.AddListener(OnEnemyDestroyed);

        bossMusicSource = gameObject.AddComponent<AudioSource>();
        bossMusicSource.loop = true;
        bossMusicSource.playOnAwake = false;
        bossMusicSource.volume = 1f;
    }

    private void Start()
    {
        StartWave();
    }

    private void Update()
    {
        if (!isSpawning)
            return;

        timeSinceLastSpawn += Time.deltaTime;
        enemiesPerSecond += Time.deltaTime * 0.05f;

        if (enemiesLeftToSpawn > 0 && timeSinceLastSpawn >= 1f / enemiesPerSecond)
        {
            SpawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            timeSinceLastSpawn = 0f;
        }
    }

    private void SpawnEnemy()
    {
        GameObject prefabToSpawn;
        bool isBossWave = (currentWave == totalNumberOfWaves);

        if (isBossWave)
        {
            prefabToSpawn = enemyPrefabs[3];
            if (!bossMusicPlaying && bossMusicClip != null)
            {
                bossMusicSource.clip = bossMusicClip;
                bossMusicSource.Play();
                bossMusicPlaying = true;
            }
        }
        else if (currentWave > 5 && Random.value < 0.2f)
        {
            prefabToSpawn = enemyPrefabs[1];
        }
        else if (Random.value < 0.3f)
        {
            prefabToSpawn = enemyPrefabs[2];
        }
        else
        {
            prefabToSpawn = enemyPrefabs[0];
        }

        Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity);
    }

    private void OnEnemyDestroyed()
    {
        enemiesAlive = Mathf.Max(0, enemiesAlive - 1);

        // Regular wave: proceed to next wave when all enemies are gone
        if (enemiesLeftToSpawn == 0 && enemiesAlive == 0)
        {
            EndWave();
        }
    }

    private void StartWave()
    {
        isSpawning = true;
        timeSinceLastSpawn = 0f;
        enemiesAlive = 0;

        if (currentWave == totalNumberOfWaves)
        {
            enemiesLeftToSpawn = 1;
        }
        else
        {
            enemiesLeftToSpawn = CalculateEnemiesForWave(currentWave);
        }
    }

    private void EndWave()
    {
        isSpawning = false;
        // Final boss wave: end game when the boss dies
        if (currentWave > totalNumberOfWaves)
        {
            if (bossMusicPlaying)
            {
                bossMusicSource.Stop();
                bossMusicPlaying = false;
            }

            Debug.Log("Am terminat TD!");
            PlayerController.minigamesCompleted++;
            SceneManager.LoadScene("MainScene1");
        }
        currentWave++;
        StartWave();
    }

    private int CalculateEnemiesForWave(int waveNumber)
    {
        return Mathf.RoundToInt(baseEnemies + Mathf.Pow(waveNumber, difficultyScalingFactor));
    }
}
