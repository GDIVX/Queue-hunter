using System;
using UnityEngine;

namespace Game.Movement
{
    public class RotrateTowardsMouse : MonoBehaviour
    {
        private Camera main;

        private void Awake()
        {
            if (Camera.main == null)
            {
                Debug.LogError("Main camera is not assigned");
                return;
            }

            main = Camera.main;
        }

        public void Rotate()
        {
            if (main == null)
            {
                main = Camera.main;
            }

            Vector3 mousePosition = main.ScreenToWorldPoint(Input.mousePosition);
            transform.rotation = Quaternion.LookRotation(mousePosition.normalized);
        }
    }
}