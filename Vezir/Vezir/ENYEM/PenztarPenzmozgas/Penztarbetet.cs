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
    public partial class Penztarbetet : Szulogyerekvaltozasok
    {
        private VezerloControl VezerloControl;
        private Controltipus mozgascontrol;
        private Tablainfo mozgasinfo;
        private MezoTag termekkodtag;
        private Tablainfo koltsfocsopinfo;
        private Tablainfo koltsalcsopinfo;
        private Tablainfo koltscsopinfo;
        private Tablainfo koltsegkodok;
        private Tablainfo termfocsopinfo;
        private Tablainfo termalcsopinfo;
        private Tablainfo termcsopinfo;
        private Tablainfo termekkodok;
        private Tablainfo termfocsopalcsop;
        private Tablainfo termalcsopcsop;
        private Tablainfo termcsopkod;
        private Tablainfo koltsfocsopalcsop;
        private Tablainfo koltsalcsopcsop;
        private Tablainfo koltscsopkod;
        public Penztarbetet(FakUserInterface fak, VezerloControl hivo, Vezerloinfo aktivvezerles)
        {
            InitializeComponent();
            ParameterAtvetel(fak, hivo, aktivvezerles);
            VezerloControl = hivo;
            panel1.Controls.Remove(panel12);
            SzuloGyerekInit();
            termfocsopinfo = FakUserInterface.GetKodtab("C", "Termfocsop");
            termalcsopinfo = FakUserInterface.GetKodtab("C", "Termalcsop");
            termcsopinfo = FakUserInterface.GetKodtab("C", "Termcsop");
            termekkodok = FakUserInterface.GetBySzintPluszTablanev("C", "TERMEKKOD");
            termfocsopalcsop = FakUserInterface.GetOsszef("C", "Termfocsopalcsop");
            termalcsopcsop = FakUserInterface.GetOsszef("C", "Termalcsopcsop");
            termcsopkod = FakUserInterface.GetOsszef("C", "Termcsopkod");
            termekkodtag = (MezoTag)termekkod.Tag;
            koltsfocsopinfo = FakUserInterface.GetKodtab("C", "Koltsfocsop");
            koltsalcsopinfo = FakUserInterface.GetKodtab("C", "Koltsalcsop");
            koltscsopinfo = FakUserInterface.GetBySzintPluszTablanev("C", "KOLTSEGCSOPORT");
            koltsegkodok = FakUserInterface.GetBySzintPluszTablanev("C", "KOLTSEGKOD");
            koltsfocsopalcsop = FakUserInterface.GetOsszef("C", "Koltsfocsopalcsop");
            koltsalcsopcsop = FakUserInterface.GetOsszef("C", "Koltsalcsopcsop");
            koltscsopkod = FakUserInterface.GetOsszef("C", "Koltscsopkod");
            dataGridView1.Columns.Remove(dataGridView1.Columns["KIVET"]);
            dataGridView1.Columns.Remove(dataGridView1.Columns["KOLTSEGKOD_ID_K"]);
            toolStripfo.Visible = false;
            mozgascontrol = ControltipusCollection.Find(groupBox1);
            mozgasinfo = mozgascontrol.Tablainfo;
        }
        public override void Controloktolt(Controltipus egycont, bool force, bool kellchild, bool kellfocus)
        {
            HozferJog = UserParamTabla.AktualTermeszetesJogosultsag;
            mozgascontrol.UserFilter = "BETET <> 0";
            base.Controloktolt(egycont, force, kellchild, kellfocus);
            egycont.HozferJog = HozferJog;
            DatumokAllit(mozgasinfo.AktualViewRow);
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
            if (conttip != null)
            {
                int i = conttip.ButtonokList.IndexOf(egybut);
                string butname = conttip.ButtonNevek[i];
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
                        DataRow dr = mozgasinfo.AktualViewRow;
                        dr["KIVET"] = 0;
                        if (!mozgasinfo.ModositasiHiba && mozgasinfo.Valtozott && termekkod.Text != "")
                        {
                            string[] csopidk;
                            int id1 = termekkodtag.Egyinp.Combo_Info.ComboInfo.IndexOf(termekkod.Text);
                            string termkodid = termekkodtag.Egyinp.Combo_Info.ComboIdk[id1];
                            dr["TERMEKKOD_ID"] = termkodid;
                            string koltskod = FakUserInterface.GetTartal(koltsegkodok, "KOLTSEGKOD_ID", "TERMEKKOD_ID", termkodid)[0];
                            dr["KOLTSEGKOD_ID"] = koltskod;
                            csopidk = FakUserInterface.GetTartal(termcsopkod, "SORSZAM1", "SORSZAM2", termkodid);
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
                            csopidk = FakUserInterface.GetTartal(koltscsopkod, "SORSZAM1", "SORSZAM2", koltskod);

                            if (csopidk != null)
                            {
                                dr["KOLTSCSOP_ID"] = csopidk[0];
                                string[] alcsopidk = FakUserInterface.GetTartal(koltsalcsopcsop, "SORSZAM1", "SORSZAM2", csopidk[0]);
                                if (alcsopidk != null)
                                {
                                    dr["KOLTSALCSOP_ID"] = alcsopidk[0];
                                    string[] focsopidk = FakUserInterface.GetTartal(koltsfocsopalcsop, "SORSZAM1", "SORSZAM2", alcsopidk[0]);
                                    if (focsopidk != null)
                                    {
                                        dr["KOLTSFOCSOP_ID"] = focsopidk[0];
                                    }
                                }
                            }


                        }
                        break;
                }
            }
        }
    }
}
