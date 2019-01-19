using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;


public class SettingManager : MonoBehaviour
{
    public Toggle FullscreenToggle;

    public Dropdown ResolutionDropdown;
    public Dropdown TextureQualityDropdown;
    public Dropdown AntialisasingDropdown;
    public Dropdown VSyncDropdown;

    public Button SaveButton;
    public Button BackButton;

    public Slider MusicVolumeSlider;

    public AudioSource MusicSource;

    public Resolution[] Resolutions;

    public OptionsScript OptionsScript;

    private GameSettings gameSettings;

    private PausedMenu pausedMenu;

    private static SettingManager settingManager;

    private void Awake()
    {
        if (settingManager == null)
        {
            DontDestroyOnLoad(gameObject);
            settingManager = this;
        }
        else if (settingManager != this)
        {
            Destroy(gameObject);
        }
    }


    void OnEnable()
    {
        gameSettings = new GameSettings();
        
        FullscreenToggle.onValueChanged.AddListener(delegate { OnFullscreenToggle(); });
        ResolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionChange(); });
        TextureQualityDropdown.onValueChanged.AddListener(delegate { OnTextureQualityChange(); });
        AntialisasingDropdown.onValueChanged.AddListener(delegate { OnAntialiasingChange(); });
        VSyncDropdown.onValueChanged.AddListener(delegate { OnVSyncChange(); });
        MusicVolumeSlider.onValueChanged.AddListener(delegate { OnMusicVolumeChange(); });

        SaveButton.onClick.AddListener(delegate { OnSaveClick(); });
        BackButton.onClick.AddListener(delegate { OnBackClick(); });

        Resolutions = Screen.resolutions;

        foreach (Resolution resolution in Resolutions)
        {
            ResolutionDropdown.options.Add(new Dropdown.OptionData(resolution.ToString()));
        }

        LoadSettings();
    }

    public void OnFullscreenToggle()
    {
        gameSettings.Fullscreen = Screen.fullScreen = FullscreenToggle.isOn;
    }

    public void OnResolutionChange()
    {
        Screen.SetResolution(Resolutions[ResolutionDropdown.value].width, Resolutions[ResolutionDropdown.value].height, Screen.fullScreen);
        gameSettings.ResoultionIndex = ResolutionDropdown.value;
    }

    public void OnTextureQualityChange()
    {
        QualitySettings.masterTextureLimit = gameSettings.TextureQuality = TextureQualityDropdown.value;
    }

    public void OnAntialiasingChange()
    {
        QualitySettings.antiAliasing = gameSettings.Antialiasing = (int) Mathf.Pow(2.0f, AntialisasingDropdown.value);
    }

    public void OnVSyncChange()
    {
        QualitySettings.vSyncCount = gameSettings.VSync = VSyncDropdown.value;
    }

    public void OnMusicVolumeChange()
    {
        MusicSource.volume = gameSettings.MusicVolume = MusicVolumeSlider.value;
    }

    public void OnSaveClick()
    {
        SaveSettings();
        OptionsScript.ChangeActive();

        if (GameObject.FindGameObjectWithTag("PauseMenu"))
        {
            pausedMenu = GameObject.FindGameObjectWithTag("PauseMenu").GetComponent<PausedMenu>();
            pausedMenu.Child.SetActive(true);
        }
    }

    public void OnBackClick()
    {
        LoadSettings();
        OptionsScript.ChangeActive();

        if (GameObject.FindGameObjectWithTag("PauseMenu"))
        {
            pausedMenu = GameObject.FindGameObjectWithTag("PauseMenu").GetComponent<PausedMenu>();
            pausedMenu.Child.SetActive(true);
        }
    }

    public void SaveSettings()
    {
        string jsonData = JsonUtility.ToJson(gameSettings, true);
        File.WriteAllText(Application.persistentDataPath + "/gamesettings.json", jsonData);
    }

    public void LoadSettings()
    {
        //Grab the saved settings
        gameSettings = JsonUtility.FromJson<GameSettings>(File.ReadAllText(Application.persistentDataPath + "/gamesettings.json"));
        //Apply the saved values to the game
        MusicVolumeSlider.value = gameSettings.MusicVolume;
        AntialisasingDropdown.value = gameSettings.Antialiasing;
        VSyncDropdown.value = gameSettings.VSync;
        TextureQualityDropdown.value = gameSettings.TextureQuality;
        ResolutionDropdown.value = gameSettings.ResoultionIndex;
        FullscreenToggle.isOn = gameSettings.Fullscreen;
        Screen.fullScreen = gameSettings.Fullscreen;

        ResolutionDropdown.RefreshShownValue();
    }

}
