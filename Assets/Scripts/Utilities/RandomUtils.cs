using System.Collections.Generic;
using UnityEngine;

    static class RandomUtils
    {
        static System.Random rng = new System.Random();

        public static float NextGaussian(float mu = 0, float sigma = 1)
        {
            float u1 = (float)rng.NextDouble();
            float u2 = (float)rng.NextDouble();

            float rand_std_normal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) *
                                Mathf.Sin(2.0f * Mathf.PI * u2);

            float rand_normal = mu + sigma * rand_std_normal;

            return rand_normal;
        }
    }
