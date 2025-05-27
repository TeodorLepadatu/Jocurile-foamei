using UnityEngine;

public class PlayerControllerFuturistic : MonoBehaviour
{
	private Animator anim;
	private bool isRunning;
	public float speed = 0;
	public float acceleration = 0.000000000001f;
	private Rigidbody2D rb;
	public float jumpForce = 8f;
	public bool isJumping = false;

	void Start()
	{
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();

	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Return))
		{
			isRunning = !isRunning; 
			anim.SetBool("isRunning", isRunning);
			speed = isRunning ? 2f : 0f;
		}

		if(Input.GetKeyDown(KeyCode.Space) && !isJumping)
		{
			Jump();
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

	void Jump()
	{
		Vector2 velocity = rb.linearVelocity;
		velocity.y = jumpForce;
		rb.linearVelocity = velocity;
		isJumping = true;
	}

	void OnCollissionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Ground"))
		{
			isJumping = false;
		}
	}
}
