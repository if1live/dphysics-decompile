using System;
using System.Reflection;
using UnityEngine;

public struct Vector2d
{
    public FInt x;

    public FInt y;

    public static Vector2d zero;

    public static Vector2d one;

    public static Vector2d up;

    public static Vector2d right;

    public FInt this[int index]
    {
        get
        {
            switch (index)
            {
                case 0:
                {
                    return this.x;
                }
                case 1:
                {
                    return this.y;
                }
            }
            throw new IndexOutOfRangeException("Invalid Vector2d index!");
        }
        set
        {
            switch (index)
            {
                case 0:
                {
                    this.x = value;
                    return;
                }
                case 1:
                {
                    this.y = value;
                    return;
                }
            }
            throw new IndexOutOfRangeException("Invalid Vector2d index!");
        }
    }

    public Vector2d localright
    {
        get
        {
            return new Vector2d(this.y, -this.x);
        }
    }

    static Vector2d()
    {
        Vector2d.zero = new Vector2d(0, 0);
        Vector2d.one = new Vector2d(1, 1);
        Vector2d.up = new Vector2d(0, 1);
        Vector2d.right = new Vector2d(1, 0);
    }

    public Vector2d(FInt x, FInt y)
    {
        this.x = x;
        this.y = y;
    }

    public Vector2d(int xInt, int yInt)
    {
        this.x.RawValue = (long)xInt << 20;
        this.y.RawValue = (long)yInt << 20;
    }

    public void Add(ref Vector2d Other, out Vector2d ret)
    {
        //ret = new Vector2d();
        this.x.Add(Other.x.RawValue, out ret.x);
        this.y.Add(Other.y.RawValue, out ret.y);
    }

    public static void Cross(ref Vector2d U, ref Vector2d B, out FInt ret)
    {
        FInt fInt;
        U.x.Multiply(B.y.RawValue, out ret);
        U.y.Multiply(B.x.RawValue, out fInt);
        ret.Subtract(fInt.RawValue, out ret);
    }

    public void Divide(long OtherRawValue, out Vector2d ret)
    {
        //ret = new Vector2d();
        this.x.Divide(OtherRawValue, out ret.x);
        this.y.Divide(OtherRawValue, out ret.y);
    }

    public void Divide(int OtherValue, out Vector2d ret)
    {
        //ret = new Vector2d();
        this.x.Divide(OtherValue, out ret.x);
        this.y.Divide(OtherValue, out ret.y);
    }

    public static void Dot(ref Vector2d lhs, ref Vector2d rhs, out FInt ret)
    {
        FInt fInt;
        lhs.x.Multiply(rhs.x.RawValue, out ret);
        lhs.y.Multiply(rhs.y.RawValue, out fInt);
        ret.Add(fInt.RawValue, out ret);
    }

    public bool Equals(ref Vector2d other)
    {
        if (this.x.RawValue != other.x.RawValue)
        {
            return false;
        }
        return this.y.RawValue == other.y.RawValue;
    }

    public override int GetHashCode()
    {
        return this.x.GetHashCode() ^ this.y.GetHashCode() << 2;
    }

    public void Invert()
    {
        this.x.RawValue = -this.x.RawValue;
        this.y.RawValue = -this.y.RawValue;
    }

    public FInt Magnitude(out FInt ret)
    {
        //ret = new FInt();
        FInt fInt;
        FInt fInt1;
        this.x.AbsoluteValue(out fInt);
        if (fInt.RawValue == (long)0)
        {
            if (this.y.RawValue == (long)-1 || this.y.RawValue == (long)1)
            {
                ret.RawValue = (long)1048576;
            }
        }
        else if (fInt.RawValue == (long)1 && this.y.RawValue == (long)0)
        {
            ret.RawValue = (long)1048576;
        }
        this.x.Multiply(this.x.RawValue, out ret);
        this.y.Multiply(this.y.RawValue, out fInt1);
        ret.Add(fInt1.RawValue, out ret);
        Mathd.Sqrt(ret.RawValue, out ret);
        return ret;
    }

    public void Multiply(long OtherRawValue, out Vector2d ret)
    {
        //ret = new Vector2d();
        this.x.Multiply(OtherRawValue, out ret.x);
        this.y.Multiply(OtherRawValue, out ret.y);
    }

    public void Multiply(int OtherValue, out Vector2d ret)
    {
        //ret = new Vector2d();
        this.x.Multiply(OtherValue, out ret.x);
        this.y.Multiply(OtherValue, out ret.y);
    }

    public void Normalize()
    {
        FInt fInt;
        FInt fInt1;
        FInt fInt2;
        if (this.x.RawValue == (long)0 && this.y.RawValue == (long)0)
        {
            return;
        }
        this.x.AbsoluteValue(out fInt);
        this.y.AbsoluteValue(out fInt1);
        if (fInt.RawValue == (long)0)
        {
            if (fInt1.RawValue == (long)1048576)
            {
                return;
            }
        }
        else if (fInt1.RawValue == (long)0 && fInt.RawValue == (long)1048576)
        {
            return;
        }
        if (fInt.RawValue <= fInt1.RawValue)
        {
            this.Divide(fInt1.RawValue + fInt.RawValue / (long)2, out this);
        }
        else
        {
            this.Divide(fInt.RawValue + fInt1.RawValue / (long)2, out this);
        }
        this.Magnitude(out fInt2);
        if (fInt2.RawValue > (long)0 && fInt2.RawValue != (long)1048576)
        {
            this.Divide(fInt2.RawValue, out this);
        }
    }

    public static Vector2d operator +(Vector2d a, Vector2d b)
    {
        a.Add(ref b, out a);
        return a;
    }

    public static Vector2d operator /(Vector2d a, FInt d)
    {
        a.Divide(d.RawValue, out a);
        return a;
    }

    public static Vector2d operator /(Vector2d a, int d)
    {
        a.Divide(d, out a);
        return a;
    }

    public static explicit operator Vector2d(Vector3 v)
    {
        return new Vector2d(FInt.Create(v.x), FInt.Create(v.z));
    }

    public static explicit operator Vector3(Vector2d v)
    {
        return new Vector3(v.x.ToFloat(), 0f, v.y.ToFloat());
    }

    public static Vector2d operator *(Vector2d a, FInt d)
    {
        a.Multiply(d.RawValue, out a);
        return a;
    }

    public static Vector2d operator *(Vector2d a, int d)
    {
        a.Multiply(d, out a);
        return a;
    }

    public static Vector2d operator -(Vector2d a, Vector2d b)
    {
        a.Subtract(ref b, out a);
        return a;
    }

    public static Vector2d operator -(Vector2d a)
    {
        return new Vector2d(-a.x, -a.y);
    }

    public static void Reflect(ref Vector2d vector, ref Vector2d normal, out Vector2d ret)
    {
        FInt fInt;
        Vector2d.Dot(ref vector, ref normal, out fInt);
        normal.Multiply(fInt.RawValue, out ret);
        ret.Multiply(2, out ret);
        ret.Subtract(ref vector, out ret);
    }

    public void Rotate(long CosRaw, long SinRaw, out Vector2d ret)
    {
        //ret = new Vector2d();
        FInt fInt;
        FInt fInt1;
        FInt fInt2;
        this.x.Multiply(CosRaw, out fInt1);
        this.y.Multiply(-SinRaw, out fInt);
        fInt1.Add(fInt.RawValue, out fInt1);
        this.x.Multiply(SinRaw, out fInt2);
        this.y.Multiply(CosRaw, out fInt);
        fInt2.Add(fInt.RawValue, out fInt2);
        ret.x = -fInt2;
        ret.y = fInt1;
    }

    public void RotateTowards(ref Vector2d target, FInt amount, out Vector2d ret)
    {
        FInt fInt;
        int num;
        Vector2d vector2d;
        Vector2d vector2d1;
        FInt fInt1;
        ret = this;
        Vector2d vector2d2 = this.localright;
        Vector2d.Dot(ref vector2d2, ref target, out fInt);
        if (fInt.RawValue != (long)0)
        {
            num = (fInt.RawValue <= (long)0 ? -1 : 1);
        }
        else
        {
            num = 0;
        }
        if (!num.Equals(0))
        {
            vector2d2.Multiply(num, out vector2d1);
            vector2d1.Multiply(amount.RawValue, out vector2d1);
            this.Add(ref vector2d1, out vector2d);
            Vector2d vector2d3 = vector2d.localright;
            Vector2d.Dot(ref vector2d3, ref target, out fInt1);
            fInt1.Sign();
            if (fInt1.RawValue == (long)num || fInt1.RawValue > (long)0 == num > 0)
            {
                vector2d.Normalize();
                ret = vector2d;
                return;
            }
            ret = target;
        }
    }

    public FInt SqrMagnitude(out FInt ret)
    {
        FInt fInt;
        this.x.Multiply(this.x.RawValue, out ret);
        this.y.Multiply(this.y.RawValue, out fInt);
        ret.Add(fInt.RawValue, out ret);
        return ret;
    }

    public void Subtract(ref Vector2d Other, out Vector2d ret)
    {
        //ret = new Vector2d();
        this.x.Subtract(Other.x.RawValue, out ret.x);
        this.y.Subtract(Other.y.RawValue, out ret.y);
    }

    public Vector2 ToSinglePrecision()
    {
        return new Vector2(this.x.ToFloat(), this.y.ToFloat());
    }

    public override string ToString()
    {
        string[] str = new string[] { "(", null, null, null, null };
        double num = this.x.ToDouble();
        str[1] = num.ToString();
        str[2] = ", ";
        double num1 = this.y.ToDouble();
        str[3] = num1.ToString();
        str[4] = ")";
        return string.Concat(str);
    }
}