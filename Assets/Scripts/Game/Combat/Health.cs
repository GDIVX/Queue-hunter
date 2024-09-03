using System;
using System.Collections;
using Combat;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Combat
{
    public class Health : MonoBehaviour, IDamageable, IHealable, ITarget
    {
        [SerializeField] private float maxHealth;
        [ShowInInspector, ReadOnly] private float _currentHealth;
        [SerializeField] private float deathTime;
        public event Action<float, IDamageable> OnUpdateValue;

        public UnityEvent OnDeathUnityEvent;
        public UnityEvent OnTakeDamage;
        public UnityEvent<float, float> OnHealthChanged;
        public UnityEvent<float, Vector3> OnTakeDamageUI;
        public UnityEvent<IDestroyable> OnAboutToBeDestroyed;

        public GameObject GameObject => gameObject;
        public IDamageable Damageable => this;


        public void Init(int modelHealth)
        {
            maxHealth = modelHealth;
            _currentHealth = maxHealth;
        }

        public event Action<IDestroyable> OnDestroyed;
        public Vector3 Position => transform.localPosition;

        public float CurrentHealth => _currentHealth;
        public float MaxHealth => maxHealth;

        private void Start()
        {
            _currentHealth = maxHealth;
            OnUpdateValue?.Invoke(_currentHealth, this);
        }

        [Button]
        public void HandleDamage(float damage)
        {
            //can we take this hit?
            if (damage >= CurrentHealth)
            {
                HandleDeath(damage);
                return;
            }

            _currentHealth = CurrentHealth - damage;
            OnUpdateValue?.Invoke(damage, this);
            OnTakeDamage?.Invoke();
            OnTakeDamageUI?.Invoke(damage, transform.position);
            OnHealthChanged?.Invoke(_currentHealth, maxHealth);
        }

        private void HandleDeath(float damage)
        {
            _currentHealth = 0;
            OnUpdateValue?.Invoke(-_currentHealth, this);
            OnTakeDamage?.Invoke();
            OnTakeDamageUI?.Invoke(damage, transform.position);
            OnHealthChanged?.Invoke(_currentHealth, maxHealth);

            StartCoroutine(WaitAndThenHandleDeath());
        }

        IEnumerator WaitAndThenHandleDeath()
        {
            OnAboutToBeDestroyed?.Invoke(this);
            yield return new WaitForSeconds(deathTime);
            OnDestroyed?.Invoke(this);
            OnDeathUnityEvent?.Invoke();
        }

        [Button]
        public void Heal(float amount)
        {
            _currentHealth = Mathf.Clamp(CurrentHealth + amount, CurrentHealth, MaxHealth);
            OnUpdateValue?.Invoke(amount, this);
        }

        [Button]
        public void MaxHeal()
        {
            Heal(MaxHealth);
        }

        public GameObject TargetGO()
        {
            return this.gameObject;
        }

        // public void KillEntity()
        // {
        //     if (isDying) return;
        //     StartCoroutine(KillAfterSeconds());
        // }
        //
        // private IEnumerator KillAfterSeconds()
        // {
        //     isDying = true;
        //     yield return new WaitForSeconds(deathTime);
        //     gameObject.SetActive(false);
        // }
    }
}