namespace FakPlusz.VezerloFormok
{
    partial class TervTreeView
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
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.kinyit = new System.Windows.Forms.ToolStripMenuItem();
            this.becsuk = new System.Windows.Forms.ToolStripMenuItem();
            this.kelltooltip = new System.Windows.Forms.ToolStripMenuItem();
            this.nemkelltooltip = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.Alap = new System.Windows.Forms.ToolStripMenuItem();
            this.Tabla = new System.Windows.Forms.ToolStripMenuItem();
            this.Leiroalap = new System.Windows.Forms.ToolStripMenuItem();
            this.Leirotabla = new System.Windows.Forms.ToolStripMenuItem();
            this.Alaptooltip = new System.Windows.Forms.ToolStripMenuItem();
            this.Tablatooltip = new System.Windows.Forms.ToolStripMenuItem();
            this.Leiroalaptooltip = new System.Windows.Forms.ToolStripMenuItem();
            this.Leirotablatooltip = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.AlapTablaView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EgyszeruTablaView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OsszetettKozepsoTablaView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OsszetettAlsoTablaView)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.contextMenuStrip1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.kinyit,
            this.becsuk,
            this.kelltooltip,
            this.nemkelltooltip});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenuStrip1.Size = new System.Drawing.Size(205, 114);
            // 
            // kinyit
            // 
            this.kinyit.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.kinyit.Image = global::FakPlusz.Properties.Resources.kinyit;
            this.kinyit.ImageTransparentColor = System.Drawing.Color.Black;
            this.kinyit.Name = "kinyit";
            this.kinyit.Size = new System.Drawing.Size(204, 22);
            this.kinyit.Text = "Kinyit";
            this.kinyit.Click += new System.EventHandler(this.kinyit_Click);
            // 
            // becsuk
            // 
            this.becsuk.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.becsuk.Image = global::FakPlusz.Properties.Resources.becsuk;
            this.becsuk.ImageTransparentColor = System.Drawing.Color.Black;
            this.becsuk.Name = "becsuk";
            this.becsuk.Size = new System.Drawing.Size(204, 22);
            this.becsuk.Text = "Becsuk";
            this.becsuk.Click += new System.EventHandler(this.becsuk_Click);
            // 
            // kelltooltip
            // 
            this.kelltooltip.Image = global::FakPlusz.Properties.Resources.MultiplePagesImage;
            this.kelltooltip.Name = "kelltooltip";
            this.kelltooltip.Size = new System.Drawing.Size(204, 22);
            this.kelltooltip.Text = "Magyarázat látható";
            this.kelltooltip.Click += new System.EventHandler(this.kelltooltip_Click);
            // 
            // nemkelltooltip
            // 
            this.nemkelltooltip.Image = global::FakPlusz.Properties.Resources.RegisterClosed;
            this.nemkelltooltip.Name = "nemkelltooltip";
            this.nemkelltooltip.Size = new System.Drawing.Size(204, 22);
            this.nemkelltooltip.Text = "Magyarázat nem látható";
            this.nemkelltooltip.Click += new System.EventHandler(this.nemkelltooltip_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Alap,
            this.Leiroalap,
            this.Alaptooltip,
            this.Leiroalaptooltip});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(843, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // Alap
            // 
            this.Alap.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Tabla});
            this.Alap.Name = "Alap";
            this.Alap.Size = new System.Drawing.Size(48, 20);
            this.Alap.Text = "Tábla";
            // 
            // Tabla
            // 
            this.Tabla.Name = "Tabla";
            this.Tabla.Size = new System.Drawing.Size(103, 22);
            this.Tabla.Text = "Tábla";
            this.Tabla.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.DropDownItem_Clicked);
            // 
            // Leiroalap
            // 
            this.Leiroalap.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Leirotabla});
            this.Leiroalap.Name = "Leiroalap";
            this.Leiroalap.Size = new System.Drawing.Size(71, 20);
            this.Leiroalap.Text = "Leirótábla";
            this.Leiroalap.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.DropDownItem_Clicked);
            // 
            // Leirotabla
            // 
            this.Leirotabla.Name = "Leirotabla";
            this.Leirotabla.Size = new System.Drawing.Size(126, 22);
            this.Leirotabla.Text = "Leirótábla";
            // 
            // Alaptooltip
            // 
            this.Alaptooltip.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Tablatooltip});
            this.Alaptooltip.Name = "Alaptooltip";
            this.Alaptooltip.Size = new System.Drawing.Size(103, 20);
            this.Alaptooltip.Text = "Tábla Tooltipek";
            this.Alaptooltip.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.DropDownItem_Clicked);
            // 
            // Tablatooltip
            // 
            this.Tablatooltip.Name = "Tablatooltip";
            this.Tablatooltip.Size = new System.Drawing.Size(158, 22);
            this.Tablatooltip.Text = "Tábla Tooltipek";
            // 
            // Leiroalaptooltip
            // 
            this.Leiroalaptooltip.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Leirotablatooltip});
            this.Leiroalaptooltip.Name = "Leiroalaptooltip";
            this.Leiroalaptooltip.Size = new System.Drawing.Size(126, 20);
            this.Leiroalaptooltip.Text = "Leirótábla Tooltipek";
            this.Leiroalaptooltip.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.DropDownItem_Clicked);
            // 
            // Leirotablatooltip
            // 
            this.Leirotablatooltip.Name = "Leirotablatooltip";
            this.Leirotablatooltip.Size = new System.Drawing.Size(181, 22);
            this.Leirotablatooltip.Text = "Leirótábla Tooltipek";
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(843, 403);
            this.panel1.TabIndex = 2;
            // 
            // TervTreeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "TervTreeView";
            this.Size = new System.Drawing.Size(843, 427);
            this.Controls.SetChildIndex(this.menuStrip1, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.comboBox1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.AlapTablaView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EgyszeruTablaView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OsszetettKozepsoTablaView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OsszetettAlsoTablaView)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem kinyit;
        private System.Windows.Forms.ToolStripMenuItem becsuk;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem Alap;
        private System.Windows.Forms.ToolStripMenuItem Leiroalap;
        private System.Windows.Forms.ToolStripMenuItem Alaptooltip;
        private System.Windows.Forms.ToolStripMenuItem Leiroalaptooltip;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem Tabla;
        private System.Windows.Forms.ToolStripMenuItem Leirotabla;
        private System.Windows.Forms.ToolStripMenuItem Tablatooltip;
        private System.Windows.Forms.ToolStripMenuItem Leirotablatooltip;
        private System.Windows.Forms.ToolStripMenuItem kelltooltip;
        private System.Windows.Forms.ToolStripMenuItem nemkelltooltip;
    }
}
