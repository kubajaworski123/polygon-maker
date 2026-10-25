using System;
using System.Collections.Generic;
using System.Text;

namespace GKProjekt1.ClickDetect;

public class EdgeClickHandler : ClickHandler
{
    private const float clickTolerance = 7.0f;
    public override ClickData? CheckClick(Scene scene, PointF mousePos)
    {
        var result = new ClickData();
        foreach (var polygon in scene.Polygons)
        {
            foreach (var edge in polygon.Edges)
            {
                if (IsPointNearEdge(mousePos, edge, clickTolerance))
                {
                    result.ClickTarget = ClickTarget.Edge;
                    result.Polygon = polygon;
                    result.Edge = edge;
                    return result;
                }
            }
        }
        return null;
    }
    private static bool IsPointNearEdge(PointF point, Edge edge, float tolerance)
    {
        return GeometryUtils.Distance(point, edge.Start.Position, edge.End.Position) <= tolerance;
    }
}