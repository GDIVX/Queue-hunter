using Assets.Scripts.Game.Movement;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Core.ECS.Common
{
    [CreateAssetMenu(fileName = "MovementParams", menuName = "Game/Movement/Params")]
    public class MovementParamsComAsset : ComponentAsset<MovementParamsComponent>
    {
        public override object Instantiate()
        {
            return Value.Clone();
        }

    }
}