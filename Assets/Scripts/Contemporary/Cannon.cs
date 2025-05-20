using UnityEngine;

public class CannonShooter : MonoBehaviour
{
    public GameObject projectilePrefab;   // Cannonball prefab
    public Transform firePoint;           // Firing origin
    public float shootForce = 10f;
    public float fireInterval = 1f;       // Time between bursts
    private float timer = 0f, delayTimer;
    public float delay = 3f;

    void Update()
    {
        timer += Time.deltaTime;
        delayTimer += Time.deltaTime;

        if(delayTimer < delay) {
            return;
        }

        if (timer >= fireInterval)
        {
            FireProjectiles();
            timer = 0f;
        }
    }

    void FireProjectiles()
    {
        // Three slight vertical offsets to spread shots
        float[] xOffsets = { -0.5f, 0f, 0.5f };

        foreach (float offset in xOffsets)
        {
            Vector3 spawnPos = firePoint.position + new Vector3(offset, 0, 0);
            GameObject projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            rb.linearVelocity = Vector2.right * shootForce;
        }
    }
}
