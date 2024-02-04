using Assets.Scripts.Engine.ECS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityInspector : MonoBehaviour
{
    [SerializeField] int entityID;
    [SerializeField] List<DataComponent> components;

    public void Init(Entity entity)
    {
        entityID = entity.ID;

        components = new();

        foreach (var component in entity.Components)
        {
            components.Add(component as DataComponent);
        }
    }
}
