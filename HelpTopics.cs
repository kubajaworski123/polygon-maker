using System;
using System.Collections.Generic;
using System.Text;

namespace GKProjekt1
{
    public static class HelpTopics
    {
        public const string Usage = """
            Left button
            Drag a vertex to move it. The vertex follows the cursor. Neighboring edges keep their constraints, and Bezier control points keep the continuity set at that vertex.
            Drag a Bezier control point to move it. The vertices of its own segment stay in place. If continuity requires it, the edge on the other side of the vertex rotates or its control point moves.
            Drag anywhere else on a polygon to move the whole polygon. A click that does not hit a vertex, a control point, or an edge grabs the nearest polygon.
            
            Right button, edge
            The menu applies to the chord of the edge, including the dashed chord of a Bezier segment or an arc.
            Line segment, Bezier curve, and circular arc switch the edge type. Turning an edge into a Bezier shows the control polygon and two control points.
            Add vertex in the middle inserts a vertex at the center of the chord and removes the constraints of that edge.
            Set length... opens a dialog. The initial value is the current length.
            Parallel to... turns the cursor into a cross. Left-click the other edge, which may belong to another polygon. Any other click cancels.
            Each existing constraint is listed as Remove constraint.
            
            Right button, vertex
            G0, G1, and C1 set the continuity. G0 and G1 are available next to a Bezier segment or an arc. C1 is available only next to a Bezier segment. G1 is disabled when the other end of an adjacent arc already has G1. The checked item is the continuity in effect.
            Delete vertex removes the vertex when the polygon has more than three vertices. Constraints of the removed edges are deleted.
            """;

        public const string Assumptions = """
            Constraints
            Length and parallel constraints apply to the straight chord between the two vertices. This is also true when the edge is a Bezier segment or an arc.
            An edge cannot be made parallel to itself or to an edge that shares a vertex. A pair that is already parallel is rejected too.
            One edge may carry several constraints. While a change is applied, the first required direction and the first required length are the ones that move vertices. Afterwards every constraint is checked. A length may differ by less than half a pixel.
            If a change cannot be satisfied together with the existing constraints, it is undone. During a drag, the whole polygon is moved instead, so the grabbed point stays under the cursor and every constraint stays valid.
            
            Picking
            An edge is hit by its chord. A click on a Bezier curve or an arc, far from the dashed chord, does not open the edge menu.
            A left click that misses every vertex, control point, and edge drags the polygon whose chord is nearest to the cursor. The click is not tested against the polygon interior, and the shape of a Bezier or an arc is ignored.
            
            Continuity
            Until the user chooses, a vertex next to a Bezier segment is G1 and every other vertex is G0. Switching an edge to a Bezier does not replace a choice the user already made.
            An arc may have G1 at only one end. If both ends would otherwise default to G1, the end of the arc stays G0.
            At a G1 arc, the arc follows the neighbor. The neighbor is not moved to follow the arc. With G0 at both ends, the arc is a semicircle whose radius is half the chord. When the tangent is nearly parallel to the chord, or the radius would be very large, the arc is drawn as the chord.
            A line is parametrized by t from 0 to 1, so its tangent is the whole edge. The Bezier tangent at a vertex is three times the handle. Under C1 the handle therefore lies at one third of the adjacent edge. C1 does not make the curvature continuous.
            
            Editing the polygon
            Splitting a Bezier edge produces two straight edges. The curve is not kept.
            The user does not create or delete polygons. The polygons on the initial scene are the ones that can be edited, and a polygon may cross itself.
            """;
        public const string DataModel = """
                Scene
                A scene holds a list of polygons. Every edge of every polygon is reached through that list, so a constraint can join edges from different polygons. The polygons are created by the initial scene. The user edits them and does not add or remove whole polygons.
                
                Polygon
                A polygon stores its vertices in order and one edge from each vertex to the next, closing from the last vertex back to the first. Adding or removing a vertex rebuilds this list. An edge that still joins the same two vertices is kept, together with its type, control points, and constraints. An edge that disappears detaches its constraints.
                A polygon may cross itself. Nothing in the model forbids that.
                
                Points
                A vertex and a Bezier control point are both points with a position. The same point object is kept while it moves, because constraints and the solver refer to the object, not to a copy of its coordinates.
                A vertex knows the edge coming in and the edge going out, and through them the previous and the next vertex. It also stores the continuity the user chose. An empty choice means the continuity is decided from the neighboring edges.
                A control point knows its Bezier edge, which endpoint it belongs to, and the edge on the other side of that endpoint.
                
                Edge
                An edge joins two vertices and is a line segment, a cubic Bezier segment, or a circular arc. A Bezier edge owns two control points. An arc does not store a center or a radius. Those are computed from the endpoints and from the continuity at those endpoints.
                An edge keeps a list of constraints.
                
                Constraints
                A constraint names the edges it binds, a label drawn near the middle of each of them, the direction or length it requires, the vertices that stay fixed while it is applied, and a test that says whether it currently holds. Removing it takes it off every edge it is attached to.
                A length constraint binds one edge and fixes that edge's start vertex. A parallel constraint binds two edges and fixes both ends of the first one. The two edges may belong to different polygons. Both edges show the same label.
                
                Program structure
                The drawing of a line, a Bezier segment, and an arc goes through a strategy object. The Library strategy and the Bresenham strategy are two such objects, and the radio buttons switch the one the window holds.
                Mouse input is a state. The idle state decides what was hit. Dragging a vertex, dragging a control point, dragging a polygon, and choosing the second edge of a parallel constraint are the other states.
                A click is tested by a chain of responsibility: first vertices, then Bezier control points, then edges, then polygons. The first hit is the one that counts.
                """;
        public const string Algorithms = """
                Line segments
                The Library strategy draws a segment with a library function. The Bresenham strategy uses the midpoint algorithm.
                
                Bezier segments
                The Library strategy draws a cubic Bezier with the framework. The Bresenham strategy first changes the Bernstein form to the power basis,
                P(t) = A3·t³ + A2·t² + A1·t + A0,
                with A0 = V0, A1 = 3(V1 − V0), A2 = 3(V2 − 2·V1 + V0), and A3 = V3 − 3·V2 + 3·V1 − V0. V0 and V3 are the endpoints, and V1 and V2 are the control points.
                Successive points are then produced by forward differences. With a step d = 1/n, each point adds a first difference, that difference adds a second, and the second adds a constant third. The number of steps follows the length of the control polygon. The resulting points are joined by the midpoint line algorithm.
                The tangent at an endpoint needs no extra evaluation. P'(0) = 3(V1 − V0) and P'(1) = 3(V3 − V2).
                
                Continuity
                G0 only requires the shared vertex.
                G1 requires the outgoing tangent to be a positive multiple of the incoming one, so the handles stay on one straight line through the vertex and keep their current length.
                C1 requires the two tangent vectors to be equal.
                
                Arcs
                With G0 at both ends, the center is the midpoint of the chord and the radius is half the chord length.
                """;
    }
}
