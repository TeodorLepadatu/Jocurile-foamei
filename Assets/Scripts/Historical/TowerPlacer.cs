using UnityEngine;

public class TowerPlacer : MonoBehaviour
{
    [SerializeField] private GameObject towerPrefab;

    private void Update()
    {
        // Check if the left mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            // Ensure a tower has been selected before placement
            if (BuildManager.main.GetSelectedTower() == null)
            {
                Debug.Log("No tower selected! Please select a tower before placing.");
                return;
            }

            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (IsPositionValid(worldPos))
            {
                towerPrefab = BuildManager.main.GetSelectedTower();

                if (BuildManager.costDictionary[towerPrefab] > PlayerController.gold)
                {
                    Debug.Log("You do not have enough gold to buy this tower!");
                    return;
                }
                else
                {
                    Instantiate(towerPrefab, worldPos, Quaternion.identity);
                    LevelManager.main.currency -= BuildManager.costDictionary[towerPrefab];

                    // Reset the selected tower after placement
                    BuildManager.main.SetSelectedTower(-1);
                }
            }
        }
    }

    private bool IsPositionValid(Vector2 position)
    {
        float checkRadius = 0.5f;

        Collider2D hit = Physics2D.OverlapCircle(position, checkRadius);
        if (hit != null)
        {
            Debug.Log($"Invalid placement: hit object '{hit.name}'");
            return false;
        }
        return true;
    }
}
