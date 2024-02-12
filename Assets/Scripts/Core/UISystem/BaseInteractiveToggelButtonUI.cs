using System;
using UnityEngine;

namespace Tzipory.Systems.UISystem
{
    public abstract class BaseInteractiveToggelButtonUI : BaseInteractiveUIElement
    {
        public event Action<ButtonState> OnChangeState;
        [SerializeField] private bool _disableOffToggel;
        [SerializeField] private  ButtonState _startStat = ButtonState.Off;
        
        private ButtonState _state;

        public ButtonState State => _state;

        private void Start()
        {
            _state = _startStat;
            ChangeState(_state);
            OnClickEvent += ToggleStat;
        }

        private void OnDisable()
        {
            OnClickEvent -= ToggleStat;
        }

        public void ChangeState(ButtonState state)
        {
            switch (state)
            {
                case ButtonState.Off:
                    OnChangeToOffState();
                    break;
                case ButtonState.On:
                    OnChangeToOnState();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
            
            _state = state;
            OnChangeState?.Invoke(_state);
        }

        private void ToggleStat()
        {
            if (_state == ButtonState.Off)
            {
                _state = ButtonState.On;
                OnChangeToOnState();
            }
            else
            {
                if (_disableOffToggel)
                    return;
                _state = ButtonState.Off;
                OnChangeToOffState();
            }
            
            OnChangeState?.Invoke(_state);
        }

        public abstract void OnChangeToOnState();
        public abstract void OnChangeToOffState();
    }

    public enum ButtonState
    {
        Off,
        On
    }
}