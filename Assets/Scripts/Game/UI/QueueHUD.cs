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

        private ObjectPool<MarbleUI> marbleUIPool;
        private Dictionary<Marble, MarbleUI> activeMarbles = new Dictionary<Marble, MarbleUI>();

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

        private void OnMarbleCreated(Marble marble, float distanceToTravel, float timeToTravel)
        {
            MarbleUI marbleUI = marbleUIPool.Get();
            marbleUI.Initialize(marble.Sprite);
            marbleUI.gameObject.SetActive(true);
            marbleUI.transform.SetParent(marbleUIParent);
            marbleUI.transform.position = start.transform.position;
            activeMarbles[marble] = marbleUI;

            // Set initial position based on ratio and start the tween animation
            UpdateMarblePosition(marble, distanceToTravel, timeToTravel);
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


        private void UpdateMarblePosition(Marble marble, float distanceToTravel, float timeToTravel)
        {
            if (activeMarbles.TryGetValue(marble, out MarbleUI marbleUI))
            {
                float endY = end.transform.position.y + (distanceToTravel * marbleUI.transform.localScale.y);

                marbleUI.transform.DOMoveY(endY, timeToTravel).SetEase(Ease.Linear);
            }
        }
    }
}