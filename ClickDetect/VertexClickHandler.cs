using System;
using System.Collections.Generic;
using System.Text;

namespace GKProjekt1.ClickDetect
{
    public class VertexClickHandler : ClickHandler
    {
        private const float clickTolerance = 8f;
        public override ClickData? CheckClick(Scene scene, PointF mousePos)
        {
            var result = new ClickData();
            foreach (var polygon in scene.Polygons)
            {
                foreach (var vertex in polygon.Vertices)
                {
                    if (GeometryUtils.Distance(mousePos, vertex.Position) <= clickTolerance)
                    {
                        result.ClickTarget = ClickTarget.Vertex;
                        result.Polygon = polygon;
                        result.Vertex = vertex;
                        return result;
                    }
                }
            }
            return null;
        }
    }
}
