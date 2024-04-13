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
    bool canMove = true;
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
        if (skewedInput != Vector3.zero && canMove)
        {
            lastDir = skewedInput;
            Move();
        }
        else anim.SetBool("isRunning", false);

        if (isDashing) DuringDash();

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
    public void StartDash()
    {
        if (!canDash) return;
        canDash = false;
        isDashing = true;

        //animation trigger
        anim.SetTrigger("DashTrigger");

        // Get the dash direction based on player input
        dashDirection = lastDir;

        // Record start time of dash
        dashStartTime = Time.time;
    }

    void DuringDash()
    {
        float dashTimeElapsed = Time.time - dashStartTime;
        if (dashTimeElapsed < dashDuration)
        {
            // Calculate progress of dash
            float t = dashTimeElapsed / dashDuration;
            // Apply the lerp to move the player
            transform.position = Vector3.Lerp(transform.position, transform.position + dashDirection * dashDistance, t);
        }
        else
        {
            // End dash
            EndDash();
        }
    }

    void EndDash()
    {
        isDashing = false;
        StartCoroutine(ResetDash());
    }


    public IEnumerator ResetDash()
    {
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    #endregion
}
