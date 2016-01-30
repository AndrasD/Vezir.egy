using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FakPlusz;
using FakPlusz.Alapfunkciok;
using FakPlusz.Formok;
using FakPlusz.SzerkesztettListak;

namespace FakPlusz.Alapfunkciok
{
    public partial class KezdetiAblak : Form
    {
        public DataView view = new DataView();
        private FakUserInterface Fak;
        private AdatTabla basetabla = new AdatTabla("BASE");
        public bool Open = false;
        public KezdetiAblak()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
        }
        public void OpenKezdeti(FakUserInterface fak, string[] szovegek,string[] azonok)
        {
            Open = true;
            Fak = fak;
            Sqlinterface.Select((DataTable)basetabla, fak.Rendszerconn, "BASE", "", "order by PARENT,SORREND", false);
            long maxparent = Convert.ToInt64(basetabla.Rows[basetabla.Rows.Count - 1]["PARENT"].ToString());
            if (szovegek != null)
            {
                maxparent++;
                long maxsorsz = 0;
                for (int i = 0; i < szovegek.Length; i++)
                {
                    maxsorsz++;
                    DataRow newrow = basetabla.NewRow();
                    newrow["SORREND"] = maxsorsz;
                    newrow["SZOVEG"] = szovegek[i];
                    newrow["AZON"] = azonok[i];
                    newrow["PARENT"] = maxparent;
                    basetabla.Rows.Add(newrow);
                }
            }
            view.Table = basetabla;
            dataGridView1.DataSource = view;
            DataGridViewTextBoxColumn tcol = (DataGridViewTextBoxColumn)dataGridView1.Columns[0];
            tcol.DataPropertyName = "SZOVEG";
            this.Visible = true;
            dataGridView1.Visible = true;
            Fak.MainForm.AddOwnedForm(this);
            this.Refresh();
        }
        public void Sorkesz(string szoveg,string azon)
        {
            if (Open)
            {
                for (int i = 0; i < view.Count; i++)
                {
                    if (view[i].Row["SZOVEG"].ToString() == szoveg && view[i].Row["AZON"].ToString() == azon)
                    {
                        DataGridViewImageCell cell = (DataGridViewImageCell)dataGridView1.Rows[i].Cells[1];
                        cell.Value = global::FakPlusz.Properties.Resources.accept;
                        Fak.SetAktRowVisible(dataGridView1, i);
                        this.Refresh();
                        break;
                    }
                }
            }
        }
        public void ShowProgress()
        {
            progressBar1.Visible = true;
            this.Refresh();
        }
        public void RefreshForm()
        {
            if (Open && progressBar1.Visible)
                progressBar1.Refresh();
        }
        public void CloseKezdeti()
        {
            if (Open)
            {
                Open = false;
                Fak.MainForm.RemoveOwnedForm(this);
                this.Visible = false;
            }
        }

    }
}
