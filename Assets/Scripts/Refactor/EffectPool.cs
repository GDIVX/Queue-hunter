using Combat;
using Game.Combat;
using Game.Queue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPool : MonoBehaviour
{
    [SerializeField] float amountToPool;
    [SerializeField] List<Combat.Projectile> effects = new List<Combat.Projectile>();
    [SerializeField] MarbleQueue queue;
    [SerializeField] ProjectileFactory projectileFactory;
    [SerializeField] Transform spawnPoint;

    private void Start()
    {
        queue = GetComponent<MarbleQueue>();
        projectileFactory = GetComponent<ProjectileFactory>();
        Init();
    }

    void Init()
    {
        amountToPool = queue.startingQueue.Count;
        for (int i = 0; i < amountToPool; i++)
        {
            var go = projectileFactory.Create(queue.startingQueue[i]._projectileModel, spawnPoint.position);
            effects.Add(go);
            go.gameObject.SetActive(true);
        }
    }

    public Projectile GetProjectile(Marble marble)
    {

        foreach (var go in effects)
        {
            if (marble.GetType().ToString() == go.GetProjectileType().ToString())
            {
                return go;
            }    
        }
        return null;
    }

}
