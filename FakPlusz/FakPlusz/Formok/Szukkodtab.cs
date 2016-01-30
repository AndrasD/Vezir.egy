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
    /// Szukitett kodtablak karbantartasanak UserControl-ja
    /// </summary>
    public partial class Szukkodtab : Csakgrid
    {
        private bool igazivaltozas = true;
        private Osszefinfo osszefinfo;
        private string[] origtart;
        public DataView dataView = new DataView();
        private string checkboxert = "0";
        private int checkboxcol = -1;
        /// <summary>
        /// objectum letrehozasa
        /// </summary>
        /// <param name="vezerles">
        /// vezerloinformacio
        /// </param>
        public Szukkodtab(Vezerloinfo vezerles)
        {
            InitializeComponent();
            ParameterAtvetel(vezerles, false);
        }
        /// <summary>
        /// Inicializalas, ha nincs meg adat, ennek jelzese utan kilep
        /// </summary>
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
                if (LezartCeg && HozferJog == Base.HozferJogosultsag.Irolvas)
                    HozferJog = Base.HozferJogosultsag.Csakolvas;
                Tabinfo.HozferJog = HozferJog;
                DataView = Tabinfo.DataView;
                osszefinfo = Tabinfo.Osszefinfo;
                osszefinfo.InitKell = true;
                osszefinfo.OsszefinfoInit();
                osszefinfo.TolteniKell = true;
                if (!osszefinfo.Osszefinfotolt())
                {
                    if (DataView.Count != 0)
                    {
                        Tabinfo.TeljesTorles();
                        Rogzit();
                    }
                    MessageBox.Show(" Nincs adat a szükitett kódtáblához!");
                    AktivPage.Controls.Remove(this);
                    return;
                }
                Beszurhat = false;
                dataGridView1.ReadOnly = true;
                Tabinfo.AktualControlInfo = FakUserInterface.ControlTagokTolt(this, panel2, ref Tabinfo, AktivPage, AktivMenuItem, AktivDropDownItem);
                MezoControlInfok[0] = Tabinfo.AktualControlInfo;
                UserControlInfo = FakUserInterface.Attach(this, Vezerles, ref Tabinfo, AktivPage, AktivMenuItem, AktivDropDownItem);
                osszefinfo.AktualTag = TablainfoTag;
                osszefinfo.DataGridView1 = dataGridView1;
                dataView = osszefinfo.AktualDataView1;
                if (Tabinfo.LezartVersion || UjVerzio && !verzvaltas || HozferJog == HozferJogosultsag.Csakolvas)
                    dataGridView1.ReadOnly = true;
                else
                    dataGridView1.ReadOnly = false;
                AktivPage.Text = TablainfoTag.Azonositok.Szoveg;
                if (Tabinfo.HozferJog == HozferJogosultsag.Irolvas && (!Tabinfo.KellVerzio || Tabinfo.VerzioTerkepArray.Count != 0))
                    AktivPage.Text += " karbantartása";
                else
                    AktivPage.Text += " áttekintése";
                if (!Tervezoe)
                    Hivo.Hivo.AltalanosInit();
                Columntolt();
            }
            else
            {
                MezoControlInfok[0].UserControlInfo = UserControlInfo;
                return;
            }
        }
        /// <summary>
        /// A  DataGridView-ban megjelenitendo DataView osszeallitasa
        /// </summary>
        public void Columntolt()
        {
            if (FakUserInterface.Alkalmazas != "TERVEZO")
            {
                if (osszefinfo.alkalmid1col != -1)
                    dataView.RowFilter = "ALKALMAZAS_ID = " + FakUserInterface.AlkalmazasId;
                //if (osszefinfo.alkalmid2col != -1)
                //    dataView2.RowFilter = "ALKALM_ID = " + FakUserInterface.AlkalmazasId;
                if (osszefinfo.tabinfo1.Kodtipus == "Alkalm")
                    dataView.RowFilter = "SZOVEG='" + FakUserInterface.Alkalmazas + "'";
                //if (osszefinfo.tabinfo2.Kodtipus == "Alkalm")
                //    dataView2.RowFilter = "SZOVEG='" + FakUserInterface.Alkalmazas + "'";
            }
            rogzit.Enabled = false;
            VerziobuttonokAllit();
            Aktualtablainfo = new Tablainfo[] { Tabinfo };
            DataView = Tabinfo.DataView;
            if (DataView.Count == 0)
            {
                checkboxert = TablainfoTag.Azonositok.Defert;
                Modositott = true;
            }
            else
                checkboxert = "0";
            string azontip1 = TablainfoTag.Tablainfo.Azontip1;
            checkboxcol = osszefinfo.Adattabla1.Columns.Count - 1;
            torolalap.Visible = false;
            origtart = new string[dataView.Count];
            for (int i = 0; i < origtart.Length; i++)
                origtart[i] = checkboxert;
            for (int i = 0; i < DataView.Count; i++)
            {
                bool talalt = false;
                DataRow drr = DataView[i].Row;
                for (int j = 0; j < dataView.Count; j++)
                {
                    DataRow dr = dataView[j].Row;
                    string id1 = dr["SORSZAM"].ToString().Trim();
                    if (drr["RSORSZAM"].ToString().Trim() == id1)
                    {
                        talalt = true;
                        break;
                    }
                }
                if (!talalt)
                {
                    Tabinfo.Adattabla.Adatsortorol(i);
                    if (DataView.Count != 0)
                        i = -1;
                }
            }
            for (int i = 0; i < dataView.Count; i++)
            {
                DataRow dr = dataView[i].Row;
                dr[checkboxcol] = origtart[i];
                for (int j = 0; j < DataView.Count; j++)
                {
                    DataRow drr = DataView[j].Row;
                    string id1 = dr["SORSZAM"].ToString().Trim();
                    if (drr["RSORSZAM"].ToString().Trim() == id1)
                    {
                        origtart[i] = "1";
                        dr[checkboxcol] = 1;
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// Valtozasok rogzitese
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void rogzit_Click(object sender, EventArgs e)
        {
            igazivaltozas = false;
            DataRow drr = null;
            for (int i = 0; i < dataView.Count; i++)
            {
                DataRow dr = dataView[i].Row;
                int jj = Tabinfo.DataView.Find(dr["SORSZAM"]);
                if (jj == -1)
                {
                    if (dr[checkboxcol].ToString() == "1")
                    {
                        drr = Tabinfo.Ujsor();
                        drr["RSORSZAM"] = dr["SORSZAM"];
                        drr["PREV_ID1"] = dr["PREV_ID"];
                        drr["VERZIO_ID1"] = dr["VERZIO_ID"];
                        drr["KOD"] = dr["KOD"];
                        drr["SZOVEG"] = dr["SZOVEG"];
                        FakUserInterface.ValtoztatasFunkcio = "ADD";
                        Tabinfo.ValtozasNaplozas(drr);
                    }
                }
                else if (dr[checkboxcol].ToString() == "0")
                    Tabinfo.Adattabla.Adatsortorol(jj);
                else
                {
                    drr = Tabinfo.DataView[jj].Row;
                    string szov = drr["SZOVEG"].ToString();
                    if (szov != dr["SZOVEG"].ToString())
                    {
                        FakUserInterface.ValtoztatasFunkcio = "MODIFY";
                        drr["SZOVEG"] = dr["SZOVEG"];
                        Tabinfo.ValtozasNaplozas(drr);
                    }
                }
            }
            //    if (dr[checkboxcol].ToString() != origtart[i])
            //    {
            //        if (origtart[i] == "1")
            //        {
            //            for (int j = 0; j < Tabinfo.DataView.Count; j++)
            //            {
            //                drr = Tabinfo.DataView[j].Row;
            //                if (drr["RSORSZAM"].ToString() == dr["SORSZAM"].ToString())
            //                {
            //                    Tabinfo.Adattabla.Adatsortorol(j);
            //                    break;
            //                }
            //            }
            //        }
            //        else
            //        {
            //            drr = Tabinfo.Ujsor();
            //            drr["RSORSZAM"] = dr["SORSZAM"];
            //            drr["PREV_ID1"] = dr["PREV_ID"];
            //            drr["VERZIO_ID1"] = dr["VERZIO_ID"];
            //            drr["KOD"] = dr["KOD"];
            //            drr["SZOVEG"] = dr["SZOVEG"];
            //            FakUserInterface.ValtoztatasFunkcio = "ADD";
            //            Tabinfo.ValtozasNaplozas(drr);
            //        }
            //    }
            //}
            //if (Modositott)
            //    Tabinfo.Modositott = true;
            if (Tabinfo.Modositott)
            {
                string savsort = Tabinfo.DataView.Sort;
                Tabinfo.DataView.Sort = "";
                int sorrend = 100;
                for (int i = 0; i < Tabinfo.DataView.Count; i++)
                {
                    drr = Tabinfo.DataView[i].Row;
                    drr["SORREND"] = sorrend;
                    sorrend = sorrend + 100;
                }
                Tabinfo.DataView.Sort = savsort;
            }
            if (Tabinfo.Modositott)
            {
                if (Tervezoe || !Tervezoe && Hivo.Hivo.RogzitesElott())
                {
                    FakUserInterface.Rogzit(Aktualtablainfo);
                    Hivo.RogzitesUtan();
                }
            }
            Columntolt();
            igazivaltozas = true;
        }
        /// <summary>
        /// Mindent elolrol
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void elolrolalap_Click(object sender, EventArgs e)
        {
            igazivaltozas = false;
            FakUserInterface.ForceAdattolt(Aktualtablainfo);
            Columntolt();
            igazivaltozas = true;
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
        /// Cella editalas vege, kiertekeles, ha valtozas volt, ennek jelzese
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (igazivaltozas && HozferJog ==  HozferJogosultsag.Irolvas)
            {
                bool hiba = false;
                DataGridViewCell checkboxcell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if(checkboxcell.Value.ToString()==origtart[e.RowIndex])
                    checkboxcell.ErrorText="";
                if (checkboxcell.Value.ToString() != origtart[e.RowIndex])
                {
                    if (origtart[e.RowIndex] == "1")
                    {
                        Tabinfo.Osszefinfo.tabinfo1.ViewSorindex=e.RowIndex;
                        string nev = Tabinfo.Osszefinfo.tabinfo1.ComboFileba;
                        string ertek = Tabinfo.Osszefinfo.tabinfo1.AktualViewRow[nev].ToString();
                        hiba = ComboHasznalatban(Tabinfo, null, origtart[e.RowIndex],checkboxcell.Value.ToString(),origtart[e.RowIndex]);
                        if (hiba)
                            checkboxcell.Value = origtart[e.RowIndex];
                    }
                    if (!hiba)
                    {
                        Tabinfo.Modositott = true;
                        rogzit.Enabled = true;
                    }
                }
            }
        }
        private string Hibavizsg(DataGridViewCell dcell,DataRow dr)
        {
            string szov = "";
            dcell.ErrorText = szov;
            return szov;
        }
        /// <summary>
        /// uj verzio eloallit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void uj_Click(object sender, EventArgs e)
        {
            igazivaltozas = false;
            Tabinfo.CreateNewVersion();
            Tabinfo.Osszefinfo.NewVersionKieg();
            ValtozasBeallit("Verziovaltozas");
            AltalanosInit();
            igazivaltozas = true;
        }
        /// <summary>
        /// verzio torles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void teljestorles_Click(object sender, EventArgs e)
        {
            igazivaltozas = false;
            Tabinfo.DeleteLastVersion();
            ValtozasBeallit("Verziovaltozas");
            AltalanosInit();
            igazivaltozas = true;
        }

    }
}
