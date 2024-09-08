using System;

namespace Combat
{
    public interface IDamageable : IDestroyable
    {
        void HandleDamage(float damage);
        bool CanBeDamaged { get; }
        event Action<float, IDamageable> OnUpdateValue;
    }
}