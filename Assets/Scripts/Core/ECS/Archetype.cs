using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Engine.ECS
{
    /// <summary>
    /// EntityDefinition is a ScriptableObject that can be used to create entities with predefined components.
    /// Note: This is not for runtime use. It is only for creating entities in the editor.
    /// </summary>
    [CreateAssetMenu(fileName = "Archetype", menuName = "ECS/Archetype")]
    public class Archetype : ScriptableObject
    {
        [SerializeField] List<string> tags;
        [SerializeField] List<DataComponent> components;

        public List<DataComponent> Components { get => components; set => components = value; }

        public Entity CreateEntity()
        {
            var entity = ECSManager.Instance.CreateEntity();

            foreach (var component in Components)
            {
                if (component == null)
                {
                    Debug.LogError($"Component is null in {name}");
                    continue;
                }

                entity.AddComponent(component.Clone());
            }

            foreach (var tag in tags)
            {
                entity.Tags.Add(tag);
            }

            ECSManager.Instance.OnEntityInitialized?.Invoke(entity);

            return entity;
        }

        public Archetype DeppCopy()
        {
            //Create a new Archetype
            var copy = CreateInstance<Archetype>();

            copy.tags = new List<string>(tags);

            copy.components = new List<DataComponent>();

            foreach (var component in components)
            {
                copy.components.Add(component.Clone() as DataComponent);
            }

            return copy;
        }
    }
}