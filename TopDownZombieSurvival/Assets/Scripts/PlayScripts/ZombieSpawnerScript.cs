using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.AI;

public class ZombieSpawnerScript : MonoBehaviour
{

    public GameObject Zombie;
    public GameObject Base;

    public float SpawnAmount;

    public GameControllerScript GameController;

    void Start()
    {
        GameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerScript>();
        Base = GameObject.FindGameObjectWithTag("Base");
    }

    public void UpdateSpawnAmount()
    {
        //Capping the maximum amount of zombies at 100 (10 spawners)
        if (GameController.WaveNumber < 10)
            SpawnAmount = GameController.WaveNumber;
        else
            SpawnAmount = 10;
    }

    public void SpawnZombie()
    {
        for (int i = 0; i < SpawnAmount; i++)
        { 
            GameObject myZombie = Instantiate(Zombie, transform.position, transform.rotation);

            NormalZombieScript normalZombieScript = myZombie.GetComponent<NormalZombieScript>();
            normalZombieScript.enabled = true;
            normalZombieScript.UpdateTarget();
            normalZombieScript.MoveToTarget();

            myZombie.GetComponent<NavMeshAgent>().enabled = true;
            myZombie.GetComponent<ZombieStats>().enabled = true;
            myZombie.GetComponent<HealthScript>().enabled = true;
            
        }
    }

}
