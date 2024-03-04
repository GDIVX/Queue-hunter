using Assets.Scripts.Core.ECS.Interfaces;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Core.ECS
{
    public class ComponentParams<T> : ComponentParams where T : IComponent
    {
        [SerializeField] T component;
        public T Component
        {
            get
            {
                return (T)component.Instantiate();
            }
        }

        public override IComponent GetComponent()
        {
            return Component;
        }
    }

    public abstract class ComponentParams : ScriptableObject
    {
        IComponent component;

        public abstract IComponent GetComponent();
    }
}