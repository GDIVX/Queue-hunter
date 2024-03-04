using Assets.Scripts.Core.ECS;
using UnityEngine;

namespace Assets.Scripts.Game.Movement
{
    [CreateAssetMenu(fileName = "Movement", menuName = "Game/Movement/MovementParams")]
    public class MovementParams : ComponentParams<MovementComponent>
    {
    }
}