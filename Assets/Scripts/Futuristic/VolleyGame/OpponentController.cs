using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class OpponentController : MonoBehaviour
{
	[Header("References")]
	[Tooltip("The ball to chase")]
	public Transform ballTransform;

	[Header("Movement")]
	[Tooltip("How fast the opponent runs")]
	public float moveSpeed = 15f;

	[Header("Court Settings")]
	[Tooltip("X position of the net (middle of the court)")]
	public float netX = 0f;

	private Rigidbody2D rb;
	private float direction;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();

		rb.bodyType = RigidbodyType2D.Dynamic;
		rb.gravityScale = 0f;
		rb.freezeRotation = true;
		rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
	}

	void Update()
	{
	
		if (ballTransform.position.x > netX)
		{
			float dx = ballTransform.position.x - transform.position.x;
			direction = Mathf.Abs(dx) > 0.1f ? Mathf.Sign(dx) : 0f;
		}
		else
		{
		
			direction = 0f;
		}
	}

	void FixedUpdate()
	{
		
		Vector2 v = rb.linearVelocity;
		v.x = direction * moveSpeed;
		rb.linearVelocity = v;
	}
}
