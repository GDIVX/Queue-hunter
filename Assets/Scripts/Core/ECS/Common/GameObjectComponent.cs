using UnityEngine;
using Sirenix.OdinInspector;
using Assets.Scripts.Core.ECS.Interfaces;

namespace Assets.Scripts.Core.ECS.Common
{
    [System.Serializable]
    public class GameObjectComponent : DataComponent
    {
        [SerializeField]
        private GameObject gameObject;


        public GameObject GameObject
        {
            get
            {
                return gameObject;
            }
            set
            {
                SafeSet(ref gameObject, value);
            }
        }

        public override IComponent Instantiate()
        {

            GameObjectComponent component = new();

            if (gameObject != null)
            {
                component.GameObject = gameObject;
            }

            return component;
        }

        protected override void OnSetActive(bool value)
        {
            if (gameObject != null)
            {
                gameObject.SetActive(value);
            }
        }


    }
}