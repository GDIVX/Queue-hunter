using Assets.Scripts.Core.ECS;
using UnityEngine;

namespace Game.Combat
{
    [CreateAssetMenu(fileName = "Health Component" , menuName = "Game/Health")]
    public class HealthParams : ComponentParams<HealthComponent>
    {
        
    }
}