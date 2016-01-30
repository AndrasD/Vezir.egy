using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Data;
using System.Text;
using System.IO;
using System.Windows.Forms;
using FakPlusz.Alapfunkciok;
using FakPlusz.Formok;

namespace FakPlusz.Alapcontrolok
{
    /// <summary>
    /// Parameterezesi lehetoseghez
    /// </summary>

    public partial class Parameterez : Base
    {
        /// <summary>
        /// 
        /// </summary>
        public Label TolLabel = null;
        /// <summary>
        /// 
        /// </summary>
        public Label IgLabel = null;
        /// <summary>
        /// 
        /// </summary>
        public TabPage ListaAdatbevPage = null;
        /// <summary>
        /// 
        /// </summary>
        public ToolStripButton[] AlapertButtonok = null;
        /// <summary>
        /// 
        /// </summary>
        public ToolStripButton[] MindButtonok = null;
        /// <summary>
        /// 
        /// </summary>
        public ToolStripButton[] EgysemButtonok = null;
        /// <summary>
        /// 
        /// </summary>
        public ToolStripButton[] OkButtonok = null;
        /// <summary>
        /// 
        /// </summary>
        public ToolStripButton[] ListaButtonok = null;
        /// <summary>
        /// 
        /// </summary>
        public bool[] OkVolt = null;
        /// <summary>
        /// 
        /// </summary>
        public DateTimePicker TolPicker = null;
        /// <summary>
        /// 
        /// </summary>
        public DateTimePicker IgPicker = null;
        private int aktindex = 0;
        private bool[] origok;
        private DataGridView gridview;
        /// <summary>
        /// objectum letrehozasa
        /// </summary>
        public Parameterez()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        public override void ValasztekParameterekInit()
        {
            ValasztekValtozas = true;
            AktivControl.ValasztekParameterekInit();
            ValasztekInfo = AktivControl.ValasztekInfo;
            ValasztekTablaNev = AktivControl.ValasztekTablaNev;
            ValasztekIdNev = AktivControl.ValasztekIdNev;
            ValasztekIdk = AktivControl.ValasztekIdk;
            ValasztekItemek = AktivControl.ValasztekItemek;
            ValasztekIndex = AktivControl.ValasztekIndex;
            ValasztekTabla = new DataTable(ValasztekTablaNev);
            Valasztek.Items.Clear();
            string pid = ValasztekIdk[ValasztekIndex];
            string selectstring = " where " + ValasztekIdNev + "= " + pid;
            Sqlinterface.Select(ValasztekTabla, FakUserInterface.AktualCegconn, ValasztekTablaNev, selectstring, "", false);
            Valasztek.Items.AddRange(ValasztekItemek);
            Valasztek.SelectedIndex = ValasztekIndex;
            SzurtIdkAllitasa();
            AktivControl.ValasztekTabla = ValasztekTabla;
            AktivControl.ValasztekIndex = ValasztekIndex;
            AktivControl.SzurtIdk = SzurtIdk;
            if (!Elsoeset)
            {
                FakUserInterface.EventTilt = true;
                tabControl1.Controls.Remove(ListaAdatbevPage);
                FakUserInterface.EventTilt = false;
            }
            Valasztek.SelectionLength = 0;
            datumparamdarab.Text = SzurtIdk.Count.ToString();
            ValtozasokAtadasa();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramok">
        /// </param>
        /// <returns>
        /// </returns>
        public ArrayList EgyszeruInit(string[] paramok)
        {
            EgyszeruParamValtozas = true;
            AktivControl.EgyediEgyszeruInit();
            EgyszeruTabla = AktivControl.EgyszeruTabla;
            EgyszeruTablaView = AktivControl.EgyszeruTablaView;
            gridview = egyszeruparamgridview;
            EgyszeruTablaView.Table = EgyszeruTabla;
            EgyszeruIdNev = AktivControl.EgyszeruIdNev;
            EgyszeruMegnevColumnNev = AktivControl.EgyszeruMegnevColumnNev;
            EgyszeruOrigertekek = new bool[EgyszeruTablaView.Count];
            if (VanDatum)
                aktindex = 1;
            if (paramok == null)
            {
                for (int i = 0; i < EgyszeruOrigertekek.Length; i++)
                    EgyszeruOrigertekek[i] = true;
                AktivControl.EgyszeruOrigertekek = EgyszeruOrigertekek;
                SzurtIdkAllitasa();
            }
            else
            {
                SzurtIdk = new ArrayList(paramok);
                for (int i = 0; i < EgyszeruTablaView.Count; i++)
                {
                    string id = EgyszeruTablaView[i].Row[EgyszeruIdNev].ToString();
                    EgyszeruOrigertekek[i] = SzurtIdk.IndexOf(id) != -1;
                }
            }
            origok = EgyszeruOrigertekek;
            AktivControl.SzurtIdk = SzurtIdk;
            EgyszeruParamValtozas = true;
            ParamAllitasKozos((string[])SzurtIdk.ToArray(typeof(string)), egyszeruparamgridview, EgyszeruTablaView, EgyszeruIdNev, EgyszeruMegnevColumnNev);
            if(!Elsoeset)
                tabControl1.Controls.Remove(ListaAdatbevPage);
            ValtozasokAtadasa();
            egyszerudarab.Text = SzurtIdk.Count.ToString();
            return SzurtIdk;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramok">
        /// </param>
        /// <param name="buttonindex">
        /// </param>
        public void OsszetettInit(string[] paramok, int buttonindex)
        {
            OsszetettParamValtozas = true;
            AktivControl.EgyediOsszetettInit();
            AktualisRadiobuttonIndex = buttonindex;
            OsszetettKozepsoTablaView = RadiobuttonViewk[buttonindex];
            gridview = osszetettparamkozepsogridview;
            OsszetettKozepsoTabla = OsszetettKozepsoTablaView.Table;
            OsszetettKozepsoIdNev = RadiobuttonIdNevek[buttonindex];
            OsszetettKozepsoMegnevColumnNev = RadiobuttonMegnevColumnNevek[buttonindex];
            SzuroTablaView = SzuroTablaViewk[buttonindex];
            SzuroTablaIdNev = SzuroTablaIdNevek[buttonindex];
            OsszetettOrigertekek = new bool[OsszetettKozepsoTablaView.Count];
            if (paramok == null)
            {
                for (int i = 0; i < OsszetettOrigertekek.Length; i++)
                    OsszetettOrigertekek[i] = true;
            }
            else
            {
                ArrayList ar = new ArrayList(paramok);
                for (int i = 0; i < OsszetettKozepsoTablaView.Count; i++)
                {
                    string id = OsszetettKozepsoTablaView[i].Row[OsszetettKozepsoIdNev].ToString();
                    OsszetettOrigertekek[i] = ar.IndexOf(id) != -1;
                }
            }
            AktivControl.OsszetettOrigertekek =OsszetettOrigertekek;
            origok = OsszetettOrigertekek;
            OsszetettKozepsoParamAlapertAllitas();
            ParamAllitasKozos(paramok, osszetettparamkozepsogridview, OsszetettKozepsoTablaView, RadiobuttonIdNevek[buttonindex], RadiobuttonMegnevColumnNevek[buttonindex]); 
            OsszetettParamAlsoAllitas((string[])SzurtIdk.ToArray(typeof(string)));
            ValtozasokAtadasa();
            if(!Elsoeset)
                tabControl1.Controls.Remove(ListaAdatbevPage);

        }
        /// <summary>
        /// 
        /// </summary>
        public override void OsszetettKozepsoParamAllitas()
        {
            ArrayList ar = new ArrayList();
            for (int i = 0; i < OsszetettOrigertekek.Length; i++)
            {
                if (OsszetettOrigertekek[i])
                    ar.Add(OsszetettKozepsoTabla.Rows[i][OsszetettKozepsoIdNev].ToString());
            }
            OsszetettKozepsoIdk = (string[])ar.ToArray(typeof(string));
            AktivControl.OsszetettKozepsoIdk = OsszetettKozepsoIdk;
            AktivControl.EgyediOsszetettValtozas();
        }
        /// <summary>
        /// 
        /// </summary>
        public override void OsszetettAlsoTablaInit()
        {
            base.OsszetettAlsoTablaInit();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramok"></param>
        /// <param name="view"></param>
        /// <param name="tabla"></param>
        /// <param name="idnev"></param>
        /// <param name="colnev"></param>
        public void ParamAllitasKozos(string[] paramok, DataGridView view, DataView tabla, string idnev, string colnev)
        {
            int db = 0;
            ArrayList ar = new ArrayList();
            if (paramok != null)
                ar = new ArrayList(paramok);
            view.Rows.Clear();
            view.Rows.Add(tabla.Count);
            bool[] origok = new bool[tabla.Count];
            for (int i = 0; i < tabla.Count; i++)
            {
                bool van = true;
                DataRow row = tabla[i].Row;
                if (ar.Count != 0)
                {
                    if (ar.IndexOf(row[idnev].ToString()) == -1)
                        van = false;
                }
                else
                    van = true;
                view.Rows[i].Cells[0].Value = row[colnev];
                view.Rows[i].Cells[1].Value = van;
                origok[i] = van;
                if (van)
                    db++;
            }
            if (VanEgyszeru)
                egyszerudarab.Text = db.ToString();
            if (VanOsszetett)
                osszetettdarab.Text = db.ToString();
            return;
        }
        /// <summary>
        /// 
        /// </summary>
        public override void OsszetettKozepsoParamAlapertAllitas()
        {
            osszetettparamkozepsogridview.Rows.Clear();
            osszetettparamkozepsogridview.Rows.Add(OsszetettKozepsoTabla.Rows.Count);
            osszetettparamkozepsogridview.Columns[0].HeaderText = Radiobuttonok[AktualisRadiobuttonIndex].Text;

            for(int i=0;i<OsszetettKozepsoTabla.Rows.Count;i++)
                osszetettparamkozepsogridview.Rows[i].Cells[1].Value = true;
            OsszetettOrigertekek = new bool[osszetettparamkozepsogridview.Rows.Count];
            for (int i = 0; i < OsszetettOrigertekek.Length; i++)
                OsszetettOrigertekek[i] = true;
            OsszetettKozepsoParamAllitas();
        }
        private void OsszetettParamAlsoAllitas(string[] paramok)
        {
            if (OsszetettAlsoTabla != null)
            {
                ParamAllitasKozos(paramok, osszetettparamalsogridview, OsszetettAlsoTablaView, OsszetettAlsoIdNev, OsszetettAlsoMegnevColumnNev);
                OsszetettParamValtozas = true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mindatumok"></param>
        /// <param name="maxdatumok"></param>
        /// <param name="alapertdatumok"></param>
        /// <param name="aktdatumok"></param>
        /// <returns></returns>
        public override bool DatumParameterezInit(DateTime[] mindatumok, DateTime[] maxdatumok,DateTime[] alapertdatumok,DateTime[] aktdatumok)
        {
            if (mindatumok.Length == 1 && CsakEgyHonap || mindatumok.Length==2)
            {
                TolMinDatum = mindatumok[0];
                TolAlapert = alapertdatumok[0];
                TolMaxDatum = maxdatumok[0];
                Datumtol = aktdatumok[0];
                Datumig = Datumtol.AddMonths(1).AddDays(-1);
                if (!CsakEgyHonap)
                {
                    IgMinDatum = mindatumok[1];
                    IgAlapert = alapertdatumok[1];
                    IgMaxDatum = maxdatumok[1];
                    Datumig = aktdatumok[1];
                }
                tabControl1.SelectedIndex = 0;
                if(CsakEgyHonap)
                    Frissit(Datumtol);
                else
                    Frissit(Datumtol,Datumig);
                return true;
            }
            else
            {
                FakPlusz.MessageBox.Show("Megadott dátumok száma("+mindatumok.Length.ToString()+") hibás!");
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="datum"></param>
        public void Frissit(DateTime datum)
        {
            FakUserInterface.Cegadatok(datum);
            FrissitKozos(datum,DateTimePicker.MaximumDateTime);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tol"></param>
        /// <param name="ig"></param>
        public void Frissit(DateTime tol,DateTime ig)
        {
            FakUserInterface.Cegadatok(new DateTime[]{tol,ig});
            FrissitKozos(tol,ig);
        }
        private void FrissitKozos(DateTime tol,DateTime ig)
        {
            if (Listae)
            {
                string[] listakepidk = FakUserInterface.GetSzurtOsszefIdk(UserContListakep, new object[] { new string[] { UserContId }, "" });
                ListaNev = FakUserInterface.GetTartal(ListaInfo, "SZOVEG", "ID", listakepidk[0])[0];
            }
            if (Adatszolge)
            {
                AktivControl.UserAdatSzolgInfo = UserAdatSzolgInfo;
                AktivControl.AdatszolgaltatasInfok = new  TablainfoCollection();
                ArrayList adatszolgnevek = new ArrayList();
                string adatszolgnev = FakUserInterface.GetTartal(ListaInfo, "ADATSZOLGNEV", "SZOVEG", ListaNev)[0];
                do
                {
                    adatszolgnevek.Add(adatszolgnev);
                    UserAdatSzolgInfo.DataView.RowFilter = "SZOVEG = '" + adatszolgnev + "'";
                    AktivControl.AdatszolgaltatasInfok.Add(FakUserInterface.GetAdatkozl(adatszolgnev));
                    adatszolgnev = UserAdatSzolgInfo.DataView[0].Row["SZULOTABLA"].ToString();
                } while (adatszolgnev != "");
                AktivControl.AdatszolgaltatasNevek = (string[])adatszolgnevek.ToArray(typeof(string));
                UserAdatSzolgInfo.DataView.RowFilter = "";
            }
            FakUserInterface.EventTilt = true;
            if (TolPicker != null)
            {
                TolPicker.MinDate = DateTimePicker.MinimumDateTime;
                TolPicker.MaxDate = DateTimePicker.MaximumDateTime;
                TolPicker.Value = tol;
                if(AktivControl.Datumtol!=tol)
                {
                    AktivControl.Datumtol=tol;
                    DatumValtozas=true;
                }
                TolPicker.MinDate = TolMinDatum;
                TolPicker.MaxDate = TolMaxDatum;
            }
            if (IgPicker != null)
            {
                IgPicker.MinDate = DateTimePicker.MinimumDateTime;
                IgPicker.MaxDate = DateTimePicker.MaximumDateTime;
                IgPicker.Value = ig;
                 if(AktivControl.Datumig!=ig)
                {
                    AktivControl.Datumig=ig;
                    DatumValtozas=true;
 //                   AktivControl.DatumValtozas=true;
                }
                IgPicker.MinDate = IgMinDatum;
                IgPicker.MaxDate = IgMaxDatum;
            }
            FakUserInterface.EventTilt = false;
            if (DatumValtozas)
                Valtozas = true;
            if (Elsoeset)
            {
                Valtozas=true;
                if(Listae)
                    ((Alaplista)AktivControl).AlaplistaInit(FakUserInterface, AktivControl, AktivControl.AktivVezerles);
            }
            for (int i = 0; i < ListaButtonok.Length; i++)
            {
                ListaButtonok[i].Enabled = true;
                OkButtonok[i].Enabled = false;
                OkVolt[i] = true;
                if (tabControl1.Controls.IndexOf(ListaAdatbevPage) == -1)
                {
                    FakUserInterface.EventTilt = true;
                    tabControl1.Controls.Add(ListaAdatbevPage);
                    FakUserInterface.EventTilt = false;
                }
            }
            ValtozasokAtadasa();
        }
        /// <summary>
        /// 
        /// </summary>
        public override void SzurtIdkAllitasa()
        {
            SzurtIdk=new ArrayList();
            if (VanValasztek && AlapIdNev!=null && AlapIdNev!="")
            {
                foreach (string id in ValasztekIdk)
                {
                    for (int i = 0; i < ValasztekTabla.Rows.Count; i++)
                    {
                        string alapid = ValasztekTabla.Rows[i][ValasztekIdNev].ToString();
                        if (id == alapid)
                            SzurtIdk.Add(ValasztekTabla.Rows[i][AlapIdNev].ToString());
                    }
                }
            }
            if (VanEgyszeru)
            {
                for (int i = 0; i < EgyszeruTablaView.Count; i++)
                {
                    if (EgyszeruOrigertekek[i])
                        SzurtIdk.Add(EgyszeruTablaView[i].Row[EgyszeruIdNev].ToString());
                }
                egyszerudarab.Text = SzurtIdk.Count.ToString();
            }
            if (VanOsszetett)
            {
                for (int i = 0; i < SzuroTablaView.Count; i++)
                {
                    string egyert = SzuroTablaView[i].Row[SzuroTablaIdNev].ToString();
                    for (int j = 0; j < OsszetettOrigertekek.Length; j++)
                    {
                        if (OsszetettOrigertekek[j])
                        {
                            string ert = OsszetettKozepsoTablaView[j].Row[OsszetettKozepsoIdNev].ToString();
                            if (ert == egyert)
                            {
                                SzurtIdk.Add(SzuroTablaView[i].Row[OsszetettAlsoIdNev].ToString());
                                break;
                            }
                        }
                    }
                }
            }
            AktivControl.SzurtIdk = SzurtIdk;
        }
        private void datumparamvalasztek_SelectionChangeCommitted(object sender, EventArgs e)
        {
            OkVolt[0] = false;
            OkButtonok[0].Enabled = true;
            for (int i = 0; i < ListaButtonok.Length; i++)
                ListaButtonok[i].Enabled = false;
            FakUserInterface.EventTilt = true;
            tabControl1.Controls.Remove(ListaAdatbevPage);
            FakUserInterface.EventTilt = false;
        }
        //private void paramgridview_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        //{
        //    DataGridView view = (DataGridView)sender;
        //    DataGridViewRow dgrow = view.Rows[e.RowIndex];
        //    DataGridViewCell dcell = dgrow.Cells[1];
        //    int aktindex = 0;
        //    bool[] origok = null;
        //    string nev = view.Name;
        //    if (VanEgyszeru)
        //    {
        //        origok = EgyszeruOrigertekek;
        //        if (VanDatum)
        //            aktindex = 1;
        //        else
        //            aktindex = 0;
        //    }
        //    else
        //    {
        //        origok = OsszetettOrigertekek;
        //        if (VanDatum)
        //            aktindex = 1;
        //        else
        //            aktindex = 0;
        //    }
        //    bool ertek = (bool)dcell.Value;
        //    if (ertek != origok[e.RowIndex])
        //    {
        //        origok[e.RowIndex] = ertek;
        //        OrigokAllitas(origok,ertek, e.RowIndex, aktindex);
        //    }
        //}
  //      private void OrigokAllitas(bool[] origok, bool ertek, int rowindex, int aktindex)
        private void OrigokAllitas(bool ertek,int rowindex)
        {
            bool vantrue = false;
            if (rowindex != -1)
                origok[rowindex] = ertek;
            else
            {
                for (int i = 0; i < origok.Length; i++)
                    origok[i] = ertek;
            }
            for (int i = 0; i < origok.Length; i++)
            {
                if (origok[i])
                    vantrue = true;
            }
            OkVolt[aktindex] = false;
            OkButtonok[aktindex].Enabled = vantrue;
            for (int i = 0; i < ListaButtonok.Length; i++)
                ListaButtonok[i].Enabled = false;
            FakUserInterface.EventTilt = true;
            tabControl1.Controls.Remove(ListaAdatbevPage);
            FakUserInterface.EventTilt = false;
            if (VanEgyszeru)
                EgyszeruParamValtozas = true;
            //               AktivControl.EgyszeruParamValtozas = true;
            else
            {
                osszetettparamalsogridview.Rows.Clear();
                //if (vantrue)
                //{
                //    OsszetettParamValtozas = true;
                //    OsszetettKozepsoParamAllitas();
                //}
            }
            ValtozasokAtadasa();
        }
        private void radiobutton_CheckedChanged(object sender, EventArgs e)
        {
            if (!FakUserInterface.EventTilt)
            {
                RadioButton but = (RadioButton)sender;
                string nev = but.Name.Substring(but.Name.Length - 1);
                if (but.Checked)
                {
                    OsszetettParamValtozas = true;
                    AktualisRadiobuttonIndex = Convert.ToInt16(nev) - 1;
                    OsszetettInit(null,AktualisRadiobuttonIndex);
                    osszetettparamkozepsook.Enabled = true;
                    for (int i = 0; i < ListaButtonok.Length; i++)
                        ListaButtonok[i].Enabled = false;
 //                   tabControl1.Controls.Remove(ListaAdatbevPage);
                    osszetettparamalsogridview.Rows.Clear();
                }
            }
        }

        private void ok_click(object sender, EventArgs e)
        {
            string butname = ((ToolStripButton)sender).Name;
            int aktindex = 0;
            bool vanszurtid = true;
            for (int i = 0; i < OkButtonok.Length; i++)
            {
                if (OkButtonok[i].Name == butname)
                    aktindex = i;
            }
            bool volttrue = true;

            if (VanDatum && aktindex==0)
            {
                DateTime toldate = DateTimePicker.MinimumDateTime;
                DateTime igdate = DateTimePicker.MaximumDateTime;
                bool hiba = false;
                DatumValtozas = false;
                ValasztekValtozas = false;
                if (TolPicker != null)
                {
                    toldate = TolPicker.Value;
                    if (IgPicker != null)
                        igdate = IgPicker.Value;
                    if (toldate.CompareTo(igdate) > 0)
                    {
                        FakUserInterface.ErrorProvider.SetError(TolPicker, "Tól > Ig!");
                        hiba = true;
                    }
                    else if (IgPicker != null)
                    {
                        if (VanEgyediDatumHibavizsg)
                        {
                            string hibaszov = AktivControl.EgyediDatumValtozas(toldate, igdate);
                            if (hibaszov != "")
                            {
                                FakUserInterface.ErrorProvider.SetError(TolPicker, hibaszov);
                                hiba = true;
                            }
                        }

                        else if (IgPicker != null && toldate.Year != igdate.Year)
                        {
                            FakUserInterface.ErrorProvider.SetError(TolPicker, "Azonos év kell!");
                            hiba = true;
                        }
                    }
                    if (!hiba)
                    {
                        if (toldate.CompareTo(Datumtol) != 0 || igdate.CompareTo(Datumig) != 0)
                        {
                            if (AktivControl.Datumtol != toldate || AktivControl.Datumig != igdate)
                            {
                                Datumtol = toldate;
                                Datumig = igdate;
                                if (IgPicker == null)
                                    Datumig = Datumtol.AddMonths(1).AddDays(-1);
                                AktivControl.Datumtol = toldate;
                                AktivControl.Datumig = igdate;
                                DatumValtozas = true;
                            }
                        }
                    }
                }
                
                if (!hiba)
                {
                    FakUserInterface.ErrorProvider.SetError(TolPicker, "");
                    OkButtonok[aktindex].Enabled = false;
                    OkVolt[aktindex] = true;
                    if (Valasztek != null && (ValasztekIndex != AktivControl.ValasztekIndex || ValasztekIndex != Valasztek.SelectedIndex))
                    {
                        ValasztekIndex = Valasztek.SelectedIndex;
                        AktivControl.ValasztekIndex = ValasztekIndex;
                        ValasztekValtozas = true;
                        string pid = ValasztekIdk[ValasztekIndex];
                        string selectstring = " where " + ValasztekIdNev + "= " + pid;
                        Sqlinterface.Select(ValasztekTabla, FakUserInterface.AktualCegconn, ValasztekTablaNev, selectstring, "", false);
                        SzurtIdkAllitasa();
                        datumparamdarab.Text = SzurtIdk.Count.ToString();
                        //                       AktivControl.EgyediValasztekValtozas();
                    }
                }
            }
            if ((VanEgyszeru || VanOsszetett) && (VanDatum && aktindex != 0 || !VanDatum))
            {
                volttrue = false;
                for (int i = 0; i < origok.Length; i++)
                {
                    bool ertek = (bool)gridview.Rows[i].Cells[1].Value;
                    if (ertek)
                        volttrue = true;
                    if (ertek != origok[i])
                    {
                        origok[i] = ertek;
                        if (VanEgyszeru)
                            EgyszeruParamValtozas = true;
                        else
                            OsszetettParamValtozas = true;
                    }
                }
                OkVolt[aktindex] = true;
                OkButtonok[aktindex].Enabled = false;
                if (VanEgyszeru || VanOsszetett)
                {
                    SzurtIdkAllitasa();
                    if (SzurtIdk.Count == 0)
                        vanszurtid = false;

                    if (VanOsszetett && vanszurtid)
                        OsszetettParamAlsoAllitas((string[])SzurtIdk.ToArray(typeof(string)));
                }
            }
            bool okvolt = true;
            for (int i = 0; i < OkVolt.Length; i++)
            {
                if (!OkVolt[i])
                    okvolt = false;
            }
            if (okvolt)
            {
                for (int i = 0; i < ListaButtonok.Length; i++)
                    ListaButtonok[i].Enabled = volttrue && vanszurtid ;
                if (DatumValtozas || ValasztekValtozas || EgyszeruParamValtozas || OsszetettParamValtozas ||
                     ListaParamValtozas)
                    Valtozas = true;
            }
            ValtozasokAtadasa();
        }
        private void lista_Click(object sender, EventArgs e)
        {
            if (tabControl1.Controls.IndexOf(ListaAdatbevPage) == -1)
          {
                FakUserInterface.EventTilt = true;
                tabControl1.Controls.Add(ListaAdatbevPage);
                FakUserInterface.EventTilt = false;
            }
            tabControl1.SelectedIndex = 1;
        }

        private void mind_click(object sender, EventArgs e)
        {
            gridview.Rows[0].Cells[0].Selected = true;
            for (int i = 0; i < gridview.Rows.Count; i++)
            {
                Beallit(i, true);
            }
            gridview.Rows[0].Cells[0].Selected = false;
            gridview.Rows[0].Cells[1].Selected = true;
        }

        private void egysem_click(object sender, EventArgs e)
        {
            gridview.Rows[0].Cells[0].Selected = true;
            for (int i = 0; i < gridview.Rows.Count; i++)
            {
                Beallit(i, false);
            }
            gridview.Rows[0].Cells[0].Selected = false;
            gridview.Rows[0].Cells[1].Selected = true;
        }
        
        private void alapert_click(object sender, EventArgs e)
        {
            DatumValtozas = false;
            ValasztekValtozas = false;
            string butname = ((ToolStripButton)sender).Name;
            int aktindex = 0;
            FakUserInterface.EventTilt = true;
            if (TolPicker != null)
            {
                TolPicker.MinDate = DateTimePicker.MinimumDateTime;
                TolPicker.MaxDate = DateTimePicker.MaximumDateTime;
                TolPicker.Value = TolAlapert;
                TolPicker.MinDate = TolMinDatum;
                TolPicker.MaxDate = TolMaxDatum;
                if (Datumtol != TolAlapert)
                    Valtozas = true;
 //               Datumtol = TolAlapert;
            }
            if (IgPicker != null)
            {
                IgPicker.MinDate = DateTimePicker.MinimumDateTime;
                IgPicker.MaxDate = DateTimePicker.MaximumDateTime;
                IgPicker.Value = IgAlapert;
                IgPicker.MinDate = IgMinDatum;
                IgPicker.MaxDate = IgMaxDatum;
                if (Datumig != IgAlapert)
                    Valtozas = true;
//                Datumig = IgAlapert;
            }
            FakUserInterface.EventTilt = false;
            if (VanValasztek)
            {
                if (ValasztekIndex != 0)
                {
                    ValasztekIndex = 0;
                    Valasztek.SelectedIndex = 0;
                    if (AktivControl.ValasztekIndex != 0)
                        Valtozas = true;
                }
            }
            if(Valtozas)
            {
                OkVolt[aktindex] = false;
                OkButtonok[aktindex].Enabled = true;
                for (int i = 0; i < ListaButtonok.Length; i++)
                    ListaButtonok[i].Enabled = false;
                if (!Elsoeset)
                {
                    FakUserInterface.EventTilt = true;
                    tabControl1.Controls.Remove(ListaAdatbevPage);
                    FakUserInterface.EventTilt = false;
                }
            }
            ValtozasokAtadasa();
        }
        private void help_click(object sender, EventArgs e)
        {
            if (FakUserInterface.EventTilt)
                return;
            ToolStripButton cont = (ToolStripButton)sender;
            string azon = cont.Tag.ToString();
            FakUserInterface.ShowHelp(azon, false, this);

        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (!FakUserInterface.EventTilt)
            {
                if (e.TabPageIndex == 1)
                {
                    if (!Listae && AktivControl.AktivDropindex != -1)
                    {
                        FakUserInterface.EventTilt = true;
                        AktivControl.TabControl.SelectedIndex = -1;
                        AktivControl.TabControl.SelectedIndex = AktivControl.AktivDropindex;
                        FakUserInterface.EventTilt = false;
                        AktivControl.Focus();
                    }
                    tabstopboljott = true;
                    AktivControl.TabStop = false;
                    AktivControl.TabStop = true;
                }
            }
        }
        private void listatextbox_Validated(object sender, EventArgs e)
        {

        }
        private void listaparamgridview_SelectionChanged(object sender, EventArgs e)
        {

        }
        private void datumparamtol_ValueChanged(object sender, EventArgs e)
        {
            if (!FakUserInterface.EventTilt)
            {
                DateTimePicker picker = (DateTimePicker)sender;
                string datum = "";
                DateTime datumdate;
                bool tole=false;
                bool valt = false;
                if (picker.Name.Contains("tol"))
                    tole = true;
                if (tole)
                {
                    datum = DatumtolString;
                    datumdate = Datumtol;
                }
                else
                {
                    datum = DatumigString;
                    datumdate = Datumig;
                }
                string ev=picker.Value.Year.ToString();
                string ho = picker.Value.Month.ToString();
                if (ho.Length == 1)
                    ho = "0" + ho;
                string evho = ev + "." + ho;
                if (TeljesEv)
                {
                    if (datum.Substring(0, 4) != ev)
                        valt = true;
                    else
                    {
                        FakUserInterface.EventTilt = true;
                        picker.Value = datumdate;
                        FakUserInterface.EventTilt = false;
                    }
                }
                else if (TeljesHonap)
                {
                    if (datum.Substring(0, 7) != evho)
                        valt = true;
                    else
                    {
                        FakUserInterface.EventTilt = true;
                        picker.Value = datumdate;
                        FakUserInterface.EventTilt = false;
                    }
                }
                else if (datumdate.CompareTo(picker.Value) != 0)
                    valt = true;
                if (valt)
                {
                    OkVolt[0] = false;
                    OkButtonok[0].Enabled = true;
                    for (int i = 0; i < ListaButtonok.Length; i++)
                        ListaButtonok[i].Enabled = false;
                    tabControl1.Controls.Remove(ListaAdatbevPage);
                    if (datumparamvalasztek.Visible)
                        Hivo.EgyediValasztekValtozas(TolPicker.Value,IgPicker.Value);
                }
            }
        }
        private void tabControl1_Deselecting(object sender, TabControlCancelEventArgs e)
        {
            if (FakUserInterface.EventTilt)
                e.Cancel = true;
            else if (AktivControl.Visible && (AktivControl.AktivMenuItem.Text.Contains("*") || AktivControl.AktivMenuItem.Text.Contains("!")))
            {
                if (AktivControl.Userabortkerdes())
                    e.Cancel = true;
            }
        }

        private void paramgridview_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex == 1)
            {
                DataGridView view = (DataGridView)sender;
                DataGridViewRow dgrow = view.Rows[e.RowIndex];
                DataGridViewCell dcell = dgrow.Cells[1];
                Beallit(e.RowIndex, !(bool)dcell.Value);
            }
//                int aktindex = 0;
//                bool[] origok;
////                string nev = view.Name;
//                origok = EgyszeruOrigertekek;
//                if (VanOsszetett)
//                    origok = OsszetettOrigertekek;
//                if (VanDatum)
//                    aktindex = 1;
//                else
//                    aktindex = 0;
//                bool ertek = !(bool)dcell.Value;

//                if (ertek != origok[e.RowIndex])
//                {
//                    origok[e.RowIndex] = ertek;
//                    OrigokAllitas(origok, ertek, e.RowIndex, aktindex);
//                }
//            }

        }
        private void Beallit(int rowindex, bool ertek)
        {
            DataGridViewCell dcell = gridview.Rows[rowindex].Cells[1];
            bool valt = ertek != (bool)dcell.Value;
            gridview.Rows[rowindex].Cells[1].Value = ertek;
            OrigokAllitas(ertek, rowindex);
        }
    }
}


