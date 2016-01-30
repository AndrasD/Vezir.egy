using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Globalization;
using System.Data;
using System.Threading;
using FakPlusz;
using FakPlusz.Alapcontrolok;
using FormattedTextBox;

namespace FakPlusz.Alapfunkciok
{
    /// <summary>
    /// User programbol hozzaferheto property-k, metodusok
    /// </summary>
    /// 
    public partial class FakUserInterface
    {
        private Form _mainform = null;
        /// <summary>
        /// 
        /// </summary>
        public Form MainForm
        {
            get { return _mainform; }
        }
        private bool _enyem = true;
        /// <summary>
        /// true: Gittacska gepen
        /// Arra hasznalom, hogy az alaprendszer tablait a tartalomjegyzekbol ne lehessen torolni
        /// 
        /// </summary>
        public bool Enyem
        {
            //get { return false; }
            get { return _enyem; }
        }
        private bool _mysqle = false;
        /// <summary>
        /// MySql az adatbazis?
        /// </summary>
        public bool MySqle
        {
            get { return _mysqle; }
        }
        private Tablainfo _leirotablainfo = null;
        private TablainfoCollection _tablainfok = new TablainfoCollection();
        /// <summary>
        /// Az osszes tablaszintu informacio
        /// </summary>
        public TablainfoCollection Tablainfok
        {
            get { return _tablainfok; }
        }
        /// <summary>
        /// KODTAB,OSSZEF,... leirotablainfoi
        /// </summary>
        public TablainfoCollection SpecialisLeiroTabinfok = new TablainfoCollection();
        /// <summary>
        /// Osszefugges infok kollekcioja
        /// </summary>
        public OsszefinfoCollection Osszefuggesek = new OsszefinfoCollection();
        /// <summary>
        /// gyokerlancok
        /// </summary>
        public ArrayList gyokerchainek = new ArrayList();
        /// <summary>
        /// A termeszetes ceg plusz cegalatti tablak szulo/gyermek lancba rendezettek
        /// Az olyan szulo, melynek nincs szuloje, gyokertablainformacio
        /// Egy alkalmazasban legalabb egy ilyen van, de tobb is lehet
        /// A collection annyi elemu, ahany gyokertablainfo van
        /// </summary>
        public TablainfoCollection[] GyokerChainek
        {
            get
            {
                if (gyokerchainek.Count == 0)
                    return null;
                else
                    return (TablainfoCollection[])gyokerchainek.ToArray(typeof(TablainfoCollection));
            }
        }

        private TablainfoCollection _gyokertablainfok = new TablainfoCollection();
        /// <summary>
        /// A gyokertablainformacio(k)
        /// </summary>
        public TablainfoCollection GyokerTablainfok
        {
            get { return _gyokertablainfok; }
        }
        /// <summary>
        /// gyokertablainfok TablainfoTag-jai
        /// </summary>
        public TablainfoTagCollection GyokerTablainfoTagok = new TablainfoTagCollection();
        private string _rendszerconn = "";
        /// <summary>
        /// rendszeradatbazis Connection String
        /// </summary>
        public string Rendszerconn
        {
            get { return _rendszerconn; }
        }
        private string _userconn = "";
        /// <summary>
        /// useradatbazis Connection String
        /// </summary>
        public string Userconn
        {
            get { return _userconn; }
        }
        private ArrayList _cegconnectionok = new ArrayList();
        /// <summary>
        /// a cegconnectionok
        /// </summary>
        public ArrayList Cegconnectionok
        {
            get { return _cegconnectionok; }
        }
        private string _aktualcegconn = "";
        /// <summary>
        /// Az aktualisan valasztott alkalmazasi adatbazis Connection String-je
        /// </summary>
        public string AktualCegconn
        {
            get { return _aktualcegconn; }
            set { _aktualcegconn = value; }
        }
        private long _aktualcegid = -1;
        /// <summary>
        /// Aktualis ceg identity
        /// </summary>
        public long AktualCegid
        {
            get { return _aktualcegid; }
            set { _aktualcegid = value; }
        }
        private int _aktualcegindex = -1;
        /// <summary>
        /// Aktualis ceg indexe
        /// </summary>
        public int AktualCegIndex
        {
            get { return _aktualcegindex; }
        }
        private string _aktualcegnev = "";
        /// <summary>
        /// az aktualis ceg neve
        /// </summary>
        public string AktualCegnev
        {
            get { return _aktualcegnev; }
        }
        private bool _ujceg = false;
        /// <summary>
        /// Uj ceg inicialiazalas jelzes
        /// </summary>
        public bool Ujceg
        {
            get { return _ujceg; }
            set { _ujceg = value; }
        }
        /// <summary>
        /// CEG tabla tablainformacioja
        /// </summary>
        private Tablainfo _ceginfo = null;
        /// <summary>
        /// CEGEK tablainfoja
        /// </summary>
        public Tablainfo Ceginfo
        {
            get
            {
                return _ceginfo;
            }
        }
        private bool _formfaload = false;
        private string[] _alapszintek = new string[] { "R", "U", "C" };
        private string _szintstring = "";
        /// <summary>
        /// Cegszint alatti szintek
        /// </summary>
        public string Szintstring
        {
            get { return _szintstring; }
            set
            {
                if (!_szintstring.Contains(value))
                    _szintstring += value;
            }
        }
        private string[] _alapconnectionstringek = new string[] { "", "", "" };
        private TreeNode _leironode = null;
        /// <summary>
        /// LEIRO Node-ja
        /// </summary>
        public TreeNode LeiroNode
        {
            get { return _leironode; }
        }
        private TablainfoTag _leirotag = null;
        /// <summary>
        /// LEIRO TablainfoTag-ja
        /// </summary>
        public TablainfoTag LeiroTag
        {
            get { return _leirotag; }
        }
        private TreeNode _basenode = null;
        /// <summary>
        /// BASE Node-ja
        /// </summary>
        public TreeNode BaseNode
        {
            get { return _basenode; }
        }
        private TablainfoTag _basetag = null;
        /// <summary>
        /// BASE TablainfoTag-ja
        /// </summary>
        public TablainfoTag BaseTag
        {
            get { return _basetag; }
        }
        private ProgressBar ProgressBar = null;
        private Label ProgressLabel = null;
        private DateTime _mindatum = DateTime.MinValue;
        /// <summary>
        /// A leheto legkisebb datum
        /// </summary>
        public DateTime Mindatum
        {
            get { return _mindatum; }
        }
        private DateTime _maxdatum = DateTime.MaxValue;
        /// <summary>
        /// A leheto legnagyobb datum
        /// </summary>
        public DateTime Maxdatum
        {
            get { return _maxdatum; }
        }
        private Color _aktivinputbackcolor = Color.Orange;
        /// <summary>
        /// Aktiv input Control BackColor, default:Color.Orange
        /// </summary>
        public Color AktivInputBackColor
        {
            get { return _aktivinputbackcolor; }
            set { _aktivinputbackcolor = value; }
        }
        private Color _inaktivinputbackcolor = Color.White;
        /// <summary>
        /// Inaktiv input Control BackColor, default:Color.White
        /// </summary>
        public Color InaktivInputBackColor
        {
            get { return _inaktivinputbackcolor; }
            set { _inaktivinputbackcolor = value; }
        }
        private Font _aktivinputfont = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        /// <summary>
        /// Aktiv input Control Font, default:Arial, 9F
        /// </summary>
        public Font AktivInputFont
        {
            get { return _aktivinputfont; }
            set { _aktivinputfont = value; }
        }
        private Font _inaktivinputfont = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        /// <summary>
        /// Inaktiv input Control Font, default:Arial, 9F
        /// </summary>
        public Font InaktivInputFont
        {
            get { return _inaktivinputfont; }
            set { _inaktivinputfont = value; }
        }
        private Color _aktivcontrolbackcolor = Color.Khaki;
        /// <summary>
        /// Aktiv  input Panel/GroupBox BackCOlor, default: Color.Khaki
        /// </summary>
        public Color AktivControlBackColor
        {
            get { return _aktivcontrolbackcolor; }
            set { _aktivcontrolbackcolor = value; }
        }
        private Color _inaktivcontrolbackcolor = System.Drawing.SystemColors.ControlLight;
        /// <summary>
        /// Inaktiv  input Panel/GroupBox BackCOlor, default: SystemColors.ControlLight
        /// </summary>
        public Color InaktivControlBackColor
        {
            get { return _inaktivcontrolbackcolor; }
            set { _inaktivcontrolbackcolor = value; }
        }
        string[] _fullnevek = new string[]{"System.Windows.Forms.TextBox","FormattedTextBox.FormattedTextBox",
                "System.Windows.Forms.ComboBox","System.Windows.Forms.CheckBox","System.Windows.Forms.RadioButton",
                "System.Windows.Forms.DateTimePicker","System.Windows.Forms.Label","System.Windows.Forms.DataGridView","System.Windows.Forms.DataGridViewTextBoxCell",
                "System.Windows.Forms.DataGridViewCheckBoxCell","System.Windows.Forms.DataGridViewComboBoxCell"};
        string[] _nevek = new string[]{"TextBox","FormattedTextBox","ComboBox","CheckBox","RadioButton",
                "DateTimePicker","Label","DataGridView","TextBoxCell","CheckBoxCell","ComboBoxCell"};
        private UserControlCollection _usercontrolok = new UserControlCollection();
        /// <summary>
        /// A programbol aktivizalt UserControlok informacioi
        /// </summary>
        public UserControlCollection UserControlok
        {
            get { return _usercontrolok; }
        }
        private Vezerloinfo _mainvezerles;
        /// <summary>
        /// Vezerloinformaciok 
        /// </summary>
        public Vezerloinfo MainVezerles
        {
            get { return _mainvezerles; }
            set { _mainvezerles = value; }
        }
        private string[] _hozferstringek = null;
        /// <summary>
        /// Lehetseges hozzaferesi jogosultsagok szovegesen
        /// </summary>
        public string[] HozferStringek
        {
            get { return _hozferstringek; }
        }
        /// <summary>
        /// Cegszint es cegszint alatti szintek (egy szint egy karakter)
        /// </summary>
        public string Adatszintek
        {
            get { return "C" + _szintstring; }
        }
        private int _kezeloid = -1;
        /// <summary>
        /// A bejelentkezett kezelo id-je, TERVEZO-ben -1
        /// </summary>
        public int KezeloId
        {
            get { return _kezeloid; }
            set { _kezeloid = value; }
        }
        private string _adatbazisfajta = "Sql";
        /// <summary>
        /// "Sql" vagy "MySql"
        /// </summary>
        public string Adatbazisfajta
        {
            get { return _adatbazisfajta; }
        }
        private DateTime[] _aktintervallum;
        /// <summary>
        /// A cegszintu adatok ervenyessegenek intervalluma .
        /// Mindig egy adott honap 1-tol a honap utolso napjaig
        /// </summary>
        public DateTime[] Aktintervallum
        {
            get
            {
                return _aktintervallum;
            }
        }
        private ArrayList _specdatumnevek = new ArrayList();
        /// <summary>
        /// 
        /// </summary>
        public ArrayList SpecDatumNevek
        {
            get { return _specdatumnevek; }
            set { _specdatumnevek = value; }
        }
        private string[] _shortdateintervallum;
        /// <summary>
        /// az Aktintervallum ShortDate formaban
        /// </summary>
        public string[] ShortDateIntervallum
        {
            get { return _shortdateintervallum; }
        }
        private ComboCollection _comboinfok = new ComboCollection();
        /// <summary>
        /// Az osszes Combo-informacio
        /// </summary>
        public ComboCollection ComboInfok
        {
            get { return _comboinfok; }
        }
        private DateTime _letrehozasdatuma = DateTime.Now;
        /// <summary>
        /// a fak objektum letrehozasanak datuma
        /// </summary>
        public DateTime LetrehozasDatuma
        {
            get { return _letrehozasdatuma; }
            set { _letrehozasdatuma = value; }
        }
        private DateTime _valtoztatasdatuma = DateTime.Now;
        /// <summary>
        /// Az utoljara kiadott adatbazis update datuma
        /// </summary>
        public DateTime ValtoztatasDatuma
        {
            get { return _valtoztatasdatuma; }
            set { _valtoztatasdatuma = value; }
        }
        /// <summary>
        /// true, ha a valtozasokat rogziteni kell a megfelelo VALTOZASNAPLO-ba
        /// </summary>
        public bool KellValtozasRogzit = true;
        private string _valtoztatasfunkcio = "";
        /// <summary>
        /// Milyen valtoztatast eszkozlunk az adattablan:
        /// ADD/TOROL/MODIFY/DELETEVERSION/CREATEVERSION
        /// </summary>
        public string ValtoztatasFunkcio
        {
            get { return _valtoztatasfunkcio; }
            set { _valtoztatasfunkcio = value; }
        }
        private bool _bajvan = false;
        /// <summary>
        /// Ha true, ellentmondas van legalabb egy tabla es annak leirotablaja kozott
        /// </summary>
        public bool BajVan
        {
            get { return _bajvan; }
            set { _bajvan = value; }
        }
        private TablainfoCollection _bajtablainfo = new TablainfoCollection();
        /// <summary>
        /// Azon tablak Tablainfo-it gyujti, melyekkel 'baj' van
        /// A 'baj' attol lehet, hogy rosszkor tortent tablamodositas (vagy nem tortent, holott kellet volna)
        /// </summary>
        public Tablainfo BajTablaInfo
        {
            set
            {
                _bajtablainfo.Add(value);
                _bajvan = true;
            }
        }
        private string BajInfo
        {
            get
            {
                if (!_bajvan || _bajtablainfo.Count == 0)
                    return "";
                string szov = "";
                foreach (Tablainfo egyinfo in _bajtablainfo)
                {
                    string azontip = egyinfo.Azontip;
                    string szint = egyinfo.Szint;
                    string tablanev = egyinfo.Tablanev;
                    if (tablanev == "TARTAL")
                        szov += "TARTAL ";
                    else
                    {
                        switch (szint)
                        {
                            case "R":
                                szov += "Rendszerszinten ";
                                break;
                            case "U":
                                szov += "Userszinten ";
                                break;
                            default:
                                szov += _aktualcegnev + " cégnél ";
                                break;
                        }
                        szov += tablanev;
                    }
                    if (egyinfo.LeiroHibaszov == "")
                        szov += "\n";
                    else
                        szov += egyinfo.LeiroHibaszov + "\n";
                }
                CloseProgress();
                FakPlusz.MessageBox.Show("Struktúra változott\nElöbb új verziót kellett volna gyártani!\n  " + szov);
                return szov;
            }
        }
        private VerzioinfoCollection _verzioinfok;
        /// <summary>
        /// rendszer/user/alkalmazas verzioinformacioi
        /// </summary>
        public VerzioinfoCollection VerzioInfok
        {
            get { return _verzioinfok; }
        }
        private bool _eventtilt = false;
        /// <summary>
        /// Bizonyos programreszek elott, ahol az input-control elhagyasakor vegrehajtando
        /// funkciok (vagy egyeb funkciok) nem kivanatosak, ez true-ra allitando
        /// Ha egyeb funkcioknal (pl. TabControl Select eventjei) is hasznalni akarjuk, a lekerdezesrol
        /// nekunk kell gondoskodnunk
        /// </summary>
        public bool EventTilt
        {
            get { return _eventtilt; }
            set { _eventtilt = value; }
        }
        private ErrorProvider _errorprovider = new ErrorProvider();
        /// <summary>
        /// Az egesz rendszerben hasznalhato ErrorProvider
        /// </summary>
        public ErrorProvider ErrorProvider
        {
            get { return _errorprovider; }
        }
        private string _alkalmazas = "";
        /// <summary>
        /// A konkret alkalmazasi program azonositoja
        /// </summary>
        public string Alkalmazas
        {
            get { return _alkalmazas; }
        }
        private string _alkalmazasid = "";
        /// <summary>
        /// az Alkalmazasok kodtablaban a konkret alkalmazas Id-je
        /// </summary>
        public string AlkalmazasId
        {
            get { return _alkalmazasid; }
        }
        /// <summary>
        /// kell-e naplozni a valtozasokat
        /// </summary>
        public bool KellValtozas = true;

        /// <summary>
        /// Objectum letrehozasa
        /// </summary>
        /// <param name="alkalmazas">
        /// Az alkalmazas neve
        /// </param>
        /// <param name="form">
        /// Main Form
        /// </param>
        /// <param name="kellprogress">
        /// </param>
        /// <param name="connstringek">
        /// connection stringek tombje
        /// </param>
        /// <param name="adatbazisfajta">
        /// Sql vagy MySql
        /// </param>
        /// <param name="mainvezerles">
        /// a  hivo Vezerloinfo objectuma
        /// </param>
        /// <param name="kezeloid">
        /// a bejelentkezett kezelo id-je, TERVEZO eseten -1
        /// </param>
        public FakUserInterface(string alkalmazas, Form form, bool kellprogress, string[] connstringek, string adatbazisfajta, Vezerloinfo mainvezerles, int kezeloid)
        {
            NewFakUserInterface(alkalmazas, form, kellprogress, connstringek, adatbazisfajta, mainvezerles, kezeloid,false);
        }
        /// <summary>
        /// Ha az adatbazisunk Sql tipusu, ezt a parametert nem kell megadni
        /// </summary>
        /// <param name="alkalmazas"></param>
        /// <param name="form"></param>
        /// <param name="kellprogress"></param>
        /// <param name="connstringek"></param>
        /// <param name="mainvezerles"></param>
        /// <param name="kezeloid"></param>
        public FakUserInterface(string alkalmazas, Form form, bool kellprogress, string[] connstringek, Vezerloinfo mainvezerles, int kezeloid)
        {
            NewFakUserInterface(alkalmazas, form, kellprogress, connstringek, "Sql", mainvezerles, kezeloid,false);
        }
        public FakUserInterface(string alkalmazas, Form form, bool kellprogress, string[] connstringek, Vezerloinfo mainvezerles, int kezeloid,bool webe)
        {
            NewFakUserInterface(alkalmazas, form, kellprogress, connstringek, "Sql", mainvezerles, kezeloid, webe);
        }  
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tol"></param>
        /// <param name="ig"></param>
        public void SetUserSzamitasokDatumHatarok(DateTime tol, DateTime ig)
        {
            UserSzamitasok.UserSzamitasok.DatumHatarok(tol, ig);
        }
        /// <summary>
        /// 
        /// </summary>
        public bool KellSzamitasDatum
        {
            get { return UserSzamitasok.UserSzamitasok.KellSzamitasDatum; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="kelle"></param>
        public void SetUserSzamitasokKellSzamitasDatum(bool kelle)
        {
            UserSzamitasok.UserSzamitasok.KellSzamitasDatum = kelle;
        }
        /// <summary>
        /// meagadott szintu  tablainformaciok kerese
        /// </summary>
        /// <param name="szint">
        /// kivant szint vagy a kivant szinteket tartalmazo string (egy szint egy betu)
        /// </param>
        /// <returns>
        /// a megfelelo gyujtemeny vagy null
        /// </returns>
        public TablainfoCollection GetBySzint(string szint)
        {
            return _tablainfok.GetBySzint(szint);
        }
        /// <summary>
        /// tablainformacio keresese szint es tablanev szerint
        /// </summary>
        /// <param name="szint">
        /// A kivant szint (R/U/C/...)
        /// </param>
        /// <param name="tablanev">
        /// A kivant tablanev
        /// </param>
        /// <returns>
        /// A kivant tablainformacio vagy Null
        /// </returns>
        public Tablainfo GetBySzintPluszTablanev(string szint, string tablanev)
        {
            return _tablainfok.GetBySzintPluszTablanev(szint, tablanev);
        }
        /// <summary>
        /// tablainformaciok keresese tablanev szerint
        /// </summary>
        /// <param name="tablanev">
        /// A kivant tablanev
        /// </param>
        /// <returns>
        /// A parameternek megfelelo tablainformaciok
        /// </returns>
        public TablainfoCollection GetByTablanev(string tablanev)
        {
            return _tablainfok.GetByTablanev(tablanev);
        }
        /// <summary>
        /// Kodtabla tipusu tablainformacio keresese szint es kodtipus szerint
        /// </summary>
        /// <param name="szint">
        /// A kivant szint
        /// </param>
        /// <param name="kodtipus">
        /// A kivant kodtipus
        /// </param>
        /// <returns>
        /// A parameterekenek megfelelo tablainformacio vagy Null
        /// </returns>
        public Tablainfo GetKodtab(string szint, string kodtipus)
        {
            return _tablainfok.GetByAzontip("SZ" + szint + "K" + kodtipus);
        }
        /// <summary>
        /// Csoport tipusu tablainformacio keresese szint es kodtipus szerint
        /// </summary>
        /// <param name="szint">
        /// A kivant szint
        /// </param>
        /// <param name="kodtipus">
        /// A kivant kodtipus
        /// </param>
        /// <returns>
        /// A parameterekenek megfelelo tablainformacio vagy Null
        /// </returns>
        public Tablainfo GetCsoport(string szint, string kodtipus)
        {
            Tablainfo info = _tablainfok.GetByAzontip("SZ" + szint + "C" + kodtipus);
            if (info.Osszefinfo.InitKell)
                info.Osszefinfo.OsszefinfoInit();
            if (info.Osszefinfo.TolteniKell)
                info.Osszefinfo.Osszefinfotolt();
            return info;
        }
        /// <summary>
        /// Adatkozles tipusu tablainformacio keresese kodtipus szerint
        /// </summary>
        /// <param name="kodtipus">
        /// a kivant kodtipus
        /// </param>
        /// <returns>
        /// a kodtipusnak megfelelo tablainformacio vagy null
        /// </returns>
        public Tablainfo GetAdatkozl(string kodtipus)
        {
            return _tablainfok.GetByAzontip("SZRA" + kodtipus);
        }
        /// <summary>
        /// Osszefugges tipusu tablainformacio keresese szint es kodtipus szerint
        /// </summary>
        /// <param name="szint">
        /// A kivant szint
        /// </param>
        /// <param name="kodtipus">
        /// A kivant kodtipus
        /// </param>
        /// <returns>
        /// A parameterekenek megfelelo tablainformacio vagy Null
        /// </returns>
        public Tablainfo GetOsszef(string szint, string kodtipus)
        {
            Tablainfo info = _tablainfok.GetByAzontip("SZ" + szint + "O" + kodtipus);
            if (info == null)
                return null;
            if (info.Osszefinfo.InitKell)
                info.Osszefinfo.OsszefinfoInit();
            if (info.Osszefinfo.TolteniKell)
                info.Osszefinfo.Osszefinfotolt();
            return info;
        }
        /// <summary>
        /// Szukitett kodtabla tipusu tablainformacio keresese kodtipus szerint
        /// </summary>
        /// <param name="kodtipus">
        /// A kivant kodtipus
        /// </param>
        /// <returns>
        /// A parameterekenek megfelelo tablainformacio vagy Null
        /// </returns>
        public Tablainfo GetSzukitettKodtab(string kodtipus)
        {
            Tablainfo info = _tablainfok.GetByAzontip("SZCS" + kodtipus);
            if (info.Osszefinfo.InitKell)
                info.Osszefinfo.OsszefinfoInit();
            if (info.Osszefinfo.TolteniKell)
                info.Osszefinfo.Osszefinfotolt();
            return info;
        }
        /// <summary>
        /// Mezonev tipusu tablainformacio keresese kodtipus szerint
        /// </summary>
        /// <param name="kodtipus">
        /// A kivant kodtipus
        /// </param>
        /// <returns>
        /// A parameterekenek megfelelo tablainformacio vagy Null
        /// </returns>

        public Tablainfo GetMezonevek(string kodtipus)
        {
            return _tablainfok.GetByAzontip("SZRM" + kodtipus);
        }
        /// <summary>
        /// Teljes azonositoval megadott tablainformacio keresese
        /// </summary>
        /// <param name="azontip">
        /// A kivant azonosito
        /// </param>
        /// <returns>
        /// A parameternek megfelelo tablainformacio vagy Null
        /// </returns>
        public Tablainfo GetByAzontip(string azontip)
        {
            return _tablainfok.GetByAzontip(azontip);
        }
        /// <summary>
        /// Az osszes termeszetes tablainformacio keresese
        /// </summary>
        /// <returns>
        /// A termeszetes tablainformaciok tombje
        /// </returns>
        public Tablainfo[] GetTermTablaInfok()
        {
            return _tablainfok.GetByTermszarm("T ");
        }
        /// <summary>
        /// Az osszes ceg ill. ceg alatti szintu termeszetes tablainformacio keresese
        /// </summary>
        /// <returns>
        /// a kivant collection
        /// </returns>
        public TablainfoCollection GetCegPluszCegalattiTermTablaInfok()
        {
            return _tablainfok.GetCegPluszCegalattiTermTablaInfok();
        }
        /// <summary>
        /// Egy tablainfo adattablajanak osszes sorabol a kivant nevu column tartalmainak tombjet kivanjuk
        /// </summary>
        /// <param name="tabinfo">
        /// a tablainformacio
        /// </param>
        /// <param name="kivantadatnev">
        /// a column neve
        /// </param>
        /// <returns>
        /// a tartalmak tombje
        /// </returns>
        public string[] GetTartal(Tablainfo tabinfo, string kivantadatnev)
        {
            return tabinfo.Adattabla.GetTartal(kivantadatnev, "", "");
        }
        /// <summary>
        /// Egy tablainfo adattablajanak azon sorabol,ahol a DATUMTOL es DATUMIG oszlop tartalma adott intervallumba esik,
        /// a kivant nevu column tartalmat kivanjuk
        /// </summary>
        /// <param name="tabinfo">
        /// a tablainformacio
        /// </param>
        /// <param name="kivantadatnev">
        /// a kivant column neve
        /// </param>
        /// <param name="datumtolig">
        /// a datumintervallum
        /// </param>
        /// <returns>
        /// a kivant tartalom vagy ""
        /// </returns>
        public string[] GetTartalByDatumTolIg(Tablainfo tabinfo, string kivantadatnev, DateTime[] datumtolig)
        {
            return tabinfo.Adattabla.GetTartal(kivantadatnev, datumtolig);
        }
        /// <summary>
        /// Egy tablainfo adattablajanak osszes sorabol azon sorok kivant nevu oszlopanak tartalmait 
        /// akarjuk, mely sorokban egy masik oszlop tartalma adott erteku
        /// </summary>
        /// <param name="tabinfo">
        /// a tablainformacio
        /// </param>
        /// <param name="kivantadatnev">
        /// a kivant column neve
        /// </param>
        /// <param name="tartalnev">
        /// azon column neve, melynek tartalmat vizsgalni kell
        /// </param>
        /// <param name="ertek">
        /// a vizsgalando column kivant erteke
        /// </param>
        /// <returns>
        /// a kivant column (vizsgalatnak megfelelo) tartalmainak tombje
        /// </returns>
        public string[] GetTartal(Tablainfo tabinfo, string kivantadatnev, string tartalnev, string ertek)
        {
            return tabinfo.Adattabla.GetTartal(kivantadatnev, tartalnev, ertek);
        }
        /// <summary>
        /// Egy tablainfo adattablajanak osszes sorabol azon sorok kivant nevu oszlopanak tartalmait 
        /// akarjuk, mely sorokban egy masik oszlop tartalma megfelel az ertek[] parameter valamelyikenek
        /// </summary>
        /// <param name="tabinfo">
        /// a tablainformacio
        /// </param>
        /// <param name="kivantadatnev">
        /// a kivant column neve
        /// </param>
        /// <param name="tartalnev">
        /// azon column neve, melynek tartalmat vizsgalni kell
        /// </param>
        /// <param name="ertek">
        /// a vizsgalando column kivant ertekeinek tombje
        /// </param>
        /// <returns>
        /// a kivant column (vizsgalatnak megfelelo) tartalmainak tombje
        /// </returns>
        public string[] GetTartal(Tablainfo tabinfo, string kivantadatnev, string tartalnev, string[] ertek)
        {
            return tabinfo.Adattabla.GetTartal(kivantadatnev, tartalnev, ertek);
        }
        /// <summary>
        /// Termeszetes adattabla toltese id alapjan 
        /// </summary>
        /// <param name="info">
        /// toltendo adattabla tablainformacioja
        /// </param>
        public void AdattoltByAktid(Tablainfo info)
        {
            info.SelectString = " where " + info.IdentityColumnName + "='" + info.AktIdentity + "'";
            info.Adattolt(Aktintervallum, true);
            info.Tartalmaktolt();
        }
        public void OsszesAdattoltByParent(string parenttablanev, bool kellkezdetvegfigyeles)
        {
            OsszesAdattoltByParent(parenttablanev, null, null, kellkezdetvegfigyeles);
        }
        /// <summary>
        /// Termeszetes Child adattablak toltese parent id alapjan
        /// </summary>
        /// <param name="parenttablanev">
        /// a parent tabla neve
        /// </param>
        public void OsszesAdattoltByParent(string parenttablanev)
        {
            OsszesAdattoltByParent(parenttablanev, null, null,false);
        }
        public void OsszesAdattoltByParent(string parenttablanev, string[] specnevek, string[] specdatumok)
        {
            OsszesAdattoltByParent(parenttablanev, specnevek, specdatumok, false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parenttablanev"></param>
        /// <param name="specnevek"></param>
        /// <param name="specdatumok"></param>
        public void OsszesAdattoltByParent(string parenttablanev, string[] specnevek, string[] specdatumok,bool kellkezdetvegfigyeles)
        {
            Tablainfo info = _gyokertablainfok[parenttablanev];
            if (info == null)
                info = ((Tablainfo[])GetByTablanev(parenttablanev).ToArray(typeof(Tablainfo)))[0];
            string selwhere = " where " + info.IdentityColumnName + "='" + info.AktIdentity + "'";
            AdattoltByParent(info, selwhere, specnevek, specdatumok,kellkezdetvegfigyeles);
        }
        private string GetSelWhere(Tablainfo childinfo, string specnev, string[] specdatumok)
        {
            string selwhere = "";
            ArrayList ar = new ArrayList(childinfo.SpecDatumNevek);
            if (ar.Contains(specnev))
                selwhere = " and " + specnev + " <= '" + specdatumok[1] + "' and " + specnev + " >= '" + specdatumok[0] + "'";
            return selwhere;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="kozosselwhere"></param>
        /// <param name="specnevek"></param>
        /// <param name="specdatumok"></param>
        private void AdattoltByParent(Tablainfo parent, string kozosselwhere, string[] specnevek, string[] specdatumok,bool kellkezdetvegfigyeles)
        {

            foreach (Tablainfo egychild in parent.TermChildTabinfo)
            {
                string kezdetefeltetel = "";
                if (kellkezdetvegfigyeles && egychild.TablaColumns["KEZDETE"]!=null)        
                {
                              kezdetefeltetel = "(KEZDETE IS NULL OR KEZDETE <= '" + DatumToString(Aktintervallum[1]) +
                                 "') AND (VEGE IS NULL  OR VEGE >= '" + DatumToString(Aktintervallum[0]) + "') ";
                }
                string selwhere = "";
                ArrayList specnevar = new ArrayList();
                if (specnevek != null)
                {
                    if (specnevek.Length < specdatumok.Length)
                        selwhere = GetSelWhere(egychild, specnevek[0], specdatumok);
                    else
                    {
                        specnevar = new ArrayList(specnevek);
                        bool elso = true;
                        bool voltmar = false;
                        for (int i = 0; i < egychild.SpecDatumNevek.Length; i++)
                        {
                            int j = specnevar.IndexOf(egychild.SpecDatumNevek[i]);
                            if (j != -1)
                            {
                                if (elso)
                                {
                                    selwhere += " and (";
                                    elso = false;
                                }
                                if (voltmar)
                                    selwhere += " or ";
                                selwhere += specnevek[j] + " ='" + specdatumok[j] + "'";
                                voltmar = true;
                            }
                        }
                        if (!elso)
                            selwhere += ")";
                    }
                }

                egychild.SelectString = kozosselwhere + selwhere;
                if (kezdetefeltetel != "")
                    egychild.SelectString += " AND " + kezdetefeltetel;
                egychild.Adattolt(Aktintervallum, true);
                egychild.Tartalmaktolt();
                if (egychild.DataView.Count == 0)
                    selwhere = " where "+ egychild.IdentityColumnName + " = -1";
                else
                {
                    selwhere = "";
                    for (int i = 0; i < egychild.DataView.Count; i++)
                    {
                        egychild.AktIdentity = Convert.ToInt64(egychild.DataView[i].Row[egychild.IdentityColumnName].ToString());
                        if (selwhere == "")
                            selwhere = " where (";
                        else
                            selwhere += " OR ";
                        selwhere += egychild.IdentityColumnName + " = " + egychild.AktIdentity.ToString();
                        if (i == egychild.DataView.Count - 1)
                            selwhere += ")";
                    }
                }
                AdattoltByParent(egychild, selwhere, specnevek, specdatumok, kellkezdetvegfigyeles);
           }
        }
        /// <summary>
        /// Termeszetes Child adattablak toltese parent id alapjan, majd a parent tabla torlese
        /// </summary>
        /// <param name="parenttablanev">
        /// a parent tabla neve
        /// </param>
        public void OsszesAdattorolByParent(string parenttablanev)
        {
            Tablainfo info = _gyokertablainfok[parenttablanev];
            if (info == null)
                info = ((Tablainfo[])GetByTablanev(parenttablanev).ToArray(typeof(Tablainfo)))[0];
            AdattorolByParent(info);
            info.TeljesTorles();
        }
        /// <summary>
        /// Termeszetes child adattablak torlese parent tabinfo alapjan, majd a parent view aktualis soranak torlese
        /// </summary>
        /// <param name="info">
        /// tablainfo
        /// </param>
        public void OsszesAdattorolByParent(Tablainfo info)
        {
            AdattorolByParent(info);
            info.Adatsortorol(info.ViewSorindex);
        }
        /// <summary>
        /// Belso hasznalatra
        /// </summary>
        /// <param name="parent"></param>
        private void AdattorolByParent(Tablainfo parent)
        {
            foreach (Tablainfo egychild in parent.TermChildTabinfo)
            {
                AdattorolByParent(egychild);
                egychild.TeljesTorles();
            }
        }
        /// <summary>
        /// string kiegeszitese space-ekkel jobbrol kivant hosszra
        /// </summary>
        /// <param name="mezo">
        /// eredeti string
        /// </param>
        /// <param name="hossz">
        /// kivant hossz
        /// </param>
        /// <returns>
        /// kiegeszitett string
        /// </returns>
        public string SpaceFillFromRight(string mezo, int hossz)
        {
            string mez = mezo;
            if (mez.Length < hossz)
            {
                int h = hossz;
                do
                {
                    mez += " ";
                } while (mez.Length < h);
            }
            return mez;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="datum"></param>
        /// <returns></returns>
        public string DatumToString(DateTime datum)
        {
            string dat = datum.ToShortDateString();
            if (dat.EndsWith("."))
                dat = dat.Substring(0, dat.Length - 1);
            return dat;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="datum"></param>
        /// <returns></returns>
        public DateTime EvDatum(DateTime datum)
        {
            string evdatum = datum.Year.ToString() + ".01.01";
            return Convert.ToDateTime(evdatum);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="datum"></param>
        /// <returns></returns>
        public DateTime HoDatum(DateTime datum)
        {
            string hodatum = DatumToString(datum).Substring(0, 8) + "01";
            return Convert.ToDateTime(hodatum);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="datum"></param>
        /// <returns></returns>
        public DateTime HoVegDatum(DateTime datum)
        {
            DateTime kovho = datum.AddMonths(1);
            string kovhostr = DatumToString(kovho).Substring(0, 8) + "01";
            return Convert.ToDateTime(kovhostr).AddDays(-1);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="kisebbdatum"></param>
        /// <param name="nagyobbdatum"></param>
        /// <returns></returns>
        public string DatumokKulonbsegeEvHoNap(DateTime kisebbdatum, DateTime nagyobbdatum)
        {
            if(kisebbdatum.CompareTo(nagyobbdatum)>=0)
                return( "   0. 0. 0");
            int ev = kisebbdatum.Year;
            int ho = kisebbdatum.Month;
            int nap = kisebbdatum.Day;

            int ujev = nagyobbdatum.Year;
            string ujevstr = ujev.ToString();
            int ujho = nagyobbdatum.Month;
            string ujhostr = ujho.ToString();
            int ujnap = nagyobbdatum.Day;
            if (ujhostr.Length == 1)
                ujhostr = "0" + ujhostr;
            DateTime date1 = Convert.ToDateTime(ujevstr + "." + ujhostr + ".01").AddDays(-1);
            int lastday = date1.Day;
            ujev = ujev - ev;
            ujho = ujho - ho;
            ujnap = ujnap - nap;
            if (ujho < 0 || ujho == 0 && ujnap <=0)
            {
                ujev = ujev - 1;
                ujho += 12;
            }
            if (ujnap <= 0)
            {
                ujho = ujho - 1;
                if (ujho == 0)
                    ujnap += 31;
                else
                    ujnap += lastday;
            }
            ujevstr = SpaceFillFromLeft(ujev.ToString(),4)+".";
            ujhostr = SpaceFillFromLeft(ujho.ToString(),2)+".";
            string ujnapstr = SpaceFillFromLeft(ujnap.ToString(),2);
            return ujevstr + ujhostr + ujnapstr;

        }
        /// <summary>
        /// string kiegeszitese space-ekkel balrol kivant hosszra
        /// </summary>
        /// <param name="mezo">
        /// eredeti string
        /// </param>
        /// <param name="hossz">
        /// kivant hossz
        /// </param>
        /// <returns>
        /// kiegeszitett string
        /// </returns>
        public string SpaceFillFromLeft(string mezo, int hossz)
        {
            string mez = mezo;
            if (mez.Length < hossz)
            {
                int h = hossz;
                do
                {
                    mez = " " + mez;
                } while (mez.Length < h);
            }
            return mez;
        }
        /// <summary>
        /// Input Control- ba adott ertek bevitele. 
        /// Olyan esetekre, mikor az Input Control-nak nemcsak adatbevitellel akarunk erteket adni
        /// </summary>
        /// <param name="cont">
        /// Input Control
        /// </param>
        /// <param name="ertek">
        /// kivant ertek
        /// </param>
        /// <returns>
        /// true : a kivant ertek bevitele megtortent
        /// false: a kivant ertek bevitele hibat okoz
        /// </returns>
        public bool SetValue(Control cont, string ertek)
        {
            try
            {
                MezoTag tag = (MezoTag)cont.Tag;
                return tag.SetValue(ertek);
            }
            catch { return true; }
        }
        /// <summary>
        /// Input Control tartalma hibas, hibaszoveg beallitas.
        /// Olyan esetekben hasznalando, mikor tobb InputControl tartalma egyuttesen vizsgalva lehet hibas
        /// </summary>
        /// <param name="cont">
        /// Input Control
        /// </param>
        /// <param name="szov">
        /// kivant hibaszoveg
        /// </param>
        public void SetHibaszov(Control cont, string szov)
        {
            ((MezoTag)cont.Tag).SetHibaSzov(szov);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="kod"></param>
        /// <returns></returns>
        public string GetUzenetSzoveg(string kod)
        {
            Tablainfo uzenetinfo = GetBySzintPluszTablanev("R", "UZENETEK");
            string[] tartal = uzenetinfo.Adattabla.GetTartal("SZOVEG", "KOD", kod);
            if (tartal == null)
                return "";
            else
                return tartal[0];
        }
        /// <summary>
        /// Egyenlore nem tudom, kell-e
        /// </summary>
        public void VerzioHelyreAllit()
        {
        }
        /// <summary>
        /// Teljes verziotorles valamely adatbazisra vonatkozoan (rendszer/user/alkalmazas(ceg)
        /// </summary>
        /// <param name="tabinfo">
        /// a kivant adatbazis verziotablajanak tablainformacioja
        /// </param>
        public void DeleteVersionAll(Tablainfo tabinfo)
        {
            string szint = tabinfo.Szint;
            ArrayList rendszermodositandok = new ArrayList();
            ArrayList usermodositandok = new ArrayList();
            ArrayList cegmodositandok = new ArrayList();
            Tablainfo leirotabinfo;
            int verindex = _verzioinfok.IndexOf(szint);
            _verzioinfok[verindex].Delete = true;
            string verz = _verzioinfok[verindex].VersionArray[_verzioinfok[verindex].VersionArray.Length - 1].ToString();
            TablainfoCollection verztabinfo = _tablainfok;
            OpenProgress();
            foreach (Tablainfo egytabinfo in verztabinfo)
            {
                ProgressRefresh();
                leirotabinfo = egytabinfo.TablaTag.LeiroTablainfo;
                if (egytabinfo.Szint == szint || szint == "R" && egytabinfo.Tablanev == "TARTAL")
                {
                    if (egytabinfo.DeleteLastVersion(verz))
                    {
                        if (egytabinfo.Tablanev == "TARTAL")
                            rendszermodositandok.Add(egytabinfo);
                        else
                        {
                            switch (egytabinfo.Szint)
                            {
                                case "R":
                                    rendszermodositandok.Add(egytabinfo);
                                    break;
                                case "U":
                                    usermodositandok.Add(egytabinfo);
                                    break;
                                case "C":
                                    cegmodositandok.Add(egytabinfo);
                                    break;
                            }
                        }
                        if (egytabinfo.VerzioTerkepArray.Count == 0 && leirotabinfo != null)
                        {
                            if (leirotabinfo.DeleteLastVersion(verz))
                                rendszermodositandok.Add(leirotabinfo);
                        }
                    }
                }
            }
            tabinfo.Adattabla.Adatsortorol(tabinfo.DataView.Count - 1);
            Tablainfo[] tabinf = null;
            switch (verindex)
            {
                case 0:
                    rendszermodositandok.Add(tabinfo);
                    break;
                case 1:
                    usermodositandok.Add(tabinfo);

                    break;
                case 2:
                    cegmodositandok.Add(tabinfo);
                    break;
            }
            if (rendszermodositandok.Count != 0)
            {
                tabinf = (Tablainfo[])rendszermodositandok.ToArray(typeof(Tablainfo));
                UpdateTransaction(tabinf);
            }
            if (usermodositandok.Count != 0)
            {
                tabinf = (Tablainfo[])usermodositandok.ToArray(typeof(Tablainfo));
                UpdateTransaction(tabinf);
            }
            if (cegmodositandok.Count != 0)
            {
                tabinf = (Tablainfo[])cegmodositandok.ToArray(typeof(Tablainfo));
                UpdateTransaction(tabinf);
            }
            CloseProgress();
        }
        public void CreateVersionAll()               // cegszintu tablakra
        {
            OpenProgress();
            ArrayList cegmodositandok = new ArrayList();
            int verindex = _verzioinfok.IndexOf("C");
            Verzioinfok verinf = _verzioinfok[verindex];
            for(int i=0;i< _tablainfok.Count;i++)
            {
                ProgressRefresh();
                Tablainfo egyinfo = _tablainfok[i];
                if (egyinfo.Szint == "C" && egyinfo.Tablanev!="TARTAL" && egyinfo.KellVerzio && egyinfo.LastVersionId < verinf.LastVersionId)
                    egyinfo.CreateNewVersion();
            }
            CloseProgress();
        }
        /// <summary>
        /// Adott tabla update
        /// </summary>
        /// <param name="info">
        /// kivant tabla tablainformacioja
        /// </param>
        /// <returns>
        /// true: sikeres Update
        /// </returns>
        public bool Rogzit(Tablainfo info)
        {
            return (UpdateTransaction(new Tablainfo[] { info }));
        }
        /// <summary>
        /// tablak update-je
        /// </summary>
        /// <param name="info">
        /// kivant tablak tablainformacio array
        /// Vigyazz! egy adatbazisban legyenek
        /// </param>
        /// <returns>
        /// true: sikeres update
        /// </returns>
        public bool Rogzit(Tablainfo[] info)
        {
            return (UpdateTransaction(info));
        }
        /// <summary>
        /// Termeszetes tabla es Child-tablak (es azoknak Child-tablai...) update-je
        /// </summary>
        /// <param name="info">
        /// kivant tablainformacio
        /// </param>
        /// <returns>
        /// true: sikeres update
        /// </returns>
        public bool RogzitAll(Tablainfo info)
        {
            Tablainfo[] rogzitinfo = GetAllChildinfo(info);
            return (UpdateTransaction(rogzitinfo));
        }
        /// <summary>
        /// megadott tablainformacio Child-tablai(es azoknak Child-tablai...) bekerese
        /// </summary>
        /// <param name="info">
        /// kivant tablainformacio
        /// </param>
        /// <returns>
        /// a kivant tablainformacio es Child-tablai informacioinak tombje 
        /// </returns>
        private Tablainfo[] GetAllChildinfo(Tablainfo info)
        {
            ArrayList ar = GetAllChildinfo(info, new ArrayList());
            if (ar.Count != 0)
                return (Tablainfo[])ar.ToArray(typeof(Tablainfo));
            else
                return null;
        }
        /// <summary>
        /// Belso hasznalatra
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ar"></param>
        /// <returns></returns>
        private ArrayList GetAllChildinfo(Tablainfo info, ArrayList ar)
        {
            ar.Add(info);
            Tablainfo[] childinfo = (Tablainfo[])info.TermChildTabinfo.ToArray(typeof(Tablainfo));
            if (childinfo == null)
                return ar;
            else
            {
                foreach (Tablainfo egytabinfo in childinfo)
                {
                    ArrayList reszar = GetAllChildinfo(egytabinfo, new ArrayList());
                    ar.AddRange(reszar);
                }
                return ar;
            }
        }
        ///<summary>
        /// Feladata MezoControlInfoCollection eloallitasa, ez MezoControlInfo-kbol all.
        /// Egy MezoControlInfo egy input Control szukseges informacioit tartalmazza.
        /// Alkalmazasi programokbol altalaban ezt hivjuk.
        /// </summary>
        /// <param name="hivo">
        /// a UserControl, amely szamara a collection-t eloallitjuk
        /// </param>
        /// <param name="cont">
        /// Azon Control(-ok) tombje, mely(ek)ben az input Control-(oka)t felvettuk. Altalaban GroupBox-ok tombje
        /// </param>
        /// <param name="aktivpage">
        /// Az a TabPage, melyben a UserControl fut (vagy null, ha nem )
        /// </param>
        /// <param name="aktivmenuitem">
        /// Az ToolStripMenuItem, mely (vagy melynek valamely dropiteme) a UserControl-t aktivizalja (vagy null)
        /// </param>
        /// <param name="aktivdropdownitem">
        /// Az aktivizalo dropitem (vagy null)
        /// </param>
        /// <returns>
        /// A letrehozott collection
        /// </returns>
        public MezoControlCollection ControlTagokTolt(Base hivo, Control[] cont, TabPage aktivpage, ToolStripMenuItem aktivmenuitem, ToolStripMenuItem aktivdropdownitem)
        {
            Tablainfo tabinfo;
            ArrayList tabinfoar = new ArrayList();
            MezoControlCollection coll = new MezoControlCollection();
            char[] vesszo = new char[] { Convert.ToChar(",") };
            foreach (Control egycont in cont)
            {
                bool vesszopoz = egycont.Tag.ToString().Contains(",");
                if (vesszopoz)
                {
                    string[] split = egycont.Tag.ToString().Split(vesszo);
                    tabinfo = _tablainfok.GetBySzintPluszTablanev(split[0], split[1]);
                }
                else
                    tabinfo = _tablainfok.GetByAzontip(egycont.Tag.ToString());
                egycont.Tag = tabinfo;
                if (tabinfoar.IndexOf(tabinfo) == -1)
                    tabinfoar.Add(tabinfo);
                MezoControlInfo egyinfo = ControlTagokTolt(hivo, egycont, ref tabinfo, aktivpage, aktivmenuitem, aktivdropdownitem);
                egyinfo.ParentControl = egycont;
                coll.Add(ControlTagokTolt(hivo, egycont, ref tabinfo, aktivpage, aktivmenuitem, aktivdropdownitem));
            }
            foreach (Tablainfo egytabinfo in tabinfoar)
            {
                egytabinfo.AktualControlInfo = coll[egytabinfo];
            }
            return coll;
        }
        /// <summary>
        /// Feladata MezoControlInfoCollection eloallitasa, ez MezoControlInfo-kbol all.
        /// Egy MezoControlInfo egy input Control szukseges informacioit tartalmazza.
        /// A TERVEZO ezt hasznalja.
        /// </summary>
        /// <param name="hivo">
        /// a UserControl, amely szamara a collection-t eloallitjuk
        /// </param>
        /// <param name="control">
        /// panel2  (az Alap UserControl-ban definialt, ez tartalmazza a dataGridView1-et)
        /// </param>
        /// <param name = "info">
        /// az a tablainformacio, melyet a UserControl kezel
        /// </param>
        /// <param name="aktivpage">
        /// Az a TabPage, melyben a UserControl fut
        /// </param>
        /// <param name="aktivmenuitem">
        /// A TERVEZO-ben null
        /// </param>
        /// <param name="aktivdropitem">
        /// A TERVEZO-ben null
        /// </param>
        /// <returns>
        /// az eloallitott collection
        /// </returns>
        public MezoControlInfo ControlTagokTolt(Base hivo, Control control, ref Tablainfo info, TabPage aktivpage, ToolStripMenuItem aktivmenuitem, ToolStripMenuItem aktivdropitem)
        {
            Tablainfo tabinfo = info;
            if (info == null)
            {
                char[] vesszo = new char[] { Convert.ToChar(",") };
                bool vesszopoz = control.Tag.ToString().Contains(",");
                if (vesszopoz)
                {
                    string[] split = control.Tag.ToString().Split(vesszo);
                    tabinfo = _tablainfok.GetBySzintPluszTablanev(split[0], split[1]);
                }
                else
                    tabinfo = _tablainfok.GetByAzontip(control.Tag.ToString());
                control.Tag = tabinfo;
            }
            Control.ControlCollection Controls = control.Controls;
            control.Tag = tabinfo;
            MezoControlInfo egycontinfo = null;
            MezoControlCollection egycontarr = info.ControlInfok;
            if (egycontarr.Count != 0)
            {
                egycontinfo = egycontarr[hivo];
                if (egycontinfo != null)
                {
                    if (egycontinfo.Tabinfo != info || egycontinfo.InputelemArray.Count == 0)
                    {
                        egycontarr.Remove(egycontinfo);
                        info.ControlInfok.Remove(egycontinfo);
                        egycontinfo = null;
                    }
                }
            }
            if (egycontinfo != null)
                return egycontinfo;
            egycontinfo = CreateControlinfo(tabinfo, hivo);
            tabinfo.AktualControlInfo = egycontinfo;
            MezotagCollection controlok = egycontinfo.InputelemArray;
            for (int i = 0; i < Controls.Count; i++)
            {
                cbtbinfotolt(hivo, egycontinfo, Controls[i], tabinfo, controlok, aktivpage, null, null);
                tabinfo.ControlInfok.Add(egycontinfo);
            }
            for (int i = 0; i < controlok.Count; i++)
            {
                MezoTag egytag = controlok[i];
                if (i == 0)
                    egytag.First = true;
                else if (i == controlok.Count - 1)
                    egytag.Last = true;
                egytag.OsszesTag = egycontinfo.Inputeleminfok;
            }
            return egycontinfo;
        }
        /// <summary>
        /// tablainformacio szamara MezoConrolInfo-t allit elo, belso hasznalatra
        /// </summary>
        /// <param name="tabinfo">
        /// a tablainformacio
        /// </param>
        /// <param name="hivo">
        /// a hivo UserControl
        /// </param>
        /// <returns>
        /// a MezoControlInfo
        /// </returns>
        private MezoControlInfo CreateControlinfo(Tablainfo tabinfo, Base hivo)
        {
            MezoControlInfo egyc = new MezoControlInfo(hivo, tabinfo);
            return egyc;
        }

        /// <summary>
        /// Belso hasznalatra
        /// Vegul is minden ControlTagokTolt itt kot ki, ez vegzi egy-egy inputControl mezoinformaciojanak letrehozasat es a collection-be fuzeset
        /// </summary>
        private void cbtbinfotolt(Base hivo, MezoControlInfo egycontinfo, Control control, Tablainfo tabinfo, MezotagCollection Controlok, TabPage aktivpage, ToolStripMenuItem aktivmenuitem, ToolStripMenuItem aktivdropitem)
        {
            EventTilt = true;
            egycontinfo.ParentControl = control.Parent;
            MezotagCollection controlok = Controlok;
            TextBox tb;
            FormattedTextBox.FormattedTextBox ftb;
            ComboBox cb;
            DateTimePicker pk;
            Label lb;
            MezoTag egytag = null;
            Cols egycol;
            Cols egyinp;
            DataGridView gw;
            int numind;
            string egytagstring = "";
            bool talalt = false;
            if (control.Tag != null)
            {
                try
                {
                    MezoTag tag = (MezoTag)control.Tag;
                    egytagstring = tag.Mezonev;
                }
                catch
                {
                    egytagstring = control.Tag.ToString();
                }
            }
            string nevi = control.GetType().FullName;
            for (int i = 0; i < _fullnevek.Length; i++)
            {
                if (_fullnevek[i] == nevi)
                {
                    talalt = true;
                    numind = i;
                    if (egytagstring != "" || i == 7)
                    {
                        if (i == 7) // DataGridView
                        {
                            gw = (DataGridView)control;
                            egycontinfo.DataGridView = gw;
                            egytag = new MezoTag(tabinfo, "", this, egycontinfo, aktivpage, aktivmenuitem, aktivdropitem, gw);
                            tabinfo.Adattabla.GridView = gw;
                            egytag.Hivo = hivo;
                            egytag.Elemindex = i;
                            control.Tag = egytag;
                            egytag.Control = control;
                            egytag.Controltipus = _nevek[i];
                            if (gw.ReadOnly)
                                gw.Columns.AddRange(tabinfo.CreateGridViewColumns(true));
                            else
                            {
                                egytag.Control.Enabled = false;
                                //egytag.DataGridView.CellEnter += new DataGridViewCellEventHandler(DataGridView_CellEnter);
                                //egytag.DataGridView.CellLeave += new DataGridViewCellEventHandler(DataGridView_CellLeave);
                                //egytag.DataGridView.UserAddedRow += new DataGridViewRowEventHandler(DataGridView_UserAddedRow);
                                //egytag.DataGridView.UserDeletingRow += new DataGridViewRowCancelEventHandler(DataGridView_UserDeletingRow);
                                //                                egytag.DataGridView.Scroll += new ScrollEventHandler(DataGridView_Scroll);

                                for (int j = 0; j < egytag.DataGridView.ColumnCount; j++)
                                {
                                    DataGridViewColumn dgcol = (DataGridViewColumn)egytag.DataGridView.Columns[j];
                                    if (!dgcol.ReadOnly && dgcol.Visible)
                                    {
                                        MezoTag egytag1 = new MezoTag(tabinfo, dgcol.Name, this, egycontinfo, aktivpage, aktivmenuitem, aktivdropitem);
                                        egytag1.Hivo = hivo;
                                        controlok.Add(egytag1);
                                        try
                                        {
                                            DataGridViewTextBoxColumn tbcol = (DataGridViewTextBoxColumn)dgcol;
                                            egytag1.AktivCell = new DataGridViewTextBoxCell();
                                            egytag1.AktivCell.Style.BackColor = _aktivinputbackcolor;
                                            egytag1.AktivCell.Style.Font = _aktivinputfont;
                                            egytag1.TextBoxColumn = tbcol;
                                            egytag1.Controltipus = "TextBoxCell";
                                        }
                                        catch
                                        {
                                            try
                                            {
                                                DataGridViewCheckBoxColumn cbcol = (DataGridViewCheckBoxColumn)dgcol;
                                                egytag1.AktivCell = new DataGridViewCheckBoxCell();
                                                egytag1.AktivCell.Style.BackColor = _aktivinputbackcolor;
                                                egytag1.AktivCell.Style.Font = _aktivinputfont;
                                                egytag1.CheckBoxColumn = cbcol;
                                                egytag1.Controltipus = "CheckBoxCell";
                                            }
                                            catch
                                            {
                                                DataGridViewComboBoxColumn combocol = (DataGridViewComboBoxColumn)dgcol;
                                                egytag1.AktivCell = new DataGridViewComboBoxCell();
                                                egytag1.AktivCell.Style.BackColor = _aktivinputbackcolor;
                                                egytag1.AktivCell.Style.Font = _aktivinputfont;
                                                egytag1.ComboBoxColumn = combocol;
                                                egytag1.Controltipus = "ComboBoxCell";
                                            }
                                        }

                                        dgcol.Tag = egytag1;
                                        egytag1.Control = null;
                                        egytag1.ParentGrid = egytag.DataGridView;
                                        egytag1.ParentView = egytag.DataView;
                                    }
                                }
                            }
                        }
                        else
                        {
                            egycol = tabinfo.TablaColumns[egytagstring];
                            if (egycol.Comboe && egycol.Csakolvas)
                                egyinp = egycol;
                            else
                                egyinp = tabinfo.InputColumns[egytagstring];
                            egytag = new MezoTag(tabinfo, egytagstring, this, egycontinfo, aktivpage, aktivmenuitem, aktivdropitem);
                            egytag.Elemindex = i;
                            egytag.Hivo = hivo;
                            control.Tag = egytag;
                            if (control.Enabled)
                            {
                                control.Enter += new EventHandler(Elem_Enter);
                                control.Leave += new EventHandler(Elem_Leave);
                            }
                            egytag.Control = control;
                            egytag.ParentControl = control.Parent;
                            egytag.Controltipus = _nevek[i];
                            controlok.Add(egytag);
                            switch (i)
                            {
                                case 1:
                                    ftb = (FormattedTextBox.FormattedTextBox)control;
                                    if (egycol.Numeric(egycol.DataType))
                                    {
                                        int tized = egycol.Tizedesek;
                                        switch (tized)
                                        {
                                            case 0:
                                                ftb.Format = "N0";
                                                break;
                                            case 1:
                                                ftb.Format = "N1";
                                                break;
                                            case 3:
                                                ftb.Format = "N3";
                                                break;
                                            case 4:
                                                ftb.Format = "N4";
                                                break;
                                            default:
                                                ftb.Format = "N2";
                                                break;
                                        }
                                    }
                                    else
                                        ftb.Format = egycol.Format;
                                    break;
                                case 5:
                                    pk = (DateTimePicker)control;
                                    pk.Value = DateTime.Today;
                                    if (egyinp == null)
                                        pk.Enabled = false;
                                    break;
                                case 6:
                                    lb = (Label)control;
                                    lb.Text = egycol.Sorszov;
                                    break;
                            }
                            if (!egycol.Inputlathato)
                                control.Visible = false;
                            if (egyinp == null)
                                control.Enabled = false;
                            else
                            {
                                switch (i)
                                {
                                    case 0:
                                        tb = (TextBox)control;
                                        if (egyinp.InputMaxLength > 0)
                                            tb.MaxLength = egyinp.InputMaxLength;
                                        break;
                                    case 1:
                                        ftb = (FormattedTextBox.FormattedTextBox)control;
                                        if (egyinp.InputMaxLength > 0)
                                            ftb.MaxLength = egyinp.InputMaxLength;
                                        break;
                                    case 2:
                                        cb = (ComboBox)control;
                                        cb.SelectionLength = 0;
                                        cb.Text = egyinp.ComboAktSzoveg;
                                        cb.Items.Clear();
                                        egycol.Combo_Info.SetComboItems(cb, egycol, egycol.Tartalom);
                                        if (egycol.Combo_Info.Szovegbe.Length != 0)
                                            cb.DropDownWidth = egycol.Combo_Info.Maxhossz * 9;
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            if (!talalt && control.Controls.Count != 0)
            {
                for (int j = 0; j < control.Controls.Count; j++)
                {
                    cbtbinfotolt(hivo, egycontinfo, control.Controls[j], tabinfo, controlok, aktivpage, aktivmenuitem, aktivdropitem);
                    egycontinfo.ParentControl = control.Controls[j].Parent;
                }
            }
            EventTilt = false;
        }
        /// <summary>
        /// Ha egy teljes combovalasztek bizonyos feltetelek szerint szukitendo,a szuroinfo alapjan
        /// elvegzi ezt a szukitest
        /// </summary>
        /// <param name="combo">
        /// ComboBox
        /// </param>
        /// <param name="szuroinfo">
        /// Azon tartalmak tombje melyek a tablaba iraskor elofordulhatnak
        /// </param>
        /// <returns>
        /// true: szures rendben
        /// false: ha a ComboBox nincs bemutatva, azaz nem szerepelt egy ControlTagokTolt-ben, vagy a ComboBox-hoz
        /// tartozo mezonev a leirasok szerint CsakOlvas-hato
        /// </returns>
        public bool Comboinfoszures(ComboBox combo, string[] szuroinfo)
        {
            if (combo.Tag == null)
                return false;
            return ((MezoTag)combo.Tag).Comboinfoszures(szuroinfo);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="combo"></param>
        /// <param name="szuroinfo"></param>
        /// <param name="szovege"></param>
        /// <returns></returns>
        public bool Comboinfoszures(ComboBox combo, string[] szuroinfo, bool szovege)
        {
            if (combo.Tag == null)
                return false;
            return ((MezoTag)combo.Tag).Comboinfoszures(szuroinfo, szovege);
        }
        /// <summary>
        /// Ha egy teljes combovalasztek bizonyos feltetelek szerint szukitendo, es a feltetelek osszefuggesfuggoek,
        /// az osszefugges informacio es a szuroinfo alapjan
        /// elvegzi ezt a szukitest
        /// </summary>
        /// <param name="combo">
        /// ComboBox
        /// </param>
        /// <param name="osszefinfo">
        /// az osszefugges info
        /// </param>
        /// <param name="szures">
        /// az osszefugges szurofeltetele
        /// </param>
        /// <returns></returns>
        public bool Comboinfoszures(ComboBox combo, Osszefinfo osszefinfo, object[] szures)
        {
            string[] idk = osszefinfo.GetSzurtOsszef(szures);
            if (combo.Tag != null)
            {
                return ((MezoTag)combo.Tag).Comboinfoszures(idk);
            }
            return false;
        }
        /// <summary>
        /// Egy osszefugges informacio szurt elemeinek id-tombjet akarom megkapni
        /// Az adott osszefugges (Osszefinfo) GetSzurtOsszef(szures) metodusat hivja, leirasa ott
        /// </summary>
        /// <param name="osszef">
        /// osszefugges informacio
        /// </param>
        /// <param name="szures">
        /// szurofeltetel, felepitese az Osszefinfo-nal
        /// </param>
        /// <returns>
        /// id-tomb vagy null
        /// </returns>
        public string[] GetSzurtOsszefIdk(Osszefinfo osszef, object[] szures)
        {
            return osszef.GetSzurtOsszef(szures);
        }
        /// <summary>
        /// Osszefuggesinformacio DataView(i)-ra filter osszeallitas
        /// Az adott osszefuggesinformacio (Osszefinfo) GetSzurtOsszefView(szures) metodusat hivja.
        /// Leiras ott
        /// </summary>
        /// <param name="osszef">
        /// A szurni kivant osszefuggesinformacio
        /// </param>
        /// <param name="szures">
        /// szurofeltetel, felepitese az Osszefinfo-nal
        /// </param>
        /// <returns>
        /// filterezett DataView(k)
        /// </returns>
        public DataView GetSzurtOsszefView(Osszefinfo osszef, object[] szures)
        {
            return osszef.GetSzurtOsszefView(szures);
        }
        /// <summary>
        /// Tablainformacio adattabla azon sorainak id-tombjet kivanjuk, melyek a szuresnek megfelelnek
        /// </summary>
        /// <param name="tabinfo">
        /// A kivant tablainformacio
        /// </param>
        /// <param name="szures">
        /// a parameter object[] egy eleme:
        /// new object{string columnneve,string kivant tartalom}
        /// </param>
        /// <returns>
        /// a szuresnek megfelelo id-tomb vagy null
        /// </returns>
        public string[] GetSzurtTabinfoIdk(Tablainfo tabinfo, object[] szures)
        {
            DataRow[] rows = tabinfo.Find(szures);
            ArrayList ar = new ArrayList();
            int idindex = tabinfo.IdentityColumnIndex;
            for (int i = 0; i < rows.Length; i++)
                ar.Add(rows[i][idindex].ToString());
            if (ar.Count == 0)
                return null;
            return (string[])ar.ToArray(typeof(string));
        }
        //public bool GridHibavizsg(Control hivo, DataGridView gw)
        //{
        //    bool hiba = false; ;
        //    MezoTag egytag = (MezoTag)gw.Tag;
        //    if (egytag != null)
        //    {
        //        for (int i = 0; i < gw.Rows.Count; i++)
        //        {
        //            for (int j = 0; j < gw.Columns.Count; j++)
        //            {
        //                DataGridViewCell cell = gw[j, i];
        //                DataGridViewColumn col = (DataGridViewColumn)gw.Columns[j];
        //                if (col.Tag != null)
        //                {
        //                    MezoTag egytag1 = (MezoTag)col.Tag;
        //                    egytag1.SelectedCell = cell;
        //                    cell.Tag = egytag1;
        //                    //                            hiba=Hibavizsg(egytag1);
        //                }
        //            }
        //        }
        //    }
        //    return hiba;
        //}
        /// <summary>
        /// UserControlInfo eloallitasa, ahol csak egy tablainformacio van.
        /// Egyelemu tablainfo tombot letrehozva a masik Attach-ot hivja
        /// A tobbi ott van leirva
        /// </summary>
        /// <param name="cont"></param>
        /// <param name="vezerles"></param>
        /// <param name="tabinfo"></param>
        /// <param name="tabpage"></param>
        /// <param name="menuitem"></param>
        /// <param name="dropitem"></param>
        /// <returns></returns>
        public UserControlInfo Attach(Base cont, Vezerloinfo vezerles, ref Tablainfo tabinfo, TabPage tabpage, ToolStripMenuItem menuitem, ToolStripMenuItem dropitem)
        {
            Tablainfo[] tabinfok = new Tablainfo[] { tabinfo };
            return Attach(cont, vezerles, ref tabinfok, tabpage, menuitem, dropitem);
        }
        /// <summary>
        /// A megadott UserControl-hoz tartozo UserControlInfo objektumot hoz letre, mely befuzesre kerul egyfelol
        /// minden, a parameterek kozt megadott Tablainfo objectum UserControlCollection-jebe, masfelol a sajatjaba
        /// is. A UserControlInfo eloallitashoz elobb a ControlTagokTolt-ot kell kiadni
        /// 
        /// </summary>
        /// <param name="cont">
        /// UserControl
        /// </param>
        /// <param name="vezerles">
        /// vezerloinfo
        /// </param>
        /// <param name="tabinfok">
        /// a UserControl altal hasznalt tablainformaciok tombje
        /// </param>
        /// <param name="tabpage">
        /// ha a UserControl TabPage-ben fut, a TabPage, egyebkent null
        /// </param>
        /// <param name="menuitem">
        /// ha van MenuStrip, a UserControlhoz tartozo aktiv menuitem, egyebkent null
        /// </param>
        /// <param name="dropitem">
        /// ha a UserControl dropitemhez tartozik, az aktiv dropitem, egyebkent null
        /// </param>
        /// <returns>
        /// letrehozott objectum
        /// </returns>
        public UserControlInfo Attach(Base cont, Vezerloinfo vezerles, ref Tablainfo[] tabinfok, TabPage tabpage, ToolStripMenuItem menuitem, ToolStripMenuItem dropitem)
        {
            UserControlInfo egyinfo = null;
            UserControlCollection egyinfok = _usercontrolok[cont,menuitem,dropitem];
            if (egyinfok == null)
                egyinfo = new UserControlInfo(cont, vezerles, tabinfok, tabpage, menuitem, dropitem);
            else
            {
                egyinfo = egyinfok.Find(cont, tabinfok);
                if (egyinfo == null)
                    egyinfo = new UserControlInfo(cont, vezerles, tabinfok, tabpage, menuitem, dropitem);

                Tablainfo[] egytabinfok = egyinfo.Tabinfok;
                foreach (Tablainfo tabinfo in tabinfok)
                {
                    Tablainfo egytabinfo = egyinfo.TabinfoArray.GetByAzontip(tabinfo.Azontip);
                    if (egytabinfo == null)
                        egytabinfo = (Tablainfo)egyinfo.TabinfoArray[egyinfo.TabinfoArray.Add(tabinfo)];
                    if (tabinfo.UserControlok.IndexOf(egyinfo) == -1)
                    {
                        tabinfo.UserControlok.Add(egyinfo);
                        tabinfo.AktualControlInfo = egytabinfo.AktualControlInfo;
                        tabinfo.AktualControlInfo.UserControlInfo = egyinfo;
                    }
                }
            }
            for (int i = 0; i < egyinfo.EgyContinfoArray.Count; i++)
            {
                for (int j = 0; j < egyinfo.EgyContinfoArray[i].Inputeleminfok.Length; j++)
                {
                    MezoTag egytag = egyinfo.EgyContinfoArray[i].Inputeleminfok[j];
                    if (!egytag.Controltipus.Contains("DataGrid") && egytag.Control != null && egytag.Control.Enabled)
                    {
                        if (egyinfo.VeryFirstTag == null)
                            egyinfo.VeryFirstTag = egytag;
                        egyinfo.VeryLastTag = egytag;
                    }
                }
            }
            return egyinfo;
        }
        /// <summary>
        /// UserLog-ba uj sor
        /// </summary>
        /// <param name="usercontnev">
        /// usercontrol neve
        /// </param>
        /// <param name="userparamok">
        /// a usercontrol parameterei
        /// </param>
        public void WriteLogInfo(string usercontnev, string userparamok)
        {
            WriteLogInfo(usercontnev, 0, userparamok);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="usercontnev"></param>
        /// <param name="parametertipus"></param>
        /// <param name="userparamok"></param>
        public void WriteLogInfo(string usercontnev, int parametertipus, string userparamok)
        {
            Tablainfo info = GetBySzintPluszTablanev("U", "USERLOG");
            DataRow row = info.Ujsor();
            row["KEZELO_ID"] = KezeloId;
            row["ALKALMAZAS_ID"] = AlkalmazasId;
            row["PARAMETERTIPUS"] = parametertipus;
            row["USERCONTNEV"] = usercontnev;
            row["USERPARAMOK"] = userparamok;
            info.Modositott = true;
            Rogzit(info);
            
        }
        public void SetAktRowVisible(DataGridView gridview, int sorindex)
        {
            int firstdisprowindex = gridview.FirstDisplayedScrollingRowIndex;
            if (firstdisprowindex == -1)
                firstdisprowindex = 0;
            if (sorindex != -1 && firstdisprowindex > sorindex)
                gridview.FirstDisplayedScrollingRowIndex = sorindex;
            else
            {
                int rowcount = gridview.DisplayedRowCount(false);
                int teljesrowcount = gridview.DisplayedRowCount(true);
                int lastvisible = rowcount + firstdisprowindex;
                int teljeslastvisible = teljesrowcount + firstdisprowindex;
                if (lastvisible < sorindex && firstdisprowindex < sorindex)
                    gridview.FirstDisplayedScrollingRowIndex = sorindex;
                else
                {
                    if (lastvisible == sorindex)
                        gridview.FirstDisplayedScrollingRowIndex++;
                    else if (teljeslastvisible == sorindex)
                        gridview.FirstDisplayedScrollingRowIndex = gridview.FirstDisplayedScrollingRowIndex + 2;
                }
            }
        }
        /// <summary>
        /// Munka vegen kiadando kerdesek, tevekenysegek, ha a kezeloi szint Fejleszto
        /// </summary>
        /// <param name="cont">
        /// a Main Form
        /// </param>
        /// <returns>
        /// Befejezi a munkat kerdesre adott valasz
        /// </returns>
        public MessageBox.DialogResult MunkaVege(Form cont)
        {
            return MunkaVege(cont, Base.KezSzint.Fejleszto);
        }
        /// <summary>
        /// Munka vegen kiadando kerdesek, tevekenysegek kezeloi szint alapjan
        /// </summary>
        /// <param name="cont">
        /// a Main Form
        /// </param>
        /// <param name="kezszint">
        /// Kezeloi szint
        /// </param>
        /// <returns>
        /// Befejezi a munkat kerdesre adott valasz
        /// </returns>
        public MessageBox.DialogResult MunkaVege(Form cont, Base.KezSzint kezszint)
        {
            MessageBox.DialogResult valasz;
            //MessageAlap mes;
            //if (kezszint == Base.KezSzint.Fejleszto)
            //{
            //    if (LastBackupDateTime[0].CompareTo(LastUpdateDateTime[0]) < 0)
            //    //                if (KellBackup(Rendszerconn))
            //    {
            //        valasz = MessageBox.Show("Indulhat a rendszer mentése?", "", MessageBox.MessageBoxButtons.IgenNem);// == FakPlusz.MessageBox.DialogResult.Igen);
            //        if (valasz == MessageBox.DialogResult.Cancel)
            //            return valasz;
            //        else if (valasz == MessageBox.DialogResult.Igen)
            //        {
            //            mes = new MessageAlap("Rendszer mentése folyamatban!");
            //            mes.Show(cont);
            //            Sqlinterface.Mentes(Rendszerconn);
            //            mes.Close();
            //            cont.Refresh();
            //        }
            //    }
            //}
            //if (kezszint != Base.KezSzint.Vezeto)
            //{
            //    if (kezszint == Base.KezSzint.Fejleszto || kezszint == Base.KezSzint.Minden || kezszint.ToString().Contains("Rendszer"))
            //    {
            //        if (LastBackupDateTime[1].CompareTo(LastUpdateDateTime[1]) < 0)
            //        {
            //            valasz = MessageBox.Show("Indulhat az alkalmazás mentése?", "", MessageBox.MessageBoxButtons.IgenNem);// == FakPlusz.MessageBox.DialogResult.Igen);
            //            if (valasz == MessageBox.DialogResult.Cancel)
            //                return valasz;
            //            else if (valasz == MessageBox.DialogResult.Igen)
            //            {
            //                mes = new MessageAlap("Alkalmazás mentése folyamatban!");
            //                mes.Show(cont);
            //                Sqlinterface.Mentes(Userconn);
            //                mes.Close();
            //                cont.Refresh();
            //            }
            //        }
            //    }
            //    bool kellmentes = false;
            //    for (int i = 2; i < ConnectionStringArray.Count; i++)
            //    {
            //        if (LastBackupDateTime[i].CompareTo(LastUpdateDateTime[i]) < 0)
            //        {
            //            kellmentes = true;
            //            break;
            //        }
            //    }
            //    if (kellmentes)
            //    {
            //        valasz = MessageBox.Show("Indulhat a cégek mentése?", "", MessageBox.MessageBoxButtons.IgenNem);
            //        if (valasz == FakPlusz.MessageBox.DialogResult.Cancel)
            //            return valasz;
            //        else if (valasz == MessageBox.DialogResult.Igen)
            //        {
            //            mes = new MessageAlap("Cégek mentése folyamatban!");
            //            mes.Show(cont);
            //            cont.Refresh();
            //            for (int i = 2; i < ConnectionStringArray.Count; i++)
            //            {
            //                if (LastBackupDateTime[i].CompareTo(LastUpdateDateTime[i]) < 0)
            //                {
            //                    _ceginfo.ViewSorindex = i - 2;
            //                    string cegnev = _ceginfo.AktualViewRow["SZOVEG"].ToString();
            //                    mes.SetText(cegnev);
            //                    mes.Refresh();
            //                    Sqlinterface.Mentes(ConnectionStringArray[i].ToString());
            //                }
            //            }
            //            mes.Close();
            //        }
            //    }
            //}
            valasz = MessageBox.Show("Biztosan befejezte a munkát?", "", MessageBox.MessageBoxButtons.IgenNem);// == FakPlusz.MessageBox.DialogResult.Igen);
            if (valasz != FakPlusz.MessageBox.DialogResult.Igen)
                return MessageBox.DialogResult.Cancel;
            else
                return valasz;
        }
        /// <summary>
        /// Egyseges Enter event kezelese az input Controlok-ra
        /// Kuldo input Control BackColor,Font allitas, LastEnter allitas (majd kell)
        /// </summary>
        /// <param name="sender">
        /// kuldo Control
        /// </param>
        /// <param name="e">
        /// EventArgs
        /// </param>
        public void Elem_Enter(object sender, EventArgs e)
        {
            Control cont = (Control)sender;
            {
                MezoTag egytag = (MezoTag)cont.Tag;
                if (egytag.UserControlInfo != null)
                {
                    {
                        try
                        {
                            TextBox box = (TextBox)cont;
                            if (!box.Multiline)
                            {
                                cont.BackColor = _aktivinputbackcolor;
                                cont.Font = _aktivinputfont;
                            }
                        }
                        catch
                        {
                            cont.BackColor = _aktivinputbackcolor;
                            cont.Font = _aktivinputfont;
                        }
                        if (cont.Parent.Name.Contains("groupBox"))
                            cont.Parent.BackColor = _aktivcontrolbackcolor;
                        egytag.Elementer = true;
                        egytag.UserControlInfo.LastEnter = egytag;
                    }
                }
            }
        }
        /// <summary>
        /// Egyseges Leave event kezelese az input Controlok-ra
        /// input Control BackColor,Font allitas
        /// Ha az event nincs tiltva, az elem hibavizsgalata
        /// </summary>
        /// <param name="sender">
        /// kuldo Control
        /// </param>
        /// <param name="e">
        /// EventArgs
        /// </param>
        public void Elem_Leave(object sender, EventArgs e)
        {
            //               bool groupbox = false;
            Control cont = (Control)sender;
            string text = cont.Text;
            cont.BackColor = _inaktivinputbackcolor;
            cont.Font = _inaktivinputfont;
            if (cont.Parent.Name.Contains("groupBox"))
                //                {
                cont.Parent.BackColor = _inaktivcontrolbackcolor;
            //                    groupbox = true;
            //                }
            if (!_eventtilt)
            {
                MezoTag egytag = (MezoTag)cont.Tag;
                if (egytag.UserControlInfo != null && egytag.UserControlInfo.User.Enabled)
                {
                    string conttext = egytag.Control.Text;
                    try
                    {
                        FormattedTextBox.FormattedTextBox tb = (FormattedTextBox.FormattedTextBox)egytag.Control;
                        tb.RemoveFormatCharacters();
                        conttext = tb.Text;
                        if (conttext == "")
                            tb.InsertFormatCharacters();
                    }
                    catch { }
                    bool hiba = egytag.EgyHibavizsg(egytag.Control.Text);
                    if (!hiba && !egytag.EgyControlInfo.Vanehiba())
                        egytag.Tabinfo.ModositasiHiba = false;
                    if (egytag.Changed)
                        egytag.Elementer = false;
                    egytag.Lastleave = true;
                    for (int i = 0; i < egytag.EgyControlInfo.Inputeleminfok.Length; i++)
                    {
                        MezoTag mastag = egytag.EgyControlInfo.Inputeleminfok[i];
                        if (mastag != egytag)
                            mastag.Lastleave = false;
                    }
                    if (egytag.Tabinfo.ModositasiHiba)
                        egytag.ModositasiHiba = true;
                }
                //if (groupbox)
                //    cont.Parent.Enabled = false;

                //if (egytag.Last && egytag.UserControlInfo != null && egytag != egytag.UserControlInfo.VeryLastTag)
                //{
                //    foreach(MezoTag egymezotag in egytag.OsszesTag)
                //    {
                //        if (!egymezotag.Controltipus.Contains("DataGrid") && egymezotag.Control.Enabled && egymezotag != egytag)
                //        {
                //            _eventtilt = true;
                //            egymezotag.Control.Focus();
                //            _eventtilt = false;
                //            break;
                //        }
                //    }
                //}
            }
        }
    }
}
