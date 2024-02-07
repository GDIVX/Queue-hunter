
using Assets.Scripts.Core.ECS.Interfaces;

namespace Assets.Scripts.ECS
{
    public interface ITag : IComponent
    {
        string Name { get; }
    }
}