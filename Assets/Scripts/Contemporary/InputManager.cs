using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class InputManager : MonoBehaviour
{
    public GameObject inputFieldObject;
    private InputField inputField;

    public GameObject successObject;

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
            string userInput = inputField.text.Trim().ToLower();

            if (userInput == "no shutdown")
            {   
                successObject.SetActive(true);

                StartCoroutine(DelayedReset());
            }
            else
            {
                Debug.Log("Incorrect command: " + userInput);
            }
        }
    }

    private IEnumerator DelayedReset()
    {
        yield return new WaitForSeconds(3f);
        
        var sr = selectedSwitch.GetComponent<SpriteRenderer>();
        sr.color = Color.green;
        SwitchController.turnedSwitches++;
        selectedSwitch.objectSelector.greenPermanently = true;
        Service.objectSelector.isSelected = false;
        inputField.text = "";
        inputFieldObject.SetActive(false);

        successObject.SetActive(false);

        if(SwitchController.turnedSwitches == 2) {
            
        }
    }
}
