using Assets.Scripts.Core.ECS;
using Assets.Scripts.Core.ECS.Common;
using Assets.Scripts.Engine.ECS;
using Assets.Scripts.Engine.ECS.Common;
using Assets.Scripts.Game.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MeleeAttackSystem : GameSystem
{
    public MeleeAttackSystem(SignalBus signalBus) : base(signalBus)
    {
    }

    protected override bool ShouldProcessArchetype(Archetype archetype)
    {
        return archetype.HasComponents<MeleeAttackComponent, PositionComponent, PlayerInputComponent, GameObjectComponent>();
    }

    protected override void OnUpdate(Archetype archetype)
    {
        var meleeAttBatch = archetype.GetComponents<MeleeAttackComponent>();
        var posBatch = archetype.GetComponents<PositionComponent>();    
        var inputBatch = archetype.GetComponents<PlayerInputComponent>();

        for (int i = 0; i < archetype.Count; i++)
        {
            
        }
    }

}
