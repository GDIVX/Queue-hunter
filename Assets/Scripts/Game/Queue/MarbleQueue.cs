using System;
using System.Collections;
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
        [SerializeField] private List<MarbleModel> startingQueue;

        private readonly List<Marble> _pendingMarbles = new();
        private readonly Queue<Marble> _queue = new();

        public UnityEvent<Marble> onMarbleEjected;
        public UnityEvent<Marble> onMarbleStop;
        public UnityEvent<Marble, float, float> onMarbleCreated;


        private void Start()
        {
            //populare with 8 defualt marlbles
            foreach (MarbleModel model in startingQueue)
            {
                Marble marble = model.Create();
                StartCoroutine(AddMarbleEnum(marble));
            }
        }

        private void Update()
        {
            if (_pendingMarbles.Count == 0) return;

            // Use a backward loop to safely remove items from the list
            for (int i = _pendingMarbles.Count - 1; i >= 0; i--)
            {
                var pendingMarble = _pendingMarbles[i];

                // Update the timer
                pendingMarble.CurrentTravelTime -= Time.deltaTime;

                if (pendingMarble.CurrentTravelTime > 0)
                {
                    continue;
                }

                // Marble has reached its destination
                pendingMarble.CurrentTravelTime = 0;

                // Remove the marble from the pending list and enqueue it
                _pendingMarbles.RemoveAt(i);
                _queue.Enqueue(pendingMarble);
                onMarbleStop?.Invoke(pendingMarble);
            }
        }

        private IEnumerator AddMarbleEnum(Marble marble)
        {
            _pendingMarbles.Add(marble);
            yield return new WaitForEndOfFrame();
            AddMarble(marble);
        }


        private void AddMarble(Marble marble)
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
            float queueLength = capacity;
            float distanceToTravel = queueLength - index;
            float timeToTravel = distanceToTravel / marble.InQueueSpeed;
            marble.CurrentTravelTime = timeToTravel;
            marble.TotalTravelTime = timeToTravel;

            //Add to the pending list

            onMarbleCreated?.Invoke(marble, distanceToTravel, timeToTravel);
        }


//TODO 
        public void RemoveMarble()
        {
        }

        public Marble EjectMarble()
        {
            Marble marble = _queue.Dequeue();

            onMarbleEjected?.Invoke(marble);

            //return the marble to the top of the queue
            StartCoroutine(AddMarbleEnum(marble));


            return marble;
        }

        public List<Marble> GetQueue()
        {
            return _queue.ToList();
        }
    }
}