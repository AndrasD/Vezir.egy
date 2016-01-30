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
    /// Fogalommeghatarozas attekintesenek UserControl-ja. Megjelenit minden olyan csoportmeghatarozast, melynek
    /// valamelyik eleme a kivant fogalom
    /// </summary>
    public partial class Fogalom : Csakgrid
    {
        private DataView dataView = new DataView();
        private AdatTabla adattabla = new AdatTabla("FOGALMAK");
        /// <summary>
        /// objectum letrehozasa
        /// </summary>
        /// <param name="vezerles">
        /// vezerloinformacio
        /// </param>
        public Fogalom(Vezerloinfo vezerles)
        {
            InitializeComponent();
            ParameterAtvetel(vezerles, false);
            HozferJog = HozferJogosultsag.Csakolvas;
        }
        /// <summary>
        /// Inicializalas, a DataGridView-ban megjelenitendo DataView osszeallitas
        /// </summary>
        public override void AltalanosInit()
        {
            bool valt = UjTag;
            if (!valt)
            {
                valt = !Tabinfo.KellVerzio && ValtozasLekerdezExcept(new string[] { "Verziovaltozas" }).Count != 0 ||
                    Tabinfo.KellVerzio && ValtozasLekerdez().Count != 0;
            }
            if (!valt)
            {
                if(MezoControlInfok[0]!=null)
                    MezoControlInfok[0].UserControlInfo = UserControlInfo;
                return;
            }
            UjTag = false;
            ValtozasTorol();
            Tabinfo = TablainfoTag.Tablainfo;
            AktivPage.Text = AktivDropDownItem.Text + " áttekintése";
            string azontip = Tabinfo.Azontip1;
            Tabinfo = FakUserInterface.Tablainfok.GetByAzontip(azontip);
            DataView = Tabinfo.DataView;
            string fejlec = Tabinfo.Szoveg;
            Aktualtablainfo = new Tablainfo[] { Tabinfo };
            adattabla.Columns.Clear();
            adattabla.Rows.Clear();
            dataView.Table = null;
            FakUserInterface.Select(adattabla, Tabinfo.Adattabla.Connection, Tabinfo.Tablanev, Tabinfo.Adattabla.LastSel, Tabinfo.OrderString, false);
            dataView.Table = adattabla;
            dataView.Sort = DataView.Sort;
            toolStrip1.Visible = false;
            Tabinfo.AktualControlInfo = FakUserInterface.ControlTagokTolt(this, panel2, ref Tabinfo, AktivPage, null, null);
            MezoControlInfok[0] = Tabinfo.AktualControlInfo;
            UserControlInfo = FakUserInterface.Attach(this, Vezerles, ref Aktualtablainfo, AktivPage, null, null);
            MezoControlInfok[0].UserControlInfo = UserControlInfo;
            dataGridView1.Columns.Clear();
            Cols egycol = Tabinfo.TablaColumns["SZOVEG"];
            dataGridView1.Columns.Add(Tabinfo.Ujtextcolumn("SZOVEG", fejlec, true));
            int idacol = Tabinfo.IdentityColumnIndex;
            string aktverzid;
            string maxverzid;
            bool kell;
            bool forditott;
            Tablainfo[] infok = FakUserInterface.Tablainfok.GetByTermszarm("SZ");
            foreach (Tablainfo tabinfo in infok)
                //for (int i = 0; i < infok.Length; i++)
                {
                    kell = false;
                    forditott = false;
                    TablainfoTag egytag = tabinfo.TablaTag;
                    if (tabinfo.Adatfajta == "C" && tabinfo.Osszefinfo != null)
                    {
                        if (tabinfo.Azontip1 == azontip)
                            kell = true;
                        else if (tabinfo.Azontip2 == azontip)
                        {
                            kell = true;
                            forditott = true;
                        }
                    }
                    if (kell)
                    {
                        Tablainfo egytabinfo = egytag.Tablainfo;
                        aktverzid = egytabinfo.AktVerzioId.ToString();
                        maxverzid = egytabinfo.LastVersionId.ToString();

                        if (egytabinfo.KellVerzio && aktverzid != maxverzid)
                        {
                            egytabinfo.AktVerzioId = Convert.ToInt32(maxverzid);
                            string sel = egytabinfo.SelectString;
                            if (sel == "")
                                sel = " where ";
                            else
                                sel += " and ";
                            sel += "VERZIO_ID='" + maxverzid + "'";
                            egytabinfo.Adattabla.LastSel = sel;
                            egytabinfo.Adattabla.Rows.Clear();
                            FakUserInterface.Select(egytabinfo.Adattabla, egytabinfo.Adattabla.Connection, egytabinfo.Tablanev, sel, egytabinfo.OrderString, false);
                        }
                        DataTable Adattablac = egytabinfo.Adattabla;
                        int identc1col;
                        int identc2col;
                        Tablainfo elem2info;
                        if (!forditott)
                        {
                            identc1col = Adattablac.Columns.IndexOf("SORSZAM1");
                            identc2col = Adattablac.Columns.IndexOf("SORSZAM2");
                            elem2info = egytabinfo.Osszefinfo.tabinfo2;
                        }
                        else
                        {
                            identc1col = Adattablac.Columns.IndexOf("SORSZAM2");
                            identc2col = Adattablac.Columns.IndexOf("SORSZAM1");
                            elem2info = egytabinfo.Osszefinfo.tabinfo1;
                        }
                        aktverzid = elem2info.AktVerzioId.ToString();
                        maxverzid = elem2info.LastVersionId.ToString();
                        if (elem2info.KellVerzio && aktverzid != maxverzid)
                        {
                            elem2info.AktVerzioId = Convert.ToInt32(maxverzid);
                            string sel = elem2info.SelectString;
                            if (sel == "")
                                sel = " where ";
                            else
                                sel += " and ";
                            sel += "VERZIO_ID='" + maxverzid + "'";
                            elem2info.Adattabla.LastSel = sel;
                            elem2info.Adattabla.Rows.Clear();
                            FakUserInterface.Select(elem2info.Adattabla, elem2info.Adattabla.Connection, elem2info.Tablanev, sel, elem2info.OrderString, false);
                        }
                        DataTable Adattabla2 = elem2info.Adattabla;
                        int ident2col = elem2info.IdentityColumnIndex;
                        int kod2col = elem2info.Kodcol;
                        int szoveg2col = elem2info.Adattabla.Columns.IndexOf(elem2info.SzovegColName);
                        DataColumn egydatcol;
                        string colszov;
                        string propname;
                        if (!forditott)
                            colszov = tabinfo.Szoveg2;
                        else
                            colszov = tabinfo.Szoveg1;
                        propname = colszov;
                        if (elem2info.Kodtipus == "9997")
                            colszov = egytabinfo.Kodtipus;
//                        if (adattabla.Columns.IndexOf(colszov) == -1 && adattabla.Columns.IndexOf(propname)==-1)
                        {
                            egydatcol = new DataColumn(colszov, Type.GetType("System.String"));
                            adattabla.Columns.Add(egydatcol);
                            dataGridView1.Columns.Add(Tabinfo.Ujtextcolumn(colszov, colszov, true));
                        }
                        for (int l = 0; l < dataView.Count; l++)
                        {
                            DataRow dr = dataView[l].Row;
                            string ida = dr[idacol].ToString().Trim();
                            foreach (DataRow drk in Adattablac.Rows)
                            {
                                if (drk[identc1col].ToString().Trim() == ida)
                                {
                                    string idc = drk[identc2col].ToString().Trim();
                                    foreach (DataRow drl in Adattabla2.Rows)
                                    {
                                        string idl = drl[ident2col].ToString().Trim();
                                        if (idl == idc)
                                        {
                                            string szov = drl[szoveg2col].ToString().Trim();
                                            dr[adattabla.Columns.Count - 1] = szov;
                                            break;
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            dataGridView1.DataSource = dataView;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }
        
        /// <summary>
        /// ures
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
        }
        /// <summary>
        /// ures
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
        }
        /// <summary>
        /// ures
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}
