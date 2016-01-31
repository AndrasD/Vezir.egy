using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Globalization;
using System.Drawing;
using System.Data;
using System.Threading;
using FakPlusz;
using FakPlusz.Alapcontrolok;
using FakPlusz.UserAlapcontrolok;
namespace FakPlusz.Alapfunkciok
{
    /// <summary>
    /// 
    /// </summary>
    partial class FakUserInterface
    {
        public TablainfoCollection Combohasznalok = new TablainfoCollection();
        /// <summary>
        /// 
        /// </summary>
        public TablainfoCollection NaploTablainfok = new TablainfoCollection();
        /// <summary>
        /// Naplozas DataViewk
        /// </summary>
        public DataTable[] NaploTablak = null;
        /// <summary>
        /// utolso update datumok rendszer/user/aktceg szintre
        /// </summary>
        public DateTime[] LastUpdateDateTime;
        /// <summary>
        /// utolso backup datumok rendszer/user/aktceg szintre
        /// </summary>
        public DateTime[] LastBackupDateTime;
        /// <summary>
        /// 
        /// </summary>
        public ProgressForm ProgressForm = new ProgressForm();
        public KezdetiAblak KezdetiForm = new KezdetiAblak();
        /// <summary>
        /// 
        /// </summary>
        public ArrayList ConnectionStringArray;
        /// <summary>
        /// 
        /// </summary>
        public DataTable Alkalmazasok = new DataTable();
        /// <summary>
        /// 
        /// </summary>
        public ArrayList AlkalmazasNevek = new ArrayList();
        /// <summary>
        /// 
        /// </summary>
        public ArrayList AlkalmazasIdk = new ArrayList();
        public Helpwindow HelpWindow = null;
        public bool Webe = false;
        /// <summary>
        /// objectum letrehozasa, FakUserInterface hivja az ott leirt parameterekkel
        /// a teljes strukturat, ami a FakPlusz mukodesehez kell, letrehozza
        /// </summary>
        /// <param name="alkalmazas">
        /// alkalmazas neve
        /// </param>
        /// <param name="form">
        /// main form
        /// </param>
        /// <param name="kellprogress">
        /// kell progressbar?
        /// </param>
        /// <param name="connstringek">
        /// connectionstringek tombje
        /// </param>
        /// <param name="adatbazisfajta">
        /// adatbazisfajta:Sql vagy MySql
        /// </param>
        /// <param name="mainvezerles">
        /// main vezerloinfo
        /// </param>
        /// <param name="kezeloid">
        /// kezelo id-je vagy -2
        /// </param>
        private void NewFakUserInterface(string alkalmazas, Form form, bool kellprogress, string[] connstringek, string adatbazisfajta, Vezerloinfo mainvezerles, int kezeloid, bool webe)
        {
            UserSzamitasok.UserSzamitasok.Init(this);
            _alkalmazas = alkalmazas;
            _mainvezerles = mainvezerles;
            _mainform = form;
            KezeloId = kezeloid;
            Webe = webe;
            if (adatbazisfajta == "MySql")
                _mysqle = true;
            else
                _mysqle = false;
            ConnectionStringArray = new ArrayList(connstringek);
            _rendszerconn = connstringek[0];
            _userconn = connstringek[1];
            _alapconnectionstringek[0] = connstringek[0];
            _alapconnectionstringek[1] = connstringek[1];
            NaploTablak = new DataTable[connstringek.Length];
            LastUpdateDateTime = new DateTime[connstringek.Length];
            LastBackupDateTime = new DateTime[connstringek.Length];
            NaploTablak[0] = new DataTable("VALTOZASNAPLO");
            NaploTablak[1] = new DataTable("VALTOZASNAPLO");
            LastUpdateDateTime[0] = DateTime.MinValue;
            LastUpdateDateTime[1] = DateTime.MinValue;
            int maxi = connstringek.Length;
            if (alkalmazas == "TERVEZO")
            {
                maxi = 3;
                if (!_rendszerconn.Contains("GITTA") && !_rendszerconn.Contains("localhost"))
                    _enyem = false;
            }
            for (int i = 2; i < maxi; i++)
            {
                _cegconnectionok.Add(connstringek[i]);
                NaploTablak[i] = new DataTable("VALTOZASNAPLO");
                LastUpdateDateTime[i] = DateTime.MinValue;
            }
            Sqlinterface.RendszerUserConn(_rendszerconn, _userconn);
            Sqlinterface.SetCegConnectionok((string[])_cegconnectionok.ToArray(typeof(string)));
            Sqlinterface.Cegconn(_cegconnectionok[0].ToString());
            //LastBackupDateTime[0] = Sqlinterface.GetLastBackupDate(_rendszerconn);
            //LastBackupDateTime[1] = Sqlinterface.GetLastBackupDate(_userconn);
            Sqlinterface.Select(NaploTablak[0], _rendszerconn, "VALTOZASNAPLO", "", "", true);
            NaploTablak[0].Rows.Clear();
            Sqlinterface.Select(NaploTablak[1], _userconn, "VALTOZASNAPLO", "", "", true);
            NaploTablak[1].Rows.Clear();
            for (int i = 2; i < maxi; i++)
            {
                //LastBackupDateTime[i] = Sqlinterface.GetLastBackupDate(connstringek[i]);
                Sqlinterface.Select(NaploTablak[i], connstringek[i], "VALTOZASNAPLO", "", "", true);
                NaploTablak[i].Rows.Clear();
            }
            //DataTable dtcegek = new DataTable();
            DataTable dtcegalkalmazasok = new DataTable();
            Sqlinterface.Select(Alkalmazasok, _rendszerconn, "KODTAB", " where KODTIPUS='Alkalm'", "", false);
            foreach (DataRow dr in Alkalmazasok.Rows)
            {
                string sorszam = dr["SORSZAM"].ToString();
                string nev = dr["SZOVEG"].ToString();
                AlkalmazasNevek.Add(nev);
                AlkalmazasIdk.Add(sorszam);
                if (_alkalmazas != "TERVEZO" && nev == _alkalmazas)
                    _alkalmazasid = sorszam;
            }
            if (_alkalmazas != "TERVEZO" && _alkalmazasid == "")
            {
                CloseProgress();
                FakPlusz.MessageBox.Show("Nincs " + _alkalmazas + " nevü alkalmazás!");
                _bajvan = true;
                return;
            }
            //Sqlinterface.Select(dtcegek, _userconn, "CEGEK", "", "CEG_ID", false);
            string kezdevho = DateTime.Today.ToShortDateString().Substring(0, 8) + ".01";
            DateTime modkezdetedatum = Convert.ToDateTime(kezdevho);
            DateTime modvegedatum = modkezdetedatum.AddMonths(1).AddDays(-1);
            string vegevho = DatumToString(modvegedatum);
            _aktintervallum = new DateTime[] { modkezdetedatum, modvegedatum };
            _shortdateintervallum = new string[] { kezdevho, vegevho };
            _verzioinfok = new VerzioinfoCollection();
            string[] szintek = new string[] { "R", "U", "C" };
            for (int i = 0; i < szintek.Length; i++)
            {
                Verzioinfok egyver = new Verzioinfok(this);
                _verzioinfok.Add(egyver, szintek[i]);
                egyver.VerzioinfoCollection = _verzioinfok;
            }
            //if (kellprogress)
            //  OpenProgress();

            SetProgressText("Alapinfok töltése");
            Rendszerversiontolt();
            ProgressRefresh();
            Userversiontolt();
            ProgressRefresh();
            AdatTabla leirotab = new AdatTabla("LEIRO");
            _leirotag = new TablainfoTag(leirotab, this);
            string[] szovegek = new string[] { "Rendszerszintû Combok", "Userszintû Combok", "Céginicializálás" };
            string[] azonok = new string[] { "RC", "UC", "CC" };
            if (!_bajvan)
            {
                _leirotablainfo = _leirotag.Tablainfo;
                _leironode = _leirotablainfo.TablaTag.Node;
                AdatTabla dt = new AdatTabla("BASE");
                Sqlinterface.Select((DataTable)dt, _rendszerconn, "BASE", "", "order by PARENT,SORREND", false);
                _basetag = new TablainfoTag(dt, this);
                _basenode = _basetag.Node;
                if (kellprogress)
                {
                    KezdetiForm.OpenKezdeti(this, szovegek, azonok);
                    //                   KezdetiForm.OpenKezdeti(this, null, null);
                    //                   _mainform.AddOwnedForm(KezdetiForm);
                }
                DataView fofaview = dt.DataView;
                fofaview.Table = (DataTable)dt;
                fofaview.Sort = "PARENT,SORREND";
                fofaview.RowFilter = "[parent] = '0'";
                SetProgressText("Rendszer információk töltése");
                TreeNode egynode;
                string baseszoveg;
                string azon;
                for (int i = 0; i < fofaview.Count; i++)
                {
                    baseszoveg = fofaview[i].Row["SZOVEG"].ToString();
                    azon = fofaview[i].Row["AZON"].ToString();
                    TablainfoTag tag = new TablainfoTag(dt, i, this);
                    if (kellprogress)
                        KezdetiForm.Sorkesz(baseszoveg, azon);
                    GyokerTablainfoTagok.Add(tag);
                    egynode = tag.Node;
                }
                foreach (TablainfoTag tag in GyokerTablainfoTagok)
                {
                    TablainfoTagokEpit(tag, fofaview);
                    if (_bajvan)
                        break;
                }
                if (kellprogress)
                {
                    //_mainform.RemoveOwnedForm(KezdetiForm);
                    //KezdetiForm.Visible = false;
                    //OpenProgress();
                }
                fofaview.Sort = "SORREND";
                if (!_bajvan)
                {
                    _leirotablainfo.Beallit();
                    _leirotablainfo.Combobeallit();
                    _basetag.LeiroTablainfo.Beallit();
                    _basetag.LeiroTablainfo.Combobeallit();
                    _basetag.Tablainfo.Beallit();
                    _basetag.Tablainfo.Combobeallit();
                    KezdetiForm.ShowProgress();
                    Combotoltes("R");
                    KezdetiForm.Sorkesz(szovegek[0], azonok[0]);
                    Combotoltes("U");
                    KezdetiForm.Sorkesz(szovegek[1], azonok[1]);
                }
            }
            string selstring = "";
            if (_bajvan)
            {
                string szov = this.BajInfo;
                if (szov == "")
                {
                    CloseProgress();
                    FakPlusz.MessageBox.Show("Programhiba!");
                }
            }
            else                                        // elso ceg adatai
            {
                for (int i = 0; i < _cegconnectionok.Count; i++)
                {
                    if (selstring == "")
                        selstring += " where ";
                    else
                        selstring += " or ";
                    selstring += "CEGCONNECTION='" + _cegconnectionok[i].ToString() + "'";
                }
                _ceginfo = Tablainfok.GetBySzintPluszTablanev("U", "CEGEK");
                int count = _ceginfo.DataView.Count;
                bool[] megvan = new bool[_cegconnectionok.Count];
                if (_ceginfo.DataView.Count < _cegconnectionok.Count)     // uj ceg(ek)
                {
                    for (int i = 0; i < _cegconnectionok.Count; i++)
                    {
                        string conn = _cegconnectionok[i].ToString();
                        for (int j = 0; j < _ceginfo.DataView.Count; j++)
                        {
                            DataRow dr = _ceginfo.DataView[j].Row;
                            if (dr["CEGCONNECTION"].ToString() == conn)
                            {
                                megvan[i] = true;
                                break;
                            }
                        }
                        if (!megvan[i])
                        {
                            DataRow dr = _ceginfo.Ujsor();
                            dr["SZOVEG"] = (count++).ToString() + ".cég";
                            dr["CEGCONNECTION"] = conn;
                        }

                    }
                }
                if (_alkalmazas != "TERVEZO")
                {
                    if (_ceginfo.Modositott)
                        UpdateTransaction(new Tablainfo[] { _ceginfo });
                    //_ceginfo.Adattabla.Rows.Clear();
                    //_ceginfo.Azonositok.Selectstring = selstring;
                    //Sqlinterface.Select(_ceginfo.Adattabla, _userconn, "CEGEK", selstring, _ceginfo.Azonositok.Orderstring, false);
                }
                if (!_bajvan)
                {
                    KezdetiForm.Sorkesz(szovegek[2], azonok[2]);
                    Cegadatok(0);
                    for (int i = 0; i < _tablainfok.Count; i++)
                    {
                        int conind = ConnectionStringArray.IndexOf(_tablainfok[i].Adattabla.Connection);
                        _tablainfok[i].NaploTablainfo = NaploTablainfok[conind];

                    }
                }

                if (!_bajvan)
                {
                    GyokerekEsGyokerChainek();
                    _comboinfok.Kiajanlinfo = _tablainfok.GetByAzontip("SZCTKIAJANL");
                    string azon = "";
                    foreach (Tablainfo info in _tablainfok)
                    {
                        if (info.KellVerzio && (info.LehetCombo || info.LehetOsszef || info.LehetCsoport))
                        {
                            string azontip = info.Azontip;
                            foreach (Tablainfo info1 in _tablainfok)
                            {
                                if (info != info1 && info1.Tablanev != "TARTAL" && info1.KellVerzio)
                                {
                                    if (info1.Tablanev != "OSSZEF")
                                    {
                                        foreach (Cols egycol in info1.ComboColumns)
                                        {
                                            if (egycol.Comboazontip == azontip)
                                            {
                                                if (info.ComboHasznalok.IndexOf(info1) == -1)
                                                {
                                                    info.ComboHasznalok.Add(info1);
                                                    if (Combohasznalok.IndexOf(info) == -1)
                                                        Combohasznalok.Add(info);
                                                }
                                            }
                                        }
                                    }
                                    else if ((info1.Azontip1 == azontip || info1.Azontip2 == azontip) && info.ComboHasznalok.IndexOf(info1) == -1)
                                    {
                                        info.ComboHasznalok.Add(info1);
                                        if (Combohasznalok.IndexOf(info) == -1)
                                            Combohasznalok.Add(info);
                                    }
                                }
                            }
                        }
                    }

                    CloseProgress();
                    KezdetiForm.CloseKezdeti();
                    //                   _mainform.RemoveOwnedForm(KezdetiForm);
                    //                   KezdetiForm.Visible = false;

                }
                else
                {
                    CloseProgress();
                    KezdetiForm.CloseKezdeti();
                    //                   _mainform.RemoveOwnedForm(KezdetiForm);
                    //                  KezdetiForm.Visible = false;
                }
                //                HelpWindow = new Helpwindow(this, Alkalmazas == "TERVEZO");
            }
        }
        private void GyokerekEsGyokerChainek()
        {
            TablainfoCollection cegpluszcegalatti = GetCegPluszCegalattiTermTablaInfok();
            foreach (Tablainfo egyinfo in cegpluszcegalatti)
            {
                if (egyinfo.FirstTermParentTabinfo == null && egyinfo.TermChildTabinfo.Count == 0)
                // se szulo ,se gyerek, esetleg egy ujabb lanc
                {
                    GyokerTablainfok.Add(egyinfo);
                    int i = cegpluszcegalatti.IndexOf(egyinfo);
                    string ident = egyinfo.IdentityColumnName;
                    for (int j = i + 1; j < cegpluszcegalatti.Count; j++)
                    {
                        Tablainfo adatinfo = cegpluszcegalatti[j];
                        if (adatinfo.Azontip != egyinfo.Azontip)
                        {
                            int k = adatinfo.TablaColumns.IndexOf(ident);
                            if (k != -1)
                            {
                                adatinfo.Gyokerek.Add(egyinfo);
                                egyinfo.TermChildTabinfo.Add(adatinfo);

                                if (adatinfo.TermChildTabinfo.Count == 0)
                                    gyokerchainek.Add(egyinfo.TermChildTabinfo);
                            }
                        }
                    }
                }
                else if (egyinfo.FirstTermParentTabinfo != null)
                {
                    int j = GyokerTablainfok.IndexOf(egyinfo.FirstTermParentTabinfo);
                    TablainfoCollection gyokchainek = (TablainfoCollection)gyokerchainek[j];
                    gyokchainek.Add(egyinfo);
                }
            }
        }
        /// <summary>
        /// Alkalmazasi adatbazisvaltas kovetesehez
        /// </summary>
        public void OpenProgress()
        {
            OpenProgress(ProgressForm.ProgressFormText);
        }
        public void OpenProgress(string progresstext)
        {
            ProgressForm.Text = progresstext;
            if (!_formfaload)
            {

                _formfaload = true;
                _mainform.AddOwnedForm(ProgressForm);
                ProgressForm.Visible = true;
                ProgressLabel = ProgressForm.ProgressLabel;
                ProgressLabel.Visible = true;
                ProgressBar = ProgressForm.ProgressBar;
                ProgressBar.Visible = true;
            }
        }
        /// <summary>
        /// Nem kell mar a kovetes
        /// </summary>
        public void CloseProgress()
        {
            if (_formfaload)
            {
                ProgressBar.Visible = false;
                ProgressLabel.Text = "";
                _formfaload = false;
                _mainform.RemoveOwnedForm(ProgressForm);
                ProgressForm.Visible = false;
                ProgressForm.Text = ProgressForm.ProgressFormText;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        public void SetProgressText(string text)
        {
            if (_formfaload)
            {
                ProgressLabel.Text = text;
                ProgressRefresh();
            }
        }
        /// <summary>
        /// Form refresh
        /// </summary>
        public void ProgressRefresh()
        {
            if (KezdetiForm.Open)
                KezdetiForm.RefreshForm();
            else if (_formfaload)
            {
                ProgressForm.Refresh();
            }

        }
        public void ShowHelp(string azonosito, bool felsoe, Control cont)
        {
            if (HelpWindow == null)
                HelpWindow = new Helpwindow(this, Alkalmazas == "TERVEZO" || _rendszerconn.Contains("GITTA") || _rendszerconn.Contains("localhost"), _mainform);
            System.Drawing.Point loc = new System.Drawing.Point(0, 0);
            if (!felsoe)
            {
                int locy = _mainform.Height - HelpWindow.Height;
                loc = new System.Drawing.Point(0, locy);
            }
            HelpWindow.Location = loc;
            if (!this._eventtilt)
                HelpWindow.Helpszerkeszt(azonosito, cont);
        }
        private void TablainfoTagokEpit(TablainfoTag Tag, DataView view)
        {
            int sorindex = 0;
            string baseszoveg;
            string azon;
            TablainfoTag egytag;
            DataRow elsosor = view[0].Row;
            view.RowFilter = "[PARENT]='" + Tag.Azonositok.Nextparent.ToString() + "'";
            AdatTabla dt = (AdatTabla)view.Table;
            if (view.Count == 0)
            {
                baseszoveg = elsosor["SZOVEG"].ToString();
                azon = elsosor["AZON"].ToString();
                sorindex = view.Table.Rows.IndexOf(elsosor);
                if (KezdetiForm != null)
                    KezdetiForm.Sorkesz(baseszoveg, azon);
                return;
            }
            for (int i = 0; i < view.Count; i++)
            {
                if (!_bajvan)
                {
                    DataRow dr = view[i].Row;
                    baseszoveg = dr["SZOVEG"].ToString();
                    azon = dr["AZON"].ToString();
                    sorindex = view.Table.Rows.IndexOf(dr);
                    egytag = new TablainfoTag(dt, sorindex, null, null, Tag, this);
                    if (KezdetiForm != null)
                        KezdetiForm.Sorkesz(baseszoveg, azon);
                    if (!_bajvan)
                    {
                        TablainfoTagokEpit(egytag, view);
                        if (_bajvan)
                            break;
                        view.RowFilter = "[PARENT]='" + Tag.Azonositok.Nextparent.ToString() + "'";
                    }
                }
            }
        }
        /// <summary>
        /// Adott szintu Tablainfo-k Combo informacioinak beallitasa
        /// </summary>
        /// <param name="szint">
        /// kivant szint
        /// </param>
        public void Combotoltes(string szint)
        {
            TablainfoCollection tabinfok = Tablainfok.GetBySzint(szint);
            SetProgressText(" Comboadatok töltése ");
            Tablainfo leirotabinfo;
            foreach (Tablainfo tabinfo in tabinfok)
            {
                ProgressRefresh();
                if (tabinfo.Tablanev != "LEIRO")
                {
                    if (tabinfo.Tablanev == "TARTAL" || tabinfo.Szint == szint)
                    {
                        tabinfo.Combobeallit();
                        leirotabinfo = tabinfo.LeiroTablainfo;
                        leirotabinfo.Combobeallit();
                        if (tabinfo.Szint == szint && tabinfo.TablaTag.Adattabla != null)
                        {
                            tabinfo.TablaTag.Azonositok.AzonositokUpdate(tabinfo.TablaTag.Adattabla.Rows[tabinfo.TablaTag.SorIndex]);
                            tabinfo.TablaTag.ToolTipText();
                        }
                    }
                }
                else
                    tabinfo.Combobeallit();
            }
        }
        /// <summary>
        /// I vagy nem I alapjan true vagy false allitas
        /// </summary>
        /// <param name="ertek">
        /// I vagy nem I
        /// </param>
        /// <returns>
        /// true: ha I
        /// </returns>
        public bool SetBoolByIgenNem(string ertek)
        {
            if (ertek == "I")
                return true;
            else
                return false;
        }
        /// <summary>
        /// hozzaferesi jogosultsag ertek alapjan
        /// </summary>
        /// <param name="ertek"></param>
        /// <returns></returns>
        public Base.HozferJogosultsag SetJogszint(string ertek)
        {
            if (ertek == "")
                return (Base.HozferJogosultsag)(-1);
            else
                return (Base.HozferJogosultsag)Convert.ToInt16(ertek);
        }
        /// <summary>
        /// Comboinformaciok update-je
        /// </summary>
        /// <param name="tag">
        /// TablainfoTag
        /// </param>
        public void Combokupdate(TablainfoTag tag)
        {
            bool saveevent = EventTilt;
            EventTilt = true;
            bool forditott = tag.Forditott;
            Tablainfo tabinfo = tag.Tablainfo;
            string egyazontip = tabinfo.Azontip;
            string adatfajta = tabinfo.Adatfajta;
            int i = _comboinfok.ComboazontipCombok.Find(tag);
            bool lehetcombo = tag.Azonositok.Lehetcombo;
            if (!lehetcombo)
            {
                if (i != -1)
                    _comboinfok.ComboazontipCombok.Deleteinfo(i);
                Comboinfok alapcombo = _comboinfok.AlapComboFind(tag);
                if (alapcombo != null)
                {
                    i = alapcombo.Find(tabinfo);
                    if (i != -1)
                        alapcombo.Deleteinfo(i);
                }
            }
            else if (!forditott)
            {
                _comboinfok.ComboazontipCombok.Infoba(tag);
                if (tag.Azonositok.Combofileba != "" && tag.Azonositok.Comboszovegbe != null)
                {
                    _comboinfok.AlapComboAdd(tag);
                    string azontip = tag.Azonositok.Azontip;
                    foreach (Comboinfok egyinf in _comboinfok.AlapCombok)
                    {
                        if (egyinf.Combotag.Azonositok.Azontip == azontip)
                        {
                            foreach (ComboBox cb in egyinf.ComboArray)
                            {
                                if (cb.Items.Count == egyinf.ComboFileinfo.Count)
                                {
                                    for (int k = 0; k < cb.Items.Count; k++)
                                        cb.Items[k] = egyinf.ComboInfo[k].ToString();
                                }
                                else
                                {
                                    cb.Items.Clear();
                                    cb.Items.AddRange((object[])egyinf.ComboInfo.ToArray(typeof(object)));
                                }
                                if (cb.Items.Count != 0)
                                {
                                    cb.SelectedIndex = 0;
                                }
                                MezoTag mezotag = (MezoTag)cb.Tag;
                                if (mezotag != null)
                                {
                                    Cols egycol = mezotag.Egycol;
                                    if (cb.Items.Count == 0)
                                    {
                                        egycol.ComboAktFileba = "0";
                                        egycol.ComboAktSzoveg = "";
                                    }
                                    else if (egycol.Combo_Info != null)
                                    {
                                        egycol.ComboAktFileba = egycol.Combo_Info.ComboFileinfo[0].ToString();
                                        egycol.ComboAktSzoveg = egycol.Combo_Info.ComboInfo[0].ToString();
                                    }
                                }

                            }
                        }

                    }
                }
            }
            if (tag.Azonositok.Lehetosszef)
                _comboinfok.LehetOsszefCombok.Infoba(tag);
            else
            {
                i = _comboinfok.LehetOsszefCombok.Find(tag);
                if (i != -1)
                    _comboinfok.LehetOsszefCombok.Deleteinfo(i);
            }
            if (tag.Azonositok.Lehetcsoport)
                _comboinfok.LehetCsoportCombok.Infoba(tag);
            else
            {
                i = _comboinfok.LehetCsoportCombok.Find(tag);
                if (i != -1)
                    _comboinfok.LehetCsoportCombok.Deleteinfo(i);
            }
            EventTilt = saveevent;
        }
        private bool Rendszeradatok(DateTime[] intervallum)
        {
            return (Rendszeradatok(intervallum, "R", false));
        }
        private bool Rendszeradatok(DateTime[] intervallum, bool force)
        {
            return (Rendszeradatok(intervallum, "R", force));
        }
        private bool Rendszeradatok(DateTime[] intervallum, string szintstring)
        {
            return Rendszeradatok(intervallum, szintstring, false);
        }
        private bool Useradatok(DateTime[] intervallum)
        {
            return (Rendszeradatok(intervallum, "U", false));
        }
        private bool Useradatok(DateTime[] intervallum, bool force)
        {
            return (Rendszeradatok(intervallum, "R", force));
        }
        private bool Rendszeradatok(DateTime[] intervallum, string szint, bool force)
        {
            if (!force && !_ujceg && intervallum[0].CompareTo(_aktintervallum[0]) == 0 && intervallum[1].CompareTo(_aktintervallum[1]) == 0)
                return true;
            _aktintervallum = intervallum;
            string kezdevho = DatumToString(_aktintervallum[0]);
            string vegevho = DatumToString(_aktintervallum[1]);
            _shortdateintervallum = new string[] { kezdevho, vegevho };
            Verzioinfok verinf = null;
            bool[] kell = new bool[szint.Length];
            for (int i = 0; i < szint.Length; i++)
            {
                kell[i] = true;
                string egyszint = szint.Substring(i, 1);
                verinf = _verzioinfok[egyszint];
                if (verinf.Verziotabla.Rows.Count == 1 && !verinf.Delete)
                    kell[i] = false;
                verinf.AktVerzioId = verinf.GetVerzioId(intervallum);
            }
            bool volttrue = false;
            foreach (bool kelle in kell)
            {
                if (kelle)
                {
                    volttrue = true;
                    break;
                }
            }
            if (!volttrue && !force)
                return true;
            TablainfoTag tag;
            Tablainfo[] infok = Tablainfok.GetByTermszarmPluszSzint("SZ", szint);
            foreach (Tablainfo tabinfo in infok)
            {
                bool egykell = false;
                tag = tabinfo.TablaTag;
                if (tabinfo.LeiroTablainfo.VerzioTerkepArray.Count > 1 && tabinfo.LeiroTablainfo.AktVerzioId != tag.Tablainfo.LeiroTablainfo.Azonositok.Verzioinfok.AktVerzioId)
                {
                    egykell = true;
                    tabinfo.LeiroTablainfo.Adattolt(_aktintervallum, true);
                }
                if (tabinfo.KellVerzio && tabinfo.VerzioTerkepArray.Count > 1 || force)
                {
                    egykell = true;
                    tabinfo.Adattolt(_aktintervallum, true);
                }
                if (!tag.Forditott)
                    Combokupdate(tag);
                if (egykell || force)
                {
                    tabinfo.Beallit();
                    tabinfo.Combobeallit();
                }
            }
            return true;
        }
        /// <summary>
        /// belso hasznalatra
        /// </summary>
        private void Rendszerversiontolt()
        {
            Versiontolt("R", _rendszerconn);
        }
        /// <summary>
        /// belso hasznalatra
        /// </summary>
        private void Userversiontolt()
        {
            if (_rendszerconn == _userconn)
                _verzioinfok.Remove("U");
            else
                Versiontolt("U", _userconn);
        }
        /// <summary>
        /// belso hasznalatra
        /// </summary>
        /// <param name="cegconn"></param>
        /// <param name="intervallum"></param>
        private void Cegversiontolt(string cegconn, DateTime[] intervallum)
        {
            if (_rendszerconn == cegconn)
                _verzioinfok.Remove("C");
            else if (_ujceg || intervallum[1].CompareTo(_verzioinfok[2].AktVerzioKezd) < 0 || intervallum[0].CompareTo(_verzioinfok[2].AkVerzioVeg) > 0)
                Versiontolt("C", cegconn);
        }
        /// <summary>
        /// Egy alkalmazasi adatbazis eloallitasa az alkalmazas sorszama szerint
        /// </summary>
        /// <param name="ind">
        /// a kivant sorszam
        /// </param>
        /// <returns>
        /// true: sikeres
        /// </returns>
        public bool Cegadatok(int ind)
        {
            _aktualcegindex = ind;
            DataRow dr = _ceginfo.DataView[ind].Row;
            string cegnev = dr["SZOVEG"].ToString();
            string cegconn = _cegconnectionok[ind].ToString();
            string cegid = dr["CEG_ID"].ToString();
            _alapconnectionstringek[2] = cegconn;
            DateTime kezd;
            DateTime veg;
            DataTable dt = new DataTable("CEGSZERZODES");
            string selstring = "";
            if (_alkalmazas != "TERVEZO")
                selstring = " where ALKALMAZAS_ID = " + AlkalmazasId + " AND CEG_ID= " + cegid;
            dt = Sqlinterface.Select(dt, cegconn, "CEGSZERZODES", selstring, "", false);
            if (dt.Rows.Count == 0)
            {
                kezd = Convert.ToDateTime(DateTime.Today.Year.ToString() + "." + DateTime.Today.Month.ToString() + ".01");
                veg = kezd.AddMonths(1).AddDays(-1);
            }
            else
            {
                DataRow row = dt.Rows[0];
                kezd = Convert.ToDateTime(row["INDULODATUM"].ToString());
                veg = kezd.AddMonths(1).AddDays(-1);
            }
            return Cegadatok(cegconn, cegnev, new DateTime[] { kezd, veg });
        }
        /// <summary>
        /// Az eppen aktualis alkalmazas(ceg) az adott datum szerinti ervenyes informacioinak ujraeloallitasa
        /// </summary>
        /// <param name="datum">
        /// a datum, mely velhetoleg az eredeti tolteshez kepest valtozott
        /// </param>
        /// <returns>
        /// true: sikeres
        /// </returns>
        public bool Cegadatok(DateTime datum)
        {
            if (_aktualcegconn == "")
            {
                CloseProgress();
                System.Windows.Forms.MessageBox.Show("Nincs inicializalt cég", "Cégadatok(dátum):");
                return false;
            }
            DateTime[] dtt = new DateTime[2] { datum, datum.AddMonths(1).AddDays(-1) };
            DateTime[] aktintervallum = _aktintervallum;
            if (datum.CompareTo(_aktintervallum[0]) < 0)
                dtt = aktintervallum;
            if (dtt[0].CompareTo(aktintervallum[0]) == 0 && dtt[1].CompareTo(aktintervallum[1]) == 0)
                return true;
            return Cegadatok(dtt);
        }
        /// <summary>
        /// Az eppen aktualis alkalmazas(ceg) egy adott datumtartomanyban ervenyes informacioinak ujraeloallitasa
        /// </summary>
        /// <param name="interv">
        /// datumtartomany
        /// </param>
        /// <returns>
        /// true: sikeres
        /// </returns>
        public bool Cegadatok(DateTime[] interv)
        {
            DateTime[] aktintervallum = _verzioinfok[2].AktIntervallum;
            if (_aktualcegconn == "")
            {
                CloseProgress();
                FakPlusz.MessageBox.Show("Nincs inicializalt cég", "Cegadatok(dátum):");
                return false;
            }
            if (aktintervallum != null)
            {
                if (interv[0].CompareTo(aktintervallum[0]) == 0 && interv[1].CompareTo(aktintervallum[1]) == 0)
                    return true;
                if (interv[0].CompareTo(aktintervallum[0]) >= 0 && (aktintervallum[1] == Mindatum || interv[1] <= aktintervallum[1]))
                    return true;
            }
            return Cegadatok(_aktualcegconn, _aktualcegnev, interv);
        }
        /// <summary>
        /// Egy alkalmazasi adatbazis adott datum szerinti informacioinak eloallitasa
        /// </summary>
        /// <param name="cegconn">
        /// alkalmazasi adatbazis Connection String
        /// </param>
        /// <param name="cegnev">
        /// ceg neve
        /// </param>
        /// <param name="datum">
        ///  datum, melynek alapjan egy datumintervallumot allit elo a honap vegeig
        /// </param>
        /// <returns>
        /// true: sikeres volt
        /// Nem sikeres, ha hibas a Connection String vagy az eloallitas soran inkonzisztencia
        /// derul ki
        /// </returns>
        public bool Cegadatok(string cegconn, string cegnev, DateTime datum)
        {
            DateTime[] aktintervallum = _aktintervallum;
            DateTime[] dtt = new DateTime[2] { datum, datum.AddMonths(1).AddDays(-1) };
            if (cegconn != "" && cegconn == _aktualcegconn && dtt[0].CompareTo(_aktintervallum[0]) == 0 && dtt[1].CompareTo(_aktintervallum[1]) == 0)
                return true;
            return Cegadatok(cegconn, cegnev, dtt);
        }
        /// <summary>
        /// Egy alkalmazasi adatbazis adott datumintervallum szerinti informacioinak eloallitasa
        /// </summary>
        /// <param name="cegconn">
        /// alkalmazasi adatbazis Connection String
        /// </param>
        /// <param name="cegnev">
        /// ceg neve
        /// </param>
        /// <param name="interv">
        ///  a kert datumintervallum
        /// </param>
        /// <returns>
        /// true: sikeres volt 
        /// </returns>
        public bool Cegadatok(string cegconn, string cegnev, DateTime[] interv)
        {
            return Cegadatok(cegconn, cegnev, interv, false);
        }
        public bool Cegadatok(string cegconn, string cegnev, DateTime[] interv, bool force)
        {
            if (!force && cegconn != "" && cegconn == _aktualcegconn && interv[0].CompareTo(_aktintervallum[0]) == 0 && interv[1].CompareTo(_aktintervallum[1]) == 0)
                return true;
            if (cegconn != "" && !Sqlinterface.Cegconn(cegconn))
                return false;
            Rendszeradatok(interv, "RU");
            _ujceg = true;
            if (cegconn != "")
                Cegversiontolt(cegconn, interv);
            _aktualcegnev = cegnev;
            TablainfoCollection tabinfok = Tablainfok.GetBySzint("C" + _szintstring);
            SetProgressText(cegnev + " adatainak töltése");
            foreach (Tablainfo tabinfo in tabinfok)
            {
                ProgressRefresh();
                TablainfoTag tag = tabinfo.TablaTag;
                if (tabinfo.Tablanev != "")
                {
                    if (tabinfo.Tablanev == "TARTAL")
                    {
                        ProgressRefresh();
                        tabinfo.LeiroVizsg();
                        tabinfo.Combobeallit();
                        tabinfo.LeiroTablainfo.Combobeallit();
                        ProgressRefresh();
                    }
                    else if (cegconn != "")
                    {
                        tabinfo.Adattabla.Connection = cegconn;
                        if (!tag.Forditott)
                        {
                            ProgressRefresh();
                            tabinfo.Init(true);
                            if (tabinfo.TablaColumns.Count == 0)
                            {
                                _bajvan = true;
                                return false;
                            }
                            else
                            {
                                ProgressRefresh();
                                tabinfo.LeiroVizsg();
                                tabinfo.Combobeallit();
                                if (tabinfo.Azonositok.Combofileba != "" || tabinfo.LehetCsoport || tabinfo.LehetOsszef)
                                    Combokupdate(tag);
                                if (_aktualcegconn == "")
                                    tabinfo.LeiroTablainfo.Combobeallit();
                                int conind = ConnectionStringArray.IndexOf(tabinfo.Adattabla.Connection);
                                if (conind != -1)
                                {
                                    if (tabinfo.Tablanev == "VALTOZASNAPLO")
                                    {
                                        NaploTablainfok.Add(tabinfo);
                                        tabinfo.NaploTablainfo = tabinfo;
                                    }
                                    else if (NaploTablainfok.IndexOf(conind) != -1)
                                        tabinfo.NaploTablainfo = NaploTablainfok[conind];
                                    tabinfo.NaploTabla = NaploTablak[conind];
                                }
                                ProgressRefresh();
                            }
                        }
                        tag.Azonositok.ToolTipText();
                    }
                }
            }
            ProgressRefresh();
            _aktualcegconn = cegconn;
            _alapconnectionstringek[2] = cegconn;
            string[] cegidk;
            if (!_ceginfo.Modositott)
                cegidk = _ceginfo.Adattabla.GetTartal("CEG_ID", "CEGCONNECTION", cegconn);
            else
                cegidk = new string[] { "1" };
            if (cegidk == null)
            {
                _bajvan = true;
                CloseProgress();
                FakPlusz.MessageBox.Show("CEGEK tábla tartalma ellentmond a Connection.txt\ntartalmának!");
                return false;
            }
            else
            {
                ProgressRefresh();
                _aktualcegid = Convert.ToInt64(cegidk[0]);
                _aktualcegindex = _cegconnectionok.IndexOf(cegconn);
                if (_bajvan)
                {
                    string szov = BajInfo;
                    return false;
                }
                else
                {
                    _ujceg = false;
                    return true;
                }
            }
        }

        /// <summary>
        /// Verzioinformaciok toltese
        /// </summary>
        /// <param name="szint">
        /// a kivant szint
        /// </param>
        /// <param name="conn">
        /// az adatbazis ConnectionString-je
        /// </param>
        public void Versiontolt(string szint, string conn)
        {
            Verzioinfok egyverinfo = _verzioinfok[szint];
            egyverinfo.Init(conn, szint);
        }
        /// <summary>
        /// belso hasznalatra
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        private DateTime[] Intmegallapit(TablainfoTag tag)
        {
            Verzioinfok verinf = null;
            DateTime[] interv = new DateTime[2];
            string szint = tag.Tablainfo.Szint;
            string termszarm = tag.Tablainfo.TermSzarm.Trim();
            string azon = tag.Tablainfo.Azon;
            string tablanev = tag.Tablainfo.Tablanev;
            if (azon == "LEIR" || tablanev == "LEIRO" || tablanev == "TARTAL" || szint == "R")
                verinf = _verzioinfok[0];
            else if (szint == "U")
                verinf = _verzioinfok[1];
            else
                verinf = _verzioinfok[2];
            interv[0] = verinf.AktVerzioKezd;
            interv[1] = verinf.AkVerzioVeg;
            return interv;
        }
        /// <summary>
        /// Select adatbazisfunkcio
        /// </summary>
        /// <param name="dt">
        /// DataTable 
        /// </param>
        /// <param name="conn">
        /// adatbazis Connection String
        /// </param>
        /// <param name="tablanev">
        /// Tabla neve
        /// </param>
        /// <param name="selwhere">
        /// WHERE ...
        /// </param>
        /// <param name="selord">
        /// ORDER BY ...
        /// </param>
        /// <param name="top">
        /// true: csak az elso sor
        /// </param>
        /// <returns>
        /// DataTable
        /// </returns>
        public DataTable Select(DataTable dt, string conn, string tablanev, string selwhere, string selord, bool top)
        {
            return (Sqlinterface.Select(dt, conn, tablanev, selwhere, selord, top));
        }
        /// <summary>
        /// Tablak Update-je
        /// </summary>
        /// <param name="modositandok">
        /// tablak tombje
        /// </param>
        /// <returns>
        /// true:ok
        /// </returns>
        public bool UpdateTransaction(Tablainfo[] modositandok)
        {
            string conn = "";
            foreach (Tablainfo egyinfo in modositandok)
            {
                if (conn == "")
                    conn = egyinfo.Adattabla.Connection;
                else if (conn != egyinfo.Adattabla.Connection)
                {
                    CloseProgress();
                    System.Windows.Forms.MessageBox.Show("Egy tranzakcióban különbözö conn-ok!", "UpdateTransaction");
                    return false;
                }
            }
            return (TryToUpdate(modositandok, conn));
        }
        private bool TryToUpdate(Tablainfo[] tomb, string conn)
        {
            Tablainfo firstparent = null;
            TablainfoCollection chain = null;
            Tablainfo parinfo = null;
            Tablainfo[] childinfo = null;
            if (tomb.Length == 0)
                return true;
            bool vanmodositas = false;
            foreach (Tablainfo egyinfo in tomb)
            {
                if (egyinfo.Modositott)
                {
                    vanmodositas = true;
                    break;
                }
            }
            if (!vanmodositas)
                return true;

            Sqlinterface.ConnOpen(conn);
            Sqlinterface.BeginTransaction(conn);
            foreach (Tablainfo egyinfo in tomb)
            {
                parinfo = egyinfo.TermParentTabinfo;
                firstparent = egyinfo.FirstTermParentTabinfo;
                chain = egyinfo.TermParentTabinfoChain;
                if (egyinfo.TermChildTabinfo != null && egyinfo.TermChildTabinfo.Count != 0)
                    childinfo = (Tablainfo[])egyinfo.TermChildTabinfo.ToArray(typeof(Tablainfo));
                int lastmodcol = egyinfo.Columns.IndexOf("LAST_MOD");
                if (egyinfo.Modositott)
                {
                    if (!egyinfo.Leiroe && egyinfo.Tablanev != "TARTAL")
                    {
                        if (parinfo != null && egyinfo.AktIdentity == -1 && parinfo.AktIdentity != -1)
                        {
                            foreach (Tablainfo chaininfo in chain)
                            {
                                if (chaininfo.AktIdentity != -1)
                                {
                                    int savcount = egyinfo.DataView.Count;
                                    for (int k = 0; k < egyinfo.DataView.Count; k++)
                                    {
                                        if (egyinfo.DataView[k].Row.RowState == DataRowState.Added)
                                        {
                                            egyinfo.DataView[k].Row[chaininfo.IdentityColumnName] = chaininfo.AktIdentity;
                                            if (savcount > egyinfo.DataView.Count)
                                                k--;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (lastmodcol != -1)
                    {
                        foreach (DataRow row in egyinfo.Rows)
                        {
                            if (row.RowState != DataRowState.Deleted && row["MODOSITOTT_M"].ToString() == "1")
                                row[lastmodcol] = DateTime.Now;
                        }
                    }
                    if (egyinfo.KellVerzio && egyinfo.Adattabla.Columns.IndexOf("PREV_ID") != -1)
                    {
                        string whereand = " where ";
                        if (egyinfo.SelectString != "")
                        {
                            whereand = " and ";
                        }
                        string selstr = "select * from " + egyinfo.Tablanev + " " + egyinfo.SelectString + whereand;
                        if (!egyinfo.NewVersionCreated || !egyinfo.Leiroe && egyinfo.Tablanev != "TARTAL")
                        {
                            egyinfo.Adattabla.LastSel = egyinfo.SelectString + whereand + "VERZIO_ID=" + egyinfo.AktVerzioId.ToString();
                            selstr += "VERZIO_ID=" + egyinfo.AktVerzioId.ToString() + " AND ";
                        }
                        selstr += "PREV_ID=0";
                        if (!Sqlinterface.CommandBuilderUpd(conn, (DataTable)egyinfo.Adattabla, selstr))
                            return false;
                        else
                        {
                            DataTable dt = new DataTable();
                            dt.Rows.Clear();
                            dt = Sqlinterface.Fill(dt);
                            foreach (DataRow row in dt.Rows)
                                row["PREV_ID"] = row[egyinfo.IdentityColumnIndex];
                            if (!Sqlinterface.CommandBuilderUpd(conn, dt, selstr))
                                return false;

                        }
                    }
                    else
                    {
                        if (!Sqlinterface.CommandBuilderUpd(conn, egyinfo.Tablanev, egyinfo.IdentityColumnName, (DataTable)egyinfo.Adattabla))
                            return false;
                        if (egyinfo.AktIdentity == -1 && egyinfo.Tablanev != "VALTOZASNAPLO" && egyinfo.Tablanev != "USERLOG" && (egyinfo.SelectString != "" && egyinfo.TermSzarm != "SZ" || firstparent != null))
                        {
                            string lastsel = egyinfo.SelectString;
                            DataTable dt = new DataTable();
                            dt = Sqlinterface.Fill(dt);
                            if (dt.Rows.Count != 0)
                            {
                                egyinfo.AktIdentity = Convert.ToInt64(dt.Rows[0][egyinfo.IdentityColumnIndex].ToString());
                                if (lastsel != "" || egyinfo.TermSzarm != "SZ")
                                {
                                    lastsel = " where " + egyinfo.IdentityColumnName + "='" + egyinfo.AktIdentity.ToString() + "'";
                                    egyinfo.Adattabla.LastSel = lastsel;
                                }
                            }
                            if (firstparent != null && firstparent.AktIdentity != -1)
                            {
                                lastsel = " where " + firstparent.IdentityColumnName + "='" + firstparent.AktIdentity.ToString() + "'";
                                egyinfo.Adattabla.LastSel = lastsel;
                            }
                        }
                    }
                }
            }
            Sqlinterface.CommitTransaction();
            string szint = tomb[0].Szint;
            foreach (Tablainfo egyinfo in tomb)
            {
                if (egyinfo.Modositott)
                {
                    OkUpdateUtan(egyinfo);
                    egyinfo.LastUpdate = DateTime.Now;
                    int conind = ConnectionStringArray.IndexOf(egyinfo.Adattabla.Connection);
                    if (conind != -1)
                    {
                        if (LastUpdateDateTime[conind].CompareTo(egyinfo.LastUpdate) < 0)
                            LastUpdateDateTime[conind] = egyinfo.LastUpdate;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// Tabla sikeres update-je utan vegrehajtando funkciok
        /// </summary>
        /// <param name="tablainfo"></param>
        private void OkUpdateUtan(Tablainfo tablainfo)
        {
            string filter;
            string sort = "";
            string szint = "";
            if (tablainfo.Tablanev != "VALTOZASNAPLO" && tablainfo.Tablanev != "USERLOG")
            {
                string adattablanev = "";
                if (tablainfo.Leiroe)
                    adattablanev = tablainfo.TablaTag.Tablainfo.Tablanev;
                ValtozasRogzit(tablainfo.NaploTablainfo, tablainfo.NaploTabla, tablainfo.Azon, tablainfo.Tablanev, adattablanev);
            }
            if (tablainfo.Tablanev == "VALTOZASNAPLO" || tablainfo.Tablanev == "USERLOG")
            {
                tablainfo.Adattabla.Rows.Clear();
                tablainfo.Modositott = false;
                tablainfo.Changed = false;
                return;
            }
            tablainfo.Rogzitett = true;
            Verzioinfok verinfo = null;
            TablainfoTag tag = tablainfo.TablaTag;
            bool leiroe = tablainfo.Leiroe;
            if (!leiroe)
            {
                if (tablainfo.Tablanev.Contains("VERSION"))
                {
                    szint = tablainfo.Szint;
                    string conn = tablainfo.Adattabla.Connection;
                    verinfo = _verzioinfok[szint];
                    int aktid = verinfo.AktVerzioId;
                    Versiontolt(szint, conn);
                    if (aktid != verinfo.AktVerzioId)
                    {
                        if (szint == "C")
                            Cegadatok(Aktintervallum);
                        else
                            Rendszeradatok(Aktintervallum, szint, verinfo.Delete);
                        verinfo.Delete = false;
                    }
                }
            }
            tablainfo.Modositott = false;
            tablainfo.Changed = false;
            filter = tablainfo.DataView.RowFilter;
            ForceAdattolt(tablainfo, true);
            if (tablainfo.Tablanev == "BASE")
                tablainfo.DataView.RowFilter = filter;
            if (tablainfo.Osszefinfo != null)
                tablainfo.Osszefinfo.InitKell = true;
            bool torolt = true;
            if (leiroe || tablainfo.Azon == "LEIR" || tablainfo.Tablanev == "BASE" || tablainfo.Tablanev == "TARTAL")
                torolt = false;
            else if (tablainfo.ParentTag.Tablainfo.ToroltTagok.Count == 0 || tablainfo.ParentTag.Tablainfo.ToroltTagok.IndexOf(tablainfo) == 0)
                torolt = false;
            if (!torolt)
            {
                if (tablainfo.Azon != "LEIR" && tablainfo.Tablanev != "BASE" && !leiroe)
                    Combokupdate(tag);
                if (leiroe)
                {
                    if (tablainfo.TablaTag.Tablainfo.Adatfajta != "K")
                    {
                        tablainfo.Beallit();
                        tablainfo.Combobeallit();
                    }
                    else
                        KodtablakFrissitese(tablainfo);
                    Tablainfo adattablainfo = tablainfo.TablaTag.Tablainfo;
                    if (adattablainfo.Tablanev == "TARTAL" || "RU".Contains(adattablainfo.Szint) || _aktualcegconn != "")
                    {
                        tablainfo.TablaTag.Tablainfo.Beallit();
                        tablainfo.TablaTag.Tablainfo.Combobeallit();
                    }
                }
                if (!leiroe)
                {
                    if (tablainfo.Tablanev == "BASE")
                    {
                        TablainfoTag newtag = null;
                        TablainfoTag newtartaltag = null;
                        TartalomjegyzekbolTorol(tablainfo, false);
                        filter = tablainfo.DataView.RowFilter;
                        sort = tablainfo.DataView.Sort;
                        tablainfo.DataView.RowFilter = "";
                        tablainfo.DataView.Sort = "SORSZAM DESC";
                        DataRow dr = tablainfo.DataView[0].Row;
                        int rowindex = tablainfo.Adattabla.Rows.IndexOf(dr);
                        szint = dr["SZINT"].ToString();
                        string azon = dr["AZON"].ToString().Trim();
                        if (azon.Length < 4)
                        {
                            int beszursorrend = Convert.ToInt32(dr["SORREND"].ToString()) + 1;
                            DataRow newrow = tablainfo.Ujsor();
                            newrow["AZON"] = azon + "T";
                            newrow["PARENT"] = dr["SORSZAM"];
                            newrow["SZOVEG"] = "Táblázatok";
                            newrow["TABLANEV"] = "TARTAL";
                            newrow["SZINT"] = szint;
                            newrow["SORREND"] = beszursorrend;
                            UpdateTransaction(new Tablainfo[] { tablainfo });
                            newtag = new TablainfoTag(tablainfo.Adattabla, rowindex, null, null, (TablainfoTag)GyokerTablainfoTagok[1], this);
                            rowindex = tablainfo.Adattabla.Rows.IndexOf(tablainfo.DataView[0].Row);
                            newtartaltag = new TablainfoTag(tablainfo.Adattabla, rowindex, null, null, newtag, this);
                            newtartaltag.Tablainfo.Combobeallit();
                            newtartaltag.LeiroTablainfo.Combobeallit();
                        }
                        else
                        {
                            for (int j = 0; j < ((TablainfoTag)GyokerTablainfoTagok[1]).ChildTablainfoTagok.Count; j++)
                            {
                                newtag = ((TablainfoTag)GyokerTablainfoTagok[1]).ChildTablainfoTagok[j];
                                if (newtag.Azonositok.Azon == azon)
                                {
                                    newtag.TablainfoTagUpdate(tablainfo.Adattabla, rowindex);
                                    break;
                                }
                            }
                        }
                        tablainfo.DataView.RowFilter = filter;
                        tablainfo.DataView.Sort = sort;
                    }
                    if (tablainfo.Tablanev == "TARTAL")
                    {
                        TartalomjegyzekbolTorol(tablainfo, true);
                        AdatTabla adattabla = tablainfo.Adattabla;
                        for (int i = 0; i < adattabla.Rows.Count; i++)
                        {
                            TablainfoTag tag1 = null;
                            string sorszint = adattabla.Rows[i]["SZINT"].ToString().Trim();
                            string azontip = adattabla.Rows[i]["AZONTIP"].ToString().Trim();
                            string adatfajta = adattabla.Rows[i]["ADATFAJTA"].ToString().Trim();
                            string kodtipus = adattabla.Rows[i]["KODTIPUS"].ToString().Trim();
                            string termszarm = adattabla.Rows[i]["TERMSZARM"].ToString().Trim();
                            string tablanev = adattabla.Rows[i]["TABLANEV"].ToString().Trim();
                            Tablainfo egyinfo = null;
                            if (azontip.Contains("SZRM"))
                            {
                                egyinfo = GetBySzintPluszTablanev(kodtipus.Substring(0, 1), kodtipus.Substring(1));
                                if (egyinfo != null)
                                {
                                    egyinfo = egyinfo.LeiroTablainfo;
                                    tag1 = tag.ChildTablainfoTagok[i];
                                    if (tag1 == null)
                                        tag1 = new TablainfoTag(adattabla, i, null, null, tag, this);
                                }
                            }
                            else
                                egyinfo = Tablainfok.GetByAzontip(azontip);
                            if (egyinfo == null)
                            {
                                tag1 = new TablainfoTag(adattabla, i, null, null, tag, this);
                                if (azontip.Contains("SZRM"))
                                    egyinfo = tag1.LeiroTablainfo;
                                else
                                {
                                    egyinfo = tag1.Tablainfo;
                                    if (_aktualcegconn != "")
                                    {
                                        tag1.LeiroTablainfo.Combobeallit();
                                        tag1.Tablainfo.Combobeallit();
                                    }
                                }
                                if ("CO".Contains(tag1.Azonositok.Adatfajta))
                                    tag1.FordTag = new TablainfoTag(adattabla, i, tablainfo, tag1, tag, this, true);
                            }
                            else
                            {
                                if (!azontip.Contains("SZRM"))
                                {
                                    tag1 = egyinfo.TablaTag;
                                    if (tag1.ParentTag.ChildTablainfoTagok.IndexOf(egyinfo.Azontip) == -1)
                                    {
                                        tag1.SorIndex = i;
                                        tag1.Torolt = false;
                                    }
                                }
                                tag1.TablainfoTagUpdate(adattabla, i);
                                if (egyinfo.Osszefinfo != null)
                                    egyinfo.Osszefinfo.InitKell = true;
                            }
                            if (!azontip.Contains("SZRM"))
                                AzontipSzerintBevisz(azontip);
                        }
                        if (tablainfo.Modositott)
                            UpdateTransaction(new Tablainfo[] { tablainfo });
                    }
                    else if (tablainfo.Azon != "LEIR" && tablainfo.Tablanev != "BASE")
                    {
                        AzontipSzerintUpdate(tablainfo);
                        if (!tag.Forditott && tablainfo.Tablanev != "BASE")
                        {
                            Combokupdate(tag);
                            TablainfoTag elsotag = null;
                            if (tablainfo.Azon == "SZRM")
                            {
                                Tablainfo egytabinfo = Tablainfok.GetByAzontip("SZRM" + tablainfo.Kodtipus);
                                if (egytabinfo != null)
                                    elsotag = egytabinfo.TablaTag;
                            }
                            else if (tablainfo.Azontip1 != "" && Tablainfok.GetByAzontip(tablainfo.Azontip1) != null)
                                elsotag = Tablainfok.GetByAzontip(tablainfo.Azontip1).TablaTag;
                            if (elsotag != null)
                                Combokupdate(elsotag);
                            if (tablainfo.Azontip2 != "" && Tablainfok.GetByAzontip(tablainfo.Azontip2) != null)
                            {
                                elsotag = Tablainfok.GetByAzontip(tablainfo.Azontip2).TablaTag;
                                Combokupdate(elsotag);
                            }
                        }
                        if (tablainfo.NewVersionCreated || tablainfo.LastVersionDeleted || tablainfo.VerzioTerkepArray.Count == 0)
                        {
                            if (tablainfo.Osszefinfo != null)
                                tablainfo.Osszefinfo.InitKell = true;
                            TablainfoTag fordtag = tablainfo.TablaTag.FordTag;
                            if (fordtag != null)
                                fordtag.ToolTipText();
                        }
                    }
                }
                if (tablainfo.Tablanev != "BASE")
                {
                    tag.ToolTipText();
                    if (tag.FordTag != null)
                        tag.FordTag.ToolTipText();
                    tablainfo.NewVersionCreated = false;
                    tablainfo.LastVersionDeleted = false;
                }
            }
        }
        private void KodtablakFrissitese(Tablainfo tablainfo)
        {
            _formfaload = false;
            OpenProgress();
            SetProgressText("Kódtáblák frissitése");
            ProgressRefresh();
            string azontip = tablainfo.Azontip;
            foreach (Tablainfo info in Tablainfok)
            {
                if (info.LeiroTablainfo.Azontip == azontip)
                {
                    SetProgressText(info.TablaTag.Azonositok.Szoveg);
                    ProgressRefresh();
                    info.LeiroTablainfo = tablainfo;
                    info.LeiroTablainfo.Beallit();
                    ProgressRefresh();
                    info.LeiroTablainfo.Combobeallit();
                    ProgressRefresh();
                    info.Beallit();
                    ProgressRefresh();
                    info.Combobeallit();
                }
            }
            CloseProgress();
        }
        private void TartalomjegyzekbolTorol(Tablainfo tartalinfo, bool csaktorolttagokat)
        {
            if (tartalinfo.ToroltTagok.Count == 0)
                return;
            Tablainfo tablainfo = tartalinfo;
            if (!csaktorolttagokat)
            {
                Tablainfo[] modositando = new Tablainfo[tartalinfo.ToroltTagok.Count];
                TablainfoTag parent = ((Tablainfo)tablainfo.ToroltTagok[0]).TablaTag.ParentTag;
                for (int i = 0; i < tablainfo.ToroltTagok.Count; i++)
                {
                    Tablainfo egyinfo = (Tablainfo)tablainfo.ToroltTagok[i];
                    modositando[i] = egyinfo.TablaTag.LeiroTablainfo;
                    egyinfo.TablaTag.LeiroTablainfo.TeljesTorles();
                    egyinfo.TablaTag.Torolt = true;
                }
                parent.Torolt = true;
                UpdateTransaction(modositando);
            }
            else if (csaktorolttagokat)
            {
                foreach (object[] ob in tablainfo.ToroltTagok)
                {
                    TablainfoTag tag1 = (TablainfoTag)ob[0];
                    string beszverz = ob[1].ToString();
                    string tablanev = ob[2].ToString();
                    string kodtipus = ob[3].ToString();
                    string azontip = ob[4].ToString();
                    string verid = ob[5].ToString();
                    string termszarm = azontip.Substring(0, 2).Trim();
                    string szint = azontip.Substring(2, 1);
                    AzontipSzerintTorol(azontip);
                    if (beszverz == verid)
                    {
                        if (termszarm == "T")
                        {
                            Tablainfo szrmtartalinfo = Tablainfok.GetByAzontip("SZRMTARTAL");
                            TablainfoTag tartaltag = szrmtartalinfo.TablaTag;
                            string szrmkodtipus = azontip.Substring(2, 1) + azontip.Substring(4);
                            szrmtartalinfo.Adattabla.SetViewIndex(szrmtartalinfo.Find("KODTIPUS", szrmkodtipus));
                            szrmtartalinfo.Adatsortorol(szrmtartalinfo.ViewSorindex);
                            UpdateTransaction(new Tablainfo[] { szrmtartalinfo });
                        }

                        tag1.Tablainfo.TeljesTorles();
                        UpdateTransaction(new Tablainfo[] { tag1.Tablainfo });
                        if (SpecialisLeiroTabinfok.GetByAzontip(tag1.Tablainfo.LeiroTablainfo.Azontip) == null)
                        {
                            tag1.Tablainfo.LeiroTablainfo.TeljesTorles();
                            UpdateTransaction(new Tablainfo[] { tag1.Tablainfo.LeiroTablainfo });
                        }
                        else if (tablanev == "OSSZEF" && tag1.Tablainfo.Osszefinfo != null)
                            Osszefuggesek.Remove(tag1.Tablainfo.Osszefinfo);
                    }
                    if (!tag1.Forditott)
                        tag1.Torolt = true;
                }
            }
            tablainfo.ToroltTagok.Clear();
        }
        /// <summary>
        /// ha egy tablainformacioban modositottunk, az osszes erre hivatkozo osszefuggesben jelezzuk,
        /// hogy ujra kell inicializalni vagy ujra kell tolteni
        /// </summary>
        /// <param name="tabinfo">
        /// a tablainformacio, amelyiket modositottunk
        /// </param>
        public void AzontipSzerintUpdate(Tablainfo tabinfo)
        {
            string azontip = tabinfo.Azontip;
            foreach (Tablainfo egyinf in Tablainfok)
            {
                Osszefinfo osszefinfo = egyinf.Osszefinfo;
                if (osszefinfo != null)
                {
                    if (egyinf.Azontip1 == azontip)
                    {
                        if (tabinfo.DataView.Count == 0 || osszefinfo.aktverid1 != tabinfo.LastVersionId.ToString() || tabinfo.LastVersionDeleted || tabinfo.NewVersionCreated)

                            osszefinfo.InitKell = true;
                        else
                            osszefinfo.TolteniKell = true;
                    }
                    if (egyinf.Azontip2 == azontip)
                    {
                        if (tabinfo.DataView.Count == 0 || osszefinfo.aktverid2 != tabinfo.LastVersionId.ToString() || tabinfo.LastVersionDeleted || tabinfo.NewVersionCreated)
                            osszefinfo.InitKell = true;
                        else
                            osszefinfo.TolteniKell = true;
                    }
                }
            }
        }
        /// <summary>
        /// Ha a tartalomjegyzekbol egy sort toroltunk, az osszes erre hivatkozo tablainformacio adattablajat toroljuk es a fastrukturabol 
        /// kivesszuk
        /// </summary>
        /// <param name="azontip">
        /// teljes azonosito
        /// </param>
        private void AzontipSzerintTorol(string azontip)
        {
            Tablainfo[] infok = Tablainfok.GetByAzontip1(azontip);
            Torol(infok, azontip);
            infok = Tablainfok.GetByAzontip2(azontip);
            Torol(infok, azontip);
        }
        /// <summary>
        /// Belso hasznalatra
        /// </summary>
        private void Torol(Tablainfo[] tabinfok, string azontip)
        {
            if (tabinfok != null)
            {
                foreach (Tablainfo egyinf in tabinfok)
                {
                    if (egyinf.Tablanev != "LEIRO")
                    {
                        egyinf.TeljesTorles();
                        if (egyinf.Osszefinfo != null)
                            egyinf.Osszefinfo.TolteniKell = true;
                        UpdateTransaction(new Tablainfo[] { egyinf });
                        if (!egyinf.TablaTag.Torolt)
                            egyinf.TablaTag.Torolt = true;
                        string egyinfazontip = egyinf.Azontip;
                        Tablainfo tabinfo = egyinf.TablaTag.ParentTag.Tablainfo;      // ez a tartalomjegyzek
                        for (int i = 0; i < tabinfo.DataView.Count; i++)
                        {
                            DataRow dr = tabinfo.DataView[i].Row;
                            if (dr["KODTIPUS"].ToString() == azontip)
                            {
                                tabinfo.Adattabla.Adatsortorol(i);
                                i = -1;
                            }
                        }
                        if (tabinfo.Modositott)
                            UpdateTransaction(new Tablainfo[] { tabinfo });
                        AzontipSzerintTorol(egyinfazontip);
                    }
                }
            }
        }
        /// <summary>
        /// Ha a tartalomjegyzekbe egy olyan sort viszunk be, amit elozoleg toroltunk, visszarakja a fa strukturaba
        /// az osszes erre hivatkozo tablainformacio node-jat
        /// </summary>
        /// <param name="azontip">
        /// a teljes azonosito
        /// </param>
        private void AzontipSzerintBevisz(string azontip)
        {
            foreach (Tablainfo egyinf in Tablainfok)
            {
                if (egyinf.Azontip1 == azontip || egyinf.Azontip2 == azontip)
                {
                    string egyinfazontip = egyinf.Azontip;
                    if (egyinf.Osszefinfo != null)
                        egyinf.Osszefinfo.TolteniKell = true;
                    if (egyinf.TablaTag.Torolt)
                        egyinf.TablaTag.Torolt = false;
                    AzontipSzerintBevisz(egyinfazontip);
                }
            }
        }
        /// <summary>
        /// adattabla utolso Select-jeinek kiadasa, minden valtozasjelzo torlese( azaz a Select utani allapot
        /// helyreallitasa)
        /// </summary>
        /// <param name="tabinfo">
        /// tablainformacio
        /// </param>
        /// <param name="force">
        /// true: mindenkeppen
        /// false: csak valtozas eseten
        /// </param>
        /// <returns></returns>
        public Tablainfo ForceAdattolt(Tablainfo tabinfo, bool force)
        {
            bool kell = false;
            if (!force && (tabinfo.Modositott || tabinfo.ModositasiHiba || tabinfo.Changed))
                kell = true;
            if (kell || force)
            {
                string conn = tabinfo.Adattabla.Connection;
                DataView naploview = new DataView(tabinfo.NaploTabla);
                if ((tabinfo.Modositott || force) && tabinfo.Tablanev != "VALTOZASNAPLO")
                {
                    string adattablanev = "";
                    if (tabinfo.Leiroe)
                        adattablanev = tabinfo.TablaTag.Tablainfo.Tablanev;
                    naploview.RowFilter = "AZON='" + tabinfo.Azon + "' and TABLANEV='" + tabinfo.Tablanev + "'";
                    if (adattablanev != "")
                        naploview.RowFilter += " and ADATTABLANEV='" + adattablanev + "'";
                    for (int i = 0; i < naploview.Count; i++)
                    {
                        naploview[i].Row.Delete();
                        i--;
                    }
                }
                tabinfo.DataView.RowFilter = "";
                tabinfo.Modositott = false;
                tabinfo.ModositasiHiba = false;
                tabinfo.Changed = false;
                if (tabinfo.Azon == "LEIR")
                    tabinfo.Adattabla.LastSel = LeiroTag.Azonositok.Selectstring;
                Select((DataTable)tabinfo.Adattabla, conn, tabinfo.Tablanev, tabinfo.Adattabla.LastSel + " ", tabinfo.OrderString, false);
                if (tabinfo.AktualControlInfo != null && tabinfo.Azon != "LEIR" && _aktualcegconn != "")
                {
                    tabinfo.Tartalmaktolt(true);
                    if (tabinfo.Tablanev != "TARTAL" && tabinfo.TermSzarm.Trim() == "T")
                        //                    {
                        //                       tabinfo.Tartalmaktolt(tabinfo.DataView.Count - 1);
                        tabinfo.ViewSorindex = 0;
                    //                   }
                }
            }
            return (tabinfo);
        }
        /// <summary>
        /// Ha volt valtozas, adattabla utolso Select-jenek kiadasa, valtozasjelzesek torlese
        /// </summary>
        /// <param name="tabinfo">
        /// a Tablainfo
        /// </param>
        /// <returns>
        /// az eredeti adattabla
        /// </returns>
        public Tablainfo ForceAdattolt(Tablainfo tabinfo)
        {
            return ForceAdattolt(tabinfo, false);
        }
        /// <summary>
        /// Ha volt valtozas, adattablak utolso Select-jeinek kiadasa, valtozasjelzesek
        /// </summary>
        /// <param name="info">
        /// adattablak tabalinformacio tombje
        /// </param>
        public void ForceAdattolt(Tablainfo[] info)
        {
            ForceAdattolt(info, false);
        }
        /// <summary>
        /// adattablak utolso Select-jeinek kiadasa, minden valtozasjelzo torlese( azaz a Select utani allapot
        /// helyreallitasa)
        /// </summary>
        /// <param name="info">
        /// adattablak tabalinformacio array
        /// </param>
        /// <param name="force">
        /// true: mindenkeppen kiadando a Select
        /// false: csak, ha valtozas volt
        /// </param>
        public void ForceAdattolt(Tablainfo[] info, bool force)
        {
            if (info != null)
            {
                for (int i = 0; i < info.Length; i++)
                    ForceAdattolt(info[i], force);
            }
        }
        /// <summary>
        /// Tobb egyideju hasznalatokor a tobbi kezelo altal vegzett modositasokbol az engem erdeklot atemelni 
        /// ha szukseges
        /// </summary>
        /// <returns>
        /// true: szukseges volt
        /// </returns>
        public bool KellFrissit()
        {
            return KellFrissit(false);
        }
        /// <summary>
        /// Tobb egyideju hasznalatokor a tobbi kezelo altal vegzett modositasokbol az engem erdeklot atemelni 
        /// </summary>
        /// <param name="force">
        /// true: mindenkeppen
        /// false: csak, ha szukseges
        /// </param>
        /// <returns>
        /// true: volt tevekenyseg
        /// </returns>
        public bool KellFrissit(bool force)
        {
            bool kellvaltozas = KellValtozas;
            KellValtozas = false;
            Tablainfo[] tabinfok = new Tablainfo[] { Tablainfok.GetBySzintPluszTablanev("R", "VALTOZASNAPLO"), Tablainfok.GetBySzintPluszTablanev("U", "VALTOZASNAPLO") };
            char[] kettospont = new char[1];
            kettospont[0] = Convert.ToChar(":");
            string dat = ValtoztatasDatuma.ToShortDateString();
            string tim = ValtoztatasDatuma.ToString().Replace(dat, "");
            dat = dat.Replace(".", "-");
            if (dat.EndsWith("-"))
                dat = dat.Substring(0, dat.Length - 1);
            string[] ido = tim.Split(kettospont);
            string hour = ido[0];
            string minute = ido[1];
            string second = ido[2];
            string LastSel = "";
            if (!force)
                LastSel = " where ( KEZELO_ID IS NULL OR KEZELO_ID<>" + KezeloId.ToString() + ")";
            if (LastSel != "")
                LastSel += " and ";
            else
                LastSel += " where ";
            LastSel += "(ALKALM = '' OR ALKALM = '" + Alkalmazas + "')";
            LastSel += " and LAST_MOD > { d '" + dat + "'}";
            LastSel += " and (DATENAME(hour,LAST_MOD)>" + hour + " or DATENAME(hour,LAST_MOD)=" + hour +
                " and DATENAME(minute,LAST_MOD)>" + minute + " or DATENAME(hour,LAST_MOD)=" + hour + " and DATENAME(minute,LAST_MOD)=" + minute
                + " and DATENAME(second,LAST_MOD)>" + second + ")";
            tabinfok[0].Adattabla.LastSel = LastSel;
            tabinfok[1].Adattabla.LastSel = LastSel;
            ForceAdattolt(tabinfok, true);
            if (tabinfok[0].DataView.Count == 0 && tabinfok[1].DataView.Count == 0)
                return false;
            foreach (Tablainfo tabinfo in tabinfok)
            {
                DataView view = tabinfo.DataView;
                if (view.Count != 0)
                {
                    int viewind = 0;
                    string tablanev = "";
                    string adattablanev = "";
                    string szint = "";
                    string azon = "";
                    string funkcio = "";
                    string mezonev = "";
                    string kodtipus = "";
                    string azontip = "";
                    if (Kelle(view, ref viewind, ref tablanev, ref adattablanev, ref szint, ref azon, ref funkcio, ref mezonev, ref kodtipus, ref azontip))
                    {
                        DataRow dr = null;
                        do
                        {
                            dr = view[viewind].Row;
                            if (dr["TABLANEV"].ToString() != tablanev || dr["SZINT"].ToString() != szint || adattablanev != "" && adattablanev != dr["ADATTABLANEV"].ToString())
                                Kelle(view, ref viewind, ref tablanev, ref adattablanev, ref szint, ref azon, ref funkcio, ref mezonev, ref kodtipus, ref azontip);
                            if (viewind == view.Count - 1)
                                break;
                            viewind++;
                        } while (true);
                    }
                    tabinfo.Adattabla.LastSel = tabinfo.SelectString;
                    ForceAdattolt(tabinfo, true);
                    if (kellvaltozas)
                        KellValtozas = true;
                }
            }
            return true;
        }
        private bool Kelle(DataView view, ref int viewind, ref string tablanev, ref string adattablanev, ref string szint, ref string azon, ref string funkcio, ref string mezonev, ref string kodtipus, ref string azontip)
        {
            Tablainfo valttabinfo = null;
            char[] vesszo = new char[1];
            vesszo[0] = Convert.ToChar(",");
            do
            {
                string alkalmazas = view[viewind]["ALKALM"].ToString().Trim();
                string userek = view[viewind]["USEREK"].ToString();
                string cegid = view[viewind]["CEG_ID"].ToString();
                string[] usertomb = userek.Split(vesszo);
                if (Alkalmazas == alkalmazas || alkalmazas == "" && usertomb[0] == "")
                    break;
                foreach (string egystring in usertomb)
                {
                    if (egystring != "" && egystring == Alkalmazas)
                        break;
                }
                if (viewind == view.Count - 1)
                    return false;
                viewind++;
            } while (true);
            tablanev = view[viewind]["TABLANEV"].ToString();
            adattablanev = view[viewind]["ADATTABLANEV"].ToString();
            szint = view[viewind]["SZINT"].ToString();
            azon = view[viewind]["AZON"].ToString();
            funkcio = view[viewind]["FUNKCIO"].ToString();
            mezonev = view[viewind]["MEZONEV"].ToString();
            kodtipus = view[viewind]["KODTIPUS"].ToString();
            if (kodtipus == "" || tablanev == "TARTAL")
                azontip = azon + tablanev;
            else
                azontip = azon + kodtipus;
            valttabinfo = null;
            if (tablanev == "BASE")
                valttabinfo = _basetag.Tablainfo;
            else if (azon == "LEIR")
                valttabinfo = _leirotablainfo;
            else if (adattablanev == "")
                valttabinfo = Tablainfok.GetByAzontip(azontip);
            else if (funkcio != "TOROL")
                valttabinfo = Tablainfok.GetBySzintPluszTablanev(szint, adattablanev).TablaTag.LeiroTablainfo;
            if (valttabinfo != null)
                Frissites(valttabinfo, view[viewind].Row);
            return true;
        }
        private void Frissites(Tablainfo tabinfo, DataRow dr)
        {
            string funkcio = dr["FUNKCIO"].ToString();
            string szint = dr["SZINT"].ToString();
            string verid = dr["VERZIO_ID"].ToString();
            if (!funkcio.Contains("VERSION"))
                ForceAdattolt(tabinfo, true);
            if (tabinfo.Tablanev.Contains("VERSION"))
            {
                string conn = tabinfo.Adattabla.Connection;
                Versiontolt(szint, conn);
                if (funkcio == "ADD" || funkcio == "TOROL")
                {
                    if (szint == "C")
                        Cegadatok(Aktintervallum);
                    else
                        Rendszeradatok(Aktintervallum, szint, funkcio == "TOROL");
                }
            }
            else
            {
                string tablanev = tabinfo.Tablanev;
                TablainfoTag tag = tabinfo.TablaTag;
                bool leiroe = tabinfo.Leiroe;
                if (funkcio.Contains("VERSION"))
                {
                    int[] verzioinfok = (int[])tabinfo.Verzioinfok.VersionIdCollection.ToArray(typeof(int));
                    if (funkcio == "DELETEVERSION")
                    {
                        tabinfo.AktVerzioId = verzioinfok[verzioinfok.Length - 2];
                        tabinfo.VerzioTerkepArray.RemoveAt(tabinfo.VerzioTerkepArray.Count - 1);
                    }
                    else
                    {
                        tabinfo.AktVerzioId = verzioinfok[verzioinfok.Length - 1];
                        tabinfo.VerzioTerkepArray.Add(tabinfo.AktVerzioId);
                    }
                    tabinfo.Adattabla.LastSel = tabinfo.SelectString + " and VERZIO_ID = " + tabinfo.AktVerzioId.ToString();
                    ForceAdattolt(tabinfo, true);
                    tabinfo.TablaTag.Azonositok.ToolTipText();
                    if (tabinfo.Osszefinfo != null)
                        tabinfo.Osszefinfo.InitKell = true;
                    return;
                }
                ForceAdattolt(tabinfo, true);
                if (tabinfo.Tablanev == "BASE" && funkcio == "TOROL")
                {
                    TablainfoTag termgyokertag = (TablainfoTag)GyokerTablainfoTagok[1];
                    foreach (TablainfoTag egytartaltag in termgyokertag.ChildTablainfoTagok)
                    {
                        if (egytartaltag.Azonositok.Szint == szint)
                            egytartaltag.Torolt = true;
                    }
                    return;
                }
                if (tabinfo.Azon == "LEIR")
                {
                    foreach (Tablainfo egyinfo in Tablainfok)
                    {
                        egyinfo.LeiroTablainfo.Beallit();
                        egyinfo.LeiroTablainfo.Combobeallit();
                    }
                    return;
                }
                if (tabinfo.Leiroe)
                {
                    if (tabinfo.TablaTag.Tablainfo.Adatfajta != "K")
                    {
                        tabinfo.Beallit();
                        tabinfo.Combobeallit();
                    }
                    else
                        KodtablakFrissitese(tabinfo);
                    Tablainfo adattablainfo = tabinfo.TablaTag.Tablainfo;
                    tabinfo.TablaTag.Tablainfo.Beallit();
                    tabinfo.TablaTag.Tablainfo.Combobeallit();
                    return;
                }
                if (tabinfo.Tablanev != "TARTAL" && tabinfo.Tablanev != "BASE")
                {
                    tabinfo.Beallit();
                    tabinfo.Combobeallit();
                    Combokupdate(tabinfo.TablaTag);
                    if (tabinfo.Osszefinfo != null)
                        tabinfo.Osszefinfo.InitKell = true;
                    return;
                }
                if (tabinfo.Tablanev == "BASE")
                {
                    string savfilt;
                    savfilt = tabinfo.DataView.RowFilter;
                    TablainfoTag newtag = null;
                    TablainfoTag newtartaltag = null;
                    tabinfo.DataView.RowFilter = "szint='" + szint + "'";
                    for (int i = 0; i < tabinfo.DataView.Count; i++)
                    {
                        DataRow dr1 = tabinfo.DataView[i].Row;
                        int rowindex = tabinfo.Adattabla.Rows.IndexOf(dr1);
                        if (funkcio == "ADD")
                        {
                            if (dr1["AZON"].ToString().Trim().Length < 4)
                                newtag = new TablainfoTag(tabinfo.Adattabla, rowindex, null, null, (TablainfoTag)GyokerTablainfoTagok[1], this);
                            else
                                newtartaltag = new TablainfoTag(tabinfo.Adattabla, rowindex, null, null, newtag, this);
                        }
                        else
                        {
                            TablainfoTag termgyokertag = (TablainfoTag)GyokerTablainfoTagok[1];
                            foreach (TablainfoTag egytartaltag in termgyokertag.ChildTablainfoTagok)
                            {
                                if (egytartaltag.Azonositok.Szint == szint)
                                {
                                    if (dr1["AZON"].ToString().Trim().Length < 4)
                                        egytartaltag.TablainfoTagUpdate(tabinfo.Adattabla, rowindex);
                                }
                            }
                        }
                    }
                    tabinfo.DataView.RowFilter = savfilt;
                }
                if (tabinfo.Tablanev == "TARTAL")
                {
                    Tablainfo egyinfo = null;
                    TablainfoTag tag1 = null;
                    DataRow dr1 = null;
                    AdatTabla adattabla = tabinfo.Adattabla;
                    int i = 0;
                    string kodtipus = dr["KODTIPUS"].ToString();
                    string azontip = dr["AZON"].ToString() + kodtipus;
                    string termszarm = azontip.Substring(0, 2).Trim();
                    if (azontip.Contains("SZRM"))
                        egyinfo = GetMezonevek(kodtipus);
                    else
                        egyinfo = Tablainfok.GetByAzontip(azontip);

                    for (i = 0; i < adattabla.Rows.Count; i++)
                    {
                        dr1 = adattabla.Rows[i];
                        if (azontip.Contains("SZRM") && dr1["KODTIPUS"].ToString() == kodtipus)
                            break;
                        if (!azontip.Contains("SZRM") && dr1["AZONTIP"].ToString() == azontip)
                            break;
                    }
                    if (egyinfo == null)
                    {
                        tag1 = new TablainfoTag(adattabla, i, null, null, tag, this);
                        tag1.LeiroTablainfo.Combobeallit();
                        tag1.Tablainfo.Combobeallit();
                        if ("CO".Contains(tag1.Azonositok.Adatfajta))
                            tag1.FordTag = new TablainfoTag(adattabla, i, tabinfo, tag1, tag, this, true);
                        if (termszarm == "T")
                            AzontipSzerintBevisz(azontip.Substring(2, 1) + egyinfo.Tablanev);
                        else
                            AzontipSzerintBevisz(azontip);
                    }
                    else
                    {
                        tag1 = egyinfo.TablaTag;
                        if (funkcio == "MODIFY")
                        {
                            if (tag1.ParentTag.ChildTablainfoTagok.IndexOf(egyinfo.Azontip) == -1)
                            {
                                tag1.SorIndex = i;
                                tag1.Torolt = false;
                            }
                            tag1.TablainfoTagUpdate(adattabla, i);
                        }
                        else
                            tag1.Torolt = true;
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cont"></param>
        public void RemoveAllControls(Control cont)
        {
            _eventtilt = true;
            for (int i = 0; i < cont.Controls.Count; i++)
            {
                cont.Controls.RemoveAt(i);
                i = -1;
            }
            _eventtilt = false;
        }
        public void Kezeloszereprendberak(Tablainfo szerepinfo)
        {
            if (szerepinfo != null)
            {
                string savfilt = szerepinfo.DataView.RowFilter;
                string filt = "";
                Tablainfo kezalkalm = GetOsszef("U", "KezeloAlkalm");
                string[] idk = GetTartal(kezalkalm, "SORSZAM1");
                if (idk.Length < szerepinfo.Adattabla.Rows.Count)
                {
                    foreach (string egyid in idk)
                    {
                        if (filt != "")
                            filt += "AND ";
                        filt += "KEZELO_ID <> " + egyid;
                    }
                    szerepinfo.DataView.RowFilter = filt;
                    for (int i = 0; i < szerepinfo.DataView.Count; i++)
                    {
                        szerepinfo.ViewSorindex = i;
                        szerepinfo.Adatsortorol(i);
                        i = -1;
                    }
                    szerepinfo.DataView.RowFilter = savfilt;
                }
                if (idk.Length > 1)
                {
                    for (int i = 0; i < idk.Length; i++)
                    {
                        szerepinfo.DataView.RowFilter = "KEZELO_ID = " + idk[i];
                        bool van = szerepinfo.DataView.Count != 0;
                        szerepinfo.DataView.RowFilter = "";
                        if (!van)
                        {
                            DataRow row = szerepinfo.Ujsor();
                            row["CEG_ID"] = AktualCegid;
                            row["ALKALMAZAS_ID"] = AlkalmazasId;
                            row["KEZELO_ID"] = idk[i];
                            row["SZEREPKOD"] = "10";
                        }
                    }
                }
                szerepinfo.DataView.RowFilter = savfilt;
                if (szerepinfo.Modositott)
                    Rogzit(szerepinfo);
            }
        }

        private void ValtozasRogzit(Tablainfo naplotablainfo, DataTable naplotabla, string azon, string tablanev, string adattablanev)
        {
            if (KellValtozasRogzit && tablanev != "VALTOZASNAPLO" && naplotablainfo != null)
            {
                DataView naploview = new DataView(naplotabla);
                AdatTabla valttabla = naplotablainfo.Adattabla;
                DataRow dr;
                DataRow dr1;
                naploview.RowFilter = "AZON='" + azon + "' and TABLANEV = '" + tablanev + "'";
                if (adattablanev != "")
                    naploview.RowFilter += " and ADATTABLANEV='" + adattablanev + "'";
                for (int i = 0; i < naploview.Count; i++)
                {
                    dr = naploview[i].Row;
                    if (dr["TABLANEV"].ToString() == tablanev && dr["AZON"].ToString() == azon && (adattablanev == ""
                        || dr["ADATTABLANEV"].ToString() == adattablanev))
                    {
                        dr1 = valttabla.NewRow();
                        for (int j = 0; j < naplotabla.Columns.Count; j++)
                        {
                            dr1[j] = dr[j];
                            dr1["MODOSITOTT_M"] = 1;
                        }
                        valttabla.Rows.Add(dr1);
                        dr.Delete();
                        i--;
                        naplotablainfo.Modositott = true;
                    }
                }
                UpdateTransaction(new Tablainfo[] { naplotablainfo });
                ValtoztatasDatuma = DateTime.Now;
                naploview.RowFilter = "";
            }
        }
    }
}




