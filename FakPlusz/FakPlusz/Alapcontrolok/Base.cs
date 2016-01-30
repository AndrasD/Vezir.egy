using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FakPlusz.Alapfunkciok;
using FakPlusz.Formok;
using FakPlusz.UserAlapcontrolok;
namespace FakPlusz.Alapcontrolok
{
    /// <summary>
    /// Az osszes tobbi UserControl alapja, azaz ettol orokolnek, akar a Tervezben, akar felhasznaloi rendszerekben
    /// </summary>
    public partial class Base : UserControl
    {
        /// <summary>
        /// TreeView panelje
        /// </summary>
        public Panel TreePanel = null;
        /// <summary>
        /// A UserControl TabControlja
        /// </summary>
        public TabControl TabControl = null;
        /// <summary>
        /// A TabPagek
        /// </summary>
        public TabPage[] TabPagek = null;
        /// <summary>
        /// A TabControl panelje vagy null
        /// </summary>
        public Panel MenuPanel = null;
        /// <summary>
        /// Vezerlocontroloknal erdekes, ha az inditando UserControl mar letezik, true
        /// </summary>
        public bool Aktiv = true;
        /// <summary>
        /// A UserControl-t elso esetben hivtuk, true
        /// </summary>
        public bool Elsoeset = true;
        /// <summary>
        /// A UserControl neve
        /// </summary>
        public virtual string ControlNev
        {
            get { return this.Name; }
            set
            {
                this.Name = value;
            }
        }
        ///// <summary>
        ///// true, ha elhagyta a UserControlt
        ///// </summary>
        //public bool Leaved = true;
        /// <summary>
        /// true azt jelenti, hogy a UserControl inicializalasa uj TablainfoTag szamara tortenik (AltalanosInit)
        /// </summary>
        public bool UjTag = true;
        /// <summary>
        /// true: hibas
        /// </summary>
        public bool Hibas = false;
        /// <summary>
        /// modositott
        /// </summary>
        public bool Modositott = false;
        /// <summary>
        /// true: adatbeviteknel beszurhat a tablasorok koze
        /// </summary>
        public bool Beszurhat = true;
        /// <summary>
        /// true: adatbevitelnel modosithat a sorok tartalman
        /// </summary>
        public bool Modosithat = true;
        /// <summary>
        /// true: adatbevitelnel torolheto-e egy sor
        /// </summary>

        public bool Torolhet = true;
        /// <summary>
        /// true: Leirotabla mezoinformacioi
        /// </summary>
        public bool Leiroe = false;
        //        public bool Focused = false;
        /// <summary>
        /// Ebben a TabPage-ben fut
        /// </summary>
        /// 
        public TabPage AktivPage = null;
        /// <summary>
        /// Vezerlo UserControl eseten pagevaltaskor az eredeti aktiv page
        /// </summary>
        public TabPage OldPage = null;
        /// <summary>
        /// Vezerlo UserControl eseten az inditott (vagy inditando) osszes lehetseges aktiv page
        /// </summary>
        public TabPage[] AktivPagek = null;
        /// <summary>
        /// A Fak objectum
        /// </summary>
        private FakUserInterface fakuserinterface;
        /// <summary>
        /// A FakUserInterface objectum, allithato
        /// </summary>
        public FakUserInterface FakUserInterface
        {
            get { return fakuserinterface; }
            set { fakuserinterface = value; }
        }
        private int cegindex = -1;
        /// <summary>
        /// az aktualis ceg indexe
        /// </summary>
        public int Cegindex
        {
            get { return cegindex; }
            set { cegindex = value; }
        }
        private bool lezartceg = false;
        /// <summary>
        /// true - lezart a ceg
        /// </summary>
        public bool LezartCeg
        {
            get { return lezartceg; }
            set { lezartceg = value; }
        }
        public bool Tervezoe
        {
            get { return fakuserinterface.Alkalmazas == "TERVEZO"; }
            
        }
        /// <summary>
        /// Formvezerles UserControl allitja
        /// true: ha adat- es leiroadatkarbantartas is van
        /// </summary>
        public bool FormvezerleshezKetPageKell = true;
        /// <summary>
        /// Osszesito tablainformacio
        /// </summary>
        public TablainfoTag TablainfoTag = null;
        /// <summary>
        /// a tablainformacio
        /// </summary>
        public Tablainfo Tabinfo = null;
        /// <summary>
        /// A Leirotabla informaciok
        /// </summary>
        public Tablainfo LeiroTabinfo = null;
        /// <summary>
        /// az aktiv menuitem vagy null
        /// </summary>
        public ToolStripMenuItem AktivMenuItem = null;
        /// <summary>
        /// menuitemek
        /// </summary>
        public ToolStripMenuItem[] MenuItemek;
        /// <summary>
        /// Aktiv menuindex
        /// </summary>
        public int AktivMenuindex = -1;
        /// <summary>
        /// aktiv dropitem vagy null
        /// </summary>
        public ToolStripMenuItem AktivDropDownItem = null;
        /// <summary>
        /// dropitemek
        /// </summary>
        public ToolStripMenuItem[] DropItemek;
        /// <summary>
        /// aktiv dropitem indexe
        /// </summary>
        public int AktivDropindex = -1;
        /// <summary>
        /// Ha nincs menustrip, null
        /// </summary>
        public MenuStrip MenuStrip = null;
        /// <summary>
        /// AktivPage indexe
        /// </summary>
        public int AktivPageIndex = -1;
        /// <summary>
        /// Hivo Control
        /// </summary>
        public Base Hivo;
        /// <summary>
        /// Aktiv Control
        /// </summary>
        public Base AktivControl = null;
        public Control FirstFocusControl = null;
        /// <summary>
        /// Aktualis Node
        /// </summary>
        public TreeNode AktualNode = null;
        /// <summary>
        /// elso node a faban
        /// </summary>
        public TreeNode FirstNode = null;
        /// <summary>
        /// utolso node a faban
        /// </summary>
        public TreeNode LastNode = null;
        /// <summary>
        /// a fa
        /// </summary>
        public TreeView TreeView = null;
        /// <summary>
        /// kell nyilvantartasi kod?
        /// </summary>
        public bool Kellnyilvkod = false;
        /// <summary>
        /// a megelozo aktualis node
        /// </summary>
        public TreeNode PrevNode = null;
        /// <summary>
        /// A UserControl vezerloinformacioja
        /// </summary>
        public Vezerloinfo Vezerles = null;
        /// <summary>
        /// Vezerlo UserControl eseten az inditott (AktivControl) UserControl vezerloinformacioja
        /// </summary>
        public Vezerloinfo AktivVezerles = null;
        /// <summary>
        /// A UserControl informacio
        /// </summary>
        public UserControlInfo UserControlInfo = null;
        // Innen kezdve a parameterezheto UserControlok alapadatai
        /// <summary>
        /// Parameterezes lehetosegei
        /// </summary>
        public enum Parameterezes
        {
            /// <summary>
            /// nincs parameter
            /// </summary>
            Nincsparameterezes,
            /// <summary>
            /// Csak datum
            /// </summary>
            Datum,
            /// <summary>
            /// Datum plusz valamilyen kodtablabol valasztas
            /// </summary>
            Datumpluszvalasztek,
            /// <summary>
            /// Valtoztathato listaadatok
            /// </summary>
            Listaparamok,
            /// <summary>
            /// Valtoztathato listaadatok plusz datum
            /// </summary>
            ListaparamokpluszDatum,
            /// <summary>
            /// A listazando/beviteli termeszetes alaptabla soraibol szurjuk a szuksegeset
            /// </summary>
            Egyszeruszures,
            /// <summary>
            /// Egyszeru szures plusz datum
            /// </summary>
            EgyszeruszurespluszDatum,
            /// <summary>
            /// Kodtablat valasztunk, melynek ertekeibol kivalasztjuk a szuksegeseket
            /// Ezzel szurjuk a termeszetes alaptablat
            /// </summary>
            Osszetettszures,
            /// <summary>
            /// Osszetett szures plusz Datum
            /// </summary>
            OsszetettszurespluszDatum
        };
        private Parameterezes paramfajta = Parameterezes.Nincsparameterezes;
        /// <summary>
        /// A UserControl Parameterezes enum-ja. Set a string formajat is allitja
        /// </summary>
        /// <summary>
        /// annyi eleme lesz, ahany inputsora van az inputtablanak
        /// a hibavizsgalatok allitjak illetve torlik a szovegeket
        /// </summary>
        public string[] hibaszov;
        /// <summary>
        /// annyi eleme lesz, ahany inputsora van az inputtablanak
        /// elemenkent, ha true: az adott inputsor tartalma valtozott
        /// </summary>
        public bool[] valtozott;
        public DataView dataView1;
        public DataView dataView2;
        public Parameterezes Paramfajta
        {
            get { return paramfajta; }
            set
            {
                paramfajta = value;
                paramfajtastring = paramfajta.ToString();
            }
        }
        private string paramfajtastring = Parameterezes.Nincsparameterezes.ToString();
        /// <summary>
        /// A UserControl Parameterezes string formaja
        /// </summary>
        public string ParamfajtaString
        {
            get { return paramfajtastring; }
        }
        /// <summary>
        /// A UserControl Parameterez UserControl-ja, ha van
        /// </summary>
        public Parameterez Parameterez = null;
        /// <summary>
        /// Adatkozlesek infoi
        /// </summary>
        public Adatkivitel[] Adatkivitelek = null;
        /// <summary>
        /// A UserControl Adatkivitel UserControlja, ha van
        /// </summary>
        public Adatkivitel AdatkivitelControl = null;
        /// <summary>
        /// Listazashoz filter
        /// </summary>
        public string ListaFilter = "";
        private string[] beallitandodatumnevek = new string[] { "" };
        /// <summary>
        /// Milyen nevu specialis datumokat kell beallitani
        /// </summary>
        public string[] BeallitandoDatumNevek
        {
            get
            {
                if (beallitandodatumnevek == null)
                {
                    UserContSpecDatumNevek = FakUserInterface.GetOsszef("R", "UserContSpecDatum").Osszefinfo;
                    Tablainfo specdatumnevek = UserContSpecDatumNevek.tabinfo2;
                    Tablainfo usernevek = FakUserInterface.GetBySzintPluszTablanev("R", "USERCONTROLNEVEK");
                    string usernev = this.Name;
                    if (this.Name == "Parameterez")
                        usernev = this.AktivControl.Name;
                    string id = FakUserInterface.GetTartal(usernevek, "ID", "SZOVEG", usernev)[0];
                    string[] beallitandodatumnevidk = FakUserInterface.GetSzurtOsszefIdk(UserContSpecDatumNevek, new object[] { id, "" });
                    if(beallitandodatumnevidk!=null)
                        beallitandodatumnevek = FakUserInterface.GetTartal(specdatumnevek, "SZOVEG", "SORSZAM", beallitandodatumnevidk);
                }
                return beallitandodatumnevek;
            }
            set
            {
                beallitandodatumnevek = value;
            }
        }
        private DateTime datumtol = DateTimePicker.MinimumDateTime;
        private string datumtolstring;
        /// <summary>
        /// Datumtol string formaban
        /// </summary>
        public string DatumtolString
        {
            get { return datumtolstring; }
        }
        /// <summary>
        /// -tol
        /// </summary>
        public DateTime Datumtol
        {
            get { return datumtol; }
            set
            {
                if (datumtol != value)
                    DatumValtozas = true;
                datumtol = value;
                if (TeljesEv)
                    datumtol = FakUserInterface.EvDatum(datumtol);
                else if (TeljesHonap)
                    datumtol = FakUserInterface.HoDatum(datumtol);
                datumtolstring = FakUserInterface.DatumToString(datumtol);
            }
        }
        private DateTime datumig = DateTimePicker.MaximumDateTime;
        private string datumigstring;
        /// <summary>
        /// Datumig string formaban
        /// </summary>
        public string DatumigString
        {
            get { return datumigstring; }
        }
        /// <summary>
        /// -ig
        /// </summary>
        public DateTime Datumig
        {
            get { return datumig; }
            set
            {
                if (datumig != value)
                    DatumValtozas = true;
                datumig = value;
                if (TeljesEv)
                    datumig = FakUserInterface.EvDatum(datumig);
                else if (TeljesHonap)
                    datumig = FakUserInterface.HoDatum(datumig);
                datumigstring = FakUserInterface.DatumToString(datumig);
            }
        }
        /// <summary>
        /// Datummegadas teljes honap
        /// </summary>
        public bool TeljesHonap = false;
        /// <summary>
        /// Datummegadas egy honap
        /// </summary>
        public bool CsakEgyHonap = false;
        /// <summary>
        /// Datummegadas teljes ev
        /// </summary>
        public bool TeljesEv = false;
        private DateTime tolmindatum = DateTimePicker.MinimumDateTime;
        private string tolmindatumstring;
        /// <summary>
        /// -tol datum minimalis erteke string alakban
        /// </summary>
        public string TolMinDatumString
        {
            get { return tolmindatumstring; }
        }
        /// <summary>
        /// -tol datum minimalis erteke. A Set atalakitja
        /// </summary>
        public DateTime TolMinDatum
        {
            get { return tolmindatum; }
            set
            {
                tolmindatum = value;
                tolmindatumstring = FakUserInterface.DatumToString(tolmindatum);
                if (TeljesEv)
                {
                    tolmindatumstring = tolmindatum.Year.ToString() + ".01.01";
                    tolmindatum = Convert.ToDateTime(tolmindatumstring);
                }
                else if (TeljesHonap)
                {
                    tolmindatumstring = tolmindatumstring.Substring(0, 8) + "01";
                    tolmindatum = Convert.ToDateTime(tolmindatumstring);
                }
                if (CsakEgyHonap)
                {
                    tolmaxdatum = tolmindatum;
                    tolmaxdatumstring = tolmindatumstring;
                }
            }
        }
        private DateTime tolalapert = DateTime.Today;
        private string tolalapertstring;
        /// <summary>
        /// -tol alapertelmezes string alakban
        /// </summary>
        public string TolAlapertString
        {
            get { return tolalapertstring; }
        }
        /// <summary>
        /// -tol alapertelmezes . A Set atalakitja
        /// </summary>
        public DateTime TolAlapert
        {
            get { return tolalapert; }
            set
            {
                tolalapert = value;
                tolalapertstring = FakUserInterface.DatumToString(tolalapert);
                if (TeljesEv)
                {
                    tolalapertstring = tolalapert.Year.ToString() + ".01.01";
                    tolalapert = Convert.ToDateTime(tolalapertstring);
                }
                else if (TeljesHonap)
                {
                    tolalapertstring = tolalapertstring.Substring(0, 8) + "01";
                    tolalapert = Convert.ToDateTime(tolalapertstring);
                }
                if (CsakEgyHonap)
                {
                    igalapert = tolalapert;
                    igalapertstring = tolalapertstring;
                }
            }
        }
        private DateTime tolmaxdatum = DateTimePicker.MaximumDateTime;
        private string tolmaxdatumstring;
        /// <summary>
        /// -tol max erteke string alakban
        /// </summary>
        public string TolMaxDatumString
        {
            get { return tolmaxdatumstring; }
        }
        /// <summary>
        /// -tol maximalis erteke . A set atalakitja
        /// </summary>
        public DateTime TolMaxDatum
        {
            get { return tolmaxdatum; }
            set
            {
                tolmaxdatum = value;
                tolmaxdatumstring = FakUserInterface.DatumToString(tolmaxdatum);
                if (TeljesEv)
                {
                    tolmaxdatumstring = tolmaxdatum.Year.ToString() + ".01.01";
                    tolmaxdatum = Convert.ToDateTime(tolmaxdatumstring);
                }
                else if (TeljesHonap)
                {
                    tolmaxdatumstring = tolmaxdatumstring.Substring(0, 8) + "01";
                    tolmaxdatum = Convert.ToDateTime(tolmaxdatumstring);
                }
            }
        }
        private DateTime igmindatum = DateTimePicker.MinimumDateTime;
        private string igmindatumstring;
        /// <summary>
        /// -ig minimalis erteke string alakban
        /// </summary>
        public string IgMinDatumString
        {
            get { return igmindatumstring; }
        }
        /// <summary>
        /// -ig minimalis erteke. A set atalakitja
        /// </summary>
        public DateTime IgMinDatum
        {
            get { return igmindatum; }
            set
            {
                igmindatum = value;
                igmindatumstring = FakUserInterface.DatumToString(igmindatum);
                if (TeljesEv)
                {
                    igmindatumstring = igmindatum.Year.ToString() + ".01.01";
                    igmindatum = Convert.ToDateTime(igmindatumstring);
                }
                else if (TeljesHonap)
                {
                    igmindatumstring = igmindatumstring.Substring(0, 8) + "01";
                    igmindatum = Convert.ToDateTime(igmindatumstring);
                }
            }
        }
        private DateTime igalapert = DateTime.Today;
        private string igalapertstring;
        /// <summary>
        /// -ig alapertelmezes string alakban
        /// </summary>
        public string IgAlapertString
        {
            get { return igalapertstring; }
        }
        /// <summary>
        /// -ig alapertelmezese. A set atalakitja
        /// </summary>
        public DateTime IgAlapert
        {
            get { return igalapert; }
            set
            {
                igalapert = value;
                igalapertstring = FakUserInterface.DatumToString(igalapert);
                if (TeljesEv)
                {
                    igalapertstring = igalapert.Year.ToString() + ".01.01";
                    igalapert = Convert.ToDateTime(igalapertstring);
                }
                else if (TeljesHonap)
                {
                    igalapertstring = igalapertstring.Substring(0, 8) + "01";
                    igalapert = Convert.ToDateTime(igalapertstring);
                }
            }
        }
        private DateTime igmaxdatum = DateTimePicker.MaximumDateTime;
        private string igmaxdatumstring;
        /// <summary>
        /// -ig maximalis erteke string alakban
        /// </summary>
        public string IgMaxDatumString
        {
            get { return igmaxdatumstring; }
        }
        /// <summary>
        /// -ig maximalis erteke . A set atalakitja
        /// </summary>
        public DateTime IgMaxDatum
        {
            get { return igmaxdatum; }
            set
            {
                igmaxdatum = value;
                igmaxdatumstring = FakUserInterface.DatumToString(igmaxdatum);
                if (TeljesEv)
                {
                    igmaxdatumstring = igmaxdatum.Year.ToString() + ".01.01";
                    igmaxdatum = Convert.ToDateTime(igmaxdatumstring);
                }
                else if (TeljesHonap)
                {
                    igmaxdatumstring = igmaxdatumstring.Substring(0, 8) + "01";
                    igmaxdatum = Convert.ToDateTime(igmaxdatumstring);
                }
            }
        }
        /// <summary>
        /// Parameterezeshez kell
        /// </summary>
        public DataTable AlapTabla;
        /// <summary>
        /// Parameterezeshez kell
        /// </summary>
        public DataView AlapTablaView = new DataView();
        /// <summary>
        /// Parameterezeshez kell
        /// </summary>
        public string AlapTablaSelectString = "";
        /// <summary>
        /// Parameterezeshez kell
        /// </summary>
        public string AlapTablaNev;
        /// <summary>
        /// Parameterezeshez kell
        /// </summary>
        public string AlapIdNev;
        /// <summary>
        /// Parameterezeshez kell
        /// </summary>
        public string AlapMegnevezesColumn = "";
        /// <summary>
        /// Parameterezeshez kell
        /// </summary>
        public ArrayList AlapIdk;

        /// <summary>
        /// kodtablavalasztek combo-ja, ha kell valasztek
        /// </summary>
        public ComboBox Valasztek = null;
        /// <summary>
        /// Parameterezeshez kell
        /// </summary>
        public string ValasztekIdNev = "";
        /// <summary>
        /// Combo-itemek
        /// </summary>
        public string[] ValasztekItemek = null;
        /// <summary>
        /// Parameterezeshez kell
        /// </summary>
        public string[] ValasztekIdk = null;
        private int valasztekindex = 0;
        /// <summary>
        /// A valasztott kodertek indexe . Valtozas eseten ValasztekValtozas allitasa
        /// </summary>
        public int ValasztekIndex
        {
            get { return valasztekindex; }
            set
            {
                if (valasztekindex != value)
                {
                    ValasztekValtozas = true;
                    valasztekindex = value;
                }
            }
        }
        /// <summary>
        /// A valasztek tablainformacioja
        /// </summary>
        public Tablainfo ValasztekInfo = null;
        /// <summary>
        /// A valasztek adattablaja
        /// </summary>
        public DataTable ValasztekTabla = null;
        /// <summary>
        /// A valasztektabla neve
        /// </summary>
        public string ValasztekTablaNev = "";

        /// <summary>
        /// Parameterek kozott van-e datum
        /// </summary>
        public bool VanDatum = false;
        /// <summary>
        /// Parameterek kozott van-e valasztek
        /// </summary>
        public bool VanValasztek = false;
        /// <summary>
        /// Parameterek kozott van-e egyszeru parameterezes
        /// </summary>
        public bool VanEgyszeru = false;
        /// <summary>
        /// Parameterek kozott van-e osszetett parameterezes
        /// </summary>
        public bool VanOsszetett = false;
        /// <summary>
        /// Parameterek kozott van-e lista parameterezes
        /// </summary>
        public bool VanLista = false;
        /// <summary>
        /// Volt-e valamilyen parametervaltozas
        /// </summary>
        public bool Valtozas = false;
        /// <summary>
        /// Volt-e datumvaltozas
        /// </summary>
        public bool DatumValtozas = false;
        /// <summary>
        /// Volt-e valasztekvaltozas
        /// </summary>
        public bool ValasztekValtozas = false;
        /// <summary>
        /// Volt-e egyszeru parametervaltozas
        /// </summary>
        public bool EgyszeruParamValtozas = false;
        /// <summary>
        /// Volt-e osszetett parametervaltozas
        /// </summary>
        public bool OsszetettParamValtozas = false;
        /// <summary>
        /// Volt-e lista parametervaltozas
        /// </summary>
        public bool ListaParamValtozas = false;
        /// <summary>
        /// Parameterezeshez kell
        /// </summary>
        public DataTable[] SzuroTablak = null;
        /// <summary>
        /// Parameterezeshez kell
        /// </summary>
        public DataView[] SzuroTablaViewk = null;
        /// <summary>
        /// Parameterezeshez kell
        /// </summary>
        public string[] SzuroTablaIdNevek = null;
        /// <summary>
        /// A szures tablaja, ha van szures
        /// </summary>
        public DataTable SzuroTabla = null;
        /// <summary>
        /// Szurotabla view-ja
        /// </summary>
        public DataView SzuroTablaView = null;
        /// <summary>
        /// A szurotabla id-neve
        /// </summary>
        public string SzuroTablaIdNev = "";
        /// <summary>
        /// a termeszetes alaptabla szurt id-i
        /// </summary>
        public ArrayList SzurtIdk;
        /// <summary>
        /// szurotabla neve
        /// </summary>
        public string SzuroTablaNev = "";
        /// <summary>
        /// Parameterezeshez kell
        /// </summary>
        public DataTable EgyszeruTabla = null;
        /// <summary>
        /// Parameterezeshez kell
        /// </summary>
        public DataView EgyszeruTablaView = new DataView();
        /// <summary>
        /// Parameterezeshez kell
        /// </summary>
        public string EgyszeruMegnevColumnNev = "";
        /// <summary>
        /// Parameterezeshez kell
        /// </summary>
        public string EgyszeruIdNev = "";
        /// <summary>
        /// Parameterezeshez kell
        /// </summary>
        public string[] EgyszeruIdk = null;
        /// <summary>
        /// Parameterezeshez kell
        /// </summary>
        public bool[] EgyszeruOrigertekek = null;
        /// <summary>
        /// Lista?
        /// </summary>
        public bool Listae = false;
        /// <summary>
        /// Adatszolgaltatas?
        /// </summary>
        public bool Adatszolge = false;
        /// <summary>
        /// LISTAK tablainfo
        /// </summary>
        public Tablainfo ListaInfo = null;
        /// <summary>
        /// UserControl id-je (a USERCONTROLNEVEK tabla adott soranak id-je)
        /// </summary>
        public string UserContId = "";
        /// <summary>
        /// UserControl/Listak osszefugges
        /// </summary>
        public Osszefinfo UserContListakep = null;
        /// <summary>
        /// Parameterezeshez kell
        /// </summary>
        public Osszefinfo UserContSpecDatumNevek = null;
        /// <summary>
        /// Parameterezeshez kell
        /// </summary>
        public string[] ListaParamnevek = null;
        /// <summary>
        /// Parameterezeshez kell
        /// </summary>
        public string[] ListaParamok = null;
        /// <summary>
        /// Parameterezeshez kell
        /// </summary>
        public DataView[] RadiobuttonViewk = null;
        /// <summary>
        /// Parameterezeshez kell
        /// </summary>
        public bool[] RadiobuttonAllapotok = null;
        /// <summary>
        /// Parameterezeshez kell
        /// </summary>
        public RadioButton[] Radiobuttonok = null;
        /// <summary>
        /// Parameterezeshez kell
        /// </summary>
        public string[] RadiobuttonIdNevek = null;
        /// <summary>
        /// Parameterezeshez kell
        /// </summary>
        public string[] RadiobuttonMegnevColumnNevek;
        /// <summary>
        /// Parameterezeshez kell
        /// </summary>
        public int AktualisRadiobuttonIndex;
        /// <summary>
        /// Parameterezeshez kell
        /// </summary>
        public DataTable OsszetettKozepsoTabla = null;
        /// <summary>
        /// Parameterezeshez kell
        /// </summary>
        public DataView OsszetettKozepsoTablaView = new DataView();
        /// <summary>
        /// Parameterezeshez kell
        /// </summary>
        public string OsszetettKozepsoMegnevColumnNev = "";
        /// <summary>
        /// Parameterezeshez kell
        /// </summary>
        public string OsszetettKozepsoNev = "";
        /// <summary>
        /// Parameterezeshez kell
        /// </summary>
        public string OsszetettRadioButtonNev = "";
        /// <summary>
        /// Parameterezeshez kell
        /// </summary>
        public string OsszetettKozepsoIdNev = "";
        /// <summary>
        /// Parameterezeshez kell
        /// </summary>
        public string[] OsszetettKozepsoIdk = null;
        /// <summary>
        /// Parameterezeshez kell
        /// </summary>
        public bool[] OsszetettOrigertekek = null;
        /// <summary>
        /// Parameterezeshez kell
        /// </summary>
        public string OsszetettAlsoIdNev = "";
        /// <summary>
        /// Parameterezeshez kell
        /// </summary>
        public DataTable OsszetettAlsoTabla = null;
        /// <summary>
        /// Parameterezeshez kell
        /// </summary>
        public DataView OsszetettAlsoTablaView = new DataView();
        /// <summary>
        /// Parameterezeshez kell
        /// </summary>
        public string OsszetettAlsoMegnevColumnNev = "";
        /// <summary>
        /// Parameterezeshez kell
        /// </summary>
        public string[] OsszetettAlsoIdk = null;
        /// <summary>
        /// Mai datum
        /// </summary>
        public string MaiDatumString = DateTime.Today.ToShortDateString();
        /// <summary>
        /// Parameterezeshez kell
        /// </summary>
        public string ListaNev = "";
        /// <summary>
        /// Adatszolg-hoz
        /// </summary>
        public bool AdatszolgKozosFileba = true;
        /// <summary>
        /// Adatszolg-hoz
        /// </summary>
        public bool AdatszolgKellNapisorszam = false;
        /// <summary>
        /// Adatszolg-hoz
        /// </summary>
        public int Napisorszam = 0;
        /// <summary>
        /// vezerlest Tabstop allitason keresztul kapta?
        /// </summary>
        public bool tabstopboljott = true;

        /// <summary>
        /// Adatszolg neve/Specialis fix ertekek
        /// </summary>
        public Osszefinfo AdatszolgSpecfix = null;
        /// <summary>
        /// USERADATSZOLG tablainfo - Adatszolgaltatasok neveinek felsorolasa (az osszefuggo adatszolgaltatasoknal
        /// a gyerek adatszolgnev kitoltve
        /// </summary>
        public Tablainfo UserAdatSzolgInfo = null;
        /// <summary>
        /// SPECADATSZOLGNEVEK tablainfo - ezek hasznalhatoak az adatszolgaltatas leirasanal specialis fix ertekkent
        /// </summary>
        public Tablainfo SpecAdatSzolgnevInfo = null;
        /// <summary>
        /// specialis fixertek nevek tombje
        /// </summary>
        public ArrayList SpecFixertekNevek = new ArrayList();
        /// <summary>
        /// egy UserControl altal hasznalt adatszolgaltatas(ok) neve(i)
        /// </summary>
        public string[] AdatszolgaltatasNevek = null;
        /// <summary>
        /// Az adatszolgaltatas nevekhez tartozo tablainfok
        /// </summary>
        public TablainfoCollection AdatszolgaltatasInfok = null;
        private string adatszolgaltatasfoldernev = "";
        /// <summary>
        /// Az adatszolgaltatas folder(ek) neve
        /// </summary>
        public string[] AdatSzolgaltatasFolderTree = null;
        /// <summary>
        /// a teljes foldernev
        /// </summary>
        public string AdatszolgaltatasFoldernev
        {
            get { return adatszolgaltatasfoldernev; }
            set
            {
                adatszolgaltatasfoldernev = value;
                if (value != "")
                {
                    if (value.Contains("/"))
                    {
                        char[] vesszo = new char[] { Convert.ToChar("/") };
                        AdatSzolgaltatasFolderTree = value.Split(vesszo);
                    }
                    else
                    {
                        AdatSzolgaltatasFolderTree = new string[1];
                        AdatSzolgaltatasFolderTree[0] = value;
                    }
                }
            }
        }
        //        public string[] AdatszolgaltatasFilenevEleje = null;
        /// <summary>
        /// 
        /// </summary>
        public string[] AdatszolgaltatasFilenevek = null;
        /// <summary>
        /// 
        /// </summary>
        public string AdatkivitelFilename = "";
        /// <summary>
        /// Lehetseges hozzaferesi jogosultsagok
        /// </summary>
        public enum HozferJogosultsag
        {
            /// <summary>
            /// Teljes hozzaferes
            /// </summary>
            Irolvas,
            /// <summary>
            /// csak olvas
            /// </summary>
            Csakolvas,
            /// <summary>
            /// nincs hozzaferes
            /// </summary>
            Semmi
        };
        /// <summary>
        /// Hozzaferesi jogosultsag a hivo szerint
        /// </summary>
        public HozferJogosultsag HozferJog;
        /// <summary>
        /// Hozzaferesi jogosultsagok string alakban
        /// </summary>
        public string[] HozferStringek = new string[] { "Irolvas", "Csakolvas", "Semmi" };
        /// <summary>
        /// Lehetseges kezeloi szintek
        /// </summary>
        public enum KezSzint
        {
            /// <summary>
            /// fejleszto
            /// </summary>
            Fejleszto,
            /// <summary>
            /// kizarolagos kezelo
            /// </summary>
            Minden,
            /// <summary>
            /// rendszergazda
            /// </summary>
            Rendszergazda,
            /// <summary>
            /// rendszergazda es kiemelt kezelo is
            /// </summary>
            Rendszergazdapluszkiemelt,
            /// <summary>
            /// rendszergazda es kezelo is
            /// </summary>
            Rendszergazdapluszkezelo,
            /// <summary>
            /// kiemelt kezelo
            /// </summary>
            Kiemeltkezelo,
            /// <summary>
            /// kiemelt kezelo es kezelo is
            /// </summary>
            Kiemeltkezelopluszkezelo,
            /// <summary>
            /// sima kezelo
            /// </summary>
            Kezelo,
            /// <summary>
            /// vezeto
            /// </summary>
            Vezeto,
            /// <summary>
            /// ceg kizarolagos kezeloje
            /// </summary>
            CegMinden,
            /// <summary>
            /// nem kezelheti
            /// </summary>
            Semmi,
            /// <summary>
            /// is-is
            /// </summary>
            Rendszergazdapluszkiemeltpluszkezelo,
            /// <summary>
            /// rendszerg+vezeto
            /// </summary>
            Rendszergazdapluszvezeto
        };
        /// <summary>
        /// Aktualis kezeloi szint
        /// </summary>
        public KezSzint KezeloiSzint;
        /// <summary>
        /// Kezeloi szintek szovegesen
        /// </summary>
        public string[] SzovegesKezeloiSzint = new string[]{"Fejlesztö","Kizárólagos kezelö","Rendszergazda","Rendszerg.+Kiemelt kezelö","Rendszerg.+Kezelö",
            "Kiemelt kezelö","Kiem.kezelö+Kezelö","Kezelö","Vezetö","Egyedüli cégkezelö","Semmi","Rendszg.+Kiemelt+Kezelö","Rendszg.+Vezetö"};
        /// <summary>
        /// Ceg + cegalatti szintu tablainformaciok gyujtemenye
        /// </summary>
        public TablainfoCollection TermCegPluszCegalattiTabinfok;
        /// <summary>
        /// A UserControl altal hasznalt tablainformaciok
        /// </summary>
        public Tablainfo[] Aktualtablainfo;
        /// <summary>
        /// A UserControl altal hasznalt tablainformaciok inputcontrolinformaciok
        /// </summary>
        public MezoControlInfo[] MezoControlInfok;
        /// <summary>
        /// Leirotablainformaciok UserControl-ja eseten a tablainformacio karbantartasanak TabPage, ha van
        /// </summary>
        public TabPage KarbantartoPage = null;
        /// <summary>
        /// objectum letrehozasa
        /// </summary>
        public Base()
        {
            InitializeComponent();
        }
        public virtual void Inputtablaba()
        {
        }
        public virtual void InputtablabaTovabb()
        {
        }
        /// <summary>
        /// felulirando
        /// </summary>
        public virtual void AltalanosInit()
        {
        }
        public virtual void VerziobuttonokAllit()
        {
        }
        public virtual void Egyedi_Validated(object sender, EventArgs e)
        {
            Control cont = (Control)sender;
            MezoTag tag = (MezoTag)cont.Tag;
            if (!FakUserInterface.EventTilt)
            {
                ControlAlap hivo;
                try
                {
                    hivo = (ControlAlap)tag.Hivo;

                    bool hiba = tag.EgyHibavizsg(tag.Control.Text);
                    if (tag.Hibaszov == "")
                    {
                        {
                            hiba = hivo.EgyediValidalas(tag);
                            FakUserInterface.ErrorProvider.SetError(cont, tag.Hibaszov);
                        }
                    }
                    else
                        hiba = true;
                    if (hiba)
                        tag.Tabinfo.ModositasiHiba = true;
                    Controltipus egycont = hivo.ControltipusCollection.Find((GroupBox)tag.ParentControl);
                    if (egycont.ButtonNevek == null || egycont.ButtonNevek.Length == 0)
                    {
                        if (!hivo.ControltipusCollection.Hibas)
                        {
                            egycont.Hivo.teljesrogzit.Visible = true;
                            egycont.Hivo.teljesrogzit.Enabled = true;
                        }
                        else
                        {
                            egycont.Hivo.teljesrogzit.Visible = false;
                            egycont.Hivo.teljesrogzit.Enabled = false;
                        }
                    }
                    if (egycont.Parent != null)
                    {
                        int i = egycont.ButtonNevekList.IndexOf("ok");
                        if (i != -1)
                        {
                            if (!tag.Tabinfo.Ures)
                            {
                                if (!egycont.Hibas)
                                {
                                    egycont.Buttonok[i].Visible = true;
                                    egycont.Buttonok[i].Enabled = true;
                                }
                                else
                                    egycont.Buttonok[i].Enabled = false;
                            }
                        }
                    }
                    hivo.ButtonokEnableAllit(egycont, true);
                }
                catch { }
            }
        }
        public virtual void Egyedi_Ok()
        {
        }
        /// <summary>
        /// elfogadas elotti egyedi validalas. Ha szukseges, felulirando
        /// </summary>
        /// <returns>
        /// false: nincs hiba
        /// </returns>
        public virtual bool VegeValidalas()
        {
            return false;
        }
        /// <summary>
        /// Rogzites elotti egyedi tevekenysegek, ha szukseges, felulirando
        /// </summary>
        public virtual void VegeTevekenysegek()
        {

        }
        /// <summary>
        /// Parameterezes hivja
        /// </summary>
        public void ValtozasokAtadasa()
        {
            AktivControl.Valtozas = Valtozas;
            AktivControl.DatumValtozas = DatumValtozas;
            AktivControl.ValasztekValtozas = ValasztekValtozas;
            AktivControl.EgyszeruParamValtozas = EgyszeruParamValtozas;
            AktivControl.OsszetettParamValtozas = OsszetettParamValtozas;
            AktivControl.ListaParamValtozas = ListaParamValtozas;
        }
        /// <summary>
        /// Parameterezes vagy annak UserControlja hivja
        /// </summary>
        public void ValtozasokTorlese()
        {
            Valtozas = false;
            DatumValtozas = false;
            ValasztekValtozas = false;
            EgyszeruParamValtozas = false;
            OsszetettParamValtozas = false;
            ListaParamValtozas = false;
        }
        /// <summary>
        /// Parameterezes
        /// </summary>
        public virtual void SzurtIdkAllitasa()
        {
        }
        /// <summary>
        /// Parameterezett UserControlnal
        /// </summary>
        public virtual void AlapTablaInit()
        {
            AlapTabla = new DataTable(AlapTablaNev);
            AlapTabla = FakUserInterface.Select(AlapTabla, FakUserInterface.AktualCegconn, AlapTablaNev, AlapTablaSelectString, "", false);
            AlapTablaView.Table = AlapTabla;
            AlapIdk = new ArrayList();
            for (int i = 0; i < AlapTabla.Rows.Count; i++)
                AlapIdk.Add(AlapTabla.Rows[i][AlapIdNev].ToString());
            if (this.Name != "Parameterez")
            {
                Parameterez.AlapTabla = AlapTabla;
                Parameterez.AlapTablaView = AlapTablaView;
                Parameterez.AlapIdNev = AlapIdNev;
                Parameterez.AlapIdk = AlapIdk;
                Parameterez.AlapTablaSelectString = AlapTablaSelectString;
                Parameterez.AlapTablaNev = AlapTablaNev;
            }
            else
            {
                AktivControl.AlapTabla = AlapTabla;
                AktivControl.AlapTablaView = AlapTablaView;
                AktivControl.AlapIdk = AlapIdk;
            }
        }
        /// <summary>
        /// Parameterezett UserControlnal
        /// </summary>
        public virtual void ValasztekParameterekInit()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramok">
        /// </param>
        /// <returns></returns>
        public virtual ArrayList EgyszeruParameterekInit(string[] paramok)
        {
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramok"></param>
        /// <param name="buttonindex"></param>
        public virtual void OsszetettParameterekInit(string[] paramok, int buttonindex)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void OsszetettKozepsoParamAlapertAllitas()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void OsszetettKozepsoParamAllitas()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void OsszetettAlsoTablaInit()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void EgyediAdatszolgaltatasInit()
        {
        }
        /// <summary>
        /// valtozott-e valamelyik tablainformacio, hasznalathoz felulirando
        /// </summary>
        /// <returns>
        /// ha nincs felulirva, false
        /// </returns>
        public virtual bool Changed()
        {
            return false;
        }
        /// <summary>
        /// Valtozas ellenorzes az Aktualtablainfo elemein
        /// Valtozas eseten hivja az Elveszithet() eljarast. Ha abbol true a visszateres, Elolrol() eljaras
        /// </summary>
        /// <returns>
        /// false - mehet tovabb
        /// true - valtozas volt es maradjon meg
        /// </returns>
        public virtual bool Userabortkerdes()
        {
            if (Aktualtablainfo != null)
            {
                for (int i = 0; i < Aktualtablainfo.Length; i++)
                {
                    if (Aktualtablainfo[i].Valtozott && Aktualtablainfo[i].Adattabla.Rows.Count!=0)
                    {
                        if (Elveszithet())
                        {
                            Elolrol();
                            return false;
                        }
                        else
                            return true;
                    }
                }
                FakUserInterface.ForceAdattolt(Aktualtablainfo);
            }
            return false;
        }
        /// <summary>
        /// Tablainformacio valtozas eseten kerdes, ha elveszitheto, ujraselectal
        /// </summary>
        /// <param name="tabinfo">
        /// a tablainformacio
        /// </param>
        /// <returns>
        /// false, ha nem valtozott vagy ujraselectalt
        /// true, ha valtozott es nem akarjuk elvesziteni
        /// </returns>
        public virtual bool Userabortkerdes(Tablainfo tabinfo)
        {
            if (!tabinfo.Valtozott)
                return false;
            if (tabinfo.Adattabla.Rows.Count == 0)
            {
                FakUserInterface.ForceAdattolt(tabinfo);
                return false;
            }
            if (Elveszithet())
            {
                Elolrol(tabinfo);
                return false;
            }
            else
                return true;
        }
        /// <summary>
        /// programozando egyedi hibavizsgalat, ha kell. Hivasa, ha a kozos hibavizsgalatok nem talaltak hibat
        /// </summary>
        /// <param name="dcell">
        /// az input cella
        /// </param>
        /// <param name="tabinfo">
        /// a tablainformacio
        /// </param>
        /// <returns>
        /// A hibaszoveg, ha hiba volt, egyebkent ures
        /// </returns>
        public virtual string EgyediHibavizsg(DataGridViewCell dcell, Tablainfo tabinfo)
        {
            return "";
        }
        /// <summary>
        /// Rogzites utan vegrehajtando tennivalok: beallitja az adatbazis utolso modositasanak datumat,
        /// beallitja a control neve szerinti valtozasokat.
        /// Ha nem a TERVEZO mukodik, a Hivo RogzitesUtan-jat hivja, ha a tablainformacio szintje "U" vagy "C"
        /// </summary>
        public virtual void RogzitesUtan()
        {
            //            FakUserInterface.GetLastUpdate();
            ValtozasBeallitExcept(new string[] { "Verziovaltozas" });
            if (Hivo != null)
                Hivo.RogzitesUtan();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool RogzitesElott()
        {
            return true;
        }
        /// <summary>
        /// Elorol utan vegrehajtando tennivalok, ha van ilyen, felulirando
        /// </summary>
        public virtual void ElolrolUtan()
        {
        }
        /// <summary>
        /// Ha specialis eljaras kell az "Elveszithetjük a módositásokat?" kiadasa elott, felulirando
        /// Pl. a ControlAlap felulirja
        /// </summary>
        /// <returns></returns>
        public virtual bool Elhagyhat()
        {
            return true;
        }
        /// <summary>
        /// Valaszra varo uzenet:"Elveszithetjük a módositásokat?"
        /// </summary>
        /// <returns>
        /// Ha a valasz Igen,true
        /// </returns>
        public virtual bool Elveszithet()
        {
            return MessageBox.Show(FakUserInterface.GetUzenetSzoveg("Modositasvesztes"), "", MessageBox.MessageBoxButtons.IgenNem)
                                  == MessageBox.DialogResult.Igen;
        }

        /// <summary>
        /// Tablainformaciok (Aktualtablainfo) visszaallitasa az adatbazis szerint akkor is, ha nem volt valtozas
        /// </summary>
        public virtual void Elolrol()
        {
            Elolrol(true);
        }
        /// <summary>
        /// Ujraselectalas akkor is, ha nem volt valtozas a tablainformacioban
        /// </summary>
        /// <param name="tabinfo">
        /// A tablainformacio
        /// </param>
        public virtual void Elolrol(Tablainfo tabinfo)
        {
            FakUserInterface.ForceAdattolt(tabinfo, true);
        }
        /// <summary>
        /// Tablainformaciok (Aktualtablainfo) visszaallitasa az adatbazis szerint 
        /// </summary>
        /// <param name="force">
        /// false: csak a megvaltozottake
        /// true: mindegyike
        /// </param>
        public virtual void Elolrol(bool force)
        {
            FakUserInterface.ForceAdattolt(Aktualtablainfo, force);
        }
        /// <summary>
        /// Uj alkalmazas (ceg) eseten vegrehajtando,itt ures, megfelelo modon felulirando
        /// </summary>
        /// <returns>
        /// true: rendben
        /// </returns>
        public virtual bool Ceginicializalas()
        {
            return true;
        }
        /// <summary>
        /// Uj alkalmazas (ceg) eseten vegrehajtando, atveszi a cegindexet, felulirando
        /// </summary>
        /// <param name="cegindex">
        /// a ceg indexe
        /// </param>
        /// <returns></returns>
        public virtual bool Ceginicializalas(int cegindex)
        {
            this.cegindex = cegindex;
            return true;
        }
        /// <summary>
        /// Ha a felhasználás használja a UserLog táblát, inditáskor ezt hivhatja 
        /// Felül kell irnia
        /// </summary>
        /// <param name="cegindex">
        /// az utoljára aktiv cég indexe
        /// </param>
        /// <param name="lezartcege">
        /// lezart a ceg?
        /// </param>
        /// <param name="kezszint">
        /// kezeloi szint
        /// </param>
        /// <param name="usercontnev">
        /// az utolsó aktiv UserControl neve
        /// </param>
        /// <param name="userparam">
        /// a felhasználóspecifikus USERPARAMOK
        /// </param>
        /// <returns></returns>
        //public virtual bool Ceginicializalas(int cegindex, string usercontnev, string userparam)
        //{
        //    this.cegindex = cegindex;
        //    return true;
        //}
        public virtual bool Ceginicializalas(int cegindex, bool lezartcege, KezSzint kezszint, string usercontnev, string userparam)
        {
            this.cegindex = cegindex;
            return true;
        }
        /// <summary>
        /// egy inputelem hibavizsgalata 
        /// </summary>
        /// <param name="egytag">
        /// az inputelem Tag-ja
        /// </param>
        /// <returns>
        /// true:hibas
        /// </returns>
        public virtual bool Hibavizsg(MezoTag egytag)
        {
            if (egytag == null)
                return false;
            else
                return egytag.EgyHibavizsg(egytag.Control.Text);
        }
        /// <summary>
        /// Ha a mar letrehozott UserControlt aktivizaljuk, ez szolal meg. Itt ures, felulirando
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void TabStop_Changed(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void SetListaAdatszolg()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void EgyediDatumInit()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void EgyediDatumValtozas()
        {
        }
        public bool VanEgyediDatumHibavizsg = false;
        public virtual string EgyediDatumValtozas(DateTime tol, DateTime ig)
        {
            return "";
        }
        
        /// <summary>
        /// 
        /// </summary>
        public virtual void EgyediValasztekInit()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void EgyediValasztekValtozas(DateTime tol,DateTime ig)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void EgyediEgyszeruInit()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void EgyediEgyszeruValtozas()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void EgyediOsszetettInit()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void EgyediOsszetettValtozas()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        public void SajatJelzesBeallit()
        {
            Valtozaskezeles.SajatJelzesBeallit(this.Name);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool SajatJelzesLekerdez()
        {
            return Valtozaskezeles.SajatJelzesLekerdez(this.Name);
        }
        /// <summary>
        /// 
        /// </summary>
        public void SajatJelzesTorol()
        {
            Valtozaskezeles.SajatJelzesTorol(this.Name);
        }
        /// <summary>
        /// valamilyen torlesi funkcio eseten ellenorzi, hogy a torlendo adatot valahol mar hasznaltuk-e
        /// Ha igen, uzenetet ad
        /// </summary>
        /// <param name="tabinfo">
        /// A tablainformacio
        /// </param>
        /// <returns>
        /// hasznaltuk:true
        /// </returns>
        public bool ComboHasznalatban(Tablainfo tabinfo)
        {
            return ComboHasznalatban(tabinfo,false);
        }
        public bool ComboHasznalatban(Tablainfo tabinfo,bool csaktermszarm)
        {
            return ComboHasznalatban(tabinfo,null,"","","",csaktermszarm);
        }
        public bool ComboHasznalatban(Tablainfo tabinfo, DataGridViewCell cell, string oldertek, string aktertek, string aktidentity)
        {
            return ComboHasznalatban(tabinfo, cell, oldertek, aktertek, aktidentity, false);
        }
        /// <summary>
        /// valamilyen torlesi funkcio eseten ellenorzi, hogy a torlendo adatot valahol mar hasznaltuk-e
        /// Ha igen, a gridview aktiv soranak cellajanal hibauzenet
        /// </summary>
        /// <param name="tabinfo">
        /// A tablainformacio
        /// </param>
        /// <param name="cell">
        /// az aktiv cella
        /// </param>
        /// <param name="oldertek">
        /// regi ertek
        /// </param>
        /// <param name="aktertek">
        /// aktualis ertek
        /// </param>
        /// <param name="aktidentity">
        /// aktualis identity
        /// </param>
        /// <returns>
        /// hasznaltuk:true
        /// </returns>
        public bool ComboHasznalatban(Tablainfo tabinfo, DataGridViewCell cell, string oldertek, string aktertek, string aktidentity,bool csaktermszarm)
        {
            bool hasznalatban = false;
            Cols col = null;
            if (cell != null)
            {
                cell.ErrorText = "";
                col = tabinfo.InputColumns[cell.RowIndex];
            }
            string szoveg = "";
            if (!csaktermszarm)
            {
                OsszefinfoCollection coll = FakUserInterface.Osszefuggesek[tabinfo.Azontip];
                if (coll.Count != 0)
                {
                    if (aktidentity != "")
                    {
                        foreach (Osszefinfo egycol in coll)
                        {
                            string hasonnev;
                            if (egycol.tabinfo.Adatfajta == "S")
                                hasonnev = "RSORSZAM";
                            else if (egycol.azontip1 == tabinfo.Azontip)
                                hasonnev = "SORSZAM1";
                            else
                                hasonnev = "SORSZAM2";
                            egycol.TolteniKell = true;
                            egycol.Osszefinfotolt();
                            for (int j = 0; j < egycol.DataView.Count; j++)
                            {
                                DataRow dr = egycol.DataView[j].Row;
                                if (dr[hasonnev].ToString() == aktidentity)
                                {
                                    hasznalatban = true;
                                    if (szoveg == "")
                                        szoveg = "Nem módositható/törölhetö, az alábbi összetett táblafajtákban szerepel:\n";
                                    szoveg += egycol.tabinfo.TablaTag.Node.Text + "\n";
                                    break;
                                }
                            }
                        }
                        if (hasznalatban)
                        {
                            if (cell == null)
                                FakPlusz.MessageBox.Show(szoveg);
                            else
                                cell.ErrorText = szoveg;
                        }
                    }
                    else
                    {
                        hasznalatban = true;
                        szoveg = "Nem módositható/törölhetö, az alábbi összetett táblafajtákban szerepel:\n";
                        foreach (Osszefinfo egycol in coll)
                            szoveg += egycol.tabinfo.TablaTag.Node.Text + "\n";
                        if (cell == null)
                            FakPlusz.MessageBox.Show(szoveg);
                        else
                            cell.ErrorText = szoveg;
                    }
                }
            }
            if (!hasznalatban && tabinfo.Tablanev != "TARTAL" && (tabinfo.LehetCombo || tabinfo.TermSzarm == "T "))
            {
                Comboinfok comboinf = FakUserInterface.ComboInfok.ComboinfoKeres(tabinfo.Azontip);
                if (comboinf.Tabinfok.Count != 0)
                {
                    string ertek = aktertek;
                    if (ertek == "")
                    {
                        string nev = tabinfo.ComboFileba;
                        if (tabinfo.AktualViewRow == null)
                            ertek = comboinf.DefFileba;
                        else
                            ertek = tabinfo.AktualViewRow[nev].ToString();
                    }
                    if (ertek == "" || ertek == "0")
                    {
                    }
                    else
                    {
                        string azontip = tabinfo.Azontip;
                        string azon = tabinfo.Azon;
                        foreach (Tablainfo egytabinfo in comboinf.Tabinfok)
                        {
                            if (egytabinfo != tabinfo && egytabinfo.Tablanev != "KIAJANL")
                            {
                                string colnev = egytabinfo.ComboColumns.GetByComboAzontip(azontip).ColumnName;
                                Cols egycol = egytabinfo.ComboColumns[colnev];
                                int j = egycol.Combo_Info.ComboInfo.IndexOf(ertek);
                                if (j == -1)
                                    j = egycol.Combo_Info.ComboFileinfo.IndexOf(ertek);
                                else
                                    ertek = egycol.Combo_Info.ComboFileinfoAll()[j];
                                if (!csaktermszarm || egytabinfo.TermSzarm.Trim() == "T")
                                {
                                    DataTable dt = new DataTable(egytabinfo.Tablanev);
                                    if (egytabinfo.Tablanev != "TARTAL" && egytabinfo.Tablanev != "BASE" && egytabinfo.Tablanev != "LEIRO" && egytabinfo.TermSzarm.Trim() == "T")
                                        FakUserInterface.Select(dt, egytabinfo.Adattabla.Connection, egytabinfo.Tablanev, "", "", false);
                                    else
                                        dt = egytabinfo.Adattabla;
                                    DataView view = new DataView();
                                    view.Table = dt;
                                    view.RowFilter = colnev + "='" + ertek + "'";
                                    int kodtipuscol = egytabinfo.Kodtipuscol;
                                    string kodtipus = egytabinfo.Kodtipus;
                                    for (int i = 0; i < view.Count; i++)
                                    {
                                        DataRow row = view[i].Row;
                                        if (row[colnev].ToString() == ertek)
                                        {
                                            if (egytabinfo.Tablanev != "CEGKEZELOKIOSZT" || row["SZEREPKOD"].ToString() != "10")
                                            {
                                                if (szoveg == "")
                                                    szoveg = "Már használatban van\n  ";
                                                szoveg += egytabinfo.TablaTag.Node.Text + "-ben\n";
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            Tablainfo info = egytabinfo;
                                            if (kodtipuscol != -1)
                                                info = FakUserInterface.GetByAzontip(egytabinfo.Azon + row["KODTIPUS"].ToString());
                                            if (info != null)
                                            {
                                                if (szoveg == "")
                                                    szoveg = "Már használatban van\n  ";
                                                szoveg += info.TablaTag.Node.Text + "-ben\n";
                                                break;
                                            }
                                        }
                                    }
                                }
                                //                       }
                                //if (megvan)
                                //    break;
                            }
                        }
                    }
                    if (szoveg != "")
                    {
                        hasznalatban = true;
                        szoveg += "\nNem módositható/törölhetö!";
                        if (cell == null)
                            FakPlusz.MessageBox.Show(szoveg);
                        else
                            cell.ErrorText = szoveg;
                    }
                    else if (cell != null)
                        cell.ErrorText = "";

                }
            }
            return hasznalatban;
        }

        /// <summary>
        /// a hivo UserControl osszes beallitando valtozasat beallitja az osszes ezekre erzekeny UserControl szamara
        /// </summary>
        public void ValtozasBeallit()
        {
            Valtozaskezeles.ValtozasBeallit(this.Name, Valtozaskezeles.valtnevek, Valtozaskezeles.uscontnevek, null);
        }
        /// <summary>
        /// valtnev nevu valtozasat beallitja az osszes erre erzekeny UserControl szamara
        /// </summary>
        /// <param name="valtnev">
        /// a valtozas neve
        /// </param>
        public void ValtozasBeallit(string valtnev)
        {
            Valtozaskezeles.ValtozasBeallit(this.Name, new string[] { valtnev }, Valtozaskezeles.uscontnevek, null);
        }
        /// <summary>
        /// valtnevek-ben felsorolt nevu valtozasokat beallitja az osszes erre erzekeny UserControl szamara
        /// </summary>
        /// <param name="valtnevek">
        /// az igenyelt valtozasnevek
        /// </param>
        public void ValtozasBeallit(string[] valtnevek)
        {
            Valtozaskezeles.ValtozasBeallit(this.Name, valtnevek, Valtozaskezeles.uscontnevek, null);
        }
        /// <summary>
        /// valtnev nevu valtozast beallitja a celcontrol nevu UserControl szamara
        /// </summary>
        /// <param name="celcontrol"></param>
        /// <param name="valtnev"></param>
        public void ValtozasBeallit(string celcontrol, string valtnev)
        {
            Valtozaskezeles.ValtozasBeallit(this.Name, new string[] { valtnev }, new string[] { celcontrol }, null);
        }
        /// <summary>
        /// valtnevek-ben szereplo valtozasokat beallitja a celcontrol nevu UserControl szamara
        /// </summary>
        /// <param name="celcontrol"></param>
        /// <param name="valtnevek"></param>
        public void ValtozasBeallit(string celcontrol, string[] valtnevek)
        {
            Valtozaskezeles.ValtozasBeallit(this.Name, valtnevek, new string[] { celcontrol }, null);
        }
        /// <summary>
        /// valtnev nevu valtozast beallitja a celcontrolok-ban felsorolt nevu UserControlok szamara
        /// </summary>
        /// <param name="celcontrolok"></param>
        /// <param name="valtnev"></param>
        public void ValtozasBeallit(string[] celcontrolok, string valtnev)
        {
            Valtozaskezeles.ValtozasBeallit(this.Name, new string[] { valtnev }, celcontrolok, null);
        }
        /// <summary>
        /// valtnevek-ben szereplo valtozasokat beallitja a celcontrolok-ban felsorolt nevu UserControlok szamara
        /// </summary>
        /// <param name="celcontrolok"></param>
        /// <param name="valtnevek"></param>
        public void ValtozasBeallit(string[] celcontrolok, string[] valtnevek)
        {
            Valtozaskezeles.ValtozasBeallit(this.Name, valtnevek, celcontrolok, null);
        }
        /// <summary>
        /// A hivo UserControl osszes beallitando valtozasa kozul a kivetel-ben szereploek kivetelevel beallitja
        /// a valtozasokat minden ezekre erzekeny UserControl szamara
        /// </summary>
        /// <param name="kivetel"></param>
        public void ValtozasBeallitExcept(string[] kivetel)
        {
            Valtozaskezeles.ValtozasBeallit(this.Name, Valtozaskezeles.valtnevek, Valtozaskezeles.uscontnevek, kivetel);
        }
        /// <summary>
        /// torli a hivo UserControl szamara beallitott valtozasokat
        /// </summary>
        public void ValtozasTorol()
        {
            Valtozaskezeles.ValtozasTorol(this.Name, "");
        }
        /// <summary>
        /// torli a hivo UserControl szamara beallitott valtnev nevu valtozast
        /// </summary>
        /// <param name="valtnev">
        /// a torlendo valtozas neve
        /// </param>
        public void ValtozasTorol(string valtnev)
        {
            Valtozaskezeles.ValtozasTorol(this.Name, valtnev);
        }
        /// <summary>
        /// a felsoroltak kivetelevel torli a hivo UserControl valtozasait
        /// </summary>
        /// <param name="valtnevek">
        /// a nem torlendo valtozasok
        /// </param>
        public void ValtozasTorolExcept(string[] valtnevek)
        {
            Valtozaskezeles.ValtozasTorolExcept(this.Name, valtnevek);
        }
        /// <summary>
        /// A UserControl szamara beallitott valtozasainak tombje
        /// </summary>
        /// <returns>
        /// a tomb vagy null
        /// </returns>
        public ArrayList ValtozasLekerdez()
        {
            return Valtozaskezeles.ValtozasLekerdez(this.Name, "");
        }
        /// <summary>
        /// Be van-e allitva egy adott nevu valtozas?
        /// </summary>
        /// <param name="valtnev">
        /// a valtozas neve
        /// </param>
        /// <returns>
        /// Ha igen, egyelemu tomb, egyebkent null
        /// </returns>
        public ArrayList ValtozasLekerdez(string valtnev)
        {
            return Valtozaskezeles.ValtozasLekerdez(this.Name, valtnev);
        }
        /// <summary>
        /// Van-e beallitva a megadottakon kivuli valtozas?
        /// </summary>
        /// <param name="valtnevek">
        /// azon valtozasok neve, melyek nem erdekelnek
        /// </param>
        /// <returns>
        /// A tobbi valtozas tombje vagy null
        /// </returns>
        public ArrayList ValtozasLekerdezExcept(string[] valtnevek)
        {
            return Valtozaskezeles.ValtozasLekerdezExcept(this.Name, valtnevek);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Visible_Changed(object sender, EventArgs e)
        {

        }

        public virtual void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }
        public virtual void UserInputtablabaTovabb(Base control,Tablainfo info )
        {
        }
        public virtual void comboBox1_TextUpdate(object sender, EventArgs e)
        {

        }
        public virtual void elolrol_Click(object sender, EventArgs e)
        {
        }
        public virtual void elozo_Click(object sender, EventArgs e)
        {
        }
        public virtual void kovetkezo_Click(object sender, EventArgs e)
        {
        }
        public virtual void ok_Click(object sender, EventArgs e)
        {
        }
        public virtual void uj_Click(object sender, EventArgs e)
        {
        }
        public virtual void teljestorles_Click(object sender, EventArgs e)
        {
        }
        public virtual void elozoverzio_Click(object sender, EventArgs e)
        {
        }
        public virtual void kovetkezoverzio_Click(object sender,EventArgs e)
        {
        }
        public virtual bool DatumParameterezInit(DateTime[] mindatumok, DateTime[] maxdatumok, DateTime[] alapertdatumok, DateTime[] aktdatumok)
        {
            return false;
        }

        /// <summary>
        /// Lathatova teszi a GridViewban az aktualis sort
        /// </summary>
        public void SetAktRowVisible(DataGridView gridview,Tablainfo tabinfo)
        {
            SetAktRowVisible(gridview, tabinfo.ViewSorindex);
        }
        public void SetAktRowVisible(DataGridView gridview, int sorindex)
        {
            FakUserInterface.SetAktRowVisible(gridview, sorindex);
        }
        public virtual void SetUserSelect(Tablainfo tabinfo, string userselect)
        {
            tabinfo.UserSelect = userselect;
        }
        public virtual void SaveUserLogLastRow()
        {
        }
        public virtual Base SetAktivControl(TablainfoTag tabinfotag,Vezerloinfo vezerloinfo)
        {
            return null;
        }
    }
    /// <summary>
    /// Valtozasok kezelesenek osztalya
    /// </summary>
   public static class Valtozaskezeles
   {
       static DataTable UserControlSajatJelzes = new DataTable("Sajatjelzes");
       static DataView UserControlSajatJelzesView = new DataView();
       static DataTable UserControlBeallitottValtozasok = new DataTable("Valtozasok");
       static DataTable UserControlValtozastBeallit = new DataTable("Beallit");
       static DataTable UserControlValtozasraErzekeny = new DataTable("Erzekeny");
       static DataView UserControlValtozastBeallitView = new DataView();
       static DataView UserControlValtozasraErzekenyView = new DataView();
       static DataView UserControlBeallitottValtozasokView = new DataView();
       static FakUserInterface FakUserInterface = null;
       /// <summary>
       /// valtozasnevek tombje
       /// </summary>
       public static string[] valtnevek = null;
       /// <summary>
       /// usercontrolnevek tombje
       /// </summary>
       public static string[] uscontnevek = null;
       /// <summary>
       /// usercontrolnevek listaja
       /// </summary>
       public static ArrayList uscontnevekarray = new ArrayList();
       /// <summary>
       /// tooltiptextek tombje
       /// </summary>
       public static string[] tooltiptextek = null;
       /// <summary>
       /// Harom adattablat allit elo :
       ///     1. UserControlBeallitottValtozasok
       ///     2. UserControlValtozastBeallit
       ///     3. UserControlValtozasraErzekeny
       /// A tablak minden sora egy-egy UserControl-ra vonatkozik. Azokra a UserControlokra, melyek a Tervezo-vel
       /// a UserControlok nevei("USERCONTROLNEVEK" SQL tabla) kozt definialtak es ott az Owner alkalmazas ures vagy azonos az alkalmazas parameterevel
       /// Oszlopok :
       ///     USERID: a "USERCONTROLNEVEK" ID-jenek tarolasara , 
       ///     USERCONTROLNEV : a "USERCONTROLNEVEK" SZOVEG-enek tarolasara
       ///     A Tervezo-vel a Valtozasfajtak-ban definialt valtozasok nevei kozul mindaz, melynek Owner alkalmazasa
       ///         ures illetve azonos az alkalmazas parameterevel "True" ill "False" tarolasara
       ///      1.-ben minden False-ra inicializalt
       ///      2.-ben azok az oszlopok tartalmaznak True-t, melyek a Valtbeallit osszefuggesben definialtak
       ///      3.-ban azok az oszlopok tartalmaznak True-t, melyek a Valterzekeny osszefuggesben definialtak
       /// Letrehozza ezenkivul a tooltiptextek tombjet a "USERCONTROLNEVEK" SQL tabla erintett sorainak
       /// TOOLTIP nevu oszlopanak tartalmabol
       /// </summary>
       /// <param name="fak">
       /// FakUserInterface
       /// </param>
       /// <param name="alkalmazas">
       /// alkalmazas neve, vagy ures
       /// ha ures, minden UserControlNev es minden Valtozasnev bekerul a tablakba
       /// </param>
       public static void ValtozaskezelesInit(FakUserInterface fak, string alkalmazas)
       {
           FakUserInterface = fak;
           Tablainfo Valtozasnevek = FakUserInterface.GetBySzintPluszTablanev("R", "USERVALTOZASNEVEK");
           string[] ertek;
           if (alkalmazas == "")
               ertek = new string[] { "" };
           else
               ertek = new string[] { alkalmazas, "" };
           string[] valtozasidk = Valtozasnevek.Adattabla.GetTartal("ID", "ALKALMAZAS_ID_K", ertek);
           valtnevek = Valtozasnevek.Adattabla.GetTartal("SZOVEG", "ID", valtozasidk);
           Tablainfo UserControlok = FakUserInterface.GetBySzintPluszTablanev("R", "USERCONTROLNEVEK");
           uscontnevek = UserControlok.Adattabla.GetTartal("SZOVEG", "ALKALMAZAS_ID_K", ertek);
           uscontnevekarray.AddRange(uscontnevek);
           tooltiptextek = UserControlok.Adattabla.GetTartal("TOOLTIP", "ALKALMAZAS_ID_K", ertek);
           string[] useridk = UserControlok.Adattabla.GetTartal("ID", "SZOVEG", uscontnevek);
           ColumnsRowsGyart(UserControlSajatJelzes, null, uscontnevek, useridk, null, null);
           UserControlSajatJelzesView.Table = UserControlSajatJelzes;
           UserControlSajatJelzesView.Sort = "USERCONTROLNEV";
           Tablainfo Beallitinfo = FakUserInterface.GetOsszef("R", "Valtbeallit");
           Tablainfo Erzekenyinfo = FakUserInterface.GetOsszef("R", "Valterzekeny");
           ColumnsRowsGyart(UserControlValtozastBeallit, Beallitinfo, uscontnevek, useridk, valtozasidk, valtnevek);
           ColumnsRowsGyart(UserControlValtozasraErzekeny, Erzekenyinfo, uscontnevek, useridk, valtozasidk, valtnevek);
           ColumnsRowsGyart(UserControlBeallitottValtozasok, null, uscontnevek, useridk, valtozasidk, valtnevek);
           UserControlValtozastBeallitView.Table = UserControlValtozastBeallit;
           UserControlValtozastBeallitView.Sort = "USERCONTROLNEV";
           UserControlValtozasraErzekenyView.Table = UserControlValtozasraErzekeny;
           UserControlValtozasraErzekenyView.Sort = "USERCONTROLNEV";
           UserControlBeallitottValtozasokView.Table = UserControlBeallitottValtozasok;
           UserControlBeallitottValtozasokView.Sort = "USERCONTROLNEV";

       }
       private static void ColumnsRowsGyart(DataTable dt, Tablainfo osszefinfo, string[] contnevek, string[] idk, string[] valtozasidk, string[] valtnevek)
       {
           dt.Columns.Add("USERID");
           dt.Columns.Add("USERCONTROLNEV");
           if (valtnevek == null)
               dt.Columns.Add("SAJATJELZES");
           else
           {
               for (int i = 0; i < valtnevek.Length; i++)
                   dt.Columns.Add(valtnevek[i]);
           }
           for (int i = 0; i < contnevek.Length; i++)
           {
               DataRow ujsor = dt.NewRow();
               ujsor["USERID"] = idk[i];
               ujsor["USERCONTROLNEV"] = contnevek[i];
               if (valtnevek == null)
                   ujsor[2] = false;
               else
                   for (int j = 0; j < valtnevek.Length; j++)
                       ujsor[valtnevek[j]] = false;

               if (osszefinfo != null)
               {
                   string[] id = osszefinfo.Adattabla.GetTartal("SORSZAM2", "SORSZAM1", idk[i]);
                   if (id != null)
                   {
                       for (int k = 0; k < id.Length; k++)
                       {
                           string keresendo = id[k];
                           for (int j = 0; j < valtozasidk.Length; j++)
                           {
                               if (valtozasidk[j] == keresendo)
                                   ujsor[valtnevek[j]] = true;
                           }
                       }
                   }
               }
               dt.Rows.Add(ujsor);
           }
       }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="contnev"></param>
       public static void SajatJelzesBeallit(string contnev)
       {
           int drind = UserControlSajatJelzesView.Find(contnev);
           if (drind != -1)
           {
               DataRow dr = UserControlSajatJelzesView[drind].Row;
               dr[2] = true;
           }
       }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="contnev"></param>
       /// <returns></returns>
       public static bool SajatJelzesLekerdez(string contnev)
       {
           int drind = UserControlSajatJelzesView.Find(contnev);
           if (drind == -1)
               return false;
           DataRow dr = UserControlSajatJelzesView[drind].Row;
           return dr[2].ToString() == "True";
       }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="contnev"></param>
       public static void SajatJelzesTorol(string contnev)
       {
           int drind = UserControlSajatJelzesView.Find(contnev);
           if (drind != -1)
           {
               DataRow dr = UserControlSajatJelzesView[drind].Row;
               dr[2] = false;
           }
       }
       /// <summary>
       /// A beallitott valtozasok tablajaban a megadott nevu UserControl soraban a megadott nevu valtozas oszlopat false-ra allitja
       /// </summary>
       /// <param name="contnev">
       /// a UserControl neve
       /// </param>
       /// <param name="valtnev">
       /// a valtozas neve
       /// </param>
       public static void ValtozasTorol(string contnev, string valtnev)
       {
           UserControlBeallitottValtozasokView.RowFilter = "USERCONTROLNEV='" + contnev + "'";
           for (int j = 0; j < UserControlBeallitottValtozasokView.Count; j++)
           {
               DataRow dr = UserControlBeallitottValtozasokView[j].Row;
               for (int i = 0; i < valtnevek.Length; i++)
               {
                   if (valtnev == "" || valtnev == valtnevek[i])
                       dr[valtnevek[i]] = false;
               }
           }
           UserControlBeallitottValtozasokView.RowFilter = "";
       }
       /// <summary>
       /// A beallitott valtozasok tablajaban a megadott nevu UserControl soraban minden olyan valtozasoszlop
       /// tartalmat false-ra allitja, melynek neve a kivetelek tombjeben nem szerepel
       /// </summary>
       /// <param name="contnev">
       /// a UserControl neve
       /// </param>
       /// <param name="valtnevek">
       /// a kivetelek tombje
       /// </param>
       public static void ValtozasTorolExcept(string contnev, string[] valtnevek)
       {
           UserControlBeallitottValtozasokView.RowFilter = "USERCONTROLNEV='" + contnev + "'";
           for (int k = 0; k < UserControlBeallitottValtozasokView.Count; k++)
           {
               DataRow dr = UserControlBeallitottValtozasokView[k].Row; ;//UserControlBeallitottValtozasokView.Find(contnev)].Row;
               for (int i = 2; i < UserControlBeallitottValtozasok.Columns.Count; i++)
               {
                   string name = UserControlBeallitottValtozasok.Columns[i].ColumnName;
                   bool kell = true;
                   for (int j = 0; j < valtnevek.Length; j++)
                   {
                       if (valtnevek[j] == name)
                       {
                           kell = false;
                           break;
                       }
                   }
                   if (kell)
                       dr[name] = false;
               }
           }
       }
       /// <summary>
       /// Megkeresi a contnev nevu sort a UserControlValtozastBeallit tablaban. A sorban megkeresi azokat az oszlopokat,
       /// melyek neve szerepel a valtnevek tombben, es nem szerepel a kivetel tombben es erteke True
       /// Ezen oszlopoknevekkel keres a UserControlValtozasraErzekeny tablaban, ott kivalasztja azokat a sorokat, 
       /// melyek adott oszloptartalma True szerepel es, ha a sor USERCONTROLNEV oszlopa szerepel a
       /// celkontnevek tombjeben, akkor a UserControlBeallitottValtozasok tabla adott soranak,oszlopanak
       /// tartalmat True-ra allitja
       /// </summary>
       /// <param name="contnev"></param>
       /// <param name="valtnevek"></param>
       /// <param name="celcontnevek"></param>
       /// <param name="kivetel"></param>
       public static void ValtozasBeallit(string contnev, string[] valtnevek, string[] celcontnevek, string[] kivetel)
       {
           string controlnev = contnev;
           int colindex;
           int i;
           DataRow dr;
           DataView view = UserControlValtozastBeallitView;
           view.RowFilter = "";
           if (celcontnevek.Length != uscontnevek.Length)
           {
               view.RowFilter = "USERCONTROLNEV = '" + controlnev + "'";
               for (int j = 0; j < celcontnevek.Length; j++)
                   view.RowFilter += " OR USERCONTROLNEV = '" + celcontnevek[j] + "'";
           }
           i = view.Find(controlnev);
           if (i != -1)
           {
               dr = view[i].Row;
               for (int j = 0; j < valtnevek.Length; j++)
               {
                   bool kell = true;
                   if (kivetel != null)
                   {
                       foreach (string kiv in kivetel)
                       {
                           if (valtnevek[j] == kiv)
                           {
                               kell = false;
                               break;
                           }
                       }
                   }
                   if (kell)
                   {
                       colindex = dr.Table.Columns.IndexOf(valtnevek[j]);
                       if (colindex != -1)
                       {
                           if (dr[colindex].ToString() == "True")             // a control beallithatja ill.erzekeny az adott nevu valtozasra
                           {
                               for (int k = 0; k < UserControlValtozasraErzekeny.Rows.Count; k++)
                               {
                                   DataRow dr1 = UserControlValtozasraErzekeny.Rows[k];
                                   if (dr1[colindex].ToString() == "True") // ez a control erzekeny az adott nevu valtozasra
                                   {
                                       UserControlBeallitottValtozasok.Rows[k][colindex] = true;
                                   }
                               }
                           }
                       }
                   }
               }
           }
           view.RowFilter = "";
       }
       /// <summary>
       /// A UserControlBeallitottValtozasok tombjeben kivalasztja a
       /// USERCONTROLNEV=contnev sort.
       /// A visszaadott array azon oszlopneve(ke)t tartalmazza, melyek erteke True
       /// valtnev="" esetben az osszes oszlopot, egyebkent csak a valtnev nevut vizsgalja
       /// Ha nincs True ertek, 0 hosszu array a visszateres
       /// </summary>
       /// <param name="contnev"></param>
       /// <param name="valtnev"></param>
       /// <returns></returns>
       public static ArrayList ValtozasLekerdez(string contnev, string valtnev)
       {
           ArrayList ar = new ArrayList();
           UserControlBeallitottValtozasokView.RowFilter = "USERCONTROLNEV='" + contnev + "'";
           //if (UserControlBeallitottValtozasokView.Count == 0)
           //{
           //    UserControlBeallitottValtozasokView.RowFilter = "";
           //    return ar;
           //}
           //int i = UserControlBeallitottValtozasokView.Find(contnev);
           //if (i != -1)
           for(int i=0;i<UserControlBeallitottValtozasokView.Count;i++)
           {
               DataRow dr = UserControlBeallitottValtozasokView[i].Row;
               for (int j = 2; j < UserControlBeallitottValtozasok.Columns.Count; j++)
               {
                   DataColumn col = UserControlBeallitottValtozasok.Columns[j];
                   string name = col.ColumnName;
                   if ((valtnev == "" || valtnev == name) && dr[j].ToString() == "True")
                       ar.Add(name);
               }
           }
           UserControlBeallitottValtozasokView.RowFilter = "";
           return ar;
       }
       /// <summary>
       /// Olyan, mint a ValtozasLekerdez, csak mindig minden oszlopot vizsgalat ala vesz,
       /// melynek neve nem szerepel a valtnevek tombben
       /// </summary>
       /// <param name="contnev"></param>
       /// <param name="valtnevek"></param>
       /// <returns></returns>
       public static ArrayList ValtozasLekerdezExcept(string contnev, string[] valtnevek)
       {
           ArrayList ar = new ArrayList();
           UserControlBeallitottValtozasokView.RowFilter = "USERCONTROLNEV='" + contnev + "'";
           //int i = UserControlBeallitottValtozasokView.Find(contnev);
           //if (i != -1)
           for (int i = 0; i < UserControlBeallitottValtozasokView.Count; i++)
           {
               DataRow dr = UserControlBeallitottValtozasokView[i].Row;
               for (int j = 2; j < UserControlBeallitottValtozasok.Columns.Count; j++)
               {
                   DataColumn col = UserControlBeallitottValtozasok.Columns[j];
                   string name = col.ColumnName;
                   bool kell = true;
                   for (int k = 0; k < valtnevek.Length; k++)
                   {
                       if (valtnevek[k] == name)
                       {
                           kell = false;
                           break;
                       }
                   }
                   if (kell && dr[j].ToString() == "True")
                       ar.Add(name);
               }
           }
           UserControlBeallitottValtozasokView.RowFilter = "";
           return ar;
       }

//2
//3
//<startup useLegacyV2RuntimeActivationPolicy="true">
//        <supportedRuntime version="v4.0"/>
//</startup>
   }
}
