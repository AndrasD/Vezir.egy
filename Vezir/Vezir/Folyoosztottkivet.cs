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
    public partial class Folyoosztottkivet : Szulogyerekvaltozasok
    {
        private VezerloControl VezerloControl;
        private Controltipus mozgascontrol;
        private Controltipus tetelcontrol;
        private Tablainfo mozgasinfo;
        private Tablainfo tetelinfo;
        private Tablainfo koltsfocsopinfo;
        private Tablainfo koltsalcsopinfo;
        private Tablainfo koltscsopinfo;
        private Tablainfo koltsegkodok;
        private Tablainfo termfocsopinfo;
        private Tablainfo termalcsopinfo;
        private Tablainfo termcsopinfo;
        private Tablainfo termekkodok;
        private Tablainfo koltsfocsopalcsop;
        private Tablainfo koltsalcsopcsop;
        private Tablainfo koltscsopkod;
        private Tablainfo termfocsopalcsop;
        private Tablainfo termalcsopcsop;
        private Tablainfo termcsopkod;
        private Tablainfo koltscsopcsop;
        private Tablainfo koltssemainfo;
        private Tablainfo szazalinfo;
        private MezoTag koltscsoptag;
        private MezoTag koltsfocsoptag;
        private MezoTag koltsalcsoptag;
        private MezoTag koltskodtag;
        private MezoTag termkodtag;
        private MezoTag termcsoptag;
        private MezoTag termalcsoptag;
        private MezoTag termfocsoptag;
        private MezoTag tetelszamtag;
        private bool okvolt = false;
        public Folyoosztottkivet(FakUserInterface fak, VezerloControl hivo, Vezerloinfo aktivvezerles)
        {
            InitializeComponent();
            ParameterAtvetel(fak, hivo, aktivvezerles);
            VezerloControl = hivo;
            panel12.Controls.Remove(panel2);
            panel12.Controls.Remove(panel111);
            toolStripfo.Visible = false;
            SzuloGyerekInit();
            mozgascontrol = ControltipusCollection.Find(groupBox1);
            mozgasinfo = mozgascontrol.Tablainfo;
            tetelcontrol = ControltipusCollection.Find("panel112");
            tetelinfo = tetelcontrol.Tablainfo;
            koltsfocsopinfo = FakUserInterface.GetKodtab("C", "Koltsfocsop");
            koltsalcsopinfo = FakUserInterface.GetKodtab("C", "Koltsalcsop");
            koltscsopinfo = FakUserInterface.GetBySzintPluszTablanev("C", "KOLTSEGCSOPORT");
            FakUserInterface.Combokupdate(koltscsopinfo.TablaTag);
            koltscsopcsop = FakUserInterface.GetOsszef("C", "Koltscsopkod");
            koltssemainfo = FakUserInterface.GetCsoport("C", "Feloszt");
            szazalinfo = FakUserInterface.GetKodtab("C", "Fszazal");
            koltsegkodok = FakUserInterface.GetBySzintPluszTablanev("C", "KOLTSEGKOD");
            termekkodok = FakUserInterface.GetBySzintPluszTablanev("C", "TERMEKKOD");
            termfocsopinfo = FakUserInterface.GetKodtab("C", "Termfocsop");
            termalcsopinfo = FakUserInterface.GetKodtab("C", "Termalcsop");
            termcsopinfo = FakUserInterface.GetKodtab("C", "Termcsop");
            termekkodok = FakUserInterface.GetBySzintPluszTablanev("C", "TERMEKKOD");
            termfocsopalcsop = FakUserInterface.GetOsszef("C", "Termfocsopalcsop");
            termalcsopcsop = FakUserInterface.GetOsszef("C", "Termalcsopcsop");
            termcsopkod = FakUserInterface.GetOsszef("C", "Termcsopkod");
            koltsfocsopalcsop = FakUserInterface.GetOsszef("C", "Koltsfocsopalcsop");
            koltsalcsopcsop = FakUserInterface.GetOsszef("C", "Koltsalcsopcsop");
            koltscsopkod = FakUserInterface.GetOsszef("C", "Koltscsopkod");
            koltscsoptag = (MezoTag)koltscsop.Tag;
            koltsfocsoptag = (MezoTag)koltsegfocsop.Tag;
            koltsalcsoptag = (MezoTag)koltsegalcsop.Tag;
            koltskodtag = (MezoTag)koltsegkod.Tag;
            termkodtag = (MezoTag)termekkod.Tag;
            termcsoptag = (MezoTag)termekcsoport.Tag;
            termalcsoptag = (MezoTag)termekalcsop.Tag;
            termfocsoptag = (MezoTag)termekfocsop.Tag;
            tetelszamtag = (MezoTag)tetelszam.Tag;
            mozgasinfo.TablaColumns["KOLTSCSOP_ID"].Lehetures = false;
            mozgasinfo.TablaColumns["KOLTSCSOP_ID"].ReadOnly = false;
            mozgasinfo.TablaColumns["KOLTSCSOP_ID"].Lathato = true;
            dataGridView1.Columns.Remove(dataGridView1.Columns["BETET"]);
            dataGridView1.Columns.Remove(dataGridView1.Columns["TERMEKKOD_ID_K"]);
            dataGridView1.Columns.Remove(dataGridView1.Columns["KOLTSEGKOD_ID_K"]);
        }
        public override void Ujcontroloktolt()
        {
            //mozgasinfo.TablaColumns["KOLTSCSOP_ID"].Lehetures = false;
            //mozgasinfo.TablaColumns["KOLTSCSOP_ID"].ReadOnly = false;
            //mozgasinfo.TablaColumns["KOLTSCSOP_ID"].Lathato = true;
            //koltscsop.Enabled = true;
            string[] idk = FakUserInterface.GetTartal(koltscsopinfo, "KOLTSEGCSOPORT_ID", "SEMAE", "I");
            ArrayList ar = new ArrayList();
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
            FakUserInterface.Comboinfoszures(koltscsop, (string[])ar.ToArray(typeof(string)));
            base.Ujcontroloktolt();
        }
        public override void Controloktolt(Controltipus egycont, bool force, bool kellchild, bool kellfocus)
        {
            HozferJog = UserParamTabla.AktualTermeszetesJogosultsag;
            mozgascontrol.UserFilter = "KIVET <> 0 AND TETELSOROK_SZAMA <> 0";
            base.Controloktolt(egycont, force, kellchild, kellfocus);
            egycont.HozferJog = HozferJog;
            if (egycont == mozgascontrol)
            {
                DatumokAllit(mozgasinfo.AktualViewRow);
            }
        }
        private void DatumokAllit(DataRow dr)
        {
            DateTime szladatum = UserParamTabla.SzamlaDatumtol;
            if (dr != null)
                szladatum = Convert.ToDateTime(dr["SZLA_DATUM"].ToString());
            szamladatum.MinDate = DateTimePicker.MinimumDateTime;
            szamladatum.MaxDate = DateTimePicker.MaximumDateTime;
            szamladatum.Value = szladatum;
            szamladatum.MinDate = UserParamTabla.SzamlaDatumtol;
            szamladatum.MaxDate = UserParamTabla.SzamlaDatumig;
            //mozgasinfo.TablaColumns["KOLTSCSOP_ID"].Lehetures = false;
            //mozgasinfo.TablaColumns["KOLTSCSOP_ID"].ReadOnly = false;
            //mozgasinfo.TablaColumns["KOLTSCSOP_ID"].Lathato = true;
            //koltscsop.Enabled = true;
        }
        public override void ButtonokEnableAllit(Controltipus egycont, bool kellchild)
        {
            base.ButtonokEnableAllit(egycont, kellchild);
            if (mozgasinfo.AktualViewRow != null && HozferJog == HozferJogosultsag.Irolvas)
            {
                uj1.Visible = true;
                uj1.Enabled = true;
                torol1.Visible = true;
                torol1.Enabled = true;
            }
            uj12.Visible = false;
            rogzit12.Visible = false;
            ok12.Visible = false;
            torol12.Visible = false;

        }
        public override bool RogzitesElott()
        {
            return VezerloControl.RogzitesElott();
        }
        public override void GridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            base.GridView_CellMouseClick(sender, e);
            DatumokAllit(mozgasinfo.AktualViewRow);
        }
        public override void Button_Click(object sender, EventArgs e)
        {
            ToolStripButton egybut = (ToolStripButton)sender;
            ToolStrip owner = (ToolStrip)egybut.Owner;
            Controltipus conttip = ControltipusCollection.Find(owner);
            if (conttip != null && conttip == mozgascontrol)
            {
                int i = conttip.ButtonokList.IndexOf(egybut);
                string butname = conttip.ButtonNevek[i];
                okvolt = butname == "ok";
                base.Button_Click(sender, e);
                switch (butname)
                {
                    case "uj":
                        DatumokAllit(null);
                        break;
                    case "elolrol":
                        DatumokAllit(mozgasinfo.AktualViewRow);
                        break;
                    case "elozo":
                        DatumokAllit(mozgasinfo.AktualViewRow);
                        break;
                    case "kovetkezo":
                        DatumokAllit(mozgasinfo.AktualViewRow);
                        break;
                    case "ok":
                        if (!mozgasinfo.ModositasiHiba && mozgasinfo.Valtozott)
                        {
                            DataRow dr = mozgasinfo.AktualViewRow;
                            dr["BETET"] = 0;
                            if (tetelinfo.DataView.Count != 0)
                                    tetelinfo.TeljesTorles();
                            SemaKiertekel(dr["KOLTSCSOP_ID"].ToString());
                            tetelszamtag.SetValue(tetelinfo.DataView.Count.ToString());
                            DatumokAllit(dr);
                            dr["CEGHONAP_ID"] = UserParamTabla.Ceghonap_Id;
                            Button_Click(rogzit1, e);
                        }
                        break;
                }
            }
            else
            {
                base.Button_Click(sender, e);
                int i = conttip.ButtonokList.IndexOf(egybut);
                string butname = conttip.ButtonNevek[i];
                switch (butname)
                {
                    case "uj":
                        break;
                    case "ok":
                        break;
                }
            }
        }
        public override bool EgyediValidalas(MezoTag egytag)
        {
            Control cont = egytag.Control;
            if (cont.Name == "osszbrutto" && cont.Text == "" || cont.Text == "0")
            {
                egytag.SetHibaSzov("Nem lehet üres!");
                mozgasinfo.ModositasiHiba = true;
//                egytag.Hibaszov = "Nem lehet üres!";
                return true;
            }
            if (egytag == koltscsoptag && egytag.Changed)
            {
            }

            if (egytag == tetelszamtag)
            {
                if (tetelszam.Text == "0" || tetelszam.Text == "")
                    tetelszamtag.SetValue("1");
            }
            return false;
        }
        private void SemaKiertekel(string koltsegcsopid)
        {
            if (koltsegcsopid != "" && koltsegcsopid != "0")
            {
                string koltsalcsopid = FakUserInterface.GetTartal(koltsalcsopcsop, "SORSZAM1", "SORSZAM2", koltsegcsopid)[0];
                string koltsfocsopid = FakUserInterface.GetTartal(koltsfocsopalcsop, "SORSZAM1", "SORSZAM2", koltsalcsopid)[0];
                int osszazalek = 0;
                koltscsopkod.DataView.RowFilter = "SORSZAM1=" + koltsegcsopid;
                int count = koltscsopkod.DataView.Count;
                ArrayList koltsidk = new ArrayList();
                string koltsid;
                ArrayList szazalekok = new ArrayList();
                ArrayList szovegek = new ArrayList();
                string szoveg;
                string termekkodid;
                string termcsopid;
                string termalcsopid;
                string termfocsopid;
                for (int ii = 0; ii < count; ii++)
                {
                    DataRow dr1 = koltscsopkod.DataView[ii].Row;
                    koltsid = dr1["SORSZAM2"].ToString();
                    szoveg = FakUserInterface.GetTartal(koltsegkodok, "SZOVEG", "KOLTSEGKOD_ID", koltsid)[0];
                    string id = dr1["SORSZAM"].ToString();
                    string[] szazalidk = FakUserInterface.GetTartal(koltssemainfo, "SORSZAM2", "SORSZAM1", id);
                    if (szazalidk != null && szazalidk[0] != "0")
                    {
                        koltsidk.Add(koltsid);
                        string kod = FakUserInterface.GetTartal(szazalinfo, "KOD", "SORSZAM", szazalidk[0])[0];
                        szazalekok.Add(kod);
                        osszazalek = osszazalek + Convert.ToInt32(kod);
                        szovegek.Add(szoveg);
                    }
                }

                koltscsopkod.DataView.RowFilter = "";
                decimal[] bruttok = new decimal[szazalekok.Count];
                Decimal bruttoossz = Convert.ToDecimal(mozgasinfo.AktualViewRow["KIVET"].ToString());
                decimal bmaradek = bruttoossz;
                string egykoltsegkod;
                for (int ii = 0; ii < szazalekok.Count; ii++)
                {
                    decimal egyszazal = Convert.ToDecimal(szazalekok[ii].ToString());
                    //                   bruttok[ii] = bruttoossz * egyszazal / 100;
                    bruttok[ii] = Decimal.Round(bruttoossz * egyszazal / 100, 0);
                    bmaradek = bmaradek - bruttok[ii];
                    if (ii == szazalekok.Count - 1)
                    {
                        bruttok[ii] = bruttok[ii] + bmaradek;
                        bmaradek = 0;
                    }
                    Cols koltscol = koltskodtag.Egyinp;
                    int id = koltscol.Combo_Info.ComboId.IndexOf(koltsidk[ii]);
                    termekkodid = FakUserInterface.GetTartal(koltsegkodok, "TERMEKKOD_ID", "KOLTSEGKOD_ID", koltsidk[ii].ToString())[0];
                    termcsopid = FakUserInterface.GetTartal(termcsopkod, "SORSZAM1", "SORSZAM2", termekkodid)[0];
                    termalcsopid = FakUserInterface.GetTartal(termalcsopcsop, "SORSZAM1", "SORSZAM2", termcsopid)[0];
                    termfocsopid = FakUserInterface.GetTartal(termfocsopalcsop, "SORSZAM1", "SORSZAM2", termalcsopid)[0];
                    egykoltsegkod = szovegek[ii].ToString();
                    koltsegkod.Text = egykoltsegkod;
                    megjegyzes1.Text = egykoltsegkod;
                    brutto.Text = bruttok[ii].ToString();
                    Button_Click(ok12, new EventArgs());
                    DataRow dr = tetelinfo.AktualViewRow;
                    DataRow mozgdr = mozgasinfo.AktualViewRow;
                    dr["CEGHONAP_ID"] = UserParamTabla.Ceghonap_Id;
                    dr["SZLA_DATUM"] = mozgdr["SZLA_DATUM"];
                    dr["TERMEKKOD_ID"] = termekkodid;
                    dr["TERMCSOP_ID"] = termcsopid;
                    dr["TERMALCSOP_ID"] = termalcsopid;
                    dr["TERMFOCSOP_ID"] = termfocsopid;
                    dr["KOLTSEGKOD_ID"] = koltsidk[ii];
                    dr["KOLTSCSOP_ID"] = koltsegcsopid;
                    dr["KOLTSALCSOP_ID"] = koltsalcsopid;
                    dr["KOLTSFOCSOP_ID"] = koltsfocsopid;
                    dr["FOLYOSZAMLA_ID"]=mozgdr["FOLYOSZAMLA_ID"];
                    if (ii != szazalekok.Count - 1)
                        Button_Click(uj12, new EventArgs());
                    else
                    {
                        mozgasinfo.AktualViewRow["TETELSOROK_SZAMA"] = szazalekok.Count;
                    }
                }







            }
        }

        //private void koltscsop_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (!FakUserInterface.EventTilt && koltscsop.SelectedIndex != -1)
        //    {
        //        Cols egycol = koltscsoptag.Egyinp;
        //        if (egycol.OrigTartalom != egycol.Combo_Info.ComboFileinfo[koltscsop.SelectedIndex].ToString())
        //        {
        //            koltscsoptag.EgyHibavizsg(koltscsop.Text);
        //        }
        //    }
        //}
    }
}