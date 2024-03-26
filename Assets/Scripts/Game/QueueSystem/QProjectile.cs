using UnityEngine;

public class QProjectile : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] float projectileSpeed;
    Vector3 lastMPos;
    Vector3 target;

    private void Start()
    {
        mainCamera = Camera.main;
        target = GetMousePosition() - transform.position;
    }

    private void Update()
    {
        if (target != Vector3.zero) transform.position += new Vector3 (target.x, 0, target.z).normalized * Time.deltaTime * projectileSpeed;
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
            return Vector3.zero;
        }
    }


}
