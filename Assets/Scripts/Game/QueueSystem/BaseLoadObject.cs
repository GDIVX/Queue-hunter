using UnityEngine;

namespace Scirpts
{
    public abstract class BaseLoadObject
    {
        private readonly float _speed;//need to be update wait custom GameTime
        private readonly ILoader _loader;

        public int LoadedIndex;
        public float ProgressPercentage  => (DistanceTraveled / _loader.MaxDistanceToTravel) * 100;

        public bool IsReady { get; private set; }
        public bool IsLoaded { get; private set; }

        public float DistanceTraveled { get; private set; }

        public int Id { get; }

        protected BaseLoadObject(ILoader loader,float speed,int id)
        {
            _loader = loader;
            Id = id;
            _speed = speed;
            IsLoaded = false;
        }

        public void OnLoaded(int loadIndex)
        {
            LoadedIndex = loadIndex;
            IsLoaded = true;
        }
        

        public void UpdateTravelStatus()
        {
            if (!IsLoaded)
            {
                if (DistanceTraveled >= _loader.CurrentDistanceToTravel)
                {
                    IsLoaded = true;
                    return;
                }

                DistanceTraveled += _speed * Time.deltaTime;
            }
            else
            {
                if (DistanceTraveled < _loader.GetDistanceByLoadedIndex(LoadedIndex))
                {
                    IsReady = false;
                    DistanceTraveled += _loader.FireRate * Time.deltaTime;
                }
                else
                    IsReady = true;
            }
        }
    }
}