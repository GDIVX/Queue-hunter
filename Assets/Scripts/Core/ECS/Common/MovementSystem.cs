using Assets.Scripts.Core.ECS;
using Assets.Scripts.Engine.ECS;
using Assets.Scripts.Engine.ECS.Common;
using Assets.Scripts.Game.Input;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using Zenject.SpaceFighter;
using static UnityEngine.EventSystems.EventTrigger;


namespace Assets.Scripts.Game.Movement
{
    public class MovementSystem : GameSystem
    {
        public MovementSystem(Zenject.SignalBus signalBus) : base(signalBus)
        {

        }
        protected override bool ShouldProcessArchetype(Archetype archetype)
        {
            return archetype.HasComponents<PlayerInputComponent, MovementComponent, PositionComponent, RotationComponent>();
        }

        protected override void OnUpdate(Archetype archetype)
        {
            var inputBatch = archetype.GetComponents<PlayerInputComponent>();

            var movementParamsBatch = archetype.GetComponents<MovementComponent>();

            var positionBatch = archetype.GetComponents<PositionComponent>();

            var rotationBatch = archetype.GetComponents<RotationComponent>();

            for (int i = 0; i < archetype.Count; i++)
            {
                var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
                var skewedInput = matrix.MultiplyPoint3x4(inputBatch[i].MovementInput);

                //rot detection
                if (skewedInput != Vector3.zero)
                {
                    rotationBatch[i].Rotation = GetRelativeRotation(positionBatch[i], movementParamsBatch[i]);
                }

                //move detection
                if (skewedInput != Vector3.zero)
                {
                    movementParamsBatch[i].LastDir = skewedInput;
                    Move(positionBatch[i], movementParamsBatch[i]);
                }
                else movementParamsBatch[i].isRunning.Value = false;

            }
        }


        #region MOVE_FUNCS
        void Move(PositionComponent posComp, MovementComponent moveParamsComp)
        {
            posComp.Position += moveParamsComp.LastDir * Time.deltaTime * moveParamsComp.Speed;
            moveParamsComp.isRunning.Value = true;
        }
        #endregion

        #region ROT_FUNCS
        Vector3 GetRelativeRotation(PositionComponent posComp, MovementComponent moveParamsComp)
        {
            var relative = (posComp.Position + moveParamsComp.LastDir) - posComp.Position;
            return relative;
        }


        #endregion
    }
}
