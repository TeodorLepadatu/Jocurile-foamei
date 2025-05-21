using UnityEngine;

public class CoalGenerator : MonoBehaviour
{
    public GameObject coalPrefab;         // Assign a UI prefab (e.g., Image)
    public static bool canGenerate = true;

    private Transform canvasTransform;

    void Start()
    {
        // Find the UI canvas inside Step3
        GameObject step3 = GameObject.Find("Step3");
        canvasTransform = step3.transform.Find("Canvas");
    }

    void Update()
    {
        if (canGenerate)
        {
            GameObject newCoal = Instantiate(coalPrefab, canvasTransform);
            canGenerate = false;
        }
    }
}
