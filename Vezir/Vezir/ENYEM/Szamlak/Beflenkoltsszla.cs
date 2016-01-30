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
    public partial class Beflenkoltsszla : AlapParameterControl
    {
        private Tablainfo koltsinfo;
        string ev;
        public Beflenkoltsszla()
        {
            InitializeComponent();
        }
        public void BeflenkoltsszlaInit(FakUserInterface fak, VezerloControl hivo, Vezerloinfo aktivvezerles)
        {
            TermCegPluszCegalattiTabinfok = hivo.TermCegPluszCegalattiTabinfok;
            koltsinfo = TermCegPluszCegalattiTabinfok["KOLTSSZLA"];
            DatasetTablak = new DataTable[] { dataset.CEGEV, dataset.KOLTSSZLA };
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
            koltsinfo.Adattabla.Rows.Clear();
            koltsinfo.DataView.RowFilter = "";
            ev = DatumtolString.Substring(0, 4);
            FakUserInterface.Select(koltsinfo.Adattabla, FakUserInterface.AktualCegconn, "KOLTSSZLA", " where MARADEK <> 0 ", "", false);
            koltsinfo.Tartalmaktolt();
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
