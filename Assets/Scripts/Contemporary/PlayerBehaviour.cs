using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;

    private Vector2 movement;

    void Update()
    {
        // Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Animate
        animator.SetFloat("moveX", movement.x);
        animator.SetFloat("moveY", movement.y);
        animator.SetFloat("speed", movement.sqrMagnitude);

        // Flip sprite based on horizontal direction
        if (movement.x > 0)
            transform.localScale = new Vector3(0.12f, 0.12f, 1);  // Facing right
        else if (movement.x < 0)
            transform.localScale = new Vector3(-0.12f, 0.12f, 1); // Facing left
    }


    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
