using System;
using System.Collections.Generic;
using System.Text;

namespace GKProjekt1.Constraints
{
    public class LengthConstraint : EdgeConstraint
    {
        public Edge Edge { get; }
        public float Length { get; set; }
        public override IReadOnlyList<Edge> Edges => [Edge];
        public override string Label => $"L={Length:0}";
        public override float? RequiredLength(Edge e) => Length;
        public override bool IsSatisfied() => MathF.Abs(Edge.GetLength() - Length) < 0.5f;
        public override IEnumerable<Vertex> Anchors => [Edge.Start];
        public LengthConstraint(Edge edge, float length)
        {
            Edge = edge;
            Length = length;
            edge.Constraints.Add(this);
        }
    }
}
