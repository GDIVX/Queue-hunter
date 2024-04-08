using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Combat
{
    [RequireComponent(typeof(ProjectileFactory))]
    public class ProjectilesLifetimeHandler : MonoBehaviour
    {
        [ShowInInspector, ReadOnly] private GameObjectPool<ProjectileModel, Projectile> pool;
        [SerializeField] private ProjectileFactory factory;

        private void Start()
        {
            factory ??= GetComponent<ProjectileFactory>();

            pool = new(factory, factory);
        }

        [Button]
        public Projectile Create(ProjectileModel model, Vector3 position, Quaternion rotation)
        {
            var projectile = pool.Create(model, position);
            projectile.transform.localRotation = rotation;

            projectile.GetComponent<DamageOnCollision>().OnCollision += (damage, collision) =>
            {
                if (!projectile.IsActive) return;
                pool.Return(projectile);
            };

            StartCoroutine(HandleProjectileLifetime(projectile, model.Lifetime));

            return projectile;
        }

        IEnumerator HandleProjectileLifetime(Projectile projectile, float lifetime)
        {
            yield return new WaitForSeconds(lifetime);
            if (!projectile.IsActive) yield break;
            ;
            pool.Return(projectile);
        }
    }
}