using System;

namespace DPhysics
{
    public class CollisionResult
    {
        public bool Intersect;

        public Vector2d PenetrationVector;

        public Vector2d PenetrationDirection;

        public CollisionPair pair;

        public CollisionResult(CollisionPair _pair)
        {
            this.pair = _pair;
        }

        public void Calculate()
        {
            FInt fInt;
            FInt fInt1;
            Vector2d vector2d;
            Vector2d vector2d1;
            Vector2d vector2d2;
            Vector2d vector2d3;
            Vector2d vector2d4;
            FInt fInt2;
            int num;
            this.Intersect = false;
            this.PenetrationVector = Vector2d.zero;
            DCollider bodyA = this.pair.BodyA.dCollider;
            DCollider bodyB = this.pair.BodyB.dCollider;
            if (bodyA.MyBounds.IsCircle && bodyB.MyBounds.IsCircle)
            {
                if (Bounder.CanIntersect(bodyA, bodyB, ref this.pair.CombinedSqrRadius, out fInt))
                {
                    if (!bodyA.IsCircle || !bodyB.IsCircle)
                    {
                        goto Label0;
                    }
                    this.Intersect = true;
                    Mathd.Sqrt(fInt.RawValue, out fInt1);
                    if (fInt1.RawValue <= (long)0)
                    {
                        bodyA.center.Subtract(ref this.pair.BodyA.Velocity, out vector2d1);
                        bodyB.center.Subtract(ref this.pair.BodyB.Velocity, out vector2d2);
                        vector2d1.Subtract(ref vector2d2, out vector2d);
                        vector2d.Magnitude(out fInt1);
                        vector2d.Normalize();
                    }
                    else
                    {
                        bodyA.center.Subtract(ref bodyB.center, out vector2d);
                        vector2d.Normalize();
                    }
                    FInt fInt3 = (bodyA.radius + bodyB.radius) - fInt1;
                    vector2d.Multiply(fInt3.RawValue, out this.PenetrationVector);
                    this.PenetrationDirection = vector2d;
                }
                return;
            }
            else if (!Bounder.CanIntersect(bodyA, bodyB))
            {
                return;
            }
        Label0:
            num = (bodyA.Edges != null ? (int)bodyA.Edges.Length : 0);
            int num1 = num;
            int num2 = (bodyB.Edges != null ? (int)bodyB.Edges.Length : 0);
            FInt maxValue = FInt.MaxValue;
            Vector2d vector2d5 = new Vector2d();
            this.Intersect = true;
            for (int i = 0; i < num1 + num2; i++)
            {
                vector2d3 = (i >= num1 ? bodyB.Edges[i - num1] : bodyA.Edges[i]);
                Vector2d vector2d6 = vector2d3.localright;
                vector2d6.Normalize();
                FInt zeroF = FInt.ZeroF;
                FInt zeroF1 = FInt.ZeroF;
                FInt zeroF2 = FInt.ZeroF;
                FInt zeroF3 = FInt.ZeroF;
                CollisionResult.ProjectPolygon(vector2d6, bodyA, out zeroF, out zeroF2);
                CollisionResult.ProjectPolygon(vector2d6, bodyB, out zeroF1, out zeroF3);
                FInt fInt4 = CollisionResult.IntervalDistance(zeroF, zeroF2, zeroF1, zeroF3);
                if (fInt4.RawValue >= (long)0)
                {
                    this.Intersect = false;
                    return;
                }
                fInt4.Inverse(out fInt4);
                if (fInt4.RawValue < maxValue.RawValue)
                {
                    maxValue = fInt4;
                    vector2d5 = vector2d6;
                    bodyA.center.Subtract(ref bodyB.center, out vector2d4);
                    Vector2d.Dot(ref vector2d4, ref vector2d5, out fInt2);
                    if (fInt2.RawValue < (long)0)
                    {
                        vector2d5.Invert();
                    }
                }
            }
            this.PenetrationDirection = vector2d5;
            vector2d5.Multiply(maxValue.RawValue, out this.PenetrationVector);
        }

        public static FInt IntervalDistance(FInt minA, FInt maxA, FInt minB, FInt maxB)
        {
            if (minA.RawValue < minB.RawValue)
            {
                return minB - maxA;
            }
            return minA - maxB;
        }

        public static void ProjectPolygon(Vector2d axis, DCollider dCollider, out FInt min, out FInt max)
        {
            min = new FInt();
            max = new FInt();
            FInt fInt;
            FInt fInt1;
            min.RawValue = (long)0;
            max.RawValue = (long)0;
            if (dCollider.IsCircle)
            {
                Vector2d.Dot(ref dCollider.center, ref axis, out fInt);
                fInt.Subtract(dCollider.radius.RawValue, out min);
                fInt.Add(dCollider.radius.RawValue, out max);
                return;
            }
            Vector2d.Dot(ref axis, ref dCollider.points[0], out fInt1);
            min.RawValue = fInt1.RawValue;
            max.RawValue = fInt1.RawValue;
            for (int i = 0; i < (int)dCollider.points.Length; i++)
            {
                Vector2d.Dot(ref dCollider.points[i], ref axis, out fInt1);
                if (fInt1.RawValue < min.RawValue)
                {
                    min = fInt1;
                }
                else if (fInt1.RawValue > max.RawValue)
                {
                    max = fInt1;
                }
            }
        }
    }
}