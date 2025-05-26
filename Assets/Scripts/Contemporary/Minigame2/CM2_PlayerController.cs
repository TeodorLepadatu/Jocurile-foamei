using UnityEngine;

public class CM2_PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public int maxHealth = 7;
    public int currentHealth;
    public GameObject eggPrefab;
    private bool hasEgg = false;

    Rigidbody2D rb;
    Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        CM2_UIManager.Instance.UpdateHearts(currentHealth);
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    public void TakeDamage()
    {
        currentHealth--;
        CM2_UIManager.Instance.UpdateHearts(currentHealth);
        if (currentHealth <= 0)
        {
            // Handle Game Over
        }
    }

    public void Heal()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth++;
            CM2_UIManager.Instance.UpdateHearts(currentHealth);
        }
    }

    public void PickUpEgg()
    {
        if (hasEgg) return;
        hasEgg = true;

        // Throw toward mouse
        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject egg = Instantiate(eggPrefab, transform.position, Quaternion.identity);
        egg.GetComponent<Rigidbody2D>().linearVelocity = (mouseWorld - (Vector2)transform.position).normalized;

        hasEgg = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Egg"))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            CM2_ThrownEgg eggScript = other.GetComponent<CM2_ThrownEgg>();
            if (eggScript != null)
            {
                eggScript.Launch(mousePos);
            }
        }
        else if (other.CompareTag("GoldenApple"))
        {
            Heal();
            Destroy(other.gameObject);
        }
    }
}
