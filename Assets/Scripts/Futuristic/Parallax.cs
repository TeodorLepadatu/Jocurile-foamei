using UnityEngine;

public class Parallax : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private float length, startpos;
    public GameObject cam;
	public float parallaxEffect;

	void Start()
    {
        startpos = transform.position.x;
		length = GetComponent<SpriteRenderer>().bounds.size.x;

	}

    // Update is called once per frame
    void Update()
    {
        float dist = cam.transform.position.x * parallaxEffect;
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
		transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        if(temp > startpos + length)
		{
			startpos += length;
		}
		else if (temp < startpos - length)
		{
			startpos -= length;
		}
	}
}
