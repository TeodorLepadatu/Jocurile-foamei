using UnityEngine;

/// <summary>
/// Controls the behavior of a bullet projectile, including movement, targeting, and collision handling.
/// </summary>
public class Bullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb; // Reference to the Rigidbody2D component for physics-based movement

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 20f; // Speed at which the bullet travels
    [SerializeField] private int bulletDamage = 1;    // Amount of damage this bullet deals on hit

    private Transform target; // The target this bullet is homing towards

    /// <summary>
    /// Checks if the target still exists every frame.
    /// If the target is destroyed or missing, the bullet destroys itself.
    /// </summary>
    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
    }

    /// <summary>
    /// Sets the target for this bullet to home in on.
    /// </summary>
    /// <param name="_target">The Transform of the target.</param>
    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    /// <summary>
    /// Handles the bullet's movement towards its target using physics.
    /// </summary>
    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * bulletSpeed;
    }

    /// <summary>
    /// Handles collision with other objects.
    /// Deals damage to objects with a Health component and destroys the bullet.
    /// </summary>
    /// <param name="other">Collision information.</param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        other.gameObject.GetComponent<Health>().TakeDamage(bulletDamage);
        Destroy(gameObject);
    }
}
