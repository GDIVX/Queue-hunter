using Assets.Scripts.Core.ECS;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Scripts.Engine.ECS.Common
{
    [CreateAssetMenu(fileName = "ModelComponent", menuName = "ECS/Components/Model")]
    public class ModelComponent : DataComponent
    {
        [SerializeField] private GameObject model;

        public GameObject Model
        {
            get
            {
                return model;
            }
            set
            {
                //if the entity is disabled, it is read only
                if (IsActive) model = value;
            }
        }

        private void OnDestroy()
        {
            Destroy(model);
        }
    }
}