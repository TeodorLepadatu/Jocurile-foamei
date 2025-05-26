using UnityEngine;

public class CM2_EggProjectile : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 5f); // auto-despawn
    }
}
