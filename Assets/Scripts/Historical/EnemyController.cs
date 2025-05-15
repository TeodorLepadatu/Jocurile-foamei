using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
    public float speed = 2.5f;
    public float followRange = 10.0f;
    

    public int maxHealth = 100;
    private int currentHealth;

    public Slider healthBar; 

    private Rigidbody2D rigidbody2d;
    private Transform playerTransform;
    private bool isFollowing = false;
    private float damageTimer = 0f;
    public float damageInterval = 1.0f;
    private bool broken = false;

    public GameObject goldPrefab;
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
        //PlayerController.gold += 10;
        for (int i = 0; i < 10; i++)
        {
            GameManager.instance.SpawnGold(goldPrefab);
        }
        PlayerController.minigamesCompleted += 1;
        Debug.Log("a facut: " + PlayerController.minigamesCompleted);
    }
}
