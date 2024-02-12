using System;
using System.Collections.Generic;
using Tzipory.Systems.UISystem;
using Tzipory.Tools.Enums;
using UnityEngine;

namespace Tzipory.GameplayLogic.Managers.MainGameManagers
{
    public class UIManager : MonoBehaviour
    {
        private static readonly Dictionary<UIGroup, List<BaseUIElement>> UIGroups = new();

        private static readonly Array UIGroupTagsArray = Enum.GetValues(typeof(UIGroup));

        public static void Init(UIGroup parameter, bool showOnInit = false, bool updateOnShow = false)
        {
            if (!UIGroups.TryGetValue(parameter, out var uiElements))
            {
                Debug.LogWarning($"Can not find UiGroup in tag {parameter}");
                return;
            }

            for (int i = 0; i < uiElements.Count; i++)
                uiElements[i].Init();
            if (showOnInit) ShowUIGroup(parameter, updateOnShow);
        }

        public static void Init()
        {
            foreach (var uiGroupsValue in UIGroups.Values)
            {
                foreach (var uiElement in uiGroupsValue)
                    uiElement.Init();
            }
        }

        public static void AddUIElement(BaseUIElement element, UIGroup group)
        {
            if (group == UIGroup.None)
            {
                Debug.LogError($"Ui element group is none at {element.ElementName}",element);
                return;
            }

            foreach (Enum uiGroupsKey in UIGroupTagsArray)
            {
                if (!group.HasFlag(uiGroupsKey)) continue;
                
                if (UIGroups.TryGetValue((UIGroup)uiGroupsKey, out var foundUIGroup))
                {
                    if (foundUIGroup.Contains(element))
                    {
                        Debug.LogWarning("UiElement already exists in this group",element);
                        return;
                    }

                    foundUIGroup.Add(element);
                }
                else
                    UIGroups.Add((UIGroup)uiGroupsKey, new List<BaseUIElement>() { element });
            }
        }

        public static void RemoveUIElement(BaseUIElement element)
        {
            var group = element.UIGroupTags;
            
            foreach (Enum uiGroupsKey in UIGroupTagsArray)
            {
                if (!group.HasFlag(uiGroupsKey)) continue;

                if (!UIGroups.TryGetValue((UIGroup) uiGroupsKey, out var foundUIGroup)) continue;
                
                if (foundUIGroup.Contains(element))
                    foundUIGroup.Remove(element);
                else
                    Debug.LogError("UiElement dont exist in this group",element);
            }
        }

        public static void ShowUIGroup(UIGroup group, bool updateOnShow = false)
        {
            if (UIGroups.TryGetValue(group, out var foundUIGroup))
            {
                for (int i = 0; i < foundUIGroup.Count; i++)
                {
                    foundUIGroup[i].Show();
                    if (updateOnShow)
                        foundUIGroup[i].UpdateUIVisual();
                }
            }
            else
                Debug.LogWarning($"Can not find {group} group");
        }

        public static void ShowUIGroups(IEnumerable<UIGroup> groups, bool updateOnShow = false)
        {
            foreach (var uiGroup in groups)
                ShowUIGroup(uiGroup, updateOnShow);
        }

        public static void HidUIGroup(UIGroup group)
        {
            if (UIGroups.TryGetValue(group, out var foundUIGroup))
            {
                foreach (var uiElement in foundUIGroup)
                    uiElement.Hide();
            }
            else
                Debug.LogWarning($"Can not find {group} group");
        }

        public static void UpdateVisualUIGroup(UIGroup group)
        {
            if (UIGroups.TryGetValue(group, out var foundUIGroup))
            {
                foreach (var uiElement in foundUIGroup)
                    uiElement.UpdateUIVisual();
            }
            else
                Debug.LogWarning($"Can not find {group} group");
        }
        
        public static void UpdateVisualUIGroups(IEnumerable<UIGroup> groups)
        {
            foreach (var group in groups)
                UpdateVisualUIGroup(group);
        }
    }
}