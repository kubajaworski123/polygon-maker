using GKProjekt1.Constraints;
using System;
using System.Collections.Generic;
using System.Text;

namespace GKProjekt1
{
    public class Polygon
    {
        public List<Vertex> Vertices { get; } = new List<Vertex>();
        public List<Edge> Edges { get; } = new List<Edge>();
        public IEnumerable<Node> AllNodes =>
        Vertices.Cast<Node>().Concat(Edges.SelectMany(e => new Node[] { e.BezierPoint1, e.BezierPoint2 }));
        public void AddVertex(Vertex vertex)
        {
            Vertices.Add(vertex);
            RebuildEdges();
        }
        public void AddVertex(float x, float y)
        {
            Vertices.Add(new Vertex(x, y));
            RebuildEdges();
        }
        public void Move(float deltaX, float deltaY)
        {
            foreach (var vertex in Vertices)
            {
                vertex.MoveByVector(deltaX, deltaY);
            }

            foreach (var edge in Edges)
            {
                if (edge.Type == EdgeType.Bezier)
                {
                    edge.BezierPoint1.Position = GeometryUtils.MovePoint(edge.BezierPoint1.Position, deltaX, deltaY);
                    edge.BezierPoint2.Position = GeometryUtils.MovePoint(edge.BezierPoint2.Position, deltaX, deltaY);

                }
            }
        }
        public Vertex SplitEdge(Edge targetEdge)
        {
            int edgeIdx = Edges.IndexOf(targetEdge);
            if (edgeIdx == -1)
            {
                return null;
            }

            PointF middle = targetEdge.GetMiddle();
            Vertex newVertex = new Vertex(middle);
            int insertIdx = Vertices.IndexOf(targetEdge.End);
            if (insertIdx == -1)
            {
                Vertices.Add(newVertex);
            }
            else
            {
                Vertices.Insert(insertIdx, newVertex);
            }

            RebuildEdges();
            ContinuityEnforcer.EnforceAround([newVertex]);
            return newVertex;
        }
        public bool RemoveVertex(Vertex vertex)
        {
            if (Vertices.Count <= 3)
            {
                return false;
            }

            Vertex? prev = vertex.PrevV;
            Vertex? next = vertex.NextV;
            bool removed = Vertices.Remove(vertex);
            if (removed)
            {
                vertex.InEdge = null;
                vertex.OutEdge = null;
                RebuildEdges();
                if (prev != null)
                {
                    ContinuityEnforcer.Enforce(prev);
                }
                if (next != null)
                {
                    ContinuityEnforcer.Enforce(next);
                }
            }
            return removed;
        }

        public void RebuildEdges()
        {
            var oldEdges = new List<Edge>(Edges);
            Edges.Clear();
            if (Vertices.Count < 2)
            {
                return;
            }
            for (int i = 0; i < Vertices.Count; i++)
            {
                Vertex current = Vertices[i];
                Vertex next = Vertices[(i + 1) % Vertices.Count];

                Edge existing = oldEdges.Find(e => e.Start == current && e.End == next);
                if (existing != null)
                {
                    Edges.Add(existing);
                    current.OutEdge = existing;
                    next.InEdge = existing;
                }
                else
                {
                    Edge toAdd = new Edge(current, next);
                    Edges.Add(toAdd);
                    current.OutEdge = toAdd;
                    next.InEdge = toAdd;
                }
            }
            foreach (var removed in oldEdges.Where(e => !Edges.Contains(e)))
            {
                foreach (var c in removed.Constraints.ToList())
                    c.Detach();
            }
        }
    }
}
