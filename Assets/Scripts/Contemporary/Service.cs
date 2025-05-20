using UnityEngine;

public class Service : MonoBehaviour
{
    [HideInInspector] public static ObjectSelector objectSelector;
    public GameObject switch1, switch2;

    void Awake()
    {
        switch1.SetActive(true);
        switch2.SetActive(true);
        objectSelector = GetComponent<ObjectSelector>();
    }
}
