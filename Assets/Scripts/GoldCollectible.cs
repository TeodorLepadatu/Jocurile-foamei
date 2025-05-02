using UnityEngine;

public class GoldCollectible : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {

        PlayerController controller = other.GetComponent<PlayerController>();

        if (controller != null)
        {
            controller.ChangeGold(1);
            //UIHandler.instance.SetGoldValue(controller.gold);
            Destroy(gameObject);
        }
    }
}