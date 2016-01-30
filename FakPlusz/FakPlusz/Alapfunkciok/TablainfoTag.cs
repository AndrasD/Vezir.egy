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
    /// A tablainformaciot es a hozza tartozo leirotablainformaciot osszefogo objectumok listaja
    /// </summary>
    public class TablainfoTagCollection : ArrayList
    {
        ArrayList azontipek = new ArrayList();
        ArrayList azonok = new ArrayList();
        ArrayList tablanevek = new ArrayList();
        ArrayList kodtipusok = new ArrayList();
        ArrayList tagok = new ArrayList();
        ArrayList sorrendek = new ArrayList();

        /// <summary>
        /// kereses index szerint
        /// </summary>
        /// <param name="index">
        /// kivant index
        /// </param>
        /// <returns>
        /// kivant TablainfoTag vagy null
        /// </returns>
        public new TablainfoTag this[int index]
        {
            get
            {
                if (index < 0 || index > this.Count - 1)
                    return null;
                else
                    return (TablainfoTag)base[index];
            }
            set { base[index] = value; }
        }
        /// <summary>
        /// kereses teljes azonositoval
        /// </summary>
        /// <param name="name">
        /// kivant azonosito
        /// </param>
        /// <returns>
        /// kivant TablainfoTag vagy null
        /// </returns>
        public TablainfoTag this[string name]
        {
            get
            {
                int i = azontipek.IndexOf(name);
                if (i == -1)
                    return null;
                else
                    return (TablainfoTag)this[i];
            }
        }
        /// <summary>
        /// ??? talan nem kell
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override int IndexOf(object value)
        {
            return azontipek.IndexOf(value.ToString());
        }
        /// <summary>
        /// Uj  elem beszuras
        /// </summary>
        /// <param name="sorrend">
        /// sorrend
        /// </param>
        /// <param name="value">
        /// Uj elem
        /// </param>
        public new int Insert(int sorrend, object value)
        {
            TablainfoTag tag = (TablainfoTag)value;
            int index;
            if (tag.Azonositok.Azontip == "")
                index = azonok.IndexOf(tag.Azonositok.Azon);
            else
                index = azontipek.IndexOf(tag.Azonositok.Azontip);
            if (index == -1)
            {
                if (sorrendek.Count == 0)
                    index = 0;
                else if (sorrend >= Convert.ToInt32(sorrendek[sorrendek.Count - 1].ToString()))
                    index = sorrendek.Count;
                else
                {
                    for (index = 0; index < sorrendek.Count; index++)
                    {
                        if (sorrend < Convert.ToInt32(sorrendek[index].ToString()))
                            break;
                    }
                }
            }
            else if (tag.Forditott)
                index++;
            azontipek.Insert(index, tag.Azonositok.Azontip);
            azonok.Insert(index, tag.Azonositok.Azon);
            kodtipusok.Insert(index, tag.Azonositok.Kodtipus);
            sorrendek.Insert(index, tag.Sorrend);
            tablanevek.Insert(index, tag.Azonositok.Tablanev);
            base.Insert(index, value);
            return index ;
        }
        /// <summary>
        /// elem torlese a collection-bol
        /// </summary>
        /// <param name="obj">
        /// torlendo elem
        /// </param>
        public override void Remove(object obj)
        {
            TablainfoTag tag = (TablainfoTag)obj;
            int i ;
            if(tag.Azonositok.Azontip=="")
                i=azonok.IndexOf(tag.Azonositok.Azon);
            else
                i=azontipek.IndexOf(tag.Azonositok.Azontip);
            azontipek.Remove(tag.Azonositok.Azontip);
            azonok.Remove(tag.Azonositok.Azon);
            tablanevek.Remove(tag.Azonositok.Tablanev);
            kodtipusok.Remove(tag.Azonositok.Kodtipus);
            sorrendek.Remove(tag.Sorrend);
            if (i != -1)
                base.RemoveAt(i);
        }
    }
    /// <summary>
    /// A tablainformaciot es a hozza tartozo leirotablainformaciot osszefogo objectum
    /// </summary>
    public class TablainfoTag
    {
        /// <summary>
        /// Az informaciokat tartalmazo sor indexe az informacios tablaban
        /// </summary>
        public int SorIndex
        {
            get { return _sorindex; }
            set { _sorindex = value; }
        }
        /// <summary>
        /// adattabla
        /// </summary>
        public AdatTabla Adattabla
        {
            get { return _adattabla; }
        }
        /// <summary>
        /// sorrend
        /// </summary>
        public int Sorrend
        {
            get { return _sorrend; }
        }
        /// <summary>
        /// Fak
        /// </summary>
        public FakUserInterface Fak
        {
            get { return _fak; }
        }
        /// <summary>
        /// Adattabla tablainformacioinak objectuma
        /// </summary>
        public Tablainfo Tablainfo
        {
            get { return _tabinfo; }
            set { _tabinfo = value; }
        }
        /// <summary>
        /// Leirotabla tablainformacioinak objectuma
        /// </summary>
        public Tablainfo LeiroTablainfo
        {
            get { return _leirotabinfo; }
        }
        /// <summary>
        /// Az adattabla azonositoi
        /// </summary>
        public Azonositok Azonositok
        {
            get { return _azonositok; }
        }
        /// <summary>
        /// A leirotabla azonositoi
        /// </summary>
        public Azonositok LeiroAzonositok
        {
            get { return _leiroazonositok; }
        }
        /// <summary>
        /// a forditott TablainfoTag (Csoportnal, osszefuggesnel)
        /// </summary>
        public TablainfoTag FordTag
        {
            get { return _fordtag; }
            set { _fordtag = value; }
        }
        /// <summary>
        /// true: a forditott TablainfoTag eseten
        /// </summary>
        public bool Forditott
        {
            get { return _forditott; }
        }
        /// <summary>
        /// true: ha a TERVEZO-vel ebben a futasban toroljuk, false, ha beszurjuk
        /// </summary>
        public bool Torolt
        {
            get { return _torolt; }
            set
            {
                _torolt = value;
                if (_torolt)
                    Remove();
                else
                    Insert();
            }
        }
        /// <summary>
        /// A megjelenitendo TreeView-ban ez lesz a Node-ja
        /// </summary>
        public TreeNode Node
        {
            get { return _node; }
        }
        /// <summary>
        /// A parent TablainfoTag, vagy null
        /// </summary>
        public TablainfoTag ParentTag
        {
            get { return _parenttag; }
        }
        /// <summary>
        /// A child TablainfoTagok
        /// </summary>
        public TablainfoTagCollection ChildTablainfoTagok
        {
            get { return _childTablainfoTagok; }
            set { _childTablainfoTagok = value; }
        }
        /// <summary>
        /// A Node ToolTipText eloallitasa/frissitese
        /// </summary>
        public void ToolTipText()
        {
            _azonositok.ToolTipText();
        }
        /// <summary>
        /// LEIRO vagy BASE sajat TablainfoTag objectum eloallitasa
        /// </summary>
        /// <param name="dt">
        /// Tartalmazza a LEIRO sajat leirasanak sorait vagy a BASE sorait
        /// </param>
        /// <param name="fak">
        /// Fak
        /// </param>
        public TablainfoTag(AdatTabla dt, FakUserInterface fak)
        {
            TablainfoTagIni(dt, -1, null, null, null, fak, false);
        }
        /// <summary>
        /// BASE tabla "Szarmazekos" illetve "Termeszetes" sorabol TablainfoTag eloallitasa
        /// </summary>
        /// <param name="dt">
        /// BASE sorait tartalmazo adattabla
        /// </param>
        /// <param name="sorindex">
        /// a kivant sor
        /// </param>
        /// <param name="fak">
        /// Fak
        /// </param>
        public TablainfoTag(AdatTabla dt, int sorindex, FakUserInterface fak)
        {
            TablainfoTagIni(dt, sorindex, null, null, null, fak, false);
        }
        /// <summary>
        /// TablainfoTag objectum letrehozasa (nem forditott) vagy aktualizalasa
        /// </summary>
        /// <param name="dt">
        /// Informaciot tartalmazo adattabla - BASE/TARTAL/LEIRO vagy valamelyik adattabla 
        /// </param>
        /// <param name="sorindex">
        /// az informacios sor indexe
        /// </param>
        /// <param name="tabinfo">
        /// A tablainformacio, ha aktualizalas
        /// </param>
        /// <param name="eredetitag">
        /// null
        /// </param>
        /// <param name="parenttag">
        /// parent tag, ha van
        /// </param>
        /// <param name="fak">
        /// Fak
        /// </param>
        public TablainfoTag(AdatTabla dt, int sorindex, Tablainfo tabinfo, TablainfoTag eredetitag, TablainfoTag parenttag, FakUserInterface fak)
        {
            TablainfoTagIni(dt, sorindex, tabinfo, eredetitag, parenttag, fak, false);
        }
        /// <summary>
        /// Forditott TablainfoTag objectum letrehozasa vagy aktualizalasa
        /// </summary>
        /// <param name="dt">
        /// Informaciot tartalmazo adattabla - BASE/TARTAL/LEIRO vagy valamelyik adattabla 
        /// </param>
        /// <param name="sorindex">
        /// az informacios sor indexe
        /// </param>
        /// <param name="tabinfo">
        /// A tablainformacio
        /// </param>
        /// <param name="eredetitag">
        /// az eredeti TablainfoTag
        /// </param>
        /// <param name="parenttag">
        /// parent tag
        /// </param>
        /// <param name="fak">
        /// Fak
        /// </param>
        /// <param name="forditott">
        /// true
        /// </param>
        public TablainfoTag(AdatTabla dt, int sorindex, Tablainfo tabinfo, TablainfoTag eredetitag, TablainfoTag parenttag, FakUserInterface fak, bool forditott)
        {
            TablainfoTagIni(dt, sorindex, tabinfo, eredetitag, parenttag, fak, forditott);
        }
        private int _sorindex;
        private AdatTabla _adattabla = null;
        private Azonositok _azonositok = null;
        private Azonositok _leiroazonositok = null;
        private bool _forditott;
        private bool _torolt = false;
        private TreeNode _node = null;
        private TablainfoTag _parenttag = null;
        private TablainfoTagCollection _childTablainfoTagok = new TablainfoTagCollection();
        private Tablainfo _tabinfo;
        private Tablainfo _leirotabinfo = null;
        private FakUserInterface _fak;
        private TablainfoTag _fordtag = null;
        private int _sorrend = -1;
        private bool termtablae = false;
        private string aktszint;
        private string akttablanev;
        private Tablainfo tartalinfo;
        private int tartalinfosorindex;
        private string owner;
        private string user;
        private void TablainfoTagIni(AdatTabla dt, int sorindex, Tablainfo tabinfo, TablainfoTag eredetitag, TablainfoTag parenttag, FakUserInterface fak, bool forditott)
        {
            termtablae = false;
            _fak = fak;
            _fak.ProgressRefresh();
            _adattabla = dt;
            _sorindex = sorindex;
            _tabinfo = tabinfo;
            _parenttag = parenttag;
            _forditott = forditott;
            if (_node == null)
                _node = new TreeNode();
            DataRow dr = null;
            if (sorindex != -1)
            {
                dr = dt.Rows[sorindex];
                if (dt.Columns.IndexOf("SORREND") != -1)
                    _sorrend = Convert.ToInt32(dr["SORREND"].ToString());
            }
            if (tabinfo == null || forditott && eredetitag._fordtag == null)
            {
                _azonositok = new Azonositok(this, dr, _sorindex, dt.TableName, fak);
                string szint = _azonositok.Szint;
                if (!"RUC".Contains(szint))
                    fak.Szintstring = szint;
                _leiroazonositok = new Azonositok(this, dr, _sorindex, dt.TableName, fak, true);
                fak.ProgressRefresh();
                switch (dt.TableName)
                {
                    case "LEIRO":
                        _azonositok.Szoveg = "Leiró tábla";
                        _leirotabinfo = new Tablainfo(dt, sorindex, this, _leiroazonositok, fak, true);
                        _leirotabinfo.NaploTablainfo = _fak.NaploTablainfok[0];
                        _leirotabinfo.NaploTabla = _fak.NaploTablak[0];
                        _leirotabinfo.Init(true);
                        _tabinfo = new Tablainfo(this, _azonositok, _fak);
                        _tabinfo.NaploTablainfo = _fak.NaploTablainfok[0];
                        _tabinfo.LeiroTablainfo = _leirotabinfo;
                        _tabinfo.Adattabla = _leirotabinfo.Adattabla;
                        _tabinfo.Adattabla.Tablainfo = _tabinfo;
                        _tabinfo.Beallit();
                        _tabinfo.LeiroVizsg();
                        if (_azonositok.Azon == "LEIR")
                            _tabinfo.Adattabla.LastSel = _azonositok.Selectstring;
                        break;
                    case "BASE":
                        if (sorindex == -1)     // BASE sajatmaga
                        {
                            _azonositok.Szoveg = "BASE tábla";
                            AdatTabla leirotabla = new AdatTabla("LEIRO");
                            _leirotabinfo = new Tablainfo(leirotabla, -1, this, _leiroazonositok, fak, true);
                            _leirotabinfo.NaploTablainfo = _fak.NaploTablainfok[0];
                            _leirotabinfo.NaploTabla = _fak.NaploTablak[0];
                            _leirotabinfo.Init(false);
                            _tabinfo = new Tablainfo(dt, sorindex, this, _azonositok, _fak, false);
                            _tabinfo.NaploTablainfo = _fak.NaploTablainfok[0];
                            _tabinfo.LeiroTablainfo = _leirotabinfo;
                            _tabinfo.Init(false);
                            _tabinfo.LeiroVizsg();
                            _fak.Combokupdate(this);
                        }
                        else if (_azonositok.Tablanev != "" && !forditott)
                        {
                            fak.ProgressRefresh();
                            AdatTabla tartaltabla = new AdatTabla("TARTAL");
                            AdatTabla leirotabla = new AdatTabla("LEIRO");
                            _leirotabinfo = new Tablainfo(leirotabla, -1, this, _leiroazonositok, fak, true);
                            _leirotabinfo.NaploTablainfo = _fak.NaploTablainfok[0];
                            _leirotabinfo.NaploTabla = _fak.NaploTablak[0];
                            _leirotabinfo.Init(true);
                            _tabinfo = new Tablainfo(tartaltabla, -1, this, _azonositok, fak);
                            _tabinfo.LeiroTablainfo = _leirotabinfo;
                            _tabinfo.Init(true);
                            _tabinfo.LeiroVizsg();
                            if (!fak.BajVan)
                            {
                                for (int i = 0; i < tartaltabla.Rows.Count; i++)
                                {
                                    fak.ProgressRefresh();
                                    DataRow dr1 = tartaltabla.Rows[i];
                                    owner = dr1["OWNER"].ToString();
                                    bool kell = false;
                                    if (fak.Alkalmazas == "TERVEZO" || owner == fak.AlkalmazasId || owner == fak.Alkalmazas)
                                        kell = true;
                                    else
                                    {
                                        char[] vesszo = new char[1];
                                        vesszo[0] = Convert.ToChar(",");
                                        string[] userek = dr1["USEREK"].ToString().Split(vesszo);
                                        if (owner == "" && userek[0] == "")
                                            kell = true;
                                        else
                                        {
                                            for (int j = 0; j < userek.Length; j++)
                                            {
                                                if (userek[j] != "" && userek[j] == fak.Alkalmazas)
                                                {
                                                    kell = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    if (kell)
                                    {
                                        TablainfoTag tartaltag = new TablainfoTag(tartaltabla, i, null, null, this, fak);
                                        if ("CO".Contains(_azonositok.Adatfajta))
                                        {
                                            fak.ProgressRefresh();
                                            tartaltag.FordTag = new TablainfoTag(tartaltabla, i, _tabinfo, tartaltag, this, fak, true);
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case "TARTAL":
                        if (!forditott)
                        {
                            akttablanev = _azonositok.Tablanev;
                            string akttermszarm = _azonositok.Termszarm.Trim();
                            aktszint = _azonositok.Szint;
                            string kodtipus = _azonositok.Kodtipus;
                            termtablae = akttermszarm == "T";
                            owner = _azonositok.Ownerid;
                            user = _azonositok.User;
                            AdatTabla adattabla = new AdatTabla(akttablanev);
                            if (akttablanev == "LEIRO")
                                _leirotabinfo = fak.LeiroTag.Tablainfo;
                            else
                            {
                                if (adattabla.TableName == "KODTAB" || adattabla.TableName == "OSSZEF" || adattabla.TableName == "LISTA" || 
                                    adattabla.TableName=="STATISZTIKA" || adattabla.TableName=="ADATSZOLG" || adattabla.TableName=="NAPTARAK")
                                {
                                    _leirotabinfo = fak.SpecialisLeiroTabinfok.GetByAzontip(_leiroazonositok.Azontip);
                                    if(_leirotabinfo==null)
                                    {
                                        AdatTabla leiroadattabla = new AdatTabla("LEIRO");
                                        _leirotabinfo = new Tablainfo(leiroadattabla, -1, this, _leiroazonositok, fak, true);
                                        _leirotabinfo.NaploTablainfo = _fak.NaploTablainfok[0];
                                        _leirotabinfo.NaploTabla = _fak.NaploTablak[0];
                                        _leirotabinfo.Init(false);
                                        fak.SpecialisLeiroTabinfok.Add(_leirotabinfo);
                                    }
                                }
                                else
                                {
                                    AdatTabla leiroadattabla = new AdatTabla("LEIRO");
                                    _leirotabinfo = new Tablainfo(leiroadattabla, -1, this, _leiroazonositok, fak, true);
                                    _leirotabinfo.NaploTablainfo = _fak.NaploTablainfok[0];
                                    _leirotabinfo.NaploTabla = _fak.NaploTablak[0];
                                    _leirotabinfo.Init(false);
                                }

                            }
                            if (akttablanev == "LEIRO")
                            {
                                _tabinfo = fak.Tablainfok.GetBySzintPluszTablanev(kodtipus.Substring(0, 1), kodtipus.Substring(1));
                                if (_tabinfo != null)
                                    _tabinfo = _tabinfo.LeiroTablainfo;
                                else
                                    return;
                            }
                            else
                            {
                                _tabinfo = new Tablainfo(adattabla, -1, this, _azonositok, fak);
                                _tabinfo.LeiroTablainfo = _leirotabinfo;
                                if (_azonositok.Verzioinfok.AktualConnection != "")
                                {
                                    _tabinfo.Init(true);
                                    _tabinfo.LeiroVizsg();
                                    if (!fak.BajVan)
                                        fak.Combokupdate(this);
                                }
                            }
                        }
                        else
                        {
                            eredetitag.FordTag = this;
                            _tabinfo = eredetitag._tabinfo;
                            _leirotabinfo = eredetitag._leirotabinfo;
                        }
                        break;
                }
                if (_parenttag != null)
                {
                    Insert();
                    if (_parenttag.Azonositok.Szulotabla != "" && _parenttag.Azonositok.Szuloszint != "")
                    {
                        _azonositok.Szulotabla = _parenttag.Azonositok.Szulotabla;
                        _azonositok.Szuloszint = _parenttag.Azonositok.Szuloszint;
                    }
                    if (termtablae)
                    {
                        tartalinfo = fak.Tablainfok.GetByAzontip("SZRMTARTAL");
                        TablainfoTag parent = tartalinfo.TablaTag;
                        int sorrend;
                        if (tartalinfo.DataView.Count == 0)
                            sorrend = 100;
                        else
                            sorrend = Convert.ToInt32(tartalinfo.DataView[tartalinfo.DataView.Count - 1]["SORREND"].ToString()) + 1000;
                        string megnevezes = _azonositok.Szoveg + " tábla mezöi";
                        string kodtipus = aktszint + akttablanev;
                        string azontip = "SZRM" + aktszint + akttablanev;
                        DataRow row = tartalinfo.Find("KODTIPUS", aktszint + akttablanev);
                        if (row == null)
                        {
                            row = tartalinfo.Ujsor();
                            row["SORREND"] = sorrend;
                            sorrend = sorrend + 1000;
                        }
                        if (row["KODTIPUS"].ToString() != kodtipus)
                        {
                            tartalinfo.Modositott = true;
                            row["KODTIPUS"] = kodtipus;
                        }
                        if (row["SZOVEG"].ToString() != megnevezes)
                        {
                            tartalinfo.Modositott = true;
                            row["SZOVEG"] = megnevezes;
                        }
                        if (row["AZONTIP"].ToString() != azontip)
                        {
                            tartalinfo.Modositott = true;
                            row["AZONTIP"] = azontip;
                        }
                        if (row["OWNER"].ToString() != owner)
                        {
                            tartalinfo.Modositott = true;
                            row["OWNER"] = owner;
                        }
                        if (row["USEREK"].ToString() != user)
                        {
                            tartalinfo.Modositott = true;
                            row["USEREK"] = user;
                        }
                        row = tartalinfo.Find("KODTIPUS", aktszint + akttablanev);
                        tartalinfosorindex = tartalinfo.Rows.IndexOf(row);
                        TablainfoTag tag = tartalinfo.TablaTag.ChildTablainfoTagok[azontip];
                        if (tartalinfo.Modositott)
                        {
                            row["MODOSITOTT_M"] = 1;
                            fak.UpdateTransaction(new Tablainfo[] { tartalinfo });
                        }
                        if (tag == null) 
                        {
                            fak.ProgressRefresh();
                            tag = new TablainfoTag(tartalinfo.Adattabla, tartalinfosorindex, null, null, parent, fak);
                            fak.Combokupdate(tag);
                        }
                    }
                }
                _node.Text = _azonositok.Szoveg;
                _node.Tag = this;
                _azonositok.ToolTipText();
            }
        }
        /// <summary>
        /// Info update
        /// </summary>
        /// <param name="dt">
        /// Informacios tabla
        /// </param>
        /// <param name="sorindex">
        /// informacios sor indexe
        /// </param>
        public void TablainfoTagUpdate(AdatTabla dt, int sorindex)
        {
            _azonositok.AzonositokUpdate(dt.Rows[sorindex]);
            if (sorindex != -1 && dt.Columns.IndexOf("SORREND") != -1)
                _sorrend = Convert.ToInt32(dt.Rows[sorindex]["SORREND"].ToString());
            _node.Text = _azonositok.Szoveg;
            _azonositok.ToolTipText();
            if (_fordtag != null)
            {
                _fordtag._azonositok.AzonositokUpdate(dt.Rows[sorindex]);
                _fordtag._azonositok.ToolTipText();
            }
            if(_tabinfo!=null)
                _fak.Combokupdate(this);
        }
        /// <summary>
        /// lancbol remove-ol
        /// </summary>
        public void Remove()
        {
            _parenttag.Node.Nodes.Remove(_node);
            _parenttag.ChildTablainfoTagok.Remove(this);
            if (!_forditott)
                _fak.Tablainfok.Remove(this._tabinfo);
            if (_fordtag != null)
            {
                _parenttag.Node.Nodes.Remove(_fordtag.Node);
                _parenttag.ChildTablainfoTagok.Remove(_fordtag);
            }
        }
        /// <summary>
        /// lancba beszur
        /// </summary>
        public void Insert()
        {
            int insind =_parenttag.ChildTablainfoTagok.Insert(_sorrend, this);
            if (_forditott)
                insind++;
            _parenttag.Node.Nodes.Insert(insind, _node);
            if (!_forditott)
            {
                _fak.Tablainfok.Add(this._tabinfo);
                if (this._tabinfo != null)
                {
                    Tablainfo leirotinfo = _tabinfo.LeiroTablainfo;
                    int conind = -1;
                    if (this._tabinfo.Tablanev == "BASE" || this._tabinfo.Tablanev == "TARTAL" || this._tabinfo.Azon == "SZRM" || this._tabinfo.Leiroe)
                        conind = 0;
                    else
                        conind = _fak.ConnectionStringArray.IndexOf(this._tabinfo.Adattabla.Connection);
                    if (conind != -1)
                    {
                        if (this._tabinfo.Tablanev == "VALTOZASNAPLO")
                        {
                            _fak.NaploTablainfok.Add(this._tabinfo);
                            this._tabinfo.NaploTablainfo = this._tabinfo;
                            _parenttag.Tablainfo.NaploTablainfo = this._tabinfo;
                            _parenttag.LeiroTablainfo.NaploTablainfo = _fak.NaploTablainfok[0];
                            if (conind == 0)
                            {
                                _fak.LeiroTag.Tablainfo.NaploTabla = _fak.NaploTablak[0];
                                _fak.LeiroTag.Tablainfo.NaploTablainfo = _fak.NaploTablainfok[0];
                                _fak.BaseTag.Tablainfo.NaploTabla = _fak.NaploTablak[0];
                                _fak.BaseTag.Tablainfo.NaploTablainfo = _fak.NaploTablainfok[0];
                            }
                        }
                        else if(_fak.NaploTablainfok.Count > conind)
                            this._tabinfo.NaploTablainfo = _fak.NaploTablainfok[conind];
                        this._tabinfo.NaploTabla = _fak.NaploTablak[conind];
                        if(_parenttag.Tablainfo!=null)
                            _parenttag.Tablainfo.NaploTabla = _fak.NaploTablak[conind];
                    }
                }
            }
            if (_fordtag != null)
            {
               insind = _parenttag.ChildTablainfoTagok.Insert(_sorrend+1, _fordtag);
               _parenttag.Node.Nodes.Insert(insind , _fordtag.Node);
            }
        }
    }
}


