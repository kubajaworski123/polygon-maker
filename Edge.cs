using GKProjekt1.Constraints;
using System;
using System.Collections.Generic;
using System.Text;

namespace GKProjekt1
{
    public class Edge
    {
        public const float BezierControlPointOffset = 0.25f;
        public Vertex Start { get;}
        public Vertex End { get;}
        public EdgeType Type { get; set; } = EdgeType.Line;
        public BezierControlPoint BezierPoint1 { get; }
        public BezierControlPoint BezierPoint2 { get; }
        public List<EdgeConstraint> Constraints { get; } = new();
        public Edge(Vertex start, Vertex end)
        {
            Start = start;
            End = end;
            BezierPoint1 = new BezierControlPoint(this, PointF.Empty);
            BezierPoint2 = new BezierControlPoint(this, PointF.Empty);
            ResetControlPoints();
        }
        public void SetToLine() => Type = EdgeType.Line;
        public void SetToArc() => Type = EdgeType.Arc;
        public void SetToBezier()
        {
            Type = EdgeType.Bezier;
            ResetControlPoints();
        }
        public void ResetControlPoints()
        {
            if (Start == null || End == null)
            {
                return;
            }
            PointF v = GeometryUtils.DirectionVector(Start.Position, End.Position);
            PointF n = GeometryUtils.PerpendicularVector(v);
            float nx = n.X * BezierControlPointOffset;
            float ny = n.Y * BezierControlPointOffset;
            BezierPoint1.Position = new PointF(Start.Position.X + v.X / 3.0f + nx, Start.Position.Y + v.Y / 3.0f + ny);
            BezierPoint2.Position = new PointF(Start.Position.X + 2.0f * v.X / 3.0f + nx, Start.Position.Y + 2.0f * v.Y / 3.0f + ny);
        }
        public float GetLength()
        {
            return GeometryUtils.Distance(Start.Position, End.Position);
        }
        public PointF GetMiddle()
        {
            return GeometryUtils.MidPoint(Start.Position, End.Position);
        }
        public PointF GetDirection() => GeometryUtils.DirectionVector(Start.Position, End.Position);
        public PointF GetUnitDirection()
        {
            PointF d = GetDirection();
            float len = GetLength();
            return len < 1e-6f ? PointF.Empty : new PointF(d.X / len, d.Y / len);
        }
        public Vertex Other(Vertex v) => v == Start ? End : Start;
        public PointF? GetRequiredDirection() =>
            Constraints.Select(c => c.RequiredDirection(this)).FirstOrDefault(d => d.HasValue);
        public float? GetRequiredLength() =>
            Constraints.Select(c => c.RequiredLength(this)).FirstOrDefault(l => l.HasValue);
    }
}
