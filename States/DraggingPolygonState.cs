using System;
using System.Collections.Generic;
using System.Text;

namespace GKProjekt1.States
{
    public class DraggingPolygonState : DraggingState
    {
        private readonly Polygon polygon;
        public DraggingPolygonState(MainWindow window, Scene scene, Polygon polygon, Point startMousePos)
            : base(window, scene, startMousePos)
        {
            this.polygon = polygon;
        }
        protected override void OnDrag(float dx, float dy, Point currentMousePos)
        {
            polygon.Move(dx, dy);
        }
    }
}
