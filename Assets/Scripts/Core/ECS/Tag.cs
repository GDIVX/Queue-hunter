using Assets.Scripts.Core.ECS.Interfaces;
using Assets.Scripts.ECS;
using Assets.Scripts.Engine.ECS;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.ECS
{
    [System.Serializable]
    public class Tag : ITag
    {
        IEntity _parent;

        [ShowInInspector]
        public string Name { get; private set; }
        public bool IsActive { get; set; }

        public Tag(string name)
        {
            Name = name;
            IsActive = true;
        }

        public IComponent Clone()
        {
            return new Tag(Name);
        }

        public IEntity GetParent()
        {
            return _parent;
        }

        public void SetParent(IEntity entity)
        {
            _parent = entity;
        }
    }
}