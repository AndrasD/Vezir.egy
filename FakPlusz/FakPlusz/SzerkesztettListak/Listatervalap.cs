using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FakPlusz.Alapcontrolok;
using FakPlusz.Alapfunkciok;
using FakPlusz.UserAlapcontrolok;

namespace FakPlusz.SzerkesztettListak
{
    /// <summary>
    /// Szerkesztett listak illetve statisztikak UserControljainak alapja. SzerkesztettAlaptol orokol
    /// A tervezesnel a Mit csinaltam? hozza letre illetve alktivizalja az objectumot
    /// </summary>
    public partial class Listatervalap : SzerkesztettAlap
    {
        DataGridView parametergridview = new DataGridView();
        int tartalsorindex;
        Tablainfo tartalinfo = null;
        DataRow tartalsor;
        /// <summary>
        /// objectum letrehozasa
        /// </summary>
        public Listatervalap()
        {
            InitializeComponent();
            MezoControlInfok = new MezoControlInfo[1];
            listaparamcombok.AddRange(new ComboBox[]{sorrendsorszaml,osszegfokkepzesl,
                osszegzendol,atlagolandol,csakosszegsorl,oszlopsorszaml});
            statisztikaparamcombok.AddRange(new ComboBox[] { sorrendsorszams, atlagolandos, oszlopsorszams });

            feltcombok.AddRange(new ComboBox[] { nyitozarojelf, elsoelemf, relaciof, masodikelemf, zarozarojelf, esvagyf });
            sorcombok.AddRange(new ComboBox[] { nyitozarojels, elsoelems, relacios, masodikelems, zarozarojels, esvagys });
            oszlopcombok.AddRange(new ComboBox[] { nyitozarojelo, elsoelemo, relacioo, masodikelemo, zarozarojelo, esvagyo });

            for (int i = 0; i < panel1.Controls.Count; i++)
                savcont.Add(panel1.Controls[i]);
        }
        /// <summary>
        /// Aktivizalas,ujraaktivizalas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void TabStop_Changed(object sender, EventArgs e)
        {
            base.TabStop_Changed(sender, e);
        }
        /// <summary>
        /// Inicializalas
        /// </summary>
        public override void AltalanosInit()
        {
            megnevezes.SelectionLength = 0;
            comboBox2.SelectionLength = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectionLength = 0;
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectionLength = 0;
            comboBox4.SelectedIndex = 0;
            if (FakUserInterface.EventTilt)
                return;
            int ig = 1;
            if (!listae)
                ig = 3;
            if (!UjTag && ValtozasLekerdez().Count ==0)
            {
                if(MezoControlInfok[0]!=null)
                    MezoControlInfok[0].UserControlInfo = UserControlInfo;
                return;
            }
            else
            {
                AktivPage.Text = AktivDropDownItem.Text;
                FakUserInterface.EventTilt = true;
                base.AltalanosInit();
                if (altlistazo != null)
                    altlistazo.UjTag = true;
                ValtozasTorol();
                UjTag = false;
                panel1.Controls.Clear();
                panel1.Controls.AddRange((Control[])savcont.ToArray(typeof(Control)));
                for (int i = 0; i < aktivsorindex.Length; i++)
                    aktivsorindex[i] = -1;
                tabControl1.Controls.Clear();
                toolstripek.Clear();
                gridviewk.Clear();
                if (listae)
                {
                    combooszlopok = listaparamcombooszlopok;
                    SelectionLengthNullaz(listaparamcombok);
                    osszescombo.Add(listaparamcombok);
                    gridviewk.Add(listagridview);
                    toolstripek.AddRange(new ToolStrip[] { toolStrip6, toolStrip3 });
                    szurofeltetel = listaszurofeltetel;
                    tabControl1.Controls.AddRange(new TabPage[] { tabPage5, tabPage2 });
                    parametertabla = listaparametertabla;
                    paramfilter = listafilter;
                }
                else
                {
                    combooszlopok = statisztikaparamcombooszlopok;
                    osszescombo.Add(statisztikaparamcombok);
                    SelectionLengthNullaz(statisztikaparamcombok);
                    gridviewk.Add(statisztikagridview);
                    toolstripek.AddRange(new ToolStrip[] { toolStrip8, toolStrip3, toolStrip4, toolStrip5 });
                    szurofeltetel = statisztikaszurofeltetel;
                    tabControl1.Controls.AddRange(new TabPage[] { tabPage7, tabPage2, tabPage3, tabPage4 });
                    parametertabla = statisztikaparametertabla;
                    paramfilter = statisztikafilter;
                }
                osszestabla.AddRange(new AdatTabla[] { parametertabla, felteteltabla, sorfelteteltabla, oszlopfelteteltabla });
                gridviewk.AddRange(new DataGridView[] { feltetelfgridview, feltetelsgridview, feltetelogridview });
                parameterview.Table = parametertabla;
                feltetelview.Table = felteteltabla;
                feltetelsview.Table = sorfelteteltabla;
                felteteloview.Table = oszlopfelteteltabla;
                for (int i = 0; i < gridviewk.Count; i++)
                {
                    DataGridView gridview = (DataGridView)gridviewk[i];
                    gridview.Columns.Clear();
                    gridview.Visible = false;
                    gridview.AutoGenerateColumns = false;
                    DataTable table = ((DataView)osszesview[i]).Table;
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        DataColumn col = table.Columns[j];
                        DataGridViewTextBoxColumn gridcol = new DataGridViewTextBoxColumn();
                        gridcol.HeaderText = col.Caption;
                        gridcol.Name = col.ColumnName;
                        gridcol.DataPropertyName = col.ColumnName;
                        if (col.ColumnName == "MEZONEVE" || col.ColumnName == "ELSOELEM" || col.ColumnName == "MASODIKELEM")
                            gridcol.Width = 250;
                        gridview.Columns.Add(gridcol);
                    }
                    gridview.DataSource = (DataView)osszesview[i];
                    gridview.Visible = true;
                }
                SelectionLengthNullaz(feltcombok);
                SelectionLengthNullaz(sorcombok);
                SelectionLengthNullaz(oszlopcombok);
                osszescombo.Add(feltcombok);
                osszescombo.Add(sorcombok);
                osszescombo.Add(oszlopcombok);
                rogzit.Enabled = false;
                Tabinfo = TablainfoTag.Tablainfo;
                HozferJog = Tabinfo.Azonositok.Jogszintek[(int)KezeloiSzint];
                tartalinfo = TablainfoTag.Adattabla.Tablainfo;
                tartalsorindex = TablainfoTag.SorIndex;
                tartalsor = tartalinfo.Adattabla.Rows[tartalsorindex];
                if (HozferJog != HozferJogosultsag.Irolvas)
                {
                    //uj.Visible = false;
                    //teljestorles.Visible = false;
                    ellenorzes.Visible = false;
                    rogzit.Visible = false;
                }
                megnevezes.Text = tartalsor["SZOVEG"].ToString();
                Tabinfo.AktualControlInfo = new MezoControlInfo(this, Tabinfo);
                MezoControlInfok[0] = Tabinfo.AktualControlInfo;
                UserControlInfo = FakUserInterface.Attach(this, Vezerles, ref Tabinfo, AktivPage, AktivMenuItem, AktivDropDownItem);
                MezoControlInfok[0].UserControlInfo = UserControlInfo;
                Kodtipus = Tabinfo.Kodtipus;
                Azontip = Tabinfo.Azontip;
                Tabinfo.DataView.RowFilter = "KODTIPUS='" + Kodtipus + "'";
                VerziobuttonokAllit(HozferJog);
                alaptreeview.Nodes.Clear();
                kelltreeview.Nodes.Clear();
                TermCegPluszCegalattiTabinfok = FakUserInterface.GetCegPluszCegalattiTermTablaInfok();
                for (int i = 0; i < TermCegPluszCegalattiTabinfok.Count; i++)
                {
                    Tablainfo egyinfo = TermCegPluszCegalattiTabinfok[i].LeiroTablainfo;
                    string filt = egyinfo.DataView.RowFilter;
                    if (listae)
                        egyinfo.DataView.RowFilter = "LISTABA='I'";
                    else
                        egyinfo.DataView.RowFilter = "STATISZTIKABA='I'";
                    if (egyinfo.DataView.Count != 0)
                    {
                        TreeNode fonode = new TreeNode(egyinfo.TablaTag.Azonositok.Szoveg);
                        fonode.Tag = egyinfo.Azontip;
                        for (int j = 0; j < egyinfo.DataView.Count; j++)
                        {
                            DataRow dr = egyinfo.DataView[j].Row;
                            TreeNode mezonode = new TreeNode(fonode.Text + "->" + dr["SORSZOV"].ToString());
                            mezonode.Tag = fonode.Tag.ToString() + "->" + dr["ADATNEV"].ToString();
                            fonode.Nodes.Add(mezonode);
                        }
                        alaptreeview.Nodes.Add(fonode);
                    }
                    egyinfo.DataView.RowFilter = filt;

                }
                if (Tabinfo.DataView.Count != 0)
                {
                    DataRow dr = Tabinfo.DataView[0].Row;
                    string szov = dr["VANDATUMINT"].ToString();
                    VanDatum = szov == "I";
                    if (szov == "N")
                        comboBox2.SelectedIndex = 0;
                    else
                        comboBox2.SelectedIndex = 1;
                    comboBox2.SelectionLength = 0;
                    comboBox2.SelectionStart = 0;
                    szov = dr["TELJESHONAP"].ToString();
                    TeljesHonap = szov == "I";
                    if (szov == "N")
                        comboBox3.SelectedIndex = 0;
                    else
                        comboBox3.SelectedIndex = 1;
                    comboBox3.SelectionLength = 0;
                    comboBox3.SelectionStart = 0;
                    szov = dr["TELJESEV"].ToString();
                    TeljesEv = szov == "I";
                    if (szov == "N")
                        comboBox4.SelectedIndex = 0;
                    else
                        comboBox4.SelectedIndex = 1;
                    comboBox4.SelectionLength = 0;
                    comboBox4.SelectionStart = 0;
                    osszeslistaelem = dr["OSSZESLISTAELEM"].ToString();
                    Elemez(kelltreeview, osszeslistaelem, alaptreeview);
                    string[] feltetelek;
                    if (listae)
                        feltetelek = new string[] { "", dr["FELTETEL"].ToString() };
                    else
                        feltetelek = new string[] {"",dr["FELTETEL"].ToString(),dr["SORFELTETEL"].ToString(),
                        dr["OSZLOPFELTETEL"].ToString()};
                    if (listagridview.Rows.Count != 0)
                    {
                        aktivsorindex[0] = 0;
                        label9.Text = parameterview[0].Row["MEZONEVE"].ToString();
                    }
                    string feltetel;
                    for (int i = 1; i <= ig; i++)
                    {
                        feltetel = feltetelek[i];
                        if (feltetel != "")
                        {
                            Feltelemez((DataTable)osszestabla[i], feltetel);
                            aktivsorindex[i] = 0;
                        }
                        else
                            aktivsorindex[i] = -1;
                    }
                }
            }
            for (int i = 0; i <= ig; i++)
            {
                if (i == 0)
                    Egyebrendezes(i);
                else
                    Feltetelrendezes(i);
                AlbuttonokAllit(i, HozferJog);
            }
            FakUserInterface.EventTilt = false;
        }
        private void SelectionLengthNullaz(ArrayList combok)
        {
            for (int i = 0; i < combok.Count; i++)
               ((ComboBox)combok[i]).SelectionLength = 0;
        }
        private void OsszesListaelemGyart(DataRow dr)
        {
            string filt = parameterview.RowFilter;
            string sort = parameterview.Sort;
            parameterview.RowFilter = "";
            parameterview.Sort = "AZONTIP,MEZONEV";
            string osszeslistaelem = "";
            DataTable table = parameterview.Table;
            for (int i = 0; i < parameterview.Count; i++)
            {
                DataRow dr1 = parameterview[i].Row;
                string egysor = "";
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    string egycolnev = table.Columns[j].ColumnName;
                    if (egysor != "")
                        egysor += ";";
                    egysor += dr1[egycolnev].ToString();
                }
                if (osszeslistaelem != "")
                    osszeslistaelem += "]";
                osszeslistaelem += egysor;
            }
            dr["OSSZESLISTAELEM"] = osszeslistaelem;
            parameterview.RowFilter = filt;
            parameterview.Sort = sort;
        }
        private string FeltetelSorbarak(int tabindex)
        {
            DataView view = (DataView)osszesview[tabindex];
            DataTable adattabla = view.Table;
            string sorstring = "";
            for (int i = 0; i < view.Count; i++)
            {
                DataRow dr = view[i].Row;
                if (sorstring != "")
                    sorstring += "]";
                for (int j = 0; j < adattabla.Columns.Count; j++)
                {
                    string colname = adattabla.Columns[j].ColumnName;
                    sorstring += dr[colname].ToString();
                    if (j != adattabla.Columns.Count - 1)
                        sorstring += ";";
                }
            }
            return sorstring;
        }
        /// <summary>
        /// Fo Buttonok visible/enabled allitasa jogosultsag alapjan
        /// </summary>
        /// <param name="jogszint">
        /// Jogosultsag
        /// </param>
        public void VerziobuttonokAllit(HozferJogosultsag jogszint)
        {
 //           rogzit.Visible = true;
            ellenorzes.Visible = false; 
            if (Tabinfo.Modositott && !Tabinfo.ModositasiHiba)
                ellenorzes.Visible = true;
            elolrolalap.Visible = true;
            if (Tabinfo.DataView.Count == 0 || Tabinfo.Modositott || Tabinfo.ModositasiHiba)
                preview.Visible = false;
            else
                preview.Visible = true;
            rogzit.Enabled = false;
            //uj.Visible = false;
            //teljestorles.Visible = false;
            //elozoverzio.Visible = false;
            //kovetkezoverzio.Visible = false;
            vissza.Visible = false;
//            help.Visible = false;
            //if (Tabinfo.KellVerzio && Tabinfo.VerzioTerkepArray.Count > 1)
            //{
            //    if (Tabinfo.AktVerzioId != Tabinfo.FirstVersionId)
            //        elozoverzio.Visible = true;
            //    if (Tabinfo.AktVerzioId != Tabinfo.LastVersionId)
            //        kovetkezoverzio.Visible = true;
            //}
            if (jogszint == HozferJogosultsag.Irolvas)
            {
                if (Beszurhat && (!Tabinfo.KellVerzio || Tabinfo.VerzioTerkepArray.Count != 0 && !Tabinfo.LezartVersion))
                {
                    rogzit.Visible = true;
                    //if (Tabinfo.KellVerzio && Tabinfo.Azonositok.Verzioinfok.VersionArray.Length > 1 && Tabinfo.LastVersionId == Tabinfo.Azonositok.Verzioinfok.LastVersionId)
                    //    teljestorles.Visible = true;
                }
                else if (Tabinfo.KellVerzio)
                {
                    if (Tabinfo.LezartVersion || Tabinfo.VerzioTerkepArray.Count == 0)
                    {
                        rogzit.Visible = false;
                        //if (Tabinfo.VerzioTerkepArray.Count == 0 || Tabinfo.LastVersionId < Tabinfo.Azonositok.Verzioinfok.LastVersionId)//VersionArray[Tabinfo.VerzioInfok.VersionArray.Length - 1])
                        //    uj.Visible = true;
                    }
                    else if (Tabinfo.VerzioTerkepArray.Count > 1)
                    {
                        //                       teljestorles.Visible = true;
                    }
                    else
                        rogzit.Visible = true;
                }
            }
        }
        /// <summary>
        /// Fo Buttonok visible/enabled alap allitasa 
        /// </summary>
        public override void VerziobuttonokAllit()
        {
            rogzit.Visible = false;
            //uj.Visible = false;
            //teljestorles.Visible = false;
            //elozoverzio.Visible = false;
            //kovetkezoverzio.Visible = false;
            vissza.Visible = true;
//            help.Visible = true;
            ellenorzes.Visible = false;
            elolrolalap.Visible = false;
            preview.Visible = false;
            torolalap.Visible = false;
        }
        /// <summary>
        /// Aktualis tabpage Buttonok visible/enabled alap allitasa jogosultsag alapjan
        /// </summary>
        /// <param name="tabpageindex">
        /// tabpage indexe
        /// </param>
        /// <param name="jogszint">
        /// jogosultsag
        /// </param>
        public void AlbuttonokAllit(int tabpageindex, HozferJogosultsag jogszint)
        {
            DataView view = (DataView)osszesview[tabpageindex];
            DataTable adattabla = view.Table;
            int viewcount = view.Count;
            int esvagyind = adattabla.Columns.IndexOf("ESVAGY");
            string esvagy = "";
            ToolStrip toolstrip = (ToolStrip)toolstripek[tabpageindex];
            int sorindex = aktivsorindex[tabpageindex];
            SetAktRowVisible((DataGridView)gridviewk[tabpageindex], sorindex);
            if (viewcount != 0 && esvagyind != -1)
                esvagy = view[viewcount - 1].Row[esvagyind].ToString();
            bool hiba = false;
            ArrayList comboar = (ArrayList)osszescombo[pageindex];
            ellenorzes.Enabled = true;
            for (int j = 0; j < comboar.Count; j++)
            {
                ComboBox combo = (ComboBox)comboar[j];
                if (combo.Tag.ToString() == "1")
                {
                    hiba = true;
                    ellenorzes.Enabled = false;
                    break;
                }
            }
            for (int i = 0; i < toolstrip.Items.Count; i++)
            {
                ToolStripButton button = (ToolStripButton)toolstrip.Items[i];
                string buttonname = button.Name.Substring(0, button.Name.Length - 1);
                switch (buttonname)
                {
                    case "ujsor":
                        if (sorindex == -1 || esvagyind != -1 && esvagy == "" && tabpageindex == 1  || hiba)
                            button.Enabled = false;
                        else
                            button.Enabled = true;
                        break;
                    case "elozosor":
                        if (sorindex == -1 || sorindex == 0)
                            button.Enabled = false;
                        else
                            button.Enabled = true;
                        break;
                    case "kovsor":
                        if (sorindex == -1 || sorindex == viewcount - 1)
                            button.Enabled = false;
                        else
                            button.Enabled = true;
                        break;
                    case "sortorol":
                        if (sorindex == -1)
                            button.Enabled = false;
                        else
                            button.Enabled = true;
                        break;
                    case "elolrol":
                        if (viewcount == 0)
                            button.Enabled = false;
                        else
                            button.Enabled = true;
                        break;
                    case "ok":
                        button.Enabled = true;
                        if (hiba || pageindex ==0 && parameterview.Count == 0 || pageindex==1 && elsoelemf.Items.Count==0 ||
                            pageindex ==2 && elsoelems.Items.Count ==0 || pageindex==3 && elsoelemo.Items.Count==0)
 //                       if (hiba || aktivsorindex[tabpageindex] == -1)
                        {
                            button.Enabled = false;
                            ellenorzes.Enabled = false;
                        }
                        break;
                }
            }
        }
        /// <summary>
        /// Fo Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void fobuttonok_Click(object sender, EventArgs e)
        {
            if (FakUserInterface.EventTilt)
                return;
            DataRow dr = null;
            int ig = 2;
            if (!listae)
                ig = 4;
            ToolStripButton but = (ToolStripButton)sender;
            switch (but.Name)
            {
                //case "uj":
                //    break;
                //case "elozoverzio":
                //    break;
                //case "kovetkezoverzio":
                //    break;
                //case "teljestorles":
                //    break;
                case "ellenorzes":
                    int matrixpontok = 0;
                    int oszlopfeltek = 0;
                    int sorfeltek = 0;
                    if (!listae)
                    {
                        sorfeltek = ((DataView)osszesview[2]).Count;
                        oszlopfeltek = ((DataView)osszesview[3]).Count;
                    }
                    bool volthiba = Hibavizsg();
                    string hiba = "";
                    DataView view = (DataView)osszesview[0];
                    string sort = view.Sort;
                    ComboBox[] combok;
                    ComboBox combo;
                    string[] sortok;
                    string[] filterek;
                    string[] hibak;
                    if (!volthiba)
                    {
                        if (listae)
                        {
                            combok = new ComboBox[] { sorrendsorszaml, oszlopsorszaml };
                            sortok = new string[] { "SORRENDSORSZAM", "OSZLOPSORSZAM" };
                            filterek = new string[] { "SORRENDSORSZAM NOT = 0", "OSZLOPSORSZAM NOT = 0" };
                            hibak = new string[] { "Sorrendszint sorszáma nem folytonos!", "Oszlopok sorszámozása nem folytonos!" };
                        }
                        else
                        {
                            combok = new ComboBox[] { sorrendsorszams, oszlopsorszams };
                            sortok = new string[] { "SORRENDSORSZAM", "MATRIXSORSZAM" };
                            filterek = new string[] { "SORRENDSORSZAM NOT = 0", "MATRIXSORSZAM NOT = 0" };
                            hibak = new string[] { "Sorrendszint sorszáma nem folytonos!", "Mátrixpontok sorszámozása nem folytonos!" };
                        }
                        for (int i = 0; i < sortok.Length; i++)
                        {
                            view.Sort = sortok[i];
                            view.RowFilter = filterek[i];
                            combo = combok[i];
                            combo.Tag = "0";
                            FakUserInterface.ErrorProvider.SetError(combo, "");
                            if (view.Count != 0)
                            {
                                if (!listae)
                                {
                                    if (i == 1)
                                    {
                                        matrixpontok = view.Count;
                                        if (matrixpontok == 0 || oszlopfeltek == 0 || sorfeltek == 0)
                                        {
                                            volthiba = true;
                                            MessageBox.Show("Hiányos feladatmegadás!\nEllenörizze a mátrixpontokat, sor-/oszlopfeltételeket!");
                                            break;
                                        }
                                        else if (1 + (oszlopfeltek + 1) * matrixpontok > 10)
                                        {
                                            volthiba = true;
                                            MessageBox.Show("Nem fér el a listán!\n Vagy a mátrixpontokat vagy az \noszlopmeghatározásokat csökkenteni kell");
                                            break;
                                        }
                                    }
                                }
                                for (int j = 0; j < view.Count; j++)
                                {
                                    if (j + 1 != Convert.ToInt32(view[j][sortok[i]].ToString()))
                                    {
                                        hiba = hibak[i];
                                        volthiba = true;
                                        combo.Tag = "1";
                                        FakUserInterface.ErrorProvider.SetError(combo, hiba);
                                        break;
                                    }
                                }
                            }
                            if (!volthiba && hiba == "" && i == 0 && view.Count>1)           // sorrend
                            {
                                ArrayList ar = new ArrayList();
                                Tablainfo tabinfo;
                                for (int j = 0; j < view.Count; j++)
                                {
                                    dr = view[j].Row;
                                    tabinfo = FakUserInterface.GetByAzontip(dr["AZONTIP"].ToString());
                                    if (ar.IndexOf(tabinfo) == -1)
                                        ar.Add(tabinfo);
                                }
                                if (ar.Count > 1)
                                {

                                    for (int j = 1; j < ar.Count-1; j++)
                                    {
                                        tabinfo = (Tablainfo)ar[j];
                                        Tablainfo szulo = tabinfo.TermParentTabinfo;
                                        if (szulo == null)
                                        {
                                            hiba = " Sorrendszintek nem felelnek meg az adatbázis táblasorrendjének!";
                                            volthiba = true;
                                            combo.Tag = "1";
                                            FakUserInterface.ErrorProvider.SetError(combo, hiba);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    view.Sort = sort;
                    view.RowFilter = "";
                    if (!volthiba)
                    {
                        ArrayList comboar;
                        for (int i = 1; i < ig; i++)
                        {
                            comboar = (ArrayList)osszescombo[i];
                            ComboBox esvagy = (ComboBox)comboar[comboar.Count - 1];
                            esvagy.Tag = 0;
                            view = (DataView)osszesview[i];
                            if (view.Count != 0)
                            {
                                hiba = "";
                                dr = view[view.Count - 1].Row;
                                aktivsorindex[i] = view.Count - 1;
                                Feltetelrendezes(i);
                                if (esvagy.Text != "")
                                {
                                    esvagy.Tag = 1;
                                    volthiba = true;
                                    hiba = "Utolsó sorban üresnek kell lennie!";
                                    Tabinfo.ModositasiHiba = true;
                                }
                                else
                                {
                                    esvagy.Tag = 0;
                                    Tabinfo.ModositasiHiba = false;
                                }
                                FakUserInterface.ErrorProvider.SetError(esvagy, hiba);
                                if (!volthiba)
                                {
                                    if (i == 1)
                                    {
                                        for (int k = 0; k < view.Count; k++)
                                        {
                                            dr = view[k].Row;
                                            aktivsorindex[i] = k;
                                            Feltetelrendezes(i);
                                            if (k != view.Count - 1 && esvagy.Text == "")
                                            {
                                                esvagy.Tag = 1;
                                                volthiba = true;
                                                hiba = "Közbülsö sorban nem lehet üres!";
                                                Tabinfo.ModositasiHiba = true;
                                            }
                                        }
                                    }
                                    FakUserInterface.ErrorProvider.SetError(esvagy, hiba);
                                }
                                int nyitozcount = 0;
                                int zarozcount = 0;
                                for (int k = 0; k < view.Count; k++)
                                {
                                    dr = view[k].Row;
                                    if (dr["NYITOZAROJEL"].ToString() != "")
                                        nyitozcount++;
                                    if (dr["ZAROZAROJEL"].ToString() != "")
                                        zarozcount++;
                                }
                                if (nyitozcount != zarozcount)
                                {
                                    if (hiba != "")
                                        hiba += "\n";
                                    hiba += "Nyitózárójelszám = " + nyitozcount.ToString() + " Zárózárójelszám = " + zarozcount.ToString();
                                    volthiba = true;
                                }
                                FakUserInterface.ErrorProvider.SetError(esvagy, hiba);
                            }
                        }
                    }
                    AlbuttonokAllit(pageindex, HozferJog);
                    VerziobuttonokAllit(HozferJog);
                    if (volthiba)
                        rogzit.Enabled = false;
                    else
                        rogzit.Enabled = true;
                    break;
                case "preview":
                    if (altlistazo == null)
                        altlistazo = new Altlistazoalap(FakUserInterface, this, Vezerles);
//                    ValtozasBeallit();
                    altlistazo.Tabinfo = Tabinfo;
                    altlistazo.listae = listae;
                    altlistazo.listakodtipus = Kodtipus;
                    altlistazo.listaazontip = Azontip;
                    altlistazo.TeljesHonap = TeljesHonap;
                    altlistazo.VanDatum = VanDatum;
                    panel1.Controls.Clear();
                    panel1.Controls.Add(altlistazo);
                    altlistazo.Dock = DockStyle.Fill;
                    altlistazo.TabStop = false;
                    altlistazo.TabStop = true;
                    break;
                case "vissza":
                    panel1.Controls.Clear();
                    panel1.Controls.AddRange((Control[])savcont.ToArray(typeof(Control)));
                    VerziobuttonokAllit(HozferJog);
                    break;
                case "help":
                    string azon = but.Tag.ToString();
                    FakUserInterface.ShowHelp(azon,false,this);
                    break;
                case "rogzit":
                    if (Tabinfo.DataView.Count == 0)
                        Tabinfo.Ujsor();
                    dr = Tabinfo.DataView[0].Row;
                    dr["KODTIPUS"] = Kodtipus;
                    dr["SZOVEG"] = megnevezes.Text;
                    string szov = comboBox2.Text.Substring(0, 1);
                    dr["VANDATUMINT"] = szov;
                    szov = comboBox3.Text.Substring(0, 1);
                    dr["TELJESHONAP"] = szov;
                    TeljesHonap = szov == "I";
                    szov = comboBox4.Text.Substring(0, 1);
                    dr["TELJESEV"] = szov;
                    TeljesEv = szov == "I";
                    rogzit.Enabled = false;
                    OsszesListaelemGyart(dr);
                    string[] feltcol = new string[] { "", "FELTETEL", "SORFELTETEL", "OSZLOPFELTETEL" };
                    for (int i = 1; i < ig; i++)
                    {
                        dr[feltcol[i]] = FeltetelSorbarak(i);
                    }
                    Tabinfo.Modositott = true;
                    FakUserInterface.Rogzit(Tabinfo);
                    if (megnevezes.Text != tartalsor["SZOVEG"].ToString())
                    {
                        tartalsor["SZOVEG"] = megnevezes.Text;
                        tartalsor["MODOSITOTT_M"] = 1;
                        tartalinfo.Modositott = true;
                        FakUserInterface.Rogzit(tartalinfo);
                    }
                    RogzitesUtan();
                    VerziobuttonokAllit(HozferJog);
                    UjTag = true;
                    ValtozasBeallit();
                    AltalanosInit();
                    break;
                case "elolrolalap":
                    FakUserInterface.ForceAdattolt(Tabinfo);
                    UjTag = true;
                    ValtozasBeallit();
                    AltalanosInit();
                    break;
            }
        }
        private void albuttonok_Click(object sender, EventArgs e)
        {
            if (FakUserInterface.EventTilt)
                return;
            DataRow dr;
            ToolStripButton button = (ToolStripButton)sender;
            string buttonname = button.Name.Substring(0, button.Name.Length - 1);
            DataView view = (DataView)osszesview[pageindex];
            DataTable adattabla = view.Table;
            int sorindex = aktivsorindex[pageindex];
            int viewcount = view.Count;
            ArrayList comboar = (ArrayList)osszescombo[pageindex];
            bool hiba = false;
            for (int i = 0; i < comboar.Count; i++)
            {
                ComboBox combo = (ComboBox)comboar[i];
                if (combo.Tag.ToString() == "1")
                    hiba = true;
            }
            bool okvolt = false;
            string oszlnev = "";
            if (pageindex > 0)
            {
                if (pageindex == 1)
                    oszlnev = "VANFELTBEN";
                else if (pageindex == 2)
                    oszlnev = "VANSORFELTBEN";
                else
                    oszlnev = "VANOSZLFELTBEN";
            }
            switch (buttonname)
            {
                case "ujsor":
                    aktivsorindex[pageindex] = -1;
                    break;
                case "elozosor":
                    aktivsorindex[pageindex]--;
                    break;
                case "kovsor":
                    aktivsorindex[pageindex]++;
                    break;
                case "ok":
                    okvolt = true;
                    hiba = Hibavizsg(pageindex);
                    if (!hiba)
                        Tabinfo.ModositasiHiba = false;
                    break;
                case "sortorol":
                    dr = view[aktivsorindex[pageindex]].Row;
                    string azontip = dr["AZONTIP"].ToString();
                    string mezonev = dr["MEZONEV"].ToString();
                    dr.Delete();
                    aktivsorindex[pageindex] = view.Count - 1;
                    string filt = view.RowFilter;
                    view.RowFilter = "AZONTIP='" + azontip + "' and MEZONEV='" + mezonev + "'";
                    if (view.Count == 0)
                    {
                        parameterview.RowFilter = view.RowFilter;
                        parameterview[0].Row[oszlnev] = "Nem";
                    }
                    view.RowFilter = filt;
                    Tabinfo.Modositott = true;
                    VerziobuttonokAllit(HozferJog);
                    break;
                case "elolrol":
                    if (viewcount == 0)
                        break;
                    if (aktivsorindex[pageindex] == -1)
                        aktivsorindex[pageindex] = 0;
                    break;
                case "help":
                    string azon = button.Tag.ToString();
                    FakUserInterface.ShowHelp(azon,true,this);
                    break;
            }
            if (okvolt)
                VerziobuttonokAllit(HozferJog);
            if (!hiba && okvolt)
            {
                if (sorindex == -1)
                    dr = adattabla.NewRow();
                else
                    dr = view[sorindex].Row;
                for (int i = 0; i < adattabla.Columns.Count; i++)
                {
                    string colname = adattabla.Columns[i].ColumnName;
                    string combonev = "";
                    switch (colname)
                    {
                        case "NYITOZAROJEL":
                            combonev = "nyitozarojel";
                            break;
                        case "ELSOELEM":
                            combonev = "elsoelem";
                            break;
                        case "RELACIO":
                            combonev = "relacio";
                            break;
                        case "MASODIKELEM":
                            combonev = "masodikelem";
                            break;
                        case "ZAROZAROJEL":
                            combonev = "zarozarojel";
                            break;
                        case "ESVAGY":
                            combonev = "esvagy";
                            break;
                        case "OSZLOPSORSZAM":
                            combonev = "oszlopsorszam";
                            break;
                        case "MATRIXSORSZAM":
                            combonev = "oszlopsorszam";
                            break;
                        case "SORRENDSORSZAM":
                            combonev = "sorrendsorszam";
                            break;
                        case "KELLOSSZEGZES":
                            combonev = "osszegfokkepzes";
                            break;
                        case "OSSZEGZENDO":
                            combonev = "osszegzendo";
                            break;
                        case "ATLAGOLANDO":
                            combonev = "atlagolando";
                            break;
                        case "CSAKOSSZEGSORBA":
                            combonev = "csakosszegsor";
                            break;
                    }
                    for (int j = 0; j < comboar.Count; j++)
                    {
                        ComboBox combo = (ComboBox)comboar[j];
                        if (combonev != "" && combo.Name.Contains(combonev))
                        {
                            dr[colname] = combo.Text;
                            if (combonev.Contains("sorszam"))
                            {
                                for (int k = 0; k < parameterview.Count; k++)
                                {
                                    DataRow egydr = parameterview[k].Row;
                                    if (egydr == dr)
                                    {
                                        aktivsorindex[pageindex] = k;
                                        sorindex = k;
                                        break;
                                    }
                                }
                            }
                            break;
                        }
                    }

                }
                if (pageindex > 0)
                {
                    parameterview.RowFilter = "MEZONEVE = '" + dr["ELSOELEM"].ToString() + "'";
                    dr["AZONTIP"] = parameterview[0].Row["AZONTIP"];
                    dr["MEZONEV"] = parameterview[0].Row["MEZONEV"];
                    parameterview[0].Row[oszlnev] = "Igen";
                    parameterview.RowFilter = "";
                }
                else
                {
                }
                Tabinfo.Modositott = true;
                VerziobuttonokAllit(HozferJog);
                if (sorindex == -1)
                {
                    adattabla.Rows.Add(dr);
                    aktivsorindex[pageindex] = view.Count - 1;
                }
                else if (sorindex != view.Count - 1)
                    aktivsorindex[pageindex]++;
                DataGridView gridv = (DataGridView)gridviewk[pageindex];
                for (int i = 0; i < gridv.SelectedRows.Count; i++)
                    gridv.Rows[i].Selected = false;
                gridv.Rows[aktivsorindex[pageindex]].Selected = true;
            }
            if (!hiba)
            {
                if (pageindex != 0)
                    Feltetelrendezes(pageindex);
                else
                    Egyebrendezes(pageindex);
            }
            AlbuttonokAllit(pageindex, HozferJog);
            
        }
        /// <summary>
        /// gridview sorara click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void dataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (FakUserInterface.EventTilt)
                return;
            aktivsorindex[pageindex] = e.RowIndex;
            if (pageindex !=0)
                Feltetelrendezes(pageindex);
            else
                Egyebrendezes(pageindex);
            AlbuttonokAllit(pageindex, HozferJog);
        }
        /// <summary>
        /// Node hozzaadas
        /// </summary>
        /// <param name="node">
        /// 
        /// </param>
        public virtual void NodeAdd(TreeNode node)
        {
            TreeView view = kelltreeview;
            string mezonev;
            TreeNode befonode;
            string beazontip;

            ArrayList benodeok = new ArrayList();
            if (node.Nodes.Count != 0)
            {
                befonode = node;
                for (int i = 0; i < node.Nodes.Count; i++)
                    benodeok.Add(node.Nodes[i]);
            }
            else
            {
                befonode = node.Parent;
                benodeok.Add(node);
            }
            TreeNode fonode = null;
            beazontip = befonode.Tag.ToString();
            for (int i = 0; i < view.Nodes.Count; i++)
            {
                string azontip = view.Nodes[i].Tag.ToString();
                if (azontip == beazontip)
                {
                    fonode = view.Nodes[i];
                    int mezonodeindex = -1;
                    TreeNode mezonode;
                    for (int j = 0; j < benodeok.Count; j++)
                    {
                        TreeNode benode = (TreeNode)benodeok[j];
                        mezonode = new TreeNode(benode.Text);
                        string mezotag = benode.Tag.ToString();
                        split = mezotag.Split(nyil);
                        mezonev = split[2];
                        mezonode.Tag = mezotag;
                        for (int k = 0; k < fonode.Nodes.Count; k++)
                        {
                            TreeNode hasonmezonode = fonode.Nodes[k];
                            string hasontag = hasonmezonode.Tag.ToString();
                            if (mezotag == hasontag)
                            {
                                mezonodeindex = k;
                                break;
                            }
                        }
                        if (mezonodeindex == -1)
                        {
                            Tablasorbabe(beazontip, mezonev, mezonode.Text);
                            fonode.Nodes.Add(mezonode);
                        }
                        break;
                    }
                }
            }
            if (fonode == null)
            {
                fonode = new TreeNode(befonode.Text);
                fonode.Tag = befonode.Tag;
                for (int i = 0; i < benodeok.Count; i++)
                {
                    TreeNode benode = (TreeNode)benodeok[i];
                    TreeNode mezonode = new TreeNode(benode.Text);
                    string mezotag = benode.Tag.ToString();
                    split = mezotag.Split(nyil);
                    mezonev = split[2];
                    mezonode.Tag = mezotag;
                    Tablasorbabe(beazontip, mezonev, mezonode.Text);
                    TreeNode ujnode = new TreeNode(mezonode.Text);
                    ujnode.Tag = mezonode.Tag;
                    fonode.Nodes.Add(ujnode);
                }
                if (fonode.Nodes.Count != 0)
                    view.Nodes.Add(fonode);
            }
            if (pageindex !=0)
                Feltetelrendezes(pageindex);
            else
                Egyebrendezes(pageindex);
            AlbuttonokAllit(pageindex, HozferJog);
        }
        private void Egyebrendezes(int tabindex)
        {
            string combonev = "";
            DataGridView gridview = (DataGridView)gridviewk[tabindex];
            DataRow dr = null;
            string paramoszlopnev = "";
            string filt = parameterview.RowFilter;
            DataView dataview = (DataView)osszesview[tabindex];
            DataTable table = dataview.Table;
            ArrayList comboarray = (ArrayList)osszescombo[tabindex];
            ArrayList paramoszloparray = combooszlopok;
            ComboBox egycombo;
            string text;
            int drindex = -1;
            int textindex = -1;
            string sorrendsorsz = "0";
            string oszlopsorsz = "0";
            bool osszegzendo = false;
            bool atlagolando = false;
            bool csakosszegsorba = false;

            Label mezolabel;
            if (listae)
                mezolabel = label9;
            else
                mezolabel = label21;
            int sorindex = aktivsorindex[tabindex];
            if (sorindex >= dataview.Count)
            {
                sorindex = dataview.Count - 1;
                aktivsorindex[tabindex] = sorindex;
            }
            if (sorindex == -1 && dataview.Count != 0)
            {
                sorindex = 0;
                aktivsorindex[tabindex] = 0;
            }
            if (sorindex != -1)
            {
                dr = dataview[sorindex].Row;
                mezolabel.Text = dr["MEZONEVE"].ToString();
            }
            for (int i = 0; i < comboarray.Count; i++)
            {
                egycombo = (ComboBox)comboarray[i];
                egycombo.Tag = "0";
                FakUserInterface.ErrorProvider.SetError(egycombo, "");
                combonev = egycombo.Name.Substring(0, egycombo.Name.Length - 1);
                paramoszlopnev = paramoszloparray[i].ToString();
                text = egycombo.Text;
                if (dr == null)
                {
                    egycombo.Text = "";
                    egycombo.Enabled = false;
                }
                else
                {
                    egycombo.Enabled = true;
                    text = dr[paramoszlopnev].ToString();
                    egycombo.Text = text;
                    switch (combonev)
                    {
                        case "sorrendsorszam":      // sorrend szintje
                            if (dr["SORRENDBE"].ToString() == "N")
                            {
                                egycombo.Text = "0";
                                text = "0";
                                dr[paramoszlopnev] = text;
                                egycombo.Enabled = false;
                            }
                            sorrendsorsz = text;
                            break;
                        case "osszegfokkepzes":
                            if (sorrendsorsz == "0")
                            {
                                egycombo.Enabled = false;
                                if (text == "Igen")
                                {
                                    dr[paramoszlopnev] = "Nem";
                                    text = "Nem";
                                    egycombo.Text = text;
                                }
                            }
                            else
                                egycombo.Enabled = true;
                            break;
                        case "atlagolando":
                            if (dr["ATLAGBA"].ToString() == "N")
                            {
                                egycombo.Text = "Nem";
                                text = "Nem";
                                egycombo.Enabled = false;
                            }
                            if (egycombo.Text == "Nem")
                                atlagolando = false;
                            else
                                atlagolando = true;
                            break;
                        case "osszegzendo":
                            if (dr["OSSZEGBE"].ToString() == "N")
                            {
                                egycombo.Text = "Nem";
                                text = "Nem";
                                dr[paramoszlopnev] = text;
                                egycombo.Enabled = false;
                            }
                            if (egycombo.Text == "Nem")
                                osszegzendo = false;
                            else
                                osszegzendo = true;
                            break;
                        case "csakosszegsor":
                            if (!osszegzendo && !atlagolando && sorrendsorsz == "0")
                            {
                                egycombo.Text = "Nem";
                                text = "Nem";
                                dr[paramoszlopnev]=text;
                                egycombo.Enabled = false;
                            }
                            if (text == "Igen")
                                csakosszegsorba = true;
                            break;
                        case "oszlopsorszam":
                            oszlopsorsz = text;
                            if (sorrendsorsz != "0" && csakosszegsorba || !listae && dr["MATRIXPONTBA"].ToString() == "N")
                            {
                                egycombo.Enabled = false;
                                egycombo.Text = "0";
                                text = "0";
                                dr[paramoszlopnev] = text;
                            }
                            break;
                    }
                    egycombo.SelectionLength = 0;
                    egycombo.SelectionStart = 0;
                }
            }
            for (int i = 0; i < comboarray.Count; i++)
            {
                egycombo = (ComboBox)comboarray[i];
                combonev = egycombo.Name.Substring(0, egycombo.Name.Length - 1);
                paramoszlopnev = paramoszloparray[i].ToString();
                text = egycombo.Text;
                if (text == "")
                    text = "1";
                parameterview.RowFilter = paramfilter[i];
                if (egycombo.Name.Contains("sorszam"))
                {
                    parameterview.Sort = paramoszlopnev;
                    textindex = Convert.ToInt32(text);
                    if (textindex > dataview.Count)
                    {
                        textindex = dataview.Count;
                        text = textindex.ToString();
                        egycombo.Text = text;
                    }

                    DataRow egydr = null;
                    int egytext = 0;
                    int maxindex = 0;
                    egycombo.Items.Clear();
                    for (int j = 0; j <= dataview.Count; j++)
                    {
                        if (egycombo.Name.Contains("sorrend") || listae && j <= 10 || !listae && j <= 2)
                        {
                            egycombo.Items.Add(j);
                            maxindex = j;
                            if (j != dataview.Count)
                            {
                                egydr = dataview[j].Row;
                                if (egydr == dr)
                                    drindex = j;
                            }
                        }
                    }
                    string sortstring = dataview.Sort;
                    dataview.Sort = "";
                    for (int j = 0; j < dataview.Count; j++)
                    {
                        egydr = dataview[j].Row;
                        egytext = Convert.ToInt32(egydr[paramoszlopnev].ToString());
                        if (egytext > maxindex)
                            egydr[paramoszlopnev] = maxindex;
                    }
                    dataview.Sort = sortstring;
                }
                egycombo.SelectionLength = 0;
                egycombo.SelectionStart = 0;
            }
            parameterview.RowFilter = "";
            parameterview.Sort = "";
            if (sorindex != -1)
            {
                gridview.SelectedRows[0].Selected = false;
                gridview.Rows[sorindex].Selected = true;
            }
        }

        private void Feltetelrendezes(int tabindex)
        {
            bool megvan = false;
            DataRow dr = null;
            string colname = "";
            string feltcolname = "";
            ComboBox zarozarojel = null;
            ComboBox nyitozarojel = null;
            ComboBox elsoelem = null;
            ComboBox masodikelem = null;
            ComboBox esvagy = null;
            DataView dataview = (DataView)osszesview[tabindex];
            DataTable table = dataview.Table;
            ArrayList comboarray = (ArrayList)osszescombo[tabindex];
            int sorindex = -1;
            string[] masodikelemitems = null;
            DataRow aktivsor = null;
            string aktivmezonev = "";
            switch (tabindex)
            {
                case 1:
                    feltcolname = "FELTETELBE";
                    break;
                case 2:
                    feltcolname = "SORFELTETELBE";
                    break;
                case 3:
                    feltcolname = "OSZLOPFELTETELBE";
                    break;
            }
            sorindex = aktivsorindex[tabindex];
            parameterview.RowFilter = feltcolname + "='I'";
            bool volttorles = false;
            if (sorindex != -1)
            {
                if (parameterview.Count == 0)
                {
                    table.Clear();
                    sorindex = -1;
                    aktivsorindex[tabindex] = -1;
                }
                else
                {
                    aktivsor = dataview[sorindex].Row;
                    aktivmezonev = aktivsor["ELSOELEM"].ToString();
                    for (int i = 0; i < dataview.Count; i++)
                    {
                        megvan = false;
                        DataRow drview = dataview[i].Row;
                        string mezonev = drview["ELSOELEM"].ToString();
                        for (int j = 0; j < parameterview.Count; j++)
                        {
                            dr = parameterview[j].Row;
                            if (dr["MEZONEVE"].ToString() == mezonev)
                            {
                                megvan = true;
                                break;
                            }
                        }
                        if (!megvan)
                        {
                            if (mezonev == aktivmezonev)
                            {
                                aktivsor = null;
                                sorindex = -1;
                                aktivsorindex[tabindex] = -1;
                            }
                            drview.Delete();
                            volttorles = true;
                            i = -1;
                        }
                    }

                }
                if (volttorles && aktivsor != null)
                {
                    for (int i = 0; i < dataview.Count; i++)
                    {
                        dr = dataview[i].Row;
                        if (dr["ELSOELEM"].ToString() == aktivmezonev)
                        {
                            sorindex = i;
                            aktivsorindex[tabindex] = sorindex;
                            break;
                        }
                    }
                }
            }
            bool nyito = false;
            bool zaro = false;
            for (int j = 0; j < comboarray.Count; j++)
            {
                ComboBox egycombo = (ComboBox)comboarray[j];
                egycombo.Tag = "0";
                FakUserInterface.ErrorProvider.SetError(egycombo,"");
                colname = egycombo.Name.Substring(0, egycombo.Name.Length - 1);
                string egyitem;
                switch (colname)
                {
                    case "nyitozarojel":
                        nyitozarojel = egycombo;
                        nyito = false;
                        if (aktivsor != null)
                        {
                            egyitem = aktivsor["NYITOZAROJEL"].ToString();
                            if (egyitem != "")
                                nyito = true;
                            egycombo.SelectedIndex = egycombo.Items.IndexOf(egyitem);
                        }
                        else
                            egycombo.SelectedIndex = 0;
                        egycombo.Text = egycombo.Items[egycombo.SelectedIndex].ToString();
                        break;

                    case "elsoelem":
                        elsoelem = egycombo;
                        egyitem = "";
                        if (aktivsor != null)
                            egyitem = aktivsor["ELSOELEM"].ToString();
                        elsoelem.Items.Clear();
                        elsoelem.Text = "";
                        for (int k = 0; k < parameterview.Count; k++)
                        {
                            dr = parameterview[k].Row;
                            string egydritem = dr["MEZONEVE"].ToString();
                            int l = elsoelem.Items.Add(egydritem);
                            if (egyitem == egydritem || egyitem == "")
                            {
                                egyitem = egydritem;
                                megvan = true;
                                elsoelem.Text = egydritem;
                                elsoelem.SelectedIndex = l;
                                if (dr["COMBOELEMEK"].ToString() != "")
                                    masodikelemitems = dr["COMBOELEMEK"].ToString().Split(vesszo);
                            }
                        }
                        break;
                    case "relacio":
                        egyitem = "";
                        if (aktivsor != null)
                            egyitem = aktivsor["RELACIO"].ToString();
                        else
                            egycombo.SelectedIndex = 0;
                        if (aktivsor != null)
                            egycombo.SelectedIndex = egycombo.Items.IndexOf(egyitem);
                        egycombo.Text = egycombo.Items[egycombo.SelectedIndex].ToString();
                        if (egycombo.Text.Contains("Mind"))
                        {
                            masodikelems.Items.Clear();
                            masodikelems.Text = "";
                            masodikelems.Enabled = false;
                        }
                        else
                            masodikelems.Enabled = true;
                        break;
                    case "masodikelem":
                        egyitem = "";
                        masodikelem = egycombo;
                        if (egycombo.Enabled)
                        {
                            if (aktivsor != null)
                                egyitem = aktivsor["MASODIKELEM"].ToString();
                            masodikelem.Items.Clear();
                            if (masodikelemitems != null)
                            {
                                masodikelem.Items.AddRange(masodikelemitems);
                                int l = masodikelem.Items.IndexOf(egyitem);
                                if (l == -1)
                                    masodikelem.SelectedIndex = 0;
                                else
                                    masodikelem.SelectedIndex = l;
                                masodikelem.Text = masodikelem.SelectedItem.ToString();
                            }
                            else
                                masodikelem.Text = egyitem;
                        }
                        break;
                    case "zarozarojel":
                        zarozarojel = egycombo;
                        zaro = false;
                        if (aktivsor != null)
                        {
                            egyitem = aktivsor["ZAROZAROJEL"].ToString();
                            if (egyitem != "")
                                zaro = true;
                            zarozarojel.SelectedIndex = zarozarojel.Items.IndexOf(egyitem);
                        }
                        else
                            zarozarojel.SelectedIndex = 0;
                        zarozarojel.Text = zarozarojel.Items[zarozarojel.SelectedIndex].ToString();
                        break;
                    case "esvagy":
                        esvagy = egycombo;
                        if (aktivsor != null)
                        {
                            egyitem = aktivsor["ESVAGY"].ToString();
                            esvagy.SelectedIndex = esvagy.Items.IndexOf(egyitem);
                        }
                        else
                            esvagy.SelectedIndex = 0;
                        esvagy.Text = esvagy.Items[esvagy.SelectedIndex].ToString();
                        break;
                }
                egycombo.SelectionLength = 0;
                egycombo.SelectionStart = 0;
            }
            parameterview.RowFilter = "";
            if (nyito)
                zarozarojel.Enabled = false;
            else
                zarozarojel.Enabled = true;
            if (zaro)
                nyitozarojel.Enabled = false;
            else
                nyitozarojel.Enabled = true;
        }

        private void Tablasorbabe(string beazontip, string mezonev, string szoveg)
        {
            DataRow dr = parametertabla.NewRow();
            Tablainfo tabinfo = FakUserInterface.GetByAzontip(beazontip);
            ColCollection combocols = tabinfo.ComboColumns;
            Tablainfo leirotabinfo = tabinfo.LeiroTablainfo;
            DataColumn column;
            string colname;
            string filtmezonev = "";
            Type datatype;
            string filt = leirotabinfo.DataView.RowFilter;
            for (int i = 0; i < parametertabla.Columns.Count; i++)
            {
                column = parametertabla.Columns[i];
                colname = column.ColumnName;
                for (int j = 0; j < neminitoszlopok.Length; j++)
                {
                    if (colname == neminitoszlopok[j])
                    {
                        dr[colname] = "Nem";
                        break;
                    }
                }
                for (int j = 0; j < nullainitoszlopok.Length; j++)
                {
                    if (colname == nullainitoszlopok[j])
                    {
                        dr[colname] = 0;
                        break;
                    }
                }
                datatype = column.DataType;
                if (colname == "AZONTIP")
                    dr[colname] = beazontip;
                else if (colname == "MEZONEV")
                {
                    dr[colname] = mezonev;
                    filtmezonev = mezonev;
                }
                else if (colname == "MEZONEVE")
                    dr[colname] = szoveg;
                else
                {
                    for (int j = 0; j < szurofeltetel.Length; j++)
                    {
                        string egyfelt = szurofeltetel[j];
                        if (egyfelt.Contains(colname))
                        {
                            leirotabinfo.DataView.RowFilter = "ADATNEV='" + filtmezonev + "' and " + szurofeltetel[j];
                            if (leirotabinfo.DataView.Count == 0)
                                dr[colname] = "N";
                            else
                                dr[colname] = "I";
                            break;
                        }
                    }
                }
            }
            dr["COMBOELEMEK"] = "";
            Cols combocol = combocols[mezonev];
            if (combocol != null)
            {
                string egyelem = "";
                string[] osszesszov = combocol.Combo_Info.ComboSzovinfoAll();
                for (int i = 0; i < osszesszov.Length; i++)
                {
                    if (egyelem != "")
                        egyelem += ",";
                    egyelem += osszesszov[i];
                }
                dr["COMBOELEMEK"] = egyelem;
            }

            parametertabla.Rows.Add(dr);
            leirotabinfo.DataView.RowFilter = filt;
        }
        /// <summary>
        /// Node torles
        /// </summary>
        /// <param name="node"></param>
        public virtual void NodeRemove(TreeNode node)
        {
            TreeView view = kelltreeview;
            TreeNode kifonode;
            TreeNode mezonode;
            string mezonev;
            string kiazontip;
            TreeNode kiegynode = null;
            if (node.Nodes.Count != 0)
            {
                kifonode = node;
            }
            else
            {
                kifonode = node.Parent;
                kiegynode = node;
            }
            kiazontip = kifonode.Tag.ToString();
            for (int i = 0; i < view.Nodes.Count; i++)
            {
                TreeNode fonode = view.Nodes[i];
                string azontip = fonode.Tag.ToString();
                if (azontip == kiazontip)
                {
                    if (kiegynode == null)
                    {
                        for (int j = 0; j < fonode.Nodes.Count; j++)
                        {
                            mezonode = fonode.Nodes[j];
                            split = mezonode.Tag.ToString().Split(nyil);
                            mezonev = split[2];
                            Tablasorbolki(kiazontip, mezonev, mezonode.Text);
                        }
                        view.Nodes.Remove(fonode);
                        break;
                    }
                    else
                    {
                        for (int j = 0; j < fonode.Nodes.Count; j++)
                        {
                            mezonode = fonode.Nodes[j];
                            if (mezonode.Tag == kiegynode.Tag)
                            {
                                split = mezonode.Tag.ToString().Split(nyil);
                                mezonev = split[2];
                                Tablasorbolki(kiazontip, mezonev, mezonode.Text);
                                fonode.Nodes.RemoveAt(j);
                                if (fonode.Nodes.Count == 0)
                                    view.Nodes.Remove(fonode);
                                break;
                            }
                        }
                    }
                }
            }
            if (pageindex !=0)
                Feltetelrendezes(pageindex);
            else
                Egyebrendezes(pageindex);
            AlbuttonokAllit(pageindex, HozferJog);
        }
        private void Tablasorbolki(string kiazontip, string mezonev, string szoveg)
        {
            parameterview.RowFilter = "AZONTIP='" + kiazontip + "' AND MEZONEV='" + mezonev + "'";
            parameterview[0].Row.Delete();
            parameterview.RowFilter = "";
        }
        private void listaba_Click(object sender, EventArgs e)
        {
            if (FakUserInterface.EventTilt)
                return;
            if (alaptreenode != null)
            {
                NodeAdd(alaptreenode);
                Tabinfo.Modositott = true;
                VerziobuttonokAllit(HozferJog);
            }
            alaptreenode = null;

        }

        private void listabol_Click(object sender, EventArgs e)
        {
            if (FakUserInterface.EventTilt)
                return;
            if (kelltreenode != null)
            {
                NodeRemove(kelltreenode);
                Tabinfo.Modositott = true;
                VerziobuttonokAllit(HozferJog);
            }
            kelltreenode = null;
        }

        private void alaptreeview_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (FakUserInterface.EventTilt)
                return;
            alaptreenode = e.Node;
        }

        private void kelltreeview_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (FakUserInterface.EventTilt)
                return;
            kelltreenode = e.Node;
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (!FakUserInterface.EventTilt)
            {
                pageindex = e.TabPageIndex;
                DataView view = (DataView)osszesview[pageindex];
                if (pageindex != 0)
                    Feltetelrendezes(pageindex);
                else
                    Egyebrendezes(pageindex);
                AlbuttonokAllit(pageindex, HozferJog);
            }
        }
        public void combo_DropDownClosed(object sender, EventArgs e)
        {
            ComboBox combo = (ComboBox)sender;
            if (combo.SelectedIndex == -1)
                combo.Text = "";
            else
                combo.Text = combo.Items[combo.SelectedIndex].ToString();
            combo.SelectionLength = 0;
            combo.SelectionStart = 0;
            Hibavizsg(combo, pageindex);
            AlbuttonokAllit(pageindex, HozferJog);
        }
        private void combo_SelectionChangeComitted(object sender, EventArgs e)
        {
            if (FakUserInterface.EventTilt)
                return;
            ComboBox combo = (ComboBox)sender;
            combo.Text = combo.SelectedItem.ToString();
            combo.SelectionLength = 0;
            combo.SelectionStart = 0;
            Hibavizsg(combo, pageindex);
            AlbuttonokAllit(pageindex, HozferJog);
        }
        private bool Hibavizsg()
        {
            bool hiba = false;
            int ig=2;
            if(!listae)
                ig =4;
            for (int i = 0; i < ig; i++)
            {
                ArrayList comboar = (ArrayList)osszescombo[i];
                for (int j = 0; j < comboar.Count; j++)
                {
                    ComboBox combo = (ComboBox)comboar[j];
                    if(Hibavizsg(combo,i)!="")
                        hiba=true;
                }
            }
            return hiba;
        }
        private bool Hibavizsg(int tabindex)
        {
            bool hiba = false;
            ArrayList comboar = (ArrayList)osszescombo[tabindex];
            for (int j = 0; j < comboar.Count; j++)
            {
                ComboBox combo = (ComboBox)comboar[j];
                if (Hibavizsg(combo, tabindex)!="")
                    hiba = true;
            }
            return hiba;
        }
        private string  Hibavizsg(ComboBox combo, int tabindex)
        {

            ArrayList comboar = (ArrayList)osszescombo[tabindex];
            ArrayList paramoszloparray = combooszlopok;
            combo.Tag = 0;
            FakUserInterface.ErrorProvider.SetError(combo, "");
            string combonev = combo.Name;
            string rovidcombonev = combonev.Substring(0, combonev.Length - 1);
            string hiba = "";
            string sorrend = "0";
            ComboBox sorrendcombo = null;
            string osszegf = "Nem";
            ComboBox osszegfcombo = null;
            string csakosszegf = "Nem";
            ComboBox csakosszegfcombo = null;
            string oszlopsorsz = "0";
            ComboBox oszlopcombo = null;
            string rovidnev = "";
            ComboBox egycombo;
            bool osszegzendo = false;
            bool atlagolando = false;
            if (tabindex == 0)
            {
                for (int i = 0; i < comboar.Count; i++)
                {
                    egycombo = (ComboBox)comboar[i];
                    rovidnev = egycombo.Name.Substring(0, egycombo.Name.Length - 1);
                    string text = egycombo.Text;
                    switch (rovidnev)
                    {
                        case "sorrendsorszam":
                            sorrend = text;
                            sorrendcombo = egycombo;
                            break;
                        case "osszegfokkepzes":
                            osszegf = text;
                            osszegfcombo = egycombo;
                            break;
                        case "osszegzendo":
                            osszegzendo = text == "Igen";
                            break;
                        case "atlagolando":
                            atlagolando = text == "Igen";
                            break;
                        case "csakosszegsor":
                            csakosszegf = text;
                            csakosszegfcombo = egycombo;
                            break;
                        case "oszlopsorszam":
                            oszlopsorsz = text;
                            oszlopcombo = egycombo;
                            break;
                    }
                }
                switch (rovidcombonev)
                {
                    case "sorrendsorszam":
                        if (combo.Enabled)
                        {
                            if (csakosszegfcombo != null)
                                csakosszegfcombo.Enabled = true;
                            if (sorrend != "0")
                            {
                                if (osszegfcombo != null)
                                    osszegfcombo.Enabled = true;
                                if (csakosszegfcombo == null)
                                {
                                    oszlopcombo.Enabled = false;
                                    oszlopcombo.Text = "0";
                                }
                            }
                            else
                            {
                                oszlopcombo.Enabled = true;
                                if (!atlagolando && !osszegzendo && csakosszegfcombo != null)
                                {
                                    csakosszegfcombo.Text = "Nem";
                                    csakosszegfcombo.Enabled = false;
                                }
                            }
                        }
                        break;
                    case "osszegfokkepzes":
                        if (combo.Enabled)
                        {
                            if (sorrend == "0")
                            {
                                osszegfcombo.Enabled = false;
                                osszegfcombo.Text = "Nem";
                            }
                            else
                                osszegfcombo.Enabled = true;
                        }
                        break;
                    case "csakosszegsor":
                        if (combo.Enabled)
                        {
                            if (sorrend != "0" && csakosszegf == "Igen")//|| sorrend == "0" && csakosszegf == "Nem" && oszlopcombo.Text!="0")
                            {
                                oszlopcombo.Enabled = false;
                                oszlopcombo.Text = "0";
                            }
                        }
                        break;

                    case "oszlopsorszam":
                        break;

                }

                if (hiba == "")
                {
                    if (combonev.Contains("sorszam") && combo.Text != "0")
                    {
                        string paramoszlopnev = paramoszloparray[comboar.IndexOf(combo)].ToString();
                        for (int i = 0; i < parameterview.Count; i++)
                        {
                            if (i != aktivsorindex[tabindex])
                            {
                                DataRow dr = parameterview[i].Row;
                                if (dr[paramoszlopnev].ToString() == combo.Text)
                                {
                                    hiba = dr["MEZONEVE"].ToString() + " azonos sorszámú!";
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (combo.Name == "relacios")
                {
                    if (combo.Text.Contains("Mind"))
                    {
                        masodikelems.Items.Clear();
                        masodikelems.Text = "";
                        masodikelems.Enabled = false;
                    }
                    else
                        masodikelems.Enabled = true;
                }
                if (combo.Enabled)
                {
                    string comboname = combo.Name.Substring(0, combo.Name.Length - 1);
                    ComboBox elsoelem = null;
                    Tablainfo tabinfo;
                    string combotext = combo.Text;
                    if (aktivsorindex[tabindex] != -1)
                    {
                        string[] lehetures = new string[] { "nyitozarojel", "zarozarojel", "esvagy" };
                        combo.Tag = 0;
                        bool megvan = false;
                        for (int i = 0; i < lehetures.Length; i++)
                        {
                            if (comboname == lehetures[i])
                            {
                                megvan = true;
                                break;
                            }

                        }
                        if (!megvan && combotext == "")
                            hiba = "Nem lehet üres!";
                    }
                    if (hiba == "")
                    {
                        switch (comboname)
                        {
                            case "elsoelem":
                                for (int i = 0; i < parameterview.Count; i++)
                                {
                                    DataRow dr = parameterview[i].Row;
                                    if (dr["MEZONEVE"].ToString() == combotext)
                                    {
                                        string comboelemek = dr["COMBOELEMEK"].ToString();
                                        for (int j = 0; j < comboar.Count; j++)
                                        {
                                            egycombo = (ComboBox)comboar[j];

                                            if (egycombo.Name.Contains("masodikelem"))
                                            {
                                                string text = egycombo.Text;
                                                bool vanitems = false;
                                                if (egycombo.Items.Count != 0)
                                                    vanitems = true;
                                                egycombo.Items.Clear();
                                                if (comboelemek != "")
                                                {
                                                    split = comboelemek.Split(vesszo);
                                                    egycombo.Items.AddRange(split);
                                                    bool megvan = false;
                                                    for (int k = 0; k < split.Length; k++)
                                                    {
                                                        if (split[k] == text)
                                                        {
                                                            megvan = true;
                                                            break;
                                                        }
                                                    }
                                                    if (!megvan)
                                                    {
                                                        egycombo.SelectedIndex = 0;
                                                        egycombo.Text = egycombo.SelectedItem.ToString();
                                                    }
                                                    break;
                                                }
                                                else if (vanitems)
                                                    egycombo.Text = "";
                                            }
                                        }
                                        break;
                                    }
                                }
                                break;
                            case "masodikelem":
                                for (int i = 0; i < comboar.Count; i++)
                                {
                                    egycombo = (ComboBox)comboar[i];
                                    if (egycombo.Name.Contains("elsoelem"))
                                    {
                                        elsoelem = egycombo;
                                        break;
                                    }
                                }
                                if (elsoelem.Items.Count != 0 && elsoelem.SelectedIndex != -1)
                                {
                                    combotext = elsoelem.Items[elsoelem.SelectedIndex].ToString();
                                    for (int i = 0; i < parameterview.Count; i++)
                                    {
                                        DataRow dr = parameterview[i].Row;
                                        if (dr["MEZONEVE"].ToString() == combotext)
                                        {
                                            parametersorindexfeltelsoelembol = i;
                                            if (combo.Items.Count == 0)
                                            {
                                                string azontip = dr["AZONTIP"].ToString();
                                                tabinfo = FakUserInterface.GetByAzontip(azontip);
                                                Cols egycol = tabinfo.TablaColumns[dr["MEZONEV"].ToString()];
                                                try { Convert.ChangeType(combo.Text, egycol.DataType); }
                                                catch
                                                {
                                                    hiba = "Hibás adattipus!";
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                            case "nyitozarojel":
                                for (int i = 0; i < comboar.Count; i++)
                                {
                                    egycombo = (ComboBox)comboar[i];
                                    if (egycombo.Name.Contains("zarozarojel"))
                                    {
                                        if (combotext == "")
                                            egycombo.Enabled = true;
                                        else
                                        {
                                            egycombo.Text = "";
                                            egycombo.Enabled = false;
                                        }
                                        break;
                                    }
                                }
                                break;
                            case "zarozarojel":
                                for (int i = 0; i < comboar.Count; i++)
                                {
                                    egycombo = (ComboBox)comboar[i];
                                    if (egycombo.Name.Contains("nyitozarojel"))
                                    {
                                        if (combotext == "")
                                            egycombo.Enabled = true;
                                        else
                                        {
                                            egycombo.Text = "";
                                            egycombo.Enabled = false;
                                        }
                                        break;
                                    }
                                }
                                break;
                        }
                    }
                }
            }
            if (hiba != "")
                combo.Tag = "1";
            FakUserInterface.ErrorProvider.SetError(combo, hiba);
            return hiba ;
        }

        private void masodikelem_Validated(object sender, EventArgs e)
        {
            if (FakUserInterface.EventTilt)
                return;
            Hibavizsg((ComboBox)sender,pageindex);
            AlbuttonokAllit(pageindex, HozferJog);
        }

        private void gridview_VisibleChanged(object sender, EventArgs e)
        {
//            if (FakUserInterface.EventTilt)
//                return;
            DataGridView gridview = (DataGridView)sender;
            if (gridview.Visible)
            {
                if (pageindex != -1)
                {
                    //if (pageindex > 0)
                    //    gridview.ColumnHeadersVisible = false;
                    for (int j = 0; j < gridview.Columns.Count; j++)
                    {
                        DataGridViewColumn col = gridview.Columns[j];
                        string colname = col.DataPropertyName;
                        for (int k = 0; k < nemlathatooszlopok.Length; k++)
                        {
                            if (colname == nemlathatooszlopok[k])
                            {
                                col.Visible = false;
                                break;
                            }
                        }
                    }

                }
            }

        }

        private void GridView_SelectionChanged(object sender, EventArgs e)
        {
        }

        private void megnevezes_Leave(object sender, EventArgs e)
        {
            if (FakUserInterface.EventTilt)
                return;
            if (megnevezes.Text != tartalsor["SZOVEG"].ToString())
            {
                Tabinfo.Modositott = true;
                rogzit.Enabled = true;
            }
        }

        private void combofelso_DropDownClosed(object sender, EventArgs e)
        {
            if (FakUserInterface.EventTilt)
                return;
            ComboBox combo = (ComboBox)sender;
            string colnev = "";
            if (combo.Name.Contains("2"))
            {
                combo = comboBox2;
                colnev = "VANDATUMINT";
            }

            else if (combo.Name.Contains("3"))
            {
                combo = comboBox3;
                colnev = "TELJESHONAP";
            }
            else
            {
                combo = comboBox4;
                colnev = "TELJESEV";
            }
            string tart = combo.Items[combo.SelectedIndex].ToString().Substring(0,1);
            if (Tabinfo.DataView.Count == 0 || Tabinfo.DataView.Count!=0 && tart != Tabinfo.DataView[0].Row[colnev].ToString())
            {
                Tabinfo.Modositott = true;
                rogzit.Enabled = true;
            }

        }

        private void help_button_Click(object sender, EventArgs e)
        {
            if (FakUserInterface.EventTilt)
                return;
            string azon = help_button.Tag.ToString();
            FakUserInterface.ShowHelp(azon,false,this);
            //ShowHelp(azon);
        }
        //private void ShowHelp(string azon)
        //{
        //    FakUserInterface.HelpWindow.Helpszerkeszt(azon);
        //    FakUserInterface.HelpWindow.ShowDialog();
        //}
    }
}
