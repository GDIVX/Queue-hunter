using System.Collections;
using UnityEngine;

public class QPlayerController : MonoBehaviour
{
    Camera _mainCamera;
    [SerializeField] GameObject projectile;
    [SerializeField] private QueueSystem _queueSystem;
    Vector3 lastMPos;
    Vector2 moveDir;
    Vector3 lookDir;

    [SerializeField] float speed;

    private GameObject _player;
    [SerializeField] Animator _anim;

    public Camera MainCamera
    {
        get
        {
            if (_mainCamera is null)
            {
                _mainCamera = Camera.main;

                if (_mainCamera is null)
                {
                    return null;
                }

                return _mainCamera;
            }
            return _mainCamera;
        }
    }

    private void Start()
    {
        _player = ShotPostionHelper.Player;
        StartCoroutine(GetAnim());
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
        if (MainCamera is null)
        {
            return Vector3.zero;
        }

        var ray = MainCamera.ScreenPointToRay(Input.mousePosition);

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

        if (Input.GetMouseButtonDown(0)) StartCoroutine(ShootMarble());

        if (Input.GetMouseButtonDown(1))
        {
            _anim.SetTrigger("MeleeAttackTrigger");
        }
    }

    void Shoot()
    {
        if (_queueSystem.GetMorbole(out var morbel))
        {
            var proj = Instantiate(projectile, new Vector3(_player.transform.position.x, _player.transform.position.y + 0.5f, _player.transform.position.z), Quaternion.identity);
            Debug.Log($"Fire marble");
        }
    }

    IEnumerator ShootMarble()
    {
        _anim.SetTrigger("1HSpellTrigger");
        yield return new WaitForSeconds(1);
        Shoot();
    }

    IEnumerator GetAnim()
    {
        yield return new WaitForSeconds(3);
        _anim = FindObjectOfType<Animator>();
    }
}