using System;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class BuildingStats : ObjectStats
    {
        public GameObject Prefab;

        public int WoodRepairCost;
        public int StoneRepairCost;
        public int BuildingLimit;
        public int Level = 1;

        public string Name;

        public ParticleSystem UpgradeParticle;
        public ParticleSystem SellParticle;
        public ParticleSystem RepairParticle;
        public ParticleSystem BuyParticle;

        private HealthScript healthScript;
        private GrabBuildingStats grabBuildingStats;

        public int GetWood()
        {
            return Wood;
        }

        public int GetStone()
        {
            return Stone;
        }

        public void SetWood(int wood)
        {
            int oldWood = Wood;

            Wood = wood;

            if (oldWood != Wood)
                CalculateRepairCosts();
        }

        public int GetSellWood()
        {
            return Wood / 2;
        }

        public int GetSellStone()
        {
            return Stone / 2;
        }

        public int GetUpgradeWood()
        {
            return Wood * 2;
        }

        public int GetUpgradeStone()
        {
            return Stone * 2;
        }

        public void BuildingUpgrade()
        {
            if (!healthScript)
                healthScript = GetComponent<HealthScript>();

            if (!grabBuildingStats)
                grabBuildingStats = GameObject.FindGameObjectWithTag("SellMenu").GetComponent<GrabBuildingStats>();

            Level++;
            Damage += Damage / 2;
            AttackCoolDown -= AttackCoolDown / 3;
            AttackRange += AttackRange / 4;
            healthScript.MaxHealth += healthScript.MaxHealth / 2;
            healthScript.HealthGenAmount += healthScript.HealthGenAmount / 2;

            Gold += Gold;
            Wood += Wood;
            Stone += Stone;

            CalculateRepairCosts();
            grabBuildingStats.SellMenuStats();
        }

        public void CalculateRepairCosts()
        {
            if (!healthScript)
                healthScript = GetComponent<HealthScript>();

            if (!grabBuildingStats)
                grabBuildingStats = GameObject.FindGameObjectWithTag("SellMenu").GetComponent<GrabBuildingStats>();

            float healthGainPerWood = healthScript.MaxHealth / Wood;
            float healthGainPerStone = healthScript.MaxHealth / Stone;

            float repairHealth = healthScript.MaxHealth - healthScript.CurrentHealth;

            WoodRepairCost = Mathf.RoundToInt(repairHealth / healthGainPerWood);
            StoneRepairCost = Mathf.RoundToInt(repairHealth / healthGainPerStone);

            if (!healthScript.IsAlive)
                Destroy(gameObject);

            grabBuildingStats.UpdateRepairCostAndHealth();

        }

    }
}
