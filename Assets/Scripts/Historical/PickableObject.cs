using UnityEngine;

public class PickableObject : MonoBehaviour 
{
    public string objectType;
    private Rigidbody2D rigidbody2D;
    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }
    public void PickUp()
    {
        Destroy(gameObject); 
    }
}
