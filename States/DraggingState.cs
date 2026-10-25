using System;
using System.Collections.Generic;
using System.Text;

namespace GKProjekt1.States
{
    public abstract class DraggingState : IActionState
    {
        protected readonly MainWindow window;
        protected readonly Scene scene; 
        private PointF lastMousePos;
        protected DraggingState(MainWindow window, Scene scene, Point startMousePos)
        {
            this.window = window;
            this.scene = scene;
            this.lastMousePos = startMousePos;
        }
        public void OnMouseDown(MouseEventArgs e) { }
        public void OnMouseMove(MouseEventArgs e)
        {
            float dx = e.Location.X - lastMousePos.X;
            float dy = e.Location.Y - lastMousePos.Y;
            lastMousePos = e.Location;
            OnDrag(dx, dy, e.Location);
            window.Invalidate();
        }
        protected abstract void OnDrag(float dx, float dy, Point currentMousePos);
        public void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                window.TransitionTo(new IdleState(window, scene));
            }
        }
    }
}
