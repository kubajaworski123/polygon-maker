using System;
using System.Collections.Generic;
using System.Text;

namespace GKProjekt1
{
    public class HelpDialog : Form
    {
        public HelpDialog(string title, string content)
        {
            Text = title;
            FormBorderStyle = FormBorderStyle.Sizable;
            StartPosition = FormStartPosition.CenterParent;
            MinimizeBox = false;
            ShowInTaskbar = false;
            ClientSize = new Size(560, 420);

            var text = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 10f),
                Text = content
            };
            var body = new Panel { Dock = DockStyle.Fill, Padding = new Padding(12) };
            body.Controls.Add(text);

            var closeButton = new Button { Text = "Zamknij", DialogResult = DialogResult.OK, AutoSize = true };
            var buttons = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.RightToLeft,
                Dock = DockStyle.Bottom,
                AutoSize = true,
                Padding = new Padding(8)
            };
            buttons.Controls.Add(closeButton);

            Controls.Add(body);
            Controls.Add(buttons);
            AcceptButton = closeButton;
            CancelButton = closeButton;
        }
    }
}
