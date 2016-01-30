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
//using MainAlap;

namespace FakPlusz.Formok
{
    /// <summary>
    /// Forditott csoportmeghatarozasok attekintesenek UserControl-ja
    /// </summary>
    public partial class Attekint : Gridpluszinput
    {
        private int identacol = -1;
        private int identa1col = -1;
        private int identa2col = -1;
        private int inputszovcol;
        private DataView dataview1;
        private DataView dataview2;
        private Osszefinfo osszefinfo;
        private int selectedrowindex = -1;
        /// <summary>
        /// objectum letrehozasa
        /// </summary>
        /// <param name="vezerles">
        /// vezerloinformacio
        /// </param>
        public Attekint(Vezerloinfo vezerles)
        {
            InitializeComponent();
            ParameterAtvetel(vezerles, false);
            HozferJog = Base.HozferJogosultsag.Csakolvas;
        }

        /// <summary>
        /// Inicializalas, ha meg nincs adat, ennek jelzese utan kilep
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
            }
            //else
            {
                UjTag = false;
                selectedrowindex = -1;
                ValtozasTorol();
                Tabinfo = TablainfoTag.Tablainfo;
                AktivPage.Text = AktivDropDownItem.Text + " áttekintése";
                dataGridView1.ReadOnly = true;
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
                    return;
                }
                dataGridView1.ReadOnly = true;
                osszefinfo.AktualTag = TablainfoTag;
                osszefinfo.DataGridView1 = dataGridView1;
                dataview1 = osszefinfo.AktualDataView1;
                DataView = Tabinfo.DataView;
                Inputtabla = osszefinfo.Inputtabla;
                osszefinfo.DataGridView2 = dataGridView2;
                dataview2 = osszefinfo.AktualDataView2;
                inputszovcol = dataview2[0].Row.Table.Columns.IndexOf("SZOVEG");
                Beszurhat = false;
                Modosithat = false;
                Tabinfo.Modosithat = false;
                VerziobuttonokAllit();
                Aktualtablainfo = new Tablainfo[] { Tabinfo };
                identacol = Tabinfo.IdentityColumnIndex;
                identa1col = Tabinfo.Adattabla.Columns.IndexOf("SORSZAM1");
                identa2col = Tabinfo.Sorszam2col;
                Columntolt();
            }
        }
        /// <summary>
        /// A felso resz DataGridView-ban megjelenitendo DataView osszeallitasa
        /// </summary>
        private void Columntolt()
        {
            for (int i = 0; i < DataView.Count; i++)
            {
                bool talalt = false;
                DataRow drr = DataView[i].Row;
                for (int j = 0; j < dataview1.Count; j++)
                {
                    DataRow dr = dataview1[j].Row;
                    string id1 = dr[osszefinfo.ident2col].ToString().Trim();
                    if (drr[identa2col].ToString().Trim() == id1)
                    {
                        talalt = true;
                        break;
                    }
                }
                if (!talalt)
                {
                    drr.Delete();
                    i--;
                }
            }
            Inputtablatolt(dataview1[0]);
        }
        /// <summary>
        /// A felso DataGridView adott soranak megjelenitese az also DataGridView-ban
        /// </summary>
        /// <param name="dr">
        /// a kivant sor
        /// </param>
        private void Inputtablatolt(DataRowView dr)
        {
            if (dataview1.Count != 0)
            {
                if (selectedrowindex == -1)
                {
                    selectedrowindex = 0;
                    dataGridView1.Rows[0].Selected = true;
                }
                else
                {
                    dataGridView1.Rows[0].Selected = false;
                    dataGridView1.Rows[selectedrowindex].Selected = true;
                }
                if (selectedrowindex == 0)
                {
                    elozo.Enabled = false;
                    elolrol.Enabled = false;
                }
                else
                {
                    elozo.Enabled = true;
                    elolrol.Enabled = true;
                }
                if (selectedrowindex == dataview1.Count - 1)
                    kovetkezo.Enabled = false;
                else
                    kovetkezo.Enabled = true;
                Inputtabla.Clear();
                label2.Text = dr[osszefinfo.szoveg2col].ToString().Trim() + " megtekintése";
                DataRow row;
                DataRow NewRow;
                DataRow drr;
                string id1 = "";
                string id2 = "";
                id1 = dr[osszefinfo.ident1col].ToString().Trim();
                for (int j = 0; j < Tabinfo.DataView.Count; j++)
                {
                    string hasona1;
                    string hasona2;
                    drr = Tabinfo.DataView[j].Row;
                    hasona1 = drr[identa2col].ToString().Trim();
                    hasona2 = drr[identa1col].ToString().Trim();
                    //                   }
                    if (hasona1 == id1)
                    {
                        for (int i = 0; i < dataview2.Count; i++)
                        {
                            row = dataview2[i].Row;
                            id2 = row[osszefinfo.ident2col].ToString().Trim();
                            if (hasona2 == id2)
                            {
                                NewRow = Inputtabla.NewRow();
                                NewRow[0] = row[osszefinfo.szoveg1col].ToString();
                                Inputtabla.Rows.Add(NewRow);
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// A felso DataGridView ujabb soranak kivalasztasa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex != -1 && e.RowIndex != selectedrowindex)
            {
                selectedrowindex = e.RowIndex;
                Inputtablatolt(dataview1[e.RowIndex]);
            }
        }
        /// <summary>
        /// elozo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void elozo_Click(object sender, EventArgs e)
        {
            selectedrowindex--;
            Inputtablatolt(dataview1[selectedrowindex]);
        }
        /// <summary>
        /// kovetkezo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void kovetkezo_Click(object sender, EventArgs e)
        {
            selectedrowindex++;
            Inputtablatolt(dataview1[selectedrowindex]);
        }
        /// <summary>
        /// elolrol
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void elolrol_Click(object sender, EventArgs e)
        {
            selectedrowindex = 0;
            Inputtablatolt(dataview1[selectedrowindex]);
        }
    }
}
