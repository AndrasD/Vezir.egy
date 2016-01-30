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
    public partial class Evzar : ControlAlap
    {
        private VezerloControl VezerloControl;
        private Tablainfo cegevinfo = UserParamTabla.Cegevinfo;
        private string aktev;
        public Evzar(FakUserInterface fak, VezerloControl hivo, Vezerloinfo aktivvezerles)
        {
            InitializeComponent();
            ParameterAtvetel(fak, hivo, aktivvezerles);
            VezerloControl = hivo;
            AlapinfoInit(new object[] { new object[] { Alapinfotipus.Alap, new Panel[] { panel1} } });
            teljesrogzit.Text = "Év zárása";
            teljeselolrol.Visible = false;
            Tabinfo = cegevinfo;
        }
        public override void Ujcontroloktolt()
        {
            ValtozasTorol();
            aktev = UserParamTabla.NemLezartEvek[0].ToString();
            textBox1.Text = aktev;
            teljesrogzit.Enabled = UserParamTabla.AktualTermeszetesJogosultsag == Base.HozferJogosultsag.Irolvas;
        }
        public override void teljesrogzit_Click(object sender, EventArgs e)
        {

            cegevinfo.DataView.RowFilter = "EV = '" + aktev + "'";
            DataRow dr = cegevinfo.DataView[0].Row;
            cegevinfo.DataView.RowFilter = "";
            if (dr["KELLZARAS"].ToString() == "I")
                VezerloControl.Zaras();
            cegevinfo.DataView.RowFilter = "EV = '" + aktev + "'";
            dr = cegevinfo.DataView[0].Row;
            cegevinfo.DataView.RowFilter = "";
            dr["LEZART"] = "I";
            dr["MODOSITOTT_M"] = 1;
            cegevinfo.Modositott = true;
            cegevinfo.DataView.RowFilter = "";
            FakUserInterface.Rogzit(cegevinfo);
            //FakUserInterface.OpenProgress();
            //ArrayList ar = new ArrayList(UserParamTabla.AktualCegInformaciok);
            //int j = ar.IndexOf(UserParamTabla.AktualCegInformacio);
            //UserParamTabla.SetAktualCeginformacio(UserParamTabla.LezartCeg, j);
            //FakUserInterface.CloseProgress();
            ValtozasBeallit();
            UserParamTabla.AktualCegInformacio.LezartEv = true;
            UserParamTabla.SetKozosAllapotok();
            if(!AktivDropDownItem.Enabled)
                this.Visible = false;
            else
                Ujcontroloktolt();
        }

    }
}
