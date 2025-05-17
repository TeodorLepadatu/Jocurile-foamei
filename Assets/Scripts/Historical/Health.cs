using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int hitPoints = 2;
    [SerializeField] private int currencyWorth = 5;

    public GameObject goldPrefab;
    public void TakeDamage(int dmg)
    {
        hitPoints -= dmg;
        if (hitPoints <= 0)
        {
            EnemySpawner.onEnemyDestroy.Invoke();
            for(int i=0; i < currencyWorth; i++)
            {
                LevelManager.main.SpawnGold(goldPrefab, transform.position);
            }
            Destroy(gameObject);
        }
    }
}
