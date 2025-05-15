using UnityEngine;

public class MagicZoneManager : MonoBehaviour
{
    public PickableObject leafObject;
    public PickableObject waterObject;
    public PickableObject fireObject;
    public GameObject dartMonkeyPrefab; 

    public static bool allPlacedCorrectly = false;

    void Update()
    {
        if (!allPlacedCorrectly &&
            leafObject != null && waterObject != null && fireObject != null)
        {
            if (leafObject.IsInTargetZone() &&
                waterObject.IsInTargetZone() &&
                fireObject.IsInTargetZone())
            {
                allPlacedCorrectly = true;
                Debug.Log("All objects are in their correct magic zones!");
                leafObject.GetDestroyed();
                waterObject.GetDestroyed();
                fireObject.GetDestroyed();

                Instantiate(dartMonkeyPrefab, new Vector3(4f, 11f, 0f), Quaternion.identity);
            }
        }
    }
}
