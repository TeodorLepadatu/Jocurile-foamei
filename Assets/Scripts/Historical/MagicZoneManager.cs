using UnityEngine;

public class MagicZoneManager : MonoBehaviour
{
    public PickableObject leafObject;
    public PickableObject waterObject;
    public PickableObject fireObject;

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
            }
        }
    }
}
