using Combat;
using Game.Combat;
using Game.Queue;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EffectPool : MonoBehaviour
{
    [SerializeField] ProjectileFactory projectileFactory;
    [SerializeField] Transform spawnPoint;
    [SerializeField] private GameObject explosionObject;
    [SerializeField] List<Combat.Projectile> effects = new List<Combat.Projectile>();
    [SerializeField] private List<GameObject> explosionEffects = new List<GameObject>();

    private void Awake()
    {
        projectileFactory = GetComponent<ProjectileFactory>();
    }

    public void ProccessQueue(MarbleQueue queue)
    {
        foreach (var item in queue._marbles)
        {
            CreateProjectile(item);
        }
    }

    public void CreateProjectile(Marble marble)
    {
        Projectile proj = projectileFactory.Create(marble.ProjectileModel, spawnPoint.position);
        effects.Add(proj);
        proj.SetAvailable(true);
        if (proj.GetProjectileType().ToString() == "Fire")
        {
            proj.gameObject.SetActive(false);
            if (proj.TryGetComponent<ProjectileCollision>(out var col))
            {
                col.explosionObject = CreateExplosion(proj.transform);
            }
        }
        else proj.gameObject.SetActive(false);
    }

    public Projectile GetProjectile(Marble marble)
    {

        foreach (var proj in effects)
        {
            if (marble.GetMarbleType().ToString() == proj.GetProjectileType().ToString() && proj.isAvailable)
            {
                proj.SetAvailable(false);
                return proj;
            }
        }
        return null;
    }

    GameObject CreateExplosion(Transform parent)
    {
        GameObject expObject = Instantiate(explosionObject, parent);
        explosionEffects.Add(expObject);
        return expObject;
    }
}
