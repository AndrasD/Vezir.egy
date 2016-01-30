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
    /// Termeszetes tablak mezoneveinek attekintesere szolgalo UserControl
    /// </summary>
    public partial class Mezonevek : Csakgrid
    {
        private bool valtnaplo = false;
        private bool userlog = false;
        /// <summary>
        /// objectum letrehozasa
        /// </summary>
        /// <param name="vezerles">
        /// vezerloinformacio
        /// </param>
        public Mezonevek(Vezerloinfo vezerles)
        {
            InitializeComponent();
            ParameterAtvetel(vezerles, false);
        }
        /// <summary>
        /// 
        /// </summary>
        public override void AltalanosInit()
        {
            base.AltalanosInit();
            string LastSel = "";
            valtnaplo = false;
            userlog = false;
            if (Tabinfo.Tablanev == "USERLOG")
            {
                userlog = true;
                LastSel = Tabinfo.SelectString;
            }
            else if (Tabinfo.Tablanev == "VALTOZASNAPLO")
            {
                valtnaplo = true;
                LastSel = " where ( KEZELO_ID IS NULL OR KEZELO_ID = " + FakUserInterface.KezeloId.ToString()+")";
                if(!Tervezoe)
                    LastSel += " and (ALKALM = '' OR ALKALM = '" + FakUserInterface.Alkalmazas + "')";
            }
            if (valtnaplo)
            {
                foreach (DataGridViewTextBoxColumn col in dataGridView1.Columns)
                {
                    col.Width = col.HeaderText.Length * 9;
                    col.MinimumWidth = 5;
                }
            }
            if (valtnaplo || userlog)
            {
                Tabinfo.Adattabla.LastSel = LastSel;
                FakUserInterface.ForceAdattolt(Tabinfo, true);
                if (Tabinfo.DataView.Count == 0 || Tabinfo.HozferJog != HozferJogosultsag.Irolvas)
                    toolStrip1.Visible = false;
                else
                    toolStrip1.Visible = true;
                this.Visible = true;
                AktivPage.Text += " " + Tabinfo.DataView.Count.ToString() + " sor";
            }
        }
        /// <summary>
        /// torol felulbiralasa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void torolalap_Click(object sender, EventArgs e)
        {
            if (Tabinfo.Adattabla.Rows.Count!=0 && MessageBox.Show(FakUserInterface.GetUzenetSzoveg("Osszestorolheto"),"", MessageBox.MessageBoxButtons.IgenNem) ==
                MessageBox.DialogResult.Igen)
            {
                string selstring = "";
                if (valtnaplo || userlog)
                {
                    string procnev ="";
                    string conn = Tabinfo.Adattabla.Connection;
                    string tablanev = Tabinfo.Tablanev;
                    if (valtnaplo)
                    {

                        selstring = "delete from Valtozasnaplo";
                        if (!Tervezoe)
                            selstring += " where ALKALM = '' OR ALKALM = '" + FakUserInterface.Alkalmazas + "'";
                        procnev="VALTOZASNAPLO_PROC";
                    }
                    if (userlog)
                    {
                        selstring = "delete from Userlog";
                        if (!Tervezoe)
                            selstring += " where ALKALMAZAS_ID = " + FakUserInterface.AlkalmazasId;
                        procnev="USERLOG_PROC";
                    }

                    Sqlinterface.SpecCommand(new DataTable(), conn, tablanev, selstring, "");
                    DataTable dt = new DataTable(tablanev);
                    dt = Sqlinterface.Select(dt, conn,tablanev, "", "", true);
                    if (dt.Rows.Count == 0)
                    {

                        Sqlinterface.StoredProcedureCommand(dt, conn, procnev);
                    }
                    toolStrip1.Visible = false;
                    Tabinfo.Adattabla.Select();
                    UjTag = true;
                    AltalanosInit();
                }
            }
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
        public override void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
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
    }
}
