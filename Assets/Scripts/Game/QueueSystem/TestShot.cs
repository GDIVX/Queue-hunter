using System;
using UnityEngine;

namespace Scirpts
{
    public class TestShot : MonoBehaviour
    {
        [SerializeField] private QueueSystem _queueSystem;
        
        [ContextMenu("Test")]
        public void Test()
        {
            if (_queueSystem.GetMorbole(out BaseLoadObject baseLoadObject))
            {
                
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Test();
            }
        }
    }
}