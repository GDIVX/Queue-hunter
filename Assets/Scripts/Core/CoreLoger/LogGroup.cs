using Queue.Systems.SaveLoadSystem;
using Queue.Systems.SaveLoadSystem.SaveSystemJson;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Queue.Tools.Debag
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