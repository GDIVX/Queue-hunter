using System;
using UnityEngine;

namespace Combat
{
    public interface ITarget : IDestroyable
    {
        Vector3 Position { get; }
        IDamageable Damageable { get; }
        bool CompareTag(string tag);

    }
}