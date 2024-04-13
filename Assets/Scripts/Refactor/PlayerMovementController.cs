using Assets.Scripts.Core.ECS.Common;
using Assets.Scripts.Engine.ECS.Common;
using Assets.Scripts.Game.Movement;
using Queue.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    #region MovementParams
    [SerializeField] float speed;
    [SerializeField] private Vector3 lastDir;
    Vector3 movementInput;
    Vector3 relative;
    bool isRunning;
    #endregion

    #region DashParams
    [SerializeField]private float dashDuration;
    [SerializeField]private float dashDistance;
    [SerializeField]private float dashCooldown;
    private bool canDash = true;
    private bool isDashing;
    private Vector3 dashDirection;
    private float dashStartTime;
    #endregion

    Animator anim;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        movementInput = new Vector3(UnityEngine.Input.GetAxisRaw("Horizontal"), 0, UnityEngine.Input.GetAxisRaw("Vertical"));
        var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        var skewedInput = matrix.MultiplyPoint3x4(movementInput);

        //rot detection
        if (skewedInput != Vector3.zero)
        {
            relative = GetRelativeRotation();
            UpdateRotation();
        }

        //move detection
        if (skewedInput != Vector3.zero)
        {
            lastDir = skewedInput;
            Move();
        }
        else anim.SetBool("isRunning", false);

    }

    #region MoveFunctions
    void Move()
    {
        transform.position += lastDir * Time.deltaTime * speed;
        anim.SetBool("isRunning", true);
    }

    Vector3 GetRelativeRotation()
    {
        var relative = (transform.position + lastDir) - transform.position;
        return relative;
    }

    private void UpdateRotation()
    {
        if (relative != Vector3.zero)
        {
            var rot = Quaternion.LookRotation(relative, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 360 * Time.deltaTime);
        }
    }
    #endregion

    #region DashFunctions
    void StartDash(DashComponent playerDash, MovementComponent movementParams)
    {
        playerDash.CanDash = false;
        playerDash.IsDashing = true;
        playerDash.DashTrigger.Value = true;
        playerDash.DashTrigger.Value = false;
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
