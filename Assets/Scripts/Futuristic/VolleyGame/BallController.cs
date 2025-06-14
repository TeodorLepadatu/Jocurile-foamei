using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallController : MonoBehaviour
{
	[Header("Hit Settings")]
	public float playerHitStrength = 8f;
	public float opponentHitStrength = 12f; // bigger hit for opponent

	[Header("Border Bounce Settings")]
	public float borderBounceMultiplier = 1.5f; // multiplier for bounce strength when hitting borders

	[Header("Physics Settings")]
	public PhysicsMaterial2D bouncyMaterial;

	[Header("Speed Clamp")]
	public float maxSpeed = 14f; // maximum speed of the ball

	private Rigidbody2D rb;
	private Collider2D col;

	void Awake()
	{   // Initialize the ball controller
		rb = GetComponent<Rigidbody2D>();
		col = GetComponent<Collider2D>();
		if (col != null && bouncyMaterial != null)
			col.sharedMaterial = bouncyMaterial;

		rb.bodyType = RigidbodyType2D.Dynamic;
		rb.gravityScale = 1f;
		rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
		rb.freezeRotation = true;
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		string tag = collision.collider.tag;

	
		if (tag == "Player" || tag == "Opponent")
		{
		
			float strength = (tag == "Player") //setting hit strength based on the tag
				? playerHitStrength
				: opponentHitStrength;

			
			Vector2 hitDir = (rb.position - (Vector2)collision.transform.position).normalized; // direction of the hit

			rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // reset vertical velocity to prevent upward movement
			// Apply the hit force in the direction of the hit
			rb.AddForce(hitDir * strength, ForceMode2D.Impulse);
			return;
		}

		//collision for border bounce	
		if (tag == "Border")
		{
			Vector2 incoming = rb.linearVelocity;
			Vector2 normal = collision.contacts[0].normal;

			Vector2 reflected = Vector2.Reflect(incoming, normal); // reflect the ball off the border
			Vector2 boosted = reflected.normalized // boost the speed after reflection
								* incoming.magnitude
								* borderBounceMultiplier;

			float speed = Mathf.Min(boosted.magnitude, maxSpeed); // clamp the speed to maxSpeed
			rb.linearVelocity = reflected.normalized * speed; // set the new velocity
		}
	}

	void LateUpdate()
	{

		Vector3 p = transform.position;
		p.z = 0f; // ensure the ball stays in 2D space
		transform.position = p;
	}
}
