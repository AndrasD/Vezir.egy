using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using FakPlusz.Alapcontrolok;
namespace FakPlusz.Alapfunkciok
{
    /// <summary>
    /// Tablaazonositok osztalya tartalomjegyzekbol vagy mezoleirasokbol
    /// </summary>
    public class Azonositok
    {
        private string _azon = "";
        /// <summary>
        /// 
        /// </summary>
        public string Azon
        {
            get { return _azon; }
        }
        private string _termszarm = "";
        /// <summary>
        /// 
        /// </summary>
        public string Termszarm
        {
            get { return _termszarm; }
        }
        private int _sorindex = -1;
        /// <summary>
        /// 
        /// </summary>
        public int Sorindex
        {
            get { return _sorindex; }
        }
        private string _szint = "";
        /// <summary>
        /// 
        /// </summary>
        public string Szint
        {
            get { return _szint; }
        }
        private string _adatfajta = "";
        /// <summary>
        /// A tabla adatfajtaja
        /// </summary>
        /// <summary>
        /// a node parent-je a TreeView-ban
        /// </summary>
        public string Adatfajta
        {
            get { return _adatfajta; }
        }
        private string _parent = "";
        /// <summary>
        /// 
        /// </summary>
        public string Parent
        {
            get { return _parent; }
        }
        private string _nextparent = "";
        /// <summary>
        /// a tabla SORSZAM mezo tartalma, ha van ilyen(azaz vannak child-nodeok
        /// </summary>
        public string Nextparent
        {
            get { return _nextparent; }
        }
        private string _kodtipus = "";
        /// <summary>
        /// a tabla kodtipusa, ha van ilyen
        /// </summary>
        public string Kodtipus
        {
            get { return _kodtipus; }
        }
        private string _tablanev = "";
        /// <summary>
        /// a tabla neve
        /// </summary>
        public string Tablanev
        {
            get { return _tablanev; }
        }
        private string _szoveg = "";
        /// <summary>
        /// a node Text-je
        /// </summary>
        public string Szoveg
        {
            get { return _szoveg; }
            set { _szoveg = value; }
        }
        private int _kodhossz = 0;
        /// <summary>
        /// Kod hossza
        /// </summary>
        public int Kodhossz
        {
            get { return _kodhossz; }
        }
        private int _szoveghossz = 0;
        /// <summary>
        /// Szoveg hossza
        /// </summary>
        public int Szoveghossz
        {
            get { return _szoveghossz; }
        }
        private string _defert = "";
        /// <summary>
        /// Kezdoertek
        /// </summary>
        public string Defert
        {
            get { return _defert; }
        }
        private string _beszurhat = "";
        /// <summary>
        /// Beszurhat sort? I/N
        /// </summary>
        public string Beszurhat
        {
            get { return _beszurhat; }
        }
        private string _modosithat = "";
        /// <summary>
        /// Modosithat a tablaban? I/N
        /// </summary>
        public string Modosithat
        {
            get { return _modosithat; }
        }
        private string _sorazonositomezo = "";
        /// <summary>
        /// A sort azonosito mezo
        /// </summary>
        public string Sorazonositomezo
        {
            get { return _sorazonositomezo; }
        }
        private string _sort = "";
        /// <summary>
        /// A DataView sort-ja
        /// </summary>
        public string Sort
        {
            get { return _sort; }
        }
        private string _szovegcolname = "";
        /// <summary>
        /// A szoveg oszlop neve
        /// </summary>
        public string Szovegcolname
        {
            get { return _szovegcolname; }
        }
        private string _useridstring = "";
        /// <summary>
        /// 
        /// </summary>
        public string UserIdstring
        {
            get { return _useridstring; }
        }
        private string _selectstring = "";
        /// <summary>
        /// Select string
        /// </summary>
        public string Selectstring
        {
            get { return _selectstring; }
            set { _selectstring = value; }
        }
        private string _orderstring = "";
        /// <summary>
        /// Order string a select-hez
        /// </summary>
        public string Orderstring
        {
            get { return _orderstring; }
            set { _orderstring = value; }
        }
        private bool _lehetcombo = false;
        /// <summary>
        /// A tabla szerepel Combo-ban? 
        /// </summary>
        public bool Lehetcombo
        {
            get { return _lehetcombo; }
        }
        private string _combofileba = "";
        /// <summary>
        /// Combo eseten a fileban tarolando oszlop neve
        /// </summary>
        public string Combofileba
        {
            get { return _combofileba; }
        }
        private string[] _comboszovegbe = null;
        /// <summary>
        /// Combo eseten a megjelenitendo oszlop(ok) neve
        /// </summary>
        public string[] Comboszovegbe
        {
            get { return _comboszovegbe; }
        }
        private string _combosort = "";
        /// <summary>
        /// ComboItemek sort-ja
        /// </summary>
        public string Combosort
        {
            get { return _combosort; }
        }
        private bool _lehetosszef = false;
        /// <summary>
        /// Szerepelhet-e a tabla osszefuggesben
        /// </summary>
        public bool Lehetosszef
        {
            get { return _lehetosszef; }
        }
        private bool _lehetcsoport = false;
        /// <summary>
        /// Szerepelhet-e a tabla csoportmeghatarozasban
        /// </summary>
        public bool Lehetcsoport
        {
            get { return _lehetcsoport; }
        }
        private bool _origlehetures = false;
        /// <summary>
        /// Eredeti parameterezes szerint lehet-e a tabla ures
        /// </summary>
        public bool EredetiLehetures
        {
            get { return _origlehetures; }
        }
        private bool _lehetures = false;
        /// <summary>
        /// Jelenlegi konstellacioban a tabla lehet-e ures. User valtoztathatja
        /// </summary>
        public bool Lehetures
        {
            get { return _lehetures; }
            set { _lehetures = value; }
        }
        private bool _nemkell = false;
        /// <summary>
        /// Kell-e a tabla. Alapertelmezesben false, user valtoztathatja
        /// </summary>
        public bool Nemkell
        {
            get { return _nemkell; }
            set { _nemkell = value; }
        }
        private string _szulotabla = "";
        /// <summary>
        /// Szulotabla neve
        /// </summary>
        public string Szulotabla
        {
            get { return _szulotabla; }
            set { _szulotabla = value; }
        }
        private string _szuloszint = "";
        /// <summary>
        /// Szulotabla szintje
        /// </summary>
        public string Szuloszint
        {
            get { return _szuloszint; }
            set { _szuloszint = value; }
        }
        private string _azontip = "";
        /// <summary>
        /// Tabla teljes azonositoja
        /// </summary>
        public string Azontip
        {
            get { return _azontip; }
        }
        private string _szrmazontip = "";
        /// <summary>
        /// "SZRM" tablak teljes azonositoja
        /// </summary>
        public string Szrmazontip
        {
            get { return _szrmazontip; }
        }
        private string _azontip1 = "";
        /// <summary>
        /// Osszetett tipusu tablak 1.elem teljes azonositoja
        /// </summary>
        public string Azontip1
        {
            get { return _azontip1; }
        }
        private string _szoveg1 = "";
        /// <summary>
        /// Osszetett tipusu tablak 1.elem szovege
        /// </summary>
        public string Szoveg1
        {
            get { return _szoveg1; }
        }
        private string _azontip2 = "";
        /// <summary>
        /// Osszetett tipusu tablak 2.elem teljes azonositoja
        /// </summary>
        public string Azontip2
        {
            get { return _azontip2; }
        }
        private string _szoveg2 = "";
        /// <summary>
        /// Osszetett tipusu tablak 2.elem szovege
        /// </summary>
        public string Szoveg2
        {
            get { return _szoveg2; }
        }
        private bool _kellverzio = false;
        /// <summary>
        /// Kell verzio?
        /// </summary>
        public bool Kellverzio
        {
            get { return _kellverzio; }
        }
        private string _owner = "";
        /// <summary>
        /// A tabla tulajdonosanak neve
        /// </summary>
        public string Owner
        {
            get { return _owner; }
        }
        private string _ownerid = "";
        /// <summary>
        /// A tablatulajdonos Id-je
        /// </summary>
        public string Ownerid
        {
            get { return _ownerid; }
        }
        private string _ownernev = "";
        //public string Ownernev
        //{
        //    get { return _ownernev; }
        //}
        private string _user = "";
        /// <summary>
        /// A tablat hasznalhatok stringje (vesszokkel elvalasztva)
        /// </summary>
        public string User
        {
            get { return _user; }
        }
        private string[] _userek = new string[0];
        /// <summary>
        /// A tablat hasznalhatok nevenek tombje, ha User ures, 0 hosszu
        /// </summary>
        //public string[] Userek
        //{
        //    get { return _userek; }
        //}
        //private string[] _useridk = new string[0];
        //public string[] UserIdk
        //{
        //    get { return _useridk; }
        //}
        private bool _enyem = false;
        /// <summary>
        /// Gittae?
        /// </summary>
        public bool Enyem
        {
            get { return _enyem; }
        }
        private bool _leiroenyem = false;
        /// <summary>
        /// Gittae?
        /// </summary>
        public bool LeiroEnyem
        {
            get { return _leiroenyem; }
        }
        private bool _torolheto = true;
        /// <summary>
        /// torolheto a tabla a tartalomjegyzekbol?
        /// </summary>
        public bool Torolheto
        {
            get { return _torolheto; }
        }
        private bool _teljeshonap = false;
        /// <summary>
        /// Ha listaknal,statisztikaknal tol/ig megadas van, teljes honap legyen-e
        /// </summary>
        public bool Teljeshonap
        {
            get { return _teljeshonap; }
        }
        private Verzioinfok _verzioinfok = null;
        /// <summary>
        /// A tabla verzioinformacioi
        /// </summary>
        public Verzioinfok Verzioinfok
        {
            get { return _verzioinfok; }
        }
        private FakUserInterface _fak;
        /// <summary>
        /// A Fak
        /// </summary>
        public FakUserInterface Fak
        {
            get { return _fak; }
        }
        private bool _leiroe;
        /// <summary>
        /// Leirotabla azonositokrol van-e szo
        /// </summary>
        public bool Leiroe
        {
            get { return _leiroe; }
        }
        private DateTime _lastupdate = DateTime.MinValue;
        /// <summary>
        /// A tabla utolso updatejenek datuma
        /// </summary>
        public DateTime Lastupdate
        {
            get { return _lastupdate; }
            set { _lastupdate = value; }
        }
        private DateTime _lastbackupdate = DateTime.MinValue;
        /// <summary>
        /// A tabla adatbazis utolso backup-janak datuma
        /// </summary>
        public DateTime Lastbackupdate
        {
            get { return _lastbackupdate; }
            set { _lastbackupdate = value; }
        }
        private string _tooltiptext = "";
        /// <summary>
        /// A tabla tooltip szovege
        /// </summary>
        public string Tooltiptext
        {
            get { return _tooltiptext; }
        }
        private Azonositok _leiroazonositok = null;
        /// <summary>
        /// A tabla leirotablajanak azonositoi
        /// </summary>
        public Azonositok Leiroazonositok
        {
            get { return _leiroazonositok; }
        }
        private Base.HozferJogosultsag[] _jogszintek = new Base.HozferJogosultsag[]{Base.HozferJogosultsag.Irolvas,Base.HozferJogosultsag.Semmi,Base.HozferJogosultsag.Semmi,Base.HozferJogosultsag.Semmi,
                   Base.HozferJogosultsag.Semmi, Base.HozferJogosultsag.Semmi,Base.HozferJogosultsag.Semmi,Base.HozferJogosultsag.Semmi,Base.HozferJogosultsag.Semmi,Base.HozferJogosultsag.Semmi,Base.HozferJogosultsag.Semmi,Base.HozferJogosultsag.Semmi,
                   Base.HozferJogosultsag.Semmi};

        /// <summary>
        /// Jogosultsagi szintek tombje
        /// </summary>
        public Base.HozferJogosultsag[] Jogszintek
        {
            get { return _jogszintek; }
        }
        private TablainfoTag _tablatag;
        /// <summary>
        /// A tabla TablainfoTag objectuma
        /// </summary>
        public TablainfoTag Tablatag
        {
            get { return _tablatag; }
        }
        private DataRow _adatsor;
        /// <summary>
        /// Az informacios adatsor
        /// </summary>
        public DataRow Adatsor
        {
            get { return _adatsor; }
        }
        /// <summary>
        /// tabla azonositok objectumanak eloallitas 
        /// </summary>
        /// <param name="tag">
        /// osszevont tablainformaciok
        /// </param>
        /// <param name="dr">
        /// informacios adatsor
        /// </param>
        /// <param name="sorindex">
        /// az adatsor sorindexe
        /// </param>
        /// <param name="tablanev">
        /// tabla neve
        /// </param>
        /// <param name="fak">
        /// fakuserinterface
        /// </param>
        public Azonositok(TablainfoTag tag, DataRow dr, int sorindex, string tablanev, FakUserInterface fak)
        {
            AzonositoInit(tag, dr, sorindex, tablanev, fak, false);
        }
        /// <summary>
        /// tabla vagy leirotabla azonositok objectumanak eloallitasa
        /// </summary>
        /// <param name="tag">
        /// A TablainfoTag
        /// </param>
        /// <param name="dr">
        /// Az informacios adatsor
        /// </param>
        /// <param name="sorindex">
        /// az adatsor sorindexe
        /// </param>
        /// <param name="tablanev">
        /// tabla neve
        /// </param>
        /// <param name="fak">
        /// fakuserinterface
        /// </param>
        /// <param name="leiroe">
        /// true: leirotabla
        /// </param>
        public Azonositok(TablainfoTag tag, DataRow dr, int sorindex, string tablanev, FakUserInterface fak, bool leiroe)
        {
            AzonositoInit(tag, dr, sorindex, tablanev, fak, leiroe);
        }
        /// <summary>
        /// objectum inicializalasa
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="dr"></param>
        /// <param name="sorindex"></param>
        /// <param name="tablanev"></param>
        /// <param name="fak"></param>
        /// <param name="leiroe"></param>
        public void AzonositoInit(TablainfoTag tag, DataRow dr, int sorindex, string tablanev, FakUserInterface fak, bool leiroe)
        {
            _fak = fak;
            _tablatag = tag;
            _sorindex = sorindex;
            _leiroe = leiroe;
            _adatsor = dr;
            if (dr == null && tablanev == "BASE")
            {
                _azon = "BASE";
                _verzioinfok = fak.VerzioInfok["R"];
                _kellverzio = false;
                _leiroenyem = true;
                if (!_leiroe)
                {
                    _tablanev = "BASE";
                    _szint = "R";
                    _selectstring = "";
                    _orderstring = " order by PARENT,SORSZAM";
                    _beszurhat = "I";
                    _modosithat = "I";
                    _sorazonositomezo = "SZOVEG";
                    _sort = "SORREND";
                    _szovegcolname = "SZOVEG";
                    _enyem = false;
                    _lehetcombo = true;
                    _azon = "BASE";
                    _azontip = "BASEBASE";
                    _combofileba = "SZINT";
                    _comboszovegbe = new string[] { "SZOVEG" };
                    _combosort = "SORREND";
                }
                else
                {
                    _tablanev = "LEIRO";
                    _szint = "R";
                    _selectstring = " where AZON='" + _azon + "' and TABLANEV='BASE'";
                    _orderstring = " order by VERZIO_ID,AZON,TABLANEV,ADATNEV";
                    _beszurhat = "N";
                    _modosithat = "I";
                    _torolheto = false;
                    _enyem = true;
                    _sorazonositomezo = "ADATNEV";
                    _sort = "ADATNEV";
                    _szovegcolname = "ADATNEV";
                }
            }
            else if (dr == null || leiroe)    // leiro
            {
                if (dr == null)
                {
                    _azon = "LEIR";
                    _tablanev = "LEIRO";
                    _enyem = true;
                    _leiroenyem = true;
                    _szint = "R";
                    _kellverzio = false;
                }
                else
                {
                    _azon = dr["AZON"].ToString();
                    _tablanev = dr["TABLANEV"].ToString();
                    if (dr.Table.TableName == "TARTAL")
                    {
                        _enyem = _fak.SetBoolByIgenNem(dr["ENYEM"].ToString());
                        _leiroenyem = _fak.SetBoolByIgenNem(dr["LEIROENYEM"].ToString());
                    }
                    _szint = "R";
                    _kellverzio = true;
                }
                _selectstring = " where AZON='" + _azon + "' and TABLANEV='" + _tablanev + "'";
                _orderstring = " order by VERZIO_ID,AZON,TABLANEV,ADATNEV";
                _beszurhat = "N";
                _modosithat = "I";
                _torolheto = false;
                _sorazonositomezo = "ADATNEV";
                _sort = "ADATNEV";
                _szovegcolname = "ADATNEV";
                _verzioinfok = fak.VerzioInfok["R"];
                //if (_tablanev == "TARTAL" && _fak.Alkalmazas != "TERVEZO")
                //{
                //    _ownernev = "OWNER";
                //    UpdateSelectString();
                //}
            }
            else if (dr != null)
            {
                AzonositokUpdate(dr);
                if (_tablanev == "LEIRO")
                {
                    _szovegcolname = "SORSZOV";
                    _enyem = true;
                    _verzioinfok = fak.VerzioInfok["R"];
                }
                else if (_tablanev == "TARTAL")
                {
                    string savsel = _selectstring;
                    _selectstring = " where AZON='" + dr["AZON"].ToString() + "'";
                    if (savsel != "")
                        _selectstring += " AND " + savsel;
                    _orderstring = " order by VERZIO_ID,SORREND, AZONTIP,AZONTIP1,AZONTIP2";
                    _sorazonositomezo = "AZONTIP";
                    _sort = "SORREND";
                    _azontip = _azon + _tablanev;
                    _modosithat = "I";
                    _torolheto = true;
                    _enyem = false;
                    _leiroenyem = true;
                    if (_azon != "SZRM")
                        _beszurhat = "I";
                    else
                        _beszurhat = "N";
                    _kellverzio = true;
                }
                else if (_tablanev == "KODTAB" || _tablanev == "OSSZEF" || _tablanev == "ADATSZOLG" ||
                    _tablanev == "LISTA" || _tablanev == "STATISZTIKA")
                {
                    _leiroenyem = true;
                    _selectstring = " where KODTIPUS='" + _kodtipus + "'";
                    if (_tablanev == "OSSZEF")
                    {
                        _beszurhat = "N";
                        _torolheto = false;
                    }
                }
                else if (_tablanev == "VALTOZASNAPLO")
                {
                    string tim = DateTime.Today.ToShortDateString();
                    tim = tim.Replace(".", "-");
                    if (tim.EndsWith("-"))
                        tim = tim.Substring(0, tim.Length - 1);
                    string lastm = "{d '" + tim + "'}";
                    _selectstring = " where LAST_MOD>" + lastm;
                    if (_fak.Alkalmazas != "TERVEZO")
                        _selectstring += " AND (ALKALM = '' OR ALKALM = '" + _fak.Alkalmazas + "')";
                }
                else if (_tablanev == "USERLOG" ||_tablanev == "CEGKEZELOKIOSZT" || _tablanev == "CEGSZERZODES" )
                {
                    if (_fak.AlkalmazasId != "")
                        _selectstring = " where ALKALMAZAS_ID='" + _fak.AlkalmazasId + "'";
                }
                if (_leiroe || _tablanev == "TARTAL")
                    _verzioinfok = _fak.VerzioInfok["R"];
                else if (_szint != "" && _adatfajta != "")
                //               {
                {
                    _verzioinfok = _fak.VerzioInfok[_szint];
                    if (_verzioinfok == null)
                        _verzioinfok = _fak.VerzioInfok["C"];
                }
                    UpdateSelectString();
            }
        }
        private void UpdateSelectString()
        {
            if (_szint=="R" || _tablanev == "" || _tablanev == "LEIRO" || _fak.Alkalmazas == "TERVEZO" || _verzioinfok.AktualConnection == "")
                return;
            DataTable dt = new DataTable();
            Sqlinterface.Select(dt, _verzioinfok.AktualConnection, _tablanev,_selectstring, "", true);
            string idnev = dt.Columns[0].ColumnName;
            _ownernev = "";
            if (dt.Columns.IndexOf("OWNER") != -1)
                _ownernev = "OWNER";
            else if (dt.Columns.IndexOf("ALKALMAZAS_ID") != -1)
                _ownernev = "ALKALMAZAS_ID";
            if (_ownernev == "")
                return;
            if (dt.Columns.IndexOf("USEREK") == -1)
            {
                if (_selectstring == "")
                    _selectstring = " WHERE ";
                else
                    _selectstring += " AND ";
                _selectstring += "(" + _ownernev + " = '' OR " + _ownernev + " = 0 OR "+_ownernev+" = " + _fak.AlkalmazasId + ")";
                return;
            }
            else
            {
                dt.Rows.Clear();
//                string selstring=_selectstring+" AND "+_ownernev + "<> "+_fak.AlkalmazasId;
                Sqlinterface.Select(dt, _verzioinfok.AktualConnection, _tablanev, _selectstring, "", false);
                if (dt.Rows.Count == 0)
                    return;
                string selstring="";
                char[] vesszo = new char[1];
                vesszo[0] = Convert.ToChar(",");
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr[_ownernev].ToString() != "" && dr[_ownernev].ToString()!="0" && dr[_ownernev].ToString() != _fak.AlkalmazasId)
                    {
                        _user = dr["USEREK"].ToString();
                        string id = dr[idnev].ToString();
                        if (_user == "")
                        {
                            if (selstring != "")
                                selstring += " AND ";
                            selstring += idnev + " <> " + id;
                        }
                        else
                        {
                            _userek = _user.Split(vesszo);
                            ArrayList ar = new ArrayList(_userek);
                            if (ar.IndexOf(_fak.Alkalmazas) == -1)
                            {
                                if (selstring != "")
                                    selstring += " AND ";
                                selstring += idnev + " <> " + id;
                            }
                        }
                    }
                }
                if (selstring != "")
                {
                    if (_selectstring != "")
                        _selectstring += " AND ";
                    else
                        _selectstring += " WHERE ";
                    _selectstring += selstring;
                }
            }
        }
        
        /// <summary>
        /// Lasd Azonositok-nal
        /// </summary>
        /// <param name="dr"></param>
        public void AzonositokUpdate(DataRow dr)
        {
            char[] vesszo = new char[1];
            vesszo[0] = Convert.ToChar(",");
            foreach (DataColumn dc in dr.Table.Columns)
            {
                string tartal = dr[dc.ColumnName].ToString().Trim();
                bool eredetijog = true;
                switch (dc.ColumnName)
                {
                    case "AZON":
                        _azon = tartal;
                        switch (_azon.Length)
                        {
                            case 1:
                                _termszarm = _azon;
                                _szint = "R";
                                break;
                            case 2:
                                _termszarm = _azon;
                                _szint = "R";
                                break;
                            case 3:
                                _termszarm = _azon.Substring(0, 2);
                                _szint = _azon.Substring(2, 1);
                                break;
                            case 4:
                                _termszarm = _azon.Substring(0, 2);
                                _szint = _azon.Substring(2, 1);
                                _adatfajta = _azon.Substring(3, 1);
                                break;
                        }

                        break;
                    case "SZOVEG":
                        _szoveg = tartal;
                        _szovegcolname = "SZOVEG";
                        break;
                    case "PARENT":
                        _parent = tartal;
                        break;
                    case "SORSZAM":
                        _nextparent = tartal;
                        break;
                    case "KODTIPUS":
                        _kodtipus = tartal;
                        break;
                    case "TABLANEV":
                        _tablanev = tartal;
                        break;
                    case "KODHOSSZ":
                        _kodhossz = Convert.ToInt16(tartal);
                        break;
                    case "SZOVEGHOSSZ":
                        _szoveghossz = Convert.ToInt16(tartal);
                        break;
                    case "KEZDOERTEK":
                        _defert = tartal;
                        break;
                    case "BESZURHAT":
                        _beszurhat = tartal;
                        _torolheto = _fak.SetBoolByIgenNem(tartal);
                        break;
                    case "MODOSITHAT":
                        _modosithat = tartal;
                        break;
                    case "SORAZONOSITOMEZO":
                        _sorazonositomezo = tartal;
                        break;
                    case "SORT":
                        _sort = tartal;
                        break;
                    case "SELWHERE":
                        if (tartal != "")
                            _selectstring = " where " + tartal;
                        break;
                    case "SELORD":
                        if (tartal != "")
                            _orderstring = " order by " + tartal;
                        break;
                    case "LEHETCOMBO":
                        _lehetcombo = _fak.SetBoolByIgenNem(tartal);
                        break;
                    case "COMBOFILEBA":
                        _combofileba = tartal;
                        break;
                    case "COMBOSZOVEGBE":
                        if (tartal != "")
                        {
                            _comboszovegbe = tartal.Split(vesszo);
                        }
                        break;
                    case "COMBOSORT":
                        _combosort = tartal;
                        if (tartal == "" && _comboszovegbe!=null)
                        {
                            foreach (string s in _comboszovegbe)
                            {
                                if (tartal != "")
                                    tartal += ",";
                                tartal += s;
                            }
                            _combosort = tartal;
                        }
                        break;
                    case "LEHETCSOPORT":
                        _lehetcsoport = _fak.SetBoolByIgenNem(tartal);
                        break;
                    case "LEHETOSSZEF":
                        _lehetosszef = _fak.SetBoolByIgenNem(tartal);
                        break;
                    case "LEHETURES":
                        _lehetures = _fak.SetBoolByIgenNem(tartal);
                        _origlehetures = _lehetures;
                        break;
                    case "SZULOTABLA":
                        _szulotabla = tartal;
                        break;
                    case "SZULOSZINT":
                        _szuloszint = tartal;
                        break;
                    case "AZONTIP":
                        _azontip = tartal;
                        if (_azontip.Substring(0, 4) != "SZRM" && _termszarm.Trim() == "T" && _fak.Adatszintek.Contains(_szint))
                            _szrmazontip = "SZRM" + _szint + _tablanev;
                        break;
                    case "AZONTIP1":
                        _azontip1 = tartal;
                        break;
                    case "SZOVEG1":
                        _szoveg1 = tartal;
                        break;
                    case "AZONTIP2":
                        _azontip2 = tartal;
                        break;
                    case "SZOVEG2":
                        _szoveg2 = tartal;
                        break;
                    case "KELLVERZIO":
                        _kellverzio = _fak.SetBoolByIgenNem(tartal);
                        break;
                    case "OWNER":
                        _ownerid = tartal;
                        _ownernev = "OWNER";
                        if (tartal != "")
                        {
                            int ind = _fak.AlkalmazasIdk.IndexOf(tartal);
                            if (ind != -1)
                                _owner = _fak.AlkalmazasNevek[ind].ToString();
                        }
                        else
                            _owner = "";
                        if (_owner != "" && _owner != _fak.Alkalmazas)
                            eredetijog = false;

                        break;
                    //case "ALKALMAZAS_ID":
                    //    _ownerid = tartal;
                    //    _ownernev = "ALKALMAZAS_ID";
                    //    if (tartal != "")
                    //    {
                    //        int ind = _fak.AlkalmazasIdk.IndexOf(tartal);
                    //        if (ind != -1)
                    //            _owner = _fak.AlkalmazasNevek[ind].ToString();
                    //    }
                    //    else
                    //        _owner = "";
                    //    break;
                    case "USEREK":
                        _userek = tartal.Split(vesszo);
                        _user = tartal;
                    //    if (tartal != "")
                    //    {
                    //        _useridk = new string[_userek.Length];

                    //        for(int i = 0; i<_userek.Length;i++)
                    //        {
                    //            _useridk[i]="";
                    //            int ind = _fak.AlkalmazasNevek.IndexOf(_userek[i]);
                    //            if (ind != -1)
                    //                _useridk[i] = _fak.AlkalmazasIdk[ind].ToString();

                    //        }
                    //    }
                        break;
                    case "FEJLESZTO":
                        if (!eredetijog && tartal == "0")
                            tartal = "1";
                        _jogszintek[0] = _fak.SetJogszint(tartal);
                        break;
                    case "MINDEN":
                        if (!eredetijog && tartal == "0")
                            tartal = "1";
                        _jogszintek[1] = _fak.SetJogszint(tartal);
                        break;
                    case "RENDSZERGAZDA":
                        if (!eredetijog && tartal == "0")
                            tartal = "1";
                        _jogszintek[2] = _fak.SetJogszint(tartal);
                        break;
                    case "RENDSZERPLKIEM":
                        if (!eredetijog && tartal == "0")
                            tartal = "1";
                        _jogszintek[3] = _fak.SetJogszint(tartal);
                        break;
                    case "RENDSZERPLKEZELO":
                        if (!eredetijog && tartal == "0")
                            tartal = "1";
                        _jogszintek[4] = _fak.SetJogszint(tartal);
                        break;
                    case "KIEMELTKEZELO":
                        if (!eredetijog && tartal == "0")
                            tartal = "1";
                        _jogszintek[5] = _fak.SetJogszint(tartal);
                        break;
                    case "KIEMELTPLKEZELO":
                        if (!eredetijog && tartal == "0")
                            tartal = "1";
                        _jogszintek[6] = _fak.SetJogszint(tartal);
                        break;
                    case "KEZELO":
                        if (!eredetijog && tartal == "0")
                            tartal = "1";
                        _jogszintek[7] = _fak.SetJogszint(tartal);
                        break;
                    case "VEZETO":
                        if (!eredetijog && tartal == "0")
                            tartal = "1";
                        _jogszintek[8] = _fak.SetJogszint(tartal);
                        break;
                    case "CEGMINDEN":
                        if (!eredetijog && tartal == "0")
                            tartal = "1";
                        _jogszintek[9] = _fak.SetJogszint(tartal);
                        break;
                    case "ENYEM":
                        _enyem = _fak.SetBoolByIgenNem(tartal);
                        break;
                    case "LEIROENYEM":
                        _leiroenyem = _fak.SetBoolByIgenNem(tartal);
                        break;
                    case "TELJESHONAP":
                        _teljeshonap = _fak.SetBoolByIgenNem(tartal);
                        break;
                    case "TOOLTIP":
                        _tooltiptext = tartal;
                        break;
                }
            }
            if (_combofileba == "" || _comboszovegbe == null)
                _lehetcombo = false;
            Base.HozferJogosultsag rhozfer = _jogszintek[2];
             Base.HozferJogosultsag kiemkez = _jogszintek[6]; // ??????????????
             Base.HozferJogosultsag vezeto = _jogszintek[8];
             if (rhozfer == Base.HozferJogosultsag.Irolvas || kiemkez == Base.HozferJogosultsag.Irolvas)
                 _jogszintek[11] = Base.HozferJogosultsag.Irolvas;
             else if (rhozfer == Base.HozferJogosultsag.Csakolvas || kiemkez == Base.HozferJogosultsag.Csakolvas)
                 _jogszintek[11] = Base.HozferJogosultsag.Csakolvas;
             else
                 _jogszintek[11] = Base.HozferJogosultsag.Semmi;
             if (rhozfer == Base.HozferJogosultsag.Irolvas || vezeto == Base.HozferJogosultsag.Irolvas)
                 _jogszintek[12] = Base.HozferJogosultsag.Irolvas;
             else if (rhozfer == Base.HozferJogosultsag.Csakolvas || vezeto == Base.HozferJogosultsag.Csakolvas)
                 _jogszintek[12] = Base.HozferJogosultsag.Csakolvas;
             else
                 _jogszintek[12] = Base.HozferJogosultsag.Semmi;
            //if (_ownernev != "" && _fak.Alkalmazas!="TERVEZO")
            //{
            //    if(_user!="")
            //    {
            //        foreach(string usid in _useridk)
            //        {
            //            if(usid!="")
            //            {
            //                if(_useridstring !="")
            //                    _useridstring += " OR ";
            //                _useridstring +=_ownernev + "="+usid;
            //            }
            //        }
            //    }
            //    if (_useridstring != "")
            //    {
            //        _useridstring = "(" + _useridstring + ")";
            //        _useridstring += " AND ";
            //    }
            //    _useridstring += _ownernev +"=" + _fak.AlkalmazasId;
//            }
        }
        /// <summary>
        /// lasd Azonositok-nal
        /// </summary>
        public void ToolTipText()
        {
            if (_tablatag.ParentTag != null && _parent == null)
                _parent = _tablatag.ParentTag.Azonositok._nextparent;

            string k = "";
            if (_szoveg1 != "" && _szoveg2 != "")
            {
                if (_tablatag.Forditott)
                {
                    k = "\n Forditott";
                    _szoveg = _szoveg2 + "/" + _szoveg1 + "(" + _kodtipus + ")";
                }
                else
                    _szoveg = _szoveg1 + "/" + _szoveg2 + "(" + _kodtipus + ")";
                _tablatag.Node.Text = _szoveg;
            }
            string text = "";
            if (_tooltiptext == "")
                text = _szoveg;
            else
                text = _tooltiptext;
            if (_lastupdate != DateTime.MinValue)
                text += "\n Utolsó módositás: " + _lastupdate.ToString();
            if (_lastbackupdate != DateTime.MinValue)
                text += "\n Utolsó mentés: " + _lastbackupdate.ToString();
            if (_fak.Alkalmazas == "TERVEZO")
            {
                if (_owner != "")
                    text += "\n Owner: " + _owner;
                if (_userek != null && _userek.Length != 0 && _userek[0] != "")
                {
                    text += "\n Userek: " + _userek[0];
                    for (int i = 1; i < _userek.Length; i++)
                        text += "," + _userek[i];
                }
                text += "\n Azonosito: " + _azon + "\n Adatfajta: " + _adatfajta + "\n Parent: " + _parent + "\n Sorszam: " + _nextparent + k;
                if (_kodtipus != "")
                    text += "\n Kodtipus: " + _kodtipus;
                if (_tablanev != "")
                    text += "\n Tablanev: " + _tablanev;
                if (_azontip != "")
                    text += "\n Azontip: " + _azontip;
                if (_azontip1 != "")
                {
                    k = "\n 1.elem: ";
                    if (_tablatag.Forditott)
                        text += k + _azontip2;
                    else
                        text += k + _azontip1;
                }
                if (_azontip2 != "")
                {
                    k = "\n 2.elem: ";
                    if (_tablatag.Forditott)
                        text += k + _azontip1;
                    else
                        text += k + _azontip2;
                }
                if (_selectstring != "")
                    text += "\n Selwhere: " + _selectstring;
                if (_kellverzio)
                    k = "Igen";
                else
                    k = "Nem";
                text += "\n KellVerzio: " + k;
                Tablainfo tabinfo = null;
                if (_tablatag != null)
                    tabinfo = _tablatag.Tablainfo;
                if (tabinfo != null && _kellverzio)
                {
                    if (tabinfo.VerzioTerkepArray.Count != 0)
                    {
                        string verid = tabinfo.VerzioTerkepArray[tabinfo.VerzioTerkepArray.Count - 1].ToString();
                        if (_verzioinfok.VersionArray != null)
                        {
                            for (int i = 0; i < _verzioinfok.VersionArray.Length; i++)
                            {
                                if (_verzioinfok.VersionArray[i].ToString() == verid)
                                {
                                    text += "\n Legmagasabb verzio: " + _verzioinfok.VerzValtasoka[i];
                                    break;
                                }
                            }
                        }
                    }
                    else
                        text += "\n Nincs meg verzio";
                }
            }
            if (_tablatag != null)
                _tablatag.Node.ToolTipText = text;
        }
    }
}


