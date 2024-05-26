using System;
using Game.Combat;
using UnityEngine;

namespace Game.Queue
{
    [Serializable]
    public class Marble
    {
        public float Speed { get; set; }
        public Sprite Sprite { get; set; }
        public ProjectileModel ProjectileModel { get; private set; }

        public Vector3 Position { get; set; }

        public Marble(float speed, ProjectileModel projectileModel, Sprite sprite)
        {
            Speed = speed;
            ProjectileModel = projectileModel;
            Sprite = sprite;
        }

        public void UpdatePosition(Vector3 goal, float minDistanceToGoal = 0f)
        {
            //Determine if we already reached our goal within the desired distance
            if (Vector3.Distance(goal, Position) <= minDistanceToGoal) return;

            //lerp one frame towards the goal
            Position = Vector3.Lerp(Position, goal, Time.deltaTime * Speed);
        }
    }
}