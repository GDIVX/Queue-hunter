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
        public UnityEvent<bool> onShootingMarbleAttempted;
        public UnityEvent onShootingMarble;
        public UnityEvent onShootingMarbleEnd;

        public UnityEvent onShootingFire;
        public UnityEvent onShootingLightning;
        public UnityEvent onShootingIce;

        private void Awake()
        {
            _projectileFactory ??= GetComponent<ProjectileFactory>();
            _queue ??= GetComponent<MarbleQueue>();
            _effectPool ??= GetComponent<EffectPool>();
        }

        public void ShootNextMarble()
        {
            if (isShooting) return;

            //If the queue is empty, throw an event for the UI/UX and return
            if (_queue.IsEmpty())
            {
                onShootingMarbleAttempted?.Invoke((false));
                return;
            }

            //Try to fetch the next marble from the queue
            Marble marble = _queue.EjectMarble();

            if (marble == null)
            {
                Debug.LogError("Failed to fetch marble from a non empty queue");
                onShootingMarbleAttempted?.Invoke((false));
                return;
            }
            if (marble.GetMarbleType().ToString() == "Fire") onShootingFire?.Invoke();

            else if (marble.GetMarbleType().ToString() == "Lightning") onShootingLightning?.Invoke();

            else if (marble.GetMarbleType().ToString() == "Ice") onShootingIce?.Invoke();



            onShootingMarble.Invoke();
            StartCoroutine(PreFireCoroutine(marble));


            //trigger event for sucssus
            onShootingMarbleAttempted?.Invoke(true);
            isShooting = true;
            StartCoroutine(MarbleShotCoroutine());

        }

        protected IEnumerator MarbleShotCoroutine()
        {
            yield return new WaitForSeconds(marbleShotTime);
            isShooting = false;
            onShootingMarbleEnd?.Invoke();
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