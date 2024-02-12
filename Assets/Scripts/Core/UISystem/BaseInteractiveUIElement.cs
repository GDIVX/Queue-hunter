using System;
using Sirenix.OdinInspector;
using Tzipory.Tools.TimeSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tzipory.Systems.UISystem
{
    public abstract class BaseInteractiveUIElement : BaseUIElement , IPointerEnterHandler,IPointerExitHandler , IPointerClickHandler ,IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
    {
        public event Action OnClickEvent;
        public event Action OnDragEvent;
        public event Action OnBeginDragEvent;
        public event Action OnEndDragEvent;
        public event Action OnDoubleClickEvent;
        public event Action OnEnter;
        public event Action OnExit;
        
        [SerializeField] private bool _enableDrag;
        [SerializeField,ShowIf(nameof(_enableDrag))] private CanvasGroup _canvasGroup;
        [SerializeField] private bool _enableDoubleClick;
        [SerializeField,ShowIf(nameof(_enableDoubleClick))] private float _doubleClickSpeed = 0.5f;
        
        
        private int _clickNum;

        private float _doubleClickTimer;
        
        public bool EnableDrag => _enableDrag;
        
        private void Update()
        {
            if (_clickNum == 0 || !_enableDoubleClick)
                return;
            
            _doubleClickTimer -= Time.deltaTime;
            
            if (_doubleClickTimer <= 0)
            {
                _clickNum = 0;
                _doubleClickTimer  = 0;
            }
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            OnEnter?.Invoke();
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            OnExit?.Invoke();
        }

        #region DragLogic

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            if (!_enableDrag)
                return;
            
            _clickNum = 0;
            
            _canvasGroup.alpha = 0.6f;
            _canvasGroup.blocksRaycasts = false;
            OnBeginDragEvent?.Invoke();
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            if(!_enableDrag)
                return;
            OnDragEvent?.Invoke();
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            if (!_enableDrag)
                return;
            
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;
            
            OnEndDragEvent?.Invoke();
        }

        #endregion
        
        public void OnPointerClick(PointerEventData eventData)
        {
            switch (_clickNum)
            {
                case 0:
                    OnClick(eventData);
                    return;
                case 1:
                    OnDoubleClick(eventData);
                    return;
            }
        }

        protected virtual void OnClick(PointerEventData eventData)
        {
            OnClickEvent?.Invoke();

            if (!_enableDoubleClick) return;
            
            _clickNum++;
            _doubleClickTimer = _doubleClickSpeed;
        }
        
        protected virtual void OnDoubleClick(PointerEventData eventData)
        {
            OnDoubleClickEvent?.Invoke();
            _doubleClickTimer = 0;
            _clickNum = 0;
        }

        public virtual void OnDrop(PointerEventData eventData)
        {
        }

        private void OnDisable()
        {
            _clickNum = 0;
            _doubleClickTimer = 0;
        }
    }
}