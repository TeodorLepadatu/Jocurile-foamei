using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float speed = 3f;

    private Rigidbody2D rb;
    private Vector2 movement;

    public int maxHealth = 5;
    public int health { get { return currentHealth; } }

    private int currentHealth = 1;

    private NonPlayerCharacter nearbyNPC; // Reference to the NPC the player is near

    public int gold = 0;

    public GameObject projectilePrefab;
    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 120;
        currentHealth = maxHealth;

        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Read movement from old InputManager (still fine for now)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        movement = new Vector2(horizontal, vertical);

        // Normalize to avoid faster diagonal movement
        if (movement.magnitude > 1)
        {
            movement = movement.normalized;
        }
        if (Input.GetKeyDown(KeyCode.Q)) // You can change this to another key
        {
            Launch();
        }

    }

    private void FixedUpdate()
    {
        // Move the player using Rigidbody2D
        rb.linearVelocity = movement * speed;
    }

    public void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHandler.instance.SetHealthValue(currentHealth / (float)maxHealth);
    }

    public void ChangeGold(int amount)
    {
        gold += amount;
        UIHandler.instance.SetGoldValue(gold);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player entered an NPC's hitbox
        NonPlayerCharacter npc = other.GetComponent<NonPlayerCharacter>();
        if (npc != null)
        {
            nearbyNPC = npc; // Store a reference to the NPC
            Debug.Log("Player entered NPC hitbox: " + npc.gameObject.name);

            // Automatically trigger the NPC's dialogue
            npc.playerNearby = true;
            nearbyNPC.DisplayDialogue();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the player exited an NPC's hitbox
        NonPlayerCharacter npc = other.GetComponent<NonPlayerCharacter>();
        if (npc != null && npc == nearbyNPC)
        {
            nearbyNPC = null; // Clear the reference to the NPC
            Debug.Log("Player exited NPC hitbox: " + npc.gameObject.name);
        }
    }

    void Launch()
    {
        Vector2 lookDirection = movement.normalized;
        if (lookDirection == Vector2.zero)
            lookDirection = Vector2.up; // Default direction if player is idle

        GameObject projectileObject = Instantiate(projectilePrefab, rb.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 10f); // Choose a force value that works well
    }

}
