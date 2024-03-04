using Assets.Scripts.Core.ECS;
using UnityEngine;


namespace Assets.Scripts.Game.Input
{
    [CreateAssetMenu(fileName = "PlayerInput", menuName = "Game/Input/PlayerInput")]
    public class PlayerInputParams : ComponentParams<PlayerInputComponent> { }
}