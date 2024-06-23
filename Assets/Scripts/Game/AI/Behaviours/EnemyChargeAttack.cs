using System;
using System.Collections;
using AI;
using Combat;
using Game.Combat;
using Game.Utility;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Game.AI.Behaviours
{
    [RequireComponent(typeof(Collider))]
    public class EnemyChargeAttack : MonoBehaviour
    {
        [SerializeField, BoxGroup("Dependencies")]
        private EnemyMovement movementController;

        [SerializeField, BoxGroup("Dependencies")]
        private EnemyTargeting targeting;

        [SerializeField, BoxGroup("Settings")] private MinMaxFloat distanceToTargetRange;
        [SerializeField, BoxGroup("Settings")] private float coolDown;
        [SerializeField, BoxGroup("Settings")] private float attackDamage;

        private ITarget _target;

        public UnityEvent OnPreperingToCharge, OnChargeStart, OnChargeEnd;

        private bool _isCharging = false;

        private void OnValidate()
        {
            movementController ??= GetComponent<EnemyMovement>();
            targeting ??= GetComponent<EnemyTargeting>();
        }

        private void OnEnable()
        {
            targeting.OnTargetFound += OnTargetFound;
        }

        private void OnTargetFound(ITarget target)
        {
            if (!CanCharge(target)) return;

            _target = target;
            ChargeAt(target);
        }

        private void ChargeAt(ITarget target)
        {
            StartCoroutine(HandleCooldown(target));

            Vector3 destination = GetChargeDestination();

            StartMovementTo(destination);

            EndCharge();
        }

        private void StartMovementTo(Vector3 destination)
        {
            throw new NotImplementedException();
        }

        private Vector3 GetChargeDestination()
        {
            throw new NotImplementedException();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_isCharging) return;

            if (_target == null) return;

            if (!other.gameObject == _target.GameObject) return;

            HandleDamange(_target.Damageable);
        }

        private void HandleDamange(IDamageable targetDamageable)
        {
            targetDamageable.HandleDamage(attackDamage);
        }


        private IEnumerator HandleCooldown(ITarget target)
        {
            OnPreperingToCharge?.Invoke();
            yield return new WaitForSeconds(coolDown);
            StartCharge(target);
        }

        private void StartCharge(ITarget target)
        {
            _isCharging = true;
            _target = target;
            OnChargeStart?.Invoke();
        }

        private void EndCharge()
        {
            _isCharging = false;
            _target = null;
            OnChargeEnd?.Invoke();
        }

        private bool CanCharge(ITarget target)
        {
            if (_isCharging) return false;

            var distanceToTarget = Vector3.Distance(transform.position, target.Position);
            var isWithinRange = distanceToTarget <= distanceToTargetRange.Max &&
                                distanceToTarget >= distanceToTargetRange.Min;
            return isWithinRange;
        }
    }
}