using System.Collections;
using Combat;
using Game.Queue;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerMovementController : MonoBehaviour
{

    #region RotationParams
    [SerializeField] float rotationSpeed = 360;

    #endregion

    #region MovementParams
    [SerializeField] float regularSpeed;
    [SerializeField] float halfSpeed;
    [SerializeField] private Vector3 lastDir;
    [SerializeField] private MarbleShooter _shooter;
    [SerializeField] LayerMask groundLayer;
    private Vector3 skewedInput;


    Vector3 movementInput;
    bool isRunning;
    bool isAttacking;
    public bool canMove = true;

    public float Speed
    {
        get => _speed;
        set => _speed = value;
    }

    public bool IsAttacking
    {
        get => isAttacking;
        set => isAttacking = value;
    }

    public UnityEvent<string, bool> onMove;
    public UnityEvent<string, bool> onMoveEnd;
    public UnityEvent<string> onDash;

    #endregion

    #region DashParams

    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;
    private bool canDash = true;
    private bool isDashing;
    private Vector3 dashDirection;
    private float dashStartTime;
    public UnityEvent<float> OnDashCDChanged;
    public UnityEvent OnDashStarted;

    #endregion


    Rigidbody rb;
    [SerializeField] private float _speed;
    [SerializeField] private float gravityFactor;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        _shooter = GetComponentInChildren<MarbleShooter>();


    }


    private void FixedUpdate()
    {
        movementInput = new Vector3(UnityEngine.Input.GetAxisRaw("Horizontal"), 0,
            UnityEngine.Input.GetAxisRaw("Vertical"));
        var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        skewedInput = matrix.MultiplyPoint3x4(movementInput);


        //move detection
        if (skewedInput != Vector3.zero && canMove)
        {
            if (isAttacking)
                RotateTowardsAttack();
            else
                RotateForward();
            lastDir = skewedInput.normalized;
            Move();
        }
        else
        {
            rb.velocity = Vector3.zero;
            onMoveEnd?.Invoke("isRunning", false);
        }

        if (isDashing) DuringDash();
    }

    #region MoveFunctions

    void Move()
    {
        rb.velocity = new Vector3(lastDir.x * Time.fixedDeltaTime * (Speed * 100),
            rb.velocity.y,
            lastDir.z * Time.fixedDeltaTime * (Speed * 100));
        rb.AddForce(new Vector3(0, -1, 0) * gravityFactor, ForceMode.Acceleration);
        onMove?.Invoke("isRunning", true);
    }

    void RotateForward()
    {
        if (isAttacking) return;

            Quaternion targetRotation = Quaternion.LookRotation(skewedInput);


        targetRotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(targetRotation);
    }

    public void DisableMovement()
    {
        canMove = false;
    }

    public void EnableMovement()
    {
        canMove = true;
    }

    public void SetSlowSpeed()
    {
        Speed = halfSpeed;
    }

    public void SetRegularSpeed()
    {
        Speed = regularSpeed;
    }

    public void SetShooting(bool shooting)
    {
        isAttacking = shooting;
        if (isDashing) EndDash();
    }

    public void RotateTowardsAttack()
    {
        Ray cameraRay;

        // Cast a ray from the camera to the mouse cursor
        cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(cameraRay, out var hitInfo, Mathf.Infinity, groundLayer))
        {
            Vector3 targetPosition = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
            Vector3 dir = (targetPosition - transform.position).normalized;
            dir.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(dir);


            targetRotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                (rotationSpeed));
            rb.MoveRotation(targetRotation);
        }
    }

    #endregion

    #region DashFunctions

    public void StartDash()
    {
        if (!canDash) return;
        canDash = false;
        isDashing = true;
        isAttacking = false;

        OnDashStarted?.Invoke();

        //animation trigger
        onDash?.Invoke("DashTrigger");

        // Get the dash direction based on player input
        dashDirection = lastDir;

        // Record start time of dash
        dashStartTime = Time.fixedTime;
        OnDashCDChanged?.Invoke(dashCooldown);
    }

    void DuringDash()
    {
        float dashTimeElapsed = Time.fixedTime - dashStartTime;
        if (dashTimeElapsed < dashDuration)
        {
            // Calculate progress of dash
            float t = dashTimeElapsed / dashDuration;
            //Rotate player towards dash direction
            Quaternion targetRotation = Quaternion.LookRotation(skewedInput);


            targetRotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                (rotationSpeed) * Time.fixedDeltaTime);
            rb.MoveRotation(targetRotation);
            //Actual dash logic
            rb.velocity = dashDirection * Time.fixedDeltaTime * (Speed * 400);
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