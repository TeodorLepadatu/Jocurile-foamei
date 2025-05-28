using UnityEngine;

public class PlayerControllerFuturistic : MonoBehaviour
{
	private Animator anim;
	private bool isRunning;
	public float speed = 0;
	public float acceleration = 0.000000000001f;
	private Rigidbody2D rb;
	public float jumpForce = 14f;
	public bool isJumping = false;

	public GameObject[] hearts;
	private int health;


	private bool isDead;
	private bool isAttacking;
	void Start()
	{
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		health = hearts.Length;

	}

	void Update()
	{
		
		isRunning = anim.GetBool("isRunning");
		isDead = anim.GetBool("isDead");
		if (Input.GetKeyDown(KeyCode.Return) && !isDead)
		{
			isRunning = !isRunning; 
			anim.SetBool("isRunning", isRunning);
			speed = isRunning ? 2f : 0f;
		}

		if(Input.GetKeyDown(KeyCode.Space) && !isDead)
		{
			anim.SetBool("isAttacking", true);
		}

		if(Input.GetKeyUp(KeyCode.Space) && !isDead)
		{
			anim.SetBool("isAttacking", false);
		}


		if (Input.GetKeyDown(KeyCode.W) && !isJumping)
		{
			Jump();
		}

		if (IsRunning())
		{	
			//speed += acceleration * Time.deltaTime;
			transform.Translate(Vector3.right * Time.deltaTime * speed);
		}
	}

	public bool IsRunning()
	{
		isDead = anim.GetBool("isDead");
		isRunning = anim.GetBool("isRunning");
		return isRunning && !isDead;
	}

	void Jump()
	{
		Vector2 jumpVelocity = new Vector2(0, jumpForce);
		rb.linearVelocity = jumpVelocity;
		isJumping = true;
	}


	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Ground"))
		{
			isJumping = false;
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Enemy"))
		{
			EnemyControllerFuturistic enemy = other.GetComponent<EnemyControllerFuturistic>();
			if (enemy != null)
			{
				if (!anim.GetBool("isAttacking") && !enemy.isDead)
				{
					TakeDamage();
				}
				else if (anim.GetBool("isAttacking") && !enemy.isDead)
				{

						enemy.Die();
				}
			}
		}
		
	}

	void TakeDamage()
	{
		if (health <= 0) return;

		health--;

		hearts[health].SetActive(false);

		if (health <= 0)
		{
			Debug.Log("Game Over");
			anim.SetBool("isDead", true);
			anim.SetBool("isRunning", false);

		}
	}

	public bool IsDead()
	{
		return anim.GetBool("isDead");
	}

}
