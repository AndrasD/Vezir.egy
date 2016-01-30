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
    public partial class Egyenleg : AlapParameterControl
    {
        private Tablainfo egyenleginfo;
        private Tablainfo nyitozaroinfo;
        private Tablainfo cegevinfo;
        string ev;
        public Egyenleg()
        {
            InitializeComponent();
        }
        public void EgyenlegInit(FakUserInterface fak, VezerloControl hivo, Vezerloinfo aktivvezerles)
        {
            TermCegPluszCegalattiTabinfok = hivo.TermCegPluszCegalattiTabinfok;
            egyenleginfo = TermCegPluszCegalattiTabinfok["EGYENLEG"];
            nyitozaroinfo = TermCegPluszCegalattiTabinfok["NYITOZARO"];
            cegevinfo = UserParamTabla.Cegevinfo;
            DatasetTablak = new DataTable[] { dataset.CEGEV,dataset.NYITOZARO,dataset.EGYENLEG};
            AlaplistaControlInit(fak, hivo, aktivvezerles, DatasetTablak);
        }
        public override ReportDocument EgyediReportIni(string listanev)
        {
            ListaNev = listanev;
            switch (listanev)
            {
                case "Egyenlegrpt":
                    return new Egyenlegrpt();
                default:
                    return null;
            }
        }
        public override void DataSetTolt()
        {
            ev = DatumtolString.Substring(0, 4);
            string evid = FakUserInterface.GetTartal(cegevinfo, "CEGEV_ID", "EV", ev)[0];
            FakUserInterface.Select(nyitozaroinfo.Adattabla, FakUserInterface.AktualCegconn, "NYITOZARO", " where CEGEV_ID = " + evid, "", false);
            nyitozaroinfo.Tartalmaktolt();
            FakUserInterface.Select(egyenleginfo.Adattabla, FakUserInterface.AktualCegconn, "EGYENLEG", " where CEGEV_ID =" + evid, "", false);
            egyenleginfo.Tartalmaktolt();
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
