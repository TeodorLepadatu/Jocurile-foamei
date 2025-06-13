using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SettingsMenu : MonoBehaviour
{
    [Header("Sliders")]
    public Slider fpsSlider;
    public Text fpsValueText;
    public Slider volumeSlider;
    public Text volumeValueText;

    [Header("Messages")]
    public Text appliedMessageText;

    private float appliedFPS;
    private float appliedVolume;

    private void Start()
    {
        // Slider listeners
        fpsSlider.onValueChanged.AddListener(UpdateFPSValue);
        volumeSlider.onValueChanged.AddListener(UpdateVolumeValue);

        // Load saved settings
        appliedFPS = PlayerPrefs.GetFloat("FPS", 60f);
        appliedVolume = PlayerPrefs.GetFloat("Volume", 1f);

        fpsSlider.value = appliedFPS;
        volumeSlider.value = appliedVolume;

        Application.targetFrameRate = (int)appliedFPS;
        AudioListener.volume = appliedVolume;

        UpdateFPSValue(appliedFPS);
        UpdateVolumeValue(appliedVolume);

        appliedMessageText.text = "";
    }

    public void UpdateFPSValue(float value)
    {
        fpsValueText.text = value.ToString("0");
    }

    public void UpdateVolumeValue(float value)
    {
        volumeValueText.text = Mathf.RoundToInt(value * 100).ToString();
    }

    public void ApplySettings()
    {
        // Save sliders
        appliedFPS = fpsSlider.value;
        appliedVolume = volumeSlider.value;

        PlayerPrefs.SetFloat("FPS", appliedFPS);
        PlayerPrefs.SetFloat("Volume", appliedVolume);
        PlayerPrefs.Save();

        Application.targetFrameRate = (int)appliedFPS;
        AudioListener.volume = appliedVolume;

        appliedMessageText.text = "Settings applied";
    }

    public void BackToMainMenu()
    {
        appliedMessageText.text = "";
    }
}
