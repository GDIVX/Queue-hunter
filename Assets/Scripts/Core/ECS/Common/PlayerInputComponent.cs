using Assets.Scripts.Core.ECS;
using Assets.Scripts.Core.ECS.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;


namespace Assets.Scripts.Game.Input
{
    [System.Serializable]
    public class PlayerInputComponent : DataComponent
    {
        [ShowInInspector , ReadOnly]
        private Vector3 movementInput;

        [ShowInInspector , ReadOnly]
        private KeyCode pressedKey;

        public Vector3 MovementInput
        {
            get => movementInput;
            set
            {
                SafeSet(ref movementInput, value);
            }
        }

        public KeyCode PressedKey
        {
            get => pressedKey;
            set
            {
                SafeSet(ref pressedKey, value);
            }
        }

        public override IComponent Instantiate()
        {
            PlayerInputComponent component = new PlayerInputComponent()
            {
                MovementInput = movementInput,
                PressedKey = pressedKey
            };
            return component;
        }
    }
}