using UnityEngine;

public class HeartSpawner : MonoBehaviour
{
	public GameObject heart;
	public Transform[] spawnPoints; // Array of spawn points for the hearts
							
	void Start()
    {
		InvokeRepeating("Spawn", 2f, 20f); // Start spawning hearts after 2 seconds, then every 20 seconds
	}

	void Spawn()
	{   // randomly select a spawn point and instantiate a heart
		Instantiate(heart,
			spawnPoints[Random.Range(0, spawnPoints.Length)].position,
			Quaternion.identity);
	}

}
