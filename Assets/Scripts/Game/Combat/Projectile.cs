using Game.Combat;
using UnityEngine;
using Utility;

namespace Combat
{
    public class Projectile : MonoBehaviour, IPoolable
    {
        public bool IsActive { get; set; }
        [SerializeField] private Type type;

        public ProjectileMovement Movement { get; private set; }
        public DamageOnCollision DamageOnCollision { get; private set; }

        public enum Type
        {
            Fire,
            Lightning,
            Ice
        }

        public void Initialize(ProjectileMovement movement, DamageOnCollision damageOnCollision)
        {
            Movement = movement;
            DamageOnCollision = damageOnCollision;
        }

        public Type GetProjectileType()
        {
            return this.type;
        }
    }
}