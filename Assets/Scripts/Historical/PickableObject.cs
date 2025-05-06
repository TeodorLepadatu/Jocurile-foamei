using UnityEngine;

public class PickableObject : MonoBehaviour
{
    public string objectName;
    private Rigidbody2D rb;
    public Vector2 areaMinLeaf = new Vector2(4f, 0f);  
    public Vector2 areaMaxLeaf = new Vector2(8f, -3f);  
    public Vector2 areaMinWater = new Vector2(3f, -2f);
    public Vector2 areaMaxWater = new Vector2(6f, -5f);
    public Vector2 areaMinFire = new Vector2(5f, -2f);
    public Vector2 areaMaxFire = new Vector2(8f, -5f);
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        /*
        if (IsInTargetZone())
        {
            Debug.Log(name + " is inside the magic zone!");
        }
        */
    }
    public bool IsInTargetZone()
    {
        Vector2 pos = transform.position;
        if(objectName == "Leaf")
        {
            return IsInArea(pos, areaMinLeaf, areaMaxLeaf);
        }
        else if (objectName == "Water")
        {
            return IsInArea(pos, areaMinWater, areaMaxWater);
        }
        else if (objectName == "Fire")
        {
            return IsInArea(pos, areaMinFire, areaMaxFire);
        }
        return false;
    }
    private bool IsInArea(Vector2 pos, Vector2 min, Vector2 max)
    {
        return pos.x >= min.x && pos.x <= max.x && pos.y <= min.y && pos.y >= max.y;
    }
    public void PickUp(Transform parent)
    {
        rb.simulated = false; // Disable physics
        rb.linearVelocity = Vector2.zero;
        transform.SetParent(parent);
        transform.localPosition = new Vector3(0, 0.75f, 0); // adjust position in front of player
    }

    public void Drop(Vector2 dropPosition)
    {
        transform.SetParent(null);
        transform.position = dropPosition;
        rb.simulated = true; // Re-enable physics
    }
}
