using System;
using Queue.GameplayLogic.Managers.MainGameManagers;
using Queue.Tools.Enums;
using UnityEngine;

namespace Queue.Systems.UISystem
{
    public abstract class BaseUIElement : MonoBehaviour, IUIElement
    {
        public Action OnShow;
        public Action OnHide;
        
        [SerializeField] private bool _showOnAwake = false;
        [SerializeField] private UIGroup _uiGroupTags;
        
        [SerializeField,HideInInspector] protected RectTransform RectTransform;
        
        public string ElementName => gameObject.name;
        public UIGroup UIGroupTags => _uiGroupTags;

        public bool IsInitialization { get; protected set; }
        
        protected virtual void Awake()
        {
            UIManager.AddUIElement(this,UIGroupTags);
            
            if (_showOnAwake)
                Show();
            else
                gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            UIManager.RemoveUIElement(this);
            Hide();
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
            OnShow?.Invoke();
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
            OnHide?.Invoke();
        }

        public virtual void UpdateUIVisual()
        {
            
        }

        public virtual void Init()
        {
            IsInitialization = true;
        }

        private void OnValidate()
        {
            RectTransform ??= GetComponent<RectTransform>();
        }
    }
}