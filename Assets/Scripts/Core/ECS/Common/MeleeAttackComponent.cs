using Assets.Scripts.Core.ECS;
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

}
