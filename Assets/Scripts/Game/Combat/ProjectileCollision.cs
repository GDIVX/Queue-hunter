using Game.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollision : MonoBehaviour
{
    [SerializeField] private GameObject explosionObject;

    private void OnTriggerEnter(Collider other)
    {
        Explode(other);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Instantiate(explosionObject, collision.transform.position, Quaternion.identity);
    //    this.gameObject.SetActive(false);
    //}

    public void Explode(Collider col)
    {
        if (!col.CompareTag("Player") && !col.CompareTag("Vision"))
        {
            Instantiate(explosionObject, col.transform.position, Quaternion.identity);
            this.gameObject.SetActive(false);
        }
    }
}
