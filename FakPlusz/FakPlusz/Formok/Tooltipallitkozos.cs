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
    /// 
    /// </summary>
    public partial class Tooltipallitkozos : Gridpluszinput
    {
        /// <summary>
        /// 
        /// </summary>
        public TextBox textBox = new TextBox();
        private bool enyem = false;
        private string kieg = " karbantartása";
        private string azonszovegkieg = " módositása";
        /// <summary>
        /// 
        /// </summary>
        public Tooltipallitkozos()
        {
            InitializeComponent();
            panel5.Controls.Remove(dataGridView2);
            textBox.Multiline = true;
            textBox.AcceptsReturn = true;
            panel5.Controls.Add(textBox);
            textBox.Dock = DockStyle.Fill;
            dataGridView1.Visible = false;
        }
        /// <summary>
        /// inicalizalas felulbiralva
        /// </summary>
        public override void AltalanosInit()
        {
            bool valt = UjTag;
            if (!valt)
            {
                valt = !Tabinfo.KellVerzio && ValtozasLekerdezExcept(new string[] { "Verziovaltozas" }).Count != 0 ||
                    Tabinfo.KellVerzio && ValtozasLekerdez().Count != 0;
            }
            if(!valt)
            {
                if (Tabinfo.TablaColumns["TOOLTIP"] == null || Tabinfo.DataView.Count == 0 || Tabinfo.InputColumns.Count == 0 || !AktivDropDownItem.Enabled)
                {
                    this.Visible = false;
                    return;
                }
                else
                {
                    this.Visible = true;
                    MezoControlInfok[0].UserControlInfo = UserControlInfo;
                    Inputtablaba();
                }
            }
            else
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
                if (!Leiroe)
                {
                    Tabinfo = TablainfoTag.Tablainfo;
                    if (Tabinfo.Tablanev == "BASE")
                        Tabinfo.DataView.RowFilter = "substring(azon,1,1)='T' and substring(azon,3,1) <> 'R' and substring(azon,3,1) <> 'U' and substring(azon,3,1) <> 'C' and szint<>'' and tablanev=''";
                }
                else
                    Tabinfo = TablainfoTag.LeiroTablainfo;
                if (TablainfoTag.Azonositok.Azon == "LEIR")
                    toolStrip1.Visible = false;
                HozferJog = Base.HozferJogosultsag.Irolvas;
                szovegcol = Tabinfo.Adattabla.Columns.IndexOf(Tabinfo.SzovegColName);
                DataView = Tabinfo.DataView;
                Inputtabla = Tabinfo.Inputtabla;
                InputColumns = new ColCollection();
                if (Tabinfo.TablaColumns["TOOLTIP"] == null || Tabinfo.DataView.Count == 0 || Tabinfo.InputColumns.Count == 0 || !AktivDropDownItem.Enabled)
                {
                    this.Visible = false;
                    return;
                }
                else
                {
                    this.Visible = true;
                    InputColumns.Add(Tabinfo.TablaColumns["TOOLTIP"]);
                }
                Tabinfo.AktualControlInfo = FakUserInterface.ControlTagokTolt(this, panel2, ref Tabinfo, AktivPage, null, null);
                MezoControlInfok[0] = Tabinfo.AktualControlInfo;
                UserControlInfo = FakUserInterface.Attach(this, Vezerles, ref Tabinfo, AktivPage, AktivMenuItem, AktivDropDownItem);
                MezoControlInfok[0].UserControlInfo = UserControlInfo;
                dataGridView1.Dock = DockStyle.Fill;
                Tabinfo.Modositott = false;
                Beszurhat = false;
                sorrendcolindex = -1;
                rogzit.Enabled = false;
                enyem = TablainfoTag.Azonositok.Enyem;
                if (Leiroe)
                    enyem = TablainfoTag.Azonositok.LeiroEnyem;
                if (FakUserInterface.Enyem)
                    enyem = false;
                if (enyem)
                {
                    kieg = " áttekintése";
                    azonszovegkieg = " megtekintése";
                }
                AktivPage.Text = AktivDropDownItem.Text + kieg;
                if (Tabinfo.ViewSorindex == -1)
                    Tabinfo.ViewSorindex = 0;

                dataGridView1.Visible = false;
                dataGridView1.Visible = true;
                Inputtablaba();
                VerziobuttonokAllit();
                eleszur.Visible = false;
                mogeszur.Visible = false;
                torolalap.Visible = false;

            }
        }
        /// <summary>
        /// ures
        /// </summary>
        /// <param name="dcell"></param>
        /// <returns></returns>
        public override string Hibavizsg(DataGridViewCell dcell)
        {
            return "";
        }
        /// <summary>
        /// felso gridviewbol sorvalasztas felulbiralva
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (NemkellOk())
                {
                    Tabinfo.ViewSorindex = e.RowIndex;
                    string azonszoveg = Tabinfo.AktualViewRow[szovegcol].ToString();
                    Inputtablaba(azonszoveg);
                }
            }
        }
        /// <summary>
        /// aktualis sor inputgridviewba
        /// </summary>
        public override void Inputtablaba()
        {
            Inputtablaba(Tabinfo.AktualViewRow[szovegcol].ToString());
        }
        /// <summary>
        /// inputtabla toltes + azonositoszoveg toltes
        /// </summary>
        /// <param name="azonszoveg">
        /// az azonositoszoveg
        /// </param>
        public override void Inputtablaba(string azonszoveg)
        {
            SetSelectedRow(Tabinfo.ViewSorindex);
            SetAktRowVisible(dataGridView1,Tabinfo);
            Azonositoszoveg = azonszoveg + azonszovegkieg;
            label2.Text = Azonositoszoveg;
            if (Tabinfo.ViewSorindex == 0)
                elozo.Enabled = false;
            else
                elozo.Enabled = true;
            if (Tabinfo.ViewSorindex == DataView.Count - 1)
                kovetkezo.Enabled = false;
            else
                kovetkezo.Enabled = true;
            textBox.Text = Tabinfo.AktualViewRow["TOOLTIP"].ToString();
        }
        /// <summary>
        /// ok click felulbiralva
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void ok_Click(object sender, EventArgs e)
        {
            if (textBox.Text.Trim() != Tabinfo.AktualViewRow["TOOLTIP"].ToString())
            {
                Tabinfo.AktualViewRow["TOOLTIP"] = textBox.Text.Trim();
                Tabinfo.Modositott = true;
                FakUserInterface.ValtoztatasFunkcio = "MODIFY";
                Tabinfo.ValtozasNaplozas(Tabinfo.AktualViewRow);
                rogzit.Enabled = true;
            }
            dataGridView1.Rows[Tabinfo.ViewSorindex].Selected = false;
            if (Tabinfo.ViewSorindex == Tabinfo.DataView.Count - 1)
                Tabinfo.ViewSorindex = 0;
            else
                Tabinfo.ViewSorindex = Tabinfo.ViewSorindex + 1;
            dataGridView1.Rows[Tabinfo.ViewSorindex].Selected = true;
            SetAktRowVisible(dataGridView1,Tabinfo);
            Inputtablaba();
        }
        /// <summary>
        /// Nemkellok felulbiralva
        /// </summary>
        /// <returns></returns>
        public override bool NemkellOk()
        {
            if (Tabinfo.AktualViewRow["TOOLTIP"].ToString().Trim() == textBox.Text.Trim() || MessageBox.Show(FakUserInterface.GetUzenetSzoveg("Okfelejt"), "", MessageBox.MessageBoxButtons.IgenNem)
                 != MessageBox.DialogResult.Igen)
                return true;
            else
                return false;
        }
        /// <summary>
        /// rogzitesutan felulbiralva
        /// </summary>
        public override void RogzitesUtan()
        {
            ValtozasBeallit();
        }
    }
}
