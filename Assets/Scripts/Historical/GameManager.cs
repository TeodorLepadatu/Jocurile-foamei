using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
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
}
