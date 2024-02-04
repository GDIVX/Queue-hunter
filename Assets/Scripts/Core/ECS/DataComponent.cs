using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Assets.Scripts.Engine.ECS

{
    public abstract class DataComponent : ScriptableObject, IComponent, IDirty
    {
        [InfoBox("$InfoBoxMessage")]

        private bool isDirty;

        [SerializeField] bool _isActive = true;

        public Entity Entity { get; private set; }

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

        public IComponent Clone()
        {
            var clone = Instantiate(this);

            if (clone == null)
            {
                Debug.LogError($"Failed to clone {nameof(DataComponent)}");
                return null;
            }

            clone.Initialize();
            return clone;
        }
        public Entity GetParent()
        {
            return Entity;
        }

        virtual public void Initialize() { }

        public void SetParent(Entity entity)
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