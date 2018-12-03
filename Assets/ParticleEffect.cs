using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleEffect : MonoBehaviour
{
    private ParticleSystem _particles;

    void Awake()
    {
        _particles = GetComponent<ParticleSystem>();
    }

    public void PlayParticleEffect()
    {
        _particles?.Play();
    }
}
