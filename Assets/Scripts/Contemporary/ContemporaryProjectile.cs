using UnityEngine;

public class ContemporaryProjectile : MonoBehaviour
{
    public int damage = 10;

    void OnTriggerEnter2D(Collider2D other)
    {
        // If the projectile hits something with a Health script
        PlayerHealth target = other.GetComponent<PlayerHealth>();
        if (target != null)
        {
            target.TakeDamage(damage);
        }
    }
}
