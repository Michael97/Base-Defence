using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class AttackScript : MonoBehaviour {

    public Transform Target;
    public Vector3 Origin;
    public float Speed;
    public float Damage;
    public GameObject ImpactEffect;

    // Update is called once per frame

    void Start()
    {
        transform.position = Origin;
    }

    void FixedUpdate()
    {
        if (Target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, Speed);
            transform.rotation = Quaternion.LookRotation(Target.transform.position - transform.position, Vector3.up);

            if (transform.position == Target.transform.position)
            {
                GameObject effectLifeTime = Instantiate(ImpactEffect, transform.position, transform.rotation);
                Destroy(effectLifeTime, 0.5f);
                Destroy(gameObject);
                Target.GetComponent<HealthScript>().TakeDamage(Damage);
            }
        }
        else if (Target == null)
            Destroy(gameObject);
    }
}
