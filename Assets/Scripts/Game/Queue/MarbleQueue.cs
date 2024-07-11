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
        /// <summary>
        /// seriliazation only, do not use in runtime.
        /// </summary>
        [SerializeField] private List<MarbleModel> startingQueue;

        [ShowInInspector, ReadOnly] public readonly List<Marble> _marbles = new List<Marble>();

        public UnityEvent<Marble> onMarbleEjected;

        public UnityEvent<Marble> onMarbleMovedToTop;
        public UnityEvent<MarbleQueue> onQueueInitialized;
        public UnityEvent<Marble> onMarbleInitialized;

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


            onQueueInitialized?.Invoke(this);
        }

        private void Update()
        {
            if (_marbles.Count == 0) return;

            for (int i = 0; i < _marbles.Count; i++)
            {
                Marble marble = _marbles[i];
                marble.UpdatePosition(new(0, i, 0));
            }
        }

        private Marble CreateMarble(MarbleModel model)
        {
            //Create a new marble
            Marble marble = model.Create();
            AddToTop(marble);
            onMarbleInitialized?.Invoke(marble);

            return marble;
        }

        private void AddToTop(Marble marble)
        {
            //Add it to the list
            _marbles.Add(marble);
            //Set its position to the top of the container
            marble.Position = new(0, Count + 1, 0);
            onMarbleMovedToTop?.Invoke(marble);
        }


        public Marble EjectMarble()
        {
            Marble marble = _marbles.First();
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