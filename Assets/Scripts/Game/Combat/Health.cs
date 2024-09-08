using System;
using System.Collections;
using AI;
using Combat;
using Game.Utility;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Combat
{
    public class Health : MonoBehaviour, IDamageable, IHealable, ITarget, IInit<IEnemyModel>
    {
        [SerializeField] private float maxHealth;
        [SerializeField] private float deathTime;
        [ShowInInspector, ReadOnly] private float _currentHealth;
        [ShowInInspector, ReadOnly] private bool _canBeDamaged;
        public event Action<float, IDamageable> OnUpdateValue;

        public UnityEvent OnDeathUnityEvent;
        public UnityEvent OnTakeDamage;
        public UnityEvent<float, float> OnHealthChanged;
        public UnityEvent<float, Vector3> OnTakeDamageUI;
        public UnityEvent<IDestroyable> OnAboutToBeDestroyed;
        public UnityEvent<string> OnDeathAnim;

        public GameObject GameObject => gameObject;
        public IDamageable Damageable => this;


        public event Action<IDestroyable> OnDestroyed;
        public Vector3 Position => transform.localPosition;

        public float CurrentHealth => _currentHealth;
        public float MaxHealth => maxHealth;

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            _currentHealth = maxHealth;
            _canBeDamaged = true;
            OnUpdateValue?.Invoke(_currentHealth, this);
        }

        public void Init(IEnemyModel input)
        {
            maxHealth = input.Health;
            Init();
        }

        [Button]
        public void HandleDamage(float damage)
        {
            if (!_canBeDamaged) return;
            //can we take this hit?
            if (damage >= CurrentHealth)
            {
                OnTakeDamageUI?.Invoke(damage, transform.position);
                Kill();
                return;
            }

            _currentHealth = CurrentHealth - damage;
            OnUpdateValue?.Invoke(damage, this);
            OnTakeDamage?.Invoke();
            //OnTakeDamageUI?.Invoke(damage, transform.position);
            DamageFeedbackUI.Instance.GetDamageNumber(damage, transform.position);
            OnHealthChanged?.Invoke(_currentHealth, maxHealth);
        }

        [Button]
        public void Kill()
        {
            _canBeDamaged = false;
            _currentHealth = 0;
            OnUpdateValue?.Invoke(-_currentHealth, this);
            OnTakeDamage?.Invoke();
            OnHealthChanged?.Invoke(_currentHealth, maxHealth);

            StartCoroutine(WaitAndThenHandleDeath());
        }

        IEnumerator WaitAndThenHandleDeath()
        {
            OnAboutToBeDestroyed?.Invoke(this);
            OnDeathAnim?.Invoke("DeathTrigger");
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
    }
}