namespace Vezir
{
    partial class Penztarpenztar
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
            this.penztarba = new System.Windows.Forms.ComboBox();
            this.szamladatum = new System.Windows.Forms.DateTimePicker();
            this.label11 = new System.Windows.Forms.Label();
            this.megjegyzes = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.penztarbol = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.osszbrutto = new FormattedTextBox.FormattedTextBox();
            this.panel111.SuspendLayout();
            this.panel12.SuspendLayout();
            this.panel112.SuspendLayout();
            this.panel11.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AlapTablaView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EgyszeruTablaView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OsszetettKozepsoTablaView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OsszetettAlsoTablaView)).BeginInit();
            this.SuspendLayout();
            // 
            // panel111
            // 
            this.panel111.Location = new System.Drawing.Point(789, 200);
            this.panel111.Size = new System.Drawing.Size(31, 63);
            // 
            // groupBox112
            // 
            this.groupBox112.Location = new System.Drawing.Point(4, 48);
            this.groupBox112.Size = new System.Drawing.Size(734, 151);
            this.groupBox112.Tag = "C,BEVSZLA";
            this.groupBox112.Text = "Számla";
            // 
            // groupBox111
            // 
            this.groupBox111.Size = new System.Drawing.Size(29, 14);
            this.groupBox111.Tag = "C,BEVSZLA";
            this.groupBox111.Text = "Számlák";
            // 
            // panel12
            // 
            this.panel12.Location = new System.Drawing.Point(66, 468);
            this.panel12.Size = new System.Drawing.Size(387, 111);
            this.panel12.TabIndex = 19;
            // 
            // panel112
            // 
            this.panel112.Location = new System.Drawing.Point(3, 3);
            this.panel112.Size = new System.Drawing.Size(745, 268);
            this.panel112.TabIndex = 17;
            this.panel112.Tag = "panel11";
            // 
            // groupBox122
            // 
            this.groupBox122.Location = new System.Drawing.Point(128, 43);
            this.groupBox122.Size = new System.Drawing.Size(207, 27);
            this.groupBox122.TabIndex = 11;
            this.groupBox122.Tag = "C,KOLTSSZLA_TETEL";
            this.groupBox122.Text = "Tételsor";
            // 
            // groupBox121
            // 
            this.groupBox121.Size = new System.Drawing.Size(743, 45);
            this.groupBox121.TabIndex = 10;
            this.groupBox121.Tag = "C,KOLTSSZLA_TETEL";
            this.groupBox121.Text = "Tételsorok:";
            // 
            // panel11
            // 
            this.panel11.Location = new System.Drawing.Point(4, 0);
            this.panel11.Size = new System.Drawing.Size(746, 585);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.osszbrutto);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.penztarba);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.penztarbol);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.megjegyzes);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.szamladatum);
            this.groupBox1.Location = new System.Drawing.Point(4, 442);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox1.Padding = new System.Windows.Forms.Padding(0);
            this.groupBox1.Size = new System.Drawing.Size(740, 141);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.Tag = "C,PENZTARBOLPENZTARBA";
            this.groupBox1.Text = "Pénztárból pénztárba";
            // 
            // groupBox2
            // 
            this.groupBox2.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox2.Padding = new System.Windows.Forms.Padding(0);
            this.groupBox2.Size = new System.Drawing.Size(744, 417);
            this.groupBox2.Tag = "C,PENZTARBOLPENZTARBA";
            this.groupBox2.Text = "Pénztárból pénztárba";
            // 
            // panel1
            // 
            this.panel1.Size = new System.Drawing.Size(824, 601);
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(779, 391);
            this.panel2.Size = new System.Drawing.Size(42, 84);
            // 
            // groupBox3
            // 
            this.groupBox3.Size = new System.Drawing.Size(40, 49);
            // 
            // penztarba
            // 
            this.penztarba.FormattingEnabled = true;
            this.penztarba.Location = new System.Drawing.Point(283, 58);
            this.penztarba.Name = "penztarba";
            this.penztarba.Size = new System.Drawing.Size(243, 23);
            this.penztarba.TabIndex = 4;
            this.penztarba.Tag = "PENZTAR_BA";
            // 
            // szamladatum
            // 
            this.szamladatum.CustomFormat = "";
            this.szamladatum.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.szamladatum.Location = new System.Drawing.Point(136, 11);
            this.szamladatum.Name = "szamladatum";
            this.szamladatum.Size = new System.Drawing.Size(93, 21);
            this.szamladatum.TabIndex = 1;
            this.szamladatum.Tag = "SZLA_DATUM";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 96);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(76, 15);
            this.label11.TabIndex = 31;
            this.label11.Text = "Megjegyzés:";
            // 
            // megjegyzes
            // 
            this.megjegyzes.Location = new System.Drawing.Point(85, 93);
            this.megjegyzes.Name = "megjegyzes";
            this.megjegyzes.Size = new System.Drawing.Size(317, 21);
            this.megjegyzes.TabIndex = 5;
            this.megjegyzes.Tag = "SZOVEG";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(280, 17);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(51, 15);
            this.label14.TabIndex = 37;
            this.label14.Text = "Összeg:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 40);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(54, 15);
            this.label12.TabIndex = 34;
            this.label12.Text = "Honnan:";
            // 
            // penztarbol
            // 
            this.penztarbol.FormattingEnabled = true;
            this.penztarbol.Location = new System.Drawing.Point(6, 58);
            this.penztarbol.Name = "penztarbol";
            this.penztarbol.Size = new System.Drawing.Size(243, 23);
            this.penztarbol.TabIndex = 3;
            this.penztarbol.Tag = "PENZTAR_BOL";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(280, 40);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 15);
            this.label6.TabIndex = 37;
            this.label6.Text = "Hová:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 14);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(127, 15);
            this.label5.TabIndex = 39;
            this.label5.Text = "Pénzmozgás dátuma:";
            // 
            // osszbrutto
            // 
            this.osszbrutto.Form = FormattedTextBox.FormattedTextBox.formnum.None;
            this.osszbrutto.Format = "";
            this.osszbrutto.Location = new System.Drawing.Point(340, 14);
            this.osszbrutto.Name = "osszbrutto";
            this.osszbrutto.Size = new System.Drawing.Size(186, 21);
            this.osszbrutto.TabIndex = 2;
            this.osszbrutto.Tag = "KIVET";
            // 
            // Penztarpenztar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "Penztarpenztar";
            this.Size = new System.Drawing.Size(824, 626);
            this.panel111.ResumeLayout(false);
            this.panel111.PerformLayout();
            this.panel12.ResumeLayout(false);
            this.panel112.ResumeLayout(false);
            this.panel112.PerformLayout();
            this.panel11.ResumeLayout(false);
            this.panel11.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AlapTablaView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EgyszeruTablaView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OsszetettKozepsoTablaView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OsszetettAlsoTablaView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker szamladatum;
        private System.Windows.Forms.TextBox megjegyzes;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox penztarbol;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox penztarba;
        private FormattedTextBox.FormattedTextBox osszbrutto;
        //private FormattedTextBox.FormattedTextBox osszbrutto;
        //private FormattedTextBox.FormattedTextBox maradek;
    }
}
