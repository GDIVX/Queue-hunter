using Sirenix.OdinInspector;
using Tzipory.Systems.SaveLoadSystem;
using Tzipory.Systems.SaveLoadSystem.SaveSystemJson;
using UnityEngine;

namespace Tzipory.Tools.Debag
{
    [System.Serializable]
    public class LogGroup : ISave
    {
        [SerializeField,HideInInspector] public string Name;
        [SerializeField,OnValueChanged(nameof(Save))] public bool IsActive;
        [SerializeField,OnValueChanged(nameof(Save))] public Color Color;
        public string ObjectName => Name;

        [Button]
        public void Delete()
        {
            GameSaveUtilityJson.DeleteObject(CoreLogger.LOGGroupPath,this);
        }

        private void Save()
        {
            GameSaveUtilityJson.SaveObject(CoreLogger.LOGGroupPath,this);
        }
    }
}