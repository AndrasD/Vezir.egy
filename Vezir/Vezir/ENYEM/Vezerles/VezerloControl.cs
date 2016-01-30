using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FakPlusz.Alapfunkciok;
using FakPlusz;
using FakPlusz.Alapcontrolok;
using FakPlusz.UserAlapcontrolok;
using FakPlusz.Formok;
using FakPlusz.VezerloFormok;

namespace Vezir
{
    public partial class VezerloControl : VezerloAlapControl
    {
        private TreeNode alkalmnode = new TreeNode("Alkalmazás ");
        private TreeNode beallitasnode = new TreeNode("Beállitások");
        private TreeNode userlognode = new TreeNode("LogTábla,Változások naplózása");
        private TreeNode modosithatonodeok = new TreeNode("Kódtáblák, szerkesztések");
        private TreeNode tartalnode;
        private TreeNode cegszintunode = new TreeNode();
        public bool ElsoEset = true;
        public Main MainForm;
        public MainControl MainControl;
        public string UserParamok = "";
        private ArrayList letezousercontnevek;
        public bool[] EredetiEnable;
        public ArrayList OsszesTabPage = new ArrayList();
        public ToolStripMenuItem[] OsszesMenuItem = null;
        public ArrayList OsszesDropItem = new ArrayList();
        public ArrayList OsszesEredetiEnable = new ArrayList();
        public VezerloinfoCollection OsszesVezerles = new VezerloinfoCollection();
        public TabControl[] TabControlok = null;
        public Partner kulonpartner = null;
        public Tablainfo szarmazekospartnerinfo;
        public Tablainfo szarmazekoskiegpartnerinfo;
        public Tablainfo partnergyokerinfo;
        public Cols bevpartnercol;
        public Cols koltspartnercol;
        public Cols partnerkoltsegkodcol;
        public Cols partnertermekkodcol;
        public Cols partnersemakodcol;
        public Cols termsemakodcol;
        public int koltsegkodindex;
        public int termekkodindex;
        public int semakodindex;
        public int termsemakodindex;
        public int bevpartnerindex;
        public int koltspartnerindex;
        public bool lehetvevo = true;
        public bool lehetszallito = true;
        public ComboBox bevpartnercombo;
        public ComboBox koltspartnercombo;
        private bool ujcegverzio = false;
        private ArrayList partnerszoveglist;
        public VezerloControl(FakUserInterface fak, Vezerloinfo parent, Panel treepanel, Panel mainpanel)
        {
            InitializeComponent();
            FakUserInterface = fak;
            ParentVezerles = parent;
            Hivo = parent.Control;
            TreePanel = treepanel;
            mainpanel.Controls.Add(this);
            MenuPanel = panel1;
            MainControl = (MainControl)Hivo;
            MainForm = (Main)MainControl.MainForm;
            AktivMenuindex = -1;
            AktivDropindex = -1;
            KezeloiSzint = UserParamTabla.AktualCegInformaciok[UserParamTabla.AktivCegIndex].KezeloiSzint;
            //usercontnevek = FakUserInterface.GetBySzintPluszTablanev("R", "USERCONTROLNEVEK");
            //kezszintek = FakUserInterface.GetKodtab("R", "Kezszint");
            //UserContKezszint = FakUserInterface.GetOsszef("R", "UserContKezszint");
            UserParamok = UserParamTabla.UserParamok;
            TreeView = (TreeView)treepanel.Controls[0];
            letezousercontnevek = new ArrayList(MainControl.letezousercontrolnevek);
            MenuStrip = menuStrip1;
            Vezerles = new Vezerloinfo(FakUserInterface, this, AktivMenuindex, parent, ref KezeloiSzint, ref HozferJog, letezousercontnevek);
            Vezerles.LetezoControlok = new Base[Vezerles.LetezoUserControlNevek.Count];
            OsszesMenuItem = Vezerles.MenuItemek;
            TabControlok = new TabControl[OsszesMenuItem.Length];
            for (int i = 0; i < TabControlok.Length; i++)
            {
                TabControlok[i] = new TabControl();
                TabControlok[i].ShowToolTips = true;
                TabControlok[i].Dock = DockStyle.Fill;
                TabControlok[i].Selecting += TabControl_Selecting;
                TabControlok[i].Click += TabControl_Click;
            }
            OsszesDropItem = Vezerles.DropItemek;
            OsszesTabPage = Vezerles.TabPagek;
            OsszesEredetiEnable = Vezerles.EredetiDropEnablek;
            Vezerloinfo alvez;
            Base control = null;
            for (int i = 0; i < Vezerles.MenuNevek.Length; i++)
            {
                if (Vezerles.MenuNevek[i] == "Formvez")
                {
                    Vezerles.AktivControl = this;
                    control = new Formvezerles(FakUserInterface, treepanel, MenuPanel, Vezerles, ref KezeloiSzint, ref UserParamTabla.CegSzarmazekosJogosultsag);
                    MainControl.FormVezerles = (Formvezerles)control;
                    alvez = null;
                }
                else
                {
                    alvez = new Vezerloinfo(FakUserInterface, Vezerles, i, ref KezeloiSzint, ref UserParamTabla.AktualTermeszetesJogosultsag, letezousercontnevek);
                    alvez.TabControl = TabControlok[i];
                    TabControlok[i].Controls.AddRange((TabPage[])OsszesTabPage[i]);
                }
                //if (alvez != null)
                //{
                //    ArrayList droparray = new ArrayList();
                //    ToolStripMenuItem[] dropok = (ToolStripMenuItem[])alvez.DropItemek[0];
                //    droparray = new ArrayList(dropok);
                //    alvez.SetMenuAlmenuItems(droparray);
                //}
            }
        }
        private TreeNode ujnodegyart(TreeNode node, TreeNode parentnode)
        {
            TreeNode ujnode = new TreeNode();
            ujnode.Text = node.Text;
            ujnode.ToolTipText = node.ToolTipText;
            ujnode.Tag = node.Tag;
            TablainfoTag tag = (TablainfoTag)ujnode.Tag;
            if (tag.Azonositok.Owner == "" && tag.Azonositok.User == "" || tag.Azonositok.User.Contains(FakUserInterface.Alkalmazas) || tag.Azonositok.Owner == FakUserInterface.Alkalmazas )
            {
                if (parentnode != null)
                    parentnode.Nodes.Add(ujnode);
                return ujnode;
            }
            else
                return null;
        }
        public override bool Ceginicializalas(int cegindex)
        {
            szarmazekospartnerinfo=FakUserInterface.GetByAzontip("SZCTPARTNER");
            szarmazekoskiegpartnerinfo=FakUserInterface.GetByAzontip("SZCTVEZIRPARTNER");
            partnergyokerinfo = UserParamTabla.TermCegPluszCegalattiTablainfok["PARTNERGYOKER"];
            FakUserInterface.Select(partnergyokerinfo.Adattabla, FakUserInterface.AktualCegconn,"PARTNERGYOKER", "", "",false);
            if (partnergyokerinfo.Adattabla.Rows.Count == 0)
            {
                DataRow row = partnergyokerinfo.Ujsor();
                FakUserInterface.Rogzit(partnergyokerinfo);
            }
                partnergyokerinfo.ViewSorindex = 0;
//                string gyokerid = partnergyokerinfo.AktIdentity.ToString();
//                FakUserInterface.AdattoltByAktid(partnergyokerinfo);
                FakUserInterface.OsszesAdattoltByParent("PARTNERGYOKER");
            

            alkalmnode.Nodes.Clear();
            beallitasnode.Nodes.Clear();
            userlognode.Nodes.Clear();
            modosithatonodeok.Nodes.Clear();
            cegszintunode.Nodes.Clear();
            if (UserParamTabla.UserHozferJogosultsag != Base.HozferJogosultsag.Semmi)
            {
                TreeView.Nodes.Add(alkalmnode);
                alkalmnode.Nodes.Add(beallitasnode);

                //Tablainfo tabinfo = FakUserInterface.GetBySzintPluszTablanev("U", "UVERSION");
                //ujnodegyart(tabinfo.TablaTag.Node, beallitasnode);
                Tablainfo tabinfo = this.FakUserInterface.GetBySzintPluszTablanev("U", "CEGEK");
                ujnodegyart(tabinfo.TablaTag.Node, beallitasnode);
                tabinfo = this.FakUserInterface.GetBySzintPluszTablanev("U", "KEZELOK");
                ujnodegyart(tabinfo.TablaTag.Node, beallitasnode);
                tabinfo = this.FakUserInterface.GetOsszef("U", "KezeloAlkalm");
                ujnodegyart(tabinfo.TablaTag.Node, beallitasnode);
                tabinfo = this.FakUserInterface.GetBySzintPluszTablanev("U", "RENDSZERGAZDA");
                ujnodegyart(tabinfo.TablaTag.Node, beallitasnode);
                //if (KezeloiSzint != Base.KezSzint.Rendszergazda)
                //{
                    tabinfo = this.FakUserInterface.GetBySzintPluszTablanev("U", "BANKOK");
                    ujnodegyart(tabinfo.TablaTag.Node, beallitasnode);
                //}
                tabinfo = this.FakUserInterface.GetByAzontip("SZUKTARTAL");
                if (tabinfo.DataView.Count != 0)
                {
                    tartalnode = ujnodegyart(tabinfo.TablaTag.Node, null);
                    for (int i = 0; i < tabinfo.TablaTag.ChildTablainfoTagok.Count; i++)
                    {
                        TreeNode node = tabinfo.TablaTag.ChildTablainfoTagok[i].Node;
                        ujnodegyart(node, tartalnode);
                    }
                    modosithatonodeok.Nodes.Add(tartalnode);

                }
                if (modosithatonodeok.Nodes.Count != 0)
                    alkalmnode.Nodes.Add(modosithatonodeok);
                alkalmnode.Nodes.Add(userlognode);
                tabinfo = this.FakUserInterface.GetByAzontip("SZUTUSERLOG");
                tartalnode = ujnodegyart(tabinfo.TablaTag.Node, userlognode);
                tabinfo = this.FakUserInterface.GetByAzontip("SZUTVALTOZASNAPLO");
                tartalnode = ujnodegyart(tabinfo.TablaTag.Node, userlognode);
            }
            Cegindex = cegindex;
            LezartCeg = UserParamTabla.LezartCeg;
            KezeloiSzint = UserParamTabla.AktualKezeloiSzint;
            string kezszint = Convert.ToInt32(KezeloiSzint).ToString();
            HozferJog = UserParamTabla.AktualTermeszetesJogosultsag;
            if (HozferJog != Base.HozferJogosultsag.Semmi) // && ValtozasLekerdez().Count !=0)
                UserParamTabla.Infotolt();
            if (HozferJog == Base.HozferJogosultsag.Semmi || !UserParamTabla.MenuStripLathato && MenuPanel.Controls.Count != 0)
            {
                try
                {
                    TabControl c = (TabControl)MenuPanel.Controls[0];
                    FakUserInterface.RemoveAllControls(MenuPanel);
                }
                catch { };
            }
            menuStrip1.Visible = UserParamTabla.MenuStripLathato;
            for (int i = 0; i < Vezerles.ChildVezerloinfoCollection.Count; i++)
            {
                Vezerloinfo egyvez = Vezerles.ChildVezerloinfoCollection[i];
                egyvez.KezeloiSzint = KezeloiSzint;
                egyvez.HozferJog = HozferJog;
            }
            Datumtol = UserParamTabla.Datumtol;
            Datumig = UserParamTabla.Datumig;
            cegszintunode.Nodes.Clear();
            if (cegszintunode.Parent != null)
                TreeView.Nodes.Remove(cegszintunode);
            if (UserParamTabla.CegSzarmazekosJogosultsag != Base.HozferJogosultsag.Semmi)
            {
                cegszintunode.Text = UserParamTabla.AktualCegInformacio.CegNev;
                Tablainfo tabinfo = FakUserInterface.GetByAzontip("SZCKTARTAL");
                TreeNode tartalnode = ujnodegyart(tabinfo.TablaTag.Node, null);
                for (int i = 0; i < tabinfo.TablaTag.ChildTablainfoTagok.Count; i++)
                {
                    if (tabinfo.TablaTag.ChildTablainfoTagok[i].Azonositok.Jogszintek[(int)KezeloiSzint] != Base.HozferJogosultsag.Semmi)
                    {
                        TreeNode node = tabinfo.TablaTag.ChildTablainfoTagok[i].Node;
                        ujnodegyart(node, tartalnode);
                    }
                }
                if (tartalnode.Nodes.Count != 0)
                    cegszintunode.Nodes.Add(tartalnode);
                tabinfo = FakUserInterface.GetByAzontip("SZCTTARTAL");
                tartalnode = ujnodegyart(tabinfo.TablaTag.Node, null);
                for (int i = 0; i < tabinfo.TablaTag.ChildTablainfoTagok.Count; i++)
                {
                    Tablainfo egyinfo = tabinfo.TablaTag.ChildTablainfoTagok[i].Tablainfo;
                    string nev = egyinfo.Tablanev;
                    bool kell=true;
                    if(nev.StartsWith("PARTNER") && !UserParamTabla.Lehetpartner)
                        kell=false;
                    if (kell)
                    {
                        if (tabinfo.TablaTag.ChildTablainfoTagok[i].Azonositok.Jogszintek[(int)KezeloiSzint] != Base.HozferJogosultsag.Semmi)
                        {
                            TreeNode node = tabinfo.TablaTag.ChildTablainfoTagok[i].Node;
                            if (node.Text != "Kiajánlások" && node.Text != "Cégverziók")
                                ujnodegyart(node, tartalnode);
                        }
                    }
                }
                if (tartalnode.Nodes.Count != 0)
                    cegszintunode.Nodes.Add(tartalnode);
                tabinfo = FakUserInterface.GetByAzontip("SZCOTARTAL");
                tartalnode = ujnodegyart(tabinfo.TablaTag.Node, null);
                for (int i = 0; i < tabinfo.TablaTag.Node.Nodes.Count; i++)
                {
                    if (tabinfo.TablaTag.ChildTablainfoTagok[i].Azonositok.Jogszintek[(int)KezeloiSzint] != Base.HozferJogosultsag.Semmi)
                    {
                        TreeNode node = tabinfo.TablaTag.ChildTablainfoTagok[i].Node;
                        ujnodegyart(node, tartalnode);
                    }
                }
                if (tartalnode.Nodes.Count != 0)
                    cegszintunode.Nodes.Add(tartalnode);
                tabinfo = FakUserInterface.GetByAzontip("SZCCTARTAL");
                tartalnode = ujnodegyart(tabinfo.TablaTag.Node, null);
                for (int i = 0; i < tabinfo.TablaTag.ChildTablainfoTagok.Count; i++)
                {
                    if (tabinfo.TablaTag.ChildTablainfoTagok[i].Azonositok.Jogszintek[(int)KezeloiSzint] != Base.HozferJogosultsag.Semmi)
                    {
                        TreeNode node = tabinfo.TablaTag.ChildTablainfoTagok[i].Node; //Node.Nodes[i];
                        ujnodegyart(node, tartalnode);
                    }
                }
                if (tartalnode.Nodes.Count != 0)
                    cegszintunode.Nodes.Add(tartalnode);
                tabinfo = FakUserInterface.GetByAzontip("SZCSTARTAL");
                tartalnode = ujnodegyart(tabinfo.TablaTag.Node, null);
                for (int i = 0; i < tabinfo.TablaTag.ChildTablainfoTagok.Count; i++)
                {
                    if (tabinfo.TablaTag.ChildTablainfoTagok[i].Azonositok.Jogszintek[(int)KezeloiSzint] != Base.HozferJogosultsag.Semmi)
                    {
                        TreeNode node = tabinfo.TablaTag.ChildTablainfoTagok[i].Node;
                        ujnodegyart(node, tartalnode);
                    }
                }
                if (tartalnode.Nodes.Count != 0)
                    cegszintunode.Nodes.Add(tartalnode);
                tabinfo = FakUserInterface.GetByAzontip("SZCNTARTAL");
                tartalnode = ujnodegyart(tabinfo.TablaTag.Node, null);
                for (int i = 0; i < tabinfo.TablaTag.ChildTablainfoTagok.Count; i++)
                {
                    if (tabinfo.TablaTag.ChildTablainfoTagok[i].Azonositok.Jogszintek[(int)KezeloiSzint] != Base.HozferJogosultsag.Semmi)
                    {
                        TreeNode node = tabinfo.TablaTag.ChildTablainfoTagok[i].Node;
                        ujnodegyart(node, tartalnode);
                    }
                }
                if (tartalnode.Nodes.Count != 0)
                    cegszintunode.Nodes.Add(tartalnode);
                if (cegszintunode.Nodes.Count != 0)
                    TreeView.Nodes.Add(cegszintunode);
            }
            if (TreeView.Nodes.Count == 0)
                TreeView.Parent.Visible = false;
            else
                TreeView.Parent.Visible = true;
            if (UserParamTabla.UserParamok != "" && UserParamTabla.AktualVezerloControlNev == this.Name)
            {
                if (UserParamTabla.AktualControlNev != "")
                {
                    TabControl_Selecting(UserParamTabla.AktualMenuItemIndex, UserParamTabla.AktualDropItemIndex);
                    UserParamTabla.UserParamok = "";
                }
            }
            //for (int i = 0; i < Vezerles.ChildVezerloinfoCollection.Count; i++)
            //{
            //    Vezerloinfo alvez = Vezerles.ChildVezerloinfoCollection[i];
            //    ArrayList droparray = new ArrayList();
            //    ToolStripMenuItem[] dropok = (ToolStripMenuItem[])alvez.DropItemek[0];
            //    droparray = new ArrayList(dropok);
            //    alvez.SetMenuAlmenuItems(droparray);
            //}
            return true;
        }
        public void MenuBeallitasok()
        {
            for (int i = 0; i < Vezerles.MenuItemek.Length; i++)
            {
                ToolStripMenuItem[] dropok = (ToolStripMenuItem[])Vezerles.DropItemek[i];
                ArrayList droparray = new ArrayList(dropok);
                Vezerles.ChildVezerloinfoCollection[i].SetMenuAlmenuItems(droparray);
                for (int j = 0; j < dropok.Length; j++)
                    dropok[j].Enabled = false;
                for (int j = 0; j < dropok.Length; j++)
                {
                    if (!dropok[j].Enabled)
                        MenuBeallitasok(i, j);
                }
            }
        }
        public void MenuBeallitasok(int menuindex, int dropindex)
        {
            {
                bool[] allapotok = UserParamTabla.Allapotok;
                Vezerloinfo alvez = Vezerles.ChildVezerloinfoCollection[menuindex];
                ToolStripMenuItem[] dropok = (ToolStripMenuItem[])alvez.DropItemek[0];
                bool[] eredetienab = (bool[])alvez.EredetiDropEnablek[0];
                if (!alvez.Name.Contains("Formvezerles"))
                {
                    ArrayList ar = (ArrayList)Vezerles.LekerdezendoAllapotNevek[menuindex];
                    dropok[dropindex].Enabled = false;
                    bool enab = false;
                    if (eredetienab[dropindex])
                    {
                        string[] egylek = (string[])alvez.LekerdezendoAllapotNevek[dropindex];
                        enab = true;
                        for (int l = 0; l < egylek.Length; l++)
                        {
                            int allind = UserParamTabla.UserAllapotNevek.IndexOf(egylek[l]);
                            if (allind != -1 && !allapotok[allind])
                            {
                                enab = false;
                                break;
                            }
                        }
                    }
                    dropok[dropindex].Enabled = enab;
                }
            }
        }
        public override void TabControl_Deselecting(object sender, TabControlCancelEventArgs e)
        {
        }
        public override void TabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (!FakUserInterface.EventTilt && e.TabPage != null)
            {
                {
                    DropItemek = (ToolStripMenuItem[])OsszesDropItem[AktivMenuindex];
                    if (!DropItemek[e.TabPageIndex].Enabled)
                    {
                        e.Cancel = true;
                        if (AktivControl != null)
                            AktivControl.Focus();
                        return;
                    }
                }
                if (AktivControl != null && Userabortkerdes())
                {
                    e.Cancel = true;
                    if (AktivControl != null)
                        AktivControl.Focus();
                    return;
                }
                if (AktivControl != null && AktivControl.Name.Contains("kiegyenl"))
                    UserParamTabla.Infotoltkell = true;
                if (!TabControl_Selecting(AktivMenuindex, e.TabPageIndex))
                    e.Cancel = true;
                if (AktivControl != null)
                    AktivControl.Focus();
            }
        }
        public void ComboSzures(string contnev, ComboBox combo)
        {
            FakUserInterface.ForceAdattolt(szarmazekospartnerinfo, true);
            FakUserInterface.ForceAdattolt(szarmazekoskiegpartnerinfo, true);
            string feltmezo;
            if (contnev == "Bevszla")
                feltmezo="BEVPARTNER";
            else
                feltmezo="KOLTSPARTNER";
            {
                ArrayList partnerarszov = new ArrayList();
                for (int i = 0; i < szarmazekospartnerinfo.DataView.Count; i++)
                {
                    DataRow row = szarmazekospartnerinfo.DataView[i].Row;
                    string id = row["PARTNER_ID"].ToString();
                    szarmazekoskiegpartnerinfo.DataView.RowFilter="PARTNER_ID = "+id;
                    DataRow kiegrow = szarmazekoskiegpartnerinfo.DataView[0].Row;
                    if (kiegrow[feltmezo].ToString() == "I")
                        partnerarszov.Add(row["SZOVEG"].ToString());
                }
                szarmazekoskiegpartnerinfo.DataView.RowFilter="";
                string[] szovidk = (string[])partnerarszov.ToArray(typeof(string));
                string elozo = combo.Text;
                FakUserInterface.Comboinfoszures(combo, szovidk,true);
                if (elozo == "" || combo.Items.IndexOf(elozo) == -1)
                {
                    FakUserInterface.EventTilt = true;
                    combo.SelectedIndex = 0;
                    combo.Text = combo.Items[combo.SelectedIndex].ToString();
                    FakUserInterface.EventTilt = false;
                }
                else if (combo.Text != "")
                    combo.SelectedIndex = combo.Items.IndexOf(combo.Text);

            }
//            szarmazekospartnerinfo.DataView.Sort = "SORREND";

        }
        public virtual bool TabControl_Selecting(int menuindex, int dropindex)
        {
            bool valtozas = false;
            AktivVezerles = Vezerles.ChildVezerloinfoCollection[menuindex];
            string contnev = AktivVezerles.ControlNevek[dropindex].ToString();
            ToolStripItem[] dropok = (ToolStripItem[])AktivVezerles.DropItemek[0];
            if (!dropok[dropindex].Enabled)
                return false;
            if (contnev!="Bevetelek" && contnev!="Kiadasok" && contnev != "Beflenbevszla" && contnev != "Beflenkoltsszla" && AktivVezerles.Parameterez[dropindex] != null && AktivVezerles.Parameterez[dropindex].Listae)
            {
                if (UserParamTabla.KellZaras)
                    Zaras();
            }
            AktivControl = AktivVezerles.LetezoControlok[dropindex];
            if (AktivControl != null)
                valtozas = AktivControl.ValtozasLekerdez().Count != 0;
            else
            {
                UjControlInicializalas(contnev, Vezerles, menuindex, dropindex);
                if (contnev == "Bevszla")
                    bevpartnercombo = ((Bevszla)AktivControl).partner;
                else if (contnev == "Koltsszla")
                    koltspartnercombo = ((Koltsszla)AktivControl).partner;
                valtozas = AktivControl.ValtozasLekerdez().Count != 0;
            }
            TabControl = TabControlok[menuindex];
            if (MenuPanel.Controls.Count == 0)
                MenuPanel.Controls.Add(TabControl);
            else if (MenuPanel.Controls[0] != TabControl)
            {
                FakUserInterface.RemoveAllControls(MenuPanel);
                MenuPanel.Controls.Add(TabControl);
            }
            if (AktivControl.ValtozasLekerdez().Count == 0)
                AktivControl.SajatJelzesBeallit();
            if (menuindex != AktivMenuindex || dropindex != AktivDropindex)
                valtozas = true;
            AktivMenuindex = menuindex;
            AktivDropindex = dropindex;
            AktivPage = AktivControl.AktivPage;
            AktivMenuItem = AktivControl.AktivMenuItem;
            AktivDropDownItem = AktivControl.AktivDropDownItem;
            if (valtozas || AktivControl.ValtozasLekerdez().Count != 0)
            {
                if (AktivControl.Parameterez == null)
                {
                    if (FakUserInterface.KellSzamitasDatum)
                        FakUserInterface.SetUserSzamitasokKellSzamitasDatum(false);
                    UserParamTabla.Infotolt();
                }
                else
                    AktivControl.ValtozasBeallit("AktivValtozas");
                AktivControl.Visible = false;
                AktivControl.TabStop = false;
                AktivControl.TabStop = true;
                if (AktivPage.Controls.Count == 0)
                {
                    if (AktivControl.Parameterez == null)
                        AktivPage.Controls.Add(AktivControl);
                    else
                        AktivPage.Controls.Add(AktivControl.Parameterez);
                }
            }
            AktivControl.Visible = false;
            ValtozasTorol();
            if (AktivControl.Parameterez != null)
            {
                FakPlusz.Alapcontrolok.Parameterez param = AktivControl.Parameterez;
                if (AktivControl.Paramfajta != param.Paramfajta)
                {
                    AktivControl.Paramfajta = param.Paramfajta;
                }
            }
            if (AktivControl.Parameterez == null || AktivControl.ValtozasLekerdez().Count == 0)
                WriteLoginfo();
            AktivControl.Dock = DockStyle.Fill;
            AktivControl.Visible = true;
            FakUserInterface.EventTilt = true;
            TabControl.SelectedTab = AktivPage;
            FakUserInterface.EventTilt = false;
            AktivControl.Focus();
            return true;
        }
        public void Zaras()
        {
            DataTable dt = new DataTable();
            DataTable ceghoinfotabla = new DataTable("CEGSZLAHONAPOK");
            MainControl.ValtozasBeallit("Datumvaltozas");
            string[] gyujtotablanevek = new string[] { "AFAEGYENLEG", "BEVETELKIADAS", "EGYENLEG", "NYITOZARO", "PENZMOZGASOK" };
            TablainfoCollection gyujtotablainfok = new TablainfoCollection();
            for (int i = 0; i < gyujtotablanevek.Length; i++)
                gyujtotablainfok.Add(TermCegPluszCegalattiTabinfok[gyujtotablanevek[i]]);
            Tablainfo nyitozaroinfo = gyujtotablainfok["NYITOZARO"];
            Tablainfo afaegyenleginfo = gyujtotablainfok["AFAEGYENLEG"];
            Tablainfo bevkiadinfo = gyujtotablainfok["BEVETELKIADAS"];
            Tablainfo egyenleginfo = gyujtotablainfok["EGYENLEG"];
            Tablainfo penzmozgasok = gyujtotablainfok["PENZMOZGASOK"];
            Tablainfo bevszlainfo = TermCegPluszCegalattiTabinfok["BEVSZLA"];
            Tablainfo bevszlatetel = TermCegPluszCegalattiTabinfok["BEVSZLA_TETEL"];
            Tablainfo koltsszlainfo = TermCegPluszCegalattiTabinfok["KOLTSSZLA"];
            Tablainfo koltsszlatetel = TermCegPluszCegalattiTabinfok["KOLTSSZLA_TETEL"];
            Tablainfo bankbolbankba = TermCegPluszCegalattiTabinfok["BANKBOLBANKBA"];
            Tablainfo bankbolpenztarba = TermCegPluszCegalattiTabinfok["BANKBOLPENZTARBA"];
            Tablainfo bankimozgas = TermCegPluszCegalattiTabinfok["BANKIMOZGAS"];
            Tablainfo bankimozgastetel = TermCegPluszCegalattiTabinfok["BANKIMOZGAS_TETEL"];
            Tablainfo penztarbolbankba = TermCegPluszCegalattiTabinfok["PENZTARBOLBANKBA"];
            Tablainfo penztarbolpenztarba = TermCegPluszCegalattiTabinfok["PENZTARBOLPENZTARBA"];
            Tablainfo penztarmozgas = TermCegPluszCegalattiTabinfok["PENZTARMOZGAS"];
            TablainfoCollection tetelespenzmozgasok = new TablainfoCollection();
            tetelespenzmozgasok.Add(bankbolbankba);
            tetelespenzmozgasok.Add(bankbolpenztarba);
            tetelespenzmozgasok.Add(bankimozgas);
            tetelespenzmozgasok.Add(penztarbolpenztarba);
            tetelespenzmozgasok.Add(penztarbolbankba);
            tetelespenzmozgasok.Add(penztarmozgas);
            string[] bevetelnevek = new string[] {"BEVETEL1","BEVETEL2","BEVETEL3","BEVETEL4","BEVETEL5",
                "BEVETEL6","BEVETEL7","BEVETEL8","BEVETEL9","BEVETEL10","BEVETEL11","BEVETEL12"};
            string[] kiadasnevek = new string[]{"KIADAS1","KIADAS2","KIADAS3","KIADAS4","KIADAS5","KIADAS6",
                "KIADAS7","KIADAS8","KIADAS9","KIADAS10","KIADAS11","KIADAS12"};
            string[] eredmnevek = new string[] { "EREDM1", "EREDM2", "EREDM3", "EREDM4", "EREDM5", "EREDM6",
                "EREDM7", "EREDM8", "EREDM9", "EREDM10", "EREDM11", "EREDM12" };
            int maxev = UserParamTabla.AktualisDatum.Year;
            int induloev = maxev;
            string elozocegid = "";
            string elozoev = "";
            DataRow penzmozgasdr;
            if (UserParamTabla.NemLezartEvek.Count != 1)
                induloev = Convert.ToInt32(UserParamTabla.NemLezartEvek[0].ToString());
            if (UserParamTabla.LezartEvek.Count != 0)
            {
                elozoev = UserParamTabla.LezartEvek[UserParamTabla.LezartEvek.Count - 1].ToString();
                elozocegid = FakUserInterface.GetTartal(UserParamTabla.Cegevinfo, "CEGEV_ID", "EV", elozoev)[0];
            }
            FakUserInterface.OpenProgress();
            FakUserInterface.SetProgressText("Listaállományok elöállitása");
            UserParamTabla.Infotoltkell = true;
            UserParamTabla.Cegevinfo.DataView.RowFilter = "EV < '" + induloev.ToString() + "'";
            string[] cegidk = new string[UserParamTabla.Cegevinfo.DataView.Count];
            string sel = "";
            for (int i = 0; i < cegidk.Length; i++)
            {
                DataRow dr = UserParamTabla.Cegevinfo.DataView[i].Row;
                cegidk[i] = dr["CEGEV_ID"].ToString();
                if (sel == "")
                    sel = " where ";
                else
                    sel = " or ";
                sel += "CEGEV_ID = " + cegidk[i];
            }
            UserParamTabla.Cegevinfo.DataView.RowFilter = "";
            for (int i = 0; i < gyujtotablanevek.Length; i++)
            {
                FakUserInterface.ProgressRefresh();
                string tablanev = gyujtotablanevek[i];
                Tablainfo egyinfo = gyujtotablainfok[i];
                egyinfo.Adattabla.Rows.Clear();
                dt = new DataTable();
                if (cegidk.Length != 0)
                    Sqlinterface.Select(dt, FakUserInterface.AktualCegconn, tablanev, sel, "", false);
                Sqlinterface.StoredProcedureCommand(dt, FakUserInterface.AktualCegconn, tablanev + "_PROC");
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    DataRow dr = dt.Rows[j];
                    DataRow ujsor = egyinfo.Ujsor();
                    for (int k = 0; k < dt.Columns.Count; k++)
                    {
                        string colnev = dt.Columns[k].ColumnName;
                        Cols egycol = egyinfo.TablaColumns[colnev];
                        if (!egycol.IsIdentity)
                            ujsor[colnev] = dr[colnev];
                    }
                }
                if (egyinfo.Modositott)
                    FakUserInterface.Rogzit(egyinfo);
            }
            Tablainfo dolgozok = FakUserInterface.GetBySzintPluszTablanev("C", "DOLOZOK");
            for (int ii = induloev; ii <= maxev; ii++)
            {
                FakUserInterface.ProgressRefresh();
                string aktev = ii.ToString();
                FakUserInterface.Cegadatok(new DateTime[] { Convert.ToDateTime(aktev + ".01.01"), Convert.ToDateTime(aktev + ".12.31") });
                UserParamTabla.AktualisCegIntervallum = FakUserInterface.VerzioInfok["C"].AktIntervallum;
                UserParamTabla.AktualisCegverzioId = FakUserInterface.VerzioInfok["C"].AktVerzioId;
                string aktcegid = FakUserInterface.GetTartal(UserParamTabla.Cegevinfo, "CEGEV_ID", "EV", aktev)[0];
                sel = " where CEGEV_ID = " + aktcegid;
                FakUserInterface.Select(ceghoinfotabla, FakUserInterface.AktualCegconn, "CEGSZLAHONAPOK", sel, "", false);
                if (ii != induloev)
                {
                    elozoev = (ii - 1).ToString();
                    elozocegid = FakUserInterface.GetTartal(UserParamTabla.Cegevinfo, "CEGEV_ID", "EV", elozoev)[0];
                }
                DataRow dr;
                DataRow bevkiaddr;
                string[] bankidk = FakUserInterface.GetTartal(FakUserInterface.GetBySzintPluszTablanev("C", "FOLYOSZAMLAK"), "FOLYOSZLA_ID");
                string[] banknevek = FakUserInterface.GetTartal(FakUserInterface.GetBySzintPluszTablanev("C", "FOLYOSZAMLAK"), "SZOVEG");
                ArrayList bankidar = new ArrayList(bankidk);
                string[] penztaridk = FakUserInterface.GetTartal(FakUserInterface.GetKodtab("C", "Penztarak"), "SORSZAM");
                string[] penztarnevek = FakUserInterface.GetTartal(FakUserInterface.GetKodtab("C", "Penztarak"), "SZOVEG");
                Tablainfo koltskodinfo = FakUserInterface.GetBySzintPluszTablanev("C", "KOLTSEGKOD");
                koltskodinfo.DataView.RowFilter = "DOLGOZO_ID>0";
                ArrayList dolgozoskoltsidk = new ArrayList();
                ArrayList dolgozoidk = new ArrayList();
                for (int i = 0; i < koltskodinfo.DataView.Count; i++)
                {
                    dolgozoskoltsidk.Add(koltskodinfo.DataView[i].Row[koltskodinfo.IdentityColumnName].ToString());
                    dolgozoidk.Add(koltskodinfo.DataView[i].Row["DOLGOZO_ID"].ToString());
                }
                koltskodinfo.DataView.RowFilter = "";
                ArrayList penztaridar = new ArrayList(penztaridk);
                decimal[] elozobankinyito = new decimal[bankidk.Length];
                decimal[] elozopenztarnyito = new decimal[penztaridk.Length];
                if (elozocegid != "")
                {
                    sel = " where CEGEV_ID = " + elozocegid;
                    for (int i = 0; i < nyitozaroinfo.Adattabla.Rows.Count; i++)
                    {
                        dr = nyitozaroinfo.Adattabla.Rows[i];
                        decimal zaro = Convert.ToDecimal(dr["ZARO"].ToString());
                        string id = dr["FOLYOSZAMLA_ID"].ToString();
                        int j;
                        if (id != "" && id != "0")
                        {
                            j = bankidar.IndexOf(id);
                            elozobankinyito[j] = zaro;
                        }
                        else
                        {
                            id = dr["PENZTAR_ID"].ToString();
                            if (id != "" && id != "0")
                            {
                                j = penztaridar.IndexOf(id);
                                elozopenztarnyito[j] = zaro;
                            }
                        }
                    }
                }
                for (int i = 0; i < bankidk.Length; i++)
                {
                    nyitozaroinfo.ViewSorindex = -1;
                    dr = nyitozaroinfo.Adattabla.Ujsor();
                    dr["EGYENLEGEV"] = aktev;
                    dr["CEGEV_ID"] = aktcegid;
                    string id = bankidk[i];
                    dr["FOLYOSZAMLA_ID"] = id;
                    dr["JELOLO"] = 1;
                    dr["NYITO"] = elozobankinyito[i];
                    dr["SZOVEG"] = banknevek[i];
                }
                for (int i = 0; i < penztaridk.Length; i++)
                {
                    nyitozaroinfo.ViewSorindex = -1;
                    dr = nyitozaroinfo.Adattabla.Ujsor();
                    dr["EGYENLEGEV"] = aktev;
                    dr["CEGEV_ID"] = aktcegid;
                    string id = penztaridk[i].ToString();
                    dr["PENZTAR_ID"] = id;
                    dr["JELOLO"] = 2;
                    dr["NYITO"] = elozopenztarnyito[i];
                    dr["SZOVEG"] = penztarnevek[i];
                }
                if (nyitozaroinfo.Modositott)
                    FakUserInterface.UpdateTransaction(new[] { nyitozaroinfo });

                string bevsavfilt = bevszlainfo.DataView.RowFilter;
                string koltssavfilt = koltsszlainfo.DataView.RowFilter;
                string bevsavsort = bevszlainfo.DataView.Sort;
                string koltssavsort = koltsszlainfo.DataView.Sort;
                string bevtetelfilt = bevszlatetel.DataView.RowFilter;
                string bevtetelsort = bevszlatetel.DataView.Sort;
                string koltstetelfilt = koltsszlatetel.DataView.RowFilter;
                string koltstetelsort = koltsszlatetel.DataView.Sort;
                string szlafilter = "";
                string penzmozgassavsort = "";
                string penzmozgasfilter = "";
                string penzmozgassort = "TERMFOCSOP_ID_K,TERMALCSOP_ID_K,TERMCSOP_ID_K,TERMEKKOD_ID_K,KOLTSFOCSOP_ID_K,KOLTSALCSOP_ID_K,KOLTSCSOP_ID_K,KOLTSEGKOD_ID_K";
                string conn = FakUserInterface.AktualCegconn;
                bevszlainfo.Adattabla.Rows.Clear();
                Sqlinterface.Select(bevszlainfo.Adattabla, conn, "BEVSZLA", " where FIZETVE = 'I' and YEAR(KIEGYENL_DATUM) =" + aktev, " order by KIEGYENL_DATUM", false);
                bevszlainfo.Tartalmaktolt();
                koltsszlainfo.Adattabla.Rows.Clear();
                Sqlinterface.Select(koltsszlainfo.Adattabla, conn, "KOLTSSZLA", " where FIZETVE = 'I' and YEAR(KIEGYENL_DATUM) =" + aktev, " order by KIEGYENL_DATUM", false);
                koltsszlainfo.Tartalmaktolt();
                sel = " where YEAR(SZLA_DATUM) = " + aktev;
                string ord = " order by SZLA_DATUM";
                for (int i = 0; i < tetelespenzmozgasok.Count; i++)
                {
                    Tablainfo egyinfo = tetelespenzmozgasok[i];
                    egyinfo.Adattabla.Rows.Clear();
                    Sqlinterface.Select(egyinfo.Adattabla, conn, egyinfo.Tablanev, sel, ord, false);
                    egyinfo.Tartalmaktolt();
                }
                for (int k = 0; k < 12; k++)
                {
                    FakUserInterface.ProgressRefresh();
                    string aktho = (k + 1).ToString();
                    if (aktho.Length == 1)
                        aktho = "0" + aktho;
                    string datumtolstring = aktev + "." + aktho + ".01";
                    string evho = datumtolstring.Substring(0, 7);
                    DateTime datumtol = Convert.ToDateTime(datumtolstring);
                    DateTime datumig = datumtol.AddMonths(1).AddDays(-1);
                    string datumigstring = FakUserInterface.DatumToString(datumig);
                    int hocolind = k;
                    string bevcolnev = bevetelnevek[hocolind];
                    string kiadcolnev = kiadasnevek[hocolind];
                    string eredcolnev = eredmnevek[hocolind];
                    szlafilter = "KIEGYENL_DATUM >='" + datumtolstring + "' AND KIEGYENL_DATUM <='" + datumigstring + "'";
                    bevszlainfo.DataView.RowFilter = szlafilter;
                    bevszlatetel.DataView.Sort = "TERMFOCSOP_ID_K,TERMALCSOP_ID_K,TERMCSOP_ID_K,TERMEKKOD_ID_K";
                    decimal eddigibev = 0;
                    decimal eddigikiad = 0;
                    decimal bev = 0;
                    decimal kiad = 0;
                    string egyenlegid;
                    DataRow bdr;
                    string egyenleg_idnev;
                    string[] egyenlegidk = null;
                    string[] egyenleg_idnevek = null;
                    string[] partnerszovegek = null;
                    object[] osszegek = null;
                    string[] gyujtoosszegnevek = null;
                    string[] bevkiadcolnevek = null;
                    string mozgevho = "";
                    string termekkodid = "";
                    string termcsopid = "";
                    string termalcsopid = "";
                    string termfocsopid = "";
                    string koltsegkodid = "";
                    string koltscsopid = "";
                    string koltsalcsopid = "";
                    string koltsfocsopid = "";
                    string dolgozoid = "";
                    for (int j = 0; j < bevszlainfo.DataView.Count; j++)
                    {
                        bdr = bevszlainfo.DataView[j].Row;
                        egyenlegid = bdr["FOLYOSZAMLA_ID"].ToString();
                        if (egyenlegid != "0")
                            egyenleg_idnev = "FOLYOSZAMLA_ID";
                        else
                        {
                            egyenlegid = bdr["PENZTAR_ID"].ToString();
                            egyenleg_idnev = "PENZTAR_ID";
                        }
                        nyitozaroinfo.DataView.RowFilter = "EGYENLEGEV='" + aktev + "' AND " + egyenleg_idnev + "=" + egyenlegid;
                        if (nyitozaroinfo.DataView.Count == 0)
                        {
                            nyitozaroinfo.ViewSorindex = -1;
                            dr = nyitozaroinfo.Adattabla.Ujsor();
                            dr["EGYENLEGEV"] = aktev;
                            dr["CEGEV_ID"] = aktcegid;
                            dr[egyenleg_idnev] = egyenlegid;
                            FakUserInterface.UpdateTransaction(new Tablainfo[] { nyitozaroinfo });
                            nyitozaroinfo.DataView.RowFilter = "EGYENLEGEV='" + aktev + "' AND " + egyenleg_idnev + "=" + egyenlegid;
                        }
                        string nyitozaroid = nyitozaroinfo.DataView[0].Row["NYITOZARO_ID"].ToString();
                        DataRow egyenlegdr;
                        string partnerid = bdr["PARTNER_ID"].ToString();
                        egyenleginfo.ViewSorindex = -1;
                        egyenlegdr = egyenleginfo.Ujsor();
                        egyenlegdr["NYITOZARO_ID"] = nyitozaroid;
                        egyenlegdr["PARTNER_ID"] = partnerid;
                        egyenlegdr["CEGEV_ID"] = aktcegid;
                        egyenlegdr["MEGNEVEZES"] = bdr["MEGJEGYZES"];
                        string id = bdr["BEVSZLA_ID"].ToString();
                        egyenlegdr[bevcolnev] = Convert.ToDecimal(bdr["OSSZBEVETEL"].ToString());
                        penzmozgasok.ViewSorindex = -1;
                        penzmozgasdr = penzmozgasok.Ujsor();
                        penzmozgasdr["CEGEV_ID"] = aktcegid;
                        penzmozgasdr["CEGHONAP_ID"] = bdr["CEGHONAP_ID"];
                        penzmozgasdr["NYITOZARO_ID"] = nyitozaroid;
                        penzmozgasdr["PARTNER_ID"] = partnerid;
                        penzmozgasdr["PARTNERSZOVEG"] = bdr["PARTNER_ID_K"];
                        penzmozgasdr["SZOVEG"] = bdr["MEGJEGYZES"];
                        penzmozgasdr["BETET"] = bdr["OSSZBEVETEL"];
                        penzmozgasdr["MOZGASDATUM"] = bdr["KIEGYENL_DATUM"];
                        mozgevho = bdr["KIEGYENL_DATUM"].ToString().Substring(0, 7);
                        penzmozgasdr["EVHO"] = mozgevho;
                        bevszlatetel.Rows.Clear();
                        Sqlinterface.Select(bevszlatetel.Adattabla, bevszlatetel.Adattabla.Connection, "BEVSZLA_TETEL", "where BEVSZLA_ID = " + id, "", false);
                        bevszlatetel.DataView.RowFilter = "";
                        for (int i = 0; i < bevszlatetel.DataView.Count; i++)
                        {
                            dr = bevszlatetel.DataView[i].Row;
                            termfocsopid = dr["TERMFOCSOP_ID"].ToString();
                            termalcsopid = dr["TERMALCSOP_ID"].ToString();
                            termcsopid = dr["TERMCSOP_ID"].ToString();
                            termekkodid = dr["TERMEKKOD_ID"].ToString();
                            bev = Convert.ToDecimal(dr["NETTO"].ToString());
                            bevkiadinfo.DataView.RowFilter = "BEVKIADEV = " + aktev + " AND TERMFOCSOP_ID = " + termfocsopid + " AND TERMALCSOP_ID = " + termalcsopid +
                                 " AND TERMCSOP_ID = " + termcsopid + "AND TERMEKKOD_ID = " + termekkodid;
                            int count = bevkiadinfo.DataView.Count;
                            bevkiaddr = null;
                            if (count != 0)
                            {
                                bevkiaddr = bevkiadinfo.DataView[0].Row;
                                eddigibev = Convert.ToDecimal(bevkiaddr[bevcolnev].ToString()) + bev;
                            }
                            bevkiadinfo.DataView.RowFilter = "";
                            if (count == 0)
                            {
                                bevkiadinfo.ViewSorindex = -1;
                                bevkiaddr = bevkiadinfo.Ujsor();
                                bevkiaddr["BEVKIADEV"] = aktev;
                                bevkiaddr["CEGEV_ID"] = aktcegid;
                                bevkiaddr["TERMFOCSOP_ID"] = termfocsopid;
                                bevkiaddr["TERMALCSOP_ID"] = termalcsopid;
                                bevkiaddr["TERMCSOP_ID"] = termcsopid;
                                bevkiaddr["TERMEKKOD_ID"] = termekkodid;
                                eddigibev = bev;
                            }
                            bevkiaddr[bevcolnev] = eddigibev;
                        }
                    }
                    koltsszlainfo.DataView.RowFilter = szlafilter;
                    koltsszlatetel.DataView.Sort = "KOLTSFOCSOP_ID_K,KOLTSALCSOP_ID_K,KOLTSCSOP_ID_K,KOLTSEGKOD_ID_K";
                    for (int j = 0; j < koltsszlainfo.DataView.Count; j++)
                    {
                        bdr = koltsszlainfo.DataView[j].Row;
                        egyenlegid = bdr["FOLYOSZAMLA_ID"].ToString();
                        if (egyenlegid != "0")
                            egyenleg_idnev = "FOLYOSZAMLA_ID";
                        else
                        {
                            egyenlegid = bdr["PENZTAR_ID"].ToString();
                            egyenleg_idnev = "PENZTAR_ID";
                        }
                        nyitozaroinfo.DataView.RowFilter = "EGYENLEGEV='" + aktev + "' AND " + egyenleg_idnev + "=" + egyenlegid;
                        if (nyitozaroinfo.DataView.Count == 0)
                        {
                            nyitozaroinfo.ViewSorindex = -1;
                            dr = nyitozaroinfo.Adattabla.Ujsor();
                            dr["EGYENLEGEV"] = aktev;
                            dr["CEGEV_ID"] = aktcegid;
                            dr[egyenleg_idnev] = egyenlegid;
                            FakUserInterface.UpdateTransaction(new Tablainfo[] { nyitozaroinfo });
                            nyitozaroinfo.DataView.RowFilter = "EGYENLEGEV='" + aktev + "' AND " + egyenleg_idnev + "=" + egyenlegid;
                        }
                        string nyitozaroid = nyitozaroinfo.DataView[0].Row["NYITOZARO_ID"].ToString();
                        string partnerid = bdr["PARTNER_ID"].ToString();
                        DataRow egyenlegdr;
                        egyenleginfo.ViewSorindex = -1;
                        egyenlegdr = egyenleginfo.Ujsor();
                        egyenlegdr["CEGEV_ID"] = aktcegid;
                        egyenlegdr["NYITOZARO_ID"] = nyitozaroid;
                        egyenlegdr["PARTNER_ID"] = partnerid;
                        egyenlegdr["MEGNEVEZES"] = bdr["MEGJEGYZES"];
                        egyenlegdr[kiadcolnev] = Convert.ToDecimal(bdr["OSSZKIADAS"].ToString());
                        penzmozgasok.ViewSorindex = -1;
                        penzmozgasdr = penzmozgasok.Ujsor();
                        penzmozgasdr["CEGEV_ID"] = aktcegid;
                        penzmozgasdr["CEGHONAP_ID"] = bdr["CEGHONAP_ID"];
                        penzmozgasdr["NYITOZARO_ID"] = nyitozaroid;
                        penzmozgasdr["PARTNER_ID"] = partnerid;
                        penzmozgasdr["PARTNERSZOVEG"] = bdr["PARTNER_ID_K"];
                        penzmozgasdr["SZOVEG"] = bdr["MEGJEGYZES"];
                        penzmozgasdr["KIVET"] = bdr["OSSZKIADAS"];
                        penzmozgasdr["MOZGASDATUM"] = bdr["KIEGYENL_DATUM"];
                        mozgevho = bdr["KIEGYENL_DATUM"].ToString().Substring(0, 7);
                        penzmozgasdr["EVHO"] = mozgevho;
                        string id = bdr["KOLTSSZLA_ID"].ToString();
                        koltsszlatetel.Rows.Clear();
                        Sqlinterface.Select(koltsszlatetel.Adattabla, koltsszlatetel.Adattabla.Connection, "KOLTSSZLA_TETEL", "where KOLTSSZLA_ID = " + id, "", false);
                        koltsszlatetel.DataView.RowFilter = "";

                        for (int i = 0; i < koltsszlatetel.DataView.Count; i++)
                        {
                            dr = koltsszlatetel.DataView[i].Row;
                            termfocsopid = dr["TERMFOCSOP_ID"].ToString();
                            termalcsopid = dr["TERMALCSOP_ID"].ToString();
                            termcsopid = dr["TERMCSOP_ID"].ToString();
                            termekkodid = dr["TERMEKKOD_ID"].ToString();
                            koltsfocsopid = dr["KOLTSFOCSOP_ID"].ToString();
                            koltsalcsopid = dr["KOLTSALCSOP_ID"].ToString();
                            koltscsopid = dr["KOLTSCSOP_ID"].ToString();
                            koltsegkodid = dr["KOLTSEGKOD_ID"].ToString();
                            int pos = dolgozoskoltsidk.IndexOf(koltsegkodid);
                            if (pos == -1)
                                dolgozoid = "0";
                            else
                                dolgozoid = dolgozoidk[pos].ToString();
                            kiad = Convert.ToDecimal(dr["NETTO"].ToString());
                            bevkiadinfo.DataView.RowFilter = "BEVKIADEV = " + aktev + " AND TERMFOCSOP_ID = " + termfocsopid + " AND TERMALCSOP_ID = " + termalcsopid +
                                 "AND TERMCSOP_ID = " + termcsopid + "AND TERMEKKOD_ID = " + termekkodid + " AND (KOLTSKOD_ID = 0 OR KOLTSKOD_ID = " + koltsegkodid + ")";
                            int count = bevkiadinfo.DataView.Count;
                            bevkiaddr = null;
                            if (count != 0)
                            {
                                bevkiaddr = bevkiadinfo.DataView[0].Row;
                                bevkiaddr["KOLTSFOCSOP_ID"] = koltsfocsopid;
                                bevkiaddr["KOLTSALCSOP_ID"] = koltsalcsopid;
                                bevkiaddr["KOLTSCSOP_ID"] = koltscsopid;
                                bevkiaddr["KOLTSKOD_ID"] = koltsegkodid;
                                bevkiaddr["DOLGOZO_ID"] = dolgozoid;
                                eddigikiad = Convert.ToDecimal(bevkiaddr[kiadcolnev].ToString()) + kiad;
                            }
                            bevkiadinfo.DataView.RowFilter = "";
                            if (count == 0)
                            {
                                bevkiadinfo.ViewSorindex = -1;
                                bevkiaddr = bevkiadinfo.Ujsor();
                                bevkiaddr["BEVKIADEV"] = aktev;
                                bevkiaddr["CEGEV_ID"] = aktcegid;
                                bevkiaddr["TERMFOCSOP_ID"] = termfocsopid;
                                bevkiaddr["TERMALCSOP_ID"] = termalcsopid;
                                bevkiaddr["TERMCSOP_ID"] = termcsopid;
                                bevkiaddr["TERMEKKOD_ID"] = termekkodid;
                                bevkiaddr["KOLTSFOCSOP_ID"] = koltsfocsopid;
                                bevkiaddr["KOLTSALCSOP_ID"] = koltsalcsopid;
                                bevkiaddr["KOLTSCSOP_ID"] = koltscsopid;
                                bevkiaddr["KOLTSKOD_ID"] = koltsegkodid;
                                bevkiaddr["DOLGOZO_ID"] = dolgozoid;
                                eddigikiad = kiad;
                            }
                            bevkiaddr[kiadcolnev] = eddigikiad;
                        }
                    }
                    eddigibev=0;
                    eddigikiad=0;
                    penzmozgasfilter = "SZLA_DATUM>='" + datumtolstring + "' AND SZLA_DATUM<='" + datumigstring + "' AND (TERMEKKOD_ID <> 0 OR KOLTSEGKOD_ID<>0)";
                    penzmozgassavsort = bankimozgastetel.DataView.Sort;
                    bankimozgastetel.DataView.RowFilter = penzmozgasfilter;
                    for (int i = 0; i < bankimozgastetel.DataView.Count; i++)
                    {
                        dr = bankimozgastetel.DataView[i].Row;
                        termekkodid=dr["TERMEKKOD_ID"].ToString();
                        if (termekkodid != "0")
                        {
                            termfocsopid = dr["TERMFOCSOP_ID"].ToString();
                            termalcsopid = dr["TERMALCSOP_ID"].ToString();
                            termcsopid = dr["TERMCSOP_ID"].ToString();
                            termekkodid = dr["TERMEKKOD_ID"].ToString();
                            koltsfocsopid = dr["KOLTSFOCSOP_ID"].ToString();
                            koltsalcsopid = dr["KOLTSALCSOP_ID"].ToString();
                            koltscsopid = dr["KOLTSCSOP_ID"].ToString();
                            koltsegkodid = dr["KOLTSEGKOD_ID"].ToString();
                            int pos = dolgozoskoltsidk.IndexOf(koltsegkodid);
                            if (pos == -1)
                                dolgozoid = "0";
                            else
                                dolgozoid = dolgozoidk[pos].ToString();
                            kiad = Convert.ToDecimal(dr["KIVET"].ToString());
                            bev=Convert.ToDecimal(dr["BETET"].ToString());
                            bevkiadinfo.DataView.RowFilter = "BEVKIADEV = " + aktev + " AND TERMFOCSOP_ID = " + termfocsopid + " AND TERMALCSOP_ID = " + termalcsopid +
                                    "AND TERMCSOP_ID = " + termcsopid + "AND TERMEKKOD_ID = " + termekkodid + " AND KOLTSKOD_ID = " + koltsegkodid;
                            int count = bevkiadinfo.DataView.Count;
                            bevkiaddr = null;
                            if (count != 0)
                            {
                                bevkiaddr = bevkiadinfo.DataView[0].Row;

                                bevkiaddr["KOLTSFOCSOP_ID"] = koltsfocsopid;
                                bevkiaddr["KOLTSALCSOP_ID"] = koltsalcsopid;
                                bevkiaddr["KOLTSCSOP_ID"] = koltscsopid;
                                bevkiaddr["KOLTSKOD_ID"] = koltsegkodid;
                                bevkiaddr["DOLGOZO_ID"] = dolgozoid;
                                eddigikiad = Convert.ToDecimal(bevkiaddr[kiadcolnev].ToString()) + kiad;
                                eddigibev = Convert.ToDecimal(bevkiaddr[bevcolnev].ToString()) + bev;
                            }
                            bevkiadinfo.DataView.RowFilter = "";
                            if (count == 0)
                            {
                                bevkiadinfo.ViewSorindex = -1;
                                bevkiaddr = bevkiadinfo.Ujsor();
                                bevkiaddr["BEVKIADEV"] = aktev;
                                bevkiaddr["CEGEV_ID"] = aktcegid;
                                bevkiaddr["TERMFOCSOP_ID"] = termfocsopid;
                                bevkiaddr["TERMALCSOP_ID"] = termalcsopid;
                                bevkiaddr["TERMCSOP_ID"] = termcsopid;
                                bevkiaddr["TERMEKKOD_ID"] = termekkodid;
                                bevkiaddr["KOLTSFOCSOP_ID"] = koltsfocsopid;
                                bevkiaddr["KOLTSALCSOP_ID"] = koltsalcsopid;
                                bevkiaddr["KOLTSCSOP_ID"] = koltscsopid;
                                bevkiaddr["KOLTSKOD_ID"] = koltsegkodid;
                                bevkiaddr["DOLGOZO_ID"] = dolgozoid;
                                eddigikiad = kiad;
                                eddigibev = bev;
                            }
                            bevkiaddr[bevcolnev] = eddigibev;
                            bevkiaddr[kiadcolnev] = eddigikiad;
                        }
                    }
                    bankimozgastetel.DataView.RowFilter = "";
                    bankimozgastetel.DataView.Sort = penzmozgassavsort;
                    penzmozgassavsort=bankimozgas.DataView.Sort;
                    bankimozgas.DataView.RowFilter = penzmozgasfilter + " AND TETELSOROK_SZAMA = 0";
                    bankimozgas.DataView.Sort = penzmozgassort;
                    for (int i = 0; i < bankimozgas.DataView.Count; i++)
                    {
                        dr = bankimozgas.DataView[i].Row;
                        termekkodid=dr["TERMEKKOD_ID"].ToString();
                        if (termekkodid != "0")
                        {
                            termfocsopid = dr["TERMFOCSOP_ID"].ToString();
                            termalcsopid = dr["TERMALCSOP_ID"].ToString();
                            termcsopid = dr["TERMCSOP_ID"].ToString();
                            termekkodid = dr["TERMEKKOD_ID"].ToString();
                            koltsfocsopid = dr["KOLTSFOCSOP_ID"].ToString();
                            koltsalcsopid = dr["KOLTSALCSOP_ID"].ToString();
                            koltscsopid = dr["KOLTSCSOP_ID"].ToString();
                            koltsegkodid = dr["KOLTSEGKOD_ID"].ToString();
                            int pos = dolgozoskoltsidk.IndexOf(koltsegkodid);
                            if (pos == -1)
                                dolgozoid = "0";
                            else
                                dolgozoid = dolgozoidk[pos].ToString();
                            kiad = Convert.ToDecimal(dr["KIVET"].ToString());
                            bev=Convert.ToDecimal(dr["BETET"].ToString());
                            bevkiadinfo.DataView.RowFilter = "BEVKIADEV = " + aktev + " AND TERMFOCSOP_ID = " + termfocsopid + " AND TERMALCSOP_ID = " + termalcsopid +
                                    "AND TERMCSOP_ID = " + termcsopid + "AND TERMEKKOD_ID = " + termekkodid + " AND KOLTSKOD_ID = " + koltsegkodid;
                            int count = bevkiadinfo.DataView.Count;
                            bevkiaddr = null;
                            if (count != 0)
                            {
                                bevkiaddr = bevkiadinfo.DataView[0].Row;
                                bevkiaddr["KOLTSFOCSOP_ID"] = koltsfocsopid;
                                bevkiaddr["KOLTSALCSOP_ID"] = koltsalcsopid;
                                bevkiaddr["KOLTSCSOP_ID"] = koltscsopid;
                                bevkiaddr["KOLTSKOD_ID"] = koltsegkodid;
                                bevkiaddr["DOLGOZO_ID"] = dolgozoid;
                                eddigikiad = Convert.ToDecimal(bevkiaddr[kiadcolnev].ToString()) + kiad;
                                eddigibev = Convert.ToDecimal(bevkiaddr[bevcolnev].ToString()) + bev;
                            }
                            bevkiadinfo.DataView.RowFilter = "";
                            if (count == 0)
                            {
                                bevkiadinfo.ViewSorindex = -1;
                                bevkiaddr = bevkiadinfo.Ujsor();
                                bevkiaddr["BEVKIADEV"] = aktev;
                                bevkiaddr["CEGEV_ID"] = aktcegid;
                                bevkiaddr["TERMFOCSOP_ID"] = termfocsopid;
                                bevkiaddr["TERMALCSOP_ID"] = termalcsopid;
                                bevkiaddr["TERMCSOP_ID"] = termcsopid;
                                bevkiaddr["TERMEKKOD_ID"] = termekkodid;
                                bevkiaddr["KOLTSFOCSOP_ID"] = koltsfocsopid;
                                bevkiaddr["KOLTSALCSOP_ID"] = koltsalcsopid;
                                bevkiaddr["KOLTSCSOP_ID"] = koltscsopid;
                                bevkiaddr["KOLTSKOD_ID"] = koltsegkodid;
                                bevkiaddr["DOLGOZO_ID"] = dolgozoid;
                                eddigikiad = kiad;
                                eddigibev = bev;
                            }
                            bevkiaddr[bevcolnev] = eddigibev;
                            bevkiaddr[kiadcolnev] = eddigikiad;
                        }
                    }
                    bankimozgas.DataView.RowFilter = "";
                    bankimozgas.DataView.Sort = penzmozgassavsort;
                    penztarmozgas.DataView.RowFilter = penzmozgasfilter;
                    penzmozgassavsort = penztarmozgas.DataView.Sort;
                    penztarmozgas.DataView.Sort = penzmozgassavsort;
                    for (int i = 0; i < penztarmozgas.DataView.Count; i++)
                    {
                        dr = penztarmozgas.DataView[i].Row;
                        termekkodid = dr["TERMEKKOD_ID"].ToString();
                        if (termekkodid != "0")
                        {
                            termfocsopid = dr["TERMFOCSOP_ID"].ToString();
                            termalcsopid = dr["TERMALCSOP_ID"].ToString();
                            termcsopid = dr["TERMCSOP_ID"].ToString();
                            termekkodid = dr["TERMEKKOD_ID"].ToString();
                            koltsfocsopid = dr["KOLTSFOCSOP_ID"].ToString();
                            koltsalcsopid = dr["KOLTSALCSOP_ID"].ToString();
                            koltscsopid = dr["KOLTSCSOP_ID"].ToString();
                            koltsegkodid = dr["KOLTSEGKOD_ID"].ToString();
                            int pos = dolgozoskoltsidk.IndexOf(koltsegkodid);
                            if (pos == -1)
                                dolgozoid = "0";
                            else
                                dolgozoid = dolgozoidk[pos].ToString();
                            kiad = Convert.ToDecimal(dr["KIVET"].ToString());
                            bev = Convert.ToDecimal(dr["BETET"].ToString());
                            bevkiadinfo.DataView.RowFilter = "BEVKIADEV = " + aktev + " AND TERMFOCSOP_ID = " + termfocsopid + " AND TERMALCSOP_ID = " + termalcsopid +
                                    "AND TERMCSOP_ID = " + termcsopid + "AND TERMEKKOD_ID = " + termekkodid + " AND KOLTSKOD_ID = " + koltsegkodid;
                            int count = bevkiadinfo.DataView.Count;
                            bevkiaddr = null;
                            if (count != 0)
                            {
                                bevkiaddr = bevkiadinfo.DataView[0].Row;
                                bevkiaddr["KOLTSFOCSOP_ID"] = koltsfocsopid;
                                bevkiaddr["KOLTSALCSOP_ID"] = koltsalcsopid;
                                bevkiaddr["KOLTSCSOP_ID"] = koltscsopid;
                                bevkiaddr["KOLTSKOD_ID"] = koltsegkodid;
                                bevkiaddr["DOLGOZO_ID"] = dolgozoid;
                                eddigikiad = Convert.ToDecimal(bevkiaddr[kiadcolnev].ToString()) + kiad;
                                eddigibev = Convert.ToDecimal(bevkiaddr[bevcolnev].ToString()) + bev;
                            }
                            bevkiadinfo.DataView.RowFilter = "";
                            if (count == 0)
                            {
                                bevkiadinfo.ViewSorindex = -1;
                                bevkiaddr = bevkiadinfo.Ujsor();
                                bevkiaddr["BEVKIADEV"] = aktev;
                                bevkiaddr["CEGEV_ID"] = aktcegid;
                                bevkiaddr["TERMFOCSOP_ID"] = termfocsopid;
                                bevkiaddr["TERMALCSOP_ID"] = termalcsopid;
                                bevkiaddr["TERMCSOP_ID"] = termcsopid;
                                bevkiaddr["TERMEKKOD_ID"] = termekkodid;
                                bevkiaddr["KOLTSFOCSOP_ID"] = koltsfocsopid;
                                bevkiaddr["KOLTSALCSOP_ID"] = koltsalcsopid;
                                bevkiaddr["KOLTSCSOP_ID"] = koltscsopid;
                                bevkiaddr["KOLTSKOD_ID"] = koltsegkodid;
                                bevkiaddr["DOLGOZO_ID"] = dolgozoid;
                                eddigikiad = kiad;
                                eddigibev = bev;
                            }
                            bevkiaddr[bevcolnev] = eddigibev;
                            bevkiaddr[kiadcolnev] = eddigikiad;
                        }
                    }
                    penztarmozgas.DataView.RowFilter = "";
                    penztarmozgas.DataView.Sort = penzmozgassavsort;
                    string datumfilter = "SZLA_DATUM>='" + datumtolstring + "' AND SZLA_DATUM<='" + datumigstring + "'";
                    for (int i = 0; i < tetelespenzmozgasok.Count; i++)
                    {
                        Tablainfo egyinfo = tetelespenzmozgasok[i];
                        egyinfo.DataView.RowFilter = datumfilter;
                        for (int j = 0; j < egyinfo.DataView.Count; j++)
                        {
                            bdr = egyinfo.DataView[j].Row;
                            switch (egyinfo.Tablanev)
                            {
                                case "BANKBOLBANKBA":
                                    egyenlegidk = new string[] { bdr["FOLYOSZAMLA_BOL"].ToString(), bdr["FOLYOSZAMLA_BA"].ToString() };
                                    egyenleg_idnevek = new string[] { "FOLYOSZAMLA_ID", "FOLYOSZAMLA_ID" };
                                    partnerszovegek = new string[] { bdr["FOLYOSZAMLA_BA_K"].ToString() + "-ba", bdr["FOLYOSZAMLA_BOL_K"].ToString() + "-ból" };
                                    osszegek = new object[] { bdr["KIVET"], bdr["KIVET"] };
                                    gyujtoosszegnevek = new string[] { "KIVET", "BETET" };
                                    bevkiadcolnevek = new string[] { kiadcolnev, bevcolnev };
                                    break;
                                case "BANKBOLPENZTARBA":
                                    egyenlegidk = new string[] { bdr["FOLYOSZAMLA_BOL"].ToString(), bdr["PENZTAR_BA"].ToString() };
                                    egyenleg_idnevek = new string[] { "FOLYOSZAMLA_ID", "PENZTAR_ID" };
                                    partnerszovegek = new string[] { bdr["FOLYOSZAMLA_BOL_K"].ToString() + "-ba", bdr["PENZTAR_BA_K"].ToString() + "-ból" };
                                    osszegek = new object[] { bdr["KIVET"], bdr["KIVET"] };
                                    gyujtoosszegnevek = new string[] { "KIVET", "BETET" };
                                    bevkiadcolnevek = new string[] { kiadcolnev, bevcolnev };
                                    break;
                                case "BANKIMOZGAS":
                                    bool betet = Convert.ToDecimal(bdr["BETET"].ToString()) != 0;
                                    egyenlegidk = new string[] {bdr["FOLYOSZAMLA_ID"].ToString() };
                                    egyenleg_idnevek = new string[] { "FOLYOSZAMLA_ID" };
                                    if (betet)
                                    {
                                        string termek = bdr["TERMEKKOD_ID_K"].ToString();
                                        if (termek != "" && termek!="0")
                                            partnerszovegek = new string[] { "Term.kód = " + termek};
                                        else
                                            partnerszovegek = new string[] { bdr["FOLYOSZAMLA_ID_K"].ToString() + "-ba" };
                                        osszegek = new object[] { bdr["BETET"] };
                                        gyujtoosszegnevek = new string[] { "BETET" };
                                        bevkiadcolnevek = new string[] { bevcolnev };
                                    }
                                    else
                                    {
                                        string koltseg = bdr["KOLTSEGKOD_ID_K"].ToString();
                                        if (koltseg != "" && koltseg!="0")
                                            partnerszovegek = new string[] { "Költs.kód = " + koltseg };
                                        else
                                            partnerszovegek = new string[] { bdr["FOLYOSZAMLA_ID_K"].ToString() + "-ból" };
                                        osszegek = new object[] { bdr["KIVET"] };
                                        gyujtoosszegnevek = new string[] { "KIVET" };
                                        bevkiadcolnevek = new string[] { kiadcolnev };
                                    }
                                    break;
                                case "PENZTARBOLPENZTARBA":
                                    egyenlegidk = new string[] { bdr["PENZTAR_BOL"].ToString(), bdr["PENZTAR_BA"].ToString() };
                                    egyenleg_idnevek = new string[] { "PENZTAR_ID", "PENZTAR_ID" };
                                    partnerszovegek = new string[] { bdr["PENZTAR_BA_K"].ToString() + "-ba", bdr["PENZTAR_BOL_K"].ToString() + "-ból" };
                                    osszegek = new object[] { bdr["KIVET"], bdr["KIVET"] };
                                    gyujtoosszegnevek = new string[] { "KIVET", "BETET" };
                                    bevkiadcolnevek = new string[] { kiadcolnev, bevcolnev };
                                    break;
                                case "PENZTARBOLBANKBA":
                                    egyenlegidk = new string[] { bdr["PENZTAR_BOL"].ToString(), bdr["FOLYOSZAMLA_BA"].ToString() };
                                    egyenleg_idnevek = new string[] { "PENZTAR_ID", "FOLYOSZAMLA_ID" };
                                    partnerszovegek = new string[] { bdr["FOLYOSZAMLA_BA_K"].ToString() + "-ba", bdr["PENZTAR_BOL_K"].ToString() + "-ból" };
                                    osszegek = new object[] { bdr["KIVET"], bdr["KIVET"] };
                                    gyujtoosszegnevek = new string[] { "KIVET", "BETET" };
                                    bevkiadcolnevek = new string[] { kiadcolnev, bevcolnev };
                                    break;
                                case "PENZTARMOZGAS":
                                    betet = Convert.ToDecimal(bdr["BETET"].ToString()) != 0;
                                    egyenlegidk = new string[] {bdr["PENZTAR_ID"].ToString()};
                                    egyenleg_idnevek = new string[] { "PENZTAR_ID" };
                                    if (betet)
                                    {
                                        string termek = bdr["TERMEKKOD_ID_K"].ToString();
                                        if (termek != "" && termek!="0")
                                            partnerszovegek = new string[] { "Term.kód = " + termek };
                                        else
                                            partnerszovegek = new string[] { bdr["PENZTAR_ID_K"].ToString() + "-ba" };
                                        osszegek = new object[] { bdr["BETET"] };
                                        gyujtoosszegnevek = new string[] { "BETET" };
                                        bevkiadcolnevek = new string[] { bevcolnev };
                                    }
                                    else
                                    {
                                        string koltseg = bdr["KOLTSEGKOD_ID_K"].ToString();
                                        if (koltseg != "" && koltseg!="0")
                                            partnerszovegek = new string[] { "Költs.kód = " + koltseg };
                                        else
                                            partnerszovegek = new string[] { bdr["PENZTAR_ID_K"].ToString() + "-ból" };
                                        osszegek = new object[] { bdr["KIVET"] };
                                        gyujtoosszegnevek = new string[] { "KIVET" };
                                        bevkiadcolnevek = new string[] { kiadcolnev };
                                    }
                                    break;
                            }
                            for (int kk = 0; kk < egyenlegidk.Length; kk++)
                            {
                                nyitozaroinfo.DataView.RowFilter = "EGYENLEGEV='" + aktev + "' AND " + egyenleg_idnevek[kk] + "=" + egyenlegidk[kk];
                                if (nyitozaroinfo.DataView.Count == 0)
                                {
                                    nyitozaroinfo.ViewSorindex = -1;
                                    dr = nyitozaroinfo.Adattabla.Ujsor();
                                    dr["EGYENLEGEV"] = aktev;
                                    dr["CEGEV_ID"] = aktcegid;
                                    dr[egyenleg_idnevek[kk]] = egyenlegidk[kk];
                                    FakUserInterface.UpdateTransaction(new Tablainfo[] { nyitozaroinfo });
                                    nyitozaroinfo.DataView.RowFilter = "EGYENLEGEV='" + aktev + "' AND " + egyenleg_idnevek[kk] + "=" + egyenlegidk[kk];
                                }
                                string nyitozaroid = nyitozaroinfo.DataView[0].Row["NYITOZARO_ID"].ToString();
                                DataRow egyenlegdr;
                                egyenleginfo.ViewSorindex = -1;
                                egyenlegdr = egyenleginfo.Ujsor();
                                egyenlegdr["CEGEV_ID"] = aktcegid;
                                egyenlegdr["NYITOZARO_ID"] = nyitozaroid;
                                egyenlegdr["PARTNER_ID"] = "0";
                                egyenlegdr["MEGNEVEZES"] = bdr["SZOVEG"];
                                egyenlegdr[bevkiadcolnevek[kk]] = osszegek[kk];
                                penzmozgasok.ViewSorindex = -1;
                                penzmozgasdr = penzmozgasok.Ujsor();
                                penzmozgasdr["CEGEV_ID"] = aktcegid;
                                penzmozgasdr["CEGHONAP_ID"] = bdr["CEGHONAP_ID"];
                                penzmozgasdr["NYITOZARO_ID"] = nyitozaroid;
                                penzmozgasdr["PARTNER_ID"] = "0";
                                penzmozgasdr["PARTNERSZOVEG"] = partnerszovegek[kk];
                                penzmozgasdr["SZOVEG"] = bdr["SZOVEG"];
                                penzmozgasdr[gyujtoosszegnevek[kk]] = osszegek[kk];
                                penzmozgasdr["MOZGASDATUM"] = bdr["SZLA_DATUM"];
                                mozgevho = bdr["SZLA_DATUM"].ToString().Substring(0, 7);
                                penzmozgasdr["EVHO"] = mozgevho;
                            }
                        }
                    }
                }
                decimal bevossz = 0;
                decimal kiadossz = 0;
                decimal eredmossz = 0;
                bevkiadinfo.DataView.RowFilter = "BEVKIADEV='" + aktev + "'";
                for (int i = 0; i < bevkiadinfo.DataView.Count; i++)
                {
                    bevossz = 0;
                    kiadossz = 0;
                    eredmossz = 0;
                    dr = bevkiadinfo.DataView[i].Row;
                    for (int j = 0; j < 12; j++)
                    {
                        decimal egybev = Convert.ToDecimal(dr[bevetelnevek[j]].ToString());
                        decimal egykiad = Convert.ToDecimal(dr[kiadasnevek[j]].ToString());
                        decimal egyeredm = egybev - egykiad;
                        dr[eredmnevek[j]] = egyeredm;
                        bevossz += egybev;
                        kiadossz += egykiad;
                        eredmossz += egyeredm;
                    }
                    dr["BEVOSSZ"] = bevossz;
                    dr["KIADOSSZ"] = kiadossz;
                    dr["EREDMOSSZ"] = eredmossz;
                }
                nyitozaroinfo.DataView.RowFilter = "EGYENLEGEV='" + aktev + "'"; ;
                for (int i = 0; i < nyitozaroinfo.DataView.Count; i++)
                {
                    decimal zaro = Convert.ToDecimal(nyitozaroinfo.DataView[i]["NYITO"].ToString());
                    string nyitoid = nyitozaroinfo.DataView[i].Row["NYITOZARO_ID"].ToString();
                    egyenleginfo.DataView.RowFilter = "NYITOZARO_ID=" + nyitoid;
                    if (egyenleginfo.DataView.Count == 0)
                    {
                        egyenleginfo.DataView.RowFilter = "";
                        egyenleginfo.ViewSorindex=-1;
                        dr = egyenleginfo.Ujsor();
                        dr["NYITOZARO_ID"] = nyitoid;
                        dr["CEGEV_ID"] = aktcegid;
                    }
                    else
                    {
                    for (int k = 0; k < egyenleginfo.DataView.Count; k++)
                    {
                        bevossz = 0;
                        kiadossz = 0;
                        eredmossz = 0;
                        dr = egyenleginfo.DataView[k].Row;
                        for (int j = 0; j < 12; j++)
                        {
                            decimal egybev = Convert.ToDecimal(dr[bevetelnevek[j]].ToString());
                            decimal egykiad = Convert.ToDecimal(dr[kiadasnevek[j]].ToString());
                            decimal egyeredm = egybev - egykiad;
                            dr[eredmnevek[j]] = egyeredm;
                            bevossz += egybev;
                            kiadossz += egykiad;
                            eredmossz += egyeredm;
                        }
                        dr["BEVOSSZ"] = bevossz;
                        dr["KIADOSSZ"] = kiadossz;
                        dr["EREDMOSSZ"] = eredmossz;
                        zaro += eredmossz;
                    }
                    nyitozaroinfo.DataView[i].Row["ZARO"] = zaro;
                    nyitozaroinfo.DataView[i].Row["MODOSITOTT_M"] = 1;
                    nyitozaroinfo.Modositott = true;
                    }
                }
                bevszlainfo.Adattabla.Rows.Clear();
                Sqlinterface.Select(bevszlainfo.Adattabla, FakUserInterface.AktualCegconn, "BEVSZLA", " where YEAR(DATUM_TELJ) =" + aktev, " order by DATUM_TELJ", false);
                koltsszlainfo.Adattabla.Rows.Clear();
                Sqlinterface.Select(koltsszlainfo.Adattabla, FakUserInterface.AktualCegconn, "KOLTSSZLA", " where YEAR(DATUM_TELJ) =" + aktev, " order by DATUM_TELJ", false);
                decimal eddigiafa = 0;
                decimal afa = 0;
                for (int k = 0; k < 12; k++)
                {
                    FakUserInterface.ProgressRefresh();
                    string aktho = (k + 1).ToString();
                    if (aktho.Length == 1)
                        aktho = "0" + aktho;
                    string datumtolstring = aktev + "." + aktho + ".01";
                    DateTime datumtol = Convert.ToDateTime(datumtolstring);
                    DateTime datumig = datumtol.AddMonths(1).AddDays(-1);
                    string datumigstring = FakUserInterface.DatumToString(datumig);
                    int hocolind = k;
                    string bevcolnev = bevetelnevek[hocolind];
                    string kiadcolnev = kiadasnevek[hocolind];
                    string eredcolnev = eredmnevek[hocolind];
                    szlafilter = "DATUM_TELJ >='" + datumtolstring + "' AND DATUM_TELJ <='" + datumigstring + "'";
                    bevszlainfo.DataView.RowFilter = szlafilter;
                    for (int j = 0; j < bevszlainfo.DataView.Count; j++)
                    {
                        DataRow bdr = bevszlainfo.DataView[j].Row;
                        string id = bdr["BEVSZLA_ID"].ToString();
                        bevszlatetel.Rows.Clear();
                        Sqlinterface.Select(bevszlatetel.Adattabla, bevszlatetel.Adattabla.Connection, "BEVSZLA_TETEL", "where BEVSZLA_ID = " + id, "", false);
                        bevszlatetel.DataView.RowFilter = "";
                        for (int i = 0; i < bevszlatetel.DataView.Count; i++)
                        {
                            dr = bevszlatetel.DataView[i].Row;
                            afa = Convert.ToDecimal(dr["AFA"].ToString());
                            afaegyenleginfo.DataView.RowFilter = "BEVKIADEV = " + aktev + " AND TERMEKKOD_ID = " + dr["TERMEKKOD_ID"].ToString();
                            bevkiaddr = null;
                            int count = afaegyenleginfo.DataView.Count;
                            if (count != 0)
                            {
                                bevkiaddr = afaegyenleginfo.DataView[0].Row;
                                eddigiafa = Convert.ToDecimal(bevkiaddr[bevcolnev].ToString()) + afa;
                            }
                            afaegyenleginfo.DataView.RowFilter = "";
                            if (count == 0)
                            {
                                afaegyenleginfo.ViewSorindex = -1;
                                bevkiaddr = afaegyenleginfo.Ujsor();
                                bevkiaddr["BEVKIADEV"] = aktev;
                                bevkiaddr["CEGEV_ID"] = aktcegid;
                                bevkiaddr["TERMFOCSOP_ID"] = dr["TERMFOCSOP_ID"];
                                bevkiaddr["TERMALCSOP_ID"] = dr["TERMALCSOP_ID"];
                                bevkiaddr["TERMCSOP_ID"] = dr["TERMCSOP_ID"];
                                bevkiaddr["TERMEKKOD_ID"] = dr["TERMEKKOD_ID"];
                                eddigiafa = afa;
                            }
                            bevkiaddr[bevcolnev] = eddigiafa;
                        }
                    }
                    koltsszlainfo.DataView.RowFilter = szlafilter;
                    for (int j = 0; j < koltsszlainfo.DataView.Count; j++)
                    {
                        DataRow bdr = koltsszlainfo.DataView[j].Row;
                        string id = bdr["KOLTSSZLA_ID"].ToString();
                        koltsszlatetel.Rows.Clear();
                        Sqlinterface.Select(koltsszlatetel.Adattabla, koltsszlatetel.Adattabla.Connection, "KOLTSSZLA_TETEL", "where KOLTSSZLA_ID = " + id, "", false);
                        koltsszlatetel.DataView.RowFilter = "";
                        for (int i = 0; i < koltsszlatetel.DataView.Count; i++)
                        {
                            dr = koltsszlatetel.DataView[i].Row;
                            afa = Convert.ToDecimal(dr["AFA"].ToString());
                            bevkiaddr = null;
                            afaegyenleginfo.DataView.RowFilter = "BEVKIADEV = " + aktev + " AND TERMEKKOD_ID = " + dr["TERMEKKOD_ID"].ToString();
                            int count = afaegyenleginfo.DataView.Count;
                            if (count != 0)
                            {
                                bevkiaddr = afaegyenleginfo.DataView[0].Row;
                                eddigiafa = Convert.ToDecimal(bevkiaddr[kiadcolnev].ToString()) + afa;
                            }
                            afaegyenleginfo.DataView.RowFilter = "";
                            if (count == 0)
                            {
                                afaegyenleginfo.ViewSorindex = -1;
                                bevkiaddr = afaegyenleginfo.Ujsor();
                                bevkiaddr["BEVKIADEV"] = aktev;
                                bevkiaddr["CEGEV_ID"] = aktcegid;
                                bevkiaddr["TERMFOCSOP_ID"] = dr["TERMFOCSOP_ID"];
                                bevkiaddr["TERMALCSOP_ID"] = dr["TERMALCSOP_ID"];
                                bevkiaddr["TERMCSOP_ID"] = dr["TERMCSOP_ID"];
                                bevkiaddr["TERMEKKOD_ID"] = dr["TERMEKKOD_ID"];
                                bevkiaddr["KOLTSKOD_ID"] = dr["KOLTSEGKOD_ID"];
                                eddigiafa = afa;
                            }
                            bevkiaddr[kiadcolnev] = eddigiafa;
                        }
                    }
                    //     }
                    afaegyenleginfo.DataView.RowFilter = "BEVKIADEV='" + aktev + "'";
                    for (int i = 0; i < afaegyenleginfo.DataView.Count; i++)
                    {
                        bevossz = 0;
                        kiadossz = 0;
                        eredmossz = 0;
                        dr = afaegyenleginfo.DataView[i].Row;
                        for (int j = 0; j < 12; j++)
                        {
                            decimal egybev = Convert.ToDecimal(dr[bevetelnevek[j]].ToString());
                            decimal egykiad = Convert.ToDecimal(dr[kiadasnevek[j]].ToString());
                            decimal egyeredm = egybev - egykiad;
                            dr[eredmnevek[j]] = egyeredm;
                            bevossz += egybev;
                            kiadossz += egykiad;
                            eredmossz += egyeredm;
                        }
                        dr["BEVOSSZ"] = bevossz;
                        dr["KIADOSSZ"] = kiadossz;
                        dr["EREDMOSSZ"] = eredmossz;
                    }
                    bevszlainfo.DataView.RowFilter = bevsavfilt;
                    koltsszlainfo.DataView.RowFilter = koltssavfilt;
                    bevszlainfo.DataView.Sort = bevsavsort;
                    koltsszlainfo.DataView.Sort = koltssavsort;
                    bevszlatetel.DataView.RowFilter = bevtetelfilt;
                    bevszlatetel.DataView.Sort = bevtetelsort;
                    koltsszlatetel.DataView.RowFilter = koltstetelfilt;
                    koltsszlatetel.DataView.Sort = koltstetelsort;
                    bankbolbankba.DataView.RowFilter = "";
                    bankbolpenztarba.DataView.RowFilter = "";
                    bankimozgas.DataView.RowFilter = "";
                    penztarbolbankba.DataView.RowFilter = "";
                    penztarbolpenztarba.DataView.RowFilter = "";
                    penztarmozgas.DataView.RowFilter = "";
                    for (int i = 0; i < gyujtotablainfok.Count; i++)
                        gyujtotablainfok[i].DataView.RowFilter = "";
                    for (int i = 0; i < UserParamTabla.Cegevinfo.DataView.Count; i++)
                    {
                        dr = UserParamTabla.Cegevinfo.DataView[i].Row;
                        dr["KELLZARAS"] = "N";
                        dr["MODOSITOTT_M"] = 1;
                        UserParamTabla.Cegevinfo.Modositott = true;
                    }
                    FakUserInterface.Rogzit(new Tablainfo[] { UserParamTabla.Cegevinfo, afaegyenleginfo, bevkiadinfo, egyenleginfo, nyitozaroinfo, penzmozgasok });
                    UserParamTabla.SetKozosAllapotok();
                }
                FakUserInterface.CloseProgress();
                UserParamTabla.KellZaras = false;
                UserParamTabla.Infotolt();
            }
        }
        public override void SaveUserLogLastRow()
        {
            DataTable userlogtable = new DataTable("USERLOG");
            Sqlinterface.Select(userlogtable, FakUserInterface.Userconn, "USERLOG", "", " order by LAST_MOD DESC", true);
            UserParamTabla.AktualCegInformacio.UserLogsor = userlogtable.Rows[0];
        }
        public override void WriteLoginfo()
        {
            UserParamTabla.AktualVezerloControlNev = this.Name;
            if (AktivControl != null)
                UserParamTabla.AktualControlNev = AktivControl.Name;
            else
                UserParamTabla.AktualControlNev = "";
            UserParamTabla.AktualMenuItemIndex = AktivMenuindex;
            UserParamTabla.AktualDropItemIndex = AktivDropindex;
            UserParamTabla.AktualPageindex = Hivo.AktivPageIndex;
            ListaFilter = "";
            ListaParamok = null;
            int paramint = 0;
            Parameterez parameterez;
            string DatumParamok = "";
            string param = UserParamTabla.AktualControlNev + "," + AktivMenuindex.ToString() + "," + AktivDropindex.ToString();
            if (AktivControl != null && AktivControl.ParamfajtaString.Contains("Nincs"))
                param += ",{" + UserParamTabla.SzamlaDatumtolString + "," + UserParamTabla.SzamlaDatumigString + "}";
            if (AktivControl != null && !AktivControl.ParamfajtaString.Contains("Nincs"))
            {
                UserParamTabla.Datumtol = AktivControl.Datumtol;
                UserParamTabla.Datumig = AktivControl.Datumig;
                DatumParamok = "{" + UserParamTabla.DatumtolString + "," + UserParamTabla.DatumigString;

                parameterez = AktivControl.Parameterez;
                paramint = (int)AktivControl.Paramfajta;
                if (AktivControl.VanDatum)
                {
                    if (AktivControl.VanValasztek)
                    {
                        UserParamTabla.ValasztekIndex = AktivControl.ValasztekIndex;
                        if (DatumParamok == "")
                            DatumParamok = "{";
                        DatumParamok += "," + UserParamTabla.ValasztekIndex.ToString() + "}";
                    }
                    else
                        DatumParamok += "}";
                    param += "," + DatumParamok;
                }
                if (AktivControl.SzurtIdk != null && AktivControl.SzurtIdk.Count != 0)
                    UserParamTabla.SzurtIdk = (string[])AktivControl.SzurtIdk.ToArray(typeof(string));
                string[] paramok = UserParamTabla.SzurtIdk;
                if (paramok != null && paramok.Length != 0)
                {
                    param += "/";
                    for (int i = 0; i < paramok.Length; i++)
                    {
                        param += paramok[i];
                        if (i != paramok.Length - 1)
                            param += ",";
                        else
                            param += "/";
                    }
                }
                if (AktivControl.VanOsszetett)
                {
                    param += "[" + UserParamTabla.RadioButtonIndex.ToString();// +",";
                    paramok = UserParamTabla.OsszetettKozepsoParamok;
                    if (paramok != null)
                    {
                        param += ",";
                        for (int i = 0; i < paramok.Length; i++)
                        {
                            param += paramok[i];
                            if (i != paramok.Length - 1)
                                param += ",";
                            else
                                param += "]";
                        }
                    }
                    else
                        param += "]";
                }

            }
            FakUserInterface.WriteLogInfo(this.Name, paramint, param);
            SaveUserLogLastRow();
        }
        public override Base SetAktivControl(TablainfoTag tabinfotag,Vezerloinfo vezerles)
        {
            Base control = null;
            Tablainfo tabinfo = tabinfotag.Tablainfo;
            if (tabinfo != null && tabinfo.Tablanev == "PARTNER")
            {
                if (kulonpartner == null)
                    kulonpartner = new Partner(FakUserInterface, this, vezerles);

                control = kulonpartner;
                int contindex = vezerles.OsszesControlNev.IndexOf("Partner");
                vezerles.OsszesLetezoControl[contindex] = control;
            }
            return control;
        }
        public override void AltalanosInit()
        {
            Base control = AktivControl.AktivControl;
            Tabinfo = control.TablainfoTag.Tablainfo;
            //Tablainfo vezir
            if (Tabinfo.Tablanev == "KOLTSEGCSOPORT")
                Tabinfo.DataView.RowFilter = "";
            else if (Tabinfo.Tablanev == "CEGSZERZODES")
            {
                Cols egycol = Tabinfo.TablaColumns["INDULODATUM"];
                egycol.Csakolvas = false;
                if (UserParamTabla.Ceghonapinfo.Adattabla.Rows.Count > 1)
                    egycol.Csakolvas = true;
                else
                {
                    if (VanMarTermadat())
                        egycol.Csakolvas = true;
                }
                ArrayList har = new ArrayList(AktivControl.AktivControl.hibaszov);
                ArrayList var = new ArrayList(AktivControl.AktivControl.valtozott);
                if (egycol.Csakolvas)
                {
                    int i = Tabinfo.InputColumns.IndexOf(egycol);
                    if (i != -1)
                    {
                        Tabinfo.InputColumns.RemoveAt(i);
                        Tabinfo.Inputtabla.Rows.RemoveAt(i);
                        har.RemoveAt(i);
                        var.RemoveAt(i);
                    }
                }
                else if (Tabinfo.InputColumns.IndexOf(egycol) == -1)
                {

                    Tabinfo.InputColumns.Insert(1, egycol);
                    DataRow ujsor = Tabinfo.Inputtabla.NewRow();
                    ujsor["SZOVEG"] = egycol.Sorszov;
                    ujsor["TARTALOM"] = egycol.OrigTartalom;
                    Tabinfo.Inputtabla.Rows.InsertAt(ujsor, 1);
                    har.Insert(1, "");
                    var.Insert(1, false);
                }
                AktivControl.AktivControl.hibaszov = (string[])har.ToArray(typeof(string));
                AktivControl.AktivControl.valtozott = (bool[])var.ToArray(typeof(bool));
            }
            VerziobuttonokAllit();
        }
        private bool VanMarTermadat()
        {
            string[] tablanevek = new string[] {"BEVSZLA","BEVSZLA_TETEL","KOLTSSZLA","KOLTSSZLA_TETEL","BANKIMOZGAS",
                "PENZTARMOZGAS","BANKBOLBANKBA","BANKBOLPENZTARBA","PENZTARBOLPENZTARBA","PENZTARBOLBANKBA"};
            ArrayList ar = new ArrayList(tablanevek);
            foreach(Tablainfo info in TermCegPluszCegalattiTabinfok)
            {
                if (ar.IndexOf(info.Tablanev)!=-1)
                {
                    DataTable dt = new DataTable();
                    dt = Sqlinterface.Select(dt, FakUserInterface.AktualCegconn, info.Tablanev, "", "", true);
                    if (dt.Rows.Count != 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public override void VerziobuttonokAllit()
        {
            if (AktivControl.AktivControl != null)
            {
                try
                {
                    Alap alap = (Alap)AktivControl.AktivControl;
                    alap.elozoverzio.Enabled = false;
                    alap.kovetkezoverzio.Enabled = false;
                    if (alap.Tabinfo.Tablanev == "CVERSION")
                    {
                        alap.teljestorles.Enabled = true;
                    }
                    else
                        alap.teljestorles.Enabled = false;
                }
                catch { }
            }
            if (Tabinfo == null || Tabinfo.Tablanev != "VEZIRPARTNER")
                return;
 //           lehetszallito = koltspartnercol.Tartalom == "I";
//            lehetvevo = bevpartnercol.Tartalom == "I";
        }
        //public override void ok_Click(object sender, EventArgs e)
        //{
        //    Base cont = (Base)sender;
        //    Tablainfo tabinfo = cont.Tabinfo;
        //    Cols egycol;
        //    DataRow dr;
        //    string colnev;
        //    if (tabinfo.Tablanev == "VEZIRPARTNER")
        //    {
        //        for (int i = 0; i < tabinfo.InputColumns.Count; i++)
        //        {
        //            egycol = tabinfo.InputColumns[i];
        //            colnev = egycol.ColumnName;
        //            dr = tabinfo.Inputtabla.Rows[i];
        //            string ertek = dr[1].ToString();
        //            switch (colnev)
        //            {
        //                case "BEVPARTNER":
        //                    lehetvevo = ertek == "Igen";
        //                    break;
        //                case "KOLTSPARTNER":
        //                    lehetszallito = ertek == "Igen";
        //                    break;
        //                case "TERMEKKOD_ID":
        //                    if (ertek == "" && lehetvevo)
        //                        cont.hibaszov[i] = "Nem lehet üres!";
        //                    break;
        //                case "KOLTSEGKOD_ID":
        //                    if (ertek == "" && lehetszallito)
        //                        cont.hibaszov[i] = "Nem lehet üres!";
        //                    break;
        //            }
        //        }
        //    }
        //    else if (tabinfo.Tablanev == "CVERSION")
        //    {
        //        //DateTime inddate = Convert.ToDateTime(tabinfo.InputColumns[0].Tartalom);
        //        //if (inddate.Month != 1 || inddate.Day != 1)
        //        //    cont.hibaszov[0] = "Csak év elejével indulhat!";
        //    }
        //}
        public override string EgyediHibavizsg(DataGridViewCell dcell, Tablainfo tabinfo)
        {
            if (tabinfo.Tablanev == "VEZIRPARTNER" && bevpartnercol == null)
                AltalanosInit();
            dcell.ErrorText = "";
            Cols egycol = tabinfo.InputColumns[dcell.RowIndex];
            string colnev = egycol.ColumnName;

            string ertek = dcell.Value.ToString();
            if (tabinfo.Tablanev == "VEZIRPARTNER")
            {
                bool kellhiba = false;
               switch (colnev)
                {
                    case "BEVPARTNER":
                        if (ertek != "")
                            lehetvevo = ertek.Substring(0, 1) == "I";
                        lehetszallito = koltspartnercol.ComboAktFileba == "I";
                        kellhiba = true;
                        break;
                    case "KOLTSPARTNER":
                        if (ertek != "")
                            lehetszallito = ertek.Substring(0, 1) == "I";
                        lehetvevo = bevpartnercol.ComboAktFileba == "I";
                        kellhiba = true;
                        break;
                }
                if (!lehetvevo && !lehetszallito)
                {
                    if (kellhiba)
                        return " Se vevö, se szállitó?";
                    else
                        return "";
                }
                string tablanev;
                string szov = "";
                string filter;
                DataTable dt = new DataTable();
                if (tabinfo.AktualViewRow != null && tabinfo.AktualViewRow["PARTNER_ID"].ToString() != "" &&
                    tabinfo.AktualViewRow["PARTNER_ID"].ToString() != "0" && (!lehetvevo || !lehetszallito))
                {
                    filter = "PARTNER_ID=" + tabinfo.AktualViewRow["PARTNER_ID"].ToString();
                    if (!lehetvevo)
                    {
                        tablanev = "BEVSZLA";
                        szov = "Bevételi számlák között már szerepel!";
                    }
                    else
                    {
                        tablanev = "KOLTSSZLA";
                        szov = "Költségszámlák között már szerepel!";
                    }
                    FakPlusz.Sqlinterface.Select(dt, FakUserInterface.AktualCegconn, tablanev, " where " + filter, "", true);
                    if (dt.Rows.Count != 0)
                        return szov;
                }
                if (lehetvevo)
                {
                    if (colnev == "TERMEKKOD_ID" && ertek == "")
                        return "Nem lehet üres!";
                }
                else
                {
                    partnertermekkodcol.ComboAktFileba = "0";
                    partnertermekkodcol.ComboAktSzoveg = "";
                    partnertermekkodcol.Tartalom = "";
                    tabinfo.Inputtabla.Rows[termekkodindex][1] = "";
                    termsemakodcol.ComboAktFileba = "0";
                    termsemakodcol.ComboAktSzoveg = "";
                    termsemakodcol.Tartalom = "";
                    tabinfo.Inputtabla.Rows[termsemakodindex][1] = "";
                }
                if (lehetszallito)
                {
                    if (colnev == "KOLTSEGKOD_ID" && ertek == "")
                        return "Nem lehet üres!";
                }
                else
                {
                    partnerkoltsegkodcol.ComboAktSzoveg = "";
                    partnerkoltsegkodcol.Tartalom = "";
                    tabinfo.Inputtabla.Rows[koltsegkodindex][1] = "";
                    partnersemakodcol.ComboAktFileba = "0";
                    partnersemakodcol.ComboAktSzoveg = "";
                    partnersemakodcol.Tartalom = "";
                    tabinfo.Inputtabla.Rows[semakodindex][1] = "";
                }
            }
            else if (tabinfo.Tablanev == "KOLTSEGCSOPORT" && egycol.ColumnName == "SEMAE" && ertek == "N" && tabinfo.AktualViewRow != null)
            {
                string id = tabinfo.AktualViewRow["KOLTSEGCSOPORT_ID"].ToString();
                Tablainfo partnerinfo = FakUserInterface.GetBySzintPluszTablanev("C", "PARTNER");
                string[] idk = FakUserInterface.GetTartal(partnerinfo, "PARTNER_ID", "SEMA_ID", id);
                if (idk != null)
                {
                    string[] szov = FakUserInterface.GetTartal(partnerinfo, "SZOVEG", "PARTNER_ID", idk);
                    if (szov != null)
                    {
                        dcell.ErrorText = "Az alábbi partnereknél már szerepel:";
                        for (int i = 0; i < szov.Length; i++)
                            dcell.ErrorText += "\n" + szov[i];
                    }
                    return dcell.ErrorText;
                }
            }
            return "";
        }
        public override void RogzitesUtan()
        {
            if (AktivControl.Name == "Formvezerles")
            {
                string tablanev = "PARTNER";
                Tablainfo tabinfo = AktivControl.AktivControl.Tabinfo;
                if (AktivControl.AktivControl.Name != "Partner")
                    tablanev = tabinfo.Tablanev;
                switch (tablanev)
                {
                    case "CVERSION":
                        if (ujcegverzio)
                        {
                            //                            int savid = UserParamTabla.AktualisCegverzioId;
                            FakUserInterface.CreateVersionAll();
                            FakUserInterface.Cegadatok(UserParamTabla.AktualCegInformacio.CegConnection, UserParamTabla.CegNevek[UserParamTabla.AktivCegIndex], new DateTime[] { UserParamTabla.AktualisDatum, UserParamTabla.AktualisDatumig });
                            UserParamTabla.AktualisCegverzioId = FakUserInterface.VerzioInfok["C"].AktVerzioId;
                            UserParamTabla.AktualisCegIntervallum = FakUserInterface.VerzioInfok["C"].AktIntervallum;
                            ujcegverzio = false;
                        }
                        break;
                    case "PARTNER":
                        FakUserInterface.ForceAdattolt(szarmazekospartnerinfo, true);
                        FakUserInterface.ForceAdattolt(szarmazekoskiegpartnerinfo, true);
                        if (bevpartnercombo != null)
                            ComboSzures("Bevszla", bevpartnercombo);
                        if (koltspartnercombo != null)
                            ComboSzures("Koltsszla", koltspartnercombo);
                        break;

                }

            }
            else if (AktivMenuItem.Name != "technikai")
            {
                Tablainfo cegevinfo = UserParamTabla.Cegevinfo;
                int i = UserParamTabla.NemLezartEvek.IndexOf(UserParamTabla.SzamlaDatumtol.Year.ToString());
                if (i != -1)
                {
                    for (int j = i; j < UserParamTabla.NemLezartEvek.Count; j++)
                    {
                        cegevinfo.DataView.RowFilter = "EV = '" + UserParamTabla.NemLezartEvek[j].ToString() + "'";
                        cegevinfo.DataView[0].Row["KELLZARAS"] = "I";
                        cegevinfo.DataView[0].Row["MODOSITOTT_M"] = 1;
                        cegevinfo.Modositott = true;
                    }
                }
                cegevinfo.DataView.RowFilter = "";
                if (cegevinfo.Modositott)
                {
                    FakUserInterface.Rogzit(cegevinfo);
                    UserParamTabla.KellZaras = true;
                }
            }
            else if (AktivControl != null && (AktivControl.Name == "Evzar" || AktivControl.Name == "Evnyit"))
            {
            }
            UserParamTabla.SetKozosAllapotok();
            if (AktivControl.Name != "Formvezerles" && !AktivControl.AktivDropDownItem.Enabled)
            {
                AktivControl.Tabinfo.Modositott = false;
                AktivPage.Controls.Clear();
                AktivControl.Visible = false;
            }
        }

        public override void DropDownItem_Clicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripMenuItem dropitem = (ToolStripMenuItem)e.ClickedItem;
            ToolStripMenuItem owneritem = (ToolStripMenuItem)dropitem.OwnerItem;
            int dropindex = owneritem.DropDownItems.IndexOf(dropitem);
            int menuindex = owneritem.Owner.Items.IndexOf(owneritem);
            TabPage[] pagek = (TabPage[])OsszesTabPage[menuindex];
            bool csakvaltozas = false;
            bool valtozas = false;
            if (AktivControl != null)
            {
                if (Userabortkerdes())
                {
                    AktivControl.Focus();
                    return;
                }
                if (AktivControl.Name == "Szerklist" || AktivControl.Listae || AktivControl.Name.Contains("kiegyenl"))
                    UserParamTabla.Infotoltkell = true;
                if (AktivControl.Name == "Formvezerles")
                {
                    csakvaltozas = true;
                    //int aktverid = FakUserInterface.VerzioInfok["C"].AktVerzioId;
                    //if (AktivControl.AktivControl.Tabinfo.AktVerzioId > aktverid)
                    //    AktivControl.AktivControl.Tabinfo.ElozoVerzio();
                    //if (AktivControl.AktivControl.Tabinfo.AktVerzioId < aktverid)
                    //    AktivControl.AktivControl.Tabinfo.KovetkezoVerzio();
                    if (pagek[dropindex].Controls.Count != 0)
                        AktivControl = (Base)pagek[dropindex].Controls[0];
                    else
                    {
                        Vezerloinfo alvez = Vezerles.ChildVezerloinfoCollection[menuindex];
                        string contname = alvez.ControlNevek[dropindex].ToString();
                        int contindex = Vezerles.LetezoUserControlNevek.IndexOf(contname);
                        AktivControl = Vezerles.LetezoControlok[contindex];
                        pagek[dropindex].Controls.Add(AktivControl);
                    }
                }
            }
            valtozas = ValtozasLekerdez().Count != 0 || AktivControl != null && AktivControl.ValtozasLekerdez().Count != 0;
            if (owneritem == AktivMenuItem && e.ClickedItem == AktivDropDownItem)
            {
                if (!csakvaltozas && !valtozas)
                {
                    ValtozasTorol();
                    AktivControl.Focus();
                    return;
                }
                else if (csakvaltozas || AktivControl == null)
                {
                    valtozas = AktivControl == null || AktivControl.ValtozasLekerdez().Count != 0;
                    FakUserInterface.RemoveAllControls(MenuPanel);
                    if (AktivControl != null && AktivControl.Aktiv || valtozas)
                    {
                        MenuPanel.Controls.Add(TabControlok[menuindex]);
                        if (valtozas && AktivControl != null)
                        {
                            UserParamTabla.Infotolt();
                            AktivControl.Visible = false;
                            AktivControl.TabStop = false;
                            AktivControl.TabStop = true;
                        }
                    }
                    ValtozasTorol();
                    AktivControl.Visible = true;
                    AktivControl.Focus();
                    return;
                }
            }
            if (owneritem != AktivMenuItem)
            {
                FakUserInterface.EventTilt = true;
                FakUserInterface.RemoveAllControls(MenuPanel);
                MenuPanel.Controls.Add(TabControlok[menuindex]);
                FakUserInterface.EventTilt = false;
            }
            TabControl_Selecting(menuindex, dropindex);
        }
        private void UjControlInicializalas(string contnev, Vezerloinfo vez, int menuindex, int dropindex)
        {
            int contind = vez.LetezoUserControlNevek.IndexOf(contnev);
            TabControl = TabControlok[menuindex];
            vez.TabControl = TabControl;
            AktivMenuindex = menuindex;
            AktivDropindex = dropindex;
            MenuItemek = (ToolStripMenuItem[])vez.MenuItemek;
            AktivMenuItem = MenuItemek[menuindex];
            DropItemek = (ToolStripMenuItem[])vez.DropItemek[menuindex];
            AktivDropDownItem = DropItemek[dropindex];
            TabPagek = (TabPage[])vez.TabPagek[menuindex];
            AktivPage = TabPagek[dropindex];
            AktivVezerles = vez.ChildVezerloinfoCollection[menuindex];
            vez.AktivVezerles = AktivVezerles;
            AktivVezerles.TabControl = TabControl;
            AktivVezerles.AktivPage = AktivPage;
            AktivVezerles.AktivMenuItem = AktivMenuItem;
            AktivVezerles.AktivDropDownItem = AktivDropDownItem;
            AktivControl = EgyediIndit(contnev, AktivVezerles);
            vez.AktivControl = AktivControl;
            vez.LetezoControlok[contind] = AktivControl;
            AktivControl.Dock = DockStyle.Fill;
            AktivPage.Controls.Add(AktivControl);
            AktivVezerles.AktivControl = AktivControl;
            AktivVezerles.LetezoControlok[dropindex] = AktivControl;
            vez.AktivControl = AktivControl;
            AktivControl.TabControl = TabControl;
            AktivControl.AktivDropDownItem = AktivDropDownItem;
            AktivControl.AktivMenuItem = AktivMenuItem;
            AktivControl.AktivPage = AktivPage;
            AktivControl.AktivMenuindex = menuindex;
            AktivControl.AktivDropindex = dropindex; 
            AktivControl.AktivVezerles = AktivVezerles;
            contind = AktivVezerles.ControlNevek.IndexOf(AktivControl.ControlNev);
            if (contind != -1)
            {
                Parameterez parameterez = AktivVezerles.Parameterez[contind];
                if (parameterez != null)
                {
                    parameterez.ListaAdatbevPage.Controls.Add(AktivControl);
                    parameterez.AktivControl = AktivControl;
                }
            }

        }
        public override bool Userabortkerdes()
        {
            if (AktivControl == null)
                return false;
            if (AktivControl.Name == "Formvezerles")
            {
                Base cont = AktivControl.AktivControl;
                if (cont != null)
                {
                    try
                    {
                        return cont.Userabortkerdes(cont.Tabinfo);
                    }
                    catch
                    {
                        return cont.Userabortkerdes();
                    }
                }
                else
                    return false;
            }
            else
                return AktivControl.Userabortkerdes();
        }
        public override bool RogzitesElott()
        {
            string idnev = "";
            if (AktivControl.Name == "Formvezerles")
            {
                string hibaszov = "";
                Base control = AktivControl.AktivControl;
                Tabinfo = AktivControl.AktivControl.Tabinfo;
                if (AktivControl.AktivControl.Name=="Partner" || Tabinfo.Tablanev == "PARTNER")
                {
                    Tabinfo = FakUserInterface.GetByAzontip("SZCTPARTNER");
                    partnerszoveglist = new ArrayList();
                    for (int i = 0; i < Tabinfo.DataView.Count; i++)
                    {
                        DataRow row = Tabinfo.DataView[i].Row;
                        if (row.RowState == DataRowState.Added)
                            partnerszoveglist.Add(row["SZOVEG"].ToString());
                    }
                    return true;
                }
                if (control.Name == "Osszef" && Tabinfo.Szint == "C" && Tabinfo.Adatfajta == "O" && Tabinfo.Kodtipus.Contains("Term"))
                {
                    Tablainfo elsoinfo = Tabinfo.Osszefinfo.tabinfo1;
                    Tablainfo masodikinfo = Tabinfo.Osszefinfo.tabinfo2;
                    string[] idk = FakUserInterface.GetTartal(Tabinfo, "SORSZAM2");
                    if (idk != null)
                    {
                        ArrayList ar = new ArrayList(idk);
                        for (int i = 0; i < ar.Count; i++)
                        {
                            if (i < ar.Count - 1)
                            {
                                string egyid = ar[i].ToString();
                                int j = ar.IndexOf(egyid, i + 1);
                                if (j != -1)
                                {
                                    if (hibaszov == "")
                                        hibaszov = "Nem szerepelhet több helyen";
                                    hibaszov += "\n" + FakUserInterface.GetTartal(masodikinfo, masodikinfo.SzovegColName, masodikinfo.IdentityColumnName, egyid)[0];
                                }
                            }
                        }
                        if (hibaszov != "")
                        {
                            FakPlusz.MessageBox.Show(hibaszov);
                            return false;
                        }

                    }
                }
                string[] nevek = new string[] { "KODTAB", "BANKOK", "FOLYOSZAMLAK" };
                string[] kodnevek = new string[] { "KOD", "KOD", "FOLYOSZLASZAM" };
                int[] hosszak = new int[] { 0, 8, 16 };
                int nevind = -1;
                for (int i = 0; i < nevek.Length; i++)
                {
                    if (nevek[i] == Tabinfo.Tablanev)
                    {
                        nevind = i;
                        break;
                    }
                }
                if (nevind != -1)
                {
                    string megnev = nevek[nevind];
                    string kodtipus = Tabinfo.Kodtipus;
                    string kodnev = kodnevek[nevind];
                    int hossz = hosszak[nevind];
                    for (int i = 0; i < Tabinfo.DataView.Count; i++)
                    {
                        string kod = Tabinfo.DataView[i].Row[kodnev].ToString();
                        if (hossz != 0 && kod.Length != hossz)
                        {
                            if (hibaszov != "")
                                hibaszov += "\n";
                            hibaszov += hossz.ToString() + " a kötelezö kódhossz ";
                        }
                        try
                        {
                            Convert.ToInt64(kod);
                        }
                        catch
                        {
                            if (hibaszov != "")
                                hibaszov += "\n";
                            hibaszov += kod + " legyen numerikus";
                            if (kodtipus == "Afa")
                                hibaszov += ", ebböl lesz az áfa értéke";
                            else if (nevind == 0)
                                hibaszov += ", ebböl lenne a felosztási százalék";
                        }

                    }
                    if (hibaszov != "")
                    {
                        FakPlusz.MessageBox.Show(hibaszov);
                        return false;
                    }
                    else if (kodtipus == "Afa")
                    {
                        for (int i = 0; i < Tabinfo.DataView.Count; i++)
                        {
                            DataRow row = Tabinfo.DataView[i].Row;
                            if (row["MODOSITOTT_M"].ToString() == "1" && row.RowState != DataRowState.Added)
                            {
                                string id = row["SORSZAM"].ToString();
                                Tablainfo[] infok = new Tablainfo[] {FakUserInterface.GetBySzintPluszTablanev("C","TERMEKKOD"),
                                    FakUserInterface.GetBySzintPluszTablanev("C","KOLTSEGKOD")};
                                for (int j = 0; j < infok.Length; j++)
                                {
                                    Tablainfo info = infok[j];
                                    string[] szovegek = FakUserInterface.GetTartal(info, "SZOVEG", "AFA_ID", id);
                                    if (szovegek != null)
                                    {
                                        if (hibaszov == "")
                                            hibaszov += "Nem módositható, már szerepel az alábbiakban:\n";
                                        foreach (string egyszov in szovegek)
                                            hibaszov += egyszov + "\n";
                                    }
                                }
                            }
                        }
                        if (hibaszov != "")
                        {
                            FakPlusz.MessageBox.Show(hibaszov);
                            return false;
                        }
                    }
                    return true;
                }
                if (control.Name != "Csoport")
                {
                    Tabinfo = control.Tabinfo;
                    if (Tabinfo.Tablanev == "CEGSZERZODES")
                    {
                        if (Tabinfo.InputColumns.Count == 3)
                        {
                            DataRow dr = Tabinfo.Adattabla.Rows[0];
                            string inddat = dr["INDULODATUM"].ToString();
                            DateTime indulo = Convert.ToDateTime(inddat);
                            string ev = indulo.Year.ToString();
                            string ho = indulo.Month.ToString();
                            if (ho.Length == 1)
                                ho = "0" + ho;
                            inddat = ev + "." + ho + "." + "01";
                            dr["INDULODATUM"] = inddat;
                            dr["AKTUALISDATUM"] = inddat;
                            DataRow evsor;
                            DataRow hosor;
                            UserParamTabla.Ceghonapinfo.DataView.RowFilter = "";
                            evsor = UserParamTabla.Cegevinfo.DataView[0].Row;
                            hosor = UserParamTabla.Ceghonapinfo.DataView[0].Row;
                            UserParamTabla.Cegevinfo.Modositott = true;
                            UserParamTabla.Ceghonapinfo.Modositott = true;
                            evsor["EV"] = ev;
                            evsor["MODOSITOTT_M"] = 1;
                            hosor["EVHONAP"] = ev + "." + ho;
                            hosor["MODOSITOTT_M"] = 1;
                            hosor["SZLA_DATUM"] = inddat;
                            FakUserInterface.Rogzit(new Tablainfo[] { Tabinfo, UserParamTabla.Cegevinfo, UserParamTabla.Ceghonapinfo });
                            UserParamTabla.AktualCegInformacio.InduloDatum = Convert.ToDateTime(inddat);
                            UserParamTabla.AktualCegInformacio.AktualisDatum = Convert.ToDateTime(inddat);
                            UserParamTabla.SetAktualCeginformacio(false, UserParamTabla.AktivCegIndex);
                        }
//                        return true;
                    }
                    else if (Tabinfo.Tablanev == "CVERSION")
                    {
                        if (Tabinfo.DataView[Tabinfo.DataView.Count - 1].Row.RowState == DataRowState.Added)   // uj verzio
                                      ujcegverzio=true;
                    }
                    return true;
                }
                else
                {
                    Tabinfo = control.Tabinfo;
                    Osszefinfo osszef = Tabinfo.Osszefinfo.Osszefinfo1;
                    idnev = "KOLTSEGCSOPORT_ID";
                    if (Tabinfo.Kodtipus == "Feloszt")
                        idnev = "KOLTSEGCSOPORT_ID";
                    else
                        idnev = "SORSZAM";
                    if (Tabinfo.Kodtipus != "Feloszt" && Tabinfo.Kodtipus != "Termfeloszt")
                        return true;
                    DataView semaview = osszef.DataView1;
                    DataView osszefview = osszef.DataView;
                    Tablainfo szazalekok = FakUserInterface.GetKodtab("C", "Fszazal");
                    int osszazalek = 0;
                    for (int i = 0; i < semaview.Count; i++)
                    {
                        string semasorsz = semaview[i].Row[idnev].ToString();
                        osszazalek = 0;
                        string szov = semaview[i].Row["SZOVEG"].ToString();
                        osszefview.RowFilter = "SORSZAM1=" + semasorsz + " AND VALASZT<>''";
                        for (int j = 0; j < osszefview.Count; j++)
                        {
                            DataRow dr = osszefview[j].Row;
                            string szazalkod = dr["VALASZT"].ToString();
                            string szazal = FakUserInterface.GetTartal(szazalekok, "KOD", "SZOVEG", szazalkod)[0];
                            osszazalek += Convert.ToInt32(szazal);
                        }
                        if (osszazalek != 100 && osszazalek != 0)
                        {
                            if (hibaszov != "")
                                hibaszov += "\n";
                            hibaszov += szov + " : " + osszazalek.ToString();
                        }

                    }
                    osszefview.RowFilter = "";
                    if (hibaszov != "")
                    {
                        hibaszov = "Hibás százalékok!\n" + hibaszov;
                        FakPlusz.MessageBox.Show(hibaszov);
                        return false;
                    }
                    else
                        return true;           //  !!!
                }
            }
            else
            {
                Tablainfo cegevinfo = UserParamTabla.Cegevinfo;
                int i = UserParamTabla.NemLezartEvek.IndexOf(UserParamTabla.SzamlaDatumtol.Year.ToString());
                if (i != -1)
                {
                    for (int j = i; j < UserParamTabla.NemLezartEvek.Count; j++)
                    {
                        cegevinfo.DataView.RowFilter = "EV = '" + UserParamTabla.NemLezartEvek[j].ToString() + "'";
                        cegevinfo.DataView[0].Row["KELLZARAS"] = "I";
                        cegevinfo.DataView[0].Row["MODOSITOTT_M"] = 1;
                        cegevinfo.Modositott = true;
                    }
                }
                cegevinfo.DataView.RowFilter = "";
                if (cegevinfo.Modositott)
                {
                    FakUserInterface.Rogzit(cegevinfo);
                    UserParamTabla.KellZaras = true;
                }
            }
            return true;
        }
        
        
        
        public virtual Base EgyediIndit(string contnev, Vezerloinfo aktivvezerles)
        {
            Base cont = null;
            Base.HozferJogosultsag egyhozfer = UserParamTabla.UserContHozferjog[UserParamTabla.LetezoUserControlNevek.IndexOf(contnev)];
            aktivvezerles.HozferJog = egyhozfer;
            switch (contnev)
            {
                case "Bevszla":
                    cont = new Bevszla(FakUserInterface, this, aktivvezerles);
                    break;
                case "Beflenbevszla":
                    cont = new Beflenbevszla();
                    break;
                case "Bevszlakiegyenl":
                    cont = new Bevszlakiegyenl(FakUserInterface, this, aktivvezerles);
                    break;
                case "Koltsszla":
                    cont = new Koltsszla(FakUserInterface, this, aktivvezerles);
                    break;
                case "Beflenkoltsszla":
                    cont = new Beflenkoltsszla();
                    break;
                case "Koltsszlakiegyenl":
                    cont = new Koltsszlakiegyenl(FakUserInterface, this, aktivvezerles);
                    break;
                case "Folyofolyo":
                    cont = new Folyofolyo(FakUserInterface, this, aktivvezerles);
                    break;
                case "Folyopenztar":
                    cont = new Folyopenztar(FakUserInterface, this, aktivvezerles);
                    break;
                case "Folyokivet":
                    cont = new Folyokivet(FakUserInterface, this, aktivvezerles);
                    break;
                case "Folyobetet":
                    cont = new Folyobetet(FakUserInterface, this, aktivvezerles);
                    break;
                case "Folyoosztottkivet":
                    cont = new Folyoosztottkivet(FakUserInterface, this, aktivvezerles);
                    break;
                case "Penztarpenztar":
                    cont = new Penztarpenztar(FakUserInterface, this, aktivvezerles);
                    break;
                case "Penztarfolyo":
                    cont = new Penztarfolyo(FakUserInterface, this, aktivvezerles);
                    break;
                case "Penztarkivet":
                    cont = new Penztarkivet(FakUserInterface, this, aktivvezerles);
                    break;
                case "Penztarbetet":
                    cont = new Penztarbetet(FakUserInterface, this, aktivvezerles);
                    break;
                case "Bevetelek":
                    cont = new Bevetelek();
                    break;
                case "Kiadasok":
                    cont = new Kiadasok();
                    break;
                case "Penzmozgas":
                    cont = new Penzmozgas();
                    break;
                case "Afaforgalom":
                    cont = new Afaforgalom();
                    break;
                case "Egyenleg":
                    cont = new Egyenleg();
                    break;
                case "Eveseredmeny":
                    cont = new Eveseredmeny();
                    break;
                case "DolgozoiEredm":
                    cont = new DolgozoiEredm();
                    break;
                case "Ujhonap":
                    cont = new Ujhonap(FakUserInterface, this, aktivvezerles);
                    break;
                case "Evzar":
                    cont = new Evzar(FakUserInterface, this, aktivvezerles);
                    break;
                case "Evnyit":
                    cont = new Evnyit(FakUserInterface, this, aktivvezerles);
                    break;
                case "Szerklist":
                    cont = new Szerklist(FakUserInterface, this, aktivvezerles);
                    break;

            }
            int i = aktivvezerles.ControlNevek.IndexOf(cont.ControlNev);
            cont.Parameterez = aktivvezerles.Parameterez[i];
            if (cont.Parameterez != null)
            {
                cont.Parameterez.AktivControl = cont;
                switch (cont.ControlNev)
                {
                    case "Beflenbevszla":
                        ((Beflenbevszla)cont).BeflenbevszlaInit(FakUserInterface, this, aktivvezerles);
                        break;
                    case "Beflenkoltsszla":
                        ((Beflenkoltsszla)cont).BeflenkoltsszlaInit(FakUserInterface, this, aktivvezerles);
                        break;
                    case "Bevetelek":
                        ((Bevetelek)cont).BevetelekInit(FakUserInterface, this, aktivvezerles);
                        break;
                    case "Kiadasok":
                        ((Kiadasok)cont).KiadasokInit(FakUserInterface, this, aktivvezerles);
                        break;
                    case "Penzmozgas":
                        ((Penzmozgas)cont).PenzmozgasInit(FakUserInterface, this, aktivvezerles);
                        break;
                    case "Eveseredmeny":
                        ((Eveseredmeny)cont).EveseredmenyInit(FakUserInterface, this, aktivvezerles);
                        break;
                    case "DolgozoiEredm":
                        ((DolgozoiEredm)cont).DolgozoiEredmInit(FakUserInterface, this, aktivvezerles);
                        break;
                    case "Afaforgalom":
                        ((Afaforgalom)cont).AfaforgalomInit(FakUserInterface, this, aktivvezerles);
                        break;
                    case "Egyenleg":
                        ((Egyenleg)cont).EgyenlegInit(FakUserInterface, this, aktivvezerles);
                        break;
                }
            }
            return cont;
        }
        public override void TabControl_Click(object sender, EventArgs e)
        {
            if (AktivControl != null && AktivControl.UserControlInfo != null)
            {
                MezoTag tag = AktivControl.UserControlInfo.LastEnter;
                if (tag != null)
                    tag.Control.Focus();
            }
        }

        private void technikai_Click(object sender, EventArgs e)
        {

        }
    }

}