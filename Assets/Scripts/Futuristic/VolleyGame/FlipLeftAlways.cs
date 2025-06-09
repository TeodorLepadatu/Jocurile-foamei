using UnityEngine;

[ExecuteAlways]
public class FlipLeftAlways : MonoBehaviour
{
	void Start()
	{
		
		var srs = GetComponentsInChildren<SpriteRenderer>();
		// Pentru fiecare, seteaz� flipX = true
		foreach (var sr in srs)
			sr.flipX = true;
	}

}
