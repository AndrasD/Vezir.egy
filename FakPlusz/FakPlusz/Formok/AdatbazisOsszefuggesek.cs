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
    /// Adatbazisosszefuggesek reportjat allitja elo
    /// </summary>
    public partial class AdatbazisOsszefuggesek:Alaplista
    {
        /// <summary>
        /// a hasznalt DataSet
        /// </summary>
        private AltalanosDataSet dataset = new AltalanosDataSet();
        /// <summary>
        /// elso eset-e
        /// </summary>
        private bool elso = true;
        /// <summary>
        /// a hasznalt report
        /// </summary>
        private AdatbazisOsszefReport report = new AdatbazisOsszefReport();
        /// <summary>
        /// a kivant alkalmazasnev
        /// </summary>
        private string alkalmnev = "";
        /// <summary>
        /// valotozott-e az alkalmazas neve
        /// </summary>
        private bool alkalmvaltozas = true;
        /// <summary>
        /// az alkalmazas neve
        /// a Set valuevaltozas eseten true-ra allitja az alkalmvaltozas-t
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
                }
            }
        }
        private TreeView treeview;
        /// <summary>
        /// objectum letrehozasa
        /// </summary>
        /// <param name="vezerles"></param>
        public AdatbazisOsszefuggesek(Vezerloinfo vezerles)
        {
            InitializeComponent();
            Vezerles = vezerles;
            treeview = vezerles.ParentVezerles.TreeView;
            Hivo = Vezerles.Hivo;
            FakUserInterface = vezerles.Fak;
            this.Visible = false;
            this.ReportSource = report;
            reportdoc = report;
        }
        /// <summary>
        /// nyomtat esetben felulirja az alapot
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //public override void Buttonok_Click(object sender, EventArgs e)
        //{
        //    //string butnev = ((ToolStripButton)sender).Name;
        //    //if (butnev != "nyomtat")
        //    //    base.Buttonok_Click(sender, e);
        //    //else
        //    //{
        //    //    if (nyomtmin != null)
        //    //    {
        //    //        int elsopage = Convert.ToInt32(nyomtmin.SelectedItem.ToString());
        //    //        int utsopage = Convert.ToInt32(nyomtmax.SelectedItem.ToString());
        //    //        report.PrintToPrinter(1, false, elsopage, utsopage);
        //    //    }
        //    //}
        //}
        /// <summary>
        /// aktivizalas
        /// Elso esetben, valtozas vagy alkalmazas nevenek valtozasa eseten ujra eloallitja a reportot
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void  TabStop_Changed(object sender, EventArgs e)
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
//                    report.PrintOptions.PrinterName = PrinterName;
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
                }
                this.Visible = true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void InfokGyujt()
        {
            TreeNode termszarmnode;
            TreeNode szintnode;
            TreeNode tartalnode;
            TreeNode tablanode;
            int alkalmind = 0;
            int termszarmind = 0;
            int szintind = 0;
            int tablaind = 0;
            bool elsokodtab = true;
            bool elsolista = true;
            bool elsostatisztika = true;
            bool elsoadatszolg = true;
            bool elsoszukitettkodtab = true;
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
            DataRow dt1newrow;
            DataRow dt2newrow;
            DataRow dt3newrow;
            DataRow dt4newrow;
            DataRow dt6newrow;
            dt1newrow = dt1.NewRow();
            string nev = "";
            if (alkalmnev == "TERVEZO")
                nev = "Alaprendszer";
            else
                nev = "Alkalmazás: " + alkalmnev;
            dt1newrow["Column1"] = nev;
            dt1newrow["Column20"] = alkalmind;
            dt1.Rows.Add(dt1newrow);
            for (int j = 0; j < treeview.Nodes.Count; j++)
            {
                termszarmind++;
                dt2newrow = dt2.NewRow();
                termszarmnode = treeview.Nodes[j];
                dt2newrow["Column1"] = termszarmnode.Text;
                dt2newrow["Column20"] = alkalmind;
                dt2newrow["Column19"] = termszarmind;
                dt2.Rows.Add(dt2newrow);
                for (int k = 0; k < termszarmnode.Nodes.Count; k++)
                {
                    szintind++;
                    elsokodtab = true;
                    szintnode = termszarmnode.Nodes[k];
                    dt3newrow = dt3.NewRow();
                    dt3newrow["Column1"] = szintnode.Text;
                    dt3newrow["Column20"] = alkalmind;
                    dt3newrow["Column19"] = termszarmind;
                    dt3newrow["Column18"] = szintind;
                    dt3.Rows.Add(dt3newrow);
                    for (int l = 0; l < szintnode.Nodes.Count; l++)
                    {
                        tartalnode = szintnode.Nodes[l];
                        for (int m = 0; m < tartalnode.Nodes.Count; m++)
                        {
                            tablanode = tartalnode.Nodes[m];
                            TablainfoTag tag = (TablainfoTag)tablanode.Tag;
                            Tablainfo tabinfo = tag.Tablainfo;
                            bool kellmezotulajdonsag = true;
                            if (tabinfo.Tablanev != "LEIRO")
                            {
                                Azonositok azonbase = tabinfo.Azonositok;
                                if ((azonbase.Owner == "" && alkalmnev == "TERVEZO" || azonbase.Owner == alkalmnev) && !tag.Forditott)
                                {
                                    tablaind++;
                                    dt4newrow = dt4.NewRow();
                                    string tnev = "Tábla neve: " + tabinfo.Tablanev;
                                    string text1 = "";
                                    string jogtext = "";
                                    string combotext = "";
                                    if (tabinfo.Tablanev == "KODTAB" || tabinfo.Tablanev == "OSSZEF" || tabinfo.Tablanev == "LISTA" || tabinfo.Tablanev == "STATISZTIKA" || tabinfo.Tablanev == "ADATSZOLG" || tabinfo.Tablanev == "NAPTARAK")
                                    {
                                        tnev += "/" + tablanode.Text;
                                        if (tabinfo.Adatfajta == "O")
                                            tnev += "(Összefüggés)";
                                        else if (tabinfo.Adatfajta == "C")
                                            tnev += "(Csoport)";
                                        else if (tabinfo.Adatfajta == "S")
                                            tnev += "(Szükitett kódtábla)";
                                        if (tabinfo.Tablanev == "KODTAB")
                                        {
                                            if (tabinfo.Adatfajta == "K")
                                            {
                                                if (elsokodtab)
                                                {
                                                    kellmezotulajdonsag = true;
                                                    elsokodtab = false;
                                                }
                                                else
                                                    kellmezotulajdonsag = false;
                                            }
                                            else if (elsoszukitettkodtab)
                                            {
                                                kellmezotulajdonsag = true;
                                                elsoszukitettkodtab = false;
                                            }
                                            else
                                                kellmezotulajdonsag = false;
                                        }
                                        else if (tabinfo.Tablanev == "LISTA")
                                        {
                                            if (elsolista)
                                            {
                                                kellmezotulajdonsag = true;
                                                elsolista = false;
                                            }
                                            else
                                                kellmezotulajdonsag = false;
                                        }
                                        else if (tabinfo.Tablanev == "STATISZTIKA")
                                        {
                                            if (elsostatisztika)
                                            {
                                                kellmezotulajdonsag = true;
                                                elsostatisztika = false;
                                            }
                                            else
                                                kellmezotulajdonsag = false;
                                        }
                                        else if (tabinfo.Tablanev == "ADATSZOLG")
                                        {
                                            if (elsoadatszolg)
                                            {
                                                kellmezotulajdonsag = true;
                                                elsoadatszolg = false;
                                            }
                                            else
                                                kellmezotulajdonsag = false;
                                        }
                                        else
                                            kellmezotulajdonsag = false;
                                    }
                                    dt4newrow["Column1"] = tnev;
                                    dt4newrow["Column20"] = alkalmind;
                                    dt4newrow["Column19"] = termszarmind;
                                    dt4newrow["Column18"] = szintind;
                                    dt4newrow["Column17"] = tablaind;
                                    dt4.Rows.Add(dt4newrow);
                                    dt6newrow = dt6.NewRow();
                                    if (azonbase.User != "")
                                        text1 += "Userek: " + azonbase.User + "\n";
                                    if (azonbase.Szulotabla != "")
                                        text1 += "Szülötábla: " + azonbase.Szulotabla + "\n";
                                    if (azonbase.Selectstring != "")
                                        text1 += "Select string: " + azonbase.Selectstring + "\n";
                                    if (azonbase.Orderstring != "")
                                        text1 += "Order string: " + azonbase.Orderstring + "\n";
                                    if (tabinfo.Sort != "")
                                        text1 += "DataView Sort:" + tabinfo.Sort + "\n";
                                    if (!tabinfo.TermSzarm.Contains("T"))
                                    {
                                        if (tabinfo.Tablanev == "LISTA" || tabinfo.Tablanev == "ADATSZOLG")
                                        {
                                        }
                                        if (tabinfo.Tablanev == "KODTAB")
                                        {
                                            text1 += "Kódhossz: " + tabinfo.KodHossz + "\n";
                                            text1 += "Szöveghossz:" + tabinfo.SzovegHossz + "\n";
                                        }
                                        text1 += "Kell verzió? " + BoolToString(tabinfo.KellVerzio);
                                        text1 += "Szerepelhet összefüggésben? " + BoolToString(azonbase.Lehetosszef);
                                        text1 += "Szerepelhet csoportmeghatározásban? " + BoolToString(azonbase.Lehetcsoport);
                                    }
                                    else
                                        text1 += "Lehet üres? " + BoolToString(tabinfo.EredetiLehetUres);
                                    text1 += "Lehet comboválasztékban? " + BoolToString(azonbase.Lehetcombo);
                                    if (azonbase.Lehetosszef)
                                    {
                                    }
                                    if (azonbase.Lehetcsoport)
                                    {
                                    }
                                    jogtext += "Jogosultságok: " + "\n";
                                    string[] kezszintstringek = SzovegesKezeloiSzint;
                                    string[] hozferstringek = HozferStringek;
                                    for (int n = 0; n < azonbase.Jogszintek.Length; n++)
                                        jogtext += "    " + kezszintstringek[n] + ": " + HozferStringek[Convert.ToInt16(azonbase.Jogszintek[n])] + "\n";
                                    if (tabinfo.TermSzarm.Contains("T"))
                                    {
                                        combotext += "Combo tipusú mezök combotáblái:\n";
                                        for (int n = 0; n < tabinfo.ComboColumns.Count; n++)
                                        {
                                            Cols egycol = tabinfo.ComboColumns[n];
                                            Comboinfok cinf = egycol.Combo_Info;
                                            combotext += "Mezö neve: " + egycol.ColumnName + "\n";
                                            string tablanev = cinf.Combotag.Tablainfo.Tablanev;
                                            combotext += " Combotábla: " + tablanev;
                                            if (tablanev == "KODTAB")
                                                combotext += "/" + cinf.Combotag.Tablainfo.Szoveg;
                                            combotext += "\n";
                                        }
                                    }
                                    dt6newrow["Column1"] = text1;
                                    dt6newrow["Column2"] = jogtext;
                                    dt6newrow["Column3"] = combotext;
                                    dt6newrow["Column20"] = alkalmind;
                                    dt6newrow["Column19"] = termszarmind;
                                    dt6newrow["Column18"] = szintind;
                                    dt6newrow["Column17"] = tablaind;
                                    dt6.Rows.Add(dt6newrow);
                                    InfokGyujt(dt5, azonbase, tabinfo, alkalmind, termszarmind, szintind, tablaind, kellmezotulajdonsag);
                                }
                            }
                        }
                    }
                }
            }
        }
        
        
        private void InfokGyujt(DataTable dt5, Azonositok azonbase, Tablainfo tabinfo, int alkalmind, int termszarmind, int szintind, int tablaind,bool kellmezotulajdonsag)
        {
            DataRow newrow1 = dt5.NewRow();
            string mezotext2 = "";
            string tulajdonsagtext2 = "";
            string mezotext3 = "";
            string tulajdonsagtext3 = "";
            string fejcolumn = "Column1";
            Cols egycol;
            string tervmezostring = mezotext2;
            string tervtulajdonsagstring = tulajdonsagtext2;
            if (tabinfo.Tablanev != "VALTOZASNAPLO" && tabinfo.Tablanev!="USERLOG")
            {
                if (kellmezotulajdonsag)
                {
                    tervmezostring = mezotext3;
                    tervtulajdonsagstring = tulajdonsagtext3;
                    fejcolumn = "Column2";
                    newrow1["Column1"] = "Mezötulajdonságok adatbázis táblaterv alapján: ";
                    for (int k = 0; k < tabinfo.TablaColumns.Count; k++)
                    {
                        int count = 0;
                        egycol = tabinfo.TablaColumns[k];
                        mezotext2 += "Mezö neve: " + egycol.ColumnName;
                        tulajdonsagtext2 += "Adattipus: " + egycol.DataType.ToString() + "\n";
                        count++;
                        tulajdonsagtext2 += "Adathossz: " + egycol.DbaseColumnSize.ToString() + "\n";
                        count++;
                        if (egycol.Numeric(egycol.DataType) && !egycol.DataType.ToString().Contains("Int"))
                        {
                            tulajdonsagtext2 += "Tizedesek száma: " + egycol.Tizedesek.ToString() + "\n";
                            count++;
                        }
                        tulajdonsagtext2 += "Lehet null? " + BoolToString(egycol.IsAllowDbNull);
                        count++;
                        if (egycol.IsIdentity)
                        {
                            tulajdonsagtext2 += "Ez az identity " + "\n";
                            count++;
                        }
                        for (int i = 0; i < count; i++)
                            mezotext2 += "\n";
                    }
                }
                if (tabinfo.Tablanev != "OSSZEF")
                {
                    newrow1[fejcolumn] = "TERVEZO-ben megadott mezötulajdonságok: ";
                    for (int k = 0; k < tabinfo.TablaColumns.Count; k++)
                    {
                        int count = 0;
                        egycol = tabinfo.TablaColumns[k];
                        if ((egycol.Lathato || egycol.Comboe) && !egycol.IsIdentity)
                        {
                            tervmezostring += "Mezö neve: " + egycol.ColumnName;
                            if (egycol.Sorszov != egycol.ColumnName)
                            {
                                tervtulajdonsagstring += "Sor szövege: " + egycol.Sorszov + "\n";
                                count++;
                            }
                            if (egycol.Caption != egycol.ColumnName)
                            {
                                tervtulajdonsagstring += "Oszlop szövege: " + egycol.Caption + "\n";
                                count++;
                            }
                            tervtulajdonsagstring += "Csak olvas? " + BoolToString(egycol.ReadOnly);
                            count++;
                            tervtulajdonsagstring += "Lehet üres? " + BoolToString(egycol.Lehetures);
                            count++;
                            if (egycol.Adathossz != "0" && egycol.Adathossz != egycol.DbaseColumnSize.ToString())
                            {
                                tervtulajdonsagstring += "Adathossz: " + egycol.Adathossz + "\n";
                                count++;
                            }

                            if (egycol.DefaultValue.ToString() != "" && egycol.DefaultValue.ToString() != "0")
                            {
                                tervtulajdonsagstring += "Kezdöérték: " + egycol.DefaultValue.ToString() + "\n";
                                count++;
                            }
                            if (egycol.Format != "")
                            {
                                tervtulajdonsagstring += "Formátum: " + egycol.Format + "\n";
                                count++;
                            }
                            for (int i = 0; i < count; i++)
                                tervmezostring += "\n";

                        }
                    }
                }
            }

            if (kellmezotulajdonsag)
            {
                newrow1["Column4"] = mezotext2;
                newrow1["Column5"] = tulajdonsagtext2;
                newrow1["Column6"] = tervmezostring;
                newrow1["Column7"] = tervtulajdonsagstring;
            }
            else 
            {
                newrow1["Column4"] = tervmezostring;
                newrow1["Column5"] = tervtulajdonsagstring;
            }
            newrow1["Column20"] = alkalmind;
            newrow1["Column19"] = termszarmind;
            newrow1["Column18"] = szintind;
            newrow1["Column17"] = tablaind;
            dt5.Rows.Add(newrow1);

        }
        private string BoolToString(bool ertek)
        {
            if (ertek)
                return "Igen\n";
            return "Nem\n";
        }
    }
}
