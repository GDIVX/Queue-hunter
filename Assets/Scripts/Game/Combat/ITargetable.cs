using System;
using UnityEngine;

namespace Combat
{
    public interface ITargetable : IDestroyable
    {
        Vector3 Position { get; }
        IDamageable Damageable { get; }
        bool CompareTag(string tag);

    }
}