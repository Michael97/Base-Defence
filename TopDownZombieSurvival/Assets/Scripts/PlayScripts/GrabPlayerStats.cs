using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;

public class GrabPlayerStats : MonoBehaviour
{

    public Text txtHealth;

    public Text txtGold;
    public Text txtWood;
    public Text txtStone;
    public Text txtScore;

    public PlayerStats playerStats;
    public HealthScript healthScript;

    public GameObject GameController;

    public void NewPlayer()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        healthScript = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthScript>();
        UpdateStats();
    }
	
	public void UpdateStats ()
	{
	    UpdateHealth();
        KilledZombie();
	}

    public void UpdateHealth()
    {
       // txtHealth.text = "Health: " + healthScript.CurrentHealth + " / " + healthScript.MaxHealth;
    }

    public void KilledZombie()
    {
        txtGold.text = "Gold: " + playerStats.Gold;
        txtScore.text = "Score: " + playerStats.Score;
        UpdateWood();
        UpdateStone();
    }

    public void BuildingBoughtSoldRepairedUpgraded()
    {
        UpdateStone();
        UpdateWood();
    }

    public void UpdateWood()
    {
        txtWood.text = "Wood: " + playerStats.Wood;
    }

    public void UpdateStone()
    {
        txtStone.text = "Stone: " + playerStats.Stone;
    }

}
