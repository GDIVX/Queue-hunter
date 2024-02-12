using System.Globalization;
using TMPro;
using UnityEngine;

namespace Tzipory.Systems.UISystem
{
    public abstract class BaseCounterUIHandler : BaseUIElement
    {
        [SerializeField] protected TMP_Text _currentCount;
        [SerializeField] protected TMP_Text _maxCount;
        
        
        protected void UpdateUiData(float currentCunt)
        {
            _currentCount.text = currentCunt.ToString(CultureInfo.CurrentCulture);
        }
        
        protected void UpdateUiData(int currentCunt)
        {
            _currentCount.text = currentCunt.ToString();
        }
    }
}