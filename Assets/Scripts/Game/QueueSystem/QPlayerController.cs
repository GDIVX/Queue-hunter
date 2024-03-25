using UnityEngine;

public class QPlayerController : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject projectile;
    [SerializeField] private QueueSystem _queueSystem;
    Vector3 lastMPos;
    Vector2 moveDir;
    Vector3 lookDir;

    [SerializeField] float speed;


    private void Start()
    {
       // mainCamera = Camera.main;
    }

    private void Update()
    {
        LookAtMouse();
        GetInput();
       // Move();
    }

    void LookAtMouse()
    {
        lookDir = GetMousePosition() - transform.position;
        lookDir.y = 0f;
        transform.forward = lookDir;
    }

    Vector3 GetMousePosition()
    {
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity))
        {
            lastMPos = hitInfo.point;
            return hitInfo.point;
        }
        else
        {
            return lastMPos;
        }
    }

    void Move()
    {
        transform.position += new Vector3 (moveDir.x, 0 , moveDir.y) * Time.deltaTime * speed;
        mainCamera.transform.position += new Vector3(moveDir.x, 0, moveDir.y) * Time.deltaTime * speed;
        moveDir = Vector2.zero;
    }

    void GetInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            moveDir = Vector2.up;
        }

        if (Input.GetKey(KeyCode.S))
        {
            moveDir = Vector2.down;
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveDir = Vector2.right;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDir = Vector2.left;
        }

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
        {
            moveDir = new Vector2(1, 1);
        }
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
        {
            moveDir = new Vector2(-1, 1);
        }
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            moveDir = new Vector2(1, -1);
        }
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
        {
            moveDir = new Vector2(-1, -1);
        }

        if (Input.GetMouseButtonDown(0)) Shoot();

    }

    void Shoot()
    {
        if (_queueSystem.GetMorbole(out var morbel))
        {
            var proj = Instantiate(projectile, transform.position, Quaternion.identity);
            Debug.Log($"Fire marble");
        }
    }
}