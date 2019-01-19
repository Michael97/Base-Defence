using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveMenu : MonoBehaviour
{

    public Text txtWaveNumber;
    public Text txtTime;

    public void UpdateTime(float time)
    {
        txtTime.text = "Countdown " + Mathf.Round(time) + " seconds";
    }

    public void UpdateWave(int wave)
    {
        txtWaveNumber.text = wave.ToString();
    }

    public void OnNextWaveClick()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerScript>().DayNightCycleTime = 0.1f;
    }

}
