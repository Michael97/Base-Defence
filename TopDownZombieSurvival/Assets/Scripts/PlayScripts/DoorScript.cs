using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    IgnoreCollsion();
	}

    public void IgnoreCollsion()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>());
    }

}
