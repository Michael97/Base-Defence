using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class GrabBuildingStats : MonoBehaviour
{
    public GameObject BuildingPrefab;
    
    //scripts
    private BuildingStats buildingStats;
    private HealthScript healthScript;
    private GrabPlayerStats grabPlayerStats;

    public GridSystem GridSystem;

    public PlayerStats playerStats;

    public GameObject Child;

    public Text txtName;
    public Text txtHealth;
    public Text txtDamage;
    public Text txtRepairPrice;
    public Text txtSellPrice;
    public Text txtUpgradePrice;

    void Start()
    {
        grabPlayerStats = GameObject.FindGameObjectWithTag("PlayerStats").GetComponent<GrabPlayerStats>();
        NewPlayer();
        Child.SetActive(false);
    }

    void NewPlayer()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
            playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }

    public void SellMenuStats()
    {
        if (!playerStats)
            NewPlayer();

        buildingStats = BuildingPrefab.GetComponent<BuildingStats>();
        healthScript = BuildingPrefab.GetComponent<HealthScript>();
        txtName.text = buildingStats.Name + " Level " + buildingStats.Level;
        txtDamage.text = "Damage: " + buildingStats.Damage;
        txtUpgradePrice.text = "Upgrade: Wood: " + buildingStats.GetUpgradeWood() + ", Stone: " +
                               buildingStats.GetUpgradeStone();
        UpdateRepairCostAndHealth();
    }

    public void UpdateRepairCostAndHealth()
    {
        if (!playerStats)
            NewPlayer();
        //updates the health and repair after its been attacked
        txtHealth.text = "Health: " + healthScript.CurrentHealth + " / " + healthScript.MaxHealth;

        txtRepairPrice.text = "Repair: Wood: " + buildingStats.WoodRepairCost + ", Stone: " +
                              buildingStats.StoneRepairCost;

        txtSellPrice.text = "Sell: Wood " + buildingStats.GetSellWood() + ", Stone " +
                            buildingStats.GetSellStone();

    }

    public void UpgradeButton()
    {
        if (!playerStats)
            NewPlayer();

        if (playerStats == null)
            return;

        if (playerStats.Wood - buildingStats.GetUpgradeWood() >= 0 &&
            playerStats.Stone - buildingStats.GetUpgradeStone() >= 0)
        {
            playerStats.Wood -= buildingStats.GetUpgradeWood();
            playerStats.Stone -= buildingStats.GetUpgradeStone();
            grabPlayerStats.BuildingBoughtSoldRepairedUpgraded();
            buildingStats.CalculateRepairCosts();
            UpdateRepairCostAndHealth();
            buildingStats.BuildingUpgrade();
        }
    }

    public void SellButton()
    {
        if (!playerStats)
            NewPlayer();

        //If the player is dead dont sell
        if (playerStats == null)
            return;

        playerStats.Wood += buildingStats.Wood / 2;
        playerStats.Stone += buildingStats.Stone / 2;

        Destroy(BuildingPrefab);

        grabPlayerStats.BuildingBoughtSoldRepairedUpgraded();

        Child.SetActive(false);
    }

    public void RepairButton()
    {
        if (!playerStats)
            NewPlayer();

        //if the player is dead dont repair
        if (playerStats == null)
            return;

        if (playerStats.Wood - buildingStats.WoodRepairCost >= 0 &&
            playerStats.Stone - buildingStats.StoneRepairCost >= 0)
        {
            playerStats.Wood -= buildingStats.WoodRepairCost;
            playerStats.Stone -= buildingStats.StoneRepairCost;
            healthScript.CurrentHealth = healthScript.MaxHealth;
            grabPlayerStats.BuildingBoughtSoldRepairedUpgraded();
            buildingStats.CalculateRepairCosts();
            UpdateRepairCostAndHealth();
        }
    }

}
