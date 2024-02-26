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
            return archetype.HasComponents<PlayerInputComponent, MovementParamsComponent, PositionComponent, RotationComponent>();
        }

        //protected override bool ShouldProcessEntity(IEntity entity)
        //{
        //    return entity.HasComponent<PlayerInput, MovementParams, PositionComponent, RotationComponent>();
        //}

        //protected override void OnUpdate(IEntity entity)
        //{

        //    var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        //    var skewedInput = matrix.MultiplyPoint3x4(entity.GetComponent<PlayerInput>().MovementInput);

        //    //rot detection
        //    if (skewedInput != Vector3.zero)
        //    {
        //        entity.GetComponent<RotationComponent>().Rotation = GetRelativeRotation(entity);
        //    }

        //    //move detection
        //    if (skewedInput != Vector3.zero)
        //    {
        //        entity.GetComponent<MovementParams>().LastDir = skewedInput;
        //        Move(entity);
        //    }
        //}

        protected override void OnUpdate(Archetype archetype)
        {
            var inputBatch = archetype.GetComponents<PlayerInputComponent>();

            var movementParamsBatch = archetype.GetComponents<MovementParamsComponent>();

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
            }
        }


        #region MOVE_FUNCS
        void Move(PositionComponent posComp, MovementParamsComponent moveParamsComp)
        {
            posComp.Position += moveParamsComp.LastDir * Time.deltaTime * moveParamsComp.Speed;
        }
        #endregion

        #region ROT_FUNCS
        Vector3 GetRelativeRotation(PositionComponent posComp, MovementParamsComponent moveParamsComp)
        {
            var relative = (posComp.Position + moveParamsComp.LastDir) - posComp.Position;
            return relative;
        }


        #endregion
    }
}
