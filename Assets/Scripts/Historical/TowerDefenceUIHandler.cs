using System.Collections;
using System.Transactions;
using UnityEngine;
using UnityEngine.UIElements;

public class TowerDefenceUIHandler : MonoBehaviour
{
    public static TowerDefenceUIHandler instance { get; private set; }

    private VisualElement m_Healthbar;
    private int currentHealth;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        PlayerController player = FindObjectOfType<PlayerController>();
        currentHealth = player.health;
        m_Healthbar = uiDocument.rootVisualElement.Q<VisualElement>("HealthBar");
        //Debug.Log("HealthBar: " + m_Healthbar);
        SetHealthValue((float)currentHealth / (float)player.maxHealth);
    }
    private void Update()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        currentHealth = player.health;
        SetHealthValue((float)currentHealth / (float)player.maxHealth);
    }
    public void SetHealthValue(float percentage)
    {
        m_Healthbar.style.width = Length.Percent(100 * percentage);
    }

}
