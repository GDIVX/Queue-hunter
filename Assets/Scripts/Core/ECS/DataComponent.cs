using Assets.Scripts.Core.ECS.Interfaces;
using Assets.Scripts.Engine.ECS;
using Sirenix.OdinInspector;
using System;
using System.Reflection;
using UnityEngine;

namespace Assets.Scripts.Core.ECS

{
    public abstract class DataComponent : ScriptableObject
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
                OnSetActive(value);
            }
        }

        public event Action<DataComponent, bool> OnDirty;
        public event Action<DataComponent> OnDestroyed;




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

        protected virtual void OnSetActive(bool value) { }

        private void OnDestroy()
        {
            OnDestroyed?.Invoke(this);
            //Entity.RemoveComponent(this);
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}