using System;
using System.Collections.Generic;
using System.Text;

namespace GKProjekt1.States
{
    public interface IActionState
    {
        void OnMouseDown(MouseEventArgs e);
        void OnMouseMove(MouseEventArgs e);
        void OnMouseUp(MouseEventArgs e);
    }
}
