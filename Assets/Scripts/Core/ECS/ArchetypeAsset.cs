using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Scripts.Core.ECS
{
    [CreateAssetMenu(fileName = "ArchetypeAsset", menuName = "ECS/ArchetypeAsset")]
    public class ArchetypeAsset : ScriptableObject
    {
        public string Name;
        public List<string> Tags;
        public List<AssetReferenceT<DataComponent>> components;
    }
}