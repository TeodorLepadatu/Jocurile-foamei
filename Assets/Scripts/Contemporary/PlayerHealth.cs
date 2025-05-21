using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage. Remaining: {currentHealth}");
        
        Debug.Log(currentHealth);
        if (currentHealth <= 0)
        {
            
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} died.");
        // Add death logic here (destroy, disable, animation, etc.)
        Destroy(gameObject); // Optional
    }
}
