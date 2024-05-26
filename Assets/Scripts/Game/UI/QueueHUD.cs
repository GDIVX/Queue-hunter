using System;
using System.Collections.Generic;
using DG.Tweening;
using Game.Queue;
using Game.Utility;
using UnityEngine;

namespace Game.UI
{
    public class QueueHUD : MonoBehaviour
    {
        [SerializeField] private MarbleQueue marbleQueue;
        [SerializeField] private MarbleUI marbleUIPrefab;
        [SerializeField] private Transform marbleUIParent;
        [SerializeField] private GameObject start, end;
        [SerializeField] private float spacing;

        private ObjectPool<MarbleUI> marbleUIPool;
        private Dictionary<Marble, MarbleUI> activeMarbles = new Dictionary<Marble, MarbleUI>();
        private List<Marble> marblesToProcess = new List<Marble>();

        private void Awake()
        {
            marbleUIPool = new ObjectPool<MarbleUI>(marbleUIPrefab);
        }

        private void OnEnable()
        {
            marbleQueue.onMarbleCreated.AddListener(OnMarbleCreated);
            marbleQueue.onMarbleEjected.AddListener(OnMarbleEjected);
        }

        private void OnDisable()
        {
            marbleQueue.onMarbleCreated.RemoveListener(OnMarbleCreated);
            marbleQueue.onMarbleEjected.RemoveListener(OnMarbleEjected);
        }

        private void OnMarbleCreated(Marble marble)
        {
            MarbleUI marbleUI = marbleUIPool.Get();
            marbleUI.Initialize(marble.Sprite);
            marbleUI.gameObject.SetActive(true);
            marbleUI.transform.SetParent(marbleUIParent);
            marbleUI.transform.position = start.transform.position;
            activeMarbles[marble] = marbleUI;
            marblesToProcess.Add(marble);
        }

        private void OnMarbleEjected(Marble marble)
        {
            if (activeMarbles.TryGetValue(marble, out MarbleUI marbleUI))
            {
                marbleUI.gameObject.SetActive(false);
                marbleUIPool.ReturnToPool(marbleUI);
                activeMarbles.Remove(marble);
            }
        }

        private void Update()
        {
            foreach (Marble marble in marblesToProcess)
            {
                if (activeMarbles.TryGetValue(marble, out MarbleUI marbleUI))
                {
                    marbleUI.transform.position = marble.Position;
                }
            }
        }

        // private void UpdateMarblePosition(Marble marble)
        // {
        //     if (activeMarbles.TryGetValue(marble, out MarbleUI marbleUI))
        //     {
        //         float endY = end.transform.position.y +
        //                      (distanceToTravel * (marbleUI.transform.localScale.y + spacing));
        //
        //         marbleUI.transform.DOMoveY(endY, timeToTravel).SetEase(Ease.Linear);
        //     }
        // }
    }
}