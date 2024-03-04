using Assets.Scripts.Core.ECS.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Scripts.Core.ECS
{
    public class ComponentsFactory : IComponentsFactory
    {
        private Dictionary<string, IComponent> _components = new();


        public async Task<T> CreateComponentAsync<T>(string address) where T : IComponent
        {
            if (_components.TryGetValue(address, out IComponent cachedComponent))
            {
                // Ensure the cached component is of the expected type.
                if (cachedComponent is not T typedComponent)
                {
                    Debug.LogError($"Type mismatch for cached component at address {address}. Expected: {typeof(T)}, Found: {cachedComponent.GetType()}");
                    return default;
                }
                return (T)typedComponent.Instantiate();
            }

            var loadedAsset = await Addressables.LoadAssetAsync<UnityEngine.Object>(address).Task;
            if (loadedAsset is T loadedComponent)
            {
                return Create(address, loadedComponent);
            }
            else if (loadedAsset != null)
            {
                Debug.LogError($"Type mismatch for loaded component at address {address}. Expected: {typeof(T)}, Found: {loadedAsset.GetType()}");
                Addressables.Release(loadedAsset); // Release the asset if it's not of the expected type
            }

            Debug.LogError($"Could not find component with address {address}.");
            return default;
        }

        public T Create<T>(string address, T loadedComponent) where T : IComponent
        {
            _components[address] = loadedComponent;
            return Instantiate(loadedComponent);
        }

        public T Instantiate<T>(T loadedComponent) where T : IComponent
        {
            return (T)loadedComponent.Instantiate();
        }
    }
}
