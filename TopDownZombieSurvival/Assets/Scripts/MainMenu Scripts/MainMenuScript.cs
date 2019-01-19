using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{

    public OptionsScript OptionsScript;

    public Button OptionsButton;
    public Button PlayButton;
    public Button ExitButton;

    void OnEnable()
    {
        OptionsScript = GameObject.FindGameObjectWithTag("OptionsMenu").GetComponent<OptionsScript>();

        OptionsButton.onClick.AddListener(delegate { OnOptionsClick(); });
        PlayButton.onClick.AddListener(delegate { OnPlayClick(); });
        ExitButton.onClick.AddListener(delegate { OnExitClick(); });
    }

    //Lodas the Endless Survival Mode
    public void OnPlayClick()
    {
        SceneManager.LoadScene("PlayLevel");
    }

    //When the options button is clicked, activate the options menu
    //, grab soundScript and call NewScene() and SetVolumeSlider()
    public void OnOptionsClick()
    {
        OptionsScript.ChangeActive();
    }

    //Exit the application
    public void OnExitClick()
    {
        Application.Quit();
    }

}
