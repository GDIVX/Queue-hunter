
using Assets.Scripts.Core.ECS.Interfaces;
using Assets.Scripts.Engine.ECS;

namespace Assets.Scripts.ECS
{
    public interface ITag : IActivable
    {
        string Name { get; }
        ITag Copy();
    }
}