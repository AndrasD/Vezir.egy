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
    
    public partial class Bevszla : Szulogyerekvaltozasok
    {
        private VezerloControl VezerloControl;
        private Controltipus szla;
        private Controltipus szlatetel;
        private Tablainfo szlainfo;
        private Tablainfo tetelinfo;
        private Tablainfo afainfo;
        private Tablainfo partnerinfo;
        private Tablainfo vezirpartnerinfo;
        private Tablainfo termfocsopinfo;
        private Tablainfo termalcsopinfo;
        private Tablainfo termcsopinfo;
        private Tablainfo termekkodok;
        private Tablainfo termfocsopalcsop;
        private Tablainfo termalcsopcsop;
        private Tablainfo termcsopkod;
        private Tablainfo szazalekinfo;
        private Tablainfo szazalekosfeloszt;
        private decimal maradekossz = 0;
        private int tetelekszama = 0;
        private decimal bruttoossz = 0;
        private decimal bruttoosszegzes = 0;
        private decimal afaszazalek = 0;
        private decimal egynetto = 0;
        private decimal egybrutto = 0;
        private MezoTag nettotag;
        private MezoTag bruttotag;
        private MezoTag termekkodtag;
        private MezoTag termekfocsoptag;
        private MezoTag termekalcsoptag;
        private MezoTag termekcsoptag;
        private MezoTag partnertag;
        private MezoTag maradektag;
        private MezoTag tetelszamtag;
        private MezoTag afakulcstag;
        private string partnerid;
        private string egytermekkod = "";
        private string egytermekid = "";
        private UjPartner ujpartnerform = null;
        private bool sajateventtilt = false;
        private string afakod;
        private string afaszov;
        private string[] semaidk = null;
        private string[] afaidk = null;
        private string egyafa = "";
        private string bevev = "";
        private string bevevho = "";
        private bool irhat = false;
        private bool semaboljott = false;
        public Bevszla(FakUserInterface fak,VezerloControl hivo,Vezerloinfo aktivvezerles)
        {
            InitializeComponent();
            ParameterAtvetel(fak, hivo, aktivvezerles);
            VezerloControl = hivo;
            panel2.Parent.Controls.Remove(panel2);
            panel111.Parent.Controls.Remove(panel111);
            toolStrip12.Items.Remove(elolrol12);
            toolStrip12.Items.Remove(rogzit12);
//            toolStrip12.Items.Remove(torol12);
            SzuloGyerekInit();
            toolStripfo.Visible = false;
            szla = ControltipusCollection.Find(groupBox1);
            szlatetel = ControltipusCollection.Find(groupBox122);
            szlainfo = szla.Tablainfo;
            tetelinfo = szlatetel.Tablainfo;
            afainfo=fak.GetKodtab("C","Afa");
            bruttotag = (MezoTag)brutto.Tag;
            nettotag = (MezoTag)netto.Tag;
            termekkodtag = (MezoTag)termekkod.Tag;
            partnertag = (MezoTag)partner.Tag;
            maradektag  = (MezoTag)maradek.Tag;
            tetelszamtag = (MezoTag)tetelszam.Tag;
            afakulcstag = (MezoTag)afakulcs.Tag;
            termekfocsoptag = (MezoTag)termfocsop.Tag;
            termekalcsoptag = (MezoTag)termalcsop.Tag;
            termekcsoptag = (MezoTag)termcsop.Tag;
            partnerinfo = FakUserInterface.GetByAzontip("SZCTPARTNER");
            vezirpartnerinfo = FakUserInterface.GetByAzontip("SZCTVEZIRPARTNER");
            termfocsopinfo = FakUserInterface.GetKodtab("C", "Termfocsop");
            termalcsopinfo = FakUserInterface.GetKodtab("C", "Termalcsop");
            termcsopinfo = FakUserInterface.GetKodtab("C", "Termcsop");
            termekkodok = FakUserInterface.GetBySzintPluszTablanev("C", "TERMEKKOD");
            termfocsopalcsop = FakUserInterface.GetOsszef("C", "Termfocsopalcsop");
            termalcsopcsop = FakUserInterface.GetOsszef("C", "Termalcsopcsop");
            termcsopkod = FakUserInterface.GetOsszef("C", "Termcsopkod");
            szazalekinfo = FakUserInterface.GetKodtab("C", "Fszazal");
            szazalekosfeloszt = FakUserInterface.GetCsoport("C", "Termfeloszt");
        }
        private void ujpartner_Click(object sender, EventArgs e)
        {
            bool volthiba = AktivPage.Text.Contains("!");
            if (ujpartnerform == null)
                ujpartnerform = new UjPartner();
            ujpartnerform.UjPartnerInit(AktivVezerles, VezerloControl, this.Name, szlainfo, partnerinfo, vezirpartnerinfo, partner, FakUserInterface);
            if (ujpartnerform.ShowDialog() == DialogResult.OK)
            {
                //                VezerloControl.ComboSzures(this.Name, partner);
                Termekkodalapertelmezes();
                //    partner.SelectedIndex = partner.Items.IndexOf(partnerinfo.Adattabla.Rows[partnerinfo.Adattabla.Rows.Count-1]["SZOVEG"].ToString());
            }
            else if (!volthiba)
            {
                AktivMenuItem.Text = AktivMenuItem.Text.Replace("!", "");
                AktivDropDownItem.Text = AktivDropDownItem.Text.Replace("!", "");
                AktivPage.Text = AktivPage.Text.Replace("!", "");
            }
        }
        public override void TabStop_Changed(object sender, EventArgs e)
        {
            if (TabStop)
            {
                bool cegvaltozas = ValtozasLekerdez("CegValtozas").Count != 0;
                bool valt = ValtozasLekerdez().Count != 0;
                szlainfo.DataView.Sort = "";
                szlainfo.DataView.RowFilter = "";
                base.TabStop_Changed(sender, e);
                if (valt)
                    VezerloControl.ComboSzures(this.Name, partner);
                if (szlainfo.ViewSorindex == -1 && szlainfo.Adattabla.Rows.Count != 0)
                    szlainfo.ViewSorindex = 0;
                DatumokAllit(szlainfo.AktualViewRow);
                if (cegvaltozas)
                {
                    if (szlainfo.Valtozott && !szla.Uj)
                    {
                        foreach (MezoTag egytag in szla.MezoControlInfo.Inputeleminfok)
                        {
                            if (egytag.Control.Name != "MARADEK" && egytag.Hibaszov != "")
                                egytag.SetHibaSzov("");
                        }
                        szlainfo.Modositott = false;
                        szlainfo.Changed = false;
                    }
                }
            }
        }
        private void IrhatAllitasok()
        {
            irhat = szla.HozferJog == Base.HozferJogosultsag.Irolvas;
            string mar = "0";
            if (irhat)
            {
                if (!szla.Uj && fizetve.Checked)
                    irhat = false;
                mar = maradek.Text.ToString();
                if (mar == "0")
                    uj1.Enabled = !szla.Uj && szla.HozferJog == HozferJogosultsag.Irolvas;
                else
                    uj1.Enabled = false;
            }
            torol1.Enabled = !szla.Uj; //&& irhat;
            ok1.Visible = true;
            ok1.Enabled = true;
            //if (mar != "0" && irhat)
            //    uj12.Enabled = true;
            uj12.Enabled = mar != "0" && irhat;
            torol12.Enabled = irhat && tetelinfo.AktualViewRow != null;
            FakUserInterface.EventTilt = true;
            foreach (MezoTag egytag in szla.MezoControlInfo.Inputeleminfok)
            {
                string nev = egytag.Control.Name;
                switch (nev)
                {
                    case "partner":
                        partner.Enabled = szla.Uj;
                        break;
                    case "szamlaszam":
                        szamlaszam.Enabled = irhat;
                        break;
                    case "szamladatum":
                        szamladatum.Enabled = irhat;
                        break;
                    case "fizdatum":
                        fizdatum.Enabled = irhat;
                        break;
                    case "teljdatum":
                        teljdatum.Enabled = irhat;
                        break;
                    case "megjegyzes":
                        megjegyzes.Enabled = irhat;
                        break;
                    case "folyoszamla":
                        folyoszamla.Enabled = szla.Uj;
                        break;
                    case "penztar":
                        penztar.Enabled = szla.Uj;
                        break;
                    case "fizetve":
                        break;
                    case "osszbrutto":
                        osszbrutto.ReadOnly = !irhat || !szla.Uj || tetelinfo.DataView.Count != 0;
                        break;
                    case "maradek":
                        maradek.ReadOnly = true;
                        break;
                    case "kiegyenldat":
                        kiegyenldat.Enabled = false;
                        break;
                    case "tetelszam":
                        tetelszam.Enabled = false;
                        break;
                }
            }
 //           torol12.Enabled = irhat;
            groupBox122.Enabled=irhat;
            panel112.Enabled = true;
            FakUserInterface.EventTilt = false;
        }
        private void Termekkodalapertelmezes()
        {
            partnerid = FakUserInterface.GetTartal(partnerinfo, "PARTNER_ID", "SZOVEG", partner.Text)[0];
            vezirpartnerinfo.DataView.RowFilter = "PARTNER_ID =" + partnerid;
            DataRow dr = vezirpartnerinfo.DataView[0].Row;
            vezirpartnerinfo.DataView.RowFilter = "";
            egytermekid = dr["TERMEKKOD_ID"].ToString();
            if (egytermekid != "0" && egytermekid !="")
            {
                Cols termcol = termekkodtag.Egyinp;
                int id = termcol.Combo_Info.ComboId.IndexOf(egytermekid);
                if(id!=-1)
                egytermekkod = termcol.Combo_Info.ComboInfo[id].ToString();
                if (tetelinfo.ViewSorindex == -1)
                    termekkod.Text = egytermekkod;
            }
        }
        private void DatumokAllit(DataRow dr)
        {
            DateTime szladatum = UserParamTabla.SzamlaDatumtol;
            bruttoossz = 0;
            bruttoosszegzes = 0;
            maradekossz = 0;
            tetelekszama = 0;
            label9.Visible = false;
            kiegyenldat.Visible = false;
            szamladatum.MinDate = DateTimePicker.MinimumDateTime;
            szamladatum.MaxDate = DateTimePicker.MaximumDateTime;
            fizdatum.MinDate = DateTimePicker.MinimumDateTime;
            fizdatum.MaxDate = DateTimePicker.MaximumDateTime;
            teljdatum.MinDate = DateTimePicker.MinimumDateTime;
            teljdatum.MaxDate = DateTimePicker.MaximumDateTime;
            if (dr == null)
            {
                szamladatum.Value = szladatum;
                fizdatum.Value = szladatum;
                teljdatum.Value = szladatum;
                fizetve.Checked = false;
            }
            else
            {
                szladatum = Convert.ToDateTime(dr["SZLA_DATUM"].ToString());
                szamladatum.Value = szladatum;
                fizdatum.Value = Convert.ToDateTime(dr["DATUM_FIZ"].ToString());
                teljdatum.Value = Convert.ToDateTime(dr["DATUM_TELJ"].ToString());
            }
            szamladatum.MinDate = UserParamTabla.SzamlaDatumtol;
            szamladatum.MaxDate = UserParamTabla.SzamlaDatumig;
            if (dr == null && fizdatum.Value.CompareTo(szladatum) < 0)
                fizdatum.Value = szladatum;
            fizdatum.MinDate = szladatum;
            string kovev = (UserParamTabla.SzamlaDatumig.Year + 1).ToString() + ".12.31";
            DateTime kovevvege = Convert.ToDateTime(kovev);
            fizdatum.MaxDate = kovevvege;
            if (dr == null && teljdatum.Value.CompareTo(szladatum) < 0)
                teljdatum.Value = szladatum;
            teljdatum.MinDate = szladatum;
            teljdatum.MaxDate = kovevvege;
            ArrayList ar = new ArrayList();
            DataRow dr1;
            for (int i = 0; i < szazalekosfeloszt.DataView.Count; i++)
            {
                dr1 = szazalekosfeloszt.DataView[i].Row;
                int j = ar.IndexOf(dr1["SORSZAM2"].ToString());
                if (j == -1)
                    ar.Add(dr1["SORSZAM2"].ToString());
            }
            semaidk = (string[])ar.ToArray(typeof(string));
            if (dr != null)
            {
                osszbrutto.ReadOnly = true;
                ujpartner.Visible = false;
                bruttoossz = Convert.ToDecimal(dr["OSSZBEVETEL"].ToString());
                if (tetelinfo.DataView.Count != 0)
                {
                    if (dr["PENZTAR_ID"].ToString() == "0")
                    {
                        szlainfo.InputColumns["PENZTAR_ID"].Lehetures = true;
                        szlainfo.InputColumns["FOLYOSZAMLA_ID"].Lehetures = false;
                        penztar.Text = "";
                    }
                    else
                    {
                        szlainfo.InputColumns["PENZTAR_ID"].Lehetures = false;
                        szlainfo.InputColumns["FOLYOSZAMLA_ID"].Lehetures = true;
                        folyoszamla.Text = "";
                    }
                    if (fizetve.Checked)
                    {
                        label9.Visible = true;
                        kiegyenldat.Visible = true;
                    }
                }
            }
            else
            {
                osszbrutto.ReadOnly = false;
                szlainfo.InputColumns["PENZTAR_ID"].Lehetures = true;
                szlainfo.InputColumns["FOLYOSZAMLA_ID"].Lehetures = true;
                ujpartner.Visible = HozferJog==Base.HozferJogosultsag.Irolvas;
                penztar.Text = "";
                folyoszamla.SelectedIndex = 0;
                fizetve.Checked = false;
            }
            Termekkodalapertelmezes();
//            if(dr!=null)
            if (!semaboljott)
            {
                Maradek_Tetelszam_Szamit(bruttoossz);
                IrhatAllitasok();
            }
          //  SzamlainputokEnable(tetelinfo.DataView.Count == 0);
        }
        private void Maradek_Tetelszam_Szamit(decimal osszesbrutto)
        {
            string szlaid = szlainfo.AktIdentity.ToString();
            if (tetelinfo.AktualViewRow != null)
            {
                string tetelid = tetelinfo.AktualViewRow["BEVSZLA_ID"].ToString();
                if (tetelid != szlaid && tetelid != "0")
                    return;
            }
            string ossztartal = szlainfo.InputColumns["OSSZBEVETEL"].Tartalom;
            if(osszesbrutto==0)
            {
                if (ossztartal == "")
                    ossztartal = "0";
                bruttoossz = Convert.ToDecimal(ossztartal);
            }
            else
                bruttoossz = osszesbrutto;
            if (ossztartal == "")
                ossztartal = "0";
            bruttoossz = Convert.ToDecimal(ossztartal);
            bruttoosszegzes = 0;
            for (int i = 0; i < tetelinfo.DataView.Count; i++)
                bruttoosszegzes += Convert.ToDecimal(tetelinfo.DataView[i].Row["BEVETEL"].ToString());
            maradekossz = bruttoossz - bruttoosszegzes;
            if (Math.Abs(Convert.ToInt64(maradekossz)) <= 1)
                maradekossz = 0;
            maradektag.SetValue(maradekossz.ToString());
            maradek.Text = maradekossz.ToString();
            if (maradekossz != 0)
                maradektag.SetHibaSzov(" Maradék nem 0!");
            else
            {
                maradektag.SetHibaSzov("");
                if (ValtozasLekerdez("CegValtozas").Count == 1)
                {
                    szlainfo.ModositasiHiba = false;
                    szlainfo.Changed = false;
                }
            }
            uj12.Enabled = maradekossz != 0;
            ok12.Enabled = !uj12.Enabled;
            tetelekszama = tetelinfo.DataView.Count;
            tetelszamtag.SetValue(tetelekszama.ToString());
            tetelszam.Text = tetelekszama.ToString();
            if (szlainfo.AktualViewRow != null)
            {
                decimal mar = Convert.ToDecimal(szlainfo.AktualViewRow["MARADEK"].ToString());
                int db = Convert.ToInt32(szlainfo.AktualViewRow["TETELSOROK_SZAMA"].ToString());
                if (mar != maradekossz || db != tetelekszama)
                {
                    szlainfo.AktualViewRow["MARADEK"] = maradekossz;
                    szlainfo.AktualViewRow["TETELSOROK_SZAMA"] = tetelekszama;
                    szlainfo.Modositott = true;
                    szlainfo.AktualViewRow["MODOSITOTT_M"]=1;
                }
                else if (ValtozasLekerdez("CegValtozas").Count == 1)
                {
                    szlainfo.Changed = false;
                }

            }
        }
        public override void Beallit(Controltipus egycont, int viewindex, bool kellchild)
        {
            base.Beallit(egycont, viewindex, kellchild);
            Tablainfo info = egycont.Tablainfo;
            if (info == tetelinfo)
            {
                string szov = afakulcs.Text;
                afakod = FakUserInterface.GetTartal(afainfo, "KOD", "SZOVEG", szov)[0];
                afaszazalek = Convert.ToInt32(afakod);
            }

        }
        public override void GridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            base.GridView_CellMouseClick(sender, e);
            DataGridView view = (DataGridView)sender;
            Controltipus conttip = ControltipusCollection.Find(view);
            if (conttip == szla)
            {
                DatumokAllit(szlainfo.AktualViewRow);
 //               SzamlainputokEnable(tetelinfo.DataView.Count == 0);
            }
        }
        public override bool EgyediValidalas(MezoTag egytag)
        {
            bool hiba = false;
            int id;
            if (!sajateventtilt)
            {
                FakUserInterface.EventTilt = true;
                switch (egytag.Control.Name)
                {
                    case "partner":
                        Termekkodalapertelmezes();
                        break;

                    case "folyoszamla":
                        if (folyoszamla.Text != "")
                        {
                            penztar.Text = "";
                            fizetve.Checked=false;
                            label9.Visible = false;
                            kiegyenldat.Visible = false;
                        }
                        else if (penztar.Text == "")
                        {
                            penztar.SelectedIndex = 0;
                            fizetve.Checked = true;
                            label9.Visible = true;
                            kiegyenldat.Visible = true;
                            kiegyenldat.Value = szamladatum.Value;
                            kiegyenldat.Enabled = false;
                        }
                        break;
                    case "penztar":
                        if (penztar.Text != "")
                        {
                            folyoszamla.Text = "";
                            fizetve.Checked = true;
                        }
                        else if (folyoszamla.Text == "")
                        {
                            folyoszamla.SelectedIndex = 0;
                            fizetve.Checked = false;
                        }
                        break;
                    case "szamladatum":
                        if (szamladatum.Value.CompareTo(fizdatum.MinDate) != 0)
                            fizdatum.MinDate = UserParamTabla.SzamlaDatumtol;
                        if (fizdatum.Value.CompareTo(szamladatum.Value) < 0)
                            fizdatum.Value = szamladatum.Value;
                        fizdatum.MinDate = szamladatum.Value;
                        if (szamladatum.Value.CompareTo(teljdatum.MinDate) != 0)
                            teljdatum.MinDate = UserParamTabla.SzamlaDatumtol;
                        if (teljdatum.Value.CompareTo(szamladatum.Value) < 0)
                            teljdatum.Value = teljdatum.Value;
                        teljdatum.MinDate = szamladatum.Value;
                        break;
                    case "osszbrutto":
                        bruttoossz = Convert.ToDecimal(osszbrutto.Text);
                        break;
                    case "termekkod":
                        if (termekkod.SelectedIndex != -1)
                        {
                            id = termekkodtag.Egyinp.Combo_Info.ComboInfo.IndexOf(termekkod.Items[termekkod.SelectedIndex].ToString());
                            string kodid = termekkodtag.Egyinp.Combo_Info.ComboIdk[id];
                            string afaid = FakUserInterface.GetTartal(termekkodok, "AFA_ID", "TERMEKKOD_ID", kodid)[0];
                            afakod = FakUserInterface.GetTartal(afainfo, "KOD", "SORSZAM", afaid)[0];
                            afaszov = FakUserInterface.GetTartal(afainfo, "SZOVEG", "SORSZAM", afaid)[0];
                            afakulcs.Text = afaszov;
                            afaszazalek = Convert.ToDecimal(afakod);
                            string[] csopidk = FakUserInterface.GetTartal(termcsopkod, "SORSZAM1", "SORSZAM2", kodid);
                            if (csopidk != null)
                            {
                                termcsop.Text = FakUserInterface.GetTartal(termcsopinfo, "SZOVEG", "SORSZAM", csopidk[0])[0];
                                string[] alcsopidk = FakUserInterface.GetTartal(termalcsopcsop, "SORSZAM1", "SORSZAM2", csopidk[0]);
                                if (alcsopidk != null)
                                {
                                    termalcsop.Text = FakUserInterface.GetTartal(termalcsopinfo, "SZOVEG", "SORSZAM", alcsopidk[0])[0];
                                    string[] focsopidk = FakUserInterface.GetTartal(termfocsopalcsop, "SORSZAM1", "SORSZAM2", alcsopidk[0]);
                                    if (focsopidk != null)
                                        termfocsop.Text = FakUserInterface.GetTartal(termfocsopinfo, "SZOVEG", "SORSZAM", focsopidk[0])[0];
                                }
                            }
                            Nettoafaszamit();
                        }
                        break;
                    case "netto":
                        hiba = Bruttoafaszamit();
                        break;
                }
                FakUserInterface.EventTilt = sajateventtilt;
            }
            return hiba;
        }
        private bool Bruttoafaszamit()
        {
            if (!semaboljott)
            {
                Decimal egyafa;
                egynetto = Convert.ToDecimal(netto.Text);
                afaszazalek = Convert.ToDecimal(afakod) / 100;
                egyafa = egynetto * afaszazalek;
                egyafa = Decimal.Round(egyafa, 0);
                egybrutto = egynetto + egyafa;
                bruttoossz = Convert.ToDecimal(osszbrutto.Text);
                if (egybrutto > bruttoossz)
                {
                    egybrutto = bruttoossz;
                    egyafa = egybrutto - egynetto;
                }
                afa.Text = egyafa.ToString();
                if (!afa.Text.Contains(","))
                    afa.Text += ",00";
                brutto.Text = egybrutto.ToString();
                if (!brutto.Text.Contains(","))
                    brutto.Text += ",00";
                bruttotag.SetValue(brutto.Text.ToString());
                ((MezoTag)afa.Tag).SetValue(afa.Text.ToString());
                if (szlainfo.AktualViewRow == null)
                    Maradek_Tetelszam_Szamit(0);
                else
                    Maradek_Tetelszam_Szamit(Convert.ToDecimal(szlainfo.AktualViewRow["OSSZBEVETEL"].ToString()));
                return maradektag.Hibaszov != "";
            }
            return false;
        }

        private void Nettoafaszamit()
        {
            if (!semaboljott)
            {
                afaszazalek = Convert.ToDecimal(afakod) / 100;
                egybrutto = Convert.ToDecimal(brutto.Text);
                egynetto = Decimal.Round(egybrutto / (1 + afaszazalek), 0);
                netto.Text = egynetto.ToString();
                ((MezoTag)brutto.Tag).SetValue(brutto.Text);
                ((MezoTag)netto.Tag).SetValue(netto.Text);
                Decimal egyafa = egybrutto - egynetto;
                afa.Text = egyafa.ToString();
                ((MezoTag)afa.Tag).SetValue(afa.Text);
            }
        }
        public override void ButtonokEnableAllit(Controltipus egycont, bool kellchild)
        {
            base.ButtonokEnableAllit(egycont, kellchild);
            if (egycont == szla)
                IrhatAllitasok();
            if (egycont == szlatetel)
            {
                if (HozferJog == Base.HozferJogosultsag.Csakolvas )
                    egycont.Panel.Enabled = true;
                uj12.Enabled = irhat;
                torol12.Enabled = irhat;
            }
        }
        public override void Button_Click(object sender, EventArgs e)
        {
            sajateventtilt = true;
            ToolStripButton egybut = (ToolStripButton)sender;
            ToolStrip owner = (ToolStrip)egybut.Owner;
            Controltipus conttip = ControltipusCollection.Find(owner);
            if(conttip!=null)
            {
                 int sav = szlainfo.ViewSorindex;
                 int i = conttip.ButtonokList.IndexOf(egybut);
                 string butname = conttip.ButtonNevek[i];
                 if (conttip != szla && butname == "torol")
                 {
                     if(tetelinfo.AktualViewRow==null)
                         return;
                     if (FakPlusz.MessageBox.Show(FakUserInterface.GetUzenetSzoveg("Torolheto"), "", FakPlusz.MessageBox.MessageBoxButtons.IgenNem) == FakPlusz.MessageBox.DialogResult.Igen)
                     {
                         egybrutto = Convert.ToDecimal(tetelinfo.AktualViewRow["BEVETEL"].ToString());
                         conttip.Tablainfo.Adatsortorol(conttip.Tablainfo.ViewSorindex);
                         if (szlainfo.AktualViewRow != null)
                         {
                             szlainfo.AktualViewRow["MARADEK"] = Convert.ToDecimal(szlainfo.AktualViewRow["MARADEK"].ToString()) + egybrutto;
                             szlainfo.AktualViewRow["TETELSOROK_SZAMA"] = Convert.ToInt32(szlainfo.AktualViewRow["TETELSOROK_SZAMA"].ToString()) - 1;
                             szlainfo.Modositott = true;
                             szlainfo.AktualViewRow["MODOSITOTT_M"] = 1;
                             ControltipusCollection.Rogzit(szla);
                             Beallit(szla, sav, true);
                             Button_Click(uj12, e);
                         }
                         return;
                     }
                     else
                         return;
                 }
                 bool mostok  = false;
                 if (conttip == szla && szlainfo.AktualViewRow == null)
                     mostok = true;
                 base.Button_Click(sender, e);
                 DataRow dr;
                 if (conttip == szla)
                 {
                    switch (butname)
                    {
                        case "uj":
                            DatumokAllit(null);
                            break;
                        case "ok":
                            if (!szlainfo.ModositasiHiba && szlainfo.Valtozott)
                            {
                                ujpartner.Visible = false;
                                szlainfo.AktualViewRow["CEGHONAP_ID"] = UserParamTabla.Ceghonap_Id;
                                if (fizetve.Checked)
                                    szlainfo.AktualViewRow["KIEGYENL_DATUM"] = szamladatum.Value;
                                else
                                    szlainfo.AktualViewRow["KIEGYENL_DATUM"] = DBNull.Value;
                                if (tetelinfo.DataView.Count == 0)
                                {
                                    partnerid = FakUserInterface.GetTartal(partnerinfo, "PARTNER_ID", "SZOVEG", partner.Text)[0];
                                    vezirpartnerinfo.DataView.RowFilter = "PARTNER_ID =" + partnerid;
                                    dr = vezirpartnerinfo.DataView[0].Row;
                                    vezirpartnerinfo.DataView.RowFilter = "";
                                    if (mostok)
                                    {
                                        semaboljott = true;
                                        Button_Click(uj12, e);
                                        SemaKiertekel(dr);
                                        semaboljott = false;
                                    }
                                    if (tetelinfo.DataView.Count == 0)
                                    {
                                        if (egytermekid != "0")
                                        {
                                            Cols termcol = termekkodtag.Egyinp;
                                            int id = termcol.Combo_Info.ComboId.IndexOf(egytermekid);
                                            egytermekkod = termcol.Combo_Info.ComboInfo[id].ToString();
                                            termekkod.Text = egytermekkod;
                                            megnevezes.Text = FakUserInterface.GetTartal(termekkodok, "SZOVEG", "TERMEKKOD_ID", egytermekid)[0];
                                            string[] afaidk = FakUserInterface.GetTartal(termekkodok, "AFA_ID", "TERMEKKOD_ID", egytermekid);
                                            if (afaidk != null)
                                            {
                                                afakod = FakUserInterface.GetTartal(afainfo, "KOD", "SORSZAM", afaidk[0])[0];
                                                afakulcs.Text = FakUserInterface.GetTartal(afainfo, "SZOVEG", "KOD", afakod)[0];
                                                afaszazalek = Convert.ToDecimal(afakod);
                                            }
                                            brutto.Text = osszbrutto.Text;
                                            Nettoafaszamit();
                                            if (!tetelinfo.ModositasiHiba)
                                            {
                                                FakUserInterface.EventTilt = true;
                                                Button_Click(ok12, e);
                                                szla.Panel.Enabled = true;
                                                szla.InputGroupBox.Enabled = true;
                                                szla.InputGroupBox.Focus();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (!Bruttoafaszamit() && maradekossz == 0)
                                            base.Button_Click(rogzit1, e);
                                        DatumokAllit(szlainfo.AktualViewRow);
                                    }
                                }
                            }
 //                           IrhatAllitasok();
                            DatumokAllit(szlainfo.AktualViewRow);
                            break;
                        case "torol":
                            if (szlainfo.DataView.Count == 0)
                                ujpartner.Visible = HozferJog == Base.HozferJogosultsag.Irolvas; 
                            break;
                        case "elolrol":
                            DatumokAllit(szlainfo.AktualViewRow);
                            if (maradek.Text == "0")
                            {
                                szla.InputGroupBox.Enabled = true;
                                szla.InputGroupBox.Focus();
                            }
                            else
                                Button_Click(uj12, e);
                            break;
                        case "elozo":
                            DatumokAllit(szlainfo.AktualViewRow);
                            break;
                        case "kovetkezo":
                            DatumokAllit(szlainfo.AktualViewRow);
                            break;
                        case "rogzit":
                            DatumokAllit(szlainfo.AktualViewRow);
                            break;
                   }
                }
                else
                {
                    switch (butname)
                    {
                        case "uj":
                            brutto.Text = maradek.Text;
                            if (egytermekid != "0")
                                termekkod.Text = egytermekkod;
                            else
                                termekkod.Text = "";
                            ((MezoTag)megnevezes.Tag).SetValue(termekkod.Text);
                            int id = termekkodtag.Egyinp.Combo_Info.ComboInfo.IndexOf(termekkod.Text);
                            if (id != -1)
                            {
                                string egyid = termekkodtag.Egyinp.Combo_Info.ComboId[id].ToString();
                                string[] afaidk = FakUserInterface.GetTartal(termekkodok, "AFA_ID", "TERMEKKOD_ID", egyid.ToString());
                                if (afaidk != null)
                                {
                                    afakod = FakUserInterface.GetTartal(afainfo, "KOD", "SORSZAM", afaidk[0])[0];
                                    afakulcs.Text = FakUserInterface.GetTartal(afainfo, "SZOVEG", "KOD", afakod)[0];
                                }
                                Nettoafaszamit();
                            }
                            uj12.Enabled = false;
                            break;
                        case "ok":
                            if (!tetelinfo.ModositasiHiba && tetelinfo.Valtozott)
                            {
                                dr = tetelinfo.AktualViewRow;
                                dr["CEGHONAP_ID"] = UserParamTabla.Ceghonap_Id;
                                dr["BEVETEL"] = brutto.Text;
                                int id1 = termekkodtag.Egyinp.Combo_Info.ComboInfo.IndexOf(termekkod.Text);
                                string termkodid = termekkodtag.Egyinp.Combo_Info.ComboIdk[id1];
                                dr["TERMEKKOD_ID"] = termkodid;
                                string[] csopidk = FakUserInterface.GetTartal(termcsopkod, "SORSZAM1", "SORSZAM2", termkodid);
                                if (csopidk != null)
                                {
                                    termcsop.Text = FakUserInterface.GetTartal(termcsopinfo, "SZOVEG", "SORSZAM", csopidk[0])[0];
                                    dr["TERMCSOP_ID"] = csopidk[0];
                                    string[] alcsopidk = FakUserInterface.GetTartal(termalcsopcsop, "SORSZAM1", "SORSZAM2", csopidk[0]);
                                    if (alcsopidk != null)
                                    {
                                        termalcsop.Text = FakUserInterface.GetTartal(termalcsopinfo, "SZOVEG", "SORSZAM", alcsopidk[0])[0];
                                        dr["TERMALCSOP_ID"] = alcsopidk[0];
                                        string[] focsopidk = FakUserInterface.GetTartal(termfocsopalcsop, "SORSZAM1", "SORSZAM2", alcsopidk[0]);
                                        if (focsopidk != null)
                                        {
                                            termfocsop.Text = FakUserInterface.GetTartal(termfocsopinfo, "SZOVEG", "SORSZAM", focsopidk[0])[0];
                                            dr["TERMFOCSOP_ID"] = focsopidk[0];
                                        }
                                    }
                                }
                                string afaid = FakUserInterface.GetTartal(termekkodok, "AFA_ID", "TERMEKKOD_ID", termkodid)[0];
                                afakod = FakUserInterface.GetTartal(afainfo, "KOD", "SORSZAM", afaid)[0];
                                afaszov = FakUserInterface.GetTartal(afainfo, "SZOVEG", "SORSZAM", afaid)[0];
                                afaszazalek = Convert.ToInt32(afakod);
                                afakulcs.Text = afaszov;
                                dr["AFAKULCS"] = afaszov;
                                if (!semaboljott)
                                {
                                    if (!Bruttoafaszamit())
                                    {
                                        dr["AFA"] = afa.Text;
                                        dr["NETTO"] = egynetto;
                                        dr["BEVETEL"] = egybrutto;
                                    }
                                    if (maradekossz == 0)
                                    {
                                        uj12.Enabled = false;
                                    }
                                    else
                                    {
                                        Button_Click(uj12, e);
                                        uj12.Enabled = false;
                                    }
                                }
                            }
                            break;
                        default:
                            Maradek_Tetelszam_Szamit(0);
                            if (butname == "torol")
                                szlainfo.Modositott = true;
                            if (maradekossz == 0)
                            {
                                uj12.Enabled = false;
                                ok12.Enabled = true;
                            }
                            else
                            {
                                uj12.Enabled = true;
                                ok12.Enabled = false;
                                rogzit1.Enabled = false;
                                if (butname != "torol")
                                {
                                    base.Button_Click(uj12, e);
                                    Button_Click(uj12, e);
                                }
                                else
                                {
                                    uj12.Enabled = true;
                                    ok12.Enabled = false;
                                    //                                    ok12.Enabled = false;
                                }

                            }
                            break;
                    }
                }
            }
            FakUserInterface.EventTilt = false;
            sajateventtilt = false;
        }
        private void SemaKiertekel(DataRow dr)
        {
            string semaid = dr["TERMSEMA_ID"].ToString();
            if (semaid != "" && semaid != "0")
            {

                int osszazalek = 0;
                termcsopkod.DataView.RowFilter = "SORSZAM1=" + semaid;
                int count = termcsopkod.DataView.Count;
                ArrayList termidk = new ArrayList();
                string termid;
                ArrayList szazalekok = new ArrayList();
                ArrayList szovegek = new ArrayList();
                string szoveg;
                ArrayList afaidkar = new ArrayList();
                ArrayList afaszovegek = new ArrayList();

                for (int ii = 0; ii < count; ii++)
                {
                    DataRow dr1 = termcsopkod.DataView[ii].Row;
                    termid = dr1["SORSZAM2"].ToString();
                    szoveg = FakUserInterface.GetTartal(termekkodok, "SZOVEG", "TERMEKKOD_ID", termid)[0];
                    string id = dr1["SORSZAM"].ToString();
                    string[] szazalidk = FakUserInterface.GetTartal(szazalekosfeloszt, "SORSZAM2", "SORSZAM1", id);
                    if (szazalidk != null && szazalidk[0] != "0")
                    {
                        termidk.Add(termid);
                        string kod = FakUserInterface.GetTartal(szazalekinfo, "KOD", "SORSZAM", szazalidk[0])[0];
                        szazalekok.Add(kod);
                        osszazalek = osszazalek + Convert.ToInt32(kod);
                        szovegek.Add(szoveg);
                        afaidk = FakUserInterface.GetTartal(termekkodok, "AFA_ID", "TERMEKKOD_ID", termid);
                        for (int j = 0; j < afaidk.Length; j++)
                        {
                            egyafa = FakUserInterface.GetTartal(afainfo, "KOD", "SORSZAM", afaidk[j])[0];
                            afaidkar.Add(egyafa);
                            afaszovegek.Add(FakUserInterface.GetTartal(afainfo, "SZOVEG", "KOD", egyafa)[0]);
                        }
                    }
                }
                termcsopkod.DataView.RowFilter = "";
                if (szazalekok.Count == 0)
                    return;
                decimal[] bruttok = new decimal[szazalekok.Count];
                decimal[] afak = new decimal[szazalekok.Count];
                decimal[] nettok = new decimal[szazalekok.Count];
                bruttoossz = Convert.ToDecimal(szlainfo.AktualViewRow["OSSZBEVETEL"].ToString());
                decimal afaszazal = Convert.ToDecimal(afaidkar[0].ToString());
                decimal nettoossz = Decimal.Round((bruttoossz / (1 + afaszazal / 100)), 0);
                decimal bmaradek = bruttoossz;
                for (int ii = 0; ii < szazalekok.Count; ii++)
                {
                    decimal egyszazal = Convert.ToDecimal(szazalekok[ii].ToString());
                    bruttok[ii] = Decimal.Round(bruttoossz * egyszazal / 100, 0);
                    bmaradek = bmaradek - bruttok[ii];
                    if (ii == szazalekok.Count - 1)
                    {
                        bruttok[ii] = bruttok[ii] + bmaradek;
                        bmaradek = 0;
                    }
                    afaszazal = Convert.ToDecimal(afaidkar[ii].ToString());
                    nettok[ii]= Decimal.Round((bruttok[ii]/(1+afaszazal/100)),0);
                    afak[ii] = bruttok[ii] - nettok[ii];
 //                   bmaradek = bmaradek - bruttok[ii];
                    Cols termcol = termekkodtag.Egyinp;
                    int id = termcol.Combo_Info.ComboId.IndexOf(termidk[ii]);
                    egytermekkod = szovegek[ii].ToString();
                    termekkod.Text = egytermekkod;
                    megnevezes.Text = egytermekkod;
                    afakulcs.Text = afaszovegek[ii].ToString();
                    brutto.Text = bruttok[ii].ToString();
                    netto.Text = nettok[ii].ToString();
                    afa.Text = afak[ii].ToString();
                    Button_Click(ok12, new EventArgs());
                    if (ii != szazalekok.Count - 1)
                        Button_Click(uj12, new EventArgs());
                    else
                    {
                        szlainfo.AktualViewRow["MARADEK"] = bmaradek;
                        szlainfo.AktualViewRow["TETELSOROK_SZAMA"] = szazalekok.Count;
                    }
                }
            }

        }
        public override bool RogzitesElott()
        {
            return VezerloControl.RogzitesElott();
        }

        private void termekkod_DropDownClosed(object sender, EventArgs e)
        {
            EgyediValidalas(termekkodtag);
        }

    }
}
