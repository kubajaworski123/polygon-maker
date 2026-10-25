using System;
using System.Collections.Generic;
using System.Text;

namespace GKProjekt1
{
    public class Vertex : Node
    {
        public ContinuityType Continuity { get; set; } = ContinuityType.G1;
        public Edge? InEdge { get; set; }
        public Edge? OutEdge { get; set; }
        public Vertex? NextV  => OutEdge?.End ?? null;
        public Vertex? PrevV => InEdge?.Start ?? null;
        public Vertex(float x, float y)
        {
            Position = new PointF(x, y);
        }

        public Vertex(PointF position)
        {
            Position = position;
        }
        public void MoveByVector(float dx, float dy)
        {
            Position = new PointF(Position.X + dx, Position.Y + dy);
        }
        public void MoveTo(PointF target)
        {
        float dx = target.X - Position.X;
        float dy = target.Y - Position.Y;
        Position = target;
        if (OutEdge?.Type == EdgeType.Bezier)
            OutEdge.BezierPoint1.Position = GeometryUtils.MovePoint(OutEdge.BezierPoint1.Position, dx, dy);
        if (InEdge?.Type == EdgeType.Bezier)
            InEdge.BezierPoint2.Position = GeometryUtils.MovePoint(InEdge.BezierPoint2.Position, dx, dy);
        }
    }
}
