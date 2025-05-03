using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 direction, float force)
    {
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

        Destroy(gameObject); 
    }
}
