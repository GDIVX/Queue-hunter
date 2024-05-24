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
        private readonly List<Marble> _queue = new();
        private readonly SortedDictionary<float, List<Marble>> _sortedMarbleList = new();

        public UnityEvent<Marble> onMarbleEjected;
        public UnityEvent<Marble> onMarbleCreated;

        private void Start()
        {
            StartCoroutine(Init());
        }

        private IEnumerator Init()
        {
            foreach (MarbleModel model in startingQueue)
            {
                yield return CreateMarble(model);
            }
        }

        private IEnumerator CreateMarble(MarbleModel model)
        {
            Marble marble = model.Create();
            AddToSortedList(marble);
            yield return AddMarbleEnum(marble);
        }

        private void AddToSortedList(Marble marble)
        {
            if (!_sortedMarbleList.ContainsKey(marble.CurrY))
            {
                _sortedMarbleList[marble.CurrY] = new List<Marble>();
            }
            _sortedMarbleList[marble.CurrY].Add(marble);
        }

        private void RemoveFromSortedList(Marble marble)
        {
            if (_sortedMarbleList.ContainsKey(marble.CurrY))
            {
                _sortedMarbleList[marble.CurrY].Remove(marble);
                if (_sortedMarbleList[marble.CurrY].Count == 0)
                {
                    _sortedMarbleList.Remove(marble.CurrY);
                }
            }
        }

        private void Update()
        {
            if (_pendingMarbles.Count == 0) return;

            for (var i = 0; i < _pendingMarbles.Count; i++)
            {
                var pendingMarble = _pendingMarbles[i];
                pendingMarble.CurrentTravelTime -= Time.deltaTime;
                float oldCurrY = pendingMarble.CurrY;
                pendingMarble.CurrY = GeYPosition(pendingMarble);

                if (pendingMarble.CurrentTravelTime > 0)
                {
                    // Update SortedList if CurrY has changed
                    if (oldCurrY != pendingMarble.CurrY)
                    {
                        RemoveFromSortedList(pendingMarble);
                        AddToSortedList(pendingMarble);
                    }
                    continue;
                }

                pendingMarble.CurrentTravelTime = 0;
                pendingMarble.CurrY = pendingMarble.EndY;

                // Remove the marble from the pending list and enqueue it
                _pendingMarbles.Remove(pendingMarble);
                _queue.Add(pendingMarble);
            }
        }

        private void Recalculate()
        {
            foreach (Marble marble in _queue.ToList()) // ToList to avoid modification during iteration
            {
                CalculateTravel(marble);

                if (marble.CurrentTravelTime > 0)
                {
                    _queue.Remove(marble);
                    _pendingMarbles.Add(marble);
                }
            }

            foreach (Marble marble in _pendingMarbles)
            {
                CalculateTravel(marble);
            }
        }

        private IEnumerator AddMarbleEnum(Marble marble)
        {
            marble.CurrY = (int)capacity;
            CalculateTravel(marble);
            yield return new WaitForSeconds(delayOnCreation);
            _pendingMarbles.Add(marble);
            onMarbleCreated?.Invoke(marble);
        }

        private void CalculateTravel(Marble marble)
        {
            // Calculate the index
            int index = 0;
            foreach (var kvp in _sortedMarbleList)
            {
                if (kvp.Value.Contains(marble))
                {
                    index += kvp.Value.IndexOf(marble);
                    break;
                }
                index += kvp.Value.Count;
            }
            marble.EndY = index;

            if (index >= capacity)
            {
                Debug.LogWarning("Queue capacity reached, cannot add more marbles.");
                return;
            }

            float distanceToTravel = marble.CurrY - index;
            float timeToTravel = Mathf.Abs(distanceToTravel) / marble.InQueueSpeed;
            marble.CurrentTravelTime = timeToTravel;
            marble.TotalTravelTime = timeToTravel;
        }

        float GeYPosition(Marble marble)
        {
            return marble.CurrentTravelTime * marble.InQueueSpeed;
        }

        public void RemoveMarble()
        {
            if (_queue.Count > 0)
            {
                Marble marble = _queue[0];
                _queue.Remove(marble);
                Recalculate();
            }
            else
            {
                Debug.LogWarning("No marbles to remove.");
            }
        }

        public Marble EjectMarble()
        {
            if (_queue.Count > 0)
            {
                Marble marble = _queue[0];
                _queue.Remove(marble);
                Recalculate();

                onMarbleEjected?.Invoke(marble);

                StartCoroutine(AddMarbleEnum(marble));
                return marble;
            }
            else
            {
                Debug.LogWarning("No marbles to eject.");
                return null;
            }
        }

        public List<Marble> GetQueue()
        {
            return _queue.ToList();
        }
    }
}
