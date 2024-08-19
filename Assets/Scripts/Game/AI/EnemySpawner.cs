using Combat;
using UnityEngine;

namespace AI
{
    public class EnemySpawner
    {
        public int Count => _pool.CountOffPool;
        private readonly GameObjectPool<EnemyModel, Enemy> _pool;

        public EnemySpawner()
        {
            EnemyFactory factory = new();
            EnemyRecycler recycler = new();

            _pool = new(factory, recycler);
        }

        public Enemy Spawn(EnemyModel model, Vector3 position)
        {
            return _pool.Get(model, position);
        }

        public void Return(Enemy enemy)
        {
            _pool.Return(enemy);
        }
    }
}