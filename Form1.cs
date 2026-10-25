using GKProjekt1.ClickDetect;
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
        private ClickData? activeClickData;
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
                // Domyslne ustawienie ciaglosci G1 w sasiadujacych wierzcholkach zgodnie z wymaganiem
                edge.Start.Continuity = ContinuityType.G1;
                edge.End.Continuity = ContinuityType.G1;
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

            edgeContextMenu.Items.Add(lineItem);
            edgeContextMenu.Items.Add(bezierItem);
            edgeContextMenu.Items.Add(arcItem);
            edgeContextMenu.Items.Add(new ToolStripSeparator());
            edgeContextMenu.Items.Add(splitItem);

            edgeContextMenu.Show(this, location);
        }

        public void ShowVertexContextMenu(Point location, Vertex vertex, Polygon polygon)
        {
            vertexContextMenu.Items.Clear();

            var g0Item = new ToolStripMenuItem("Ciaglosc G0", null, (s, e) =>
            {
                vertex.Continuity = ContinuityType.G0;
                Invalidate();
            })
            { Checked = vertex.Continuity == ContinuityType.G0 };

            var g1Item = new ToolStripMenuItem("Ciaglosc G1", null, (s, e) =>
            {
                vertex.Continuity = ContinuityType.G1;
                Invalidate();
            })
            { Checked = vertex.Continuity == ContinuityType.G1 };

            var c1Item = new ToolStripMenuItem("Ciaglosc C1", null, (s, e) =>
            {
                vertex.Continuity = ContinuityType.C1;
                Invalidate();
            })
            { Checked = vertex.Continuity == ContinuityType.C1 };

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

            Invalidate(); // Wymuszenie ponownego przerysowania okna
        }
    }
}
