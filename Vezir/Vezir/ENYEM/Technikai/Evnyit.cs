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
    public partial class Evnyit : ControlAlap
    {
        private VezerloControl VezerloControl;
        private Tablainfo cegevinfo = UserParamTabla.Cegevinfo;
        private string aktev;
        public Evnyit(FakUserInterface fak, VezerloControl hivo, Vezerloinfo aktivvezerles)
        {
            InitializeComponent();
            ParameterAtvetel(fak, hivo, aktivvezerles);
            VezerloControl = hivo;
            AlapinfoInit(new object[] { new object[] { Alapinfotipus.Alap, new Panel[] { panel1} } });
            teljesrogzit.Text = "Év újranyitása";
            teljeselolrol.Visible = false;
            Tabinfo = cegevinfo;
        }
        public override void Ujcontroloktolt()
        {
            ValtozasTorol();
            aktev = UserParamTabla.LezartEvek[UserParamTabla.LezartEvek.Count-1].ToString();
            textBox1.Text = aktev;
            teljesrogzit.Enabled = UserParamTabla.CegTermeszetesJogosultsag == Base.HozferJogosultsag.Irolvas;
        }
        public override void teljesrogzit_Click(object sender, EventArgs e)
        {

            cegevinfo.DataView.RowFilter = "EV = '" + aktev + "'";
            DataRow dr = cegevinfo.DataView[0].Row;
            dr["LEZART"] = "N";
            dr["MODOSITOTT_M"] = 1;
            cegevinfo.Modositott = true;
            cegevinfo.DataView.RowFilter = "";
            FakUserInterface.Rogzit(cegevinfo);
            ValtozasBeallit();
            UserParamTabla.AktualCegInformacio.LezartEv = false;
            UserParamTabla.SetKozosAllapotok();
            UserParamTabla.LezartNemLezarEv();
            if (UserParamTabla.LezartEvek != null && UserParamTabla.LezartEvek.Count != 0)
                Ujcontroloktolt();
            else
            {
                AktivDropDownItem.Enabled = false;
                this.Visible = false;
            }
        }
    }
}
