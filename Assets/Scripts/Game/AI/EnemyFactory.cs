using Combat;
using Game.Combat;
using Game.Utility;
using UnityEngine;
using Utility;

namespace AI
{
    public class EnemyFactory : IGameObjectFactory<IEnemyModel, Enemy>
    {
        public Enemy Create(IEnemyModel model, Vector3 position)
        {
            //instantiate a new prefab
            GameObject prefab = Object.Instantiate(model.Prefab, position, Quaternion.identity);
            var enemy = prefab.GetComponent<Enemy>();

            foreach (var init in prefab.GetComponents<IInit<IEnemyModel>>())
            {
                init.Init(model);
            }

            return enemy;
        }
    }
}