using System;
using System.Collections.Generic;
using System.Text;

namespace GKProjekt1.DrawingStrategies
{
    public class LibraryDrawingStrategy : DrawingStrategy
    {
        public override void DrawLine(Graphics g, Bitmap bufferBitmap, Point p1, Point p2, Color color)
        {
            using (var pen = new Pen(color, 1f))
            {
                g.DrawLine(pen, p1, p2);
            }
        }
        public override void DrawBezier(Graphics g, Bitmap bufferBitmap, PointF p0, PointF p1, PointF p2, PointF p3, Color color)
        {
            using (var pen = new Pen(color, 1f))
            {
                g.DrawBezier(pen, p0, p1, p2, p3);
            }
        }
    }
}
