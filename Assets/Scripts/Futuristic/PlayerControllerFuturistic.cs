using UnityEngine;
using System.Collections;

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

	private float elapsedTime = 0f;


	private bool isDead;
	private bool isAttacking;

	public GameObject gameOverScreen;
	void Start()
	{
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		health = hearts.Length;
		if (gameOverScreen != null)
			gameOverScreen.SetActive(false);
		anim.SetBool("isRunning", false);
		anim.SetBool("isDead", false);
		anim.SetBool("isAttacking", false);
		speed = 0;
		health = 5;


	}

	void Update()
	{
		elapsedTime += Time.deltaTime;

		isRunning = anim.GetBool("isRunning");
		isDead = anim.GetBool("isDead");

		if (Input.GetKeyDown(KeyCode.Return) && !isDead)
		{
			isRunning = !isRunning;
			anim.SetBool("isRunning", isRunning);
		}

		if (Input.GetKeyDown(KeyCode.Space) && !isDead)
			anim.SetBool("isAttacking", true);
		if (Input.GetKeyUp(KeyCode.Space) && !isDead)
			anim.SetBool("isAttacking", false);

		if (Input.GetKeyDown(KeyCode.W) && !isJumping)
			Jump();

		if (IsRunning())
		{
			
			if (elapsedTime <= 20f)
				speed = 2f;
			else if (elapsedTime <= 40f)
				speed = 3.5f;
			else
				speed = 5f;

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
						ScoreManager.instance.AddScore(1);
				}
			}
		}

		else if (other.CompareTag("Bush"))
		{
			TakeDamage();
		}


	}

	IEnumerator ShowGameOverAfterDelay()
	{
		yield return new WaitForSeconds(1f);

		if (gameOverScreen != null)
		{
			gameOverScreen.SetActive(true);
			Time.timeScale = 0f;
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

			StartCoroutine(ShowGameOverAfterDelay());
		}
	}

	public bool IsDead()
	{
		return anim.GetBool("isDead");
	}

	public void AddLife()
	{
		if (health < hearts.Length)
		{
			hearts[health].SetActive(true);
			health++;
		}
	}



}
