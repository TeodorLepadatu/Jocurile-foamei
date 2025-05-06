using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public GameObject[] enemyPrefabs;

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

    private void Awake()
    {
        onEnemyDestroy.AddListener(EnemyDestroyed);
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

        // Gradually increase the spawn rate over time
        enemiesPerSecond += Time.deltaTime * 0.05f; // Adjust the multiplier (0.05f) to control the rate of increase

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

        if (currentWave > 10)
        {
            Debug.Log("Am terminat!");
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
        }
        else if (currentWave > 5 && Random.value < 0.2f) // 20% chance for tank after wave 5
        {
            prefabToSpawn = enemyPrefabs[1]; // Tank
        }
        else if (Random.value < 0.3f) // 30% chance for fast enemy
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

