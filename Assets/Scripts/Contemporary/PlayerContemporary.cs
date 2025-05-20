using UnityEngine;
using System.Collections;

public class PlayerContemporary : MonoBehaviour
{
    private GameObject heldObject = null;
    private GameObject nearbyObject = null;
    private string portName = null;
    private float playerScale = 0.11f;
    public GameObject pccgeSprite;
    [SerializeField] private GameObject[] objectsToActivate;
    [SerializeField] private GameObject[] objectToDeactivate;

    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;

    protected Vector2 movement;

    protected virtual void Update()
    {
        HandleMovement();
        HandlePickup();
    }

    protected virtual void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    protected void HandleMovement()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

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
        if (Input.GetKeyDown(KeyCode.E) && nearbyObject != null && heldObject == null)
        {
            heldObject = nearbyObject;
            heldObject.GetComponent<Collider2D>().enabled = false;
            portName = heldObject.name;

            Debug.Log(portName);
        }

        if (Input.GetKeyDown(KeyCode.Q) && heldObject != null)
        {
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

            heldObject.GetComponent<Collider2D>().enabled = true;
            heldObject = null;
        }

        if (heldObject != null)
        {
            heldObject.transform.position = transform.position + new Vector3(0.5f, 0.5f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pickable"))
        {
            nearbyObject = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Pickable") && other.gameObject == nearbyObject)
        {
            nearbyObject = null;
        }
    }

    private IEnumerator DelayedActivate()
    {
        yield return new WaitForSeconds(3f);
        
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
