using System;
using System.Collections.Generic;
using System.Text;

namespace GKProjekt1.Constraints
{
    public class ParallelConstraint : EdgeConstraint
    {
        private static int nextId = 1;
        public Edge A { get; }
        public Edge B { get; }
        public int Id { get; } = nextId++;
        public override IReadOnlyList<Edge> Edges => [A, B];
        public override string Label => $"∥{Id}";
        public Edge Other(Edge e) => e == A ? B : A;
        public override PointF? RequiredDirection(Edge e) => Other(e).GetDirection();
        public override IEnumerable<Vertex> Anchors => [A.Start, A.End];
        public override bool IsSatisfied() =>
            MathF.Abs(GeometryUtils.CrossProduct(A.GetUnitDirection(), B.GetUnitDirection())) < 1e-3f;
        public ParallelConstraint(Edge a, Edge b)
        {
            A = a;
            B = b;
            a.Constraints.Add(this);
            b.Constraints.Add(this);
        }
    }
}
