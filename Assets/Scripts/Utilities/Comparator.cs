using System;
public class Comparator
{
    private float _threshold;

    public Comparator(float threshold)
    {
        _threshold = threshold;
    }

    public bool ApproximatelyEqual(ref float a, ref float b)
    {
        return ((a - b) < 0 ? ((a - b) * -1) : (a - b)) <= _threshold;
    }
}