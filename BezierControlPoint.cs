using System;
using System.Collections.Generic;
using System.Text;

namespace GKProjekt1
{
    public class BezierControlPoint : Node
    {
        public Edge Curve { get; }
        private bool IsFirst => this == Curve.BezierPoint1;
        public Vertex Vertex => IsFirst ? Curve.Start : Curve.End;
        public Edge? NeighborEdge => IsFirst ? Vertex.InEdge : Vertex.OutEdge;
        public PointF Tangent => IsFirst ? Curve.GetTangentAtStart() : Curve.GetTangentAtEnd();
        public BezierControlPoint(Edge curve, PointF position)
        {
            Curve = curve;
            Position = position;
        }
    }
}
