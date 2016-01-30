using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FakPlusz.Alapfunkciok;
using FakPlusz.Alapcontrolok;
using FakPlusz.SzerkesztettListak;
using FakPlusz.DataSetek;

namespace FakPlusz.Formok
{
    /// <summary>
    /// Leirasok, megjegyzesek reportjat allitja elo
    /// </summary>
    public partial class LeirasOsszefuggesek : Alaplista
    {
        private AltalanosDataSet dataset = new AltalanosDataSet();
        private LeirasReport report = new LeirasReport();
        private bool elso = true;
        private string alkalmnev = "";
        private bool alkalmvaltozas = true;
        private string alkalmid = "";
        private TreeView treeview;
        /// <summary>
        /// alkalmazas neve
        /// </summary>
        public string Alkalmnev
        {
            get { return alkalmnev; }
            set
            {
                if (alkalmnev != value)
                {
                    alkalmnev = value;
                    alkalmvaltozas = true;
                    if (Alkalmnev == "TERVEZO")
                        alkalmid = "";
                    else
                    {
                        Tablainfo alkalminfo = FakUserInterface.GetKodtab("R", "Alkalm");
                        alkalminfo.DataView.RowFilter = "SZOVEG ='" + alkalmnev + "'";
                        alkalmid = alkalminfo.DataView[0]["SORSZAM"].ToString();
                        alkalminfo.DataView.RowFilter = "";
                    }
                }
            }
        }
        /// <summary>
        /// objectum letrehozasa
        /// </summary>
        /// <param name="vezerles">
        /// vezerloinfo
        /// </param>
        public LeirasOsszefuggesek(Vezerloinfo vezerles)
        {
            InitializeComponent();
            Vezerles = vezerles;
            treeview = vezerles.ParentVezerles.TreeView;
            Hivo = Vezerles.Hivo;
            FakUserInterface = vezerles.Fak;
            this.Visible = false;
            //this.adatkiv.Visible = false;
            //tabControl1.Controls.Remove(datumparampage);
            //tabControl1.Controls.Remove(egyszeruparampage);
            //tabControl1.Controls.Remove(listaparampage);
            //tabControl1.Controls.Remove(osszetettparampage);
 //           this.DataSet = dataset;
            this.ReportSource = report;
            reportdoc = report;
        }
        /// <summary>
        /// nyomtat click felulbiral
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //public override void Buttonok_Click(object sender, EventArgs e)
        //{
        //    string butnev = ((ToolStripButton)sender).Name;
        //    if (butnev != "nyomtat")
        //        base.Buttonok_Click(sender, e);
        //    else
        //    {
        //        if (nyomtmin != null)
        //        {
        //            int elsopage = Convert.ToInt32(nyomtmin.SelectedItem.ToString());
        //            int utsopage = Convert.ToInt32(nyomtmax.SelectedItem.ToString());
        //            report.PrintToPrinter(1, false, elsopage, utsopage);
        //        }
        //    }
        //}
        /// <summary>
        /// aktivizalas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void TabStop_Changed(object sender, EventArgs e)
        {
            if (this.TabStop)
            {
                if (elso)
                    AlaplistaInit(FakUserInterface, Hivo, Vezerles);
                if (elso || ValtozasLekerdez().Count != 0 || alkalmvaltozas)
                {
                    ValtozasTorol();
                    InfokGyujt();
                    report.SetDataSource(dataset);
 //                   report.PrintOptions.PrinterName = PrinterName;
                    this.Visible = false;
                    this.Dock = DockStyle.Fill;
                    elso = false;
                    alkalmvaltozas = false;
                    FakUserInterface.OpenProgress();
                    FakUserInterface.SetProgressText("");
                    FakUserInterface.EventTilt = true;
                    crystalReportViewer1.Enabled = false;
                    FakUserInterface.EventTilt = false;
                    crystalReportViewer1.Enabled = true;
                    FakUserInterface.CloseProgress();
                }
                this.Visible = true;
            }
        }
        private void InfokGyujt()
        {
            TreeNode termszarmnode;
            TreeNode szintnode;
            TreeNode tartalnode;
            int alkalmind = 0;
            int termszarmind = 0;
            int szintind = 0;
            int adatfajtaind = 0;
            int tablaind = 0;
            string aktualtermszarm = "SZ";
            string aktualszint = "R";
            string aktualadatfajta = "T";
            string termszarm = "";
            string termszarmszov = "Származékos";
            string szint = "";
            string szintszov = "Rendszerszintü";
            string adatfajta = "";
            string adatfajtaszov = "";
            string tablanev = "";
            Tablainfo alkalminfo = FakUserInterface.GetKodtab("R", "Alkalm");
            TablainfoCollection termtablak = FakUserInterface.GetCegPluszCegalattiTermTablaInfok();
            DataTable dt1 = dataset.DataTable1;
            DataTable dt2 = dataset.DataTable2;
            DataTable dt3 = dataset.DataTable3;
            DataTable dt4 = dataset.DataTable4;
            DataTable dt5 = dataset.DataTable5;
            DataTable dt6 = dataset.DataTable6;
            dt1.Rows.Clear();
            dt2.Rows.Clear();
            dt3.Rows.Clear();
            dt4.Rows.Clear();
            dt5.Rows.Clear();
            dt6.Rows.Clear();
            DataRow dt1newrow = null;
            DataRow dt2newrow;
            Tablainfo tabinfo;
            DataRow row;
            termszarmind++;
            szintind++;
            tablaind++;
            if (alkalmnev == "TERVEZO")
            {
                dt1newrow = dt1.NewRow();
                dt1newrow["COLUMN1"] = alkalmind.ToString() + termszarmind.ToString() + szintind.ToString() + tablaind;
                dt1newrow["COLUMN2"] = "Származékos/Rendszerszintü/Táblázat/Táblanév:LEIRO\nMezöleirások alaptáblája";
                dt1newrow["COLUMN3"] = "Mezönév";
                dt1newrow["COLUMN4"] = "Megjelenitendö szöveg\n a sorban";
                dt1newrow["COLUMN5"] = "Megjelenitendö\nszöveg oszlopban";
                dt1newrow["COLUMN6"] = "Megjegyés:";
                dt1newrow["COLUMN20"] = alkalmind;
                dt1newrow["COLUMN19"] = termszarmind;
                dt1newrow["COLUMN18"] = szintind;
                dt1newrow["COLUMN17"] = tablaind;
                dt1.Rows.Add(dt1newrow);
                tabinfo = FakUserInterface.LeiroTag.Tablainfo;
                for (int i = 0; i < tabinfo.DataView.Count; i++)
                {
                    row = tabinfo.DataView[i].Row;
                    dt2newrow = dt2.NewRow();
                    dt2newrow["COLUMN1"] = row["ADATNEV"];
                    dt2newrow["COLUMN2"] = row["TOOLTIP"];
                    dt2newrow["COLUMN3"] = row["SORSZOV"];
                    dt2newrow["COLUMN4"] = row["OSZLSZOV"];
                    dt2newrow["COLUMN20"] = alkalmind;
                    dt2newrow["COLUMN19"] = termszarmind;
                    dt2newrow["COLUMN18"] = szintind;
                    dt2newrow["COLUMN17"] = tablaind;
                    dt2.Rows.Add(dt2newrow);
                }
                tablaind++;
            }
            dt1newrow = dt1.NewRow();
            dt1newrow["COLUMN1"] = alkalmind.ToString() + termszarmind.ToString() + szintind.ToString() + tablaind;
            dt1newrow["COLUMN2"] = "Származékos/Rendszerszintü/Táblázat/Táblanév:" + "BASE\nA fastruktúra alapja";
            dt1newrow["COLUMN3"] = "Fa node neve";
            dt1newrow["COLUMN4"]="Megjegyés:";

            dt1newrow["COLUMN20"] = alkalmind;
            dt1newrow["COLUMN19"] = termszarmind;
            dt1newrow["COLUMN18"] = szintind;
            dt1newrow["COLUMN17"] = tablaind;
            dt1.Rows.Add(dt1newrow);
            tabinfo = FakUserInterface.BaseTag.Tablainfo;
            string savfilt = tabinfo.DataView.RowFilter;
            tabinfo.DataView.RowFilter = "";
            for (int i = 0; i < tabinfo.DataView.Count; i++)
            {
                row = tabinfo.DataView[i].Row;
                if (row["TOOLTIP"].ToString() != "")
                {
                    dt2newrow = dt2.NewRow();
                    dt2newrow["COLUMN1"] = row["SZOVEG"];
                    dt2newrow["COLUMN3"] = row["TOOLTIP"];
                    dt2newrow["COLUMN20"] = alkalmind;
                    dt2newrow["COLUMN19"] = termszarmind;
                    dt2newrow["COLUMN18"] = szintind;
                    dt2newrow["COLUMN17"] = tablaind;
                    dt2.Rows.Add(dt2newrow);
                }
            }
            tabinfo.DataView.RowFilter = savfilt;
            aktualtermszarm = "";
            aktualszint = "";
            aktualadatfajta = "";
            string owner = "";
            string kodtipus = "";
            string tartaltooltip = "";
            for (int j = 0; j < treeview.Nodes.Count; j++)
            {
                termszarmnode = treeview.Nodes[j];
                for (int k = 0; k < termszarmnode.Nodes.Count; k++)
                {
                    szintnode = termszarmnode.Nodes[k];
                    for (int l = 0; l < szintnode.Nodes.Count; l++)
                    {
                        tartalnode = szintnode.Nodes[l];
                        TablainfoTag tag = (TablainfoTag)tartalnode.Tag;
                        tabinfo = tag.Tablainfo;
                        DataView view = tabinfo.DataView;
                        view.RowFilter = "OWNER = '" + alkalmid + "'";
                        bool van = view.Count != 0;
                        if (alkalmid != "" && tabinfo.Adatfajta == "T")
                        {
                            view.RowFilter = "";
                            van = view.Count != 0;
                        }
                        if (van)
                        {
                            termszarmszov = tag.ParentTag.ParentTag.Azonositok.Szoveg;
                            szintszov = tag.ParentTag.Azonositok.Szoveg;
                            adatfajtaszov = tag.Azonositok.Szoveg;
                            for (int m = 0; m < tabinfo.DataView.Count; m++)
                            {
                                row = tabinfo.DataView[m].Row;
                                termszarm = tabinfo.TermSzarm;
                                szint = tabinfo.Szint;
                                adatfajta = tabinfo.Adatfajta;
                                owner = row["OWNER"].ToString();
                                tablanev = row["TABLANEV"].ToString();
                                Tablainfo egytabinfo = null;
                                kodtipus = row["KODTIPUS"].ToString();
                                if (kodtipus != "")
                                    tablanev = "Kódtipus:" + kodtipus;
                                else
                                    tablanev = "Tábla neve:" + tablanev;
                                tartaltooltip = row["TOOLTIP"].ToString();
                                if (tartaltooltip == "")
                                    tartaltooltip = row["SZOVEG"].ToString();
                                int rowind = tabinfo.Adattabla.GetRowIndex(row);
                                if (rowind < tag.ChildTablainfoTagok.Count)
                                {
                                    TablainfoTag tabinfotag = tag.ChildTablainfoTagok[rowind];
                                    for (int n = 0; n < 2; n++)
                                    {
                                        if (n == 0)
                                        {
                                            egytabinfo = tabinfotag.Tablainfo;
                                            if (kodtipus != "")
                                                tablanev = "Kódtipus:" + kodtipus;
                                            else
                                                tablanev = "Tábla neve:" + tablanev;
                                            if (termszarm != "SZ" && FakUserInterface.Adatszintek.Contains(szint))
                                                egytabinfo = egytabinfo.LeiroTablainfo;
                                        }
                                        else if (owner!=alkalmid)
                                            break;
                                        else
                                            egytabinfo = tabinfotag.LeiroTablainfo;
                                        bool vantooltip = false;
                                        bool vanszoveg = false;
                                        bool vanowner = false;
                                        if (egytabinfo.TablaColumns.IndexOf("TOOLTIP") != -1)
                                            vantooltip = true;
                                        if (egytabinfo.TablaColumns.IndexOf("SZOVEG") != -1)
                                            vanszoveg = true;
                                        if (egytabinfo.TablaColumns.IndexOf("ALKALMAZAS_ID") != -1)
                                            vanowner = true;
                                        if (vantooltip)
                                        {
                                            if (n == 0)
                                            {
                                                if (termszarm != aktualtermszarm)
                                                {
                                                    termszarmind++;
                                                    aktualtermszarm = termszarm;
                                                }
                                                if (szint != aktualszint)
                                                {
                                                    szintind++;
                                                    aktualszint = szint;
                                                }
                                                if (adatfajta != aktualadatfajta)
                                                {
                                                    adatfajtaind++;
                                                    aktualadatfajta = adatfajta;
                                                }
                                                tablaind++;
                                                dt1newrow = dt1.NewRow();
                                                dt1newrow["COLUMN1"] = alkalmind.ToString() + termszarmind.ToString() + szintind.ToString() + tablaind;
                                                dt1newrow["COLUMN2"] = termszarmszov + "/" + szintszov + "/" + adatfajtaszov + "/" + tablanev + "\n" +
                                                    tartaltooltip;
                                                if (egytabinfo.Tablanev == "LEIRO")
                                                    n = 1;
                                            }
                                            if (n == 1)
                                                egytabinfo = tabinfotag.LeiroTablainfo;
                                            if (egytabinfo.Tablanev == "LEIRO")
                                            {
                                                if (kodtipus != "")
                                                    break;
                                                egytabinfo.DataView.RowFilter = "TOOLTIP <> ''";
                                                if (egytabinfo.DataView.Count == 0)
                                                {
                                                    egytabinfo.DataView.RowFilter = "";
                                                    break;
                                                }
                                                else
                                                {
                                                    alkalmind++;
                                                    dt1newrow = dt1.NewRow();
                                                    dt1newrow["COLUMN1"] = alkalmind.ToString() + termszarmind.ToString() + szintind.ToString() + tablaind;
                                                    dt1newrow["COLUMN2"] = termszarmszov + "/" + szintszov + "/" + adatfajtaszov + "/" + tablanev + "\n" +
                                                        tartaltooltip;
                                                    dt1newrow["COLUMN3"] = "Mezönév";
                                                    dt1newrow["COLUMN4"] = "Megjelenitendö szöveg\n a sorban:";
                                                    dt1newrow["COLUMN5"] = "Megjelenitendö\nszöveg oszlopban:";
                                                }
                                            }
                                            else
                                            {
                                                dt1newrow["COLUMN4"] = "Identity:";
                                                dt1newrow["COLUMN5"] = "Szöveg:";
                                            }
                                            dt1newrow["COLUMN6"] = "Magyarázat:";
                                            dt1newrow["COLUMN20"] = alkalmind;
                                            dt1newrow["COLUMN19"] = termszarmind;
                                            dt1newrow["COLUMN18"] = szintind;
                                            dt1newrow["COLUMN17"] = tablaind;
                                            dt1.Rows.Add(dt1newrow);
                                            if (vanowner)
                                            {
                                                string alk = "0";
                                                if (alkalmid != "")
                                                    alk = alkalmid;
                                                egytabinfo.DataView.RowFilter = "ALKALMAZAS_ID = '" + alk + "'";
                                            }
                                            for (int nn = 0; nn < egytabinfo.DataView.Count; nn++)
                                            {
                                                egytabinfo.ViewSorindex = nn;
                                                row = egytabinfo.DataView[nn].Row;
                                                dt2newrow = dt2.NewRow();
                                                if (egytabinfo.Tablanev == "LEIRO")
                                                {
                                                    dt2newrow["COLUMN1"] = row["ADATNEV"];
                                                    dt2newrow["COLUMN2"] = row["TOOLTIP"];
                                                    dt2newrow["COLUMN3"] = row["SORSZOV"];
                                                    dt2newrow["COLUMN4"] = row["OSZLSZOV"];
                                                }
                                                else
                                                {
                                                    dt2newrow["COLUMN3"] = egytabinfo.AktIdentity;
                                                    if (vanszoveg)
                                                        dt2newrow["COLUMN4"] = row["SZOVEG"];
                                                    else if (egytabinfo.TablaColumns["SORSZOV"] != null)
                                                        dt2newrow["COLUMN4"] = egytabinfo.TablaColumns["SORSZOV"].Sorszov;
                                                    if (vantooltip)
                                                        dt2newrow["COLUMN2"] = row["TOOLTIP"];
                                                }
                                                dt2newrow["COLUMN20"] = alkalmind;
                                                dt2newrow["COLUMN19"] = termszarmind;
                                                dt2newrow["COLUMN18"] = szintind;
                                                dt2newrow["COLUMN17"] = tablaind;
                                                dt2.Rows.Add(dt2newrow);
                                            }
                                            egytabinfo.DataView.RowFilter = "";

                                        }
                                    }
                                }
                            }
                        }
                        view.RowFilter = "";
                    }
                }
            }
        }
    }
}

        

        


