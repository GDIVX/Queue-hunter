using Combat;
using Game.Combat;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Game.Queue
{
    [CreateAssetMenu(fileName = "Marble", menuName = "Game/Marbles", order = 0)]
    public class MarbleModel : ScriptableObject
    {
        [SerializeField] private float _inQueueSpeed;
        [SerializeField] public ProjectileModel _projectileModel;
        [SerializeField] private Sprite sprite;
        [SerializeField] private Type type;

        public enum Type
        {
            Fire,
            Lightning,
            Ice
        }

        public float InQueueSpeed
        {
            get => _inQueueSpeed;
            set => _inQueueSpeed = value;
        }

        public Marble Create()
        {
            return new(_inQueueSpeed, _projectileModel, sprite, type);
        }
    }
}