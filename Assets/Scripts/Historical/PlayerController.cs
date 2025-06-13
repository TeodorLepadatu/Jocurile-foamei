using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

/// <summary>
/// Controls player movement, health, interactions, shooting, and inventory in the historical game context.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float speed = 3f; // Player movement speed

    private Rigidbody2D rb; // Reference to Rigidbody2D for movement
    private Vector2 movement; // Current movement vector

    public int maxHealth = 10; // Maximum health
    public int health { get { return currentHealth; } } // Public getter for current health

    public int currentHealth = 1; // Current health value
    public InputAction talkAction; // Input action for talking to NPCs
    private NonPlayerCharacter nearbyNPC; // Reference to the NPC the player is near

    public static bool resetGold = false; // Static flag to reset gold on scene load
    private Vector2 lastMovement; // Last non-zero movement direction
    public GameObject projectilePrefab; // Prefab for the player's projectile
    public GameObject pickedUpObject = null; // Currently picked up object
    public GameObject upgradedProjectilePrefab; // Prefab for upgraded projectile
    public float pickupRadius = 1f; // Radius for picking up objects
    public LayerMask pickableLayer; // Layer mask for pickable objects

    public static int minigamesCompleted = 0; // Tracks completed minigames
    private float launchCooldown = 0.5f; // Cooldown between shots
    private float launchTimer = 0f; // Timer for launch cooldown
    public static PlayerController instance; // Singleton instance
    public static bool hasKilledBoss = false; // Tracks if the boss has been killed
    private AudioSource audioSource; // Reference to the player's audio source

    /// <summary>
    /// Ensures singleton pattern and sets initial position if in MainScene1.
    /// </summary>
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        //Debug.Log("mg: " + minigamesCompleted);
        DontDestroyOnLoad(gameObject);
        if (SceneManager.GetActiveScene().name == "MainScene1")
        {
            transform.position = new Vector3(10.22f, -3.51f, 0f);
        }
    }

    /// <summary>
    /// Subscribes to scene loaded event.
    /// </summary>
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
    /// Unsubscribes from scene loaded event.
    /// </summary>
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Sets player position when MainScene1 is loaded.
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainScene1")
        {
            transform.position = new Vector3(10.22f, -3.51f, 0f);
        }
    }

    /// <summary>
    /// Initializes audio, health, and disables collisions with towers in TowerDefence scene.
    /// </summary>
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.loop = true;
            audioSource.Play();
        }
        if (resetGold)
        {
            CurrencyHolder.reset();
        }
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        talkAction.Enable();
        resetGold = true;

        // Ignore collisions with towers in TowerDefence scene
        if (SceneManager.GetActiveScene().name == "TowerDefence")
        {
            Collider2D playerCollider = GetComponent<Collider2D>();
            BoxCollider2D[] boxColliders = FindObjectsOfType<BoxCollider2D>();

            foreach (BoxCollider2D box in boxColliders)
            {
                if (box != null && playerCollider != null)
                {
                    Physics2D.IgnoreCollision(playerCollider, box);
                }
            }
        }
    }

    /// <summary>
    /// Handles input, movement, shooting, interaction, pickup/drop, and win condition.
    /// </summary>
    private void Update()
    {
        float horizontal = 0f;
        float vertical = 0f;

        // Handle movement input
        if (Controls.GetKey(Controls.Action.MoveRight)) horizontal += 1f;
        if (Controls.GetKey(Controls.Action.MoveLeft)) horizontal -= 1f;
        if (Controls.GetKey(Controls.Action.MoveUp)) vertical += 1f;
        if (Controls.GetKey(Controls.Action.MoveDown)) vertical -= 1f;

        // Handle shooting cooldown
        if (launchTimer > 0f)
            launchTimer -= Time.deltaTime;

        movement = new Vector2(horizontal, vertical);

        // Normalize movement if diagonal
        if (movement.magnitude > 1)
            movement = movement.normalized;

        // Store last movement direction for dropping objects
        if (movement != Vector2.zero)
            lastMovement = movement;

        // Handle shooting
        if (Controls.GetKey(Controls.Action.Shoot) && launchTimer <= 0f)
        {
            Launch();
            launchTimer = launchCooldown;
        }

        // Handle NPC interaction
        if (Controls.GetKey(Controls.Action.InteractWithNPC))
        {
            if (nearbyNPC != null && nearbyNPC.playerNearby)
            {
                UIHandler.instance.DisplayDialogue();
                nearbyNPC.DisplayDialogue();
            }
        }

        // Handle pickup/drop logic
        if (Controls.GetKey(Controls.Action.Pickup))
        {
            if (pickedUpObject != null)
            {
                // Drop the object in front of player
                PickableObject po = pickedUpObject.GetComponent<PickableObject>();
                if (po != null)
                {
                    Vector2 dropPos = rb.position + lastMovement.normalized * 0.5f;
                    po.Drop(dropPos);
                }
                pickedUpObject = null;
            }
            else
            {
                // Try to pick up a nearby object
                Collider2D[] hits = Physics2D.OverlapCircleAll(rb.position, pickupRadius, pickableLayer);
                foreach (Collider2D hit in hits)
                {
                    if (hit.CompareTag("Pickable"))
                    {
                        PickableObject po = hit.GetComponent<PickableObject>();
                        if (po != null)
                        {
                            po.PickUp(transform);
                            pickedUpObject = po.gameObject;
                            GameObject dartMonkey = GameObject.Find("dart_monkey");

                            if (dartMonkey != null)
                                Destroy(dartMonkey);
                            break;
                        }
                    }
                }
            }
        }

        // Check for win condition (minigames completed and in area)
        if (minigamesCompleted >= 2 && IsInArea(transform.position, new Vector2(14f, 13f), new Vector2(19f, 10f)))
        {
            Debug.Log("You have completed the game!");
            Destroy(gameObject);
            SceneManager.LoadScene("VictoryScreenScene");
        }

        // Update UI
        UIHandler.instance.SetGoldValue(CurrencyHolder.getCurrency());
        UIHandler.instance.SetHealthValue(currentHealth / (float)maxHealth);
    }

    /// <summary>
    /// Checks if the player is within a rectangular area.
    /// </summary>
    private bool IsInArea(Vector2 pos, Vector2 min, Vector2 max)
    {
        return pos.x >= min.x && pos.x <= max.x && pos.y <= min.y && pos.y >= max.y;
    }

    /// <summary>
    /// Handles physics-based movement.
    /// </summary>
    private void FixedUpdate()
    {
        rb.linearVelocity = movement * speed;
    }

    /// <summary>
    /// Changes the player's health and updates the UI. Triggers game over if health reaches zero.
    /// </summary>
    public void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHandler.instance.SetHealthValue(currentHealth / (float)maxHealth);
        if (currentHealth <= 0)
        {
            if (audioSource != null)
                audioSource.Stop();

            Debug.Log("Player is dead!");
            GameManager.GameOver();
        }
    }

    /// <summary>
    /// Changes the player's gold and updates the UI.
    /// </summary>
    public void ChangeGold(int amount)
    {
        CurrencyHolder.addCurrency(amount);
        UIHandler.instance.SetGoldValue(CurrencyHolder.getCurrency());
    }

    /// <summary>
    /// Detects when the player enters an NPC's trigger area and sets up dialogue.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        NonPlayerCharacter npc = other.GetComponent<NonPlayerCharacter>();
        if (npc != null)
        {
            nearbyNPC = npc;
            Debug.Log("Player entered NPC hitbox: " + npc.gameObject.name);

            npc.playerNearby = true;
            nearbyNPC.DisplayDialogue();
        }
    }

    /// <summary>
    /// Detects when the player exits an NPC's trigger area.
    /// </summary>
    private void OnTriggerExit2D(Collider2D other)
    {
        NonPlayerCharacter npc = other.GetComponent<NonPlayerCharacter>();
        if (npc != null && npc == nearbyNPC)
        {
            nearbyNPC = null;
        }
    }

    /// <summary>
    /// Instantiates and launches a projectile in the direction of movement.
    /// Uses upgraded projectile if all magic zones are placed correctly.
    /// </summary>
    void Launch()
    {
        Vector2 lookDirection = movement.normalized;
        if (lookDirection == Vector2.zero)
            lookDirection = Vector2.up;
        if (MagicZoneManager.allPlacedCorrectly)
        {
            projectilePrefab = upgradedProjectilePrefab;
        }
        GameObject projectileObject = Instantiate(projectilePrefab, rb.position + new Vector2(0, 1) + lookDirection * 0.5f, Quaternion.identity);

        Collider2D playerCollider = GetComponent<Collider2D>();
        Collider2D projectileCollider = projectileObject.GetComponent<Collider2D>();
        if (playerCollider != null && projectileCollider != null)
        {
            Physics2D.IgnoreCollision(playerCollider, projectileCollider);
        }

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 10f);
    }
}
