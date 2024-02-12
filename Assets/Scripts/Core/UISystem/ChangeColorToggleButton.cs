using UnityEngine;
using UnityEngine.UI;

namespace Tzipory.Systems.UISystem
{
    public abstract class ChangeColorToggleButton : BaseInteractiveToggelButtonUI
    {
        [SerializeField] private Image _image;
        [SerializeField] private Color onColor;
        [SerializeField] private Color offColor;
        
        public override void OnChangeToOnState()
        {
            _image.color = onColor;
            On();
        }

        public override void OnChangeToOffState()
        {
            _image.color = offColor;
            Off();
        }

        protected abstract void On();
        protected abstract void Off();
    }
}