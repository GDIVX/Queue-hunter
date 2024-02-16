using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using Tzipory.Systems.SaveLoadSystem.SaveSystemJson;
using Tzipory.Tools.Debag;
using UnityEditor;
using UnityEngine;

namespace Tzipory.Editor
{
    public class LogGroupEditor : OdinMenuEditorWindow
    {
        private static List<LogGroup> _logGroups;
        
        private LogGroupSerialize _newLogGroupSerialize;
        
        [MenuItem("Tools/Log Editor")]
        private static void OpenWindows()
        {
            var window = GetWindow<LogGroupEditor>();
            
            window.position = new Rect(200,200,1000,1000);
            window.Show();
        }
        
        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            _newLogGroupSerialize = new LogGroupSerialize(CreateNewLogGroup);
            
            if (GameSaveUtilityJson.LoadObjects($"{Application.dataPath}/GameSetting/LogGroups",out IEnumerable<LogGroup> saveData))
                _logGroups = saveData.ToList();
            else
                CoreLogger.LogError("Can not load log group to the log editor");
            
            tree.Add("Create log group",_newLogGroupSerialize);

            IEnumerable<LogGroup> logGroups;

            if (Application.isPlaying)
                logGroups = CoreLogger.LogGroups.Values;
            else
                logGroups = _logGroups;
            
            foreach (var logGroup in logGroups)
            {
                var logGroupGroupName = logGroup.Name ?? "";
                
                tree.Add(logGroupGroupName, logGroup);
            }   
            
            return tree;
        }
        
        protected override void OnBeginDrawEditors()
        {
            base.OnBeginDrawEditors();
            OdinMenuTreeSelection selection  = MenuTree.Selection;
        
            SirenixEditorGUI.BeginHorizontalToolbar();
            {
                GUILayout.FlexibleSpace();
            
                if (SirenixEditorGUI.ToolbarButton("Enable All"))
                {
                    foreach (var logGroup in _logGroups)
                        logGroup.IsActive = true;
                    
                    GameSaveUtilityJson.SaveObjects(CoreLogger.LOGGroupPath,_logGroups);
                }
                
                if (SirenixEditorGUI.ToolbarButton("Disable All"))
                {
                    foreach (var logGroup in _logGroups)
                        logGroup.IsActive = false;
                    
                    GameSaveUtilityJson.SaveObjects(CoreLogger.LOGGroupPath,_logGroups);
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        
        }
        
        public static void DeleteGroup(LogGroup logGroup)
        {
            if (!_logGroups.Contains(logGroup))
                throw  new System.Exception($"Log group {logGroup.Name} not found!");
                    
            _logGroups.Remove(logGroup);

            GameSaveUtilityJson.DeleteObject($"{CoreLogger.LOGGroupPath}/{logGroup.Name}.json");
            GameSaveUtilityJson.DeleteObject($"{CoreLogger.LOGGroupPath}/{logGroup.Name}.json.meta");
        }
        
        private void CreateNewLogGroup(LogGroup logGroup)
        {
            GameSaveUtilityJson.SaveObject(CoreLogger.LOGGroupPath,logGroup);
            BuildMenuTree();
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            SaveLogData();
            
            if (_newLogGroupSerialize is not null)
                _newLogGroupSerialize = null;
        }

        private static void SaveLogData()
        {
            GameSaveUtilityJson.SaveObjects(CoreLogger.LOGGroupPath,_logGroups);
        }
        
        [System.Serializable]
        private class LogGroupSerialize
        {
            private Action<LogGroup> _onSave;

            [SerializeField] private string _name = "New Log Group";
            [SerializeField] private Color _color;
            
            public LogGroupSerialize(Action<LogGroup> onSave)
            {
                _onSave = onSave;
            }

            [Button]
            private void Save()
            {
                if (_name is null || _name.Length == 0)
                {
                    Debug.LogError($"Log group as no Name!");
                    return;
                }
                
                LogGroup  LOGGroup = new LogGroup()
                {
                    Name = _name,
                    Color = _color
                };
                
                _onSave.Invoke(LOGGroup);
            }
        }
    }

    
}