using Assets.Scripts.Engine.ECS;
using Zenject;

namespace Assets.Scripts.Core.ECS
{
    public interface ISystemManager 
    {
        T Create<T>() where T : IGameSystem;
        T GetSystem<T>() where T : IGameSystem;
    }
}