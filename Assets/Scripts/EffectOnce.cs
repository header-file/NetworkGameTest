using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectOnce : MonoBehaviour
{
    public ParticleSystem PS;


    void Update()
    {
        if (!PS.isPlaying)
            gameObject.SetActive(false);
    }

    void OnEnable()
    {
        PS.Play();
    }
}
