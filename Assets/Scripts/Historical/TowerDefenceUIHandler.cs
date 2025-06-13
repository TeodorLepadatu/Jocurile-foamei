using System.Collections;
using System.Transactions;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Handles the UI for the Tower Defence mode, specifically updating the health bar based on the player's health.
/// </summary>
public class TowerDefenceUIHandler : MonoBehaviour
{
    // Singleton instance for global access
    public static TowerDefenceUIHandler instance { get; private set; }

    private VisualElement m_Healthbar; // Reference to the health bar UI element
    private int currentHealth;         // Cached value of the player's current health

    /// <summary>
    /// Sets up the singleton instance.
    /// </summary>
    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Initializes the health bar reference and sets its initial value based on the player's health.
    /// </summary>
    private void Start()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        PlayerController player = FindObjectOfType<PlayerController>();
        currentHealth = player.health;
        m_Healthbar = uiDocument.rootVisualElement.Q<VisualElement>("HealthBar");
        SetHealthValue((float)currentHealth / (float)player.maxHealth);
    }

    /// <summary>
    /// Updates the health bar every frame to reflect the player's current health.
    /// </summary>
    private void Update()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        currentHealth = player.health;
        SetHealthValue((float)currentHealth / (float)player.maxHealth);
    }

    /// <summary>
    /// Sets the width of the health bar based on the given percentage (0.0 to 1.0).
    /// </summary>
    /// <param name="percentage">The percentage of health remaining.</param>
    public void SetHealthValue(float percentage)
    {
        m_Healthbar.style.width = Length.Percent(100 * percentage);
    }
}
