using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider fpsSlider;
    public Text fpsValueText;
    public Slider volumeSlider;
    public Text volumeValueText;
    public Text appliedMessageText;

    private float appliedFPS;
    private float appliedVolume;

    void Start()
    { 
        fpsSlider.onValueChanged.AddListener(UpdateFPSValue);
        volumeSlider.onValueChanged.AddListener(UpdateVolumeValue);

        appliedFPS = PlayerPrefs.GetInt("FPS", 60);
        appliedVolume = PlayerPrefs.GetInt("Volume", 100);

        UpdateFPSValue(fpsSlider.value);
        UpdateVolumeValue(volumeSlider.value);

        appliedMessageText.text = "";
    }

    public void UpdateFPSValue(float value)
    {
        fpsValueText.text = value.ToString();
    }

    public void UpdateVolumeValue(float value)
    {
        volumeValueText.text = value.ToString();
        AudioListener.volume = value;
    }

    public void ApplySettings()
    {
        appliedFPS = fpsSlider.value;
        appliedVolume = volumeSlider.value;

        PlayerPrefs.SetFloat("FPS", appliedFPS);
        PlayerPrefs.SetFloat("Volume", appliedVolume);
        PlayerPrefs.Save();

        Application.targetFrameRate = (int)appliedFPS;

        appliedMessageText.text = "Settings applied";
    }

    public void BackToMainMenu()
    {
        appliedMessageText.text = "";
    }
}
