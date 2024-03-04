using Assets.Scripts.Core.ECS;
using Assets.Scripts.Core.ECS.Interfaces;
using Assets.Scripts.Game.Input;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MeleeAttackComponent", menuName = "Game/Combat/MeleeAttackComponent")]
public class MeleeAttackComponent : DataComponent
{
    [ShowInInspector]
    Vector3 position;

    [ShowInInspector]
    float attackRange;

    public ColliderTriggerHandler triggerHandler;



    public Vector3 Position
    {
        get => position;
        set
        {
            SafeSet(ref position, value);
        }
    }

    public float AttackRange
    {
        get => attackRange;
        set
        {
            SafeSet(ref attackRange, value);
        }
    }

    public override IComponent Instantiate()
    {
        MeleeAttackComponent component = new MeleeAttackComponent()
        {
            Position = position,
            AttackRange = attackRange
        };
        return component;
    }
}
