using Assets.Scripts.Core.ECS;
using UnityEngine;


namespace Assets.Scripts.Engine.ECS.Common
{
    [CreateAssetMenu(fileName = "RotationComponent", menuName = "ECS/Components/RotationComponent")]
    public class RotationParams : ComponentParams<RotationComponent> { }
}