using Assets.Scripts.Engine.ECS;
using System;

namespace Assets.Scripts.Core.ECS.Interfaces
{
    public interface IComponent : IActivable, IDirty, ICloneable 
    {

    }
}