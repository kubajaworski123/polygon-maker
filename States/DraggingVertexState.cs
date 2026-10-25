using GKProjekt1.Constraints;
using System;
using System.Collections.Generic;
using System.Text;

namespace GKProjekt1.States
{
    public class DraggingVertexState : DraggingState
    {
        private readonly Polygon polygon;
        private readonly Vertex vertex;
        public DraggingVertexState(MainWindow window, Scene scene, Polygon polygon, Vertex vertex, Point startMousePos)
            : base(window, scene, startMousePos)
        {
            this.polygon = polygon;
            this.vertex = vertex;               
        }
        protected override void OnDrag(float dx, float dy, Point currentMousePos)
        {
            if (!ConstraintSolver.TryMoveVertex(scene, vertex, currentMousePos))
            {
                polygon.Move(currentMousePos.X - vertex.Position.X, currentMousePos.Y - vertex.Position.Y);
            }
        }
    }
}
