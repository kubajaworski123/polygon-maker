using System;
using System.Collections.Generic;
using System.Text;

namespace GKProjekt1.ClickDetect
{
    public class PolygonClickHandler : ClickHandler
    {
        public override ClickData? CheckClick(Scene scene, PointF mousePos)
        {
            float distance = float.MaxValue;
            var result = new ClickData();
            foreach (var polygon in scene.Polygons)
            {
                foreach (var edge in polygon.Edges)
                {
                    var distToEdge = GeometryUtils.Distance(mousePos, edge.Start.Position, edge.End.Position);
                    if (distToEdge < distance)
                    {
                        distance = GeometryUtils.Distance(mousePos, edge.Start.Position, edge.End.Position);
                        result.ClickTarget = ClickTarget.Polygon;
                        result.Polygon = polygon;
                    }
                }
            }
            return result;
        }
    }
}
