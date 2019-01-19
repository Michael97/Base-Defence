using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerStats : ObjectStats
    {
        public int Score;
        public int HarvestAmount;

        public GrabPlayerStats grabPlayerStats;

        void Start()
        {
            grabPlayerStats = GameObject.FindGameObjectWithTag("PlayerStats").GetComponent<GrabPlayerStats>();
        }

        public void UpdateHealth()
        {
           grabPlayerStats.UpdateHealth();
        }

        public void KilledZombie()
        {
            grabPlayerStats.KilledZombie();
        }

        public void SaveStats()
        {
            //use playerprefs to save the stats
            Destroy(gameObject);
        }
    }
}
