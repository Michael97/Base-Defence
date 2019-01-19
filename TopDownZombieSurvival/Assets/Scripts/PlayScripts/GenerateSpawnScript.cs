using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.AI;

public class GenerateSpawnScript : MonoBehaviour {

    private Vector3 newPos;
    public GameObject Terrain;
    public GameObject baseObject;

    void Start()
    {
        float sphereSize = Terrain.transform.localScale.x / 2;

        if (gameObject.tag == "ZombieSpawner")
        {
            baseObject = GameObject.FindGameObjectWithTag("Base");
            newPos = new Vector3(baseObject.transform.position.x, baseObject.transform.position.y, baseObject.transform.position.z);
            float ang = Random.value * 360;
            newPos.x += Terrain.transform.localScale.x / 3f * Mathf.Sin(ang * Mathf.Deg2Rad);
            newPos.z += Terrain.transform.localScale.x / 3f * Mathf.Cos(ang * Mathf.Deg2Rad);
        }
        else
            newPos = RandomNavSphere(new Vector3(0,0,0), sphereSize, -1);

        transform.position = newPos;
        float y = Random.Range(0.0f, 360.0f);
        transform.Rotate(0, y, 0);
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }
}
