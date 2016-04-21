using System;
using System.Collections.Generic;

namespace DPhysics
{
    public static class Partition
    {
        private const int MaxContained = 16;

        private const int DefiniteMaxDepth = 7;

        private static int MaxDepth;

        private static FInt MinimumPartitionHalfLength;

        public static HashSet<Body> AllBodies;

        static Partition()
        {
            Partition.MinimumPartitionHalfLength = FInt.Create(2);
            Partition.AllBodies = new HashSet<Body>();
        }

        private static void Establish(HashSet<Body> ContainedBodies)
        {
            if (ContainedBodies.Count >= 2)
            {
                ushort num = 0;
                foreach (Body containedBody in ContainedBodies)
                {
                    ushort num1 = 0;
                    foreach (Body body in ContainedBodies)
                    {
                        if (num1 > num)
                        {
                            if (containedBody.SimID >= body.SimID)
                            {
                                body.MyPairs[containedBody.SimID].SamePartition = true;
                            }
                            else
                            {
                                containedBody.MyPairs[body.SimID].SamePartition = true;
                            }
                        }
                        num1 = (ushort)(num1 + 1);
                    }
                    num = (ushort)(num + 1);
                }
            }
        }

        private static void GenerateBounds(HashSet<Body> ContainedBodies, out long xMin, out long xMax, out long yMin, out long yMax)
        {
            bool flag = true;
            xMin = (long)0;
            xMax = (long)0;
            yMin = (long)0;
            yMax = (long)0;
            foreach (Body containedBody in ContainedBodies)
            {
                if (flag)
                {
                    flag = false;
                    xMax = containedBody.dCollider.MyBounds.xMax;
                    xMin = containedBody.dCollider.MyBounds.xMin;
                    yMax = containedBody.dCollider.MyBounds.yMax;
                    yMin = containedBody.dCollider.MyBounds.yMin;
                }
                if (containedBody.dCollider.MyBounds.xMin < xMin)
                {
                    xMin = containedBody.dCollider.MyBounds.xMin;
                }
                else if (containedBody.dCollider.MyBounds.xMax > xMax)
                {
                    xMax = containedBody.dCollider.MyBounds.xMax;
                }
                if (containedBody.dCollider.MyBounds.yMin >= yMin)
                {
                    if (containedBody.dCollider.MyBounds.yMax <= yMax)
                    {
                        continue;
                    }
                    yMax = containedBody.dCollider.MyBounds.yMax;
                }
                else
                {
                    yMin = containedBody.dCollider.MyBounds.yMin;
                }
            }
            long num = yMax - yMin + (xMax - xMin);
            Partition.MaxDepth = (int)(num >> 25);
            Partition.MaxDepth = Partition.MaxDepth + 1;
            if (Partition.MaxDepth > 7)
            {
                Partition.MaxDepth = 7;
            }
        }

        private static void GetSplitPoint(long xMin, long xMax, long yMin, long yMax, out long xSplit, out long ySplit)
        {
            xSplit = (xMin + xMax) / (long)2;
            ySplit = (yMin + yMax) / (long)2;
        }

        public static void NewPartition(int depth, long xMin, long xMax, long yMin, long yMax, HashSet<Body> ContainedBodies)
        {
            long num;
            long num1;
            long num2;
            long num3;
            long num4;
            long num5;
            if (ContainedBodies.Count <= 16)
            {
                Partition.Establish(ContainedBodies);
                return;
            }
            if (depth >= Partition.MaxDepth)
            {
                Partition.Establish(ContainedBodies);
                return;
            }
            Partition.GetSplitPoint(xMin, xMax, yMin, yMax, out num, out num1);
            if (xMax - num <= Partition.MinimumPartitionHalfLength.RawValue && yMax - num1 <= Partition.MinimumPartitionHalfLength.RawValue)
            {
                Partition.Establish(ContainedBodies);
                return;
            }
            int num6 = depth + 1;
            for (int i = 0; i < 4; i++)
            {
                bool flag = (i == 0 ? true : i == 3);
                bool flag1 = (i == 0 ? true : i == 1);
                if (!flag)
                {
                    num5 = num1;
                    num4 = yMin;
                }
                else
                {
                    num5 = yMax;
                    num4 = num1;
                }
                if (!flag1)
                {
                    num3 = num;
                    num2 = xMin;
                }
                else
                {
                    num3 = xMax;
                    num2 = num;
                }
                HashSet<Body> bodies = new HashSet<Body>();
                foreach (Body containedBody in ContainedBodies)
                {
                    if (!containedBody.Active)
                    {
                        continue;
                    }
                    if (flag)
                    {
                        if (containedBody.dCollider.MyBounds.yMax < num1)
                        {
                            continue;
                        }
                    }
                    else if (containedBody.dCollider.MyBounds.yMin >= num1)
                    {
                        continue;
                    }
                    if (flag1)
                    {
                        if (containedBody.dCollider.MyBounds.xMax < num)
                        {
                            continue;
                        }
                    }
                    else if (containedBody.dCollider.MyBounds.xMin >= num)
                    {
                        continue;
                    }
                    bodies.Add(containedBody);
                }
                Partition.NewPartition(num6, num2, num3, num4, num5, bodies);
            }
        }

        public static void StartPartitioning()
        {
            long num;
            long num1;
            long num2;
            long num3;
            Partition.GenerateBounds(Partition.AllBodies, out num, out num1, out num2, out num3);
            Partition.NewPartition(0, num, num1, num2, num3, Partition.AllBodies);
        }
    }
}