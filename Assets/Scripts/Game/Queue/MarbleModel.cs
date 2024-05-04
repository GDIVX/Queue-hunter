using Combat;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Queue
{
    [CreateAssetMenu(fileName = "Marble", menuName = "Game/Marbles", order = 0)]
    public class MarbleModel : ScriptableObject
    {
        [SerializeField] private float _inQueueSpeed;
        [SerializeField] private ProjectileModel _projectileModel;
        [SerializeField] private GameObject UIVIew;

        public float InQueueSpeed
        {
            get => _inQueueSpeed;
            set => _inQueueSpeed = value;
        }

        public ProjectileModel ProjectileModel
        {
            get => _projectileModel;
            set => _projectileModel = value;
        }

        public Marble CreateMarble()
        {
            return new Marble(InQueueSpeed, ProjectileModel);
        }
    }
}