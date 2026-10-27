using System;
using System.Collections.Generic;
using System.Text;

namespace GKProjekt1.Constraints
{
    public static class ContinuityEnforcer
    {
        private const float MinLength = 1e-3f;

        public static void EnforceAround(IEnumerable<Vertex> movedVertices)
        {
            var affected = new HashSet<Vertex>();
            foreach (var vertex in movedVertices)
            {
                affected.Add(vertex);
                if (vertex.PrevV != null)
                {
                    affected.Add(vertex.PrevV);
                }
                if (vertex.NextV != null)
                {
                    affected.Add(vertex.NextV);
                }
            }
            foreach (var vertex in affected)
            {
                Enforce(vertex);
            }
        }

        public static void Enforce(Vertex vertex)
        {
            ContinuityType continuity = vertex.GetEffectiveContinuity();
            if (continuity == ContinuityType.G0
                || vertex.InEdge == null 
                || vertex.OutEdge == null
                || vertex.HasAdjacentEdgeOfType(EdgeType.Arc))
            {
                return;
            }
            if (vertex.OutEdge.Type == EdgeType.Bezier)
            {
                PointF tangent = vertex.InEdge.GetTangentAtEnd();
                BezierControlPoint controlPoint = vertex.OutEdge.BezierPoint1;
                controlPoint.Position = AlignedPoint(vertex.Position, tangent, controlPoint.Position, continuity, BezierTangentScale);
            }
            else if (vertex.InEdge.Type == EdgeType.Bezier)
            {
                PointF tangent = GeometryUtils.Scale(vertex.OutEdge.GetTangentAtStart(), -1f);
                BezierControlPoint controlPoint = vertex.InEdge.BezierPoint2;
                controlPoint.Position = AlignedPoint(vertex.Position, tangent, controlPoint.Position, continuity, BezierTangentScale);
            }
        }
        public const float BezierTangentScale = 1f / 3f;
        public const float LineTangentScale = 1f;
        public static PointF AlignedPoint(PointF vertex, PointF tangent, PointF current, ContinuityType continuity, float c1Scale)
        {
            float tangentLength = GeometryUtils.Length(tangent);
            if (tangentLength < MinLength)
            {
                return current;
            }

            if (continuity == ContinuityType.C1)
            {
                return GeometryUtils.MovePoint(vertex, tangent.X * c1Scale, tangent.Y * c1Scale);
            }

            float distance = GeometryUtils.Distance(vertex, current);
            if (distance < MinLength)
            {
                distance = tangentLength * c1Scale;
            }
            return GeometryUtils.PointInDirection(vertex, tangent, distance);
        }
    }
}
