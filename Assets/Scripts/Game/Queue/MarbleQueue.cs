using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Queue
{
    public class MarbleQueue : MonoBehaviour
    {
        [SerializeField] private uint capacity;
        [SerializeField] private List<MarbleModel> startingQueue;
        [SerializeField] private float delayOnCreation;

        private readonly List<Marble> _pendingMarbles = new();
        private readonly Queue<Marble> _queue = new();

        public UnityEvent<Marble> onMarbleEjected;
        public UnityEvent<Marble> onMarbleCreated;


        private void Start()
        {
            StartCoroutine(Init());
        }

        private IEnumerator Init()
        {
            //populare with 8 defualt marlbles
            foreach (MarbleModel model in startingQueue)
            {
                Marble marble = model.Create();
                StartCoroutine(AddMarbleEnum(marble));
                yield return new WaitForSeconds(delayOnCreation);
            }
        }

        private void Update()
        {
            if (_pendingMarbles.Count == 0) return;

            foreach (Marble pendingMarble in _pendingMarbles.ToList())
            {
                // Update the timer
                pendingMarble.CurrentTravelTime -= Time.deltaTime;

                if (pendingMarble.CurrentTravelTime > 0)
                {
                    continue;
                }

                // Marble has reached its destination
                pendingMarble.CurrentTravelTime = 0;

                // Remove the marble from the pending list and enqueue it
                _pendingMarbles.Remove(pendingMarble);
                _queue.Enqueue(pendingMarble);

                foreach (Marble marble in _pendingMarbles)
                {
                    CalculateTravel(marble);
                }
            }
        }

        private IEnumerator AddMarbleEnum(Marble marble)
        {
            CalculateTravel(marble);
            yield return new WaitForSeconds(delayOnCreation);
            _pendingMarbles.Add(marble);
            onMarbleCreated?.Invoke(marble);
        }


        private void CalculateTravel(Marble marble)
        {
            //Calculate target index
            int index = _queue.Count + _pendingMarbles.Count;
            marble.EndY = index;

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