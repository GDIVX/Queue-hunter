using Assets.Scripts.Core;
using Assets.Scripts.Core.ECS;
using Assets.Scripts.Core.ECS.Interfaces;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Assets.Scripts.Game.Movement
{
    [Serializable]
    public class MovementComponent : DataComponent
    {
        [ShowInInspector]
        private float speed;

        [ShowInInspector , ReadOnly]
        private Vector3 lastDir;

        public BindableProperty<bool> isRunning = new(false, "isRunning");

        public Vector3 LastDir
        {
            get => lastDir; 
            set
            {
                SafeSet(ref lastDir, value);
            }
        }

        public float Speed
        {
            get => speed;
            set
            {
                SafeSet(ref speed, value);
            }
        }

        public override IComponent Instantiate()
        {
            MovementComponent component = new MovementComponent()
            {
                Speed = speed,
                LastDir = lastDir
            };
            return component;
        }
    }
}