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
    public Color ConstraintLabelColor { get; set; } = Color.DarkBlue;
    public float ConstraintLabelOffset { get; set; } = 14f;
    public float ConstraintLabelSpacing { get; set; } = 16f;
    public Color ContinuityLabelColor { get; set; } = Color.DarkGreen;
    public float ContinuityLabelOffset { get; set; } = 16f;
    private readonly Font labelFont = new Font("Segoe UI", 9f);

    public void Render(Graphics g, Bitmap bufferBitmap, Scene scene, DrawingStrategy strategy)
    {
        DrawEdges(g, bufferBitmap, scene, strategy);
        DrawVertices(scene, g, strategy);
        DrawConstraintLabels(g, scene);
        DrawContinuityLabels(g, scene);
    }
    private void DrawContinuityLabels(Graphics g, Scene scene)
    {
        using var brush = new SolidBrush(ContinuityLabelColor);
        foreach (var polygon in scene.Polygons)
        {
            foreach (var vertex in polygon.Vertices)
            {
                if (vertex.PrevV == null || vertex.NextV == null
                    || !(vertex.HasAdjacentEdgeOfType(EdgeType.Bezier) || vertex.HasAdjacentEdgeOfType(EdgeType.Arc))
                    || vertex.GetEffectiveContinuity() == ContinuityType.G0)
                {
                    continue;
                }
                PointF toPrev = GeometryUtils.Normalize(GeometryUtils.DirectionVector(vertex.Position, vertex.PrevV.Position));
                PointF toNext = GeometryUtils.Normalize(GeometryUtils.DirectionVector(vertex.Position, vertex.NextV.Position));
                PointF bisector = new PointF(toPrev.X + toNext.X, toPrev.Y + toNext.Y);
                PointF outward = GeometryUtils.Length(bisector) < 1e-3f
                    ? GeometryUtils.PerpendicularVector(toNext)
                    : GeometryUtils.Scale(bisector, -1f);

                string label = vertex.GetEffectiveContinuity().ToString();
                SizeF size = g.MeasureString(label, labelFont);
                PointF center = GeometryUtils.PointInDirection(vertex.Position, outward, ContinuityLabelOffset);
                g.DrawString(label, labelFont, brush, center.X - size.Width / 2f, center.Y - size.Height / 2f);
            }
        }
    }
    private void DrawConstraintLabels(Graphics g, Scene scene)
    {
        using var brush = new SolidBrush(ConstraintLabelColor);
        foreach (var edge in scene.AllEdges)
        {
            if (edge.Constraints.Count == 0)
            {
                continue;
            }
            PointF middle = edge.GetMiddle();
            PointF normal = GeometryUtils.PerpendicularVector(edge.GetDirection());
            for (int i = 0; i < edge.Constraints.Count; i++)
            {
                string label = edge.Constraints[i].Label;
                SizeF size = g.MeasureString(label, labelFont);
                PointF center = GeometryUtils.PointInDirection(middle, normal, ConstraintLabelOffset + ConstraintLabelSpacing * i);
                g.DrawString(label, labelFont, brush, center.X - size.Width / 2f, center.Y - size.Height / 2f);
            }
        }
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
                    strategy.DrawLine(g, bufferBitmap, p1, p2, EdgeColor, edge.Width);
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
                    strategy.DrawBezier(g, bufferBitmap, edge.Start.Position, edge.BezierPoint1.Position, edge.BezierPoint2.Position, edge.End.Position, EdgeColor, edge.Width);
                }
                else if (edge.Type == EdgeType.Arc)
                {
                    using (var dashPen = new Pen(ControlPolygonColor, 1f) { DashStyle = DashStyle.Dash })
                    {
                        g.DrawLine(dashPen, edge.Start.Position, edge.End.Position);
                    }
                    ArcGeometry? arc = edge.GetArcGeometry();
                    if (arc.HasValue)
                    {
                        strategy.DrawArc(g, bufferBitmap, arc.Value, EdgeColor, edge.Width);
                    }
                    else
                    {
                        strategy.DrawLine(g, bufferBitmap, Point.Round(edge.Start.Position), Point.Round(edge.End.Position), EdgeColor, edge.Width);
                    }
                }
            }
        }
    }
}
