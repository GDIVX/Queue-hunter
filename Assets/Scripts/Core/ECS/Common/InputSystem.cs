using Assets.Scripts.Core.ECS;
using Assets.Scripts.Engine.ECS;
using System.Collections;
using UnityEngine;
using UnityEngine.Windows;

namespace Assets.Scripts.Game.Input
{
    public class InputSystem : GameSystem
    {
        public InputSystem(Zenject.SignalBus signalBus) : base(signalBus)
        {
        }

        protected override bool ShouldProcessArchetype(Archetype archetype)
        {
            return archetype.HasComponent<PlayerInputComponent>();
        }

        protected override void OnUpdate(Archetype archetype)
        {
            var inputBatch = archetype.GetComponents<PlayerInputComponent>();

            for (int i = 0; i < archetype.Count; i++)
            {
                Vector3 movementInput = new Vector3(UnityEngine.Input.GetAxisRaw("Horizontal"), 0, UnityEngine.Input.GetAxisRaw("Vertical"));

                inputBatch[i].MovementInput = movementInput;

                if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
                {
                    inputBatch[i].PressedKeys.Add(KeyCode.Space);
                }

                if (UnityEngine.Input.GetKeyUp(KeyCode.Space) && inputBatch[i].PressedKeys.Contains(KeyCode.Space))
                {
                    inputBatch[i].PressedKeys.Remove(KeyCode.Space);
                }

                if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse0))
                {
                    inputBatch[i].PressedKeys.Add(KeyCode.Mouse0);
                }

                if (UnityEngine.Input.GetKeyUp(KeyCode.Mouse0) && inputBatch[i].PressedKeys.Contains(KeyCode.Mouse0))
                {
                    inputBatch[i].PressedKeys.Remove(KeyCode.Mouse0);
                }
            }
        }
    }
}