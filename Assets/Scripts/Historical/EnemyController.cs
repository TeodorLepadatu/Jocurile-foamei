using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
    public float speed = 2.5f;
    public float followRange = 10.0f;
    public float damageInterval = 1.0f;

    public int maxHealth = 100;
    private int currentHealth;

    public Slider healthBar; 

    private Rigidbody2D rigidbody2d;
    private Transform playerTransform;
    private bool isFollowing = false;
    private float damageTimer = 0f;

    private bool broken = false;
    private static bool enemyExists = false; // Static flag to track if the enemy already exists

    void Awake()
    {
        if (enemyExists)
        {
            Destroy(gameObject); // Destroy duplicate enemy
            return;
        }

        enemyExists = true; // Mark that the enemy exists
        DontDestroyOnLoad(gameObject); // Persist this enemy across scenes
    }

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        rigidbody2d = GetComponent<Rigidbody2D>();

        GameObject player = GameObject.Find("Player Character");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player not found!");
        }
    }

    void FixedUpdate()
    {
        if (broken || playerTransform == null) return;

        float distanceToPlayer = Vector2.Distance(playerTransform.position, transform.position);
        isFollowing = distanceToPlayer <= followRange;

        if (isFollowing)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            Vector2 position = rigidbody2d.position + direction * speed * Time.deltaTime;
            rigidbody2d.MovePosition(position);
        }

        damageTimer += Time.deltaTime;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();
        if (controller != null && damageTimer >= damageInterval)
        {
            controller.ChangeHealth(-1);
            damageTimer = 0f;
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (healthBar != null)
            healthBar.value = currentHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
        PlayerController.gold += 10;
        PlayerController.minigamesCompleted += 1;
        Debug.Log("a facut: " + PlayerController.minigamesCompleted);
    }
}
