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
    public partial class Kiadasok : AlapParameterControl
    {
        private Tablainfo partnerinfo;
        private Tablainfo partnertetelinfo;
        private string[] partneridk;
        public Kiadasok()
        {
            InitializeComponent();
        }

        public void KiadasokInit(FakUserInterface fak, VezerloControl hivo, Vezerloinfo aktivvezerles)
        {
            partnerinfo = fak.GetByAzontip("SZCTPARTNER");
            partnertetelinfo = fak.GetByAzontip("SZCTVEZIRPARTNER");
            partneridk = fak.GetTartal(partnertetelinfo, "PARTNER_ID", "KOLTSPARTNER", "I");
            TermCegPluszCegalattiTabinfok = hivo.TermCegPluszCegalattiTabinfok;
            ValasztekTablaNev = "PARTNER";
            ValasztekIdNev = "PARTNER_ID";
            DatasetTablak = new DataTable[] { dataset.CEGSZLAHONAPOK, dataset.KOLTSSZLA, dataset.KOLTSSZLA_TETEL };
            AlaplistaControlInit(fak, hivo, aktivvezerles, DatasetTablak);
            AlapTablaNev = "CEGSZLAHONAPOK";
            AlapTablaSelectString = " where SZLA_DATUM> = '" + DatumtolString + "' and SZLA_DATUM <= '" + DatumigString + "'";
            AlapIdNev = "CEGHONAP_ID";
        }
        public override void EgyediEgyszeruInit()
        {
            EgyszeruTabla = new DataTable("PARTNER");
            Sqlinterface.Select(EgyszeruTabla, FakUserInterface.AktualCegconn, "PARTNER", "" , " order by SZOVEG ", false);
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
                case "Kiadasrpt":
                    return new Kiadasrpt();
                default:
                    return null;
            }
        }
        public override void DataSetTolt()
        {
            if (AlapTabla.Rows.Count != 0)
            {
                string sel = "";
                DataTable koltsszlatabla = new DataTable();
                for (int i = 0; i < ErintettTablaInfok.Count; i++)
                {
                    Tablainfo egytabinfo = (Tablainfo)ErintettTablaInfok[i];
                    if (egytabinfo.Tablanev == "KOLTSSZLA")
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
                        Sqlinterface.Select(egytabinfo.Adattabla, egytabinfo.Adattabla.Connection, "KOLTSSZLA", sel, " order by SZLA_DATUM", false);
                        koltsszlatabla = egytabinfo.Adattabla;
                    }
                    else if (egytabinfo.Tablanev == "KOLTSSZLA_TETEL")
                    {
                        sel = "";
                        string id = "";
                        for (int j = 0; j < koltsszlatabla.Rows.Count; j++)
                        {
                            id = koltsszlatabla.Rows[j]["KOLTSSZLA_ID"].ToString();
                            if (sel == "")
                                sel = "where ";
                            else
                                sel += " or ";
                            sel += "KOLTSSZLA_ID = " + id;

                        }
                        Sqlinterface.Select(egytabinfo.Adattabla, egytabinfo.Adattabla.Connection, "KOLTSSZLA_TETEL", sel, " order by KOLTSSZLA_ID", false);

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
