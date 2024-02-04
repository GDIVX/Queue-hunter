using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Scripts.Engine.ECS.Common
{
    [CreateAssetMenu(fileName = "Archetype Loader", menuName = "ECS/Components/Archetype Loader Component")]
    public class ArchetypeContainerComponent : DataComponent
    {
        [SerializeField] AssetReferenceT<Archetype> _archetype;

        public AssetReferenceT<Archetype> Archetype { get => _archetype; set => _archetype = value; }
    }
}