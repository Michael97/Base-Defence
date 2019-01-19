using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public bool IsAlive;

    public float MaxHealth;
    public float CurrentHealth;
    public float HealthGenAmount;
    public float RegenTime;
    public float RegenCurrentTime;

    public DeadMenu DeadMenu;

    void Start()
    {
        DeadMenu = GameObject.FindGameObjectWithTag("DeadMenu").GetComponent<DeadMenu>();
    }

    void FixedUpdate()
    {
        HealthRegen();
    }



    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            IsAlive = false;
            if (tag == "Zombie")
                GetComponent<NormalZombieScript>().Dead();

            if (tag == "Player")
            {
                DeadMenu.Child.SetActive(true);
                GetComponent<PlayerStats>().SaveStats();
            }

            if (tag == "Cannon" || tag == "Wall" || tag == "Door" || tag == "Base")
                Destroy(gameObject);

         /*   if (tag == "Base")
            {
                //game over screen with stats and stuff
            }*/
        }
    }

    public void HealthRegen()
    {
        RegenCurrentTime -= Time.deltaTime;

        if (RegenCurrentTime <= 0 && CurrentHealth < MaxHealth)
        {
            CurrentHealth += HealthGenAmount;
            RegenCurrentTime = RegenTime;
            if (tag == "Player")
                GetComponent<PlayerStats>().UpdateHealth();
            else if (tag != "Player")
                GameObject.FindGameObjectWithTag("SellMenu").GetComponent<GrabBuildingStats>().UpdateRepairCostAndHealth();
        }
    }
}
