using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class OpponentController : MonoBehaviour
{
	[Header("References")]
	public Transform ballTransform;

	public Transform homePoint;

	[Header("Movement Settings")]
	public float moveSpeed = 5f;
	public float xOffsetFromBall = 1f;
	public float yOffsetFromBall = -0.5f;

	[Header("Court Settings")]
	public float netX = 0f;

	[Header("Smash Settings")]
	public float hitStrength = 10f;

	private Rigidbody2D rb;
	private Vector2 targetPos;

	void Awake()
	{   // Initialize the opponent controller
		rb = GetComponent<Rigidbody2D>();
		rb.bodyType = RigidbodyType2D.Dynamic;
		rb.gravityScale = 0f;
		rb.freezeRotation = true;
		rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
	}

	void Update()
	{   // Update the target position based on the ball's position
		if (ballTransform.position.x > netX) //if the ball is on tge opponents side
		{
			targetPos = new Vector2(
				ballTransform.position.x + xOffsetFromBall,
				ballTransform.position.y + yOffsetFromBall
			);
		}
		else
		{   // If the ball is on the opponent's side, move to the home point
			targetPos = homePoint.position;
		}
	}

	void FixedUpdate()
	{
		const float stopThreshold = 0.3f; // distance at which the opponent stops moving

		Vector2 delta = targetPos - rb.position;
		float dist = delta.magnitude;

		if (dist < stopThreshold)
		{
			rb.linearVelocity = Vector2.zero;
			rb.position = targetPos;
		}
		else
		{
	
			Vector2 dir = delta / dist;       // same as normalized
			rb.linearVelocity = dir * moveSpeed;
		}
	}


	void OnCollisionEnter2D(Collision2D collision)
	{
		if (!collision.collider.CompareTag("Ball")) return;
		// Handle collision with the ball
		Rigidbody2D ballRb = collision.collider.attachedRigidbody;
		if (ballRb == null) return;
		
		ContactPoint2D cp = collision.contacts[0]; // get the contact point of the collision
		Vector2 smashDir = (cp.point - (Vector2)transform.position).normalized; // direction of the smash

		ballRb.linearVelocity = new Vector2(ballRb.linearVelocity.x, 0f); // reset vertical velocity to prevent upward movement
		ballRb.AddForce(smashDir * hitStrength, ForceMode2D.Impulse); // apply the smash force in the direction of the smash
	}
}
