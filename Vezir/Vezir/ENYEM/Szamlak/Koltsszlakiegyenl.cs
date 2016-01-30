using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using FakPlusz.Alapfunkciok;
using FakPlusz;
using FakPlusz.Alapcontrolok;
using FakPlusz.UserAlapcontrolok;
using FakPlusz.Formok;
using FakPlusz.VezerloFormok;

namespace Vezir
{

    public partial class Koltsszlakiegyenl : Csakgrid
    {
        private VezerloControl vezhivo;
        private bool valt = true;
        private bool rogzitett = false;
        private MezoControlInfo mezcontinfo = null;
        private string[] jelenfizetve = null;

        public Koltsszlakiegyenl(FakUserInterface fak, VezerloControl hivo, Vezerloinfo vezerles)
        {
            InitializeComponent();
            vezhivo = hivo;
            vezerles.Hivo = hivo;
            ParameterAtvetel(vezerles, false);
            uj.Visible = false;
            eleszur.Visible = false;
            mogeszur.Visible = false;
            torolalap.Visible = false;
            elozoverzio.Visible = false;
            kovetkezoverzio.Visible = false;
            teljestorles.Visible = false;
            rogzit.Enabled = false;
            Tabinfo = UserParamTabla.TermCegPluszCegalattiTablainfok["KOLTSSZLA"];
            TablainfoTag = Tabinfo.TablaTag;
            mezcontinfo = new MezoControlInfo(hivo, Tabinfo);
        }
        public override void AltalanosInit()
        {
            valt = ValtozasLekerdez().Count != 0;
            if (valt || rogzitett)
            {
                Tabinfo = UserParamTabla.TermCegPluszCegalattiTablainfok["KOLTSSZLA"];
                TablainfoTag = Tabinfo.TablaTag;
                rogzitett = false;
                Tabinfo.Modositott = false;
                bool cegvalt = ValtozasLekerdez("CegValtozas").Count != 0;
                Tabinfo.AktualControlInfo = mezcontinfo;
                MezoControlInfok[0] = Tabinfo.AktualControlInfo;
                if (UserControlInfo == null)
                {
                    UserControlInfo = FakUserInterface.Attach(this, Vezerles, ref Tabinfo, AktivPage, AktivMenuItem, AktivDropDownItem);
                    MezoControlInfok[0].UserControlInfo = UserControlInfo;
                }
                ValtozasTorol();
                FakUserInterface.EventTilt = true;
                Tabinfo.Adattabla.Rows.Clear();
                FakUserInterface.EventTilt = false;
                Sqlinterface.Select(Tabinfo.Adattabla, FakUserInterface.AktualCegconn, "KOLTSSZLA", " where FIZETVE = 'N' AND MARADEK = 0 ", " order by SZLA_DATUM", false);
                ValtozasBeallit();
                if (Tabinfo.Adattabla.Rows.Count == 0)
                {
                    FakPlusz.MessageBox.Show("Minden számla ki van egyenlitve!");
                    return;
                }
                else
                {
                    Tabinfo.Tartalmaktolt();
                    if (dataGridView1.Columns.Count == 0)
                    {
                        dataGridView1.AutoGenerateColumns = false;
                        dataGridView1.SelectionMode = DataGridViewSelectionMode.CellSelect;
                        dataGridView1.ReadOnly = false;
                        dataGridView1.Columns.Add(Tabinfo.UjCheckboxcolumn(Tabinfo.TablaColumns["FIZETVE"], false));
                        dataGridView1.Columns.Add(Tabinfo.Ujtextcolumn(Tabinfo.KiegColumns["PARTNER_ID_K"], true));
                        dataGridView1.Columns.Add(Tabinfo.Ujtextcolumn(Tabinfo.TablaColumns["AZONOSITO"], true));
                        dataGridView1.Columns.Add(Tabinfo.Ujtextcolumn(Tabinfo.TablaColumns["MEGJEGYZES"], true));
                        dataGridView1.Columns.Add(Tabinfo.Ujtextcolumn(Tabinfo.TablaColumns["SZLA_DATUM"], true));
                        dataGridView1.Columns.Add(Tabinfo.Ujtextcolumn(Tabinfo.TablaColumns["OSSZKIADAS"], true));
                        dataGridView1.Columns.Add(Tabinfo.Ujtextcolumn(Tabinfo.TablaColumns["KIEGYENL_DATUM"], true));
                    }
                }
                jelenfizetve = new string[Tabinfo.DataView.Count];
                for (int i = 0; i < jelenfizetve.Length; i++)
                    jelenfizetve[i] = "N";
                dataGridView1.SelectionMode = DataGridViewSelectionMode.CellSelect;
                dataGridView1.DataSource = Tabinfo.DataView;
                dateTimePicker1.MinDate = DateTimePicker.MinimumDateTime;
                dateTimePicker1.MaxDate = DateTimePicker.MaximumDateTime;
                DateTime szamladat = Convert.ToDateTime(Tabinfo.Adattabla.Rows[0]["SZLA_DATUM"].ToString());
                dateTimePicker1.MinDate = Convert.ToDateTime(Tabinfo.Adattabla.Rows[0]["SZLA_DATUM"].ToString());
                if (DateTime.Today.CompareTo(szamladat) > 0)
                    szamladat = DateTime.Today;
                dateTimePicker1.MaxDate = szamladat;
                Datumtol = szamladat;
                dateTimePicker1.Enabled = true;
                rogzit.Enabled = false;
            }
            else if (Datumtol != dateTimePicker1.Value)
                Datumtol = dateTimePicker1.Value;
        }
        public override void VerziobuttonokAllit()
        {
            base.VerziobuttonokAllit();
            eleszur.Visible = false;
            mogeszur.Visible = false;
            if (!Tabinfo.Modositott)
                rogzit.Enabled = false;
            else
                rogzit.Enabled = true;
        }
        public override void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!FakUserInterface.EventTilt)
            {
                if (Tabinfo.DataView.Count > e.RowIndex && Hivo.AktivControl.Name == this.Name && e.ColumnIndex == 0)
                {
                    DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)dataGridView1.Rows[e.RowIndex].Cells[0];
                    string ertek = cell.Value.ToString();
                    FakUserInterface.EventTilt = true;
                    Datumtol = dateTimePicker1.Value;
                    if (ertek == "N")
                        cell.Value = "I";
                    else
                        cell.Value = "N";
                    FakUserInterface.EventTilt = false;
                    if (jelenfizetve[e.RowIndex] == "N")
                    {
                        Tabinfo.Modositott = true;
                        rogzit.Enabled = true;
                        jelenfizetve[e.RowIndex] = "I";
                        Tabinfo.DataView[e.RowIndex].Row["KIEGYENL_DATUM"] = DatumtolString;
                        Tabinfo.DataView[e.RowIndex].Row["MODOSITOTT_M"] = 1;
                        dateTimePicker1.Enabled = false;
                        dataGridView1.Refresh();
                    }
                    else
                    {
                        jelenfizetve[e.RowIndex] = "N";
                        Tabinfo.DataView[e.RowIndex].Row["KIEGYENL_DATUM"] = DBNull.Value;
                        Tabinfo.DataView[e.RowIndex].Row["MODOSITOTT_M"] = 0;
                        bool v = false;
                        for (int i = 0; i < jelenfizetve.Length; i++)
                        {
                            if (jelenfizetve[i] == "I")
                            {
                                v = true;
                                break;
                            }
                        }
                        Tabinfo.Modositott = v;
                        rogzit.Enabled = v;
                        dateTimePicker1.Enabled = !v;
                    }
                }
            }
        }
        public override void rogzit_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Enabled = true;
            Datumtol = dateTimePicker1.Value;
            string sel = " UPDATE KOLTSSZLA set FIZETVE = 'I', KIEGYENL_DATUM = '" + DatumtolString + "'";
            string felt = " where ";
            bool elso = true;
            for (int i = 0; i < Tabinfo.Adattabla.Rows.Count; i++)
            {
                DataRow dr = Tabinfo.Adattabla.Rows[i];
                if (dr["FIZETVE"].ToString() == "I")
                {
                    dr["KIEGYENL_DATUM"] = Datumtol;
                    dr["MODOSITOTT_M"] = 1;
                    if (!elso)
                        felt += " OR ";
                    else
                        elso = false;
                    felt += "KOLTSSZLA_ID =" + dr["KOLTSSZLA_ID"].ToString();
                }
            }
            if (!elso)
            {
                DataTable dt = new DataTable("KOLTSSZLA");
                Sqlinterface.SpecCommand(dt, FakUserInterface.AktualCegconn, "KOLTSSZLA", sel + felt, "");
                Sqlinterface.Select(dt, FakUserInterface.AktualCegconn, "KOLTSSZLA", " where FIZETVE = 'N' AND MARADEK = 0 ", " order by SZLA_DATUM", true);
                if (dt.Rows.Count == 0)
                    FakPlusz.MessageBox.Show("Minden számla ki van egyenlitve!");
                //Hivo.RogzitesUtan();
                Hivo.RogzitesUtan();
                if (this.Visible)
                {
                    rogzit.Enabled = false;
                    AltalanosInit();
                }
            }
        }
        public override void elolrolalap_Click(object sender, EventArgs e)
        {
            rogzitett = true;
            AltalanosInit();
        }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            Datumtol = dateTimePicker1.Value;
        }
    }
}
