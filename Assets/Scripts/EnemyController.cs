using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
    // Public variables
    public float speed = 2.5f; // Speed of the enemy
    public float followRange = 10.0f; // Distance within which the enemy starts following the player
    public float damageInterval = 1.0f; // Time in seconds between damage ticks

    // Private variables
    private Rigidbody2D rigidbody2d;
    private Transform playerTransform; // Reference to the player's transform
    private bool isFollowing = false; // Whether the enemy is currently following the player
    private float damageTimer = 0f; // Timer to track damage intervals

    bool broken = true;
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();

        // Find the player in the scene by name
        GameObject player = GameObject.Find("Player Character");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure the player GameObject is named 'Player Character'.");
        }
    }

    // FixedUpdate has the same call rate as the physics system
    void FixedUpdate()
    {
        if (!broken)
        {
            return;
        }
        if (playerTransform != null)
        {
            // Calculate the distance to the player
            float distanceToPlayer = Vector2.Distance(playerTransform.position, transform.position);

            // Check if the player is within the follow range
            if (distanceToPlayer <= followRange)
            {
                isFollowing = true;
            }
            else
            {
                isFollowing = false;
            }

            // If following, move toward the player
            if (isFollowing)
            {
                Vector2 direction = (playerTransform.position - transform.position).normalized;
                Vector2 position = rigidbody2d.position + direction * speed * Time.deltaTime;
                rigidbody2d.MovePosition(position);
            }
        }

        // Update the damage timer
        damageTimer += Time.deltaTime;
    }

    // Trigger-based collision detection
    void OnTriggerStay2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy.Fix();
        }
        if (controller != null && damageTimer >= damageInterval)
        {
            controller.ChangeHealth(-1); // Apply damage to the player
            damageTimer = 0f; // Reset the timer
        }

    }
    public void Fix()
    {
        broken = false;
        rigidbody2d.simulated = false;
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
  }
}

