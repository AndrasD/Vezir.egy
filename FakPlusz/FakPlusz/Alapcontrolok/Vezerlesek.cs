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
using FakPlusz.SzerkesztettListak;
namespace FakPlusz.Alapcontrolok
{
    /// <summary>
    /// A Vezerloinfo strukturaba rendezesehez, valamely vezerles vagy vezerlesek keresesehez szukseges
    /// </summary>
    public class VezerloinfoCollection : ArrayList
    {
        ArrayList veznevek = new ArrayList();
        /// <summary>
        /// Objectum letrehozasa  
        /// </summary>
        public VezerloinfoCollection()
        {
        }
        /// <summary>
        /// kereses index szerint
        /// </summary>
        /// <param name="index">
        /// az index
        /// </param>
        /// <returns>
        /// vezerloinfo vagy null
        /// </returns>
        public new Vezerloinfo this[int index]
        {
            get
            {
                if (index == -1 || index >= this.Count)
                    return null;
                else
                    return (Vezerloinfo)base[index];
            }
            set
            {
                if (index > -1 && index < this.Count)
                    base[index] = value;
            }
        }
        /// <summary>
        /// Adott nevu Vezerloinfo keresese
        /// </summary>
        /// <param name="veznev"></param>
        /// <returns></returns>
        public Vezerloinfo this[string veznev]
        {
            get
            {
                int i = veznevek.IndexOf(veznev);
                if (i == -1)
                    return null;
                else
                    return (Vezerloinfo)this[i];
            }
        }
        /// <summary>
        /// Adott Vezerloinfo keresese
        /// </summary>
        /// <param name="vez"></param>
        /// <returns></returns>
        public Vezerloinfo this[Vezerloinfo vez]
        {
            get
            {
                int i = veznevek.IndexOf(vez.Name);
                if (i == -1)
                    return null;
                else
                    return this[i];
            }
        }
        /// <summary>
        /// info hozzaadas
        /// </summary>
        /// <param name="value">
        /// az info
        /// </param>
        /// <returns>
        /// listaindex
        /// </returns>
        public override int Add(object value)
        {
            Vezerloinfo egyvez = (Vezerloinfo)value;
            int i = veznevek.IndexOf(egyvez.Name);
            if (i == -1)
            {
                i = base.Add(value);
                veznevek.Add(egyvez.Name);
            }
            return i;
        }
        /// <summary>
        /// kollekcio hozzaadas
        /// </summary>
        /// <param name="c">
        /// a kollekcio
        /// </param>
        public override void AddRange(ICollection c)
        {
            object[] ob=(object[])c;
            for (int i = 0; i < c.Count; i++)
            {
                Add(ob[i]);
            }
        }
    }
    /// <summary>
    /// A Tervezo illetve az alkalmazasok UserControljainak letrehozasahoz, illetve megszolaltatasahoz 
    /// szukseges informaciokat tartalmazza
    /// </summary>
    public class Vezerloinfo
    {
        /// <summary>
        /// fakuserinterface
        /// </summary>
        public FakUserInterface Fak;

        private Base control = null;
        /// <summary>
        /// Melyik UserControl vezerlese
        /// </summary>
        public Base Control
        {
            get { return control; }
            set
            {
                control = value;
            }
        }
        private TabControl tabcontrol;
        /// <summary>
        /// A Vezerles TabControlja vagy null
        /// </summary>
        public TabControl TabControl
        {
            get { return tabcontrol; }
            set 
            {
                if (tabcontrol == null)
                {
                    tabcontrol = value;
                    for (int i = 0; i < ChildVezerloinfoCollection.Count; i++)
                        ChildVezerloinfoCollection[i].tabcontrol = value;
                }
            }
        }
        /// <summary>
        /// A TabControl TabPage-ei
        /// </summary>
        public ArrayList TabPagek = new ArrayList();
        /// <summary>
        /// A TabPagek-hez rendelt UserControlok nevei
        /// </summary>
        public ArrayList ControlNevek = new ArrayList();
        /// <summary>
        /// A mar letrehozott UserControlok, null tartalommal, ha meg nem lett letrehozva
        /// </summary>
        public Base[] LetezoControlok;
        /// <summary>
        /// Az eppen aktiv TabPage vagy null
        /// </summary>
        public TabPage AktivPage;
        /// <summary>
        /// Az eppen aktiv UserControl vagy null
        /// </summary>
        public Base AktivControl = null;
        /// <summary>
        /// 
        /// </summary>
        public ToolStripMenuItem AktivMenuItem = null;
        /// <summary>
        /// 
        /// </summary>
        public ToolStripMenuItem AktivDropDownItem = null;
        /// <summary>
        /// az objectumot letrehozo UserControl
        /// </summary>
        public Base Hivo;
        /// <summary>
        /// kezeloi szint
        /// </summary>
        public Base.KezSzint KezeloiSzint;
        /// <summary>
        /// hozzaferesi jogosultsag
        /// </summary>
        public Base.HozferJogosultsag HozferJog;
        /// <summary>
        /// A Vezerloinfo letrehozasanak strukturajahoz a Parent
        /// </summary>
        public Vezerloinfo ParentVezerles = null;
        /// <summary>
        /// A vezerles letrehozasanak strukturajahoz a childvezerlesek
        /// </summary>
        public VezerloinfoCollection ChildVezerloinfoCollection = new VezerloinfoCollection();
        /// <summary>
        /// A childvezerlesek kozul az eppen aktiv vagy null
        /// </summary>
        public Vezerloinfo AktivVezerles = null;
        /// <summary>
        /// A TabControl panelje vagy null
        /// </summary>
        public Panel MenuPanel;
        /// <summary>
        /// 
        /// </summary>
        public Panel TreePanel;
        /// <summary>
        /// 
        /// </summary>
        public TreeView TreeView;
        /// <summary>
        /// A MenuItemeket tartalmazza
        /// </summary>
        public MenuStrip MenuStrip= null;
        /// <summary>
        /// Az a menuitem, melynek Dropitemei kozt van a UserControlt aktivizalo dropitem
        /// </summary>
        public ToolStripMenuItem[] MenuItemek = null;
        /// <summary>
        /// A Dropitemek tombje
        /// </summary>
        public ArrayList DropItemek = new ArrayList();
        /// <summary>
        /// A vezerles neve
        /// </summary>
        public string Name;
        /// <summary>
        /// ures objectum letrehozasa 
        /// azert kell, hogy szukseg eseten az osztaly bovitheto legyen
        /// </summary>
        public Tablainfo termtabinfo;
        /// <summary>
        /// 
        /// </summary>
        public Tablainfo usernevek;
        /// <summary>
        /// 
        /// </summary>
        public Tablainfo allapotok;
        /// <summary>
        /// 
        /// </summary>
        public Tablainfo menuitemek;
        /// <summary>
        /// 
        /// </summary>
        public Tablainfo almenuitemek;
        /// <summary>
        /// 
        /// </summary>
        public Osszefinfo usernevusernev;
        /// <summary>
        /// 
        /// </summary>
        public Osszefinfo usernevallapoterzekeny;
        /// <summary>
        /// 
        /// </summary>
        public Osszefinfo usernevmenuitemek;
        /// <summary>
        /// 
        /// </summary>
        public Osszefinfo usernevalmenuitemek;
        /// <summary>
        /// 
        /// </summary>
        public Osszefinfo menuitemalmenuitemek;
        /// <summary>
        /// 
        /// </summary>
        public Osszefinfo usercontlistakep;
        /// <summary>
        /// 
        /// </summary>
        public string[] OsszesAllapotNev = null;
        /// <summary>
        /// 
        /// </summary>
        public string[] OsszesAllapotId = null;
        /// <summary>
        /// 
        /// </summary>
        public string[] MenuNevek = null;
        /// <summary>
        /// 
        /// </summary>
        public ArrayList OsszesControlNev = new ArrayList();
        /// <summary>
        /// 
        /// </summary>
        public ArrayList OsszesLetezoControl = new ArrayList();
        /// <summary>
        /// 
        /// </summary>
        public ArrayList AlmenuNevek = new ArrayList();
        /// <summary>
        /// 
        /// </summary>
        public ArrayList AlmenuUserControlNevek = new ArrayList();
        /// <summary>
        /// 
        /// </summary>
        public ArrayList LekerdezendoAllapotNevek = new ArrayList();
        /// <summary>
        /// 
        /// </summary>
        public ArrayList EredetiDropEnablek = new ArrayList();
        /// <summary>
        /// 
        /// </summary>
        public ArrayList LetezoUserControlNevek = new ArrayList();
        public Base.HozferJogosultsag[] UserContHozferJogok = null;
        /// <summary>
        /// 
        /// </summary>
        public ArrayList MultiUser = new ArrayList();
        /// <summary>
        /// 
        /// </summary>
        public ArrayList MultiUserToolTip = new ArrayList();
        /// <summary>
        /// 
        /// </summary>
        public Base.Parameterezes[] Parameterek = null;
        /// <summary>
        /// 
        /// </summary>
        public bool[] Teljeshonap = null;
        /// <summary>
        /// 
        /// </summary>
        public bool[] Teljesev = null;
        /// <summary>
        /// 
        /// </summary>
        public bool[] Csakegyhonap = null;
        /// <summary>
        /// 
        /// </summary>
        public bool[] Listae = null;
        /// <summary>
        /// 
        /// </summary>
        public bool[] Adatszolge = null;
        /// <summary>
        /// 
        /// </summary>
        public Parameterez[] Parameterez = null;
        /// <summary>
        /// 
        /// </summary>
        public Vezerloinfo()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fakuserinterface"></param>
        /// <param name="parentvezerles"></param>
        /// <param name="menuindex"></param>
        /// <param name="kezszint"></param>
        /// <param name="hozfer"></param>
        /// <param name="letezousercontnevek"></param>
        public Vezerloinfo(FakUserInterface fakuserinterface, Vezerloinfo parentvezerles, int menuindex, ref Base.KezSzint kezszint, ref Base.HozferJogosultsag hozfer,ArrayList letezousercontnevek)
        {
            Fak = fakuserinterface;
            KezeloiSzint = kezszint;
            HozferJog = hozfer;
            Vezerloinfo vez = parentvezerles;
            ParentVezerles = vez;
            usernevek = vez.usernevek;
            Parameterek = vez.Parameterek;
            Teljeshonap = vez.Teljeshonap;
            Teljesev = vez.Teljesev;
            Csakegyhonap = vez.Csakegyhonap;
            termtabinfo = vez.termtabinfo;
            menuitemek = vez.menuitemek;
            almenuitemek = vez.almenuitemek;
            //usernevusernev = vez.usernevusernev;
            usernevallapoterzekeny = vez.usernevallapoterzekeny;
            usernevmenuitemek = vez.usernevmenuitemek;
            usernevalmenuitemek = vez.usernevalmenuitemek;
            menuitemalmenuitemek = vez.menuitemalmenuitemek;
            usercontlistakep = vez.usercontlistakep;
            allapotok = vez.allapotok;
            OsszesAllapotId = vez.OsszesAllapotId;
            OsszesAllapotNev = vez.OsszesAllapotNev;
            MenuNevek = new string[] { vez.MenuNevek[menuindex] };
            Name = parentvezerles.Name + "Alvez" + menuindex.ToString();
            string[] menuidk = Fak.GetTartal(menuitemek, "SORSZAM", "SZOVEG", MenuNevek);
            string[] useridk = Fak.GetSzurtOsszefIdk(usernevmenuitemek, new object[] { "", menuidk });
            string[] usernev = Fak.GetTartal(usernevek, "SZOVEG", "ID", useridk);
            ControlNevek = new ArrayList(usernev);
            Parameterek = new Base.Parameterezes[ControlNevek.Count];
            Teljeshonap = new bool[ControlNevek.Count];
            Teljesev = new bool[ControlNevek.Count];
            Csakegyhonap = new bool[ControlNevek.Count];
            Parameterez = new Parameterez[ControlNevek.Count];
            Listae = new bool[ControlNevek.Count];
            Adatszolge=new bool[ControlNevek.Count];
            string[] almenuidk = Fak.GetSzurtOsszefIdk(usernevalmenuitemek, new object[] { useridk, "" });
            string[] all = null;
            Tablainfo igenneminfo = Fak.GetKodtab("R", "9997");
//            ArrayList ar = new ArrayList(OsszesAllapotId);
            for (int i = 0; i < usernev.Length; i++)
            {
                fakuserinterface.ProgressRefresh();
                string egynev = usernev[i];
                string[] id = Fak.GetTartal(usernevek, "ID", "SZOVEG", egynev);
                string[] paramok = Fak.GetTartal(usernevek, "PARAMETEREZES", "ID", id);
                string[] teljeshonap = Fak.GetTartal(usernevek, "TELJESHONAP", "ID", id);
                string[] teljesev = Fak.GetTartal(usernevek, "TELJESEV", "ID", id);
                string[] csakegyhonap = Fak.GetTartal(usernevek, "CSAKEGYHONAP", "ID", id);
                Parameterek[i] = (Base.Parameterezes)Convert.ToInt16(paramok[0]);
                Teljeshonap[i] = teljeshonap[0] == "I";
                Teljesev[i] = teljesev[0] == "I";
                Csakegyhonap[i] = csakegyhonap[0] == "I";
                if (Parameterek[i] != Base.Parameterezes.Nincsparameterezes)
                {
                    Parameterez[i] = new Parameterez();
                    Parameterez[i].Paramfajta = Parameterek[i];
                    Parameterez[i].TeljesHonap = Teljeshonap[i];
                    Parameterez[i].TeljesEv = Teljesev[i];
                    if (Teljesev[i])
                        Parameterez[i].CsakEgyHonap = true;
                    else
                        Parameterez[i].CsakEgyHonap = Csakegyhonap[i];
                    Parameterez[i].Dock = DockStyle.Fill;
                }
                string[] lekerdezendoidk = Fak.GetSzurtOsszefIdk(usernevallapoterzekeny, new object[] { id, "" });
                all = new string[] { "" };
                if(lekerdezendoidk!=null)
                    all = Fak.GetTartal(allapotok, "SZOVEG", "ID", lekerdezendoidk);
                LekerdezendoAllapotNevek.Add(all);
                string[] listakepidk = Fak.GetSzurtOsszefIdk(usercontlistakep, new object[] { id, "" });
                Listae[i] = listakepidk!=null;
                if (listakepidk != null)
                {
                    Tablainfo listainfo = Fak.GetBySzintPluszTablanev("R", "LISTAK");
                    Tablainfo specadatszolgnevek = Fak.GetBySzintPluszTablanev("R", "SPECADATSZOLGNEVEK");
                    Tablainfo adatszolginfo = Fak.GetBySzintPluszTablanev("R", "USERADATSZOLG");
                    Osszefinfo adatszolgspecfix = Fak.GetOsszef("R", "AdatszolgSpecfix").Osszefinfo;
                    string listanev = Fak.GetTartal(listainfo, "SZOVEG", "ID", listakepidk)[0];
                    string[] adatszolgnevek = Fak.GetTartal(listainfo, "ADATSZOLGNEV", "SZOVEG", listanev);
                    Adatszolge[i] = adatszolgnevek[0] != "";
                    if (Parameterez[i] != null)
                    {
                        Parameterez[i].UserContId = id[0];
                        Parameterez[i].UserContListakep = usercontlistakep;
                        Parameterez[i].ListaInfo = listainfo;
                        Parameterez[i].UserAdatSzolgInfo = adatszolginfo;
                        Parameterez[i].SpecAdatSzolgnevInfo = specadatszolgnevek;
                        string[] spec = new string[specadatszolgnevek.DataView.Count];
                        for (int j = 0; j < spec.Length; j++)
                            spec[j] = specadatszolgnevek.DataView[j]["SZOVEG"].ToString();
                        Parameterez[i].SpecFixertekNevek = new ArrayList(spec);
                        Parameterez[i].AdatszolgSpecfix = adatszolgspecfix;
                        Parameterez[i].Listae = Listae[i];
                        Parameterez[i].ListaNev = listanev;
                        Parameterez[i].Adatszolge = Adatszolge[i];
                    }
                }
                if (Parameterez[i] != null)
                {
                    Parameterez par = Parameterez[i];
                    par.FakUserInterface = Fak;
                    TabControl tabcont = par.tabControl1;
                    tabcont.Controls.Remove(par.listapage);
                    tabcont.Controls.Remove(par.adatbevitelpage);
                    if (Listae[i])
                        par.ListaAdatbevPage = par.listapage;
                    else
                    {
                        par.ListaAdatbevPage = par.adatbevitelpage;
                        par.label1.Text="";
                    }
                    tabcont.Controls.Add(par.ListaAdatbevPage);
                    Base.Parameterezes egypar = Parameterek[i];
                    par.Paramfajta = egypar;
                    switch (egypar)
                    {
                        case Base.Parameterezes.Datum:
                            par.VanDatum = true;
                            tabcont.Controls.Remove(par.listaparampage);
                            tabcont.Controls.Remove(par.egyszeruparampage);
                            tabcont.Controls.Remove(par.osszetettparampage);
                            par.datumparampanel.Controls.Remove(par.datumparamvalasztekpanel);
                            par.datumparamvalasztek.Visible = false;
                            par.TolLabel = par.datumparamtollabel;
                            par.TolPicker = par.datumparamtol;
                            par.IgLabel=par.datumparamiglabel;
                            par.IgPicker=par.datumparamig;
                            if (!Listae[i])
                            {
                                par.datumparamlista.Text = "Adatbevitel";
                                par.label1.Text="";
                            }
                            par.AlapertButtonok=new ToolStripButton[] {par.datumparamalapert};
                            par.ListaButtonok = new ToolStripButton[] { par.datumparamlista };
                            par.OkButtonok = new ToolStripButton[] { par.datumparamok };
                            if (par.TeljesEv)
                                par.datumparamtol.CustomFormat = "yyyy";
                            else if (!par.TeljesHonap)
                            {
                                par.datumparamtol.CustomFormat = "yyyyMMMMMMMMMMM.dd";
                                par.datumparamig.CustomFormat = "yyyyMMMMMMMMMMM.dd";
                            }
//                            par.IgPicker.CustomFormat = par.datumparamtol.CustomFormat;
//                            par.TolPicker.ShowUpDown = par.TeljesHonap || par.TeljesEv;
//                            par.IgPicker.ShowUpDown= par.TeljesHonap || par.TeljesEv;
                            break;
                        case Base.Parameterezes.Datumpluszvalasztek:
                            par.VanDatum = true;
                            par.VanValasztek = true;
                            tabcont.Controls.Remove(par.listaparampage);
                            tabcont.Controls.Remove(par.egyszeruparampage);
                            tabcont.Controls.Remove(par.osszetettparampage);
                            par.TolLabel = par.datumparamtollabel;
                            par.TolPicker = par.datumparamtol;
                            par.IgLabel=par.datumparamiglabel;
                            par.IgPicker=par.datumparamig;
                            par.Valasztek = par.datumparamvalasztek;
                            if(!Listae[i])
                            {
                                par.datumparamlista.Text = "Adatbevitel";
                                par.label1.Text="";
                            }
                            par.AlapertButtonok = new ToolStripButton[] { par.datumparamalapert };
                            par.ListaButtonok = new ToolStripButton[] { par.datumparamlista };
                            par.OkButtonok = new ToolStripButton[] { par.datumparamok };
                            if (par.TeljesEv)
                                par.datumparamtol.CustomFormat = "yyyy";
                            else if (!par.TeljesHonap)
                                par.datumparamtol.CustomFormat = "yyyyMMMMMMMMMMM.dd";
                            par.IgPicker.CustomFormat = par.datumparamtol.CustomFormat;
//                            par.TolPicker.ShowUpDown = par.TeljesHonap || par.TeljesEv;
//                            par.IgPicker.ShowUpDown= par.TeljesHonap || par.TeljesEv;
                           break;
                        case Base.Parameterezes.Listaparamok:
                            par.VanLista = true;
                            par.listaparamdatumpanel.Visible = false;
                            par.listaparamtoolstrip.Visible = false;
                            tabcont.Controls.Remove(par.datumparampage);
                            tabcont.Controls.Remove(par.egyszeruparampage);
                            tabcont.Controls.Remove(par.osszetettparampage);
                            break;
                        case Base.Parameterezes.ListaparamokpluszDatum:
                            par.VanDatum = true;
                            par.VanLista = true;
                            tabcont.Controls.Remove(par.datumparampage);
                            tabcont.Controls.Remove(par.egyszeruparampage);
                            tabcont.Controls.Remove(par.osszetettparampage);
                            break;
                        case Base.Parameterezes.Egyszeruszures:
                            par.VanEgyszeru = true;
                            tabcont.Controls.Remove(par.datumparampage);
                            tabcont.Controls.Remove(par.listaparampage);
                            tabcont.Controls.Remove(par.osszetettparampage);
                            par.egyszeruparampanel.Controls.Remove(par.egyszeruparamdatumpanel);
                            if (!Listae[i])
                            {
                                par.egyszeruparamalsolista.Text = "Adatbevitel";
                                par.label1.Text="";
                            }
                            par.AlapertButtonok = new ToolStripButton[] { par.egyszeruparamalapert };
                            par.MindButtonok = new ToolStripButton[] { par.egyszeruparamalsomind };
                            par.EgysemButtonok = new ToolStripButton[] { par.egyszeruparamalsoegysem };
                            par.OkButtonok = new ToolStripButton[] { par.egyszeruparamalsook };
                            par.ListaButtonok = new ToolStripButton[] { par.egyszeruparamalsolista };
                            break;
                        case Base.Parameterezes.EgyszeruszurespluszDatum:
                            par.VanDatum = true;
                            par.VanEgyszeru = true;
                            tabcont.Controls.Remove(par.datumparampage);
                            tabcont.Controls.Remove(par.listaparampage);
                            tabcont.Controls.Remove(par.osszetettparampage);
                            par.TolLabel = par.egyszeruparamtollabel;
                            par.TolPicker = par.egyszeruparamtol;
                            par.IgLabel=par.egyszeruparamiglabel;
                            par.IgPicker=par.egyszeruparamig;
                            if (!Listae[i])
                            {
                                par.egyszeruparamlista.Text = "Adatbevitel";
                                par.egyszeruparamalsolista.Text = "Adatbevitel";
                                par.label1.Text="";
                            }
                            par.AlapertButtonok = new ToolStripButton[] { par.egyszeruparamalapert,null};
                            par.MindButtonok = new ToolStripButton[] { null, par.egyszeruparamalsomind };
                            par.EgysemButtonok = new ToolStripButton[] { null, par.egyszeruparamalsoegysem };
                            par.ListaButtonok = new ToolStripButton[] { par.egyszeruparamlista, par.egyszeruparamalsolista };
                            par.OkButtonok = new ToolStripButton[] { par.egyszeruparamok, par.egyszeruparamalsook };
                            if (par.TeljesEv)
                                par.egyszeruparamtol.CustomFormat = "yyyy";
                            else if (!par.TeljesHonap)
                                par.datumparamtol.CustomFormat = "yyyyMMMMMMMMMMM.dd";
                            par.IgPicker.CustomFormat = par.datumparamtol.CustomFormat;
//                            par.TolPicker.ShowUpDown = par.TeljesHonap || par.TeljesEv;
//                            par.IgPicker.ShowUpDown= par.TeljesHonap || par.TeljesEv;
                            break;
                        case Base.Parameterezes.Osszetettszures:
                            par.VanOsszetett = true;
                            tabcont.Controls.Remove(par.datumparampage);
                            tabcont.Controls.Remove(par.listaparampage);
                            tabcont.Controls.Remove(par.egyszeruparampage);
                            par.osszetettparampanel.Controls.Remove(par.osszetettparamdatumpanel);
                            par.MindButtonok = new ToolStripButton[] { par.osszetettparamkozepsomind};
                            par.EgysemButtonok = new ToolStripButton[] { par.osszetettparamkozepsoegysem};
                            par.ListaButtonok = new ToolStripButton[] { par.osszetettparamkozepsolista};
                            par.OkButtonok = new ToolStripButton[] { par.osszetettparamkozepsook};
                            par.Radiobuttonok = new RadioButton[]{par.radioButton1,par.radioButton2,par.radioButton3,
                                par.radioButton4,par.radioButton5};
                            par.RadiobuttonViewk = new DataView[5];
                            par.RadiobuttonAllapotok = new bool[5];
                            break;
                        case Base.Parameterezes.OsszetettszurespluszDatum:
                            par.VanDatum = true;
                            par.VanOsszetett = true;
                            tabcont.Controls.Remove(par.datumparampage);
                            tabcont.Controls.Remove(par.listaparampage);
                            tabcont.Controls.Remove(par.egyszeruparampage);
                            par.TolLabel = par.osszetettparamtollabel;
                            par.TolPicker = par.osszetettparamtol;
                            par.IgLabel=par.osszetettparamiglabel;
                            par.IgPicker=par.osszetettparamig;
                            if (!Listae[i])
                            {
                                par.osszetettparamlista.Text = "Adatbevitel";
                                par.label1.Text="";
                            }
                            par.AlapertButtonok = new ToolStripButton[] { par.osszetettparamalapert};
                            par.MindButtonok = new ToolStripButton[] {null,par.osszetettparamkozepsomind};
                            par.EgysemButtonok = new ToolStripButton[] { null,par.osszetettparamkozepsoegysem};
                            par.OkButtonok = new ToolStripButton[] { par.osszetettparamfelsook, par.osszetettparamkozepsook };
                            par.ListaButtonok = new ToolStripButton[] { par.osszetettparamlista, par.osszetettparamkozepsolista };
                            par.Radiobuttonok = new RadioButton[]{par.radioButton1,par.radioButton2,par.radioButton3,
                                par.radioButton4,par.radioButton5};
                            par.RadiobuttonViewk= new DataView[5];
                            par.RadiobuttonAllapotok = new bool[5];
                            if (par.TeljesEv)
                                par.osszetettparamtol.CustomFormat = "yyyy";
                            else if (!par.TeljesHonap)
                                par.datumparamtol.CustomFormat = "yyyyMMMMMMMMMMM.dd";
//                             par.TolPicker.ShowUpDown = par.TeljesHonap || par.TeljesEv;
//                            par.IgPicker.ShowUpDown= par.TeljesHonap || par.TeljesEv;
                            par.IgPicker.CustomFormat = par.datumparamtol.CustomFormat;
                            break;
                    }
                    if (par.OkButtonok != null)
                    {
                        par.OkVolt = new bool[par.OkButtonok.Length];
                        for (int ii = 0; ii < par.OkVolt.Length; ii++)
                            par.OkVolt[ii] = true;
                    }
                    if (par.TolPicker != null)
                    {
                        if (par.CsakEgyHonap)
                        {
                            par.TolLabel.Text = "Adja meg a kivánt dátumot:";
                            if (par.TeljesEv)
                                par.TolLabel.Text = "Adja meg a kivánt évet:";
                            par.IgPicker.Visible = false;
                            par.IgLabel.Visible = false;
                            par.IgLabel = null;
                            par.IgPicker = null;
                        }
                    }
                    Fak.EventTilt = true;
                    par.tabControl1.SelectedIndex = 1;
                    Fak.EventTilt = false;
                }
            }
            LetezoUserControlNevek = parentvezerles.LetezoUserControlNevek;
            UserContHozferJogok = parentvezerles.UserContHozferJogok;
            OsszesControlNev = parentvezerles.OsszesControlNev;
            OsszesLetezoControl = parentvezerles.OsszesLetezoControl;
            if (vez.AlmenuNevek.Count > menuindex)
            {
                AlmenuNevek.Add(vez.AlmenuNevek[menuindex]);
                AlmenuUserControlNevek.Add(vez.AlmenuUserControlNevek[menuindex]);
                DropItemek.Add(vez.DropItemek[menuindex]);
                EredetiDropEnablek.Add(vez.EredetiDropEnablek[menuindex]);
                TabPagek.Add(vez.TabPagek[menuindex]);
            }
            else
            {
                AlmenuNevek.Add(null);
                AlmenuUserControlNevek.Add(null);
                DropItemek.Add(null);
                EredetiDropEnablek.Add(null);
                TabPagek.Add(null);
            }
            parentvezerles.ChildVezerloinfoCollection.Add(this);
            LetezoControlok = new Base[ControlNevek.Count];
            vez.LekerdezendoAllapotNevek.Add(LekerdezendoAllapotNevek);
        }
        public Vezerloinfo(FakUserInterface fakuserinterface, string controlnev, Vezerloinfo parentvezerles, ref Base.KezSzint kezszint, ref Base.HozferJogosultsag hozfer, ArrayList letezousercontnevek,ref Base.HozferJogosultsag[] userhozferek)
        {
             VezerlesInit(fakuserinterface, null, controlnev,-1,-1, parentvezerles, ref kezszint, ref hozfer, letezousercontnevek,ref userhozferek);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fakuserinterface"></param>
        /// <param name="controlnev"></param>
        /// <param name="parentvezerles"></param>
        /// <param name="kezszint"></param>
        /// <param name="hozfer"></param>
        /// <param name="letezousercontnevek"></param>
        public Vezerloinfo(FakUserInterface fakuserinterface, string controlnev, Vezerloinfo parentvezerles, ref Base.KezSzint kezszint, ref Base.HozferJogosultsag hozfer, ArrayList letezousercontnevek)
        {
            Base.HozferJogosultsag[] h = null;
            VezerlesInit(fakuserinterface, null, controlnev,-1,-1, parentvezerles, ref kezszint, ref hozfer, letezousercontnevek,ref h);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fakuserinterface"></param>
        /// <param name="controlnev"></param>
        /// <param name="menuindex"></param>
        /// <param name="parentvezerles"></param>
        /// <param name="kezszint"></param>
        /// <param name="hozfer"></param>
        /// <param name="letezousercontnevek"></param>
        public Vezerloinfo(FakUserInterface fakuserinterface, string controlnev, int menuindex, Vezerloinfo parentvezerles, ref Base.KezSzint kezszint, ref Base.HozferJogosultsag hozfer, ArrayList letezousercontnevek)
        {
            Base.HozferJogosultsag[] h = null;
            VezerlesInit(fakuserinterface, null, controlnev, menuindex, -1, parentvezerles, ref kezszint, ref hozfer, letezousercontnevek,ref h);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fakuserinterface"></param>
        /// <param name="control"></param>
        /// <param name="menuindex"></param>
        /// <param name="parentvezerles"></param>
        /// <param name="kezszint"></param>
        /// <param name="hozfer"></param>
        /// <param name="letezousercontnevek"></param>
        public Vezerloinfo(FakUserInterface fakuserinterface, Base control,int menuindex, Vezerloinfo parentvezerles, ref Base.KezSzint kezszint, ref Base.HozferJogosultsag hozfer, ArrayList letezousercontnevek)
        {
            Base.HozferJogosultsag[] h = null;
            VezerlesInit(fakuserinterface, control , control.Name,menuindex,-1, parentvezerles, ref kezszint, ref hozfer, letezousercontnevek,ref h);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fakuserinterface"></param>
        /// <param name="control"></param>
        /// <param name="menuindex"></param>
        /// <param name="almenuindex"></param>
        /// <param name="parentvezerles"></param>
        /// <param name="kezszint"></param>
        /// <param name="hozfer"></param>
        /// <param name="letezousercontnevek"></param>
        public Vezerloinfo(FakUserInterface fakuserinterface, Base control, int menuindex, int almenuindex, Vezerloinfo parentvezerles, ref Base.KezSzint kezszint, ref Base.HozferJogosultsag hozfer, ArrayList letezousercontnevek)
        {
            Base.HozferJogosultsag[] h = null;
            VezerlesInit(fakuserinterface, control, control.Name, menuindex, almenuindex, parentvezerles, ref kezszint, ref hozfer, letezousercontnevek,ref h);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fakuserinterface"></param>
        /// <param name="control"></param>
        /// <param name="controlnev"></param>
        /// <param name="menuindex"></param>
        /// <param name="almenuindex"></param>
        /// <param name="parentvezerles"></param>
        /// <param name="kezszint"></param>
        /// <param name="hozfer"></param>
        /// <param name="letezousercontnevek"></param>
        public void VezerlesInit(FakUserInterface fakuserinterface, Base control, string controlnev, int menuindex, int almenuindex, Vezerloinfo parentvezerles, ref Base.KezSzint kezszint, ref Base.HozferJogosultsag hozfer, ArrayList letezousercontnevek,ref Base.HozferJogosultsag[] conthozferek)
        {
            Fak = fakuserinterface;
            fakuserinterface.ProgressRefresh();
            Name = controlnev + "Vez";
            KezeloiSzint = kezszint;
            HozferJog = hozfer;
            ArrayList menunevek = new ArrayList();
            string[] menuidk = null;
            string[] menuitemidk = null;
            string[] useridk = null;
            string[] usernev = null;
            string[] almenuidk = null;
            string[] almenuitemidk = null;
            string[] hivottnevek = null;
            string[] hivottnevidk = null;
            string id;
            Vezerloinfo vez = null;
            string usernevfilter = "";
            if (control != null)
                usernevfilter = ((VezerloAlapControl)control).UserNevFilter;
            else if (parentvezerles != null && parentvezerles.Control != null)
                usernevfilter = ((VezerloAlapControl)parentvezerles.Control).UserNevFilter;
            if (usernevfilter == "")
            {
                if (Fak.Alkalmazas != "TERVEZO")
                    usernevfilter = "ALKALMAZAS_ID=" + Fak.AlkalmazasId;
                else
                    usernevfilter = "ALKALMAZAS_ID=0";
            }
            if (parentvezerles == null)
            {
                if (Fak.Alkalmazas != "TERVEZO")
                {
                    LetezoUserControlNevek = letezousercontnevek;
                    UserContHozferJogok = conthozferek;
                }
                usernevek = Fak.GetBySzintPluszTablanev("R", "USERCONTROLNEVEK");
                Parameterek = new Base.Parameterezes[usernevek.DataView.Count];
                Teljeshonap = new bool[usernevek.DataView.Count];
                Teljesev = new bool[usernevek.DataView.Count];
                Csakegyhonap = new bool[usernevek.DataView.Count];
                allapotok = Fak.GetBySzintPluszTablanev("R", "USERALLAPOTOK");
                menuitemek = Fak.GetKodtab("R", "Menupontok");
                almenuitemek = Fak.GetKodtab("R", "Almenupontok");
                usernevusernev = Fak.GetOsszef("R", "UserContStru").Osszefinfo;
                usernevallapoterzekeny = Fak.GetOsszef("R", "Alkalmallapoterzekeny").Osszefinfo;
                usernevmenuitemek = Fak.GetOsszef("R", "UserContMenu").Osszefinfo;
                usernevalmenuitemek = Fak.GetOsszef("R", "UserContAlmenu").Osszefinfo;
                menuitemalmenuitemek = Fak.GetOsszef("R", "MenuAlmenu").Osszefinfo;
                usercontlistakep = Fak.GetOsszef("R", "UserContListakep").Osszefinfo;
                OsszesAllapotId = Fak.GetTartal(allapotok, "ID", "SZOVEG", "");
                OsszesAllapotNev = Fak.GetTartal(allapotok, "SZOVEG", "ID", "");
                termtabinfo = Fak.GetBySzintPluszTablanev("R", "TABLANEVEK");
            }
            else
            {
                vez = parentvezerles;
                Hivo = parentvezerles.Control;
                LetezoUserControlNevek = vez.LetezoUserControlNevek;
                UserContHozferJogok = vez.UserContHozferJogok;
                usernevek = vez.usernevek;
                Parameterek = vez.Parameterek;
                Teljeshonap = vez.Teljeshonap;
                Teljesev = vez.Teljesev;
                Csakegyhonap = vez.Csakegyhonap;
                allapotok = vez.allapotok;
                termtabinfo = vez.termtabinfo;
                menuitemek = vez.menuitemek;
                almenuitemek = vez.almenuitemek;
                usernevusernev = vez.usernevusernev;
                usernevallapoterzekeny = vez.usernevallapoterzekeny;
                usernevmenuitemek = vez.usernevmenuitemek;
                usernevalmenuitemek = vez.usernevalmenuitemek;
                menuitemalmenuitemek = vez.menuitemalmenuitemek;
                usercontlistakep = vez.usercontlistakep;
                OsszesAllapotId = vez.OsszesAllapotId;
                OsszesAllapotNev = vez.OsszesAllapotNev;
                OsszesControlNev = vez.OsszesControlNev;
                OsszesLetezoControl = vez.OsszesLetezoControl;
            }
            Fak.ProgressRefresh();
            if (control != null)
            {
                if (OsszesControlNev.IndexOf(control.Name) == -1)
                {
                    OsszesControlNev.Add(control.Name);
                    OsszesLetezoControl.Add(control);
                    if (control.Name == "Formvezerles")
                    {
                        parentvezerles.LetezoUserControlNevek.Add(control.Name);
                        ArrayList ar = new ArrayList(parentvezerles.LetezoControlok);
                        ar.Add(control);
                        parentvezerles.LetezoControlok = (Base[])ar.ToArray(typeof(Base));
                    }
                }
                MenuStrip = control.MenuStrip;
                if (MenuStrip != null)
                {

                    MenuItemek = new ToolStripMenuItem[MenuStrip.Items.Count];
                    MenuNevek = new string[MenuItemek.Length];
                    for (int i = 0; i < MenuItemek.Length; i++)
                    {
                        Fak.ProgressRefresh();
                        MenuItemek[i] = (ToolStripMenuItem)MenuStrip.Items[i];
                        MenuNevek[i] = MenuItemek[i].Text;
                        ArrayList droparray = new ArrayList();
                        ArrayList almenuar = new ArrayList();
                        ArrayList tabpagear = new ArrayList();
                        for (int j = 0; j < MenuItemek[i].DropDownItems.Count; j++)
                        {
                            droparray.Add(MenuItemek[i].DropDownItems[j]);
                            string nev = MenuItemek[i].DropDownItems[j].Text;
                            almenuar.Add(nev);
                            TabPage p = new TabPage(nev);
                            //p.AutoScroll = true;
                            //p.AutoScrollMargin = new Size(3, 3);
                            tabpagear.Add(p);
                        }
                        string[] almenuks = (string[])almenuar.ToArray(typeof(string));
                        DropItemek.Add((ToolStripMenuItem[])droparray.ToArray(typeof(ToolStripMenuItem)));
                        AlmenuNevek.Add(almenuks);
                        TabPage[] pagek = (TabPage[])tabpagear.ToArray(typeof(TabPage));
                        TabPagek.Add(pagek);
                        EredetiDropEnablek.Add(new bool[almenuks.Length]);
                    }
                    for (int i = 0; i < MenuNevek.Length; i++)
                    {
                        Fak.ProgressRefresh();
                        menuidk = Fak.GetTartal(menuitemek, "SORSZAM", "SZOVEG", MenuNevek[i]);
                        if (menuidk != null)
                        {
                            hivottnevidk = Fak.GetSzurtOsszefIdk(usernevmenuitemek, new object[] { "", menuidk });
                            if (hivottnevidk != null)
                                hivottnevek = Fak.GetTartal(usernevek, "SZOVEG", "ID", hivottnevidk);
                        }
                        TabPage[] pagek = (TabPage[])TabPagek[i];
                        if (menuidk == null)
                            AlmenuUserControlNevek.Add(null);
                        else
                        {
                            almenuidk = Fak.GetSzurtOsszefIdk(menuitemalmenuitemek, new object[] { menuidk, "" });
                            if (almenuidk == null)
                                AlmenuUserControlNevek.Add(null);
                            else
                            {
                                useridk = Fak.GetSzurtOsszefIdk(usernevalmenuitemek, new object[] { "", almenuidk });
                                if (useridk == null)
                                    AlmenuUserControlNevek.Add(null);
                                else
                                {
                                    string[] usernevpagek = new string[pagek.Length];
                                    if (Fak.Alkalmazas == "TERVEZO" || control.Name == "Formvezerles")
                                        usernev = Fak.GetTartal(usernevek, "SZOVEG", "ID", useridk);
                                    else
                                        usernev = hivottnevek;
                                    bool multi = false;
                                    if (usernev.Length > usernevpagek.Length)
                                    {
                                        multi = true;
                                        MultiUser.Add(usernev);
                                        ArrayList tooltipek = new ArrayList();
                                        for (int j = 0; j < usernev.Length; j++)
                                        {
                                            int k = Valtozaskezeles.uscontnevekarray.IndexOf(usernev[j]);
                                            string tooltipszov = Valtozaskezeles.tooltiptextek[k];
                                            tooltipek.Add(tooltipszov);
                                            if (OsszesControlNev.IndexOf(usernev[j]) == -1)
                                            {
                                                OsszesControlNev.Add(usernev[j]);
                                                OsszesLetezoControl.Add(null);
                                            }
                                        }
                                        MultiUserToolTip.Add((string[])tooltipek.ToArray(typeof(string)));
                                        AlmenuUserControlNevek.Add(new string[] { usernev[0] });
                                    }
                                    else
                                    {
                                        MultiUser.Add(null);
                                        MultiUserToolTip.Add(null);
                                        for (int j = 0; j < usernev.Length; j++)
                                            usernevpagek[j] = usernev[j];
                                        for (int j = usernev.Length; j < pagek.Length; j++)
                                            usernevpagek[j] = usernev[usernev.Length - 1];
                                        usernev = usernevpagek;
                                        for (int j = 0; j < usernev.Length; j++)
                                        {
                                            int k = Valtozaskezeles.uscontnevekarray.IndexOf(usernev[j]);
                                            string toolstripszov = Valtozaskezeles.tooltiptextek[k];
                                            pagek[j].ToolTipText = toolstripszov;
                                        }
                                        AlmenuUserControlNevek.Add(usernev);
                                    }
                                    if (multi)
                                    {
                                        if (Parameterek.Length != OsszesControlNev.Count)
                                        {
                                            Parameterek = new Base.Parameterezes[OsszesControlNev.Count];
                                            Parameterez = new Parameterez[OsszesControlNev.Count];
                                            Teljeshonap = new bool[OsszesControlNev.Count];
                                            Teljesev = new bool[OsszesControlNev.Count];
                                            Csakegyhonap = new bool[OsszesControlNev.Count];
                                            Listae = new bool[OsszesControlNev.Count];
                                        }
                                        for (int k = 0; k < usernev.Length; k++)
                                        {
                                            string egynev = usernev[k];
                                            int j = OsszesControlNev.IndexOf(egynev);
                                            string[] id1 = Fak.GetTartal(usernevek, "ID", "SZOVEG", egynev);
                                            string[] paramok = Fak.GetTartal(usernevek, "PARAMETEREZES", "ID", id1);
                                            string[] teljeshonap = Fak.GetTartal(usernevek, "TELJESHONAP", "ID", id1);
                                            string[] teljesev = Fak.GetTartal(usernevek, "TELJESEV", "ID", id1);
                                            string[] csakegyhonap = Fak.GetTartal(usernevek, "CSAKEGYHONAP", "ID", id1);
                                            Parameterek[j] = (Base.Parameterezes)Convert.ToInt16(paramok[0]);
                                            Teljeshonap[j] = teljeshonap[0] == "I";
                                            Teljesev[j] = teljesev[0] == "I";
                                            Csakegyhonap[j] = csakegyhonap[0] == "I";
                                            if (Parameterek[j] != Base.Parameterezes.Nincsparameterezes)
                                            {
                                                Parameterez[j] = new Parameterez();
                                                Parameterez[j].Paramfajta = Parameterek[j];
                                                Parameterez[j].TeljesHonap = Teljeshonap[j];
                                                Parameterez[j].TeljesEv = Teljesev[j];
                                                if (Teljesev[j])
                                                    Parameterez[j].CsakEgyHonap = true;
                                                else
                                                    Parameterez[j].CsakEgyHonap = Csakegyhonap[j];
                                                Parameterez[j].Dock = DockStyle.Fill;
                                                Parameterez par = Parameterez[j];
                                                par.FakUserInterface = Fak;
                                                TabControl tabcont = par.tabControl1;
                                                tabcont.Controls.Remove(par.listapage);
                                                tabcont.Controls.Remove(par.adatbevitelpage);
                                                if (Listae[i])
                                                    par.ListaAdatbevPage = par.listapage;
                                                else
                                                {
                                                    par.ListaAdatbevPage = par.adatbevitelpage;
                                                    par.label1.Text = "";
                                                }
                                                tabcont.Controls.Add(par.ListaAdatbevPage);
                                                Base.Parameterezes egypar = Parameterek[j];
                                                par.Paramfajta = egypar;
                                                switch (egypar)
                                                {
                                                    case Base.Parameterezes.Datum:
                                                        par.VanDatum = true;
                                                        tabcont.Controls.Remove(par.listaparampage);
                                                        tabcont.Controls.Remove(par.egyszeruparampage);
                                                        tabcont.Controls.Remove(par.osszetettparampage);
                                                        par.datumparampanel.Controls.Remove(par.datumparamvalasztekpanel);
                                                        par.datumparamvalasztek.Visible = false;
                                                        par.TolLabel = par.datumparamtollabel;
                                                        par.TolPicker = par.datumparamtol;
                                                        par.IgLabel = par.datumparamiglabel;
                                                        par.IgPicker = par.datumparamig;
                                                        if (!Listae[i])
                                                        {
                                                            par.datumparamlista.Text = "Adatbevitel";
                                                            par.label1.Text = "";
                                                        }
                                                        par.AlapertButtonok = new ToolStripButton[] { par.datumparamalapert };
                                                        par.ListaButtonok = new ToolStripButton[] { par.datumparamlista };
                                                        par.OkButtonok = new ToolStripButton[] { par.datumparamok };
                                                        if (par.TeljesEv)
                                                        {
                                                            par.datumparamtol.CustomFormat = "yyyy";
                                                            par.datumparamtol.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
                                                        }
                             //                           par.TolPicker.ShowUpDown = par.TeljesHonap || par.TeljesEv;
                             //                           par.IgPicker.ShowUpDown= par.TeljesHonap || par.TeljesEv;
                                                        break;
                                                    case Base.Parameterezes.Datumpluszvalasztek:
                                                        par.VanDatum = true;
                                                        par.VanValasztek = true;
                                                        tabcont.Controls.Remove(par.listaparampage);
                                                        tabcont.Controls.Remove(par.egyszeruparampage);
                                                        tabcont.Controls.Remove(par.osszetettparampage);
                                                        par.TolLabel = par.datumparamtollabel;
                                                        par.TolPicker = par.datumparamtol;
                                                        par.IgLabel = par.datumparamiglabel;
                                                        par.IgPicker = par.datumparamig;
                                                        par.Valasztek = par.datumparamvalasztek;
                                                        if (!Listae[i])
                                                        {
                                                            par.datumparamlista.Text = "Adatbevitel";
                                                            par.label1.Text = "";
                                                        }
                                                        par.AlapertButtonok = new ToolStripButton[] { par.datumparamalapert };
                                                        par.ListaButtonok = new ToolStripButton[] { par.datumparamlista };
                                                        par.OkButtonok = new ToolStripButton[] { par.datumparamok };
                                                        if (par.TeljesEv)
                                                        {
                                                            par.datumparamtol.CustomFormat = "yyyy";
                                                            par.datumparamtol.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
                                                        }
//                             par.TolPicker.ShowUpDown = par.TeljesHonap || par.TeljesEv;
//                            par.IgPicker.ShowUpDown= par.TeljesHonap || par.TeljesEv;
                                                       break;
                                                }
                                                if (par.OkButtonok != null)
                                                {
                                                    par.OkVolt = new bool[par.OkButtonok.Length];
                                                    for (int ii = 0; ii < par.OkVolt.Length; ii++)
                                                        par.OkVolt[ii] = true;
                                                }
                                                if (par.TolPicker != null)
                                                {
                                                    if (par.CsakEgyHonap)
                                                    {
                                                        par.TolLabel.Text = "Adja meg a kivánt dátumot:";
                                                        if (par.TeljesEv)
                                                            par.TolLabel.Text = "Adja meg a kivánt évet:";
                                                        par.IgPicker.Visible = false;
                                                        par.IgLabel.Visible = false;
                                                        par.IgLabel = null;
                                                        par.IgPicker = null;
                                                    }
                                                }
                                                Fak.EventTilt = true;
                                                par.tabControl1.SelectedIndex = 0;
                                                Fak.EventTilt = false;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                MenuPanel = control.MenuPanel;
                TreePanel = control.TreePanel;
                TreeView = control.TreeView;
                TabControl = control.TabControl;
                Control = control;
            }
            if (menuindex != -1)
            {
                hivottnevek = new string[] { controlnev };
                ControlNevek = new ArrayList(hivottnevek);
                usernevek.DataView.RowFilter = usernevfilter;
                id = Fak.GetTartal(usernevek, "ID", "SZOVEG", controlnev)[0];
                hivottnevidk = Fak.GetSzurtOsszefIdk(usernevusernev, new object[] { id, "" });
                if (control == null)
                {
                    MenuNevek = new string[] { parentvezerles.MenuNevek[menuindex] };
                    AlmenuNevek.Add(parentvezerles.AlmenuNevek[menuindex]);
                    AlmenuUserControlNevek.Add(parentvezerles.AlmenuUserControlNevek[menuindex]);
                    DropItemek.Add(parentvezerles.DropItemek[menuindex]);
                    TabPagek.Add(parentvezerles.TabPagek[menuindex]);
                    EredetiDropEnablek.Add(parentvezerles.EredetiDropEnablek[menuindex]);
                    Name = parentvezerles.Name + "Alvez" + controlnev + almenuindex.ToString();
                    ControlNevek = AlmenuUserControlNevek;
                    if (ControlNevek.Count != 0)
                        LetezoControlok = new Base[((string[])ControlNevek[0]).Length];
                }
            }
            else
            {
                usernevek.DataView.RowFilter = usernevfilter;
                id = Fak.GetTartal(usernevek, "ID", "SZOVEG", controlnev)[0];
                hivottnevidk = Fak.GetSzurtOsszefIdk(usernevusernev, new object[] { id, "" });
                hivottnevek = null;
                almenuitemidk = null;
                if (hivottnevidk != null)
                {
                    usernevek.DataView.RowFilter = usernevfilter;
                    hivottnevek = Fak.GetTartal(usernevek, "SZOVEG", "ID", hivottnevidk);
                }
                else
                {
                    hivottnevidk = new string[] { id };
                    hivottnevek = new string[] { controlnev };
                }
                ControlNevek = new ArrayList(hivottnevek);
                if (control == null)
                {
                    menuitemidk = Fak.GetSzurtOsszefIdk(usernevmenuitemek, new object[] { id, "" });
                    if (menuitemidk != null)
                    {
                        MenuNevek = Fak.GetTartal(menuitemek, "SZOVEG", "SORSZAM", menuitemidk);
                        menunevek = new ArrayList(MenuNevek);
                    }
                    else
                    {
                        foreach (string nev in hivottnevek)
                        {
                            Fak.ProgressRefresh();
                            usernevek.DataView.RowFilter = usernevfilter;
                            string[] egyid = Fak.GetTartal(usernevek, "ID", "SZOVEG", nev);
                            menuitemidk = Fak.GetSzurtOsszefIdk(usernevmenuitemek, new object[] { egyid[0], "" });
                            if (menuitemidk != null)
                            {
                                string[] mennevek = Fak.GetTartal(menuitemek, "SZOVEG", "SORSZAM", menuitemidk);
                                foreach (string mennev in mennevek)
                                {
                                    if (menunevek.IndexOf(mennev) == -1)
                                        menunevek.Add(mennev);
                                }
                            }
                        }
                        if (menunevek.Count != 0)
                            MenuNevek = (string[])menunevek.ToArray(typeof(string));
                    }
                    if (MenuStrip != null && MenuNevek.Length != MenuStrip.Items.Count)
                    {
                        Fak.BajVan = true;
                        FakPlusz.MessageBox.Show(" paraméterezett menüneveinek száma:" + MenuNevek.Length.ToString() + "" + MenuStrip.Items.Count.ToString(), controlnev);
                    }
                    for (int i = 0; i < menunevek.Count; i++)
                    {
                        Fak.ProgressRefresh();
                        string keres = menunevek[i].ToString();
                        id = Fak.GetTartal(menuitemek, "SORSZAM", "SZOVEG", keres)[0];
                        almenuitemidk = Fak.GetSzurtOsszefIdk(menuitemalmenuitemek, new object[] { new string[] { id }, "" });
                        string[] almenunevek = null;
                        if (almenuitemidk != null)
                        {
                            almenunevek = Fak.GetTartal(almenuitemek, "SZOVEG", "SORSZAM", almenuitemidk);
                            AlmenuNevek.Add(almenunevek);
                            string[] tooltipszovegek = null;
                            string[] almenucontnevek = null;
                            string[] almenucontnevidk = Fak.GetSzurtOsszefIdk(usernevalmenuitemek, new object[] { hivottnevidk, almenuitemidk });
                            if (almenucontnevidk != null)
                            {
                                almenucontnevidk = Fak.GetTartal(usernevalmenuitemek.tabinfo, "SORSZAM1", "SORSZAM", almenucontnevidk);
                                usernevek.DataView.RowFilter = usernevfilter;
                                almenucontnevek = Fak.GetTartal(usernevek, "SZOVEG", "ID", almenucontnevidk);
                                tooltipszovegek = new string[almenucontnevek.Length];
                                for (int jj = 0; jj < almenucontnevek.Length; jj++)
                                {
                                    int k = Valtozaskezeles.uscontnevekarray.IndexOf(almenucontnevek[jj]);
                                    tooltipszovegek[jj] = Valtozaskezeles.tooltiptextek[k];
                                }
                                AlmenuUserControlNevek.Add(almenucontnevek);
                            }
                            else
                                AlmenuUserControlNevek.Add(null);
                            DropItemek.Add(new ToolStripMenuItem[almenunevek.Length]);
                            EredetiDropEnablek.Add(new bool[almenunevek.Length]);
                            TabPage[] tabpagek = new TabPage[almenunevek.Length];
                            for (int j = 0; j < almenunevek.Length; j++)
                            {
                                tabpagek[j] = new TabPage(almenunevek[j]);
                                //tabpagek[j].AutoScroll = true;
                                //tabpagek[j].AutoScrollMargin = new Size(3, 3);
                                if (tooltipszovegek != null && tooltipszovegek.Length > j)
                                    tabpagek[j].ToolTipText = tooltipszovegek[j];
                            }
                            TabPagek.Add(tabpagek);
                        }
                        else
                        {
                            TabPagek.Add(null);
                            AlmenuNevek.Add(null);
                            DropItemek.Add(null);
                            EredetiDropEnablek.Add(null);
                            AlmenuUserControlNevek.Add(null);
                        }
                    }
                }
                if (ControlNevek.Count != 0)
                    LetezoControlok = new Base[ControlNevek.Count];
            }
            if (control != null)
            {
                for (int i = 0; i < AlmenuUserControlNevek.Count; i++)
                {
                    string[] nevek = (string[])AlmenuUserControlNevek[i];
                    if (nevek != null)
                    {
                        for (int j = 0; j < nevek.Length; j++)
                        {
                            string egynev = nevek[j];
                            if (OsszesControlNev.IndexOf(egynev) == -1)
                            {
                                OsszesControlNev.Add(egynev);
                                OsszesLetezoControl.Add(null);
                            }
                        }
                    }
                }
            }
            ParentVezerles = parentvezerles;
            if (parentvezerles != null)
                parentvezerles.ChildVezerloinfoCollection.Add(this);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="droparray"></param>
        public void SetMenuAlmenuItems(ArrayList droparray)
        {
            bool[] eredetidropenab = (bool[])EredetiDropEnablek[0];
            string[] contnevek = (string[])AlmenuUserControlNevek[0];
            if (droparray != null)
            {
                for (int i = 0; i < droparray.Count; i++)
                {
                    ToolStripMenuItem drop = (ToolStripMenuItem)droparray[i];
                    if (contnevek != null)
                    {
                        int cind = LetezoUserControlNevek.IndexOf(contnevek[i]);
                        
                        eredetidropenab[i] = cind != -1;
                        if (eredetidropenab[i])
                        {
                            if (UserContHozferJogok != null)
                            {
                                if (UserContHozferJogok[cind] == Base.HozferJogosultsag.Semmi)
                                    eredetidropenab[i] = false;
                            }
                        }
                        drop.Enabled = eredetidropenab[i];
                    }
                }
            }
        }
    }
}
