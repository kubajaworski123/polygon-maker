using System;
using System.Collections.Generic;
using System.Text;

namespace GKProjekt1
{
    public class BezierControlPoint : Node
    {
        public Edge Curve { get; }
        public BezierControlPoint(Edge curve, PointF position)
        {
            Curve = curve;
            Position = position;
        }
    }
}
