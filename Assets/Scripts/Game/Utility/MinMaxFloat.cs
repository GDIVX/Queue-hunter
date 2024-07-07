using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Utility
{
    /// <summary>
    /// Represents a range with minimum and maximum float values.
    /// </summary>
    [Serializable]
    public struct MinMaxFloat
    {
        [SerializeField] private float min;
        [SerializeField] private float max;

        /// <summary>
        /// Gets or sets the minimum value of the range.
        /// </summary>
        public float Min
        {
            readonly get => min;
            set
            {
                if (value > max)
                {
                    max = value;
                }

                min = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum value of the range.
        /// </summary>
        public float Max
        {
            readonly get => max;
            set
            {
                if (value < min)
                {
                    min = value;
                }

                max = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MinMaxFloat"/> struct with specified minimum and maximum values.
        /// </summary>
        /// <param name="min">The minimum value of the range.</param>
        /// <param name="max">The maximum value of the range.</param>
        public MinMaxFloat(float min, float max)
        {
            this.min = min > max ? max : min;
            this.max = max < min ? min : max;
        }

        /// <summary>
        /// Gets a random value within the range.
        /// </summary>
        /// <returns>A random float value between Min and Max.</returns>
        public float GetRandomValue()
        {
            return Random.Range(Min, Max);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"Min: {Min}, Max: {Max}";
        }
    }
}