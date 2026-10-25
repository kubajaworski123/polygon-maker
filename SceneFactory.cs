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
            poly1.AddVertex(150, 150);
            poly1.AddVertex(450, 120);
            poly1.AddVertex(400, 380);
            poly1.AddVertex(120, 320);

            if (poly1.Edges.Count > 0)
            {
                poly1.Edges[0].SetToBezier();
            }

            scene.AddPolygon(poly1);

            var poly2 = new Polygon();
            poly2.AddVertex(600, 200);
            poly2.AddVertex(850, 180);
            poly2.AddVertex(750, 450);

            scene.AddPolygon(poly2);

            return scene;
        }
    }
}
