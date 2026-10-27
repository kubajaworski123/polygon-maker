using GKProjekt1.Constraints;
using System;
using System.Collections.Generic;
using System.Text;

namespace GKProjekt1
{
    public static class SceneFactory
    {
        public static Scene CreateDefaultScene()
        {
            var scene = new Scene();
            var poly1 = new Polygon();
            poly1.AddVertex(90, 230);
            poly1.AddVertex(270, 180);
            poly1.AddVertex(450, 190);
            poly1.AddVertex(530, 300);
            poly1.AddVertex(510, 450);
            poly1.AddVertex(360, 550);
            poly1.AddVertex(160, 540);
            poly1.AddVertex(70, 400);

            poly1.Edges[0].SetToBezier();
            poly1.Edges[7].SetToArc();
            poly1.Vertices[7].SetContinuity(ContinuityType.G1);

            scene.AddPolygon(poly1);

            var poly2 = new Polygon();
            poly2.AddVertex(600, 200);
            poly2.AddVertex(850, 180);
            poly2.AddVertex(750, 450);
            poly2.Edges[2].SetToArc();

            scene.AddPolygon(poly2);

            ConstraintSolver.TryApply(scene, new LengthConstraint(poly1.Edges[5], 200f));
            ConstraintSolver.TryApply(scene, new ParallelConstraint(poly1.Edges[1], poly1.Edges[5]));
            ConstraintSolver.TryApply(scene, new LengthConstraint(poly2.Edges[0], 250f));

            return scene;
        }
    }
}
