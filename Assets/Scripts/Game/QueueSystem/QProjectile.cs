using UnityEngine;

public class QProjectile : MonoBehaviour
{
    [SerializeField] private GameObject _expolotion;
    private Camera _mainCamera;
    [SerializeField] private float projectileSpeed;
    private Vector3 _dir;

    private Vector3 _mousePos;

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
        _dir = GetMousePosition() - ShotPostionHelper.Player.transform.position;
    }

    private void Update()
    {
        transform.position += new Vector3(_dir.x, 0, _dir.z).normalized * (Time.deltaTime * projectileSpeed);
    }

    private Vector3 GetMousePosition()
    {
        if (MainCamera is null)
        {
            return Vector3.zero;
        }

        var ray = MainCamera.ScreenPointToRay(Input.mousePosition);

      
        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity))
        {
            _mousePos = hitInfo.point;
            return hitInfo.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Instantiate(_expolotion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
