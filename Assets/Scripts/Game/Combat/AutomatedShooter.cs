using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Combat;
using Sirenix.OdinInspector;
using UnityEngine;


namespace Combat
{
    public class AutomatedShooter : MonoBehaviour
    {
        [SerializeField, TabGroup("Settings")] private float projectilePerSecond;
        [SerializeField, TabGroup("Settings")] private Vector3 projectileSpawnOffset;
        [SerializeField, TabGroup("Settings")] private ProjectilesLifetimeHandler projectilesHandler;
        [SerializeField, TabGroup("Settings")] private string lookupTag;

        [SerializeField, TabGroup("View")] private Transform View;
        [SerializeField, TabGroup("View")] private float rotationSpeed;

        //TODO use addressable 
        [SerializeField] private ProjectileModel projectileModel;

        [ShowInInspector, ReadOnly, TabGroup("Debug")]
        private readonly List<ITarget> targets = new();

        [ShowInInspector, ReadOnly, TabGroup("Debug")]
        private bool isShooting;

        Quaternion rotation;

        public void Reset()
        {
            isShooting = false;
            targets.Clear();
        }

        private void OnTriggerEnter(Collider other)
        {
            //Track ITarget objects that have the look up tag on their game object
            if (other.TryGetComponent(out ITarget targetable) && other.CompareTag(lookupTag))
            {
                targets.Add(targetable);
                targetable.OnDestroyed += (x) => targets.Remove(targetable);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out ITarget targetable) && other.CompareTag(lookupTag))
            {
                if (targets.Contains(targetable))
                {
                    targets.Remove(targetable);
                }
            }
        }

        private void Update()
        {
            // Check if there are targets and we are not already shooting
            if (targets.Count > 0 && !isShooting)
            {
                StartCoroutine(HandleTargetCour());
                return;
            }

            if (targets.Count == 0) return;
            if (View.rotation == rotation) return;
            //Rotate the view towards the direction of fire
            View.rotation = Quaternion.Lerp(View.rotation, rotation, Time.deltaTime * rotationSpeed);
        }


        IEnumerator HandleTargetCour()
        {
            isShooting = true; // Set the flag to prevent multiple coroutines

            while (targets.Count > 0)
            {
                //fetch the closest target
                var target = targets.OrderBy(t => (t.Position - transform.position).sqrMagnitude).FirstOrDefault();
                HandleTarget(target);

                // Wait for the specified time before spawning the next projectile
                yield return new WaitForSeconds(1f / projectilePerSecond);
            }

            isShooting = false; // Reset the flag when done
        }

        private void HandleTarget(ITarget target)
        {
            // Calculate the spawn position
            var spawnPosition = transform.position + projectileSpawnOffset;
            // Calculate direction to the target
            var direction = (target.Position - spawnPosition).normalized;
            direction = direction == Vector3.zero ? transform.forward : direction;
            // Convert the direction into rotation
            rotation = Quaternion.LookRotation(direction);

            // Create a projectile at the offset position, not the spawnPosition
            projectilesHandler.Create(projectileModel, spawnPosition, rotation);
        }
    }
}