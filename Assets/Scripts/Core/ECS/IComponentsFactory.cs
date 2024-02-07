using Assets.Scripts.Core.ECS.Interfaces;
using System.Threading.Tasks;
using Zenject;

namespace Assets.Scripts.Core.ECS
{
    public interface IComponentsFactory
    {
        Task<T> CreateComponentAsync<T>(string address) where T : class, IComponent;

    }
}