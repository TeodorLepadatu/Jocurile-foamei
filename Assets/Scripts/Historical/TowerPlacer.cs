using System;
using UnityEngine;

public class TowerPlacer : MonoBehaviour
{
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private float placementRange = 3.0f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (BuildManager.selectedTower > BuildManager.main.towerPrefabs.Length)
            {
                Debug.Log("No tower selected! Please select a tower before placing.");
                return;
            }

            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Proximity check
            PlayerController player = FindObjectOfType<PlayerController>();
            if (player == null)
            {
                Debug.LogWarning("Player not found in scene!");
                return;
            }
            float distance = Vector2.Distance(player.transform.position, worldPos);
            if (distance > placementRange)
            {
                Debug.Log("You are too far away to place a tower here!");
                return;
            }

            if (IsPositionValid(worldPos))
            {
                Debug.Log(BuildManager.selectedTower);
                towerPrefab = BuildManager.main.towerPrefabs[BuildManager.selectedTower];

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
