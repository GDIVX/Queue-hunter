using System;
using Combat;
using Game.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileCollision : MonoBehaviour
{
    [SerializeField] GameObject effectObject;
    [SerializeField] public GameObject? explosionObject;
    public UnityEvent onEffectRequest;
    public UnityEvent onEffectCollision;
    [SerializeField] private float destroyTime;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] float resetTime;

    private void OnEnable()
    {
        StartCoroutine(KillProjectile());
        effectObject?.SetActive(true);
    }

    private void OnValidate()
    {
        if (explosionObject == null)
        {
            throw new Exception(
                $"{gameObject} has not explosion object assigned. Please look at the prefab for ProjectileCollision/explosionObject ");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (/*!other.CompareTag("Player") && !other.CompareTag("Vision") && !other.CompareTag("Ground")*/ /*other.CompareTag(targetLayer.ToString())*/ other.gameObject.layer == targetLayer)
        {
            var point = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            if (explosionObject != null)
            {
                explosionObject.transform.position = point;
            }
            StartCoroutine(ManageActiveness());
        }
    }

    protected IEnumerator ManageActiveness()
    {
        if (TryGetComponent<Projectile>(out var proj))
        {
            onEffectCollision?.Invoke();
            if (explosionObject != null)
            {
                explosionObject.SetActive(true);
            }
            effectObject.SetActive(false);

            yield return new WaitForSeconds(resetTime);

            if (explosionObject != null)
            {
                explosionObject?.SetActive(false);
            }

            proj.SetAvailable(true);
            gameObject.SetActive(false);
        }
    }

    private IEnumerator KillProjectile()
    {
        yield return new WaitForSeconds(destroyTime);
        if (!gameObject.activeSelf) yield break;
        if (TryGetComponent<Projectile>(out var proj))
        {
            effectObject.SetActive(false);
            if (explosionObject != null)
            {
                explosionObject?.SetActive(false);
            }
            proj.SetAvailable(true);
        }
        gameObject.SetActive(false);
    }



}
