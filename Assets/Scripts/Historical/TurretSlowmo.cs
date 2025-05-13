using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class TurretSlowmo : MonoBehaviour 
{
    [Header("References")]
    [SerializeField] private LayerMask enemyMask;

    [Header("Attributes")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float aps = 0.25f; //attacks per second
    [SerializeField] private float freezeTime = 1f;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private Button upgradeButton;
    private float timeUntilFire;
    public int cost = 20;
    //private bool isUIOpen = false;
    private void Start()
    {
        upgradeUI.SetActive(false);
    }
    private void Update()
    { 
            timeUntilFire += Time.deltaTime;
            if (timeUntilFire >= 1f / aps)
            {
                FreezeEnemies();
                timeUntilFire = 0f;
            }
    }
    
    private void FreezeEnemies()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask);
        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];
                EnemyMovement em = hit.transform.GetComponent<EnemyMovement>();
                em.UpdateSpeed(0.5f);
                StartCoroutine(ResetEnemySpeed(em));
            }
        }
    }

    private IEnumerator ResetEnemySpeed(EnemyMovement em)
    {
        yield return new WaitForSeconds(freezeTime);
        em.ResetSpeed();
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
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
        OpenUpgradeUI();
        //isUIOpen = true;
    }

    private void OnMouseExit()
    {
        CloseUpgradeUI();
        //isUIOpen = false;
    }

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

}
