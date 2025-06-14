using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// Spawns enemy waves, handles boss music, and manages wave progression for Tower Defence.
/// The next wave starts immediately after all enemies from the previous wave are defeated.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public GameObject[] enemyPrefabs;

    [Header("Boss Music")]
    [SerializeField] private AudioClip bossMusicClip;
    private AudioSource bossMusicSource;

    [Header("Attributes")]
    [SerializeField] public int baseEnemies = 5;
    [SerializeField] public float enemiesPerSecond = 0.5f;
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
        // Ensure no duplicate listeners across reloads
        onEnemyDestroy.RemoveAllListeners();
        onEnemyDestroy.AddListener(EnemyDestroyed);

        bossMusicSource = gameObject.AddComponent<AudioSource>();
        bossMusicSource.loop = true;
        bossMusicSource.playOnAwake = false;
        bossMusicSource.volume = 1f; // full music relative volume; master via AudioListener
    }

    private void Start()
    {
        StartWave(); // Start the first wave immediately
    }

    private void Update()
    {
        if (!isSpawning)
            return;

        timeSinceLastSpawn += Time.deltaTime;
        enemiesPerSecond += Time.deltaTime * 0.05f;

        // Spawn at the current rate until we've spawned them all
        if (timeSinceLastSpawn >= 1f / enemiesPerSecond && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            timeSinceLastSpawn = 0f;
        }

        // Once no enemies remain (or less) AND none left to spawn, end the wave
        if (enemiesAlive <= 0 && enemiesLeftToSpawn == 0)
        {
            EndWave();
        }

        // After final wave completes, return to main scene
        if (currentWave > totalNumberOfWaves)
        {
            Debug.Log("Am terminat TD!");
            PlayerController.minigamesCompleted++;
            SceneManager.LoadScene("MainScene1");
        }
    }

    /// <summary>
    /// Spawns an enemy based on the current wave and random chance.
    /// Starts boss music if a boss is spawned.
    /// </summary>
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

    /// <summary>
    /// Called when an enemy is destroyed.
    /// Clamps the alive count to never go below zero.
    /// </summary>
    private void EnemyDestroyed()
    {
        enemiesAlive = Mathf.Max(0, enemiesAlive - 1);
    }

    /// <summary>
    /// Starts a new wave immediately.
    /// For boss waves, only one boss is spawned.
    /// </summary>
    private void StartWave()
    {
        isSpawning = true;
        if (currentWave % 10 == 0)
        {
            // Boss wave: only spawn one
            enemiesLeftToSpawn = 1;
        }
        else
        {
            enemiesLeftToSpawn = EnemiesPerWave();
        }
    }

    /// <summary>
    /// Ends the current wave, stops boss music if needed, and immediately starts the next wave.
    /// </summary>
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
        StartWave(); // Immediately start the next wave
    }

    /// <summary>
    /// Calculates the number of enemies for the current wave.
    /// </summary>
    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies + Mathf.Pow(currentWave, difficultyScalingFactor));
    }
}
