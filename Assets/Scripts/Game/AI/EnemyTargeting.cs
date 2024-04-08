using System;
using System.Collections.Generic;
using System.Linq;
using Combat;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace AI
{
    [RequireComponent(typeof(Collider))]
    public class EnemyTargeting : MonoBehaviour
    {
        [SerializeField] private List<WeightPerTag> weightPerTags;

        private float viewDistance;

        private PriorityQueue<ITargetable> priorityQueue;

        public ITargetable GetTarget()
        {
            if (priorityQueue.Count == 0) return null;
            priorityQueue.Sort();
            return priorityQueue.Peek();
        }

        private void Awake()
        {
            viewDistance = GetComponent<Collider>().bounds.size.magnitude / 2;
            priorityQueue = new((targetA, targetB) =>
            {
                float utilityA = GetUtilityScore(targetA);
                float utilityB = GetUtilityScore(targetB);

                return utilityA.CompareTo(utilityB);
            });

        }

        private void Update()
        {
            if(priorityQueue.Count != 0) return;
            var player = GameObject.FindWithTag("Player");
            ITargetable castleTarget = player.GetComponent<ITargetable>();
            priorityQueue.Enqueue(castleTarget);
            
        }
        

        private float GetUtilityScore(ITargetable target)
        {
            //get the tag of target
            var pair = weightPerTags.FirstOrDefault(x => target.CompareTag(x.tag));
            if (pair.tag.IsNullOrWhitespace()) return -1; //invalid, return minimum value

            float weight = pair.weight;

            //get the distance to the target
            float distance = Vector3.Distance(target.Position, transform.position);

            //clamp the score between 0 to 
            var score = (distance / viewDistance) * weight;

            Color color = Color.Lerp(Color.green, Color.red, score);
            Debug.DrawLine(transform.position, target.Position, color, Time.deltaTime);

            return score;
        }

        private void OnTriggerEnter(Collider other)
        {
            //dose the collision has a tag that we are interested with?
            var pair = weightPerTags.FirstOrDefault(x => other.CompareTag(x.tag));
            if (pair.tag.IsNullOrWhitespace()) return;

            if (other.TryGetComponent(out ITargetable target))
            {
                priorityQueue.Enqueue(target);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            //dose the collision has a tag that we are interested with?
            var pair = weightPerTags.FirstOrDefault(x => other.CompareTag(x.tag));
            if (pair.tag.IsNullOrWhitespace()) return;

            if (other.TryGetComponent(out ITargetable target))
            {
                priorityQueue.Remove(target);
            }
        }
    }

    [Serializable]
    public struct WeightPerTag
    {
        public string tag;
        public float weight;
    }
}