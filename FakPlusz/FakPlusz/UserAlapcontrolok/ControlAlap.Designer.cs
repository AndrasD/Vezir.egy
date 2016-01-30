namespace FakPlusz.UserAlapcontrolok
{
    partial class ControlAlap
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
            this.toolStripfo = new System.Windows.Forms.ToolStrip();
            this.teljesrogzit = new System.Windows.Forms.ToolStripButton();
            this.teljeselolrol = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.AlapTablaView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EgyszeruTablaView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OsszetettKozepsoTablaView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OsszetettAlsoTablaView)).BeginInit();
            this.toolStripfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.Size = new System.Drawing.Size(121, 23);
            // 
            // toolStripfo
            // 
            this.toolStripfo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripfo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.teljesrogzit,
            this.teljeselolrol});
            this.toolStripfo.Location = new System.Drawing.Point(0, 0);
            this.toolStripfo.Name = "toolStripfo";
            this.toolStripfo.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStripfo.Size = new System.Drawing.Size(844, 25);
            this.toolStripfo.TabIndex = 0;
            this.toolStripfo.Text = "toolStripfo";
            // 
            // teljesrogzit
            // 
            this.teljesrogzit.Image = global::FakPlusz.Properties.Resources.rogzit;
            this.teljesrogzit.ImageTransparentColor = System.Drawing.Color.Black;
            this.teljesrogzit.Name = "teljesrogzit";
            this.teljesrogzit.Size = new System.Drawing.Size(125, 22);
            this.teljesrogzit.Text = "Munkaasztal rögzit";
            this.teljesrogzit.Click += new System.EventHandler(this.teljesrogzit_Click);
            // 
            // teljeselolrol
            // 
            this.teljeselolrol.Image = global::FakPlusz.Properties.Resources.elolrol;
            this.teljeselolrol.ImageTransparentColor = System.Drawing.Color.Black;
            this.teljeselolrol.Name = "teljeselolrol";
            this.teljeselolrol.Size = new System.Drawing.Size(126, 22);
            this.teljeselolrol.Text = "Munkaasztal elölröl";
            this.teljeselolrol.Click += new System.EventHandler(this.teljeselolrol_Click);
            // 
            // ControlAlap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStripfo);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ControlAlap";
            this.Size = new System.Drawing.Size(844, 449);
            this.Controls.SetChildIndex(this.toolStripfo, 0);
            this.Controls.SetChildIndex(this.comboBox1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.AlapTablaView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EgyszeruTablaView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OsszetettKozepsoTablaView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OsszetettAlsoTablaView)).EndInit();
            this.toolStripfo.ResumeLayout(false);
            this.toolStripfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        /// <summary>
        /// Munkaasztal rogzit
        /// </summary>
        public System.Windows.Forms.ToolStripButton teljesrogzit;
        /// <summary>
        /// Toolstrip
        /// </summary>
        public System.Windows.Forms.ToolStrip toolStripfo;
        public System.Windows.Forms.ToolStripButton teljeselolrol;
    }
}
