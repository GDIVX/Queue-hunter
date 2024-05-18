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
            marbleQueue.onMarbleStop.AddListener(OnMarbleStop);
        }

        private void OnDisable()
        {
            marbleQueue.onMarbleCreated.RemoveListener(OnMarbleCreated);
            marbleQueue.onMarbleEjected.RemoveListener(OnMarbleEjected);
            marbleQueue.onMarbleStop.RemoveListener(OnMarbleStop);
        }

        private void OnMarbleCreated(Marble marble, float ratio)
        {
            MarbleUI marbleUI = marbleUIPool.Get();
            marbleUI.Initialize(marble.Sprite);
            marbleUI.gameObject.SetActive(true);
            marbleUI.transform.SetParent(marbleUIParent);
            marbleUI.transform.position = start.transform.position;
            activeMarbles[marble] = marbleUI;

            // Set initial position based on ratio and start the tween animation
            UpdateMarblePosition(marble, ratio);
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

        private void OnMarbleStop(Marble marble)
        {
            // Marble has stopped, its position will be updated as needed
        }

        private void UpdateMarblePosition(Marble marble, float ratio)
        {
            if (activeMarbles.TryGetValue(marble, out MarbleUI marbleUI))
            {
                float startY = start.transform.position.y;
                float endY = end.transform.position.y;
                float targetYPosition = Mathf.Lerp(startY, endY, ratio);

                // Use DoTween to animate the position change
                float remainingTime = marble.CurrentTravelTime;
                marbleUI.transform.DOMoveY(targetYPosition, remainingTime).SetEase(Ease.Linear);
            }
        }
    }
}
