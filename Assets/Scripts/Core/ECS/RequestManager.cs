using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Engine.ECS
{
    public class RequestManager : MonoBehaviour
    {
        // Singleton implementation
        private static RequestManager _instance;
        public static RequestManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<RequestManager>()
                        ?? new GameObject("RequestManager").AddComponent<RequestManager>();
                }
                return _instance;
            }
        }

        Queue<Request> _requests = new();

        public class Request
        {
            public Action OnComplete;
            internal Action actionToPerform;

            public Request(Action actionToPerform)
            {
                this.actionToPerform = actionToPerform;
            }
        }

        public Request Schedule(Action action)
        {
            Request request = new Request(action);
            _requests.Enqueue(request);
            return request;
        }

        private void LateUpdate()
        {
            if (_requests.Count > 0)
            {
                StartCoroutine(ProcessRequests());
            }
        }

        IEnumerator ProcessRequests()
        {
            yield return new WaitForEndOfFrame();

            while (_requests.Count > 0)
            {
                Request request = _requests.Dequeue();
                try
                {
                    request.actionToPerform?.Invoke();
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error executing request: {ex.Source} {ex.Message} {ex.StackTrace}");
                    request.OnComplete?.Invoke();
                }
            }
        }
    }
}
