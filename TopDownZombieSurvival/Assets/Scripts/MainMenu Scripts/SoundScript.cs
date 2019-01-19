using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundScript : MonoBehaviour {

    public static float SavedVolume = 0.5f;

    public Slider VolumeSlider;
    public AudioSource VolumeAudio;

    void Start()
    {
        NewScene();
    }

    //When the options menu is called, grab these components inside it
    public void NewScene()
    {
        VolumeAudio = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>();

        if (GameObject.FindGameObjectWithTag("VolumeSlider"))
            VolumeSlider = GameObject.FindGameObjectWithTag("VolumeSlider").GetComponent<Slider>();
    }

    //Called to change the ingame music level
    public void UpdateGameVolume()
    {
        if (VolumeAudio)
        {
            VolumeAudio.volume = SavedVolume;
            VolumeSlider.onValueChanged.AddListener(ListenerMethod);
        }
    }

    //Calls this when the volume slider is adjusted
    public void UpdateSavedVolume()
    {
        if (VolumeSlider)
        {
            SavedVolume = VolumeSlider.value;
            UpdateGameVolume();
        }
    }

    //Sets the volume slider to the current volume level
    public void SetVolumeSlider()
    {
        if (VolumeSlider)
            VolumeSlider.value = SavedVolume;
    }

    public void ListenerMethod(float value)
    {
        if (VolumeSlider)
            SavedVolume = VolumeSlider.value;
    }

}
