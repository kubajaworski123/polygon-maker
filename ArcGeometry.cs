using System;
using System.Collections.Generic;
using System.Text;

namespace GKProjekt1
{
    public readonly record struct ArcGeometry(PointF Center, float Radius, float StartAngle, float SweepAngle)
    {
        private const float MinLength = 1e-3f;
        private const float MaxRadiusToChord = 50f;

        public PointF TangentAtStart => TangentAt(StartAngle);
        public PointF TangentAtEnd => TangentAt(StartAngle + SweepAngle);

        private PointF TangentAt(float angle) =>
            new PointF(-MathF.Sin(angle) * Radius * SweepAngle, MathF.Cos(angle) * Radius * SweepAngle);

        public static ArcGeometry Semicircle(PointF start, PointF end)
        {
            PointF center = GeometryUtils.MidPoint(start, end);
            return new ArcGeometry(center, GeometryUtils.Distance(start, end) / 2f, AngleOf(center, start), MathF.PI);
        }

        public static ArcGeometry? FromStartTangent(PointF start, PointF end, PointF tangent)
        {
            PointF chord = GeometryUtils.DirectionVector(start, end);
            float chordLength = GeometryUtils.Length(chord);
            if (GeometryUtils.Length(tangent) < MinLength || chordLength < MinLength)
            {
                return Semicircle(start, end);
            }

            PointF normal = GeometryUtils.Normalize(GeometryUtils.PerpendicularVector(tangent));
            float normalDotChord = GeometryUtils.DotProduct(normal, chord);
            if (MathF.Abs(normalDotChord) < MinLength)
            {
                return null;
            }
            float lambda = chordLength * chordLength / (2f * normalDotChord);
            if (MathF.Abs(lambda) > MaxRadiusToChord * chordLength)
            {
                return null;
            }

            PointF center = GeometryUtils.PointInDirection(start, normal, lambda);
            float startAngle = AngleOf(center, start);
            float endAngle = AngleOf(center, end);
            PointF increasingAngleTangent = new PointF(-MathF.Sin(startAngle), MathF.Cos(startAngle));
            float sweep = GeometryUtils.DotProduct(increasingAngleTangent, tangent) >= 0
                ? PositiveAngle(endAngle - startAngle)
                : -PositiveAngle(startAngle - endAngle);
            return new ArcGeometry(center, MathF.Abs(lambda), startAngle, sweep);
        }

        public static ArcGeometry? FromEndTangent(PointF start, PointF end, PointF tangent)
        {
            if (GeometryUtils.Length(tangent) < MinLength)
            {
                return Semicircle(start, end);
            }
            ArcGeometry? reversed = FromStartTangent(end, start, GeometryUtils.Scale(tangent, -1f));
            if (reversed is not ArcGeometry arc)
            {
                return null;
            }
            return new ArcGeometry(arc.Center, arc.Radius, arc.StartAngle + arc.SweepAngle, -arc.SweepAngle);
        }

        private static float AngleOf(PointF center, PointF point) => MathF.Atan2(point.Y - center.Y, point.X - center.X);

        private static float PositiveAngle(float angle)
        {
            float result = angle % (2f * MathF.PI);
            return result < 0 ? result + 2f * MathF.PI : result;
        }
    }
}