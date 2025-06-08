using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallController : MonoBehaviour
{
	[Header("Hit Settings")]
	public float hitStrength = 8f;

	[Header("Border Bounce Settings")]
	public float borderBounceMultiplier = 1.5f;

	[Header("Physics Settings")]
	public PhysicsMaterial2D bouncyMaterial;

	public float maxSpeed = 14f;

	private Rigidbody2D rb;
	private Collider2D col;

	void Awake()
	{
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

		if (collision.collider.CompareTag("Player"))
		{
			Vector2 hitDir = (rb.position - (Vector2)collision.transform.position).normalized;
			rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
			rb.AddForce(hitDir * hitStrength, ForceMode2D.Impulse);
			return;
		}

		if (collision.collider.CompareTag("Border"))
		{
			Vector2 incoming = rb.linearVelocity;
			Vector2 normal = collision.contacts[0].normal;

			Vector2 reflected = Vector2.Reflect(incoming, normal);
			Vector2 boosted = reflected.normalized
								* incoming.magnitude
								* borderBounceMultiplier;

			float clampedMag = Mathf.Min(boosted.magnitude, maxSpeed);
			rb.linearVelocity = boosted.normalized * clampedMag;
		}
	}

	void LateUpdate()
	{
		Vector3 pos = transform.position;
		pos.z = 0f;
		transform.position = pos;
	}

}
