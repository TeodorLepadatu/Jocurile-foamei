using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovePlayer : MonoBehaviour
{
	[Header("Movement Settings")]
	[Tooltip("Viteza de miscare")]
	public float moveSpeed = 30f;

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

		bool running = false;

		if (Controls.GetKey(Controls.Action.MoveRight)) { moveX = 1f; running = true; }
		if (Controls.GetKey(Controls.Action.MoveLeft)) {moveX = -1f; running = true; }
		if (Controls.GetKey(Controls.Action.MoveUp)) {moveY = 1f; running = true; }
		if (Controls.GetKey(Controls.Action.MoveDown)) {moveY = -1f; running = true; }

		movement = new Vector2(moveX, moveY).normalized;

		animator.SetBool("isRunning", running);
		//Debug.Log($"Running: {running}");
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
