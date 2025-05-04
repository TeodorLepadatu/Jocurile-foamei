using UnityEngine;

//[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Collider2D))]
public class NonPlayerCharacter : MonoBehaviour
{
    [TextArea(2, 5)]
    //public AudioClip dialogueAudio; 
    //private AudioSource audioSource;

    public bool playerNearby = false; 

    private void Start()
    {
        //audioSource = GetComponent<AudioSource>();

        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            playerNearby = true;
            Debug.Log("Player is near NPC: " + gameObject.name);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            playerNearby = false;
        }
    }

    public void DisplayDialogue()
    {
        if (playerNearby)
        {
            /* Play the dialogue audio if available
            if (dialogueAudio != null)
            {
                audioSource.clip = dialogueAudio;
                audioSource.Play();
            }
            else
            {
                Debug.LogWarning("No audio clip assigned for this NPC.");
            }
            */
        }
        else
        {
            Debug.Log("Player is not near the NPC.");
        }
    }
}
