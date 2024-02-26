using Assets.Scripts.Core.ECS;
using Assets.Scripts.Engine.ECS;
using System.Collections;
using UnityEngine;
using UnityEngine.Windows;

namespace Assets.Scripts.Game.Input
{
    public class InputHandler : GameSystem
    {
        public InputHandler(Zenject.SignalBus signalBus) : base(signalBus)
        {
        }
        protected override bool ShouldProcessArchetype(Archetype archetype)
        {
            return archetype.HasComponent<PlayerInput>();
        }

        protected override void OnUpdate(Archetype archetype)
        {
            var inputBatch = archetype.GetComponents<PlayerInput>();

            for (int i = 0; i < archetype.Count; i++)
            {
                Vector3 movementInput = new Vector3(UnityEngine.Input.GetAxisRaw("Horizontal"), 0, UnityEngine.Input.GetAxisRaw("Vertical"));

                inputBatch[i].MovementInput = movementInput;

                if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
                {
                    inputBatch[i].PressedKey = KeyCode.Space;
                }

                if (UnityEngine.Input.GetKeyUp(KeyCode.Space))
                {
                    inputBatch[i].PressedKey = KeyCode.None;
                }
            }
        }
    }
}