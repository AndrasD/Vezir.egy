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
    public partial class Penzmozgas : AlapParameterControl
    {
        private Tablainfo nyitozaroinfo;
        private Tablainfo penzmozgasinfo;
        private string ev;
        public Penzmozgas()
        {
            InitializeComponent();
        }
        public void PenzmozgasInit(FakUserInterface fak, VezerloControl hivo, Vezerloinfo aktivvezerles)
        {
            TermCegPluszCegalattiTabinfok = hivo.TermCegPluszCegalattiTabinfok;
            ValasztekTablaNev = "PARTNER";
            ValasztekIdNev = "PARTNER_ID";
            DatasetTablak = new DataTable[] { dataset.CEGEV,dataset.NYITOZARO,dataset.PENZMOZGASOK };
            AlaplistaControlInit(fak, hivo, aktivvezerles, DatasetTablak);
            nyitozaroinfo = TermCegPluszCegalattiTabinfok["NYITOZARO"];
            penzmozgasinfo = TermCegPluszCegalattiTabinfok["PENZMOZGASOK"];
        }
        public override void EgyediEgyszeruInit()
        {
        }
        public override ReportDocument EgyediReportIni(string listanev)
        {
            ListaNev = listanev;
            switch (listanev)
            {
                case "Penzmozgasrpt":
                    return new Penzmozgasrpt();
                default:
                    return null;
            }
        }
        public override void DataSetTolt()
        {
                ev = DatumtolString.Substring(0, 4);
                string aktcegnevid = FakUserInterface.GetTartal(UserParamTabla.Cegevinfo, "CEGEV_ID", "EV", ev)[0];
                FakUserInterface.Select(nyitozaroinfo.Adattabla, FakUserInterface.AktualCegconn, "NYITOZARO", " where CEGEV_ID = " + aktcegnevid, "", false);
                string sel = "";
                sel = " where YEAR(MOZGASDATUM)=" + ev;
                FakUserInterface.Select(penzmozgasinfo.Adattabla, FakUserInterface.AktualCegconn, "PENZMOZGASOK", sel, "", false);
                base.DataSetTolt();
        }
        public override void EgyediListaResz()
        {
            reportdoc.SetDataSource(dataset);
            ReportSource = reportdoc;
            reportdoc.SetParameterValue("cegnev", UserParamTabla.AktualCegInformacio.CegNev);
            reportdoc.SetParameterValue("listacim",ev);
        }
    }
}
