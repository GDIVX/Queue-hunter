using Assets.Scripts.Core.ECS.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAnimViewModel : MonoBehaviour
{
    [HideInInspector]
    public EntityInspector entityInspector;

    private Animator animator;

    public virtual void Start()
    {
        entityInspector = GetComponentInParent<EntityInspector>();
        animator = GetComponent<Animator>();


    }

    public void UpdateBool(string valueName, bool value)
    {
        animator.SetBool(valueName, value);
    }

    public void UpdateFloat(string valueName, float value)
    {
        animator.SetFloat(valueName, value);
    }

    public void UpdateTrigger(string triggerName, bool value)
    {
        if (value == true)
            animator.SetTrigger(triggerName);
    }
}
