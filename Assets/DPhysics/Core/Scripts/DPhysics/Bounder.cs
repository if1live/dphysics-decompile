using System;

namespace DPhysics
{
    public class Bounder
    {
        private DCollider polygon;

        public FInt Radius;

        private FInt SqrRadius;

        public long xMax;

        public long xMin;

        public long yMax;

        public long yMin;

        public bool IsCircle;

        public Bounder(DCollider pol)
        {
            this.polygon = pol;
            this.BuildBounds(true);
        }

        public void BuildBounds(bool Thorough)
        {
            FInt fInt;
            if (this.polygon.IsCircle)
            {
                this.Radius = this.polygon.radius;
                this.yMax = this.Radius.RawValue;
                this.xMax = this.yMax;
                this.yMin = -this.Radius.RawValue;
                this.xMin = this.yMin;
                this.IsCircle = true;
                return;
            }
            if ((int)this.polygon.backupPoints.Length > 0)
            {
                this.xMin = (long)0;
                this.xMax = (long)0;
                this.yMin = (long)0;
                this.yMax = (long)0;
                Vector2d[] points = this.polygon.Points;
                for (int i = 0; i < (int)points.Length; i++)
                {
                    Vector2d vector2d = points[i];
                    if (Thorough)
                    {
                        vector2d.SqrMagnitude(out fInt);
                        if (fInt.RawValue > this.SqrRadius.RawValue)
                        {
                            this.SqrRadius = fInt;
                            Mathd.Sqrt(fInt.RawValue, out this.Radius);
                        }
                    }
                    if (vector2d.x.RawValue < this.xMin)
                    {
                        this.xMin = vector2d.x.RawValue;
                    }
                    else if (vector2d.x.RawValue > this.xMax)
                    {
                        this.xMax = vector2d.x.RawValue;
                    }
                    if (vector2d.y.RawValue < this.yMin)
                    {
                        this.yMin = vector2d.y.RawValue;
                    }
                    else if (vector2d.y.RawValue > this.yMax)
                    {
                        this.yMax = vector2d.y.RawValue;
                    }
                }
                if (Thorough && ((this.Radius * this.Radius) * Mathd.PI).RawValue <= (this.yMax - this.yMin) * (this.xMax - this.xMin) >> 20)
                {
                    this.IsCircle = true;
                    this.xMax = this.Radius.RawValue;
                    this.yMax = this.xMax;
                    this.yMin = -this.Radius.RawValue;
                    this.xMin = this.yMin;
                }
            }
        }

        public static bool CanIntersect(DCollider polyA, DCollider polyB)
        {
            if (polyA.MyBounds.xMax >= polyB.MyBounds.xMin && polyA.MyBounds.xMin <= polyB.MyBounds.xMax && polyA.MyBounds.yMax >= polyB.MyBounds.yMin && polyA.MyBounds.yMin <= polyB.MyBounds.yMax)
            {
                return true;
            }
            return false;
        }

        public static bool CanIntersect(DCollider polyA, DCollider polyB, ref FInt CombinedSqrRadius, out FInt sqrdistance)
        {
            sqrdistance = new FInt();
            Vector2d vector2d;
            if (polyA.MyBounds.xMax >= polyB.MyBounds.xMin && polyA.MyBounds.xMin <= polyB.MyBounds.xMax && polyA.MyBounds.yMax >= polyB.MyBounds.yMin && polyA.MyBounds.yMin <= polyB.MyBounds.yMax)
            {
                polyA.center.Subtract(ref polyB.center, out vector2d);
                vector2d.SqrMagnitude(out sqrdistance);
                if (sqrdistance.RawValue <= CombinedSqrRadius.RawValue)
                {
                    return true;
                }
            }
            sqrdistance.RawValue = (long)0;
            return false;
        }

        public void Offset(ref Vector2d change)
        {
            Bounder rawValue = this;
            rawValue.xMin = rawValue.xMin + change.x.RawValue;
            Bounder bounder = this;
            bounder.xMax = bounder.xMax + change.x.RawValue;
            Bounder rawValue1 = this;
            rawValue1.yMin = rawValue1.yMin + change.y.RawValue;
            Bounder bounder1 = this;
            bounder1.yMax = bounder1.yMax + change.y.RawValue;
        }
    }
}