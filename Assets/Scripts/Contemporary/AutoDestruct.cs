using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float lifetime = 1f;  // seconds until destroy

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
