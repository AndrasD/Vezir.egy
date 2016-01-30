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
    public partial class Partner : ControlAlap
    {
        public bool Vevokotelezo = false;
        public bool Szallitokotelezo = false;
        public UjPartner UjpartnerForm = null;
        private Tablainfo bevsemainfo;
        private Tablainfo termcsopinfo;
        private Tablainfo termcsopcsop;
        private Tablainfo koltssemainfo;
        private Tablainfo koltscsopinfo;
        private Tablainfo koltscsopcsop;
        private Tablainfo felosztinfo;
        private Cols bevpartnercol;
        private Cols koltspartnercol;
        private Cols partnerkoltsegkodcol;
        private Cols partnertermekkodcol;
        private Cols partnersemakodcol;
        private Cols termsemakodcol;
        private int bevpartnerindex;
        private int koltspartnerindex;
        private int koltsegkodindex;
        private int termekkodindex;
        private int semakodindex;
        private int termsemakodindex;

        private DataTable table3 = new DataTable("KOLTSFELOSZT");
        private DataTable table2 = new DataTable("TERMFELOSZT");
        private DataView view3 = new DataView();
        private DataView view2 = new DataView();
        private Tablainfo partnerinfo;
        private Tablainfo vezpartnerinfo;
        private Tablainfo gyokerpartner;
        private VezerloControl VezerloControl;
        private bool lehetvevo = false;
        private bool lehetszallito = false;
        private Controltipus kieg = null;
        private string ujszoveg = "";
        private DataTable bevszla = new DataTable("BEVSZLA");
        private DataTable koltszla = new DataTable("KOLTSSZLA");
        public Partner(FakUserInterface fak, Base hivo, Vezerloinfo vezerles)
        {
            InitializeComponent();
            ParameterAtvetel(fak, hivo, vezerles);
            partnerinfo = FakUserInterface.GetByAzontip("T CTPARTNER");
            vezpartnerinfo = FakUserInterface.GetByAzontip("T CTVEZIRPARTNER");
            vezpartnerinfo.ComboColumns["MARKETINGPARTNER"].DefaultValue = "N";
            gyokerpartner = UserParamTabla.TermCegPluszCegalattiTablainfok["PARTNERGYOKER"];
            gyokerpartner.ViewSorindex = 0;
            Hivo = hivo;
            toolStripfo.Visible = false;
            toolStrip1.Items.Remove(torol);
            if (hivo.Name == "VezerloControl")
            {
                VezerloControl = (VezerloControl)hivo;
                FakUserInterface.AdattoltByAktid(gyokerpartner);
                FakUserInterface.OsszesAdattoltByParent("PARTNERGYOKER");

            }
            else
            {
                VezerloControl = (VezerloControl)hivo.Hivo;
                partnerinfo.AktIdentity = -1;
                FakUserInterface.AdattoltByAktid(partnerinfo);
                FakUserInterface.OsszesAdattoltByParent("PARTNER");
//                panel1.Controls.Remove(toolStrip1);
                panel1.Controls.Remove(groupBox1);
                groupBox2.Dock = DockStyle.Fill;
                toolStrip1.Items.Remove(uj);
                toolStrip1.Items.Remove(elozo);
                toolStrip1.Items.Remove(kovetkezo);
                elolrol.Text = "Vissza";
            }
            AlapinfoInit(new object[] { new object[] { Alapinfotipus.Alap, new Panel[] { panel1, panel2 } } });
            kieg = ControltipusCollection.Find(groupBox3);
            bevpartnercol = vezpartnerinfo.InputColumns["BEVPARTNER"];
            koltspartnercol = vezpartnerinfo.InputColumns["KOLTSPARTNER"];
            partnerkoltsegkodcol = vezpartnerinfo.ComboColumns["KOLTSEGKOD_ID"];
            partnertermekkodcol = vezpartnerinfo.ComboColumns["TERMEKKOD_ID"];
            partnersemakodcol = vezpartnerinfo.ComboColumns["SEMA_ID"];
            termsemakodcol = vezpartnerinfo.ComboColumns["TERMSEMA_ID"];
            bevpartnerindex = vezpartnerinfo.InputColumns.IndexOf("BEVPARTNER");
            koltspartnerindex = vezpartnerinfo.InputColumns.IndexOf("KOLTSPARTNER");
            koltsegkodindex = vezpartnerinfo.InputColumns.IndexOf("KOLTSEGKOD_ID");
            termekkodindex = vezpartnerinfo.InputColumns.IndexOf("TERMEKKOD_ID");
            semakodindex = vezpartnerinfo.InputColumns.IndexOf("SEMA_ID");
            termsemakodindex = vezpartnerinfo.InputColumns.IndexOf("TERMSEMA_ID");
        }
        public override bool EgyediValidalas(MezoTag egytag)
        {
            bool hiba = false;
            if (egytag.Control.Name == "textBox1" && partnerinfo.AktIdentity==-1)
            {
                egytag.SetHibaSzov("");
                VezerloControl.szarmazekospartnerinfo.DataView.RowFilter = "SZOVEG = '" + textBox1.Text + "'";
                hiba = VezerloControl.szarmazekospartnerinfo.DataView.Count != 0;
                VezerloControl.szarmazekospartnerinfo.DataView.RowFilter = "";
                if (hiba)
                    egytag.SetHibaSzov("Van már ilyen!");

            }
            return hiba;
        }
        
        public override void Button_Click(object sender, EventArgs e)
        {
            bool vanhiba = false;
            ToolStripButton but = (ToolStripButton)sender;
            if (but.Name == "ok1")
            {
                vanhiba = false;
                lehetvevo = vevolehet.Text.Contains("I");
                lehetszallito = szallitolehet.Text.Contains("I");
                kieg.MezoControlInfo.ClearError();
                vanhiba = Hibavizsg(kieg);
                for (int i = 0; i < kieg.MezoControlInfo.Inputeleminfok.Length; i++)
//                for(int i=0;i<kieg.Tablainfo.InputColumns.Count;i++)
                {
                    MezoTag egytag = kieg.MezoControlInfo.Inputeleminfok[i];
                    Cols egyinp = egytag.Egyinp;
//                    Cols egyinp = kieg.Tablainfo.InputColumns[i];
//                    MezoTag egytag = egyinp.EgyTag;
                    ComboBox cont = (ComboBox)egytag.Control;
                    egytag.Hibaszov = "";
                    egytag.Hosszuhibaszov = "";
                    FakUserInterface.ErrorProvider.SetError(cont, "");
                    int contindex = cont.SelectedIndex;
                    string tartal = egytag.Control.Text;
                    switch (egyinp.ColumnName)
                    {
                        case "BEVPARTNER":
                            if (!lehetvevo && !lehetszallito)
                            {
                                egytag.SetHibaSzov("Se vevö, se szállitó?");
                                vanhiba = true;
                            }
                            if (!lehetvevo && vezpartnerinfo.AktIdentity!=-1)
                            {
                                bevszla = Sqlinterface.Select(bevszla, FakUserInterface.AktualCegconn, "BEVSZLA", " where PARTNER_ID = " + vezpartnerinfo.AktIdentity.ToString(), "", true);
                                if (bevszla.Rows.Count != 0)
                                {
                                    egytag.SetHibaSzov("Már szerepel vevöszámlán!");
                                    vanhiba = true;
                                }
                            }
                            break;
                        case "KOLTSPARTNER":
                            if (!lehetszallito && vezpartnerinfo.AktIdentity != -1)
                            {
                                koltszla = Sqlinterface.Select(koltszla, FakUserInterface.AktualCegconn, "KOLTSSZLA", "where PARTNER_ID = " + vezpartnerinfo.AktIdentity.ToString(), "", true);
                                if (koltszla.Rows.Count != 0)
                                {
                                    egytag.SetHibaSzov("Már szerepel költségszámlán!");
                                    vanhiba = true;
                                }
                            }
                            break;
                        case "TERMEKKOD_ID":
                            if (!lehetvevo)
                                cont.SelectedIndex = -1;
                            else if (contindex == -1 || tartal == "")
                            {
                                egytag.SetHibaSzov("Nem lehet üres!");
                                vanhiba = true;
                            }
                            break;
                        case "TERMSEMA_ID":
                            if (!lehetvevo)
                                cont.SelectedIndex = -1;
                            egytag.SetValue(cont.Text);
                            break;
                        case "KOLTSEGKOD_ID":
                             if (!lehetszallito)
                                cont.SelectedIndex = -1;
                            else if (contindex == -1 || tartal=="")
                            {
                                egytag.SetHibaSzov( "Nem lehet üres!");
                                vanhiba = true;
                            }
                           break;
                        case "SEMA_ID":
                            if (!lehetszallito)
                                cont.SelectedIndex = -1;
                            egytag.SetValue(egytag.Control.Text);
                            break;
                    }
                }
                if (vanhiba)
                    return;
            }
            if (but.Name == "ok")
            {
                MezoTag egytag = (MezoTag)textBox1.Tag;
                if (textBox1.Text == "")
                {
                    egytag.SetHibaSzov("Nem lehet üres!");
                    vanhiba = true;
                }
                else
                {
                    if (partnerinfo.AktIdentity != -1)
                        partnerinfo.DataView.RowFilter = partnerinfo.IdentityColumnName + " <> " + partnerinfo.AktIdentity.ToString();
                    string[] idk = FakUserInterface.GetTartal(partnerinfo, "PARTNER_ID", "SZOVEG", textBox1.Text);
                    partnerinfo.DataView.RowFilter = "";
                    if (idk != null)
                    {
                        egytag.SetHibaSzov("Már van ilyen!");
                        vanhiba = true;
                    }
                }
                if (vanhiba)
                    return;
            }

            base.Button_Click(sender, e);
            if (UjpartnerForm != null)
            {
                if (but.Name == "ok" || but.Name == "elolrol1")
                {
                    if (Vevokotelezo)
                    {
                        vevolehet.Text = "Igen";
                        vevolehet.Enabled = false;
                    }
                    else
                    {
                        szallitolehet.Text = "Igen";
                        szallitolehet.Enabled=false;
                    }
                }
                else if (but.Name == "rogzit")
                {
                    UjpartnerForm.DialogResult = DialogResult.OK;
                    UjpartnerForm.Close();
                }
                else if (but.Name == "elolrol")
                {

                    UjpartnerForm.DialogResult = DialogResult.Cancel;
                    UjpartnerForm.Close();
                    return;
                }
            }
            Semaugyek();
        }
        public override void AltalanosInit()
        {
            if (UjpartnerForm != null)
            {
                partnerinfo.Adattabla.Rows.Clear();
                partnerinfo.DataView.RowFilter = "";
                vezpartnerinfo.Adattabla.Rows.Clear();
                vezpartnerinfo.DataView.RowFilter = "";
                partnerinfo.AktIdentity = -1;
                FakUserInterface.AdattoltByAktid(partnerinfo);
                FakUserInterface.OsszesAdattoltByParent("PARTNER");
                Vevokotelezo = Hivo.Name == "Bevszla";
                Szallitokotelezo = !Vevokotelezo;
            }
            else
            {
                Vevokotelezo = false;
                Szallitokotelezo = false;
                if (Elsoeset)
                {
                    FakUserInterface.OsszesAdattoltByParent("PARTNERGYOKER");
                    partnerinfo.ViewSorindex = partnerinfo.DataView.Count - 1;
                }
            }
            dataGridView2.Width = this.Width / 2;
            dataGridView3.Width = this.Width - dataGridView2.Width;
            Column1.Width = dataGridView2.Width - 100;
            Column3.Width = dataGridView3.Width - 100;
            dataGridView2.Visible = false;
            dataGridView3.Visible = false;
            //           partnerinfo.ViewSorindex = partnerinfo.DataView.Count - 1;
            Tabinfo = vezpartnerinfo;
            bevpartnercol = Tabinfo.InputColumns["BEVPARTNER"];
            if (Vevokotelezo)
            {
                bevpartnercol.DefaultValue = "I";
                vevolehet.Enabled = false;
            }
            else
            {
                bevpartnercol.DefaultValue = "N";
                vevolehet.Enabled = true;
            }
            koltspartnercol = Tabinfo.InputColumns["KOLTSPARTNER"];
            if (Szallitokotelezo)
            {
                koltspartnercol.DefaultValue = "I";
                szallitolehet.Enabled = false;
            }
            else
            {
                koltspartnercol.DefaultValue = "N";
                szallitolehet.Enabled = true;
            }
            partnerkoltsegkodcol = Tabinfo.ComboColumns["KOLTSEGKOD_ID"];
            partnertermekkodcol = Tabinfo.ComboColumns["TERMEKKOD_ID"];
            partnersemakodcol = Tabinfo.ComboColumns["SEMA_ID"];
            termsemakodcol = Tabinfo.ComboColumns["TERMSEMA_ID"];
            bevpartnerindex = Tabinfo.InputColumns.IndexOf("BEVPARTNER");
            koltspartnerindex = Tabinfo.InputColumns.IndexOf("KOLTSPARTNER");
            koltsegkodindex = Tabinfo.InputColumns.IndexOf("KOLTSEGKOD_ID");
            termekkodindex = Tabinfo.InputColumns.IndexOf("TERMEKKOD_ID");
            semakodindex = Tabinfo.InputColumns.IndexOf("SEMA_ID");
            termsemakodindex = Tabinfo.InputColumns.IndexOf("TERMSEMA_ID");
            termcsopinfo = FakUserInterface.GetKodtab("C", "Termcsop");
            FakUserInterface.Combokupdate(termcsopinfo.TablaTag);
            termcsopcsop = FakUserInterface.GetOsszef("C", "Termcsopkod");
            bevsemainfo = FakUserInterface.GetCsoport("C", "Termfeloszt");
            koltscsopinfo = FakUserInterface.GetBySzintPluszTablanev("C", "KOLTSEGCSOPORT");
            FakUserInterface.Combokupdate(koltscsopinfo.TablaTag);
            koltscsopcsop = FakUserInterface.GetOsszef("C", "Koltscsopkod");
            koltssemainfo = FakUserInterface.GetCsoport("C", "Feloszt");
            string[] idk = null;
            idk = FakUserInterface.GetTartal(termcsopinfo, "SORSZAM");
            ArrayList ar = new ArrayList();
            if (idk != null)
            {
                for (int i = 0; i < idk.Length; i++)
                {
                    string[] id1 = FakUserInterface.GetTartal(termcsopcsop, "SORSZAM", "SORSZAM1", idk[i]);
                    if (id1 != null)
                    {
                        bevsemainfo.Osszefinfo.InitKell = true;
                        bevsemainfo.Osszefinfo.OsszefinfoInit();
                        string[] id2 = FakUserInterface.GetTartal(bevsemainfo, "SORSZAM2", "SORSZAM1", id1);
                        if (id2 != null)
                            ar.Add(idk[i]);
                    }
                }
                FakUserInterface.Comboinfoszures(bevetelfeloszt, (string[])ar.ToArray(typeof(string)));
                idk = FakUserInterface.GetTartal(koltscsopinfo, "KOLTSEGCSOPORT_ID", "SEMAE", "I");
                if (idk != null)
                {
                    ar = new ArrayList();
                    for (int i = 0; i < idk.Length; i++)
                    {
                        string[] id1 = FakUserInterface.GetTartal(koltscsopcsop, "SORSZAM", "SORSZAM1", idk[i]);
                        if (id1 != null)
                        {
                            koltssemainfo.Osszefinfo.InitKell = true;
                            koltssemainfo.Osszefinfo.OsszefinfoInit();
                            string[] idk2 = FakUserInterface.GetTartal(koltssemainfo, "SORSZAM2", "SORSZAM1", id1);
                            if (idk2 != null)
                                ar.Add(idk[i]);
                        }
                    }
                }
                FakUserInterface.Comboinfoszures(koltsegfeloszt, (string[])ar.ToArray(typeof(string)));
                SajatJelzesBeallit();
                FirstFocusControl = textBox1;
                TabStop = false;
                TabStop = true;
                Semaugyek();
            }
        }
        public override void Visible_Changed(object sender, EventArgs e)
        {
            if (this.Visible)
                textBox1.Focus();
        }
        public override void GridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            base.GridView_CellMouseClick(sender, e);
            Semaugyek();
        }
        public override bool RogzitesElott()
        {
            if (UjpartnerForm != null)
                ujszoveg = partnerinfo.DataView[0].Row["SZOVEG"].ToString();
            return true;
        }
        public override void RogzitesUtan()
        {
            FakUserInterface.ForceAdattolt(VezerloControl.szarmazekospartnerinfo, true);
            FakUserInterface.ForceAdattolt(VezerloControl.szarmazekoskiegpartnerinfo, true);
            ComboBox combo = null;
            if (VezerloControl.bevpartnercombo != null)
                VezerloControl.ComboSzures("Bevszla", VezerloControl.bevpartnercombo);
            if (VezerloControl.koltspartnercombo != null)
                VezerloControl.ComboSzures("Koltsszla", VezerloControl.koltspartnercombo);
            if (Hivo.Name == "Bevszla")
                combo = VezerloControl.bevpartnercombo;
            else if (Hivo.Name == "Koltsszla")
                combo = VezerloControl.koltspartnercombo;
            if(combo!=null)
            {
                combo.SelectedIndex = combo.Items.IndexOf(ujszoveg);
            }
            UserParamTabla.SetKozosAllapotok();
        }
        public override void EgyediBeallit(Controltipus egycont)
        {
            string savfilt = vezpartnerinfo.DataView.RowFilter;
            textBox1.ReadOnly = false;
            Tablainfo info = egycont.Tablainfo;
            if (egycont.Aktid != -1)
            {
                string[] colnevek = new string[] { "MARKETINGPARTNER", "MUNKALAPPARTNER", "SULIPARTNER" };
                vezpartnerinfo.DataView.RowFilter = "";
                for(int i=0;i<colnevek.Length;i++)
                {
                    vezpartnerinfo.DataView.RowFilter=colnevek[i]+"='I'";
                    if(vezpartnerinfo.DataView.Count!=0)
                    {
                    textBox1.ReadOnly = true;
                        break;
                    }
                }
                vezpartnerinfo.DataView.RowFilter = savfilt;
            }
        }
        private void bevetelfeloszt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!FakUserInterface.EventTilt)
            {
                Semaugyek();
            }
        }

        private void koltsegfeloszt_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (!FakUserInterface.EventTilt)
            {
                Semaugyek();
            }
        }

        private void bevetelfeloszt_TextUpdate(object sender, EventArgs e)
        {
            if (!FakUserInterface.EventTilt)
            {
                if (bevetelfeloszt.Text == "")
                {
                    Semaugyek();
                }
            }
        }
        private void koltsegfeloszt_TextUpdate(object sender, EventArgs e)
        {
            if (!FakUserInterface.EventTilt)
            {
                if (koltsegfeloszt.Text == "")
                {
                    Semaugyek();
                }
            }
       
        }
        private void bevetelkod_TextUpdate(object sender, EventArgs e)
        {
            if (!FakUserInterface.EventTilt && bevetelkod.Text == "")
                bevetelkod.SelectedIndex = -1;
        }
        private void koltsegkod_TextUpdate(object sender, EventArgs e)
        {
            if (!FakUserInterface.EventTilt && koltsegkod.Text == "")
                koltsegkod.SelectedIndex = -1;
        }

        private void Semaugyek()
        {
            DataView[] semaviewk = new DataView[] {bevsemainfo.DataView,koltssemainfo.DataView};
            Tablainfo[] csopinfo = new Tablainfo[] {termcsopinfo,koltscsopinfo};
            DataGridView[] gridview = new DataGridView[] {dataGridView2,dataGridView3};
            ComboBox[] combok = new ComboBox[]{bevetelfeloszt,koltsegfeloszt};
            string[] nevek = new string[]{"TERMSEMA_ID","SEMA_ID"};
            for (int j = 0; j < 2; j++)
            {
                if (combok[j].Text == "")
                {
                    gridview[j].Visible = false;
                }
                else
                {
                    gridview[j].Visible = true;
                    gridview[j].Rows.Clear();
                    ArrayList ar = new ArrayList();
                    string csopszov = vezpartnerinfo.InputColumns[nevek[j]].ComboAktSzoveg;
                    char[] vesszo = new char[] { Convert.ToChar("/") };
                    string[] split;
                    DataView semaview = semaviewk[j];

                    for (int i = 0; i < semaview.Count; i++)
                    {
                        string szov = semaview[i].Row["SZOVEG"].ToString();
                        if (szov.StartsWith(csopszov))
                        {
                            split = szov.Split(vesszo);
                            string[] sor = new string[] { split[0] + "/" + split[1], split[2] };
                            gridview[j].Rows.Add(sor);
                        }
                    }
                }
            }

        }

    }
}
