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
	{   // Initialize the player controller
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

		// Handle player input for running, attacking, and jumping
		// Toggle running state with Return key
		if (Input.GetKeyDown(KeyCode.Return) && !isDead) 
		{
			isRunning = !isRunning;
			anim.SetBool("isRunning", isRunning);
		}
		// Handle player input for attacking
		// Toggle attacking state with Space key
		if (Input.GetKeyDown(KeyCode.Space) && !isDead)
			anim.SetBool("isAttacking", true);
		// Stop attacking when Space key is released
		if (Input.GetKeyUp(KeyCode.Space) && !isDead)
			anim.SetBool("isAttacking", false);
		// Handle player input for jumping
		// Check if the player is pressing W key to jump
		if (Input.GetKeyDown(KeyCode.W) && !isJumping)
			Jump();
		//incremental speed based on elapsed time
		if (IsRunning())
		{
			
			if (elapsedTime <= 20f)
				speed = 3f;
			else if (elapsedTime <= 40f)
				speed = 3.5f;
			else
				speed = 5f;

			transform.Translate(Vector3.right * Time.deltaTime * speed);
		}
	}


	public bool IsRunning()
	{   // check if the player is running and not dead
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
	{   // Check if the player collides with the ground
		if (other.gameObject.CompareTag("Ground"))
		{
			isJumping = false;
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{   // Check if the player collides with a enemy or a bush
		if (other.CompareTag("Enemy")) 
		{
			EnemyControllerFuturistic enemy = other.GetComponent<EnemyControllerFuturistic>();
			if (enemy != null)
			{
				if (!anim.GetBool("isAttacking") && !enemy.isDead)
				{
					TakeDamage(); // Take damage if not attacking and enemy is not dead
				}
				else if (anim.GetBool("isAttacking") && !enemy.isDead)
				{

						enemy.Die();
						ScoreManager.instance.AddScore(1); // Increment score by 1 when enemy is killed
				}
			}
		}

		else if (other.CompareTag("Bush"))
		{
			TakeDamage(); // Take damage when colliding with a bush
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

		hearts[health].SetActive(false); // Hide the heart icon

		if (health <= 0) // Player is dead
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

	public void AddLife() // Method to add a life to the player
	{
		if (health < hearts.Length)
		{
			hearts[health].SetActive(true);
			health++;
		}
	}



}
