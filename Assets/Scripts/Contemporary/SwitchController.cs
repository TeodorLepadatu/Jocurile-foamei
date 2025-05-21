using UnityEngine;
using UnityEngine.UI;

public class SwitchController : MonoBehaviour
{
    [HideInInspector] public ObjectSelector objectSelector;
    public InputManager inputManager;
    public static int turnedSwitches = 0;

    void Start()
    {
        objectSelector = GetComponent<ObjectSelector>();
    }

    void Update()
    {
        
    }

    void OnMouseDown()
    {
        if (Service.objectSelector.isSelected)
        {
            inputManager.inputFieldObject.SetActive(true);

            // Tell the input manager which switch is selected
            inputManager.SetSelectedSwitch(this);

            // Focus input
            inputManager.inputFieldObject.GetComponent<InputField>().Select();
            inputManager.inputFieldObject.GetComponent<InputField>().ActivateInputField();
        }
        else
        {
            inputManager.inputFieldObject.SetActive(false);
        }
    }


}
