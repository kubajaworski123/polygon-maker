using System;
using System.Collections.Generic;
using System.Text;

namespace GKProjekt1
{
    public class WidthDialog : Form
    {
        private readonly NumericUpDown input;
        public int Value => (int)input.Value;
        public WidthDialog(int initialWidth)
        {
            Text = "Szerokosc krawedzi";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;

            input = new NumericUpDown
            {
                Minimum = 1,
                Maximum = 50,
                DecimalPlaces = 0,
                Width = 200,
                Value = Math.Clamp(initialWidth, 1, 50)
            };

            var okButton = new Button { Text = "OK", DialogResult = DialogResult.OK, AutoSize = true };
            var cancelButton = new Button { Text = "Anuluj", DialogResult = DialogResult.Cancel, AutoSize = true };

            var buttons = new FlowLayoutPanel { FlowDirection = FlowDirection.RightToLeft, AutoSize = true, Dock = DockStyle.Fill };
            buttons.Controls.Add(cancelButton);
            buttons.Controls.Add(okButton);

            var layout = new TableLayoutPanel { AutoSize = true, ColumnCount = 1, Padding = new Padding(10) };
            layout.Controls.Add(new Label { Text = "Szerokosc:", AutoSize = true });
            layout.Controls.Add(input);
            layout.Controls.Add(buttons);
            Controls.Add(layout);

            AcceptButton = okButton;
            CancelButton = cancelButton;
            Shown += (s, e) => input.Select(0, input.Text.Length);
        }
    }
}
