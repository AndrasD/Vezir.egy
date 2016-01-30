namespace Vezir
{
    partial class Szerklist
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
            this.valasztopage = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.Valasztek = new System.Windows.Forms.ComboBox();
            this.listapage = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.AlapTablaView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EgyszeruTablaView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OsszetettKozepsoTablaView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OsszetettAlsoTablaView)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.valasztopage.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.Location = new System.Drawing.Point(415, 51);
            this.comboBox1.Size = new System.Drawing.Size(27, 21);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.valasztopage);
            this.tabControl1.Controls.Add(this.listapage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(904, 470);
            this.tabControl1.TabIndex = 1;
            // 
            // valasztopage
            // 
            this.valasztopage.Controls.Add(this.label1);
            this.valasztopage.Controls.Add(this.Valasztek);
            this.valasztopage.Location = new System.Drawing.Point(4, 22);
            this.valasztopage.Name = "valasztopage";
            this.valasztopage.Padding = new System.Windows.Forms.Padding(3);
            this.valasztopage.Size = new System.Drawing.Size(896, 444);
            this.valasztopage.TabIndex = 0;
            this.valasztopage.Text = "Lista választás";
            this.valasztopage.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(277, 91);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Válassza ki a kivánt listát: ";
            // 
            // Valasztek
            // 
            this.Valasztek.FormattingEnabled = true;
            this.Valasztek.Location = new System.Drawing.Point(308, 117);
            this.Valasztek.Name = "Valasztek";
            this.Valasztek.Size = new System.Drawing.Size(274, 21);
            this.Valasztek.TabIndex = 0;
            this.Valasztek.DropDownClosed += new System.EventHandler(this.Valasztek_SelectedIndexChanged);
            // 
            // listapage
            // 
            this.listapage.Location = new System.Drawing.Point(4, 22);
            this.listapage.Name = "listapage";
            this.listapage.Padding = new System.Windows.Forms.Padding(3);
            this.listapage.Size = new System.Drawing.Size(896, 444);
            this.listapage.TabIndex = 1;
            this.listapage.Text = "Lista";
            this.listapage.UseVisualStyleBackColor = true;
            // 
            // Szerklist
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "Szerklist";
            this.Size = new System.Drawing.Size(904, 470);
            this.Controls.SetChildIndex(this.tabControl1, 0);
            this.Controls.SetChildIndex(this.comboBox1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.AlapTablaView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EgyszeruTablaView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OsszetettKozepsoTablaView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OsszetettAlsoTablaView)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.valasztopage.ResumeLayout(false);
            this.valasztopage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage valasztopage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox Valasztek;
        private System.Windows.Forms.TabPage listapage;
    }
}
