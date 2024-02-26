using Assets.Scripts.Core.ECS;
using Sirenix.OdinInspector;
using UnityEngine;


namespace Assets.Scripts.Game.Input
{
    [CreateAssetMenu(fileName = "PlayerInput", menuName = "Game/Input/PlayerInput")]
    public class PlayerInput : DataComponent
    {
        [ShowInInspector]
        private Vector3 movementInput;

        [ShowInInspector]
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
    }
}