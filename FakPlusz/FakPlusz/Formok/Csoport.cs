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
    /// Csoportmeghatarozasok karbantartasanak UserControl-ja
    /// </summary>
    public partial class Csoport : Csakgrid
    {
        private int identa1col = -1;
        private int identa2col = -1;
        private ArrayList fileinfoarray;
        private ArrayList comboinfoarray;
        private string[] origtart;
        private int combocol;
        //private DataView dataView1;
        //private DataView dataView2;
        private Osszefinfo osszefinfo;
 //       private Tablainfo tabinfo2;
        private bool ujverzio = true;
        /// <summary>
        /// objectum letrehozasa
        /// </summary>
        /// <param name="vezerles">
        /// vezerloinformacio
        /// </param>
        public Csoport(Vezerloinfo vezerles)
        {
            InitializeComponent();
            ParameterAtvetel(vezerles, false);
        }
        /// <summary>
        /// Inicializalas, ha nincs adat, ennek jelzese utan kilep
        /// </summary>
        /// 
        public override void AltalanosInit()
        {
            bool valt = UjTag;
            if (!valt)
            {
                valt = !Tabinfo.KellVerzio && ValtozasLekerdezExcept(new string[] { "Verziovaltozas" }).Count != 0 ||
                    Tabinfo.KellVerzio && ValtozasLekerdez().Count != 0;
            }
            if (valt)
            {
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
                HozferJog = Tabinfo.Azonositok.Jogszintek[(int)KezeloiSzint];
                AktivPage.Text = AktivDropDownItem.Text;
                Tabinfo.HozferJog = HozferJog;
                Tabinfo.Hivo = Hivo;
                dataGridView1.ReadOnly = true;
                dataGridView1.Columns.Clear();
                Tabinfo.AktualControlInfo = FakUserInterface.ControlTagokTolt(this, panel2, ref Tabinfo, AktivPage, AktivMenuItem, AktivDropDownItem);
                MezoControlInfok[0] = Tabinfo.AktualControlInfo;
                UserControlInfo = FakUserInterface.Attach(this, Vezerles, ref Tabinfo, AktivPage, AktivMenuItem, AktivDropDownItem);
                MezoControlInfok[0].UserControlInfo = UserControlInfo;
                osszefinfo = Tabinfo.Osszefinfo;
                osszefinfo.InitKell = true;
                osszefinfo.OsszefinfoInit();
                if (!osszefinfo.Osszefinfotolt())
                {
                    AktivPage.Controls.Remove(this);
                    if(!Hivo.Elsoeset)
                        MessageBox.Show(" Nincs adat a csoporthoz!");
//                    AktivPage.Controls.Remove(this);
                    return;
                }
                osszefinfo.AktualTag = TablainfoTag;
                osszefinfo.DataGridView1 = dataGridView1;
                dataView1 = osszefinfo.AktualDataView1;
                dataView2 = osszefinfo.DataView2;
                DataView = Tabinfo.DataView;
                if (osszefinfo.tabinfo2.Kodtipus == "9997")
                {
                    dataGridView1.ColumnHeadersHeight = 70;
                    dataGridView1.Columns["SZOVEG"].HeaderText = Tabinfo.Kodtipus;
                }
                Aktualtablainfo = new Tablainfo[] { Tabinfo };
                identa1col = osszefinfo.sorszam1col;
                identa2col = osszefinfo.sorszam2col;
                Beszurhat = false;
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
            }
            else if(MezoControlInfok[0]!=null)
            {
                //Tabinfo.AktualControlInfo = FakUserInterface.ControlTagokTolt(this, panel2, ref Tabinfo, AktivPage, AktivMenuItem, AktivDropDownItem);
                //MezoControlInfok[0] = Tabinfo.AktualControlInfo;
                //UserControlInfo = FakUserInterface.Attach(this, Vezerles, ref Tabinfo, AktivPage, AktivMenuItem, AktivDropDownItem);
                MezoControlInfok[0].UserControlInfo = UserControlInfo;
                return;
            }
            if (!Tervezoe)
                Hivo.Hivo.AltalanosInit();
            Columntolt();
            VerziobuttonokAllit();
            string kieg = " módositása";
            if (HozferJog != HozferJogosultsag.Irolvas)
                kieg = " áttekintése";
            AktivPage.Text += kieg;
            if (UjVerzio || HozferJog==HozferJogosultsag.Csakolvas)
                dataGridView1.ReadOnly = true;
            else
                dataGridView1.ReadOnly = false;

        }
        /// <summary>
        /// A  DataGridView-ban megjelenitendo DataView osszeallitasa
        /// </summary>
        private void Columntolt()
        {
            if (!Tervezoe)
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
            origtart = new string[dataView1.Count];
            for (int i = 0; i < origtart.Length; i++)
                origtart[i] = "0";
            ujverzio = false;
            if (DataView.Count == 0)
                ujverzio = true;
            string id1 = "";
            //for (int i = 0; i < DataView.Count; i++)
            //{
            //    bool talalt = false;
            //    DataRow drr = DataView[i].Row;
            //    for (int j = 0; j < dataView1.Count; j++)
            //    {
            //        DataRow dr = dataView1[j].Row;
            //        id1 = dr[osszefinfo.ident1col].ToString().Trim();
            //        if (drr[identa1col].ToString().Trim() == id1)
            //        {
            //            talalt = true;
            //            break;
            //        }
            //    }
            //    if (!talalt)
            //    {
            //        Tabinfo.Adattabla.Adatsortorol(i);
            //        if (DataView.Count != 0)
            //            i = -1;
            //    }
            //}
            combocol = osszefinfo.valasztind;
            fileinfoarray = osszefinfo.ComboFileInfo;
            comboinfoarray = osszefinfo.ComboSzovInfo;
            int comboindex = 0;
            if (ujverzio && Tabinfo.Azonositok.Defert!="")
                comboindex = Convert.ToInt16(Tabinfo.Azonositok.Defert);
            ArrayList beszursorrendhez = new ArrayList();
            ArrayList viewidk = new ArrayList();
            for (int i = 0; i < dataView1.Count; i++)
            {
                origtart[i] = "0";
                DataRow dr = dataView1[i].Row;
                dr[combocol] = comboinfoarray[comboindex];
                if (DataView.Count == 0 && UjVerzio)
                {
                    dr["MODOSITOTT_M"] = 1;
                    Tabinfo.Changed = true;
                }
                else
                {
                    dr["MODOSITOTT_M"] = 0;
                    Tabinfo.Changed = false;
                }
                id1 = dr[osszefinfo.ident1col].ToString().Trim();
                string previd1;
                if (dataView1.Table.Columns.IndexOf("PREV_ID") != -1)
                    previd1 = dr["PREV_ID"].ToString();
                else
                    previd1 = id1;
                string id2;
                string previd2;
//                bool talalt = false;
                for (int j = 0; j < DataView.Count; j++)
                {
                    DataRow drr = DataView[j].Row;
                    if (drr[identa1col].ToString().Trim() == id1)
                    {
//                        talalt = true;
                        beszursorrendhez.Add(drr["SORREND"].ToString());
                        viewidk.Add(id1);
                        id2 = drr[identa2col].ToString().Trim();
                        previd2 = drr["PREV_ID"].ToString();
                        for (int k = 0; k < dataView2.Count; k++)
                        {
                            DataRow dr2 = dataView2[k].Row;
                            if (dr2[osszefinfo.ident2col].ToString().Trim() == id2)
                            {

                                string egyfilert = id2;
                                dr[combocol] = comboinfoarray[fileinfoarray.IndexOf(egyfilert)];
                                origtart[i] = egyfilert;
 //                               dr[combocol] = origtart[i]; // comboinfoarray[fileinfoarray.IndexOf(egyfilert)];
                                //                                origtart[i] = dr[combocol
                                break;
                            }
                        }
                        break;
                    }
                }
            }

//                if (!talalt)
//                {
//                    bool modositott = Tabinfo.Modositott;
////                    int ezenall = 0;
//                    if (viewidk.Count == 0)      // elso sor lesz belole
//                    {
//                        if (DataView.Count == 0)
//                            Beszursorrend = 100;
//                        else
//                            Beszursorrend = Convert.ToInt32(DataView[0].Row["SORREND"].ToString())/2;
//                    }
//                    else 
//                    {
//                        int sorrendmoge = Convert.ToInt32(beszursorrendhez[beszursorrendhez.Count - 1].ToString());
//                        if (sorrendmoge == Convert.ToInt32(DataView[DataView.Count - 1].Row["SORREND"].ToString()))
//                            Beszursorrend = sorrendmoge + 100;
//                        else
//                        {
//                            int sorrendele = Convert.ToInt32(DataView[beszursorrendhez.Count].Row["SORREND"].ToString());
//                            Beszursorrend = sorrendmoge + (sorrendele - sorrendmoge) / 2;
//                        }
//                    }
//                    //    elotte = Convert.ToInt32(Tabinfo.DataView[Tabinfo.ViewSorindex - 1].Row[sorrendcolindex].ToString());
//                    //    Beszursorrend = elotte + (ezenall - elotte) / 2;
//                    //}
//                    //if (aktsorrend == 0 && utsosorrend == 0)
//                    //    Beszursorrend = elsosorrend / 2;
//                    DataRow dra=Tabinfo.Adattabla.Ujsor(Beszursorrend);
//                    dra[identa1col] = Convert.ToInt64(id1);
//                    dra["PREV_ID1"] = previd1; ;
//                    dra["VERZIO_ID1"] = osszefinfo.tabinfo1.AktVerzioId;
//                    dra[identa2col] = fileinfoarray[comboindex];
//                    dra["PREV_ID2"] = fileinfoarray[comboindex];
//                    dra["VERZIO_ID2"] = osszefinfo.tabinfo2.AktVerzioId;
//                    dra["MODOSITOTT_M"] = 1;
//                    Tabinfo.Modositott = modositott;
//                    FakUserInterface.ValtoztatasFunkcio = "ADD";
//                    Tabinfo.ValtozasNaplozas(dra);
//                }
//                origtart[i] = dr[combocol].ToString();
//            }
            //for (int i = 0; i < DataView.Count; i++)
            //{
            //    DataRow dr = DataView[i].Row;
            //    id1 = dr[identa1col].ToString().Trim();
            //    bool talalt = false;
            //    for (int j = 0; j < dataView1.Count; j++)
            //    {
            //        DataRow drr = dataView1[j].Row;
            //        if (drr[osszefinfo.ident1col].ToString().Trim() == id1)
            //        {
            //            talalt = true;
            //            break;
            //        }
            //    }
            //    if (!talalt)
            //    {
            //        Tabinfo.Adatsortorol(i);
            //        if (DataView.Count != 0)
            //            i = -1;
            //    }
            //}
        }

        /// <summary>
        /// Minden elolrol, az eddigi valtozasokat elveszitjuk
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void elolrolalap_Click(object sender, EventArgs e)
        {
            FakUserInterface.ForceAdattolt(Tabinfo, true);
            Columntolt();
        }
        /// <summary>
        /// Uj verzio eloallitasa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void uj_Click(object sender, EventArgs e)
        {
            ValtozasBeallit("Verziovaltozas");
            Tabinfo.CreateNewVersion();
            Tabinfo.Osszefinfo.NewVersionKieg();
            AltalanosInit();
            Tabinfo.Modositott = true;
            FakUserInterface.Rogzit(Tabinfo);
        }
        /// <summary>
        /// torli a verziot
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
        /// Valtozasok rogzitese
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void rogzit_Click(object sender, EventArgs e)
        {
            if (Tabinfo.Changed)
            {
                DataRow dr;
                Tabinfo.Modositott = true;
                string comboert = "";
                string filert = "0";
                string id1 = "0";
                string id2 = "0";
                string previd2 = "0";
                string sorrend = "0";
                string previd1 = "0";
                string sorrendcolname = osszefinfo.tabinfo1.SorrendColumn.ColumnName;
                for (int i = 0; i < dataView1.Count; i++)
                {
                    dr = dataView1[i].Row;
                    comboert = dr[combocol].ToString().Trim();
                    filert = fileinfoarray[comboinfoarray.IndexOf((object)comboert)].ToString().Trim();
                    id1 = dr[osszefinfo.ident1col].ToString().Trim();
                    previd1 = dr["PREV_ID"].ToString();
                    id2 = "0";
                    previd2 = "0";
                    sorrend = dr[sorrendcolname].ToString();
                    bool modositott = dr["MODOSITOTT_M"].ToString() == "1";
                    if (modositott)
                    {
                        if (origtart[i] != "0")         // volt sor a DataView-ban
                        {
                            DataView.RowFilter = "SORSZAM1 = " + id1 + " AND SORSZAM2 = " + origtart[i];
                            if (filert == "0")
                                DataView[0].Row.Delete();
                            // ez torlest jelent
                            else        // ez modositas
                            {
                                for (int k = 0; k < dataView2.Count; k++)
                                {
                                    DataRow dr2 = dataView2[k].Row;
                                    if (dr2[osszefinfo.ident2col].ToString().Trim() == filert)
                                    {
                                        id2 = dr2[osszefinfo.ident2col].ToString().Trim();
                                        if (dataView2.Table.Columns.IndexOf("PREV_ID") != -1)
                                            previd2 = dr2["PREV_ID"].ToString();
                                        else
                                            previd2 = id2;
                                        break;
                                    }
                                }
                                DataView[0].Row["PREV_ID2"] = previd2;
                                DataView[0].Row["SORSZAM2"] = filert;
                            }
                            DataView.RowFilter = "";
                        }
                        else // uj sor a DataView-ban
                        {
                            DataRow dra = Tabinfo.Adattabla.Ujsor(Convert.ToInt32(dr[sorrendcolname].ToString()));
                            dra["SORSZAM1"] = id1;
                            dra["PREV_ID1"] = previd1;
                            dra["SORSZAM2"] = filert;
                            dra["SORREND"] = dr[sorrendcolname];
                            dra["VERZIO_ID1"] = osszefinfo.tabinfo1.AktVerzioId;
                            dra["VERZIO_ID2"] = osszefinfo.tabinfo2.AktVerzioId;
                            dra["MODOSITOTT_M"] = 1;                            //                if (!talalt)
                            for (int k = 0; k < dataView2.Count; k++)
                            {
                                DataRow dr2 = dataView2[k].Row;
                                if (dr2[osszefinfo.ident2col].ToString().Trim() == filert)
                                {
                                    id2 = dr2[osszefinfo.ident2col].ToString().Trim();
                                    if (dataView2.Table.Columns.IndexOf("PREV_ID") != -1)
                                        previd2 = dr2["PREV_ID"].ToString();
                                    else
                                        previd2 = id2;
                                    break;
                                }
                            }
                            dra["PREV_ID2"] = previd2;
                        }
                    }
                            //                {
                            //                    bool modositott = Tabinfo.Modositott;
                            ////                    int ezenall = 0;
                            //                    if (viewidk.Count == 0)      // elso sor lesz belole
                            //                    {
                            //                        if (DataView.Count == 0)
                            //                            Beszursorrend = 100;
                            //                        else
                            //                            Beszursorrend = Convert.ToInt32(DataView[0].Row["SORREND"].ToString())/2;
                            //                    }
                            //                    else 
                            //                    {
                            //                        int sorrendmoge = Convert.ToInt32(beszursorrendhez[beszursorrendhez.Count - 1].ToString());
                            //                        if (sorrendmoge == Convert.ToInt32(DataView[DataView.Count - 1].Row["SORREND"].ToString()))
                            //                            Beszursorrend = sorrendmoge + 100;
                            //                        else
                            //                        {
                            //                            int sorrendele = Convert.ToInt32(DataView[beszursorrendhez.Count].Row["SORREND"].ToString());
                            //                            Beszursorrend = sorrendmoge + (sorrendele - sorrendmoge) / 2;
                            //                        }
                            //                    }
                            //                    //    elotte = Convert.ToInt32(Tabinfo.DataView[Tabinfo.ViewSorindex - 1].Row[sorrendcolindex].ToString());
                            //                    //    Beszursorrend = elotte + (ezenall - elotte) / 2;
                            //                    //}
                            //                    //if (aktsorrend == 0 && utsosorrend == 0)
                            //                    //    Beszursorrend = elsosorrend / 2;
                            //                    DataRow dra=Tabinfo.Adattabla.Ujsor(Beszursorrend);
                            //                    dra[identa1col] = Convert.ToInt64(id1);
                            //                    dra["PREV_ID1"] = previd1; ;
                            //                    dra["VERZIO_ID1"] = osszefinfo.tabinfo1.AktVerzioId;
                            //                    dra[identa2col] = fileinfoarray[comboindex];
                            //                    dra["PREV_ID2"] = fileinfoarray[comboindex];
                            //                    dra["VERZIO_ID2"] = osszefinfo.tabinfo2.AktVerzioId;
                            //                    dra["MODOSITOTT_M"] = 1;
                            //                    Tabinfo.Modositott = modositott;
                            //                    FakUserInterface.ValtoztatasFunkcio = "ADD";
                            //                    Tabinfo.ValtozasNaplozas(dra);
                        //}
                        //for (int k = 0; k < dataView2.Count; k++)
                        //{
                        //    DataRow dr2 = dataView2[k].Row;
                        //    if (dr2[osszefinfo.ident2col].ToString().Trim() == filert)
                        //    {
                        //        id2 = dr2[osszefinfo.ident2col].ToString().Trim();
                        //        if (dataView2.Table.Columns.IndexOf("PREV_ID") != -1)
                        //            previd2 = dr2["PREV_ID"].ToString();
                        //        else
                        //            previd2 = id2;
                        //        break;
                        //    }
                        //}
               //     }
                    //               bool modositott = false;
                    //    DataRow drr = DataView[i].Row;
                    //    drr["PREV_ID2"] = previd2;
                    //    if (drr["SORSZAM1"].ToString().Trim() != id1)
                    //    {
                    //        drr["SORSZAM1"] = id1;
                    //        drr["MODOSITOTT_M"] = 1;
                    //        modositott = true;
                    //    }
                    //    if (drr["SORSZAM2"].ToString().Trim() != id2)
                    //    {
                    //        drr["SORSZAM2"] = id2;
                    //        drr["MODOSITOTT_M"] = 1;
                    //        modositott = true;
                    //    }
                    //    if (modositott)
                    //    {
                    //        FakUserInterface.ValtoztatasFunkcio = "MODIFY";
                    //        Tabinfo.ValtozasNaplozas(drr);
                    //    }
                    //}
                    //string savsort = Tabinfo.DataView.Sort;
                    //DataView.Sort = "";
                    //for (int i = 0; i < DataView.Count; i++)
                    //{
                    //    dr = DataView[i].Row;
                    //    if (dr["SORSZAM2"].ToString() == "0")
                    //    {
                    //        Tabinfo.Adatsortorol(i);
                    //        i = -1;
                    //    }
                }
                //           DataView.Sort = savsort;
                bool nemhiba = true;
                if (!Tervezoe)
                    nemhiba = Hivo.Hivo.RogzitesElott();
                if (nemhiba)
                {
                    FakUserInterface.Rogzit(new Tablainfo[] { Tabinfo });
                    if (DataView.Count == 0 && Tabinfo.KellVerzio)
                    {
                        Tabinfo.VerzioTerkepArray.RemoveAt(Tabinfo.VerzioTerkepArray.Count - 1);
                        Tabinfo.AktVerzioId = Tabinfo.LastVersionId;
                        ValtozasBeallit("Verziovaltozas");
                    }
                    RogzitesUtan();
                }
                Columntolt();
            }
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
        public override void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
        }
        /// <summary>
        /// Valasztottunk a kijelolt combocellaban, jelezzuk a valtozast, ha volt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (HozferJog == HozferJogosultsag.Irolvas)
            {
                bool mod = false;
                DataGridViewCell combocell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                string val = combocell.Value.ToString();
                if (val == "" && origtart[e.RowIndex] != "0")
                    mod = true;
                else 
                {
                    val = fileinfoarray[comboinfoarray.IndexOf(val)].ToString();
                    if (val != origtart[e.RowIndex])
                        mod = true;
                }
//                if (combocell.Value == null || combocell.Value.ToString() != origtart[e.RowIndex])
                if(mod)
                {
                    dataView1[e.RowIndex].Row["MODOSITOTT_M"] = 1;
                    Tabinfo.Changed = true;
                }
            }
        }
    }
}
