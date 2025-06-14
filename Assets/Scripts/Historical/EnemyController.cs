using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls enemy behavior: following the player, taking damage, dying, and dropping gold.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
    public float speed = 2.5f;                // Movement speed of the enemy
    public float followRange = 10.0f;         // Distance at which the enemy starts following the player

    public int maxHealth = 100;               // Maximum health of the enemy
    private int currentHealth;                // Current health value

    public Slider healthBar;                  // UI slider to display health

    private Rigidbody2D rigidbody2d;          // Rigidbody2D component for movement
    private Transform playerTransform;        // Reference to the player's transform
    private bool isFollowing = false;         // Whether the enemy is currently following the player
    private float damageTimer = 0f;           // Timer to control damage intervals
    public float damageInterval = 1.0f;       // Time between each damage tick to the player
    private bool broken = false;              // If true, disables enemy behavior
    private AudioSource followAudio;          // Audio source for following sound

    public GameObject goldPrefab;             // Prefab for gold dropped on death

    void Start()
    {
        followAudio = GetComponent<AudioSource>();
        if (followAudio != null)
        {
            followAudio.loop = true;
            followAudio.volume = 0.25f; // relative SFX volume; master via AudioListener
        }

        if (PlayerController.hasKilledBoss)
        {
            Destroy(gameObject);
            return;
        }
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        rigidbody2d = GetComponent<Rigidbody2D>();

        GameObject player = GameObject.Find("Player Character");
        if (player != null)
            playerTransform = player.transform;
        else
            Debug.LogError("Player not found!");
    }

    void FixedUpdate()
    {
        if (broken || playerTransform == null) return;

        float distanceToPlayer = Vector2.Distance(playerTransform.position, transform.position);
        isFollowing = distanceToPlayer <= followRange;

        if (isFollowing)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            Vector2 newPos = rigidbody2d.position + direction * speed * Time.deltaTime;
            rigidbody2d.MovePosition(newPos);

            if (followAudio != null && !followAudio.isPlaying)
            {
                followAudio.Play();
            }
        }
        else if (followAudio != null && followAudio.isPlaying)
        {
            followAudio.Stop();
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
            Die();
    }

    void Die()
    {
        if (followAudio != null && followAudio.isPlaying)
            followAudio.Stop();

        Destroy(gameObject);

        for (int i = 0; i < 10; i++)
        {
            GameManager.instance.SpawnGold(goldPrefab, transform.position);
        }

        PlayerController.minigamesCompleted += 1;
        PlayerController.hasKilledBoss = true;
        Debug.Log("Minigames completed: " + PlayerController.minigamesCompleted);
    }
}
