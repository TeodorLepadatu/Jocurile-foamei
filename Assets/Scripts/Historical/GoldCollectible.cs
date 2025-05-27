using UnityEngine;
using UnityEngine.SceneManagement;

public class GoldCollectible : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.ChangeGold(1);
            /*
            if(SceneManager.GetActiveScene().name == "TowerDefence")
                LevelManager.main.IncreaseCurrency(1);
            */
            Destroy(gameObject);
        }
    }
}
