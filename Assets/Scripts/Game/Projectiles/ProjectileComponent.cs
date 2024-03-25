using Assets.Scripts.Core.ECS;
using Assets.Scripts.Core.ECS.Interfaces;
using UnityEngine.Serialization;

namespace Game.Projectiles
{
    [System.Serializable]
    public class ProjectileComponent : DataComponent
    {
        public float speed;

        public override IComponent Instantiate()
        {
            ProjectileComponent component = new()
            {
                speed = speed,
            };
            return component;
        }
    }
}