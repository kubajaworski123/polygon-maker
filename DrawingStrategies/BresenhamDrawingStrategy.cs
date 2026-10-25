using System;
using System.Collections.Generic;
using System.Text;

namespace GKProjekt1.DrawingStrategies
{
    internal class BresenhamDrawingStrategy : DrawingStrategy
    {
        public override void DrawLine(Graphics g, Bitmap buffer, Point p1, Point p2, Color color)
        {
            if (buffer == null) return;
            int x1 = p1.X, y1 = p1.Y;
            int x2 = p2.X, y2 = p2.Y;
            int dx = Math.Abs(x2 - x1);
            int dy = Math.Abs(y2 - y1);
            int stepX = x1 < x2 ? 1 : -1;
            int stepY = y1 < y2 ? 1 : -1;
            int x = x1;
            int y = y1;
            if (dx >= dy)
            {
                int d = 2 * dy - dx;
                int incrE = 2 * dy;
                int incrNE = 2 * (dy - dx);
                while (x != x2)
                {
                    if (d < 0)
                    {
                        d += incrE;
                        x += stepX;
                    }
                    else
                    {
                        d += incrNE;
                        x += stepX;
                        y += stepY;
                    }
                    PutPixel(buffer, x, y, color);
                }
            }
            else
            {
                int d = 2 * dx - dy;
                int incrE = 2 * dx;
                int incrNE = 2 * (dx - dy);
                while (y != y2)
                {
                    if (d < 0)
                    {
                        d += incrE;
                        y += stepY;
                    }
                    else
                    {
                        d += incrNE;
                        y += stepY;
                        x += stepX;
                    }
                    PutPixel(buffer, x, y, color);
                }
            }
        }
        private static void PutPixel(Bitmap buffer, int x, int y, Color color)
        {
            if (x >= 0 && x < buffer.Width && y >= 0 && y < buffer.Height)
            {
                buffer.SetPixel(x, y, color);
            }
        }

        public override void DrawBezier(Graphics g, Bitmap buffer, PointF p0, PointF p1, PointF p2, PointF p3, Color color)
        {
            List<Point> points = ComputeBezierPoints(p0, p1, p2, p3);
            for (int i = 1; i < points.Count; i++)
            {
                DrawLine(g, buffer, points[i - 1], points[i], color);
            }
        }

        private static List<Point> ComputeBezierPoints(PointF v0, PointF v1, PointF v2, PointF v3)
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
}
