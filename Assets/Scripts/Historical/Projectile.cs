using UnityEngine;
using UnityEngine.SceneManagement;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

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

    void OnTriggerEnter2D(Collider2D other)
    {
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null)
        {
            if(MagicZoneManager.allPlacedCorrectly)
            {
                enemy.TakeDamage(10);
            }
            else
            {
                enemy.TakeDamage(1);
            }
        }
        if(SceneManager.GetActiveScene().name == "TowerDefence")
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
