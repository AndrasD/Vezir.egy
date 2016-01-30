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
    /// TreeView-s vezerlo user control
    /// </summary>
    public partial class TervTreeView : VezerloAlapControl
    {
        private string tablanev;
        private string termszarm;
        private string szint;
        private string adatfajta;
        private string azon;
        private string azontip;
        private string[] contnevek;
        private string aktualcontnev;
        private TabControl tabControl1 = new TabControl();
        /// <summary>
        /// 
        /// </summary>
        public string userparamok = "";
        /// <summary>
        /// 
        /// </summary>
        public bool nodevaltas = true;
        private bool[] enablevaltas;
        bool fakenyem;
        bool enyem;
        bool leiroenyem;
        private bool tervezo;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fak"></param>
        /// <param name="treepanel"></param>
        /// <param name="menupanel"></param>
        /// <param name="parent"></param>
        /// <param name="kezelesiszint"></param>
        /// <param name="hozfer"></param>
        public TervTreeView(FakUserInterface fak, Panel treepanel, Panel menupanel, Vezerloinfo parent, ref Base.KezSzint kezelesiszint, ref Base.HozferJogosultsag hozfer)
        {
            InitializeComponent();
            FakUserInterface = fak;
            tervezo = Tervezoe;
            Alkalmazas = "TERVEZO";
            fakenyem = fak.Enyem;
            TreePanel = treepanel;
            TabControl = tabControl1;
            TabControl.Dock = DockStyle.Fill;
            TabControl.Selecting += TabControl_Deselecting;
            TabControl.Selected += TabControl_Selected;
            TabControl.ShowToolTips = true;
            KezeloiSzint = kezelesiszint;
            HozferJog = hozfer;
            MenuStrip = menuStrip1;
            MenuPanel = menupanel;
            SajatPanel = panel1;
            ParentVezerles = parent;
            Hivo = ParentVezerles.Control;
            SajatPanel.Controls.Add(TabControl);
            int menuindex = parent.ControlNevek.IndexOf(this.Name);
            TreeView = null;
            if (treepanel != null && treepanel.Controls.Count != 0)
                TreeView = (TreeView)treepanel.Controls[0];
            else
            {
                TreeView = new TreeView();
                TreeView.BorderStyle = BorderStyle.None;
                TreeView.Dock = DockStyle.Fill;
                TreeView.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                TreeView.FullRowSelect = true;
                TreeView.HideSelection = false;
                TreeView.ShowLines = false;
                TreeView.Indent = 16;
                TreeView.ItemHeight = 18;
                TreeView.Cursor = System.Windows.Forms.Cursors.Hand;
                TreeView.BackColor = FakUserInterface.InaktivControlBackColor;
                TreeView.ShowNodeToolTips = true;
                TreeView.TabStop = false;
                treepanel.Controls.Add(TreeView);
                TreeView.Dock = DockStyle.Fill;
                if (Tervezoe)
                    GetTreeView(TreeView);
                else
                    GetTreeView(TreeView, KezeloiSzint);
            }
            parent.TreeView = TreeView;
            Hivo.TreeView = TreeView;
            TreeView.NodeMouseClick += TreeView_NodeMouseClick;
            TreeView.ContextMenuStrip = contextMenuStrip1;
            kelltooltip.Enabled = false;
            nemkelltooltip.Enabled = true;
            if (KezeloiSzint == KezSzint.Fejleszto)
            {
                TreeView.Nodes[0].Expand();
                if (TreeView.Nodes.Count > 1)
                    TreeView.Nodes[1].Expand();
            }
            UserNevFilter = "ALKALMAZAS_ID=0";
            Vezerles = new Vezerloinfo(fak, this, menuindex, parent, ref KezeloiSzint, ref HozferJog, new ArrayList());
            AktivVezerles = Vezerles;
            ArrayList tabpagear = new ArrayList();
            ArrayList dropar = new ArrayList();
            for (int i = 0; i < Vezerles.TabPagek.Count; i++)
            {
                tabpagear.Add(((TabPage[])Vezerles.TabPagek[i])[0]);
                dropar.Add(((ToolStripMenuItem[])Vezerles.DropItemek[i])[0]);
            }
            if (tervezo)
            {
                TabPagek = (TabPage[])tabpagear.ToArray(typeof(TabPage));
                TabControl.Controls.AddRange(TabPagek);
                DropItemek = (ToolStripMenuItem[])dropar.ToArray(typeof(ToolStripMenuItem));
            }
            else
            {
                TabPagek = new TabPage[] { (TabPage)tabpagear[0] };
                TabPagek[0].Text = "";
                TabControl.Controls.AddRange(TabPagek);
                DropItemek = new ToolStripMenuItem[] { (ToolStripMenuItem)dropar[0] };
            }
            enablevaltas = new bool[DropItemek.Length];
            MenuItemek = Vezerles.MenuItemek;
            Datumtol=FakUserInterface.Aktintervallum[0];
            ValasztekIndex=0;
            AktivMenuindex = 0;
            AktivDropindex = 0;
        }
        /// <summary>
        /// TERVEZO szamara treeview osszeallitasa
        /// </summary>
        /// <param name="treeview">
        /// TreeView
        /// </param>
        /// <returns>
        /// TreeView
        /// </returns>
        public TreeView GetTreeView(TreeView treeview)
        {
            foreach (TablainfoTag tag in FakUserInterface.GyokerTablainfoTagok)
                treeview.Nodes.Add(tag.Node);
            return treeview;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="treeview"></param>
        /// <param name="kezeloiszint"></param>
        /// <returns></returns>
        public TreeView GetTreeView(TreeView treeview, Base.KezSzint kezeloiszint)
        {
            TablainfoTag tag = (TablainfoTag)FakUserInterface.GyokerTablainfoTagok[0];
            int kezszint = (int)kezeloiszint;
            foreach (TablainfoTag tag1 in tag.ChildTablainfoTagok)
            {
                if (tag1.Azonositok.Szint == "R")
                {
                    foreach (TablainfoTag tag2 in tag1.ChildTablainfoTagok)
                    {
                        if (tag2.Azonositok.Tablanev != "")
                        {
                            TreeNode node = new TreeNode(tag2.Node.Text);
                            node.Tag = tag2.Node.Tag;
                            node.ToolTipText = tag2.Node.ToolTipText;
                            foreach (TablainfoTag tag3 in tag2.ChildTablainfoTagok)
                            {
                                if (tag3.Azonositok.Jogszintek[kezszint] != Base.HozferJogosultsag.Semmi)
                                {
                                    TreeNode node1 = new TreeNode(tag3.Node.Text);
                                    node1.Tag = tag3.Node.Tag;
                                    node1.ToolTipText = tag3.Node.ToolTipText;
                                    node.Nodes.Add(node1);
                                }
                            }
                            if (node.Nodes.Count != 0)
                                treeview.Nodes.Add(node);
                        }
                    }
                }
            }
            return treeview;
        }
        /// <summary>
        /// Node valasztas, ha kell es lehet, az uj Node alapjan inditja a szukseges UserControl-t
        /// hivja a kovform()-ot es az Inditas()-t
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (FakUserInterface.EventTilt)
                return;
            TreeViewHitTestInfo hittest = TreeView.HitTest(e.X, e.Y);
            if (e.Button == MouseButtons.Right)
            {
                if (e.Node != null)
                {
                    if (e.Node.IsExpanded)
                        e.Node.Collapse(false);
                    else
                        e.Node.ExpandAll();
                }
            }
            if (e.Button == MouseButtons.Left)
            {
                if (hittest.Location == TreeViewHitTestLocations.Label)
                {
                    TablainfoTag tag = (TablainfoTag)hittest.Node.Tag;
                    if (tag == null || tag.Tablainfo == null)
                        return;
                    MenuPageValtas=false;
 //                   bool tervezo = FakUserInterface.Alkalmazas == "TERVEZO";
                    if (Tervezoe)
                    {
                        if (MenuPanel.Controls[0].Name == "")
                            VezerlesValtas = true;
                        if (hittest.Node == AktualNode)
                        {
                            VezerlesValtas = false;
                            return;
                        }
                        else
                            nodevaltas = true;
                    }
                    else if (Hivo.Hivo.AktivControl !=null && Hivo.Hivo.AktivControl != Hivo)
                    {

                        if (Hivo.Hivo.AktivControl.Userabortkerdes())
                            return;
                        Hivo.Hivo.AktivControl = Hivo;
                        nodevaltas = true;
                    }
                    else if (hittest.Node == AktualNode)
                    {
                        VezerlesValtas = false;
                        return;
                    }
                    else
                        nodevaltas = true;
                    if (AktivControl == null || AktivControl.Tabinfo == null || !AktivControl.Userabortkerdes(AktivControl.Tabinfo))
                    {
                        if (AktivControl != null)
                        {
                            if (AktivControl.Name == "Mezonevek")
                            {
                                if (AktivControl.Tabinfo.Tablanev == "VALTOZASNAPLO" || AktivControl.Tabinfo.Tablanev == "USERLOG")
                                    AktivControl.Tabinfo.Adattabla.Rows.Clear();
                            }
                        }
                        AktualNode = hittest.Node;
                        TablainfoTag = tag;
                        DropItemek[0].Text = AktualNode.Text;
                        TabPagek[0].Text = AktualNode.Text;
                        Kiertekel();
                    }

                }
            }
        }
        private void Kiertekel()
        {
            string contnev = "";
            bool kelltabla = true;
            bool kelleiro = true;
            bool kelltooltip = true;
            bool kellleirotooltip = true;
            Tabinfo = TablainfoTag.Tablainfo;
            LeiroTabinfo = TablainfoTag.LeiroTablainfo;
            tablanev = Tabinfo.Tablanev;
            szint = Tabinfo.Szint;
            termszarm = Tabinfo.TermSzarm.Trim();
            adatfajta = Tabinfo.Adatfajta;
            azon = Tabinfo.Azon;
            azontip = Tabinfo.Azontip;
            enyem = Tabinfo.Azonositok.Enyem;
            leiroenyem = LeiroTabinfo.Azonositok.Enyem;
            if (tervezo)
            {
                if (tablanev != "TARTAL" && tablanev != "VALTOZASNAPLO" && tablanev != "LISTA" && tablanev!="STATISZTIKA" && "CU".Contains(szint))
                {
                    for (int i = 0; i < Tabinfo.Azonositok.Jogszintek.Length; i++)
                        Tabinfo.Azonositok.Jogszintek[i] = HozferJogosultsag.Csakolvas;
                }
 //                   Tabinfo.Azonositok.Jogszintek
            }
            if (Tabinfo.TablaColumns.IndexOf("TOOLTIP") == -1)
                kelltooltip = false;
            //owner = Tabinfo.Azonositok.Owner;
            //ownerid = Tabinfo.Azonositok.Ownerid;
            do
            {
                if (adatfajta == "" || azon == "SZRM")
                {
                    contnev = "Mezonevek";
                    kelltooltip = false;
                    kellleirotooltip = false;
                    kelleiro = false;
                    break;
                }
                if (tablanev == "VALTOZASNAPLO" || tablanev == "USERLOG")
                {
                    contnev = "Mezonevek";
                    kelltooltip = false;
                    break;
                }
                if (tablanev == "TARTAL")
                {
                    if (tervezo)
                        contnev = "Altalanos";
                    else
                        contnev = "";
                    break;
                }
                if (adatfajta == "T")
                {
                    contnev = "Altalanos";
                    if (termszarm != "SZ")
                    {
                        kelltabla = false;
                        kelltooltip = false;
                    }
                    break;
                }
                kelltooltip = false;
                kellleirotooltip = false;
                switch (adatfajta)
                {
                    case "K":
                        contnev = "Altalanos";
                        kelltooltip = true;
                        kellleirotooltip = true;
                        break;
                    case "O":
                        contnev = "Osszef";
                        kellleirotooltip = true;
                        break;
                    case "A":
                        contnev = "Adatszolg";
                        kellleirotooltip = true;
                        break;
                    case "C":
                        if (!TablainfoTag.Forditott)
                        {
                            contnev = "Csoport";
                            kellleirotooltip = true;
                        }
                        else
                        {
                            contnev = "Attekint";
                            kelleiro = false;
                        }
                        break;
                    case "F":
                        contnev = "Fogalom";
                        kelleiro = false;
                        break;
                    case "S":
                        contnev = "Szukkodtab";
                        break;
                    case "L":
                        contnev = "Listaterv";
                        kellleirotooltip = true;
                        break;
                    case "I":
                        contnev = "Statterv";
                        kellleirotooltip = true;
                        break;
                    case "N":
   //                     if (szint == "R")
                            contnev = "Naptar";
 //                       else
 //                           contnev = "MuszakNaptar";
                        kelltooltip = true;
                        kellleirotooltip = true;
                        break;
                }
                break;
            } while (true);
            ArrayList contnevar = new ArrayList();
            if (contnev != "")
            {
                contnevar.Add(contnev);
 //               FakUserInterface.EventTilt = true;
                if (tervezo)
                {
                    contnevar.Add("Leirokarb");
                    contnevar.Add("Tooltipallit");
                    contnevar.Add("LeiroTooltipallit");
                    TabControl.Controls[1].Enabled = kelleiro;
                    TabControl.Controls[2].Enabled = kelltooltip;
                    TabControl.Controls[3].Enabled = kellleirotooltip;
                    enablevaltas[0] = DropItemek[0].Enabled != kelltabla;
                    DropItemek[0].Enabled = kelltabla;
                    enablevaltas[1] = DropItemek[1].Enabled != kelleiro;
                    DropItemek[1].Enabled = kelleiro;
                    enablevaltas[2] = DropItemek[2].Enabled != kelltooltip;
                    DropItemek[2].Enabled = kelltooltip;
                    enablevaltas[3] = DropItemek[3].Enabled != kellleirotooltip;
                    DropItemek[3].Enabled = kellleirotooltip;
                }
                else if (SajatPanel.Controls.Count == 0)
                    SajatPanel.Controls.Add(TabControl);
//                FakUserInterface.EventTilt = false;
                contnevek = (string[])contnevar.ToArray(typeof(string));
                aktualcontnev = contnev;
                TabControl_Select();
            }
            else
                SajatPanel.Controls.Clear();
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void TabControl_Selected(object sender, TabControlEventArgs e)
        {
            if (!FakUserInterface.EventTilt)
            {
                AktivPageIndex = e.TabPageIndex;
                AktivDropindex = AktivPageIndex;
                MenuPageValtas = true;
                VezerlesValtas = false;
                nodevaltas = false;
                TablainfoTag = (TablainfoTag)AktualNode.Tag;
                Kiertekel();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public override void TabControl_Select()
        {
            AktivPage = TabPagek[AktivDropindex];
            AktivDropDownItem = DropItemek[AktivDropindex];
            AktivMenuItem = MenuItemek[AktivMenuindex];
            Vezerles.AktivPage = AktivPage;
            Vezerles.AktivMenuItem = AktivMenuItem;
            Vezerles.AktivDropDownItem = AktivDropDownItem;
            Vezerles.AktivVezerles = Vezerles;
            aktualcontnev = contnevek[AktivDropindex];
            int contindex = -1;
            Base control = null;
            contindex = Vezerles.OsszesControlNev.IndexOf(aktualcontnev);
            control = (Base)Vezerles.OsszesLetezoControl[contindex];
            Parameterez = Vezerles.Parameterez[contindex];
            if (control == null)
                Aktiv = false;
            else
                Aktiv = true;
            if (VezerlesValtas || MenuPanel.Controls.Count==0 || MenuPanel.Controls[0]!=SajatPanel)
            {
                FakUserInterface.EventTilt = true;
                FakUserInterface.RemoveAllControls(MenuPanel);
                MenuPanel.Controls.Add(SajatPanel);
                FakUserInterface.EventTilt = false;
                if (!nodevaltas)
                {
                    if (SajatPanel.Controls.Count == 0)
                        SajatPanel.Controls.Add(TabControl);
                    return;
                }
            }
            if (!Tervezoe)
            {
                    control = Hivo.Hivo.SetAktivControl(TablainfoTag, Vezerles);
                    if (control != null)
                        aktualcontnev = control.Name;
            }
            Inditas(contnevek, aktualcontnev);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contnevek"></param>
        /// <param name="aktivcontrolnev"></param>
        public override void Inditas(string[] contnevek, string aktivcontrolnev)
        {
            string contnev = aktivcontrolnev;
            Base control;
            if (contnev != "")
            {
                int contindex = AktivVezerles.OsszesControlNev.IndexOf(contnev);
                control = (Base)AktivVezerles.OsszesLetezoControl[contindex];
                if (control == null)
                {
                    switch (contnev)
                    {
                        case "Altalanos":
                            control = new Altalanos(Vezerles);
                            break;
                        case "Leirokarb":
                            control = new Leirokarb(Vezerles);
                            break;
                        case "Tooltipallit":
                            control = new Tooltipallit(Vezerles);
                            break;
                        case "LeiroTooltipallit":
                            control = new LeiroTooltipallit(Vezerles);
                            break;
                        case "Adatszolg":
                            control = new Adatszolg(Vezerles);
                            break;
                        case "Naptar":
                            control = new Naptar();
                            ((Naptar)control).ParameterAtvetel(Vezerles, false);
                            break;
                        case "MuszakNaptar":
                            control = new MuszakNaptar();
                            ((Naptar)control).ParameterAtvetel(Vezerles, false);
                            break;
                        case "Attekint":
                            control = new Attekint(Vezerles);
                            break;
                        case "Csoport":
                            control = new Csoport(Vezerles);
                            break;
                        case "Fogalom":
                            control = new Fogalom(Vezerles);
                            break;
                        case "Mezonevek":
                            control = new Mezonevek(Vezerles);
                            break;
                        case "Osszef":
                            control = new Osszef(Vezerles);
                            break;
                        case "Szukkodtab":
                            control = new Szukkodtab(Vezerles);
                            break;
                        case "Listaterv":
                            control = new Listaterv(Vezerles);
                            break;
                        case "Statterv":
                            control = new Statterv(Vezerles);
                            break;
                    }
                    control.Dock = DockStyle.Fill;
                    AktivVezerles.OsszesLetezoControl[contindex] = control;
                    if (Parameterez != null)
                    {
                        control.Datumtol = Datumtol;
                        control.ValasztekIndex = ValasztekIndex;
                        control.Paramfajta = Parameterez.Paramfajta;
                        control.Valasztek = Parameterez.Valasztek;
                        control.Listae = Parameterez.Listae;
                        control.VanDatum = Parameterez.VanDatum;
                        control.VanValasztek = Parameterez.VanValasztek;
                        Parameterez.AktivControl = control;
                        Parameterez.ValasztekIndex = ValasztekIndex;
                        Parameterez.Datumtol = Datumtol;
                        FakUserInterface.EventTilt = true;
                        Parameterez.ListaAdatbevPage.Controls.Add(control);
                        FakUserInterface.EventTilt = false;
                        control.Parameterez = Parameterez;

                    }
                }
                AktivControl = control;
                AktivControl.UjTag = AktivControl.UjTag || !Aktiv || nodevaltas && AktivControl.AktualNode != AktualNode || enablevaltas[AktivDropindex];
                AktivControl.AktualNode = AktualNode;
                AktivControl.TablainfoTag = TablainfoTag;
                for (int i = 0; i < TabPagek.Length; i++)
                {
                    TabPagek[i].ToolTipText = "";
                    if (AktivVezerles.MultiUser[i] != null)
                    {
                        string[] ar = (string[])AktivVezerles.MultiUser[i];
                        ArrayList arar = new ArrayList(ar);
                        string[] artooltip = (string[])AktivVezerles.MultiUserToolTip[i];
                        int j = arar.IndexOf(AktivControl.Name);
                        if (j != -1)
                            TabPagek[i].ToolTipText = artooltip[j];
                    }
                }
                if (TablainfoTag.Azonositok.Tooltiptext != "")
                    TabPagek[0].ToolTipText = TablainfoTag.Azonositok.Tooltiptext;
                WriteLoginfo();
            }
            else
                SajatPanel.Controls.Clear();
            for (int i = 0; i < contnevek.Length; i++)
            {
                contnev = contnevek[i];
                if (contnev != "")
                {
                    int contindex = AktivVezerles.OsszesControlNev.IndexOf(contnev);
                    control = (Base)AktivVezerles.OsszesLetezoControl[contindex];
                    if (control != null && control != AktivControl)
                    {
                        control.UjTag = AktivControl.UjTag || control.UjTag || control.AktualNode != AktualNode || enablevaltas[i];
                        control.AktualNode = AktualNode;
                        control.TablainfoTag = TablainfoTag;
                    }
                }
            }
            FakUserInterface.EventTilt = true;
            Hivo.AktivControl = AktivControl;
            Hivo.Hivo.AktivControl = Hivo;
            AktivControl.AktivPage = AktivPage;
            AktivControl.TabControl = TabControl;
            FakUserInterface.RemoveAllControls(AktivPage);
            if (Parameterez == null)
            {
              //  AktivControl.Dock = DockStyle.Fill;
                AktivPage.Controls.Add(AktivControl);
              //  AktivControl.Dock = DockStyle.None;
            }
            else
            {
              //  Parameterez.Dock = DockStyle.Fill;
                AktivPage.Controls.Add(Parameterez);
             //   Parameterez.Dock = DockStyle.None;
            }
            TabControl.SelectedIndex = AktivDropindex;
            FakUserInterface.EventTilt = false;
            AktivControl.AktivMenuItem = AktivMenuItem;
            AktivControl.AktivDropDownItem = AktivDropDownItem;
            contnev = AktivControl.Name;
            if (contnev == "Listaterv" || contnev == "Statterv")
            {
                AktivControl.TabStop = false;
                AktivControl.TabStop = true;
            }
            else
            {
                AktivControl.LezartCeg = LezartCeg;
                AktivControl.AltalanosInit();
            }
            Elsoeset = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void TabStop_Changed(object sender, EventArgs e)
        {
            if (this.TabStop)
            {
                string[] split = null;
                string contnev = "";
                if (KezeloiSzint != Hivo.KezeloiSzint || LezartCeg!=Hivo.LezartCeg || ValtozasLekerdez().Count!=0)
                {
                    KezeloiSzint = Hivo.KezeloiSzint;
                    LezartCeg = Hivo.LezartCeg;
                    string[] tablanevek = (string[])Vezerles.MultiUser[0];
                    for (int i = 0; i < tablanevek.Length; i++)
                    {
                        Base control;
                        if (tablanevek[i] != this.Name)
                        {
                            int contindex = Vezerles.OsszesControlNev.IndexOf(tablanevek[i]);
                            if (Vezerles.OsszesLetezoControl[contindex] != null)
                            {
                                control = (Base)Vezerles.OsszesLetezoControl[contindex];
                                Tablainfo tabinfo = control.Tabinfo;
                                control.KezeloiSzint = KezeloiSzint;
                                control.LezartCeg = LezartCeg;
                                control.UjTag = true;
                            }
                        }
                    }
                }
                if (ValtozasLekerdez("CegValtozas").Count != 0)
                {
                    if (userparamok != "")
                    {
                        char[] vesszo = new char[] { Convert.ToChar(",") };
                        split = userparamok.Split(vesszo);
                        {
                            contnev = split[0];

                            TreeView.ShowNodeToolTips = split[6] == "I";
                            if (split[6] == "I")
                            {
                                kelltooltip.Enabled = false;
                                nemkelltooltip.Enabled = true;
                            }
                            else
                            {
                                kelltooltip.Enabled = true;
                                nemkelltooltip.Enabled = false;
                            }
                            if (contnev != "")
                            {
                                FakUserInterface.EventTilt = true;
                                if (split[1] == "")
                                    Datumtol = FakUserInterface.Aktintervallum[0];
                                else
                                    Datumtol = Convert.ToDateTime(split[1]);
                                if (split[2] != "")
                                    ValasztekIndex = Convert.ToInt32(split[2]);
                                string azontip = split[3];
                                AktivMenuindex = Convert.ToInt16(split[4]);
                                AktivDropindex = Convert.ToInt16(split[5]);
                                string forditott = split[7];
                                Tabinfo = FakUserInterface.GetByAzontip(azontip);
                                TablainfoTag = Tabinfo.Azonositok.Tablatag;
                                if (forditott == "I")
                                    TablainfoTag = TablainfoTag.FordTag;
                                AktualNode = TablainfoTag.Node;
                                FakUserInterface.RemoveAllControls(MenuPanel);
                                MenuPanel.Controls.Add(SajatPanel);
                                Vezerles.AktivVezerles = Vezerles;
                                AktivMenuItem = MenuItemek[AktivMenuindex];
                                AktivDropDownItem = DropItemek[AktivDropindex];
                                DropItemek[0].Text = AktualNode.Text;
                                TabPagek[0].Text = AktualNode.Text;
                                FakUserInterface.EventTilt = false;
                            }
                        } 
                    }

                    if (contnev != "")
                        Kiertekel();
                    else
                    {
                        FakUserInterface.EventTilt = true;
//                        FakUserInterface.RemoveAllControls(MenuPanel);
                        FakUserInterface.RemoveAllControls(SajatPanel);
                        FakUserInterface.EventTilt = false;
                    }
                    userparamok = "";
                }
                else
                {
                    FakUserInterface.EventTilt = true;
                    FakUserInterface.RemoveAllControls(MenuPanel);
                    MenuPanel.Controls.Add(SajatPanel);
                    FakUserInterface.EventTilt = false;
                    if (AktivControl != null)
                    {
                        if (AktivPage.Controls.Count == 0)
                        {
                            FakUserInterface.EventTilt = true;
                            AktivPage.Controls.Add(AktivControl);
                            FakUserInterface.EventTilt = false;
                        }
                        Tabinfo = AktivControl.Tabinfo;
                        AktivControl.TabStop = false;
                        AktivControl.TabStop = true;
                    }
                    if(ValtozasLekerdez().Count!=0)
                        WriteLoginfo();
                }
                Hivo.AktivControl = AktivControl;
                Hivo.Hivo.AktivControl = Hivo;
                ValtozasTorol();
            }
        }

        private void kinyit_Click(object sender, EventArgs e)
        {
            foreach (TreeNode node in TreeView.Nodes)
            {
                node.Expand();
                foreach (TreeNode node1 in node.Nodes)
                    node1.Expand();
            }
        }

        private void becsuk_Click(object sender, EventArgs e)
        {
            foreach (TreeNode node in TreeView.Nodes)
                node.Collapse();
        }

        private void kelltooltip_Click(object sender, EventArgs e)
        {
            TreeNode node = TreeView.SelectedNode;
            TreeView.ShowNodeToolTips = true;
            kelltooltip.Enabled = false;
            nemkelltooltip.Enabled = true;
            if (node != null)
            {
                if (node.Nodes.Count != 0)
                    node.Expand();
                else if (node.Parent != null)
                {
                    if (node.Parent.Nodes.Count != 0)
                        node.Parent.Expand();
                }
            }
            else
            {
                TreeView.Nodes[0].Expand();
                for(int i=0;i<TreeView.Nodes.Count;i++)
                    TreeView.Nodes[i].Expand();
                for (int i = 0; i < TreeView.Nodes.Count; i++)
                    TreeView.Nodes[i].Expand();
            }
            WriteLoginfo();
        }

        private void nemkelltooltip_Click(object sender, EventArgs e)
        {
            TreeView.ShowNodeToolTips = false;
            kelltooltip.Enabled = true;
            nemkelltooltip.Enabled = false;
            WriteLoginfo();
        }
        /// <summary>
        /// 
        /// </summary>
        public override void WriteLoginfo()
        {
            if (!tervezo)
            {
                string tooltip = ",I";
                if (!TreeView.ShowNodeToolTips)
                    tooltip = ",N";
                if (AktivControl != null)
                {
                    int paramtipus = (int)AktivControl.Paramfajta;
                    string datum = "";
                    string valasztekindex = "";
                    if (AktivControl.ParamfajtaString.Contains("Datum"))
                        datum = AktivControl.Datumtol.ToShortDateString();
                    if (AktivControl.Paramfajta == Parameterezes.Datumpluszvalasztek)
                        valasztekindex = AktivControl.ValasztekIndex.ToString();
                    string forditott = "N";
                    if (AktivControl.TablainfoTag.Forditott)
                        forditott="I";
                    FakUserInterface.WriteLogInfo(Hivo.Name, paramtipus, AktivControl.Name + "," + datum + "," + valasztekindex + "," + Tabinfo.Azontip + "," + AktivMenuindex.ToString() + "," + AktivDropindex.ToString() + tooltip + "," + forditott);
                }
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
                    FakUserInterface.WriteLogInfo(Hivo.Name, ",,," + azontip + ",," + tooltip + ",");
                }
                Hivo.Hivo.SaveUserLogLastRow();
            }
        }
    }
}
