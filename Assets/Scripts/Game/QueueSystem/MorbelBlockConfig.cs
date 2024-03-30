using UnityEngine;

namespace Scirpts
{
    [CreateAssetMenu(fileName = "MorbelBlockConfig", menuName = "new MorbelBlockConfig", order = 0)]
    public class MorbelBlockConfig : ScriptableObject
    {
        public MorbleDisntConfig[] MorbleDisntConfig;
    }

    [System.Serializable]
    public class MorbleDisntConfig
    {
        [SerializeField] public MorbaleConfig morbaleConfig;
        [SerializeField] public float Delay;
    }
}