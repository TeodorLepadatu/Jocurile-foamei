using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    public AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            audioSource.volume = PlayerPrefs.GetFloat("Volume") / 100f;
            instance = this;
            audioSource.loop = true;
            audioSource.playOnAwake = false;
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource.Play();
    }

    void OnDestroy() {
        audioSource.Stop();
    }
}
