using System;
using System.Collections;
using AI;
using Combat;
using DG.Tweening;
using Game.Combat;
using Game.Utility;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

namespace Game.AI.Behaviours
{
    [RequireComponent(typeof(Collider))]
    public class EnemyChargeAttack : MonoBehaviour
    {
        [SerializeField, TabGroup("Dependencies")]
        private EnemyMovement movementController;

        [SerializeField, TabGroup("Dependencies")]
        private new Rigidbody rigidbody;

        [SerializeField, TabGroup("Dependencies")]
        private EnemyTargeting targeting;

        [SerializeField, TabGroup("Settings"),
         Tooltip("The range to look for target. Targets would only be visible within this range")]
        private MinMaxFloat distanceToTargetRange;

        [SerializeField, TabGroup("Settings")] private float maxChargeDistance;
        [SerializeField, TabGroup("Settings")] private float chargeOvershootDistance;
        [SerializeField, TabGroup("Settings")] private float coolDown, windUp;
        [SerializeField, TabGroup("Settings")] private float attackDamage;
        [SerializeField, TabGroup("Settings")] private float speed;
        [SerializeField, TabGroup("Settings")] private float chargeDistanceToTargetTolerance = 1.5f;


        public UnityEvent onPreparingToCharge;
        public UnityEvent<string, bool> onChargeStart, onChargeEnd;

        [ShowInInspector, ReadOnly, TabGroup("Debug")]
        private ITarget _target;

        [ShowInInspector, ReadOnly, TabGroup("Debug")]
        private Vector3 _destination;

        [ShowInInspector, ReadOnly, TabGroup("Debug")]
        private Vector3 _velocity;

        [ShowInInspector, ReadOnly, TabGroup("Debug")]
        private bool _hasAttacked;

        [ShowInInspector, ReadOnly, TabGroup("Debug")]
        private ChargeState _currentState;

        public ChargeState CurrentState => _currentState;

        public enum ChargeState
        {
            Charging,
            Seeking,
            Recovering
        }

        private void OnValidate()
        {
            movementController ??= GetComponent<EnemyMovement>();
            targeting ??= GetComponent<EnemyTargeting>();
            rigidbody ??= GetComponent<Rigidbody>();
        }

        private void Awake()
        {
            targeting.OnTargetFound += OnTargetFound;
            _currentState = ChargeState.Seeking;
        }

        private void Update()
        {
            if (_currentState != ChargeState.Charging) return;
            HandleChargeMovement();
        }

        private void HandleChargeMovement()
        {
            if (_currentState != ChargeState.Charging)
            {
                Debug.LogError($"{name} is trying to charge while not in charge state.");
                return;
            }

            if (_target == null)
            {
                Debug.LogError($"{name} is trying to charge without a target");
                return;
            }

            var distance = Vector3.Distance(transform.position, _destination);

            //End charge if the distance is too far or too close
            if (distance <= chargeDistanceToTargetTolerance ||
                distance <= distanceToTargetRange.Min ||
                distance >= distanceToTargetRange.Max)
            {
                EndCharge(_target);
                return;
            }

            var direction = (_destination - transform.position).normalized;
            _velocity = direction * speed;
            //movementController.HandleRotation();
            rigidbody.MovePosition(transform.position + _velocity * Time.deltaTime);
        }

        private void OnTargetFound(ITarget target)
        {
            if (_target != null) return;
            if (_currentState != ChargeState.Seeking) return;

            if (!CanCharge(target)) return;
            _target = target;
            StartCoroutine(HandleWindup(_target));
        }

        private void ChargeAt(ITarget target)
        {
            if (_currentState == ChargeState.Charging)
            {
                Debug.LogError($"{name} is trying to start a charge while already in charge state.");
                return;
            }

            _destination = GetChargeDestination(target);
            _hasAttacked = false;
            StartCharge(target);
        }

        private Vector3 GetChargeDestination([NotNull] ITarget target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            var direction = (target.Position - transform.position).normalized;
            var distanceToTargetWithOvershoot =
                Vector3.Distance(transform.position, target.Position) + chargeOvershootDistance;
            var distance = Mathf.Min(maxChargeDistance, distanceToTargetWithOvershoot);
            var destination = transform.position + direction *
                distance;

            //flatten the destination to the target position y value
            destination.y = target.Position.y;
            return destination;
        }

        private void OnCollisionEnter(Collision other)
        {
            HandleCollision(other.collider);
        }

        private void OnTriggerEnter(Collider other)
        {
            HandleCollision(other);
        }

        private void HandleCollision(Collider other)
        {
            if (_currentState != ChargeState.Charging || _target == null) return;

            if (other.gameObject != _target.GameObject)
            {
                return;
            }

            HandleDamage(_target.Damageable);
            EndCharge(_target);
        }

        private void HandleDamage(IDamageable targetDamageable)
        {
            if (_hasAttacked) return;
            targetDamageable.HandleDamage(attackDamage);
            _hasAttacked = true;
        }

        private IEnumerator HandleCooldown(ITarget target)
        {
            yield return new WaitForSeconds(coolDown);
            _currentState = ChargeState.Seeking;
            movementController.SetMovementAllowed(true);
        }

        private void StartCharge(ITarget target)
        {
            _currentState = ChargeState.Charging;
            _target = target;
            onChargeStart?.Invoke("isLockedRunning", true);
        }

        private void EndCharge(ITarget target)
        {
            _target = null;
            _currentState = ChargeState.Recovering;
            onChargeEnd?.Invoke("isLockedRunning", false);
            StartCoroutine(HandleCooldown(target));
        }

        private IEnumerator HandleWindup(ITarget target)
        {
            movementController.SetMovementAllowed(false);
            onPreparingToCharge?.Invoke();
            //Rotate towards the target
            transform.LookAt(target.Position);
            yield return new WaitForSeconds(windUp);
            ChargeAt(target);
        }

        private bool CanCharge(ITarget target)
        {
            if (_currentState is ChargeState.Charging or ChargeState.Recovering) return false;
            var distanceToTarget = Vector3.Distance(transform.position, target.Position);
            return distanceToTarget <= distanceToTargetRange.Max && distanceToTarget >= distanceToTargetRange.Min;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, maxChargeDistance);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, distanceToTargetRange.Min);
            Gizmos.DrawWireSphere(transform.position, distanceToTargetRange.Max);

            if (_currentState != ChargeState.Charging) return;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, _destination);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + _velocity);
        }
    }
}