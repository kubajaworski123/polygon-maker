using GKProjekt1.ClickDetect;
using GKProjekt1.Constraints;
using System;
using System.Collections.Generic;
using System.Text;

namespace GKProjekt1.States
{
    public class SelectingParallelEdgeState : IActionState
    {
        private readonly MainWindow window;
        private readonly Scene scene;
        private readonly Edge firstEdge;
        public SelectingParallelEdgeState(MainWindow window, Scene scene, Edge firstEdge)
        {
            this.window = window;
            this.scene = scene;
            this.firstEdge = firstEdge;
            window.Cursor = Cursors.Cross;
        }
        public void OnMouseDown(MouseEventArgs e)
        {
            var hit = ClickDetectUtils.Test(scene, e.Location);
            window.Cursor = Cursors.Default;
            window.TransitionTo(new IdleState(window, scene));

            if (e.Button == MouseButtons.Left && hit.ClickTarget == ClickTarget.Edge && hit.Edge != null)
            {
                TryAddParallel(hit.Edge);
            }
            window.Invalidate();
        }
        public void OnMouseMove(MouseEventArgs e) { }
        public void OnMouseUp(MouseEventArgs e) { }
        private void TryAddParallel(Edge secondEdge)
        {
            bool sharesVertex = secondEdge.Start == firstEdge.Start || secondEdge.Start == firstEdge.End
                             || secondEdge.End == firstEdge.Start || secondEdge.End == firstEdge.End;
            if (secondEdge == firstEdge || sharesVertex)
            {
                MessageBox.Show(window, "Wybierz inna krawedz, ktora nie sasiaduje z pierwsza.", "Rownoleglosc");
                return;
            }

            bool alreadyParallel = firstEdge.Constraints
                .OfType<ParallelConstraint>()
                .Any(p => p.Other(firstEdge) == secondEdge);
            if (alreadyParallel)
            {
                MessageBox.Show(window, "Te krawedzie sa juz rownolegle.", "Rownoleglosc");
                return;
            }

            var constraint = new ParallelConstraint(firstEdge, secondEdge);
            if (!ConstraintSolver.TryApply(scene, constraint))
            {
                constraint.Detach();
                MessageBox.Show(window, "Nie mozna spelnic tego ograniczenia razem z istniejacymi.", "Rownoleglosc");
            }
        }
    }
}
