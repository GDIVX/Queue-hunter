
using Assets.Scripts.Engine.ECS;

namespace Assets.Scripts.ECS
{
    public interface ITag : IComponent
    {
        string Name { get; }
    }
}