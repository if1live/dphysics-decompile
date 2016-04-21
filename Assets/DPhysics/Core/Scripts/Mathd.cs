using System;

public static class Mathd
{
    private const int HalfShift = 10;

    public static FInt PI;

    static Mathd()
    {
        Mathd.PI = FInt.Create(3.1415);
    }

    public static long IntSqrt(long d)
    {
        long i;
        if (d <= (long)0)
        {
            return d;
        }
        long num = (d >> 1) + (long)1;
        for (i = num + d / num >> 1; i < num; i = num + d / num >> 1)
        {
            num = i;
        }
        return i;
    }

    public static void Sqrt(long RawValue, out FInt ret)
    {
        //ret = new FInt();
        if (RawValue <= (long)0)
        {
            ret.RawValue = RawValue;
            return;
        }
        ret.RawValue = Mathd.IntSqrt(RawValue) << 10;
    }
}