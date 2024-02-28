using Assets.Scripts.Core.ECS;
using Assets.Scripts.Core.ECS.Interfaces;
using Sirenix.OdinInspector;
using System;
using UnityEngine;


namespace Assets.Scripts.Game.Input
{
    [Serializable]
    public struct PlayerInputComponent : IComponent
    {
        [ShowInInspector]
        private Vector3 movementInput;

        [ShowInInspector]
        private KeyCode pressedKey;

        public Vector3 MovementInput;

        public KeyCode PressedKey;

        public bool IsActive { get; set; }
        public bool IsDirty { get; set; }

        public object Clone()
        {
            return new PlayerInputComponent();
        }
    }
}