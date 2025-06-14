using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

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

    public GameObject winPanel;
    public Text winText;
    public Text twistText;
    public GameObject losePanel;
    public Text coinText;
        

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
        coinText.text = CurrencyHolder.getCurrency().ToString();
    }

    public void PlayerDied()
    {
        losePanel.SetActive(true);
        StartCoroutine(ReloadSceneAfterDelay(3f));
    }
    public void PlayerWon()
    {
        StartCoroutine(ShowWinSequence());
    }

    IEnumerator ShowWinSequence()
    {
        winPanel.SetActive(true);
        winText.gameObject.SetActive(true);
        twistText.gameObject.SetActive(false);

        yield return new WaitForSeconds(2f);

        twistText.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("CMinigame2");
    }

    IEnumerator ReloadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Update()
    {
        eggTimer += Time.deltaTime;
        appleTimer += Time.deltaTime;

        if (eggTimer >= 4f && GameObject.FindGameObjectsWithTag("Egg").Length == 0) // every 4 seconds a new object will be spawned
        {
            SpawnEgg();
            eggTimer = 0;
        }

        if (appleTimer >= 8f && GameObject.FindGameObjectsWithTag("GoldenApple").Length == 0) // every 8 seconds a new object will be spawned
        {
            SpawnGoldenApple();
            appleTimer = 0;
        }
    }

    public void MonsterDefeated()
    {
        currentMonster++;
        CurrencyHolder.addCurrency(10);
        coinText.text = CurrencyHolder.getCurrency().ToString();

        if (currentMonster < monsterPrefabs.Length)
        {
            SpawnMonster();
        }
        else
        {
            CM2.Instance.PlayerWon();
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
