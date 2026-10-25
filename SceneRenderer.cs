using GKProjekt1.DrawingStrategies;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Text;

namespace GKProjekt1;

public class SceneRenderer
{
    public float VertexRadius { get; set; } = 6.0f;
    public float ControlPointRadius { get; set; } = 4.5f;
    public Color VertexFillColor { get; set; } = Color.White;
    public Color VertexBorderColor { get; set; } = Color.Black;
    public Color EdgeColor { get; set; } = Color.Black;
    public Color ControlPolygonColor { get; set; } = Color.Gray;

    public void Render(Graphics g, Bitmap bufferBitmap, Scene scene, DrawingStrategy strategy)
    {
        //g.SmoothingMode = SmoothingMode.AntiAlias;
        DrawEdges(g, bufferBitmap, scene, strategy);
        DrawVertices(scene, g, strategy);
    }
    private void DrawVertices(Scene scene, Graphics g, DrawingStrategy strategy)
    {
        foreach (var polygon in scene.Polygons)
        {
            foreach (var vertex in polygon.Vertices)
            {
                strategy.DrawCircle(g, vertex.Position, VertexRadius, VertexFillColor, VertexBorderColor);
            }
        }
    }
    private void DrawEdges(Graphics g, Bitmap bufferBitmap, Scene scene, DrawingStrategy strategy)
    {
        foreach (var polygon in scene.Polygons)
        {
            foreach (var edge in polygon.Edges)
            {
                if (edge.Type == EdgeType.Line)
                {
                    Point p1 = Point.Round(edge.Start.Position);
                    Point p2 = Point.Round(edge.End.Position);
                    strategy.DrawLine(g, bufferBitmap, p1, p2, EdgeColor);
                }
                else if (edge.Type == EdgeType.Bezier)
                {
                    using (var dashPen = new Pen(ControlPolygonColor, 1f) { DashStyle = DashStyle.Dash })
                    {
                        g.DrawLine(dashPen, edge.Start.Position, edge.BezierPoint1.Position);
                        g.DrawLine(dashPen, edge.BezierPoint1.Position, edge.BezierPoint2.Position);
                        g.DrawLine(dashPen, edge.BezierPoint2.Position, edge.End.Position);
                        g.DrawLine(dashPen, edge.Start.Position, edge.End.Position);
                    }
                    strategy.DrawCircle(g, edge.BezierPoint1.Position, ControlPointRadius, VertexFillColor, ControlPolygonColor);
                    strategy.DrawCircle(g, edge.BezierPoint2.Position, ControlPointRadius, VertexFillColor, ControlPolygonColor);
                    strategy.DrawBezier(g, bufferBitmap, edge.Start.Position, edge.BezierPoint1.Position, edge.BezierPoint2.Position, edge.End.Position, EdgeColor);
                }
                else if (edge.Type == EdgeType.Arc)
                {
                    using (var dashPen = new Pen(ControlPolygonColor, 1f) { DashStyle = DashStyle.Dash })
                    {
                        g.DrawLine(dashPen, edge.Start.Position, edge.End.Position);
                    }
                    strategy.DrawArc(g, bufferBitmap, edge.Start.Position, edge.End.Position, EdgeColor);
                }
            }
        }
    }
}
