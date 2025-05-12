using System.Collections;
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
        currentHealth = PlayerController.instance.currentHealth;

        m_Healthbar = uiDocument.rootVisualElement.Q<VisualElement>("HealthBar");

        SetHealthValue(currentHealth / PlayerController.instance.maxHealth);
    }

    public void SetHealthValue(float percentage)
    {
        m_Healthbar.style.width = Length.Percent(100 * percentage);
    }

}
