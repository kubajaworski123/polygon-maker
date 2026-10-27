using GKProjekt1.Constraints;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GKProjekt1.SceneSaving
{
    public static class SceneFile
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new JsonStringEnumConverter() }
        };

        public static void Save(Scene scene, string path)
        {
            File.WriteAllText(path, JsonSerializer.Serialize(ToDto(scene), Options));
        }

        public static Scene Load(string path)
        {
            SceneDto? dto = JsonSerializer.Deserialize<SceneDto>(File.ReadAllText(path), Options);
            if (dto == null)
            {
                throw new InvalidDataException("Pusty plik.");
            }
            return FromDto(dto);
        }

        private static SceneDto ToDto(Scene scene)
        {
            var dto = new SceneDto();
            var index = new Dictionary<Edge, int[]>();
            for (int i = 0; i < scene.Polygons.Count; i++)
            {
                Polygon polygon = scene.Polygons[i];
                var polygonDto = new PolygonDto();
                foreach (Vertex vertex in polygon.Vertices)
                {
                    polygonDto.Vertices.Add(new VertexDto
                    {
                        X = vertex.Position.X,
                        Y = vertex.Position.Y,
                        Continuity = vertex.ExplicitContinuity
                    });
                }
                for (int j = 0; j < polygon.Edges.Count; j++)
                {
                    Edge edge = polygon.Edges[j];
                    index[edge] = [i, j];
                    var edgeDto = new EdgeDto { Type = edge.Type, Width = edge.Width };
                    if (edge.Type == EdgeType.Bezier)
                    {
                        edgeDto.C1 = new PointDto { X = edge.BezierPoint1.Position.X, Y = edge.BezierPoint1.Position.Y };
                        edgeDto.C2 = new PointDto { X = edge.BezierPoint2.Position.X, Y = edge.BezierPoint2.Position.Y };
                    }
                    polygonDto.Edges.Add(edgeDto);
                }
                dto.Polygons.Add(polygonDto);
            }

            var seenParallels = new HashSet<ParallelConstraint>();
            foreach (Edge edge in scene.AllEdges)
            {
                foreach (EdgeConstraint constraint in edge.Constraints)
                {
                    if (constraint is LengthConstraint length)
                    {
                        dto.Lengths.Add(new LengthDto
                        {
                            Polygon = index[edge][0],
                            Edge = index[edge][1],
                            Length = length.Length
                        });
                    }
                    else if (constraint is ParallelConstraint parallel && seenParallels.Add(parallel))
                    {
                        dto.Parallels.Add(new ParallelDto { A = index[parallel.A], B = index[parallel.B] });
                    }
                }
            }
            return dto;
        }

        private static Scene FromDto(SceneDto dto)
        {
            if (dto.Version != 1)
            {
                throw new InvalidDataException("Nieznana wersja pliku.");
            }
            dto.Polygons ??= new();
            dto.Lengths ??= new();
            dto.Parallels ??= new();

            var scene = new Scene();
            foreach (PolygonDto polygonDto in dto.Polygons)
            {
                polygonDto.Vertices ??= new();
                polygonDto.Edges ??= new();
                if (polygonDto.Vertices.Count < 3 || polygonDto.Edges.Count != polygonDto.Vertices.Count)
                {
                    throw new InvalidDataException("Zla liczba wierzcholkow lub krawedzi.");
                }

                var polygon = new Polygon();
                foreach (VertexDto vertexDto in polygonDto.Vertices)
                {
                    polygon.AddVertex(vertexDto.X, vertexDto.Y);
                }
                for (int i = 0; i < polygon.Vertices.Count; i++)
                {
                    polygon.Vertices[i].ExplicitContinuity = polygonDto.Vertices[i].Continuity;
                    EdgeDto edgeDto = polygonDto.Edges[i];
                    Edge edge = polygon.Edges[i];
                    edge.Width = Math.Max(1, edgeDto.Width);
                    if (edgeDto.Type == EdgeType.Bezier)
                    {
                        if (edgeDto.C1 == null || edgeDto.C2 == null)
                        {
                            throw new InvalidDataException("Brak punktow kontrolnych Beziera.");
                        }
                        edge.Type = EdgeType.Bezier;
                        edge.BezierPoint1.Position = new PointF(edgeDto.C1.X, edgeDto.C1.Y);
                        edge.BezierPoint2.Position = new PointF(edgeDto.C2.X, edgeDto.C2.Y);
                    }
                    else if (edgeDto.Type == EdgeType.Arc)
                    {
                        edge.Type = EdgeType.Arc;
                    }
                }
                scene.AddPolygon(polygon);
            }

            foreach (LengthDto length in dto.Lengths)
            {
                new LengthConstraint(EdgeAt(scene, length.Polygon, length.Edge), length.Length);
            }
            foreach (ParallelDto parallel in dto.Parallels)
            {
                new ParallelConstraint(EdgeAt(scene, parallel.A), EdgeAt(scene, parallel.B));
            }
            return scene;
        }

        private static Edge EdgeAt(Scene scene, int polygon, int edge)
        {
            if (polygon < 0 || polygon >= scene.Polygons.Count)
            {
                throw new InvalidDataException("Zly numer wielokata.");
            }
            List<Edge> edges = scene.Polygons[polygon].Edges;
            if (edge < 0 || edge >= edges.Count)
            {
                throw new InvalidDataException("Zly numer krawedzi.");
            }
            return edges[edge];
        }

        private static Edge EdgeAt(Scene scene, int[]? address)
        {
            if (address == null || address.Length != 2)
            {
                throw new InvalidDataException("Zly wskaznik krawedzi.");
            }
            return EdgeAt(scene, address[0], address[1]);
        }
    }
}
