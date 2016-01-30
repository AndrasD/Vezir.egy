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
    public partial class Penztarkivet : Szulogyerekvaltozasok
    {
        private VezerloControl VezerloControl;
        private Controltipus mozgascontrol;
        private Tablainfo mozgasinfo;
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
        private MezoTag koltsegkodtag;
        public Penztarkivet(FakUserInterface fak, VezerloControl hivo, Vezerloinfo aktivvezerles)
        {
            InitializeComponent();
            ParameterAtvetel(fak, hivo, aktivvezerles);
            VezerloControl = hivo;
            panel1.Controls.Remove(panel12);
            SzuloGyerekInit();
            toolStripfo.Visible = false;
            mozgascontrol = ControltipusCollection.Find(groupBox1);
            mozgasinfo = mozgascontrol.Tablainfo;
            koltsfocsopinfo = FakUserInterface.GetKodtab("C", "Koltsfocsop");
            koltsalcsopinfo = FakUserInterface.GetKodtab("C", "Koltsalcsop");
            koltscsopinfo = FakUserInterface.GetBySzintPluszTablanev("C", "KOLTSEGCSOPORT");
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
            koltsegkodtag = (MezoTag)koltsegkod.Tag;
            dataGridView1.Columns.Remove(dataGridView1.Columns["BETET"]);
            dataGridView1.Columns.Remove(dataGridView1.Columns["TERMEKKOD_ID_K"]);
        }
        public override void Controloktolt(Controltipus egycont, bool force, bool kellchild, bool kellfocus)
        {
            HozferJog = UserParamTabla.AktualTermeszetesJogosultsag;
            mozgascontrol.UserFilter = "KIVET <> 0";
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
        public override void GridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            base.GridView_CellMouseClick(sender, e);
            DatumokAllit(mozgasinfo.AktualViewRow);
        }
        public override bool RogzitesElott()
        {
            return VezerloControl.RogzitesElott();
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
                       dr["BETET"] = 0;
                       if (!mozgasinfo.ModositasiHiba && mozgasinfo.Valtozott && koltsegkod.Text != "")
                        {
                            string[] csopidk;
                            int id1 = koltsegkodtag.Egyinp.Combo_Info.ComboInfo.IndexOf(koltsegkod.Text);
                            string kodid = koltsegkodtag.Egyinp.Combo_Info.ComboIdk[id1];
                            string termkod = FakUserInterface.GetTartal(koltsegkodok, "TERMEKKOD_ID", "KOLTSEGKOD_ID", kodid)[0];
                            dr["TERMEKKOD_ID"] = termkod;
                            csopidk = FakUserInterface.GetTartal(termcsopkod, "SORSZAM1", "SORSZAM2", termkod);
                            if (csopidk != null)
                            {
                                dr["TERMCSOP_ID"] = csopidk[0];
                                string[] alcsopidk = FakUserInterface.GetTartal(termalcsopcsop, "SORSZAM1", "SORSZAM2", csopidk[0]);
                                if (alcsopidk != null)
                                {
                                    dr["TERMALCSOP_ID"] = alcsopidk[0];
                                    string[] focsopidk = FakUserInterface.GetTartal(termfocsopalcsop, "SORSZAM1", "SORSZAM2", alcsopidk[0]);
                                    if (focsopidk != null)
                                    {
                                        dr["TERMFOCSOP_ID"] = focsopidk[0];
                                    }
                                }
                            }
                            csopidk = FakUserInterface.GetTartal(koltscsopkod, "SORSZAM1", "SORSZAM2", kodid);

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
