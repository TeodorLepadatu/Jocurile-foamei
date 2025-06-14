using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("Sliders")]
    public Slider fpsSlider;
    public Text fpsValueText;
    public Slider volumeSlider;       // 0–100
    public Text volumeValueText;      // e.g. "75%"

    [Header("Messages")]
    public Text appliedMessageText;

    private float appliedFPS;
    private float appliedVolume;      // stores 0–100

    private void Start()
    {
        // hook up slider callbacks
        fpsSlider.onValueChanged.AddListener(UpdateFPSValue);
        volumeSlider.onValueChanged.AddListener(UpdateVolumeValue);

        // load saved values (default to 60 FPS, 100% volume)
        appliedFPS = PlayerPrefs.GetFloat("FPS", 60f);
        appliedVolume = PlayerPrefs.GetFloat("Volume", 100f);

        fpsSlider.value = appliedFPS;
        volumeSlider.value = appliedVolume;

        // apply them
        Application.targetFrameRate = (int)appliedFPS;
        AudioListener.volume = appliedVolume / 100f;

        // update UI text
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
        // show as percent
        volumeValueText.text = value.ToString("0") + "%";
    }

    public void ApplySettings()
    {
        appliedFPS = fpsSlider.value;
        appliedVolume = volumeSlider.value;

        PlayerPrefs.SetFloat("FPS", appliedFPS);
        PlayerPrefs.SetFloat("Volume", appliedVolume);
        PlayerPrefs.Save();

        Application.targetFrameRate = (int)appliedFPS;
        AudioListener.volume = appliedVolume / 100f;

        appliedMessageText.text = "Settings applied";
    }

    public void BackToMainMenu()
    {
        appliedMessageText.text = "";
    }
}
