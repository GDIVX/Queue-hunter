using UnityEngine;
using Sirenix.OdinInspector;

namespace Assets.Scripts.Core.ECS.Common
{
    [CreateAssetMenu(fileName = "GameObjectComponent", menuName = "ECS/GameObjectComponent")]
    public class GameObjectComponentAsset : ComponentAsset<GameObjectComponent>
    {

        public override object Instantiate()
        {
            GameObject gameObject = new(_value.Name);
            GameObjectComponent component = new(gameObject, _value.Name);
            return component;

        }
    }
}