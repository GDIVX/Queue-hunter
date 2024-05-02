using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightningManager : MonoBehaviour
{
    ChainLightning[] lightningsScripts;
    [SerializeField] GameObject[] lightnings;
    [SerializeField] float chainCD;
    public void InitChainLightning()
    {
        foreach (var chain in lightnings)
        {
            if (chain.TryGetComponent<ChainLightning>(out var light))
            {
                StartCoroutine(ExpandCooldown(light));
            }
        }
    }

    IEnumerator ExpandCooldown(ChainLightning lightning)
    {
        yield return new WaitForSeconds(chainCD);
        lightning.Expand();
    }
}
