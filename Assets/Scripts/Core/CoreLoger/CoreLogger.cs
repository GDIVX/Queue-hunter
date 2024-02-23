using System.Collections.Generic;
using Queue.Systems.SaveLoadSystem.SaveSystemJson;
using Queue.Helpers;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Queue.Tools.Debag
{
#if UNITY_EDITOR 
    [InitializeOnLoad]
#endif
    public class CoreLogger
    {
        public static readonly string LOGGroupPath = $"{Application.dataPath}/GameSetting/LogGroups";

        public static readonly Dictionary<string, LogGroup> LogGroups;
        
        static CoreLogger()
        {
            if (GameSaveUtilityJson.LoadObjects(LOGGroupPath,out IEnumerable<LogGroup> saveData))
            {
                LogGroups = new  Dictionary<string, LogGroup>();

                foreach (var logGroup in saveData)
                    LogGroups.Add(logGroup.Name,logGroup);                   
               
                Log("Load log group");
            }
            else
            {
                LogGroups = new  Dictionary<string, LogGroup>();
                LogWarning("No log group found!");
            }
        }

        public static void Log(object message, string groupName = null)
        {
#if UNITY_EDITOR
            if (groupName == null)
            {
                Debug.Log(message);
                return;
            }
            
            if (!LogGroups.TryGetValue(groupName,out LogGroup logGroup))
            {
                //group not found!
                Debug.LogWarning($"Log group {groupName} not found! from object {message.GetType().Name}");
                Debug.Log(message);
                return;
            }

            if (!logGroup.IsActive)
                return;
            
            Debug.Log(ProcessMessage(message, logGroup));
#endif
        }
       
        public static void LogWarning(object message, string groupName = null)
        {
#if UNITY_EDITOR
            if (groupName == null)
            {
                Debug.LogWarning(message);
                return;
            }
            
            if (!LogGroups.TryGetValue(groupName,out LogGroup logGroup))
            {
                Debug.LogWarning($"Log group {groupName} not found! from object {message.GetType().Name}");
                Debug.LogWarning(message);
                return;
            }

            if (!logGroup.IsActive)
                return;
            
            Debug.LogWarning(ProcessMessage(message, logGroup));
#endif
        }

        public static void LogError(object message, string groupName = null)//may make problem!! in build
        {
            if (groupName == null)
            {
                Debug.LogError(message);
                return;
            }
            
            if (!LogGroups.TryGetValue(groupName,out LogGroup logGroup))
            {
                Debug.LogWarning($"Log group {groupName} not found! from object {message.GetType().Name}");
                Debug.LogError(message);
                return;
            }

            if (!logGroup.IsActive)
                return;
            
            Debug.LogError(ProcessMessage(message, logGroup));
        }

        private static string ProcessMessage(object message, LogGroup logGroup)
        {
            return logGroup == null ? message.ToString() : $"<color=#{logGroup.Color.ToRGBHex()}>{logGroup.Name}:</color> : {message}";
        }
    }
}