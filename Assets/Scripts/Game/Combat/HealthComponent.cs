using Assets.Scripts.Core.ECS;
using Assets.Scripts.Core.ECS.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Combat
{
    public class HealthComponent : DataComponent
    {
        [SerializeField] private int fullHealth;

        public int FullHeath
        {
            get => fullHealth;
            set => fullHealth = value;
        }

        public int HealthPoints { get; set; }

        public override IComponent Instantiate()
        {
            HealthComponent component = new()
            {
                HealthPoints = FullHeath,
                FullHeath = FullHeath
            };

            return component;
        }
    }
}