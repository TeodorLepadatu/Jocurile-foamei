using UnityEngine;

public class HeartPickup : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{   // Check if the collider belongs to the player
		if (other.CompareTag("Player"))
		{
			PlayerControllerFuturistic player = other.GetComponent<PlayerControllerFuturistic>();
			if (player != null)
			{
				player.AddLife(); // Increment player's life count
			}

			Destroy(gameObject); // Destroy the heart pickup after collection
		}
	}
}
