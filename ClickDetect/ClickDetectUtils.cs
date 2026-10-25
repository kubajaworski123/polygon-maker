using System;
using System.Collections.Generic;
using System.Text;

namespace GKProjekt1.ClickDetect;
public static class ClickDetectUtils
{
    private static readonly ClickHandler chain = BuildChain();
    public static ClickData Test(Scene scene, PointF mousePos)
    {
        return chain.Handle(scene, mousePos);
    }
    private static ClickHandler BuildChain()
    {
        var vertexHandler = new VertexClickHandler();
        vertexHandler.SetNext(new BezierCtrlPtClickHandler())
                     .SetNext(new EdgeClickHandler())
                     .SetNext(new PolygonClickHandler());
        return vertexHandler;
    }
}