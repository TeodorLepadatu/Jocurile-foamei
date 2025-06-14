using UnityEngine;

public class CM2_MonsterAI : MonoBehaviour
{
    public int health = 3;
    public float speed = 5f;
    private Transform player;
    private bool isDead = false;
    private float lastDamageTime = -999f; // initialized way in the past
    private float damageCooldown = 0f;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    void Update()
    {
        if (player != null)
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime * 0.5f);
    }

    public void TakeDamage()
    {
        if (isDead) return;

        health--;
        if (health <= 0)
        {
            isDead = true;
            
            CM2.Instance.MonsterDefeated();
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // if the player or the monster collides then decrease player's health
        {
            CM2_PlayerController player = collision.gameObject.GetComponent<CM2_PlayerController>();
            if (player != null)
            {
                player.TakeDamage();
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EggProjectile")) // if the thrown object collides with the monster then decrease monster's health
        {
            TakeDamage();
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // if player collides with the monster
        {
            if (Time.time - lastDamageTime >= damageCooldown) // health decrease should happen after a few seconds from the previous taken damage
            {
                CM2_PlayerController player = collision.GetComponent<CM2_PlayerController>();
                if (player != null)
                {
                    Debug.Log("take player damage");
                    player.TakeDamage();
                    lastDamageTime = Time.time;
                    damageCooldown = 2f;
                }
            }
        }
    }

}
