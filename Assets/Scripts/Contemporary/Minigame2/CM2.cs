using UnityEngine;

public class CM2 : MonoBehaviour
{
    public static CM2 Instance;
    public GameObject player;
    public GameObject[] monsterPrefabs;
    public GameObject eggPrefab;
    public GameObject goldenApplePrefab;

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

        if (appleTimer >= 10f && GameObject.FindGameObjectsWithTag("GoldenApple").Length == 0)
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
            player.transform.position = Vector2.zero;
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
        float margin = 1f;
        float x = Random.Range(-7f, 7f);
        float y = Random.Range(-3f, 3f);
        return new Vector2(x, y);
    }
}
