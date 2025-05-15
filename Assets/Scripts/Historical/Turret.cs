using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private Button upgradeButton;

    [Header("Attributes")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float bps = 1f; // Bullets per second


    private Transform target;
    private float timeUntilFire;
    public float angleAdjustment = -90f;
    public int cost = 10;
    //private bool isUIOpen = false;
    private void Start()
    {
        upgradeUI.SetActive(false);
    }
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
            timeUntilFire+=Time.deltaTime;
            if(timeUntilFire >= 1f / bps)
            {
                Shoot();
                timeUntilFire = 0f;
            }
        }
    }
    private void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, turretRotationPoint.rotation);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(target);
    }
    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask);
        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }

    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - turretRotationPoint.position.y, target.position.x - turretRotationPoint.position.x) * Mathf.Rad2Deg + angleAdjustment;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        turretRotationPoint.rotation = targetRotation;
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position,transform.forward, targetingRange);
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
    private void Sell()
    {
        int refund = Mathf.RoundToInt(cost * 0.5f);
        LevelManager.main.currency += refund;
        Debug.Log($"Turret sold! Refunded {refund} currency.");
        Destroy(gameObject);
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
