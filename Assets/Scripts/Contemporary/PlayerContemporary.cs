using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PlayerContemporary : MonoBehaviour
{
    public static PlayerContemporary Instance { get; private set; }
    private GameObject heldObject = null;
    private GameObject nearbyObject = null;
    public GameObject serverHealthBar;
    public GameObject step3;
    public GameObject step2;
    private string portName = null;
    private float playerScale = 0.11f;
    public GameObject pccgeSprite;
    [SerializeField] private GameObject[] objectsToActivate;
    [SerializeField] private GameObject[] objectToDeactivate;
    [SerializeField] public GameObject[] hearts;
    public static int currentStep = 1;
    private bool changePosition = true;
    private int numberOfHearts = 5;
    private int maxHearts = 5;
    private float damageCooldown = 0f;
    private bool isInCooldown = false;
    public GameObject draganPrefab;
    public GameObject minigame1;
    public GameObject gameOverScreen;
    public Text coinText;
    public MusicManager musicManager;

    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;

    protected Vector2 movement;

    void Start() {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        currentStep = 1;
        SwitchController.turnedSwitches = 0;
        CoalGenerator.canGenerate = true;
        coinText.text = CurrencyHolder.getCurrency().ToString();
    }


    protected void Update()
    {
        HandleMovement();
        HandlePickup();

        if(changePosition && SwitchController.turnedSwitches == 2) {
            step2.SetActive(false);
            step3.SetActive(true);
            currentStep = 3;
            changePosition = false;
            transform.position = new Vector3(-2.83f, 0.71f, 0);
        }

        if (isInCooldown)
        {
            damageCooldown += Time.deltaTime;
            if (damageCooldown >= 0.5f)
            {
                isInCooldown = false;
                damageCooldown = 0f;
            }
        }
    }

    protected void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    protected void HandleMovement()
    {
        movement = Vector2.zero;

        if (Controls.GetKey(Controls.Action.MoveUp))
            movement.y += 1;
        if (Controls.GetKey(Controls.Action.MoveDown))
            movement.y -= 1;
        if (Controls.GetKey(Controls.Action.MoveRight))
            movement.x += 1;
        if (Controls.GetKey(Controls.Action.MoveLeft))
            movement.x -= 1;

        animator.SetFloat("moveX", movement.x);
        animator.SetFloat("moveY", movement.y);
        animator.SetFloat("speed", movement.sqrMagnitude);

        if (movement.x > 0)
            transform.localScale = new Vector3(playerScale, playerScale, 1);
        else if (movement.x < 0)
            transform.localScale = new Vector3(-playerScale, playerScale, 1);
    }

    private void HandlePickup()
    {
        if (Controls.GetKey(Controls.Action.Pickup) && nearbyObject != null && heldObject == null)
        {
            heldObject = nearbyObject;
            heldObject.GetComponent<Collider2D>().enabled = false;
            portName = heldObject.name;
        }

        if (Controls.GetKey(Controls.Action.Drop) && heldObject != null)
        {
            if(currentStep == 1) {
                GameObject dropZone = GameObject.Find("PCNOCGESprite");
                if (dropZone != null && Vector2.Distance(heldObject.transform.position, dropZone.transform.position) < 3f)
                {
                    Destroy(heldObject);

                    if (pccgeSprite != null) 
                        pccgeSprite.SetActive(true);

                    dropZone.SetActive(false);
                    heldObject = null;

                    StartCoroutine(DelayedActivate());

                    return;
                }
            }
            else if(currentStep == 3) {
                GameObject dropZone = GameObject.Find("Furnace");
                if (dropZone != null && Vector2.Distance(heldObject.transform.position, dropZone.transform.position) < 3f)
                {
                    Destroy(heldObject);
                    CoalGenerator.canGenerate = true;
                    heldObject = null;

                    HealthBarController health = serverHealthBar.GetComponent<HealthBarController>();
                    health.TakeDamage(Random.Range(25, 36));

                    CurrencyHolder.addCurrency(3);
                    coinText.text = CurrencyHolder.getCurrency().ToString();


                    return;
                }
            }

            heldObject.GetComponent<Collider2D>().enabled = true;
            heldObject = null;
        }

        if (heldObject != null)
        {
            heldObject.transform.position = transform.position + new Vector3(0.5f, 0.5f, 0);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        damageCooldown += Time.deltaTime;

        if (other.CompareTag("Pickable"))
        {
            nearbyObject = other.gameObject;
        }
        
        if (!isInCooldown && numberOfHearts > 0 && other.CompareTag("Projectile"))
        {
            numberOfHearts--;
            for (int i = 0; i < maxHearts; i++) {
                hearts[i].SetActive(i < numberOfHearts);
            }
            if(numberOfHearts == 0) {
                StartCoroutine(GameOver());
            }

            isInCooldown = true;
        }

        if(other.CompareTag("RegenerateHealth")) {
            numberOfHearts++;
            for(int i = 0; i < maxHearts; i++) {
                hearts[i].SetActive(i < numberOfHearts);
            }

            StartCoroutine(RespawnApple(other.gameObject, 10f));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Pickable") && other.gameObject == nearbyObject)
        {
            nearbyObject = null;
        }
    }

    private IEnumerator RespawnApple(GameObject apple, float delay)
    {
        apple.SetActive(false);
        yield return new WaitForSeconds(delay);
        apple.SetActive(true);
    }

    private IEnumerator GameOver()
    {
        minigame1.SetActive(false);
        gameOverScreen.SetActive(true);

        transform.position = new Vector3(0.31f, -1.95f, 0f);
        transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeAll;


        yield return new WaitForSeconds(5f);

        SceneManager.LoadScene("CMinigame2");
    }

    private IEnumerator DelayedActivate()
    {
        yield return new WaitForSeconds(3f);

        if (portName == "CFESprite")
        {
            GameObject dragan = Instantiate(draganPrefab, new Vector3(4.65f, 4f, 0), Quaternion.identity);

            dragan.GetComponent<Rigidbody2D>().gravityScale = 0;
            dragan.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, -5);
        
            yield return new WaitForSeconds(3f);
            StartCoroutine(GameOver());
            yield break;
        }

        CurrencyHolder.addCurrency(8);
        coinText.text = CurrencyHolder.getCurrency().ToString();
        
        foreach (GameObject obj in objectsToActivate)
        {
            if (obj != null)
                obj.SetActive(true);
        }

        foreach (GameObject obj in objectToDeactivate)
        {
            if (obj != null)
                obj.SetActive(false);
        }
    }

}
