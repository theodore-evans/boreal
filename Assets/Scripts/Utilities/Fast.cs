using System;
public static class Fast
{
    public static bool Approximately(float a, float b, float threshold)
    {
        return ((a - b) < 0 ? ((a - b) * -1) : (a - b)) <= threshold;
    }
}