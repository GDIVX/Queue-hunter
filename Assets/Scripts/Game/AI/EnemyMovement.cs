using System;
using Combat;
using Game.AI;
using Game.Combat;
using Game.Utility;
using UnityEngine;
using Random = UnityEngine.Random;
using Sirenix.OdinInspector;
using UnityEngine.AI;
using UnityEngine.Events;

namespace AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyMovement : MonoBehaviour , IInit<EnemyModel>
    {
        [SerializeField, TabGroup("Setting")] private EnemyTargeting targeting;
        [SerializeField, TabGroup("Setting")] private NavMeshAgent navMeshAgent;

        [SerializeField, TabGroup("View")] private Transform view;
        [SerializeField, TabGroup("View")] private float rotationSpeed;

        [SerializeField, TabGroup("Setting")] private float speed;
        private Animator _animator;

        public bool IsMoving => !navMeshAgent.isStopped;

        [SerializeField, TabGroup("Events")] private UnityEvent<string, bool> onEnemyMove;
        [SerializeField, TabGroup("Events")] private UnityEvent<string, bool> onEnemyMoveEnd;
        [SerializeField] private bool canMove;


        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponentInChildren<Animator>();
        }

        public void SetMovementAllowed(bool value)
        {
            if (!navMeshAgent)
            {
                Debug.LogError($"{name} is missing a navmesh agent");
                return;
            }

            if (!navMeshAgent.isOnNavMesh)
            {
                Debug.LogError($"{name} is not placed on a nav mesh. Can't modifiy it's movement");
                return;
            }

            navMeshAgent.isStopped = !value;
        }

        private void Start()
        {
            if (navMeshAgent != null)
            {
                navMeshAgent.speed = speed;
            }
        }

        public void Init(EnemyModel model)
        {
            speed = model.Speed;
            if (navMeshAgent)
            {
                navMeshAgent.speed = speed;
                navMeshAgent.isStopped = false;
            }

            SetMovementAllowed(true);
        }


        private void Update()
        {
            if (!IsMoving)
            {
                InvokeOnEnemyMoveEvent(false);
                return;
            }


            if (TryGetTargetPosition(out var position))
            {
                MoveTo(position);
            }
        }

        public void MoveTo(Vector3 destination)
        {
            if (navMeshAgent)
            {
                navMeshAgent.SetDestination(destination);
            }

            HandleRotation();
            InvokeOnEnemyMoveEvent(true);
        }

        private void InvokeOnEnemyMoveEvent(bool isRunning)
        {
            onEnemyMoveEnd?.Invoke("isRunning", isRunning);
        }

        private bool TryGetTargetPosition(out Vector3 targetPosition)
        {
            var target = targeting.GetTarget();

            if (target == null)
            {
                targetPosition = default;
                return false;
            }

            targetPosition = target.Position;
            return true;
        }


        public void HandleRotation()
        {
            if (!navMeshAgent || !(navMeshAgent.velocity.sqrMagnitude > Mathf.Epsilon)) return;

            Quaternion rotation = Quaternion.LookRotation(navMeshAgent.velocity.normalized);
            var viewRotation = Quaternion.Lerp(view.rotation, rotation, Time.deltaTime * rotationSpeed);
            view.rotation = new Quaternion(0, viewRotation.y, 0, 0);
        }


        private void OnValidate()
        {
            // Ensure necessary components are present
            navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponentInChildren<Animator>();

            // Validate serialized fields to avoid null reference issues
            if (targeting == null)
            {
                Debug.LogError("EnemyTargeting component is not assigned.", this);
            }

            if (view == null)
            {
                Debug.LogError("View Transform is not assigned.", this);
            }

            if (navMeshAgent == null)
            {
                Debug.LogError("NavMeshAgent component is missing.", this);
            }

            if (_animator == null)
            {
                Debug.LogError("Animator component is missing in children.", this);
            }
        }

        public void Stop()
        {
            navMeshAgent.isStopped = true;
        }
    }
}