using System;
using System.Collections.Generic;
using System.Text;

namespace GKProjekt1.DrawingStrategies;

public abstract class DrawingStrategy
{
    public abstract void DrawLine(Graphics g, Bitmap buffer, Point p1, Point p2, Color color, int width);
    public abstract void DrawBezier(Graphics g, Bitmap buffer, PointF p0, PointF p1, PointF p2, PointF p3, Color color, int width);
    public virtual void DrawArc(Graphics g, Bitmap bufferBitmap, ArcGeometry arc, Color color, int width)
    {
        if (arc.Radius < 0.5f)
        {
            return;
        }
        using var pen = new Pen(color, Math.Max(1, width));
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
    public static List<Point> ComputeBezierPoints(PointF v0, PointF v1, PointF v2, PointF v3)
    {
        double a0x = v0.X;
        double a0y = v0.Y;
        double a1x = 3.0 * (v1.X - v0.X);
        double a1y = 3.0 * (v1.Y - v0.Y);
        double a2x = 3.0 * (v2.X - 2.0 * v1.X + v0.X);
        double a2y = 3.0 * (v2.Y - 2.0 * v1.Y + v0.Y);
        double a3x = v3.X - 3.0 * v2.X + 3.0 * v1.X - v0.X;
        double a3y = v3.Y - 3.0 * v2.Y + 3.0 * v1.Y - v0.Y;

        float controlPolygonLength = GeometryUtils.Distance(v0, v1)
                                   + GeometryUtils.Distance(v1, v2)
                                   + GeometryUtils.Distance(v2, v3);
        int n = Math.Clamp((int)(controlPolygonLength / 4f), 1, 1000);

        double d = 1.0 / n;
        double d2 = d * d;
        double d3 = d2 * d;

        double px = a0x;
        double py = a0y;
        double deltaX = a3x * d3 + a2x * d2 + a1x * d;
        double deltaY = a3y * d3 + a2y * d2 + a1y * d;
        double delta2X = 6.0 * a3x * d3 + 2.0 * a2x * d2;
        double delta2Y = 6.0 * a3y * d3 + 2.0 * a2y * d2;
        double delta3X = 6.0 * a3x * d3;
        double delta3Y = 6.0 * a3y * d3;
        var points = new List<Point>(n + 1) { Point.Round(v0) };

        for (int i = 1; i < n; i++)
        {
            px += deltaX;
            py += deltaY;
            deltaX += delta2X;
            deltaY += delta2Y;
            delta2X += delta3X;
            delta2Y += delta3Y;
            points.Add(new Point((int)Math.Round(px), (int)Math.Round(py)));
        }
        points.Add(Point.Round(v3));
        return points;
    }
}
