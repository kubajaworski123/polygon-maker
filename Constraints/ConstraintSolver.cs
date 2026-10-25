using System;
using System.Collections.Generic;
using System.Text;

namespace GKProjekt1.Constraints
{
    public static class ConstraintSolver
    {
        private const float Epsilon = 0.01f;
        private const float LengthTolerance = 0.5f;

        public static bool TryMoveVertex(Scene scene, Vertex vertex, PointF target) =>
            Solve(scene, [vertex], () => vertex.MoveTo(target));

        public static bool TryApply(Scene scene, EdgeConstraint constraint) =>
            Solve(scene, constraint.Anchors, () => { });

        private static bool Solve(Scene scene, IEnumerable<Vertex> anchors, Action initialChange)
        {
            var snapshot = scene.Polygons
                .SelectMany(p => p.AllNodes)
                .ToDictionary(n => n, n => n.Position);

            initialChange();

            var locked = new HashSet<Vertex>(anchors);
            var queue = new Queue<Vertex>(locked);
            bool ok = true;

            while (ok && queue.Count > 0)
            {
                Vertex v = queue.Dequeue();
                foreach (Edge? edge in new[] { v.InEdge, v.OutEdge })
                {
                    if (edge != null && !EnforceEdge(edge, v, locked, queue))
                    {
                        ok = false;
                        break;
                    }
                }
            }

            if (ok && scene.AllEdges.SelectMany(e => e.Constraints).All(c => c.IsSatisfied()))
            {
                return true;
            }

            foreach (var (node, position) in snapshot)
            {
                node.Position = position;
            }
            return false;
        }

        private static bool EnforceEdge(Edge edge, Vertex fixedEnd, HashSet<Vertex> locked, Queue<Vertex> queue)
        {
            if (edge.Constraints.Count == 0)
            {
                return true;
            }

            Vertex other = edge.Other(fixedEnd);

            if (locked.Contains(other))
            {
                float? requiredLength = edge.GetRequiredLength();
                if (requiredLength.HasValue && MathF.Abs(edge.GetLength() - requiredLength.Value) > LengthTolerance)
                {
                    return false;
                }
                AlignPartners(edge, locked, queue);
                return true;
            }

            PointF current = GeometryUtils.DirectionVector(fixedEnd.Position, other.Position);
            PointF direction = OrientLike(edge.GetRequiredDirection() ?? current, current);
            float length = edge.GetRequiredLength() ?? GeometryUtils.Length(current);

            PointF target = GeometryUtils.PointInDirection(fixedEnd.Position, direction, length);
            if (GeometryUtils.Distance(target, other.Position) > Epsilon)
            {
                other.MoveTo(target);
                locked.Add(other);
                queue.Enqueue(other);
            }
            return true;
        }

        private static void AlignPartners(Edge edge, HashSet<Vertex> locked, Queue<Vertex> queue)
        {
            foreach (var partner in edge.Constraints.OfType<ParallelConstraint>().Select(p => p.Other(edge)))
            {
                if (GeometryUtils.AreParallel(edge.GetDirection(), partner.GetDirection()))
                {
                    continue;
                }

                bool startLocked = locked.Contains(partner.Start);
                bool endLocked = locked.Contains(partner.End);
                if (startLocked && endLocked)
                {
                    // Naruszenie (jeśli realne) wykryje końcowe sprawdzenie IsSatisfied.
                    continue;
                }

                Vertex pivot = endLocked ? partner.End : partner.Start;
                Vertex moving = partner.Other(pivot);

                PointF current = GeometryUtils.DirectionVector(pivot.Position, moving.Position);
                PointF direction = OrientLike(edge.GetDirection(), current);
                float length = partner.GetRequiredLength() ?? GeometryUtils.Length(current);

                moving.MoveTo(GeometryUtils.PointInDirection(pivot.Position, direction, length));
                locked.Add(moving);
                queue.Enqueue(moving);
            }
        }

        private static PointF OrientLike(PointF direction, PointF reference) =>
            GeometryUtils.DotProduct(direction, reference) < 0
                ? new PointF(-direction.X, -direction.Y)
                : direction;
    }
}
