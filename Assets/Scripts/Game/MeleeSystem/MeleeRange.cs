using System.Collections.Generic;
using Game.MeleeSystem;
using UnityEngine;

public class MeleeRange : MonoBehaviour
{
    public static List<DamageReciver> EntityInRange;
    
    private void Awake()
    {
        EntityInRange = new List<DamageReciver>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Enemy"))
            if (other.gameObject.GetComponent<DamageReciver>() == null)
                return;

        Debug.Log("Add");
        EntityInRange.Add(other.GetComponent<DamageReciver>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Enemy"))
        {
            var damageReciver = other.gameObject.GetComponent<DamageReciver>();
            
            if (damageReciver != null)
            {
                if (!EntityInRange.Contains(damageReciver))
                    return;
                
            }
            else
                return;
        }
            

        Debug.Log("Remove");
        EntityInRange.Remove(other.GetComponent<DamageReciver>());
    }
}