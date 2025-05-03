using UnityEngine;

public class GoldCollectible : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.ChangeGold(1);
            Destroy(gameObject);
        }
    }
}
