using UnityEngine;

public class BananaFarm : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private int goldPerSpawn = 10;
    [SerializeField] public int cost = 50;
    [SerializeField] private int upgradeCost = 40;
    [SerializeField] private float upgradeIntervalMultiplier = 0.8f; // 20% faster per upgrade
    [SerializeField] private int upgradeGoldBonus = 5; // +5 gold per upgrade
    public GameObject upgradeUI;
    public GameObject goldPrefab;
    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            LevelManager.main.SpawnGold(goldPrefab, transform.position);
        }
    }

    public void Upgrade()
    {
        if (LevelManager.main.currency >= upgradeCost)
        {
            LevelManager.main.SpendCurrency(upgradeCost);
            spawnInterval *= upgradeIntervalMultiplier;
            goldPerSpawn += upgradeGoldBonus;
            upgradeCost = Mathf.RoundToInt(upgradeCost * 1.5f);
            Debug.Log($"BananaFarm upgraded! New interval: {spawnInterval}, New gold per spawn: {goldPerSpawn}, Next upgrade cost: {upgradeCost}");
        }
        else
        {
            Debug.Log("Not enough currency to upgrade BananaFarm!");
        }
    }

    public void Sell()
    {
        int refund = Mathf.RoundToInt(cost * 0.5f);
        LevelManager.main.IncreaseCurrency(refund);
        Debug.Log($"BananaFarm sold! Refunded {refund} gold.");
        Destroy(gameObject);
    }

    public void OpenUpgradeUI()
    {
        upgradeUI.SetActive(true);
    }

    public void CloseUpgradeUI()
    {
        upgradeUI.SetActive(false);
    }

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

    private void OnMouseExit()
    {
        CloseUpgradeUI();
    }
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
