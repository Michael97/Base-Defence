using System.Collections;
using System.Collections.Generic;
using Boo.Lang;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PausedMenu : MonoBehaviour
{

    public GameObject Child;
    public GameObject ControlsChild;
    public GameObject Options;

    public Button ControlsButtonBack;

    public bool paused = true;

    public Button ResumeButton;
    public Button ControlsButton;
    public Button OptionsButton;
    public Button ExitButton;
    public Button PauseButton;

    void Start()
    {
        Time.timeScale = 1;
        Child.SetActive(false);
        ControlsChild.SetActive(false);
    }

    void OnEnable()
    {
        PauseButton.onClick.AddListener(delegate { isPaused(); });
        ResumeButton.onClick.AddListener(delegate { OnResumeClick(); });
        ControlsButton.onClick.AddListener(delegate { OnControlsClick(); });
        OptionsButton.onClick.AddListener(delegate { OnOptionsClick(); });
        ExitButton.onClick.AddListener(delegate { OnExitClick(); });
        ControlsButtonBack.onClick.AddListener(delegate { OnControlsBackClick(); });
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            isPaused();
    }

    private void isPaused()
    {
        paused = !paused;
        Child.SetActive(paused);
        if (paused)
            Time.timeScale = 0;
        if (!paused)
            Time.timeScale = 1;
    }

    public void OnResumeClick()
    {
        isPaused();
    }

    public void OnControlsClick()
    {
        ControlsChild.SetActive(true);
    }

    public void OnOptionsClick()
    {
        if (!Options)
            Options = GameObject.FindGameObjectWithTag("OptionsMenu");

        Child.SetActive(false);
        Options.GetComponent<OptionsScript>().ChangeActive();
    }

    public void OnExitClick()
    {
        SceneManager.LoadScene("MainMenu");
    }

    //Controls Menu
    public void OnControlsBackClick()
    {
        ControlsChild.SetActive(false);
    }


}
