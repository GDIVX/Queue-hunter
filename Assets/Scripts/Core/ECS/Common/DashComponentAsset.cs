using Assets.Scripts.Game.Movement;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Core.ECS.Common
{
    [CreateAssetMenu(fileName = "Movement", menuName = "Game/Movement/PlayerDash")]
    public class DashComponentAsset : ComponentAsset<DashComponent>
    {
        public override object Instantiate()
        {
            return _value.Clone();
        }
    }
}