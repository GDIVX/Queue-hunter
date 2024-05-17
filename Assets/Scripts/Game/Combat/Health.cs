using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Combat
{
    public class Health : MonoBehaviour, IDamageable, IHealable, ITargetable
    {
        [SerializeField] private float maxHealth;
        [ShowInInspector, ReadOnly] private float currentHealth;
        [SerializeField] private bool canBeDamaged = true;
        [SerializeField] private float destroyCooldown;
        public event Action<float, IDamageable> OnUpdateValue;

        public UnityEvent OnDeathUnityEvent;

        public IDamageable Damageable => this;

        public bool CanBeDamaged
        {
            get => canBeDamaged;
            set => canBeDamaged = value;
        }

        public void Init(int modelHealth)
        {
            maxHealth = modelHealth;
            currentHealth = maxHealth;
        }

        public event Action<IDestroyable> OnDestroyed;
        public Vector3 Position => transform.localPosition;

        public float CurrentHealth => currentHealth;
        public float MaxHealth => maxHealth;

        private void Start()
        {
            currentHealth = maxHealth;
            OnUpdateValue?.Invoke(currentHealth, this);
        }

        [Button]
        public void HandleDamage(float damage)
        {
            if (!CanBeDamaged) return;

            OnUpdateValue?.Invoke(damage, this);

            //can we take this hit?
            if (damage >= CurrentHealth)
            {
                canBeDamaged = false;
                OnDestroyed?.Invoke(this);
                OnDeathUnityEvent?.Invoke();
                return;
            }

            currentHealth = CurrentHealth - damage;
        }

        [Button]
        public void Heal(float amount)
        {
            currentHealth = Mathf.Clamp(CurrentHealth + amount, CurrentHealth, MaxHealth);
            OnUpdateValue?.Invoke(amount, this);
        }

        [Button]
        public void MaxHeal()
        {
            Heal(MaxHealth);
        }

        public void KillEntity()
        {
            StartCoroutine(DestroyAfterCountdown());
        }

        protected IEnumerator DestroyAfterCountdown()
        {
            yield return new WaitForSeconds(destroyCooldown);
            gameObject.SetActive(false);
        }
    }
}