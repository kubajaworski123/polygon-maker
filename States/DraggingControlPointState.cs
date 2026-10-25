using System;
using System.Collections.Generic;
using System.Text;

namespace GKProjekt1.States
{
    public class DraggingControlPointState : DraggingState
    {
        private readonly BezierControlPoint controlPoint;
        public DraggingControlPointState(MainWindow window, Scene scene, BezierControlPoint controlPoint, Point startMousePos)
        : base(window, scene, startMousePos)
        {
            this.controlPoint = controlPoint;
        }
        protected override void OnDrag(float dx, float dy, Point currentMousePos)
        {
            controlPoint.Position = currentMousePos;
        }
    }
}
