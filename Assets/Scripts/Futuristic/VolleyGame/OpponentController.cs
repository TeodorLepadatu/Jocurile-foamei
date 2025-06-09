using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class OpponentController : MonoBehaviour
{
	[Header("References")]
	public Transform ballTransform;

	[Header("Movement Settings")]

	public float moveSpeed = 5f;
	public float xOffsetFromBall = 1f;
	public float yOffsetFromBall = 0.5f;

	[Header("Court Settings")]
	public float netX = 0f;

	[Header("Smash Settings")]
	public float hitStrength = 10f;

	private Rigidbody2D rb;
	private Vector2 targetPos;

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
			
			targetPos = new Vector2(
				ballTransform.position.x + xOffsetFromBall,
				ballTransform.position.y + yOffsetFromBall
			);
		}
		else
		{
		
			targetPos = rb.position;
		}
	}

	void FixedUpdate()
	{
	
		Vector2 dir = (targetPos - rb.position).normalized;

		rb.linearVelocity = dir * moveSpeed;
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (!collision.collider.CompareTag("Ball")) return;

		Rigidbody2D ballRb = collision.collider.attachedRigidbody;
		if (ballRb == null) return;

		ContactPoint2D cp = collision.contacts[0];
		Vector2 smashDir = (cp.point - (Vector2)transform.position).normalized;

		ballRb.linearVelocity = new Vector2(ballRb.linearVelocity.x, 0f);
		ballRb.AddForce(smashDir * hitStrength, ForceMode2D.Impulse);
	}
}
