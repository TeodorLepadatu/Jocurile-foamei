using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;


public class InputManager : MonoBehaviour
{
    public GameObject inputFieldObject;
    private InputField inputField;
    private string userInput;

    public GameObject successObject;
    public GameObject minigame1;
    public GameObject gameOverScreen;
    
    private SwitchController selectedSwitch;

    void Start()
    {
        inputField = inputFieldObject.GetComponent<InputField>();
    }

    public void SetSelectedSwitch(SwitchController switchCtrl)
    {
        selectedSwitch = switchCtrl;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            userInput = inputField.text.Trim().ToLower();

            if (userInput == "no shutdown") // turn on the switch
            {   
                successObject.SetActive(true);

                CurrencyHolder.addCurrency(5);
                PlayerContemporary.Instance.coinText.text = CurrencyHolder.getCurrency().ToString();

                StartCoroutine(DelayedReset());
            }
            else
            {
                StartCoroutine(DelayedReset());
            }
        }
    }

    private IEnumerator DelayedReset()
    {
        if(userInput != "no shutdown") {
            yield return new WaitForSeconds(1f);
        }
        else {
            yield return new WaitForSeconds(2f);
        }
        
        if(userInput != "no shutdown") { // game is lost
            minigame1.SetActive(false);
            gameOverScreen.SetActive(true);

            yield return new WaitForSeconds(3f);

            SceneManager.LoadScene("CMinigame2");
            yield break;
        }
        
        var sr = selectedSwitch.GetComponent<SpriteRenderer>();
        sr.color = Color.green;
        SwitchController.turnedSwitches++;
        selectedSwitch.objectSelector.greenPermanently = true;
        Service.objectSelector.isSelected = false;
        inputField.text = "";
        inputFieldObject.SetActive(false);

        successObject.SetActive(false);
    }
}
