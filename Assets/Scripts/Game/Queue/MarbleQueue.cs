using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Game.Queue
{
    public class MarbleQueue : MonoBehaviour
    {
        [SerializeField] private uint capacity;
        [SerializeField] private float padding;
        [SerializeField] private List<MarbleModel> startingQueue;

        private readonly List<Marble> _pendingMarbles = new();
        private readonly Queue<Marble> _queue = new();

        public UnityEvent<Marble> onMarbleEjected;
        public UnityEvent<Marble> onMarbleStop;
        public UnityEvent<Marble, float> onMarbleCreated;

        public uint Capacity => capacity;
        public float Padding => padding;

        private void Start()
        {
            //populare with 8 defualt marlbles
            foreach (MarbleModel model in startingQueue)
            {
                Marble marble = model.Create();
                AddMarble(marble);
            }
        }

        private void Update()
        {
            if (_pendingMarbles.Count == 0) return;

            for (var i = 0; i < _pendingMarbles.Count; i++)
            {
                var pendingMarble = _pendingMarbles[i];
                //TODO in future feature:
                //Check for thedering

                //Update the timer
                pendingMarble.CurrentTravelTime -= Time.deltaTime;

                if (pendingMarble.CurrentTravelTime > 0)
                {
                    continue;
                }

                //remove the marble from the pending list and eqnque it
                _pendingMarbles.Remove(pendingMarble);
                _queue.Enqueue(pendingMarble);
                onMarbleStop?.Invoke(pendingMarble);
            }
        }

        public void AddMarble(Marble marble)
        {
            //Calculate target index
            int index = _queue.Count;

            if (index > capacity)
            {
                //we are overflowing
                //TODO: Edge case
                return;
            }

            //Calculate travel time
            float distanceToTravel = padding * index;
            float timeToTravel = distanceToTravel / marble.InQueueSpeed;
            marble.CurrentTravelTime = timeToTravel;

            //Add to the pending list
            _pendingMarbles.Add(marble);
            float ratio = distanceToTravel / (capacity * padding);

            onMarbleCreated?.Invoke(marble, ratio);
        }

//TODO 
        public void RemoveMarble()
        {
        }

        public Marble EjectMarble()
        {
            Marble res = _queue.Dequeue();

            onMarbleEjected?.Invoke(res);

            //return the marble to the top of the queue
            AddMarble(res);

            return res;
        }

        public List<Marble> GetQueue()
        {
            return _queue.ToList();
        }
    }
}