using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;

    private Transform target;
    private int pathIndex = 0;

    private float baseSpeed;

    private void Start()
    {
        baseSpeed = moveSpeed;
        target = LevelManager.main.path[pathIndex];
    }
    private void Update()
    {
        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;
            if (pathIndex >= LevelManager.main.path.Length)
            {
                // Enemy slips through
                LevelManager.main.DamagePlayer(1); // Deal 1 damage to the player
                EnemySpawner.onEnemyDestroy.Invoke(); // Notify the spawner
                Destroy(gameObject); // Destroy the enemy
                return;
            }
            target = LevelManager.main.path[pathIndex];
        }
    }

    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }
    
    public void UpdateSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }

    public void ResetSpeed()
    {
        moveSpeed = baseSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController controller = collision.gameObject.GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.ChangeHealth(-1);
        }
    }


}
