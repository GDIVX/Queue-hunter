using Assets.Scripts.Core.ECS;
using Assets.Scripts.Core.ECS.Interfaces;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Movement
{
    [Serializable]
    public class DashComponent : DataComponent
    {
        [ShowInInspector]
        private float dashDuration;

        [ShowInInspector]
        private float dashDistance;

        [ShowInInspector]
        private float dashCooldown;

        private bool canDash = true;

        private bool isDashing;

        private Vector3 dashDirection;

        private float dashStartTime;

        public bool CanDash
        {
            get => canDash;
            set
            {
                SafeSet(ref canDash, value);
            }
        }

        public Vector3 DashDirection
        {
            get => dashDirection;
            set
            {
                SafeSet(ref dashDirection, value);
            }
        }

        public float DashStartTime
        {
            get => dashStartTime;
            set
            {
                SafeSet(ref dashStartTime, value);
            }
        }

        public float DashCooldown
        {
            get => dashCooldown;
            set
            {
                SafeSet(ref dashCooldown, value);
            }
        }

        public float DashDuration
        {
            get => dashDuration;
            set
            {
                SafeSet(ref dashDuration, value);
            }
        }

        public float DashDistance
        {
            get => dashDistance;
            set
            {
                SafeSet(ref dashDistance, value);
            }
        }

        public bool IsDashing
        {
            get => isDashing;
            set
            {
                SafeSet(ref isDashing, value);
            }
        }

        public override IComponent Instantiate()
        {
            DashComponent component = new DashComponent()
            {
                DashDuration = dashDuration,
                DashDistance = dashDistance,
                DashCooldown = dashCooldown
            };
            return component;
        }
    }
}
