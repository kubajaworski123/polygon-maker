using GKProjekt1.Constraints;
using System;
using System.Collections.Generic;
using System.Text;

namespace GKProjekt1.States
{
    public class DraggingControlPointState : DraggingState
    {
        private readonly Polygon polygon;
        private readonly BezierControlPoint controlPoint;
        public DraggingControlPointState(MainWindow window, Scene scene, Polygon polygon, BezierControlPoint controlPoint, Point startMousePos)
        : base(window, scene, startMousePos)
        {
            this.polygon = polygon;
            this.controlPoint = controlPoint;
        }
        protected override void OnDrag(float dx, float dy, Point currentMousePos)
        {
            if (!ConstraintSolver.TryMoveControlPoint(scene, controlPoint, currentMousePos))
            {
                polygon.Move(currentMousePos.X - controlPoint.Position.X, currentMousePos.Y - controlPoint.Position.Y);
            }
        }
    }
}
