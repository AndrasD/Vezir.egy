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
using FakPlusz.VezerloFormok;

namespace FakPlusz.Formok
{
    /// <summary>
    /// Naptarkezeles
    /// </summary>
    public partial class Naptar : Gridpluszinput
    {
        private string[] magyarnapok = new string[] { "Hétfö", "Kedd", "Szerda", "Csütörtök", "Péntek", "Szombat", "Vasárnap" };
        private string[] angolnapok = new string[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        private ArrayList angolarray;
        private int husvethetfoho=0;
        private int husvethetfonap=0;
        private int punkosdhetfoho=0;
        private int punkosdhetfonap=0;
        int husvetvasho = 0;
        int husvetvasnap = 0;
        int punkosdvasho = 0;
        int punkosdvasnap = 0;
        private DataGridViewCell tempcellpiheno = new DataGridViewComboBoxCell();
        private DataGridViewCell tempcellmunka = new DataGridViewComboBoxCell();
        private DataGridViewCell tempcellunnep = new DataGridViewComboBoxCell();
        private DataGridViewCell tempcellegyeb = new DataGridViewComboBoxCell();

        private DataGridViewCell tempcell1piheno = new DataGridViewTextBoxCell();
        private DataGridViewCell tempcell1munka = new DataGridViewTextBoxCell();
        private DataGridViewCell tempcell1unnep = new DataGridViewTextBoxCell();
        private DataGridViewCell tempcell1egyeb = new DataGridViewComboBoxCell();
        private TabPage parameterpage = null;
        private int ev = 0;
        private int elszkezdho = 1;
        private int tanevkezdho = 1;
        private int maxev = DateTimePicker.MaximumDateTime.Year;
        private HozferJogosultsag SajatHozferJog = HozferJogosultsag.Irolvas;
        private Cols Kodidcol = null;
        private Cols evparamcol = null;
        private string kodtipus = "";
        private string munkanapkezdobetu = "";
        private Tablainfo naptarkodtipusinfo;
        private Tablainfo napfajtakinfo;
        private Tablainfo kodtipfajtainfo;
        private Tablainfo elszevkezdhoinfo;
        private Tablainfo tanevkezdhoinfo;
        private Tablainfo kodtipkezdhoinfo;
        private Tablainfo szurtnapfajtainfo;
        private Tablainfo kodtipkezdtanevhoinfo;
        private int[] evtomb = new int[12];
        private int[] hotomb = new int[12];
//        private bool folytonosnaparrendben = false;
//        private bool egyeninaptarrendben = false;
        /// <summary>
        /// objectum letrehozas
        /// </summary>
        public Naptar()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vezerles"></param>
        /// <param name="leiroe"></param>
        public override void ParameterAtvetel(Vezerloinfo vezerles, bool leiroe)
        {
            base.ParameterAtvetel(vezerles, leiroe);
            tempcellmunka.Style.ForeColor = Color.Black;
            tempcellpiheno.Style.ForeColor = Color.Green;
            tempcellunnep.Style.ForeColor = Color.Red;
            tempcellegyeb.Style.ForeColor = Color.Blue;
            tempcell1munka.Style.ForeColor = Color.Black;
            tempcell1piheno.Style.ForeColor = Color.Green;
            tempcell1unnep.Style.ForeColor = Color.Red;
            tempcell1egyeb.Style.ForeColor = Color.Blue;
            angolarray = new ArrayList(angolnapok);
        }
        public virtual void Tempcellini1()
        {
            for (int i = 0; i < Tabinfo.DataView.Count; i++)
                Tempcellini1(i);
        }
        public virtual void Tempcellini1(int grid1rowindex)
        {
            if (this.Visible)
            {
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    try
                    {
                        DataGridViewCell dcell1 = dataGridView1.Rows[grid1rowindex].Cells[i];
                        string nev = dataGridView1.Columns[i].Name;
                        if (nev.StartsWith("N") && dcell1.Value.ToString() != "")
                        {
                            string val = dcell1.Value.ToString().Substring(0, 1);
                            if (val == munkanapkezdobetu)
                                dcell1.Style.ApplyStyle(tempcell1munka.Style);
                            else
                            {
                                switch (val)
                                {
                                    case "P":
                                        dcell1.Style.ApplyStyle(tempcell1piheno.Style);
                                        break;
                                    case "Ü":
                                        dcell1.Style.ApplyStyle(tempcell1unnep.Style);
                                        break;
                                    default:
                                        dcell1.Style.ApplyStyle(tempcell1egyeb.Style);
                                        break;
                                }
                            }
                        }
                    }
                    catch
                    {
                        break;
                    }
                }
            }
        }
        public virtual void Tempcellini2()
        {
            Cols egycol = null;
            string ev = "";
            string ho = "";
            string nap = "";
            string datumstring;
            DateTime datum;
            string szoveg;
            int elsonapsor = InputColumns.IndexOf("N01") - 1;
            DataGridViewCell szovegcell = null;
            Inputtabla.Columns[0].ReadOnly = false;
            for (int i = 0; i < InputColumns.Count; i++)
            {
                egycol = InputColumns[i];
                DataGridViewRow dgrow = dataGridView2.Rows[i];
                szovegcell = dgrow.Cells[0];
                if (egycol.ColumnName=="EV")
                    ev = dgrow.Cells[1].Value.ToString();
                if (egycol.ColumnName=="HONAP")
                {
                    ho = dgrow.Cells[1].Value.ToString();
                    if (ho.Length == 1)
                        ho = "0" + ho;
                }
                if (InputColumns[i].Comboe && InputColumns[i].ColumnName.StartsWith("N"))
                {
                    nap = (i - elsonapsor).ToString();
                    if (nap.Length == 1)
                        nap = "0" + nap;
                    szoveg = nap + " ";
                    datumstring = ev + "." + ho + "." + nap;
                    try
                    {
                        datum = Convert.ToDateTime(datumstring);
                        string szov = datum.DayOfWeek.ToString();
                        int szovind = angolarray.IndexOf(szov);
                        if (szovind != -1)
                            szov = magyarnapok[szovind];
                        szoveg = szoveg + szov;
                        Inputtabla.Rows[i][0] = szoveg;
                    }
                    catch { }
                    DataGridViewCell dcell = dgrow.Cells[1];
                    if (dcell.Value.ToString() != "")
                    {
                        string val = dcell.Value.ToString().Substring(0, 1);
                        if (val == munkanapkezdobetu)
                            dcell.Style.ApplyStyle(tempcellmunka.Style);
                        else
                        {
                            switch (val)
                            {
                                case "P":
                                    dcell.Style.ApplyStyle(tempcellpiheno.Style);
                                    break;
                                case "Ü":
                                    dcell.Style.ApplyStyle(tempcellunnep.Style);
                                    break;
                                default:
                                    dcell.Style.ApplyStyle(tempcellegyeb.Style);
                                    break;
                            }
                        }
                    }
                }
            }
            Inputtabla.Columns[0].ReadOnly = true;
        }
        /// <summary>
        /// 
        /// </summary>
        public override void AltalanosInit()
        {
            DataRow row = null;
            string maxev = "";
            if (!FakUserInterface.EventTilt)
            {
                if (TablainfoTag.Tablainfo.InputColumns.Count < 33)
                {
                    this.Visible = false;
                    Parameterez.Visible = false;
                    MessageBox.Show("A leirótáblában kevés az input-mezö!");
                    return;
                }
                if (parameterpage == null)
                    parameterpage = (TabPage)Parameterez.tabControl1.Controls[0];
                ArrayList valtozasok = ValtozasLekerdez();
                bool cegvaltozas = ValtozasLekerdez("CegValtozas").Count != 0;
                bool ujtag = UjTag;
                if (valtozasok.Count != 0)
                {
                    FakUserInterface.EventTilt = true;
                    this.Visible = false;
                    FakUserInterface.EventTilt = false;
                }
                if (ujtag || valtozasok.Count != 0 || Valtozas)
                {
                    UjTag = true;
                    base.AltalanosInit();
                    naptarkodtipusinfo = FakUserInterface.GetKodtab("R", "Naptarfajta");
                    napfajtakinfo = FakUserInterface.GetKodtab("R", "NAPFAJTA");
                    kodtipfajtainfo = FakUserInterface.GetOsszef("R", "Naptarkodtipnapfajta");
                    elszevkezdhoinfo = FakUserInterface.GetKodtab("R", "Evkezdet");
                    tanevkezdhoinfo = FakUserInterface.GetKodtab("R", "Tanevkezdet");
                    kodtipkezdhoinfo = FakUserInterface.GetCsoport(Tabinfo.Szint, "Naptarkodtipkezdho");
                    kodtipkezdtanevhoinfo = FakUserInterface.GetCsoport(Tabinfo.Szint, "Naptarkodtiptanevkezdho");
                    szurtnapfajtainfo = FakUserInterface.GetKodtab("R", "Szurtnapfajta");
                    szurtnapfajtainfo.TeljesTorles();
                    kodtipus = Tabinfo.Kodtipus;
                    string kodtipid = FakUserInterface.GetTartal(naptarkodtipusinfo, "SORSZAM", "SZOVEG", kodtipus)[0];
                    string kezdhoid = "";
                    string[] idk = FakUserInterface.GetTartal(kodtipkezdhoinfo, "SORSZAM2", "SORSZAM1", kodtipid);
                    if(idk!=null)
                        kezdhoid = idk[0];
                    if (kezdhoid != "")
                    {
                        idk = FakUserInterface.GetTartal(elszevkezdhoinfo, "KOD", "SORSZAM", kezdhoid);
                        if (idk != null)
                            elszkezdho = Convert.ToInt16(idk[0]);
                    }
                    string tanevkezdhoid = "";
                    idk = FakUserInterface.GetTartal(kodtipkezdtanevhoinfo, "SORSZAM2", "SORSZAM1", kodtipid);
                    if(idk!=null)
                        tanevkezdhoid = idk[0];
                    if (tanevkezdhoid != "")
                    {
                        idk = FakUserInterface.GetTartal(tanevkezdhoinfo, "KOD", "SORSZAM",tanevkezdhoid);
                        if (idk != null)
                            tanevkezdho = Convert.ToInt16(idk[0]);
                    }
                    kodtipfajtainfo.DataView.RowFilter = "SORSZAM1 = " + kodtipid;
                    ArrayList ar = new ArrayList();
                    for (int i = 0; i < kodtipfajtainfo.DataView.Count; i++)
                    {
                        row = kodtipfajtainfo.DataView[i].Row;
                        string napfajtaid = row["SORSZAM2"].ToString();
                        napfajtakinfo.DataView.RowFilter = "SORSZAM = " + napfajtaid;
                        string sorrend = napfajtakinfo.DataView[0].Row["SORREND"].ToString();
                        bool ins = false;
                        for (int j = 0; j < ar.Count; j++)
                        {
                            string egysorr = ar[j].ToString();
                            if (sorrend.CompareTo(egysorr) < 0)
                            {
                                ar.Insert(j, sorrend);
                                ins = true;
                                break;
                            }
                        }
                        if (!ins)
                            ar.Add(sorrend);
                    }
                    for (int i = 0; i < ar.Count; i++)
                    {
                        napfajtakinfo.DataView.RowFilter = "SORREND = " + ar[i].ToString();
                        row = napfajtakinfo.DataView[0].Row;
                        string kod = row["KOD"].ToString();
                        string szov = row["SZOVEG"].ToString();
                        if (i == 0)
                            munkanapkezdobetu = kod;
                        DataRow ujsor = szurtnapfajtainfo.Ujsor();
                        ujsor["KOD"] = kod;
                        ujsor["SZOVEG"] = szov;
                    }
                    
                    kodtipfajtainfo.DataView.RowFilter = "";
                    napfajtakinfo.DataView.RowFilter = "";
                    FakUserInterface.Rogzit(szurtnapfajtainfo);
                }
                Kodidcol=Tabinfo.TablaColumns["KOD_ID"];
                if (elszkezdho != 1)
                    evparamcol = Kodidcol;
                else
                    evparamcol = Tabinfo.TablaColumns["EV"];
                Cols elsonapcol = Tabinfo.TablaColumns["N01"];
                if(ValasztekIndex==-1)
                    ValasztekIndex=0;
                SajatHozferJog = Tabinfo.HozferJog;
                Tabinfo.DataView.RowFilter = "";
                if (SajatHozferJog!=Base.HozferJogosultsag.Irolvas && Tabinfo.DataView.Count == 0)
                {
                    Parameterez.Visible = false;
                    this.Visible = false;
                    FakPlusz.MessageBox.Show("Nincs adat!");
                    return;
                }
                Tablainfo cegszerz = FakUserInterface.GetBySzintPluszTablanev("C", "CEGSZERZODES");
                string indulodatum = "";
                string aktualdatum = "";
                bool vanadat = Tabinfo.DataView.Count != 0;
                if (!Tervezoe)
                {
                    string savfilt = cegszerz.DataView.RowFilter;
                    cegszerz.DataView.RowFilter = "ALKALMAZAS_ID=" + FakUserInterface.AlkalmazasId;
                    row = cegszerz.DataView[0].Row;
                    indulodatum = row["INDULODATUM"].ToString();
                    aktualdatum = row["AKTUALISDATUM"].ToString();
                    maxev = aktualdatum.Substring(0, 4);
                    cegszerz.DataView.RowFilter = savfilt;
                    Datumtol = Convert.ToDateTime(indulodatum);
                    Datumig = Convert.ToDateTime(aktualdatum);
                    Tabinfo.DataView.RowFilter = "KOD_ID = '" + maxev + "'";
                    vanadat = Tabinfo.DataView.Count != 0;
                    Tabinfo.DataView.RowFilter = "";
                }
                if(ujtag || cegvaltozas)
                {
                    if (!vanadat)
                    {
                        Verzioinfok verinf = FakUserInterface.VerzioInfok["R"];
                        if (Tabinfo.Szint=="R")
                        {
                            Datumtol = verinf.AktIntervallum[0];
                            ev = verinf.AktIntervallum[0].Year;
                            Evgyart(ev);
                            vanadat = true;
                        }
                        else
                        {
                            if (FakUserInterface.Alkalmazas == "TERVEZO")
                            {
                                Parameterez.Visible = false;
                                return;
                            }
//                            Tablainfo cegszerz = FakUserInterface.GetBySzintPluszTablanev("C", "CEGSZERZODES");
//                            cegszerz.DataView.RowFilter = "ALKALMAZAS_ID=" + FakUserInterface.AlkalmazasId;
 //                           DataRow row = cegszerz.DataView[0].Row;
                            //Datumtol = Convert.ToDateTime(row["INDULODATUM"].ToString());
                            ev = Convert.ToInt32(maxev);
                            Evgyart(ev);
                            vanadat = true;
                            //if (Muszaknaptar)
                            //{
                            //    muszaknaptarrendben = Muszaknaptargyart(Datumtol);
                            //    if (!muszaknaptarrendben)
                            //    {
                            //        Parameterez.Visible = false;
                            //        return;
                            //    }
                            //}
                        }
                    }
                    string evstring = "";
                    if (vanadat && Tervezoe)
                    {
                        evstring = Tabinfo.DataView[0].Row[evparamcol.ColumnName].ToString();
                        ev = Convert.ToInt32(evstring);
                        Datumtol = Convert.ToDateTime(evstring + ".01.01");
                        Datumig = DateTimePicker.MaximumDateTime;

                        if (SajatHozferJog != Base.HozferJogosultsag.Irolvas)
                        {
                            evstring = Tabinfo.DataView[Tabinfo.DataView.Count - 1].Row[evparamcol.ColumnName].ToString();
                            Datumig = Convert.ToDateTime(evstring + ".01.01");
                            if (SajatHozferJog == Base.HozferJogosultsag.Irolvas)
                                Datumig = Datumig.AddYears(1);
                        }
                        else
                        {
                            
                        }
                    }
                    if (VanValasztek)
                        Parameterez.ValasztekParameterekInit();
                    DateTime[] mindatum = new DateTime[1];
                    DateTime[] maxdatum = new DateTime[1];
                    DateTime[] alapertdatum = new DateTime[1];
                    DateTime[] aktdatum = new DateTime[1];
                    aktdatum[0] = Datumtol;
                    for (int i = 0; i < mindatum.Length; i++)
                    {
                        mindatum[i] = Datumtol;
                        maxdatum[i] = Datumig;
                        alapertdatum[i] = Datumtol;
                    }
                    int savwidth = dataGridView2.Columns[1].Width;

                    DatumParameterezInit(mindatum, maxdatum, alapertdatum, aktdatum);
                    dataGridView2.Columns[1].Width = savwidth;
                }
                ev = Datumtol.Year;
                Parameterez.Visible = true;
                Tabinfo.DataView.RowFilter = evparamcol.ColumnName +"= " + ev.ToString();
                if (SajatHozferJog != Base.HozferJogosultsag.Irolvas)
                {
                    if (Tabinfo.DataView.Count == 0)
                    {
                        Parameterez.Visible = false;
                        FakPlusz.MessageBox.Show("Nincs adat!");
                        return;
                    }
                }
                if (Valtozas)
                {
                    if (Tabinfo.DataView.Count == 0)
                    {
                        Tabinfo.DataView.RowFilter = "";
                        int maxev1 = Convert.ToInt32(Tabinfo.DataView[Tabinfo.DataView.Count - 1].Row[evparamcol.ColumnName].ToString());
                        for (int i = maxev1 + 1; i <= ev; i++)
                            Evgyart(i);
                        Tabinfo.DataView.RowFilter = evparamcol.ColumnName + "=" + ev.ToString();
                    }
                    Tabinfo.Tartalmaktolt();
                    ((Formvezerles)Hivo).terv.WriteLoginfo();
                }
                if (Parameterez.tabControl1.SelectedIndex == 1)
                    this.Visible = true;
                if (this.Visible)
                {
                    for (int i = 0; i < Tabinfo.DataView.Count; i++)
                    {
                        Tempcellini1();
                    }
                }
                if (ujtag || valtozasok.Count != 0 || DatumValtozas || ValasztekValtozas)
                    Tabinfo.ViewSorindex = 0;
                else
                {
                    Tabinfo.Tartalmaktolt();
                    Tabinfo.ViewSorindex = Tabinfo.ViewSorindex;
                }
                if (Valtozas)
                {
                    ValtozasokTorlese();
                    Parameterez.ValtozasokTorlese();
                }
                Inputtablaba();
            }
            else
            {
                UjTag = false;
                ValtozasTorol();
            }
        }
        public override bool DatumParameterezInit(DateTime[] mindatumok, DateTime[] maxdatumok, DateTime[] alapertdatumok, DateTime[] aktdatumok)
        {
            if (!Tervezoe)
                return Hivo.DatumParameterezInit(mindatumok, maxdatumok, alapertdatumok, aktdatumok);
            else
                return Parameterez.DatumParameterezInit(mindatumok, maxdatumok, alapertdatumok, aktdatumok);
        }
        /// <summary>
        /// 
        /// </summary>
        public override void VerziobuttonokAllit()
        {
            base.VerziobuttonokAllit();
            teljestorles.Visible = false;
            uj.Visible = false;
            elozoverzio.Visible = false;
            kovetkezoverzio.Visible = false;
        }
        private void Evgyart(int ev)
        {
            int i=0;
            int ho = elszkezdho;
            do
            {
                hotomb[i] = ho;
                evtomb[i] = ev;
                i++;
                ho++;
            } while (ho<13);
            if (elszkezdho != 1)
            {
                ho = 1;
                do
                {
                    hotomb[i] = ho;
                    evtomb[i] = ev + 1;
                    i++;
                    ho++;
                } while (i < 12);
            }
            Husvet(ev);
            if (elszkezdho != 1)
            {
                ArrayList ar = new ArrayList(hotomb);
                int k = ar.IndexOf(husvethetfoho);
                if (evtomb[k].ToString() != ev.ToString())
                {
                    Husvet(ev + 1);
                }
            }
            for (int j = 0; j < 12; j++)
            {
                DataRow newrow = Tabinfo.Ujsor();
                newrow[evparamcol.ColumnName] = ev;
                newrow["EV"] = evtomb[j];
                newrow["HONAP"] = hotomb[j];
                string honap = hotomb[j].ToString();
                if (honap.Length == 1)
                    honap = "0" + honap;
                string tims = evtomb[j].ToString() + "." + honap + "." + "01";
                DateTime tim = Convert.ToDateTime(tims).AddMonths(1).AddDays(-1);
                int maxnap = tim.Day;
                newrow = Unnepek(tims, newrow, maxnap);
            }
            Rogzit();
            Tabinfo.Tartalmaktolt();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void ok_Click(object sender, EventArgs e)
        {
            int viewsoridex = Tabinfo.ViewSorindex;
            if (viewsoridex == -1)
                viewsoridex = 0;
            base.ok_Click(sender, e);
            Tempcellini1(viewsoridex);
            Tempcellini1(Tabinfo.ViewSorindex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void comboBox1_TextUpdate(object sender, EventArgs e)
        {
            comboBox1.Text = aktivcol.ComboAktSzoveg;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (comboBox1.Visible && comboBox1.SelectedIndex != -1 && aktivcell.RowIndex != -1)
            {
                Cols egycol = InputColumns[aktivcell.RowIndex];
                string tartal = egycol.ComboAktSzoveg;
                if (tartal == "Ünnepnap")
                    comboBox1.SelectedIndex = 2;
                else if (comboBox1.SelectedIndex == 2 && tartal != "Ünnepnap")
                    comboBox1.SelectedIndex = comboBox1.Items.IndexOf(tartal);
                base.comboBox1_SelectionChangeCommitted(sender, e);
                tartal = egycol.ComboAktSzoveg;
                
                switch(comboBox1.SelectedIndex)
                {
                    case 0:
                        aktivcell.Style.ApplyStyle(tempcellmunka.Style);
                        break;
                    case 1:
                        aktivcell.Style.ApplyStyle(tempcellpiheno.Style);
                        break;
                    case 2:
                        aktivcell.Style.ApplyStyle(tempcellunnep.Style);
                        break;

                    default:
                        aktivcell.Style.ApplyStyle(tempcellegyeb.Style);
                        break;
                }
            }
            if (Tabinfo.Changed)
                Parameterez.tabControl1.Controls.Remove(parameterpage);
        }
        /// <summary>
        /// 
        /// </summary>
        public override void Rogzit()
        {
            Tabinfo.DataView.Table = Tabinfo.Adattabla;
            base.Rogzit();
            elolrolalap_Click(new object(), new EventArgs());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void rogzit_Click(object sender, EventArgs e)
        {
            FakUserInterface.EventTilt = true;
            base.rogzit_Click(sender, e);
            FakUserInterface.EventTilt = false;
            elolrolalap_Click(new object(), new EventArgs());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void elolrolalap_Click(object sender, EventArgs e)
        {
            base.elolrolalap_Click(sender, e);
            if (Parameterez.tabControl1.Controls.IndexOf(parameterpage) == -1)
            {
                Parameterez.tabControl1.Controls.Clear();
                Parameterez.tabControl1.Controls.Add(parameterpage);
                Parameterez.tabControl1.Controls.Add(Parameterez.ListaAdatbevPage);
                Parameterez.tabControl1.SelectedIndex = 1;
                FakUserInterface.EventTilt = false;
            }
            Tempcellini1();
            Tempcellini1(Tabinfo.ViewSorindex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            base.dataGridView2_CellClick(sender, e);
            if (aktivcol != null && !aktivcol.Comboe)
            {
                DataGridViewCell cell = dataGridView2.Rows[Tabinfo.InputColumns.IndexOf(aktivcol.ColumnName)].Cells[1];
                cell.Selected = false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void dataGridView2_EndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (aktivcol != null && aktivcell != null)
            {
                if (aktivcol.ColumnName == "EV" || aktivcol.ColumnName == "HONAP")
                    aktivcell.Value = aktivcol.Tartalom;
                else
                    base.dataGridView2_EndEdit(sender, e);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public override void Inputtablaba()
        {
            if (dataGridView2.Columns[0].Width < 90)
                dataGridView2.Columns[0].Width = 90;
            Inputtabla.Columns[1].ReadOnly = false;
            dataGridView2.ReadOnly = false;
            dataGridView2.Rows[0].Cells[1].ReadOnly = false;
            dataGridView2.Rows[1].Cells[1].ReadOnly = false;
            if (Tabinfo.Adattabla.Rows.Count == 0)
                return;
            if (Tabinfo.ViewSorindex == -1)
                Tabinfo.ViewSorindex = 0;
            base.Inputtablaba();
            string ev = Tabinfo.AktualViewRow["EV"].ToString();
            string honap = Tabinfo.AktualViewRow["HONAP"].ToString();
            if (honap.Length == 1)
                honap = "0" + honap;
            string tims = ev.ToString() + "." + honap + "." + "01";
            DateTime tim = Convert.ToDateTime(tims).AddMonths(1).AddDays(-1);
            int maxsor = tim.Day + 2;
//            string nap;
            for (int i = 0; i < Inputtabla.Rows.Count; i++)
            {
                if (i < maxsor)
                    dataGridView2.Rows[i].Visible = true;
                else
                    dataGridView2.Rows[i].Visible = false;
            }
            if(this.Visible)
                Tempcellini2();
            if (SajatHozferJog != HozferJogosultsag.Irolvas)
            {
                Inputtabla.Columns[1].ReadOnly = true;
                dataGridView2.ReadOnly = true;
            }
            else
            {
                dataGridView2.Rows[0].Cells[1].ReadOnly = true;
                dataGridView2.Rows[1].Cells[1].ReadOnly = true;
            }
        }
        public virtual void Husvet(int ev)
        {
            DateTime marc1 = Convert.ToDateTime(ev.ToString()+".03.01");
            int a4 = ev - Convert.ToInt32(ev / 19) * 19;
            int b4 = ev - Convert.ToInt32(ev / 4) * 4;
            int c4 = ev - Convert.ToInt32(ev / 7) * 7;
            int d0 = 19 * a4 + 24;
            int d4 = d0 - Convert.ToInt32(d0 / 30) * 30;
            int e0 = 2 * b4 + 4 * c4 + 6 * d4 + 5;
            int e4 = e0 - Convert.ToInt32(e0 / 7) * 7;
            int h = 0;
            if (e4 == 6)
            {
                if (d4 == 29)
                    h = 50;
                else if (d4 == 28 && a4 > 10)
                    h = 49;
                else
                    h = 22 + d4 + e4;
            }
            else
                h = 22 + d4 + e4;
            DateTime husvethetfo = marc1.AddDays(h);
            DateTime husvetvas = husvethetfo.AddDays(-1);
            DateTime punkosdhetfo = husvethetfo.AddDays(49);
            DateTime punkosdvas = punkosdhetfo.AddDays(-1);
            husvetvasho = husvetvas.Month;
            husvetvasnap = husvetvas.Day;
            husvethetfoho = husvethetfo.Month;
            husvethetfonap = husvethetfo.Day;
            punkosdvasho = punkosdvas.Month;
            punkosdvasnap = punkosdvas.Day;
            punkosdhetfoho = punkosdhetfo.Month;
            punkosdhetfonap = punkosdhetfo.Day;
        }
        private DataRow Unnepek(string tims, DataRow row, int maxnap)
        {
            DateTime tim = Convert.ToDateTime(tims);
            int ev = tim.Year;
            int ho = tim.Month;
            string evs = ev.ToString();
            string hostr;
            string napstr;
            string unnep = "Ü";
            string munkanap = munkanapkezdobetu;
            string pihenonap = "P";
            string[] napnevek = new string[31];
            for (int i = 0; i < napnevek.Length; i++)
            {
                string colnev = "N";
                hostr = ho.ToString();
                if (hostr.Length == 1)
                    hostr = "0" + hostr;
                napstr = (i + 1).ToString();
                if (napstr.Length == 1)
                    napstr = "0" + napstr;
                napnevek[i] = colnev + napstr;
                if (i < maxnap)
                {
                    string aktdatstr = evs + "." + hostr + "." + napstr;
                    DateTime aktdat = Convert.ToDateTime(aktdatstr);
                    string napnev = aktdat.DayOfWeek.ToString();
                    if (napnev.StartsWith("S"))
                        row[napnevek[i]] = pihenonap;
                    else
                        row[napnevek[i]] = munkanap;
                }
                else
                    row[napnevek[i]] = "";

            }
            if (ho == 1)
            {
                if (row[napnevek[0]].ToString() != pihenonap || munkanap != "M")
                    row[napnevek[0]] = unnep;
            }
            if (ho == 3)
            {
                if (row[napnevek[14]].ToString() != pihenonap || munkanap != "M")
                    row[napnevek[14]] = unnep;
            }
            if (ho == 5)
            {
                if (row[napnevek[0]].ToString() != pihenonap || munkanap != "M")
                    row[napnevek[0]] = unnep;
            }
            if (ho == 8)
            {
                if (row[napnevek[19]].ToString() != pihenonap || munkanap != "M")
                    row[napnevek[19]] = unnep;
            }
            if (ho == 10)
            {
                if (row[napnevek[22]].ToString() != pihenonap || munkanap != "M")
                    row[napnevek[22]] = unnep;
            }
            if (ho == 11)
            {
                if (row[napnevek[0]].ToString() != pihenonap)
                    row[napnevek[0]] = unnep;
            }
            if (ho == 12)
            {
                if (row[napnevek[24]].ToString() != pihenonap || munkanap != "M")
                    row[napnevek[24]] = unnep;
                if (row[napnevek[25]].ToString() != pihenonap || munkanap != "M")
                    row[napnevek[25]] = unnep;
            }
            if (ho == husvetvasho)
                row[napnevek[husvetvasnap - 1]] = unnep;
            if (ho == husvethetfoho)
                row[napnevek[husvethetfonap - 1]] = unnep;
            if (ho == punkosdvasho)
                row[napnevek[punkosdvasnap - 1]] = unnep;
            if (ho == punkosdhetfoho)
                row[napnevek[punkosdhetfonap - 1]] = unnep;
            if (szurtnapfajtainfo.DataView.Count > 3 && (ho < tanevkezdho && ho >= elszkezdho || ho >= tanevkezdho && ho < elszkezdho))
            {
                string egyebnapnev = szurtnapfajtainfo.DataView[szurtnapfajtainfo.DataView.Count - 1].Row["KOD"].ToString();
                for (int i = 0; i < maxnap; i++)
                {
                    string napbetu = row[napnevek[i]].ToString();
                    if (napbetu == munkanap)
                        row[napnevek[i]] = egyebnapnev;
                }
            }
            return row;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public new void Visible_Changed(object sender, EventArgs e)
        {
            if (!FakUserInterface.EventTilt)
            {
                if (this.Visible && Tabinfo != null)
                {
                    Tempcellini1();
                    if (Tabinfo.ViewSorindex != -1)
                    {
                        Tempcellini1(Tabinfo.ViewSorindex);
                        Inputtablaba();
                    }
                }
            }
        }
    }
}
