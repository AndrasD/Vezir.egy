namespace FakPlusz.SzerkesztettListak
{
    partial class Altlistazoalap
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.crystalReportViewer1 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.nyomtmax = new System.Windows.Forms.ComboBox();
            this.nyomtmin = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lapszam = new System.Windows.Forms.ToolStripStatusLabel();
            this.maxlapszam = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.elsolap = new System.Windows.Forms.ToolStripButton();
            this.elozo = new System.Windows.Forms.ToolStripButton();
            this.kovetkezo = new System.Windows.Forms.ToolStripButton();
            this.utsolap = new System.Windows.Forms.ToolStripButton();
            this.nyomtat = new System.Windows.Forms.ToolStripButton();
            this.panel5 = new System.Windows.Forms.Panel();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.parameterview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listaparametertabla)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statisztikaparametertabla)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.parametertabla)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.felteteltabla)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sorfelteteltabla)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.oszlopfelteteltabla)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.feltetelview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.feltetelsview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.felteteloview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AlapTablaView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EgyszeruTablaView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OsszetettKozepsoTablaView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OsszetettAlsoTablaView)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // kellenetreeview
            // 
            this.kellenetreeview.LineColor = System.Drawing.Color.Black;
            // 
            // comboBox1
            // 
            this.comboBox1.Location = new System.Drawing.Point(150, 97);
            this.comboBox1.Size = new System.Drawing.Size(297, 24);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.SelectionChangeCommitted += new System.EventHandler(this.comboBox1_SelectionChangeCommitted);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(704, 503);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControl1_Selecting);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dateTimePicker2);
            this.tabPage1.Controls.Add(this.dateTimePicker1);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.comboBox1);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(696, 475);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Paraméterezés";
            this.tabPage1.UseVisualStyleBackColor = true;
            this.tabPage1.Controls.SetChildIndex(this.label1, 0);
            this.tabPage1.Controls.SetChildIndex(this.comboBox1, 0);
            this.tabPage1.Controls.SetChildIndex(this.label6, 0);
            this.tabPage1.Controls.SetChildIndex(this.label7, 0);
            this.tabPage1.Controls.SetChildIndex(this.label8, 0);
            this.tabPage1.Controls.SetChildIndex(this.dateTimePicker1, 0);
            this.tabPage1.Controls.SetChildIndex(this.dateTimePicker2, 0);
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.CustomFormat = "yyyy.MMMMMMMMMMM.dd";
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker2.Location = new System.Drawing.Point(315, 175);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(159, 22);
            this.dateTimePicker2.TabIndex = 8;
            this.dateTimePicker2.Tag = "0";
            this.dateTimePicker2.CloseUp += new System.EventHandler(this.dateTimePicker_ValueChanged);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "yyyy.MMMMMMMMMMMM.dd";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(118, 175);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(159, 22);
            this.dateTimePicker1.TabIndex = 7;
            this.dateTimePicker1.Tag = "0";
            this.dateTimePicker1.CloseUp += new System.EventHandler(this.dateTimePicker_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(480, 180);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(23, 16);
            this.label8.TabIndex = 6;
            this.label8.Text = "-ig";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(283, 180);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(26, 16);
            this.label7.TabIndex = 5;
            this.label7.Text = "-tól";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(222, 78);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(159, 16);
            this.label6.TabIndex = 4;
            this.label6.Text = "Válassza ki a kivánt listát:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(193, 148);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(224, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Kivánt dátum vagy dátumintervallum:";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel3);
            this.tabPage2.Controls.Add(this.panel2);
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(696, 475);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Eredmény";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.crystalReportViewer1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 32);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(690, 413);
            this.panel3.TabIndex = 6;
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
            this.crystalReportViewer1.Location = new System.Drawing.Point(0, 0);
            this.crystalReportViewer1.Name = "crystalReportViewer1";
            this.crystalReportViewer1.SelectionFormula = "";
            this.crystalReportViewer1.Size = new System.Drawing.Size(690, 413);
            this.crystalReportViewer1.TabIndex = 0;
            this.crystalReportViewer1.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            this.crystalReportViewer1.ViewTimeSelectionFormula = "";
            this.crystalReportViewer1.EnabledChanged += new System.EventHandler(this.crystalReportViewer1_EnabledChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.nyomtmax);
            this.panel2.Controls.Add(this.nyomtmin);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.statusStrip1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(3, 445);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(690, 27);
            this.panel2.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(506, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(21, 15);
            this.label4.TabIndex = 7;
            this.label4.Text = "-ig";
            this.label4.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(409, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(24, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "-tól";
            this.label3.Visible = false;
            // 
            // nyomtmax
            // 
            this.nyomtmax.FormattingEnabled = true;
            this.nyomtmax.Location = new System.Drawing.Point(439, 3);
            this.nyomtmax.Name = "nyomtmax";
            this.nyomtmax.Size = new System.Drawing.Size(47, 23);
            this.nyomtmax.TabIndex = 5;
            this.nyomtmax.Visible = false;
            this.nyomtmax.SelectionChangeCommitted += new System.EventHandler(this.nyomt_SelectionChangeCommitted);
            // 
            // nyomtmin
            // 
            this.nyomtmin.FormattingEnabled = true;
            this.nyomtmin.Location = new System.Drawing.Point(356, 3);
            this.nyomtmin.Name = "nyomtmin";
            this.nyomtmin.Size = new System.Drawing.Size(47, 23);
            this.nyomtmin.TabIndex = 4;
            this.nyomtmin.Visible = false;
            this.nyomtmin.SelectionChangeCommitted += new System.EventHandler(this.nyomt_SelectionChangeCommitted);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(234, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(116, 15);
            this.label5.TabIndex = 3;
            this.label5.Text = "Nyomtatandó lapok:";
            this.label5.Visible = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lapszam,
            this.maxlapszam});
            this.statusStrip1.Location = new System.Drawing.Point(0, 5);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(182, 22);
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
            // panel1
            // 
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(690, 29);
            this.panel1.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.elsolap,
            this.elozo,
            this.kovetkezo,
            this.utsolap,
            this.nyomtat});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolStrip1.Location = new System.Drawing.Point(4, 3);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(399, 23);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // elsolap
            // 
            this.elsolap.Image = global::FakPlusz.Properties.Resources.ele;
            this.elsolap.ImageTransparentColor = System.Drawing.Color.Black;
            this.elsolap.Name = "elsolap";
            this.elsolap.Size = new System.Drawing.Size(67, 20);
            this.elsolap.Text = "Elsö lap";
            this.elsolap.Click += new System.EventHandler(this.Buttonok_Click);
            // 
            // elozo
            // 
            this.elozo.Image = global::FakPlusz.Properties.Resources.elözö;
            this.elozo.ImageTransparentColor = System.Drawing.Color.Black;
            this.elozo.Name = "elozo";
            this.elozo.Size = new System.Drawing.Size(74, 20);
            this.elozo.Text = "Elözö lap";
            this.elozo.Click += new System.EventHandler(this.Buttonok_Click);
            // 
            // kovetkezo
            // 
            this.kovetkezo.Image = global::FakPlusz.Properties.Resources.következö;
            this.kovetkezo.ImageTransparentColor = System.Drawing.Color.Black;
            this.kovetkezo.Name = "kovetkezo";
            this.kovetkezo.Size = new System.Drawing.Size(103, 20);
            this.kovetkezo.Text = "Következö lap";
            this.kovetkezo.Click += new System.EventHandler(this.Buttonok_Click);
            // 
            // utsolap
            // 
            this.utsolap.Image = global::FakPlusz.Properties.Resources.moge;
            this.utsolap.ImageTransparentColor = System.Drawing.Color.Black;
            this.utsolap.Name = "utsolap";
            this.utsolap.Size = new System.Drawing.Size(80, 20);
            this.utsolap.Text = "Utolsó lap";
            this.utsolap.Click += new System.EventHandler(this.Buttonok_Click);
            // 
            // nyomtat
            // 
            this.nyomtat.Image = global::FakPlusz.Properties.Resources.Print;
            this.nyomtat.ImageTransparentColor = System.Drawing.Color.Black;
            this.nyomtat.Name = "nyomtat";
            this.nyomtat.Size = new System.Drawing.Size(74, 20);
            this.nyomtat.Text = "Nyomtat";
            this.nyomtat.Click += new System.EventHandler(this.Buttonok_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.comboBox2);
            this.panel5.Controls.Add(this.label2);
            this.panel5.Location = new System.Drawing.Point(468, -3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(219, 32);
            this.panel5.TabIndex = 7;
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "  25",
            "  50",
            "  75",
            "100",
            "200",
            "500"});
            this.comboBox2.Location = new System.Drawing.Point(75, 6);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(83, 23);
            this.comboBox2.TabIndex = 4;
            this.comboBox2.SelectionChangeCommitted += new System.EventHandler(this.zoom_SelectionChangeCommitted);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Zoom:";
            // 
            // Altlistazoalap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "Altlistazoalap";
            this.Size = new System.Drawing.Size(704, 503);
            ((System.ComponentModel.ISupportInitialize)(this.parameterview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listaparametertabla)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statisztikaparametertabla)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.parametertabla)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.felteteltabla)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sorfelteteltabla)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.oszlopfelteteltabla)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.feltetelview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.feltetelsview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.felteteloview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AlapTablaView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EgyszeruTablaView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OsszetettKozepsoTablaView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OsszetettAlsoTablaView)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox nyomtmax;
        private System.Windows.Forms.ComboBox nyomtmin;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolStripStatusLabel lapszam;
        private System.Windows.Forms.ToolStripStatusLabel maxlapszam;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton elsolap;
        private System.Windows.Forms.ToolStripButton elozo;
        private System.Windows.Forms.ToolStripButton kovetkezo;
        private System.Windows.Forms.ToolStripButton utsolap;
 //       private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label6;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.ToolStripButton nyomtat;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;

    }
}
