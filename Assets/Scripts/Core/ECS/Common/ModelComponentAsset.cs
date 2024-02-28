using Assets.Scripts.Core.ECS;
using Assets.Scripts.Core.ECS.Common;
using UnityEngine;

namespace Assets.Scripts.Engine.ECS.Common
{
    [CreateAssetMenu(fileName = "ModelComponent", menuName = "ECS/Components/Model")]
    public class ModelComponentAsset : ComponentAsset<ModelComponent>
    {

        public override object Instantiate()
        {
            return _value.Clone();
        }

    }

   
}