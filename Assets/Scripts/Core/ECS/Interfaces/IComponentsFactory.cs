using Assets.Scripts.Core.ECS.Interfaces;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Core.ECS
{
    public interface IComponentsFactory
    {
        T Create<T>(string address, T loadedComponent) where T : ScriptableObject, IComponent;
        Task<T> CreateComponentAsync<T>(string address) where T : ScriptableObject, IComponent;
        T Instantiate<T>(T loadedComponent) where T : ScriptableObject, IComponent;
    }
}