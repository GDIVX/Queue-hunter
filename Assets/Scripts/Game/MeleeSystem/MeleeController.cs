using System.Collections.Generic;
using Game.MeleeSystem;
using UnityEngine;

public class MeleeController : MonoBehaviour
{
    [SerializeField] private int _damage;
    
    private List<DamageReciver> _entityInRange;

    private void Awake()
    {
        _entityInRange = new List<DamageReciver>();
    }

    public void Attack()
    {
        foreach (var damageReciver in _entityInRange)
            damageReciver.TakeDamage(_damage);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Enemy") || _entityInRange.Contains(other.GetComponent<DamageReciver>()))
        {
            return;
        }
        
        _entityInRange.Add(other.GetComponent<DamageReciver>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Enemy"))
        {
            return;
        }
        
        _entityInRange.Remove(other.GetComponent<DamageReciver>());
    }
}
