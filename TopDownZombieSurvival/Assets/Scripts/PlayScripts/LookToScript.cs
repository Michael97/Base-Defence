using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class LookToScript : MonoBehaviour
{

    public Transform Target;
    public GameObject LookObject;
    public GameObject Projectile;

    private BuildingStats buildingStats;

    // Use this for initialization
    void Start ()
    {
        buildingStats = GetComponent<BuildingStats>();
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    // Update is called once per frame
    void FixedUpdate() {
	    lookDirection();
        buildingStats.AttackCoolDownTimer -= Time.deltaTime;
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Zombie");

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

        if (nearestEnemy != null && shortestDistance <= buildingStats.AttackRange)
        {
            Target = nearestEnemy.transform;
        }
        else
        {
            Target = null;
        }

    }

    private void lookDirection()
    {
        if (Target != null)
        {
            Quaternion transformRotationQuaternion =
                Quaternion.LookRotation(Target.position - LookObject.transform.position, Vector3.up);
            LookObject.transform.rotation = Quaternion.Slerp(transformRotationQuaternion, LookObject.transform.rotation, 0.8f);
            if (buildingStats.AttackCoolDownTimer <= 0.0f)
            {
                GameObject myProjectile = Instantiate(Projectile, transform.position, transform.rotation);
                myProjectile.GetComponent<AttackScript>().Target = Target;
                myProjectile.GetComponent<AttackScript>().Origin = transform.position;
                myProjectile.GetComponent<AttackScript>().Damage = buildingStats.Damage;
                buildingStats.AttackCoolDownTimer = buildingStats.AttackCoolDown;
            }
        }
    }
}
