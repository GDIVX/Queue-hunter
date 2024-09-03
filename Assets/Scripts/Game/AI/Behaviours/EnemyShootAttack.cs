using System;
using System.Collections;
using AI;
using Game.Combat;
using Game.Utility;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.AI.Behaviours
{
    public class EnemyShootAttack : MonoBehaviour, IInit<EnemyModel>
    {
        [SerializeField, BoxGroup("Dependencies")]
        private EnemyMovement movement;

        [SerializeField, BoxGroup("Dependencies")]
        private EnemyTargeting targeting;

        [SerializeField, BoxGroup("Dependencies")]
        private ProjectileShooter shooter;

        [SerializeField, BoxGroup("Settings")] private float windupTime;
        [SerializeField, BoxGroup("Settings")] private float attackRange;

        [ShowInInspector, ReadOnly] private ShooterState _currentState;
        private ITarget _target;

        private enum ShooterState
        {
            Idle,
            Seeking,
            Chasing,
            WindupStart,
            DuringWindup,
            Shooting,
        }

        private void OnValidate()
        {
            movement ??= GetComponent<EnemyMovement>();
            targeting ??= GetComponentInChildren<EnemyTargeting>();
            shooter ??= GetComponentInChildren<ProjectileShooter>();
        }

        public void Init(EnemyModel input)
        {
            _currentState = ShooterState.Seeking;
            if (TryGetComponent(out Health health))
            {
                health.OnAboutToBeDestroyed.AddListener(d => _currentState = ShooterState.Idle);
            }
        }

        private void Start()
        {
            _currentState = ShooterState.Seeking;
        }

        private void Update()
        {
            switch (_currentState)
            {
                case ShooterState.DuringWindup:
                    return;
                case ShooterState.Seeking:
                    HandleSeeking();
                    break;
                case ShooterState.Chasing:
                    HandleChasing();
                    break;
                case ShooterState.WindupStart:
                    HandleWindup();
                    break;
                case ShooterState.Shooting:
                    HandleShooting();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void HandleChasing()
        {
            if (_currentState != ShooterState.Chasing) return;
            if (_target == null)
            {
                _currentState = ShooterState.Seeking;
                return;
            }

            if (IsTargetInRange())
            {
                movement.Stop();
                _currentState = ShooterState.Shooting;
                return;
            }

            var directionFromTarget = (transform.position - _target.Position).normalized;
            var distanceToTarget = Vector3.Distance(transform.position, _target.Position);
            var distance = MathF.Min(attackRange, distanceToTarget);
            var moveToPosition = directionFromTarget * distance;
            movement.MoveTo(moveToPosition);
        }

        private void HandleShooting()
        {
            if (_currentState != ShooterState.Shooting) return;

            if (_target == null)
            {
                _currentState = ShooterState.Seeking;
                return;
            }

            if (!IsTargetInRange())
            {
                _currentState = ShooterState.Chasing;
                return;
            }

            shooter.Fire(_target.Position);
        }

        private bool IsTargetInRange()
        {
            return Vector3.Distance(_target.Position, transform.position) <= attackRange;
        }

        private void HandleWindup()
        {
            if (_currentState != ShooterState.WindupStart) return;

            StartCoroutine(WindupCoroutine());
        }

        IEnumerator WindupCoroutine()
        {
            _currentState = ShooterState.DuringWindup;
            yield return new WaitForSeconds(windupTime);

            _currentState = ShooterState.Shooting;
        }

        private void HandleSeeking()
        {
            if (_currentState != ShooterState.Seeking) return;

            _target = targeting.GetTarget();
            if (_target == null) return;

            //We find a target. 

            _currentState = ShooterState.Chasing;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}