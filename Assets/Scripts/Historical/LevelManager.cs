using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;
    public Transform[] path;
    public Transform startPoint;

    public int currency;
    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        currency = PlayerController.gold;
    }

    public void DamagePlayer(int damage)
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        Debug.Log("HP: "+player.currentHealth);
        if (player != null)
        {
            player.ChangeHealth(-damage); // Deduct health
            //PlayerController.hitpointsTransmitted -= damage;
        }
    }

    private void Update()
    {
        PlayerController.gold = currency;
    }

    public void IncreaseCurrency(int amount)
    {
        currency += amount;
    }

    public bool SpendCurrency(int amount)
    {
        if (currency >= amount)
        {
            //BUY TOWER
            currency -= amount;
            return true;
        }
        else
        {
            Debug.Log("Not enough currency!");
            return false;
        }
    }

    public void SpawnGold(GameObject goldPrefab, Vector3 pos)
    {
        Vector2 offset = Random.insideUnitCircle * 1f;
        Vector3 spawnPos = pos + new Vector3(offset.x, offset.y, 0);
        Instantiate(goldPrefab, spawnPos, Quaternion.identity);
    }

}
