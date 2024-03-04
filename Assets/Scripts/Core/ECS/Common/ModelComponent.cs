using Assets.Scripts.Core.ECS;
using Assets.Scripts.Core.ECS.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Scripts.Engine.ECS.Common
{
    [System.Serializable]
    public class ModelComponent : DataComponent
    {
        [SerializeField] private GameObject model;

        public GameObject Model
        {
            get
            {
                return model;
            }
            set
            {
                //if the entity is disabled, it is read only
                if (IsActive) model = value;
            }
        }

        public override IComponent Instantiate()
        {
            ModelComponent component = new ModelComponent()
            {
                Model = model
            };
            return component;
        }

    }

}