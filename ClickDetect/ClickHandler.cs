using System;
using System.Collections.Generic;
using System.Text;

namespace GKProjekt1.ClickDetect
{
    public abstract class ClickHandler
    {
        public ClickHandler? NextHandler { get; set; }
        public ClickHandler SetNext(ClickHandler handler)
        {
            NextHandler = handler;
            return handler;
        }
        public ClickData Handle(Scene scene, PointF mousePos)
        {
            ClickData? clickData = CheckClick(scene, mousePos);
            if (clickData != null)
            {
                return clickData;
            }
            else if (NextHandler != null)
            {
                return NextHandler.Handle(scene, mousePos);
            }
            else
            {
                return new ClickData { ClickTarget = ClickTarget.None };
            }
        }
        public abstract ClickData? CheckClick(Scene scene, PointF mousePos);
    }
}
