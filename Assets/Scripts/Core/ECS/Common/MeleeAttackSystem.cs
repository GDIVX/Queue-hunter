using Assets.Scripts.Core.ECS;
using Assets.Scripts.Core.ECS.Common;
using Assets.Scripts.Engine.ECS;
using Assets.Scripts.Engine.ECS.Common;
using Assets.Scripts.Game.Input;
using Queue.Tools.Debag;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MeleeAttackSystem : GameSystem
{
    private const string LogGroupName = "MeleeSystem";

    public MeleeAttackSystem(SignalBus signalBus) : base(signalBus)
    {
    }

    protected override bool ShouldProcessArchetype(Archetype archetype)
    {
        return archetype.HasComponents<MeleeAttackComponent, PlayerInputComponent>();
    }

    public override void OnEntityCreated(IEntity entity)
    {
        base.OnEntityCreated(entity);


        if (!entity.HasComponent<MeleeAttackComponent>() || !entity.HasComponent<GameObjectComponent>())
            return;

        var go = entity.GetComponent<GameObjectComponent>();
        var mc = entity.GetComponent<MeleeAttackComponent>();

        mc.triggerHandler = GetMeleeCollider(go);
    }

    protected override void OnUpdate(Archetype archetype)
    {
        var meleeAttBatch = archetype.GetComponents<MeleeAttackComponent>();
        var inputBatch = archetype.GetComponents<PlayerInputComponent>();

        for (int i = 0; i < archetype.Count; i++)
        {
            if (inputBatch[i].PressedKeys.Contains(KeyCode.Mouse0))
            {
                //get enemies in range and damage them
                if (meleeAttBatch[i].triggerHandler.GameObjects.Count > 0)
                {
                    foreach (var item in meleeAttBatch[i].triggerHandler.GameObjects)
                    {
                        if (item.TryGetComponent<EntityInspector>(out EntityInspector entity))
                        {
                            int id = entity.EntityID;
                            CoreLogger.Log($"hit entity with ID of {id}", "MeleeSystem");

                        }
                    }
                }
            }
        }
    }

    ColliderTriggerHandler GetMeleeCollider(GameObjectComponent goComp)
    {
        if (goComp.GameObject.TryGetComponent(out ColliderTriggerHandler handler))
        {
            CoreLogger.Log("got collider", LogGroupName);
            return handler;
        }

        var hanler = goComp.GameObject.GetComponentInChildren<ColliderTriggerHandler>();

        if (hanler == null)
        {
            CoreLogger.Log("Failed getting colliderr", LogGroupName);

            return null;
        }

        return hanler;
    }

}
