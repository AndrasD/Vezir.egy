using System;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Forms;
using System.Globalization;
using System.Data;
using System.Threading;
using FakPlusz.Alapcontrolok;
using FormattedTextBox;
namespace FakPlusz.Alapfunkciok
{
    /// <summary>
    /// Input controlok mezo es allapotinformacioi tulajdonos UserControl-onkent
    /// </summary>
    public class MezoControlCollection : ArrayList
    {
        /// <summary>
        /// Az inputcontrolokat letrehozo UserControl
        /// </summary>
        private ArrayList usercontrols = new ArrayList();
        /// <summary>
        /// az inputcontrolok parentcontrolja
        /// </summary>
        private ArrayList parentcontrolnames = new ArrayList();
        /// <summary>
        /// A tablainformaciok
        /// </summary>
        private ArrayList egytabinfok = new ArrayList();
        /// <summary>
        /// objectum letrehozasa
        /// </summary>
        public MezoControlCollection()
        {
        }
        /// <summary>
        /// hivatkozas index szerint
        /// </summary>
        /// <param name="index">
        /// kivant index
        /// </param>
        /// <returns>
        /// Kivant mezocontolinformacio vagy null
        /// </returns>
        public new MezoControlInfo this[int index]
        {
            get
            {
                if (index == -1)
                    return null;
                else
                    return (MezoControlInfo)base[index];
            }
            set { base[index] = value; }
        }
        /// <summary>
        /// kereses a letrehozo UserControl szerint
        /// </summary>
        /// <param name="hivo">
        /// kivant UserControl
        /// </param>
        /// <returns>
        /// Kivant mezocontolinformacio vagy null
        /// </returns>
        public MezoControlInfo this[Base hivo]
        {
            get { return (MezoControlInfo)this[usercontrols.IndexOf(hivo)]; }
        }
        /// <summary>
        /// kereses tablainformacio szerint
        /// </summary>
        /// <param name="tabinfo">
        /// kivant tablainformacio
        /// </param>
        /// <returns>
        /// mezoinformacio vagy null
        /// </returns>
        public MezoControlInfo this[Tablainfo tabinfo]
        {
            get
            {
                int j;
                int i = egytabinfok.IndexOf(tabinfo);
                j=i;
                if (i == -1)
                    return null;
                else if (this[i].InputelemArray.Count == 0)
                {
                    if (i != egytabinfok.Count - 1)
                    {
                        j = i + 1;
                        if ((Tablainfo)egytabinfok[j] == tabinfo)
                            return (this[j]);
                    }
                }
                return this[i];
            }
        }
        /// <summary>
        /// Kereses parentcontrol neve szerint
        /// </summary>
        /// <param name="parentname"></param>
        /// <returns></returns>
        public MezoControlInfo this[string parentname]
        {
            get
            {
                int i = parentcontrolnames.IndexOf(parentname);
                if (i == -1)
                    return null;
                else
                    return this[i];
            }
        }
        /// <summary>
        /// Uj mezocontrolinformacio hozzaadas
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override int Add(object value)
        {
            int i = base.Add(value);
            usercontrols.Add(((MezoControlInfo)value).Hivo);
            egytabinfok.Add(((MezoControlInfo)value).Tabinfo);
            if (((MezoControlInfo)value).ParentControl != null)
                parentcontrolnames.Add(((MezoControlInfo)value).ParentControl.Name);
            else
                parentcontrolnames.Add("");
            return i;
        }
        /// <summary>
        /// Mezocontrolinformacio eltavolitas
        /// </summary>
        /// <param name="obj"></param>
        public override void Remove(object obj)
        {
            usercontrols.Remove(((MezoControlInfo)obj).Hivo);
            egytabinfok.Remove(((MezoControlInfo)obj).Tabinfo);
            parentcontrolnames.Remove(((MezoControlInfo)obj).ParentControl.Name);
            base.Remove(obj);
        }
        /// <summary>
        /// Van -e hibas tartalmu mezocontrolinformacio ?
        /// </summary>
        /// <param name="tabinfo">
        /// A tablainformacio, amelyiknek az inputjait vizsgaljuk
        /// </param>
        /// <returns>
        /// true: talalt hibasar
        /// </returns>
        public bool Vanehiba(Tablainfo tabinfo)
        {
            MezoControlInfo egycont = this[tabinfo];
            return (egycont.Vanehiba());
        }
    }
    /// <summary>
    /// Egy UserControl input controljainak mezocontrolinformacioi
    /// </summary>
    public class MezoControlInfo
    {
        /// <summary>
        /// A UserControl
        /// </summary>
        public Base Hivo;
        /// <summary>
        /// A tablainformacio objectum-a
        /// </summary>
        public Tablainfo Tabinfo;
        /// <summary>
        /// Az inputcontrolok informacioinak tombje
        /// </summary>
        public MezoTag[] Inputeleminfok
        {
            get { return (MezoTag[])InputelemArray.ToArray(typeof(MezoTag)); }
        }
        /// <summary>
        /// Hibaszovegek menuitem ToolTipText-jehez
        /// </summary>
        public string Hosszuszoveg = "";
        /// <summary>
        /// inputcontrolok informacioi
        /// </summary>
        public MezotagCollection InputelemArray = new MezotagCollection();
        /// <summary>
        /// ha van DataGridView a cotrolok kozt
        /// </summary>
        public DataGridView DataGridView = null;
        /// <summary>
        /// A UserControl informacioi
        /// </summary>
        public UserControlInfo UserControlInfo = null;
        /// <summary>
        /// Az inputcontrol parent controlja
        /// </summary>
        public Control ParentControl = null;
        /// <summary>
        /// Objectum letrehozasa
        /// </summary>
        /// <param name="hivo">
        /// A letrehozo UserControl
        /// </param>
        /// <param name="tabinfo">
        /// A tablainformacio
        /// </param>
        public MezoControlInfo(Base hivo,Tablainfo tabinfo)
        {
            Hivo = hivo;
            Tabinfo = tabinfo;
        }
        /// <summary>
        /// Changed jelzesek beallitasa
        /// </summary>
        public void SetChanged()
        {
            for (int i = 0; i < InputelemArray.Count; i++)
            {
                MezoTag egytag = InputelemArray[i];
                if (i == 0)
                {
                    string text = egytag.ParentControl.Text;
                    if (!text.Contains("*"))
                    {
                        text += "*";
                        egytag.ParentControl.Text = text;
                    }
                }
            }
            SetOrClearChanged();
        }
        /// <summary>
        /// Toroljuk a Change jelzeseket
        /// </summary>
        public void ClearChanged()
        {
            for (int i = 0; i < InputelemArray.Count; i++)
            {
                MezoTag egytag = InputelemArray[i];
                if (i == 0)
                {
                    string text = egytag.ParentControl.Text;
                    if (text.Contains("*"))
                    {
                        text = text.Replace("*", "");
                        egytag.ParentControl.Text = text;
                    }
                }
            }
            SetOrClearChanged();
        }
    
        /// <summary>
        /// Changed jelzesek  beallitasa vagy torlese tablainformacio alapjan
        /// </summary>
        public void SetOrClearChanged()
        {
            TabPage page;
            ToolStripMenuItem dropitem;
            ToolStripMenuItem menuitem;
            UserControlInfo uscont;
            UserControlInfo aktusercont = this.Tabinfo.AktualControlInfo.UserControlInfo;
            if (this.UserControlInfo != null)
                uscont = this.UserControlInfo;
            else if (this.Tabinfo.UserControlok.Count == 0)
                return;
            else
                uscont = this.Tabinfo.UserControlok[0];
            page = uscont.AktivPage;
            dropitem = uscont.AktivDropDownItem;
            menuitem = uscont.AktivMenuItem;
            if (Tabinfo.Modositott || Tabinfo.Changed)
            {
                if (Tabinfo.KellBarmilyenValtozas)
                {
                    for (int i = 0; i < Tabinfo.UserControlok.Count; i++)
                    {
                        UserControlInfo egycont = Tabinfo.UserControlok[i];
                        if (egycont != aktusercont && egycont.KellUserControlValtozas)
                        {
                            try
                            {
                                ((Base)egycont.User).Modositott = true;
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                if (!Tabinfo.ModositasiHiba)
                {
                    if (page != null)
                    {
                        if (!page.Text.Contains("*"))
                            page.Text += "*";
                    }
                    if (dropitem != null)
                    {
                        if (!dropitem.Text.Contains("*"))
                            dropitem.Text += "*";
                    }
                    if (menuitem != null)
                    {
                        if (!menuitem.Text.Contains("*") && !menuitem.Text.Contains("!"))
                            menuitem.Text += "*";
                    }
                }
            }
            else
            {
                bool modchanged = false;
                for (int i = 0; i < uscont.Tabinfok.Length; i++)
                {
                    Tablainfo egytabinfo = uscont.Tabinfok[i];
                    if (egytabinfo.Changed && egytabinfo.DataView.Count != 0 || egytabinfo.Modositott)
                    {
                        modchanged = true;
                        break;
                    }
                }
                if (!modchanged)
                {
                    if (page != null)
                        page.Text = page.Text.Replace("*", "");
                    if (dropitem != null)
                        dropitem.Text = dropitem.Text.Replace("*", "");
                    if (menuitem != null)
                        Tabinfo.Fak.UserControlok.ClearChanged(uscont.AktivMenuItem);
                }
            }
        }
        /// <summary>
        /// hibajelzesek beallitasa
        /// </summary>
        public void SetError()
        {
            SetOrClearError();
        }
        /// <summary>
        /// toroljuk a hibajelzeseket
        /// </summary>
        public void ClearError()
        {
            for (int i = 0; i < InputelemArray.Count; i++)
            {
                MezoTag egytag = InputelemArray[i];
                egytag.Hosszuhibaszov = "";
                egytag.Hibaszov = "";
                egytag.ModositasiHiba = false;
                egytag.Fak.ErrorProvider.SetError(egytag.Control, "");
            }
            SetOrClearError();
        }
        /// <summary>
        /// hibajelzesek beallitasa vagy torlese mezocontrolinformaciokban taroltak alapjan
        /// </summary>
        public void SetOrClearError()
        {
            if (this.UserControlInfo != null)
            {
                TabPage page = UserControlInfo.AktivPage;
                ToolStripMenuItem dropitem = UserControlInfo.AktivDropDownItem;
                ToolStripMenuItem menuitem = UserControlInfo.AktivMenuItem;
                Hosszuszoveg = "";
                for (int i = 0; i < InputelemArray.Count; i++)
                {
                    MezoTag egytag = InputelemArray[i];
                    if (egytag.Hosszuhibaszov != "")
                    {
                        if (Hosszuszoveg != "")
                            Hosszuszoveg += "\n";
                        Hosszuszoveg += egytag.Hosszuhibaszov;
                    }
                }
                if (Hosszuszoveg == "")
                {
                    if (page != null)
                        page.Text = page.Text.Replace("!", "");
                    if (dropitem != null)
                        dropitem.Text = dropitem.Text.Replace("!", "");
                }
                else
                {
                    if(page!=null&&!page.Text.Contains("!"))
                        page.Text+="!";
                    if(dropitem!=null&&!dropitem.Text.Contains("!"))
                        dropitem.Text+="!";
                }
                if (this.UserControlInfo.AktivMenuItem != null)
                    Tabinfo.Fak.UserControlok.SetOrClearError(this.UserControlInfo.AktivMenuItem);
            }
        }
        /// <summary>
        /// Hibavizsgalat a mezocontrolinformaciokra 
        /// </summary>
        /// <returns>
        /// true: van hibas
        /// </returns>
        public bool Hibavizsg()
        {
            bool hiba = false;
            for (int i = 0; i < this.InputelemArray.Count; i++)
            {
                MezoTag egytag = this.InputelemArray[i];
                if (egytag.EgyHibavizsg(egytag.Control.Text))
                    hiba = true;
            }
            this.Tabinfo.ModositasiHiba = hiba;
            return hiba;
        }
        /// <summary>
        /// Van-e hibasnak jelzett mezocontrolinformacio
        /// </summary>
        /// <returns>
        /// true: van
        /// </returns>
        public bool Vanehiba()
        {
            for (int i = 0; i < this.InputelemArray.Count; i++)
            {
                if (this.InputelemArray[i].Hibaszov != "")
                    return true;
            }
            this.Tabinfo.ModositasiHiba = false;
            return false;
        }
    }
    /// <summary>
    /// inputcontrolok mezo es allapotinformacioi(MezoTag-ok gyujtemenye) az inputcontrolok TabIndex-enek sorrendjeben
    /// </summary>
    public class MezotagCollection : ArrayList
    {
        /// <summary>
        /// inputControlok TabIndex sorrendben
        /// </summary>
        private ArrayList egytagcontarray = new ArrayList();
        /// <summary>
        /// oszlopnevek TabIndex sorrendben
        /// </summary>
        private ArrayList mezonevek = new ArrayList();
        /// <summary>
        /// TabIndex-ek sorrendben
        /// </summary>
        private ArrayList tabindexek = new ArrayList();
        /// <summary>
        /// objectum letrehozasa
        /// </summary>
        public MezotagCollection()
        {
        }
        /// <summary>
        /// hivatkozas index szerint
        /// </summary>
        /// <param name="index">
        /// kivant index
        /// </param>
        /// <returns>
        /// mezocontrol informacio vagy null
        /// </returns>
        public new MezoTag this[int index]
        {
            get { return (MezoTag)base[index]; }
            set { base[index] = value; }
        }
        /// <summary>
        /// hivatkozas input control szerint
        /// </summary>
        /// <param name="cont">
        /// kivant inputcontrol
        /// </param>
        /// <returns>
        /// mezocontrol informacio vagy null
        /// </returns>
        public MezoTag this[Control cont]
        {
            get { return (MezoTag)this[egytagcontarray.IndexOf(cont)]; }
        }
        /// <summary>
        /// hivatkozas mezonev (a tabla oszlopneve) szerint
        /// </summary>
        /// <param name="mezonev">
        /// az oszlopnev
        /// </param>
        /// <returns>
        /// mezocontrol informacio vagy null 
        /// </returns>
        public MezoTag this[string mezonev]
        {
            get
            {
                int i = mezonevek.IndexOf(mezonev);
                if (i == -1)
                    return null;
                return this[i];
            }
        }
        /// <summary>
        /// Uj mezocontrol informacio(MezoTag) hozzaadasa
        /// </summary>
        /// <param name="tag">
        /// az uj informacio
        /// </param>
        /// <returns>
        /// az uj info indexe a gyujtemenyben
        /// </returns>
        public int Add(MezoTag tag)
        {
            bool megvan = false;
            int tabindex = tag.Control.TabIndex;
            int i = 0;
            for (i = 0; i < tabindexek.Count; i++)
            {
                int egyindex = Convert.ToInt16(tabindexek[i].ToString());
                if (egyindex > tabindex)
                {
                    megvan = true;
                    this.Insert(i, tag);
                    tabindexek.Insert(i, tabindex);
                    egytagcontarray.Insert(i, tag.Control);
                    mezonevek.Insert(i, tag.Mezonev);
                    break;
                }
            }
            if (!megvan)
            {
                tabindexek.Add(tabindex);
                egytagcontarray.Add(tag.Control);
                mezonevek.Add(tag.Mezonev);
                i = base.Add(tag);
            }
            return i;
        }

        /// <summary>
        /// osszes info torlese
        /// </summary>
        public override void Clear()
        {
            base.Clear();
            egytagcontarray.Clear();
        }
        /// <summary>
        /// hibas mezoinformacio kereses
        /// </summary>
        /// <returns>
        /// true: hibas
        /// </returns>
        public bool Vanehiba()
        {
            for (int i = 0; i < egytagcontarray.Count; i++)
            {
                MezoTag egytag = this[i];
                if (egytag.Hibaszov != "")
                    return true;
            }
            this[0].Tabinfo.ModositasiHiba = false;
            return false;
        }
    }
    /// <summary>
    /// mezocontrolinformacio : egy inputcontrolhoz rendelt oszlopinformacio es allapotinformaciok
    /// </summary>
    public class MezoTag
    {
        /// <summary>
        /// fakuserinterface
        /// </summary>
        public FakUserInterface Fak;
        /// <summary>
        /// a tablainformacio szintje
        /// </summary>
        public string Szint;
        /// <summary>
        /// a tabla neve
        /// </summary>
        public string Tablanev;
        /// <summary>
        /// az oszlop neve
        /// </summary>
        public string Mezonev;
        /// <summary>
        /// max. inputhossz
        /// </summary>
        public int MaxLength;
        /// <summary>
        /// a mezo Sorszovege
        /// </summary>
        public string Szoveg = "";
        /// <summary>
        /// Tartalom valtozott ?
        /// </summary>
        public bool Changed = false;
        /// <summary>
        /// Ures az input?
        /// </summary>
        public bool Ures = true;
        /// <summary>
        /// a tablainformacio
        /// </summary>
        public Tablainfo Tabinfo;
        /// <summary>
        /// A hivo UserControl
        /// </summary>
        public Base Hivo;
        /// <summary>
        /// A UserControl ebben a Page-ben fut
        /// </summary>
        public TabPage AktivPage;
        /// <summary>
        /// Teljes UserControl info
        /// </summary>
        public UserControlInfo UserControlInfo = null;
        /// <summary>
        /// Teljes MezoControl info
        /// </summary>
        public MezoControlInfo EgyControlInfo = null;
        /// <summary>
        /// Az aktiv menuitem
        /// </summary>
        public ToolStripMenuItem AktivMenuItem = null;
        /// <summary>
        /// Az aktiv drop item
        /// </summary>
        public ToolStripMenuItem AktivDropDownItem = null;
        /// <summary>
        /// Az oszlopinformacio
        /// </summary>
        public Cols Egycol
        {
            get
            {
                if (Mezonev == "")
                    return null;
                else
                    return Tabinfo.TablaColumns[Mezonev]; 
            }
        }
        /// <summary>
        /// Ha az inputcontrol lathato == Egycol, egyebkent null
        /// </summary>
        public Cols Egyinp
        {
            get
            {
                if (!Egycol.Inputlathato)
                    return null;
                else
                    return Egycol;
            }
        }
        /// <summary>
        /// Az input Control
        /// </summary>
        public Control Control;
        /// <summary>
        /// az input control Parent-je
        /// </summary>
        public Control ParentControl;
        /// <summary>
        /// DataGridView, ha ez az inputcontrol
        /// </summary>
        public DataGridView DataGridView;
        /// <summary>
        /// A tablainformacio DataView
        /// </summary>
        public DataView DataView;
        /// <summary>
        /// az aktiv cella
        /// </summary>
        public DataGridViewCell AktivCell;
        /// <summary>
        /// a selectalt cella
        /// </summary>
        public DataGridViewCell SelectedCell;
        /// <summary>
        /// szulo grid
        /// </summary>
        public DataGridView ParentGrid;
        /// <summary>
        /// szulo view
        /// </summary>
        public DataView ParentView;
        /// <summary>
        /// a textbox oszlop, ha az
        /// </summary>
        public DataGridViewTextBoxColumn TextBoxColumn = null;
        /// <summary>
        /// a checkbox oszlop, ha az
        /// </summary>
        public DataGridViewCheckBoxColumn CheckBoxColumn = null;
        /// <summary>
        /// a combobox oszlop, ha az
        /// </summary>
        public DataGridViewComboBoxColumn ComboBoxColumn = null;
        /// <summary>
        /// a textboxcella, ha az
        /// </summary>
        public DataGridViewTextBoxCell TextBoxCell = null;
        /// <summary>
        /// a checkboxcella, ha az
        /// </summary>
        public DataGridViewCheckBoxCell CheckBoxCell = null;
        /// <summary>
        /// a comboboxcella, ha az
        /// </summary>
        public DataGridViewComboBoxCell ComboBoxCell = null;

        /// <summary>
        /// sorindex
        /// </summary>
        public int RowIndex;
        /// <summary>
        /// oszlopindex
        /// </summary>
        public int ColumnIndex;

        /// <summary>
        /// A UserControl osszes MezoTag-ja
        /// </summary>
        public MezoTag[] OsszesTag = null;
        /// <summary>
        /// Inputban hiba van
        /// </summary>
        public bool ModositasiHiba = false;
        /// <summary>
        /// Az input control tipusa
        /// </summary>
        public string Controltipus = "";
        /// <summary>
        /// rovid hibaszoveg (az ErrorPovider-rel ez jelenik meg)
        /// </summary>
        public string Hibaszov = "";
        /// <summary>
        /// hosszu hibaszoveg (Az aktiv menuitem ToolTip-jeben ez jelenik meg)
        /// </summary>
        public string Hosszuhibaszov = "";
        /// <summary>
        /// ComboBox Item-ek szuresehez a user altal megadott tarolhato tartalmak
        /// </summary>
        public string[] Szuroinfo = null;
        /// <summary>
        /// A szuroinfoval szurt tarolhato tartalmak
        /// </summary>
        public string[] SzurtCombofileinfo = null;
        /// <summary>
        /// a szurt ComboBox itemek
        /// </summary>
        public string[] SzurtComboinfo = null;
        /// <summary>
        /// Elmentett Combo item
        /// </summary>
        public string SaveComboaktszoveg = "";
        /// <summary>
        /// elmentett tarolando tartalom
        /// </summary>
        public string SaveComboaktfileba = "";
        /// <summary>
        /// az input controlok kozul ebbe leptunk-e utoljara
        /// </summary>
        public bool Elementer = false;
        /// <summary>
        /// ez-e a parent control elso inputcontrolja
        /// </summary>
        public bool First = false;
        /// <summary>
        /// ez-e a parent control utolso inputcontrolja
        /// </summary>
        public bool Last = false;
        /// <summary>
        /// az input controlok kozul ez-e az utoljara elhagyott
        /// </summary>
        public bool Lastleave = false;
        /// <summary>
        /// Inputcontrol tipusa alapjan beallitott index
        /// </summary>
        public int Elemindex = -1;
        /// <summary>
        /// Fejlesztes alatt
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="egytag"></param>
        public MezoTag(DataGridViewCell cell, MezoTag egytag)
        {
            Tabinfo = egytag.Tabinfo;
            Hivo = egytag.Hivo;
            AktivPage = egytag.AktivPage;
            AktivMenuItem = egytag.AktivMenuItem;
            AktivDropDownItem = egytag.AktivDropDownItem;
            Fak = egytag.Fak;
            DataGridView = egytag.DataGridView;
            Mezonev = egytag.Mezonev;
            MaxLength = egytag.MaxLength;
            ParentGrid = egytag.ParentGrid;
            ParentView = egytag.ParentView;
            Control = egytag.Control;
            ParentControl = Control.Parent;
            Controltipus = egytag.Controltipus;
            TextBoxColumn = egytag.TextBoxColumn;
            CheckBoxColumn = egytag.CheckBoxColumn;
            ComboBoxColumn = egytag.ComboBoxColumn;
            RowIndex = cell.RowIndex;
            ColumnIndex = cell.ColumnIndex;
            switch (Controltipus)
            {
                case "TextBoxCell":
                    TextBoxCell = (DataGridViewTextBoxCell)cell;
                    break;
                case "CheckBoxCell":
                    CheckBoxCell = (DataGridViewCheckBoxCell)cell;
                    break;
                case "ComboBoxCell":
                    ComboBoxCell = (DataGridViewComboBoxCell)cell;
                    break;
            }
        }
        /// <summary>
        /// Uj mezocontrolinformacio letrehozasa, ha az nem DataGridView
        /// </summary>
        /// <param name="tabinfo">
        /// Tablainformacio
        /// </param>
        /// <param name="mezonev">
        /// mezo neve
        /// </param>
        /// <param name="fak">
        /// Fak
        /// </param>
        /// <param name="egycontinfo">
        /// azon mezocontrolinformacios gyujtemeny, melynek ez is tagja lesz
        /// </param>
        /// <param name="aktivpage">
        /// a TabPage amelyben a UserControl fut vagy null
        /// </param>
        /// <param name="aktivmenuitem">
        /// a menuitem, melyhez tartozik
        /// </param>
        /// <param name="aktivdropdownitem">
        /// a dropitem, mellyel kivalasztjuk a UserControlt
        /// </param>
        public MezoTag(Tablainfo tabinfo, string mezonev, FakUserInterface fak,MezoControlInfo egycontinfo, TabPage aktivpage, ToolStripMenuItem aktivmenuitem, ToolStripMenuItem aktivdropdownitem)
        {
            MezoControlTagGyart(tabinfo, mezonev, fak,egycontinfo, aktivpage, aktivmenuitem, aktivdropdownitem, null);
        }
        /// <summary>
        /// Objectum eloallitas, ha az inputcontrol DataGridView
        /// </summary>
        /// <param name="tabinfo">
        /// Tablainformacio
        /// </param>
        /// <param name="egycontinfo">
        /// null
        /// </param>
        /// <param name="mezonev">
        /// ""
        /// </param>
        /// <param name="fak">
        /// Fak objectum
        /// </param>
        /// <param name="aktivpage">
        /// TabPage
        /// </param>
        /// <param name="aktivmenuitem">
        /// MenuItem
        /// </param>
        /// <param name="aktivdropdownitem">
        /// Drop Item
        /// </param>
        /// <param name="gw">
        /// A DataGridView
        /// </param>
        public MezoTag(Tablainfo tabinfo, string mezonev, FakUserInterface fak,MezoControlInfo egycontinfo, TabPage aktivpage, ToolStripMenuItem aktivmenuitem, ToolStripMenuItem aktivdropdownitem, DataGridView gw)
        {
            MezoControlTagGyart(tabinfo, mezonev, fak,egycontinfo, aktivpage, aktivmenuitem, aktivdropdownitem, gw);
        }
        /// <summary>
        /// Inicializalas a hivasi parameterek alapjan
        /// </summary>
        /// <param name="tabinfo">
        /// Tablainformacio
        /// </param>
        /// <param name="egycontinfo">
        /// Mezoinformacio
        /// </param>
        /// <param name="mezonev">
        /// Mezo neve
        /// </param>
        /// <param name="fak">
        /// Fak objectum
        /// </param>
        /// <param name="aktivpage">
        /// TabPage
        /// </param>
        /// <param name="aktivmenuitem">
        /// MenuItem
        /// </param>
        /// <param name="aktivdropdownitem">
        /// Drop Item
        /// </param>
        /// <param name="gw">
        /// DataGridView vagy null
        /// </param>
        private void MezoControlTagGyart(Tablainfo tabinfo, string mezonev, FakUserInterface fak, MezoControlInfo egycontinfo, TabPage aktivpage, ToolStripMenuItem aktivmenuitem, ToolStripMenuItem aktivdropdownitem, DataGridView gw)
        {
            Fak = fak;
            Tabinfo = tabinfo;
            EgyControlInfo = egycontinfo;
            AktivPage = aktivpage;
            AktivMenuItem = aktivmenuitem;
            AktivDropDownItem = aktivdropdownitem;
            Mezonev = mezonev;
            DataGridView = gw;
            Cols egycol = Egycol;
            if (egycol != null)
            {
                Szoveg = egycol.Sorszov;
                egycol.EgyTag = this;
                MaxLength = egycol.InputMaxLength;
            }
            DataView = Tabinfo.DataView;
            if (DataGridView != null)
            {
                if (DataGridView.ReadOnly)
                    DataGridView.AutoGenerateColumns = false;
                else
                    DataGridView.AutoGenerateColumns = true;
                DataGridView.Columns.Clear();
                DataGridView.DataSource = DataView;
                DataGridView.AllowUserToOrderColumns = false;
                DataGridView.MultiSelect = false;
                DataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                if (DataGridView.ReadOnly)
                {
                    DataGridView.AllowUserToAddRows = false;
                    DataGridView.AllowUserToDeleteRows = false;
                    DataGridView.RowHeadersVisible = false;
                }
                else
                {
                    DataGridView.AllowUserToAddRows = true;
                    DataGridView.AllowUserToDeleteRows = true;
                    DataGridView.RowHeadersVisible = true;
                }
            }
        }
        /// <summary>
        /// inputcontrolba programbol ertek bevitel, hibavizsgalat utan
        /// </summary>
        /// <param name="ertek">
        /// kivant ertek
        /// </param>
        /// <returns>
        /// true, ha nem volt hibas
        /// </returns>
        public bool SetValue(string ertek)
        {
            return EgyHibavizsg(ertek);
        }
        /// <summary>
        /// Rovid es ennek alapjan hosszu hibaszoveg beallitasa es megjelenitese, hiba jelzese az osszes szukseges
        /// helyen (pl.page,dropitem,menuitem)
        /// </summary>
        /// <param name="hibaszov">
        /// a kivant szoveg
        /// </param>
        public void SetHibaSzov(string hibaszov)
        {
            Hibaszov = hibaszov;
            if (hibaszov == "")
            {
                Hosszuhibaszov = "";
                Fak.ErrorProvider.SetError(Control, "");
            }
            else
            {
                Hosszuhibaszov = ParentControl.Text.Replace("*", "") + " " + Egycol.Sorszov + ":" + hibaszov;
                Fak.ErrorProvider.SetError(Control, hibaszov);
            }
            EgyControlInfo.SetOrClearError();
        }
        /// <summary>
        /// Ures-e minden inputcontrol
        /// </summary>
        /// <returns></returns>
        public bool Uresvizsg()
        {
            bool ures = true;
            for (int i = 0; i < OsszesTag.Length; i++)
            {
                if (!Uresvizsg(OsszesTag[i]))
                    ures = false;
            }
            return ures;
        }
        /// <summary>
        /// Ures-e az adott inputcontrol
        /// </summary>
        /// <param name="egytag">
        /// A kivant inputcontrol mezocontrolinformacio
        /// </param>
        /// <returns>
        /// true, ha ures
        /// </returns>
        public bool Uresvizsg(MezoTag egytag)
        {
            Ures = true;
            Cols egycol = Egycol;
            switch (Elemindex)
            {
                case 0:
                    TextBox tb = (TextBox)Control;
                    if (tb.Text == "")
                    {
                    }
                    else if (!egycol.Numeric(egycol.DataType) || egycol.Numeric(egycol.DataType) && tb.Text.Trim() != "0")
                        Ures = false;
                    break;
                case 1:
                    FormattedTextBox.FormattedTextBox ftb = (FormattedTextBox.FormattedTextBox)Control;
                    if (ftb.Text == "")
                    {
                    }
                    if (!egycol.Numeric(egycol.DataType) || egycol.Numeric(egycol.DataType) && ftb.Text.Trim() != "0")
                        Ures = false;
                    break;
                case 2:
                    ComboBox combo = (ComboBox)Control;
                    if (combo.Text != "" && (!Tabinfo.LehetUres || combo.Text.Trim() != egycol.Combo_Info.Szovegbe[0].ToString()))
                        Ures = false;
                    break;

                case 3:
                    CheckBox cb = (CheckBox)Control;
                    if (cb.Checked)
                        Ures = false;
                    break;
                case 4:
                    Ures = false;
                    break;
                case 5:
                    break;
                default:
                    if (ParentView != null && ParentView.Count != 0)
                        Ures = false;
                    break;
            }
            if (!Ures)
                Tabinfo.Ures = false;
            else
                for (int i = 0; i < OsszesTag.Length; i++)
                {
                    if (!OsszesTag[i].Ures)
                    {
                        Tabinfo.Ures = false;
                        break;
                    }
                }
            return Tabinfo.Ures;
            //                               }
        }
        /// <summary>
        /// hibas lenne-e az inputcontrol tartalma adott ertek bevitelekor
        /// </summary>
        /// <param name="ertek">
        /// a kivant ertek
        /// </param>
        /// <returns>
        /// true, ha hibas
        /// </returns>
        public bool EgyHibavizsg(string ertek)
        {
            string tartal = ertek;
            bool changed = tartal != Control.Text;
            Hosszuhibaszov = "";
            Hibaszov = "";
            Elementer = false;
            bool hiba = false;
            if (!Fak.Ujceg)
            {
                Uresvizsg(this);
                Cols egycol = Egycol;
                Cols egyinp = Egyinp;
                switch (Elemindex)
                {
                    case 0:
                        TextBox text = (TextBox)Control;
                        if (!changed)
                        {
                            if (tartal == "")
                                tartal = text.Text.Trim();
                            else
                                text.Text = tartal;
                        }
                        if (egyinp == null)
                        {
                            if (egycol.OrigTartalom != tartal)
                                Changed = true;
                            egycol.Tartalom = tartal;
                        }
                        else if (Control.Enabled)
                            hiba = Textvizsg(text, tartal);
                        break;
                    case 1:
                        FormattedTextBox.FormattedTextBox formtext = (FormattedTextBox.FormattedTextBox)Control;
                        formtext.RemoveFormatCharacters();
                        tartal = formtext.Text;
                        if (tartal == "")
                        {
                            formtext.InsertFormatCharacters();
                            tartal = formtext.Text;
                        }
                        //if(!changed)
                        //    tartal = formtext.Text.Trim();
                        if (egyinp == null)
                        {
                            if (egycol.OrigTartalom != tartal)
                                Changed = true;
                            egycol.Tartalom = tartal;
                        }
                        else if (Control.Enabled)
                        {
                            formtext.RemoveFormatCharacters();
                            hiba = Textvizsg((TextBox)formtext, tartal);
                        }
                        formtext.InsertFormatCharacters();
                        tartal = formtext.Text;
                        break;
                    case 2:
                        ComboBox combo = (ComboBox)Control;
                        combo.SelectionLength = 0;
                        if (!changed)
                        {
                            if (tartal == "" && (this.Tabinfo.ViewSorindex != -1 || !this.Tabinfo.LehetUres))
                            {
                                tartal = combo.Text;
                            }
                            else
                                combo.Text = tartal;
                        }
                        if (egyinp != null && !egycol.ReadOnly)
                        {
                            if (tartal != egyinp.ComboAktSzoveg)
                                Changed = true;
                            if ((!combo.Visible || !combo.Enabled) && egyinp.Lehetures)
                            {
                                egyinp.Tartalom = "";
                                egycol.Tartalom = "";
                                egyinp.ComboAktSzoveg = "";
                                egyinp.ComboAktFileba = "";
                            }
                            else if (!egycol.ReadOnly && combo.Enabled)
                                hiba = Combovizsg(combo, tartal);
                        }
                        break;
                    case 3:
                        CheckBox checkbox = (CheckBox)Control;
                        if (tartal != "")
                        {
                            if (tartal == egycol.Checkyes || tartal == egycol.Checkno)
                            {
                                if (tartal == egycol.Checkyes && !checkbox.Checked)
                                {
                                    checkbox.CheckState = CheckState.Checked;
                                    Changed = true;
                                }
                                if (tartal == egycol.Checkno  && checkbox.Checked)
                                {
                                    checkbox.CheckState = CheckState.Unchecked;
                                    Changed = true;
                                }
                                egycol.Tartalom = tartal;
                            }
                            else
                            {
                                Hibaszov = Fak.GetUzenetSzoveg("Ertekhiba");
                                Hosszuhibaszov = ParentControl.Text + " " + egycol.Sorszov + ":" + Hibaszov;
                                hiba = true;
                            }
                        }
                        else
                        {
                            if (checkbox.Checked)
                                tartal = egycol.Checkyes;
                            else
                                tartal = egycol.Checkno;
                            if (tartal != egycol.OrigTartalom)
                            {
                                Changed = true;
                                egycol.Tartalom = tartal;
                            }
                            else
                            {
                                Changed = false;
                                egycol.Tartalom = egycol.OrigTartalom;
                            }
                        }
                        break;
                    case 4:
                        RadioButton radiobutton = (RadioButton)Control;
                        if (tartal == "")
                        {
                            if (radiobutton.Checked)
                            {
                                if (egycol.Tartalom != radiobutton.Text)
                                    Changed = true;
                                egycol.Tartalom = radiobutton.Text;
                                if (egyinp != null)
                                    egyinp.Tartalom = egycol.Tartalom;
                            }
                        }
                        else
                        {
                            if (tartal == egycol.DefaultValue.ToString())
                            {
                                if (!radiobutton.Checked)
                                {
                                    radiobutton.Checked = true;
                                    Changed = true;
                                }
                                else if (radiobutton.Checked)
                                {
                                    radiobutton.Checked = false;
                                    Changed = true;
                                }
                            }
                            else if (radiobutton.Checked)
                            {
                                radiobutton.Checked = false;
                                Changed = true;
                            }
                            egycol.Tartalom = tartal;
                            if (egyinp != null)
                                egyinp.Tartalom = egycol.Tartalom;
                        }
                        break;
                    case 5:
                        DateTimePicker picker = (DateTimePicker)Control;
                        if (tartal == "")
                            tartal = picker.Text.Trim();
                        if (egyinp != null)
                        {
                            if (!egyinp.Lehetures && tartal == "")
                            {
                                Hibaszov = Fak.GetUzenetSzoveg("Nemures");
                                Hosszuhibaszov = ParentControl.Text + " " + egycol.Sorszov + ":" + Hibaszov;
                                hiba = true;
                            }
                            else
                            {
                                if (egycol.OrigTartalom!="" && egycol.OrigTartalom!= tartal)
                                {
                                    Changed = true;
                                    picker.Value = Convert.ToDateTime(tartal);
                                }
                                egycol.Tartalom = tartal;
                            }
                        }
                        break;
                }
                if (Changed || changed)
                    Tabinfo.Changed = true;
                if (hiba)
                {
                    ModositasiHiba = true;
                    Tabinfo.ModositasiHiba = true;
                }
                else
                    ModositasiHiba = false;
                if (Changed || ModositasiHiba)
                    Ures = false;
            }
            Fak.ErrorProvider.SetError(Control, Hibaszov);
            return hiba;
        }
        /// <summary>
        /// Textbox vagy FormattedTextBox tartalmanak hibavizsgalata, hibajelzesek torlese vagy beallitasa
        /// </summary>
        /// <param name="textbox">
        /// Vizsgalando textbox
        /// </param>
        /// <param name="tartal">
        /// vizsgalando tartalom
        /// </param>
        /// <returns>
        /// tru: hibas
        /// </returns>
        private bool Textvizsg(Control textbox, string tartal)
        {
            Cols egycol = Egycol;
            Cols egyinp = Egyinp;
            bool hiba = false;
            string formtartal = tartal;
            try
            {
                FormattedTextBox.FormattedTextBox fmt = (FormattedTextBox.FormattedTextBox)textbox;
                fmt.RemoveFormatCharacters();
                formtartal = fmt.Text;

            }
            catch { }
            if (!egyinp.Lehetures && (tartal == "" || egycol.Numeric(egyinp.DataType) && (tartal == "0" || formtartal=="")))
            {
                if (!Tabinfo.Ures || Tabinfo.Ures && !Tabinfo.LehetUres)
                    Hibaszov = Fak.GetUzenetSzoveg("Nemures");
            }
            else if (tartal == "" && egycol.OrigTartalom!="" && egycol.OrigTartalom!="0" && egycol.Numeric(egyinp.DataType) )
                tartal = "0";
            if (tartal != "")
            {
                try
                {
                    Convert.ChangeType(tartal, egyinp.DataType);
                }
                catch
                {
                    Hibaszov = Fak.GetUzenetSzoveg("Tipushiba");
                }
            }
            if (Hibaszov == "")
            {
                if (egycol.ColumnName == "DATUMTOL")
                {
                    if (textbox.Enabled)
                    {
                        if (tartal == "")
                        {
                            Hibaszov = Fak.GetUzenetSzoveg("Nemures");
                        }
                        else if (tartal.CompareTo(Tabinfo.MinDateTime) <= 0)
                        {
                            Hibaszov = Fak.GetUzenetSzoveg("Kisebbhiba") + " " + Convert.ToDateTime(Tabinfo.MinDateTime).AddDays(1).ToShortDateString() + "!";
                        }
                    }

                }
                else if (egycol.DataType == typeof(DateTime) && !egycol.Lehetures &&
                   tartal == Fak.Mindatum.ToShortDateString())
                {
                    Hibaszov = Fak.GetUzenetSzoveg("Kisdatum");
                }
            }
            if (Hibaszov == "")
            {
                if (egycol.IsUnique)
                {
                    for (int j = 0; j < DataView.Count; j++)
                    {
                        if (j != Tabinfo.ViewSorindex)
                        {
                            DataRow dr = DataView[j].Row;
                            int k = Tabinfo.TablaColumns.IndexOf(egycol);
                            if (dr[k].ToString().Trim() == tartal)
                            {
                                Hibaszov = Fak.GetUzenetSzoveg("Duplikalt");
                                break;
                            }
                        }
                    }

                }
            }
            if(Hibaszov=="")
            {
                if (egycol.OrigTartalom == "0" && tartal == "")
                {
                }
                else
                {
                    egycol.Tartalom = tartal;
                    if (egycol.Tartalom != egycol.OrigTartalom)
                        Changed = true;
                }
            }
            else
            {
                hiba = true;
                Hosszuhibaszov = ParentControl.Text + " " + egycol.Sorszov + ":" + Hibaszov;
            }
            return hiba;
        }
        /// <summary>
        /// ComboBox tartalmanak hibavizsgalata, hibajelzesek torlese vagy beallitasa
        /// </summary>
        /// <param name="combobox">
        /// vizsgalando ComboBox
        /// </param>
        /// <param name="tartal">
        /// vizsgalando tartalom
        /// </param>
        /// <returns>
        /// true: hibas
        /// </returns>
        private bool Combovizsg(Control combobox, string tartal)
        {
            Cols egycol = Egycol;
            Cols egyinp = Egyinp;
            bool hiba = false;
            bool megvan = false;
            if ((!Tabinfo.Ures || Tabinfo.Ures && !Tabinfo.LehetUres || Tabinfo.Changed) && !egyinp.Lehetures && tartal == "")
                Hibaszov = Fak.GetUzenetSzoveg("Nemures");
            else if (tartal == "")
            {
                egyinp.Tartalom = "";
                egyinp.ComboAktSzoveg = "";
                egyinp.ComboAktFileba = "";
                egycol.Tartalom = "";
                
                if (egycol.Combo_Info != null && !egyinp.Lehetures)
                {
                    egyinp.Tartalom = egycol.Combo_Info.DefFileba;
                    egyinp.ComboAktFileba = egycol.Combo_Info.DefFileba;
                    egyinp.ComboAktSzoveg = egycol.Combo_Info.DefSzovegbe;
                }
                if (egycol.Kiegcol != null)
                {
                    egycol.Kiegcol.Tartalom = egyinp.ComboAktSzoveg;
                    egycol.Kiegcol.ComboAktSzoveg = egyinp.ComboAktSzoveg;
                    egycol.Kiegcol.ComboAktFileba = egyinp.ComboAktFileba;
                }

            }
            if (Hibaszov == "")
            {
                string[] filinfo;
                string[] comboszov;
                if (SzurtCombofileinfo == null)
                {
                    Comboinfok comboinfo = egycol.Combo_Info;
                    comboszov = comboinfo.Szovegbe;
                    filinfo = comboinfo.Fileba;
                }
                else
                {
                    comboszov = SzurtComboinfo;
                    filinfo = SzurtCombofileinfo;
                }
                if (comboszov != null && tartal != "")
                {
                    for (int j = 0; j < comboszov.Length; j++)
                    {
                        if (tartal == comboszov[j])
                        {
                            egyinp.Tartalom = filinfo[j];
                            egyinp.ComboAktSzoveg = tartal;
                            egyinp.ComboAktFileba = filinfo[j];
                            megvan = true;
                            break;
                        }
                    }
                    if (!megvan)
                        Hibaszov = Fak.GetUzenetSzoveg("Ertekhiba");
                    else
                    {
                        egycol.Tartalom = egyinp.Tartalom;
                        if (egycol.Kiegcol != null)
                        {
                            egycol.Kiegcol.Tartalom = egyinp.ComboAktSzoveg;
                            egycol.Kiegcol.ComboAktSzoveg = egyinp.ComboAktSzoveg;
                            egycol.Kiegcol.ComboAktFileba = egyinp.ComboAktFileba;
                        }
                    }
                }
            }
            if (Hibaszov != "")
            {
                Hosszuhibaszov = ParentControl.Text.Replace("*","") + " " + egycol.Sorszov + ":" + Hibaszov;
                Changed = true;
                hiba = true;
            }
            return hiba;

        }
        /// <summary>
        /// Az osszes inputcontrol tartalmainak betoltese oszlopinformaciok alapjan
        /// </summary>
        /// <param name="change">
        /// jelezzunk-e valtozast 
        /// </param>
        public void FillControls(bool change)
        {
            bool saveevent = Fak.EventTilt;
            Fak.EventTilt = true;
            MezoTag egytag;
            Cols egycol;
            Cols egyinp;
            bool modositasihiba = false;
            for (int i = 0; i < OsszesTag.Length; i++)
            {
                egytag = OsszesTag[i];
                egytag.ModositasiHiba = false;
                egytag.Hibaszov = "";
                egytag.Hosszuhibaszov = "";
                egytag.Changed = change;
                if (!change)
                    Fak.ErrorProvider.SetError(egytag.Control, "");
                egycol = egytag.Egycol;
                egyinp = egytag.Egyinp;
                switch (egytag.Elemindex)
                {
                    case 0:
                        TextBox text = (TextBox)egytag.Control;
                        if (egycol.ColumnName == "DATUMTOL")
                        {
                            if (DataView.Count != 0 && Tabinfo.ViewSorindex != 0)
                                text.Enabled = true;
                            else
                                text.Enabled = false;
                        }
                        else if (text.Parent == null || text.Parent.Enabled)
                            text.Enabled = true;
                        if (egyinp != null)
                            text.Text = egyinp.Tartalom;
                        break;
                    case 1:
                        FormattedTextBox.FormattedTextBox formtext = (FormattedTextBox.FormattedTextBox)egytag.Control;
                        if (egycol.ColumnName == "DATUMTOL")
                        {
                            if (DataView.Count != 0 && Tabinfo.ViewSorindex != 0)
                                formtext.Enabled = true;
                            else
                                formtext.Enabled = false;
                        }
                        else if (formtext.Parent == null || formtext.Parent.Enabled)
                            formtext.Enabled = true;
                        if (egyinp != null)
                        {
                            formtext.Text = egyinp.Tartalom;
                            if (formtext.Format != "")
                                formtext.InsertFormatCharacters();
                        }
                        break;
                    case 2:
                        ComboBox combo = (ComboBox)egytag.Control;
                        string[] aktcomboinfo = null;
                        string[] aktcombofileinfo = null;
                        bool megvan=true;
                        if (egyinp != null)
                        {
                            combo.Text = egyinp.ComboAktSzoveg;
                            if (egytag.Tabinfo.AktIdentity != -1 && egytag.SzurtCombofileinfo != null && !egytag.Tabinfo.LehetUres && !egyinp.Lehetures && combo.Enabled)
                            {
                                megvan = false;
                                for (int j = 0; j < egytag.SzurtCombofileinfo.Length; j++)
                                {
                                    string id = egytag.SzurtCombofileinfo[j].ToString();
                                    if (id == egycol.OrigTartalom)
                                    {
                                        megvan = true;
                                        break;
                                    }
                                }
                                //                           }
                                if (!megvan)
                                {
                                    //if (!egyinp.Lehetures)
                                    //{
                                    //    if (combo.Enabled)
                                    //    {
                                    if (!saveevent)
                                    {
                                        egytag.Changed = true;
                                        egytag.SetHibaSzov(Fak.GetUzenetSzoveg("Ertekhiba"));
                                        egytag.ModositasiHiba = true;
                                        modositasihiba = true;
                                        change = true;
                                        //                                  }
                                        aktcombofileinfo = egytag.SzurtCombofileinfo;
                                        aktcomboinfo = egytag.SzurtComboinfo;
                                        egytag.SaveComboaktfileba = aktcombofileinfo[0];
                                        egytag.SaveComboaktszoveg = aktcomboinfo[0];
                                    }
                                    //                               }
                                }
                            }
                            else
                            {
                                if (egyinp.ComboAktSzoveg != "" || egytag.Tabinfo.ViewSorindex == -1)
                                {
                                    int k = -1;
                                    if (egyinp.ComboAktSzoveg != "")
                                        k = egycol.Combo_Info.ComboInfo.IndexOf(egyinp.ComboAktSzoveg);
                                    if (k != -1)
                                    {
                                        combo.Enabled = true;
                                    }
                                    else if (egytag.Tabinfo.ViewSorindex != -1)
                                        combo.Enabled = false;
                                    else if (!egycol.Lehetures)
                                        if (combo.Items.Count != 0)
                                            combo.Text = combo.Items[0].ToString();
                                }
                                else if (!egycol.Lehetures)
                                {
                                    if (egyinp.ComboAktSzoveg == "" && egycol.Tartalom != "" && egycol.Tartalom != "0")
                                    {
                                        if (egytag.Tabinfo.ViewSorindex != -1)
                                            combo.Enabled = false;
                                    }
                                    else if (combo.Items.Count != 0)
                                        combo.Text = combo.Items[0].ToString();
                                }
                                if (combo.Text != "" && egytag.SzurtCombofileinfo != null)
                                {
                                    megvan = false;
                                    for (int j = 0; j < egytag.SzurtComboinfo.Length; j++)
                                    {
                                        if (egytag.SzurtComboinfo[j] == combo.Text)
                                        {
                                            megvan = true;
                                            break;
                                        }
                                    }
                                    if (!megvan)
                                        combo.Text = "";
                                }
                                if (combo.Text == "" && !egycol.Lehetures)
                                {
                                    if (egytag.SzurtCombofileinfo == null)
                                    {
                                        aktcombofileinfo = egycol.Combo_Info.ComboFileinfoAll();
                                        aktcomboinfo = egycol.Combo_Info.ComboSzovinfoAll();
                                    }
                                    else
                                    {
                                        aktcombofileinfo = egytag.SzurtCombofileinfo;
                                        aktcomboinfo = egytag.SzurtComboinfo;
                                    }
                                    if (aktcombofileinfo.Length > 0)
                                    {
                                        egytag.SaveComboaktfileba = aktcombofileinfo[0];
                                        egytag.SaveComboaktszoveg = aktcomboinfo[0];
                                        egyinp.ComboAktFileba = egytag.SaveComboaktfileba;
                                        egyinp.ComboAktSzoveg = egytag.SaveComboaktszoveg;
                                    }
                                    combo.Text = egytag.SaveComboaktszoveg;
                                }
                            }
                            if(combo.Items.Count!=0)
                                combo.SelectedIndex = combo.Items.IndexOf(combo.Text);
                            combo.SelectionLength = 0;
                        }
                        break;
                    case 3:
                        CheckBox chb = (CheckBox)egytag.Control;
                        if (!egycol.Csakolvas)
                        {
                            if (egycol.Tartalom == egycol.Checkyes)
                                chb.CheckState = CheckState.Checked;
                            else
                                chb.CheckState = CheckState.Unchecked;
                        }
                        break;
                    case 4:
                        RadioButton rb = (RadioButton)egytag.Control;
                        if (egycol.Tartalom == egycol.DefaultValue.ToString() || egycol.Tartalom == rb.Text)
                            rb.Checked = true;
                        else
                            rb.Checked = false;
                        break;
                    case 5:
                        DateTimePicker pk = (DateTimePicker)egytag.Control;
                        if (egyinp != null)
                        {
                            if (egycol.ColumnName == "DATUMTOL")
                            {
                                if (DataView.Count != 0 && Tabinfo.ViewSorindex != 0)
                                    pk.Enabled = true;
                                else
                                    pk.Enabled = false;
                            }
                            else
                                pk.Enabled = true;
                            try
                            {
                                pk.Value = Convert.ToDateTime(egyinp.Tartalom);
                            }
                            catch
                            {
                            }
                        }
                        break;
                    case 6:
                        Label lb = (Label)egytag.Control;
                        lb.Text = egytag.Szoveg;
                        break;
                }

            }
            if (!saveevent)
            {
                Tabinfo.Changed = change;
                Tabinfo.ModositasiHiba = modositasihiba;
                Tabinfo.Modositott = false;
            }
            Fak.EventTilt = saveevent;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileinfo"></param>
        /// <returns></returns>
        public string[] GetUnusedFileinfo(string[] fileinfo)
        {
            ArrayList filar = new ArrayList(fileinfo);
            Comboinfok combinfo = Egyinp.Combo_Info;
            if (combinfo == null)
                return null;
            string[] filinf = (string[])combinfo.ComboFileinfo.ToArray(typeof(string));
            ArrayList ar = new ArrayList(filinf);
            for (int i = 0; i < filinf.Length; i++)
            {
                if (filar.IndexOf(filinf[i]) != -1)
                    ar.Remove(filinf[i]);
            }
            return (string[])ar.ToArray(typeof(string));
        }
        /// <summary>
        /// ComboBox Itemek szurese
        /// </summary>
        /// <param name="kellfileinfo">
        /// az elofordulhato rogzitendo ertekek tombje
        /// </param>
        /// <returns>
        /// true, ha sikeres a szures, azaz egyinp nem null
        /// </returns>
        public bool Comboinfoszures(string[] kellfileinfo)
        {
            return Comboinfoszures(kellfileinfo, false);
        }
        /// <summary>
        /// ComboBox Itemek szurese
        /// </summary>
        /// <param name="kellfileinfo">
        /// 
        /// </param>
        /// <param name="szovinfoe"></param>
        /// <returns></returns>
        public bool Comboinfoszures(string[] kellfileinfo,bool szovinfoe)
        {
            Cols egycol = Egycol;
            Cols egyinp = Egyinp;
            ComboBox combo = (ComboBox)Control;
            if (egyinp == null)
                return false;
            //if (Szuroinfo != null && kellfileinfo != null && Szuroinfo.Length == kellfileinfo.Length)
            //{
            //    bool kell = false;
            //    for (int i = 0; i < Szuroinfo.Length; i++)
            //    {
            //        if (Szuroinfo[i] != kellfileinfo[i])
            //        {
            //            kell = true;
            //            break;
            //        }
            //    }
            //    if (!kell)
            //        return true;
            //}
            Szuroinfo = kellfileinfo;
            if (egycol.ReadOnly)
                return false;
            if (kellfileinfo == null)
            {
                combo.Items.Clear();
                combo.Text = "";
                egycol.Tartalom = "";
                egyinp.Tartalom = "";
                egyinp.ComboAktFileba = egycol.Tartalom;
                egyinp.ComboAktSzoveg = "";
                return true;
            }
            string[] fileinfo;
            string[] szovinfo;
            if (egycol.Combo_Info != null)
            {
                fileinfo = (string[])egycol.Combo_Info.ComboFileinfo.ToArray(typeof(string));
                szovinfo = (string[])egycol.Combo_Info.ComboInfo.ToArray(typeof(string));
            }
            else
            {
                fileinfo = (string[])egycol.ComboAzontipCombo.ComboFileinfo.ToArray(typeof(string));
                szovinfo = (string[])egycol.ComboAzontipCombo.ComboInfo.ToArray(typeof(string));
            }
            ArrayList szurtcombofileinfo = new ArrayList();
            ArrayList szurtcomboszovinfo = new ArrayList();
            for (int i = 0; i < kellfileinfo.Length; i++)//Combofileinfo.Count; i++)
            {
                string s = kellfileinfo[i];
                if (!szovinfoe)
                {
                    for (int j = 0; j < fileinfo.Length; j++)
                    {
                        bool azonos;
                        if (fileinfo[j].Trim() == "")
                            azonos = true;
                        else
                            //    if (s.Length >= fileinfo[j].Length)
                            //    azonos = s == fileinfo[j];
                            //else
                            //    azonos = s == fileinfo[j].Substring(0, s.Length);
                            azonos = s.CompareTo(fileinfo[j]) == 0;
                        if (azonos && szurtcombofileinfo.IndexOf(s)==-1)
                        {
                            szurtcombofileinfo.Add(fileinfo[j]);
                            szurtcomboszovinfo.Add(szovinfo[j]);
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < szovinfo.Length; j++)
                    {
                        bool azonos;
                        if (szovinfo[j].Trim() == "")
                            azonos = true;
                        else if (s.Length >= szovinfo[j].Length)
                            azonos = s == szovinfo[j];
                        else
                            azonos = s == szovinfo[j].Substring(0, s.Length);
                        if (azonos && szurtcomboszovinfo.IndexOf(s)==-1)
                        {
                            szurtcombofileinfo.Add(fileinfo[j]);
                            szurtcomboszovinfo.Add(szovinfo[j]);
                        }
                    }
                }
            }
            SzurtCombofileinfo = (string[])szurtcombofileinfo.ToArray(typeof(string));
            SzurtComboinfo = (string[])szurtcomboszovinfo.ToArray(typeof(string));
            if (szurtcombofileinfo.Count == 0 && combo != null)
            {
                combo.Items.Clear();
                combo.Text = "";
                egycol.Tartalom = "0";
                egyinp.Tartalom = "";
                return true;
            }
            int selind = 0;
            bool megvan = false;
            if (!egycol.Lehetures && combo != null)
            {
                if (combo.Text != "")
                {
                    for (int i = 0; i < SzurtCombofileinfo.Length; i++)
                    {
                        if (egycol.Tartalom == SzurtCombofileinfo[i])
                        {
                            megvan = true;
                            selind = i;
                            break;
                        }
                    }
                }
                if (!megvan && egycol.Tablainfo.AktIdentity == -1)
                {
                    egycol.Tartalom = SzurtCombofileinfo[0];
                    egyinp.Tartalom = egycol.Tartalom;
                    egyinp.ComboAktFileba = egycol.Tartalom;
                    egyinp.ComboAktSzoveg = SzurtComboinfo[0];
                }
                SaveComboaktszoveg = egyinp.ComboAktSzoveg;
                SaveComboaktfileba = egyinp.ComboAktFileba;
            }
            if (combo != null)
            {
                combo.Items.Clear();
                for (int i = 0; i < SzurtComboinfo.Length; i++)
                    combo.Items.Add(SzurtComboinfo[i]);
                if (megvan)
                {
                    if (combo.Enabled && combo.Visible)
                        combo.Text = egyinp.ComboAktSzoveg;
                    else
                        combo.Text = "";
                    combo.SelectedIndex = selind;
                }
            }
            return true;
        }
    }
}

