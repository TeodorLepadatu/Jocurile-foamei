using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls simple NPC movement in a straight line, reversing direction at set intervals.
/// </summary>
public class NPCController : MonoBehaviour
{
    public float speed = 0.25f;         // Movement speed of the NPC
    public bool vertical = true;        // If true, moves vertically; otherwise, moves horizontally
    public float changeTime = 1.0f;     // Time in seconds before changing direction

    private Rigidbody2D rigidbody2d;    // Reference to the Rigidbody2D component
    private float timer;                // Timer to track when to change direction
    private int direction = 1;          // Current movement direction (1 or -1)

    /// <summary>
    /// Initializes the NPC's Rigidbody2D and timer.
    /// </summary>
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        timer = changeTime;
    }

    /// <summary>
    /// Updates the timer and reverses direction when the timer runs out.
    /// </summary>
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction; // Reverse direction
            timer = changeTime;     // Reset timer
        }
    }

    /// <summary>
    /// Handles the NPC's movement in FixedUpdate for consistent physics behavior.
    /// </summary>
    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;

        if (vertical)
        {
            position.y = position.y + speed * direction * Time.deltaTime;
        }
        else
        {
            position.x = position.x + speed * direction * Time.deltaTime;
        }

        rigidbody2d.MovePosition(position);
    }
}
