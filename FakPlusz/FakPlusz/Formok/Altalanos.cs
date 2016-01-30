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

namespace FakPlusz.Formok
{
    /// <summary>
    /// Altalanos feladatok (tartalomjegyzek, kodtablak, tablazatok karbantartasanak UserControl-ja
    /// </summary>
    public partial class Altalanos : Gridpluszinput
    {
        private Comboinfok[] comboinfok;
        private ArrayList comboinfoarr = new ArrayList();
        private bool ujtag;
        private MezoTag kezelotag;
        private Tablainfo kezeloalkalm = null;
        private string[] idk = null;
        private new bool valtozott;
        public Altalanos()
        {
        }
        /// <summary>
        /// objectum letrehozasa
        /// </summary>
        /// <param name="vezerles">
        /// vezerloinformacio
        /// </param>
        public Altalanos(Vezerloinfo vezerles)
        {
            InitializeComponent();
            ParameterAtvetel(vezerles, false);
            kezeloalkalm = FakUserInterface.GetOsszef("U", "KezeloAlkalm");
        }
        /// <summary>
        /// 
        /// </summary>
        public override void  AltalanosInit()
        {
            bool modositott = false;
            ujtag = UjTag;
            if (ujtag)
            {
                if (Leiroe)
                    Tabinfo = TablainfoTag.LeiroTablainfo;
                else
                    Tabinfo = TablainfoTag.Tablainfo;
            }
            valtozott = ValtozasLekerdez().Count != 0;
            if (!Tervezoe && HozferJog==HozferJogosultsag.Irolvas)
            {
                idk = FakUserInterface.GetTartal(kezeloalkalm, "SORSZAM1", "SORSZAM2", FakUserInterface.AlkalmazasId);
                switch (Tabinfo.Tablanev)
                {
                    case "RENDSZERGAZDA":
                        if (idk == null)
                            Tabinfo.Azonositok.Jogszintek[0]=HozferJogosultsag.Csakolvas;
                        else
                            Tabinfo.Azonositok.Jogszintek[0] = HozferJogosultsag.Irolvas;
                        break;
                }
            
            }
            if (Tabinfo.Tablanev == "KIAJANL")
                modositott = Kiajanlkarbantart(Tabinfo.Szint);
            if (ujtag || valtozott || modositott)
            {
                base.AltalanosInit();
                if (!Tervezoe)
                    Hivo.Hivo.AltalanosInit();
            }
        }
        private bool Kiajanlkarbantart(string szint)
        {
            this.Visible = true;
            bool modositott = false;
            Tablainfo szukkodttartal = FakUserInterface.GetByAzontip("SZCSTARTAL");
            Tablainfo kodtartal = FakUserInterface.GetByAzontip("SZCKTARTAL");
            string filter = "";
            string tabinfofilter = "";
            if (!Tervezoe)
            {
                filter = "OWNER = '" + FakUserInterface.AlkalmazasId + "'";
                tabinfofilter = "ALKALMAZAS_ID = " + FakUserInterface.AlkalmazasId;
                szukkodttartal.DataView.RowFilter = filter;
                kodtartal.DataView.RowFilter = filter;
            }
            if (szukkodttartal.DataView.Count == 0 && kodtartal.DataView.Count == 0)
            {
                MessageBox.Show(" Nincsenek sem kódtáblák, sem szűkitett kódtáblák!");
                this.Visible = false;
                return false;
            }
            else if (Tervezoe)
                return true;
            Tabinfo.DataView.RowFilter = tabinfofilter;
            for (int i = 0; i < Tabinfo.DataView.Count; i++)
            {
                string azontip = Tabinfo.DataView[i].Row["AZONTIP"].ToString();
                string adatfajta = azontip.Substring(3, 1);
                //                   string alkalm = Tabinfo.DataView[i].Row["ALKALMAZAS_ID"].ToString();
                int count = 0;
                string sorfilter = "AZONTIP = '" + azontip + "' and " + filter;
                if (adatfajta == "S")
                {
                    szukkodttartal.DataView.RowFilter = sorfilter;
                    count = szukkodttartal.DataView.Count;
                }
                else
                {
                    kodtartal.DataView.RowFilter = sorfilter;
                    count = kodtartal.DataView.Count;
                }
                if (count == 0)
                {
                    {
                        Tabinfo.Adatsortorol(i);
                        modositott = true;
                        i = -1;
                    }
                }
            }
            comboinfoarr.Clear();
            szukkodttartal.DataView.RowFilter = filter;
            kodtartal.DataView.RowFilter = filter;
            Beszurhakell(szukkodttartal, tabinfofilter, comboinfoarr);
            Beszurhakell(kodtartal, tabinfofilter, comboinfoarr);
            Tabinfo.DataView.RowFilter = tabinfofilter;
            comboinfok = (Comboinfok[])comboinfoarr.ToArray(typeof(Comboinfok));
            Tabinfo.ComboInfok = comboinfok;
            modositott = Tabinfo.Modositott;
            if (modositott)
            {
                FakUserInterface.Rogzit(Tabinfo);
                FakUserInterface.ComboInfok.Kiajanlinfo = Tabinfo;
            }
            return modositott;
        }
        
        private void Beszurhakell(Tablainfo tartal, string tabinfofilter, ArrayList comboinfoarr)
        {
            for (int i = 0; i < tartal.DataView.Count; i++)
            {
                string azontip = tartal.DataView[i].Row["AZONTIP"].ToString();
                string adatfajta = azontip.Substring(3, 1);
                Tabinfo.DataView.RowFilter = "AZONTIP = '" + azontip +"' AND " + tabinfofilter;
                //if (tabinfofilter != "")
                //    Tabinfo.DataView.RowFilter += " AND " + tabinfofilter;
                Tablainfo egytabinfo = FakUserInterface.GetByAzontip(azontip);
                Comboinfok comboinf = FakUserInterface.ComboInfok.ComboinfoKeres(azontip);
                comboinfoarr.Add(comboinf);
                DataRow egyrow = null;
                if (egytabinfo.DataView.Count != 0)
                    egyrow = egytabinfo.DataView[0].Row;
                if (egyrow != null)
                {
                    DataRow row;
                    bool kellmod = true;
                    if (adatfajta == "S")
                    {
                        if(Tabinfo.DataView.Count==0)
                           kellmod = egyrow["RSORSZAM"].ToString() != "0";
                    }
                    else if (Tabinfo.DataView.Count!=0)
                        kellmod = egyrow["SORSZAM"].ToString() != Tabinfo.DataView[0].Row["SORSZAM"].ToString();
                    if (kellmod)
                    {
                        if (Tabinfo.DataView.Count == 0)
                            row = Tabinfo.Ujsor();
                        else
                        {
                            row = Tabinfo.DataView[0].Row;
                            row["MODOSITOTT_M"] = 1;
                            Tabinfo.Modositott = true;
                        }
                        Tabinfo.DataView.RowFilter = tabinfofilter;
                        row["AZONTIP"] = azontip;
                        row["ALKALMAZAS_ID"] = FakUserInterface.AlkalmazasId;
                        row["RSORSZAM"] = egyrow["RSORSZAM"];
                        row["SORSZAM"] = egyrow["SORSZAM"];
                        row["PREV_ID"] = row["VERZIO_ID"];
                        if (row["TIP"] != null)
                            row["TIP"] = azontip.Substring(4);

                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void rogzit_Click(object sender, EventArgs e)
        {
            if (Tervezoe || !Tervezoe && Hivo.Hivo.RogzitesElott())
            {
                base.rogzit_Click(sender, e);
                if (Tabinfo.Tablanev == "KIAJANL")
                    FakUserInterface.ComboInfok.Kiajanlinfo = Tabinfo;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void comboBox1_TextUpdate(object sender, EventArgs e)
        {
            if (aktivcol != null)
            {
                if (aktivcol.ColumnName != "SZULOTABLA" && aktivcol.ColumnName!="TABLANEV" || comboBox1.Text == "")
                    base.comboBox1_TextUpdate(sender, e);
            }
        }
        /// <summary>
        /// egyedi hibavizsgalatok
        /// </summary>
        /// <param name="dcell">
        /// a cella, melynek tartalmat vizsgalom
        /// </param>
        /// <param name="tabinfo">
        /// </param>
        /// <returns></returns>
        public override string EgyediHibavizsg(DataGridViewCell dcell,Tablainfo tabinfo)
        {
            if (dcell.Value != null)
            {
                string tartal = dcell.Value.ToString();
                Cols mezocol = InputColumns[dcell.RowIndex];
                string colname = mezocol.ColumnName;
                if (Tabinfo.Tablanev == "TARTAL" )
                {
                    if (colname == "KODTIPUS")
                    {
                        if (Tabinfo.ViewSorindex != -1 && Tabinfo.InputColumns["KODTIPUS"].OrigTartalom != tartal)
                            return ("Kódtipus nem változtatható, törölni kell a sort!");
                    }
                    if (colname == "TABLANEV" && Beszur && FakUserInterface.Adatszintek.Contains(Tabinfo.Szint))
                    {
                        Tablainfo info = FakUserInterface.GetBySzintPluszTablanev("R", "TABLANEVEK");
                        string[] tartal1 = FakUserInterface.GetTartal(info, "ALKALMAZAS_ID", "SZOVEG", tartal);
                        if (tartal1 != null)
                        {
                            mezocol = Tabinfo.TablaColumns["OWNER"];
                            mezocol.Tartalom = tartal1[0];
                            mezocol.Kiegcol.ComboAktFileba = tartal1[0];
                            return "";
                        }
                    }
                }
                if (Tabinfo.Tablanev == "CEGSZERZODES")
                    return Hivo.Hivo.EgyediHibavizsg(dcell, Tabinfo);
                if (Tabinfo.Tablanev == "TABLANEVEK" && Tabinfo.Szint == "R")
                {
                    if (colname == "SZINT" || colname == "SZOVEG" || colname == "SZULOTABLA")
                    {
                        string origszint = "";
                        string origszoveg = "";
                        string jelenszint = Tabinfo.InputColumns["SZINT"].ComboAktFileba;
                        string jelenszoveg = Tabinfo.InputColumns["SZOVEG"].Tartalom;
                        if (jelenszoveg != "")
                        {
                            string szulotablanev = Tabinfo.InputColumns["SZULOTABLA"].Tartalom;
                            if (colname == "SZULOTABLA")
                                szulotablanev = mezocol.ComboAktSzoveg;
                            string szulotablaszint = "";
                            DataTable tabla = new DataTable("Tabla");
                            tabla = Sqlinterface.GetSchemaTable(tabla, FakUserInterface.AktualCegconn, jelenszoveg);
                            ArrayList identitynevek = new ArrayList();
                            if (szulotablanev != "")
                                szulotablaszint = Tabinfo.InputColumns["SZULOSZINT"].ComboAktFileba;
                            if (colname == "SZULOTABLA")
                            {
                                if (jelenszint != "" && jelenszoveg != "" && szulotablanev != "" && szulotablaszint != "")
                                {
                                    AddIdentityName(ref identitynevek, szulotablanev);
                                    do
                                    {
                                        Tabinfo.DataView.RowFilter = "SZOVEG = '" + szulotablanev + "'";
                                        for (int i = 0; i < Tabinfo.DataView.Count; i++)
                                        {
                                            string kovszulonev = Tabinfo.DataView[i].Row["SZOVEG"].ToString();
                                            szulotablanev = Tabinfo.DataView[i].Row["SZULOTABLA"].ToString();
                                            AddIdentityName(ref identitynevek, kovszulonev);
                                        }
                                    } while (szulotablanev != "");
                                }
                                string hibaszov = "";
                                for (int i = 0; i < identitynevek.Count; i++)
                                {
                                    string nev = identitynevek[i].ToString();
                                    bool megvan = false;
                                    foreach (DataRow row in tabla.Rows)
                                    {
                                        colname = row["ColumnName"].ToString();
                                        if (colname == nev)
                                        {
                                            megvan = true;
                                            break;
                                        }
                                    }
                                    if (!megvan)
                                    {
                                        if (hibaszov != "")
                                            hibaszov += ",";
                                        hibaszov += nev;
                                    }
                                }
                                if (hibaszov != "")
                                    hibaszov += " mezö nincs a " + jelenszoveg + "-ben!";
                                Tabinfo.DataView.RowFilter = "";
                                return hibaszov;
                            }
                        }
                        else
                        {
                            for (int i = 0; i < Tabinfo.DataView.Count; i++)
                            {
                                if (i != Tabinfo.ViewSorindex)
                                {
                                    DataRow dr = Tabinfo.DataView[i].Row;
                                    origszint = dr["SZINT"].ToString().Trim();
                                    origszoveg = dr["SZOVEG"].ToString();
                                    if (origszint == jelenszint && origszoveg == jelenszoveg)
                                        return "Azonos szint és táblanév a " + (i + 1).ToString() + " sorban";
                                }
                            }
                            if (!Beszur)
                            {
                                string origtart = Tabinfo.AktualRow[colname].ToString().Trim();
                                string tart = dcell.Value.ToString().Trim();

                                if (origtart != tart)
                                {
                                    int rowindex;
                                    string szint;
                                    string tablanev;
                                    if (colname == "SZINT")
                                    {
                                        szint = origtart;
                                        rowindex = Tabinfo.InputColumns.IndexOf("SZOVEG");
                                        tablanev = Inputtabla.Rows[rowindex][1].ToString();
                                    }
                                    else
                                    {
                                        tablanev = origtart;
                                        rowindex = Tabinfo.InputColumns.IndexOf("SZINT");
                                        szint = Tabinfo.InputColumns["SZINT"].ComboAktFileba;
                                    }
                                    if (szint != "" && tablanev != "")
                                    {
                                        Tablainfo info = FakUserInterface.GetBySzintPluszTablanev(szint, tablanev);
                                        if (info != null)
                                            return "Már szerepel a felvett term.táblázatok között,\n elöbb ott kell törölni!";
                                    }
                                }
                            }
                        }

                    }
                }
                //                }
                else if (colname == "TABLANEV" && tablanevtag !=null && mezocol.Comboe && !Beszur && dcell.Value.ToString() != Tabinfo.AktualViewRow["TABLANEV_K"].ToString())
                    return "Nem módositható, törölni kell!";
                if(!Tervezoe)
                    return Hivo.Hivo.EgyediHibavizsg(dcell, Tabinfo);
                return "";
            }
            else
                return "";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            comboBox1.Tag = null;
            base.dataGridView2_CellClick(sender, e);
  //          if ((Tabinfo.Tablanev == "CEGKEZELOK" || Tabinfo.Tablanev=="RENDSZERGAZDA") && !Tervezoe)
            if (Tabinfo.Tablanev == "RENDSZERGAZDA" && !Tervezoe)
            {
                if (aktivcol != null && aktivcol.Comboe && aktivcol.ColumnName != "ALKALMAZAS_ID")
                {
                    kezelotag = new MezoTag(Tabinfo, aktivcol.ColumnName, FakUserInterface, null, null, null, null);
                    kezelotag.Control = comboBox1;
                    comboBox1.Tag = kezelotag;
                    Osszefinfo kezalkalm = FakUserInterface.GetOsszef("U", "KezeloAlkalm").Osszefinfo;
                    Tablainfo kezelok = kezalkalm.tabinfo2;
                    string[] idk = kezalkalm.GetSzurtOsszefIdk(new object[] { "", FakUserInterface.AlkalmazasId });
                    if (idk != null)
                    {
                        FakUserInterface.Comboinfoszures(comboBox1, idk);
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void torolalap_Click(object sender, EventArgs e)
        {
            if (Tabinfo.Tablanev == "TABLANEVEK")
            {
                DataRow dr = Tabinfo.AktualViewRow;
                TablainfoCollection termtablak = FakUserInterface.GetCegPluszCegalattiTermTablaInfok();
                string szint = dr["SZINT"].ToString();
                string tablanev = dr["SZOVEG"].ToString();
                foreach (Tablainfo egyinfo in termtablak)
                {
                    if (egyinfo.Szint == szint && egyinfo.Tablanev == tablanev)
                    {
                        FakPlusz.MessageBox.Show("Már szerepel a felvett term.táblázatok között,\n elöbb ott kell törölni!");
                        return;
                    }
                }
            }
            base.torolalap_Click(sender, e);
        }
        private void AddIdentityName(ref ArrayList ar, string tablanev)
        {
            DataTable dt = new DataTable();
            dt = Sqlinterface.GetSchemaTable(dt, FakUserInterface.AktualCegconn, tablanev);
            if (dt != null)
            {
                foreach (DataRow drow in dt.Rows)
                {
                    string colname = drow["ColumnName"].ToString().Trim();
                    if ((bool)drow["IsAutoIncrement"])
                    {
                        if (ar.IndexOf(colname) == -1)
                            ar.Add(colname);
                        return;
                    }
                }
            }
        }
    }
}
