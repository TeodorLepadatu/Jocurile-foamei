using UnityEngine;
using TMPro;
public class Menu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI currencyUI;

    private void OnGUI()
    {
        currencyUI.text = "Gold: "+ LevelManager.main.currency.ToString();
    }

    /*
    public void SetSelected()
    {

    }
    */
}
