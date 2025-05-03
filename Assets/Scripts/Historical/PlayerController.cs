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
    public InputAction talkAction;
    private NonPlayerCharacter nearbyNPC; 

    public int gold = 0;
    private Vector2 lastMovement;
    public GameObject projectilePrefab;
    public GameObject pickedUpObject = null;
    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 120;
        currentHealth = maxHealth;

        rb = GetComponent<Rigidbody2D>();
        talkAction.Enable();
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        movement = new Vector2(horizontal, vertical);

        if (movement.magnitude > 1)
        {
            movement = movement.normalized;
        }
        if (movement != Vector2.zero)
            lastMovement = movement;

        if (Input.GetKeyDown(KeyCode.Q)) 
        {
            Launch();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            if (nearbyNPC != null && nearbyNPC.playerNearby)
            {
                UIHandler.instance.DisplayDialogue();
                nearbyNPC.DisplayDialogue();
            }
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            if ((pickedUpObject))
            {
                
            }
            else
            {

            }
        }
    }
        
    private void FixedUpdate()
    {
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
        NonPlayerCharacter npc = other.GetComponent<NonPlayerCharacter>();
        if (npc != null)
        {
            nearbyNPC = npc; 
            Debug.Log("Player entered NPC hitbox: " + npc.gameObject.name);


            npc.playerNearby = true;
            nearbyNPC.DisplayDialogue();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        NonPlayerCharacter npc = other.GetComponent<NonPlayerCharacter>();
        if (npc != null && npc == nearbyNPC)
        {
            nearbyNPC = null; 
        }
    }

    void Launch()
    {
        Vector2 lookDirection = movement.normalized;
        if (lookDirection == Vector2.zero)
            lookDirection = Vector2.up;

        GameObject projectileObject = Instantiate(projectilePrefab, rb.position + lookDirection * 0.5f, Quaternion.identity);

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
