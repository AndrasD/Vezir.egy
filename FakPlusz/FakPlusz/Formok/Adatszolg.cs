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
namespace FakPlusz.Formok
{
    /// <summary>
    /// Adatszolgaltatasok definialasanak UserControl-ja
    /// </summary>
    public partial class Adatszolg: Gridpluszinput
    {
        private Rectangle c;
        private Rectangle x;
        private string[] mezonevek = null;
        private string[] mezoszovegek = null;
        private Comboinfok comboinfo=null;
        private int azontipindex = -1;
        private int mezonevindex;
        private int fixertekindex;
        private Cols azontipcol;
        private Cols mezonevcol;
        private Cols fixertekcol;
        private MezoTag AzontipTag = null;
        private MezoTag MezonevMezoTag = null;
        private MezoTag FixertMezoTag = null;
        private int viewindex;
        private Tablainfo useradatszolg = null;
        private Cols mezonevkiegcol = null;
        private Cols fixertkiegcol = null;
        private string[] specszovegek;
        private ArrayList specitemekarray = new ArrayList();
        private bool azontipvolt = false;
        private bool mezonevvolt = false;
        private bool fixertvolt = false;
        private bool speccolvolt = false;
        private bool editboljott = false;
        private AdatTabla adattabla = null;
        /// <summary>
        /// az objectum letrehozasa
        /// </summary>
        /// <param name="vezerles"></param>
        public Adatszolg(Vezerloinfo vezerles)
        {
            InitializeComponent();
            ParameterAtvetel(vezerles, false);
            Tablainfo specfix = FakUserInterface.GetBySzintPluszTablanev("R", "SPECADATSZOLGNEVEK");
            specszovegek=new string[specfix.DataView.Count];
            for (int i = 0; i < specfix.DataView.Count; i++)
                specszovegek[i] = specfix.DataView[i].Row["SZOVEG"].ToString();
            specitemekarray = new ArrayList(specszovegek);
            useradatszolg = FakUserInterface.GetBySzintPluszTablanev("R", "USERADATSZOLG");
        }
        /// <summary>
        /// Inicializalas
        /// </summary>
        public override void AltalanosInit()
        {
            editboljott = false;
            bool valt = UjTag;
            if (!valt)
            {
                valt = !Tabinfo.KellVerzio && ValtozasLekerdezExcept(new string[] { "Verziovaltozas" }).Count != 0 ||
                    Tabinfo.KellVerzio && ValtozasLekerdez().Count != 0;
            }
            if (!valt)
            {
                MezoControlInfok[0].UserControlInfo = UserControlInfo;
                return;
            }
            Tablainfo tabinfo = TablainfoTag.Tablainfo;
            HozferJog = tabinfo.Azonositok.Jogszintek[(int)KezeloiSzint];
            ColCollection inputcolumns = tabinfo.InputColumns;
            ColCollection tablacolumns = tabinfo.TablaColumns;
            adattabla = tabinfo.Adattabla;
            azontipindex = inputcolumns.IndexOf("AZONTIP");
            azontipcol = inputcolumns[azontipindex];
            mezonevindex = inputcolumns.IndexOf("MEZONEV");
            mezonevcol = inputcolumns[mezonevindex];
            fixertekindex = inputcolumns.IndexOf("FIXERTEK");
            fixertekcol = inputcolumns[fixertekindex];
            AzontipTag = new MezoTag(tabinfo, "AZONTIP", FakUserInterface, null, null, null, null);
            AzontipTag.Control = comboBox1;
            comboBox1.Tag = AzontipTag;
            FakUserInterface.Comboinfoszures(comboBox1, new string[] { "SZRM" });
            AzontipTag.SzurtCombofileinfo[0] = "";
            AzontipTag.SzurtComboinfo[0] = "";
            MezonevMezoTag = new MezoTag(tabinfo, "MEZONEV", FakUserInterface, null, null, null, null);
            MezonevMezoTag.Control = comboBox1;
            mezonevcol.Comboe = true;
            azontipcol.Comboe = true;
            fixertekcol.Comboe = true;
            if (adattabla.Columns.IndexOf("MEZONEV_K") == -1)
                adattabla.Columns.Add(new DataColumn("MEZONEV_K"));
            if (tabinfo.ComboColumns["MEZONEV"] == null)
                tabinfo.ComboColumns.Add(mezonevcol);
            int mezonevk = tabinfo.KiegColumns.IndexOf("MEZONEV_K");
            if (mezonevk != -1)
                mezonevkiegcol = tabinfo.KiegColumns[mezonevk];
            else
            {
                mezonevkiegcol = new Cols("MEZONEV_K", "System.String", "Mezö neve", 30, true, tabinfo, "");
                tabinfo.KiegColumns.Add(mezonevkiegcol);
            }
            mezonevcol.Kiegcol = mezonevkiegcol;
            tabinfo.TablaColumns["MEZONEV"].Kiegcol = mezonevkiegcol;
            fixertekcol = inputcolumns[fixertekindex];
            FixertMezoTag = new MezoTag(tabinfo, "FIXERTEK", FakUserInterface, null, null, null, null);
            FixertMezoTag.Control = comboBox1;
            comboBox1.Tag = FixertMezoTag;
            fixertekcol.Combo_Info = FakUserInterface.ComboInfok.ComboinfoKeres("SZRTSPECADATSZOLGNEVEK");
            tabinfo.TablaColumns["FIXERTEK"].Combo_Info = fixertekcol.Combo_Info;
            if (adattabla.Columns.IndexOf("FIXERTEK_K") == -1)
                adattabla.Columns.Add(new DataColumn("FIXERTEK_K"));
            if (tabinfo.ComboColumns["FIXERTEK"] == null)
                tabinfo.ComboColumns.Add(fixertekcol);
            int fixertekk = tabinfo.KiegColumns.IndexOf("FIXERTEK_K");
            if (fixertekk != -1)
                fixertkiegcol = tabinfo.KiegColumns[fixertekk];
            else
            {
                fixertkiegcol = new Cols("FIXERTEK_K", "System.String", "Fix érték", 30, true, tabinfo, "");
                tabinfo.KiegColumns.Add(fixertkiegcol);
            }
            fixertekcol.Kiegcol = fixertkiegcol;
            base.AltalanosInit();
            viewindex = Tabinfo.ViewSorindex;
            for (int i = 0; i < Tabinfo.DataView.Count; i++)
            {
                ComboAllitasok(i);
                Tabinfo.ViewSorindex = i;
            }
            comboBox1.Tag = MezonevMezoTag;
            if (viewindex != -1)
            {
                ComboAllitasok(viewindex);
                Tabinfo.ViewSorindex = viewindex;
                Inputtablaba(Tabinfo.AktualViewRow[szovegcol].ToString());
            }
            else
                Inputtablaba();
        }
        private void ColnevAlapjanBeallit()
        {
            ColnevAlapjanBeallit(null);
        }
        private void ColnevAlapjanBeallit(Cols egycol)
        {
            if (egycol == null)
            {
                if (aktivcell == null)
                    return;
                egycol = InputColumns[aktivcell.RowIndex];
            }
            speccolvolt = false;
            azontipvolt = false;
            mezonevvolt = false;
            fixertvolt = false;

            switch (egycol.ColumnName)
            {
                case "AZONTIP":
                    azontipvolt = true;
                    break;
                case "MEZONEV":
                    mezonevvolt = true;
                    break;
                case "FIXERTEK":
                    fixertvolt = true;
                    break;
            }
            if (azontipvolt || mezonevvolt || fixertvolt)
                speccolvolt = true;
        }
        /// <summary>
        /// also gridview cella click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!FakUserInterface.EventTilt && !dataGridView2.CurrentCell.ReadOnly)
            {
                editboljott = true;
                aktivcell = dataGridView2.CurrentCell;
                ColnevAlapjanBeallit();
                DataGridViewCell dc = aktivcell;
                if (dc != null && dc.ReadOnly != true)
                {
                    Cols egycol = InputColumns[dc.RowIndex];
                    if(!speccolvolt)
                        base.dataGridView2_CellClick(sender, e);
                    else
                    {
                        if(azontipvolt)
                            comboBox1.Tag = AzontipTag;
                        else if (mezonevvolt && azontipcol.ComboAktSzoveg != "")
                        {
                            comboBox1.Tag = MezonevMezoTag;
                            egycol.Comboe = true;
                        }
                        else if (fixertvolt)
                        {
                                egycol.Comboe = true;
                                comboBox1.Tag = FixertMezoTag;
                                comboBox1.SelectedIndex = specitemekarray.IndexOf(egycol.Tartalom);
                        }
                        if (egycol.Comboe)
                        {
                            comboBox1.Items.Clear();
                            comboBox1.Text = aktivcell.Value.ToString();
                            c = dataGridView2.GetColumnDisplayRectangle(aktivcell.ColumnIndex, true);
                            x = dataGridView2.GetRowDisplayRectangle(aktivcell.RowIndex, true);
                            if (azontipvolt)
                            {
                                comboBox1.Items.AddRange(((MezoTag)comboBox1.Tag).SzurtComboinfo);
                                comboBox1.Items[0] = "";
                            }
                            else if (mezonevvolt)
                                comboBox1.Items.AddRange(mezoszovegek);
                            else if (fixertvolt)
                                comboBox1.Items.AddRange(specszovegek);
                            if (comboBox1.Items.Count != 0)
                            {
                                int i = comboBox1.Items.IndexOf(aktivcell.Value.ToString());
                                comboBox1.TabIndex = dataGridView2.TabIndex;
                                comboBox1.Bounds = c;
                                comboBox1.Location = new System.Drawing.Point(comboBox1.Location.X + x.Location.X, comboBox1.Location.Y + x.Location.Y + dataGridView2.Location.Y);
                                comboBox1.Visible = true;
                                combovisible = true;
                                combocell = aktivcell;
                                if (i != -1)
                                    aktivcell.Value = comboBox1.Items[i];
                                dataGridView2.Controls.Add(comboBox1);
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// dataGridView2 inputcellak tartalmanak beallitasa, hibavizsgalat a cellakra, valtozasok vizsgalata
        /// felulbiralva
        /// </summary>
        public override void InputtablabaTovabb()
        {
            Cols egycol;
            DataGridViewCell dcell;
            string tartal;
            string tartal1;
            DataRow dr = null;
            bool valt = false;
            string azontip1 = "";
            string mezonev1 = "";
            string fixert = "";
            if (Tabinfo.ViewSorindex != -1)
            {
                dr = Tabinfo.AktualViewRow;
                azontip1 = dr["AZONTIP"].ToString();
                azontipcol.Tartalom = azontip1;
                mezonev1 = dr["MEZONEV"].ToString();
                mezonevcol.Tartalom = mezonev1;
                fixert = dr["FIXERTEK"].ToString();
                fixertekcol.Tartalom = fixert;
            }
            if (azontip1 != "")
            {
                comboinfo = FakUserInterface.ComboInfok.ComboinfoKeres(azontip1);
                mezonevek = comboinfo.ComboFileinfoAll();
                mezoszovegek = comboinfo.ComboSzovinfoAll();
                mezonevcol.Lehetures = false;
                mezonevcol.Comboe = true;
                mezonevcol.Combo_Info = comboinfo;
                Tabinfo.TablaColumns["MEZONEV"].Combo_Info = mezonevcol.Combo_Info;
                fixertekcol.Comboe = false;
                fixertekcol.Lehetures = true;
            }
            else
            {
                mezonevcol.Lehetures = true;
                mezonevcol.Comboe = false;
                mezonevcol.Combo_Info = null;
                Tabinfo.TablaColumns["MEZONEV"].Combo_Info = null;
                fixertekcol.Comboe = true;
                fixertekcol.Lehetures = false;
            }
            Tabinfo.ViewSorindex = Tabinfo.ViewSorindex;
            if (Tabinfo.ViewSorindex == -1 && Beszurhat || Tabinfo.ViewSorindex != -1)
            {
                for (int i = 0; i < InputColumns.Count; i++)
                {
                    egycol = InputColumns[i];
                    ColnevAlapjanBeallit(egycol);
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
                        string[] fileinfo = null;
                        string[] szovinfo = null;
                        if (azontip1 != "")
                        {
                            bool megvan = false;
                            if (azontipvolt || mezonevvolt)
                            {
                                if(azontipvolt)
                                {
                                    fileinfo=AzontipTag.SzurtCombofileinfo;
                                    szovinfo=AzontipTag.SzurtComboinfo;
                                }
                                else 
                                {
                                    fileinfo=(string[])comboinfo.ComboFileinfo.ToArray(typeof(string));
                                    szovinfo=(string[])comboinfo.ComboInfo.ToArray(typeof(string));
                                }
                                for (j =0; j < fileinfo.Length; j++)
                                {
                                    tartal = fileinfo[j];
                                    if (tartal == egycol.OrigTartalom)
                                    {
                                        megvan = true;
                                        egycol.Tartalom = tartal;
                                        egycol.ComboAktFileba = tartal;
                                        egycol.ComboAktSzoveg = szovinfo[j];
                                        break;
                                    }
                                }
                                if (!megvan)
                                {
                                    tartal = fileinfo[0];
                                    egycol.Tartalom = tartal;
                                    egycol.ComboAktFileba = tartal;
                                    egycol.ComboAktSzoveg = szovinfo[0];
                                }
                                Inputtabla.Rows[i][1] = egycol.ComboAktSzoveg;
                            }
                            else if (fixertvolt)
                            {
                                fixertekcol.Comboe = false;
                                fixertekcol.Lehetures = true;
                                Inputtabla.Rows[i][1] = "";
                            }
                        }
                        else
                        {
                            if (azontipvolt || mezonevvolt)
                            {
                                egycol.Lehetures = true;
                                egycol.Tartalom = "";
                                egycol.ComboAktSzoveg = "";
                                egycol.ComboAktFileba = "";
                                tartal = "";
                                Inputtabla.Rows[i][1] = "";
                            }
                            else if (fixertvolt)
                            {
                                egycol.Lehetures = false;
                                Inputtabla.Rows[i][1] = tartal;
                            }
                        }
                        if (!speccolvolt)
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
                                if (tartal != "" || !egycol.Lehetures)
                                {
                                    egycol.Combo_Info.Combotolt(egycol, tartal);
                                    tartal = egycol.ComboAktFileba;
                                }
                            }
                            Inputtabla.Rows[i][1] = tartal;
                        }
                    }
                    tartal1 = tartal;
                    if (egycol.Numeric(egycol.DataType) && !egycol.Comboe && tartal == "0")
                        tartal1 = "";
                    hibaszov[i] = "";
                    dcell = dataGridView2.Rows[i].Cells[1];
                    dcell.ErrorText = "";
                    if (tartal1 == "")
                    {
                        if (!egycol.Lehetures && editboljott)
                            hibaszov[i] = "Nem lehet üres";
                        else
                            hibaszov[i] = "";
                    }
                    valtozott[i] = false;
                    if (egycol.OrigTartalom != egycol.Tartalom )
                    {
                        valtozott[i] = true;
                        valt = true;
                    }
                    DataRow dr1 = Inputtabla.Rows[i];
                    DataColumn col = dr1.Table.Columns["TARTALOM"];
                    int len = col.MaxLength;
                    if (egycol.Comboe )
                    {
                        int maxlen = 0;
                        if (egycol.Combo_Info != null)
                            maxlen = egycol.Combo_Info.Maxhossz;
                        else
                            maxlen = egycol.ComboAzontipCombo.Maxhossz;
                        if (len < maxlen)
                            col.MaxLength = maxlen;
                        if(!speccolvolt)
                            dr1[col] = egycol.ComboAktSzoveg;
                    }
                    else
                    {
                        if (egycol.InputMaxLength > col.MaxLength)
                            col.MaxLength = egycol.InputMaxLength;
                        dr1[col] = tartal1;
                    }
                }
                if (valt && editboljott)
                {
                    Tabinfo.Modositott = true;
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

        }
        /// <summary>
        /// cella egyedi hibavizsgalata
        /// </summary>
        /// <param name="dcell">
        /// a cella
        /// </param>
        /// <param name="tabinfo">
        /// </param>
        /// <returns>
        /// true: hibas
        /// </returns>
        public override string EgyediHibavizsg(DataGridViewCell dcell,Tablainfo tabinfo)
        {
            int cellrowindex = dcell.RowIndex;
            Cols egycol = InputColumns[cellrowindex];
            ColnevAlapjanBeallit(egycol);
            string hiba = "";
            string azontip1;
            int i;
            if (azontipvolt)
            {
                i = egycol.ComboAzontipCombo.ComboInfo.IndexOf(dcell.Value.ToString());
                if (i == -1)
                    azontip1 = "";
                else
                    azontip1 = egycol.ComboAzontipCombo.ComboFileinfo[i].ToString().Trim();
                if (azontip1 != "")
                {
                    if (!azontip1.StartsWith("SZRM"))
                        hiba = "Csak táblanevekböl választhat!";
                    else
                    {
                        hibaszov[fixertekindex] = "";
                        InputColumns[fixertekindex].Tartalom = "";
                        Inputtabla.Rows[fixertekindex][1] = "";
                        valtozott[fixertekindex] = true;
                        DataGridViewCell cell = dataGridView2[1, fixertekindex];
                        cell.ErrorText = "";
                        comboinfo = FakUserInterface.ComboInfok.ComboinfoKeres(azontip1);
                        mezonevek = comboinfo.ComboFileinfoAll();
                        mezoszovegek = comboinfo.ComboSzovinfoAll();
                        mezonevcol.Lehetures = false;
                        mezonevcol.Combo_Info = comboinfo;
                        Tabinfo.TablaColumns["MEZONEV"].Combo_Info = mezonevcol.Combo_Info;
                        mezonevcol.Comboe = true;
                    }
                }
                else 
                {
                        mezonevcol.Lehetures = true;
                        mezonevcol.Comboe = false;
                        mezonevcol.Combo_Info = null;
                        Tabinfo.TablaColumns["MEZONEV"].Combo_Info = null;
                        i = mezonevindex;
                        if (InputColumns[i].Tartalom != "")
                        {
                            InputColumns[i].Tartalom = "";
                            Inputtabla.Rows[i][1] = "";
                            valtozott[i] = true;
                        }
                        if (fixertekcol.Tartalom == "")
                            hiba = "Táblaazonositó vagy fix érték kell!";
                        else
                            hiba = "";
                }
                if(hiba!="")
                {
                    dataGridView2.Controls.Remove(comboBox1);
                    comboBox1.Visible = false;
                }
            }
            else if (fixertvolt)
            {
                string tartal = dcell.Value.ToString().Trim();
                if (tartal != "")
                {
                    mezonevcol.Comboe = false;
                    mezonevcol.Lehetures = true;
                    DataGridViewCell cell = dataGridView2[1, 0];
                    if (cell.Value.ToString() != "")
                    {
                        valtozott[0] = true;
                        cell.Value = "";
                    }
                    cell.ErrorText = "";
                    cell = dataGridView2[1, 1];
                    if (cell.Value.ToString() != "")
                    {
                        valtozott[1] = true;
                        cell.Value = "";
                    }
                    cell.ErrorText = "";
                }
                if (tartal == "" && azontipcol.Tartalom == "")
                    hiba = "Táblaazonositó vagy fix érték kell!";
                else
                    hiba = "";
            }
            dcell.ErrorText = hiba;
            hibaszov[cellrowindex] = hiba;
            return hiba;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            editboljott = true;
            base.comboBox1_SelectionChangeCommitted(sender, e);
            if (aktivcell.ErrorText == "")
            {
                ColnevAlapjanBeallit();
                Cols egycol = InputColumns[aktivcell.RowIndex];
                if (azontipvolt || fixertvolt)
                {
                    if (egycol.ComboAktSzoveg == "" || fixertvolt)
                    {
                        mezonevcol.Comboe = false;
                        mezonevcol.Combo_Info = null;
                        mezonevcol.Tartalom = "";
                        Inputtabla.Rows[mezonevindex][1] = "";
                    }
                    else if(azontipvolt && egycol.ComboAktSzoveg != "")
                    {
                        string azontip = egycol.ComboAktFileba;
                        comboinfo = FakUserInterface.ComboInfok.ComboinfoKeres(azontip);
                        mezonevek = comboinfo.ComboFileinfoAll();
                        mezoszovegek = comboinfo.ComboSzovinfoAll();
                        mezonevcol.Combo_Info = comboinfo;
                        Tabinfo.TablaColumns["MEZONEV"].Combo_Info = mezonevcol.Combo_Info;
                        Inputtabla.Rows[mezonevindex][1] = mezoszovegek[0];
                    }
                }
            }
        }
        /// <summary>
        /// combobox textet valtoztattunk
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void comboBox1_TextUpdate(object sender, EventArgs e)
        {
            editboljott = true;
            Cols egycol = InputColumns[aktivcell.RowIndex];
            ColnevAlapjanBeallit();
            if (azontipvolt || mezonevvolt)
            {
                comboBox1.Text = "";
                base.comboBox1_TextUpdate(sender, e);
                if (aktivcell.ErrorText != "")
                    return;
            }
            int i = 0;
            if (azontipvolt || mezonevvolt)
            {
                if (azontipvolt)
                    i = mezonevindex;
                else
                    i = azontipindex;
                if (InputColumns[i].Tartalom != "")
                {
                    Inputtabla.Rows[i][1] = "";
                    InputColumns[i].Tartalom = "";
                    egycol.ComboAktSzoveg = "";
                    valtozott[i] = true;
                }
            }
            else if (fixertvolt)
            {
                i = fixertekindex;
                fixertekcol.Comboe = false;
                if (InputColumns[i].Tartalom != comboBox1.Text)
                {
                    InputColumns[i].Tartalom = comboBox1.Text;
                    Inputtabla.Rows[i][1] = comboBox1.Text;
                    valtozott[i] = true;
                }
                if (comboBox1.Text != "")
                {
                    i = mezonevindex;
                    if (InputColumns[i].Tartalom != "")
                    {
                        Inputtabla.Rows[i][1] = "";
                        InputColumns[i].Tartalom = "";
                        valtozott[i] = true;
                    }
                    i = azontipindex;
                    if (InputColumns[i].Tartalom != "")
                    {
                        Inputtabla.Rows[i][1] = "";
                        InputColumns[i].Tartalom = "";
                        valtozott[i] = true;
                    }
                }
                else
                {
                    string azontip = InputColumns[azontipindex].Tartalom;
                    if (azontip != "")
                    {
                        mezonevcol.Lehetures = false;
                        mezonevcol.Comboe = true;
                        mezonevcol.Combo_Info = comboinfo;
                        Tabinfo.TablaColumns["MEZONEV"].Combo_Info = mezonevcol.Combo_Info;
                    }
                }
            }
            VerziobuttonokAllit();
            if (valtozott[i])
                ok.Enabled = true;
        }
        private void ComboAllitasok(int viewindex)
        {

            DataRow dr = Tabinfo.DataView[viewindex].Row;
            string azontip = dr["AZONTIP"].ToString();
            if (azontip != "")
            {
                fixertekcol.Comboe = false;
                fixertekcol.Lehetures = true;
                comboinfo = FakUserInterface.ComboInfok.ComboinfoKeres(azontip);
                mezonevek = comboinfo.ComboFileinfoAll();
                mezoszovegek = comboinfo.ComboSzovinfoAll();
                mezonevcol.Combo_Info = comboinfo;
                mezonevcol.Comboe = true;
                Tabinfo.TablaColumns["MEZONEV"].Combo_Info = mezonevcol.Combo_Info;
            }
            else
            {
                mezonevcol.Comboe = false;
                mezonevcol.Lehetures = true;
                fixertekcol.Comboe = true;
                fixertekcol.Lehetures = false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void elolrolalap_Click(object sender, EventArgs e)
        {
            FakUserInterface.ForceAdattolt(Tabinfo);
            Modositott = false;
            rogzit.Enabled = false;
            UjTag = true;
            AltalanosInit();
        }
        /// <summary>
        /// Rogzit felulbiral
        /// </summary>
        public override void Rogzit()
        {
            base.Rogzit();
            AltalanosInit();
        }
        /// <summary>
        /// ok felulbiral
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void ok_Click(object sender, EventArgs e)
        {
            editboljott = false;
            string tart = fixertekcol.Tartalom;
            if (fixertekcol.Comboe)
                tart = fixertekcol.ComboAktSzoveg;
            bool valt = false;
            ok.ToolTipText = "";
            Egyedi_Ok();
            for (int i = 0; i < hibaszov.Length; i++)
            {
                if (valtozott[i])
                    valt = true;
                if (hibaszov[i] != "")
                {
                    DataGridViewCell dcell = dataGridView2.Rows[i].Cells[1];
                    dcell.ErrorText = hibaszov[i];
                    if (ok.ToolTipText != "")
                        ok.ToolTipText += "\n";
                    ok.ToolTipText += Inputtabla.Rows[i][0].ToString() + ":" + hibaszov[i];
                }
            }
            if (ok.ToolTipText == "")
            {
                AktivPage.Text = AktivPage.Text.Replace("!", "");
                if (valt || Tabinfo.Modositott)
                    rogzit.Enabled = true;
                if (valt)
                {
                    if (Beszur && Beszursorrend == 0)
                        Beszursorrend = 100;
                    DataRow aktualadatrow = Tabinfo.AdatsortoltInputtablabol(Tabinfo.ViewSorindex, Beszur, Beszursorrend);
                    if (tart != "")
                    {
                        Tabinfo.AktualViewRow["FIXERTEK"] = tart;
                        Tabinfo.AktualViewRow["FIXERTEK_K"] = tart;
                    }
                    Azonositoszoveg = "";
                    if (szovegcol != -1)
                        Azonositoszoveg = Tabinfo.AktualViewRow[szovegcol].ToString();
                    if (verzioe && Beszur)
                        Tabinfo.AktualViewRow["VERZIO_ID"] = oldversionnumber;
                    Modositott = true;
                    if (!Tabinfo.Modositott)
                        Tabinfo.Modositott = true;
                    int viewindex = 0;
                    if (!Beszur && Tabinfo.ViewSorindex != DataView.Count - 1)
                        viewindex = Tabinfo.ViewSorindex + 1;
                    ComboAllitasok(viewindex);
                    Tabinfo.ViewSorindex = viewindex;
                    SetAktRowVisible(dataGridView1,Tabinfo);
                    VerziobuttonokAllit();
                }
            }
            Inputtablaba();
        }
        /// <summary>
        /// beginedit kiegeszites
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void dataGridView2_BeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            editboljott = true;
            Cols egycol = InputColumns[e.RowIndex];
            if (egycol.ColumnName == "MEZONEV")
                e.Cancel = true;
            else
                base.dataGridView2_BeginEdit(sender, e);
        }
        /// <summary>
        /// endedit kiegeszit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void dataGridView2_EndEdit(object sender, DataGridViewCellEventArgs e)
        {
            base.dataGridView2_EndEdit(sender, e);
            Cols egycol = InputColumns[e.RowIndex];
            ColnevAlapjanBeallit(egycol);
            if (azontipvolt)
            {
                egycol.Tartalom = "";
                int i = mezonevindex;
                Inputtabla.Rows[i][1] = "";
                mezonevcol.Comboe = false;
            }
            else if (fixertvolt && egycol.Tartalom != "")
            {
                int i = azontipindex;
                if (InputColumns[i].Tartalom != "")
                {
                    Inputtabla.Rows[i][1] = "";
                    InputColumns[i].Tartalom = "";
                    valtozott[i] = true;
                }
                i=mezonevindex;
                if (InputColumns[i].Tartalom != "")
                {
                    Inputtabla.Rows[i][1] = "";
                    InputColumns[i].Tartalom = "";
                    mezonevcol.Comboe = false;
                    valtozott[i] = true;
                }
            }
            editboljott = false;
        }
        /// <summary>
        /// egyedi ok vizsgalatok
        /// </summary>
        public override void Egyedi_Ok()
        {
            int i = azontipindex;
            int j = fixertekindex;
            int k = mezonevindex;
            hibaszov[i] = "";
            hibaszov[j] = "";
            hibaszov[k] = "";
            if (Inputtabla.Rows[i][1].ToString() == "" && Inputtabla.Rows[j][1].ToString() == "")
            {
                hibaszov[i] = "Táblaazonositó vagy fix érték kell!";
                DataGridViewCell dcell = dataGridView2.Rows[i].Cells[1];
            }
            if (Inputtabla.Rows[i][1].ToString() != "" && Inputtabla.Rows[k][1].ToString() == "")
                hibaszov[k] = "Mezönév nem lehet üres!";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void eleszur_Click(object sender, EventArgs e)
        {
            editboljott = false;
            base.eleszur_Click(sender, e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void mogeszur_Click(object sender, EventArgs e)
        {
            editboljott = false;
            base.mogeszur_Click(sender, e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void elozo_Click(object sender, EventArgs e)
        {
            if (Tabinfo.ViewSorindex > 0)
            {
                ComboAllitasok(Tabinfo.ViewSorindex - 1);
                base.elozo_Click(sender, e);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void kovetkezo_Click(object sender, EventArgs e)
        {
            if (Tabinfo.ViewSorindex != -1 && Tabinfo.ViewSorindex < DataView.Count - 1 && NemkellOk())
            {
                ComboAllitasok(Tabinfo.ViewSorindex + 1);
                base.kovetkezo_Click(sender, e);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!FakUserInterface.EventTilt && e.RowIndex != -1)
            {
                if (NemkellOk())
                {
                    ComboAllitasok(e.RowIndex);
                    base.dataGridView1_CellMouseClick(sender, e);
                    Inputtablaba();
                }
            }
        }
    }
}
