using System;
using System.Linq;
using Game.Queue;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class MarbleIndicator : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private MarbleQueue marbleQueue;

        private void Start()
        {
            marbleQueue.onQueueInitialized.AddListener((q) =>
            {
                Marble marble = q._marbles.First();
                ShowMarble(marble);
            } ) ;
            marbleQueue.onMarbleEjected.AddListener(ShowMarble);
        }

        private void ShowMarble(Marble marble)
        {
            image.sprite = marble.Sprite;
        }
    }
}