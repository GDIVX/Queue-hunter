using Assets.Scripts.Core.ECS;
using Assets.Scripts.Engine.ECS;
using Assets.Scripts.Engine.ECS.Common;
using Assets.Scripts.Game.Input;
using Assets.Scripts.Game.Movement;
using Queue.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using static UnityEngine.EventSystems.EventTrigger;

public class DashSystem : GameSystem
{
    public DashSystem(SignalBus signalBus) : base(signalBus)
    {
    }
    protected override bool ShouldProcessArchetype(Archetype archetype)
    {
        return archetype.HasComponents<DashComponent, PlayerInputComponent, PositionComponent, MovementParamsComponent>();
    }

    //protected override bool ShouldProcessEntity(IEntity entity)
    //{
    //    return entity.HasComponent<PlayerDash, PlayerInput>();
    //}

    //protected override void OnUpdate(IEntity entity)
    //{
    //    //dash detection
    //    if (entity.GetComponent<PlayerDash>().CanDash && entity.GetComponent<PlayerInput>().PressedKey == KeyCode.Space)
    //    {
    //        StartDash(entity);
    //    }

    //    if (entity.GetComponent<PlayerDash>().IsDashing)
    //    {
    //        DuringDash(entity);
    //    }
    //}

    protected override void OnUpdate(Archetype archetype) 
    {
        var playerDashBatch = archetype.GetComponents<DashComponent>();

        var playerInputBatch = archetype.GetComponents<PlayerInputComponent>();

        var positionBatch = archetype.GetComponents<PositionComponent>();

        var moveParamsBatch = archetype.GetComponents<MovementParamsComponent>();

        for (int i = 0; i < archetype.Count; i++)
        {
            if (playerDashBatch[i].CanDash && playerInputBatch[i].PressedKey == KeyCode.Space)
            {
                StartDash(playerDashBatch[i], moveParamsBatch[i]);
            }

            if (playerDashBatch[i].IsDashing)
            {
                DuringDash(playerDashBatch[i], positionBatch[i]);
            }
        }
    }


    #region DASH_FUNCS
    void StartDash(DashComponent playerDash, MovementParamsComponent movementParams)
    {
        playerDash.CanDash = false;
        playerDash.IsDashing = true;

        // Get the dash direction based on player input
        playerDash.DashDirection = movementParams.LastDir;

        // Record start time of dash
        playerDash.DashStartTime = Time.time;
    }

    void DuringDash(DashComponent playerDash, PositionComponent posComp)
    {
        float dashTimeElapsed = Time.time - playerDash.DashStartTime;
        if (dashTimeElapsed < playerDash.DashDuration)
        {
            // Calculate progress of dash
            float t = dashTimeElapsed / playerDash.DashDuration;
            // Apply the lerp to move the player
            posComp.Position = Vector3.Lerp(posComp.Position, posComp.Position + playerDash.DashDirection * playerDash.DashDistance, t);
        }
        else
        {
            // End dash
            EndDash(playerDash);
        }
    }

    void EndDash(DashComponent playerDash)
    {
        playerDash.IsDashing = false;
        CoroutineHelper.Instance.StartCoroutine(ResetDash(playerDash));
    }


    public IEnumerator ResetDash(DashComponent playerDash)
    {
        yield return new WaitForSeconds(playerDash.DashCooldown);
        playerDash.CanDash = true;
    }


    #endregion

}
