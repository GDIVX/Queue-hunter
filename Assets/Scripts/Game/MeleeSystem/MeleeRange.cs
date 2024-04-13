using System.Collections.Generic;
using Combat;
using Game.MeleeSystem;
using Sirenix.OdinInspector;
using UnityEngine;

public class MeleeRange : MonoBehaviour
{
    public static List<IDamageable> EntityInRange;
    [SerializeField] private int damage;

    public int Damage
    {
        get => damage;
        private set => damage = value;
    }

    private void Awake()
    {
        EntityInRange = new List<IDamageable>();
    }

    [Button]
    public void DoDamage()
    {
        foreach (IDamageable target in EntityInRange)
        {
            target.HandleDamage(Damage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy")) return;
        if (other.TryGetComponent(out IDamageable target))
        {
            EntityInRange.Add(target);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy")) return;

        if (other.TryGetComponent(out IDamageable target))
        {
            EntityInRange.Remove(target);
        }
    }
}