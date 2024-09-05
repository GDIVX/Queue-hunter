using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EffectManager : MonoBehaviour
{
    [SerializeField] GameObject effect;
    [SerializeField] float effectDuration;
    bool isActive = false;

    public void ActivateEffect()
    {
        StartCoroutine(EffectCoroutine());
    }

    private IEnumerator EffectCoroutine()
    {
        if (!isActive)
        {
            effect.gameObject.SetActive(true);
            isActive = true;
            yield return new WaitForSeconds(effectDuration);
            effect.gameObject.SetActive(false);
            isActive = false;
        }

        else
        {
            effect.gameObject.SetActive(false);
            isActive = false;
            StartCoroutine(EffectCoroutine());
        }
    }
}
