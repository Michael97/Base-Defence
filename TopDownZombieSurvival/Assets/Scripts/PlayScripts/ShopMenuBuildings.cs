using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenuBuildings : MonoBehaviour {

    public GameObject BuildingPrefab;

    private BuildingStats buildingStats;
    private HealthScript healthScript;

    public GameObject Child;

  //  public Text txtName;
  //  public Text txtHealth;
  //  public Text txtDamage;
    public Text txtCost;
    //  public Text txtWoodCost;
    //public Text txtStoneCost;

    void Start()
    {
        Child.SetActive(false);
    }

    public void ShowStats()
    {
        buildingStats = BuildingPrefab.GetComponent<BuildingStats>();
      //  healthScript = BuildingPrefab.GetComponent<HealthScript>();
     //   txtName.text = buildingStats.Name;
     //   txtDamage.text = "Damage: " + buildingStats.Damage;
    //    txtHealth.text = "Health: " + healthScript.MaxHealth;
        txtCost.text = buildingStats.Wood + " Wood, " + buildingStats.Stone + " Stone";
        //txtStoneCost.text = "Stone: " + buildingStats.Stone;
    }
}
