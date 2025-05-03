using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Collider2D))]
public class NonPlayerCharacter : MonoBehaviour
{
    [TextArea(2, 5)]
    public string dialogueText = "Hey, you! Do you want to escape? To get the weapon you are looking for, think of something magic."; // The text the NPC will say

    public AudioClip dialogueAudio; // The audio clip for the dialogue
    private AudioSource audioSource;

    public bool playerNearby = false; // Tracks if the player is near

    private void Start()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();

        // Ensure the Collider2D is set as a trigger
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player entered the NPC's trigger
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            playerNearby = true;
            Debug.Log("Player is near NPC: " + gameObject.name);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the player exited the NPC's trigger
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            playerNearby = false;
            Debug.Log("Player left NPC: " + gameObject.name);
        }
    }

    public void DisplayDialogue()
    {
        if (playerNearby)
        {
            // Display the dialogue text in the UI
            Debug.Log("NPC says: " + dialogueText);

            // Play the dialogue audio if available
            if (dialogueAudio != null)
            {
                audioSource.clip = dialogueAudio;
                audioSource.Play();
            }
            else
            {
                Debug.LogWarning("No audio clip assigned for this NPC.");
            }
        }
        else
        {
            Debug.Log("Player is not near the NPC.");
        }
    }
}
