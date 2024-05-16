using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Combat
{
    public class ProjectileMovement : MonoBehaviour
    {
        [SerializeField] private float _speed;

        public void Initialize(float speed)
        {
            _speed = speed;
        }

        private void Update()
        {
            var direction = transform.forward;
            Vector3 translation = direction * (_speed * Time.deltaTime);
            transform.position += translation;
            
        }
    }
}