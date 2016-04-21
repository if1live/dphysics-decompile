using System;
using System.Collections.Generic;
using UnityEngine;

namespace DPhysics
{
    public static class DPhysicsManager
    {
        public const int MaxPhysicsObjects = 2000;

        public static FInt SimulationDelta;

        public static FInt Drag;

        public static FInt SleepVelocity;

        public static FInt CollisionDamp;

        public static FInt MinimumCollisionOffset;

        public static FInt Restitution;

        public static int MaxActionsUpon;

        private static Stack<ushort> AvailableCounts;

        private static Vector2d randomDirection;

        private static Vector2d randomChange;

        public static Body[] SimObjects;

        private static ushort PeakCount;

        private static float LastSimulateTime;

        public static Vector2d RandomDirection
        {
            get
            {
                DPhysicsManager.randomDirection.Rotate(DPhysicsManager.randomChange.x.RawValue, DPhysicsManager.randomChange.y.RawValue, out DPhysicsManager.randomDirection);
                FInt fInt = DPhysicsManager.randomChange.x;
                DPhysicsManager.randomChange.x = DPhysicsManager.randomChange.y;
                DPhysicsManager.randomChange.y = -fInt;
                DPhysicsManager.randomChange.Normalize();
                DPhysicsManager.randomDirection.Normalize();
                return DPhysicsManager.randomDirection;
            }
        }

        static DPhysicsManager()
        {
            DPhysicsManager.SimulationDelta = FInt.Create(0.1);
            DPhysicsManager.Drag = FInt.Create(0.8);
            DPhysicsManager.SleepVelocity = FInt.Create(0.1);
            DPhysicsManager.CollisionDamp = FInt.Create(0.8);
            DPhysicsManager.MinimumCollisionOffset = FInt.Create(0.1);
            DPhysicsManager.Restitution = FInt.Create(0.8);
            DPhysicsManager.MaxActionsUpon = 8;
            DPhysicsManager.AvailableCounts = new Stack<ushort>();
            DPhysicsManager.randomDirection = new Vector2d(3, 5);
            DPhysicsManager.randomChange = new Vector2d(-13, 7);
            DPhysicsManager.SimObjects = new Body[2000];
            DPhysicsManager.PeakCount = 0;
        }

        public static void ApplyGlobalForce(Vector2d force)
        {
            Body[] simObjects = DPhysicsManager.SimObjects;
            for (int i = 0; i < (int)simObjects.Length; i++)
            {
                Body body = simObjects[i];
                if (body != null)
                {
                    body.ApplyForce(ref force);
                }
            }
        }

        public static void ApplyGlobalVelocity(Vector2d velocity)
        {
            Body[] simObjects = DPhysicsManager.SimObjects;
            for (int i = 0; i < (int)simObjects.Length; i++)
            {
                Body body = simObjects[i];
                if (body != null)
                {
                    body.ApplyVelocity(ref velocity);
                }
            }
        }

        public static void Assimilate(Body body)
        {
            ushort peakCount;
            if (body.dCollider == null)
            {
                return;
            }
            if (DPhysicsManager.AvailableCounts.Count <= 0)
            {
                if (DPhysicsManager.PeakCount == (int)DPhysicsManager.SimObjects.Length)
                {
                    Debug.LogError("More objects assimilated than capacity. Consider changing the MaxPhysicsObjects value in DPhysicsManager.");
                    return;
                }
                peakCount = DPhysicsManager.PeakCount;
                DPhysicsManager.PeakCount = (ushort)(DPhysicsManager.PeakCount + 1);
            }
            else
            {
                peakCount = DPhysicsManager.AvailableCounts.Pop();
            }
            body.SimID = peakCount;
            DPhysicsManager.SimObjects[peakCount] = body;
            ushort simID = body.SimID;
            Dictionary<ushort, CollisionPair> nums = new Dictionary<ushort, CollisionPair>();
            body.MyPairs = nums;
            for (ushort i = (ushort)(simID + 1); i < DPhysicsManager.PeakCount; i = (ushort)(i + 1))
            {
                Body simObjects = DPhysicsManager.SimObjects[i];
                if (simObjects != null)
                {
                    nums.Add(i, new CollisionPair(body, simObjects));
                }
            }
            for (ushort j = 0; j < simID; j = (ushort)(j + 1))
            {
                Body simObjects1 = DPhysicsManager.SimObjects[j];
                if (simObjects1 != null)
                {
                    simObjects1.MyPairs.Add(simID, new CollisionPair(simObjects1, body));
                }
            }
            Partition.AllBodies.Add(body);
        }

        private static void CheckChange(Body b1)
        {
            bool flag = b1.SetPosition();
            bool flag1 = b1.SetRotation();
            b1.Changed = (flag ? true : flag1);
        }

        public static void Dessimilate(Body body)
        {
            DPhysicsManager.SimObjects[body.SimID] = null;
            DPhysicsManager.AvailableCounts.Push(body.SimID);
            ushort simID = body.SimID;
            for (ushort i = 0; i < simID; i = (ushort)(i + 1))
            {
                Body simObjects = DPhysicsManager.SimObjects[i];
                if (simObjects != null)
                {
                    simObjects.MyPairs.Remove(simID);
                }
            }
            body.MyPairs = null;
            Partition.AllBodies.Remove(body);
        }

        public static void Simulate()
        {
            Vector2d vector2d;
            FInt fInt;
            Vector2d vector2d1;
            FInt fInt1;
            FInt halfF;
            Vector2d vector2d2;
            Vector2d vector2d3;
            FInt fInt2;
            Partition.StartPartitioning();
            for (ushort i = 0; i < DPhysicsManager.PeakCount; i = (ushort)(i + 1))
            {
                Body simObjects = DPhysicsManager.SimObjects[i];
                if (!(simObjects == null) && simObjects.Active)
                {
                    Dictionary<ushort, CollisionPair> myPairs = simObjects.MyPairs;
                    for (ushort j = (ushort)(i + 1); j < DPhysicsManager.PeakCount; j = (ushort)(j + 1))
                    {
                        Body body = DPhysicsManager.SimObjects[j];
                        if (body != null)
                        {
                            CollisionPair item = myPairs[j];
                            item.GenerateCollision();
                            CollisionResult myCollisionResult = item.MyCollisionResult;
                            if (myCollisionResult.Intersect && item.SimulatePhysics)
                            {
                                bool actedCount = simObjects.ActedCount > 0;
                                bool flag = body.ActedCount > 0;
                                if (actedCount)
                                {
                                    Body actedCount1 = simObjects;
                                    actedCount1.ActedCount = actedCount1.ActedCount - 1;
                                }
                                if (flag)
                                {
                                    Body body1 = body;
                                    body1.ActedCount = body1.ActedCount - 1;
                                }
                                bool mass = simObjects.Mass != 0;
                                bool mass1 = body.Mass != 0;
                                if (!simObjects.IsTrigger && !body.IsTrigger && (mass || mass1))
                                {
                                    bool flag1 = true;
                                    body.Velocity.Subtract(ref simObjects.Velocity, out vector2d);
                                    if (vector2d.x.AbsoluteValueMoreThan(DPhysicsManager.SleepVelocity.RawValue) || vector2d.y.AbsoluteValueMoreThan(DPhysicsManager.SleepVelocity.RawValue))
                                    {
                                        Vector2d.Dot(ref myCollisionResult.PenetrationVector, ref vector2d, out fInt);
                                        if (fInt.RawValue < (long)0)
                                        {
                                            flag1 = false;
                                        }
                                    }
                                    if (flag1)
                                    {
                                        myCollisionResult.PenetrationVector.Multiply(DPhysicsManager.CollisionDamp.RawValue, out vector2d1);
                                        if (mass && mass1)
                                        {
                                            simObjects.Speed.Add(body.Speed.RawValue, out fInt1);
                                            if (!fInt1.AbsoluteValueMoreThan(DPhysicsManager.SleepVelocity.RawValue))
                                            {
                                                halfF = FInt.HalfF;
                                            }
                                            else
                                            {
                                                body.Speed.Divide(fInt1.RawValue, out halfF);
                                            }
                                            Vector2d vector2d4 = Vector2d.zero;
                                            if (!halfF.AbsoluteValueMoreThan(DPhysicsManager.MinimumCollisionOffset.RawValue))
                                            {
                                                vector2d1.Multiply(DPhysicsManager.MinimumCollisionOffset.RawValue, out vector2d4);
                                            }
                                            else
                                            {
                                                vector2d1.Multiply(halfF.RawValue, out vector2d4);
                                            }
                                            simObjects.Offset(ref vector2d4);
                                            halfF = halfF - FInt.OneF;
                                            if (!halfF.AbsoluteValueMoreThan(DPhysicsManager.MinimumCollisionOffset.RawValue))
                                            {
                                                vector2d1.Multiply(DPhysicsManager.MinimumCollisionOffset.RawValue, out vector2d4);
                                            }
                                            else
                                            {
                                                vector2d1.Multiply(halfF.RawValue, out vector2d4);
                                            }
                                            body.Offset(ref vector2d4);
                                        }
                                        else if (mass)
                                        {
                                            simObjects.Offset(ref vector2d1);
                                        }
                                        else
                                        {
                                            myCollisionResult.PenetrationVector.Invert();
                                            body.Offset(ref vector2d1);
                                        }
                                    }
                                    if (actedCount && flag)
                                    {
                                        if (mass)
                                        {
                                            Vector2d.Dot(ref simObjects.Velocity, ref myCollisionResult.PenetrationDirection, out fInt2);
                                            myCollisionResult.PenetrationDirection.Multiply(fInt2.RawValue, out vector2d2);
                                            vector2d2.Multiply((DPhysicsManager.Restitution * simObjects.Mass).RawValue, out vector2d2);
                                        }
                                        else
                                        {
                                            Vector2d.Dot(ref body.Velocity, ref myCollisionResult.PenetrationDirection, out fInt2);
                                            myCollisionResult.PenetrationDirection.Multiply(fInt2.RawValue, out vector2d2);
                                            vector2d2.Multiply(DPhysicsManager.Restitution.RawValue * (long)body.Mass, out vector2d2);
                                            vector2d2.Invert();
                                        }
                                        if (mass1)
                                        {
                                            Vector2d.Dot(ref body.Velocity, ref myCollisionResult.PenetrationDirection, out fInt2);
                                            myCollisionResult.PenetrationDirection.Multiply(fInt2.RawValue, out vector2d3);
                                            vector2d3.Multiply((DPhysicsManager.Restitution * body.Mass).RawValue, out vector2d3);
                                        }
                                        else
                                        {
                                            Vector2d.Dot(ref simObjects.Velocity, ref myCollisionResult.PenetrationDirection, out fInt2);
                                            myCollisionResult.PenetrationDirection.Multiply(fInt2.RawValue, out vector2d3);
                                            vector2d3.Multiply(DPhysicsManager.Restitution.RawValue * (long)simObjects.Mass, out vector2d3);
                                            vector2d3.Invert();
                                        }
                                        simObjects.ApplyForce(ref vector2d3);
                                        body.ApplyForce(ref vector2d2);
                                        vector2d2.Invert();
                                        vector2d3.Invert();
                                        simObjects.ApplyForce(ref vector2d2);
                                        body.ApplyForce(ref vector2d3);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            Body[] bodyArray = DPhysicsManager.SimObjects;
            for (int k = 0; k < (int)bodyArray.Length; k++)
            {
                Body body2 = bodyArray[k];
                if (body2 != null)
                {
                    DPhysicsManager.SimulateBody(body2);
                }
            }
            DPhysicsManager.LastSimulateTime = Time.time;
        }

        private static void SimulateBody(Body b1)
        {
            Vector2d vector2d;
            Vector2d vector2d1;
            Vector2d vector2d2;
            Vector2d vector2d3;
            b1.ActedCount = DPhysicsManager.MaxActionsUpon;
            bool flag = false;
            if (b1.Velocity.x.AbsoluteValueMoreThan(DPhysicsManager.SleepVelocity.RawValue) || b1.Velocity.y.AbsoluteValueMoreThan(DPhysicsManager.SleepVelocity.RawValue))
            {
                b1.Velocity.Multiply(DPhysicsManager.SimulationDelta.RawValue, out vector2d);
                b1.Offset(ref vector2d);
                b1.Velocity.Multiply(DPhysicsManager.Drag.RawValue, out b1.Velocity);
                flag = true;
            }
            else
            {
                b1.Velocity = Vector2d.zero;
            }
            if (!b1.Velocity.Equals(b1.LastVelocity))
            {
                if (!flag)
                {
                    b1.Speed = FInt.ZeroF;
                }
                else
                {
                    b1.Velocity.Magnitude(out b1.Speed);
                }
                b1.LastVelocity = b1.Velocity;
            }
            if (b1.RotationalVelocity.AbsoluteValueMoreThan(DPhysicsManager.SleepVelocity.RawValue))
            {
                Vector2d rotation = b1.Rotation.localright;
                rotation.Multiply(b1.RotationalVelocity.RawValue, out vector2d1);
                b1.Rotation.Add(ref vector2d1, out vector2d1);
                vector2d1.Normalize();
                b1.Rotation = vector2d1;
                b1.RotationalVelocity.Multiply(DPhysicsManager.Drag.RawValue, out b1.RotationalVelocity);
            }
            else if (flag && b1.Speed.RawValue > (long)0)
            {
                b1.RotationalVelocity = FInt.ZeroF;
                b1.Velocity.Divide(b1.Speed.RawValue, out vector2d3);
                Vector2d rotation1 = b1.Rotation;
                rotation1.RotateTowards(ref vector2d3, DPhysicsManager.SimulationDelta * (b1.Speed / b1.Mass), out vector2d2);
                b1.Rotation = vector2d2;
            }
            DPhysicsManager.CheckChange(b1);
        }

        public static void Visualize()
        {
            Body[] simObjects = DPhysicsManager.SimObjects;
            for (int i = 0; i < (int)simObjects.Length; i++)
            {
                Body body = simObjects[i];
                if (body != null && body.Active)
                {
                    body.Visualize((Time.time - DPhysicsManager.LastSimulateTime) / Time.fixedDeltaTime);
                }
            }
        }
    }
}