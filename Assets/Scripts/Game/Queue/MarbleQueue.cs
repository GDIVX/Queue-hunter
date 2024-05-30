using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Queue
{
    public class MarbleQueue : MonoBehaviour
    {
        [SerializeField] private List<MarbleModel> startingQueue;
        [SerializeField] private int maxCapacity;

        [ShowInInspector, ReadOnly] private readonly List<Marble> _marbles = new List<Marble>();

        public UnityEvent<Marble> onMarbleEjected;
        public UnityEvent<Marble> onMarbleCreated;

        public int MaxCapacity => maxCapacity;
        public int Count => _marbles.Count;

        private void Start()
        {
            //Initialize the queue
            foreach (MarbleModel model in startingQueue)
            {
                CreateMarble(model);
            }

            //Free up memeory by deleting the list
            startingQueue.Clear();
            startingQueue = null;
        }

        private void Update()
        {
            if (_marbles.Count == 0) return;

            for (int i = 0; i < _marbles.Count; i++)
            {
                Marble marble = _marbles[i];
                marble.UpdatePosition(new(0, i, 0), 0.1f);
            }
        }

        private Marble CreateMarble(MarbleModel model)
        {
            //Can we add another marble?
            if (_marbles.Count >= maxCapacity)
            {
                Debug.LogError($"Trying to add {model} to the queue at max capacity");
                return null;
            }

            //Create a new marble
            Marble marble = model.Create();
            AddToTop(marble);


            return marble;
        }

        private void AddToTop(Marble marble)
        {
            //Add it to the list
            _marbles.Add(marble);
            //Set its position to the top of the container
            marble.Position = new(0, maxCapacity, 0);
            onMarbleCreated?.Invoke(marble);
        }


        public Marble EjectMarble()
        {
            Marble marble = _marbles.First();

            //The marlbe isn't ready to fire
            if (!marble.IsReady) return null;

            _marbles.Remove(marble);
            onMarbleEjected?.Invoke(marble);
            AddToTop(marble);
            return marble;
        }

        public bool IsEmpty()
        {
            return _marbles.IsEmpty();
        }
    }
}