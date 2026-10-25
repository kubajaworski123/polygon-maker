using System;
using System.Collections.Generic;
using System.Text;

namespace GKProjekt1.ClickDetect
{
    internal class BezierCtrlPtClickHandler : ClickHandler
    {
        private const float clickTolerance = 8f;
        public override ClickData? CheckClick(Scene scene, PointF mousePos)
        {
            var result = new ClickData();
            foreach (var polygon in scene.Polygons)
            {
                foreach (var edge in polygon.Edges)
                {
                    if (edge.Type == EdgeType.Bezier)
                    {
                        if (GeometryUtils.Distance(mousePos, edge.BezierPoint1.Position) <= clickTolerance)
                        {
                            result.ClickTarget = ClickTarget.BezierControlPoint;
                            result.Polygon = polygon;
                            result.Edge = edge;
                            result.ControlPoint = edge.BezierPoint1;
                            return result;
                        }

                        if (GeometryUtils.Distance(mousePos, edge.BezierPoint2.Position) <= clickTolerance)
                        {
                            result.ClickTarget = ClickTarget.BezierControlPoint;
                            result.Polygon = polygon;
                            result.Edge = edge;
                            result.ControlPoint = edge.BezierPoint2;
                            return result;
                        }
                    }
                }
            }
            return null;
        }
    }
}
