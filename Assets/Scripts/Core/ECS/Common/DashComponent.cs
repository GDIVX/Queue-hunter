using Assets.Scripts.Core.ECS;
using Assets.Scripts.Core.ECS.Interfaces;
using Sirenix.OdinInspector;
using System;
using System.Collections;

using UnityEngine;

namespace Assets.Scripts.Game.Movement
{
    [Serializable]
    public struct DashComponent : IComponent
    {
        public float dashDuration;
        public float dashDistance;
        public float dashCooldown;
        public bool canDash;
        public bool isDashing;
        public Vector3 dashDirection;
        public float dashStartTime;

        public DashComponent(float dashDuration, float dashDistance, float dashCooldown)
        {
            this.dashDuration = dashDuration;
            this.dashDistance = dashDistance;
            this.dashCooldown = dashCooldown;

            //TODO: Set defualt values
            this.canDash = true;
            this.isDashing = false;
            this.dashDirection = default;
            this.dashStartTime = 0;

            IsActive = true;
            IsDirty = true;
        }

        public bool IsActive { get; set; }
        public bool IsDirty { get; set; }

        public object Clone()
        {
            return new DashComponent(dashDuration, dashDistance, dashCooldown);
        }
    }
}
