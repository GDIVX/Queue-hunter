using System.Collections;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    #region MovementParams

    public float speed;
    [SerializeField] float rotationSpeed = 360;
    [SerializeField] private Vector3 lastDir;
    Vector3 movementInput;
    Vector3 relative;
    bool isRunning;
    public bool canMove = true;
    public bool canRotate = true;

    #endregion

    #region DashParams

    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;
    private bool canDash = true;
    private bool isDashing;
    private Vector3 dashDirection;
    private float dashStartTime;

    #endregion

    Animator anim;

    Rigidbody rb;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        movementInput = new Vector3(UnityEngine.Input.GetAxisRaw("Horizontal"), 0,
            UnityEngine.Input.GetAxisRaw("Vertical"));
        var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        var skewedInput = matrix.MultiplyPoint3x4(movementInput);

        //rot detection
        if (skewedInput != Vector3.zero && canMove)
        {
            relative = GetRelativeRotation();
            UpdateRotation(relative, rotationSpeed);
        }

        //move detection
        if (skewedInput != Vector3.zero && canMove)
        {
            lastDir = skewedInput;
            Move();
        }
        else
        {
            rb.velocity = Vector3.zero;
            anim.SetBool("isRunning", false);
        }

        if (isDashing) DuringDash();
        
    }

    #region MoveFunctions

    void Move()
    {
        rb.velocity = lastDir * Time.deltaTime * (speed*100);
        anim.SetBool("isRunning", true);
    }

    Vector3 GetRelativeRotation()
    {
        var relative = (transform.position + lastDir) - transform.position;
        return relative;
    }

    public void UpdateRotation(Vector3 relative, float rotSpeed)
    {
        if (relative != Vector3.zero && canRotate)
        {
            var rot = Quaternion.LookRotation(relative, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, rotSpeed * Time.deltaTime);
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
        dashStartTime = Time.fixedTime;
    }

    void DuringDash()
    {
        float dashTimeElapsed = Time.fixedTime - dashStartTime;
        if (dashTimeElapsed < dashDuration)
        {
            // Calculate progress of dash
            float t = dashTimeElapsed / dashDuration;
            //Rotate player towards dash direction
            relative = GetRelativeRotation();
            UpdateRotation(relative, rotationSpeed);
            //Actual dash logic
            rb.velocity = dashDirection * Time.fixedDeltaTime * (speed * 400);
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