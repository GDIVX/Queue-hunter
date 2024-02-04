using Assets.Scripts.Engine.ECS;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ArchetypeLibrary", menuName = "ECS/ArchetypeLibrary")]
public class ArchetypeLibrary : ScriptableObject
{
    public List<Archetype> archetypes;
}

