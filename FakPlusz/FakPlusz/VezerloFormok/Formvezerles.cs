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
using FakPlusz.Formok;
namespace FakPlusz.VezerloFormok
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Formvezerles : VezerloAlapControl
    {
        private TabControl tabControl1 = new TabControl();
        /// <summary>
        /// 
        /// </summary>
        public TervTreeView terv;
        private string[] szovegek;
        /// <summary>
        /// 
        /// </summary>
        public Panel AktivPanel = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fak"></param>
        /// <param name="treepanel"></param>
        /// <param name="menupanel"></param>
        /// <param name="parent"></param>
        /// <param name="almenuindex"></param>
        /// <param name="kezelesiszint"></param>
        /// <param name="hozfer"></param>
        public Formvezerles(FakUserInterface fak, Panel treepanel, Panel menupanel, Vezerloinfo parent, int almenuindex, ref Base.KezSzint kezelesiszint, ref Base.HozferJogosultsag hozfer)
        {
            FormvezerlesInit(fak, treepanel, menupanel, parent, almenuindex, ref kezelesiszint, ref hozfer);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fak"></param>
        /// <param name="treepanel"></param>
        /// <param name="menupanel"></param>
        /// <param name="parent"></param>
        /// <param name="kezelesiszint"></param>
        /// <param name="hozfer"></param>
        public Formvezerles(FakUserInterface fak, Panel treepanel, Panel menupanel, Vezerloinfo parent, ref Base.KezSzint kezelesiszint, ref Base.HozferJogosultsag hozfer)
        {
            FormvezerlesInit(fak, treepanel, menupanel, parent, -1, ref kezelesiszint, ref hozfer);
        }
        private void FormvezerlesInit(FakUserInterface fak, Panel treepanel, Panel menupanel, Vezerloinfo parent, int almenuindex, ref Base.KezSzint kezelesiszint, ref Base.HozferJogosultsag hozfer)
        {
            InitializeComponent();
            FakUserInterface = fak;
            Alkalmazas = "TERVEZO";
            fak.ProgressRefresh();
            ParentVezerles = parent;
            Hivo = parent.Control;
            KezeloiSzint = kezelesiszint;
            HozferJog = hozfer;
            TreePanel = treepanel;
            MenuPanel = menupanel;
            SajatPanel = panel1;
            MenuStrip = menuStrip1;
            tabControl1.Dock = DockStyle.Fill;
            TabControl = tabControl1;
            TabControl.ShowToolTips = true;
            TabControl.Deselecting += TabControl_Deselecting;
            TabControl.Selected += TabControl_Selected;
            TabControl.Dock = DockStyle.Fill;
            SajatPanel.Controls.Add(TabControl);
            UserNevFilter = "ALKALMAZAS_ID=0";
            Vezerles = new Vezerloinfo(fak, this, -1, parent, ref kezelesiszint, ref hozfer, new ArrayList());
            if (almenuindex != -1)
                parent.ChildVezerloinfoCollection[almenuindex].AktivControl = this;
            Tablainfo alkalminfo = fak.GetKodtab("R", "Alkalm");
            if (!Tervezoe)
            {
                Tablainfo usernevek = fak.GetBySzintPluszTablanev("R", "USERCONTROLNEVEK");
                usernevek.DataView.RowFilter = UserNevFilter;
                Osszefinfo usernevusernev = fak.GetOsszef("R", "UserContStru").Osszefinfo;
                string userid = fak.GetTartal(usernevek, "ID", "SZOVEG", "Formvezerles")[0];
                string[] useridk = fak.GetSzurtOsszefIdk(usernevusernev, new object[] { userid, "" });
                string[] userek = fak.GetTartal(usernevek, "SZOVEG", "ID", useridk);
                Vezerles.ControlNevek = new ArrayList(userek);
                usernevek.DataView.RowFilter = "";
            }
            int db = alkalminfo.DataView.Count + 1;
            szovegek = new string[db];
            TabPage[] adatbtabpagek = new TabPage[db];
            ToolStripMenuItem[] adatbdropok = new ToolStripMenuItem[db];
            string[] adatbalmenunevek = new string[db];
            TabPage[] leirotabpagek = new TabPage[db];
            ToolStripMenuItem[] leirodropok = new ToolStripMenuItem[db];
            string[] leiroalmenunevek = new string[db];
            szovegek[0] = "TERVEZO";
            adatbtabpagek[0] = ((TabPage[])Vezerles.TabPagek[2])[0];
            adatbdropok[0] = ((ToolStripMenuItem[])Vezerles.DropItemek[2])[0];
            adatbalmenunevek[0] = ((string[])Vezerles.AlmenuUserControlNevek[2])[0];
            leirotabpagek[0] = ((TabPage[])Vezerles.TabPagek[3])[0];
            leirodropok[0] = ((ToolStripMenuItem[])Vezerles.DropItemek[3])[0];
            leiroalmenunevek[0] = ((string[])Vezerles.AlmenuUserControlNevek[3])[0];
            Vezerloinfo alvez;
            for (int i = 0; i < alkalminfo.DataView.Count; i++)
            {
                DataRow dr = alkalminfo.DataView[i].Row;
                string szov = dr["SZOVEG"].ToString();
                szovegek[i + 1] = szov;
                ToolStripMenuItem egyitem = new ToolStripMenuItem("Adatbázisinfo " + szov);
                Adatbazisinfo.DropDownItems.Add(egyitem);
                adatbdropok[i + 1] = egyitem;
                adatbtabpagek[i + 1] = new TabPage(egyitem.Text);
                adatbalmenunevek[i + 1] = adatbalmenunevek[0];
                egyitem = new ToolStripMenuItem("Leirások, megjegyzések " + szov);
                Leirasok.DropDownItems.Add(egyitem);
                leirodropok[i + 1] = egyitem;
                leirotabpagek[i + 1] = new TabPage(egyitem.Text);
                leiroalmenunevek[i + 1] = leiroalmenunevek[0];
            }
            Vezerles.TabPagek[2] = adatbtabpagek;
            Vezerles.DropItemek[2] = adatbdropok;
            Vezerles.AlmenuUserControlNevek[2] = adatbalmenunevek;
            Vezerles.TabPagek[3] = leirotabpagek;
            Vezerles.DropItemek[3] = leirodropok;
            Vezerles.AlmenuUserControlNevek[3] = leiroalmenunevek;
            for (int j = 0; j < Vezerles.ControlNevek.Count; j++)
            {
                fak.ProgressRefresh();
                string contnev = Vezerles.ControlNevek[j].ToString();
                if (contnev != "TervTreeView")
                {
                    alvez = new Vezerloinfo(fak, contnev, j, Vezerles, ref KezeloiSzint, ref HozferJog, new ArrayList());
                    ChildVezerlesek.Add(alvez);
                }
                else
                {
                    AktivPanel = SajatPanel;
                    if (!Tervezoe)
                        AktivPanel = MenuPanel;
                    fak.ProgressRefresh();
                    terv = new TervTreeView(fak, treepanel, AktivPanel, Vezerles, ref KezeloiSzint, ref HozferJog);
                    ChildVezerlesek.Add(terv.Vezerles);
                }
            }
            if (!Tervezoe)
                this.Controls.Remove(menuStrip1);
            MenuPanel.Controls.Add(this);
            this.Dock = DockStyle.Fill;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void TabControl_Deselecting(object sender, TabControlCancelEventArgs e)
        {
            base.TabControl_Deselecting(sender, e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void TabControl_Selected(object sender, TabControlEventArgs e)
        {
            base.TabControl_Selected(sender, e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void DropDownItem_Clicked(object sender, ToolStripItemClickedEventArgs e)
        {
            base.DropDownItem_Clicked(sender, e);
        }
        /// <summary>
        /// 
        /// </summary>
        public override void TabControl_Select()
        {
            base.TabControl_Select();
            WriteLoginfo();
        }
        /// <summary>
        /// 
        /// </summary>
        public override void WriteLoginfo()
        {
            if (!Tervezoe)
            {
                string tooltip = ",N";
                if (TreeView.ShowNodeToolTips)
                    tooltip = ",I";
                if (AktivControl != null)
                    FakUserInterface.WriteLogInfo(this.Name, AktivControl.Name + "," + Tabinfo.Azontip + "," + AktivMenuindex.ToString() + "," + AktivDropindex.ToString() + tooltip);
                else
                {
                    TreeNode node = TreeView.SelectedNode;
                    TablainfoTag tag;
                    string azontip;
                    if (node == null)
                        node = TreeView.Nodes[0];
                    if (node.Tag != null)
                        tag = (TablainfoTag)node.Tag;
                    else
                    {
                        node = node.Nodes[0];
                        if (node.Tag == null)
                            node = node.Nodes[0];
                    }
                    tag = (TablainfoTag)node.Tag;
                    if (tag.Tablainfo == null)
                    {
                        node = node.Nodes[0];
                        tag = (TablainfoTag)node.Tag;
                    }
                    if (tag.Tablainfo == null)
                    {
                        node = node.Nodes[0];
                        tag = (TablainfoTag)node.Tag;
                    }
                    azontip = tag.Tablainfo.Azontip;
                    FakUserInterface.WriteLogInfo(this.Name, "," + azontip + ",," + tooltip);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contnevek"></param>
        /// <param name="aktivcontrolnev"></param>
        public override void Inditas(string[] contnevek, string aktivcontrolnev)
        {
            string contnev = aktivcontrolnev;
            AktualNode = null;
            TablainfoTag  = null;
            Base control = null;
            switch (contnevek[0])
            {
                case "BaseKarb":
                    AktualNode = FakUserInterface.BaseNode;
                    TablainfoTag = FakUserInterface.BaseTag;
                    break;
                case "Leiroleirokarb":
                    AktualNode = FakUserInterface.LeiroNode;
                    TablainfoTag = FakUserInterface.LeiroTag;
                    break;
            }
            int contindex = Vezerles.OsszesControlNev.IndexOf(contnev);
            control = (Base)Vezerles.OsszesLetezoControl[contindex];
            if (control == null)
            {
                switch (contnev)
                {
                    case "BaseKarb":
                        control = new BaseKarb(AktivVezerles);
                        break;
                    case "Leirokarb":
                        control = new Leirokarb(AktivVezerles);
                        break;
                    case "Tooltipallit":
                        control = new Tooltipallit(AktivVezerles);
                        break;
                    case "LeiroTooltipallit":
                        control = new LeiroTooltipallit(AktivVezerles);
                        break;
                    case "Leiroleirokarb":
                        control = new Leiroleirokarb(AktivVezerles);
                        break;
                    case "AdatbazisOsszefuggesek":
                        control = new AdatbazisOsszefuggesek(AktivVezerles);
                        break;
                    case "LeirasOsszefuggesek":
                        control = new LeirasOsszefuggesek(AktivVezerles);
                        break;
                }
                control.Dock = DockStyle.Fill;
                AktivVezerles.OsszesLetezoControl[contindex] = control;
            }
            AktivControl = control;
            AktivControl.AktivMenuItem = AktivMenuItem;
            AktivControl.AktivDropDownItem = AktivDropDownItem;
            AktivControl.AktivPage = TabPagek[AktivDropindex];
            if (AktualNode != null)
            {
                AktivControl.UjTag = VezerlesValtas || AktivControl.UjTag || AktivControl.AktualNode != AktualNode;//|| AktivControl.AktivDropDownItem != AktivDropDownItem;
                AktivControl.AktualNode = AktualNode;
                AktivControl.TablainfoTag = TablainfoTag;
            }
            else
            {
                try
                {
                    ((AdatbazisOsszefuggesek)AktivControl).Alkalmnev = szovegek[AktivDropindex];
                }
                catch
                {
                    try
                    {
                        ((LeirasOsszefuggesek)AktivControl).Alkalmnev = szovegek[AktivDropindex];
                    }
                    catch
                    {
                    }
                }
            }
            AktivControl.TabStop = false;
            AktivControl.TabStop = true;
            if (!contnevek[0].Contains("Osszefuggesek"))
            {
                for (int i = 0; i < contnevek.Length; i++)
                {
                    contnev = contnevek[i];
                    contindex = Vezerles.OsszesControlNev.IndexOf(contnev);
                    control = (Base)Vezerles.OsszesLetezoControl[contindex];
                    if (control != null && control != AktivControl)
                    {
                        control.UjTag = AktivControl.UjTag || control.UjTag || control.AktualNode != AktualNode; // || control.AktivDropDownItem != AktivDropDownItem;
                        control.AktualNode = AktualNode;
                        control.TablainfoTag = TablainfoTag;
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cegindex"></param>
        /// <param name="lezartceg"></param>
        /// <param name="kezszint"></param>
        /// <param name="usercontnev"></param>
        /// <param name="userparam"></param>
        /// <returns></returns>
        public override bool Ceginicializalas(int cegindex,bool lezartceg, Base.KezSzint kezszint, string usercontnev, string userparam)
        {
            Cegindex = cegindex;
            KezeloiSzint = kezszint;
            LezartCeg = lezartceg;
            if (usercontnev != "" && userparam != "" && usercontnev == this.Name)
                terv.userparamok = userparam;
            terv.TabStop = false;
            terv.TabStop = true;
            ValtozasTorol();
            Elsoeset = false;
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        public override void RogzitesUtan()
        {
            if (Hivo.Name != "TervControl")
                Hivo.RogzitesUtan();
            Hivo.ValtozasBeallit();
        }
        public override bool DatumParameterezInit(DateTime[] mindatumok, DateTime[] maxdatumok, DateTime[] alapertdatumok, DateTime[] aktdatumok)
        {
            if (Hivo.Name != "TervControl")
                return Hivo.DatumParameterezInit(mindatumok, maxdatumok, alapertdatumok, aktdatumok);
            else
                return false;
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void TabStop_Changed(object sender, EventArgs e)
        {
            if (TabStop)
            {
                terv.TabStop = false;
                terv.TabStop = true;
            }
        }
    }
}
