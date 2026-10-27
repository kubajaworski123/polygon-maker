using System;
using System.Collections.Generic;
using System.Text;

namespace GKProjekt1.DrawingStrategies
{
    public class XiaolinWuDrawingStrategy : DrawingStrategy
    {
        public override void DrawLine(Graphics g, Bitmap buffer, Point p1, Point p2, Color color, int width)
        {
            if (buffer == null)
            {
                return;
            }

            width = Math.Max(1, width);

            int x0 = p1.X;
            int y0 = p1.Y;
            int x1 = p2.X;
            int y1 = p2.Y;
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (steep)
            {
                (x0, y0) = (y0, x0);
                (x1, y1) = (y1, x1);
            }
            if (x0 > x1)
            {
                (x0, x1) = (x1, x0);
                (y0, y1) = (y1, y0);
            }

            float dx = x1 - x0;
            float m = dx == 0f ? 0f : (y1 - y0) / dx;
            float y = y0;
            for (int x = x0; x <= x1; x++)
            {
                int flor_y = (int)Math.Floor(y);
                float frac = y - flor_y;
                if (width == 1)
                {
                    Plot(buffer, x, flor_y, 1f - frac, steep, color);
                    Plot(buffer, x, flor_y + 1, frac, steep, color);
                }
                else
                {
                    int start = flor_y - (width - 1) / 2;
                    for (int i = 0; i < width; i++)
                    {
                        Plot(buffer, x, start + i, 1f, steep, color);
                    }
                    if (frac > 0f)
                    {
                        Plot(buffer, x, start - 1, 1f - frac, steep, color);
                    }
                    Plot(buffer, x, start + width, frac, steep, color);
                }
                y += m;
            }
        }

        public override void DrawBezier(Graphics g, Bitmap buffer, PointF p0, PointF p1, PointF p2, PointF p3, Color color, int width)
        {
            List<Point> points = ComputeBezierPoints(p0, p1, p2, p3);
            for (int i = 1; i < points.Count; i++)
            {
                DrawLine(g, buffer, points[i - 1], points[i], color, width);
            }
        }

        private static void Plot(Bitmap buffer, int x, int y, float coverage, bool steep, Color color)
        {
            if (coverage <= 0f)
            {
                return;
            }
            if (steep)
            {
                (x, y) = (y, x);
            }
            if (x < 0 || y < 0 || x >= buffer.Width || y >= buffer.Height)
            {
                return;
            }

            Color existing = buffer.GetPixel(x, y);
            float a = Math.Min(coverage, 1f);
            int r = (int)(existing.R * (1f - a) + color.R * a);
            int g = (int)(existing.G * (1f - a) + color.G * a);
            int b = (int)(existing.B * (1f - a) + color.B * a);
            buffer.SetPixel(x, y, Color.FromArgb(r, g, b));
        }
    }
}
