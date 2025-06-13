using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// A turret that periodically slows enemies within range, can be upgraded or sold, and displays an upgrade UI.
/// </summary>
public class TurretSlowmo : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask enemyMask; // Layer mask to identify enemy objects

    [Header("Attributes")]
    [SerializeField] private float targetingRange = 5f; // Range in which enemies are affected
    [SerializeField] private float aps = 0.25f;         // Attacks per second (how often the turret fires)
    [SerializeField] private float freezeTime = 1f;     // Duration for which enemies are slowed
    [SerializeField] private GameObject upgradeUI;      // UI panel for upgrades
    [SerializeField] private Button upgradeButton;      // Button to trigger upgrade

    private float timeUntilFire; // Timer for attack interval
    public int cost = 20;        // Cost to upgrade or sell the turret

    /// <summary>
    /// Hides the upgrade UI at the start.
    /// </summary>
    private void Start()
    {
        upgradeUI.SetActive(false);
    }

    /// <summary>
    /// Handles the attack timer and triggers freezing enemies at the specified rate.
    /// </summary>
    private void Update()
    {
        timeUntilFire += Time.deltaTime;
        if (timeUntilFire >= 1f / aps)
        {
            FreezeEnemies();
            timeUntilFire = 0f;
        }
    }

    /// <summary>
    /// Finds all enemies within range and slows them for a duration.
    /// </summary>
    private void FreezeEnemies()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask);
        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];
                EnemyMovement em = hit.transform.GetComponent<EnemyMovement>();
                if (em != null)
                {
                    em.UpdateSpeed(0.5f); // Slow enemy
                    StartCoroutine(ResetEnemySpeed(em));
                }
            }
        }
    }

    /// <summary>
    /// Resets the enemy's speed after the freeze duration.
    /// </summary>
    private IEnumerator ResetEnemySpeed(EnemyMovement em)
    {
        yield return new WaitForSeconds(freezeTime);
        em.ResetSpeed();
    }

    /// <summary>
    /// Draws the targeting range in the editor for visualization.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
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
    /// Shows the upgrade UI when the player is close and hovers the mouse over the turret.
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
    /// Hides the upgrade UI when the mouse leaves the turret.
    /// </summary>
    private void OnMouseExit()
    {
        CloseUpgradeUI();
    }

    /// <summary>
    /// Upgrades the turret if the player has enough currency, increasing range, attack rate, and freeze duration.
    /// </summary>
    public void Upgrade()
    {
        if (LevelManager.main.currency >= cost)
        {
            LevelManager.main.SpendCurrency(cost);

            targetingRange *= 1.2f;
            aps *= 1.1f;
            freezeTime *= 1.2f;
            cost = Mathf.RoundToInt(cost * 2f);

            Debug.Log("Ice Turret upgraded! New range: " + targetingRange + ", New APS: " + aps + ", New freeze time: " + freezeTime + ", Next cost: " + cost);
        }
        else
        {
            Debug.Log("Not enough currency to upgrade!");
        }
    }

    /// <summary>
    /// Sells the turret, refunds half the cost, and destroys the turret object.
    /// </summary>
    private void Sell()
    {
        int refund = Mathf.RoundToInt(cost * 0.5f);
        LevelManager.main.currency += refund;
        Debug.Log($"Turret sold! Refunded {refund} currency.");
        Destroy(gameObject);
    }

    /// <summary>
    /// Allows the player to sell the turret by right-clicking if close enough.
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
