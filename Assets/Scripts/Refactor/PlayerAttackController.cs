using Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAttackController : MonoBehaviour
{
    [SerializeField] float meleeAttackCooldown;


    #region MeleeParams
    [SerializeField] float firstAttackCD;
    [SerializeField] float secondAttackCD;
    [SerializeField] float thirdAttackCD;
    [SerializeField] private int meleeDamage;
    private List<IDamageable> EntityInRange;
    bool canUseMelee = true;

    public int MeleeDamage
    {
        get => meleeDamage;
        private set => meleeDamage = value;
    }
    #endregion

    public UnityEvent<string> OnMeleeAttackAnimEvent;
    public UnityEvent OnMeleeAttack;
    public UnityEvent OnMeleeAttackEnd;
    public UnityEvent OnPlayerDeath;

    [SerializeField]private bool hasAttackedOnce;
    [SerializeField]private bool hasAttackedTwice;
    [SerializeField]private bool hasAttackedThree;

    private void Awake()
    {
        EntityInRange = new List<IDamageable>();
        hasAttackedThree = true;
    }


    public void InitMeleeAttack()
    {
        if (!canUseMelee) return;
        StartCoroutine(PunchCoroutine());
    }

    IEnumerator PunchCoroutine()
    {
        canUseMelee = false;
        if(hasAttackedOnce)
        {
            hasAttackedOnce = false;
            hasAttackedTwice = false;
            hasAttackedThree = false;
            OnMeleeAttack?.Invoke();
            OnMeleeAttackAnimEvent?.Invoke("MeleeAttackTrigger2");
            DoDamage();
            //trigger first punch anim
            yield return new WaitForSeconds(secondAttackCD);
            canUseMelee = true;
            hasAttackedTwice = true;
            OnMeleeAttackEnd?.Invoke();
        }

        else if (hasAttackedTwice)
        {
            hasAttackedOnce = false;
            hasAttackedTwice = false;
            hasAttackedThree = false;
            OnMeleeAttack?.Invoke();
            OnMeleeAttackAnimEvent?.Invoke("MeleeAttackTrigger3");
            DoDamage();
            //trigger first punch anim
            yield return new WaitForSeconds(thirdAttackCD);
            canUseMelee = true;
            hasAttackedThree = true;
            OnMeleeAttackEnd?.Invoke();
        }

        else
        {
            hasAttackedOnce = false;
            hasAttackedTwice = false;
            hasAttackedThree = false;
            OnMeleeAttack?.Invoke();
            OnMeleeAttackAnimEvent?.Invoke("MeleeAttackTrigger1");
            DoDamage();
            //trigger first punch anim
            yield return new WaitForSeconds(firstAttackCD);
            canUseMelee = true;
            hasAttackedOnce = true;
            OnMeleeAttackEnd?.Invoke();
        }
        //OnMeleeAttack?.Invoke();
        //DoDamage();
        //yield return new WaitForSeconds(meleeAttackCooldown);
        //canUseMelee = true;
        //OnMeleeAttackEnd?.Invoke();
    }

    public void DoDamage()
    {
        foreach (IDamageable target in EntityInRange)
        {
            target.HandleDamage(MeleeDamage);
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
        StartCoroutine(KillPlayer());
    }

    IEnumerator KillPlayer()
    {
        yield return new WaitForSeconds(3.7f);
        gameObject.SetActive(false);
    }
}
