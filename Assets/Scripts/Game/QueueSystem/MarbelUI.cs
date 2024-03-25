using UnityEngine;

namespace Scirpts
{
    public class MarbelUI : MonoBehaviour
    {
        private Morbel _morbel;
        
        
        public int Id { get; private set; }
        public float DisPro => _morbel.TravelPros;
        
        public void Init(Morbel morbel)
        {
            _morbel = morbel;
            Id = morbel.Id;
        }
    }
}