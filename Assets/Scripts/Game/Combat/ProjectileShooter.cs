using System;
using System.Collections;
using Combat;
using Game.Queue;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Combat
{
    public class ProjectileShooter : MonoBehaviour
    {
        [SerializeField] private ProjectileModel projectileModel;
        [SerializeField] private ProjectileFactory projectileFactory;
        [SerializeField] private Transform spawnPoint;

        [SerializeField] private float delayBetweenShots;
        [SerializeField] private bool isShooting;


        [Tooltip("Triggered when attempting to shoot a marble. Rerun true if it was successful")]
        public UnityEvent<bool> onShootingAttempted;

        public UnityEvent<Projectile> onShootingProjectile;

        private GameObjectPool<ProjectileModel, Projectile> _pool;

        private void Awake()
        {
            projectileFactory ??= GetComponent<ProjectileFactory>();

            _pool = new GameObjectPool<ProjectileModel, Projectile>(projectileFactory, projectileFactory);
        }

        public void Fire()
        {
            if (isShooting) return;

            StartCoroutine(StartShooting());
        }

        IEnumerator StartShooting()
        {
            isShooting = true;
            yield return new WaitForSeconds(delayBetweenShots);
            Projectile projectile = InstantiateProjectile();
            onShootingProjectile?.Invoke(projectile);
            isShooting = false;
        }

        private Projectile InstantiateProjectile()
        {
            return _pool.Get(projectileModel, spawnPoint.position);
        }
    }
}