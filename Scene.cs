using System;
using System.Collections.Generic;
using System.Text;

namespace GKProjekt1
{
    public class Scene
    {
        public List<Polygon> Polygons { get; } = new List<Polygon>();
        public IEnumerable<Edge> AllEdges => Polygons.SelectMany(p => p.Edges);
        public void AddPolygon(Polygon polygon)
        {
            Polygons.Add(polygon);
        }
        public void Clear()
        {
            Polygons.Clear();
        }
    }
}
