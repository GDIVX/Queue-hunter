using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform playerTransform;

    [SerializeField, Range(20, 50)] float camRotation = 30;

    void Update()
    {
        transform.position = new Vector3(playerTransform.position.x, 0, playerTransform.position.z);
    }

    private void OnValidate()
    {
        transform.rotation = Quaternion.Euler(camRotation, -45, 0);
    }
}
