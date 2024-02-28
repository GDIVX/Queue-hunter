using Assets.Scripts.Game.Input;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Core.ECS.Common
{
    [CreateAssetMenu(fileName = "Player Input", menuName = "Game/InputComponent")]
    public class PlayerInputAsset : ComponentAsset<PlayerInputComponent>
    {
        public override object Instantiate()
        {
            return Value.Clone();
        }
    }
}