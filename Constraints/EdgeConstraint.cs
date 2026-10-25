using System;
using System.Collections.Generic;
using System.Text;

namespace GKProjekt1.Constraints
{
    public abstract class EdgeConstraint
    {
        public abstract IReadOnlyList<Edge> Edges { get; }
        public abstract string Label { get; }
        public virtual PointF? RequiredDirection(Edge edge) => null;
        public virtual float? RequiredLength(Edge edge) => null;
        public abstract bool IsSatisfied();
        public abstract IEnumerable<Vertex> Anchors { get; }
        public void Detach()
        {
            foreach (var e in Edges) e.Constraints.Remove(this);
        }
    }
}
