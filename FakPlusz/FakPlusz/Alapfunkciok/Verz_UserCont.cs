using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Globalization;
using System.Data;
using System.Threading;
using FakPlusz;
using FakPlusz.Alapcontrolok;
namespace FakPlusz.Alapfunkciok
{
    /// <summary>
    /// Verzioinformaciok listaja
    /// </summary>
    public class VerzioinfoCollection : ArrayList
    {
        /// <summary>
        /// szintek
        /// </summary>
        public ArrayList szintek = new ArrayList();
        /// <summary>
        /// Listahoz ad
        /// </summary>
        /// <param name="verinf">
        /// verzioinfo
        /// </param>
        /// <param name="szint">
        /// szint
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public int Add(Verzioinfok verinf,string szint)
        {
            szintek.Add(szint);
            return base.Add(verinf);
        }
        /// <summary>
        /// torol
        /// </summary>
        /// <param name="szint">
        /// szint
        /// </param>
        public void Remove(string szint)
        {
            int i = szintek.IndexOf(szint);
            if (i != -1)
            {
                szintek.RemoveAt(i);
                base.RemoveAt(i);
            }
        }
        /// <summary>
        /// index alapjan keres
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public new Verzioinfok this[int index]
        {
            get { return (Verzioinfok)base[index]; }
        }
        /// <summary>
        /// szint alapjan keres
        /// </summary>
        /// <param name="szint"></param>
        /// <returns></returns>
        public Verzioinfok this[string szint]
        {
            get
            {
                int i = szintek.IndexOf(szint);
                if (i == -1)
                    return null;
                else
                    return (Verzioinfok)base[i];
            }
        }
        /// <summary>
        /// szinthez tartozo index?
        /// </summary>
        /// <param name="szint"></param>
        /// <returns></returns>
        public int IndexOf(string szint)
        {
            return szintek.IndexOf(szint);
        }
    }
    /// <summary>
    /// Az adatbazisok verzioinformacioi
    /// </summary>
    public class Verzioinfok
    {
        /// <summary>
        /// Ures lista
        /// </summary>
        public VerzioinfoCollection VerzioinfoCollection = new VerzioinfoCollection();
        /// <summary>
        /// ConnectionString
        /// </summary>
        public string AktualConnection = "";
        /// <summary>
        /// true, ha toroljuk az utolso verziot
        /// </summary>
        public bool Delete = false;
        /// <summary>
        /// verziokent a verziovaltas oka
        /// </summary>
        public string[] VerzValtasoka;
        /// <summary>
        /// verzio id-k tombje
        /// </summary>
        public int[] VersionArray;
        /// <summary>
        /// verzio-idk Array listben
        /// </summary>
        public ArrayList VersionIdCollection = new ArrayList();
        private int aktVerzioId = -1;
        /// <summary>
        /// true, ha az aktualis verzio lezart
        /// </summary>
        public bool LezartVersion = false;
        /// <summary>
        /// Aktualis verzio ervenyessegi hatarai
        /// </summary>
        public DateTime[] AktIntervallum;
        /// <summary>
        /// aktualis verzio kezdodatum
        /// </summary>
        public DateTime AktVerzioKezd
        {
            get { return AktIntervallum[0]; }
        }
        /// <summary>
        /// aktualis verzio vegdatum
        /// </summary>
        public DateTime AkVerzioVeg;
        /// <summary>
        /// verziointervallumok tombje
        /// </summary>
        public ArrayList VerzioDatumTerkep = new ArrayList();
        /// <summary>
        /// verzio adattabla
        /// </summary>
        public DataTable Verziotabla = new DataTable();
        /// <summary>
        /// Fak
        /// </summary>
        private FakUserInterface Fak;
        /// <summary>
        /// legmagasabb verzio id
        /// </summary>
        public int LastVersionId
        {
            get { return VersionArray[VersionArray.Length - 1]; }
        }
        /// <summary>
        /// aktualis verzioid, set eseten aktualizalja a tobbit is
        /// </summary>
        public int AktVerzioId
        {
            get { return aktVerzioId; }
            set
            {
                aktVerzioId = value;
                int i = VersionIdCollection.IndexOf(aktVerzioId);
                AktIntervallum = (DateTime[])VerzioDatumTerkep[i];
                if (i != VerzioDatumTerkep.Count - 1)
                    LezartVersion = true;
                else
                {
                    if (Verziotabla.Rows[i]["LEZART"].ToString().Trim() == "N")
                        LezartVersion = false;
                    else
                        LezartVersion = true;
                }
            }
        }
        /// <summary>
        /// lezart-e a legmagasabb verzio
        /// </summary>
        /// <returns></returns>
        public bool LastVerzioLezart()
        {
            if (Verziotabla.Rows[Verziotabla.Rows.Count - 1]["LEZART"].ToString().Trim() == "N")
                return false;
            else
                return true;
        }
        /// <summary>
        /// lezar-e megadott verzio
        /// </summary>
        /// <param name="aktverzio">
        /// a megadott verzio id
        /// </param>
        /// <returns>
        /// true: lezart
        /// </returns>
        public bool AktVerzioLezart(int aktverzio)
        {
            if (aktverzio != VersionArray[VersionArray.Length - 1])
                return true;
            else
                return LastVerzioLezart();
        }
        /// <summary>
        /// objectum letrehozasa
        /// </summary>
        /// <param name="fak">
        /// Fak
        /// </param>
        public Verzioinfok(FakUserInterface fak)
        {
            Fak = fak;
        }
        /// <summary>
        /// inicializalas
        /// </summary>
        /// <param name="adatconn">
        /// Connection string
        /// </param>
        /// <param name="szint">
        /// adatszint R/U/C
        /// </param>
        public void Init(string adatconn, string szint)
        {
            Verziotabla.Rows.Clear();
            VerzioDatumTerkep.Clear();
            VersionIdCollection.Clear();
            AktualConnection = adatconn;
            Verziotabla.TableName = szint + "VERSION";
            Verziotabla = Sqlinterface.Select(Verziotabla, adatconn, szint + "VERSION", "", " order by DATUMTOL", false);
            if (Verziotabla.Rows.Count == 0)
            {
                DataRow row = Verziotabla.NewRow();
                row["VERZIO_ID"] = 1;
                row["DATUMTOL"] = Convert.ToDateTime(DateTime.Today.Year.ToString()+".01.01");
                row["LEZART"] = "N";
                row["SZOVEG"] = "Indulás";
                Verziotabla.Rows.Add(row);
                Sqlinterface.ConnOpen(adatconn);
                Sqlinterface.BeginTransaction(adatconn);
                Sqlinterface.CommandBuilderUpd(adatconn,szint+"VERSION","VERSIONNUMBER",Verziotabla);
                Sqlinterface.CommitTransaction();
//                Sqlinterface.ConnClose(adatconn);
            }
            VersionArray = new int[Verziotabla.Rows.Count];
            VerzValtasoka = new string[Verziotabla.Rows.Count];
            int verid = 0;
            bool lezartversion = false;
            for (int i = 0; i < Verziotabla.Rows.Count; i++)
            {
                DateTime[] interv = new DateTime[2];
                DataRow dr = Verziotabla.Rows[i];
                string s = dr["DATUMTOL"].ToString();
                if (s == "")
                    interv[0] = Fak.Mindatum;
                else
                    interv[0] = Convert.ToDateTime(s);
                s = dr["DATUMIG"].ToString();
                if (s == "")
                    interv[1] = Fak.Mindatum;
                else
                    interv[1] = Convert.ToDateTime(s);
                VerzioDatumTerkep.Add(interv);
                verid = Convert.ToInt32(dr["VERZIO_ID"].ToString());
                VersionIdCollection.Add(verid);
                VersionArray[i] = verid;
                DateTime[] egyint = Fak.Aktintervallum;
                if (dr["LEZART"].ToString().Trim() == "I")
                    lezartversion = true;
                else
                    lezartversion = false;
                if (interv[0] == egyint[0] && interv[1] == egyint[1])
                {
                    aktVerzioId=verid;
                    AktIntervallum=interv;
                    LezartVersion = lezartversion;
                }
                else if((egyint[0].CompareTo(interv[1]) <= 0 || interv[1]== Fak.Mindatum) && egyint[1].CompareTo(interv[0]) >=0)
                {
                    aktVerzioId=verid;
                    AktIntervallum=interv;
                    LezartVersion = lezartversion;
                }
                VerzValtasoka[i] = dr["SZOVEG"].ToString();
            }
            //           }
            //else
            //{
            //    VersionArray = new int[1] { 1 };
            //    VersionIdCollection.Add(1);
            //    aktVerzioId = 1;
            //    AktIntervallum= new DateTime[]{Fak.Mindatum,Fak.Mindatum};
            //    VerzioDatumTerkep.Add(AktIntervallum);
            //}
        }
        /// <summary>
        /// adott intervallumban ervenyes verzio id kerese
        /// </summary>
        /// <param name="intervallum">
        /// intervallum
        /// </param>
        /// <returns>
        /// -1 vagy verzio id
        /// </returns>
        public int GetVerzioId(DateTime[] intervallum)
        {
            for (int i = 0; i < VerzioDatumTerkep.Count; i++)
            {
                DateTime[] egyint = (DateTime[])VerzioDatumTerkep[i];
                if (intervallum[0] == egyint[0] && intervallum[1] == egyint[1])
                    return VersionArray[i];
                if ((intervallum[0].CompareTo(egyint[1]) <= 0||egyint[1]==Fak.Mindatum) && intervallum[1].CompareTo(egyint[0]) >= 0)
                    return VersionArray[i];
            }
            return -1;
        }
    }
    /// <summary>
    /// UserControl informaciok gyujtemenye
    /// </summary>
    public class UserControlCollection : ArrayList
    {
        private ArrayList Controlok = new ArrayList();
//        private ArrayList Vezerlesek = new ArrayList();
        private ArrayList AktivMenuItemek = new ArrayList();
        private ArrayList AktivDropItemek = new ArrayList();
        private ArrayList AktivPagek = new ArrayList();
        private bool csakbase = false;
        /// <summary>
        /// true: ha csak az ArrayList Add-jat akarom vegrehajtani
        /// </summary>
        public bool Csakbase
        {
            get { return csakbase; }
            set { csakbase = value; }
        }
        /// <summary>
        /// objectum letrehozasa
        /// </summary>
        public UserControlCollection()
        {
        }
        /// <summary>
        /// kereses index szerint
        /// </summary>
        /// <param name="index">
        /// megadott index
        /// </param>
        /// <returns>
        /// keresett UserControlInfo
        /// </returns>
        public new UserControlInfo this[int index]
        {
            get { return (UserControlInfo)base[index]; }
            set { base[index] = value; }
        }
        /// <summary>
        /// Adott UserControl informaciojanak keresese
        /// </summary>
        /// <param name="cont">
        /// az adott UserControl
        /// </param>
        /// <returns>
        /// keresett UserControlInfo vagy null
        /// </returns>
        public UserControlCollection this[Control cont]
        {
            get
            {
                if (this.Count == 0)
                    return null;
                else
                {
                    UserControlCollection uscont = new UserControlCollection();
                    for (int i = 0; i < Controlok.Count; i++)
                    {
                        Control control = (Control)Controlok[i];
                        if (control.Name == cont.Name)
                            uscont.Add(this[i]);
                    }
                    if (uscont.Count == 0)
                        return null;
                    else
                        return uscont;
                }
            }
        }
        public UserControlCollection this[Control cont, ToolStripDropDownItem menuitem, ToolStripDropDownItem dropitem ]
        {
            get
            {
                if (this.Count == 0)
                    return null;
                else
                {
                    UserControlCollection uscont = new UserControlCollection();
                    for (int i = 0; i < Controlok.Count; i++)
                    {
                        Control control = (Control)Controlok[i];
                        if (control.Name == cont.Name && (menuitem == null 
                            || menuitem==(ToolStripDropDownItem)AktivMenuItemek[i])
                            && (dropitem==null || dropitem==(ToolStripDropDownItem)AktivDropItemek[i]))
                            uscont.Add(this[i]);
                    }
                    if (uscont.Count == 0)
                        return null;
                    else
                        return uscont;
                }
            }

        }
        /// <summary>
        /// Azon UserControlInfo-k keresese, melyeknek az AktivMenuItem-e azonos a keresettel
        /// </summary>
        /// <param name="menuitemname">
        /// a keresett menuitem
        /// </param>
        /// <returns>
        /// UserControlInfo gyujtemeny (lehet, hogy 0 hosszu)
        /// </returns>
        public UserControlCollection this[string menuitemname]
        {
            get
            {
                UserControlCollection ar = new UserControlCollection();
                ar.Csakbase = true;
                for (int i = 0; i < this.Count; i++)
                {
                    ToolStripMenuItem menuitem = (ToolStripMenuItem)AktivMenuItemek[i];

                    if (menuitem!=null && menuitem.Name  == menuitemname)
                        ar.Add(this[i]);
                }
                ar.Csakbase = false;
                return ar;
            }
        }
        /// <summary>
        /// Azon UserControlInfo keresese, melyeknek az AktivPage-e azonos a keresettel
        /// </summary>
        /// <param name="page">
        /// a keresett TabPage
        /// </param>
        /// <returns>
        /// UserControlInfo vagy null
        /// </returns>
        public UserControlInfo this[TabPage page]
        {
            get
            {
                for (int i = 0; i < this.Count; i++)
                {
                    TabPage item = (TabPage)AktivPagek[i];
                    if (page == item)
                        return this[i];
                }
                return null;
            }
        }
        /// <summary>
        /// kereses dropitem alapjan
        /// </summary>
        /// <param name="dropitem">
        /// a keresett dropitem
        /// </param>
        /// <returns>
        /// A Usercontrolinfo vagy null
        /// </returns>
        public UserControlInfo this[ToolStripMenuItem dropitem]
        {
            get
            {
                for (int i = 0; i < this.Count; i++)
                {
                    ToolStripMenuItem item = (ToolStripMenuItem)AktivDropItemek[i];
                    if (dropitem == item)
                        return this[i];
                }
                return null;
            }
        }
        /// <summary>
        /// kereses adott control es tablainformaciok tombje alapjan
        /// </summary>
        /// <param name="control">
        /// a control
        /// </param>
        /// <param name="tabinfok">
        /// tablainfok tombje
        /// </param>
        /// <returns>
        /// UserControlInfo, ha a UserControlInfo Control neve azonos a kivanteval es
        /// a tablainformacioi tablanevei azonosak a tablainfotomb tablaneveivel
        /// </returns>
        public UserControlInfo Find(Control control, Tablainfo[] tabinfok)
        {
            UserControlInfo uscont;
            for (int i = 0; i < Controlok.Count; i++)
            {
                if (((Control)Controlok[i]).Name == control.Name)
                {
                    uscont = this[i];
                    bool ok = true;
                    for (int j = 0; j < tabinfok.Length; j++)
                    {
                        bool egyok =false;
                        for (int k = 0; k < uscont.Tabinfok.Length; k++)
                        {
                            if (uscont.Tabinfok[k].Tablanev == tabinfok[j].Tablanev)
                            {
                                egyok = true;
                                break;
                            }
                        }
                        if (!egyok)
                        {
                            ok = false;
                            break;
                        }
                    }
                    if (ok)
                        return uscont;
                    else
                        return null;
                }
            }
            return null;
        }
        /// <summary>
        /// Uj elem hozzadasa
        /// </summary>
        /// <param name="value">
        /// az uj elem
        /// </param>
        /// <returns>
        /// az elem indexe
        /// </returns>
        public override int Add(object value)
        {
            int i = base.Add(value);
            if (!csakbase)
            {
                UserControlInfo val = (UserControlInfo)value;
                ToolStripMenuItem item = val.AktivMenuItem;
                ToolStripMenuItem dropitem = val.AktivDropDownItem;
                TabPage page = val.AktivPage;
                Controlok.Add(val.User);
                AktivMenuItemek.Add(item);
                AktivDropItemek.Add(dropitem);
                AktivPagek.Add(page);
            }
            return i;
        }
        /// <summary>
        /// AktivMenu hibat allit vagy torol
        /// </summary>
        /// <param name="menuitem">
        /// AktivMenu
        /// </param>
        public void SetOrClearError(ToolStripMenuItem menuitem)
        {
            ArrayList ar = this[menuitem.Name];
            string text="";
            for (int i = 0; i < ar.Count; i++)
            {
                UserControlInfo info = (UserControlInfo)ar[i];
                for (int j = 0; j < info.EgyContinfoArray.Count; j++)
                {
                    MezoControlInfo mezocinfo = info.EgyContinfoArray[j];
                    if (mezocinfo.Hosszuszoveg != "")
                    {
                        if (text != "")
                            text += "\n";
                        text += mezocinfo.Hosszuszoveg;
                    }
                }
            }
            if(text=="")
                menuitem.Text=menuitem.Text.Replace("!","");
            else if(!menuitem.Text.EndsWith("!"))
                menuitem.Text+="!";
            menuitem.ToolTipText = text;
        }
        /// <summary>
        /// AktivMenu changed jelzest torol, ha lehet
        /// </summary>
        /// <param name="item">
        /// Aktiv Menu
        /// </param>
        public void ClearChanged(ToolStripMenuItem item)
        {
            ArrayList ar = this[item.Name];
            bool changed = false;
            for (int i = 0; i < ar.Count; i++)
            {
                UserControlInfo info = (UserControlInfo)ar[i];
                for (int j = 0; j < info.EgyContinfoArray.Count; j++)
                {
                    MezoControlInfo mezocinfo = info.EgyContinfoArray[j];
                    if (mezocinfo.Tabinfo.DataView.Count!=0 && mezocinfo.Tabinfo.Changed)
                    {
                        changed = true;
                        break;
                    }
                }
            }
            if (!changed)
                item.Text = item.Text.Replace("*", "");
        }
    }
    /// <summary>
    /// User Control informacio
    /// </summary>
    public class UserControlInfo
    {
        private FakUserInterface Fak;
        /// <summary>
        /// A UserControl
        /// </summary>
        public Control User;
        /// <summary>
        /// Az a Mezotag, ahova utoljara leptunk
        /// </summary>
        public MezoTag LastEnter = null;
        /// <summary>
        /// A legelso MezoTag
        /// </summary>
        public MezoTag VeryFirstTag = null;
        /// <summary>
        /// A legutolso MezoTag
        /// </summary>
        public MezoTag VeryLastTag = null;
        /// <summary>
        /// A tablainformaciok
        /// </summary>
        public TablainfoCollection TabinfoArray = null;
        /// <summary>
        /// a tablainformaciok
        /// </summary>
        public Tablainfo[] Tabinfok
        {
            get
            {
                return (Tablainfo[])TabinfoArray.ToArray(typeof(Tablainfo));
            }
            set
            {
                TabinfoArray = new TablainfoCollection();
                for (int i = 0; i < value.Length; i++)
                    TabinfoArray.Add(value[i]);
            }
        }
        /// <summary>
        /// A mezoinformaciok
        /// </summary>
        public MezoControlCollection EgyContinfoArray = new MezoControlCollection();
        /// <summary>
        /// A TabPage
        /// </summary>
        public TabPage AktivPage;
        /// <summary>
        /// A menuitem
        /// </summary>
        public ToolStripMenuItem AktivMenuItem;
        /// <summary>
        /// Az almenuitem
        /// </summary>
        public ToolStripMenuItem AktivDropDownItem;
        private bool kellusercontrolvaltozas = false;
        /// <summary>
        /// Kell UserControl valtoztatas
        /// </summary>
        public bool KellUserControlValtozas
        {
            get { return kellusercontrolvaltozas; }
            set { kellusercontrolvaltozas = value; }
        }
        /// <summary>
        /// a vezerloinformacio
        /// </summary>
        public Vezerloinfo Vezerles;
        /// <summary>
        /// Az objectum letrehozasa
        /// </summary>
        /// <param name="user">
        /// a UserControl
        /// </param>
        /// <param name="vezerles">
        /// a vezerloinformacio
        /// </param>
        /// <param name="tabinfok">
        /// a tablainformaciok
        /// </param>
        /// <param name="tabpage">
        /// a TabPage vagy null,
        /// </param>
        /// <param name="menuitem">
        /// a menuitem vagy null
        /// </param>
        /// <param name="dropitem">
        /// az almenuitem vagy null
        /// </param>
        public UserControlInfo(Control user,Vezerloinfo vezerles, Tablainfo[] tabinfok, TabPage tabpage, ToolStripMenuItem menuitem, ToolStripMenuItem dropitem)
        {
            User = user;
            Vezerles = vezerles;
            Tabinfok = tabinfok;
            Fak = tabinfok[0].Fak;
            AktivPage = tabpage;
            AktivMenuItem = menuitem;
            AktivDropDownItem = dropitem;
            int usercontind = Fak.UserControlok.Add(this);
            for (int i = 0; i < tabinfok.Length; i++)
            {
                Tablainfo tabinfo = tabinfok[i];
                tabinfo.UserControlok.Add(this);
                MezoControlInfo egycont = tabinfo.AktualControlInfo; 
                if (egycont != null)
                {
                    egycont.UserControlInfo = Fak.UserControlok[usercontind];
                    EgyContinfoArray.Add(egycont);
                    MezotagCollection tagarr = egycont.InputelemArray;
                    for (int j = 0; j < tagarr.Count; j++)
                    {
                        MezoTag egytag = tagarr[j];
                        egytag.UserControlInfo = this;
                        egytag.EgyControlInfo = egycont;
                    }
                }
            }
        }
        /// <summary>
        /// Van-e hiba a a mezoinformaciok mezoiben?
        /// </summary>
        /// <returns>
        /// true:igen
        /// </returns>
        public bool Vanehiba()
        {
            bool hiba = false;
            foreach (MezoControlInfo continfo in EgyContinfoArray)
            {
                if (continfo.Vanehiba())
                    hiba = true;
            }
            return hiba;
        }
        /// <summary>
        /// Mezoinformaciok mezoinek hibavizsgalata
        /// </summary>
        /// <returns>
        /// true: hiba van
        /// </returns>
        public bool Hibavizsg()
        {
            bool hiba = false;
            foreach(MezoControlInfo continfo in EgyContinfoArray)
            {
                if (continfo.Hibavizsg())
                    hiba = true;
            }
            return hiba;
        }
        /// <summary>
        /// megkiserli a focust beallitani
        /// </summary>
        public void SetFocus()
        {
            MezoTag egytag;
            if (LastEnter == null)
                egytag = VeryFirstTag;
            else
                egytag = LastEnter;
            MezoTag ujtag = egytag;
            if (egytag != null)
            {
                if (!egytag.Control.Enabled)
                {
                    MezoControlCollection mezocont = this.EgyContinfoArray;
                    int k = -1;
                    foreach(MezoControlInfo mezoinfo in mezocont)
                    {
                            k = mezoinfo.InputelemArray.IndexOf(egytag);
                            if (k != -1)
                            {
                                for (int l = k; l < mezoinfo.Inputeleminfok.Length; l++)
                                {
                                    if (mezoinfo.Inputeleminfok[l].Control.Enabled)
                                    {
                                        ujtag = mezoinfo.Inputeleminfok[l];
                                        break;
                                    }
                                }
                            }
                    }
                }
            }
            else
                ujtag = egytag;
            if (ujtag != null)
                ujtag.Control.Focus();
        }
    }
}

