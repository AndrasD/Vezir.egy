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
    /// Ceg alatti szintek letrehozasa, karbantartasa
    /// </summary>
    public partial class BaseKarb : Gridpluszinput
    {
        private int elesorrend = 0;
        private int mogesorrend = 0;
        private string termfilter = "substring(azon,1,1)='T' and len(azon) > 1 and tablanev =''";
        private string alapfilter = "substring(azon,1,1)='T' and len(azon) > 1 and substring(azon,3,1) <> 'C'and tablanev=''";
        private bool valtozas = false;
        /// <summary>
        /// objectum letrehozasa
        /// </summary>
        /// <param name="vezerles">
        /// vezerloinformacio
        /// </param>
        public BaseKarb(Vezerloinfo vezerles)
        {
            InitializeComponent();
            ParameterAtvetel(vezerles, false);
        }
        /// <summary>
        /// inicializalas kiegeszitese
        /// </summary>
        public override void AltalanosInit()
        {
            valtozas = UjTag || ValtozasLekerdez().Count != 0;
            base.AltalanosInit();
            if (valtozas)
            {
                DataView.RowFilter = alapfilter;
                int rowindex = -1;
                if (DataView.Count != 0)
                    rowindex = 0;
                Beszurashozelokeszit(rowindex);
                Tabinfo.ViewSorindex = Tabinfo.DataView.Count - 1;
                Inputtablaba();
            }
            else
                Tabinfo.ViewSorindex = Tabinfo.ViewSorindex;
        }
        private void Beszurashozelokeszit(int rowindex)
        {
            int minsorrend = 0;
            int maxsorrend = 0;
            int aktsorrend = 0;
            int kovsorrend = 0;
            int elozosorrend = 0;
            elesorrend = 0;
            DataView.RowFilter = termfilter;
            minsorrend = Convert.ToInt32(DataView[0].Row["SORREND"].ToString());
            maxsorrend = Convert.ToInt32(DataView[DataView.Count - 1].Row["SORREND"].ToString());
            if (rowindex == -1)
            {
                elesorrend = minsorrend + 2000;
                mogesorrend = elesorrend;
            }
            DataView.RowFilter = alapfilter;
            if (elesorrend == 0)
            {
                aktsorrend = Convert.ToInt32(DataView[rowindex].Row["SORREND"].ToString());
                if (DataView.Count == 1)
                {
                    elozosorrend = minsorrend;
                    kovsorrend = maxsorrend;
                }
                else
                {
                    if (rowindex == 0)
                    {
                        elozosorrend = minsorrend;
                        kovsorrend = Convert.ToInt32(DataView[rowindex + 1].Row["SORREND"].ToString());
                    }
                    else if (rowindex == DataView.Count - 1)
                    {
                        elozosorrend = Convert.ToInt32(DataView[rowindex - 1].Row["SORREND"].ToString());
                        kovsorrend = maxsorrend;
                    }
                    else
                    {
                        elozosorrend = Convert.ToInt32(DataView[rowindex - 1].Row["SORREND"].ToString());
                        kovsorrend = Convert.ToInt32(DataView[rowindex + 1].Row["SORREND"].ToString());
                    }
                }
                elesorrend = (aktsorrend + elozosorrend) / 2;
                mogesorrend = (aktsorrend + kovsorrend) / 2;
            }
        }
        public override void dataGridView2_Enter(object sender, EventArgs e)
        {
            Tempcellini();
        }
        /// <summary>
        /// ok button event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void ok_Click(object sender, EventArgs e)
        {
            bool valt = false;
            bool volthiba = false;
            ok.ToolTipText = "";
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
                    volthiba = true;
                }
            }
            Tabinfo.ModositasiHiba = volthiba;

            if (!volthiba)
            {
                if (valt || Tabinfo.Modositott)
                    rogzit.Enabled = true;
                if (valt)
                {
                    if (!Beszur)
                        Beszursorrend = 0;
                    if (!Beszur && valtozott[0])
                        MessageBox.Show("Nem változtatható mezö, csak törölni lehet!");
                    else
                    {
                        DataRow aktualadatrow = Tabinfo.AdatsortoltInputtablabol(Tabinfo.ViewSorindex, Beszur, Beszursorrend);
                        Modositott = true;
                        if (!Tabinfo.Modositott)
                            Tabinfo.Modositott = true;
                        if (Tabinfo.ViewSorindex != DataView.Count - 1)
                            Tabinfo.ViewSorindex = Tabinfo.ViewSorindex + 1;
                        else
                            Tabinfo.ViewSorindex = 0;
                        Azonositoszoveg = Tabinfo.AktualViewRow[szovegcol].ToString() + " módositása ";
                        SetSelectedRow(Tabinfo.ViewSorindex);
                        SetAktRowVisible(dataGridView1,Tabinfo);
                        if (Tabinfo.Tablanev.Contains("VERSION"))
                        {
                            VerziobuttonokAllit();
                            if (rogzit.Enabled)
                                rogzit.Visible = true;
                        }
                    }
                    Inputtablaba();
                }
            }
        }
        /// <summary>
        /// selectalt sor ele szur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void eleszur_Click(object sender, EventArgs e)
        {

            if (NemkellOk())
            {
                DataRow dr=Tabinfo.AktualViewRow;
                Beszurashozelokeszit(Tabinfo.ViewSorindex);
                Beszursorrend = elesorrend;
                Azonositoszoveg = "Uj sor " + dr[szovegcol].ToString() + " elé";
                //int arindex = szintsorrendek.IndexOf(ezenall);
                //if (arindex == 0)
                //{
                //}
                //else
                //{
                //}
                //int elotte = -1;
                //if (Tabinfo.ViewSorindex > 0)
                //{
                //    elotte = Convert.ToInt32(Tabinfo.DataView[Tabinfo.ViewSorindex - 1].Row[sorrendcolindex].ToString());
                //    Beszursorrend = elotte + (ezenall - elotte) / 2;
                //}
                Tabinfo.ViewSorindex = -1;
                Inputtablaba(Azonositoszoveg);
            }
        }
        /// <summary>
        /// selectalt sor moge szur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void mogeszur_Click(object sender, EventArgs e)
        {
            if (NemkellOk())
            {
                DataRow dr = Tabinfo.AktualViewRow;
                Beszurashozelokeszit(Tabinfo.ViewSorindex);
                Beszursorrend = mogesorrend;
                Azonositoszoveg = "Uj sor " + dr[szovegcol].ToString() + " mögé";
                //ezenall = Convert.ToInt32(Tabinfo.AktualViewRow[sorrendcolindex].ToString());
                //if (Tabinfo.ViewSorindex == DataView.Count - 1)
                //    Beszursorrend = ezenall + 2000;
                //else
                //{
                //    mogotte = Convert.ToInt32(DataView[Tabinfo.ViewSorindex + 1].Row[sorrendcolindex].ToString());
                //    mogotte = (ezenall + mogotte) / 2;
                //    Beszursorrend = (ezenall + mogotte) / 2;
                //}
                Tabinfo.ViewSorindex = -1;
                Inputtablaba(Azonositoszoveg);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public override void Rogzit()
        {
            base.Rogzit();
            FakUserInterface.Combokupdate(TablainfoTag);
        }
    }
}
