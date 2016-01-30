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
    public partial class Folyopenztar : Szulogyerekvaltozasok
    {
        private VezerloControl VezerloControl;
        private Controltipus mozgascontrol;
        private Tablainfo mozgasinfo;
        private bool datumvaltozas = true;
        public Folyopenztar(FakUserInterface fak, VezerloControl hivo, Vezerloinfo aktivvezerles)
        {
            InitializeComponent();
            ParameterAtvetel(fak, hivo, aktivvezerles);
            VezerloControl = hivo;
            panel1.Controls.Remove(panel12);
            SzuloGyerekInit();
            toolStripfo.Visible = false;
            mozgascontrol = ControltipusCollection.Find(groupBox1);
            mozgasinfo = mozgascontrol.Tablainfo;
        }
        public override void Controloktolt(Controltipus egycont, bool force, bool kellchild, bool kellfocus)
        {
            HozferJog = UserParamTabla.AktualTermeszetesJogosultsag;
            base.Controloktolt(egycont, force, kellchild, kellfocus);
            egycont.HozferJog = HozferJog;
            DatumokAllit(mozgasinfo.AktualViewRow);
        }
        private void DatumokAllit(DataRow dr)
        {
            DateTime szladatum = UserParamTabla.SzamlaDatumtol;
            if (dr != null)
            {
                szladatum = Convert.ToDateTime(dr["SZLA_DATUM"].ToString());
                FakUserInterface.EventTilt = true;
                groupBox1.Enabled = false;
                FakUserInterface.EventTilt = false;
            }
            else
                groupBox1.Enabled = true;
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
                }
            }
        }
    }
}
