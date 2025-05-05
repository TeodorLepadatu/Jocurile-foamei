using UnityEngine;

public class SetPosition : MonoBehaviour
{
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		Vector3 screenPosition = new Vector3(0.2f, 0.2f, 10f);
		transform.position = Camera.main.ViewportToWorldPoint(screenPosition);
	}


	// Update is called once per frame
	void Update()
    {
        
    }
}
