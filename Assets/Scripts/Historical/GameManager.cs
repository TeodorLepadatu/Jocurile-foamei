using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public static void GameOver()
    {
        GameObject player = FindObjectOfType<PlayerController>().gameObject;
        if (player != null)
        {
            Destroy(player);
        }
        SceneManager.LoadScene("DeathScreenScene");
    }

    public void SpawnGold(GameObject goldPrefab)
    {
        Vector2 offset = Random.insideUnitCircle * 1f;
        Vector3 spawnPos = transform.position + new Vector3(offset.x, offset.y, 0);
        Instantiate(goldPrefab, spawnPos, Quaternion.identity);
    }
}
