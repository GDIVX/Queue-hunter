using System.Collections;
using Combat;
using Sirenix.OdinInspector;
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
        [ShowInInspector, ReadOnly] private bool _isShooting;


        [Tooltip("Triggered when attempting to shoot a marble. Rerun true if it was successful")]
        public UnityEvent<Projectile> onShootingProjectile;

        private GameObjectPool<ProjectileModel, Projectile> _pool;

        private void Awake()
        {
            projectileFactory ??= GetComponent<ProjectileFactory>();

            _pool = new GameObjectPool<ProjectileModel, Projectile>(projectileFactory, projectileFactory);
        }

        public void Fire(Vector3 targetPoint)
        {
            if (_isShooting) return;

            StartCoroutine(StartShooting(targetPoint));
        }

        IEnumerator StartShooting(Vector3 targetPoint)
        {
            _isShooting = true;
            yield return new WaitForSeconds(delayBetweenShots);
            Projectile projectile = InstantiateProjectile();

            projectile.transform.LookAt(targetPoint);

            onShootingProjectile?.Invoke(projectile);
            _isShooting = false;
        }

        private Projectile InstantiateProjectile()
        {
            return _pool.Get(projectileModel, spawnPoint.position);
        }
    }
}