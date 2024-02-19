using Assets.Scripts.Core.ECS.Interfaces;
using Assets.Scripts.Engine.ECS;
using Sirenix.OdinInspector;
using System;
using System.Reflection;
using UnityEngine;

namespace Assets.Scripts.Core.ECS

{
    public abstract class DataComponent : ScriptableObject, IComponent, IDirty
    {
        [InfoBox("$InfoBoxMessage")]

        private bool isDirty;

        [SerializeField] bool _isActive = true;

        public IEntity Entity { get; private set; }

        [ShowInInspector]
        public bool IsDirty
        {
            get => isDirty; set
            {
                isDirty = value;
                OnDirty?.Invoke(this, isDirty);
            }
        }
        protected virtual string InfoBoxMessage => "";

        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                isDirty = true;
            }
        }

        public event Action<DataComponent, bool> OnDirty;
        public event Action<DataComponent> OnDestroyed;

        public T Instantiate<T>() where T : ScriptableObject, IComponent
        {
            T original = this as T;

            if (original == null)
            {
                Debug.LogError($"Failed to cast {this} to {typeof(T).Name}");
                return null;
            }

            T clone = Instantiate(original);
            CopyFields(original, clone);

            if (clone == null)
            {
                Debug.LogError($"Failed to clone {nameof(DataComponent)}");
                return null;
            }
            return clone;
        }

        private void CopyFields<T>(T source, T destination) where T : ScriptableObject
        {
            // Get all fields of the object
            FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var field in fields)
            {
                object value = field.GetValue(source);

                if (value is ICloneable cloneable)
                {
                    // If the field type supports cloning, clone it
                    field.SetValue(destination, cloneable.Clone());
                }
                else
                {
                    // Otherwise, just set the value (works for value types and strings)
                    field.SetValue(destination, value);
                }
            }
        }
        public IEntity GetParent()
        {
            return Entity;
        }

        virtual public void Initialize() { }

        public void SetParent(IEntity entity)
        {
            Entity = entity;
        }

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }

        public DataComponent FromJson(string json)
        {
            JsonUtility.FromJsonOverwrite(json, this);
            return this;
        }

        /// <summary>
        /// Helper method to set a value and mark the component as dirty
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reference">Reference to the field to set to</param>
        /// <param name="value">the value to set to</param>
        /// <returns></returns>
        protected T SafeSet<T>(ref T reference, T value)
        {
            if (!IsActive) return reference;

            reference = value;
            IsDirty = true;
            return reference;
        }

        private void OnDestroy()
        {
            OnDestroyed?.Invoke(this);
            Entity.RemoveComponent(this);
        }

    }
}