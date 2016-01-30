using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FakPlusz.Alapfunkciok;
namespace FakPlusz.Alapcontrolok
{
    /// <summary>
    /// Azon UserControlok alapja, melyeknek ket DataGridView van, egy felso megjelenitesre, meg egy also adatbevitelre
    /// </summary>
    public partial class Gridpluszinput : Alap
    {
        private int tartalsorindex = -1;
        private Tablainfo adatszolgtartal = null;
        private Tablainfo kezalkinfo;
        private Rectangle c;
        private Rectangle x;
        /// <summary>
        /// eloszor true
        /// </summary>
        public bool first = true;
        /// <summary>
        /// az eppen aktiv cella
        /// </summary>
        public DataGridViewCell aktivcell = null;
        /// <summary>
        /// az eppen aktiv cellahoz tartozo mezoinfok
        /// </summary>
        public Cols aktivcol = null;
        /// <summary>
        /// numerikus cella Style-hoz
        /// </summary>
        private DataGridViewCell tempcellnum = new DataGridViewTextBoxCell();
        /// <summary>
        /// nem numerikus cella Style-hoz
        /// </summary>
        private DataGridViewCell tempcellstr = new DataGridViewTextBoxCell();
        /// <summary>
        /// Ha a tablaban van TABLANEV nevu mezo, annak a mezotulajdonsag objectuma
        /// </summary>
        private Cols tablanevcol = null;
        /// <summary>
        /// a tablanevkent megadott vagy default tablanev (ha ezek nem uresek) tablainformacio objectuma a letezo mezonevek ellenorzesehez
        /// </summary>
        private Tablainfo tablanevtablainfo = null;
        /// <summary>
        /// ures, ha nincs tablanev
        /// </summary>
        private string tablanev = "";

        private Tablainfo termtablainfo;
        private ArrayList comboitems = new ArrayList();
        private bool lehetinput = false;
        /// <summary>
        /// TABLANEV nevu mezo infoi vagy null
        /// </summary>
        public MezoTag tablanevtag = null;
        private MezoTag szulotablatag = null;
        private bool adatszolg = false;
        private MezoTag kezelotag = null;
        private Cols kezeloidcol = null;
//        private bool kellfej = false;
//        private bool kelllab = false;
        /// <summary>
        /// Objectum letrehozasa
        /// </summary>
        public Gridpluszinput()
        {
            InitializeComponent();
            tempcellnum.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            tempcellstr.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }
        /// <summary>
        /// Inicializalas (minden aktivizalasnal)
        /// </summary>
        public override void AltalanosInit()
        {
            tartalsorindex = -1;
            tartalinfo = null;
            bool valt = UjTag;
            if (!valt)
            {
                valt = !Tabinfo.KellVerzio && ValtozasLekerdezExcept(new string[] { "Verziovaltozas" }).Count != 0 ||
                    Tabinfo.KellVerzio && ValtozasLekerdez().Count != 0;
            }
            //if (!Tervezoe)
            //    Hivo.Hivo.AltalanosInit();
            base.AltalanosInit();
            if (valt)
                termtablainfo = FakUserInterface.GetBySzintPluszTablanev("R", "TABLANEVEK");
            else if(Tabinfo.Tablanev != "BASE")
                Inputtablaba();
            ok.Visible = false;
            if (Tabinfo.HozferJog == HozferJogosultsag.Irolvas || Beszurhat)
            {
                Base karb = null;
                bool modositott = false;
                if (KarbantartoPage != null && KarbantartoPage.Controls.Count != 0)
                {
                    karb = (Base)KarbantartoPage.Controls[0];
                    modositott = karb.Modositott;
                }
                if (!modositott && (Leiroe || !verzioe || Tabinfo.ViewSorindex == Tabinfo.DataView.Count - 1))
                    ok.Visible = true;
            }
            if (valt)
            {
                UjTag = false;
                ValtozasTorol();
                first = true;
                comboBox1.Tag = null;
                comboitems = new ArrayList();
                tablanevtag = null;
                szulotablatag = null;
                if (!Tervezoe)
                {
                    kezeloidcol = Tabinfo.ComboColumns["KEZELO_ID"];
                    if (kezeloidcol != null)
                    {
                        if (kezeloidcol.ReadOnly)
                            kezeloidcol = null;
                        else
                        {
                            kezelotag = new MezoTag(Tabinfo, "KEZELO_ID", FakUserInterface, null, null, null, null);
                            kezelotag.Control = comboBox1;
                            kezalkinfo = FakUserInterface.GetOsszef("U", "KezeloAlkalm");
                        }

                    }
                }
                if (Tabinfo.Tablanev == "TABLANEVEK" || Tabinfo.Tablanev == "USERADATSZOLG")
                {
                    szulotablatag = new MezoTag(Tabinfo, "SZULOTABLA", FakUserInterface, null, null, null, null);
                    szulotablatag.Control = comboBox1;
                }
                if(Tabinfo.Tablanev=="TARTAL")
                {
                    aktivcol = Tabinfo.TablaColumns["TABLANEV"];
                    if (aktivcol.Comboe)
                    {
                        tablanevtag = new MezoTag(Tabinfo, "TABLANEV", FakUserInterface, null, null, null, null);
                        comboBox1.Tag = tablanevtag;
                        tablanevtag.Control = comboBox1;
                        Comboinfok cinfo = aktivcol.Combo_Info;
                        string szint = Tabinfo.Azonositok.Szint;
                        if (Tabinfo.TermSzarm.Trim() == "T")
                        {
                            string savsort = termtablainfo.DataView.Sort;
                            termtablainfo.DataView.RowFilter = "SZINT = '" + szint + "'";
                            termtablainfo.DataView.Sort = termtablainfo.Azonositok.Combosort;
                            ArrayList idarray = new ArrayList();
                            for (int i = 0; i < termtablainfo.DataView.Count; i++)
                            {
                                DataRow row = termtablainfo.DataView[i].Row;
                                idarray.Add(row["SZOVEG"].ToString());
                            }
                            string[] idk = (string[])idarray.ToArray(typeof(string));
                            FakUserInterface.Comboinfoszures(comboBox1, idk, true);
                            for (int i = 0; i < comboBox1.Items.Count; i++)
                                comboitems.Add(comboBox1.Items[i]);
                            termtablainfo.DataView.RowFilter = "";
                            termtablainfo.DataView.Sort = savsort;
                        }
                        comboBox1.Tag = null;
                    }
                }
                Inputtabla.GridView = dataGridView2;
                int maxlen = 0;
                int sorszovlen = 0;
                for (int i = 0; i < Tabinfo.InputColumns.Count; i++)
                {
                    Cols egyinpcol=Tabinfo.InputColumns[i];
                    int j = egyinpcol.Sorszov.Length;
                    if (j > sorszovlen)
                        sorszovlen = j;
                    if (egyinpcol.Comboe)
                    {
                        if (egyinpcol.Combo_Info != null)
                        {
                            if (egyinpcol.Combo_Info.Maxhossz > maxlen)
                                maxlen = egyinpcol.Combo_Info.Maxhossz;
                        }
                        else if(egyinpcol.ComboAzontipCombo!=null)
                        {
                            if (egyinpcol.ComboAzontipCombo.Maxhossz > maxlen)
                                maxlen = egyinpcol.ComboAzontipCombo.Maxhossz;
                        }
                    }
                    else
                    {
                        int szovlen = egyinpcol.InputMaxLength;
                        if (szovlen > 50)
                            szovlen = 50;
                        if (szovlen > maxlen)
                            maxlen = szovlen;
                    }
                }
                if (maxlen > 50)
                    maxlen = 50;
                comboBox1.Visible = false;
                dataGridView2.Columns[0].Width = sorszovlen * 9;
                dataGridView2.Columns[1].Width = maxlen * 9;
                comboBox1.Width = dataGridView2.Columns[1].Width;
                hibaszov = Inputtabla.Hibaszov;
                valtozott = Inputtabla.Valtozott;
                dataGridView2.Dock = DockStyle.Fill;
            }
            if (Tabinfo.Tablanev != "BASE")
                Inputtablaba();
        }
        /// <summary>
        /// numerikus/nemnumerikus cellak Style property-jenek allitasa
        /// </summary>
        public override void Tempcellini()
        {
            if (InputColumns != null)
            {
                for (int i = 0; i < InputColumns.Count; i++)
                {
                    Cols egycol = InputColumns[i];
                    DataGridViewRow dgrow = dataGridView2.Rows[i];
                    DataGridViewCell dcell = dgrow.Cells[1];
                    if (!egycol.Comboe)
                    {
                        if (egycol.Numeric(egycol.DataType))
                            dcell.Style.ApplyStyle(tempcellnum.Style);
                        else
                            dcell.Style.ApplyStyle(tempcellstr.Style);
                    }
                    dcell = dgrow.Cells[0];
                    if (egycol.ToolTip != "")
                        dcell.ToolTipText = egycol.ToolTip;
                    else
                        dcell.ToolTipText = dcell.Value.ToString();
                }
            }
        }
        /// <summary>
        /// felso GridView-ban kivalasztott sor felkinalasa modositasra/megtekintesre az also GridView-ban
        /// </summary>
        public override void Inputtablaba()
        {
            Inputtablaba("");
        }
        /// <summary>
        /// felso GridView-ban kivalasztott sor felkinalasa modositasra/megtekintesre az also GridView-ban
        /// lepesei: az also buttonok allitasa, Azonositoszoveg osszeallitasa
        ///          InputtablabaTovabb() eljaras hivasa
        /// </summary>
        /// <param name="azonszoveg">
        /// ha ures: Uj sor eseten az azonositoszoveg:Uj sor 
        /// ha nem: 
        ///   nem uj sor eseten
        ///     Tabinfo.HozferJog tartalma szerint kiegeszul
        ///     modositasa vagy megtekintese szoveggel
        ///   uj sor eseten marad az alap
        /// </param>
        public virtual void Inputtablaba(string azonszoveg)
        {
            SetSelectedRow(Tabinfo.ViewSorindex);
            comboBox1.Tag = null;
            if (!verzioe)
                ok.Visible = true;
            else if (Tabinfo.ViewSorindex == -1 && Beszurhat || Tabinfo.ViewSorindex == DataView.Count - 1)
                ok.Visible = true;
            else
                ok.Visible = false;
            DataRow dr;
            if (Tabinfo.ViewSorindex == -1)
            {
                lehetinput = true;
                dr = null;
                Beszur = true;
                if (azonszoveg == "")
                    Azonositoszoveg = "Uj sor:";
                else
                    Azonositoszoveg = azonszoveg;
                elozo.Enabled = false;
                kovetkezo.Enabled = false;
                eleszur.Enabled = false;
                mogeszur.Enabled = false;
                torolalap.Enabled = false;
            }
            else
            {
                dr = Tabinfo.AktualViewRow;
                if (Tabinfo.Tablanev == "KIAJANL")
                    Tabinfo.InputColumns["RSORSZAM"].Combo_Info = FakUserInterface.ComboInfok.ComboinfoKeres(dr["AZONTIP"].ToString(), Tabinfo, Tabinfo.InputColumns["RSORSZAM"], comboBox1);
                //if (cegalktag != null && cegalkkezinfo != null && cegalkkezinfo.Osszefinfo.Adattabla.Rows.Count != 0)
                //{
                //    comboBox1.Tag = cegalktag;
                //    FakUserInterface.Comboinfoszures(comboBox1, cegalkkezinfo.Osszefinfo, new object[] { new object[] { dr["CEG_ID"].ToString(), dr["ALKALMAZAS_ID"] }, "" });
                //}
                if (szovegcol != -1)
                    Azonositoszoveg = Tabinfo.AktualViewRow[szovegcol].ToString();
                else
                    Azonositoszoveg = "";
                if (Tabinfo.HozferJog == HozferJogosultsag.Irolvas)
                {
                    string owner = "";
                    if (FakUserInterface.Enyem || FakUserInterface.Alkalmazas!="TERVEZO")
                        lehetinput = true;
                    else
                    {
                        if (Tabinfo.Ownernev != "")
                            owner = dr[Tabinfo.Ownernev].ToString();
                        if (owner != "" && owner != "0")
                            lehetinput = true;
                        else if (Tabinfo.Tablanev == "TARTAL")
                            lehetinput = false;
                        else if (Tabinfo.TablaColumns.IndexOf("ENYEM") == -1 || dr["ENYEM"].ToString() == "N")
                            lehetinput = true;
                        else
                            lehetinput = false;
                    }
                    if (verzioe)
                    {
                        dataGridView2.Rows[0].Cells[1].ReadOnly = Tabinfo.ViewSorindex == 0;
                    }
                }
                else
                    lehetinput=false;
                Beszur = false;
                if (Tabinfo.ViewSorindex == 0)
                    elozo.Enabled = false;
                else
                    elozo.Enabled = true;
                if (Tabinfo.ViewSorindex == DataView.Count - 1)
                    kovetkezo.Enabled = false;
                else
                    kovetkezo.Enabled = true;
                eleszur.Enabled = true;
                mogeszur.Enabled = true;
                torolalap.Enabled = true;
            }
            tablanevcol = TablaColumns["TABLANEV"];
            tablanevtablainfo = null;
            AdatColumnTomb = null;
            if (tablanevcol != null)
            {
                if (tablanevcol.Csakolvas || Beszur)
                    tablanev = tablanevcol.DefaultValue.ToString().Trim();
                else
                    tablanev = dr["TABLANEV"].ToString().Trim();
                if (tablanev != "")
                {
                    tablanevtablainfo = FakUserInterface.GetBySzintPluszTablanev(Tabinfo.Szint, tablanev);
                    if (tablanevtablainfo != null)
                        AdatColumnTomb = tablanevtablainfo.Adattabla.Columns;
                    else
                    {
                        string szintek = "RUC";
                        for (int i = 0; i < szintek.Length; i++)
                        {
                            string szint = szintek.Substring(i, 1);
                            AdatColumnTomb = Tabinfo.Select(szint, tablanev);
                            if (AdatColumnTomb != null)
                                break;
                        }
                    }
                }
            }
            InputtablabaTovabb();
            if (verzioe && Tabinfo.ViewSorindex != -1 && Tabinfo.ViewSorindex != Tabinfo.DataView.Count - 1 || Tabinfo.KellVerzio && Tabinfo.LezartVersion || Tabinfo.HozferJog == HozferJogosultsag.Csakolvas)
            {
                rogzit.Visible = false;
                eleszur.Visible = false;
                mogeszur.Visible = false;
                torolalap.Visible = false;
                dataGridView2.ReadOnly = true;
            }
            else
            {
                rogzit.Visible = true;
                rogzit.Enabled = Tabinfo.Valtozott;
                dataGridView2.ReadOnly = false;
            }
            if (Tabinfo.Tablanev == "USERCONTROLNEVEK" && Tabinfo.ViewSorindex != -1)
            {
                dataGridView2.Rows[1].Cells[0].ToolTipText = Tabinfo.AktualViewRow["TOOLTIP"].ToString();
            }
            if (this.Name != "Tooltipallit")
            {
                if (lehetinput)
                {
                    if (Tabinfo.ViewSorindex == -1 || Tabinfo.Modosithat)
                    {
                        if (Azonositoszoveg == "")
                            Azonositoszoveg = "Karbantartás";
                        else if(!Beszur)
                            Azonositoszoveg += " karbantartása";
                    }
                    else if (Azonositoszoveg == "")
                        Azonositoszoveg += "Megtekintés";
                    else
                        Azonositoszoveg += " megtekintése";
                }
                else if (Azonositoszoveg == "")
                    Azonositoszoveg += "Megtekintés";
                else
                    Azonositoszoveg += " megtekintése";
            }
            if (Tabinfo.ViewSorindex == -1 && !Beszurhat)
                label2.Text = "";
            else
                label2.Text = Azonositoszoveg;
        }
        /// <summary>
        /// dataGridView2 inputcellak tartalmanak beallitasa, hibavizsgalat a cellakra, valtozasok vizsgalata
        /// </summary>
        public override void InputtablabaTovabb()
        {
            Cols egycol;
            DataGridViewCell dcell;
            string tartal;
            string tartal1;
            DataRow dr = null;
            bool valt = false;
            bool hiba = false;
            if (!lehetinput)
                ok.Visible = false;
            else
                ok.Visible = true;
            if (Tabinfo.ViewSorindex != -1)
                dr = Tabinfo.AktualViewRow;
            if (Tabinfo.ViewSorindex == -1 && (Beszurhat || verzioe) || Tabinfo.ViewSorindex != -1)
            {
                for (int i = 0; i < InputColumns.Count; i++)
                {
                    egycol = InputColumns[i];
                    int j = TablaColumns.IndexOf(egycol);
                    if (!Beszur)
                        tartal = dr[j].ToString().Trim();
                    else
                        tartal = egycol.DefaultValue.ToString();
                    if (egycol.DataType == typeof(DateTime))
                    {
                        if (tartal != "")
                            tartal = Convert.ToDateTime(tartal).ToShortDateString();
                        else if (Tabinfo.ViewSorindex == -1 && verzioe)
                            tartal = oldversiondate.ToShortDateString();
                    }
                    if (egycol.Comboe)
                    {
                        if (egycol.Combo_Info == null)
                        {
                            if (egycol.ComboAzontipCombo != null)
                            {
                                egycol.ComboAzontipCombo.Combotolt(egycol, tartal);
                                tartal = egycol.ComboAktFileba;
                            }
                        }
                        else
                        {
                            if (tartal != "" && tartal != "0" || !egycol.Lehetures && !Beszur)
                            {
                                egycol.Combo_Info.Combotolt(egycol, tartal);
                                tartal = egycol.ComboAktFileba;
                                egycol.Tartalom = tartal;
                            }
                        }
                    }
                    tartal1 = tartal;
                    if (egycol.Numeric(egycol.DataType) && !egycol.Comboe && tartal == "0")
                        tartal1 = "";
                    hibaszov[i] = "";
                    dcell = dataGridView2.Rows[i].Cells[1];
                    dcell.ErrorText = "";
                    if (Beszur)
                    {
                        dcell.Value = egycol.DefaultValue;
                        if (tartal1 != "")
                            dcell.Value = tartal1;
                        //hibaszov[i] = Hibavizsg(dcell);
                        //if (hibaszov[i] != "")
                        //{
                        //    hiba = true;
                        //    dcell.ErrorText = "";
                        //}
                        if (egycol.Comboe)
                        {
                            tartal = egycol.DefaultValue.ToString();
                            if (tartal != "")
                            {
                                if (egycol.Combo_Info != null)
                                    egycol.Combo_Info.Combotolt(egycol, tartal);
                                else if(egycol.ComboAzontipCombo!=null)
                                    egycol.ComboAzontipCombo.Combotolt(egycol, tartal);
                                dcell.Value = egycol.ComboAktSzoveg;
                            }
                            else
                            {
                                egycol.ComboAktSzoveg = "";
                                egycol.ComboAktFileba = "";
                            }
                        }
                        hibaszov[i] = Hibavizsg(dcell);
                        if (hibaszov[i] != "")
                        {
                            hiba = true;
                            dcell.ErrorText = "";
                        }
                    }
                    valtozott[i] = false;
                    if (egycol.OrigTartalom == "0" && egycol.Tartalom == "")
                    {
                    }
                    else if (egycol.OrigTartalom != egycol.Tartalom && !this.Name.Contains("Naptar"))
                    {
                        valtozott[i] = true;
                        valt = true;
                    }
                    DataRow dr1 = Inputtabla.Rows[i];
                    DataColumn col = dr1.Table.Columns["TARTALOM"];
                    int len = col.MaxLength;
                    if (egycol.Comboe && (egycol.Combo_Info != null || egycol.ComboAzontipCombo != null))
                    {
                        int maxlen = 0;
                        if (egycol.Combo_Info != null)
                            maxlen = egycol.Combo_Info.Maxhossz;
                        else
                            maxlen = egycol.ComboAzontipCombo.Maxhossz;
                        if (len < maxlen)
                            col.MaxLength = maxlen;
                        dr1[1] = egycol.ComboAktSzoveg;
                    }
                    else
                    {
                        if (egycol.InputMaxLength > col.MaxLength)
                            col.MaxLength = egycol.InputMaxLength;
                        dr1[1] = tartal;
                    }
                }
                if (valt && lehetinput && !hiba)
                {
                    Tabinfo.Modositott = true;
                    Tabinfo.Changed = true;
                    ok.Enabled = true;
                }
                else
                    ok.Enabled = false;
                if (comboBox1.Visible && combocell.ColumnIndex != -1 && combocell.RowIndex != -1)
                    comboBox1.Text = combocell.Value.ToString();
                panel4.Visible = true;
                dataGridView2.Visible = true;
                dataGridView2.Refresh();
            }
            else
            {
                panel4.Visible = false;
                dataGridView2.Visible = false;
            }
            if (Tabinfo.Tablanev == "USERCONTROLNEVEK" && Tabinfo.ViewSorindex != -1)
            {
                dataGridView2.Rows[1].Cells[0].ToolTipText = Tabinfo.AktualViewRow["TOOLTIP"].ToString();
            }
        }
    
        /// <summary>
        /// Uj sor az aktualis sor ele
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void eleszur_Click(object sender, EventArgs e)
        {
            if (NemkellOk())
            {
                base.eleszur_Click(sender, e);
                Inputtablaba(Azonositoszoveg);
            }
        }
        /// <summary>
        /// Elozo sor felkinalasa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void elozo_Click(object sender, EventArgs e)
        {
            if (Tabinfo.ViewSorindex > 0 && NemkellOk())
            {
                if (!Tervezoe)
                    Hivo.Hivo.elozo_Click(Tabinfo, e);
                Tabinfo.ViewSorindex = Tabinfo.ViewSorindex - 1;
                SetSelectedRow(Tabinfo.ViewSorindex);
                Inputtablaba();
            }
        }
        /// <summary>
        /// Uj sor az aktualis moge
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void mogeszur_Click(object sender, EventArgs e)
        {
            if (NemkellOk())
            {
                base.mogeszur_Click(sender, e);
                Inputtablaba(Azonositoszoveg);
            }
        }
        /// <summary>
        /// Kovetkezo sor felkinalasa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void kovetkezo_Click(object sender, EventArgs e)
        {
            if (Tabinfo.ViewSorindex != -1 && Tabinfo.ViewSorindex < DataView.Count - 1 && NemkellOk())
            {
                Tabinfo.ViewSorindex = Tabinfo.ViewSorindex + 1;
                SetSelectedRow(Tabinfo.ViewSorindex);
                Inputtablaba();
            }
        }
        /// <summary>
        /// Aktualis sor torlese, ha lehet torolni, vagy jelzi, hogy miert nem lehet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void torolalap_Click(object sender, EventArgs e)
        {
            bool kelltorles = true;
            int nextind = Tabinfo.ViewSorindex;
            Tablainfo listatervinfo = FakUserInterface.GetBySzintPluszTablanev("R", "LISTAK");
            if (Tabinfo.ViewSorindex != -1)
            {
                DataRow dr = Tabinfo.AktualViewRow;
                long aktidentity = Tabinfo.AktIdentity;
                if (Tabinfo.Tablanev == "USERADATSZOLG")
                {
                    string aktkodtip = dr["SZOVEG"].ToString();
                    listatervinfo.DataView.RowFilter = "ADATSZOLGNEV='" + aktkodtip + "'";
                    bool vanhiv = listatervinfo.DataView.Count != 0;
                    listatervinfo.DataView.RowFilter = "";
                    if(vanhiv)
                    {
                        kelltorles = false;
                        MessageBox.Show("Listaképeknél már van rá hivatkozás!");
                    }
                    if (kelltorles)
                    {
                        for (int i = 0; i < Tabinfo.DataView.Count; i++)
                        {
                            if (i != Tabinfo.ViewSorindex)
                            {
                                DataRow egyrow = Tabinfo.DataView[i].Row;
                                if (egyrow["SZULOTABLA"].ToString() == aktkodtip )
                                {
                                    kelltorles = false;
                                    MessageBox.Show("Másik sorban már van rá hivatkozás!");
                                    break;
                                }
                            }
                        }
                    }
                    if (kelltorles)
                    {
                        adatszolgtartal = FakUserInterface.GetByAzontip("SZRATARTAL");
                        for (int i = 0; i < adatszolgtartal.DataView.Count; i++)
                        {
                            DataRow egyrow = adatszolgtartal.DataView[i].Row;
                            if (egyrow["KODTIPUS"].ToString() == aktkodtip)
                            {
                                tartalsorindex = i;
                                if (MessageBox.Show("Már van leirás az adatközlésröl!\nTörölhetö?", "", MessageBox.MessageBoxButtons.IgenNem) != MessageBox.DialogResult.Igen)
                                    kelltorles = false;
                                else
                                    adatszolgtartal.Adatsortorol(tartalsorindex);
                                break;
                            }
                        }

                    }
                }
                else if (Tabinfo.Tablanev == "BASE")
                {
                    string szint = dr["SZINT"].ToString();
                    TablainfoCollection tcoll = FakUserInterface.GetBySzint(szint);
                    string szov = "";
                    if (tcoll != null)
                    {
                        foreach(Tablainfo egyinfo in tcoll)
                        {
                            if (egyinfo.Tablanev != "TARTAL")
                            {
                                szov += egyinfo.Tablanev + "\n";
                            }
                        }
                    }
                    if (szov != "")
                    {
                        kelltorles = false;
                        MessageBox.Show("Elöbb az alábbiakat kell törölni:\n" + szov);
                    }
                }
                else if (Tabinfo.Tablanev == "TARTAL")
                {
                    // meg kell nezni, hogy torolheto-e egyaltalan
                    string azontip = dr["AZONTIP"].ToString();
                    string kodtipus = dr["KODTIPUS"].ToString();
                    string tablanev = dr["TABLANEV"].ToString();
                    string szint = Tabinfo.Szint;
                    string termszarm = Tabinfo.TermSzarm.Trim();
                    OsszefinfoCollection coll = FakUserInterface.Osszefuggesek[azontip];
                    string szov = "";
                    if (coll != null && coll.Count!=0)
                    {
                        kelltorles = false;
                        foreach (Osszefinfo egyinfo in coll)
                        {
                            switch (szint)
                            {
                                case "R":
                                    szint = "Rendszerszint/ ";
                                    break;
                                case "U":
                                    szint = "Userszint/ ";
                                    break;
                                default:
                                    szint = "Cégszint/ ";
                                    break;
                            }
                            szov += szint + egyinfo.Adattabla.Tablainfo.ParentTag.Node.Text +":\n"+egyinfo.Adattabla.Tablainfo.TablaTag.Node.Text + "\n";
                        }
                        if (!kelltorles)
                            MessageBox.Show("Elöbb az alábbiakat kell törölni:\n" + szov);
                    }
                    if (kelltorles)
                    {
                        Tablainfo egytabinfo = FakUserInterface.GetByAzontip(azontip);
                        if (egytabinfo != null)
                        {
                            if (tablanev == "ADATSZOLG")
                            {
                                egytabinfo.DataView.RowFilter = "KODTIPUS = '" + kodtipus + "'";
                                bool vanmar = egytabinfo.DataView.Count != 0;
                                egytabinfo.DataView.RowFilter = "";
                                if (vanmar)
                                {
                                    kelltorles = false;
                                    MessageBox.Show("Már van leirás az adatközlésröl!");
                                }

                            }
                            else if (FakUserInterface.Adatszintek.Contains(szint.Trim()))
                            {
                                DataTable dt = new DataTable();
                                string selstring = "";
                                if (egytabinfo.TablaColumns.IndexOf("KODTIPUS") != -1)
                                    selstring = " where KODTIPUS='" + kodtipus + "'";
                                FakPlusz.Sqlinterface.Select(dt, FakUserInterface.AktualCegconn, tablanev, selstring, "", true);
                                if (dt.Rows.Count != 0)
                                {
                                    kelltorles = false;
                                    MessageBox.Show("Már van felvitt adat,nem törölhetö!");
                                }
                            }
                            else if (egytabinfo.LehetCombo)
                            {
                                for (int i = 0; i < egytabinfo.DataView.Count; i++)
                                {
                                    egytabinfo.ViewSorindex = i;
                                    if (ComboHasznalatban(egytabinfo))
                                    {
                                        kelltorles = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                else if(kelltorles && Tabinfo.LehetCombo)
                {
                    Cols egycol = Tabinfo.TablaColumns[Tabinfo.ComboFileba];
                    kelltorles = !ComboHasznalatban(Tabinfo, null, egycol.Tartalom, egycol.Tartalom, aktidentity.ToString());
                }
                if (kelltorles)
                {
                    if (Tabinfo.AktVerzioId != Tabinfo.LastVersionId)
                    {
                        Tabinfo.AktVerzioId = Tabinfo.LastVersionId;
                        Tabinfo.Adattabla.Select();
                    }
                    base.torolalap_Click(sender, e);
                    if (Tabinfo.Modositott)
                        rogzit_Click(new object(), new EventArgs());
                }
            }
        }
        /// <summary>
        /// Egyedi_Ok() hivasa
        /// Sor modositasanak elfogadtatasa, hiba(k) eseten azok jelzese, ha nincs hiba, a modositas(ok)/beszuras
        /// eredmenye bekerul a felso gridview-ba
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void ok_Click(object sender, EventArgs e)
        {
            bool valt = false;
            ok.ToolTipText = "";
            Egyedi_Ok();
            if (!Tervezoe)
                Hivo.Hivo.ok_Click(this, e);
            for (int i = 0; i < hibaszov.Length; i++)
            {
                if (valtozott[i])
                    valt = true;
                if (hibaszov[i] != "")
                {
                    DataGridViewCell dcell = dataGridView2.Rows[i].Cells[1];
                    dcell.ErrorText = hibaszov[i];
                    if (aktivcell == dcell && comboBox1.Visible)
                        FakUserInterface.ErrorProvider.SetError(comboBox1, hibaszov[i]);
                    if (ok.ToolTipText != "")
                        ok.ToolTipText += "\n";
                    ok.ToolTipText += Inputtabla.Rows[i][0].ToString() + ":" + hibaszov[i];
                }
            }
            if(ok.ToolTipText== "")
            {
                AktivPage.Text = AktivPage.Text.Replace("!", "");
                if (valt || Tabinfo.Modositott)
                    rogzit.Enabled = true;
                if (valt)
                {
                    if (Beszur && Beszursorrend == 0)
                        Beszursorrend = 100;
                    DataRow aktualadatrow = Tabinfo.AdatsortoltInputtablabol(Tabinfo.ViewSorindex, Beszur, Beszursorrend);
                    if (Tabinfo.AlkalmazasIdColumn != null && !Tervezoe)
                        aktualadatrow["ALKALMAZAS_ID"] = FakUserInterface.AlkalmazasId;
                    Azonositoszoveg = "";
                    if(szovegcol!=-1)
                        Azonositoszoveg = Tabinfo.AktualViewRow[szovegcol].ToString();
                    if (verzioe && Beszur)
                        Tabinfo.AktualViewRow["VERZIO_ID"] = oldversionnumber;
                    Modositott = true;
                    if (!Tabinfo.Modositott)
                        Tabinfo.Modositott = true;
                    if (!verzioe && !Beszur)
                    {
                        if (Tabinfo.ViewSorindex != DataView.Count - 1)
                            Tabinfo.ViewSorindex = Tabinfo.ViewSorindex + 1;
                        else
                            Tabinfo.ViewSorindex = 0;
                    }
                    SetAktRowVisible(dataGridView1,Tabinfo);
                    VerziobuttonokAllit();
                    if (verzioe)
                    {
                        if (rogzit.Enabled)
                            rogzit.Visible = true;
                    }
                }
                Inputtablaba();
            }
        }
        /// <summary>
        /// Ha OK-t nyomunk, az ok_Click hivja, mielott a kozos ok eljarast vegrehajtja.
        /// Felul kell irni, ha kell. Az Adatszolg pl. felulirja
        /// </summary>
        public override void Egyedi_Ok()
        {
//            if(
        }
        /// <summary>
        /// Sorban tortent modositasok elfelejtese
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void elolrol_Click(object sender, EventArgs e)
        {
            if (Tabinfo.ViewSorindex == -1 && DataView.Count != 0 )
            {
                if (dataGridView1.SelectedRows.Count == 0)
                    Tabinfo.ViewSorindex = 0;
                else
                    Tabinfo.ViewSorindex = dataGridView1.SelectedRows[0].Index;
                VerziobuttonokAllit();
                Beszur = false;
            }
            Inputtablaba();
        }
        /// <summary>
        /// Jogosultsag alapjan felso buttonokat allit
        /// </summary>
        public override void VerziobuttonokAllit()
        {
            base.VerziobuttonokAllit();
            if (UjVerzio && !verzioe || Tabinfo.LezartVersion || Tabinfo.HozferJog==HozferJogosultsag.Csakolvas)
                dataGridView2.ReadOnly = true;
            else
                dataGridView2.ReadOnly = false;
        }
        /// <summary>
        /// A tabla adatbazisbeli allapotanak visszaallitasa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void elolrolalap_Click(object sender, EventArgs e)
        {
            comboBox1.Visible = false;
            base.elolrolalap_Click(sender, e);
            VerziobuttonokAllit();
            Inputtablaba();
        }
        /// <summary>
        /// rogzites, also grid toltes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void rogzit_Click(object sender, EventArgs e)
        {
            comboBox1.Visible = false;
            Rogzit();
            if (adatszolgtartal != null && tartalsorindex != -1)
                FakUserInterface.Rogzit(adatszolgtartal);
            if (Tabinfo.Tablanev == "TABLANEVEK" && Tabinfo.Szint == "R")
            {
                TablainfoCollection termtabinfok = FakUserInterface.GetCegPluszCegalattiTermTablaInfok();
                ArrayList modositandok = new ArrayList();
                TreeNode tartalnode;
                for (int i = 0; i < Tabinfo.DataView.Count; i++)
                {
                    DataRow dr = Tabinfo.DataView[i].Row;
                    string tablanev = dr["SZOVEG"].ToString();
                    string szint = dr["SZINT"].ToString();
                    string szuloszint = dr["SZULOSZINT"].ToString();
                    string szulotabla = dr["SZULOTABLA"].ToString();

                    foreach (Tablainfo egyinfo in termtabinfok)
                    {
                        if (egyinfo.Szint == szint && egyinfo.Tablanev == tablanev)
                        {
                            tartalnode = egyinfo.TablaTag.Node.Parent;
                            if (tartalnode.Parent != null)
                            {
                                Tablainfo tartalinfo = ((TablainfoTag)tartalnode.Tag).Tablainfo;
                                tartalinfo.DataView.RowFilter = "TABLANEV='" + tablanev + "' and SZINT = '" + szint + "'";
                                DataRow drt = tartalinfo.DataView[0].Row;
                                string tartalszulotabla = drt["SZULOTABLA"].ToString();
                                string tartalszuloszint = drt["SZULOSZINT"].ToString();
                                if (tartalszulotabla != szulotabla || tartalszuloszint != szuloszint)
                                {
                                    drt["SZULOSZINT"] = szuloszint;
                                    drt["SZULOTABLA"] = szulotabla;
                                    tartalinfo.Modositott = true;
                                    if (modositandok.IndexOf(tartalinfo) == -1)
                                        modositandok.Add(tartalinfo);
                                }
                                tartalinfo.DataView.RowFilter = "";
                            }
                        }
                    }
                }
                if (modositandok.Count != 0)
                {
                    Tablainfo[] infok = (Tablainfo[])modositandok.ToArray(typeof(Tablainfo));
                    FakUserInterface.UpdateTransaction(infok);
                }
            }
            VerziobuttonokAllit();
            Inputtablaba();
            Modositott = false;
            if (Tabinfo.ViewSorindex == -1)
                Azonositoszoveg = "Uj sor:";
            else if (szovegcol==-1)
                Azonositoszoveg = "Módositás:";
            else
                Azonositoszoveg = Tabinfo.AktualViewRow[szovegcol].ToString() + " módositása ";
            UjTag = true;
            AltalanosInit();
        }
        /// <summary>
        /// Sorvalasztas a felso GridView-bol
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!FakUserInterface.EventTilt)
            {
                if (NemkellOk())
                {
                    base.dataGridView1_CellMouseClick(sender, e);
                    Inputtablaba();
                }
            }
        }
        /// <summary>
        /// Uj verzio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void uj_Click(object sender, EventArgs e)
        {
            base.uj_Click(sender, e);
            Inputtablaba();
        }
        /// <summary>
        /// Ha valtoztatas utan nem nyomtunk Ok-t, udvarias kerdes
        /// </summary>
        /// <returns>
        /// true:mehet tovabb
        /// </returns>
        public virtual bool NemkellOk()
        {
            bool valt = false ;
            for (int i = 0; i < valtozott.Length; i++)
            {
                if (valtozott[i])
                {
                    valt = true;
                    break;
                }
            }
            if (!valt || MessageBox.Show(FakUserInterface.GetUzenetSzoveg("Okfelejt"), "", MessageBox.MessageBoxButtons.IgenNem)
                 != MessageBox.DialogResult.Igen)
            {
                if (valt)
                {
                    for (int i = 0; i < valtozott.Length; i++)
                    {
                        valtozott[i] = false;
                        hibaszov[i] = "";
                        dataGridView2[1, i].ErrorText = "";
                    }
                }
                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// Also GridView-ban ralepunk egy cellara. Ha nem ReadOnly, a cella lesz az altiv cella. Ha az inputcella
        /// combo jellegu, a ComboBoxunkat rahelyezzuk a cellara, beallitjuk a ComboBox valasztekot, a ComboBox
        /// select indexet a cella tartalma alapjan
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!FakUserInterface.EventTilt && e.ColumnIndex!= 0 && !dataGridView2.ReadOnly && !dataGridView2.CurrentCell.ReadOnly
                && !Inputtabla.Columns[1].ReadOnly)
            {
                aktivcell = dataGridView2.CurrentCell;
                Size cellsize = aktivcell.Size;
                DataGridViewCell dc = aktivcell;
                if (dc != null && dc.ReadOnly != true)
                {
                    aktivcol = InputColumns[dc.RowIndex];
                    string tartal = dc.Value.ToString().Trim();
                    c = dataGridView2.GetColumnDisplayRectangle(dc.ColumnIndex, true);
                    x = dataGridView2.GetRowDisplayRectangle(dc.RowIndex, true);
                    dataGridView2.Controls.Remove(comboBox1);
                    comboBox1.Visible = false;
                    combocell = null;
                    comboBox1.Text = "";
                    comboBox1.Tag = null;
                    dc.Value = tartal;
                    if (aktivcol.Comboe)
                    {
                        combocell = dc;
                        if (aktivcol.Combo_Info != null)
                        {
                            if (aktivcol.ReadOnly)
                            {
                                tartal = "";
                                dc.Value = "";
                                return;
                            }
                                aktivcol.ComboAktFileba = aktivcol.Combo_Info.GetComboAktfileba(tartal);
                        }
                        dataGridView2.Controls.Add(comboBox1);
                        if (kezeloidcol != null && kezelotag != null && aktivcol.ColumnName==kezeloidcol.ColumnName)
                        {
                            comboBox1.Tag = kezelotag;
                            comboBox1.Items.Clear();
                            string[] kezeloidk = FakUserInterface.GetSzurtOsszefIdk(kezalkinfo.Osszefinfo, new object[] { "", FakUserInterface.AlkalmazasId });
                            if (kezeloidk != null)
                            {
                                for (int i = 0; i < kezeloidk.Length; i++)
                                {
                                    int j = aktivcol.Combo_Info.ComboFileinfo.IndexOf(kezeloidk[i]);
                                    comboBox1.Items.Add(aktivcol.Combo_Info.ComboInfo[j].ToString());
                                }
                            }
                        }
                        //if (cegalktag != null) 
                        //    comboBox1.Tag = cegalktag;
                        if (aktivcol.ColumnName == "TABLANEV" && tablanevtag!=null)
                        {
                            comboBox1.Tag = tablanevtag;
                            comboBox1.Items.Clear();
                            for (int i = 0; i < comboitems.Count; i++)
                                comboBox1.Items.Add(comboitems[i]);
                        }
                        else if (aktivcol.ColumnName == "SZULOSZINT" && Tabinfo.Tablanev == "TABLANEVEK")
                        {
                            comboBox1.Items.Clear();
                            comboBox1.Items.AddRange(aktivcol.Combo_Info.ComboSzovinfoAll());
                            comboBox1.Items.Insert(0, "");
                            if (tartal == "")
                                comboBox1.SelectedIndex = 0;
                            else
                                comboBox1.SelectedIndex = aktivcol.Combo_Info.ComboInfo.IndexOf(tartal);
                        }
                        else if (aktivcol.ColumnName == "SZULOTABLA" || aktivcol.ColumnName == "FEJTABLA" || aktivcol.ColumnName == "LABTABLA")
                        {
                            comboBox1.Items.Clear();
                            string szulotablaszint = "";
                            string tablanev = "";
                            adatszolg = Tabinfo.Tablanev == "USERADATSZOLG";
                            if (!adatszolg)
                                szulotablaszint = InputColumns["SZULOSZINT"].ComboAktFileba;
                            if (szulotablaszint != "" || adatszolg)
                                tablanev = Inputtabla.Rows[Tabinfo.InputColumns.IndexOf("SZOVEG")][1].ToString();
                            if (tablanev != "")
                            {
                                comboBox1.Tag = szulotablatag;
                                if (!adatszolg)
                                    Tabinfo.DataView.RowFilter = "SZINT='" + szulotablaszint + "' and SZOVEG<>'" + tablanev + "'";
                                else
                                    Tabinfo.DataView.RowFilter = "SZOVEG<>'" + tablanev + "'";
                                if (Tabinfo.DataView.Count != 0)
                                {
                                    ArrayList idarray = new ArrayList();
                                    for (int i = 0; i < Tabinfo.DataView.Count; i++)
                                    {
                                        DataRow dr = Tabinfo.DataView[i].Row;
                                        idarray.Add(dr["SZOVEG"].ToString());
                                    }
                                    string[] idk = (string[])idarray.ToArray(typeof(string));
                                    FakUserInterface.Comboinfoszures(comboBox1, idk);
                                }
                                Tabinfo.DataView.RowFilter = "";
                            }
                            else
                            {
                                tablanev = Inputtabla.Rows[Tabinfo.InputColumns.IndexOf("SZULOTABLA")][1].ToString();
                            }
                        }
                        else if (aktivcol == kezeloidcol)
                        {
                        }
                        else if (comboBox1.Tag == null)
                        {
                            if (aktivcol.Combo_Info == null)
                                aktivcol.ComboAzontipCombo.SetComboItems(comboBox1, aktivcol, tartal);
                            else
                                aktivcol.Combo_Info.SetComboItems(comboBox1, aktivcol, tartal);
                        }
                        comboBox1.TabIndex = dataGridView2.TabIndex;
                        comboBox1.Bounds = c;
                        comboBox1.Size = cellsize;
                        comboBox1.Location = new System.Drawing.Point(comboBox1.Location.X + x.Location.X, comboBox1.Location.Y + x.Location.Y + dataGridView2.Location.Y);
                        comboBox1.Visible = true;
                        FakUserInterface.ErrorProvider.SetError(comboBox1, hibaszov[aktivcell.RowIndex]);
                        combovisible = true;
                    }
                    else
                    {
                        if (aktivcol.Numeric(aktivcol.DataType))
                            dc.Style.ApplyStyle(tempcellnum.Style);
                        else dc.Style.ApplyStyle(tempcellstr.Style);
                    }
                }
            }
            else
                aktivcol = null;
        }
        /// <summary>
        /// Cella editalasanak kezdete, csak a nem combo-jellegueknel mukodik. 
        /// Tilt minden buttont, eltarolja az aktiv cellat
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void dataGridView2_BeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (!FakUserInterface.EventTilt)
            {
                if (!Inputtabla.Columns[1].ReadOnly)
                {
                    toolStrip2.Enabled = false;
                    toolStrip1.Enabled = false;
                    DataGridViewTextBoxColumn dgcol = (DataGridViewTextBoxColumn)dataGridView2.Columns[1];
                    DataGridViewRow dgrow = dataGridView2.Rows[e.RowIndex];
                    DataGridViewCell dcell = dgrow.Cells[1];
                    aktivcell = dcell;
                    aktivcol = InputColumns[e.RowIndex];
                    if (!aktivcol.Comboe)
                        dgcol.MaxInputLength = aktivcol.InputMaxLength;
                }
                else
                {
                    aktivcell = null;
                    aktivcol = null;
                }
            }
        }
        /// <summary>
        /// Nem combo cella editalasanak vege, hibavizsgalat
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void dataGridView2_EndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (!Inputtabla.Columns[1].ReadOnly)
            {
                toolStrip2.Enabled = true;
                toolStrip1.Enabled = true;
                DataGridViewRow dgrow = dataGridView2.Rows[e.RowIndex];
                DataGridViewCell dcell = dgrow.Cells[1];
                aktivcell = dcell;
                Cols egycol = InputColumns[e.RowIndex];
                if (egycol != null)
                {
                    int i = e.RowIndex;
                    hibaszov[i] = Hibavizsg(dcell);
                    if (valtozott[i] || Tabinfo.Valtozott)
                        uj.Enabled = false;
                    else
                        uj.Enabled = true;
                }
            }
            else
            {
                aktivcell = null;
                aktivcol = null;
            }
        }
        /// <summary>
        /// Adott cella hibavizsgalata, hiba jelzese errortext-ben
        /// </summary>
        /// <param name="dcell">
        /// a cella
        /// </param>
        /// <returns>
        /// hiba szovege vagy ""
        /// </returns>
        public override string Hibavizsg(DataGridViewCell dcell)
        {
            DataGridViewRow masikrow;
            DataGridViewCell masikcell;
            int masikcolindex;
            char[] vesszo = new char[] { Convert.ToChar(",") };
            string[] split;
            int ownersorind = InputColumns.IndexOf("OWNER");
            if (ownersorind == -1)
                ownersorind = InputColumns.IndexOf("ALKALMAZAS_ID");
            string egyhibaszov = "";
            bool hiba = false;
            string tartal = "";
            string origtart = "";
            string colnev;
            bool comboe;
            int cellrowindex = dcell.RowIndex;
            if (cellrowindex != -1)
            {
                dcell.ErrorText = "";
                Cols egycol = InputColumns[cellrowindex];
                colnev=egycol.ColumnName;
                comboe = egycol.Comboe;
                if (dcell.Value != null)
                {
                    tartal = dcell.Value.ToString().Trim();
                    if (egycol.DataType == typeof(DateTime))
                    {
                        try
                        {
                            tartal = FakUserInterface.DatumToString(Convert.ToDateTime(tartal));
                        }
                        catch
                        {
                        }
                    }
                    if (tartal == "" && colnev == "TABLANEV" && AdatColumnTomb!=null)
                        AdatColumnTomb.Clear();
                    if (Beszur)
                        origtart = egycol.DefaultValue.ToString();
                    else if (!comboe || colnev == "SORSZOV" || colnev == "OSZLSZOV" || Tabinfo.Adattabla.Columns.IndexOf(colnev) != -1)
                    {
                        origtart = Tabinfo.AktualViewRow[colnev].ToString().Trim();
                        if (origtart != "" && egycol.DataType == typeof(DateTime))
                            origtart = FakUserInterface.DatumToString(Convert.ToDateTime(origtart));
                    }
                    if (comboe && colnev!="SORSZOV" && colnev!="OSZLSZOV" && !Beszur && !Leiroe)
                    {
                        origtart = Tabinfo.AktualViewRow[colnev + "_K"].ToString().Trim();
                        if (Tabinfo.Tablanev == "TARTAL" && (colnev == "AZONTIP1" || colnev == "AZONTIP2"))
                        {
                            if (tartal != origtart)
                            {
                                string ujtartal = Tabinfo.AktualViewRow[colnev].ToString();
                                OsszefinfoCollection coll = FakUserInterface.Osszefuggesek[ujtartal];
                                if (coll.Count != 0)
                                {
                                    Osszefinfo info = coll[0];
                                    hiba = true;
                                    egyhibaszov = " Nem módositható, használja a:\n" +
                                        info.tabinfo.ParentTag.Node.Text + "/" + info.tabinfo.TablaTag.Node.Text;
                                }
                            }
                        }
                        if (!hiba)
                        {
                            if (origtart == "" && colnev == "COMBOAZONTIP")
                                origtart = " ";
                            if (origtart != tartal && cellrowindex == 0 && Tabinfo.Tablanev == "BASE")
                            {
                                hiba = true;
                                egyhibaszov = "Nem változtatható mezö, csak törölni lehet!";
                            }
                        }
                    }
                    if (!hiba)
                    {
                        string tartal1 = tartal;
                        if (egycol.Numeric(egycol.DataType) && tartal == "0")
                            tartal1 = "";
                        if (!egycol.Lehetures && tartal1 == "" )
                        {
                            hiba = true;
                            egyhibaszov = FakUserInterface.GetUzenetSzoveg("Nemures");
                        }
                        if (!hiba && !egycol.Comboe)
                        {
                            if (tartal != "")
                            {
                                try
                                {
                                    Convert.ChangeType(tartal, egycol.DataType);
                                    if (egycol.DataType == typeof(DateTime))
                                        tartal = FakUserInterface.DatumToString(Convert.ToDateTime(tartal));
                                }
                                catch
                                {
                                    hiba = true;
                                    egyhibaszov = FakUserInterface.GetUzenetSzoveg("Tipushiba");
                                }
                            }
                            if (!hiba)
                            {
                                if (egycol.Numeric(egycol.DataType))
                                {
                                    if (tartal == "")
                                        tartal = "0";
                                    if (Convert.ToDouble(tartal) < egycol.Minimum)
                                    {
                                        hiba = true;
                                        egyhibaszov = FakUserInterface.GetUzenetSzoveg("Minhiba");
                                    }
                                    else if (egycol.Maximum != 0 && Convert.ToDouble(tartal) > egycol.Maximum)
                                    {
                                        hiba = true;
                                        egyhibaszov = FakUserInterface.GetUzenetSzoveg("Maxhiba");
                                    }
                                }
                            }
                            if (!hiba)
                            {
                                if (colnev == "DATUMTOL" && verzioe && Tabinfo.ViewSorindex == -1)
                                {
                                    if (Convert.ToDateTime(tartal).CompareTo(oldversiondate) < 0)
                                    {
                                        hiba = true;
                                        egyhibaszov = FakUserInterface.GetUzenetSzoveg("Kisebbhiba") + " " + oldversiondate.ToShortDateString();
                                    }
                                }
                            }
                            if (!hiba)
                            {
                                if (colnev == "USEREK")
                                {
                                    if (tartal != "")
                                    {
                                        if (ownersorind != -1 && InputColumns[ownersorind].ComboAktSzoveg == "")
                                        {
                                            egyhibaszov = " Üres Owner alkalmazáshoz nem kell User!";
                                            hiba = true;
                                        }
                                        else
                                        {
                                            split = tartal.Split(vesszo);
                                            foreach (string egysplit in split)
                                            {
                                                if (FakUserInterface.AlkalmazasNevek.IndexOf(egysplit) == -1)
                                                {
                                                    if (egyhibaszov != "")
                                                        egyhibaszov += ", ";
                                                    egyhibaszov += egysplit;
                                                }
                                            }
                                            if (egyhibaszov != "")
                                            {
                                                hiba = true;
                                                egyhibaszov += " nincs az alkalmazásnevek között!";
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    masikcolindex = -1;
                                    if (colnev == "SZAMITASNEV")
                                        masikcolindex = InputColumns.IndexOf("SZAMITASPARAMOK");
                                    else if (colnev == "SZAMITASPARAMOK")
                                        masikcolindex = InputColumns.IndexOf("SZAMITASNEV");
                                    if (masikcolindex != -1)
                                    {
                                        masikrow = dataGridView2.Rows[masikcolindex];
                                        masikcell = masikrow.Cells[1];
                                        masikcell.ErrorText = "";
                                        hibaszov[masikcolindex] = "";
                                        string masiktart = masikcell.Value.ToString();
                                        if (tartal == "" && masiktart != "" || tartal != "" && masiktart == "")
                                        {
                                            hiba = true;
                                            egyhibaszov = "Ellentmondás számitásnév és paraméter között!";
                                            masikcell.ErrorText = egyhibaszov;
                                            hibaszov[masikcolindex] = egyhibaszov;
                                            valtozott[masikcolindex] = false;
                                        }
                                        else if (colnev == "SZAMITASPARAMOK" && tartal != "")
                                        {
                                            split = tartal.Split(vesszo);
                                            Tablainfo adatinfo = Tabinfo.TablaTag.Tablainfo;
                                            foreach (string egynev in split)
                                            {
                                                int j = adatinfo.TablaColumns.IndexOf(egynev);
                                                if (j == -1)
                                                {
                                                    if (egyhibaszov != "")
                                                        egyhibaszov += ", ";
                                                    egyhibaszov += egynev;
                                                }
                                            }
                                            if (egyhibaszov != "")
                                            {
                                                egyhibaszov += " nincs a táblában!";
                                                hiba = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }           
                            
                        string name=egycol.ColumnName;
                        if(egycol.Comboe)
                            name+="_K";
                        if (!hiba && egycol.Letezoe == true)
                        {

                            DataRow dr;
                            for (int j = 0; j < Tabinfo.DataView.Count; j++)
                            {
                                if (j != Tabinfo.ViewSorindex)
                                {
                                    dr = Tabinfo.DataView[j].Row;
                                    if (dr[name].ToString().Trim() == tartal)
                                    {
                                        hiba = true;
                                        egyhibaszov = FakUserInterface.GetUzenetSzoveg("Duplikalt");
                                        break;
                                    }
                                }
                            }
                        }
                        if (!hiba && egycol.IsAllUnique)
                        {
                            foreach(Tablainfo egytabinfo in FakUserInterface.Tablainfok)
                            {
                                if (egytabinfo != Tabinfo)
                                {
                                    int j = egytabinfo.TablaColumns.IndexOf(name);
                                    if (j != -1)
                                    {
                                        for (j = 0; j < egytabinfo.DataView.Count; j++)
                                        {
                                            DataRow dr = egytabinfo.DataView[j].Row;
                                            if (dr[name].ToString().Trim() == tartal)
                                            {
                                                hiba = true;
                                                egyhibaszov = FakUserInterface.GetUzenetSzoveg("Duplikalt");
                                                break;
                                            }
                                        }
                                        if (hiba)
                                            break;
                                    }
                                }
                            }
                        }
                        if (!hiba && egycol.IsUnique)
                        {
                            for (int j = 0; j < DataView.Count; j++)
                            {
                                if (j != Tabinfo.ViewSorindex)
                                {
                                    DataRow dr = DataView[j].Row;
                                    int k = Tabinfo.TablaColumns.IndexOf(egycol);
                                    if (dr[k].ToString().Trim() == tartal)
                                    {
                                        egyhibaszov = FakUserInterface.GetUzenetSzoveg("Duplikalt");
                                        break;
                                    }
                                }
                            }
                        }
                        if (!hiba && egycol.ColumnName == "TABLANEV" && tartal != "" && Tabinfo.Tablanev == "TARTAL")
                            AdatColumnTomb = Tabinfo.Select(tartal);
                        if(!egycol.Comboe)
                        {
                            if (!hiba && egycol.Kellselect == true && tartal != "")
                            {
                                DataColumnCollection dc = null;
                                if (egycol.ColumnName == "TABLANEV" || egycol.ColumnName == "SZOVEG" && InputColumns["SZINT"].Tartalom != "")
                                {
                                    if (Tabinfo.Tablanev == "TARTAL")
                                        dc = Tabinfo.Select(tartal);
                                    else
                                    {
                                        string szint = InputColumns["SZINT"].Tartalom;
                                        dc = Tabinfo.Select(szint, tartal);
                                    }
                                    AdatColumnTomb = dc;
                                    if (dc == null)
                                    {
                                        egyhibaszov = "Nincs ilyen nevü tábla";
                                        hiba = true;
                                    }
                                }
                            }
                            if (!hiba && egycol.Kellmezoellenorzes == true && tartal != "")
                            {
                                if (AdatColumnTomb == null && !tablanevcol.ReadOnly)
                                {
                                    egyhibaszov = "Elöbb táblanév kéne!";
                                    hiba = true;
                                }
                                else
                                {
                                    split = tartal.Split(vesszo);
                                    for (int i = 0; i < split.Length; i++)
                                    {
                                        string egyelem = split[i];
                                        if (AdatColumnTomb[egyelem] == null)
                                        {
                                            if (egyhibaszov == "")
                                                egyhibaszov = tablanev + " táblában nincs ";
                                            egyhibaszov += egyelem + "\n";
                                            hiba = true;
                                        }
                                    }
                                }
                                if (!hiba && egycol.ColumnName == "SZINT" && Tabinfo.Tablanev == "BASE")
                                {
                                    if ("RUC".Contains(tartal))
                                    {
                                        egyhibaszov = "R/U/C nem adható!";
                                        hiba = true;
                                    }
                                }
                            }
                        }
                    }
                    dcell.ErrorText = egyhibaszov;
                    if (!hiba && !egycol.Comboe && !Beszur && origtart != tartal)
                    {
                        if (Tabinfo.LehetCombo && Tabinfo.ComboFileba==egycol.ColumnName)
                            hiba = ComboHasznalatban(Tabinfo, dcell, origtart, dcell.Value.ToString(), Tabinfo.AktIdentity.ToString());
                    }
                    if (!hiba)
                    {
                        if (!egycol.Comboe && origtart != tartal || egycol.Comboe && egycol.ComboAktSzoveg != tartal)
                        {
                            if (!egycol.Comboe || (egycol.ColumnName == "SORSZOV" || egycol.ColumnName == "OSZLSZOV"))
                                egycol.Tartalom = tartal;
                            else
                            {
                                Tabinfo.KiegColumns[egycol.ColumnName + "_K"].Tartalom = tartal;
                                egycol.ComboAktSzoveg = tartal;
                                if (egycol.Combo_Info != null)
                                    egycol.ComboAktFileba = egycol.Combo_Info.GetComboAktfileba(tartal);
                                else if(egycol.ComboAzontipCombo!=null)
                                    egycol.ComboAktFileba = egycol.ComboAzontipCombo.GetComboAktfileba(tartal);
                            }
                            rogzit.Enabled = false;
                            valtozott[cellrowindex] = true;
                            //if(!Beszur)
                            dcell.ErrorText = EgyediHibavizsg(dcell,Tabinfo);
                            if (dcell.ErrorText != "")
                            {
                                egyhibaszov = dcell.ErrorText;
                                hiba = true;
                            }
                        }
                    }
                    if (hiba)
                        valtozott[cellrowindex] = false;

                    hibaszov[cellrowindex]=egyhibaszov;
                    bool valt = false;
                    bool volthiba = false;
                    for (int i = 0; i < valtozott.Length; i++)
                    {
                        if (valtozott[i])
                        {
                            valt = true;
                            Tabinfo.Changed = true;
                            ok.Enabled = true;
                        }
                        if(hibaszov[i]!="")
                            volthiba=true;
                    }
                    if (!valt || volthiba)
                        ok.Enabled = false;
                    Tabinfo.ModositasiHiba = volthiba;
                }
            }
            return egyhibaszov;
        }
        /// <summary>
        /// Scroll event, ha van aktiv combocellank, annak a lathatosagat probaltam a scroll szerint valtoztatni
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void dataGridView2_Scroll(object sender, ScrollEventArgs e)
        {
            int firstdisprowindex = dataGridView2.FirstDisplayedScrollingRowIndex;
            int rowcount = dataGridView2.DisplayedRowCount(false);
            int lastvisible = rowcount + firstdisprowindex;//+1
            int i = e.NewValue - e.OldValue; ;
            if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
            {
                if (combocell != null)
                {
                    if (i > 0) // lefele scroll, felfele mennnek ki a cellak
                    {
                        if (dataGridView2.FirstDisplayedCell.RowIndex > combocell.RowIndex)
                            combovisible = false;
                        else if (lastvisible == combocell.RowIndex)
                            combovisible = false;
                        else
                            combovisible = true;
                    }
                    else    // felfele scroll, lefele mennek ki a cellak
                    {
                        if (dataGridView2.FirstDisplayedCell.RowIndex == combocell.RowIndex)
                            combovisible = true;
                        else if (lastvisible <= combocell.RowIndex)
                            combovisible = false;
                    }

                    if (combovisible)
                    {
                        c = dataGridView2.GetColumnDisplayRectangle(combocell.ColumnIndex, true);
                        x = dataGridView2.GetRowDisplayRectangle(combocell.RowIndex, true);
                        comboBox1.Bounds = c;
                        comboBox1.Size = combocell.Size;
                        comboBox1.Location = new System.Drawing.Point(comboBox1.Location.X + x.Location.X, comboBox1.Location.Y + x.Location.Y + dataGridView2.Location.Y);
                        comboBox1.Visible = true;
                    }
                    else
                        comboBox1.Visible = false;
                }
            }
        }
        /// <summary>
        /// a combo text-et valtoztattuk. Ha a combo text ures,atmentjuk az aktiv cellaba (melyre raraktuk),hibavizsgalat
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override  void comboBox1_TextUpdate(object sender, EventArgs e)
        {
            if (comboBox1.Visible)
            {
                int i = aktivcell.RowIndex;
                Cols egycol = InputColumns[aktivcell.RowIndex];
                aktivcell.Value = comboBox1.Text;
                string szov = Hibavizsg(aktivcell);
                FakUserInterface.ErrorProvider.SetError(comboBox1, szov);
                if (szov == "" && (comboBox1.Text == "" || comboBox1.Items.IndexOf(comboBox1.Text) == -1))
                {
                    if (egycol.Numeric(egycol.DataType))
                        egycol.ComboAktFileba = "0";
                    else
                        egycol.ComboAktFileba = "";
                    egycol.Tartalom = egycol.ComboAktFileba;
                    egycol.ComboAktSzoveg = "";
                    Inputtabla.Rows[i][1] = comboBox1.Text;
                }
            }
        }
        /// <summary>
        /// Elso esetben hivja a Tempcellini()-t
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void dataGridView2_Enter(object sender, EventArgs e)
        {
            if (first)
            {
                first = false;
                Tempcellini();
            }
        }
        /// <summary>
        /// Atveszi a valasztott item tartalmat, hibavizsgalatot vegez
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (comboBox1.Visible && comboBox1.SelectedIndex != -1 && aktivcell.RowIndex != -1)
            {
                Cols egycol = InputColumns[aktivcell.RowIndex];
                string ertek = comboBox1.Items[comboBox1.SelectedIndex].ToString().Trim();
                aktivcell.Value = ertek;
                string szov = Hibavizsg(aktivcell);
                FakUserInterface.ErrorProvider.SetError(comboBox1, szov);
                if (szov == "")
                {
                    if (valtozott[aktivcell.RowIndex] || Tabinfo.Valtozott)
                        uj.Enabled = false;
                    else
                        uj.Enabled = true;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void dataGridView1_VisibleChanged(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void Alap_Load(object sender, EventArgs e)
        {
            if (Tabinfo != null && Tabinfo.ViewSorindex != -1)
            {
                SetSelectedRow(Tabinfo.ViewSorindex);
            }
        }

        public virtual void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (combocell != null && combocell.RowIndex!=-1)
            {
                int sorind = combocell.RowIndex;
                int oszlind = 1;
                c = dataGridView2.GetColumnDisplayRectangle(oszlind, true);
                x = dataGridView2.GetRowDisplayRectangle(sorind, true);
                comboBox1.Bounds = c;
                comboBox1.Size = combocell.Size;
                comboBox1.Location = new System.Drawing.Point(comboBox1.Location.X + x.Location.X, comboBox1.Location.Y + x.Location.Y + dataGridView2.Location.Y);
            }
        }
    }
}