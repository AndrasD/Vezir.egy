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
//using Vezir.ENYEM.Main;
namespace Vezir
{
    public partial class MainControl : MainControlAlap
    {
        private Kukucs Tesztlap = null;
        private KukucsEredm Eredmlap = null;
        private CegAllapotok Cegallapotok = null;
        private bool cegvaltas = false;
        private bool aktivcegekelnek = true;
        private new Base.KezSzint KezeloiSzint = Base.KezSzint.Fejleszto;
        private Ceginformaciok[] AktivCeginformaciok = null;
        public VezerloControl VezerloControl = null;
        public string[] letezousercontrolnevek = new string[] {"Partner", "Bevszla", "Beflenbevszla", "Bevszlakiegyenl", "Koltsszla","Beflenkoltsszla", "Koltsszlakiegyenl", "Folyofolyo", "Folyopenztar", "Folyokivet", "Folyobetet","Folyoosztottkivet", "Penztarpenztar", "Penztarfolyo", "Penztarkivet", "Penztarbetet", "Bevetelek", "Kiadasok", "Penzmozgas", "Afaforgalom", "Egyenleg", "Eveseredmeny","DolgozoiEredm", "Ujhonap", "Evzar", "Evnyit", "Szerklist" };
        public string[] Kellusertablak = new string[] { "SZUTKEZELOK", "SZUTRENDSZERGAZDA", "SZUOKEZELOALKALM" };
        public string[] Kellcegtablak = new string[] {"SZCKTermfocsop","SZCKTermalcsop","SZCKTermcsop",
            "SZCKKoltsfocsop","SZCKKoltsalcsop","SZCKKoltscsop","SZCKAfa","SZCKSema","SZCKFszazal","SZCTCEGKEZELOKIOSZT",
            "SZCTPARTNER","SZCOTfocsoptalcsop","SZCOTalcsoptcsop","SZCOKfocsopkalcsop","SZCOKalcsopkcsop"};
        public MainControl()
        {
            InitializeComponent();
            UserControlNevek = new ArrayList(letezousercontrolnevek);
        }
        public override void EgyediInditas()
        {
            UserParamTabla.Open(Bejelentkezo, this);
            EgyTreeView = treeView1;
            kezelonev.Text = Bejelentkezo.Nev;
            AktivCeginformaciok = UserParamTabla.AktivCegInformaciok;
            if (!UserParamTabla.LezartCeg)
            {
                aktivcegekelnek = true;
                aktivcegek.Enabled = false;
                if (UserParamTabla.LezartCegInformaciok == null || UserParamTabla.LezartCegInformaciok.Length == 0)
                    lezartcegek.Enabled = false;
            }
            else
            {
                aktivcegekelnek = false;
                lezartcegek.Enabled = false;
                if (UserParamTabla.AktivCegInformaciok == null || UserParamTabla.AktivCegInformaciok.Length == 0)
                    aktivcegek.Enabled = false;
            }
            comboBox2.Items.AddRange(UserParamTabla.CegNevek);
            FakUserInterface.ProgressRefresh();
            comboBox2.SelectedIndex = UserParamTabla.AktivCegIndex;
            AktualCeginformacio = UserParamTabla.AktualCegInformacio;
            KezeloiSzint = AktualCeginformacio.KezeloiSzint;
            HozferJog = Base.HozferJogosultsag.Irolvas;
            TermCegPluszCegalattiTabinfok = UserParamTabla.TermCegPluszCegalattiTablainfok;
            CegPluszCegszintuTablanevek = UserParamTabla.TermTablaNevek;
            Vezerloinfo parentvez = Vezerles;
            Vezerles = new Vezerloinfo(FakUserInterface, this.ControlNev, null, ref KezeloiSzint, ref HozferJog, new ArrayList(letezousercontrolnevek),ref UserParamTabla.UserContHozferjog);
//            Vezerles = new Vezerloinfo(FakUserInterface, MainForm.MainControlNev, null, ref KezeloiSzint, ref HozferJog,new ArrayList(letezousercontrolnevek),ref UserParamTabla.UserHozferJogosultsag);
            //           Vezerloinfo parentvez = Vezerles;
            Vezerles.Control = this;
            string contnev;
            for (int i = 0; i < Vezerles.ControlNevek.Count; i++)
            {
                FakUserInterface.ProgressRefresh();
                contnev = Vezerles.ControlNevek[i].ToString();
                switch (contnev)
                {
                    case "VezerloControl":
                        VezerloControl = new VezerloControl(FakUserInterface, Vezerles, panel3, panel2);
                        UserParamTabla.VezerloControl = VezerloControl;
                        Vezerles.LetezoControlok[i] = VezerloControl;
                        AktivControl = VezerloControl;
                        break;
                }
            }
            Ceginicializalas();
            UserParamTabla.Close();
        }
        public override bool Ceginicializalas()
        {
            FakUserInterface.ProgressRefresh();
            Base SaveAktivControl = AktivControl.AktivControl;
            bool formvezvolt = SaveAktivControl != null && SaveAktivControl.Name == "Formvezerles" ||
                UserParamTabla.UserParamok != "" && UserParamTabla.FormvezParam;
            int ind = UserParamTabla.AktivCegIndex;
            AktualCeginformacio = UserParamTabla.AktualCegInformacio;
            Cegindex = AktualCeginformacio.Cegindex;
            KezeloiSzint = AktualCeginformacio.KezeloiSzint;
            kezeloszerep.Text = SzovegesKezeloiSzint[(int)KezeloiSzint];
            string conn = AktualCeginformacio.CegConnection;
            string cegnev = AktualCeginformacio.CegNev;
            DateTime aktdat = AktualCeginformacio.AktualisDatum;
            if (aktdat.CompareTo(FakUserInterface.Mindatum) == 0)
                aktdat = DateTime.Today;
            FakUserInterface.Cegadatok(conn, cegnev, aktdat);
            UserParamTabla.AktualisCegIntervallum = FakUserInterface.VerzioInfok["C"].AktIntervallum;
            UserParamTabla.AktualisCegverzioId = FakUserInterface.VerzioInfok["C"].AktVerzioId;
            FakUserInterface.SetProgressText("");
            szladat.MinDate = DateTimePicker.MinimumDateTime;
            szladat.MaxDate = DateTimePicker.MaximumDateTime;
            FakUserInterface.EventTilt = true;
            szladat.Value = UserParamTabla.SzamlaDatumtol;
            FakUserInterface.EventTilt = false;
            szladat.MinDate = UserParamTabla.InduloDatum;
            szladat.MaxDate = UserParamTabla.AktualisDatumig;
            string contnev = UserParamTabla.AktualControlNev;
            UserParamTabla.Infotoltkell = true;
            UserParamTabla.SetKozosAllapotok();
            string tooltip = "N";
            treeView1.Nodes.Clear();
            for (int i = 0; i < Vezerles.LetezoControlok.Length; i++)
            {
                Base control = Vezerles.LetezoControlok[i];
                if (control != null)
                {
                    FakUserInterface.ProgressRefresh();
                    AktivPageIndex = i;
                    control.Dock = DockStyle.Fill;
                    control.TermCegPluszCegalattiTabinfok = UserParamTabla.TermCegPluszCegalattiTablainfok;
                    ValtozasBeallit("CegValtozas");
                    if (i == 0)
                    {
                        control.Ceginicializalas(Cegindex);
                        SaveAktivControl = control.AktivControl;
                        if (UserParamTabla.FormvezTreeViewShowNodeToolTip)
                            tooltip = "I";
                        if (UserParamTabla.UserParamok != "")
                        {
                            if (UserParamTabla.FormvezParam)
                            {
                                FormVezerles.Ceginicializalas(Cegindex, UserParamTabla.LezartCeg, KezeloiSzint, "Formvezerles", UserParamTabla.UserParamok);
                                control.AktivControl = FormVezerles;
                            }

                        }
                        else
                        {
                            string userparam = ",,,,,," + tooltip;
                            DataTable dt = new DataTable();
                            DataRow dr;
                            Sqlinterface.Select(dt, FakUserInterface.Userconn, "USERLOG", " where ALKALMAZAS_ID = " + FakUserInterface.AlkalmazasId +
                                " AND KEZELO_ID = " + FakUserInterface.KezeloId.ToString() + " AND CEG_ID = " + FakUserInterface.AktualCegid.ToString(), "order by LAST_MOD DESC", true);
                            if (dt.Rows.Count != 0)
                            {
                                dr = dt.Rows[0];
                                string name = dr["USERCONTNEV"].ToString();
                                if (name == "Formvezerles")
                                    userparam = dr["USERPARAMOK"].ToString();

                            }
                            FormVezerles.Ceginicializalas(Cegindex, UserParamTabla.LezartCeg, KezeloiSzint, "Formvezerles", userparam);
                            if (!formvezvolt)
                                control.AktivControl = SaveAktivControl;
                        }
                    }
                }
            }
            ValtozasTorol();
            label3.Text = "Adatbeviteli hónap: (" + UserParamTabla.InduloDatumString.Substring(0, 7) + "-" + UserParamTabla.AktualisDatumString.Substring(0, 7) + ")";
            return true;
        }
        private void comboBox2_Leave(object sender, EventArgs e)
        {
            ((Control)sender).BackColor = FakUserInterface.InaktivControlBackColor;
        }
        private void comboBox2_Enter(object sender, EventArgs e)
        {
            ((Control)sender).BackColor = FakUserInterface.AktivInputBackColor;
        }
        private void comboBox2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex != Cegindex)
            {
                if (AktivControl.Userabortkerdes())
                {
                    comboBox2.SelectedIndex = Cegindex;
                    return;
                }
                FakUserInterface.OpenProgress();
                UserParamTabla.SetAktualCeginformacio(UserParamTabla.LezartCeg, comboBox2.SelectedIndex);
                UserParamTabla.UserParamokFrissit();
                FakUserInterface.CloseProgress();
                cegvaltas = true;
                ValtozasBeallit("CegValtozas");
                FakUserInterface.SetProgressText(UserParamTabla.AktualCegInformacio.CegNev + " inicializálása");
                Ceginicializalas();
                comboBox2.Text = comboBox2.Items[comboBox2.SelectedIndex].ToString();
            }
        }
        public bool AktivLezartCegInicializalas(bool lezart)
        {
            comboBox2.Items.Clear();
            if (!lezart)
            {
                aktivcegekelnek = true;
                aktivcegek.Enabled = false;
                if (UserParamTabla.LezartCegInformaciok != null && UserParamTabla.LezartCegInformaciok.Length != 0)
                    lezartcegek.Enabled = true;
            }
            else
            {
                aktivcegekelnek = false;
                lezartcegek.Enabled = false;
                if (UserParamTabla.AktivCegInformaciok != null && UserParamTabla.AktivCegInformaciok.Length != 0)
                    aktivcegek.Enabled = true;
            }
            FakUserInterface.OpenProgress();
            UserParamTabla.SetAktualCeginformacio(lezart,0);
            UserParamTabla.UserParamokFrissit();
            FakUserInterface.CloseProgress();
            comboBox2.Items.AddRange(UserParamTabla.CegNevek);
            comboBox2.SelectedIndex = 0;
            return Ceginicializalas();
        }
        private void comboBox2_DropDownClosed(object sender, EventArgs e)
        {
            if (!cegvaltas)
                SetFocus();
            cegvaltas = false;
        }

        private void dateTimePicker_CloseUp(object sender, EventArgs e)
        {
            if (!FakUserInterface.EventTilt)
            {
                FakUserInterface.EventTilt = true;
                DateTime ujdat = szladat.Value;
                if (ujdat.Year != UserParamTabla.SzamlaDatumtol.Year || ujdat.Month != UserParamTabla.SzamlaDatumtol.Month)
                {
                    if (AktivControl.Userabortkerdes())
                    {
                        szladat.Value = UserParamTabla.SzamlaDatumtol;
                        FakUserInterface.EventTilt = false;
                        return;
                    }
                    else
                    {
                        string datumstring = FakUserInterface.DatumToString(ujdat);
                        datumstring = datumstring.Substring(0, 8) + "01";
                        ujdat = Convert.ToDateTime(datumstring);
                        szladat.Value = ujdat;
                        UserParamTabla.SzamlaDatumtol = ujdat;
                        UserParamTabla.SetAktualTermJogosultsag(ujdat.Year.ToString());
                        ValtozasBeallit("Datumvaltozas");
                        UserParamTabla.Infotoltkell = true;
                        FakUserInterface.OpenProgress("Adatbeviteli hónap váltás. Türelem!");
                        FakUserInterface.Cegadatok(new DateTime[] { UserParamTabla.SzamlaDatumtol, UserParamTabla.SzamlaDatumig });
                        FakUserInterface.CloseProgress();
                        UserParamTabla.AktualisCegIntervallum = FakUserInterface.VerzioInfok["C"].AktIntervallum;
                        UserParamTabla.AktualisCegverzioId = FakUserInterface.VerzioInfok["C"].AktVerzioId;
                        UserParamTabla.SetKozosAllapotok();
                        if (AktivControl.AktivControl != null)
                        {
                            if (AktivControl.AktivControl.Name == "Formvezerles")
                            {
                                if (AktivControl.AktivControl.AktivControl != null)
                                {
                                    AktivControl.AktivControl.AktivControl.UjTag = true;
                                    AktivControl.AktivControl.AktivControl.AltalanosInit();
                                }
                            }
                            else if (!AktivControl.AktivControl.Listae && AktivControl.AktivControl.AktivDropDownItem.Enabled)
                            {
                                AktivControl.AktivControl.Visible = false;
                                UserParamTabla.Infotolt();
                                if (AktivControl.AktivControl != null && AktivControl.AktivControl.ValtozasLekerdez().Count != 0)
                                {
                                    AktivControl.AktivControl.TabStop = false;
                                    AktivControl.AktivControl.TabStop = true;
                                }
                                AktivControl.AktivControl.Visible = true;
                            }
                        }
                    }
                }  
                SetFocus();
                FakUserInterface.EventTilt = false;
            }
        }
        public void SetFocus()
        {
            if (VezerloControl.AktivControl != null && !VezerloControl.AktivControl.Listae)
            {
                if (VezerloControl.AktivControl.Parameterez != null)
                {
                    Parameterez = AktivControl.AktivControl.Parameterez;
                    int savind = Parameterez.tabControl1.SelectedIndex;
                    if (savind == -1)
                        savind = 0;
                    Parameterez.tabControl1.SelectedIndex = -1;
                    Parameterez.tabControl1.SelectedIndex = savind;
                }
                else
                    VezerloControl.AktivControl.Focus();
            }
        }
        private void aktivcegek_Click(object sender, EventArgs e)
        {
            if (!aktivcegekelnek && UserParamTabla.AktivCegInformaciok != null && UserParamTabla.AktivCegInformaciok.Length != 0 && (AktivControl.AktivControl == null || !AktivControl.AktivControl.Userabortkerdes()))
            {
                UserParamTabla.AktivCegIndex = 0;
                AktivLezartCegInicializalas(false);
            }
        }

        private void lezartcegek_Click(object sender, EventArgs e)
        {
            if (aktivcegekelnek && UserParamTabla.LezartCegInformaciok != null && UserParamTabla.LezartCegInformaciok.Length != 0 && (AktivControl.AktivControl==null ||!AktivControl.AktivControl.Userabortkerdes()))
            {
                UserParamTabla.AktivCegIndex = 0;
                AktivLezartCegInicializalas(true);
            }
        }
        private void frissit_Click(object sender, EventArgs e)
        {
            ControlAlap cont = null;
            bool focused = false;
            if (AktivControl.AktivControl != null)
                SetFocus();
            if (!AktivControl.Userabortkerdes())
            {
                if (FakUserInterface.KellFrissit(true))
                {
                    UserParamTabla.UserParamokFrissit();
                    Ceginicializalas();
                    //if (AktivControl.Name != "Formvezerles")
                    //    ValtozasBeallit("Frissites");
                    //else
                    //    AktivControl.AktivControl.UjTag = true;
                    //AktivControl.AktivControl.TabStop = false;
                    //AktivControl.AktivControl.TabStop = true;
                }
            }
            FakUserInterface.ValtoztatasDatuma = DateTime.Now;
        }
        private void hianyzok_Click(object sender, EventArgs e)
        {
            string szov = "";
            if (UserParamTabla.KozosAllapotSzovegek.Count == 0)
                szov = " Minden van, ami kell";
            else
            {
                string[] allszovegek = (string[])UserParamTabla.KozosAllapotSzovegek.ToArray(typeof(string));
                foreach (string egyszov in allszovegek)
                {
                    if (szov != "")
                        szov += "\n";
                    szov += egyszov;
                }
            }
            FakPlusz.MessageBox.Show(szov, "Mi hiányzik?");
        }

        private void teszt_Click(object sender, EventArgs e)
        {
            if (Tesztlap == null)
                Tesztlap = new Kukucs(FakUserInterface, panel2, teszt, MainForm );
            panel1.Enabled = false;
            treeView1.Enabled = false;
            teszt.Visible = false;
            Verzioinfok verinf = FakUserInterface.VerzioInfok["C"];
            //if(verinf.VersionArray.Length>1)
            //    FakUserInterface.
            Tesztlap.ShowKukucs();
        }

        private void teszt_VisibleChanged(object sender, EventArgs e)
        {
            if (teszt.Visible && !treeView1.Enabled)
            {
                treeView1.Enabled = true;
                panel1.Enabled = true;
                if (AktivControl.AktivControl != null && AktivControl.AktivControl.Name == "Formvezerles" && AktivControl.AktivControl.AktivControl != null)
                {
                    AktivControl.AktivControl.AktivControl.UjTag = true;
                    AktivControl.AktivControl.AktivControl.AltalanosInit();
                }
                SetFocus();
            }
        }

        private void kilep_Click(object sender, EventArgs e)
        {
            MainForm.Close();
        }

        private void eredm_Click(object sender, EventArgs e)
        {

            if (Eredmlap  == null)
                Eredmlap = new KukucsEredm(FakUserInterface, panel2, eredmfo);
            panel1.Enabled = false;
            treeView1.Enabled = false;
            eredmfo.Visible = false;
            Eredmlap.ShowKukucs();
        }

        private void eredmfo_VisibleChanged(object sender, EventArgs e)
        {

            if (eredmfo.Visible && !treeView1.Enabled)
            {
                treeView1.Enabled = true;
                panel1.Enabled = true;
                SetFocus();
            }
        }

        private void help_Click(object sender, EventArgs e)
        {
            string azon = ((ToolStripMenuItem)sender).Tag.ToString();
            FakUserInterface.ShowHelp(azon, true, MainForm);

        }

        private void cegall_Click(object sender, EventArgs e)
        {
            if (Cegallapotok == null)
                Cegallapotok  = new CegAllapotok(FakUserInterface, panel2, eredmfo, MainForm);
            panel1.Enabled = false;
            treeView1.Enabled = false;
            eredmfo.Visible = false;
//            cegall.Visible = false;
            Cegallapotok.ShowCegall();

        }


        //private void cegall_VisibleChanged(object sender, EventArgs e)
        //{
        //    if (cegall.Visible && !treeView1.Enabled)
        //    {
        //        treeView1.Enabled = true;
        //        panel1.Enabled = true;
        //        SetFocus();
        //    }
        //}

    }
}
