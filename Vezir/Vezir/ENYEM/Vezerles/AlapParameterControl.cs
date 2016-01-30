using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FakPlusz;
using FakPlusz.Alapcontrolok;
using FakPlusz.Alapfunkciok;
using FakPlusz.VezerloFormok;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Windows.Forms;
using CrystalDecisions.Shared;
namespace Vezir
{
    public partial class AlapParameterControl : FakPlusz.Alapcontrolok.Alaplista
    {
  //      public ReportDocument reportdoc;
        public VezirDataSet dataset = new VezirDataSet();
        public string[] VizsgalandoDatumValtozasok = null;
        public string datum = DateTime.Today.ToShortDateString();
        public VezerloControl vezerlo;
        public string foldernev = Directory.GetCurrentDirectory();
        public int rekdarabszam = 0;
        public string UjAdatkivitelFilename = "";
        public AlapParameterControl()
        {
            InitializeComponent();
        }
        public virtual void AlaplistaControlInit(FakUserInterface fak, Base hivo, Vezerloinfo vezerles, DataTable[] datasettablak)
        {
            FakUserInterface = fak;
            Vezerles = vezerles;
            Hivo = hivo;
            vezerlo = (VezerloControl)hivo;
            int i = vezerles.ControlNevek.IndexOf(this.Name);
            Parameterez = vezerles.Parameterez[i];
            Paramfajta = Parameterez.Paramfajta;
            Valasztek = Parameterez.Valasztek;
            Listae = Parameterez.Listae;
            VanDatum = Parameterez.VanDatum;
            VanValasztek = Parameterez.VanValasztek;
            VanEgyszeru = Parameterez.VanEgyszeru;
            VanOsszetett = Parameterez.VanOsszetett;
            VanLista = Parameterez.VanLista;
            ListaInfo = Parameterez.ListaInfo;
            Adatszolge = Parameterez.Adatszolge;
            UserAdatSzolgInfo = Parameterez.UserAdatSzolgInfo;
            AdatszolgSpecfix = Parameterez.AdatszolgSpecfix;
            SpecAdatSzolgnevInfo = Parameterez.SpecAdatSzolgnevInfo;
            SpecFixertekNevek = Parameterez.SpecFixertekNevek;
            BeallitandoDatumNevek = null;
            Parameterez.BeallitandoDatumNevek = BeallitandoDatumNevek;
            DatasetTablak = datasettablak;
            Datumtol = UserParamTabla.Datumtol;
            Datumig = UserParamTabla.Datumig;
            Parameterez.Datumtol = Datumtol;
            Parameterez.Datumig = Datumig;
            CsakEgyHonap = Parameterez.CsakEgyHonap;
            TeljesEv = Parameterez.TeljesEv;
            if (UserParamTabla.SzurtIdk != null && UserParamTabla.AktualControlNev== this.Name)
                SzurtIdk = new ArrayList(UserParamTabla.SzurtIdk);
            else
                SzurtIdk = new ArrayList();
            ValasztekIndex = UserParamTabla.ValasztekIndex;
            AktualisRadiobuttonIndex = UserParamTabla.RadioButtonIndex;
            if (Parameterez.Radiobuttonok != null)
            {
                FakUserInterface.EventTilt = true;
                Parameterez.Radiobuttonok[AktualisRadiobuttonIndex].Checked = true;
                FakUserInterface.EventTilt = false;
                OsszetettKozepsoIdk = UserParamTabla.OsszetettKozepsoParamok;
                OsszetettKozepsoNev = UserParamTabla.OsszetettKozepsoNev;
                OsszetettAlsoIdk = UserParamTabla.OsszetettAlsoParamok;
            }
            ArrayList ar = new ArrayList();
            foreach (DataTable datatable in DatasetTablak)
            {
                string name = datatable.TableName;
                TablainfoCollection tabinfok = vezerlo.TermCegPluszCegalattiTabinfok.GetByTablanev(name);
                foreach (Tablainfo egyinfo in tabinfok)
                    ar.Add(egyinfo);
            }
            Aktualtablainfo = (Tablainfo[])ar.ToArray(typeof(Tablainfo));
            UserControlInfo = fak.Attach(this, vezerles, ref Aktualtablainfo, AktivPage, AktivMenuItem, AktivDropDownItem);
            AlapTablaNev = "";
        }
        public override void Ujcontroloktolt()
        {
            bool valtozas = ValtozasLekerdez().Count != 0;
            bool nodevaltozas = ValtozasLekerdez("NodeValtozas").Count != 0;
            bool adatvaltozas = ValtozasLekerdez("AdatValtozas").Count != 0;
            bool aktivvaltozas = ValtozasLekerdez("AktivValtozas").Count != 0;
            bool cegvaltozas = ValtozasLekerdez("CegValtozas").Count != 0;
            if (valtozas || Valtozas || nodevaltozas || adatvaltozas || aktivvaltozas)
            {
                foreach (DataTable datatable in dataset.Tables)
                    datatable.Rows.Clear();
                rekdarabszam = 0;
                if (cegvaltozas)
                {
                    if (VanDatum)
                    {
                        int hossz = 2;
                        if (CsakEgyHonap || TeljesEv)
                            hossz = 1;
                        DateTime[] mindatum = new DateTime[hossz];
                        DateTime[] maxdatum = new DateTime[hossz];
                        DateTime[] alapertdatum = new DateTime[hossz];
                        DateTime[] aktdatum = new DateTime[hossz];
                        aktdatum[0] = UserParamTabla.Datumtol;
                        if (hossz == 2)
                            aktdatum[1] = UserParamTabla.Datumig;
                        for (int i = 0; i < mindatum.Length; i++)
                        {
                            mindatum[i] = UserParamTabla.InduloDatum;
                            maxdatum[i] = UserParamTabla.AktualisDatum;
                            alapertdatum[i] = UserParamTabla.AktualisDatum;
                        }
                        if (hossz == 2)
                        {
                            maxdatum[1] = UserParamTabla.AktualisDatumig;
                            alapertdatum[1] = UserParamTabla.AktualisDatumig;
                        }
                        else
                            maxdatum[0] = UserParamTabla.AktualisDatumig;
                        Parameterez.DatumParameterezInit(mindatum, maxdatum, alapertdatum, aktdatum);
                    }
                    else
                        Parameterez.Frissit(UserParamTabla.AktualisDatum);
                    ValtozasTorol("CegValtozas");
                    if (UserParamTabla.UserParamok == "")
                        UserParamTabla.SzurtIdk = null;
                    if(AlapTablaNev!="")
                         base.AlapTablaInit();
                    if (VanValasztek)
                        Parameterez.ValasztekParameterekInit();
                    if (VanEgyszeru)
                        SzurtIdk = Parameterez.EgyszeruInit(UserParamTabla.SzurtIdk);
                    else if (VanOsszetett)
                        Parameterez.OsszetettInit(UserParamTabla.OsszetettKozepsoParamok, UserParamTabla.RadioButtonIndex);
                }
                else if (DatumValtozas)
                {
                    Datumtol = Parameterez.Datumtol;
                    Datumig = Parameterez.Datumig;
                    if (CsakEgyHonap)
                        Parameterez.Frissit(Datumtol);
                    else
                        Parameterez.Frissit(Datumtol, Datumig);
                    AlapTablaInit();
                }
                if (VanValasztek)
                {
                    if (ValasztekValtozas || aktivvaltozas)
                    {
                        if (Parameterez.ValasztekTabla.Rows.Count == 0)
                        {
                            FakPlusz.MessageBox.Show("A darabszám 0!");
                            return;
                        }
                        else
                            UserParamTabla.ValasztekIndex = ValasztekIndex;

                    }
                }
                else if (VanEgyszeru)
                {
                    if (EgyszeruParamValtozas || aktivvaltozas)
                    {
                        EgyszeruIdk = (string[])SzurtIdk.ToArray(typeof(string));
                        UserParamTabla.SzurtIdk = EgyszeruIdk;
                    }
                }
                else if (VanOsszetett)
                {
                    if (OsszetettParamValtozas || aktivvaltozas)
                    {
                        OsszetettKozepsoIdk = Parameterez.OsszetettKozepsoIdk;
                        Parameterez.SzurtIdkAllitasa();
                        UserParamTabla.OsszetettKozepsoParamok = OsszetettKozepsoIdk;
                        OsszetettAlsoIdk = (string[])SzurtIdk.ToArray(typeof(string));
                        UserParamTabla.SzurtIdk = OsszetettAlsoIdk;
                        UserParamTabla.RadioButtonIndex = Parameterez.AktualisRadiobuttonIndex;
                    }
                }
                if (Listae && (ListaNev != Parameterez.ListaNev || reportdoc == null))
                {
                    ListaNev = Parameterez.ListaNev;
                    reportdoc = EgyediReportIni(ListaNev);
//                    if (reportdoc != null)
//                        reportdoc.PrintOptions.PrinterName = PrinterName;
                }
                UserParamTabla.Datumtol = Datumtol;
                UserParamTabla.Datumig = Datumig;
                FakUserInterface.SetUserSzamitasokDatumHatarok(Datumtol, Datumig);
                Elsoeset = false;
                Parameterez.Elsoeset = false;
                if (!valtozas)
                {
                    vezerlo.WriteLoginfo();
                }
                ListaAdatkivitel(true);
                if (Listae)
                {
                    EgyediListaResz();
                    crystalReportViewer1.Enabled = false;
                    crystalReportViewer1.Enabled = true;
                }
                ValtozasTorol();
                Parameterez.ValtozasokTorlese();
                ValtozasokTorlese();
                if (Parameterez.tabControl1.Controls.IndexOf(Parameterez.ListaAdatbevPage) != -1)
                {
                    FakUserInterface.EventTilt = true;
                    Parameterez.tabControl1.SelectedIndex = 1;
                    FakUserInterface.EventTilt = false;
                }
                ValtozasTorol("AktivValtozas");
            }
        }
        public override void AlapTablaInit()
        {
            if (AlapTablaNev != "")
            {
                switch (AlapTablaNev)
                {
                    case "CEGSZLAHONAPOK":
                        AlapTablaSelectString = "where SZLA_DATUM > = '" + Datumtol + "' AND SZLA_DATUM <='" + Datumig + "'";
                        break;
                }
                base.AlapTablaInit();
            }
        }
        public override void EgyediDatumValtozas()
        {
            Datumtol = Parameterez.Datumtol;
            Datumig = Parameterez.Datumig;
            if (CsakEgyHonap)
                Parameterez.Frissit(Datumtol);
            else
                Parameterez.Frissit(Datumtol, Datumig);
        }
        public override void EgyediOsszetettInit()
        {
        }
        public override void ListaAdatkivitel(bool lista)
        {
            rekdarabszam = 0;
            string[] interv = null;
            string[] nevek = null;
            int leng = 1;
            if (VanDatum)
            {
                nevek = BeallitandoDatumNevek;
                if (!CsakEgyHonap)
                    leng = 2;
                interv = new string[leng];
                interv[0] = DatumtolString;
                if (leng == 2)
                    interv[1] = DatumigString;
            }
                if (lista)
                    DataSetTolt();
                else
                    Adatszolgaltatas(true);
        }
        public override bool EgyediSpecFix(ref string ertek)
        {
            return base.EgyediSpecFix(ref ertek);
        }
        public virtual void EgyediListaResz()
        {
        }
        public virtual bool EgyediDatumvaltozas(bool tolvalt, bool igvalt)
        {
            return false;
        }
        public virtual ReportDocument EgyediReportIni(string listanev)
        {
            return null;
        }
    }
}
