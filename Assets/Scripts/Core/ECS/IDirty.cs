using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Engine.ECS
{
    public interface IDirty
    {
        bool IsDirty { get; set; }
    }
}