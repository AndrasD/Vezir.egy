using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace FakPlusz
{
    /// <summary>
    /// Magyaritott MessageBox Form-ja
    /// </summary>
    public partial class MessageAlap : Form
    {
        /// <summary>
        /// Belso hasznalatra
        /// </summary>
        private int tav = 10;
        /// <summary>
        /// Belso hasznalatra
        /// </summary>
        private Button button1 = null;
        /// <summary>
        /// Belso hasznalatra
        /// </summary>
        private Button button2 = null;
        /// <summary>
        /// Belso hasznalatra
        /// </summary>
        private Button button3 = null;
        /// <summary>
        /// arra az esetre kell, ha nincs button definialva, null, egyebkent a Constructor parametere
        /// </summary>
        public Button[] Buttonok = null;
        /// <summary>
        /// MessageAlap Constructor, ha csak idoszakosan akarunk info-t megjeleniteni
        /// az informaciot  a SetText(string szoveg) member hivasaval helyezhetjuk el
        /// </summary>
        /// <param name="nametext">
        /// A megjelenitendo form Text property-ben jelenik meg
        /// </param>
        public MessageAlap(string nametext)
        {
            InitializeComponent();
            this.Text = nametext;
            this.Width = nametext.Length * 7 + 100;
            this.ControlBox = false;
        }
        /// <summary>
        /// Idoszakos info megjelenitesenel ez kerul a label1.Text-be
        /// </summary>
        /// <param name="text">
        /// az info
        /// </param>
        public void SetText(string text)
        {
            if (this.Text.Length < text.Length)
                this.Width = text.Length * 7 + 100;
            this.label1.Text = text;
            this.Refresh();
        }
        /// <summary>
        /// MessageAlap Constructor, a parameterek alapjan eloallitja a kivant Form-ot
        /// </summary>
        /// <param name="text">
        /// A megjelenitendo form label1.Text property-ben jelenik meg
        /// </param>
        /// <param name="nametext">
        /// A megjelenitendo form Text property-ben jelenik meg
        /// </param>
        /// <param name="buttonok">
        /// A szukseges Button-ok tombje (max 3 elem) vagy null, ha nem kell Button
        /// </param>
        public MessageAlap(string text, string nametext, Button[] buttonok)
        {
            InitializeComponent();
            Buttonok = buttonok;
            this.Text = nametext;
            if (nametext.Length != 0)
                this.Width = nametext.Length * 7 + 100;
            label1.Text = text;
            if (buttonok != null)
            {
                int x;
                int y;
                this.Controls.AddRange(buttonok);
                switch (buttonok.Length)
                {
                    case 1:
                        button1 = buttonok[0];
                        button1.Click += new EventHandler(button_Click);
                        button1.Location = new Point((this.Size.Width - button1.Size.Width) / 2, (this.Size.Height - label1.Size.Height - label1.Location.Y) / 2 + label1.Location.Y + label1.Size.Height);
                        break;
                    case 2:
                        button1 = buttonok[0];
                        button1.Click += new EventHandler(button_Click);
                        button2 = buttonok[1];
                        button2.Click += new EventHandler(button_Click);
                        x = (this.Size.Width - button2.Size.Width - button1.Size.Width - tav) / 2;
                        y = (this.Size.Height - label1.Size.Height - label1.Location.Y) / 2 + label1.Location.Y + label1.Size.Height;
                        button1.Location = new Point(x, y);
                        button2.Location = new Point(button1.Location.X + button1.Size.Width + tav, y);
                        break;
                    case 3:
                        button1 = buttonok[0];
                        button1.Click += new EventHandler(button_Click);
                        button2 = buttonok[1];
                        button2.Click += new EventHandler(button_Click);
                        button3 = buttonok[2];
                        button3.Click += new EventHandler(button_Click);
                        y = (this.Size.Height - label1.Size.Height - label1.Location.Y) / 2 + label1.Location.Y + label1.Size.Height;
                        x = (this.Size.Width - button2.Size.Width - button3.Size.Width - button1.Size.Width - 2 * tav) / 2;
                        button1.Location = new Point(x, y);
                        button2.Location = new Point(button1.Location.X + button1.Size.Width + tav, y);
                        button3.Location = new Point(button2.Location.X + button2.Size.Width + tav, y);
                        break;

                }
            }

        }
        /// <summary>
        /// a kivalasztott button alapjan beallitja a megfelelo DialogResult-ot
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void button_Click(object sender, EventArgs e)
        {
            string[] dr = new string[] { "OK", "Mégsem", "Igen", "Nem", "None", "Cancel" };
            string text = ((Button)sender).Text;
            switch (text)
            {
                case "OK":
                    this.DialogResult = DialogResult.OK;
                    break;
                case "Mégsem":
                    this.DialogResult = DialogResult.Cancel;
                    break;
                case "Igen":
                    this.DialogResult = DialogResult.Yes;
                    break;
                case "Nem":
                    this.DialogResult = DialogResult.No;
                    break;
                default:
                    this.DialogResult = DialogResult.Cancel;
                    break;
            }
            this.Close();
        }
    }
}