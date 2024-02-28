using Assets.Scripts.Core.ECS.Interfaces;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Assets.Scripts.Core.ECS.Common
{
    [Serializable]
    public struct GameObjectComponent : IComponent
    {
        [ReadOnly] public GameObject GameObject;
        [SerializeField] public string Name;

        private bool isActive;
        public GameObjectComponent(GameObject gameObject, string name) : this()
        {
            GameObject = gameObject;
            Name = name;
            IsActive = true;
            IsDirty = true;
        }

        public bool IsActive
        {
            get => isActive;
            set
            {
                isActive = value;
                GameObject.SetActive(value);
            }
        }
        public bool IsDirty { get; set; }

        public object Clone()
        {
            return new GameObjectComponent(GameObject, Name);
        }
    }
}