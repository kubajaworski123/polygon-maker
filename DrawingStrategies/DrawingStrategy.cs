using System;
using System.Collections.Generic;
using System.Text;

namespace GKProjekt1.DrawingStrategies
{
    public abstract class DrawingStrategy
    {
        public abstract void DrawLine(Graphics g, Bitmap buffer, Point p1, Point p2, Color color);
        public abstract void DrawBezier(Graphics g, Bitmap buffer, PointF p0, PointF p1, PointF p2, PointF p3, Color color);
        public virtual void DrawArc(Graphics g, Bitmap bufferBitmap, ArcGeometry arc, Color color)
        {
            if (arc.Radius < 0.5f)
            {
                return;
            }
            using var pen = new Pen(color, 1f);
            float diameter = 2f * arc.Radius;
            float startAngle = arc.StartAngle * 180f / MathF.PI;
            float sweepAngle = arc.SweepAngle * 180f / MathF.PI;

            g.DrawArc(pen, arc.Center.X - arc.Radius, arc.Center.Y - arc.Radius, diameter, diameter, startAngle, sweepAngle);
        }
        public virtual void DrawCircle(Graphics g, PointF center, float radius, Color fillColor, Color borderColor)
        {
            float diameter = radius * 2f;
            float x = center.X - radius;
            float y = center.Y - radius;

            using (var brush = new SolidBrush(fillColor))
            using (var pen = new Pen(borderColor, 1f))
            {
                g.FillEllipse(brush, x, y, diameter, diameter);
                g.DrawEllipse(pen, x, y, diameter, diameter);
            }
        }
    }
}
