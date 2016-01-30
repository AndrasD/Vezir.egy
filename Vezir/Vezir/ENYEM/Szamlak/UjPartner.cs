using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FakPlusz.Alapfunkciok;
using FakPlusz;
using FakPlusz.Alapcontrolok;
using FakPlusz.UserAlapcontrolok;
using FakPlusz.Formok;
using FakPlusz.VezerloFormok;

namespace Vezir
{
    public partial class UjPartner : Form
    {
        private Tablainfo Szamlainfo;
        private Tablainfo Partnerinfo;
        private Tablainfo Vezirpartnerinfo;
        private Tablainfo semainfo;
        private Tablainfo semakoltsinfo;
        private Tablainfo koltsinfo;
        private Tablainfo termekinfo;
        private Tablainfo koltsszazalinfo;
        private ComboBox Partnercombo;
        private FakUserInterface FakUserInterface;
        private MezoControlCollection aktcontinfo;
        private UserControlInfo usercontrolinfo;
        private Vezerloinfo Vezerles;
        private int comboselect;
        private int partnerinfoviewindex;
        private string filter;
        private DataTable tabla = new DataTable();
        private bool lehetvevo;
        private bool lehetszallito;
        private bool hiba=false;
        private MezoTag sematag;
        private ArrayList fileinfo;
        private ArrayList szovinfo;
        private string[] comboidk;
        private Cols egycol;
        private Partner ujpartnerusercontrol = null;
        public UjPartner()
        {
            InitializeComponent();
            tabla.Columns.Add(new DataColumn("Költségkód"));
            tabla.Columns.Add(new DataColumn("Felosztási százalék"));
        }
        public void UjPartnerInit(Vezerloinfo vezerles,VezerloControl vezerlocontrol, string hivonev,Tablainfo szamlainfo,Tablainfo partnerinfo,Tablainfo vezirpartnerinfo,ComboBox partnercombo,FakUserInterface fak)
        {
            if (ujpartnerusercontrol == null)
            {
                Base hivo = vezerlocontrol.AktivControl;
                ujpartnerusercontrol = new Partner(fak,hivo,vezerles); 
                panel1.Controls.Add(ujpartnerusercontrol);
                ujpartnerusercontrol.Dock = DockStyle.Fill;
                ujpartnerusercontrol.UjpartnerForm = this;
            }
            Partnerinfo = partnerinfo;
            Vezirpartnerinfo = vezirpartnerinfo;
            Szamlainfo = szamlainfo;
            ujpartnerusercontrol.Szallitokotelezo = false;
            ujpartnerusercontrol.Vevokotelezo = false;
            if (hivonev == "Koltsszla")
            {
                ujpartnerusercontrol.Szallitokotelezo = true;
                vezirpartnerinfo.InputColumns["BEVPARTNER"].DefaultValue = "N";
                vezirpartnerinfo.InputColumns["KOLTSPARTNER"].DefaultValue = "I";
            }
            else
            {
                ujpartnerusercontrol.Vevokotelezo = true;
                vezirpartnerinfo.InputColumns["BEVPARTNER"].DefaultValue = "I";
                vezirpartnerinfo.InputColumns["KOLTSPARTNER"].DefaultValue = "N";
            }
            ujpartnerusercontrol.AltalanosInit();
       }

        private void UjPartner_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
            }
            else
            {
                Partnerinfo.Changed = false;
                Partnerinfo.Modositott = false;
                Partnerinfo.ModositasiHiba = false;
                Vezirpartnerinfo.Changed = false;
                Vezirpartnerinfo.Modositott = false;
                Vezirpartnerinfo.ModositasiHiba = false;
            }
                
        }

//        private void sema_DropDownClosed(object sender, EventArgs e)
//        {

//            groupBox2.Visible = false;
//            if (sema.SelectedIndex != -1)
//                button3.Visible = true;
//            else
//                button3.Visible = false;
//        }
//        private void termekkod_DropDownClosed(object sender, EventArgs e)
//        {
//            FakUserInterface.SetHibaszov(termekkod,"");
//            if (termekkod.Text == "")
//            {
//                FakUserInterface.SetHibaszov(termekkod, " Nem lehet üres!");
//                button1.Visible = false;
//            }
//            else
//                Hibavizsg();
//        }
//        private void koltsegkod_DropDownClosed(object sender, EventArgs e)
//        {
//            FakUserInterface.SetHibaszov(koltsegkod, "");
//            if (termekkod.Text == "")
//            {
//                FakUserInterface.SetHibaszov(koltsegkod, " Nem lehet üres!");
//                button1.Visible = false;
//            }
//            else
//                Hibavizsg();

//        }
//        private void Hibavizsg()
//        {
//            button1.Visible=false;
//            if (textBox1.Text == "")
//                return;
//            if (lehetvevo && termekkod.Text == "" || lehetszallito && koltsegkod.Text == "")
//                return;
//            button1.Visible = true;
//        }
//        private void vevoe_Click(object sender, EventArgs e)
//        {
//            if (vevoe.Checked)
//            {
//                termekkod.Enabled = true;
//                termekkod.SelectedIndex = 0;
//            }
//            else 
//            {
//                termekkod.SelectedIndex = -1;
//                termekkod.Enabled = false;
//            }
//        }
//        private void szallitoe_Click(object sender, EventArgs e)
//        {
//            if (szallitoe.Checked)
//            {
//                sema.Enabled = true;
//                koltsegkod.Enabled = true;
//                koltsegkod.SelectedIndex = 0;
//            }
//            else
//            {
//                sema.Text = "";
//                sema.Enabled = false;
//                koltsegkod.Text = "";
//                koltsegkod.Enabled = false;
//                button3.Visible = false;
//                groupBox2.Visible = false;
//            }
//        }

//        private void control_Validating(object sender, CancelEventArgs e)
//        {
//            hiba = false;
//            string hibaszov = "";
//            Control cont = (Control)sender;
//            string contnev = cont.Name;
//            string getszov = FakUserInterface.ErrorProvider.GetError(cont);
//            if (getszov != "")
//            {
////                e.Cancel = true;
//                return;
//            }
//            switch (contnev)
//            {
//                case "textBox1":
//                    if (textBox1.Text != "")
//                    {
//                        Partnerinfo.DataView.RowFilter = "SZOVEG='" + textBox1.Text.Trim() + "'";
//                        if (Partnerinfo.DataView.Count != 0)
//                        {
//                            hiba = true;
//                            hibaszov = " Már van ilyen nevü partner!";
//                        }
//                        Partnerinfo.DataView.RowFilter = filter;
//                    }
//                    break;
//                case "termekkod":
//                    if (lehetvevo && termekkod.Text == "")
//                    {
//                        hiba = true;
//                        hibaszov = " Nem lehet üres!";
//                    }
//                    break;
//                case "koltsegkod":
//                    if (lehetszallito && koltsegkod.Text == "")
//                    {
//                        hiba = true;
//                        hibaszov = " Nem lehet üres!";
//                    }
//                    break;
//            }
//            FakUserInterface.SetHibaszov(cont, hibaszov);
//            if (!hiba)
//                button1.Visible=true;
//            else
//            {
//                button1.Visible = false;
//                e.Cancel = true;
//            }
//        }

//        private void UjPartner_VisibleChanged(object sender, EventArgs e)
//        {
//            if (this.Visible)
//                this.Focus();
//        }
    }
}