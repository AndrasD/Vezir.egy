using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FakPlusz;
using FakPlusz.Alapcontrolok;
using FakPlusz.Alapfunkciok;
using FakPlusz.VezerloFormok;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Windows.Forms;
using CrystalDecisions.Shared;

namespace Vezir
{
    public partial class Eveseredmeny :AlapParameterControl
    {
        private Tablainfo bevkiadinfo;
        string ev;
        public Eveseredmeny()
        {
            InitializeComponent();
        }
        public void EveseredmenyInit(FakUserInterface fak, VezerloControl hivo, Vezerloinfo aktivvezerles)
        {
            TermCegPluszCegalattiTabinfok = hivo.TermCegPluszCegalattiTabinfok;
            bevkiadinfo = TermCegPluszCegalattiTabinfok["BEVETELKIADAS"];
            DatasetTablak = new DataTable[] {dataset.BEVETELKIADAS};
            AlaplistaControlInit(fak, hivo, aktivvezerles, DatasetTablak);
        }
        public override ReportDocument EgyediReportIni(string listanev)
        {
            ListaNev = listanev;
            switch (listanev)
            {
                case "Bevkiad":
                    return new Bevkiad();
                default:
                    return null;
            }
        }
        public override void DataSetTolt()
        {
            bevkiadinfo.Adattabla.Rows.Clear();
            ev=DatumtolString.Substring(0,4);
            FakUserInterface.Select(bevkiadinfo.Adattabla, FakUserInterface.AktualCegconn, "BEVETELKIADAS", " where BEVKIADEV='" + ev + "'", "", false);
            bevkiadinfo.Tartalmaktolt();
            base.DataSetTolt();
        }

        public override void EgyediListaResz()
        {
            reportdoc.SetDataSource(dataset);
            ReportSource = reportdoc;
            reportdoc.SetParameterValue("cegnev", UserParamTabla.AktualCegInformacio.CegNev);
            reportdoc.SetParameterValue("listacim", ev);
        }
    }
}
