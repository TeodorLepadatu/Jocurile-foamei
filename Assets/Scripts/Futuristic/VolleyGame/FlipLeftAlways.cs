using UnityEngine;

[ExecuteAlways]
public class FlipLeftAlways : MonoBehaviour
{
	void Start()
	{
		
		var srs = GetComponentsInChildren<SpriteRenderer>();
		// Pentru fiecare, seteazã flipX = true
		foreach (var sr in srs)
			sr.flipX = true;
	}

}
