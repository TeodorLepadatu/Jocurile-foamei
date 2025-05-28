using UnityEngine;
using System.Collections;

public class EnemySpwnerFuturistic : MonoBehaviour
{
	public GameObject[] enemies;
	public Transform[] spawnPoints;

	private float elapsedTime = 0f;

	void Start()
	{
		StartCoroutine(SpawnEnemies());
	}

	void Update()
	{
		elapsedTime += Time.deltaTime;
	}

	IEnumerator SpawnEnemies()
	{
		while (true)
		{
			Spawn();

			float spawnInterval;

			if (elapsedTime <= 20f)
				spawnInterval = 6f;           
			else if (elapsedTime <= 50f)
				spawnInterval = 3.5f;          
			else
				spawnInterval = 2f;           

			yield return new WaitForSeconds(spawnInterval);
		}
	}

	void Spawn()
	{
		Instantiate(
			enemies[Random.Range(0, enemies.Length)],
			spawnPoints[Random.Range(0, spawnPoints.Length)].position,
			Quaternion.identity
		);
	}
}
