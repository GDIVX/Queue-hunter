using System;
using Combat;
using UnityEngine;
using Random = UnityEngine.Random;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace AI
{
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField, TabGroup("Setting")] private EnemyTargeting targeting;

        [SerializeField, TabGroup("View")] private Transform view;
        [SerializeField, TabGroup("View")] private float rotationSpeed;

        [SerializeField, TabGroup("Setting")] private float speed;
        Animator anim;

        private bool isMoveing = true;

        [SerializeField, TabGroup("Events")] private UnityEvent<string, bool> OnEnemyMove;
        [SerializeField, TabGroup("Events")] private UnityEvent<string, bool> OnEnemyMoveEnd;


        public void SetMovementAllowed(bool value)
        {
            isMoveing = value;
        }

        private void Start()
        {
            anim = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            if (!isMoveing)
            {
                OnEnemyMoveEnd?.Invoke("isRunning", false);
                //anim.SetBool("isRunning", false);
                return;
            }
            OnEnemyMove?.Invoke("isRunning", true);
            //anim.SetBool("isRunning", true);
            //Get a target 
            ITarget target = targeting.GetTarget();
            Vector3 direction;

            if (target == null)
            {
                var circle = Random.insideUnitCircle;
                direction = new Vector3(circle.x, 0, circle.y);
            }
            else
            {
                direction = target.Position - transform.position;
            }


            Vector3 translation = direction.normalized * (speed * Time.deltaTime);

            transform.Translate(translation);

            var rotation = Quaternion.LookRotation(direction);
            view.rotation = Quaternion.Lerp(view.rotation, rotation, Time.deltaTime * rotationSpeed);
        }

        public void Init(float modelSpeed)
        {
            speed = modelSpeed;
            isMoveing = true;
        }
    }
}