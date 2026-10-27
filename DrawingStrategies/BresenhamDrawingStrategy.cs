using System;
using System.Collections.Generic;
using System.Text;

namespace GKProjekt1.DrawingStrategies
{
    internal class BresenhamDrawingStrategy : DrawingStrategy
    {
        public override void DrawLine(Graphics g, Bitmap buffer, Point p1, Point p2, Color color, int width)
        {
            if (buffer == null) return;
            width = Math.Max(1, width);
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
                    PutPixel(buffer, x, y, width, true, color);
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
                    PutPixel(buffer, x, y, width, false, color);
                }
            }
        }
        private static void PutPixel(Bitmap buffer, int x, int y, int width, bool vertical, Color color)
        {
            int start = -(width - 1) / 2;
            for (int i = 0; i < width; i++)
            {
                int offset = start + i;
                if (vertical)
                {
                    PutPixel(buffer, x, y + offset, color);
                }
                else
                {
                    PutPixel(buffer, x + offset, y, color);
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

        public override void DrawBezier(Graphics g, Bitmap buffer, PointF p0, PointF p1, PointF p2, PointF p3, Color color, int width)
        {
            List<Point> points = ComputeBezierPoints(p0, p1, p2, p3);
            var LibLineDrawer = new LibraryDrawingStrategy();
            for (int i = 1; i < points.Count; i++)
            {
                LibLineDrawer.DrawLine(g, buffer, points[i - 1], points[i], color, width);
            }
        }

    }
}
