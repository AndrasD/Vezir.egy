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
    public partial class Bevetelek : AlapParameterControl
    {
        private Tablainfo partnerinfo;
        private Tablainfo partnertetelinfo;
        private string[] partneridk;
        public Bevetelek()
        {
            InitializeComponent();
        }
        public void BevetelekInit(FakUserInterface fak, VezerloControl hivo, Vezerloinfo aktivvezerles)
        {
            partnerinfo = fak.GetByAzontip("SZCTPARTNER");
            partnertetelinfo = fak.GetByAzontip("SZCTVEZIRPARTNER");
            partneridk = fak.GetTartal(partnertetelinfo, "PARTNER_ID", "BEVPARTNER", "I");
            TermCegPluszCegalattiTabinfok = hivo.TermCegPluszCegalattiTabinfok;
            ValasztekTablaNev = "PARTNER";
            ValasztekIdNev = "PARTNER_ID";
            DatasetTablak = new DataTable[] { dataset.CEGSZLAHONAPOK, dataset.BEVSZLA, dataset.BEVSZLA_TETEL };
            AlaplistaControlInit(fak, hivo, aktivvezerles, DatasetTablak);
            AlapTablaNev = "CEGSZLAHONAPOK";
            AlapTablaSelectString = " where SZLA_DATUM> = '" + DatumtolString + "' and SZLA_DATUM <= '" + DatumigString + "'";
            AlapIdNev = "CEGHONAP_ID";
        }
        public override void EgyediEgyszeruInit()
        {
            EgyszeruTabla = new DataTable("PARTNER");
            Sqlinterface.Select(EgyszeruTabla, FakUserInterface.AktualCegconn, "PARTNER", "", " order by SZOVEG ", false);
            for (int i = 0; i < EgyszeruTabla.Rows.Count; i++)
            {
                DataRow egyrow = EgyszeruTabla.Rows[i];
                if (egyrow.RowState != DataRowState.Deleted)
                {
                    bool van = false;
                    for (int j = 0; j < partneridk.Length; j++)
                    {
                        if (egyrow["PARTNER_ID"].ToString() == partneridk[j])
                        {
                            van = true;
                            break;
                        }
                    }
                    if (!van)
                        egyrow.Delete();
                }
            }
            EgyszeruTablaView = new DataView(EgyszeruTabla);
            EgyszeruIdNev = "PARTNER_ID";
            EgyszeruMegnevColumnNev = "SZOVEG";
        }
        public override ReportDocument EgyediReportIni(string listanev)
        {
            ListaNev = listanev;
            switch (listanev)
            {
                case "Bevetelekrpt":
                    return new Bevetelekrpt();
                default:
                    return null;
            }
        }
        public override void DataSetTolt()
        {
            if (AlapTabla.Rows.Count != 0)
            {
                string sel = "";
                DataTable bevszlatabla = new DataTable();
                for (int i = 0; i < ErintettTablaInfok.Count; i++)
                {
                    Tablainfo egytabinfo = (Tablainfo)ErintettTablaInfok[i];
                    if (egytabinfo.Tablanev == "BEVSZLA")
                    {
                        sel = "";
                        for (int j = 0; j < AlapTabla.Rows.Count; j++)
                        {
                            string ceghoid = AlapTabla.Rows[j]["CEGHONAP_ID"].ToString();
                            if (sel == "")
                                sel = " where (";
                            else
                                sel += " or ";
                            sel += "CEGHONAP_ID =" + ceghoid;
                        }
                        sel += ") and (";
                        for (int j = 0; j < SzurtIdk.Count; j++)
                        {
                            sel += "PARTNER_ID =" + SzurtIdk[j].ToString();
                            if (j == SzurtIdk.Count - 1)
                                sel += ")";
                            else
                                sel += " or ";
                        }
                        Sqlinterface.Select(egytabinfo.Adattabla, egytabinfo.Adattabla.Connection, "BEVSZLA", sel, " order by SZLA_DATUM", false);
                        bevszlatabla = egytabinfo.Adattabla;
                    }
                    else if (egytabinfo.Tablanev == "BEVSZLA_TETEL")
                    {
                        sel = "";
                        string id = "";
                        for (int j = 0; j < bevszlatabla.Rows.Count; j++)
                        {
                            id = bevszlatabla.Rows[j]["BEVSZLA_ID"].ToString();
                            if (sel == "")
                                sel = "where ";
                            else
                                sel += " or ";
                            sel += "BEVSZLA_ID = " + id;

                        }
                        Sqlinterface.Select(egytabinfo.Adattabla, egytabinfo.Adattabla.Connection, "BEVSZLA_TETEL", sel, " order by BEVSZLA_ID", false);

                    }
                }
                base.DataSetTolt();
            }
        }
        public override void EgyediListaResz()
        {
            reportdoc.SetDataSource(dataset);
            ReportSource = reportdoc;
            reportdoc.SetParameterValue("cegnev", UserParamTabla.AktualCegInformacio.CegNev);
            reportdoc.SetParameterValue("evhonaptol", DatumtolString.Substring(0, 7) + "-tól");
            reportdoc.SetParameterValue("evhonapig", DatumigString.Substring(0, 7) + "-ig");
        }
    }
}
