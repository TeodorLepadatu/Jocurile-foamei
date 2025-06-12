using UnityEngine;

public class HeartPickup : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			PlayerControllerFuturistic player = other.GetComponent<PlayerControllerFuturistic>();
			if (player != null)
			{
				player.AddLife();
			}

			Destroy(gameObject); 
		}
	}
}
