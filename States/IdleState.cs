using GKProjekt1.ClickDetect;
using System;
using System.Collections.Generic;
using System.Text;

namespace GKProjekt1.States
{
    public class IdleState : IActionState
    {
        private readonly MainWindow window;
        private readonly Scene scene;
        public IdleState(MainWindow window, Scene scene)
        {
            this.window = window;
            this.scene = scene;
        }
        public void OnMouseDown(MouseEventArgs e)
        {
            var hit = ClickDetectUtils.Test(scene, e.Location);
            if (e.Button == MouseButtons.Left)
            {
                switch (hit.ClickTarget)
                {
                    case ClickTarget.Vertex when hit.Vertex != null && hit.Polygon != null:
                        window.TransitionTo(new DraggingVertexState(window, scene, hit.Polygon, hit.Vertex, e.Location));
                        break;
                    case ClickTarget.BezierControlPoint when hit.ControlPoint != null:
                        window.TransitionTo(new DraggingControlPointState(window, scene, hit.ControlPoint, e.Location));
                        break;
                    case ClickTarget.Polygon when hit.Polygon != null:
                        window.TransitionTo(new DraggingPolygonState(window, scene, hit.Polygon, e.Location));
                        break;
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (hit.ClickTarget == ClickTarget.Edge && hit.Edge != null && hit.Polygon != null)
                {
                    window.ShowEdgeContextMenu(e.Location, hit.Edge, hit.Polygon);
                }
                else if (hit.ClickTarget == ClickTarget.Vertex && hit.Vertex != null && hit.Polygon != null)
                {
                    window.ShowVertexContextMenu(e.Location, hit.Vertex, hit.Polygon);
                }
            }
        }
        public void OnMouseMove(MouseEventArgs e) { }
        public void OnMouseUp(MouseEventArgs e) { }
    }
}
