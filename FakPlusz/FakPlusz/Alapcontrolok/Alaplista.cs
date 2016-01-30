using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Data;
using System.Text;
using System.IO;
using System.Windows.Forms;
using FakPlusz.Alapfunkciok;
using FakPlusz.Formok;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Windows.Forms;
using CrystalDecisions.Shared;

namespace FakPlusz.Alapcontrolok
{
    /// <summary>
    /// Listakeszito illetve listazo+adatszolgaltato felhasznaloi UserControlok alapja
    /// </summary>
    public partial class Alaplista : Base
    {
        /// <summary>
        /// 
        /// </summary>
        public string[] TablaFilterek = null;
        private TablainfoCollection erintetttablainfok = new TablainfoCollection();
        /// <summary>
        /// 
        /// </summary>
        public TablainfoCollection ErintettTablaInfok
        {
            get { return erintetttablainfok; }
            set { erintetttablainfok = value; }
        }
        private DataTable[] datasettablak = null;
        /// <summary>
        /// A DataSet adattablai, erteket a DataSet property set-jeben kap
        /// </summary>
        public DataTable[] DatasetTablak
        {
            get { return datasettablak; }
            set
            {
                datasettablak = value;
                TablainfoCollection coll = TermCegPluszCegalattiTabinfok;
                for (int i = 0; i < datasettablak.Length; i++)
                {
                    string tablanev = datasettablak[i].TableName;
                    TablainfoCollection infok = coll.GetByTablanev(tablanev);
                    for (int j = 0; j < infok.Count; j++)
                        erintetttablainfok.Add(infok[j]);
                }
                TablaFilterek=new string[erintetttablainfok.Count];
            }

        }
        /// <summary>
        /// 
        /// </summary>
        public string[] AdatkivitelKodtipusok = null;
        /// <summary>
        /// adatkiviteli filenev(ek) listaja
        /// </summary>
        public ArrayList filenevek = new ArrayList();
        /// <summary>
        /// adatkiviteli filenev(ek) tombje
        /// </summary>
        public string[] FileNevek =null;
        /// <summary>
        /// a USERADATSZOLG tabla tabalinformacioja
        /// </summary>
        public Tablainfo AdatKivitelTablainfo = null;
        /// <summary>
        /// egy adatkivitel objectum
        /// </summary>
        public Adatkivitel Adatkivitel;
        /// <summary>
        /// egy StreamWriter
        /// </summary>
        public StreamWriter[] StreamWriterek = null;
        private int[] darabszamok;
        private string[] gyerektablanevek;
        private int[] sorszamok;
        private int[] szulosorszamok;
        private bool vangyerektabla = false;
        private Adatkivitel labadatkiv = null;
        private Adatkivitel fejadatkiv = null;
        /// <summary>
        /// a hivo allitja, ha van a konkret felhasznalasban combo-bol valasztas
        /// </summary>
        public int ComboIndex = -1;
        /// <summary>
        /// a hivo allitja
        /// </summary>
        public object ReportSource;
        public ReportDocument reportdoc;
        /// <summary>
        /// lapszam
        /// </summary>
        public int pagenumber = 0;
        /// <summary>
        /// aktualis lapszam
        /// </summary>
        public int aktpagenumber = 0;
        /// <summary>
        /// maximalis lapszam
        /// </summary>
        public int maxpagenumber = 0;
        /// <summary>
        /// Zoom szazalaka
        /// </summary>
        public int zoomszazalek = 100;
        /// <summary>
        /// a printer neve
        /// </summary>
        public string AktualPrinterName="";
        public string DefaultPrinterName = "";
//        public PrintDocument PrintDocument = new PrintDocument();
        public PrinterSettings PrinterSettings = new PrinterSettings();//  PrinterName; //.InstalledPrinters[2].ToString();
        public int peldanyszam = 1;
        /// <summary>
        /// 
        /// </summary>
        public override string ControlNev
        {
            get
            {
                return base.ControlNev;
            }
            set
            {
                this.Name = value;
                base.ControlNev = value;
            }
        }

        /// <summary>
        /// felhasznaloi program listaprogramjainak alapja
        /// </summary>
        public Alaplista()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = comboBox1.Items.IndexOf("100");
            comboBox1.SelectionLength = 0;
            AktualPrinterName = PrinterSettings.PrinterName;
            if (PrinterSettings.IsDefaultPrinter)
                DefaultPrinterName = AktualPrinterName;

        }
        /// <summary>
        /// Lista inicializalas adatkivitellel
        /// </summary>
        /// <param name="fak">
        /// FakUserInterface
        /// </param>
        /// <param name="hivo">
        /// A hivo UserControl
        /// </param>
        /// <param name="vezerles">
        /// A vezerles
        /// </param>
        public virtual void AlaplistaInit(FakUserInterface fak, Base hivo, Vezerloinfo vezerles)
        {
            FakUserInterface = fak;
            FakUserInterface.SetUserSzamitasokKellSzamitasDatum(true);
            Hivo = hivo;
            Vezerles = vezerles;
            Paramfajta = Parameterezes.Nincsparameterezes;
            int i = Vezerles.ControlNevek.IndexOf(this.Name);
            if (FakUserInterface.Alkalmazas != "TERVEZO")
            {
                if (i != -1)
                {
                    Parameterez = Vezerles.Parameterez[i];
                    Paramfajta = Parameterez.Paramfajta;
                    TeljesHonap = Parameterez.TeljesHonap;
                    CsakEgyHonap = Parameterez.CsakEgyHonap;
                    TeljesEv = Parameterez.TeljesEv;
                    VanDatum = Parameterez.VanDatum;
                    VanValasztek = Parameterez.VanValasztek;
                    VanEgyszeru = Parameterez.VanEgyszeru;
                    VanOsszetett = Parameterez.VanOsszetett;
                    VanLista = Parameterez.VanLista;
                    Adatszolge = Parameterez.Adatszolge;
                    
                }
            }
            if (!Adatszolge)
                adatkiv.Visible = false;
            else
                adatkiv.Visible = true;
            this.Dock = DockStyle.Fill;
            comboBox1.SelectedIndex = comboBox1.Items.IndexOf(zoomszazalek);
            comboBox1.SelectionLength = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void Ujcontroloktolt()
        {
        }
        /// <summary>
        /// Buttonok event-je
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Buttonok_Click(object sender, EventArgs e)
        {
            ToolStripButton egybut = (ToolStripButton)sender;
            switch (egybut.Name)
            {
                case "elsolap":
                    if (aktpagenumber != 1)
                    {
                        crystalReportViewer1.ShowFirstPage();
                        aktpagenumber = 1;
                    }
                    break;
                case "utsolap":
                    if (aktpagenumber != maxpagenumber)
                    {
                        crystalReportViewer1.ShowLastPage();
                        aktpagenumber = maxpagenumber;
                    }
                    break;
                case "elozo":
                    if (aktpagenumber != 1)
                    {
                        crystalReportViewer1.ShowPreviousPage();
                        aktpagenumber = crystalReportViewer1.GetCurrentPageNumber();
                        if (aktpagenumber == 1)
                            elozo.Enabled = false;
                        else
                            elozo.Enabled = true;
                    }
                    break;
                case "kovetkezo":
                    if (aktpagenumber != maxpagenumber)
                    {
                        crystalReportViewer1.ShowNextPage();
                        aktpagenumber = crystalReportViewer1.GetCurrentPageNumber();
                    }
                    break;
                case "nyomtat":
                    if (nyomtmin != null && maxpagenumber!=0)
                    {
                        crystalReportViewer1.PrintReport();
 //                       int elsopage = Convert.ToInt32(nyomtmin.SelectedItem.ToString());
 //                       int utsopage = Convert.ToInt32(nyomtmax.SelectedItem.ToString());
 //                       reportdoc.PrintToPrinter(peldanyszam, false, elsopage, utsopage);
                    }
                    break;
                case "adatkiv":
                    adatkivitel_Click();
                    break;
            }
            lapszam.Text = "Lapszám: " + crystalReportViewer1.GetCurrentPageNumber().ToString(); // crystalReportViewer1.GetCurrentPageNumber().ToString();
            if (maxpagenumber > 1)
            {
                elsolap.Enabled = aktpagenumber > 1;
                elozo.Enabled = aktpagenumber > 1;
                kovetkezo.Enabled = maxpagenumber > aktpagenumber;
                utsolap.Enabled = maxpagenumber > aktpagenumber;
            }
        }
        /// <summary>
        /// nyomtmin es nyomtmax combo - k valasztekat allitja be min es max kozti numerikus ertekekre 
        /// </summary>
        /// <param name="combo">
        /// a Combo
        /// </param>
        /// <param name="min">
        /// min
        /// </param>
        /// <param name="max">
        /// max
        /// </param>
        public void ItemsBeallit(ComboBox combo, int min, int max)
        {
            ArrayList ar = new ArrayList();
            for (int j = min; j <= max; j++)
                ar.Add(j.ToString());
            string[] st = (string[])ar.ToArray(typeof(string));
            string selitem = "";
            if (combo.SelectedIndex != -1)
                selitem = combo.Items[combo.SelectedIndex].ToString();
            combo.Items.Clear();
            combo.Items.AddRange(st);
            int i = combo.Items.IndexOf(selitem);
            if (i == -1)
            {
                switch (combo.Name)
                {
                    case "nyomtmin":
                        combo.SelectedIndex = 0;
                        break;
                    case "nyomtmax":
                        combo.SelectedIndex = combo.Items.Count - 1;
                        break;
                }
            }
            else
                combo.SelectedIndex = i;
            combo.SelectionLength = 0;
            combo.SelectionStart = 0;
        }
        /// <summary>
        /// Zoom-valtas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void comboBox1_SelectionChangeCommitted(object sender, EventArgs e) // igazibol dropdownclosed
        {

            string szazal = comboBox1.Items[comboBox1.SelectedIndex].ToString();
            if (zoomszazalek.ToString() != szazal)
            {
                zoomszazalek = Convert.ToInt16(szazal);
                crystalReportViewer1.Zoom(zoomszazalek);
                comboBox1.SelectionLength = 0;
            }
        }
        /// <summary>
        /// A hivo UserControl hivja, ha befejezte az uj report osszeallitasat
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void crystalReportViewer1_EnabledChanged(object sender, EventArgs e)
        {
            if (!FakUserInterface.EventTilt && crystalReportViewer1.Enabled)
            {
                FakUserInterface.OpenProgress();
                FakUserInterface.SetProgressText("");
                aktpagenumber = 0;
                FakUserInterface.ProgressRefresh();
                crystalReportViewer1.ReportSource = ReportSource;
                FakUserInterface.ProgressRefresh();
                TabPage page = (TabPage)crystalReportViewer1.Controls[0].Controls[0].Controls[0];
                string text = "";
                int size = crystalReportViewer1.Width;
                for (int i = 0; i < size; i++)
                    text += " ";
                page.Text = text;
                comboBox1.SelectedIndex = comboBox1.Items.IndexOf("100");
                comboBox1.SelectionLength = 0;
                FakUserInterface.ProgressRefresh();
                crystalReportViewer1.ShowLastPage();
                FakUserInterface.ProgressRefresh();
                int maxp = crystalReportViewer1.GetCurrentPageNumber();
                crystalReportViewer1.ShowNextPage();
                FakUserInterface.ProgressRefresh();
                maxpagenumber = crystalReportViewer1.GetCurrentPageNumber();
                FakUserInterface.ProgressRefresh();
                maxlapszam.Text = "Lapok száma: " + maxpagenumber.ToString();
                if (maxpagenumber == 0)
                {
                    lapszam.Text = "Lapszám: 0";
                    pagenumber = 0;
                    adatkiv.Visible = false;
                    nyomtat.Visible = false;
                }
                else
                {
                    FakUserInterface.ProgressRefresh();
                    crystalReportViewer1.ShowFirstPage();
                    lapszam.Text = "Lapszám: 1";
                    pagenumber = 1;
                    if (AdatszolgaltatasNevek != null)
                        adatkiv.Visible = true;
                    nyomtat.Visible = true;
                    bool vis = maxpagenumber>1;
                    elozo.Enabled = false;
                    kovetkezo.Enabled = vis;
                    elsolap.Enabled = false;
                    utsolap.Enabled = vis;
                }
                FakUserInterface.ProgressRefresh();
                ItemsBeallit(nyomtmin, pagenumber, maxpagenumber);
                FakUserInterface.ProgressRefresh();
                ItemsBeallit(nyomtmax, pagenumber, maxpagenumber);
                FakUserInterface.ProgressRefresh();
                nyomtmax.SelectedIndex = nyomtmax.Items.Count - 1;
            }
            FakUserInterface.CloseProgress();
        }
        /// <summary>
        /// A hivo hivja listazando egysegenkent, miutan a szukseges tablakat feltoltotte
        /// A DatasetTablak tombjen haladva a tablanev alapjan megkeresi a vonatkozo tablainformaciot
        /// A tablainformacio DataView sorain haladva hivja a DataSetSortolt(DataTable datasettabla, Tabalinfo tablainformacio)
        /// eljarast, mely osszeallitja a kivant uj sort a datasettablaban
        /// </summary>
        public virtual void DataSetTolt()
        {
            FakUserInterface.OpenProgress();
            FakUserInterface.SetProgressText("");

            foreach(DataTable dt in DatasetTablak)
            {
                FakUserInterface.ProgressRefresh();
                string tablanev = dt.TableName;

                foreach(Tablainfo egytabinfo in erintetttablainfok)
                {
                    if (egytabinfo.Tablanev == tablanev)
                    {
                        string savfilt = egytabinfo.DataView.RowFilter;
                        egytabinfo.DataView.RowFilter = "";
                        for (int k = 0; k < egytabinfo.DataView.Count; k++)
                        {
                            FakUserInterface.ProgressRefresh();
                            egytabinfo.ViewSorindex = k;
                            DataSetSortolt(dt, egytabinfo);
                        }
                        egytabinfo.DataView.RowFilter = savfilt;
                    }
                }
                FakUserInterface.CloseProgress();
            }
        }
        /// <summary>
        /// Egy uj sor eloallitasa
        /// </summary>
        /// <param name="datasettable">
        /// Dataset tabla, ide jon az uj sor
        /// </param>
        /// <param name="tabinfo">
        /// Tablainformacio, ennek az aktualis sorabol all ossze
        /// </param>
        public virtual void DataSetSortolt(DataTable datasettable,Tablainfo tabinfo)
        {
            DataRow row = tabinfo.AktualViewRow;
            if (AlapTablaNev == tabinfo.Tablanev && tabinfo.TablaColumns["DATUMIG"] != null)
            {
                if (row["DATUMIG"].ToString() != "")
                    return;
            }
            DataRow datasetrow=datasettable.NewRow();
            ColCollection colcol = tabinfo.TablaColumns;
            Cols col;
            string ertek = "";
            for (int i = 0; i < datasettable.Columns.Count; i++)
            {
                string colnev = datasettable.Columns[i].ColumnName;
                int j = colcol.IndexOf(colnev);
                int k = row.Table.Columns.IndexOf(colnev);
                if (k != -1)
                {
                    if (j != -1)
                    {
                        col = colcol[j];
                        if (col.DataType.ToString().Contains("Date"))
                        {
                            ertek = row[j].ToString();
                            if (ertek != "")
                                ertek = ((DateTime)row[j]).ToShortDateString();
                            datasetrow[i] = ertek;
                        }
                        else
                            datasetrow[i] = row[k];
                    }
                    else
                        datasetrow[i] = row[k];
                }
                else
                {
                    string[] keresok = new string[] { "_SORSZOV", "_OSZLSZOV" };
                    foreach (string kereso in keresok)
                    {
                        if (colnev.Contains(kereso))
                        {
                            colnev = colnev.Replace(kereso, "");
                            col = colcol[colnev];
                            if (kereso == "_SORSZOV")
                                datasetrow[i] = col.Sorszov;
                            else
                                datasetrow[i] = col.Caption;
                        }
                    }
                }
            }
            datasettable.Rows.Add(datasetrow);
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void adatkivitel_Click()
        {
            EgyediAdatszolgaltatasInit();
            ListaInfo.DataView.RowFilter = "SZOVEG ='" + ListaNev + "'";
            AdatszolgKozosFileba = ListaInfo.DataView[0]["KOZOSFILEBA"].ToString() == "I";
            AdatszolgKellNapisorszam = ListaInfo.DataView[0]["KELLNAPISORSZ"].ToString() == "I";
            if (AdatszolgKellNapisorszam)
                AdatszolgaltatasFoldernev += "/" + MaiDatumString;
            Napisorszam = 1;
            bool kelladatkiv = true;
            if (!Directory.Exists(AdatszolgaltatasFoldernev))
            {
                if (AdatszolgKellNapisorszam)
                    AdatszolgaltatasFoldernev += "/" + Napisorszam.ToString();
                Directory.CreateDirectory(AdatszolgaltatasFoldernev);
            }
            else if (AdatszolgKellNapisorszam)
            {
                string foldernev = "";
                do
                {
                    Napisorszam++;
                    foldernev = AdatszolgaltatasFoldernev + "/" + Napisorszam.ToString();
                    if (!Directory.Exists(foldernev))
                    {
                        if (FakPlusz.MessageBox.Show("Ma már készült " + (Napisorszam - 1).ToString() + " adatszolg.\n Ismétli az utolsót?", "", FakPlusz.MessageBox.MessageBoxButtons.IgenNem) == FakPlusz.MessageBox.DialogResult.Igen)
                        {
                            Napisorszam--;
                            AdatszolgaltatasFoldernev += "/" + Napisorszam.ToString();                            
                        }
                        else if (FakPlusz.MessageBox.Show("Készüljön újabb adatszolg?", "", FakPlusz.MessageBox.MessageBoxButtons.IgenNem) != FakPlusz.MessageBox.DialogResult.Igen)
                            kelladatkiv = false;
                        else
                        {
                            AdatszolgaltatasFoldernev = foldernev;
                            Directory.CreateDirectory(AdatszolgaltatasFoldernev);
                        }
                        break;
                    }
                }
                while (true);
                if (!kelladatkiv)
                    return;
            }
            ListaInfo.DataView.RowFilter = "";
            TablainfoCollection infoksorrendben = new TablainfoCollection();
            for (int i = 0; i < AdatszolgaltatasNevek.Length; i++)
            {
                string nev = AdatszolgaltatasNevek[i];
                Tablainfo info = AdatszolgaltatasInfok.GetByAzontip("SZRA" + nev);
                infoksorrendben.Add(info);
            }
            if (AdatszolgKozosFileba)
            {
                FileNevek = new string[1];
                FileNevek[0] = infoksorrendben[0].TablaTag.Node.Text;
            }
            else
            {
                FileNevek = new string[AdatszolgaltatasNevek.Length];
                for (int i = 0; i < AdatszolgaltatasNevek.Length; i++)
                {
                    string nev = AdatszolgaltatasNevek[i];
                    FileNevek[i] = infoksorrendben[i].TablaTag.Node.Text;
                }
            }
            Stream[] streamek = new Stream[FileNevek.Length];
            bool voltmar = false;
            for (int i = 0; i < FileNevek.Length; i++)
            {
                string nev = AdatszolgaltatasFoldernev+ "/" +FileNevek[i];
                if (File.Exists(nev))
                {
                    if (AdatszolgKellNapisorszam)
                        File.Delete(nev);
                    else
                        voltmar=true;
                }
            }
            kelladatkiv = true;
            if (voltmar)
            {
                if (FakPlusz.MessageBox.Show("Már volt adatszolgáltatás. Újra?", "", FakPlusz.MessageBox.MessageBoxButtons.IgenNem) == FakPlusz.MessageBox.DialogResult.Igen)
                {
                    for (int i = 0; i < FileNevek.Length; i++)
                    {
                        string nev = AdatszolgaltatasFoldernev + "/" + FileNevek[i];
                        if (File.Exists(nev))
                            File.Delete(nev);
                    }
                }
                else
                    kelladatkiv = false;
            }
            if (!kelladatkiv)
                return;
            StreamWriterek = new StreamWriter[FileNevek.Length];
            for (int i = 0; i < FileNevek.Length; i++)
            {
                string nev = AdatszolgaltatasFoldernev + "/" + FileNevek[i];
                streamek[i] = File.Create(nev);
                StreamWriterek[i] = new StreamWriter(streamek[i]);
            }
            Adatkivitelek = new Adatkivitel[AdatszolgaltatasNevek.Length];
            for (int i = 0; i < Adatkivitelek.Length; i++)
            {
                Tablainfo info = infoksorrendben[i];
                UserAdatSzolgInfo.DataView.RowFilter = "SZOVEG='" + AdatszolgaltatasNevek[i] + "'";
                StreamWriter streamwriter;
                if (StreamWriterek.Length == 1)
                    streamwriter = StreamWriterek[0];
                else
                    streamwriter = StreamWriterek[i];
                Adatkivitelek[i] = new Adatkivitel(this,info,UserAdatSzolgInfo.DataView[0].Row["SZULOTABLA"].ToString(),streamwriter);
                if (AdatszolgaltatasNevek[i].Contains("FEJ"))
                    fejadatkiv = Adatkivitelek[i];
                else if (AdatszolgaltatasNevek[i].Contains("LAB"))
                    labadatkiv = Adatkivitelek[i];
                Adatkivitelek[i].Adatkivitelek = Adatkivitelek;
                Adatkivitelek[i].StreamWriterek = StreamWriterek;
                Adatkivitelek[i].Napisorszam = Napisorszam;
            }
            UserAdatSzolgInfo.DataView.RowFilter = "";
            AdatszolgaltatasInit();
            ListaAdatkivitel(false);
            if (labadatkiv != null)
                labadatkiv.EgySorEloallitasa(1);
            for (int i = 0; i < StreamWriterek.Length; i++)
                StreamWriterek[i].Close();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lista"></param>
        public virtual void ListaAdatkivitel(bool lista)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void AdatszolgaltatasInit()
        {
            darabszamok = new int[Adatkivitelek.Length];
            gyerektablanevek = new string[Adatkivitelek.Length];
            sorszamok = new int[Adatkivitelek.Length];
            szulosorszamok = new int[Adatkivitelek.Length];
            vangyerektabla = false;
            for (int i = 0; i < Adatkivitelek.Length; i++)
            {
                darabszamok[i] = 0;
                gyerektablanevek[i] = Adatkivitelek[i].GyerekTablanev;
                if (gyerektablanevek[i] != "")
                    vangyerektabla = true;
                sorszamok[i] = 0;
                szulosorszamok[i] = 0;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="elso"></param>
        public virtual void Adatszolgaltatas(bool elso)
        {
            int[] ujsorszamok = new int[sorszamok.Length];
            int[] ujdarabszamok = new int[darabszamok.Length];
            for (int i = 0; i < Adatkivitelek.Length; i++)
            {
                ujsorszamok[i] = 0;
                ujdarabszamok[i] = Adatkivitelek[i].GetSorokDarabszama();
                if (ujdarabszamok[i] == 0 && elso)
                    ujdarabszamok[i] = 1;
            }
            if (Adatkivitelek.Length != 1)
            {
                int i = Adatkivitelek.Length - 1;
                do
                {
                    if (Adatkivitelek[i] != labadatkiv)
                    {
                        int db = ujdarabszamok[i];
                        string nev = Adatkivitelek[i].AktualisKivitelNev;
                        for (int j = 0; j < Adatkivitelek.Length; j++)
                        {
                            if (Adatkivitelek[i] != labadatkiv)
                            {
                                if (gyerektablanevek[j] == nev)
                                {
                                    Adatkivitelek[j].GyerekDarabszam = db;
                                    break;
                                }
                            }
                        }
                    }
                    i--;
                    if (i == -1)
                        break;
                } while (true);
            }
            for (int i = 0; i < Adatkivitelek.Length; i++)
            {
                darabszamok[i] = darabszamok[i] + ujdarabszamok[i];
                sorszamok[i] = sorszamok[i] + ujsorszamok[i];
                //               }
            }
            if (!vangyerektabla)
            {
                for (int i = 0; i < Adatkivitelek.Length; i++)
                {
                        sorszamok[i]++;
                        Adatkivitelek[i].EgySorEloallitasa(sorszamok[i]);
                }
            }
            else if (elso && fejadatkiv != null)
                fejadatkiv.EgySorEloallitasa(1);
            bool retert = true;
            int jj = 0;
            for (int i = 0; i < Adatkivitelek.Length; i++)
            {
                if (Adatkivitelek[i] != labadatkiv && Adatkivitelek[i] != fejadatkiv)
                {
                    jj = i;
                    break;
                }
            }
            do
            {
                retert = AdatkivUjraBelepo(jj);
            } while (retert);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="adatkivsorsz"></param>
        /// <returns></returns>
        public virtual bool AdatkivUjraBelepo(int adatkivsorsz)
        {
            if (sorszamok[adatkivsorsz] == darabszamok[adatkivsorsz])
                return false;
            sorszamok[adatkivsorsz]++;
            Adatkivitelek[adatkivsorsz].EgySorEloallitasa(sorszamok[adatkivsorsz]);
            bool retert = false;
            if (adatkivsorsz != Adatkivitelek.Length - 1)
            {
                if (Adatkivitelek[adatkivsorsz + 1] == labadatkiv)
                    Adatkivitelek[adatkivsorsz + 1].SzuloSorszam = sorszamok[adatkivsorsz];
                else
                    retert = AdatkivUjraBelepo(adatkivsorsz + 1);
            }
            return retert;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ertek"></param>
        /// <returns></returns>
        public virtual bool EgyediSpecFix(ref string ertek)
        {
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void AdatszolgaltatasSorokEloallitasa()
        {
        }
        /// <summary>
        /// Csak valtozas eseten mukodik
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void TabStop_Changed(object sender, EventArgs e)
        {
            if (this.TabStop && (ValtozasLekerdez().Count != 0 || Parameterez!=null && Parameterez.Valtozas))
            {
                Ujcontroloktolt();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void ErtekBeallit()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void EgyediSzuresBeallit()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void oszetettdatagridview_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual int SzuresKiertekel()
        {
            return 0;
        }
    }
    /// <summary>
    /// Adatkozleshez kodtipusonkent letrehozando objektumok osztalya
    /// </summary>
    public class Adatkivitel
    {
        /// <summary>
        /// 
        /// </summary>
        public TablainfoCollection Adatkivitelinfok = new TablainfoCollection();
        /// <summary>
        /// Az aktualis adatkivitel tablainformacioja
        /// </summary>
        public Tablainfo AktualisTablainfo;
        /// <summary>
        /// FakUserInterface
        /// </summary>
        public FakUserInterface FakUserInterface;
        /// <summary>
        /// Az objectumot letrehozo UserControl
        /// </summary>
        public Alaplista Hivo;
        /// <summary>
        /// Az adatkivitel filenevei
        /// </summary>
        public string[] Filenevekeleje;
        /// <summary>
        /// 
        /// </summary>
        public string[] TeljesFilenevek;
        /// <summary>
        /// 
        /// </summary>
        public string[] AdatkivitelNevek;
        /// <summary>
        /// 
        /// </summary>
        public string AktualisKivitelNev;
        /// <summary>
        /// 
        /// </summary>
        public string[] FolderTree;
        /// <summary>
        /// 
        /// </summary>
        public string Foldernev = "";
        /// <summary>
        /// 
        /// </summary>
        public StreamWriter StreamWriter;
        /// <summary>
        /// Ha a hivo AdatkivitelFoldername ures, azonos a Filename tartalmaval
        /// ha nem ures, a foldername+'\\'+ a Filename
        /// </summary>
        public string AktualisTeljesFilenev;
        /// <summary>
        /// 
        /// </summary>
        public StreamWriter[] StreamWriterek;
        /// <summary>
        /// 
        /// </summary>
        public Adatkivitel[] Adatkivitelek = null;
        /// <summary>
        /// Az adatkivitelben erintett termeszetes tablak tablainformacioi
        /// </summary>
        public Tablainfo[] Tablainfok;
        /// <summary>
        /// Az adatkivitelben definialt fix ertekek
        /// </summary>
        public string[] FixErtekek = null;
        /// <summary>
        /// az adatkivitelben definialt elemi hosszak
        /// </summary>
        public int[] Hosszak = null;
        /// <summary>
        /// az adatkivitelben definialt mezonevek mezoinformacioi
        /// </summary>
        public Cols[] MezoColok = null;
        /// <summary>
        /// 
        /// </summary>
        public int Napisorszam;
        /// <summary>
        /// 
        /// </summary>
        public int GyerekDarabszam = 0;
        /// <summary>
        /// 
        /// </summary>
        public int SzuloSorszam = 0;
        /// <summary>
        /// 
        /// </summary>
        public int Darabszam = 0;
        /// <summary>
        /// 
        /// </summary>
        public string Maidatum = DateTime.Today.ToShortDateString();
        /// <summary>
        /// 
        /// </summary>
        public string Teljdatum = DateTime.Today.AddDays(5).ToShortDateString();
        /// <summary>
        /// 
        /// </summary>
        public string GyerekTablanev = "";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hivo"></param>
        /// <param name="adatkozl"></param>
        /// <param name="gyerektablanev"></param>
        /// <param name="streamwriter"></param>
        public Adatkivitel(Alaplista hivo,Tablainfo adatkozl,string gyerektablanev,StreamWriter streamwriter)
        {
            Hivo = hivo;
            FakUserInterface = hivo.FakUserInterface;
            AktualisKivitelNev = adatkozl.DataView[0].Row["KODTIPUS"].ToString();
            GyerekTablanev = gyerektablanev;
            StreamWriter = streamwriter;
            int count = adatkozl.DataView.Count;
            Tablainfok=new Tablainfo[count];
            FixErtekek=new string[count];
            Hosszak=new int[count];
            MezoColok=new Cols[count];
            for (int i = 0; i < adatkozl.DataView.Count; i++)
            {
                DataRow row = adatkozl.DataView[i].Row;
                string azontip = row["AZONTIP"].ToString();
                if (azontip != "")
                {
                    string szint = azontip.Substring(4, 1);
                    string tablanev = azontip.Substring(5);
                    Tablainfo info = FakUserInterface.GetBySzintPluszTablanev(szint, tablanev);
                    Adatkivitelinfok.Add(info);
                    Tablainfok[i]=info;
                    FixErtekek[i]="";
                    MezoColok[i] = info.TablaColumns[row["MEZONEV"].ToString()];
                }
                else
                {
                    Tablainfok[i] = null;
                    MezoColok[i] = null;
                    FixErtekek[i] = row["FIXERTEK"].ToString();
                }
                Hosszak[i]=Convert.ToInt32(row["KIMENETIHOSSZ"].ToString());
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual int GetSorokDarabszama()
        {
            int sordb = 0;
            for (int i = 0; i < Tablainfok.Length; i++)
            {
                if (Tablainfok[i] != null)
                {
                    int count = Tablainfok[i].DataView.Count;
                    if (count > sordb)
                        sordb = count;
                }
            }
            return sordb;
        }
        /// <summary>
        /// egy kiviteli sor eloallitasa
        /// ha a kiviteli leirasban fix ertekdefiniciot talal, meghivja a SpecFixErtek(ref definialt ertek) eljarast
        /// mely a konkret alkalmazasban felulirhato, es igy kiertekelheto, ha pl. darabszamot,osszegeket vagy mas, a leirasban
        /// nem megadhato es tablaban nem tarolt numerikus erteket akarunk kozolni
        /// </summary>
        /// <param name="sorszam">
        /// </param>
        public virtual void EgySorEloallitasa(int sorszam)
        {
            Darabszam = sorszam;
            string text = "";
            for (int j = 0; j < Hosszak.Length; j++)
            {
                string ertek = FixErtekek[j].Trim();
                string oldertek = ertek;
                bool numeric = false;
                int hossz = Hosszak[j];
                Tablainfo tabinfo = Tablainfok[j];
                if (tabinfo == null)
                {
                    if (ertek == "SORSZAM")
                    {
                        numeric = true;
                        ertek = sorszam.ToString();
                    }
                    else
                    {

                        numeric = SpecFixErtek(ref ertek);
                        if(ertek == oldertek)
                            numeric=false;
                    }
                }
                else
                {
                    ertek = "";
                    Cols egycol = MezoColok[j];
                    if (egycol.Kiegcol != null)
                        egycol = egycol.Kiegcol;
                    numeric = egycol.Numeric(egycol.DataType);
                    ertek = egycol.OrigTartalom;
                }
                if (ertek.Length > hossz)
                {
                    if (numeric)
                        ertek = ertek.Substring(ertek.Length - hossz);
                    else
                        ertek = ertek.Substring(0, hossz);
                }
                else if (ertek.Length < hossz)
                {
                    if (numeric)
                    {
                        ertek = FakUserInterface.SpaceFillFromLeft(ertek, hossz);
                        ertek = ertek.Replace(" ", "0");
                    }
                    else
                        ertek = FakUserInterface.SpaceFillFromRight(ertek, hossz);

                }
                text += ertek;
            }
            text += "\n";
            StreamWriter.Write(text);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ertek"></param>
        /// <returns></returns>
        public virtual bool SpecFixErtek(ref string ertek)
        {
            bool numeric = true;
            switch (ertek)
            {
                case "NAPISORSZAM":
                    ertek = Napisorszam.ToString();
                    break;
                case "SZULOSORSZAM":
                    ertek = SzuloSorszam.ToString();
                    break;
                case "GYEREKDARAB":
                    ertek = GyerekDarabszam.ToString();
                    break;
                case "DARABSZAM":
                    ertek = Darabszam.ToString();
                    break;
                case "NAPIDATUM":
                    ertek = Maidatum;
                    numeric = false;
                    break;
                case "TELJESITESDATUM":
                    ertek = Teljdatum;
                    numeric = false;
                    break;
                default:
                    numeric = false;
                    break;
            }
            return numeric;
        }
    }
}
