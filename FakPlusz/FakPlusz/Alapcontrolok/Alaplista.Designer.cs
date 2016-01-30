namespace FakPlusz.Alapcontrolok
{
    partial class Alaplista
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.crystalReportViewer1 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.listatoolstrip = new System.Windows.Forms.ToolStrip();
            this.elsolap = new System.Windows.Forms.ToolStripButton();
            this.elozo = new System.Windows.Forms.ToolStripButton();
            this.kovetkezo = new System.Windows.Forms.ToolStripButton();
            this.utsolap = new System.Windows.Forms.ToolStripButton();
            this.nyomtat = new System.Windows.Forms.ToolStripButton();
            this.adatkiv = new System.Windows.Forms.ToolStripButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.nyomtmax = new System.Windows.Forms.ComboBox();
            this.nyomtmin = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lapszam = new System.Windows.Forms.ToolStripStatusLabel();
            this.maxlapszam = new System.Windows.Forms.ToolStripStatusLabel();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            ((System.ComponentModel.ISupportInitialize)(this.AlapTablaView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EgyszeruTablaView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OsszetettKozepsoTablaView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OsszetettAlsoTablaView)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.listatoolstrip.SuspendLayout();
            this.panel2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.Items.AddRange(new object[] {
            "  25",
            "  50",
            "  75",
            "100",
            "200",
            "500"});
            this.comboBox1.Location = new System.Drawing.Point(584, 4);
            this.comboBox1.Size = new System.Drawing.Size(71, 21);
            this.comboBox1.TabIndex = 4;
            this.comboBox1.Visible = true;
            this.comboBox1.DropDownClosed += new System.EventHandler(this.comboBox1_SelectionChangeCommitted);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Controls.Add(this.panel1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(680, 341);
            this.panel3.TabIndex = 6;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.crystalReportViewer1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 25);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(680, 316);
            this.panel4.TabIndex = 8;
            // 
            // crystalReportViewer1
            // 
            this.crystalReportViewer1.ActiveViewIndex = -1;
            this.crystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.crystalReportViewer1.DisplayBackgroundEdge = false;
            this.crystalReportViewer1.DisplayStatusBar = false;
            this.crystalReportViewer1.DisplayToolbar = false;
            this.crystalReportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crystalReportViewer1.EnableDrillDown = false;
            this.crystalReportViewer1.EnableToolTips = false;
            this.crystalReportViewer1.Location = new System.Drawing.Point(0, 0);
            this.crystalReportViewer1.Margin = new System.Windows.Forms.Padding(0);
            this.crystalReportViewer1.Name = "crystalReportViewer1";
            this.crystalReportViewer1.SelectionFormula = "";
            this.crystalReportViewer1.ShowCloseButton = false;
            this.crystalReportViewer1.ShowExportButton = false;
            this.crystalReportViewer1.ShowGotoPageButton = false;
            this.crystalReportViewer1.ShowGroupTreeButton = false;
            this.crystalReportViewer1.ShowPageNavigateButtons = false;
            this.crystalReportViewer1.ShowParameterPanelButton = false;
            this.crystalReportViewer1.ShowPrintButton = false;
            this.crystalReportViewer1.ShowRefreshButton = false;
            this.crystalReportViewer1.ShowTextSearchButton = false;
            this.crystalReportViewer1.ShowZoomButton = false;
            this.crystalReportViewer1.Size = new System.Drawing.Size(680, 316);
            this.crystalReportViewer1.TabIndex = 6;
            this.crystalReportViewer1.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            this.crystalReportViewer1.ToolPanelWidth = 0;
            this.crystalReportViewer1.ViewTimeSelectionFormula = "";
            this.crystalReportViewer1.EnabledChanged += new System.EventHandler(this.crystalReportViewer1_EnabledChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.listatoolstrip);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(680, 25);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(541, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Zoom:";
            // 
            // listatoolstrip
            // 
            this.listatoolstrip.Dock = System.Windows.Forms.DockStyle.None;
            this.listatoolstrip.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listatoolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.elsolap,
            this.elozo,
            this.kovetkezo,
            this.utsolap,
            this.nyomtat,
            this.adatkiv});
            this.listatoolstrip.Location = new System.Drawing.Point(0, 0);
            this.listatoolstrip.Name = "listatoolstrip";
            this.listatoolstrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.listatoolstrip.Size = new System.Drawing.Size(524, 25);
            this.listatoolstrip.TabIndex = 1;
            this.listatoolstrip.Text = "toolStrip1";
            // 
            // elsolap
            // 
            this.elsolap.Image = global::FakPlusz.Properties.Resources.ele;
            this.elsolap.ImageTransparentColor = System.Drawing.Color.Black;
            this.elsolap.Name = "elsolap";
            this.elsolap.Size = new System.Drawing.Size(67, 22);
            this.elsolap.Text = "Elsö lap";
            this.elsolap.Click += new System.EventHandler(this.Buttonok_Click);
            // 
            // elozo
            // 
            this.elozo.Image = global::FakPlusz.Properties.Resources.elözö;
            this.elozo.ImageTransparentColor = System.Drawing.Color.Black;
            this.elozo.Name = "elozo";
            this.elozo.Size = new System.Drawing.Size(74, 22);
            this.elozo.Text = "Elözö lap";
            this.elozo.Click += new System.EventHandler(this.Buttonok_Click);
            // 
            // kovetkezo
            // 
            this.kovetkezo.Image = global::FakPlusz.Properties.Resources.következö;
            this.kovetkezo.ImageTransparentColor = System.Drawing.Color.Black;
            this.kovetkezo.Name = "kovetkezo";
            this.kovetkezo.Size = new System.Drawing.Size(103, 22);
            this.kovetkezo.Text = "Következö lap";
            this.kovetkezo.Click += new System.EventHandler(this.Buttonok_Click);
            // 
            // utsolap
            // 
            this.utsolap.Image = global::FakPlusz.Properties.Resources.moge;
            this.utsolap.ImageTransparentColor = System.Drawing.Color.Black;
            this.utsolap.Name = "utsolap";
            this.utsolap.Size = new System.Drawing.Size(80, 22);
            this.utsolap.Text = "Utolsó lap";
            this.utsolap.Click += new System.EventHandler(this.Buttonok_Click);
            // 
            // nyomtat
            // 
            this.nyomtat.Image = global::FakPlusz.Properties.Resources.Print;
            this.nyomtat.ImageTransparentColor = System.Drawing.Color.Black;
            this.nyomtat.Name = "nyomtat";
            this.nyomtat.Size = new System.Drawing.Size(74, 22);
            this.nyomtat.Text = "Nyomtat";
            this.nyomtat.Click += new System.EventHandler(this.Buttonok_Click);
            // 
            // adatkiv
            // 
            this.adatkiv.Image = global::FakPlusz.Properties.Resources.uj;
            this.adatkiv.ImageTransparentColor = System.Drawing.Color.Black;
            this.adatkiv.Name = "adatkiv";
            this.adatkiv.Size = new System.Drawing.Size(114, 22);
            this.adatkiv.Text = "Adatszolgáltatás";
            this.adatkiv.Click += new System.EventHandler(this.Buttonok_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.nyomtmax);
            this.panel2.Controls.Add(this.nyomtmin);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.statusStrip1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 341);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(680, 35);
            this.panel2.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(560, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(18, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "-ig";
            this.label4.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(469, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(21, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "-tól";
            this.label3.Visible = false;
            // 
            // nyomtmax
            // 
            this.nyomtmax.FormattingEnabled = true;
            this.nyomtmax.Location = new System.Drawing.Point(507, 5);
            this.nyomtmax.Name = "nyomtmax";
            this.nyomtmax.Size = new System.Drawing.Size(47, 21);
            this.nyomtmax.TabIndex = 5;
            this.nyomtmax.Visible = false;
            // 
            // nyomtmin
            // 
            this.nyomtmin.FormattingEnabled = true;
            this.nyomtmin.Location = new System.Drawing.Point(416, 3);
            this.nyomtmin.Name = "nyomtmin";
            this.nyomtmin.Size = new System.Drawing.Size(47, 21);
            this.nyomtmin.TabIndex = 4;
            this.nyomtmin.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(308, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Nyomtatandó lapok:";
            this.label2.Visible = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lapszam,
            this.maxlapszam});
            this.statusStrip1.Location = new System.Drawing.Point(0, 5);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(151, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lapszam
            // 
            this.lapszam.Name = "lapszam";
            this.lapszam.Size = new System.Drawing.Size(56, 17);
            this.lapszam.Text = "Lapszám:";
            // 
            // maxlapszam
            // 
            this.maxlapszam.Name = "maxlapszam";
            this.maxlapszam.Size = new System.Drawing.Size(78, 17);
            this.maxlapszam.Text = "Lapok száma:";
            // 
            // printDialog1
            // 
            this.printDialog1.UseEXDialog = true;
            // 
            // Alaplista
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Name = "Alaplista";
            this.Size = new System.Drawing.Size(680, 376);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.comboBox1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.AlapTablaView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EgyszeruTablaView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OsszetettKozepsoTablaView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OsszetettAlsoTablaView)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.listatoolstrip.ResumeLayout(false);
            this.listatoolstrip.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip listatoolstrip;
        /// <summary>
        /// 
        /// </summary>
        public System.Windows.Forms.ToolStripButton elsolap;
        /// <summary>
        /// 
        /// </summary>
        public System.Windows.Forms.ToolStripButton elozo;
        /// <summary>
        /// 
        /// </summary>
        public System.Windows.Forms.ToolStripButton kovetkezo;
        /// <summary>
        /// 
        /// </summary>
        public System.Windows.Forms.ToolStripButton utsolap;
        private System.Windows.Forms.ToolStripButton nyomtat;
        /// <summary>
        /// 
        /// </summary>
        public System.Windows.Forms.ToolStripButton adatkiv;
        private System.Windows.Forms.Panel panel3;
        /// <summary>
        /// 
        /// </summary>
        public CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        /// <summary>
        /// 
        /// </summary>
        public System.Windows.Forms.ComboBox nyomtmax;
        /// <summary>
        /// 
        /// </summary>
        public System.Windows.Forms.ComboBox nyomtmin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lapszam;
        private System.Windows.Forms.ToolStripStatusLabel maxlapszam;
 //       private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.PrintDialog printDialog1;



    }
}
