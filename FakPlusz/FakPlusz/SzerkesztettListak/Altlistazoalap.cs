using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Drawing.Printing;
using System.Windows.Forms;
//using FakPlusz;
using FakPlusz.Alapcontrolok;
using FakPlusz.Alapfunkciok;
using FakPlusz.UserAlapcontrolok;
using FakPlusz.DataSetek;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Windows.Forms;
namespace FakPlusz.SzerkesztettListak
{
    /// <summary>
    /// Szerkesztett listak, statisztikak reportjat allitja elo, SzerkesztettAlap-tol orokol
    /// </summary>
    public partial class Altlistazoalap : SzerkesztettAlap
    {
        /// <summary>
        /// listainformaciok
        /// </summary>
        public ListaInfok Listainfok = null;
        /// <summary>
        /// lista tablainformacio
        /// </summary>
        public Tablainfo ListaTabinfo = null;
        private Tablainfo elsotabinfo = null;
        private int ig;
        private int count;
        private DataRow dr;
        private DataTable dt;
        /// <summary>
        /// a report
        /// </summary>
        public AltalanosCrystalReport report = new AltalanosCrystalReport();
        /// <summary>
        /// a dataset
        /// </summary>
        public AltalanosDataSet dataset = new AltalanosDataSet();
        private PrintOptions prop;
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
        /// zoom szazalak
        /// </summary>
        public int zoomszazalek = 100;
        /// <summary>
        /// feltetel Datumtol, Datumig nevu mezokre
        /// </summary>
        public string Datumtoligfeltetel = "";
        /// <summary>
        /// Feltetel Kezdete, Vege nevu mezokre
        /// </summary>
        public string Kezdetefeltetel = "";
        /// <summary>
        /// 
        /// </summary>
        public string Datum1 = "";
        /// <summary>
        /// 
        /// </summary>
        public string Datum2 = "";
        /// <summary>
        /// Datum1 /Datum2 megbontva nonapokra
        /// </summary>
        public ArrayList Datumtomb = new ArrayList();
        /// <summary>
        /// az elso printer neve
        /// </summary>
        public string PrinterName = PrinterSettings.InstalledPrinters[0].ToString();
        private string mindatum;
        private string maxdatum;
        private string parfeltetel = "";
        private string paroszlopmeghatarozas = "";
        private string parsormeghatarozas = "";
        private DataView fotabla;
        private ArrayList OsszesBeallitandoId;
        private ArrayList OsszesBeallitottIdErtek;
        private TablainfoCollection tabinfok;
        private bool[] uresek;
        private bool ures = false;
        private bool firsttime = true;
        private Tablainfo[] sorrendtabinfo = null;
        private Tablainfo[] oszloptabinfok = null;
        private Tablainfo[] matrixponttabinfok = null;
        private string egysor;
        private bool kellcsakosszegsorba = false;
        private int sorrendvaltozasszint = -1;
        private int SorokSzama = 0;
        private int OszlopokSzama = 0;
        private string[] SaveFilterek;
        private int[] SaveViewIndexek;
        private int[] sorokszama = null;
        private string s;
        /// <summary>
        /// objectum letrehozasa
        /// </summary>
        public Altlistazoalap(FakUserInterface fak, Base hivo, Vezerloinfo vezerles)
        {
            InitializeComponent();
            FakUserInterface = fak;
            Hivo = hivo;
            Vezerles = vezerles;
            HozferJog = vezerles.HozferJog;
            KezeloiSzint = vezerles.KezeloiSzint;
        }
        /// <summary>
        /// aktivizalas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void TabStop_Changed(object sender, EventArgs e)
        {
            if (this.TabStop && (UjTag || ValtozasLekerdez().Count != 0))
                AltalanosInit();
        }
        /// <summary>
        /// Incializalas kiegeszitessel
        /// </summary>
        public override void AltalanosInit()
        {
            if (UjTag || ValtozasLekerdez().Count != 0)
            {
                ValtozasBeallit();
                base.AltalanosInit();
                parameterview.Table = parametertabla;
                feltetelview.Table = felteteltabla;
                feltetelsview.Table = sorfelteteltabla;
                felteteloview.Table = oszlopfelteteltabla;
                parfeltetel = "";
                paroszlopmeghatarozas = "";
                parsormeghatarozas = "";
//                report.PrintOptions.PrinterName = PrinterName;
                UjTag = false;
                ValtozasTorolExcept(new string[] { "Datumvaltozas" });
                TermCegPluszCegalattiTabinfok = FakUserInterface.GetCegPluszCegalattiTermTablaInfok();
                string cegid = FakUserInterface.AktualCegid.ToString();
                string alkalmid=Tabinfo.Azonositok.Owner;
                DateTime aktdat = DateTime.Today;
                DateTime aktdatutsonap = DateTime.Today;
                DateTime indulodat=aktdat;
                Tablainfo cegszerz = FakUserInterface.GetBySzintPluszTablanev("C", "CEGSZERZODES");
                TeljesHonap = Hivo.TeljesHonap;
                TeljesEv = Hivo.TeljesEv;
                VanDatum = Hivo.VanDatum;
                //dateTimePicker1.Visible = VanDatum;
                //dateTimePicker2.Visible = VanDatum;
                //label1.Visible = VanDatum;
                if (!VanDatum)
                {
                    //if (Tervezoe)
                    //{
                    //    Tablainfo rendszverz = FakUserInterface.GetBySzintPluszTablanev("R", "RVERSION");
                    //    indulodat = Convert.ToDateTime(rendszverz.DataView[0].Row["DATUMTOL"].ToString());
                    //    aktdat = indulodat;//.AddMonths(1);
                    //}
                    //else
                    //{
                        dr = cegszerz.DataView[0].Row;
                        indulodat = Convert.ToDateTime(dr["INDULODATUM"].ToString());
                        aktdat = FakUserInterface.Aktintervallum[0];
    //                }
                    aktdatutsonap = aktdat.AddMonths(1).AddDays(-1);
                }
                string aktdatutso = aktdatutsonap.ToShortDateString();
                FakUserInterface.EventTilt = true;
//                dateTimePicker1.MinDate = indulodat;
                //dateTimePicker1.MaxDate = aktdatutsonap;
//                dateTimePicker2.MinDate = indulodat;
                //dateTimePicker2.MaxDate = aktdatutsonap;
                dateTimePicker1.Value = aktdat;
                dateTimePicker2.Value = aktdatutsonap;
                FakUserInterface.EventTilt = false;
                csakegyhonap = true;
                mindatum = indulodat.ToShortDateString();
                maxdatum = aktdatutso;
                if (listae)
                    ig = 2;
                else
                    ig = 4;
                comboBox2.Text = "100";
                comboBox2.SelectionLength = 0;
                try
                {
                    Listatervalap hivo = (Listatervalap)Hivo;
                    hivo.VerziobuttonokAllit();
                    comboBox1.Visible = false;
                    label6.Visible = false;
                    ListaTabinfo = FakUserInterface.GetByAzontip(listaazontip);
                    teljeshonap = ListaTabinfo.Azonositok.Teljeshonap;
                }
                catch
                {
                    Tablainfo tartalinfo;
                    if (listae)
                        tartalinfo = FakUserInterface.GetByAzontip("SZULTARTAL");
                    else
                        tartalinfo = FakUserInterface.GetByAzontip("SZUITARTAL");
                    comboBox1.DataSource = tartalinfo.DataView;
                    comboBox1.DisplayMember = "SZOVEG";
                    comboBox1.ValueMember = "AZONTIP";
                }
                if (!comboBox1.Visible)
                {
                    FakUserInterface.EventTilt = true;
                    tabControl1.SelectedIndex = -1;
                    FakUserInterface.EventTilt = false;
                    tabControl1.SelectedIndex = 0;
                }

            }
        }
        private void Beallitasok()
        {
            DataRow dr = Tabinfo.DataView[0].Row;
            TeljesHonap = dr["TELJESHONAP"].ToString() == "I";
            TeljesEv = dr["TELJESEV"].ToString() == "I";
            osszeslistaelem = dr["OSSZESLISTAELEM"].ToString();
            string[] feltetelek;
            if (listae)
            {
                feltetelek = new string[] { "", dr["FELTETEL"].ToString() };
                parametertabla = listaparametertabla;
            }
            else
            {
                feltetelek = new string[] {"",dr["FELTETEL"].ToString(),dr["SORFELTETEL"].ToString(),
                        dr["OSZLOPFELTETEL"].ToString()};
                parametertabla = statisztikaparametertabla;
            }
            parametertabla.Rows.Clear();
            parameterview.Table = parametertabla;
            felteteltabla.Rows.Clear();
            feltetelview.Table = felteteltabla;
            sorfelteteltabla.Rows.Clear();
            feltetelsview.Table = sorfelteteltabla;
            oszlopfelteteltabla.Rows.Clear();
            felteteloview.Table = oszlopfelteteltabla;
            osszestabla.Clear();
            osszestabla.AddRange(new AdatTabla[] { parametertabla, felteteltabla, sorfelteteltabla, oszlopfelteteltabla });
            string feltetel;
            if (Hivo.Name == "Listaterv" || Hivo.Name == "Statterv")
                Userfeltetelekkiegeszit(felteteltabla);
            Elemez(kellenetreeview, osszeslistaelem, null);
            for (int i = 1; i < ig; i++)
            {
                DataTable table = (DataTable)osszestabla[i];
                feltetel = feltetelek[i];
                if (feltetel != "")
                    Feltelemez(table, feltetel);
                else if (table.TableName == "FELTETETEL")
                    userselectcount = table.Rows.Count;
            }
        }
        private void Listazas()
        {
            //if (VanDatum)
            //{
                FakUserInterface.SetUserSzamitasokKellSzamitasDatum(true);
                FakUserInterface.SetUserSzamitasokDatumHatarok(Convert.ToDateTime(Datum1), Convert.ToDateTime(Datum2));
            //}
            //else
            //{
            //    Datum1 = FakUserInterface.DatumToString(DateTime.Today);
            //    Datum2 = Datum1;
            //}
            prop = report.PrintOptions;
            bool portop = true;
            if (listae && Listainfok.Oszlopinfok.Mezoinfok.Count > 7 || !listae && 1 + Listainfok.Matrixpontinfok.Mezoinfok.Count * (Listainfok.Matrixpontinfok.OszlopokSzama + 1) > 6)
                portop = false;
            if (portop)
                prop.PaperOrientation = PaperOrientation.Portrait;
            else
                prop.PaperOrientation = PaperOrientation.Landscape;
            foreach (DataTable dt in dataset.Tables)
                dt.Rows.Clear();
            dr = null;
            for (int i = 1; i < ig; i++)
            {
                DataView view = (DataView)osszesview[i];
                DataTable table = view.Table;
                if (view.Count != 0)
                {
                    egysor = "";
                    bool first = true;
                    int count = 1;
                    for (int j = 0; j < view.Count; j++)
                    {
                        if (j != 0)
                            egysor += newline[0].ToString();
                        DataRow row = view[j].Row;
                        for (int k = 2; k < table.Columns.Count; k++)
                        {
                            if (i != 1)
                            {
                                if (first)
                                {
                                    egysor += count.ToString();
                                    count++;
                                    first = false;
                                }
                            }
                            if (row[k].ToString() != "")
                            {
                                if (egysor != "")
                                    egysor += " ";
                                egysor += row[k].ToString();
                            }
                            if (i != -1 && table.Columns[k].ColumnName == "ESVAGY" && row[k].ToString() == "")
                                first = true;
                        }
                    }
                    switch (i)
                    {
                        case 1:
                            parfeltetel = egysor;
                            break;
                        case 2:
                            parsormeghatarozas = egysor;
                            break;
                        case 3:
                            paroszlopmeghatarozas = egysor;
                            break;
                    }
                }
            }
            elsotabinfo = Listainfok.ElsoTabinfo;
            DataTable dt1 = dataset.DataTable2;
            dr = dt1.NewRow();
            fotabla = elsotabinfo.DataView;
            OsszesBeallitandoId = Listainfok.OsszesBeallitandoId;
            OsszesBeallitottIdErtek = Listainfok.OsszesBeallitottIdErtek;
            tabinfok = Listainfok.Tablainfok;
            int tablainfoindex = Listainfok.Tablainfok.IndexOf(elsotabinfo);
            string tablanev = elsotabinfo.TablaTag.Azonositok.Szoveg;
            elsotabinfo.Adattabla.Rows.Clear();
            string selstring = elsotabinfo.GetTablainfoSelect(OsszesBeallitottIdErtek);
            string orderstring = elsotabinfo.Sort;
            ValtozasBeallit();
            elsotabinfo.DataView.Sort = orderstring;
            FakUserInterface.Select(fotabla.Table, FakUserInterface.AktualCegconn, fotabla.Table.TableName, selstring, "", false);
            elsotabinfo.Tartalmaktolt();
            if (!listae)
            {
                SorokSzama = Listainfok.Matrixpontinfok.SorokSzama;
                OszlopokSzama = Listainfok.Matrixpontinfok.OszlopokSzama;
            }
            SaveFilterek = new string[tabinfok.Count];
            SaveViewIndexek = new int[tabinfok.Count];
            sorokszama = new int[tabinfok.Count];
            int focount = fotabla.Table.Rows.Count;
            bool folytassuk = focount != 0;
            int rekszam = focount;
            int feldrekszam = 0;
            int fofilcount = elsotabinfo.RowFilterek.Count;
            int olvasdb = 0;
            int tabinfocount = tabinfok.Count - 1;
            if (tabinfocount == 0)
                tabinfocount++;
            if (fofilcount > 0)
            {
                string savfilt = "";
                elsotabinfo.RowFilterIndex = -1;
                int kulsociklusdb = 0;
                do
                {
                    //Datumbeallitasok();
                    //if (elsotabinfo.SpecDatumNevekArray.Count != 0)
                    //    elsotabinfo.DatumString=
                    for (int i = 0; i < fofilcount; i++)
                    {
                        elsotabinfo.SetRowFilter();
                        if (savfilt != elsotabinfo.DataView.RowFilter)
                        {
                            olvasdb += elsotabinfo.DataView.Count * tabinfocount;
                            feldrekszam += elsotabinfo.DataView.Count;
                            savfilt = elsotabinfo.DataView.RowFilter;
                        }
                    }
                    elsotabinfo.RowFilterIndex = -1;
                    kulsociklusdb++;
                } while (kulsociklusdb < elsotabinfo.SpecDatumNevekArray.Count);
                olvasdb += 1;
            }
            else
            {
                feldrekszam = rekszam;
                olvasdb = focount * tabinfocount;
            }
            if (!folytassuk)
                MessageBox.Show(tablanev + " beolvasott rekordszáma: 0\nA feltételek megfogalmazásában esetleg ellentmondás van!");
            else if (this.Name == "Altlistazoalap")
            {
                string figy = "";
                if (rekszam < feldrekszam)
                    figy = "\nA feltételek megfogalmazásában redundacia van!";
                olvasdb = olvasdb * Datumtomb.Count;
                feldrekszam = feldrekszam * Datumtomb.Count;
                folytassuk = MessageBox.Show(tablanev + " beolvasott rekordszáma:" + rekszam.ToString() + "\nFeldolgozott rekordok:" + feldrekszam.ToString() + figy + "\nVárható lemezműveletek száma:" + olvasdb.ToString() + "\nFolytassuk?", "", FakPlusz.MessageBox.MessageBoxButtons.IgenNem) == MessageBox.DialogResult.Igen;
            }
            if (folytassuk)
            {
                dataset.DataTable3.Rows.Clear();
                sorrendtabinfo = null;
                if (Listainfok.Sorrendinfok.Mezoinfok.Count != 0)
                    sorrendtabinfo = (Tablainfo[])Listainfok.Sorrendinfok.Mezoinfok.Tablainfok.ToArray(typeof(Tablainfo));
                if (Listainfok.Oszlopinfok.Mezoinfok.Count != 0)
                    oszloptabinfok = (Tablainfo[])Listainfok.Oszlopinfok.Mezoinfok.Tablainfok.ToArray(typeof(Tablainfo));
                if (Listainfok.Matrixpontinfok.Mezoinfok.Count != 0)
                    matrixponttabinfok = (Tablainfo[])Listainfok.Matrixpontinfok.Mezoinfok.Tablainfok.ToArray(typeof(Tablainfo));
                egysor = "";
                uresek = new bool[tabinfok.Count];
                ures = false;
                for (int j = 0; j < tabinfok.Count; j++)
                    uresek[j] = false;
                firsttime = true;
                dt = dataset.DataTable2;
                elsotabinfo.DataView.RowFilter = "";
                elsotabinfo.SetRowFilter();
                string savfilt = elsotabinfo.DataView.RowFilter;
                SaveFilterek[tabinfok.IndexOf(elsotabinfo)] = savfilt;
                int datumtombcount = 0;
                do
                {
                    if (Datumtomb.Count != 0)
                    {
                        string[] datumok = (string[])Datumtomb[datumtombcount];
                        string elem1 = datumok[0];
                        string elem2 = datumok[1];
                        DateTime[] datetimeok = new DateTime[] { Convert.ToDateTime(datumok[0]), Convert.ToDateTime(datumok[1]) };
                        DateTime ujdatum;
                        if (mindatum.CompareTo(elem1) > 0)
                            ujdatum = Convert.ToDateTime(mindatum);
                        else
                            ujdatum = Convert.ToDateTime(elem1);
                        FakUserInterface.Cegadatok(ujdatum);

                        Datumtoligfeltetel = "(DATUMTOL IS NULL AND (DATUMIG IS NULL OR DATUMIG >= '" + elem1
                            + "') OR DATUMTOL IS NOT NULL AND DATUMTOL <= '" + elem2
                            + "' AND (DATUMIG IS NULL OR DATUMIG >= '" + elem1 + "')) ";
                        Kezdetefeltetel = "(KEZDETE IS NULL OR KEZDETE <= '" + elem2
                            + "') AND (VEGE IS NULL  OR VEGE >= '" + elem1 + "') ";
                        foreach (Tablainfo egyinfo in tabinfok)
                        {
                            string datumstring = "";
                            if (egyinfo.TablaColumns.IndexOf("DATUMTOL") != -1)
                                egyinfo.DatumString = Datumtoligfeltetel;

                            else if (egyinfo.TablaColumns.IndexOf("KEZDETE") != -1)
                                egyinfo.DatumString = Kezdetefeltetel;
                            else if (egyinfo.SpecDatumNevekArray.Count != 0)
                            {
                                for (int i = 0; i < egyinfo.SpecDatumNevek.Length; i++)
                                {
                                    if (egyinfo.SpecDatumNevSzerepel[i] || egyinfo == elsotabinfo)
                                    {
                                        if (datumstring == "")
                                            datumstring += "(";
                                        else
                                            datumstring += " OR ";
                                        string nev = egyinfo.SpecDatumNevek[i];
                                        datumstring += nev + " <= '" + elem2 + "' AND " + nev + " >= '" + elem1 + "'";
                                    }
                                }
                                if (datumstring != "")
                                    datumstring += ")";
                                egyinfo.DatumString = datumstring;
                            }
                            if (egyinfo == elsotabinfo)
                            {
                                selstring = elsotabinfo.GetTablainfoSelect(OsszesBeallitottIdErtek);
                                FakUserInterface.Select(fotabla.Table, FakUserInterface.AktualCegconn, fotabla.Table.TableName, selstring, "", false);
                                elsotabinfo.Tartalmaktolt();
                            }
                            //if (egyinfo != ElsoTabinfo)
                            //{
                            //    egyinfo.BeallitandoIdkArray(elsotabinfoid, ElsoTabinfo, Tablainfok, OsszesBeallitandoId, OsszesBeallitottIdErtek);
                            //}
                        }
                    }

                    do
                    {
                        elsotabinfo.RowFilterIndex = -1;
                        elsotabinfo.DataView.RowFilter = "";
                        elsotabinfo.SetRowFilter();
                        savfilt = elsotabinfo.DataView.RowFilter;
                        SaveFilterek[tabinfok.IndexOf(elsotabinfo)] = savfilt;

                        for (int i = 0; i < fotabla.Count; i++)
                        {
                            elsotabinfo.ViewSorindex = i;
                            SaveViewIndexek[0] = elsotabinfo.ViewSorindex;
                            dr = fotabla[i].Row;
                            elsotabinfo.Tartalmaktolt(i);
                            elsotabinfo.IdErtekBeallitasok(dr, OsszesBeallitottIdErtek);
                            ures = TobbiTabinfoSelect();
                            if (!ures)
                            {
                                if (firsttime)
                                    ReportHeaderOsszeallit();
                                do
                                {
                                    sorrendvaltozasszint = SorrendValtozasok();
                                    kellcsakosszegsorba = false;
                                    if (sorrendvaltozasszint != -1)
                                    {
                                        int tol = Listainfok.Sorrendinfok.Mezoinfok.Count - 1;
                                        if (listae)
                                            count = Listainfok.Oszlopinfok.Mezoinfok.Count - 1;
                                        else
                                            count = (Listainfok.Matrixpontinfok.OszlopokSzama + 1) * Listainfok.Matrixpontinfok.Mezoinfok.Count;

                                        if (listae && (Listainfok.Oszlopinfok.Osszegzendok.Count != 0 || Listainfok.Oszlopinfok.Atlagolandok.Count!=0) || !listae)
                                        {
                                            do
                                            {
                                                dr = dt.NewRow();
                                                s = Listainfok.Sorrendinfok.Mezoinfok[tol].ElozoSorrendTartalom;
                                                if (s != "")
                                                {
                                                    if (listae || tol != Listainfok.Sorrendinfok.Mezoinfok.Count - 1)
                                                        s += " összesen:";
                                                    dr[0] = s;
                                                    dt.Rows.Add(dr);
                                                    if (listae)
                                                        Listainfok.Oszlopinfok.OsszegsorOsszeallit(tol, dt);
                                                    else
                                                        Listainfok.Matrixpontinfok.OsszegsorOsszeallit(tol, dt);
                                                    if (!listae)
                                                    {
                                                        dr = dt.NewRow();
                                                        for (int j = 0; j <= count; j++)
                                                            dr[j] = "__________";
                                                        dt.Rows.Add(dr);
                                                    }
                                                }
                                                tol = tol - 1;
                                            } while (sorrendvaltozasszint <= tol);
                                        }
                                        if (listae)
                                        {
                                            dr = dt.NewRow();
                                            for (int j = 0; j <= count; j++)
                                                dr[j] = "__________";
                                            dt.Rows.Add(dr);
                                        }
                                    }
                                    foreach (Mezoinfo info in Listainfok.Sorrendinfok.Mezoinfok)
                                    {
                                        string tart = info.ColumnInfo.Tablainfo.GetSorrendtartalom(info);
                                        if (tart != "")
                                        {
                                            kellcsakosszegsorba = true;
                                            if (listae)
                                            {
                                                dr = dt.NewRow();
                                                dr[0] = tart;
                                                dt.Rows.Add(dr);
                                            }
                                        }
                                    }
                                    if (listae)
                                    {
                                        foreach (Tablainfo egytabinfo in oszloptabinfok)
                                        {
                                            if (egytabinfo.ViewSorindex != -1)
                                                egytabinfo.Tartalmaktolt(egytabinfo.ViewSorindex);
                                        }
                                        if (egysor != "")
                                            egysor += newline[0];
                                        dr = dt.NewRow();
                                        for (int j = 0; j < dt.Columns.Count; j++)
                                        {
                                            if (j < Listainfok.Oszlopinfok.Mezoinfok.Count)
                                            {
                                                Mezoinfo info = Listainfok.Oszlopinfok.Mezoinfok[j];
                                                string tart = "";
                                                if (!info.CsakOsszegsorba || kellcsakosszegsorba)
                                                    tart = info.ColumnInfo.Tablainfo.GetOszlopTartalom(info);
                                                if (!info.CsakOsszegsorba)
                                                {
                                                    split = tart.Split(vesszo);
                                                    if (split.Length == 2)
                                                    {
                                                        if (split[1].Length > 2)
                                                        {
                                                            split[1] = split[1].Substring(0, 2);
                                                            tart = split[0] + "," + split[1];
                                                        }
                                                    }
                                                    dr[j] = tart;
                                                }
                                                else
                                                    dr[j] = "";
                                                if (info.Osszegzendo || info.Atlagolando)
                                                    Listainfok.Oszlopinfok.Osszegfeladasok(info);
                                            }
                                            else
                                                dr[j] = "";
                                        }
                                        dt.Rows.Add(dr);
                                        ures = TobbiTabinfoKovsor();
                                    }
                                    else
                                    {
                                        for (int j = 0; j < tabinfok.Count; j++)
                                        {
                                            tabinfok[j].ViewSorindex = SaveViewIndexek[j];
                                            tabinfok[j].IdErtekBeallitasok(tabinfok[j].AktualViewRow, OsszesBeallitottIdErtek);
                                        }
                                        for (int j = 0; j < SorokSzama; j++)
                                        {
 //                                           elsotabinfo.DataView.RowFilter = "";
                                            elsotabinfo.ViewSorindex = i;
                                            ures = ElsoTabinfoSorVizsg(j);
                                            if (!ures)
                                            {
                                                ures = TobbiSorTabinfoSelect(j);
                                                bool elso = true;
                                                if (!ures)
                                                {
                                                    do
                                                    {
                                                        if (!elso)
                                                        {
                                                            Tablainfo tabinfo;
                                                            for (int l = 0; l < Listainfok.Matrixpontinfok.Mezoinfok.Tablainfok.Count; l++)
                                                            {
                                                                tabinfo = Listainfok.Matrixpontinfok.Mezoinfok.Tablainfok[l];
                                                                int id = tabinfok.IndexOf(tabinfo);
                                                                tabinfo.ViewSorindex = SaveViewIndexek[id] + 1;
                                                                tabinfo.Tartalmaktolt(tabinfo.ViewSorindex);
                                                                tabinfo.IdErtekBeallitasok(tabinfo.AktualViewRow, OsszesBeallitottIdErtek);
                                                                SaveViewIndexek[id] = tabinfo.ViewSorindex;
                                                            }
                                                            ures = true;
                                                            for (int l = 0; l < Listainfok.Matrixpontinfok.Mezoinfok.Tablainfok.Count; l++)
                                                            {
                                                                tabinfo = Listainfok.Matrixpontinfok.Mezoinfok.Tablainfok[l];
                                                                if (tabinfo.ViewSorindex != -1)
                                                                    ures = false;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            for (int l = 0; l < Listainfok.Matrixpontinfok.Mezoinfok.Tablainfok.Count; l++)
                                                            {
                                                                Tablainfo tabinfo = Listainfok.Matrixpontinfok.Mezoinfok.Tablainfok[l];
                                                                int id = tabinfok.IndexOf(tabinfo);
                                                                SaveViewIndexek[id] = tabinfo.ViewSorindex;
                                                            }
                                                            elso = false;
                                                        }
                                                        if (!ures)
                                                        {
                                                            for (int k = 0; k < OszlopokSzama; k++)
                                                            {
 //                                                               elsotabinfo.DataView.RowFilter = "";
                                                                if (i > elsotabinfo.DataView.Count - 1)
                                                                    elsotabinfo.ViewSorindex = elsotabinfo.DataView.Count - 1;
                                                                else
                                                                    elsotabinfo.ViewSorindex = i;
                                                                ures = ElsoTabinfoOszlopVizsg(k);
                                                                if (!ures)
                                                                {
                                                                    ures = TobbiOszlopTabinfoSelect(k);
                                                                    if (!ures)
                                                                    {
                                                                        Listainfok.Matrixpontinfok.Osszegfeladasok(j, k);
                                                                        Listainfok.Matrixpontinfok.Osszegfeladasok(j, OszlopokSzama);
                                                                        Listainfok.Matrixpontinfok.Osszegfeladasok(SorokSzama, k);
                                                                        Listainfok.Matrixpontinfok.Osszegfeladasok(SorokSzama, OszlopokSzama);
                                                                    }
                                                                }
                                                            }
                                                            for (int k = 0; k < tabinfok.Count; k++)
                                                                tabinfok[k].ViewSorindex = SaveViewIndexek[k];
                                                        }
                                                    } while (!ures);
                                                }
                                            }
                                            for (int k = 0; k < tabinfok.Count; k++)
                                            {
                                                tabinfok[k].ViewSorindex = SaveViewIndexek[k];
                                                tabinfok[k].DataView.RowFilter = SaveFilterek[k];
                                            }
                                        }
                                        for (int k = 0; k < tabinfok.Count; k++)
                                        {
                                            tabinfok[k].ViewSorindex = SaveViewIndexek[k];
                                            tabinfok[k].DataView.RowFilter = SaveFilterek[k];
                                        }
                                        ures = true;
                                    }
                                } while (!ures);
                            }
                        }
                    } while (elsotabinfo.SetRowFilter());
                    datumtombcount++ ;
                } while (datumtombcount < Datumtomb.Count);

                    if (listae)
                        count = Listainfok.Oszlopinfok.Mezoinfok.Count - 1;
                    else
                        count = (Listainfok.Matrixpontinfok.OszlopokSzama + 1) * Listainfok.Matrixpontinfok.Mezoinfok.Count;

                    if (sorrendtabinfo != null)
                    {
                        int tol = Listainfok.Sorrendinfok.Mezoinfok.Count - 1;
                        if (listae && (Listainfok.Oszlopinfok.Osszegzendok.Count != 0 || Listainfok.Oszlopinfok.Atlagolandok.Count!=0 )|| !listae)
                        {
                            do
                            {
                                dr = dt.NewRow();
                                string s;
                                if (listae)
                                    s = Listainfok.Sorrendinfok.Mezoinfok[tol].ElozoSorrendTartalom;
                                else
                                    s = Listainfok.Matrixpontinfok.Sorrendszovegek[tol];
                                if (listae || tol != Listainfok.Sorrendinfok.Mezoinfok.Count - 1)
                                    s += " összesen:";
                                dr[0] = s;
                                dt.Rows.Add(dr);
                                if (listae)
                                    Listainfok.Oszlopinfok.OsszegsorOsszeallit(tol, dt);
                                else
                                    Listainfok.Matrixpontinfok.OsszegsorOsszeallit(tol, dt);

                                tol = tol - 1;
                                if (!listae)
                                {
                                    dr = dt.NewRow();
                                    for (int j = 0; j <= count; j++)
                                        dr[j] = "__________";
                                    dt.Rows.Add(dr);
                                }
                            } while (tol >= 0);
                        }
                        if (Listainfok.Oszlopinfok.Osszegzendok.Count != 0 || Listainfok.Oszlopinfok.Atlagolandok.Count!=0)
                        {
                            dr = dt.NewRow();
                            for (int j = 0; j <= count; j++)
                                dr[j] = "__________";
                            dt.Rows.Add(dr);
                            dr = dt.NewRow();
                            dr[0] = "Végösszesen:";
                            dt.Rows.Add(dr);
                            Listainfok.Oszlopinfok.OsszegsorOsszeallit(Listainfok.Sorrendinfok.Mezoinfok.Count, dt);
                        }
                    }
                    if (!listae)
                    {
                        if (sorrendtabinfo != null)
                        {
                            dr = dt.NewRow();
                            dr[0] = "Végösszesen:";
                            dt.Rows.Add(dr);
                        }
                        Listainfok.Matrixpontinfok.OsszegsorOsszeallit(Listainfok.Matrixpontinfok.SorrendmezokSzama, dt);
                    }
                //    datumtombcount++ ;
                //} while (datumtombcount < Datumtomb.Count);

                report.SetDataSource(dataset);
                string szov = "";
                report.SetParameterValue("cegnev", FakUserInterface.AktualCegnev);
                report.SetParameterValue("megnevezes", Tabinfo.Szoveg);
                report.SetParameterValue("feltetel", parfeltetel);
                if (parfeltetel != "")
                    szov = "Feltétel:";
                else
                    szov = "";
                report.SetParameterValue("feltszov", szov);
                if (paroszlopmeghatarozas != "")
                    szov = "Oszlopmeghatározások:";
                else
                    szov = "";
                report.SetParameterValue("oszlopmeghszov", szov);
                report.SetParameterValue("oszlopmeghatarozas", paroszlopmeghatarozas);
                if (parsormeghatarozas != "")
                    szov = "Sormeghatározások:";
                else
                    szov = "";
                report.SetParameterValue("sormeghszov", szov);
                report.SetParameterValue("sormeghatarozas", parsormeghatarozas);
                if (Datum2 == "")
                    report.SetParameterValue("datum", Datum1);
                else
                    report.SetParameterValue("datum", Datum1 + " - " + Datum2);
                if (listae)
                {
                    for (int i = 0; i < Listainfok.Oszlopinfok.Mezoinfok.Count; i++)
                    {
                        Mezoinfo info = Listainfok.Oszlopinfok.Mezoinfok[i];
                        if (!info.ColumnInfo.Comboe && info.ColumnInfo.Numeric(info.ColumnInfo.DataType))
                        {
                            report.Section2.ReportObjects[i].ObjectFormat.HorizontalAlignment = Alignment.RightAlign;
                            report.Section3.ReportObjects[i].ObjectFormat.HorizontalAlignment = Alignment.RightAlign;
                        }
                        else
                        {
                            report.Section2.ReportObjects[i].ObjectFormat.HorizontalAlignment = Alignment.LeftAlign;
                            report.Section3.ReportObjects[i].ObjectFormat.HorizontalAlignment = Alignment.LeftAlign;
                        }
                    }
                }
                crystalReportViewer1.ReportSource = report;
                crystalReportViewer1.Enabled = false;
                crystalReportViewer1.Enabled = true;
            }
            else
                tabControl1.SelectedIndex = 0;
        }
        private bool ElsoTabinfoSorVizsg(int sorindex)
        {
            bool ures = false;
            if (elsotabinfo.SorRowFilterek.Count != 0)
            {
                string filt = elsotabinfo.SorRowFilterek[sorindex].ToString();
                if (filt != "")
                {
                    string savfilt = elsotabinfo.DataView.RowFilter;
                    filt = "(" + filt + ") AND " + elsotabinfo.IdentityColumnName + "=" + elsotabinfo.AktIdentity.ToString();
                    if (savfilt == "")
                        elsotabinfo.DataView.RowFilter = filt;
                    else
                        elsotabinfo.DataView.RowFilter = "(" + savfilt + ") AND " +
                            "(" + filt + ")";
                    if (elsotabinfo.DataView.Count == 0)
                    {
                        elsotabinfo.DataView.RowFilter = savfilt;
                        ures = true;
                    }
                    else if (elsotabinfo.ViewSorindex > elsotabinfo.DataView.Count - 1)
                        elsotabinfo.ViewSorindex = 0;
                }
            }
            return ures;
        }
        private bool ElsoTabinfoOszlopVizsg(int oszlopindex)
        {
            bool ures = false;
            if (elsotabinfo.OszlopRowFilterek.Count != 0)
            {
                string filt = elsotabinfo.OszlopRowFilterek[oszlopindex].ToString();
                if (filt != "")
                {
                    string savfilt = elsotabinfo.DataView.RowFilter;
                    filt = "(" + filt + ") AND " + elsotabinfo.IdentityColumnName + "=" + elsotabinfo.AktIdentity.ToString();
                    if (savfilt == "")
                        elsotabinfo.DataView.RowFilter = filt;
                    else
                        elsotabinfo.DataView.RowFilter = "(" + savfilt + ") AND " +
                            "(" + filt + ")";
                    if (elsotabinfo.DataView.Count == 0)
                    {
                        elsotabinfo.DataView.RowFilter = savfilt;
                        ures = true;
                    }
                    else if (elsotabinfo.ViewSorindex > elsotabinfo.DataView.Count - 1)
                        elsotabinfo.ViewSorindex = 0;
                }
            }
            return ures;
        }

        private bool TobbiTabinfoSelect()
        {
            string orderstring = "";
            string selstring = "";
            for (int j = 0; j < tabinfok.Count; j++)
            {
                if (tabinfok[j] != elsotabinfo)
                {
                    tabinfok[j].DataView.RowFilter = "";
                    orderstring = tabinfok[j].Sort;
                    tabinfok[j].DataView.Sort = orderstring;
                    selstring = tabinfok[j].GetTablainfoSelect(OsszesBeallitottIdErtek);
                    if (selstring.Contains("-1"))
                        tabinfok[j].Adattabla.Rows.Clear();
                    else
                    {
                        FakUserInterface.Select(tabinfok[j].Adattabla, FakUserInterface.AktualCegconn, tabinfok[j].Tablanev, selstring, "", false);
                        tabinfok[j].Tartalmaktolt();
                    }
                    tabinfok[j].ViewSorindex = 0;
                    uresek[j] = !tabinfok[j].SetRowFilter();
                    tabinfok[j].IdErtekBeallitasok(tabinfok[j].AktualViewRow, OsszesBeallitottIdErtek);
                    SaveViewIndexek[j] = tabinfok[j].ViewSorindex;
                    SaveFilterek[j] = tabinfok[j].DataView.RowFilter;
                }
            }
            ures = false;
            for (int j = 0; j < tabinfok.Count; j++)
            {
                if (uresek[j])
                {
                    if (!tabinfok[j].EredetiLehetUres)
                        ures = true;
                    break;
                }
            }
            return ures;
        }
        private bool TobbiSorTabinfoSelect(int aktsorind)
        {
            ArrayList ar = (ArrayList)Listainfok.Sorfeltetelinfok.FeltetelinfoCollection.Reszelemek[aktsorind];
            Feltetelinfo[] feltinfok = (Feltetelinfo[])ar.ToArray(typeof(Feltetelinfo));
            TablainfoCollection tc = new TablainfoCollection();
            for (int i = 0; i < feltinfok.Length; i++)
                tc.Add(feltinfok[i].Tabinfo);

            for (int k = 0; k < tc.Count; k++)
            {
                Tablainfo tabinfo = tc[k];
                int j = tabinfok.IndexOf(tabinfo);
                string savfilt = SaveFilterek[j];
                if (tabinfo == elsotabinfo)
                    savfilt = tabinfo.DataView.RowFilter;
                tabinfo.DataView.RowFilter = "";
                if (savfilt != "")
                {
                    if (savfilt.Contains("OR"))
                        savfilt = "(" + savfilt + ")";
                    savfilt += " AND ";
                }
                string filt = tabinfo.GetSorRowFilter(aktsorind);
                if (filt != "")
                {
                    if (filt.Contains("OR"))
                        filt = "(" + filt + ")";
                    tabinfo.DataView.RowFilter = savfilt + filt;
                }
                uresek[j] = tabinfo.DataView.Count == 0;
                if (uresek[j])
                {
                    tabinfo.ViewSorindex = -1;
                    sorokszama[j] = -1;
                }
                else
                {
                    tabinfo.ViewSorindex = 0;
                    sorokszama[j] = tabinfo.DataView.Count - 1;
                }
                tabinfo.IdErtekBeallitasok(tabinfo.AktualViewRow, OsszesBeallitottIdErtek);

                //               }

            }
            ures = false;
            for (int j = 0; j < tabinfok.Count; j++)
            {
                if (uresek[j])
                {
                    ures = true;
                    break;
                }
            }
            return ures;
        }
        private bool TobbiOszlopTabinfoSelect(int aktoszlind)
        {
            ArrayList ar = (ArrayList)Listainfok.Oszlopfeltetelinfok.FeltetelinfoCollection.Reszelemek[aktoszlind];
            Feltetelinfo[] feltinfok = (Feltetelinfo[])ar.ToArray(typeof(Feltetelinfo));
            TablainfoCollection tc = new TablainfoCollection();
            for (int i = 0; i < feltinfok.Length; i++)
                tc.Add(feltinfok[i].Tabinfo);

            for (int k = 0; k < tc.Count; k++)
            {
                Tablainfo tabinfo = tc[k];
                int j = tabinfok.IndexOf(tabinfo);
                string savfilt = SaveFilterek[j];
                if (tabinfo == elsotabinfo)
                    savfilt = tabinfo.DataView.RowFilter;
                tabinfo.DataView.RowFilter = "";
                if (savfilt != "")
                {
                    if (savfilt.Contains("OR") && !savfilt.EndsWith(")"))
                        savfilt = "(" + savfilt + ")";
                    savfilt += " AND ";
                }
                string filt = tabinfo.GetOszlopRowFilter(aktoszlind);
                if (filt != "")
                {
                    if (filt.Contains("OR"))
                        filt = "(" + filt + ")";
                    tabinfo.DataView.RowFilter = savfilt + filt;
                }
                uresek[j] = tabinfo.DataView.Count == 0;
                if (uresek[j])
                    tabinfo.ViewSorindex = -1;
                else
                    tabinfo.ViewSorindex = 0;
                tabinfo.IdErtekBeallitasok(tabinfo.AktualViewRow, OsszesBeallitottIdErtek);

            }
            ures = false;
            for (int j = 0; j < tabinfok.Count; j++)
            {
                if (uresek[j])
                {
                    ures = true;
                    break;
                }
            }
            return ures;
        }
        private void ReportHeaderOsszeallit()
        {
            dr = dt.NewRow();
            int tol = 0;
            int len = 0;
            string[] fejsorok;
            int step = 0;
            if (listae)
            {
                fejsorok = Listainfok.Oszlopinfok.Fejsorok;
                len = fejsorok.Length;
                step = 1;
            }
            else
            {
                fejsorok = Listainfok.Oszlopfeltetelinfok.Fejsorok;
                tol = 1;
                len = fejsorok.Length + 1;
                step = Listainfok.Matrixpontinfok.Mezoinfok.Count;
            }
            int fejsorindex = 0;
            int k = tol;
            do
            {
                if (fejsorindex < fejsorok.Length)
                    dr[k] = fejsorok[fejsorindex];
                else if (fejsorindex == fejsorok.Length && !listae)
                    dr[k] = "Összesen";
                fejsorindex++;
                k = k + step;
            } while (fejsorindex <= fejsorok.Length);
            if (!listae)
            {
                string[] fejek = new string[step];
                for (int i = 0; i < fejek.Length; i++)
                    fejek[i] = Listainfok.Matrixpontinfok.Fejsorok[i];
                int count = Listainfok.Matrixpontinfok.OszlopokSzama;
                fejsorindex = 11;
                for (int i = 0; i <= count; i++)
                {
                    for (int j = 0; j < fejek.Length; j++)
                    {
                        dr[fejsorindex] = fejek[j];
                        fejsorindex++;
                    }
                }
            }
            dt.Rows.Add(dr);
            if (sorrendtabinfo == null)
                firsttime = false;
            dt = dataset.DataTable3;
        }
        private int SorrendValtozasok()
        {
            if (sorrendtabinfo != null)
            {
                for (int j = 0; j < sorrendtabinfo.Length; j++)
                {
                    if (sorrendtabinfo[j].ViewSorindex != -1)
                        sorrendtabinfo[j].Tartalmaktolt(sorrendtabinfo[j].ViewSorindex);
                }
                if (firsttime)
                {
                    firsttime = false;
                    return -1;
                }
                else
                {
                    int sorrendvaltozasszint = -1;
                    for (int j = 0; j < Listainfok.Sorrendinfok.Mezoinfok.Count; j++)
                    {
                        Mezoinfo info = Listainfok.Sorrendinfok.Mezoinfok[j];
                        if (info.ColumnInfo.Tablainfo.Sorrendvaltozas(info))
                        {
                            sorrendvaltozasszint = j;
                            break;
                        }
                    }
                    return sorrendvaltozasszint;
                }
            }
            else
                return -1;
        }
        private bool TobbiTabinfoKovsor()
        {
            Tablainfo egyneltobb = null;
            int tol = tabinfok.Count - 1;
            bool[] ujselectkell = new bool[tabinfok.Count];
            int vanfalse = -1;
            for(int i=0;i<ujselectkell.Length;i++)
                ujselectkell[i] = true;
            bool ures = true;
            do
            {
                egyneltobb = tabinfok[tol];
                egyneltobb.ViewSorindex = SaveViewIndexek[tol];
                if (egyneltobb.DataView.Count != 0 && egyneltobb.ViewSorindex < egyneltobb.DataView.Count - 1)
                {
                    ujselectkell[tol] = false;
                    vanfalse=tol;
                    break;
                }
                tol --;
            } while (tol>=0);
 //           for
            if(vanfalse< 1)
                return true;
            egyneltobb = tabinfok[vanfalse];
            egyneltobb.ViewSorindex++;
            SaveViewIndexek[vanfalse] = egyneltobb.ViewSorindex;
            egyneltobb.IdErtekBeallitasok(egyneltobb.AktualViewRow, OsszesBeallitottIdErtek);
            for (int i = vanfalse+1; i < tabinfok.Count; i++)
            {
                tabinfok[i].DataView.RowFilter = "";
                string orderstring = tabinfok[i].Sort;
                tabinfok[i].DataView.Sort = orderstring;
                string selstring = tabinfok[i].GetTablainfoSelect(OsszesBeallitottIdErtek);
                if (selstring.Contains("-1"))
                    tabinfok[i].Adattabla.Rows.Clear();
                else
                {
                    FakUserInterface.Select(tabinfok[i].Adattabla, FakUserInterface.AktualCegconn, tabinfok[i].Tablanev, selstring, "", false);
                    tabinfok[i].Tartalmaktolt();
                }
                tabinfok[i].ViewSorindex = 0;
                uresek[i] = !tabinfok[i].SetRowFilter();
                tabinfok[i].IdErtekBeallitasok(tabinfok[i].AktualViewRow, OsszesBeallitottIdErtek);
                SaveViewIndexek[i] = tabinfok[i].ViewSorindex;
                SaveFilterek[i] = tabinfok[i].DataView.RowFilter;
            }
                //for(int i=0;i<ujselectkell.Length;i++)
                //{
                //    if(ujselectkell[i])
                //    {
                //        if(i==0)
                //            return true;
                //        else
                //        {
                //            for(tol=i;tol<tabinfok.Count;tol++)
                //            {
                //                tabinfok[tol].DataView.RowFilter = "";
                //                string orderstring = tabinfok[tol].Sort;
                //                tabinfok[tol].DataView.Sort = orderstring;
                //                string selstring = tabinfok[tol].GetTablainfoSelect(OsszesBeallitottIdErtek);
                //                if (selstring.Contains("-1"))
                //                    tabinfok[tol].Adattabla.Rows.Clear();
                //                else
                //                    FakUserInterface.Select(tabinfok[tol].Adattabla, FakUserInterface.AktualCegconn, tabinfok[tol].Tablanev, selstring, "", false);
                //                tabinfok[tol].ViewSorindex = 0;
                //                uresek[tol] = !tabinfok[tol].SetRowFilter();
                //                tabinfok[tol].IdErtekBeallitasok(tabinfok[tol].AktualViewRow, OsszesBeallitottIdErtek);
                //                SaveViewIndexek[tol] = tabinfok[tol].ViewSorindex;
//            }
            ures = false;
            for (int j = 0; j < tabinfok.Count; j++)
            {
                if (uresek[j])
                {
                    if (!tabinfok[j].EredetiLehetUres)
                        ures = true;
                    break;
                }
            }
            return ures;
        }
        /// <summary>
        /// nyomtmin,myomtmax comboitemek beallitasa
        /// </summary>
        /// <param name="combo">
        /// combobox
        /// </param>
        /// <param name="min">
        /// minimum
        /// </param>
        /// <param name="max">
        /// maximum
        /// </param>
        public virtual void ItemsBeallit(ComboBox combo, int min, int max)
        {
            ArrayList ar = new ArrayList();
            for (int j = min; j <= max; j++)
                ar.Add(j.ToString());
            string[] st = (string[])ar.ToArray(typeof(string));
            string selitem = "";
            if (combo.SelectedIndex != -1)
                selitem = combo.SelectedItem.ToString();
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
        }
        /// <summary>
        /// nyomtmin/nyomtmax combo selectalt item valtozas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void nyomt_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox combo = (ComboBox)sender;
            if (combo.Items.Count != 0 && combo.SelectedIndex != -1)
            {
                string ertek = combo.SelectedItem.ToString();
                switch (combo.Name)
                {
                    case "nyomtmin":
                        ItemsBeallit(nyomtmax, Convert.ToInt32(ertek), maxpagenumber);
                        break;
                    case "nyomtmax":
                        ItemsBeallit(nyomtmin, pagenumber, Convert.ToInt32(ertek));
                        break;
                }
            }
        }
        /// <summary>
        /// user hivhatja a feltetelek kiegeszitesere
        /// </summary>
        /// <param name="felttabla"></param>
        public virtual void Userfeltetelekkiegeszit(AdatTabla felttabla)
        {
            felttabla.Rows.Clear();
            //DataRow dr = felttabla.NewRow();
            //dr["AZONTIP"] = "T CTNEVEK";
            //dr["MEZONEV"] = "NYILVCSOP";
            //dr["RELACIO"] = "=";
            //dr["MASODIKELEM"] = "1.Nyilv.csop";
            //dr["ESVAGY"] = "VAGY";
            //felttabla.Rows.Add(dr);
            //dr = felttabla.NewRow();
            //dr["AZONTIP"] = "T CTNEVEK";
            //dr["MEZONEV"] = "NYILVCSOP";
            //dr["RELACIO"] = "=";
            //dr["MASODIKELEM"] = "2.Nyilv.csop";
            //felttabla.Rows.Add(dr);
        }
        /// <summary>
        /// buttonok clickje
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
                    if (nyomtmin != null && maxpagenumber != 0)
                    {
                        crystalReportViewer1.PrintReport();
 //                       int elsopage = Convert.ToInt32(nyomtmin.SelectedItem.ToString());
 //                       int utsopage = Convert.ToInt32(nyomtmax.SelectedItem.ToString());
 //                       report.PrintToPrinter(1, false, elsopage, utsopage);
                    }
                    break;
            }
            
            lapszam.Text = "Lapszám: " + crystalReportViewer1.GetCurrentPageNumber().ToString();
            if (maxpagenumber > 1)
            {
                elsolap.Enabled = aktpagenumber > 1;
                elozo.Enabled = aktpagenumber > 1;
                kovetkezo.Enabled = maxpagenumber > aktpagenumber;
                utsolap.Enabled = maxpagenumber > aktpagenumber;
            }
        }
        /// <summary>
        /// tol/ig datetimepickerek validalasa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            if (!FakUserInterface.EventTilt)
            {
                FakUserInterface.EventTilt = true;
                DateTimePicker picker = (DateTimePicker)sender;
                bool elsodatum = picker.Name.EndsWith("1");
                if (VanDatum)
                {
                    if (elsodatum)
                        dateTimePicker2.Value = dateTimePicker1.Value;
                    else
                        dateTimePicker1.Value = dateTimePicker2.Value;
                    FakUserInterface.EventTilt = false;
                    return;
                }
                string year1 = dateTimePicker1.Value.Year.ToString() + ".";
                string year2 = dateTimePicker2.Value.Year.ToString() + ".";
                if(year1!=year2)
                {
                    if (elsodatum)
                    {
                        year2 = year1;
                        dateTimePicker2.Value = dateTimePicker1.Value;
                    }
                    else
                    {
                        year1 = year2;
                        dateTimePicker2.Value = dateTimePicker1.Value;
                    }
                }
                string month1 = "";
                string month2 = "";
                string nap1 = "";
                string nap2 = "";
                if (TeljesEv)
                {
                    month1 = "01.";
                    month2 = "12.";
                    nap1 = "01";
                    nap2 = "31";
                    dateTimePicker2.Value = Convert.ToDateTime(year2 + month2 + nap2);
                    dateTimePicker1.Value = Convert.ToDateTime(year1 + month1 + nap1);
                }
                else
                {
                    month1 = dateTimePicker1.Value.Month.ToString();
                    if (month1.Length == 1)
                        month1 = "0" + month1;
                    month1 += ".";
                    month2 = dateTimePicker2.Value.Month.ToString();
                    if (month2.Length == 1)
                        month2 = "0" + month2;
                    month2 += ".";
                    if (month1.CompareTo(month2) > 0)
                    {
                        if (elsodatum)
                        {
                            month2 = month1;
                            dateTimePicker2.Value = dateTimePicker1.Value;
                        }
                        else
                        {
                            month1 = month2;
                            dateTimePicker1.Value = dateTimePicker2.Value;
                        }
                    }
                    if (TeljesHonap)
                    {
                        string elsonap = year1 + month1 + "01";
                        dateTimePicker1.Value = Convert.ToDateTime(elsonap);
                        elsonap = year2 + month2 + "01";
                        dateTimePicker2.Value = Convert.ToDateTime(elsonap).AddMonths(1).AddDays(-1);
                    }
                    else
                    {
                        nap1 = dateTimePicker1.Value.Day.ToString();
                        if (nap1.Length == 1)
                            nap1 = "0" + nap1;
                        nap2 = dateTimePicker2.Value.Day.ToString();
                        if (nap2.Length == 1)
                            nap2 = "0" + nap2;
                        if (nap1.CompareTo(nap2) > 0 && month1 == month2)
                        {
                            if (elsodatum)
                                dateTimePicker2.Value = Convert.ToDateTime(year1 + month1 + nap1);
                            else
                                dateTimePicker1.Value = Convert.ToDateTime(year1 + month1 + nap2);
                        }
                    }
                }
                FakUserInterface.EventTilt = false;
            }
        }
            
        /// <summary>
        /// kivant lista/statisztika valasztas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override  void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox combo = (ComboBox)sender;
            listaazontip = combo.Tag.ToString();
            ListaTabinfo = FakUserInterface.GetByAzontip(listaazontip);
 //           teljeshonap = ListaTabinfo.Azonositok.Teljeshonap;
            Beallitasok();
        }
        /// <summary>
        /// pagevaltas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            string month1 = dateTimePicker1.Value.Month.ToString();
            if (month1.Length == 1)
                month1 = "0" + month1;
            month1+=".";
            string month2 = dateTimePicker2.Value.Month.ToString();
            if (month2.Length == 1)
                month2 = "0" +month2;
            month2 += ".";
            string nap1 = dateTimePicker1.Value.Day.ToString();
            if (nap1.Length == 1)
                nap1 = "0" + nap1;
            string nap2 = dateTimePicker2.Value.Day.ToString();
            if (nap2.Length == 1)
                nap2 = "0" + nap2;
            string year = dateTimePicker1.Value.Year.ToString() + ".";
            string datum1 = year + month1 + nap1;
            string datum2 = year + month2 + nap2;
            string utsonap;
            int tol = dateTimePicker1.Value.Month;
            int ig = dateTimePicker2.Value.Month;
            csakegyhonap = month1 == month2;
            if (!FakUserInterface.EventTilt)
            {
                if (e.TabPageIndex == 0)
                    Beallitasok();
                if (e.TabPageIndex == 1)
                {
                    if (Datum1 != datum1 || Datum2 != datum2 || ValtozasLekerdez().Count != 0)
                    {
                        ValtozasBeallit();
                        Datumtomb.Clear();
                        //if (VanDatum)
                        //{
                            Datum1 = datum1;
                            Datum2 = datum2;
                            string elem1;
                            string elem2;
                            string month;
                            string kovmonth;
                            //                        DateTime tim;
                            for (int i = tol; i <= ig; i++)
                            {
                                month = i.ToString();
                                kovmonth = (i + 1).ToString();
                                if (month.Length == 1)
                                    month = "0" + month;
                                if (kovmonth.Length == 1)
                                    kovmonth = "0" + kovmonth;
                                kovmonth += ".";
                                if (i == tol)
                                {
                                    elem1 = year + month1 + nap1;
                                    if (csakegyhonap)
                                        elem2 = year + month2 + nap2;
                                    else
                                    {
                                        utsonap = Convert.ToDateTime(year + kovmonth + "01").AddDays(-1).Day.ToString();
                                        elem2 = year + month + "." + utsonap;
                                    }
                                }
                                else
                                {
                                    elem1 = year + month + ".01";
                                    if (i < ig)
                                    {
                                        utsonap = Convert.ToDateTime(year + kovmonth + "01").AddDays(-1).Day.ToString();
                                        elem2 = year + month + "." + utsonap;
                                    }
                                    else
                                        elem2 = year + month2 + nap2;
                                }
                                Datumtomb.Add(new string[] { elem1, elem2 });
                            }
                            string[] egyelem = (string[])Datumtomb[0];
                            elem1 = egyelem[0];
                            elem2 = egyelem[1];
                            Datumtoligfeltetel = "(DATUMTOL IS NULL AND (DATUMIG IS NULL OR DATUMIG >= '" + elem1
                                + "') OR DATUMTOL IS NOT NULL AND DATUMTOL <= '" + elem2
                                + "' AND (DATUMIG IS NULL OR DATUMIG >= '" + elem1 + "')) ";
                            Kezdetefeltetel = "(KEZDETE IS NULL OR KEZDETE <= '" + elem2
                                + "') AND (VEGE IS NULL  OR VEGE >= '" + elem1 + "') ";
 //                       }
                        Listainfok = new ListaInfok(this, osszesview);
                        if (!Listainfok.Parameterhiba)
                        {
                            Listazas();
                            ValtozasTorol();
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// zoomvaltas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void zoom_SelectionChangeCommitted(object sender, EventArgs e)
        {

            string szazal = comboBox2.Items[comboBox2.SelectedIndex].ToString();
            zoomszazalek = Convert.ToInt16(szazal);
            crystalReportViewer1.Zoom(zoomszazalek);
        }

        private void crystalReportViewer1_EnabledChanged(object sender, EventArgs e)
        {
            if (!FakUserInterface.EventTilt && crystalReportViewer1.Enabled)
            {
                TabPage page = (TabPage)crystalReportViewer1.Controls[0].Controls[0].Controls[0];
                string text = "";
                int size = crystalReportViewer1.Width;
                for (int i = 0; i < size; i++)
                    text += " ";
                page.Text = text;
                crystalReportViewer1.ShowLastPage();
                maxpagenumber = crystalReportViewer1.GetCurrentPageNumber();
                maxlapszam.Text = "Lapok száma: " + maxpagenumber.ToString();
                if (maxpagenumber == 0)
                {
                    lapszam.Text = "Lapszám: 0";
                    pagenumber = 0;
                    aktpagenumber = 0;
                }
                else
                {
                    crystalReportViewer1.ShowFirstPage();
                    lapszam.Text = "Lapszám: 1";
                    pagenumber = 1;
                    aktpagenumber = 1;
                }
                ItemsBeallit(nyomtmin, pagenumber, maxpagenumber);
                ItemsBeallit(nyomtmax, pagenumber, maxpagenumber);
                nyomtmax.SelectedIndex = nyomtmax.Items.Count - 1;
                bool vis = maxpagenumber > 1;
                elozo.Enabled = false;
                kovetkezo.Enabled = vis;
                elsolap.Enabled = false;
                utsolap.Enabled = vis;
            }

        }
    }
}
