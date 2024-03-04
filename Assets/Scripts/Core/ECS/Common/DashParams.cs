using Assets.Scripts.Core.ECS;
using UnityEngine;

namespace Assets.Scripts.Game.Movement
{
    [CreateAssetMenu(fileName = "Movement", menuName = "Game/Movement/PlayerDash")]
    public class DashParams : ComponentParams<DashComponent>
    {
    }
}
