using UnityEngine;

public class MeleeController : MonoBehaviour
{
    [SerializeField] private int _damage;
    
    public void Attack()
    {
        foreach (var damageReciver in MeleeRange.EntityInRange)
            damageReciver.TakeDamage(_damage);
    }

   
}
