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

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        if (SceneManager.GetActiveScene().name == "MainScene1")
            transform.position = new Vector3(10.22f, -3.51f, 0f);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainScene1")
            transform.position = new Vector3(10.22f, -3.51f, 0f);
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.loop = true;
            audioSource.volume = 1f; // full source volume; master volume via AudioListener
            audioSource.Play();
        }

        if (resetGold)
            CurrencyHolder.reset();

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
            foreach (BoxCollider2D box in FindObjectsOfType<BoxCollider2D>())
            {
                if (box != null && playerCollider != null)
                    Physics2D.IgnoreCollision(playerCollider, box);
            }
        }
    }

    private void Update()
    {
        // --- Movement Input ---
        float horizontal = 0f;
        float vertical = 0f;
        if (Controls.GetKey(Controls.Action.MoveRight)) horizontal += 1f;
        if (Controls.GetKey(Controls.Action.MoveLeft)) horizontal -= 1f;
        if (Controls.GetKey(Controls.Action.MoveUp)) vertical += 1f;
        if (Controls.GetKey(Controls.Action.MoveDown)) vertical -= 1f;

        movement = new Vector2(horizontal, vertical).normalized;
        if (movement != Vector2.zero)
            lastMovement = movement;

        // --- Shooting ---
        if (launchTimer > 0f)
            launchTimer -= Time.deltaTime;

        if (Controls.GetKey(Controls.Action.Shoot) && launchTimer <= 0f)
        {
            Launch();
            launchTimer = launchCooldown;
        }

        // --- NPC Interaction ---
        if (Controls.GetKey(Controls.Action.InteractWithNPC) && nearbyNPC != null && nearbyNPC.playerNearby)
        {
            UIHandler.instance.DisplayDialogue();
            nearbyNPC.DisplayDialogue();
        }

        // --- Pickup & Hold / Drop Logic ---
        if (Controls.GetKeyDown(Controls.Action.Pickup))
        {
            if (pickedUpObject == null)
            {
                Collider2D[] hits = Physics2D.OverlapCircleAll(rb.position, pickupRadius, pickableLayer);
                foreach (var hit in hits)
                {
                    if (hit.CompareTag("Pickable"))
                    {
                        var po = hit.GetComponent<PickableObject>();
                        if (po != null)
                        {
                            po.PickUp(transform);
                            pickedUpObject = po.gameObject;
                            var dartMonkey = GameObject.Find("dart_monkey");
                            if (dartMonkey != null)
                                Destroy(dartMonkey);
                            break;
                        }
                    }
                }
            }
        }

        if (Controls.GetKeyUp(Controls.Action.Pickup))
        {
            if (pickedUpObject != null)
            {
                var po = pickedUpObject.GetComponent<PickableObject>();
                if (po != null)
                {
                    Vector2 dropPos = rb.position + lastMovement * 0.5f;
                    po.Drop(dropPos);
                }
                pickedUpObject = null;
            }
        }

        // --- Win Condition ---
        if (minigamesCompleted >= 2 &&
            IsInArea(transform.position, new Vector2(14f, 13f), new Vector2(19f, 10f)))
        {
            Debug.Log("You have completed the game!");
            Destroy(gameObject);
            SceneManager.LoadScene("VictoryScreenScene");
        }

        // --- UI Updates ---
        UIHandler.instance.SetGoldValue(CurrencyHolder.getCurrency());
        UIHandler.instance.SetHealthValue(currentHealth / (float)maxHealth);
    }

    private void FixedUpdate()
    {
        float moveSpeed = speed * (SceneManager.GetActiveScene().name == "TowerDefence" ? 2f : 1f);
        rb.linearVelocity = movement * moveSpeed;
    }

    public void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHandler.instance.SetHealthValue(currentHealth / (float)maxHealth);
        if (currentHealth <= 0)
        {
            audioSource?.Stop();
            Debug.Log("Player is dead!");
            GameManager.GameOver();
        }
    }

    public void ChangeGold(int amount)
    {
        CurrencyHolder.addCurrency(amount);
        UIHandler.instance.SetGoldValue(CurrencyHolder.getCurrency());
    }

    private bool IsInArea(Vector2 pos, Vector2 min, Vector2 max)
    {
        return pos.x >= min.x && pos.x <= max.x && pos.y <= min.y && pos.y >= max.y;
    }

    private void Launch()
    {
        Vector2 direction = movement == Vector2.zero ? Vector2.up : movement;
        if (MagicZoneManager.allPlacedCorrectly)
            projectilePrefab = upgradedProjectilePrefab;

        var proj = Instantiate(
            projectilePrefab,
            rb.position + Vector2.up + direction * 0.5f,
            Quaternion.identity
        );

        if (TryGetComponent<Collider2D>(out var playerCol) &&
            proj.TryGetComponent<Collider2D>(out var projCol))
        {
            Physics2D.IgnoreCollision(playerCol, projCol);
        }

        proj.GetComponent<Projectile>()?.Launch(direction, 10f);
    }
}
