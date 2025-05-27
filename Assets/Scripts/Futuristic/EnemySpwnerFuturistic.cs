using UnityEngine;

public class EnemySpwnerFuturistic : MonoBehaviour
{
    public GameObject[] enemies;
    public Transform[] spawnPoints;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        InvokeRepeating("Spawn", 2f, 6f);
	}

    // Update is called once per frame
    void Update()
    {
       
	}

    void Spawn()
    {
        Instantiate(enemies[Random.Range(0, enemies.Length)],
			spawnPoints[Random.Range(0, spawnPoints.Length)].position,
			Quaternion.identity);
	}
}
