using System;
using System.Collections.Generic;
using System.Text;

namespace GKProjekt1;

public static class GeometryUtils
{
    public static float Distance(PointF p1, PointF p2)
    {
        PointF v = DirectionVector(p1, p2);
        return (float)Math.Sqrt(DotProduct(v, v));
    }
    public static float Distance(PointF point, PointF LineStart, PointF LineEnd)
    {
        PointF v = DirectionVector(LineStart, LineEnd);
        float lengthSq = DotProduct(v, v);

        if (lengthSq == 0f)
            return Distance(point, LineStart);

        PointF r = DirectionVector(LineStart, point);
        float t = DotProduct(r, v) / lengthSq;
        t = Math.Clamp(t, 0f, 1f);

        PointF projection = new PointF(LineStart.X + t * v.X, LineStart.Y + t * v.Y);
        return Distance(point, projection);
    }
    public static PointF MidPoint(PointF p1, PointF p2)
    {
        return new PointF((p1.X + p2.X) / 2f, (p1.Y + p2.Y) / 2f);
    }
    public static float DotProduct(PointF p1, PointF p2)
    {
        return p1.X * p2.X + p1.Y * p2.Y;
    }
    public static float CrossProduct(PointF p1, PointF p2)
    {
        return p1.X * p2.Y - p1.Y * p2.X;
    }
    public static PointF DirectionVector(PointF p1, PointF p2)
    {
        return new PointF(p2.X - p1.X, p2.Y - p1.Y);
    }
    public static PointF PerpendicularVector(PointF v)
    {
        return new PointF(v.Y, -v.X);
    }
    public static PointF MovePoint(PointF point, float dx, float dy)
    {
        return new PointF(point.X + dx, point.Y + dy);
    }
    public static float Length(PointF v) => MathF.Sqrt(DotProduct(v, v));
    public static PointF Scale(PointF v, float factor) => new PointF(v.X * factor, v.Y * factor);

    public static PointF Normalize(PointF v)
    {
        float length = Length(v);
        if(length < 1e-6){
            return new PointF(1, 0);
        }
        return new PointF(v.X / length, v.Y / length);
    }
    public static PointF PointInDirection(PointF point, PointF direction, float distance)
    {
        PointF normalizedDirection = Normalize(direction);
        return new PointF(point.X + normalizedDirection.X * distance, point.Y + normalizedDirection.Y * distance);
    }
    public static bool AreParallel(PointF v1, PointF v2)
    {
        return MathF.Abs(CrossProduct(Normalize(v1), Normalize(v2))) < 1e-6;
    }
}