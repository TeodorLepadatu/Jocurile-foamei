using UnityEngine;

public class CoalGenerator : MonoBehaviour
{
    public GameObject coalPrefab;
    public static bool canGenerate = true;

    private Transform canvasTransform;

    void Start()
    {
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
