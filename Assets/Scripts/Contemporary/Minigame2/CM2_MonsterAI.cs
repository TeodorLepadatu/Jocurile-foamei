using UnityEngine;

public class CM2_MonsterAI : MonoBehaviour
{
    public int health = 3;
    public float speed = 2f;
    private Transform player;
    private bool isDead = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    void Update()
    {
        if (player != null)
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime * 0.3f);
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
        Debug.Log("Collided with: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Player"))
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
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Trigger hit with player");
            CM2_PlayerController player = collision.GetComponent<CM2_PlayerController>();
            if (player != null)
            {
                player.TakeDamage();
            }
        }

        if (collision.CompareTag("EggProjectile"))
        {
            TakeDamage();
            Destroy(collision.gameObject);
        }
    }
}
