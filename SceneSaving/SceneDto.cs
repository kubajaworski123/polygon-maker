using System;
using System.Collections.Generic;
using System.Text;

namespace GKProjekt1.SceneSaving
{
    public class SceneDto
    {
        public int Version { get; set; } = 1;
        public List<PolygonDto> Polygons { get; set; } = new();
        public List<LengthDto> Lengths { get; set; } = new();
        public List<ParallelDto> Parallels { get; set; } = new();
    }

    public class PolygonDto
    {
        public List<VertexDto> Vertices { get; set; } = new();
        public List<EdgeDto> Edges { get; set; } = new();
    }

    public class VertexDto
    {
        public float X { get; set; }
        public float Y { get; set; }
        public ContinuityType? Continuity { get; set; }
    }

    public class EdgeDto
    {
        public EdgeType Type { get; set; }
        public PointDto? C1 { get; set; }
        public PointDto? C2 { get; set; }
    }

    public class PointDto
    {
        public float X { get; set; }
        public float Y { get; set; }
    }

    public class LengthDto
    {
        public int Polygon { get; set; }
        public int Edge { get; set; }
        public float Length { get; set; }
    }

    public class ParallelDto
    {
        public int[] A { get; set; } = new int[2];
        public int[] B { get; set; } = new int[2];
    }
}
