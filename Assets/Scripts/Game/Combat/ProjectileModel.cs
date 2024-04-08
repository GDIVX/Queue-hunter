using Sirenix.OdinInspector;
using UnityEngine;

namespace Combat
{
    [CreateAssetMenu(fileName = "ProjectileModel", menuName = "Game/Combat/Projectile", order = 0)]
    public class ProjectileModel : ScriptableObject
    {
        [SerializeField] private float speed;
        [SerializeField, AssetsOnly] private GameObject viewPrefab;
        [SerializeField] private float damage;
        [SerializeField] private string lookupTagForTarget;
        [SerializeField] private float lifetime;

        public float Speed => speed;
        public GameObject ViewPrefab => viewPrefab;
        public float Damage => damage;
        public string LookupTagForTarget => lookupTagForTarget;

        public float Lifetime => lifetime;
    }
}