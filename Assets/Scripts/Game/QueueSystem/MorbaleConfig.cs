using UnityEngine;
using UnityEngine.UIElements;

namespace Scirpts
{
    [CreateAssetMenu(fileName = "MorbaleConfig", menuName = "new Morbale Config", order = 0)]
    public class MorbaleConfig : ScriptableObject
    {
        public Image _Image;
        public float speed;
        public MorbleType morbleType;
    }

    public enum MorbleType
    {
        Fire,
    }
}