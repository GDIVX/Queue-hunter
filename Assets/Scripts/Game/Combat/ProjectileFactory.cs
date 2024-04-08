using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Utility;

namespace Combat
{
    public class ProjectileFactory : MonoBehaviour, IGameObjectFactory<ProjectileModel, Projectile>,
        IGameObjectRecycler<ProjectileModel, Projectile>
    {
        public Projectile Create(ProjectileModel model, Vector3 position)
        {
            //create the view game object as the basis for the projectile
            GameObject _gameObject = Instantiate(model.ViewPrefab, position, quaternion.identity);

            //add and initialize projectile movement and dmage on collision
            var movement = _gameObject.AddComponent<ProjectileMovement>();
            movement.Initialize(model.Speed);
            var damageOnCollision = _gameObject.AddComponent<DamageOnCollision>();
            damageOnCollision.Initialize(model.Damage, model.LookupTagForTarget);
            var projectile = _gameObject.AddComponent<Projectile>();
            projectile.Initialize(movement, damageOnCollision);
            return projectile;
        }

        public Projectile Recycle(Projectile inInstance, ProjectileModel model, Vector3 position)
        {
            GameObject inGameObject = inInstance.gameObject;
            if (inGameObject.TryGetComponent(out ProjectileMovement movement))
            {
                movement.Initialize(model.Speed);
            }
            else
            {
                inInstance.AddComponent<ProjectileMovement>().Initialize(model.Speed);
            }

            if (inGameObject.TryGetComponent(out DamageOnCollision damageOnCollision))
            {
                damageOnCollision.Initialize(model.Damage, model.LookupTagForTarget);
            }
            else
            {
                inGameObject.AddComponent<DamageOnCollision>().Initialize(model.Damage, model.LookupTagForTarget);
            }

            inGameObject.transform.position = position;

            return inInstance;
        }
    }
}