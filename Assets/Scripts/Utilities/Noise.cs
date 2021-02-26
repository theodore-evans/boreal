using UnityEngine;

class Noise
{ 
    public static float NextGaussian(System.Random prng, float mu = 0, float sigma = 1)
    {
        float u1 = (float)prng.NextDouble();
        float u2 = (float)prng.NextDouble();

        float rand_std_normal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) *
                            Mathf.Sin(2.0f * Mathf.PI * u2);

        float rand_normal = mu + sigma * rand_std_normal;

        return rand_normal;
    }

}
