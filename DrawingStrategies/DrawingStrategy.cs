using System;
using System.Collections.Generic;
using System.Text;

namespace GKProjekt1.DrawingStrategies
{
    public abstract class DrawingStrategy
    {
        public abstract void DrawLine(Graphics g, Bitmap buffer, Point p1, Point p2, Color color);
        public abstract void DrawBezier(Graphics g, Bitmap buffer, PointF p0, PointF p1, PointF p2, PointF p3, Color color);
        public virtual void DrawArc(Graphics g, Bitmap bufferBitmap, PointF p1, PointF p2, Color color)
        {
            using var pen = new Pen(color, 1f);
            float d = GeometryUtils.Distance(p1, p2);
            float r = d / 2.0f;
            PointF center = GeometryUtils.MidPoint(p1, p2);
            float rectX = center.X - r;
            float rectY = center.Y - r;
            float startAngle = (float)(Math.Atan2(p1.Y - center.Y, p1.X - center.X) * (180.0 / Math.PI));
            float sweepAngle = 180.0f;

            g.DrawArc(pen, rectX, rectY, d, d, startAngle, sweepAngle);
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
