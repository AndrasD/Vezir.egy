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
    public partial class Afaforgalom : AlapParameterControl
    {
        private Tablainfo afainfo;
        string ev;
 //       private ReportDocument reportdoc;
        public Afaforgalom()
        {
            InitializeComponent();

        }
        public void AfaforgalomInit(FakUserInterface fak, VezerloControl hivo, Vezerloinfo aktivvezerles)
        {
            TermCegPluszCegalattiTabinfok = hivo.TermCegPluszCegalattiTabinfok;
            afainfo = TermCegPluszCegalattiTabinfok["AFAEGYENLEG"];
            DatasetTablak = new DataTable[] { dataset.CEGEV,dataset.AFAEGYENLEG };
            AlaplistaControlInit(fak, hivo, aktivvezerles, DatasetTablak);
        }
        public override ReportDocument EgyediReportIni(string listanev)
        {
            ListaNev = listanev;
            switch (listanev)
            {
                case "Afarpt":
                     return new Afarpt();
//                    return reportdoc;
                default:
                    return null;
            }
        }
        public override void DataSetTolt()
        {
            afainfo.Adattabla.Rows.Clear();
            afainfo.DataView.RowFilter = "";
            ev = DatumtolString.Substring(0, 4);
            FakUserInterface.Select(afainfo.Adattabla, FakUserInterface.AktualCegconn, "AFAEGYENLEG", " where BEVKIADEV='" + ev + "'", "", false);
            afainfo.Tartalmaktolt();
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
