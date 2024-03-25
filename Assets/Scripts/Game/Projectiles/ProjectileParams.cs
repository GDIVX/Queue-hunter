using Assets.Scripts.Core.ECS;
using UnityEngine;

namespace Game.Projectiles
{
    [CreateAssetMenu(fileName = "Projectile Component" , menuName = "Game/Projectile")]
    public class ProjectileParams : ComponentParams<ProjectileComponent>
    {
        
    }
}