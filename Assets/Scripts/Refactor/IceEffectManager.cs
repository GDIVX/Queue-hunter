using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class IceEffectManager : MonoBehaviour
{
    private VisualEffect iceEffect;
    bool hasPlayed;

    public UnityEvent onEffectFinishedPlaying;

    private void Start()
    {
        iceEffect = GetComponentInChildren<VisualEffect>();
    }

    void Update()
    {
        if (iceEffect.aliveParticleCount == 0 && hasPlayed)
        {
            SetAvailable();

            hasPlayed = false;
            return;
        }

        if (iceEffect.aliveParticleCount > 0)
        {
            hasPlayed = true;
        }
    }

    void SetAvailable()
    {
        onEffectFinishedPlaying?.Invoke();
    }    
}
