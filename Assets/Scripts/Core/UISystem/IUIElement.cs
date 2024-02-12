using Tzipory.Tools.Enums;

namespace Tzipory.Systems.UISystem
{
    public interface IUIElement 
    {
        public string ElementName { get; }
        public UIGroup UIGroupTags { get; }
        
        void Show();
        void Hide();
        void UpdateUIVisual();
    }
}