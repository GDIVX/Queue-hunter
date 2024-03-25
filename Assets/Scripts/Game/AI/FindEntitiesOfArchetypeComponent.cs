using System;
using System.Collections.Generic;
using Assets.Scripts.Core.ECS;
using Assets.Scripts.Core.ECS.Interfaces;
using Assets.Scripts.Engine.ECS;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.AI
{
    [Serializable]
    public class FindEntitiesOfArchetypeComponent : DataComponent
    {
        public List<IEntity> Entities { get; set; }
        public string ArchetypeName;

        public override IComponent Instantiate()
        {
            FindEntitiesOfArchetypeComponent component = new()
            {
                Entities = Entities,
                ArchetypeName = ArchetypeName
                
            };

            return component;
        }
    }
}