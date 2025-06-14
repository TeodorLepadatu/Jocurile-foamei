using UnityEngine;

public class Parallax : MonoBehaviour
{
   

    private float length, startpos; // the length of the sprite
	public GameObject cam;
	public float parallaxEffect;

	void Start()
	{   // Initialize the starting position and length of the sprite
		startpos = transform.position.x;
		length = GetComponent<SpriteRenderer>().bounds.size.x;
		cam = GameObject.FindGameObjectWithTag("MainCamera");

	}

    void Update()
    {
        float dist = cam.transform.position.x * parallaxEffect; // the distance moved by the camera multiplied by the parallax effect factor
		float temp = (cam.transform.position.x * (1 - parallaxEffect)); // the temporary position of the camera adjusted by the parallax effect
		transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z); // update the position of the sprite based on the camera's position and the parallax effect

		// if the camera has moved beyond the length of the sprite, update the start position
		if (temp > startpos + length)
		{
			startpos += length;
		}
		// if the camera has moved before the start position minus the length of the sprite, update the start position
		else if (temp < startpos - length)
		{
			startpos -= length;
		}
	}
}
