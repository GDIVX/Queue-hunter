using System;
using System.Collections;
using Combat;
using Game.Combat;
using ModestTree;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Queue
{
    [RequireComponent(typeof(ProjectileFactory))]
    public class MarbleShooter : MonoBehaviour
    {
        [SerializeField] private ProjectileFactory _projectileFactory;
        [SerializeField] private EffectPool _effectPool;
        [SerializeField] private MarbleQueue _queue;
        [SerializeField] private Transform spawnPoint;

        [SerializeField] private float marbleShotTime;
        [SerializeField] private bool isShooting;


        [Tooltip("Triggered when attempting to shoot a marble. Rerun true if it was successful")]
        public UnityEvent<bool> onShootingAttempted;

        public UnityEvent onShootingProjectile;
        public UnityEvent onShootingEnd;

        public UnityEvent onShootingFire;
        public UnityEvent onShootingLightning;
        public UnityEvent onShootingIce;

        private void Awake()
        {
            _projectileFactory ??= GetComponent<ProjectileFactory>();
            _queue ??= GetComponent<MarbleQueue>();
            _effectPool ??= GetComponent<EffectPool>();
        }

        private void Start()
        {
            isShooting = false;
        }

        public void FireNextProjectile()
        {
            if (isShooting) return;
            isShooting = true;

            //If the queue is empty, throw an event for the UI/UX and return
            if (_queue.IsEmpty())
            {
                onShootingAttempted?.Invoke((false));
                isShooting = false;
                return;
            }

            //Try to fetch the next marble from the queue
            Marble marble = _queue.EjectMarble();

            if (marble == null)
            {
                onShootingAttempted?.Invoke((false));
                isShooting = false;
                return;
            }


            FireProjectile(marble);
        }

        private void FireProjectile(Marble marble)
        {
            StartCoroutine(PreFireCoroutine(marble));


            //trigger event for sucssus
            onShootingProjectile.Invoke();
            onShootingAttempted?.Invoke(true);
            InvokeMarbleTypeHasBeenFired(marble);
            StartCoroutine(StartFireCoroutine());
        }

        private void InvokeMarbleTypeHasBeenFired(Marble marble)
        {
            switch (marble.GetMarbleType())
            {
                case MarbleModel.Type.Fire:
                    onShootingFire?.Invoke();
                    break;
                case MarbleModel.Type.Lightning:
                    onShootingLightning?.Invoke();
                    break;
                case MarbleModel.Type.Ice:
                    onShootingIce?.Invoke();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected IEnumerator StartFireCoroutine()
        {
            yield return new WaitForSeconds(marbleShotTime);
            isShooting = false;
            onShootingEnd?.Invoke();
        }

        protected IEnumerator PreFireCoroutine(Marble marble)
        {
            yield return new WaitForSeconds(.2f);

            //Instantiate a projectile

            Projectile projectile = _effectPool.GetProjectile(marble);

            projectile.transform.position = transform.position;

            if (!projectile.isActiveAndEnabled)
            {
                projectile.gameObject.SetActive(true);
            }

            //rotate the projectile
            projectile.transform.rotation = Quaternion.LookRotation(transform.forward);

            
        }
    }
}