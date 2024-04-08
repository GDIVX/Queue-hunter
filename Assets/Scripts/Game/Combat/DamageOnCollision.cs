using System;
using System.Collections;
using System.Collections.Generic;
using Combat;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DamageOnCollision : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private string _lookupTag;
    [SerializeField] private float cooldown;
    public event Action<DamageOnCollision, Collider> OnCollision;

    private bool canDamage = true;

    public void Initialize(float damage, string lookupTag)
    {
        _damage = damage;
        _lookupTag = lookupTag;
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleDamage(other);
    }

    private void HandleDamage(Collider other)
    {
        if (!canDamage) return;

        if (!other.gameObject.CompareTag(_lookupTag))
        {
            return;
        }

        if (other.gameObject.TryGetComponent(out IDamageable hit))
        {
            hit.HandleDamage(_damage);
            if (cooldown > 0)
            {
                StartCoroutine(CooldownCoroutine());
            }
        }

        OnCollision?.Invoke(this, other);
    }

    private void OnTriggerStay(Collider other)
    {
        HandleDamage(other);
    }

    IEnumerator CooldownCoroutine()
    {
        canDamage = false;
        yield return new WaitForSeconds(cooldown);
        canDamage = true;
    }
}