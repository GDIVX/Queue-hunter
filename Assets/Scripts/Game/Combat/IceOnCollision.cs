using Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceOnCollision : MonoBehaviour
{
    Collider col;
    [SerializeField] float waitTime;
    [SerializeField] float damage;

    [SerializeField] List<IDamageable> enemiesInRange = new List<IDamageable>();

    private void Start()
    {
        col = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        StartCoroutine(InflictDamageCoroutine());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out var enemy) && other.CompareTag("Enemy"))
        {
            enemiesInRange.Add(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (enemiesInRange.Contains(other as IDamageable))
        {
            enemiesInRange.Remove(other as IDamageable);
        }
    }

    void InflictDamage()
    {
        foreach (var item in enemiesInRange)
        {
            item.HandleDamage(damage);
        }
    }

    private IEnumerator InflictDamageCoroutine()
    {
        yield return new WaitForSeconds(waitTime);
        InflictDamage();
    }
}
