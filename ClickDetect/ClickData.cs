using System;
using System.Collections.Generic;
using System.Text;

namespace GKProjekt1.ClickDetect
{
    public class ClickData
    {
        public ClickTarget ClickTarget { get; set; } = ClickTarget.None;
        public BezierControlPoint? ControlPoint { get; set; }
        public Polygon? Polygon { get; set; }
        public Vertex? Vertex { get; set; }
        public Edge? Edge { get; set; }
    }
}
