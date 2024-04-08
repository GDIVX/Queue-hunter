using Assets.Scripts.Core.ECS;
using Assets.Scripts.Core.ECS.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Assets.Scripts.Game.Input
{
    [System.Serializable]
    public class PlayerInputComponent : DataComponent
    {
        [ShowInInspector , ReadOnly]
        private Vector3 movementInput;

        [ShowInInspector]
        //private KeyCode[] pressedKeys;

        private List<KeyCode> pressedKeys = new List<KeyCode>();

        public Vector3 MovementInput
        {
            get => movementInput;
            set
            {
                SafeSet(ref movementInput, value);
            }
        }

        public List<KeyCode> PressedKeys
        {
            get => pressedKeys;
            set
            {
                SafeSet(ref pressedKeys, value);
            }
        }

        public override IComponent Instantiate()
        {
            PlayerInputComponent component = new PlayerInputComponent()
            {
                MovementInput = movementInput,
                PressedKeys = pressedKeys
            };
            return component;
        }
    }
}