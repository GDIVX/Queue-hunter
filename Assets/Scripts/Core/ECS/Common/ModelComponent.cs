using Assets.Scripts.Core.ECS.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Core.ECS.Common
{
    [System.Serializable]
    public struct ModelComponent : IComponent
    {
        public GameObject Model;

        public ModelComponent(GameObject model) : this()
        {
            Model = model;
            IsActive = true;
            IsDirty = true;
        }

        public bool IsActive { get; set; }
        public bool IsDirty { get; set; }

        public object Clone()
        {
            return new ModelComponent(Model);
        }
    }
}