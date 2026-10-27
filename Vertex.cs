using GKProjekt1.Constraints;
using System;
using System.Collections.Generic;
using System.Text;

namespace GKProjekt1
{
    public class Vertex : Node
    {
        public ContinuityType? ExplicitContinuity { get; set; }
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
        public void SetContinuity(ContinuityType continuity)
        {
            ExplicitContinuity = continuity;
            ContinuityEnforcer.Enforce(this);
        }
        public bool HasAdjacentEdgeOfType(EdgeType type) => InEdge?.Type == type || OutEdge?.Type == type;
        public ContinuityType GetEffectiveContinuity()
        {
            if (ExplicitContinuity.HasValue)
            {
                return ExplicitContinuity.Value;
            }
            if (!HasAdjacentEdgeOfType(EdgeType.Bezier))
            {
                return ContinuityType.G0;
            }
            if (InEdge?.Type == EdgeType.Arc && InEdge.Start.GetEffectiveContinuity() != ContinuityType.G0)
            {
                return ContinuityType.G0;
            }
            if (OutEdge?.Type == EdgeType.Arc && OutEdge.End.ExplicitContinuity is ContinuityType explicitEnd && explicitEnd != ContinuityType.G0)
            {
                return ContinuityType.G0;
            }
            return ContinuityType.G1;
        }
        public bool CanHaveG1() =>
            !(InEdge?.Type == EdgeType.Arc && InEdge.Start.GetEffectiveContinuity() != ContinuityType.G0)
            && !(OutEdge?.Type == EdgeType.Arc && OutEdge.End.GetEffectiveContinuity() != ContinuityType.G0);
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
