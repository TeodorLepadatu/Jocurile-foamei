using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;

    private Vector2 movement;
    private GameObject heldObject = null;
    
    // Track nearby pickup object
    private GameObject nearbyObject = null;

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("moveX", movement.x);
        animator.SetFloat("moveY", movement.y);
        animator.SetFloat("speed", movement.sqrMagnitude);

        if (movement.x > 0) transform.localScale = new Vector3(0.12f, 0.12f, 1);
        else if (movement.x < 0) transform.localScale = new Vector3(-0.12f, 0.12f, 1);

        if (Input.GetKeyDown(KeyCode.E) && nearbyObject != null && heldObject == null)
        {
            heldObject = nearbyObject;
            // Optional: Disable collider to avoid re-triggering while held
            heldObject.GetComponent<Collider2D>().enabled = false;
        }

        // Drop key (Q)
        if (Input.GetKeyDown(KeyCode.Q) && heldObject != null)
        {
            heldObject.GetComponent<Collider2D>().enabled = true;
            heldObject = null;
        }

        if (heldObject != null)
        {
            Vector3 followOffset = new Vector3(0.5f, 0.5f, 0); // Adjust as needed
            heldObject.transform.position = transform.position + followOffset;
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

   
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pickable"))
        {
            nearbyObject = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Pickable") && other.gameObject == nearbyObject)
        {
            nearbyObject = null;
        }
    }
}
