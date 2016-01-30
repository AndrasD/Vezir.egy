using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FakPlusz.Alapfunkciok;
using FakPlusz.Alapcontrolok;
using FakPlusz.SzerkesztettListak;
using FakPlusz.Formok;

namespace FakPlusz
{
    public partial class Helpwindow : Form
    {
        private FakUserInterface FakUserInterface;
        private string Helpazonosito;
        private Tablainfo Tabinfo;
        private string szoveg;
        private string oldszoveg;
        private bool ujsor;
        private string id;
        private bool Tervezoe;
        private Form Mainform;
        private Control AktivControl;
        private DataRow helprow;
        public Helpwindow(FakUserInterface fak, bool tervezoe, Form mainform)
        {
            InitializeComponent();
            FakUserInterface = fak;
            Tervezoe = tervezoe;
            Mainform = mainform;
            Tabinfo = FakUserInterface.GetBySzintPluszTablanev("R", "HELPTABOK");
            if (!tervezoe)
            {
                textBox2.ReadOnly = true;
                megse.Visible = false;
            }
            else
            {
                textBox2.ReadOnly = false;
                megse.Visible = true;
            }
        }
        public void Helpszerkeszt(string helpazonosito, Control aktivcontrol)
        {
            AktivControl = aktivcontrol;
//            Tabinfo.DataView.RowFilter = "HELP_AZONOSITO = '" + helpazonosito + "'";
            Helpazonosito = helpazonosito;
            string[] idk = FakUserInterface.GetTartal(Tabinfo, "HELP_ID", "HELP_AZONOSITO", helpazonosito);
            ujsor = idk == null;
            if (!ujsor)
            {
                id = idk[0];
                Tabinfo.DataView.RowFilter = "HELP_ID = " + id;
                helprow = Tabinfo.DataView[0].Row;
                Tabinfo.DataView.RowFilter = "";
                szoveg = helprow["HELP_SZOVEG"].ToString(); // FakUserInterface.GetTartal(Tabinfo, "HELP_SZOVEG", "HELP_ID", id)[0];
            }
            else
            {
                id = null;
                helprow = null;
                szoveg = "";
            }
            oldszoveg = szoveg;
            label1.Text = helpazonosito;
            textBox2.Text = szoveg;
            textBox2.SelectionStart = 0;
            textBox2.SelectionLength = 0;
            Mainform.AddOwnedForm(this);
            this.Visible = true;
            Mainform.Enabled = false;
        }

        private void ok_Click(object sender, EventArgs e)
        {
            if (oldszoveg != textBox2.Text)
            {
                DataRow row;
                if (ujsor)
                {
                    Tabinfo.ViewSorindex = -1;
                    row = Tabinfo.Ujsor();
                }
                else
                    row = helprow;
                row["MODOSITOTT_M"] = 1;
                Tabinfo.Modositott = true;
                row["HELP_SZOVEG"] = textBox2.Text;
                row["HELP_AZONOSITO"] = Helpazonosito;
                FakUserInterface.UpdateTransaction(new Tablainfo[] { Tabinfo });
            }
            FakUserInterface.EventTilt = true;
            Mainform.Enabled = true;
            Mainform.RemoveOwnedForm(this);
            this.Visible = false;
            FakUserInterface.EventTilt = false;
        }

        private void megse_Click(object sender, EventArgs e)
        {
            FakUserInterface.EventTilt = true;
            Mainform.Enabled = true;
            Mainform.RemoveOwnedForm(this);
            this.Visible = false;
            Mainform.Enabled = true;
            Mainform.Visible = true;
            FakUserInterface.EventTilt = false;
        }

    }
}
