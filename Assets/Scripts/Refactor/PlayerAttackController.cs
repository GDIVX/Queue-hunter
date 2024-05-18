using Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAttackController : MonoBehaviour
{
    //[SerializeField] Animator anim;
    // [SerializeField] GameObject projectile;
    [SerializeField] PlayerMovementController playerMovementController;
    Camera MainCamera;
    bool isShooting;

    #region MeleeParams
    [SerializeField] private int meleeDamage;
    private List<IDamageable> EntityInRange;
    bool canUseMelee = true;

    public int MeleeDamage
    {
        get => meleeDamage;
        private set => meleeDamage = value;
    }
    #endregion

    public UnityEvent OnMeleeAttack;
    public UnityEvent OnMeleeAttackEnd;
    public UnityEvent OnPlayerDeath;

    private void Awake()
    {
        EntityInRange = new List<IDamageable>();
    }

    private void Start()
    {
        //anim = GetComponentInChildren<Animator>();
        //playerMovementController = GetComponent<PlayerMovementController>();
        MainCamera = Camera.main;
    }

    // void ShootProjectile()
    // {
    //     //TODO : Refactor
    //     // if (queueSystem.GetMorbole(out var morbel))
    //     // {
    //     //     var proj = Instantiate(projectile, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
    //     // }
    // }

    // public void InitShootCoroutine()
    // {
    //     if (isShooting) return;
    //     var pos = MainCamera.ScreenToWorldPoint(Input.mousePosition);
    //     transform.LookAt(new Vector3(pos.x, 0, pos.z));
    //     StartCoroutine(ShootMarble());
    // }

    // public IEnumerator ShootMarble()
    // {
    //     isShooting = true;
    //     anim.SetTrigger("1HSpellTrigger");
    //     playerMovementController.canMove = false;
    //     yield return new WaitForSeconds(1);
    //     ShootProjectile();
    //     yield return new WaitForSeconds(1.3f);
    //     playerMovementController.canMove = true;
    //     isShooting = false;
    // }

    public void InitMeleeAttack()
    {
        if (!canUseMelee) return;
        //var pos = MainCamera.ScreenToWorldPoint(Input.mousePosition);
        //transform.LookAt(new Vector3(pos.x, 0, pos.z));
        StartCoroutine(PunchCoroutine());
    }

    IEnumerator PunchCoroutine()
    {
        canUseMelee = false;
        //playerMovementController.canMove = false;
        //playerMovementController.Speed *= .5f;
        OnMeleeAttack?.Invoke();
        //anim.SetTrigger("MeleeAttackTrigger");
        DoDamage();
        yield return new WaitForSeconds(.3f);
        //playerMovementController.Speed *= 2;
        //playerMovementController.canMove = true;
        canUseMelee = true;
        OnMeleeAttackEnd?.Invoke();
    }

    public void DoDamage()
    {
        Debug.Log("Attacking");
        foreach (IDamageable target in EntityInRange)
        {
            target.HandleDamage(MeleeDamage);
            Debug.Log("!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Enemy")) return;
        if (other.TryGetComponent(out IDamageable target))
        {
            EntityInRange.Add(target);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Enemy")) return;

        if (other.TryGetComponent(out IDamageable target))
        {
            EntityInRange.Remove(target);
        }
    }
    
    //temp
    public void PlayerDead()
    {
        OnPlayerDeath?.Invoke();
        //anim.SetTrigger("PlayerDeathTrigger");
        StartCoroutine(KillPlayer());
    }

    IEnumerator KillPlayer()
    {
        yield return new WaitForSeconds(3.7f);
        gameObject.SetActive(false);
    }
}
