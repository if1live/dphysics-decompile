using System;
using UnityEngine;

namespace DPhysics
{
    public class DCollider : MonoBehaviour
    {
        [HideInInspector]
        public Vector2d center;

        private Vector2d rotation;

        public Vector2[] Vertices = new Vector2[0];

        [HideInInspector]
        public Vector2d[] Points;

        [HideInInspector]
        public Vector2d[] points;

        [HideInInspector]
        public Vector2d[] backupPoints;

        private Vector2d[] edges;

        public bool IsCircle;

        public double Radius;

        public FInt radius;

        public Bounder MyBounds;

        public Vector2d Center
        {
            get
            {
                return this.center;
            }
            set
            {
                Vector2d vector2d;
                if (this.center.x.RawValue != value.x.RawValue || this.center.y.RawValue != value.y.RawValue)
                {
                    value.Subtract(ref this.center, out vector2d);
                    this.Offset(ref vector2d);
                }
            }
        }

        public Vector2d[] Edges
        {
            get
            {
                return this.edges;
            }
        }

        public Vector2d Rotation
        {
            get
            {
                return this.rotation;
            }
            set
            {
                if (this.IsCircle)
                {
                    return;
                }
                this.rotation = value;
                for (int i = 0; i < (int)this.Points.Length; i++)
                {
                    this.backupPoints[i].Rotate(this.rotation.x.RawValue, this.rotation.y.RawValue, out this.Points[i]);
                }
                this.BuildBounds();
                this.BuildPoints();
                this.BuildEdges();
            }
        }

        public DCollider()
        {
        }

        public void BuildBounds()
        {
            if (this.MyBounds == null)
            {
                this.MyBounds = new Bounder(this);
                this.MyBounds.Offset(ref this.center);
                return;
            }
            this.MyBounds.BuildBounds(false);
            this.MyBounds.Offset(ref this.center);
        }

        public void BuildEdges()
        {
            Vector2d vector2d;
            for (int i = 0; i < (int)this.edges.Length; i++)
            {
                Vector2d points = this.Points[i];
                vector2d = (i + 1 < (int)this.Points.Length || (int)this.edges.Length < 3 ? this.Points[i + 1] : this.Points[0]);
                vector2d.Subtract(ref points, out points);
                this.edges[i] = points;
            }
        }

        public void BuildPoints()
        {
            if (this.IsCircle)
            {
                return;
            }
            for (int i = 0; i < (int)this.backupPoints.Length; i++)
            {
                this.Points[i].Add(ref this.center, out this.points[i]);
            }
        }

        public void Initialize(Body body)
        {
            if (!this.IsCircle)
            {
                this.backupPoints = new Vector2d[(int)this.Vertices.Length];
                this.Points = new Vector2d[(int)this.backupPoints.Length];
                this.points = new Vector2d[(int)this.backupPoints.Length];
                for (int i = 0; i < (int)this.backupPoints.Length; i++)
                {
                    Vector2 vertices = this.Vertices[i];
                    this.backupPoints[i] = new Vector2d(FInt.Create(vertices.x), FInt.Create(vertices.y));
                    this.Points[i] = this.backupPoints[i];
                    this.points[i] = this.backupPoints[i];
                }
                this.Vertices = null;
                this.edges = new Vector2d[(int)this.Points.Length];
                this.BuildEdges();
            }
            else
            {
                this.radius = FInt.Create(this.Radius);
                this.radius.AbsoluteValue(out this.radius);
            }
            this.BuildBounds();
        }

        public void Offset(ref Vector2d change)
        {
            this.center.Add(ref change, out this.center);
            this.MyBounds.Offset(ref change);
            if (this.IsCircle)
            {
                return;
            }
            for (int i = 0; i < (int)this.backupPoints.Length; i++)
            {
                this.points[i].Add(ref change, out this.points[i]);
            }
        }

        private void OnDrawGizmos()
        {
            if (this.IsCircle)
            {
                Gizmos.DrawWireSphere(new Vector3(this.center.x.ToFloat(), 0f, this.center.y.ToFloat()), (float)this.Radius);
                return;
            }
            if (this.points != null)
            {
                for (int i = 0; i < (int)this.points.Length; i++)
                {
                    Vector3 vector3 = (Vector3)this.points[i];
                    Vector3 vector31 = (i + 1 < (int)this.points.Length ? (Vector3)this.points[i + 1] : (Vector3)this.points[0]);
                    Gizmos.DrawSphere(vector3, 0.5f);
                    Gizmos.DrawLine(vector3, vector31);
                }
            }
        }

        public override string ToString()
        {
            string str = "";
            for (int i = 0; i < (int)this.points.Length; i++)
            {
                str = string.Concat(str, this.points[i].ToString());
                if (i != (int)this.points.Length - 1)
                {
                    str = string.Concat(str, ", ");
                }
            }
            return str;
        }
    }
}