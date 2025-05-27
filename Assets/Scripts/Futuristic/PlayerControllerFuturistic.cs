using UnityEngine;

public class PlayerControllerFuturistic : MonoBehaviour
{
	private Animator anim;
	private bool isRunning;
	public float speed = 0;
	public float acceleration = 0.000000000001f;
	void Start()
	{
		anim = GetComponent<Animator>();
		
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Return))
		{
			isRunning = !isRunning; 
			anim.SetBool("isRunning", isRunning);
			speed = isRunning ? 2f : 0f;
		}


		isRunning = anim.GetBool("isRunning");
		if (isRunning)
		{	
			//speed += acceleration * Time.deltaTime;
			transform.Translate(Vector3.right * Time.deltaTime * speed);
		}
	}

	public bool IsRunning()
	{
		return isRunning;
	}

}
