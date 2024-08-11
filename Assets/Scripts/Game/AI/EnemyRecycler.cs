using Combat;
using Game.Combat;
using Game.Utility;
using UnityEngine;
using Utility;

namespace AI
{
    public class EnemyRecycler : IGameObjectRecycler<EnemyModel , Enemy>
    {

        public Enemy Recycle(Enemy inInstance, EnemyModel model, Vector3 position)
        {

            inInstance.transform.position = position;
            
            foreach (IInit<IEnemyModel> init in inInstance.GetComponents<IInit<IEnemyModel>>())
            {
                init.Init(model);
            }
            
            return inInstance.GetComponent<Enemy>();
        }
    }
}