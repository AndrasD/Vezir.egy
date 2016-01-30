using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Data;
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
    public partial class UjPartnerUserControl : ControlAlap
    {
        private Form Parentform;
        public bool Vevokotelezo = false;
        public bool SzallitoKotelezo = false;
        private Tablainfo bevsemainfo;
        private Tablainfo termcsopinfo;
        private Tablainfo koltssemainfo;
        private Tablainfo koltscsopinfo;
        private Tablainfo felosztinfo;
        private DataTable table3 = new DataTable("KOLTSFELOSZT");
        private DataTable table4 = new DataTable("TERMFELOSZT");
        private DataView view3 = new DataView();
        private DataView view4 = new DataView();
        private VezerloControl VezerloControl;
        public UjPartnerUserControl(FakUserInterface fak,Base hivo,Vezerloinfo vezerles)
        {
            InitializeComponent();
            ParameterAtvetel(fak, hivo, vezerles);
            AlapinfoInit(new object[] { new object[] { Alapinfotipus.Alap, new Panel[] { panel1, panel2} } });
            //            dataGridView1.Parent.Parent.Visible = false;
            //toolStrip2.Items.Remove(elozo);
            //toolStrip2.Items.Remove(kovetkezo);
            teljeselolrol.Text = "Vissza";
            teljesrogzit.Text = "Rögzit";
            //elolrol.Image = global::Vezir.Properties.Resources.töröl;
            //elolrol.ImageTransparentColor = System.Drawing.Color.Black;
            dataGridView3.AutoGenerateColumns = false;
            dataGridView4.AutoGenerateColumns = false;
        }
        public void Init(UjPartner parent, Vezerloinfo vezerles, Tablainfo partner,Tablainfo vezirpartner)
        {
            //panel1.Visible = false;
            //label2.Text = "Új partner felvétele";
            //panel3.Height = this.Height - panel1.Height;
            dataGridView3.Width = this.Width / 2;
            dataGridView4.Width = this.Width - dataGridView3.Width;
            megnev1.Width = dataGridView3.Width - 100;
            megnev2.Width = dataGridView4.Width - 100;
            dataGridView3.Visible = false;
            dataGridView4.Visible = false;
            Parentform = parent;
//            ParameterAtvetel(vezerles, false);
            Hivo = Vezerles.AktivControl;
            VezerloControl = (VezerloControl)Hivo.Hivo;
            Hivo.AktivControl = this;
            Vezerles = vezerles;
            Tabinfo = partner;
            TablainfoTag = partner.TablaTag;
            Elsoeset = true;
            bevsemainfo = FakUserInterface.GetCsoport("C", "Termfeloszt");
            koltssemainfo = FakUserInterface.GetCsoport("C", "Feloszt");
            termcsopinfo = FakUserInterface.GetKodtab("C", "Termcsop");
            koltscsopinfo = FakUserInterface.GetBySzintPluszTablanev("C", "KOLTSEGCSOPORT");
            felosztinfo = FakUserInterface.GetKodtab("C", "Fszazal");
        }
        public override void Ujcontroloktolt()
        {
            base.Ujcontroloktolt();
        }
        //public override void AltalanosInit()
        //{
        //    UjTag = true;
        //    if (Elsoeset)
        //    {
        //        base.AltalanosInit();
        //        Hivo.Hivo.AltalanosInit();
        //        Tabinfo.ViewSorindex = Tabinfo.DataView.Count - 1;
        //        mogeszur_Click(mogeszur, new EventArgs());
        //        VezerloControl.lehetvevo = Hivo.Name == "Bevszla";
        //        VezerloControl.lehetszallito = Hivo.Name == "Koltsszla";
        //        Elsoeset = false;
        //    }

        //    //bevsemainfo.Osszefinfo.InitKell = true;
        //    //bevsemainfo.Osszefinfo.OsszefinfoInit();
        //    //koltssemainfo.Osszefinfo.InitKell = true;
        //    //koltssemainfo.Osszefinfo.OsszefinfoInit();
        //    label2.Text = "Új partner felvétele";

        //}
        //public override void ok_Click(object sender, EventArgs e)
        //{
        //    base.ok_Click(sender, e);
        //    if (ok.ToolTipText == "")
        //    {
        //        base.rogzit_Click(sender, e);
        //        Hivo.Hivo.RogzitesUtan();
        //        Parentform.DialogResult = DialogResult.OK;
        //        Parentform.Close();
        //    }
        //}
        //public override void elolrol_Click(object sender, EventArgs e)
        //{
        //    Parentform.DialogResult = DialogResult.Cancel;
        //    Parentform.Close();
        //}
        //public override string Hibavizsg(DataGridViewCell dcell)
        //{
        //    if(
        //    return base.Hibavizsg(dcell);
        //}
        public override string EgyediHibavizsg(DataGridViewCell dcell, Tablainfo tabinfo)
        {
            int sorindex = dcell.RowIndex;
            string colname = tabinfo.InputColumns[sorindex].ColumnName;
            string ertek = dcell.Value.ToString();
            string hibaszov = "";
            hibaszov = Hivo.Hivo.EgyediHibavizsg(dcell, tabinfo);
            if (hibaszov != "")
                return hibaszov;
            else
            {
                switch (colname)
                {
                    case "KOLTSPARTNER":
                        if (SzallitoKotelezo && ertek != "Igen")
                            return " Kötelezö az Igen!";
                        break;
                    case "BEVPARTNER":
                        if(Vevokotelezo && ertek!="Igen")
                            return " Kötelezö az Igen!";
                        break;
                }
            }
            Tablainfo semainfo = null;
            DataView semaview = null;
            Tablainfo csopinfo = null;
            DataGridView gridview = null;
            DataTable table = null;
            DataView view = null;
            if (colname == "TERMSEMA_ID" )
            {
                if (ertek == "")
                    dataGridView3.Visible = false;
                else
                {
                    semainfo = bevsemainfo;
                    semaview=bevsemainfo.DataView;
                    csopinfo = termcsopinfo;
                    gridview = dataGridView3;
                    table = table3;
                    view = view3;
                }
            }
            if (colname == "SEMA_ID")
            {
                if (ertek == "")
                    dataGridView4.Visible = false;
                else
                {
                    semainfo = koltssemainfo;
                    semaview = koltssemainfo.DataView;
                    csopinfo = koltscsopinfo;
                    gridview = dataGridView4;
                    table = table4;
                    view = view4;
                }
            }
            if (gridview != null)
            {
                gridview.Rows.Clear();
                ArrayList ar = new ArrayList();
                string csopszov = tabinfo.InputColumns[colname].ComboAktSzoveg;
                char[] vesszo = new char[] { Convert.ToChar("/") };
                string[] split;

                for (int i = 0; i < semaview.Count; i++)
                {
                    string szov = semaview[i].Row["SZOVEG"].ToString();
                    if (szov.StartsWith(csopszov))
                    {
                        split = szov.Split(vesszo);
                        string[] sor = new string[] { split[0] + "/" + split[1], split[2] };
                        gridview.Rows.Add(sor);
                    }
                }
                gridview.Visible = true;
//                if (gridview.Rows.Count != 0)
//                    gridview.Visible = true;
            }
            return "";

        }
        public override void teljeselolrol_Click(object sender, EventArgs e)
        {
            Parentform.DialogResult = DialogResult.Cancel;
            Parentform.Close();
        }
        public override void teljesrogzit_Click(object sender, EventArgs e)
        {
            base.teljesrogzit_Click(sender, e);
        }
    }
}
