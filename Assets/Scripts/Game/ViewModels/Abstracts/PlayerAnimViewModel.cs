using Assets.Scripts.Core.ECS;
using Assets.Scripts.Engine.ECS;
using Assets.Scripts.Game.Movement;
using Queue.Tools.Debag;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAnimViewModel : BaseAnimViewModel
{

    private void Start()
    {
        base.Start();
        Init();

        StartCoroutine(InitCoroutine());

    }

    IEnumerator InitCoroutine()
    {
        yield return new WaitForEndOfFrame();
    }

    void Init()
    {
        IEntity entity = entityInspector.Entity;

        if (entity == null)
        {
            Debug.LogError($"Didn't find entity  ");
            return;
        }

        if (entity.TryGetComponent(out MovementComponent movement))
        {
            movement.isRunning.OnValueChanged += UpdateBool;
            CoreLogger.Log("Added Movement -> Is Running : Bool" , "Animation");
        }

        if (entity.TryGetComponent(out DashComponent dash))
        {
            dash.DashTrigger.OnValueChanged += UpdateTrigger;
            CoreLogger.Log("Added Movement -> Dash : Trigger", "Animation");
        }

        //entity.GetComponent<MovementComponent>().isRunning.OnValueChanged += UpdateBool;
    }
}
