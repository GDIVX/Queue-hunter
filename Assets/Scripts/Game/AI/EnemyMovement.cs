using System;
using Combat;
using UnityEngine;
using Random = UnityEngine.Random;
using Sirenix.OdinInspector;

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

        public void SetMovementAllowed(bool value)
        {
            isMoveing = value;
            if (value) anim.SetBool("isRunning", true);
            else anim.SetBool("isRunning", false);
        }

        private void Start()
        {
            anim = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            if (!isMoveing) return;
            //Get a target 
            ITargetable target = targeting.GetTarget();
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