using UnityEngine;
using UnityEngine.SceneManagement;

public class DartMonkey : MonoBehaviour
{
    public bool playerNearby = false;
    public static bool hasCompletedMinigame = false;
    private void Start()
    {
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            playerNearby = true;
            if(!hasCompletedMinigame)
            {
                PlayerController.resetGold = false;
                SceneManager.LoadScene("TowerDefence");

                hasCompletedMinigame = true;
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
