using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Partical : MonoBehaviour
{
    public ParticleSystem[] particleSystems;

    void Start()
    {
        foreach (ParticleSystem ps in particleSystems)
        {
            if (!ps.isPlaying)
            {
                ps.Play();
            }
        }
    }
}
