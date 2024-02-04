using Assets.Scripts.Engine.ECS;

namespace Assets.Scripts.Core.ECS
{
    public interface ISystemManager
    {
        T CreateSystem<T>() where T : IGameSystem, new();
        T GetSystem<T>() where T : IGameSystem;
    }
}