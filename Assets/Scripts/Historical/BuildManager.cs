using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour 
{
    public static BuildManager main;

    [Header("References")]
    [SerializeField] public GameObject[] towerPrefabs;
    [SerializeField] public static Dictionary<GameObject, int> costDictionary = new Dictionary<GameObject, int>();
    public static int selectedTower = -1;
    private void Awake()
    {
        main = this;
    }
    private void Start()
    {
        for (int i = 0; i < towerPrefabs.Length; i++)
        {
            try { 
            costDictionary.Add(towerPrefabs[i], towerPrefabs[i].GetComponent<Turret>().cost);
            Debug.Log(towerPrefabs[i].name + " costs: " + costDictionary[towerPrefabs[i]]);
            }
            catch(System.Exception)
            {
                try
                {
                    costDictionary.Add(towerPrefabs[i], towerPrefabs[i].GetComponent<TurretSlowmo>().cost);
                    Debug.Log(towerPrefabs[i].name + " costs: " + costDictionary[towerPrefabs[i]]);
                }
                catch(System.Exception)
                {
                    costDictionary.Add(towerPrefabs[i], towerPrefabs[i].GetComponent<BananaFarm>().cost);
                }
            }
        }
    }
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

        if (selectedTower < 0 || selectedTower >= towerPrefabs.Length)
        {
            Debug.LogError("Invalid tower selection: " + selectedTower);
            return null;
        }
        return towerPrefabs[selectedTower];
    }


    public void SetSelectedTower(int _selectedTower)
    {
        selectedTower = _selectedTower;
    }
}
