using UnityEngine;

public class CM2 : MonoBehaviour
{
    public static CM2 Instance;
    public GameObject player;
    public GameObject[] monsterPrefabs;
    public GameObject eggPrefab;
    public GameObject goldenApplePrefab;
    private Vector2[] positions = {
            new Vector2(6.27f, -2.8f),
            new Vector2(2.78f, -2.68f),
            new Vector2(-1.27f, -2.74f),
            new Vector2(-6.12f, -2.78f),
            new Vector2(-4.95f, -0.84f),
            new Vector2(-2.37f, -0.82f),
            new Vector2(1.02f, -0.86f),
            new Vector2(3.93f, -0.39f),
            new Vector2(3.04f, 1.16f),
            new Vector2(0.87f, 1.2f),
            new Vector2(-2.39f, 1.2f)
        };
        

    private int currentMonster = 0;
    private float eggTimer;
    private float appleTimer;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SpawnMonster();
    }

    void Update()
    {
        eggTimer += Time.deltaTime;
        appleTimer += Time.deltaTime;

        if (eggTimer >= 4f && GameObject.FindGameObjectsWithTag("Egg").Length == 0)
        {
            SpawnEgg();
            eggTimer = 0;
        }

        if (appleTimer >= 8f && GameObject.FindGameObjectsWithTag("GoldenApple").Length == 0)
        {
            SpawnGoldenApple();
            appleTimer = 0;
        }
    }

    public void MonsterDefeated()
    {
        currentMonster++;

        if (currentMonster < monsterPrefabs.Length)
        {
            SpawnMonster();
        }
        else
        {
            Debug.Log("All monsters defeated! You win!");
            // You can trigger a Win screen or reload scene, etc.
        }
    }


    void SpawnMonster()
    {
        if (currentMonster < monsterPrefabs.Length)
        {
            Instantiate(monsterPrefabs[currentMonster], Vector2.zero, Quaternion.identity);
        }
    }

    void SpawnEgg()
    {
        Vector2 pos = RandomPosition();
        Instantiate(eggPrefab, pos, Quaternion.identity);
    }

    void SpawnGoldenApple()
    {
        Vector2 pos = RandomPosition();
        Instantiate(goldenApplePrefab, pos, Quaternion.identity);
    }

    Vector2 RandomPosition()
    {
        return positions[Random.Range(0, 11)];
    }
}
