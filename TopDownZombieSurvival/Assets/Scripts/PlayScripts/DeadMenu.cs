using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class DeadMenu : MonoBehaviour
{

    private GameControllerScript gameControllerScript;
    private GrabPlayerStats grabPlayerStats;

    public GameObject Child;

	void Start ()
	{
	    gameControllerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerScript>();
	    grabPlayerStats = GameObject.FindGameObjectWithTag("PlayerStats").GetComponent<GrabPlayerStats>();
	    Child.SetActive(false);
    }

    public void OnRespawnClick()
    {
        //Spawn Player
        gameControllerScript.RespawnPlayer();
        //Grab new players stats
        grabPlayerStats.NewPlayer();
        //Deactivate the dead menu
        Child.SetActive(false);
    }
}
