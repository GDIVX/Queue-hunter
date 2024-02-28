using Assets.Scripts.Core.ECS;
using Assets.Scripts.Core.ECS.Common;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Engine.ECS.Common
{
    public class RotationSystem : GameSystem
    {
        public RotationSystem(SignalBus signalBus) : base(signalBus)
        {
        }

        protected override bool ShouldProcessArchetype(Archetype archetype)
        {
            return archetype.HasComponents<RotationComponent, GameObjectComponent>();
        }

        protected override void OnUpdate(Archetype archetype)
        {

            //get the position component
            var rotationBatch = archetype.GetComponents<RotationComponent>();

            //Get the game object component
            var gameObjectBatch = archetype.GetComponents<GameObjectComponent>();

            //Update the position of the game object
            for (int i = 0; i < archetype.Count; i++)
            {
                UpdateRotation(gameObjectBatch[i], rotationBatch[i]);
            }

        }

        private void UpdateRotation(GameObjectComponent gameObjectComponent, RotationComponent rotationComponent)
        {
            //TODO : This throw notifications. Fix this

            //var rot = Quaternion.LookRotation(rotationComponent.rotation, Vector3.up);
            //gameObjectComponent.GameObject.transform.rotation = 
            //    Quaternion.RotateTowards(gameObjectComponent.GameObject.transform.rotation, rot, 360 * Time.deltaTime);
        }
    }
}