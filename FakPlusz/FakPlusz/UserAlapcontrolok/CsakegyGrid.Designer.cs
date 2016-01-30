namespace FakPlusz.UserAlapcontrolok
{
    partial class CsakegyGrid
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.ok = new System.Windows.Forms.ToolStripButton();
            this.rogzit = new System.Windows.Forms.ToolStripButton();
            this.torol = new System.Windows.Forms.ToolStripButton();
            this.elolrol = new System.Windows.Forms.ToolStripButton();
            this.groupBox112 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.groupBox112.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Controls.Add(this.groupBox112);
            this.panel1.Location = new System.Drawing.Point(3, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(671, 166);
            this.panel1.TabIndex = 14;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ok,
            this.rogzit,
            this.torol,
            this.elolrol});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(671, 25);
            this.toolStrip1.TabIndex = 10;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // ok
            // 
            this.ok.Image = global::FakPlusz.Properties.Resources.ok;
            this.ok.ImageTransparentColor = System.Drawing.Color.Black;
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(43, 22);
            this.ok.Text = "OK";
            // 
            // rogzit
            // 
            this.rogzit.Image = global::FakPlusz.Properties.Resources.rogzit;
            this.rogzit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.rogzit.Name = "rogzit";
            this.rogzit.Size = new System.Drawing.Size(60, 22);
            this.rogzit.Text = "Rögzit";
            // 
            // torol
            // 
            this.torol.Image = global::FakPlusz.Properties.Resources.töröl;
            this.torol.ImageTransparentColor = System.Drawing.Color.Black;
            this.torol.Name = "torol";
            this.torol.Size = new System.Drawing.Size(55, 22);
            this.torol.Text = "Töröl";
            // 
            // elolrol
            // 
            this.elolrol.Image = global::FakPlusz.Properties.Resources.elolrol;
            this.elolrol.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.elolrol.Name = "elolrol";
            this.elolrol.Size = new System.Drawing.Size(58, 22);
            this.elolrol.Text = "Elölröl";
            // 
            // groupBox112
            // 
            this.groupBox112.BackColor = System.Drawing.SystemColors.ControlLight;
            this.groupBox112.Controls.Add(this.dataGridView1);
            this.groupBox112.Location = new System.Drawing.Point(3, 28);
            this.groupBox112.Name = "groupBox112";
            this.groupBox112.Size = new System.Drawing.Size(636, 121);
            this.groupBox112.TabIndex = 9;
            this.groupBox112.TabStop = false;
            this.groupBox112.Tag = "E,ELTARTOTTAK";
            this.groupBox112.Text = "Eltartott";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 17);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(630, 101);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Location = new System.Drawing.Point(3, 42);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(772, 419);
            this.panel2.TabIndex = 15;
            // 
            // CsakegyGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Name = "CsakegyGrid";
            this.Size = new System.Drawing.Size(786, 464);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox112.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        /// <summary>
        /// 
        /// </summary>
        public System.Windows.Forms.Panel panel1;
        /// <summary>
        /// 
        /// </summary>
        public System.Windows.Forms.GroupBox groupBox112;
        /// <summary>
        /// 
        /// </summary>
        public System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        /// <summary>
        /// 
        /// </summary>
        public System.Windows.Forms.ToolStripButton ok;
        /// <summary>
        /// 
        /// </summary>
        public System.Windows.Forms.ToolStripButton rogzit;
        /// <summary>
        /// 
        /// </summary>
        public System.Windows.Forms.ToolStripButton elolrol;
        /// <summary>
        /// 
        /// </summary>
        public System.Windows.Forms.ToolStripButton torol;
        /// <summary>
        /// 
        /// </summary>
        public System.Windows.Forms.Panel panel2;
    }
}
