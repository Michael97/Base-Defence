using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

namespace Assets.Scripts
{
    public class ZombieStats : ObjectStats
    {
        public NavMeshAgent NavMeshAgent;
        public int Score;
        public float Speed = 2.0f;
        public int Health = 50;
        public float Loot = 5;

        void Start()
        {
            //Grab Necassary Components
            NavMeshAgent = GetComponent<NavMeshAgent>();

            HealthScript healthScript = GetComponent<HealthScript>();

            GameControllerScript GameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerScript>();
            
            //Set Speed of zombie
            Speed += GameController.WaveNumber / 5.0f; 
            NavMeshAgent.speed = Speed;

            //Set Damage of zombie
            Damage += GameController.WaveNumber * 2;

            //Set Health of zombie
            healthScript.MaxHealth = Health += GameController.WaveNumber * 10;
            healthScript.CurrentHealth = healthScript.MaxHealth;

            //Set the Amount of loot per zombie
            Loot = Mathf.Round(Loot + Mathf.Sqrt(GameController.WaveNumber));

            Gold = Mathf.RoundToInt(Loot);
            Stone = Mathf.RoundToInt(Loot);
            Wood = Mathf.RoundToInt(Loot);
        }

        public void IsDead()
        {
            GetComponent<ZombieWanderScript>().Dead();
        }
    }
}
