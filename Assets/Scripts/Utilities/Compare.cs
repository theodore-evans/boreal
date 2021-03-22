public static class Compare
{
    public static bool ApproximatelyEqual(float a, float b, float threshold)
    {
        if (threshold == 0) return a == b;
        return ((a - b) < 0 ? ((a - b) * -1) : (a - b)) <= threshold;
    }
}