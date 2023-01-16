using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    // make settings save-able

    private Resolution[] resolutions;
    [SerializeField] private Dropdown graphicsDropdown;
    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Toggle cameraShakeToggle;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider volumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> resolutionOptions = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            resolutionOptions.Add(resolutions[i].width + " x " + resolutions[i].height);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        Debug.Log(PlayerPrefs.GetInt("resolution"));

        // load settings
        LoadSettings();
    }

    public void SetGraphics(int graphicsIndex)
    {
        QualitySettings.SetQualityLevel(graphicsIndex);
        graphicsDropdown.value = graphicsIndex;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        resolutionDropdown.value = resolutionIndex;
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        fullscreenToggle.isOn = isFullscreen;
    }

    public void SetCameraShake(bool isShaking)
    {
        cameraShakeToggle.isOn = isShaking;
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void ChangeVolume(float volume)
    {
        volumeSlider.value += volume;
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("graphics", graphicsDropdown.value);
        PlayerPrefs.SetInt("resolution", resolutionDropdown.value);
        PlayerPrefs.SetInt("resolutionChosen", 1);
        PlayerPrefs.SetInt("fullscreen", BoolToInt(fullscreenToggle.isOn));
        //PlayerPrefs.SetInt("cameraShake", BoolToInt(cameraShakeToggle.isOn));
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
    }

    private void LoadSettings()
    {
        SetGraphics(PlayerPrefs.GetInt("graphics"));

        if (PlayerPrefs.GetInt("resolutionChosen") == 1)
        {
            SetResolution(PlayerPrefs.GetInt("resolution"));
            SetFullscreen(IntToBool(PlayerPrefs.GetInt("fullscreen")));
        }
        else
        {
            SetResolution(resolutions.Length - 1);
            SetFullscreen(true);
        }

        //SetCameraShake(IntToBool());
        ChangeVolume(PlayerPrefs.GetFloat("volume"));
    }

    private bool IntToBool(int value)
    {
        if (value > 0)
        {
            return true;
        }

        return false;
    }

    private int BoolToInt(bool value)
    {
        if (value)
        {
            return 1;
        }

        return 0;
    }
}
