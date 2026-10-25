namespace GKProjekt1
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            menuStrip1 = new MenuStrip();
            helpToolStripMenuItem = new ToolStripMenuItem();
            usageToolStripMenuItem = new ToolStripMenuItem();
            assumptionsToolStripMenuItem = new ToolStripMenuItem();
            dataModelToolStripMenuItem = new ToolStripMenuItem();
            algoritmsToolStripMenuItem = new ToolStripMenuItem();
            panel1 = new Panel();
            BresenhamAlgorithmsRadioButton = new RadioButton();
            LibraryAlgorithmsRadioButton = new RadioButton();
            menuStrip1.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(24, 24);
            menuStrip1.Items.AddRange(new ToolStripItem[] { helpToolStripMenuItem });
            resources.ApplyResources(menuStrip1, "menuStrip1");
            menuStrip1.Name = "menuStrip1";
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { usageToolStripMenuItem, assumptionsToolStripMenuItem, dataModelToolStripMenuItem, algoritmsToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            resources.ApplyResources(helpToolStripMenuItem, "helpToolStripMenuItem");
            // 
            // usageToolStripMenuItem
            // 
            usageToolStripMenuItem.Name = "usageToolStripMenuItem";
            resources.ApplyResources(usageToolStripMenuItem, "usageToolStripMenuItem");
            // 
            // assumptionsToolStripMenuItem
            // 
            assumptionsToolStripMenuItem.Name = "assumptionsToolStripMenuItem";
            resources.ApplyResources(assumptionsToolStripMenuItem, "assumptionsToolStripMenuItem");
            // 
            // dataModelToolStripMenuItem
            // 
            dataModelToolStripMenuItem.Name = "dataModelToolStripMenuItem";
            resources.ApplyResources(dataModelToolStripMenuItem, "dataModelToolStripMenuItem");
            // 
            // algoritmsToolStripMenuItem
            // 
            algoritmsToolStripMenuItem.Name = "algoritmsToolStripMenuItem";
            resources.ApplyResources(algoritmsToolStripMenuItem, "algoritmsToolStripMenuItem");
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.Window;
            panel1.Controls.Add(BresenhamAlgorithmsRadioButton);
            panel1.Controls.Add(LibraryAlgorithmsRadioButton);
            resources.ApplyResources(panel1, "panel1");
            panel1.Name = "panel1";
            // 
            // BresenhamAlgorithmsRadioButton
            // 
            resources.ApplyResources(BresenhamAlgorithmsRadioButton, "BresenhamAlgorithmsRadioButton");
            BresenhamAlgorithmsRadioButton.Name = "BresenhamAlgorithmsRadioButton";
            BresenhamAlgorithmsRadioButton.TabStop = true;
            BresenhamAlgorithmsRadioButton.UseVisualStyleBackColor = true;
            // 
            // LibraryAlgorithmsRadioButton
            // 
            resources.ApplyResources(LibraryAlgorithmsRadioButton, "LibraryAlgorithmsRadioButton");
            LibraryAlgorithmsRadioButton.Name = "LibraryAlgorithmsRadioButton";
            LibraryAlgorithmsRadioButton.TabStop = true;
            LibraryAlgorithmsRadioButton.UseVisualStyleBackColor = true;
            // 
            // MainWindow
            // 
            AutoScaleMode = AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            Controls.Add(panel1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "MainWindow";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem usageToolStripMenuItem;
        private ToolStripMenuItem assumptionsToolStripMenuItem;
        private ToolStripMenuItem dataModelToolStripMenuItem;
        private ToolStripMenuItem algoritmsToolStripMenuItem;
        private Panel panel1;
        private RadioButton BresenhamAlgorithmsRadioButton;
        private RadioButton LibraryAlgorithmsRadioButton;
    }
}
