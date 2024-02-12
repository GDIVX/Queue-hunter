using UnityEngine;

namespace Tzipory.ConfigFiles.ToolTipConfig
{
    [CreateAssetMenu(fileName = "NewToolTipConfig", menuName = "ScriptableObjects/NewToolTipConfig", order = 0)]
    public class ToolTipConfig : ScriptableObject
    {
        [SerializeField] private string[] _toolTips;

        public string GetToolTip()
        {
            return _toolTips[Random.Range(0, _toolTips.Length)];
        }
    }
}