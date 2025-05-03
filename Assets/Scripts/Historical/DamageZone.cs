using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public float damageInterval = 1f; // Time in seconds between damage ticks
    private float damageTimer;

    void Update()
    {
        // Update the damage timer
        damageTimer += Time.deltaTime;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();

        if (controller != null && damageTimer >= damageInterval)
        {
            controller.ChangeHealth(-1); // Apply damage
            damageTimer = 0f; // Reset the timer
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Reset the timer when the player leaves the damage zone
        if (other.GetComponent<PlayerController>() != null)
        {
            damageTimer = 0f;
        }
    }
}
