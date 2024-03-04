using Assets.Scripts.Core.ECS;
using UnityEngine;

namespace Assets.Scripts.Engine.ECS.Common
{
    [CreateAssetMenu(fileName = "PositionComponent", menuName = "ECS/Components/Position")]
    public class PositionParams : ComponentParams<PositionComponent>
    {
    }
}