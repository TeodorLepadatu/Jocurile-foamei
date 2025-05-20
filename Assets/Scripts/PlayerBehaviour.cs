using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;

    protected Vector2 movement;

    protected virtual void Update()
    {
        HandleMovement();
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
            transform.localScale = new Vector3(0.12f, 0.12f, 1);
        else if (movement.x < 0)
            transform.localScale = new Vector3(-0.12f, 0.12f, 1);
    }
}
