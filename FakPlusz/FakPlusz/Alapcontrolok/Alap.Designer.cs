namespace FakPlusz.Alapcontrolok
{
    partial class Alap
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.eleszur = new System.Windows.Forms.ToolStripButton();
            this.mogeszur = new System.Windows.Forms.ToolStripButton();
            this.torolalap = new System.Windows.Forms.ToolStripButton();
            this.rogzit = new System.Windows.Forms.ToolStripButton();
            this.uj = new System.Windows.Forms.ToolStripButton();
            this.elozoverzio = new System.Windows.Forms.ToolStripButton();
            this.kovetkezoverzio = new System.Windows.Forms.ToolStripButton();
            this.teljestorles = new System.Windows.Forms.ToolStripButton();
            this.elolrolalap = new System.Windows.Forms.ToolStripButton();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.AlapTablaView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EgyszeruTablaView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OsszetettKozepsoTablaView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OsszetettAlsoTablaView)).BeginInit();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.Size = new System.Drawing.Size(121, 23);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.eleszur,
            this.mogeszur,
            this.torolalap,
            this.rogzit,
            this.uj,
            this.elozoverzio,
            this.kovetkezoverzio,
            this.teljestorles,
            this.elolrolalap});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(1079, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // eleszur
            // 
            this.eleszur.Image = global::FakPlusz.Properties.Resources.ele;
            this.eleszur.ImageTransparentColor = System.Drawing.Color.Black;
            this.eleszur.Name = "eleszur";
            this.eleszur.Size = new System.Drawing.Size(65, 22);
            this.eleszur.Text = "Sor elé";
            this.eleszur.Click += new System.EventHandler(this.eleszur_Click);
            // 
            // mogeszur
            // 
            this.mogeszur.Image = global::FakPlusz.Properties.Resources.moge;
            this.mogeszur.ImageTransparentColor = System.Drawing.Color.Black;
            this.mogeszur.Name = "mogeszur";
            this.mogeszur.Size = new System.Drawing.Size(80, 22);
            this.mogeszur.Text = "Sor mögé";
            this.mogeszur.Click += new System.EventHandler(this.mogeszur_Click);
            // 
            // torolalap
            // 
            this.torolalap.Image = global::FakPlusz.Properties.Resources.töröl;
            this.torolalap.ImageTransparentColor = System.Drawing.Color.Black;
            this.torolalap.Name = "torolalap";
            this.torolalap.Size = new System.Drawing.Size(60, 22);
            this.torolalap.Text = "Törlés";
            this.torolalap.Click += new System.EventHandler(this.torolalap_Click);
            // 
            // rogzit
            // 
            this.rogzit.Image = global::FakPlusz.Properties.Resources.rogzit;
            this.rogzit.ImageTransparentColor = System.Drawing.Color.Black;
            this.rogzit.Name = "rogzit";
            this.rogzit.Size = new System.Drawing.Size(60, 22);
            this.rogzit.Text = "Rögzit";
            this.rogzit.Click += new System.EventHandler(this.rogzit_Click);
            // 
            // uj
            // 
            this.uj.Image = global::FakPlusz.Properties.Resources.uj;
            this.uj.ImageTransparentColor = System.Drawing.Color.Black;
            this.uj.Name = "uj";
            this.uj.Size = new System.Drawing.Size(73, 22);
            this.uj.Text = "Új verzió";
            this.uj.Click += new System.EventHandler(this.uj_Click);
            // 
            // elozoverzio
            // 
            this.elozoverzio.Image = global::FakPlusz.Properties.Resources.elözö;
            this.elozoverzio.ImageTransparentColor = System.Drawing.Color.Black;
            this.elozoverzio.Name = "elozoverzio";
            this.elozoverzio.Size = new System.Drawing.Size(90, 22);
            this.elozoverzio.Text = "Elözö verzió";
            this.elozoverzio.Click += new System.EventHandler(this.elozoverzio_Click);
            // 
            // kovetkezoverzio
            // 
            this.kovetkezoverzio.Image = global::FakPlusz.Properties.Resources.következö;
            this.kovetkezoverzio.ImageTransparentColor = System.Drawing.Color.Black;
            this.kovetkezoverzio.Name = "kovetkezoverzio";
            this.kovetkezoverzio.Size = new System.Drawing.Size(119, 22);
            this.kovetkezoverzio.Text = "Következö verzió";
            this.kovetkezoverzio.Click += new System.EventHandler(this.kovetkezoverzio_Click);
            // 
            // teljestorles
            // 
            this.teljestorles.BackColor = System.Drawing.SystemColors.Control;
            this.teljestorles.ForeColor = System.Drawing.SystemColors.ControlText;
            this.teljestorles.Image = global::FakPlusz.Properties.Resources.eldob_16x16;
            this.teljestorles.ImageTransparentColor = System.Drawing.Color.Red;
            this.teljestorles.Name = "teljestorles";
            this.teljestorles.Size = new System.Drawing.Size(137, 22);
            this.teljestorles.Text = "Utolsó verzió törlése";
            this.teljestorles.Click += new System.EventHandler(this.teljestorles_Click);
            // 
            // elolrolalap
            // 
            this.elolrolalap.Image = global::FakPlusz.Properties.Resources.elolrol;
            this.elolrolalap.ImageTransparentColor = System.Drawing.Color.Black;
            this.elolrolalap.Name = "elolrolalap";
            this.elolrolalap.Size = new System.Drawing.Size(58, 22);
            this.elolrolalap.Text = "Elölröl";
            this.elolrolalap.Click += new System.EventHandler(this.elolrolalap_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1079, 872);
            this.dataGridView1.TabIndex = 3;
            this.dataGridView1.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView1_CellBeginEdit);
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            this.dataGridView1.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseClick);
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            this.dataGridView1.VisibleChanged += new System.EventHandler(this.dataGridView1_VisibleChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1079, 897);
            this.panel1.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 25);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1079, 872);
            this.panel2.TabIndex = 1;
            // 
            // Alap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Alap";
            this.Size = new System.Drawing.Size(1079, 900);
            this.Load += new System.EventHandler(this.Alap_Load);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.comboBox1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.AlapTablaView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EgyszeruTablaView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OsszetettKozepsoTablaView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OsszetettAlsoTablaView)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        /// <summary>
        /// ToolStrip a tablat erinto valtozasokhoz
        /// </summary>
        public System.Windows.Forms.ToolStrip toolStrip1;
        /// <summary>
        /// A tabla/vagy leirotabla informacioinak megjelenitesere
        /// </summary>
        public System.Windows.Forms.DataGridView dataGridView1;
        /// <summary>
        /// Ez tartalmazza a ToolStripet es a panel2-t
        /// </summary>
        public System.Windows.Forms.Panel panel1;
        /// <summary>
        /// Ez tartalmazza a dataGridView-t
        /// </summary>
        public System.Windows.Forms.Panel panel2;
        /// <summary>
        /// Sor ele uj sor buttonja
        /// </summary>
        public System.Windows.Forms.ToolStripButton eleszur;
        /// <summary>
        /// Sor moge uj sor buttonja
        /// </summary>
        public System.Windows.Forms.ToolStripButton mogeszur;
        /// <summary>
        /// Rogzit button
        /// </summary>
        public System.Windows.Forms.ToolStripButton rogzit;
        /// <summary>
        /// Uj verzio buttonja
        /// </summary>
        public System.Windows.Forms.ToolStripButton uj;
        /// <summary>
        /// Utolso verzio torlesenek buttonja
        /// </summary>
        public System.Windows.Forms.ToolStripButton teljestorles;
        /// <summary>
        /// Adatbazisbeli allapot visszaallitasanak buttonja
        /// </summary>
        public System.Windows.Forms.ToolStripButton elolrolalap;
        /// <summary>
        /// Sor torlesenek buttonja
        /// </summary>
        public System.Windows.Forms.ToolStripButton torolalap;
        /// <summary>
        /// elozo verzio keresenek buttonja
        /// </summary>
        public System.Windows.Forms.ToolStripButton elozoverzio;
        /// <summary>
        /// kovetkezo verzio keresenek buttonja
        /// </summary>
        public System.Windows.Forms.ToolStripButton kovetkezoverzio;
    }

}
