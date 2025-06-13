using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Controls the behavior of a turret: targeting, shooting, upgrading, and UI interactions.
/// </summary>
public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint; // The part of the turret that rotates to aim
    [SerializeField] private LayerMask enemyMask;           // Layer mask to identify enemies
    [SerializeField] private GameObject bulletPrefab;        // Prefab for the bullet to shoot
    [SerializeField] private Transform firingPoint;          // Where bullets are spawned from
    [SerializeField] private GameObject upgradeUI;           // UI panel for upgrades
    [SerializeField] private Button upgradeButton;           // Button to trigger upgrade

    [Header("Attributes")]
    [SerializeField] private float targetingRange = 5f;      // How far the turret can target enemies
    [SerializeField] private float bps = 1f;                 // Bullets per second (fire rate)

    private Transform target;            // Current target enemy
    private float timeUntilFire;         // Timer for firing logic
    public float angleAdjustment = -90f; // Used to align the turret sprite with the aiming direction
    public int cost = 10;                // Cost to upgrade or sell the turret

    /// <summary>
    /// Hides the upgrade UI at the start.
    /// </summary>
    private void Start()
    {
        upgradeUI.SetActive(false);
    }

    /// <summary>
    /// Handles targeting, rotation, and firing logic each frame.
    /// </summary>
    private void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }
        else
            Debug.DrawLine(turretRotationPoint.position, target.position, Color.red);

        RotateTowardsTarget();

        if (!CheckTargetIsInRange())
            target = null;
        else
        {
            timeUntilFire += Time.deltaTime;
            if (timeUntilFire >= 1f / bps)
            {
                Shoot();
                timeUntilFire = 0f;
            }
        }
    }

    /// <summary>
    /// Instantiates a bullet and sets its target.
    /// </summary>
    private void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, turretRotationPoint.rotation);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(target);
    }

    /// <summary>
    /// Finds the first enemy within range using a circle cast.
    /// </summary>
    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask);
        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }

    /// <summary>
    /// Checks if the current target is still within range.
    /// </summary>
    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    /// <summary>
    /// Rotates the turret to face the current target.
    /// </summary>
    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - turretRotationPoint.position.y, target.position.x - turretRotationPoint.position.x) * Mathf.Rad2Deg + angleAdjustment;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        turretRotationPoint.rotation = targetRotation;
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
    /// Upgrades the turret if the player has enough currency, increasing fire rate and upgrade cost.
    /// </summary>
    public void Upgrade()
    {
        if (LevelManager.main.currency >= cost)
        {
            LevelManager.main.SpendCurrency(cost);
            bps *= 1.2f;
            cost = Mathf.RoundToInt(cost * 1.5f);

            Debug.Log("Turret upgraded! New range: " + targetingRange + ", Next cost: " + cost);
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
