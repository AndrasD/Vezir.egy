using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FakPlusz;
using FakPlusz.Alapfunkciok;
using FakPlusz.Alapcontrolok;
using FakPlusz.UserAlapcontrolok;

namespace FakPlusz
{
    public partial class BejelformAlap : Form
    {
        public Bejelentkezo Bejel;
        private string nev = "";
        private string jelszo = "";
        private string ujjelszo = "";
        private string kjelszo = "";
        private bool ok = false;
        private DataTable rgazdatable = new DataTable();
        private DataRow kezelorow;
        private string alkid;
        private ArrayList kezeloidkar;
        private DataTable kezelok = new DataTable();
        private bool vanujceg = false;
        public BejelformAlap()
        {
        }
        public BejelformAlap(Bejelentkezo bejel, Bitmap bitmap, ImageLayout layout, string formszov)
        {
            BejelformAlapInit(bejel, bitmap, layout, formszov, null);
        }
        public BejelformAlap (Bejelentkezo bejel, Bitmap bitmap, ImageLayout layout, string formszov,Icon icon)
        {
            BejelformAlapInit(bejel, bitmap, layout, formszov, icon);
        }
        public virtual void BejelformAlapInit(Bejelentkezo bejel, Bitmap bitmap, ImageLayout layout, string formszov, Icon icon)
        {
            InitializeComponent();
//            ErrorProvider.ContainerControl = containerControl1;
            this.panel1.BackgroundImage = bitmap;
            this.panel1.BackgroundImageLayout = layout;
            Bejel = bejel;
            alkid = Bejel.alkid;
            kezeloidkar = bejel.KezeloIdkArray;
            Sqlinterface.RendszerUserConn(Bejel.Rendszerconn, Bejel.Userconn);
            Sqlinterface.Select(kezelok, Bejel.Userconn, "KEZELOK", "", "", false);
            string maidat = DateTime.Today.ToShortDateString();
            if (maidat.EndsWith("."))
                maidat = maidat.Substring(0, maidat.Length - 1);
            if (bejel.Rgazdaid=="-1")             // kell egy rendszergazda
            {
                label2.Text = "Rendszergazda:";
                textBox2.UseSystemPasswordChar = false;
                kezelorow = null;
            }
            else
                textBox2.UseSystemPasswordChar = true;
            label5.Text = formszov;
            if (icon != null)
                this.Icon = icon;
            vanujceg = Bejel.Vanujceg;
            label4.Visible=vanujceg;
            comboBox1.Visible=vanujceg;
            if (vanujceg)
            {
                string ho_nap = ".01.01";
                int jelenev = DateTime.Today.Year;
                string[] itemek = new string[] { (jelenev - 1).ToString() + ho_nap, jelenev.ToString() + ho_nap, (jelenev + 1).ToString() + ho_nap };
                comboBox1.Items.AddRange(itemek);
                comboBox1.SelectedIndex = 1;
            }
 //           this.Focus();
            textBox1.Focus();
        }
        private void textBox_Validated() //nev
        {
            Bejel.Nev = nev;
            if (nev != "")
            {
                if (jelszo != "" && ok && kezelorow != null)
                    Bejel.Kezeloid = kezelorow["KEZELO_ID"].ToString();
                if (!Bejel.Vanujceg)
                    button1.Focus();
            }
        
        }
        public virtual void nev_Validated(object sender, EventArgs e)
        {
//            ErrorProvider.SetError(textBox1, "");
            nev = textBox1.Text;
            if (nev == "")
            {
                ok = false;
 //               ErrorProvider.SetError(textBox1, "Nem lehet üres!");
                kezelorow = null;
            }
            else
                Nevjelszoellen(nev, kjelszo);
            if (kezeloidkar.Count != 0 && kezelorow == null)
            {
                ok = false;
            }
            if (!ok)
            {
                jelszo = "";
                ujjelszo = "";
                kjelszo = "";
            }
        }
        private void Nevjelszoellen(string nev, string jelszo)
        {
            ok = false;
            if (kezeloidkar.Count == 0)
            {
                kjelszo = jelszo;
                ok = true;
            }
            else
            {
                for (int i = 0; i < kezelok.Rows.Count; i++)
                {
                    kezelorow = kezelok.Rows[i];
                    string egyid = kezelorow["KEZELO_ID"].ToString();
                    if (kezeloidkar.IndexOf(egyid) != -1)
                    {
                        if (kezelorow["SZOVEG"].ToString().Trim() == nev || kezelorow["SZOVEG"].ToString().Trim() == "")
                        {

                            kjelszo = kezelorow["JELSZO"].ToString().Trim();
                            if (kjelszo == "")
                                textBox2.UseSystemPasswordChar = false;
                            else
                                textBox2.UseSystemPasswordChar = true;
                            if (kjelszo == "" || kjelszo == jelszo)
                                ok = true;
                            break;
                        }
                    }
                }
            }
            if (!ok)
                kezelorow = null;
        }
        public virtual void jelszo_Validated(object sender, EventArgs e)
        {
 //           ErrorProvider.SetError(textBox2, "");
            jelszo =textBox2.Text;
            ok = true;
            Nevjelszoellen(nev, jelszo);
            if (kezeloidkar.Count != 0 && kezelorow == null)
//            {
                ok = false;
 //               ErrorProvider.SetError(textBox2, "Hibás felhasználó/jelszó!");
//            }
            if (!ok)
            {
                jelszo = "";
                ujjelszo = "";

            }
            Bejel.jelszo = jelszo;
            Bejel.ujjelszo = ujjelszo;
            if (ok)
//            {
////                button1.DialogResult = DialogResult.None;
////                this.AcceptButton = null;
//            }
//            else
//            {
                textBox_Validated();
//                button1.DialogResult = DialogResult.OK;
//                this.AcceptButton = button1;
//            }
        }

        //public virtual  void ujjelszo_Validated(object sender, EventArgs e)
        //{
        //    if (nev == "" || jelszo == "")
        //    {
        //        ujjelszo = "";
        //        ((TextBox)sender).Text = "";
        //    }
        //    else
        //    {
        //        ujjelszo = ((TextBox)sender).Text;
        //        textBox_Validated();
        //    }
        //    Bejel.ujjelszo = ujjelszo;
        //}

        private void button1_Click(object sender, EventArgs e)
        {
            string hibaszov = Hibavizsg();
            if (hibaszov == "")
            {
                button1.DialogResult = DialogResult.OK;
                if (Bejel.Vanujceg)
                    Bejel.UjcegIndulodatum = Convert.ToDateTime(comboBox1.Text);
                this.DialogResult = DialogResult.OK;
            }
            else
                label1.Text = hibaszov;
 //               ErrorProvider.SetError(button1,hibaszov);
        }
        private string Hibavizsg()
        {
            string hibaszov = "";
            nev = textBox1.Text;
            if (nev == "")
            {
                hibaszov = "Felhasználó nem lehet üres!";
                textBox1.Focus();
            }
            else
            {
                jelszo = textBox2.Text;
                if (jelszo == "")
                {
                    hibaszov = "Jelszó nem lehet üres!";
                    textBox2.Focus();
                }
                else
                {
                    Nevjelszoellen(nev, jelszo);
                    if (!ok)
                    {
                        hibaszov = "Felhasználó és/vagy jelszó hibás!";
                        textBox1.Focus();
                    }
                    else if (Bejel.Vanujceg)
                    {
                        if (comboBox1.SelectedIndex == -1)
                        {
                            hibaszov = "Indulás dátuma nem lehet üres!";
                            comboBox1.Focus();
                        }

                    }
                }
            }

            return hibaszov;
        }
        private void comboBox1_DropDownClosed(object sender, EventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
                button1.Focus();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void BejelformAlap_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        private void button1_Enter(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }
    }
}