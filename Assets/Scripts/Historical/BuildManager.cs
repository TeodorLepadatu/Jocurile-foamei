using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages tower building, selection, and cost lookup for the player.
/// Handles proximity checks for tower selection and maintains a dictionary of tower costs.
/// </summary>
public class BuildManager : MonoBehaviour
{
    // Singleton instance for global access
    public static BuildManager main;

    [Header("References")]
    [SerializeField] public GameObject[] towerPrefabs; // Array of all available tower prefabs
    [SerializeField] public static Dictionary<GameObject, int> costDictionary = new Dictionary<GameObject, int>(); // Maps each tower prefab to its cost
    public static int selectedTower = -1; // Index of the currently selected tower (-1 means none selected)

    /// <summary>
    /// Sets up the singleton instance.
    /// </summary>
    private void Awake()
    {
        main = this;
    }

    /// <summary>
    /// Initializes the cost dictionary for all tower prefabs.
    /// Tries to get the cost from Turret, TurretSlowmo, or BananaFarm components.
    /// </summary>
    private void Start()
    {
        for (int i = 0; i < towerPrefabs.Length; i++)
        {
            try
            {
                // Try to get cost from Turret component
                costDictionary.Add(towerPrefabs[i], towerPrefabs[i].GetComponent<Turret>().cost);
                Debug.Log(towerPrefabs[i].name + " costs: " + costDictionary[towerPrefabs[i]]);
            }
            catch (System.Exception)
            {
                try
                {
                    // If not a Turret, try TurretSlowmo
                    costDictionary.Add(towerPrefabs[i], towerPrefabs[i].GetComponent<TurretSlowmo>().cost);
                    Debug.Log(towerPrefabs[i].name + " costs: " + costDictionary[towerPrefabs[i]]);
                }
                catch (System.Exception)
                {
                    // If not a TurretSlowmo, try BananaFarm
                    costDictionary.Add(towerPrefabs[i], towerPrefabs[i].GetComponent<BananaFarm>().cost);
                }
            }
        }
    }

    /// <summary>
    /// Returns the currently selected tower prefab if the player is near the crafting table.
    /// Performs proximity and validity checks.
    /// </summary>
    public GameObject GetSelectedTower()
    {
        // Proximity check: player must be near the crafting table
        PlayerController player = FindObjectOfType<PlayerController>();
        CraftingTable craftingTable = FindObjectOfType<CraftingTable>();
        if (player == null || craftingTable == null)
        {
            Debug.LogWarning("Player or CraftingTable not found in the scene.");
            return null;
        }

        float distance = Vector2.Distance(player.transform.position, craftingTable.transform.position);
        if (distance > craftingTable.proximityDistance)
        {
            Debug.Log("You must be near the crafting table to select a tower.");
            Debug.Log("Distance: " + distance);
            return null;
        }

        // Check if the selected tower index is valid
        if (selectedTower < 0 || selectedTower >= towerPrefabs.Length)
        {
            Debug.LogError("Invalid tower selection: " + selectedTower);
            return null;
        }
        return towerPrefabs[selectedTower];
    }

    /// <summary>
    /// Sets the selected tower index.
    /// </summary>
    public void SetSelectedTower(int _selectedTower)
    {
        selectedTower = _selectedTower;
    }
}
