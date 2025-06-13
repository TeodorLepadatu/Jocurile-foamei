using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles projectile launching, collision, and damage logic for both regular and TowerDefence scenes.
/// </summary>
public class Projectile : MonoBehaviour
{
    private Rigidbody2D rigidbody2d; // Reference to the Rigidbody2D component

    /// <summary>
    /// Initializes the Rigidbody2D reference.
    /// </summary>
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Launches the projectile in a given direction with a specified force.
    /// Ignores collisions with all BoxCollider2D objects in the TowerDefence scene.
    /// </summary>
    /// <param name="direction">Direction to launch the projectile.</param>
    /// <param name="force">Force to apply to the projectile.</param>
    public void Launch(Vector2 direction, float force)
    {
        // Only ignore BoxCollider2D in the TowerDefence scene
        if (SceneManager.GetActiveScene().name == "TowerDefence")
        {
            Collider2D myCollider = GetComponent<Collider2D>();
            BoxCollider2D[] boxColliders = FindObjectsOfType<BoxCollider2D>();
            foreach (var box in boxColliders)
            {
                if (myCollider != null && box != null)
                {
                    Physics2D.IgnoreCollision(myCollider, box);
                }
            }
        }

        rigidbody2d.AddForce(direction * force, ForceMode2D.Impulse);
    }

    /// <summary>
    /// Handles trigger collisions with enemies and applies damage.
    /// </summary>
    /// <param name="other">The collider the projectile entered.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        // Deal damage to EnemyController (used in non-TowerDefence scenes)
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null)
        {
            if (MagicZoneManager.allPlacedCorrectly)
            {
                enemy.TakeDamage(10);
            }
            else
            {
                enemy.TakeDamage(1);
            }
        }

        // Deal damage to Health component (used in TowerDefence scene)
        if (SceneManager.GetActiveScene().name == "TowerDefence")
        {
            var TDEnemy = other.gameObject.GetComponent<Health>();
            if (TDEnemy != null)
            {
                if (MagicZoneManager.allPlacedCorrectly)
                {
                    TDEnemy.TakeDamage(10);
                }
                else
                {
                    TDEnemy.TakeDamage(1);
                }
            }
        }

        Destroy(gameObject);
    }

    /// <summary>
    /// Handles physical collisions with enemies in the TowerDefence scene and applies damage.
    /// </summary>
    /// <param name="other">The collision data.</param>
    void OnCollisionEnter2D(Collision2D other)
    {
        if (SceneManager.GetActiveScene().name == "TowerDefence")
        {
            var TDEnemy = other.gameObject.GetComponent<Health>();
            if (TDEnemy != null)
            {
                if (MagicZoneManager.allPlacedCorrectly)
                {
                    TDEnemy.TakeDamage(10);
                }
                else
                {
                    TDEnemy.TakeDamage(1);
                }
            }
        }

        Destroy(gameObject);
    }
}
