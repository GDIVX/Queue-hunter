using Assets.Scripts.Core.ECS.Common;
using Assets.Scripts.Engine.ECS.Common;
using Assets.Scripts.Game.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] private Vector3 lastDir;
    bool isRunning;
    Vector3 movementInput;
    Vector3 relative;

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
}
