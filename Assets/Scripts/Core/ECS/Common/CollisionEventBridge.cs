using UnityEngine;

namespace Assets.Scripts.Core.ECS.Common
{
    // Bridge component for Unity's collision events
    public class CollisionEventBridge : MonoBehaviour
    {
        private CollisionComponent _collisionComponent;

        public void Initialize(CollisionComponent collisionComponent)
        {
            _collisionComponent = collisionComponent;
        }

        private void OnTriggerEnter(Collider other)
        {
            _collisionComponent.OnUnityTriggerEnter(other);
        }

        private void OnTriggerStay(Collider other)
        {
            _collisionComponent.OnUnityTriggerStay(other);
        }

        private void OnTriggerExit(Collider other)
        {
            _collisionComponent.OnUnityTriggerExit(other);
        }
    }

}
