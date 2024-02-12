using System.Collections.Generic;
using System.Linq;

namespace Tzipory.Systems.SaveLoadSystem.PlayerPref
{
    [System.Serializable]
    public class SerializeArray<T> where T : ISave
    {
        public List<T> SavedData;

        public SerializeArray(IEnumerable<T> data)
        {
            SavedData = data.ToList();
        }

        public SerializeArray()
        {
            SavedData = new List<T>();
        }
    }
}