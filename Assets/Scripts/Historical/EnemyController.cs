using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls enemy behavior: following the player, taking damage, dying, and dropping gold.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
    // Movement and detection
    public float speed = 2.5f;                // Movement speed of the enemy
    public float followRange = 10.0f;         // Distance at which the enemy starts following the player

    // Health
    public int maxHealth = 100;               // Maximum health of the enemy
    private int currentHealth;                // Current health value

    public Slider healthBar;                  // UI slider to display health

    // Internal references
    private Rigidbody2D rigidbody2d;          // Rigidbody2D component for movement
    private Transform playerTransform;        // Reference to the player's transform
    private bool isFollowing = false;         // Whether the enemy is currently following the player
    private float damageTimer = 0f;           // Timer to control damage intervals
    public float damageInterval = 1.0f;       // Time between each damage tick to the player
    private bool broken = false;              // If true, disables enemy behavior (not used in this script)
    private AudioSource followAudio;          // Audio source for following sound

    public GameObject goldPrefab;             // Prefab for gold dropped on death

    /// <summary>
    /// Initializes enemy state, health, and finds the player.
    /// </summary>
    void Start()
    {
        followAudio = GetComponent<AudioSource>();
        if (followAudio != null)
        {
            followAudio.loop = true;
            followAudio.volume = 0.25f; // Light volume
        }

        // Destroy this enemy if the boss has already been killed
        if (PlayerController.hasKilledBoss)
        {
            Destroy(gameObject);
            return;
        }
        currentHealth = maxHealth;

        // Initialize health bar UI
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        rigidbody2d = GetComponent<Rigidbody2D>();

        // Find the player by name
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

    /// <summary>
    /// Handles following the player and playing/stopping audio.
    /// </summary>
    void FixedUpdate()
    {
        if (broken || playerTransform == null) return;

        float distanceToPlayer = Vector2.Distance(playerTransform.position, transform.position);
        isFollowing = distanceToPlayer <= followRange;

        if (isFollowing)
        {
            // Move towards the player
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            Vector2 position = rigidbody2d.position + direction * speed * Time.deltaTime;
            rigidbody2d.MovePosition(position);

            // Play follow audio if not already playing
            if (followAudio != null && !followAudio.isPlaying)
            {
                followAudio.Play();
            }
        }
        else
        {
            // Stop audio if not following
            if (followAudio != null && followAudio.isPlaying)
            {
                followAudio.Stop();
            }
        }

        damageTimer += Time.deltaTime;
    }

    /// <summary>
    /// Damages the player if they stay in the enemy's trigger area, at set intervals.
    /// </summary>
    void OnTriggerStay2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();
        if (controller != null && damageTimer >= damageInterval)
        {
            controller.ChangeHealth(-1);
            damageTimer = 0f;
        }
    }

    /// <summary>
    /// Reduces enemy health and updates the health bar. Kills the enemy if health reaches zero.
    /// </summary>
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

    /// <summary>
    /// Handles enemy death: stops audio, destroys the object, spawns gold, and updates player progress.
    /// </summary>
    void Die()
    {
        if (followAudio != null && followAudio.isPlaying)
        {
            followAudio.Stop();
        }

        Destroy(gameObject);

        // Spawn 10 gold at the enemy's position
        for (int i = 0; i < 10; i++)
        {
            GameManager.instance.SpawnGold(goldPrefab, transform.position);
        }

        // Update player progress
        PlayerController.minigamesCompleted += 1;
        Debug.Log("a facut: " + PlayerController.minigamesCompleted);
        PlayerController.hasKilledBoss = true;
    }
}
