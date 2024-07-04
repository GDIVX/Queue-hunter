using Combat;
using Game.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileCollision : MonoBehaviour
{
    [SerializeField] GameObject effectObject;
    [SerializeField] public GameObject explosionObject;
    public UnityEvent onEffectRequest;
    public UnityEvent onEffectCollision;
    [SerializeField] private float destroyTime;

    [SerializeField] float resetTime;

    private void OnEnable()
    {
        StartCoroutine(KillProjectile());
        effectObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("Vision") && !other.CompareTag("Ground"))
        {
            var point = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            explosionObject.transform.position = point;
            StartCoroutine(ManageActiveness());
            onEffectCollision?.Invoke();
        }
    }

    protected IEnumerator ManageActiveness()
    {
        if (TryGetComponent<Projectile>(out var proj))

        explosionObject.SetActive(true);
        effectObject.SetActive(false);

        yield return new WaitForSeconds(resetTime);

        explosionObject.SetActive(false);
        proj.SetAvailable(true);
        gameObject.SetActive(false);
    }

    private IEnumerator KillProjectile()
    {
        yield return new WaitForSeconds(destroyTime);
        if (!gameObject.activeSelf) yield break;
        if (TryGetComponent<Projectile>(out var proj))
        {
            effectObject.SetActive(false);
            explosionObject.SetActive(false);
            proj.SetAvailable(true);
        }
        gameObject.SetActive(false);
    }



}
