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
    /// Osszefuggesek karbantartasanak UserControl-ja
    /// </summary>
    public partial class Osszef : Gridpluszinput
    {
        private Osszefinfo osszefinfo;
        private int identacol = -1;
        private int identa1col = -1;
        private int identa2col = -1;
        private int identicol = -1;
        private int previdcol = -1;
        private int previd1col = -1;
        private int previd2col = -1;
        //private int szov1col = -1;
        //private int szov2col = -1;
        private int inputszovcol;
        private int szovegcolview2;
        //private DataView dataView1;
        //private DataView dataView2;
        private int viewindex = 0;
        private string[] origert;
        //        private bool ujverzio = false;
        private Tablainfo tabla;
        private string id1;
        private string id2;
        private string filternev1;
        private string filternev2;

        /// <summary>
        /// objectum letrehozasa
        /// </summary>
        /// <param name="vezerles">
        /// vezerloinformacio
        /// </param>
        public Osszef(Vezerloinfo vezerles)
        {
            InitializeComponent();
            ParameterAtvetel(vezerles, false);
        }
        /// <summary>
        /// Inicializalas, ha meg nincs adat, ennek jelzese utan kilep
        /// A felso DataGridView-ban megjelenitendo DataView osszeallitas
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
                MezoControlInfok[0].UserControlInfo = UserControlInfo;
                return;
            }
            UjTag = false;
            bool verzvaltas = ValtozasLekerdez("Verziovaltozas").Count != 0;
            ValtozasTorol();
            if (verzvaltas)
            { 
                foreach (Control page in AktivPage.Parent.Controls)
                {
                    if (page != AktivPage)
                    {
                        if (page.Controls.Count != 0)
                        {
                            Base cont = (Base)page.Controls[0];
                            cont.ValtozasTorol("Verziovaltozas");
                        }
                    }
                }
            }
            Tabinfo = TablainfoTag.Tablainfo;
            if (!Tervezoe)
                Hivo.Hivo.AltalanosInit();
            HozferJog = Tabinfo.Azonositok.Jogszintek[(int)KezeloiSzint];
            if (LezartCeg && HozferJog == Base.HozferJogosultsag.Irolvas && Tabinfo.Szint=="C")
                HozferJog = Base.HozferJogosultsag.Csakolvas;
            Tabinfo.HozferJog = HozferJog;
            Tabinfo.Hivo = Hivo;
            DataView = Tabinfo.DataView;
            Tabinfo.AktualControlInfo = FakUserInterface.ControlTagokTolt(this, panel2, ref Tabinfo, AktivPage, AktivMenuItem, AktivDropDownItem);
            MezoControlInfok[0] = Tabinfo.AktualControlInfo;
            UserControlInfo = FakUserInterface.Attach(this, Vezerles, ref Tabinfo, AktivPage, AktivMenuItem, AktivDropDownItem);
            MezoControlInfok[0].UserControlInfo = UserControlInfo;
            osszefinfo = Tabinfo.Osszefinfo;
            osszefinfo.InitKell = true;
            osszefinfo.OsszefinfoInit();
            if (!osszefinfo.Osszefinfotolt())
            {
                if (DataView.Count != 0)
                {
                    Tabinfo.TeljesTorles();
                    Rogzit();
                }
                AktivPage.Controls.Remove(this);
                //if (!Tervezoe)
                //{
                //    Hivo.Hivo.Hivo.Visible = true;
                //    Hivo.Hivo.Hivo.Refresh();
                //}
                if(!Hivo.Elsoeset)
                MessageBox.Show(" Nincs adat az összefüggéshez!");
//                AktivPage.Controls.Remove(this);
                return;
            }
            Aktualtablainfo = new Tablainfo[] { Tabinfo };
            identacol = osszefinfo.identcol;
            identa1col = osszefinfo.sorszam1col;
            identa2col = osszefinfo.sorszam2col;
            previdcol = Tabinfo.PrevIdcol;
            previd1col = Tabinfo.PrevId1col;
            previd2col = Tabinfo.PrevId2col;

            //szov1col = Tabinfo.Szoveg1col;
            //szov2col = Tabinfo.Szoveg2col;
            Beszurhat = false;
            //Tabinfo.AktualControlInfo = FakUserInterface.ControlTagokTolt(this, panel2, ref Tabinfo, AktivPage, AktivMenuItem, AktivDropDownItem);
            //MezoControlInfok[0] = Tabinfo.AktualControlInfo;
            //UserControlInfo = FakUserInterface.Attach(this, Vezerles, ref Tabinfo, AktivPage, AktivMenuItem, AktivDropDownItem);
            //MezoControlInfok[0].UserControlInfo = UserControlInfo;

            if (HozferJog == Base.HozferJogosultsag.Irolvas)
            {
                Tabinfo.Modosithat = true;
                Modosithat = true;
            }
            else
            {
                Tabinfo.Modosithat = false;
                Modosithat = false;
            }
            VerziobuttonokAllit();
            dataGridView2.ReadOnly = false;

            if (UjVerzio || HozferJog == HozferJogosultsag.Csakolvas)
                dataGridView2.ReadOnly = true;
            else
                //            {
                dataGridView2.ReadOnly = false;
            //if (DataView.Count == 0)
            //    ujverzio = true;
            //else
            //    ujverzio = false;
            //            }
            osszefinfo.AktualTag = TablainfoTag;
            osszefinfo.DataGridView1 = dataGridView1;
            dataView1 = osszefinfo.AktualDataView1;
            Inputtabla = osszefinfo.Inputtabla;
            osszefinfo.DataGridView2 = dataGridView2;
            dataView2 = osszefinfo.AktualDataView2;
            szovegcolview2 = osszefinfo.szoveg2col;
            filternev1 = "SORSZAM1";
            filternev2 = "SORSZAM2";
            if (TablainfoTag.Forditott)
            {
                filternev1 = "SORSZAM2";
                filternev2 = "SORSZAM1";
                szovegcolview2 = osszefinfo.szoveg1col;
            }
            viewindex = 0;
            for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
            {
                viewindex = dataGridView1.SelectedRows[i].Index;
                dataGridView1.SelectedRows[i].Selected = false;
            }
            if (viewindex > dataView1.Count)
                viewindex = 0;
            identicol = 2;
            dataGridView1.Rows[viewindex].Selected = true;
            //            Tablainfo tabla;
            if (dataView1.Table.TableName == osszefinfo.Adattabla1.TableName)
            {
                if (dataView1.Table.TableName == "OSSZEF")
                    tabla = osszefinfo.Osszefinfo1.tabinfo1;
                else
                    tabla = osszefinfo.tabinfo1;
            }
            else if (dataView2.Table.TableName == "OSSZEF")
                tabla = osszefinfo.Osszefinfo1.tabinfo2;
            else
                tabla = osszefinfo.tabinfo2;
            inputszovcol = tabla.TablaColumns.IndexOf("SZOVEG");
            if (inputszovcol == -1)
                inputszovcol = tabla.Azonositocol;
            if (FakUserInterface.Alkalmazas != "TERVEZO")
            {
                if (osszefinfo.alkalmid1col != -1)
                    dataView1.RowFilter = "ALKALMAZAS_ID = " + FakUserInterface.AlkalmazasId;
                if (osszefinfo.alkalmid2col != -1)
                    dataView2.RowFilter = "ALKALMAZAS_ID = " + FakUserInterface.AlkalmazasId;
                if (osszefinfo.tabinfo1.Kodtipus == "Alkalm")
                    dataView1.RowFilter = "SZOVEG='" + FakUserInterface.Alkalmazas + "'";
                if (osszefinfo.tabinfo2.Kodtipus == "Alkalm")
                    dataView2.RowFilter = "SZOVEG='" + FakUserInterface.Alkalmazas + "'";
            }
            //           if (ujverzio)
            //           {
            //               for (int i = 0; i < dataView1.Count; i++)
            //               {
            //                   viewindex = i;
            //                   Inputtablatolt(dataView1[viewindex].Row);
            //                   object s = ok;
            //                   ok_Click(s, new EventArgs());
            //               }
            //               Tabinfo.Modositott = true;
            //               Rogzit();
            ////               ujverzio = false;
            //           }
            viewindex = 0;
            Inputtablatolt(dataView1[viewindex].Row);
            //           ujverzio = false;
            rogzit.Enabled = false;

        }
        /// <summary>
        /// Az also DataGridView-ban a felso GridView kivant soranak megjelenitese karbantartashoz
        /// </summary>
        /// <param name="dr"></param>
        private void Inputtablatolt(DataRow dr)
        {
            DataView.RowFilter = "";
            long defert = 0;
            if (DataView.Count == 0)
                defert = Convert.ToInt16(osszefinfo.tabinfo.Azonositok.Defert);
            if (viewindex == 0)
                elozo.Enabled = false;
            else
                elozo.Enabled = true;
            if (viewindex == dataView1.Count - 1)
                kovetkezo.Enabled = false;
            else
                kovetkezo.Enabled = true;
            if (dr.Table.TableName == "LEIRO")
            {
                label2.Text = dr["SORSZOV"].ToString().Trim();
                inputszovcol = dr.Table.Columns.IndexOf("SORSZOV");
            }
            else if (dr.Table.TableName == "OSSZEF")
            {
                string[] szov = FakUserInterface.GetTartal(tabla, "SZOVEG", "SORSZAM", dr["SORSZAM1"].ToString());
                label2.Text = szov[0];
            }
            else
                label2.Text = dr[inputszovcol].ToString().Trim();
            bool vancsillag = AktivPage.Text.Contains("*");
            bool vanfelkialtojel = AktivPage.Text.Contains("!");
            AktivPage.Text = TablainfoTag.Azonositok.Szoveg;
            if (Tabinfo.HozferJog == HozferJogosultsag.Irolvas && (!Tabinfo.KellVerzio || Tabinfo.VerzioTerkepArray.Count != 0))
            {
                AktivPage.Text += " karbantartása";
                label2.Text += " módositása";
                ok.Visible = true;
            }
            else
            {
                AktivPage.Text += " áttekintése";
                label2.Text += " megtekintése";
                ok.Visible = false;
            }
            if (vancsillag)
                AktivPage.Text += "*";
            if (vanfelkialtojel)
                AktivPage.Text += "!";
            Inputtabla.Rows.Clear();
            DataRow row;
            DataRow NewRow;
            //            DataRow drr;
            string id1 = "";
            string id2 = "";
            id1 = dr[osszefinfo.ident1col].ToString().Trim();
            for (int i = 0; i < dataView2.Count; i++)
            {
                row = dataView2[i].Row;
                id2 = row[osszefinfo.ident2col].ToString().Trim();
                if (DataView.Count != 0)
                {
                    DataView.RowFilter = filternev1 + " = " + id1 + " AND " + filternev2 + " = " + id2;
                    if (DataView.Count == 0)
                        defert = 0;
                    else
                        defert = 1;
                    DataView.RowFilter = "";
                }
                NewRow = Inputtabla.NewRow();
                if (dataView2.Table.TableName == "OSSZEF")
                {
                    string[] szov = FakUserInterface.GetTartal(tabla, "SZOVEG", "SORSZAM", row["SORSZAM2"].ToString());
                    NewRow[0] = szov[0];
                }
                else if (dataView2.Table.TableName != "LEIRO")
                {
                    int j = szovegcolview2;// dataView2.Table.Columns.IndexOf(szovegcolview2);
                    if (j != -1)
                        NewRow[0] = row[j];
                    else
                        NewRow[0] = row[0];
                }
                else
                    NewRow[0] = row[inputszovcol];
                //if (ujverzio)
                //    NewRow[1] = Convert.ToInt16(osszefinfo.tabinfo.Azonositok.Defert);
                //else
                //    NewRow[1] = 0;
                NewRow[1] = defert;
                NewRow[2] = id2;
                int pridcol = dataView2.Table.Columns.IndexOf("PREV_ID");
                if (pridcol == -1)
                    NewRow[3] = id2;
                else
                    NewRow[3] = row[pridcol];
                //for (int j = 0; j < DataView.Count; j++)
                //{
                //    string hasona1;
                //    string hasona2;
                //    drr = DataView[j].Row;
                //    if (drr.RowState != DataRowState.Deleted)
                //    {
                //        if (!TablainfoTag.Forditott)
                //        {
                //            hasona1 = drr[identa1col].ToString().Trim();
                //            hasona2 = drr[identa2col].ToString().Trim();
                //        }
                //        else
                //        {
                //            hasona1 = drr[identa2col].ToString().Trim();
                //            hasona2 = drr[identa1col].ToString().Trim();
                //        }
                //        if (hasona1 == id1 && hasona2 == id2)
                //        {
                //            if(!ujverzio)
                //                NewRow[1] = defert;
                //            break;
                //        }
                //    }
                //}
                Inputtabla.Rows.Add(NewRow);
            }
            valtozott = new bool[Inputtabla.Rows.Count];
            hibaszov = new string[Inputtabla.Rows.Count];
            origert = new string[Inputtabla.Rows.Count];
            for (int i = 0; i < origert.Length; i++)
            {
                valtozott[i] = false;
                hibaszov[i] = "";
                origert[i] = Inputtabla.Rows[i][1].ToString();
            }
            ok.Enabled = false;
        }
        /// <summary>
        /// A felso GridView-bol uj sor kivalasztasa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                viewindex = e.RowIndex;
                Inputtablatolt(dataView1[e.RowIndex].Row);
            }
        }
        /// <summary>
        /// ures
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
        }
        /// <summary>
        /// ures
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void dataGridView2_BeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
        }
        /// <summary>
        /// ures
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void dataGridView2_Enter(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// Also GridView-ban cella editalas-nak vege, ha valtozas volt, ennek jelzes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void dataGridView2_EndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //DataGridViewRow dgrow = dataGridView2.Rows[e.RowIndex];
            //DataGridViewCell dcell = dgrow.Cells[1];
            //string newert = origert[e.RowIndex];
            //try
            //{
            //    newert = dcell.Value.ToString();
            //}
            //catch { }

            //if (newert != origert[e.RowIndex])
            //{
            //    Inputtabla.Rows[e.RowIndex][1] = newert;
            //    valtozott[e.RowIndex] = true;
            //    ok.Enabled = true;
            //}
        }
        public override void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow dgrow = dataGridView2.Rows[e.RowIndex];
            DataGridViewCell dcell = dgrow.Cells[e.ColumnIndex];
            if (!dcell.ReadOnly)
            {
                string newert = origert[e.RowIndex];
                if (newert == "0")
                    newert = "1";
                else
                    newert = "0";
                origert[e.RowIndex] = newert;
                Inputtabla.Rows[e.RowIndex][1] = newert;
                valtozott[e.RowIndex] = true;
                ok.Enabled = true;
            }
            //try
            //{
            //    newert = dcell.Value.ToString();
            //}
            //catch { }

            //if (newert == origert[e.RowIndex])
            //{
            //    Inputtabla.Rows[e.RowIndex][1] = newert;
            //    valtozott[e.RowIndex] = true;
            //    ok.Enabled = true;
            //}
        }
        // ures
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void dataGridView2_Scroll(object sender, ScrollEventArgs e)
        {
        }
        /// <summary>
        /// uj verzio eloallit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void uj_Click(object sender, EventArgs e)
        {
            ValtozasBeallit("Verziovaltozas");
            Tabinfo.CreateNewVersion();
            Tabinfo.Osszefinfo.NewVersionKieg();
            AltalanosInit();
        }
        /// <summary>
        /// verzio torles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void teljestorles_Click(object sender, EventArgs e)
        {
            ValtozasBeallit("Verziovaltozas");
            Tabinfo.DeleteLastVersion();
            AltalanosInit();
        }
        /// <summary>
        /// rogzit button event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void rogzit_Click(object sender, EventArgs e)
        {
            if (Tervezoe || !Tervezoe && Hivo.Hivo.RogzitesElott())
                Rogzit();
        }
        /// <summary>
        /// Rogzites
        /// </summary>
        public override void Rogzit()
        {
            if (DataView.Count == 0 && Tabinfo.KellVerzio)
            {
                if (Tabinfo.VerzioTerkepArray.Count != 0)
                {
                    Tabinfo.VerzioTerkepArray.RemoveAt(Tabinfo.VerzioTerkepArray.Count - 1);
                    Tabinfo.AktVerzioId = Tabinfo.LastVersionId;
                    ValtozasBeallit("Verziovaltozas");
                }
            }
            Tabinfo.Modositott = true;
            FakUserInterface.Rogzit(new Tablainfo[] { Tabinfo });
            if (Tabinfo.Kodtipus == "KezeloAlkalm")
                FakUserInterface.Kezeloszereprendberak(FakUserInterface.Tablainfok["CEGKEZELOKIOSZT"]);
            RogzitesUtan();

        }

        ///// <summary>
        ///// ures
        ///// </summary>
        //public override void RogzitesUtan()
        //{
        //}
        /// <summary>
        /// Mindent elolrol
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void elolrolalap_Click(object sender, EventArgs e)
        {
            FakUserInterface.ForceAdattolt(Tabinfo);
            AltalanosInit();
            viewindex = 0;
            Inputtablatolt(dataView1[viewindex].Row);
        }
        /// <summary>
        /// elozo sor, ha van
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void elozo_Click(object sender, EventArgs e)
        {
            if (NemkellOk())
            {
                viewindex--;
                Inputtablatolt(dataView1[viewindex].Row);
                SetSelectedRow(viewindex);
                SetAktRowVisible();
            }
        }
        /// <summary>
        /// kovetkezo sor, ha van
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void kovetkezo_Click(object sender, EventArgs e)
        {
            if (NemkellOk())
            {
                viewindex++;
                Inputtablatolt(dataView1[viewindex].Row);
                SetSelectedRow(viewindex);
                SetAktRowVisible();
            }
        }
        /// <summary>
        /// felso gridvieban lathatova teszi az aktualis sort
        /// </summary>
        public void SetAktRowVisible()
        {
            int firstdisprowindex = dataGridView1.FirstDisplayedScrollingRowIndex;
            if (firstdisprowindex > viewindex)
                dataGridView1.FirstDisplayedScrollingRowIndex = viewindex;
            else
            {
                int rowcount = dataGridView1.DisplayedRowCount(false);
                int teljesrowcount = dataGridView1.DisplayedRowCount(true);
                int lastvisible = rowcount + firstdisprowindex;
                int teljeslastvisible = teljesrowcount + firstdisprowindex;
                if (lastvisible == viewindex)
                    dataGridView1.FirstDisplayedScrollingRowIndex++;
                if (teljeslastvisible == viewindex)
                    dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.FirstDisplayedScrollingRowIndex + 2;
            }
        }
        /// <summary>
        /// A sor eredeti ertekei vissza
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void elolrol_Click(object sender, EventArgs e)
        {
            Inputtablatolt(dataView1[viewindex].Row);
        }
        /// <summary>
        /// sor modositasok elfogadasa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void ok_Click(object sender, EventArgs e)
        {
            bool hibas = false;
            DataRow row = null;
            DataRow drr;
            id1 = "";
            id2 = "";
            if (viewindex < dataView1.Count)
                id1 = dataView1[viewindex][osszefinfo.ident1col].ToString().Trim();
            int kell = 0;
            for (int i = 0; i < Inputtabla.Rows.Count; i++)
            {
                row = Inputtabla.Rows[i];
                kell = Convert.ToInt32(Inputtabla.Rows[i][1]);
                id2 = row[identicol].ToString().Trim();
                DataView.RowFilter = filternev1 + " = " + id1 + " AND " + filternev2 + " = " + id2;
                if (DataView.Count == 0)
                {
                    if (kell != 0)
                    {
                        DataView.RowFilter = "";
                        drr = Tabinfo.Ujsor();
                        if (!TablainfoTag.Forditott)
                        {
                            drr[identa1col] = id1;
                            if (osszefinfo.tabinfo1.KellVerzio)
                                drr[previd1col] = dataView1[viewindex]["PREV_ID"];
                            else
                                drr[previd1col] = drr[identa1col];
                            drr[identa2col] = id2;
                            drr[previd2col] = row[3];
                        }
                        else
                        {
                            drr[identa2col] = id1;// dataView1[viewindex][osszefinfo.ident1col];
                            if (osszefinfo.tabinfo2.KellVerzio)
                                drr[previd2col] = dataView1[viewindex]["PREV_ID"];
                            else
                                drr[previd2col] = drr[identa2col];
                            drr[identa1col] = id2;
                            drr[previd1col] = row[3];
                        }
                        if (DataView.Count == 1)
                            drr["SORREND"] = 100;
                        else
                        {
                            int maxsorrend = Convert.ToInt32(DataView[DataView.Count - 1].Row["SORREND"].ToString());
                            int elozosorrend = 100;
                            int kovsorrend = 0;
                            int sorrend = 0;
                            for (int j = 0; j < DataView.Count; j++)
                            {
                                DataRow viewrow = DataView[j].Row;
                                if (viewrow[filternev1].ToString() != id1 || viewrow[filternev2].ToString() != id2)
                                {
                                    elozosorrend = Convert.ToInt32(viewrow["SORREND"].ToString());
                                    if (j != DataView.Count - 1)
                                        kovsorrend = Convert.ToInt32(DataView[j + 1].Row["SORREND"].ToString());
                                    else
                                        kovsorrend = maxsorrend;
                                }
                                else
                                {
                                    sorrend = (kovsorrend - elozosorrend) / 2 + elozosorrend;
                                    break;
                                }
                            }
                            drr["SORREND"] = sorrend;
                        }
                        FakUserInterface.ValtoztatasFunkcio = "ADD";
                        Tabinfo.ValtozasNaplozas(drr);
                    }
                }
                else if (kell == 0)
                {
                    if (Tabinfo.Kodtipus == "KezeloAlkalm")
                    {
                        Comboinfok kezinf = FakUserInterface.ComboInfok.ComboinfoKeres("SZUTKEZELOK");
                        foreach (Tablainfo tinfo in kezinf.Tabinfok)
                        {
                            if (tinfo.Tablanev != "VALTOZASNAPLO" && tinfo.Tablanev != "USERLOG")
                            {
                                for (int ii = 0; ii < tinfo.DataView.Count; ii++)
                                {
                                    DataRow trow = tinfo.DataView[ii].Row;
                                    foreach (Cols egycol in tinfo.TablaColumns)
                                    {
                                        if (egycol.Combo_Info == kezinf)
                                        {
                                            string id = trow[egycol.ColumnName].ToString();
                                            if (id == id1)
                                            {
                                                if (tinfo.Tablanev != "CEGKEZELOKIOSZT")
                                                    hibas = true;
                                                else if(trow["SZEREPKOD"].ToString() != "10")
                                                {
                                                    hibas = true;
                                                }
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }
                    if (!hibas)
                        Tabinfo.Adatsortorol(0);
                }
            }
            if (!hibas)
            {
                dataGridView2.Rows[0].Cells[1].ErrorText = "";
                DataView.RowFilter = "";
                if (viewindex < dataView1.Count - 1)
                    viewindex++;
                else
                    viewindex = 0;
                Inputtablatolt(dataView1[viewindex].Row);
                SetSelectedRow(viewindex);
                SetAktRowVisible();
                rogzit.Enabled = true;
            }
            else
            {
                dataGridView2.Rows[0].Cells[1].ErrorText = " Már használatban!";
                rogzit.Enabled = false;
            }

        }
    }
}
