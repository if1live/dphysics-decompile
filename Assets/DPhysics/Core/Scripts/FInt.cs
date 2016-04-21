using System;

public struct FInt
{
    public const int SHIFT_AMOUNT = 20;

    public const long MAX_VALUE = 8796093022207L;

    public const long OneRaw = 1048576L;

    public long RawValue;

    public static FInt OneF;

    public static FInt TwoF;

    public static FInt ZeroF;

    public static FInt HalfF;

    public static FInt MaxValue;

    static FInt()
    {
        FInt.OneF = FInt.Create(1);
        FInt.TwoF = FInt.Create(2);
        FInt.ZeroF = FInt.Create(0);
        FInt.HalfF = FInt.Create((long)524288);
        FInt.MaxValue = FInt.Create(9223372036854775807L);
    }

    public void AbsoluteValue(out FInt ret)
    {
        //ret = new FInt();
        if (this.RawValue >= (long)0)
        {
            ret.RawValue = this.RawValue;
            return;
        }
        ret.RawValue = -this.RawValue;
    }

    public bool AbsoluteValueMoreThan(long OtherRawValue)
    {
        if (this.RawValue > OtherRawValue)
        {
            return true;
        }
        return this.RawValue < -OtherRawValue;
    }

    public void Add(long OtherRawValue, out FInt ret)
    {
        //ret = new FInt();
        ret.RawValue = this.RawValue + OtherRawValue;
    }

    public void Add(int OtherValue, out FInt ret)
    {
        //ret = new FInt();
        ret.RawValue = this.RawValue + ((long)OtherValue << 20);
    }

    public static FInt Create(int StartingValue)
    {
        FInt fInt = new FInt();
        fInt.RawValue = (long)StartingValue;
        fInt.RawValue = fInt.RawValue << 20;
        return fInt;
    }

    public static FInt Create(long StartingRawValue)
    {
        FInt fInt = new FInt();
        fInt.RawValue = StartingRawValue;
        return fInt;
    }

    public static FInt Create(float FloatValue)
    {
        FInt fInt = new FInt();
        fInt.RawValue = (long)((decimal)((float)FloatValue) * new decimal(1048576));
        return fInt;
    }

    public static FInt Create(double DoubleValue)
    {
        FInt fInt = new FInt();
        fInt.RawValue = (long)((decimal)((double)DoubleValue) * new decimal(1048576));
        return fInt;
    }

    public void Divide(long OtherRawValue, out FInt ret)
    {
        //ret = new FInt();
        ret.RawValue = (this.RawValue << 20) / OtherRawValue;
    }

    public void Divide(int OtherValue, out FInt ret)
    {
        //ret = new FInt();
        ret.RawValue = this.RawValue / (long)OtherValue;
    }

    public bool Equals(long OtherRawValue)
    {
        if (this.RawValue == OtherRawValue)
        {
            return true;
        }
        return false;
    }

    public static FInt FromParts(long PreDecimal, long PostDecimal)
    {
        FInt rawValue = FInt.Create(PreDecimal);
        if (PostDecimal != (long)0)
        {
            rawValue.RawValue = rawValue.RawValue + (FInt.Create((double)PostDecimal) / 1000).RawValue;
        }
        return rawValue;
    }

    public override int GetHashCode()
    {
        return this.RawValue.GetHashCode();
    }

    public void Inverse(out FInt ret)
    {
        //ret = new FInt();
        ret.RawValue = this.RawValue * (long)-1;
    }

    public bool Less(long OtherRawValue)
    {
        return this.RawValue < OtherRawValue;
    }

    public bool LessEquals(long OtherRawValue)
    {
        return this.RawValue <= OtherRawValue;
    }

    public void Modulo(long OtherRawValue, out FInt ret)
    {
        //ret = new FInt();
        ret.RawValue = this.RawValue % OtherRawValue;
    }

    public void Modulo(int OtherValue, out FInt ret)
    {
        //ret = new FInt();
        ret.RawValue = this.RawValue % ((long)OtherValue << 20);
    }

    public bool More(long OtherRawValue)
    {
        return this.RawValue > OtherRawValue;
    }

    public bool MoreEquals(long OtherRawValue)
    {
        return this.RawValue >= OtherRawValue;
    }

    public void Multiply(long OtherRawValue, out FInt ret)
    {
        //ret = new FInt();
        ret.RawValue = this.RawValue * OtherRawValue >> 20;
    }

    public void Multiply(int OtherValue, out FInt ret)
    {
        //ret = new FInt();
        ret.RawValue = this.RawValue * (long)OtherValue;
    }

    public static FInt operator +(FInt one, FInt other)
    {
        one.RawValue = one.RawValue + other.RawValue;
        return one;
    }

    public static FInt operator +(FInt one, int other)
    {
        one.RawValue = one.RawValue + ((long)other << 20);
        return one;
    }

    public static FInt operator /(FInt one, FInt other)
    {
        one.RawValue = (one.RawValue << 20) / other.RawValue;
        return one;
    }

    public static FInt operator /(FInt one, int divisor)
    {
        one.RawValue = one.RawValue / (long)divisor;
        return one;
    }

    public static bool operator ==(FInt one, FInt other)
    {
        return one.Equals(other.RawValue);
    }

    public static bool operator !=(FInt one, FInt other)
    {
        return !one.Equals(other.RawValue);
    }

    public static FInt operator %(FInt one, FInt other)
    {
        one.Modulo(other.RawValue, out one);
        return one;
    }

    public static FInt operator *(FInt one, FInt other)
    {
        one.RawValue = one.RawValue * other.RawValue >> 20;
        return one;
    }

    public static FInt operator *(FInt one, int multi)
    {
        one.RawValue = one.RawValue * (long)multi;
        return one;
    }

    public static FInt operator -(FInt one, FInt other)
    {
        one.RawValue = one.RawValue - other.RawValue;
        return one;
    }

    public static FInt operator -(FInt one, int other)
    {
        one.RawValue = one.RawValue - ((long)other << 20);
        return one;
    }

    public static FInt operator -(FInt src)
    {
        src.Inverse(out src);
        return src;
    }

    public void Sign()
    {
        if (this.RawValue != (long)0)
        {
            if (this.RawValue > (long)0)
            {
                this.RawValue = FInt.OneF.RawValue;
                return;
            }
            this.RawValue = -FInt.OneF.RawValue;
        }
    }

    public void Subtract(long OtherRawValue, out FInt ret)
    {
        //ret = new FInt();
        ret.RawValue = this.RawValue - OtherRawValue;
    }

    public void Subtract(int OtherValue, out FInt ret)
    {
        //ret = new FInt();
        ret.RawValue = this.RawValue - ((long)OtherValue << 20);
    }

    public double ToDouble()
    {
        return (double)this.RawValue / 1048576;
    }

    public float ToFloat()
    {
        return (float)((double)this.RawValue / 1048576);
    }

    public int ToInt()
    {
        return (int)(this.RawValue >> 20);
    }

    public short ToRoundedShort()
    {
        return (short)(this.RawValue >> 20);
    }

    public override string ToString()
    {
        return this.RawValue.ToString();
    }
}