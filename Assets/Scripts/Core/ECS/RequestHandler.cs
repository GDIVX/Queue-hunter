using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Engine.ECS
{
    public class RequestHandler : ILateTickable, IRequestable
    {

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

        

        void ProcessRequests()
        {
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

        public void LateTick()
        {
            if (_requests.Count > 0)
                ProcessRequests();
        }
    }
}
