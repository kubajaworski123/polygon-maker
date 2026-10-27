using GKProjekt1.ClickDetect;
using GKProjekt1.Constraints;
using GKProjekt1.DrawingStrategies;
using GKProjekt1.States;

namespace GKProjekt1
{
    public partial class MainWindow : Form
    {
        private Scene scene;
        private SceneRenderer renderer;
        private DrawingStrategy currentStrategy;
        private ContextMenuStrip edgeContextMenu;
        private ContextMenuStrip vertexContextMenu;
        private Bitmap canvasBitmap;
        private IActionState currentState;

        public MainWindow()
        {
            InitializeComponent();
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);

            scene = SceneFactory.CreateDefaultScene();
            renderer = new SceneRenderer();
            currentStrategy = new LibraryDrawingStrategy();
            edgeContextMenu = new ContextMenuStrip();
            vertexContextMenu = new ContextMenuStrip();
            canvasBitmap = new Bitmap(ClientSize.Width, ClientSize.Height);
            currentState = new IdleState(this, scene);

            LibraryAlgorithmsRadioButton.Checked = true;
            MouseDown += MainWindow_MouseDown;
            MouseMove += MainWindow_MouseMove;
            MouseUp += MainWindow_MouseUp;
            Paint += MainWindow_Paint;
            LibraryAlgorithmsRadioButton.CheckedChanged += StrategyRadioButton_CheckedChanged;
            BresenhamAlgorithmsRadioButton.CheckedChanged += StrategyRadioButton_CheckedChanged;
            usageToolStripMenuItem.Click += (s, e) => ShowHelp(usageToolStripMenuItem.Text ?? "", HelpTopics.Usage);
            assumptionsToolStripMenuItem.Click += (s, e) => ShowHelp(assumptionsToolStripMenuItem.Text ?? "", HelpTopics.Assumptions);
            dataModelToolStripMenuItem.Click += (s, e) => ShowHelp(dataModelToolStripMenuItem.Text ?? "", HelpTopics.DataModel);
            algoritmsToolStripMenuItem.Click += (s, e) => ShowHelp(algoritmsToolStripMenuItem.Text ?? "", HelpTopics.Algorithms);
        }
        private void ShowHelp(string title, string content)
        {
            using var dialog = new HelpDialog(title, content);
            dialog.ShowDialog(this);
        }
        public void TransitionTo(IActionState newState)
        {
            currentState = newState;
        }
        private void MainWindow_Paint(object sender, PaintEventArgs e)
        {
            using (Graphics bufferGraphics = Graphics.FromImage(canvasBitmap))
            {
                bufferGraphics.Clear(BackColor);
                renderer.Render(bufferGraphics, canvasBitmap, scene, currentStrategy);
            }
            e.Graphics.DrawImage(canvasBitmap, 0, 0);
        }
        private void MainWindow_MouseDown(object? sender, MouseEventArgs e)
        {
            currentState.OnMouseDown(e);
        }
        private void MainWindow_MouseMove(object? sender, MouseEventArgs e)
        {
            currentState.OnMouseMove(e);
        }
        private void MainWindow_MouseUp(object? sender, MouseEventArgs e)
        {
            currentState.OnMouseUp(e);
        }
        public void ShowEdgeContextMenu(Point location, Edge edge, Polygon polygon)
        {
            edgeContextMenu.Items.Clear();

            var lineItem = new ToolStripMenuItem("Odcinek (Linia)", null, (s, e) =>
            {
                edge.SetToLine();
                Invalidate();
            })
            { Checked = edge.Type == EdgeType.Line };

            var bezierItem = new ToolStripMenuItem("Krzywa Beziera", null, (s, e) =>
            {
                edge.SetToBezier();
                Invalidate();
            })
            { Checked = edge.Type == EdgeType.Bezier };

            var arcItem = new ToolStripMenuItem("Luk okregu", null, (s, e) =>
            {
                edge.SetToArc();
                Invalidate();
            })
            { Checked = edge.Type == EdgeType.Arc };

            var splitItem = new ToolStripMenuItem("Dodaj wierzcholek w srodku", null, (s, e) =>
            {
                polygon.SplitEdge(edge);
                Invalidate();
            });

            var lengthItem = new ToolStripMenuItem("Ustaw dlugosc...", null, (s, e) => SetEdgeLength(edge));

            var parallelItem = new ToolStripMenuItem("Rownolegla do...", null, (s, e) =>
            {
                TransitionTo(new SelectingParallelEdgeState(this, scene, edge));
            });

            edgeContextMenu.Items.Add(lineItem);
            edgeContextMenu.Items.Add(bezierItem);
            edgeContextMenu.Items.Add(arcItem);
            edgeContextMenu.Items.Add(new ToolStripSeparator());
            edgeContextMenu.Items.Add(splitItem);
            edgeContextMenu.Items.Add(new ToolStripSeparator());
            edgeContextMenu.Items.Add(lengthItem);
            edgeContextMenu.Items.Add(parallelItem);

            if (edge.Constraints.Count > 0)
            {
                edgeContextMenu.Items.Add(new ToolStripSeparator());
                foreach (var constraint in edge.Constraints.ToList())
                {
                    edgeContextMenu.Items.Add(new ToolStripMenuItem($"Usun ograniczenie: {constraint.Label}", null, (s, e) =>
                    {
                        constraint.Detach();
                        Invalidate();
                    }));
                }
            }

            edgeContextMenu.Show(this, location);
        }

        private void SetEdgeLength(Edge edge)
        {
            using var dialog = new LengthDialog(edge.GetLength());
            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            var existing = edge.Constraints.OfType<LengthConstraint>().FirstOrDefault();
            if (existing != null)
            {
                float previousLength = existing.Length;
                existing.Length = dialog.Value;
                if (!ConstraintSolver.TryApply(scene, existing))
                {
                    existing.Length = previousLength;
                    ShowConstraintConflict();
                }
            }
            else
            {
                var constraint = new LengthConstraint(edge, dialog.Value);
                if (!ConstraintSolver.TryApply(scene, constraint))
                {
                    constraint.Detach();
                    ShowConstraintConflict();
                }
            }
            Invalidate();
        }

        private void ShowConstraintConflict()
        {
            MessageBox.Show(this, "Nie mozna spelnic tego ograniczenia razem z istniejacymi.", "Ograniczenia");
        }

        public void ShowVertexContextMenu(Point location, Vertex vertex, Polygon polygon)
        {
            vertexContextMenu.Items.Clear();

            ContinuityType continuity = vertex.GetEffectiveContinuity();
            bool nextToBezier = vertex.HasAdjacentEdgeOfType(EdgeType.Bezier);
            bool nextToArc = vertex.HasAdjacentEdgeOfType(EdgeType.Arc);

            var g0Item = new ToolStripMenuItem("Ciaglosc G0", null, (s, e) =>
            {
                vertex.SetContinuity(ContinuityType.G0);
                Invalidate();
            })
            { Checked = continuity == ContinuityType.G0, Enabled = nextToBezier || nextToArc };

            var g1Item = new ToolStripMenuItem("Ciaglosc G1", null, (s, e) =>
            {
                vertex.SetContinuity(ContinuityType.G1);
                Invalidate();
            })
            { Checked = continuity == ContinuityType.G1, Enabled = (nextToBezier || nextToArc) && vertex.CanHaveG1() };

            var c1Item = new ToolStripMenuItem("Ciaglosc C1", null, (s, e) =>
            {
                vertex.SetContinuity(ContinuityType.C1);
                Invalidate();
            })
            { Checked = continuity == ContinuityType.C1, Enabled = nextToBezier && !nextToArc };

            var deleteItem = new ToolStripMenuItem("Usun wierzcholek", null, (s, e) =>
            {
                polygon.RemoveVertex(vertex);
                Invalidate();
            })
            { Enabled = polygon.Vertices.Count > 3 };

            vertexContextMenu.Items.Add(g0Item);
            vertexContextMenu.Items.Add(g1Item);
            vertexContextMenu.Items.Add(c1Item);
            vertexContextMenu.Items.Add(new ToolStripSeparator());
            vertexContextMenu.Items.Add(deleteItem);

            vertexContextMenu.Show(this, location);
        }
        private void StrategyRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (LibraryAlgorithmsRadioButton.Checked)
            {
                currentStrategy = new LibraryDrawingStrategy();
            }
            else if (BresenhamAlgorithmsRadioButton.Checked)
            {
                currentStrategy = new BresenhamDrawingStrategy();
            }

            Invalidate();
        }
    }
}
