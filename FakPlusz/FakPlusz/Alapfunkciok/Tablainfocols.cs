using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Globalization;
using System.Data;
using System.Threading;
using FakPlusz;
namespace FakPlusz.Alapfunkciok
{
    /// <summary>
    /// Mezoinformaciok 
    /// </summary>
    public class ColCollection : ArrayList
    {
        /// <summary>
        /// ColumnNevek tombje
        /// </summary>
        private ArrayList ColNames = new ArrayList();
        /// <summary>
        /// Egy-egy elme Combo jellegu mezoinformacional a Combo teljes azonositoja, egyebkent ""
        /// </summary>
        private ArrayList ColComboAzontipek = new ArrayList();
        /// <summary>
        /// objectum letrehozasa egy tabla oszlopinformaciok tarolasara
        /// </summary>
        public ColCollection()
        {
        }
        /// <summary>
        /// kereses index szerint
        /// </summary>
        /// <param name="index">
        /// kivant index
        /// </param>
        /// <returns>
        /// kivant mezoinformacio vagy null
        /// </returns>
        public new Cols this[int index]
        {
            get
            {
                if (index<0 || index > this.Count - 1)
                    return null;
                return (Cols)base[index];
            }
            set { base[index] = value; }
        }
        /// <summary>
        /// kereses Column-nev alapjan
        /// </summary>
        /// <param name="name">
        /// kivant Column-nev
        /// </param>
        /// <returns>
        /// -1, vagy index
        /// </returns>
        public int IndexOf(string name)
        {
            return ColNames.IndexOf(name);
        }
        /// <summary>
        /// kereses Column-nev alapjan
        /// </summary>
        /// <param name="name">
        /// kivant Column-nev
        /// </param>
        /// <returns>
        /// mezoinformacio vagy null
        /// </returns>
        public Cols this[string name]
        {
            get 
            {
                int i = this.ColNames.IndexOf(name);
                if (i == -1)
                    return null;
                else
                    return this[i];
            }
        }
        /// <summary>
        /// Uj elem hozzaadasa
        /// </summary>
        /// <param name="value">
        /// Uj elem
        /// </param>
        /// <returns>
        /// Uj elem indexe
        /// </returns>
        public override int Add(object value)
        {
            int i = base.Add(value);
            Cols egycol = (Cols)value;
            ColNames.Add(egycol.ColumnName);
            ColComboAzontipek.Add(egycol.Comboazontip);
            return i;
        }
        /// <summary>
        /// Combo tipusu mezoinformacio kereses a Combo teljes azonositoja alapjan
        /// </summary>
        /// <param name="comboazontip">
        /// a kivant azonosito
        /// </param>
        /// <returns>
        /// </returns>
        public Cols GetByComboAzontip(string comboazontip)
        {
            int i =  ColComboAzontipek.IndexOf(comboazontip);
            if (i == -1)
                return null;
            else
                return this[i];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="colname"></param>
        /// <returns></returns>
        public Cols GetByColNames(string colname)
        {
            int i = ColNames.IndexOf(colname);
            if (i == -1)
                return null;
            else
                return this[i];
        }
        /// <summary>
        /// Torles
        /// </summary>
        public override void Clear()
        {
            base.Clear();
            ColNames.Clear();
        }
    }
    /// <summary>
    /// Egy tabla egy oszlopanak informacioi
    /// </summary>
    public class Cols:DataColumn
    {
        private FakUserInterface _fak;
        private Tablainfo _tabinfo;
        /// <summary>
        /// A tablainformacioja
        /// </summary>
        public Tablainfo Tablainfo
        {
            get { return _tabinfo; }
        }
        /// <summary>
        /// a Cols objectum DataColumn-ra alakitva
        /// </summary>
        public DataColumn Column
        {
            get { return (DataColumn)this; }
        }
        private Comboinfok _comboinfok = null;
        /// <summary>
        /// Comboe = true eseten a normal comboinformacio vagy null
        /// </summary>
        public Comboinfok Combo_Info
        {
            get { return _comboinfok; }
            set { _comboinfok = value; }
        }
        private KulonlegesComboinfok _comboazontipcombo = null;
        /// <summary>
        /// Comboe = true eseten a kulonleges comboinformacio vagy null
        /// </summary>
        public KulonlegesComboinfok ComboAzontipCombo
        {
            get { return _comboazontipcombo; }
            set { _comboazontipcombo = value; }
        }
        private TablainfoTag _aktualtag;
        /// <summary>
        /// A TablainfoTag-ja
        /// </summary>
        public TablainfoTag AktualTag
        {
            get { return _aktualtag; }
        }
        private Cols _kiegcol = null;
        /// <summary>
        /// A kiegeszito oszlopinformacio vagy null
        /// </summary>
        public Cols Kiegcol
        {
            get { return _kiegcol; }
            set { _kiegcol = value; }
        }
        private int _inputmaxlength = 0;
        /// <summary>
        /// Az input maximalis hossza
        /// </summary>
        public int InputMaxLength
        {
            get { return _inputmaxlength; }
            set { _inputmaxlength = value; }
        }
        private int _columnsize = 0;
        /// <summary>
        /// column meret adatbazis szerint
        /// </summary>
        public int DbaseColumnSize
        {
            get { return _columnsize; }
        }
        private string _adathossz = "";
        /// <summary>
        /// adat hossza
        /// </summary>
        public string Adathossz
        {
            get { return _adathossz; }
        }

        private bool _isunique = false;
        /// <summary>
        /// Egyedi egy tartalomjegyzeken belul?
        /// </summary>
        public bool IsUnique
        {
            get { return _isunique; }
        }
        private bool _isallunique = false;
        /// <summary>
        /// Teljesen egyedi?
        /// </summary>
        public bool IsAllUnique
        {
            get { return _isallunique; }
        }
        private bool _lehetures = false;
        /// <summary>
        /// Az oszlop tartalma lehet ures?
        /// </summary>
        public bool Lehetures
        {
            get { return _lehetures; }
            set { _lehetures = value; }
        }
        private bool _isidentity = false;
        /// <summary>
        /// Az oszlop identity?
        /// </summary>
        public bool IsIdentity
        {
            get { return _isidentity; }
        }
        private bool _isAllowDbNull = false;
        /// <summary>
        /// Lehet null?
        /// </summary>
        public bool IsAllowDbNull
        {
            get { return _isAllowDbNull; }
        }
        private string _tartalom = "";
        /// <summary>
        /// aktualis tartalom
        /// </summary>
        public string Tartalom
        {
            get { return _tartalom; }
            set { _tartalom = value; }
        }
        private string _origtartalom = "";
        /// <summary>
        /// eredeti tartalom
        /// </summary>
        public string OrigTartalom
        {
            get { return _origtartalom; }
            set { _origtartalom = value; }
        }
        private string _sorszov = "";
        /// <summary>
        /// Soros megjelenitesnel a szoveg
        /// </summary>
        public string Sorszov
        {
            get { return _sorszov; }
        }
        private string _tooltip;
        /// <summary>
        /// tooltip szoveg
        /// </summary>
        public string ToolTip
        {
            get { return _tooltip; }
            set { _tooltip = value; }
        }
        private int _tizedesek;
        /// <summary>
        /// tizedesek szama az adatbazis definicio szerint
        /// </summary>
        public int Tizedesek
        {
            get { return _tizedesek; }
        }
        private bool _lathato = true;
        /// <summary>
        /// GridView-ban lathato oszlop?
        /// </summary>
        public bool Lathato
        {
            get { return _lathato; }
            set { _lathato = value; }
        }
        private int _minimum = 0;
        /// <summary>
        /// numerikus oszlop minimum erteke
        /// </summary>
        public int Minimum
        {
            get { return _minimum; }
        }
        private int _maximum = 0;
        /// <summary>
        /// numerikus oszlop maximum erteke
        /// </summary>
        public int Maximum
        {
            get { return _maximum; }
        }
        private bool _kellselect = false;
        /// <summary>
        /// Select-tel ellenorzendo-e a bevitt ertek
        /// </summary>
        public bool Kellselect
        {
            get { return _kellselect; }
        }
        private string _comboazontip = "";
        /// <summary>
        /// Comboe = true eseten a combotabla teljes azonositoja
        /// </summary>
        public string Comboazontip
        {
            get { return _comboazontip; }
        }
        private bool _comboe = false;
        /// <summary>
        /// Combo mezo?
        /// </summary>
        public bool Comboe
        {
            get { return _comboe; }
            set { _comboe = value; }
        }
        private string _comboaktfileba = "";
        /// <summary>
        /// combotablabol ez kerul rogzitesre
        /// </summary>
        public string ComboAktFileba
        {
            get { return _comboaktfileba; }
            set { _comboaktfileba = value; }
        }
        private string _comboaktszoveg = "";
        /// <summary>
        /// combotablabol ez kerul megjelenitesre
        /// </summary>
        public string ComboAktSzoveg
        {
            get { return _comboaktszoveg; }
            set { _comboaktszoveg = value; }
        }
        /// <summary>
        /// true, ha egyedi vagy teljesen egyedi
        /// </summary>
        public bool Letezoe
        {
            get{return _isallunique||_isunique;}
        }
        private string _format = "";
        /// <summary>
        /// megjeleniteshez formatum
        /// </summary>
        public string Format
        {
            get { return _format; }
        }
        private bool _readonly = false;
        /// <summary>
        /// Csak olvas?
        /// </summary>
        public bool Csakolvas
        {
            get { return _readonly; }
            set { _readonly = value; }
        }
        private bool _checkboxe = false;
        /// <summary>
        /// Az inputmezot Checbox-nak tervezem?
        /// </summary>
        public bool Checkboxe
        {
            get { return _checkboxe; }
            set { _checkboxe = value; }
        }
        private string _checkyes = "";
        /// <summary>
        /// Ha Checkboxe, Checked eseten a tarolando ertek
        /// </summary>
        public string Checkyes
        {
            get { return _checkyes; }
            set { _checkyes = value; }
        }
        private string _checkno = "";
        /// <summary>
        /// Ha Checkboxe, UnChecked eseten a tarolando ertek
        /// </summary>
        public string Checkno
        {
            get { return _checkno; }
            set { _checkno = value; }
        }
        private bool _inputlathato = false;
        /// <summary>
        /// Lathato-e az inputcontrol
        /// </summary>
        public bool Inputlathato
        {
            get { return _inputlathato; }
            set { _inputlathato = value; }
        }
        private bool _kellmezoellenorzes = false;
        /// <summary>
        /// A bevitt adatnak letezo mezonevnek, vagy vesszovel elvalasztott mezoneveknek kell lennie?
        /// </summary>
        public bool Kellmezoellenorzes
        {
            get { return _kellmezoellenorzes; }
            set { _kellmezoellenorzes = value; }
        }
        private MezoTag _egytag = null;
        /// <summary>
        /// A mezoelemhez tartozo inputinformacio
        /// </summary>
        public MezoTag EgyTag
        {
            get { return _egytag; }
            set { _egytag = value; }
        }
        private string _szamitasieljaras = "";
        /// <summary>
        /// 
        /// </summary>
        public string SzamitasiEljaras
        {
            get { return _szamitasieljaras; }
            set { _szamitasieljaras = value; }
        }
        private string[] _szamitasiparameterek = null;
        /// <summary>
        /// 
        /// </summary>
        public string[] SzamitasiParameterek
        {
            get { return _szamitasiparameterek; }
            set { _szamitasiparameterek = value; }
        }
        /// <summary>
        /// objectum letrehozas
        /// </summary>
        /// <param name="colname">
        /// az oszlop neve
        /// </param>
        /// <param name="datatype">
        /// az oszlop adattipusa
        /// </param>
        /// <param name="oszlszov">
        /// oszlopos megjeleniteshez a szoveg
        /// </param>
        /// <param name="maxlength">
        /// maximalis adathossz
        /// </param>
        /// <param name="lathato">
        /// lathato?
        /// </param>
        /// <param name="tabinfo">
        /// az objectum tablainformacioja
        /// </param>
        /// <param name="tooltip">
        /// tooltip szoveg
        /// </param>
        public Cols(string colname, string datatype, string oszlszov, int maxlength,bool lathato,Tablainfo tabinfo,string tooltip)
        {
            ColumnName = colname;
            DataType = System.Type.GetType(datatype);
            Caption = oszlszov;
            ToolTip = tooltip;
            _lathato = lathato;
            _inputmaxlength = maxlength;
            int i = Caption.Length;
            if (datatype == "System.String")
            {
                if (i > maxlength)
                    MaxLength = i;
                else
                    MaxLength = _inputmaxlength;
            }
            if (Numeric(DataType))
                DefaultValue = "0";
            _tabinfo = tabinfo;
            _aktualtag = tabinfo.TablaTag;
        }
        /// <summary>
        /// objectum eloallitasa schematabla sor alapjan
        /// </summary>
        /// <param name="drow">
        /// a schematabla sora
        /// </param>
        /// <param name="tabinfo">
        /// az objectum tablainformacioja
        /// </param>
        public Cols(DataRow drow, Tablainfo tabinfo)
        {
            _tabinfo = tabinfo;
            _aktualtag = tabinfo.TablaTag;
            ColumnName = drow["ColumnName"].ToString().Trim();
            _inputmaxlength = (int)drow["ColumnSize"];
            _columnsize = _inputmaxlength;
            DataType = (System.Type)drow["DataType"];
            ReadOnly = (bool)drow["IsReadOnly"];
            _readonly = ReadOnly;
            _isidentity = (bool)drow["IsAutoIncrement"];
            _lehetures = (bool)drow["AllowDBNull"];
            _isAllowDbNull = _lehetures;
            _inputlathato = false;
            _tartalom = "";
            _origtartalom = "";
            _tizedesek = 0;
            if (!_isidentity)
            {
                if (Numeric(DataType))
                {
                    if (!DataType.ToString().Contains("Int"))
                        _tizedesek = Convert.ToInt16(drow["NumericScale"].ToString());
                    DefaultValue = "0";
                }
                else if (DataType.ToString() == "System.DateTime")
                {
                    _inputmaxlength = 11;
                    if (ColumnName == "LAST_MOD")
                    {
                        _inputmaxlength = 30;
                    }
                }
                else
                    DefaultValue = "";
            }
            else
            {
                tabinfo.IdentityColumnName = ColumnName;
                tabinfo.IdentityColumnIndex = tabinfo.Adattabla.Columns.IndexOf(ColumnName);
            }
        }
        /// <summary>
        /// numerikus-e a mezo adattipusa
        /// </summary>
        /// <param name="datatype">
        /// adattipus
        /// </param>
        /// <returns>
        /// true: igen
        /// </returns>
        public bool Numeric(System.Type datatype)
        {
            string[] typest = new string[8];
            typest[0] = "System.Double";
            typest[1] = "System.Int16";
            typest[2] = "System.Int32";
            typest[3] = "System.Int64";
            typest[4] = "System.Decimal";
            typest[5] = "System.UInt16";
            typest[6] = "System.UInt32";
            typest[7] = "System.UInt64";
            for (int i = 0; i < typest.Length; i++)
            {
                if (datatype.ToString() == typest[i])
                    return true;
            }
            return false;
        }
        /// <summary>
        /// mezoinformaciok beallitasa a leirotablainformacio megfelelo sora alapjan
        /// </summary>
        /// <param name="dr">
        /// a leirotablainfo megfelelo sora
        /// </param>
        /// <param name="fak">
        /// fak
        /// </param>
        public void Beallitasok(DataRow dr, FakUserInterface fak)
        {
            _fak = fak;
            string tartal = dr["READONLY"].ToString().Trim();
            bool leirotartal = _tabinfo.KellOszlopSzov;
            bool termeszetes = _tabinfo.Azon.Substring(0, 2).Trim() == "T" && !_tabinfo.TablaTag.Azonositok.Azontip.Contains("TARTAL");
            DateTime dt;
            if (tartal == "N")
            {
                if (!termeszetes)
                {
                    switch (this.ColumnName)
                    {
                        case "SZAMITASNEV":
                            _readonly = true;
                            ReadOnly = true;
                            break;
                        case "SZAMITASPARAMOK":
                            _readonly = true;
                            ReadOnly = true;
                            break;
                        default:
                            _readonly = false;
                            ReadOnly = false;
                            break;
                    }
                }
                else
                {
                    _readonly = false;
                    ReadOnly = false;
                }
            }
            else if (tartal == "I")
            {
                _readonly = true;
                ReadOnly = true;
            }

            tartal = dr["DEFERT"].ToString().Trim();
            if (tartal != "")
            {
                DefaultValue = (object)tartal;
                if (DataType.ToString() == "System.DateTime")
                {
                    dt = Convert.ToDateTime(tartal);
                    if (ColumnName != "LAST_MOD")
                        DefaultValue = dt.ToShortDateString();
                    else
                        DefaultValue = dt.ToString();
                }
            }
            else if (Combo_Info != null)
                DefaultValue = Combo_Info.DefSzovegbe;
            _szamitasiparameterek = null;
            _szamitasieljaras = "";
            tartal = dr["SZAMITASNEV"].ToString();
            if (tartal != "")
            {
                string tartal1 = dr["SZAMITASPARAMOK"].ToString();
                if (tartal1 != "")
                {
                    _szamitasieljaras = tartal;
                    char[] vesszo = new char[1];
                    vesszo[0] = Convert.ToChar(",");
                    _szamitasiparameterek = tartal1.Split(vesszo);
                }
            }
            switch (ColumnName)
            {
                case "AZON":
                    if (_tabinfo.Tablanev != "BASE")
                        DefaultValue = _tabinfo.Azon;
                    break;
                case "TERMSZARM":
                    DefaultValue = _tabinfo.TermSzarm;
                    break;
                case "SZINT":
                    if (_tabinfo.Tablanev != "BASE")
                        DefaultValue = _tabinfo.Szint;
                    break;
                case "ADATFAJTA":
                    DefaultValue = _tabinfo.Adatfajta;
                    break;
                case "PARENT":
                    if (_tabinfo.Tablanev == "BASE")
                        DefaultValue = 0;
                    else
                    {
                        DefaultValue = _tabinfo.NextParent.ToString();
                        if (DefaultValue.ToString() == "")
                            DefaultValue = "0";
                    }
                    break;
                case "KODTIPUS":
                    DefaultValue = _tabinfo.Kodtipus;
                    break;
                case "COMBOAZONTIP":
                    _comboe = true;
                    tartal = dr["COMBOAZONTIP"].ToString().Trim();
                    fak.ComboInfok.ComboazontipCombok.AttachToComboinfok(this);
                    break;
                case "AZONTIP":
                    if (!ReadOnly)
                    {
                        _comboe = true;
                        fak.ComboInfok.ComboazontipCombok.AttachToComboinfok(this);
                    }
                    else
                        _comboe = false;
                    break;
                case "AZONTIP1":
                    if (!ReadOnly)
                    {
                        _comboe = true;
                        DefaultValue = _tabinfo.Azontip1;
                        if ("CO".Contains(_tabinfo.Adatfajta))
                        {
                            KulonlegesComboinfok info = null;
                            if (_tabinfo.Adatfajta == "O")
                                info = fak.ComboInfok.LehetOsszefCombok;
                            else
                                info = fak.ComboInfok.LehetCsoportCombok;
                            info.AttachToComboinfok(this);
                        }
                        else
                            fak.ComboInfok.ComboazontipCombok.AttachToComboinfok(this);
                    }
                    else
                        _comboe = false;
                    break;
                case "AZONTIP2":
                    if (!ReadOnly)
                    {
                        _comboe = true;
                        DefaultValue = _tabinfo.Azontip2;
                        if (_tabinfo.Adatfajta == "O")
                            fak.ComboInfok.LehetOsszefCombok.AttachToComboinfok(this);
                        else
                            fak.ComboInfok.ComboazontipCombok.AttachToComboinfok(this);
                    }
                    else
                        _comboe = false;
                    break;
                case "SORSZOV":
                    if (leirotartal)
                        _comboazontip = "SZRK9999";
                    else
                        _comboazontip = "";
                    break;
                case "OSZLSZOV":
                    if (leirotartal)
                        _comboazontip = "SZRK9998";
                    else
                        _comboazontip = "";
                    break;
            }
            tartal = dr["LATHATO"].ToString().Trim();
            if (tartal == "" || tartal == "I")
            {
                if (!termeszetes)
                {
                    switch (this.ColumnName)
                    {
                        case "SZAMITASNEV":
                            _lathato = false;
                            break;
                        case "SZAMITASPARAMOK":
                            _lathato = false;
                            break;
                        default:
                            _lathato = true;
                            break;
                    }
                }
                else
                    _lathato = true;
            }
            else
                _lathato = false;
            tartal = dr["LEHETURES"].ToString().Trim();
            if (tartal == "I")
                _lehetures = true;
            else if (tartal == "N")
                _lehetures = false;
            if (DataType.ToString() == "System.DateTime")
            {
                if (!_isAllowDbNull)
                    _lehetures = false;
                else
                    _lehetures = true;
            }
            tartal = dr["INPUTLATHATO"].ToString().Trim();
            if (tartal == "I")
                _inputlathato = true;
            tartal = dr["KELLMEZOELLENORZES"].ToString().Trim();
            if (tartal == "I")
                _kellmezoellenorzes = true;
            if (_comboazontip == "" && (ColumnName != "SORSZOV" && ColumnName != "OSZLSZOV" || leirotartal))
            {
                tartal = dr["COMBOAZONTIP"].ToString().Trim();
                _comboazontip = tartal;
            }
            if (_comboazontip != "")
                _comboe = true;
            _sorszov = dr["SORSZOV"].ToString().Trim();
            if (_sorszov == "")
                _sorszov = ColumnName;
            _tooltip = dr["TOOLTIP"].ToString().Trim();
            Caption = dr["OSZLSZOV"].ToString().Trim();
            if (Caption == "")
                Caption = ColumnName;
            tartal = dr["ADATHOSSZ"].ToString().Trim();
            if (tartal == "")
                tartal = "0";
            _adathossz = tartal;
            if (tartal != "0" && Numeric(DataType))
                _inputmaxlength = Convert.ToInt32(tartal);
            tartal = dr["MINIMUM"].ToString().Trim();
            if (tartal == "")
                tartal = "0";
            _minimum = Convert.ToInt32(tartal);
            tartal = dr["MAXIMUM"].ToString().Trim();
            if (tartal == "")
                tartal = "0";
            if (tartal != "0")
            {
            }
            _maximum = Convert.ToInt32(tartal);
            tartal = dr["MAXIMUM"].ToString().Trim();
            if (tartal == "")
                tartal = "0";
            if (tartal != "0")
            {
            }
            tartal = dr["KELLSELECT"].ToString().Trim();
            _kellselect = false;
            if (tartal == "I")
                _kellselect = true;
            tartal = dr["ISUNIQUE"].ToString().Trim();
            if (tartal == "I")
                _isunique = true;
            tartal = dr["ISALLUNIQUE"].ToString().Trim();
            if (tartal == "I")
                _isallunique = true;
            _format = dr["FORMAT"].ToString().Trim();
            tartal = dr["CHECKBOXE"].ToString().Trim();
            if (tartal == "I")
            {
                _checkyes = dr["CHECKYES"].ToString().Trim();
                _checkno = dr["CHECKNO"].ToString().Trim();
                if (_checkyes != "" && _checkno != "")
                    _checkboxe = true;
            }

            if (_comboe)
            {
                _tabinfo.ComboColumns.Add(this);
                if (ColumnName != "SORSZOV" && ColumnName != "OSZLSZOV")
                {
                    _kiegcol = new Cols(ColumnName + "_K", "System.String", Caption, this.InputMaxLength, _lathato, _tabinfo, this.ToolTip);
                    _tabinfo.KiegColumns.Add(_kiegcol);
                    _lathato = false;
                }
            }
        }
        /// <summary>
        /// Comboe = true  tipusu oszlop comboinformacioinak beallitasa
        /// </summary>
        public void Combobeallit()
        {
            if (ColumnName == "SORSZOV" )
            {
                if (_tabinfo.Tablanev == "TARTAL" || _tabinfo.Tablanev == "LEIRO")
                {
                    Comboinfok sinfo = _fak.ComboInfok.ComboinfoKeres("SZRK9999");
                    if (sinfo != null)
                    {
                        sinfo.AttachToComboinfok(this);
                        MaxLength = sinfo.Maxhossz;
                    }
                }
            }
            else if (ColumnName == "OSZLSZOV")
            {
                if (_tabinfo.Tablanev == "TARTAL" || _tabinfo.Tablanev == "LEIRO")
                {
                    Comboinfok oinfo = _fak.ComboInfok.ComboinfoKeres("SZRK9998");
                    if (oinfo != null)
                    {
                        oinfo.AttachToComboinfok(this);
                        MaxLength = oinfo.Maxhossz;
                    }
                }
            }
            else if (_comboazontip != "")
            {
                Comboinfok egycomboinfo = _fak.ComboInfok.ComboinfoKeres(_comboazontip);
                if (egycomboinfo != null)
                {
                    egycomboinfo.AttachToComboinfok(this);
                    _kiegcol.MaxLength = egycomboinfo.Maxhossz;
                    _kiegcol.InputMaxLength = egycomboinfo.Maxhossz;

                    try
                    {
                        MaxLength = _kiegcol.MaxLength;
                        InputMaxLength = _kiegcol.MaxLength;
                    }
                    catch
                    {
                    }
               }
            }
            else if (_comboazontipcombo.Maxhossz > _kiegcol.MaxLength)
            {
                _kiegcol.MaxLength = _comboazontipcombo.Maxhossz;
                _kiegcol.InputMaxLength = _comboazontipcombo.Maxhossz;
                try
                {
                    MaxLength = _kiegcol.MaxLength;
                    InputMaxLength = _kiegcol.MaxLength;
                }
                catch
                {
                }
            }
        }
    }
}