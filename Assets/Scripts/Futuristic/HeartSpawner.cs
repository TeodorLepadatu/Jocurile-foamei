using UnityEngine;

public class HeartSpawner : MonoBehaviour
{
	public GameObject heart;
	public Transform[] spawnPoints;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
		InvokeRepeating("Spawn", 2f, 20f);
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	void Spawn()
	{
		Instantiate(heart,
			spawnPoints[Random.Range(0, spawnPoints.Length)].position,
			Quaternion.identity);
	}

}
