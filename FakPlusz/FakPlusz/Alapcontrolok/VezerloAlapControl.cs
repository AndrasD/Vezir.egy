using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using FakPlusz.Alapfunkciok;
using FakPlusz.Formok;
using FakPlusz.Alapcontrolok;

namespace FakPlusz.Alapcontrolok
{
    /// <summary>
    /// Vezerlo User Controlok kozos alapja
    /// </summary>
    public partial class VezerloAlapControl : Base
    {
        /// <summary>
        /// 
        /// </summary>
        public Vezerloinfo ParentVezerles;
        /// <summary>
        /// 
        /// </summary>
        public VezerloinfoCollection ChildVezerlesek = new VezerloinfoCollection();
        /// <summary>
        /// 
        /// </summary>
        public Panel SajatPanel;
        /// <summary>
        /// 
        /// </summary>
        public bool VezerlesValtas = true;
        /// <summary>
        /// 
        /// </summary>
        public bool MenuPageValtas = true;
        /// <summary>
        /// 
        /// </summary>
        public string UserNevFilter = "";
        /// <summary>
        /// 
        /// </summary>
        public string Alkalmazas = "";
        /// <summary>
        /// 
        /// </summary>
        public VezerloAlapControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void VezerloAlapControlInit()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void TabControl_Deselecting(object sender, TabControlCancelEventArgs e)
        {
            if (!FakUserInterface.EventTilt)
            {
                if (AktivControl != null)
                {
                    if (AktivControl.Tabinfo != null && AktivControl.Userabortkerdes(AktivControl.Tabinfo))
                        e.Cancel = true;
                    else if (AktivControl.Aktualtablainfo != null && AktivControl.Userabortkerdes())
                        e.Cancel = true;
                    if (AktivControl.Name == "Mezonevek")
                    {
                        if (AktivControl.Tabinfo.Tablanev == "VALTOZASNAPLO" || AktivControl.Tabinfo.Tablanev == "USERLOG")
                            AktivControl.Tabinfo.Adattabla.Rows.Clear();
                    }
                }
                else if (!e.TabPage.Enabled)
                    e.Cancel = true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void TabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void TabControl_Selected(object sender, TabControlEventArgs e)
        {
            if (!FakUserInterface.EventTilt)
            {
                AktivPageIndex = e.TabPageIndex;
                AktivDropindex = AktivPageIndex;
                MenuPageValtas = true;
                VezerlesValtas = false;
                TabControl_Select();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void TabControl_Select()
        {
            TabControl = Vezerles.TabControl;
            SajatPanel.Controls.Clear();
            AktivVezerles = Vezerles.ChildVezerloinfoCollection[AktivMenuindex];
            Vezerles.AktivVezerles = AktivVezerles;
            if(!MenuPageValtas)
                SajatPanel.Controls.Add(TabControl);

            else
            {
                MenuPageValtas = false;
                TabControl.Visible = false;
                AktivVezerles.AktivMenuItem = Vezerles.MenuItemek[AktivMenuindex];
                AktivMenuItem = AktivVezerles.AktivMenuItem;
                TabPagek = (TabPage[])Vezerles.TabPagek[AktivMenuindex];
                AktivPage = TabPagek[AktivDropindex];
                AktivVezerles.AktivPage = AktivPage;
                DropItemek = (ToolStripMenuItem[])Vezerles.DropItemek[AktivMenuindex];
                AktivVezerles.AktivDropDownItem = DropItemek[AktivDropindex];
                AktivDropDownItem = AktivVezerles.AktivDropDownItem;
                FakUserInterface.EventTilt = true;
                TabControl.Controls.Clear();
                TabControl.Controls.AddRange(TabPagek);
                SajatPanel.Controls.Add(TabControl);
                TabControl.Dock = DockStyle.Fill;
                string[] contnevek = (string[])Vezerles.AlmenuUserControlNevek[AktivMenuindex];
                string aktivcontrolnev = contnevek[AktivDropindex];
                if (Vezerles.OsszesControlNev.IndexOf(aktivcontrolnev) == -1)
                {
                    Vezerles.OsszesControlNev.Add(aktivcontrolnev);
                    Vezerles.OsszesLetezoControl.Add(null);
                }
                int contind = Vezerles.OsszesControlNev.IndexOf(aktivcontrolnev);
                AktivControl = (Base)Vezerles.OsszesLetezoControl[contind];
                if (Alkalmazas == "TERVEZO")
                    Indit(contnevek, aktivcontrolnev);
                else
                    Indit(AktualNode, AktivMenuindex, AktivDropindex);
                AktivControl.AktivPage = AktivPage;
                AktivPage.Controls.Clear();
                TabControl.SelectedIndex = AktivDropindex;
                FakUserInterface.EventTilt = false;
//                TabControl.Visible = true;
                AktivControl.Dock = DockStyle.Fill;
                AktivPage.Controls.Add(AktivControl);
//                if(!AktivControl.Name.Contains("Osszefugg"))
//                    AktivControl.Dock = DockStyle.None;
                AktivControl.AktivMenuItem = AktivMenuItem;
                AktivControl.AktivDropDownItem = AktivDropDownItem;
                TabControl.Visible = true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void VezerlesValtasAllit()
        {
            Vezerloinfo jelenvezerles = null;
            if (ParentVezerles != null)
                jelenvezerles = ParentVezerles.AktivVezerles;
            if (jelenvezerles == null)
            {
                for (int i = 0; i < ChildVezerlesek.Count; i++)
                {
                    jelenvezerles = ChildVezerlesek[i].AktivVezerles;
                    if (jelenvezerles != null)
                        break;
                }
            }
            VezerlesValtas = false;
            if (jelenvezerles == null || AktivVezerles != jelenvezerles)
                VezerlesValtas = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void DropDownItem_Clicked(object sender, ToolStripItemClickedEventArgs e)
        {
            VezerlesValtasAllit();
            ToolStripMenuItem menuitem = (ToolStripMenuItem)sender;
            ToolStripMenuItem dropitem = (ToolStripMenuItem)e.ClickedItem;
            string almenunev = dropitem.Text;
            almenunev = almenunev.Replace("*", "");
            almenunev = almenunev.Replace("!", "");
            string menunev = menuitem.Text;
            menunev = menunev.Replace("*", "");
            menunev = menunev.Replace("!", "");
            ArrayList menunevarray = new ArrayList(Vezerles.MenuNevek);
            int menuindex = menunevarray.IndexOf(menunev);
            int almenuindex = menuitem.DropDownItems.IndexOf(dropitem);
            if (!MenuPageValtas)
            {
                if (menuitem == AktivMenuItem && dropitem == AktivDropDownItem && !VezerlesValtas)
                    MenuPageValtas = false;
                else
                    MenuPageValtas = true;
            }
            if(!VezerlesValtas && !MenuPageValtas)
                return;
            if (AktivControl != null && AktivControl.Tabinfo != null && AktivControl.Userabortkerdes(AktivControl.Tabinfo))
                return;
            AktivMenuindex = menuindex;
            AktivMenuItem=menuitem;
            AktivDropindex = almenuindex;
            AktivDropDownItem = dropitem;
            TabControl_Select();
        }
        public virtual void TabControl_Click(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contnevek"></param>
        /// <param name="aktivcontrolnev"></param>
        public virtual void Indit(string[] contnevek, string aktivcontrolnev)
        {
            Inditas(contnevek, aktivcontrolnev);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contnevek"></param>
        /// <param name="aktivcontrolnev"></param>
        public virtual void Inditas(string[] contnevek,string aktivcontrolnev)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="menuindex"></param>
        /// <param name="dropindex"></param>
        public virtual void Indit(TreeNode node, int menuindex, int dropindex)
        {
            Inditas(node, menuindex, dropindex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="menuindex"></param>
        /// <param name="dropindex"></param>
        public virtual void Inditas(TreeNode node, int menuindex, int dropindex)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void WriteLoginfo()
        {
        }
    }
}
