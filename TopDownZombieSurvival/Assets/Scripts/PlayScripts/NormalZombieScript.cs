using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.AI;

public class NormalZombieScript : MonoBehaviour {

    public float AttackRange;
    public float AttackCoolDown;
    public float ChaseRange;

    public GameObject Target;
    public NavMeshAgent NavMeshAgent;

    public Vector3 NewPos;

    public GameObject Grid;

    private PlayerStats playerStats;
    private ZombieStats zombieStats;
    private HealthScript zombieHealth;

    private float moveTime = 0.1f;
    private float updateTargetTime = 0.5f;

    private ArrayList enemies;


    void Start () {
        zombieStats = GetComponent<ZombieStats>();
        zombieHealth = GetComponent<HealthScript>();
    }

    void FixedUpdate()
    {
        moveTime -= Time.deltaTime;
        updateTargetTime -= Time.deltaTime;

        if (updateTargetTime <= 0.0f)
        {
            UpdateTarget();
            updateTargetTime = 1.0f;
        }

        if (moveTime <= 0.0f)
        {
            MoveToTarget();
            moveTime = 0.1f;
        }

        AttackCoolDown -= Time.deltaTime;
    }

    public void UpdateTarget()
    {
        GetTargets();

        float shortestDistance = Mathf.Infinity;

        GameObject nearestEnemy = null;

        NavMeshPath navMeshPath = new NavMeshPath();
        if (Target)
            Debug.Log(NavMeshAgent.CalculatePath(Target.transform.position, navMeshPath));

        if (Target)
            for (int i = 0; i < navMeshPath.corners.Length-1; i++)
                Debug.DrawLine(navMeshPath.corners[i], navMeshPath.corners[i+1], Color.red);

        foreach (GameObject enemy in enemies)
        {

            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            int aggroRank = enemy.GetComponent<BuildingStats>().AggroRank;

            if (nearestEnemy != null)
            {
                Debug.Log("1");
                if (aggroRank < nearestEnemy.GetComponent<BuildingStats>().AggroRank)
                {
                    Debug.Log("2");
                    if (NavMesh.CalculatePath(transform.position, enemy.transform.position, NavMesh.AllAreas, navMeshPath))
                    {
                        Debug.Log("3");
                        nearestEnemy = enemy;
                        Target = nearestEnemy;
                    }
                    else
                        return;
                }
                Debug.Log("4");
                if (aggroRank == nearestEnemy.GetComponent<BuildingStats>().AggroRank)
                {
                    Debug.Log("5");
                    if (NavMesh.CalculatePath(transform.position, enemy.transform.position, NavMesh.AllAreas, navMeshPath))
                    {
                        Debug.Log("6");
                        if (distanceToEnemy < Vector3.Distance(transform.position, nearestEnemy.transform.position))
                        {
                        nearestEnemy = enemy;
                        Target = nearestEnemy;
                        }
                    else
                        return;
                    }
                else
                    return;
                }
            }
            else
            {
                Debug.Log("7");
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
                Target = nearestEnemy;
            }
        }
            

            //if aggrorank is lower currentTarget.aggrorank
                //if path is avaliable
                    //set enemy as the target
                //else
                    //next enemy in list
            //if aggrorank is the same
                //if path is avaliable
                    //if enemy.distance is less than currentTarget.distance
                        //set enemy as the target
                //else
                    //next enemy in list
            //else
                //next enemy in list

           /* if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }*/
      //  }

        //If there is a near enemy, and it is within the chase range
        /*if (nearestEnemy != null && nearestEnemy != Target)
        {
            //Target it and eat its brains
            Target = nearestEnemy;
        }*/
        //Target it and eat its brains
    }

    public void GetTargets()
    {
        if (!Grid)
            Grid = GameObject.FindGameObjectWithTag("Grid");

        enemies = new ArrayList();
        //enemies.AddRange(GameObject.FindGameObjectsWithTag("Player"));

        GameObject[] buildings = new GameObject[Grid.transform.childCount];

        for (int i = 0; i < Grid.transform.childCount; i++)
        {
            buildings[i] = Grid.transform.GetChild(i).gameObject;
        }

        enemies.AddRange(buildings);
    }

    public void AttackTarget()
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
    }

    public void MoveToTarget()
    {
        if (Target)
        {
            NavMeshAgent.SetDestination(Target.transform.position);
            AttackTarget();
        }
        else if (!Target)
            UpdateTarget();
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

}
