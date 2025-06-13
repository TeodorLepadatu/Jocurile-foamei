using UnityEngine;

/// <summary>
/// Represents a Banana Farm that periodically spawns gold and can be upgraded or sold by the player.
/// </summary>
public class BananaFarm : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float spawnInterval = 5f; // Time (in seconds) between each gold spawn
    [SerializeField] private int goldPerSpawn = 10; // Amount of gold produced per spawn
    [SerializeField] public int cost = 50; // Initial cost to build the Banana Farm
    [SerializeField] private int upgradeCost = 40; // Cost to upgrade the Banana Farm
    [SerializeField] private float upgradeIntervalMultiplier = 0.8f; // Multiplier to decrease spawn interval on upgrade (20% faster)
    [SerializeField] private int upgradeGoldBonus = 5; // Additional gold per spawn after each upgrade
    public GameObject upgradeUI; // Reference to the upgrade UI panel
    public GameObject goldPrefab; // Prefab for the gold collectible to spawn
    private float timer = 0f; // Tracks time since last gold spawn

    /// <summary>
    /// Called every frame. Handles gold spawning based on the interval.
    /// </summary>
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            // Spawns a gold collectible at the farm's position
            LevelManager.main.SpawnGold(goldPrefab, transform.position);
        }
    }

    /// <summary>
    /// Upgrades the Banana Farm if the player has enough currency.
    /// Increases gold output and decreases spawn interval.
    /// </summary>
    public void Upgrade()
    {
        if (LevelManager.main.currency >= upgradeCost)
        {
            LevelManager.main.SpendCurrency(upgradeCost);
            spawnInterval *= upgradeIntervalMultiplier; // Make spawning faster
            goldPerSpawn += upgradeGoldBonus; // Increase gold per spawn
            upgradeCost = Mathf.RoundToInt(upgradeCost * 1.5f); // Increase next upgrade cost
            Debug.Log($"BananaFarm upgraded! New interval: {spawnInterval}, New gold per spawn: {goldPerSpawn}, Next upgrade cost: {upgradeCost}");
        }
        else
        {
            Debug.Log("Not enough currency to upgrade BananaFarm!");
        }
    }

    /// <summary>
    /// Sells the Banana Farm, refunds half the cost, and destroys the object.
    /// </summary>
    public void Sell()
    {
        int refund = Mathf.RoundToInt(cost * 0.5f);
        LevelManager.main.IncreaseCurrency(refund);
        Debug.Log($"BananaFarm sold! Refunded {refund} gold.");
        Destroy(gameObject);
    }

    /// <summary>
    /// Opens the upgrade UI panel.
    /// </summary>
    public void OpenUpgradeUI()
    {
        upgradeUI.SetActive(true);
    }

    /// <summary>
    /// Closes the upgrade UI panel.
    /// </summary>
    public void CloseUpgradeUI()
    {
        upgradeUI.SetActive(false);
    }

    /// <summary>
    /// When the mouse enters the farm, opens the upgrade UI if the player is close enough.
    /// </summary>
    private void OnMouseEnter()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance <= 3f)
            {
                OpenUpgradeUI();
            }
        }
    }

    /// <summary>
    /// When the mouse exits the farm, closes the upgrade UI.
    /// </summary>
    private void OnMouseExit()
    {
        CloseUpgradeUI();
    }

    /// <summary>
    /// Allows the player to sell the farm by right-clicking if close enough.
    /// </summary>
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1)) // Right mouse button
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            if (player != null)
            {
                float distance = Vector2.Distance(transform.position, player.transform.position);
                if (distance <= 3f)
                {
                    Sell();
                }
            }
        }
    }
}
