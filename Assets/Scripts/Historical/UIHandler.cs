using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Handles the main UI for the game, including health bar, gold display, and NPC dialogue popups.
/// </summary>
public class UIHandler : MonoBehaviour
{
    // Singleton instance for global access
    public static UIHandler instance { get; private set; }

    private VisualElement m_Healthbar;      // Reference to the health bar UI element
    private Label m_GoldAmount;             // Reference to the gold amount label

    public float displayTime = 4.0f;        // How long to display NPC dialogue
    private VisualElement m_NonPlayerDialogue; // Reference to the NPC dialogue UI element
    private float m_TimerDisplay;           // Timer for how long the dialogue is shown

    /// <summary>
    /// Sets up the singleton instance.
    /// </summary>
    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Initializes UI references and sets default values.
    /// </summary>
    private void Start()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();

        // Get references to UI elements by their names in the UI Document
        m_Healthbar = uiDocument.rootVisualElement.Q<VisualElement>("HealthBar");
        m_GoldAmount = uiDocument.rootVisualElement.Q<Label>("goldAmount");

        SetHealthValue(1.0f); // Start with full health
        SetGoldValue(0);      // Start with zero gold

        m_NonPlayerDialogue = uiDocument.rootVisualElement.Q<VisualElement>("NPCDialogue");
        m_NonPlayerDialogue.style.display = DisplayStyle.None; // Hide dialogue by default
        m_TimerDisplay = -1.0f;
    }

    /// <summary>
    /// Updates the dialogue timer and hides the dialogue when time runs out.
    /// </summary>
    private void Update()
    {
        if (m_TimerDisplay > 0)
        {
            m_TimerDisplay -= Time.deltaTime;
            if (m_TimerDisplay < 0)
            {
                m_NonPlayerDialogue.style.display = DisplayStyle.None;
            }
        }
    }

    /// <summary>
    /// Sets the health bar width based on the given percentage (0.0 to 1.0).
    /// </summary>
    /// <param name="percentage">Fraction of health remaining.</param>
    public void SetHealthValue(float percentage)
    {
        m_Healthbar.style.width = Length.Percent(100 * percentage);
    }

    /// <summary>
    /// Displays the NPC dialogue UI for a set duration.
    /// </summary>
    public void DisplayDialogue()
    {
        if (m_NonPlayerDialogue != null)
        {
            m_NonPlayerDialogue.style.display = DisplayStyle.Flex;
            m_TimerDisplay = displayTime;
        }
    }

    /// <summary>
    /// Sets the gold amount label to show the current gold.
    /// </summary>
    /// <param name="amount">The player's current gold.</param>
    public void SetGoldValue(int amount)
    {
        m_GoldAmount.text = amount + " leu leutzi";
    }
}
