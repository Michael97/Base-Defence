using System;
using System.Collections;
using System.Runtime.Remoting.Channels;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class ZombieWanderScript : MonoBehaviour {

        public float WanderRadius;
        public float WanderTimer;

        public float AttackRange;
        public float AttackCoolDown;
        public float ChaseRange;

        private float WanderCurrentTimer;

        public GameObject Grid;

        public GameObject Target;
        public NavMeshAgent NavMeshAgent;
        
        public Vector3 NewPos;

        private PlayerStats playerStats;
        private ZombieStats zombieStats;
        private HealthScript zombieHealth;

        public enum ZombieState
        {
            Wandering,
            Chasing,
            Attacking
        }

        public ZombieState EZombieState;

        void Start()
        {
            WanderCurrentTimer = WanderTimer;
            EZombieState = ZombieState.Wandering;
            StartCoroutine(Alive());

            Grid = GameObject.FindGameObjectWithTag("Grid");
            

            zombieStats = GetComponent<ZombieStats>();
            zombieHealth = GetComponent<HealthScript>();

            InvokeRepeating("UpdateTarget", 0f, 0.5f);
        }

        void FixedUpdate()
        {
            AttackCoolDown -= Time.deltaTime;
            WanderCurrentTimer += Time.deltaTime;
        }

        public void Dead()
        {
            if (GameObject.FindGameObjectWithTag("Player"))
            {
                playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
                playerStats.Score += zombieStats.Score;
                playerStats.Gold += zombieStats.Gold;
                playerStats.Wood += zombieStats.Wood;
                playerStats.Stone += zombieStats.Stone;
                playerStats.KilledZombie();
            }

            Destroy(gameObject);
        }


        IEnumerator Alive()
        {
            while (true)
            { 
                switch (EZombieState)
                {
                    case ZombieState.Wandering:

                        //if the zombie has a target
                        if (Target != null)
                        {
                            //if the target is within range
                            if (Vector3.Distance(Target.transform.position, transform.position) <= ChaseRange)
                            {
                                EZombieState = ZombieState.Chasing;
                            }
                        }
                        if (WanderCurrentTimer >= WanderTimer)
                        {
                            NewDestination();
                        }
                        //If at the destination
                        else if (Vector3.Distance(NewPos, transform.position) <= 0.3f)
                        {
                            NewDestination();
                        }
                        else
                            NavMeshAgent.SetDestination(NewPos);
                        break;
                    case ZombieState.Attacking:
                        if (Target != null)
                        {
                            if (Vector3.Distance(Target.transform.position, transform.position) <= AttackRange)
                            {
                                Quaternion.LookRotation(Target.transform.position - transform.position, Vector3.up);
                                if (AttackCoolDown <= 0.0f)
                                {
                                    Target.GetComponent<HealthScript>().TakeDamage(zombieStats.Damage);
                                    if (Target.tag != "Player")
                                        Target.GetComponent<BuildingStats>().CalculateRepairCosts();
                                    else if (Target.tag == "Player")
                                    {
                                        Target.GetComponent<PlayerStats>().UpdateHealth();
                                    }
                                    AttackCoolDown = 0.5f;
                                }
                            }
                            else if (Vector3.Distance(Target.transform.position, transform.position) >= AttackRange)
                            {
                                EZombieState = ZombieState.Chasing;
                            }
                        }
                        else if (Target == null)
                        {
                            NewDestination();
                            EZombieState = ZombieState.Wandering;
                        }
                        break;
                    case ZombieState.Chasing:
                        //If there is a target
                        if (Target != null)
                        {
                            if (Vector3.Distance(Target.transform.position, transform.position) <= ChaseRange)
                            {
                                NavMeshAgent.SetDestination(Target.transform.position);
                                EZombieState = ZombieState.Attacking;
                            }
                            else if (Vector3.Distance(Target.transform.position, transform.position) >= ChaseRange)
                            {
                                NewDestination();
                                EZombieState = ZombieState.Wandering;
                            }
                        }
                        else
                        {
                            NewDestination();
                            EZombieState = ZombieState.Wandering;
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                yield return new WaitForSeconds(0.1f);
            }
            yield return null;
        }

        private void NewDestination()
        {
            NewPos = RandomNavSphere(transform.position, WanderRadius, -1);
            NavMeshAgent.SetDestination(NewPos);
            WanderCurrentTimer = WanderTimer;
        }

        public void UpdateTarget()
        {
            ArrayList enemies = new ArrayList();

            enemies.AddRange(GameObject.FindGameObjectsWithTag("Player"));

            GameObject[] buildings = new GameObject[Grid.transform.childCount];
            for (int i = 0; i < Grid.transform.childCount; i++)
            {
                buildings[i] = Grid.transform.GetChild(i).gameObject;
            }

            enemies.AddRange(buildings);

            float shortestDistance = Mathf.Infinity;
            GameObject nearestEnemy = null;
            foreach (GameObject enemy in enemies)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
            }

            if (nearestEnemy != null && shortestDistance <= ChaseRange)
            {
                Target = nearestEnemy;
            }
            else
            {
                Target = null;
            }

        }

        public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
        {
            Vector3 randDirection = Random.insideUnitSphere * dist;

            randDirection += new Vector3(0,0,0);

            NavMeshHit navHit;

            NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

            return navHit.position;
        }
    }
}
