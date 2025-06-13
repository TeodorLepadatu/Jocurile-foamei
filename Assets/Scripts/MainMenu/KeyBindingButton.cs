using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class KeyBindingButton : MonoBehaviour, IPointerClickHandler
{
    public Controls.Action action; // Set this in Inspector
    private Text labelText;
    private bool isListening = false;

    private void Start()
    {
        labelText = GetComponentInChildren<Text>();
        UpdateLabel();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isListening = true;
        labelText.text = $"{action}: ...";
    }

    private void OnGUI()
    {
        if (!isListening) return;

        Event e = Event.current;
        if (e.isKey)
        {
            Controls.SetKey(action, e.keyCode);
            PlayerPrefs.SetString(action.ToString(), e.keyCode.ToString());
            PlayerPrefs.Save();
            isListening = false;
            UpdateLabel();
        }
    }

    private void UpdateLabel()
    {
        var key = Controls.GetBoundKey(action);
        labelText.text = $"{action}: {key}";
    }
}
