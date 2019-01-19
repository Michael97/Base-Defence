using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceScript : MonoBehaviour
{

    public int ResourceCount;

    public ParticleSystem TreeParticleSystem;

    public void Hit()
    {
        TreeParticleSystem.Play();
    }

}
