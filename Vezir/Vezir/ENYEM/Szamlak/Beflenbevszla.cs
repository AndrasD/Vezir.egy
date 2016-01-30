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
    public partial class Beflenbevszla : AlapParameterControl
    {
        private Tablainfo bevszlainfo;
        string ev;
        public Beflenbevszla()
        {
            InitializeComponent();
        }
        public void BeflenbevszlaInit(FakUserInterface fak, VezerloControl hivo, Vezerloinfo aktivvezerles)
        {
            TermCegPluszCegalattiTabinfok = hivo.TermCegPluszCegalattiTabinfok;
            bevszlainfo = TermCegPluszCegalattiTabinfok["BEVSZLA"];
            DatasetTablak = new DataTable[] { dataset.CEGEV, dataset.BEVSZLA };
            AlaplistaControlInit(fak, hivo, aktivvezerles, DatasetTablak);
        }
        public override ReportDocument EgyediReportIni(string listanev)
        {
            ListaNev = listanev;
            switch (listanev)
            {
                case "Befejezetlenbevszla":
                    return new Befejezetlenbevszla();
                default:
                    return null;
            }
        }
        public override void DataSetTolt()
        {
            bevszlainfo.Adattabla.Rows.Clear();
            bevszlainfo.DataView.RowFilter = "";
            ev = DatumtolString.Substring(0, 4);
            FakUserInterface.Select(bevszlainfo.Adattabla, FakUserInterface.AktualCegconn, "BEVSZLA", " where MARADEK <> 0 ", "", false);
            bevszlainfo.Tartalmaktolt();
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
