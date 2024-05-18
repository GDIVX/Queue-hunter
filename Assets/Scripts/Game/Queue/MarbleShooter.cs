using System;
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
        [SerializeField] private MarbleQueue _queue;
        [SerializeField] private Transform spawnPoint;


        [Tooltip("Triggered when attempting to shoot a marble. Rerun true if it was successful")]
        public UnityEvent<bool> onShootingMarbleAttempted;

        private void Awake()
        {
            _projectileFactory ??= GetComponent<ProjectileFactory>();
            _queue ??= GetComponent<MarbleQueue>();
        }

        public void ShootNextMarble()
        {
            //If the queue is empty, throw an event for the UI/UX and return
            if (_queue.GetQueue().IsEmpty())
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


            //Instantiate a projectile

            //TODO:
            //1. Fire Projectile
            //2. Chain Lightning 

            Projectile projectile = _projectileFactory.Create(marble.ProjectileModel, spawnPoint.position);

            //rotate the projectile
            projectile.transform.rotation = Quaternion.LookRotation(transform.forward);

            //trigger event for sucssus
            onShootingMarbleAttempted?.Invoke(true);
        }
    }
}