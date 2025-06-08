using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovePlayer : MonoBehaviour
{
	[Header("Movement Settings")]
	[Tooltip("Viteza de miscare")]
	public float moveSpeed = 7f;

	private Rigidbody2D rb;
	private Vector2 movement;
	private Animator animator;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		// dezactivam gravitatia
		rb.gravityScale = 0f;

		rb.freezeRotation = true;
	}

	void Update()
	{
		
		float moveX = 0f;
		float moveY = 0f;

		if (Input.GetKey(KeyCode.RightArrow)) moveX = 1f;
		if (Input.GetKey(KeyCode.LeftArrow)) moveX = -1f;
		if (Input.GetKey(KeyCode.UpArrow)) moveY = 1f;
		if (Input.GetKey(KeyCode.DownArrow)) moveY = -1f;

		movement = new Vector2(moveX, moveY).normalized;

		bool running = movement != Vector2.zero;
		animator.SetBool("isRunning", running);
	}

	void FixedUpdate()
	{
		if (movement != Vector2.zero)
		{
			rb.linearVelocity = movement * moveSpeed;
		}
		else
		{
			rb.linearVelocity = Vector2.zero;
		}
	}
}
