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
    public partial class Kukucs : Base
    {
        Panel Panel;
        Control[] controlok;
        ToolStripMenuItem Teszt;
        public Tablainfo koltsfocsopinfo;
        public Tablainfo koltsalcsopinfo;
        public Tablainfo koltscsopinfo;
        public Tablainfo koltsegkodok;
        public Tablainfo termfocsopinfo;
        public Tablainfo termalcsopinfo;
        public Tablainfo termcsopinfo;
        public Tablainfo termekkodok;
        public Tablainfo koltsfocsopalcsop;
        public Tablainfo koltsalcsopcsop;
        public Tablainfo koltscsopkod;
        public Tablainfo termfocsopalcsop;
        public Tablainfo termalcsopcsop;
        public Tablainfo termcsopkod;
        public Tablainfo afainfo;
        public Tablainfo bevsemainfo;
        public Tablainfo koltssemainfo;
        public Tablainfo fszazal;
        public Tablainfo partnerinfo;
        public Tablainfo alkalminfo;
        public string[] alkalmidk;
        public string[] alkalmszovegek;
        public string[] fszazalszovegek;
        public string[] termfocsopidk;
        public string[] termfocsopszovegek;
        public string[] termfocsopkodok;
        public string[] termalcsopidk;
        public string[] termalcsopszovegek;
        public string[] termalcsopkodok;
        public string[] termcsopidk;
        public string[] termcsopszovegek;
        public string[] termcsopkodok;
        public string[] termkodidk;
        public string[] termkodszovegek;
        public string[] termkodkodok;
        public string[] koltsfocsopidk;
        public string[] koltsfocsopszovegek;
        public string[] koltsfocsopkodok;
        public string[] koltsalcsopidk;
        public string[] koltsalcsopszovegek;
        public string[] koltsalcsopkodok;
        public string[] koltscsopidk;
        public string[] koltscsopszovegek;
        public string[] koltscsopkodok;
        public string[] koltskodidk;
        public string[] koltskodszovegek;
        public string[] koltskodkodok;
        private BevKiad[] bevkiadok = new BevKiad[2];
        private BevKiad AktualBevKiad;
        private MainAlap Main;
        public ArrayList koltsegkodidk;
        public ArrayList treenodeokarray;
        public Tablainfo[] osszestabinfo;
        public Tablainfo[] osszesosszef;
        public Verzioinfok verinf;
        public int belepoverzio = 0;
        public Kukucs(FakUserInterface fak, Panel panel, ToolStripMenuItem teszt, MainAlap main)
        {
            InitializeComponent();
            FakUserInterface = fak;
            Panel = panel;
            Teszt = teszt;
            Main = main;
            this.Dock = DockStyle.Fill;
        }
        public void ShowKukucs()
        {
            controlok = new Control[Panel.Controls.Count];
            for (int i = 0; i < Panel.Controls.Count; i++)
                controlok[i] = Panel.Controls[i];
            Panel.Controls.Clear();
            verinf=FakUserInterface.VerzioInfok["C"];
            belepoverzio = verinf.AktVerzioId;
            ShowKukucsIsmetel();
        }
        public void ShowKukucsIsmetel()
        {

            treeView1.Nodes.Clear();
            treeView2.Nodes.Clear();
            koltsfocsopinfo = FakUserInterface.GetKodtab("C", "Koltsfocsop");
            koltsalcsopinfo = FakUserInterface.GetKodtab("C", "Koltsalcsop");
            koltscsopinfo = FakUserInterface.GetBySzintPluszTablanev("C", "KOLTSEGCSOPORT");
            koltsegkodok = FakUserInterface.GetBySzintPluszTablanev("C", "KOLTSEGKOD");
            termekkodok = FakUserInterface.GetBySzintPluszTablanev("C", "TERMEKKOD");
            termfocsopinfo = FakUserInterface.GetKodtab("C", "Termfocsop");
            termalcsopinfo = FakUserInterface.GetKodtab("C", "Termalcsop");
            termcsopinfo = FakUserInterface.GetKodtab("C", "Termcsop");
            termekkodok = FakUserInterface.GetBySzintPluszTablanev("C", "TERMEKKOD");
            termfocsopalcsop = FakUserInterface.GetOsszef("C", "Termfocsopalcsop");
            termalcsopcsop = FakUserInterface.GetOsszef("C", "Termalcsopcsop");
            termcsopkod = FakUserInterface.GetOsszef("C", "Termcsopkod");
            koltsfocsopalcsop = FakUserInterface.GetOsszef("C", "Koltsfocsopalcsop");
            koltsalcsopcsop = FakUserInterface.GetOsszef("C", "Koltsalcsopcsop");
            koltscsopkod = FakUserInterface.GetOsszef("C", "Koltscsopkod");
            bevsemainfo = FakUserInterface.GetCsoport("C", "Termfeloszt");
            koltssemainfo = FakUserInterface.GetCsoport("C", "Feloszt");
            partnerinfo = FakUserInterface.GetBySzintPluszTablanev("C", "PARTNER");
//            kiajanlinfo = FakUserInterface.GetBySzintPluszTablanev("C", "KIAJANL");
            osszestabinfo = new Tablainfo[] { termfocsopinfo, termalcsopinfo, termcsopinfo, termekkodok, koltsfocsopinfo, koltsalcsopinfo, koltscsopinfo, koltsegkodok };
            osszesosszef = new Tablainfo[] { termfocsopalcsop, termalcsopcsop, termcsopkod, bevsemainfo, koltsfocsopalcsop, koltsalcsopcsop, koltscsopkod, koltssemainfo };
            //for (int i = 0; i < osszesosszef.Length; i++)
            //{
            //    Tablainfo egytabinfo = osszesosszef[i];
            //    Osszefinfo egyosszef = egytabinfo.Osszefinfo;
            //    egyosszef.InitKell = true;
            //    egyosszef.OsszefinfoInit();
            //    egyosszef.Osszefinfotolt();
            //}
            afainfo = FakUserInterface.GetKodtab("C", "Afa");
            fszazal = FakUserInterface.GetKodtab("C", "Fszazal");
            fszazalszovegek = FakUserInterface.GetTartal(fszazal, "SZOVEG");
            termfocsopidk = FakUserInterface.GetTartal(termfocsopinfo, "SORSZAM");
            termfocsopszovegek = FakUserInterface.GetTartal(termfocsopinfo, "SZOVEG");
            termfocsopkodok = FakUserInterface.GetTartal(termfocsopinfo, "KOD");
            koltsfocsopidk = FakUserInterface.GetTartal(koltsfocsopinfo, "SORSZAM");
            koltsfocsopszovegek = FakUserInterface.GetTartal(koltsfocsopinfo, "SZOVEG");
            koltsfocsopkodok = FakUserInterface.GetTartal(koltsfocsopinfo, "KOD");
            alkalminfo = FakUserInterface.GetBySzintPluszTablanev("C", "DOLGOZOK");
            alkalmidk = null;
            alkalmszovegek = null;
            comboBox12.Items.Clear();
            if (UserParamTabla.VanDolgozo)
            {
                alkalmidk = FakUserInterface.GetTartal(alkalminfo, alkalminfo.IdentityColumnName);
                alkalmszovegek = FakUserInterface.GetTartal(alkalminfo, "SZOVEG");
                comboBox12.Items.AddRange(alkalmszovegek);
            }
            TreeNode termfocsopnode = null;
            TreeNode koltsfocsopnode = null;
            koltsegkodidk = new ArrayList();
            treenodeokarray = new ArrayList();
            string savsort = "";
            if (termfocsopidk == null)
            {
                termfocsopnode = null;
            }
            else //(termfocsopidk != null)
            {
                for (int i = 0; i < termfocsopidk.Length; i++)
                {
                    string egytermfocsopid = termfocsopidk[i];
                    string egytermfocsopszov = termfocsopszovegek[i];
                    string egytermfocsopkod = termfocsopkodok[i];
                    TreeNode focsopnode = new TreeNode(egytermfocsopszov + " (" + egytermfocsopkod + ")");
                    focsopnode.Tag = egytermfocsopid;
                    focsopnode.Name = egytermfocsopkod;
                    if (i == 0)
                        termfocsopnode = focsopnode;
                    treeView1.Nodes.Add(focsopnode);
                    savsort = termfocsopalcsop.DataView.Sort;
                    termfocsopalcsop.DataView.Sort = "SORREND";
                    termalcsopidk = FakUserInterface.GetTartal(termfocsopalcsop, "SORSZAM2", "SORSZAM1", egytermfocsopid);
                    termfocsopalcsop.DataView.Sort = savsort;
                    if (termalcsopidk != null && termalcsopidk.Length != 0)
                    {
                        termalcsopszovegek = FakUserInterface.GetTartal(termalcsopinfo, "SZOVEG", "SORSZAM", termalcsopidk);
                        termalcsopkodok = FakUserInterface.GetTartal(termalcsopinfo, "KOD", "SORSZAM", termalcsopidk);
                        for (int j = 0; j < termalcsopidk.Length; j++)
                        {
                            string egytermalcsopid = termalcsopidk[j];
                            string egytermalcsopszov = termalcsopszovegek[j];
                            string egytermalcsopkod = termalcsopkodok[j];
                            TreeNode alcsopnode = new TreeNode(egytermalcsopszov + " (" + egytermalcsopkod + ")");
                            alcsopnode.Tag = egytermalcsopid;
                            alcsopnode.Name = egytermalcsopkod;
                            focsopnode.Nodes.Add(alcsopnode);
                            savsort = termalcsopcsop.DataView.Sort;
                            termalcsopcsop.DataView.Sort = "SORREND";
                            termcsopidk = FakUserInterface.GetTartal(termalcsopcsop, "SORSZAM2", "SORSZAM1", egytermalcsopid);
                            termalcsopcsop.DataView.Sort = savsort;
                            if (termcsopidk != null && termcsopidk.Length != 0)
                            {
                                termcsopszovegek = FakUserInterface.GetTartal(termcsopinfo, "SZOVEG", "SORSZAM", termcsopidk);
                                termcsopkodok = FakUserInterface.GetTartal(termcsopinfo, "KOD", "SORSZAM", termcsopidk);
                                for (int k = 0; k < termcsopidk.Length; k++)
                                {
                                    string egytermcsopid = termcsopidk[k];
                                    string egytermcsopszov = termcsopszovegek[k];
                                    string egytermcsopkod = termcsopkodok[k];
                                    TreeNode csopnode = new TreeNode(egytermcsopszov + " (" + egytermcsopkod + ")");
                                    csopnode.Tag = egytermcsopid;
                                    csopnode.Name = egytermcsopkod;
                                    alcsopnode.Nodes.Add(csopnode);
                                    savsort = termcsopkod.DataView.Sort;
                                    termcsopkod.DataView.Sort = "SORREND";
                                    termkodidk = FakUserInterface.GetTartal(termcsopkod, "SORSZAM2", "SORSZAM1", egytermcsopid);
                                    termcsopkod.DataView.Sort = savsort;
                                    if (termkodidk != null && termkodidk.Length != 0)
                                    {
                                        termkodszovegek = FakUserInterface.GetTartal(termekkodok, "SZOVEG", "TERMEKKOD_ID", termkodidk);
                                        termkodkodok = FakUserInterface.GetTartal(termekkodok, "KOD", "TERMEKKOD_ID", termkodidk);
                                        for (int l = 0; l < termkodidk.Length; l++)
                                        {
                                            string egykodid = termkodidk[l];
                                            string egyszov = termkodszovegek[l];
                                            string egykod = termkodkodok[l];
                                            TreeNode kodnode = new TreeNode(egyszov + " (" + egykod + ")");
                                            kodnode.Tag = egykodid;
                                            kodnode.Name = egykod;
                                            csopnode.Nodes.Add(kodnode);
                                            koltskodszovegek = FakUserInterface.GetTartal(koltsegkodok, "SZOVEG", "TERMEKKOD_ID", egykodid);
                                            koltskodkodok = FakUserInterface.GetTartal(koltsegkodok, "KOD", "TERMEKKOD_ID", egykodid);
                                            string[] idk = FakUserInterface.GetTartal(koltsegkodok, koltsegkodok.IdentityColumnName, "TERMEKKOD_ID", egykodid);
                                            if (koltskodszovegek != null && koltskodszovegek.Length != 0)
                                            {
                                                for (int m = 0; m < koltskodszovegek.Length; m++)
                                                {
                                                    TreeNode kodnode1 = new TreeNode(koltskodszovegek[m] + " (" + koltskodkodok[m] + ")");
                                                    kodnode1.Tag = idk[m];
                                                    kodnode1.Name = koltskodkodok[m];
                                                    //                                                    eredmkodnode.Nodes.Add(kodnode1);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (koltsfocsopidk == null)
            {
                koltsfocsopnode = null;
            }
            else    //if (koltsfocsopidk != null)
            {
                for (int i = 0; i < koltsfocsopidk.Length; i++)
                {
                    string egykoltsfocsopid = koltsfocsopidk[i];
                    string egykoltsfocsopszov = koltsfocsopszovegek[i];
                    string egykoltsfocsopkod = koltsfocsopkodok[i];
                    TreeNode focsopnode = new TreeNode(egykoltsfocsopszov + " (" + egykoltsfocsopkod + ")");
                    focsopnode.Tag = egykoltsfocsopid;
                    focsopnode.Name = egykoltsfocsopkod;
                    if (i == 0)
                        koltsfocsopnode = focsopnode;
                    treeView2.Nodes.Add(focsopnode);
                    savsort = koltsfocsopalcsop.DataView.Sort;
                    koltsfocsopalcsop.DataView.Sort = "SORREND";
                    koltsalcsopidk = FakUserInterface.GetTartal(koltsfocsopalcsop, "SORSZAM2", "SORSZAM1", egykoltsfocsopid);
                    koltsfocsopalcsop.DataView.Sort = savsort;
                    if (koltsalcsopidk != null && koltsalcsopidk.Length != 0)
                    {
                        koltsalcsopszovegek = FakUserInterface.GetTartal(koltsalcsopinfo, "SZOVEG", "SORSZAM", koltsalcsopidk);
                        koltsalcsopkodok = FakUserInterface.GetTartal(koltsalcsopinfo, "KOD", "SORSZAM", koltsalcsopidk);
                        for (int j = 0; j < koltsalcsopidk.Length; j++)
                        {
                            string egykoltsalcsopid = koltsalcsopidk[j];
                            string egykoltsalcsopszov = koltsalcsopszovegek[j];
                            string egykoltsalcsopkod = koltsalcsopkodok[j];
                            TreeNode alcsopnode = new TreeNode(egykoltsalcsopszov + "(" + egykoltsalcsopkod + ")");
                            alcsopnode.Tag = egykoltsalcsopid;
                            alcsopnode.Name = egykoltsalcsopkod;
                            focsopnode.Nodes.Add(alcsopnode);
                            savsort = koltsalcsopcsop.DataView.Sort;
                            koltsalcsopcsop.DataView.Sort = "SORREND";
                            koltscsopidk = FakUserInterface.GetTartal(koltsalcsopcsop, "SORSZAM2", "SORSZAM1", egykoltsalcsopid);
                            koltsalcsopcsop.DataView.Sort = savsort;
                            if (koltscsopidk != null && koltscsopidk.Length != 0)
                            {
                                koltscsopszovegek = FakUserInterface.GetTartal(koltscsopinfo, "SZOVEG", "KOLTSEGCSOPORT_ID", koltscsopidk);
                                koltscsopkodok = FakUserInterface.GetTartal(koltscsopinfo, "KOD", "KOLTSEGCSOPORT_ID", koltscsopidk);
                                for (int k = 0; k < koltscsopidk.Length; k++)
                                {
                                    string egykoltscsopid = koltscsopidk[k];
                                    string egykoltscsopszov = koltscsopszovegek[k];
                                    string egykoltscsopkod = koltscsopkodok[k];
                                    TreeNode csopnode = new TreeNode(egykoltscsopszov + " (" + egykoltscsopkod + ")");
                                    csopnode.Tag = egykoltscsopid;
                                    csopnode.Name = egykoltscsopkod;
                                    alcsopnode.Nodes.Add(csopnode);
                                    savsort = koltscsopkod.DataView.Sort;
                                    koltscsopkod.DataView.Sort = "SORREND";
                                    koltskodidk = FakUserInterface.GetTartal(koltscsopkod, "SORSZAM2", "SORSZAM1", egykoltscsopid);
                                    koltscsopkod.DataView.Sort = savsort;
                                    if (koltskodidk != null && koltskodidk.Length != 0)
                                    {
                                        koltskodszovegek = FakUserInterface.GetTartal(koltsegkodok, "SZOVEG", "KOLTSEGKOD_ID", koltskodidk);
                                        koltskodkodok = FakUserInterface.GetTartal(koltsegkodok, "KOD", "KOLTSEGKOD_ID", koltskodidk);
                                        for (int l = 0; l < koltskodkodok.Length; l++)
                                        {
                                            string egykod = koltskodkodok[l];
                                            string egyszov = koltskodszovegek[l];
                                            TreeNode kodnode = new TreeNode(egyszov + " (" + egykod + ")");
                                            koltsegkodok.DataView.RowFilter = "KOD = '" + egykod + "'";
                                            string egyid = koltsegkodok.DataView[0].Row["KOLTSEGKOD_ID"].ToString();
                                            koltsegkodok.DataView.RowFilter = "";
                                            kodnode.Tag = egyid;//koltskodidk[l];
                                            kodnode.Name = egykod;
                                            csopnode.Nodes.Add(kodnode);
                                            int arindex = koltsegkodidk.IndexOf(egyid);//koltskodidk[l]);
                                            ArrayList ar;
                                            if (arindex == -1)
                                            {
                                                koltsegkodidk.Add(egyid);//koltskodidk[l]);
                                                ar = new ArrayList();
                                                ar.Add(kodnode);
                                                treenodeokarray.Add(ar);
                                            }
                                            else
                                            {
                                                ar = (ArrayList)treenodeokarray[arindex];
                                                ar.Add(kodnode);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            bevkiadok[0] = new BevKiad(this, treeView1, toolStrip1, groupBox3, textBox1, textBox2, bevafacombo, comboBox2, comboBox9, bevfelosztszazal, comboBox11,null);
            bevkiadok[1] = new BevKiad(this, treeView2, toolStrip2, groupBox4, textBox3, textBox4, koltsafacombo, bevetelkodcombo, lehetsemacombo, koltsfelosztszazal, kolsfelosztandocombo,comboBox12);
            bevkiadok[0].MasikBevKiad = bevkiadok[1];
            bevkiadok[0].MasikElemikodInfo = bevkiadok[1].ElemikodInfo;
            bevkiadok[1].MasikBevKiad = bevkiadok[0];
            bevkiadok[1].MasikElemikodInfo = bevkiadok[0].ElemikodInfo;
            bevkiadok[0].SetAktualNode(termfocsopnode);
            bevkiadok[1].SetAktualNode(koltsfocsopnode);
//            treeView1.ExpandAll();
//            treeView2.ExpandAll();
            Panel.Controls.Add(this);
            bool irhat =  UserParamTabla.CegSzarmazekosJogosultsag == Base.HozferJogosultsag.Irolvas && partnerinfo.Azonositok.Jogszintek[(int)UserParamTabla.AktualCegInformacio.KezeloiSzint] == Base.HozferJogosultsag.Irolvas;
            bool lezartverzio = koltsfocsopinfo.LezartVersion;
            toolStrip1.Visible = irhat && !lezartverzio && !UserParamTabla.LezartCeg;
            toolStrip2.Visible = toolStrip1.Visible;
//            splitContainer2.Visible = irhat && !lezartverzio && !UserParamTabla.LezartCeg;
            verinf = FakUserInterface.VerzioInfok["C"];
            ujverzio.Visible = irhat && lezartverzio && verinf.LastVersionId > koltsfocsopinfo.LastVersionId;
            elozoverzio.Visible = koltsfocsopinfo.AktVerzioId > 1;
            kovetkezoverzio.Visible = koltsfocsopinfo.AktVerzioId < koltsfocsopinfo.LastVersionId;
            bevkiadok[0].KodBox.Focus();
        }
        private void rendben_Click(object sender, EventArgs e)
        {
            if (Semaellenorzes())
                return;
            Panel.Controls.Clear();
            for (int i = 0; i < controlok.Length; i++)
                Panel.Controls.Add(controlok[i]);

            if (koltsfocsopinfo.AktVerzioId != belepoverzio)
            {
                do
                {
                    if (belepoverzio < koltsfocsopinfo.AktVerzioId)
                        elozo();
                    else
                        kovetkezo();

                    if (belepoverzio == koltsfocsopinfo.AktVerzioId)
                        break;
                } while (true);
            }
            //FakUserInterface.OpenProgress("Türelem!");
            //FakUserInterface.Cegadatok(UserParamTabla.AktualCegInformacio.CegConnection, UserParamTabla.AktualCegInformacio.CegNev,
            //    new DateTime[] { UserParamTabla.SzamlaDatumtol, UserParamTabla.SzamlaDatumig},true);
            //FakUserInterface.CloseProgress();
            Teszt.Visible = true;
        }
        private bool Semaellenorzes()
        {
            bool hiba = false;
            string hibaszov = "";
            string savsort = "";
            Tablainfo[] semak = new Tablainfo[] { bevsemainfo, koltssemainfo };
            Tablainfo[] csoport_kod = new Tablainfo[] { termcsopkod, koltscsopkod };
            Tablainfo[] csoportok = new Tablainfo[] { termcsopinfo, koltscsopinfo };

            for (int i = 0; i < semak.Length; i++)
            {
                Tablainfo semainfo = semak[i];
                Tablainfo csoportinfo = csoportok[i];
                Tablainfo csoportkodinfo = csoport_kod[i];
                string csoport_kodid;
                int felosztott = 0;
                for(int j=0;j<csoportinfo.DataView.Count;j++)
                {
                    felosztott=0;
                    DataRow row = csoportinfo.DataView[j].Row;
                    string egyid = row[csoportinfo.IdentityColumnName].ToString();
                    string egyszov = row["SZOVEG"].ToString();
                    string[] osszefidk = FakUserInterface.GetTartal(csoportkodinfo,"SORSZAM","SORSZAM1",egyid);
                    if (osszefidk != null)
                    {
                        felosztott = 0;
                        for (int k = 0; k < osszefidk.Length; k++)
                        {
                            string[] szazalidk = FakUserInterface.GetTartal(semainfo, "SORSZAM2", "SORSZAM1", osszefidk[k]);
                            if (szazalidk != null)
                            {
                                string kodid = FakUserInterface.GetTartal(fszazal, "KOD", fszazal.IdentityColumnName, szazalidk[0])[0];
                                felosztott += Convert.ToInt32(kodid);
                            }
                        }
                    }
                    if (felosztott != 0 && felosztott != 100)
                    {
                        hiba = true;
                        hibaszov += egyszov + " felosztása hibás:" + felosztott.ToString() + "%\n";
                    }
                }
            }
            if (hiba)
                FakPlusz.MessageBox.Show(hibaszov);
            return hiba;
        }
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeViewHitTestInfo hittest = treeView1.HitTest(e.X, e.Y);
            if (e.Button == MouseButtons.Right)
            {
                if (e.Node != null)
                {
                    if (e.Node.IsExpanded)
                        e.Node.Collapse(false);
                    else
                        e.Node.ExpandAll();
                }
            }
            if (e.Button == MouseButtons.Left)
            {
                if (hittest.Location == TreeViewHitTestLocations.Label)
                {
                    AktualBevKiad = bevkiadok[0];
                    AktualBevKiad.Valtozott = false;
                    AktualBevKiad.Hibas = false;
                    AktualBevKiad.SetAktualNode(e.Node);
//                    FakUserInterface.EventTilt = true;
                    AktualBevKiad.MasikBevKiad.GroupBox.BackColor = FakUserInterface.InaktivControlBackColor;
         
                    if (AktualBevKiad.MasikBevKiad.AktivControl != null)
                        AktualBevKiad.MasikBevKiad.AktivControl.BackColor = FakUserInterface.InaktivInputBackColor;
 //                   AktualBevKiad.KodBox.Focus();
                }
            }
        }

        private void treeView2_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeViewHitTestInfo hittest = treeView2.HitTest(e.X, e.Y);
            if (e.Button == MouseButtons.Right)
            {
                if (e.Node != null)
                {
                    if (e.Node.IsExpanded)
                        e.Node.Collapse(false);
                    else
                        e.Node.ExpandAll();
                }
            }
            if (e.Button == MouseButtons.Left)
            {
                if (hittest.Location == TreeViewHitTestLocations.Label)
                {
                    AktualBevKiad = bevkiadok[1];
                    AktualBevKiad.Valtozott = false;
                    AktualBevKiad.Hibas = false;
                    AktualBevKiad.SetAktualNode(e.Node);
//                    FakUserInterface.EventTilt = true;
                    AktualBevKiad.MasikBevKiad.GroupBox.BackColor = FakUserInterface.InaktivControlBackColor;
                    if (AktualBevKiad.MasikBevKiad.AktivControl != null)
                        AktualBevKiad.MasikBevKiad.AktivControl.BackColor = FakUserInterface.InaktivInputBackColor;
                    //                   AktualBevKiad.KodBox.Focus();
                }
            }
        }

        private void help_Click(object sender, EventArgs e)
        {
            string azon = ((ToolStripMenuItem)sender).Tag.ToString();
            FakUserInterface.ShowHelp(azon, true, Main);

        }

        private void combo_DropDownClosed(object sender, EventArgs e)
        {
            ComboBox combo = (ComboBox)sender;
            string name = combo.Name;
            bool nemlehetures = combo.Enabled && !name.Contains("felosztszazal");
            FakUserInterface.ErrorProvider.SetError(combo, "");
            if (combo.SelectedIndex != -1)
            {
                string szov = combo.Items[combo.SelectedIndex].ToString();
                if (combo.Text != szov)
                    AktualBevKiad.Valtozott = true;
                if (combo.Name == "kolsfelosztandocombo")  // felosztando
                {
                    if (combo.Enabled)
                    {
                        AktualBevKiad.ElemikodInfo.DataView.RowFilter = "SZOVEG = '" + szov + "'";
                        DataRow row = AktualBevKiad.ElemikodInfo.DataView[0].Row;
                        AktualBevKiad.ElemiKoltsegMasolando = row;// AktualBevKiad.ElemikodInfo.DataView[0].Row;
                        AktualBevKiad.ElemikodInfo.DataView.RowFilter = "";
                        AktualBevKiad.AfaCombo.SelectedIndex = AktualBevKiad.AfaCombo.Items.IndexOf(row["AFA_ID_K"].ToString());
                        AktualBevKiad.BevetelKodCombo.SelectedIndex = AktualBevKiad.BevetelKodCombo.Items.IndexOf(row["TERMEKKOD_ID_K"].ToString()
                            );
                    }
                }
                else if (combo.Name == "comboBox12")
                {
                }
            }
            else if (nemlehetures)
            {
                AktualBevKiad.Hibas = true;
                FakUserInterface.ErrorProvider.SetError(combo, "Nem lehet üres!");
            }
            else
                AktualBevKiad.Valtozott = combo.Enabled;
            AktualBevKiad.ToolStripButtonokEnable();
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {
            AktualBevKiad = bevkiadok[0];
            AktualBevKiad.GroupBox_Enter();
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {
            AktualBevKiad = bevkiadok[1];
            AktualBevKiad.GroupBox_Enter();
        }

        private void textbox_Enter(object sender, EventArgs e)
        {
            AktualBevKiad.TextBox_Enter((TextBox)sender);
        }

        private void textbox_Leave(object sender, EventArgs e)
        {
            TextBox box = (TextBox)sender;
            FakUserInterface.ErrorProvider.SetError(box, "");

 //           if (FakUserInterface.EventTilt)
 //               FakUserInterface.EventTilt = false;
 //           else
                AktualBevKiad.TextBox_Leave((TextBox)sender);
        }

        private void combobox_Enter(object sender, EventArgs e)
        {
            AktualBevKiad.ComboBox_Enter((ComboBox)sender);

        }

        private void combobox_Leave(object sender, EventArgs e)
        {
            AktualBevKiad.ComboBox_Leave((ComboBox)sender);

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            AktualBevKiad = bevkiadok[0];
            AktualBevKiad.ItemClicked(e.ClickedItem);
        }

        private void toolStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            AktualBevKiad = bevkiadok[1];
            AktualBevKiad.ItemClicked(e.ClickedItem);
        }

        private void ujverzio_Click(object sender, EventArgs e)
        {
            if (partnerinfo.LastVersionId == verinf.LastVersionId)
                partnerinfo.DeleteLastVersion();
            for (int i = 0; i < osszestabinfo.Length; i++)
            {
                Tablainfo egytabinfo = osszestabinfo[i];
                egytabinfo.CreateNewVersion();
            }
            for (int i = 0; i < osszesosszef.Length; i++)
            {
                Tablainfo egytabinfo = osszesosszef[i];
                egytabinfo.CreateNewVersion();
            }
            partnerinfo.CreateNewVersion();
            ShowKukucsIsmetel();
        }

        private void elozoverzio_Click(object sender, EventArgs e)
        {
            elozo();
            ShowKukucsIsmetel();
        }
        private void elozo()
        {
//            afainfo.ElozoVerzio();
 //           kiajanlinfo.ElozoVerzio();
            for (int i = 0; i < osszestabinfo.Length; i++)
            {
                Tablainfo egytabinfo = osszestabinfo[i];
                egytabinfo.ElozoVerzio();
            }
            for (int i = 0; i < osszesosszef.Length; i++)
            {
                Tablainfo egytabinfo = osszesosszef[i];
                egytabinfo.ElozoVerzio();
                if (egytabinfo.Osszefinfo.InitKell)
                    egytabinfo.Osszefinfo.OsszefinfoInit();
                else if (egytabinfo.Osszefinfo.TolteniKell)
                    egytabinfo.Osszefinfo.Osszefinfotolt();
            }
            //for (int i = 0; i < osszestabinfo.Length; i++)
            //{
            //    Tablainfo egytabinfo = osszestabinfo[i];
            //    egytabinfo.ElozoVerzio();
            //}
//            kiajanlinfo.ElozoVerzio();
            partnerinfo.ElozoVerzio();
        }
        private void kovetkezoverzio_Click(object sender, EventArgs e)
        {
            kovetkezo();
            ShowKukucsIsmetel();
        }
        private void kovetkezo()
        {
//            afainfo.KovetkezoVerzio();
//            kiajanlinfo.KovetkezoVerzio();
            int aktverid = FakUserInterface.VerzioInfok["C"].AktVerzioId;
            for (int i = 0; i < osszesosszef.Length; i++)
            {
                Tablainfo egytabinfo = osszesosszef[i];
                //if (egytabinfo.LastVersionId < aktverid + 1)
                //    egytabinfo.CreateNewVersion();
                egytabinfo.KovetkezoVerzio();
                if (egytabinfo.Osszefinfo.InitKell)
                    egytabinfo.Osszefinfo.OsszefinfoInit();
                else if (egytabinfo.Osszefinfo.TolteniKell)
                    egytabinfo.Osszefinfo.Osszefinfotolt();
            }
            for (int i = 0; i < osszestabinfo.Length; i++)
            {
                Tablainfo egytabinfo = osszestabinfo[i];
                egytabinfo.KovetkezoVerzio();
            }
            partnerinfo.KovetkezoVerzio();
        }
    }
    public class BevKiad
    {
        public BevKiad MasikBevKiad;
        public TreeView TreeView;
        public TreeNode AktualNode;
        public int AktualNodePos;
        public int EredetiSzint;
        public TreeNode EredetiNode;
        public TreeNode ParentNode;
        public TreeNode ParentParentNode;
        public TreeNode ParentParentParentNode;
        public ToolStrip ToolStrip;
        public GroupBox GroupBox;
        public TextBox KodBox;
        public TextBox SzovegBox;
        public ComboBox AfaCombo;
        public ComboBox BevetelKodCombo;
        public ComboBox FelosztCombo ;
        public ComboBox FelosztSzazalCombo ;
        public ComboBox FelosztandoKoltsegCombo ;
        public ComboBox AlkalmazottiCombo;
        public string[] FeloszthatoKoltsegidk;
        public Control AktivControl = null;
        public Kukucs Kukucs;
        public string Fajta;
        public FakUserInterface FakUserInterface;
        public Tablainfo ElemikodInfo;
        public Tablainfo MasikElemikodInfo = null;
        public Tablainfo AfaInfo;
        public Tablainfo SemaInfo;
        public Tablainfo Focsopinfo;
        public Tablainfo Alcsopinfo;
        public Tablainfo Csopinfo;
        public Tablainfo FocsopAlcsopinfo;
        public Tablainfo AlcsopCsopinfo;
        public Tablainfo CsopKodInfo;
        public int AktualSzint = 0;
        public bool Uj;
        public bool OkVolt;
        public bool Ele;
        public bool Moge;
        public bool Kellfeloszt;
        public bool Mostfeloszt;
        public bool Felosztott;
        public string AktualisFelosztasiSzazalek;
        public string AktualisFelosztasiId;
        public string AktualisId;
        public string AktualisCsopkodInfoId;
        public string EddigFelosztott = "";
        private bool valtozott=false;
        public bool Valtozott
        {
            get { return valtozott; }
            set
            {
                valtozott = value;
                if (value)
                {
                    if (!GroupBox.Text.Contains("*"))
                        GroupBox.Text += "*";
                }
                else
                {
                    int pos = GroupBox.Text.IndexOf("*");
                    if (pos != -1)
                        GroupBox.Text = GroupBox.Text.Remove(pos, 1);
                }
            }
        }
        private bool hibas = false;
        public bool Hibas
        {
            get { return hibas; }
            set
            {
                hibas = value;
                if (value)
                {
                    if (!GroupBox.Text.Contains("!"))
                        GroupBox.Text += "!";
                }
                else
                {
                    int pos = GroupBox.Text.IndexOf("!");
                    if (pos != -1)
                        GroupBox.Text = GroupBox.Text.Remove(pos, 1);
                }
            }
        }
        public ToolStripItemCollection buttonok = null;
        public string[] buttonnevek = null;
        public string[] rovidnevek = null;
        public string[] csopszoveg = new string[] { "Föcsoport", "Alcsoport", "Csoport", "Elemi kód" };
        public string[] termkodszovegek;
        public string[] termkodkodok;
        public string Szoveg;
        public bool Lehetsema = false;
        public DataRow ElemiKoltsegMasolando = null;
        public TreeNode MasolandoNode;
        public BevKiad(Kukucs kukucs,TreeView treeview, ToolStrip toolstrip, GroupBox  csopbox, TextBox kodbox, TextBox szovegbox, ComboBox afacombo,ComboBox bevetelkodcombo, ComboBox felosztcombo,ComboBox felosztszazalcombo, ComboBox felosztandokoltsegcombo,ComboBox alkalmcombo)
        {
            Kukucs = kukucs;
            FakUserInterface = kukucs.FakUserInterface;
            TreeView = treeview;
            ToolStrip = toolstrip;
            buttonok = ToolStrip.Items;
            buttonnevek = new string[buttonok.Count];
            rovidnevek = new string[buttonok.Count];
            for (int i = 0; i < buttonok.Count; i++)
            {
                ToolStripItem egycont =buttonok[i];
                buttonnevek[i] = egycont.Name;
                rovidnevek[i] = egycont.Name.Substring(0, egycont.Name.Length - 1);
            }
            GroupBox  = csopbox;
            Fajta = csopbox.Tag.ToString();
            FelosztandoKoltsegCombo = felosztandokoltsegcombo;
            if (Fajta == "B")
            {
                ElemikodInfo = kukucs.termekkodok;
                Focsopinfo = kukucs.termfocsopinfo;
                Alcsopinfo = kukucs.termalcsopinfo;
                Csopinfo = kukucs.termcsopinfo;
                FocsopAlcsopinfo = kukucs.termfocsopalcsop;
                AlcsopCsopinfo = kukucs.termalcsopcsop;
                CsopKodInfo = kukucs.termcsopkod;
                SemaInfo = kukucs.bevsemainfo;
                AlkalmazottiCombo = null;
            }
            else
            {
                ElemikodInfo = kukucs.koltsegkodok;
                Focsopinfo = kukucs.koltsfocsopinfo;
                Alcsopinfo = kukucs.koltsalcsopinfo;
                Csopinfo = kukucs.koltscsopinfo;
                FocsopAlcsopinfo = kukucs.koltsfocsopalcsop;
                AlcsopCsopinfo = kukucs.koltsalcsopcsop;
                CsopKodInfo = kukucs.koltscsopkod;
                SemaInfo = kukucs.koltssemainfo;
                AlkalmazottiCombo = alkalmcombo;
                FeloszthatoKoltsegidkModosit();
            }
            KodBox = kodbox;
            SzovegBox = szovegbox;
            afacombo.Items.Clear();
            AfaCombo = afacombo;
            AfaInfo = kukucs.afainfo;
            string[] afaszovegek = FakUserInterface.GetTartal(AfaInfo, "SZOVEG");
            AfaCombo.Items.AddRange(afaszovegek);
            KodBox.Text = "";
            SzovegBox.Text = "";
            AfaCombo.Enabled = false;
            AfaCombo.SelectedIndex = -1;
            BevetelKodCombo = bevetelkodcombo;
            BevetelKodCombo.Items.Clear();
            BevetelKodCombo.Enabled = false;
            FelosztCombo = felosztcombo;
            FelosztCombo.SelectedIndex = -1;
            FelosztCombo.Enabled = false;
            FelosztSzazalCombo = felosztszazalcombo;
            FelosztSzazalCombo.Items.Clear();
            FelosztSzazalCombo.Items.AddRange(Kukucs.fszazalszovegek);
        }
        public void SetAktualNode(TreeNode node)
        {
            string hibaszov = "";
            ElemiKoltsegMasolando = null;
            KodBox.Enabled = true;
            SzovegBox.Enabled = true;
            if (MasolandoNode != null)
            {
                if (node != null)
                {
                    if (node.Parent == null || node.Parent.Parent == null || node.Parent.Parent.Parent == null)
                    {
                        hibaszov = " Elemi költség csak felosztható\nelemi költségek közé kerülhet!";
                        FakPlusz.MessageBox.Show(hibaszov);
                        return;
                    }
                }
            }
            string id;
            AktualNode = node;
            if (AktualNode == null)
            {
                if (ParentNode == null)
                {
                    AktualSzint = 0;
                }
            }
            KodBox.Text = "";
            SzovegBox.Text = "";
            Szoveg = "";
            AfaCombo.SelectedIndex = -1;
            AfaCombo.Enabled = false;
            if (Fajta == "K")
            {
                BevetelKodCombo.Items.Clear();
                BevetelKodCombo.Enabled = false;
                BevetelKodCombo.Text = "";
                termkodszovegek = FakUserInterface.GetTartal(MasikElemikodInfo, "SZOVEG");
                termkodkodok = FakUserInterface.GetTartal(MasikElemikodInfo, "KOD");
            }
            FelosztCombo.SelectedIndex = -1;
            FelosztCombo.Enabled = false;
            FelosztCombo.Text = "";
            FelosztSzazalCombo.SelectedIndex = -1;
            FelosztSzazalCombo.Enabled = false;
            FelosztSzazalCombo.Text = "";
            if (FelosztandoKoltsegCombo != null)
            {
                FelosztandoKoltsegCombo.SelectedIndex = -1;
                FelosztandoKoltsegCombo.Text = "";
                FelosztandoKoltsegCombo.Enabled = false;
            }
            if (AktualNode != null)
            {
                TreeView.SelectedNode = node;
                ParentNode = AktualNode.Parent;
                if (ParentNode == null)
                    AktualNodePos = TreeView.Nodes.IndexOf(AktualNode);
                else
                    AktualNodePos = ParentNode.Nodes.IndexOf(AktualNode);
                AktualSzint = 0;
                if (ParentNode != null)
                {
                    AktualSzint = 1;
                    ParentParentNode = ParentNode.Parent;
                    if (ParentParentNode != null)
                    {
                        AktualSzint = 2;
                        ParentParentParentNode = ParentParentNode.Parent;
                        if (ParentParentParentNode != null)
                            AktualSzint = 3;
                    }
                }
                KodBox.Text = AktualNode.Name;
                int pos = AktualNode.Text.IndexOf("(");
                Szoveg = AktualNode.Text.Substring(0, pos).Trim();
                SzovegBox.Text = Szoveg;
            }
            Kellfeloszt = false;
            EddigFelosztott = "";
            int felosztott = 0;
            id = "";
            if (AktualNode != null)
                id = AktualNode.Tag.ToString();
            else if (EredetiNode != null && EredetiSzint == AktualSzint)
                id = EredetiNode.Tag.ToString();
            if (AktualSzint > 1)
            {
                Mostfeloszt = false;
                Felosztott = false;
                Kellfeloszt = FelosztassalKapcsolatos();
                if (Kellfeloszt)
                {
                    if (Fajta == "K" && Kellfeloszt && AktualSzint == 3)
                    {
                        AfaCombo.Enabled = false;
                        BevetelKodCombo.Enabled = false;
                        Mostfeloszt = AktualNode == null;
                        Felosztott = !Mostfeloszt;
                        if (Mostfeloszt)
                        {
                            FelosztandoKoltsegCombo.Enabled = true;
                            FelosztandoKoltsegCombo.SelectedIndex = -1;
                        }
                    }
                    if (id != "")
                    {
                        if (AktualSzint == 2)
                            CsopKodInfo.DataView.RowFilter = "SORSZAM1 = " + id;
                        else
                        {
                            string csopkod = ParentNode.Tag.ToString();
                            CsopKodInfo.DataView.RowFilter = "SORSZAM1 = " + csopkod + " AND SORSZAM2 = " + id;
                        }
                        string filt = "";
                        if (CsopKodInfo.DataView.Count != 0)
                        {
                            for (int i = 0; i < CsopKodInfo.DataView.Count; i++)
                            {
                                if (filt != "")
                                    filt += " OR ";
                                filt += "SORSZAM1 = " + CsopKodInfo.DataView[i].Row["SORSZAM"].ToString();
                            }
                            CsopKodInfo.DataView.RowFilter = "";
                            SemaInfo.DataView.RowFilter = filt;
                            DataView view = SemaInfo.DataView;
                            if (view != null)
                            {
                                for (int i = 0; i < view.Count; i++)
                                {
                                    string szazalid = view[i].Row["SORSZAM2"].ToString();
                                    if (szazalid != "0")
                                    {
                                        Kukucs.fszazal.DataView.RowFilter = "SORSZAM = " + szazalid;
                                        string egyfeloszt = Kukucs.fszazal.DataView[0].Row["SZOVEG"].ToString();
                                        felosztott += Convert.ToInt32(Kukucs.fszazal.DataView[0].Row["KOD"].ToString());
                                        if (AktualSzint == 3)
                                            FelosztSzazalCombo.SelectedIndex = FelosztSzazalCombo.Items.IndexOf(egyfeloszt);
                                        Kukucs.fszazal.DataView.RowFilter = "";
                                    }
                                }
                            }
                            SemaInfo.DataView.RowFilter = "";
                        }
                    }
                    if (!Mostfeloszt && (AktualSzint == 2 || AktualNode == null))
                        EddigFelosztott = " Összes felosztott:" + felosztott.ToString() + "%";
                }
            }
            KodBox.Visible = !Mostfeloszt;
            KodBox.Enabled = !Felosztott;
            SzovegBox.Visible = !Mostfeloszt;
            SzovegBox.Enabled = !Felosztott;
//            AfaCombo.Visible = !Mostfeloszt && AktualSzint == 3;
            AfaCombo.Enabled = !Felosztott && (!Kellfeloszt||Fajta=="B") && AktualSzint==3 ;
            if (!AfaCombo.Enabled)
                AfaCombo.SelectedIndex = -1;
            FakUserInterface.ErrorProvider.SetError(AfaCombo, "");
            if (BevetelKodCombo != null)
            {
//                BevetelKodCombo.Visible = !Mostfeloszt && AktualSzint == 3 && Fajta == "K";
                BevetelKodCombo.Enabled = !Felosztott && AktualSzint == 3 && Fajta == "K";
                FakUserInterface.ErrorProvider.SetError(BevetelKodCombo, "");
            }
            if(FelosztandoKoltsegCombo!=null)
                FakUserInterface.ErrorProvider.SetError(FelosztandoKoltsegCombo, "");
            if (AlkalmazottiCombo != null)
            {
                AlkalmazottiCombo.Enabled = !Felosztott && AktualSzint == 3 && UserParamTabla.VanDolgozo;
                FakUserInterface.ErrorProvider.SetError(AlkalmazottiCombo, "");
                if (AktualSzint != 3)
                    AlkalmazottiCombo.SelectedIndex = -1;
            }
            if(FelosztSzazalCombo!=null)
                FakUserInterface.ErrorProvider.SetError(FelosztSzazalCombo, "");

            if (AktualSzint == 3)
            {
                string kod;
                if (AktualNode == null)
                {
                    ElemikodInfo.ViewSorindex = -1;
                    Comboinfok comboinf=FakUserInterface.ComboInfok.ComboinfoKeres("SZCKAfa");
                    AfaCombo.SelectedIndex = 0;
                }
                else
                {
                    
                    id = FakUserInterface.GetTartal(ElemikodInfo, "AFA_ID", ElemikodInfo.IdentityColumnName, AktualNode.Tag.ToString())[0];
                    if(id=="0")
                        id=AfaInfo.DataView[0].Row["SORSZAM"].ToString();
                    kod = FakUserInterface.GetTartal(AfaInfo, "SZOVEG", "SORSZAM", id)[0];
                    AfaCombo.SelectedIndex = AfaCombo.Items.IndexOf(kod);
                }
                FelosztSzazalCombo.Enabled = Kellfeloszt;
                if (Fajta == "K")
                {
                    BevetelKodCombo.Items.Clear();
                    BevetelKodCombo.Items.AddRange(termkodszovegek);
                   
                    if (AktualNode == null)
                        BevetelKodCombo.SelectedIndex = 0;
                    else
                    {
                        id = FakUserInterface.GetTartal(ElemikodInfo, "TERMEKKOD_ID", ElemikodInfo.IdentityColumnName, AktualNode.Tag.ToString())[0];
                        kod = FakUserInterface.GetTartal(MasikElemikodInfo, "SZOVEG", MasikElemikodInfo.IdentityColumnName, id)[0];
                        BevetelKodCombo.SelectedIndex = BevetelKodCombo.Items.IndexOf(kod);
                    }
                    if (AlkalmazottiCombo.Visible && AlkalmazottiCombo.Enabled)
                    {
                        if (AktualNode != null)
                        {
                            id = FakUserInterface.GetTartal(ElemikodInfo, "DOLGOZO_ID", ElemikodInfo.IdentityColumnName, AktualNode.Tag.ToString())[0];
                            AlkalmazottiCombo.SelectedIndex= (new ArrayList(Kukucs.alkalmidk)).IndexOf(id);
                        }
                    }
                }
            }
            OkVolt = false;
            Hibas = false;
            KodBox.Focus();
            FakUserInterface.ErrorProvider.SetError(KodBox, "");
            FakUserInterface.ErrorProvider.SetError(SzovegBox, "");
            ToolStripButtonokEnable();
        }
        private bool FelosztassalKapcsolatos()
        {
            bool kellfeloszt = false;
            string csopid = "";
            string id = "";
            if (AktualNode != null && AktualSzint == 3)
                id = AktualNode.Tag.ToString();
            if (AktualSzint == 2)
            {
                if (Fajta == "K")
                {
                    FelosztCombo.Enabled = true;
                    if (AktualNode == null)
                        FelosztCombo.SelectedIndex = 1;
                    else
                    {
                        csopid = AktualNode.Tag.ToString();
                        Csopinfo.DataView.RowFilter = Csopinfo.IdentityColumnName + " = " + csopid;
                        if (Csopinfo.DataView[0].Row["SEMAE"].ToString() == "I")
                        {
                            FelosztCombo.SelectedIndex = 0;
                            if (AktualNode.Nodes.Count != 0)
                                kellfeloszt = true;
                        }
                        else
                            FelosztCombo.SelectedIndex = 1;
                        if (AktualNode.Nodes.Count != 0)
                            FelosztCombo.Enabled = false;
                        Csopinfo.DataView.RowFilter = "";
                    }
                }
                else if (AktualNode != null)
                    kellfeloszt = true;
            }
            else
            {
                TreeNode parent = null;
                TreeNode ujnode = AktualNode;
                if (AktualNode != null)
                    parent = ParentNode;
                if (ujnode == null)
                {
                    ujnode = EredetiNode;
                    if (EredetiNode != null)
                    {
                        if (EredetiSzint == AktualSzint)
                            parent = ujnode.Parent;
                        else
                            parent = ujnode;
                    }
                }
                csopid = parent.Tag.ToString();
                if (Fajta == "K")
                {
                    Csopinfo.DataView.RowFilter = Csopinfo.IdentityColumnName + " = " + csopid;
                    if (Csopinfo.DataView[0].Row["SEMAE"].ToString() == "I")
                    {
                        FelosztCombo.SelectedIndex = 0;
                        kellfeloszt = true;
                    }
                    else
                        FelosztCombo.SelectedIndex = 1;
                    Csopinfo.DataView.RowFilter = "";
                }
                else
                {
                    FelosztCombo.SelectedIndex = 0;
                    kellfeloszt = true;
                }
            }

            return kellfeloszt;
        }
        public void FeloszthatoKoltsegidkModosit()
        {
            FelosztandoKoltsegCombo.Items.Clear();
            FelosztandoKoltsegCombo.Text = "";
            ArrayList ar = new ArrayList();
            ArrayList idk = Kukucs.koltsegkodidk;
            ArrayList nodeok = Kukucs.treenodeokarray;
            if (idk.Count != 0)
            {
                for (int i = 0; i < ElemikodInfo.DataView.Count; i++)
                {
                    DataRow row = ElemikodInfo.DataView[i].Row;
                    string szov = row["SZOVEG"].ToString();
                    string id = row[ElemikodInfo.IdentityColumnName].ToString();
                    CsopKodInfo.DataView.RowFilter = "SORSZAM2 = " + id;
                    string csopid = CsopKodInfo.DataView[0]["SORSZAM1"].ToString();
                    Csopinfo.DataView.RowFilter = Csopinfo.IdentityColumnName + " = " + csopid;
                    if (Csopinfo.DataView[0].Row["SEMAE"].ToString() == "N")
                    {
                        int idindex = idk.IndexOf(id);
                        ArrayList ar1 = (ArrayList)nodeok[idindex];
                        if (ar1.Count == 1)
                            ar.Add(szov);
                    }
                }
                Csopinfo.DataView.RowFilter="";
                CsopKodInfo.DataView.RowFilter="";
                if (ar.Count != 0)
                    FelosztandoKoltsegCombo.Items.AddRange((string[])ar.ToArray(typeof(string)));
            }
        }

        public void ItemClicked(ToolStripItem item)
        {
            string butname = item.Name;
            butname = butname.Substring(0, butname.Length - 1);
            switch (butname)
            {
                case "uj":
                    EredetiSzint = AktualSzint;
                    AktualSzint = AktualSzint + 1;
                    ParentParentParentNode = ParentParentNode;
                    ParentParentNode = ParentNode;
                    ParentNode = AktualNode;
                    EredetiNode = AktualNode;
                    SetAktualNode(null);
                    break;
                case "ele":
//                    if (MasolandoNode != null)
//                    {
//                        TreeNode ujnode = new TreeNode(MasolandoNode.Text);
////                        ujnode.Name = 
//                    }
                    Ele = true;
                    Moge = false;
                    EredetiSzint = AktualSzint;
                    EredetiNode = AktualNode;
                    SetAktualNode(null);
                    break;
                case "moge":
//                    if (MasolandoNode != null)
//                    {
//                        TreeNode ujnode = new TreeNode(MasolandoNode.Text);
////                        ujnode.Name = 
//                    }
                    Ele = false;
                    Moge = true;
                    EredetiSzint = AktualSzint;
                    EredetiNode = AktualNode;
                    SetAktualNode(null);
                    break;
                case "torol":
                    Rogzittorol(false);
                    break;
                case "ok":
                    Hibas = false;
                    bool hibas1 = false;
                    if (ElemiKoltsegMasolando == null)
                    {
                        TextBox_Leave(KodBox, false);
                        hibas1 = Hibas;
                        Hibas = false;
                        TextBox_Leave(SzovegBox, false);
                        FakUserInterface.ErrorProvider.SetError(AfaCombo , "");
                        if (AktualSzint == 3)
                        {
                            if (AfaCombo.SelectedIndex == -1)
                            {
                                FakUserInterface.ErrorProvider.SetError(AfaCombo, "Nem lehet üres!");
                                Hibas = true;
                            }
                            if (Fajta == "K")
                            {
                                if(BevetelKodCombo.SelectedIndex==-1)
                                {
                                    FakUserInterface.ErrorProvider.SetError(BevetelKodCombo, "Nem lehet üres!");
                                    Hibas=true;
                                }
                                //                          FakUserInterfac
                            }
                        }
                    }
                    OkVolt = !Hibas && !hibas1;
                    break;
                case "rogzit":
                    Rogzittorol(true);
                    break;
                case "elolrol":
                    if (AktualNode == null)
                        SetAktualNode(EredetiNode);
                    else
                        SetAktualNode(AktualNode);
                    break;
                case "masol":
                    MasikBevKiad.KodBox.Text = KodBox.Text;
                    MasikBevKiad.SzovegBox.Text = SzovegBox.Text;
                    break;
            }
            if (butname != "masol")
                KodBox.Focus();
            else
                MasikBevKiad.KodBox.Focus();
            ToolStripButtonokEnable();
        }
        private void Rogzittorol(bool rogzit)
        {
            Tablainfo modtabinfo = null;
            Tablainfo osszefinfo = null;
            Tablainfo parenttabinfo = null;
            Tablainfo semainfo = null;
            TreeNode ujnode = null;
            TablainfoCollection tabinfocollection = new TablainfoCollection();
            ArrayList osszefinfocollectionar = new ArrayList();
            ArrayList nodear = new ArrayList();
            bool bev = Fajta == "B";
            if (bev)
                semainfo = Kukucs.bevsemainfo;
            else
                semainfo = Kukucs.koltssemainfo;
            switch (AktualSzint)
            {
                case 0:
                    osszefinfo = null;
                    parenttabinfo = null;
                    modtabinfo = Focsopinfo;
                    tabinfocollection.Add(modtabinfo);
                    tabinfocollection.Add(Alcsopinfo);
                    tabinfocollection.Add(Csopinfo);
                    tabinfocollection.Add(ElemikodInfo);
                    osszefinfocollectionar.Add(null);
                    osszefinfocollectionar.Add(FocsopAlcsopinfo);
                    osszefinfocollectionar.Add(AlcsopCsopinfo);
                    osszefinfocollectionar.Add(CsopKodInfo);
                    break;
                case 1:
                    modtabinfo = Alcsopinfo;
                    osszefinfo = FocsopAlcsopinfo;
                    parenttabinfo = Focsopinfo;
                    tabinfocollection.Add(modtabinfo);
                    tabinfocollection.Add(Csopinfo);
                    tabinfocollection.Add(ElemikodInfo);
                    osszefinfocollectionar.Add(null);
                    osszefinfocollectionar.Add(AlcsopCsopinfo);
                    osszefinfocollectionar.Add(CsopKodInfo);
                    break;
                case 2:
                    modtabinfo = Csopinfo;
                    osszefinfo = AlcsopCsopinfo;
                    parenttabinfo = Alcsopinfo;
                    tabinfocollection.Add(modtabinfo);
                    tabinfocollection.Add(ElemikodInfo);
                    osszefinfocollectionar.Add(null);
                    osszefinfocollectionar.Add(CsopKodInfo);
                    break;
                case 3:
                    modtabinfo = ElemikodInfo;
                    tabinfocollection.Add(modtabinfo);
                    osszefinfo = CsopKodInfo;
                    parenttabinfo = Csopinfo;
                    osszefinfocollectionar.Add(CsopKodInfo);
                    break;
            }
            DataRow row=null;
            string szov="";
            string id;
            Tablainfo[] rogzitendo;
            ArrayList ar = new ArrayList();
            for (int i = 0; i < tabinfocollection.Count; i++)
                ar.Add(tabinfocollection[i]);
            for (int i = 0; i < osszefinfocollectionar.Count; i++)
                if (osszefinfocollectionar[i] != null)
                    ar.Add((Tablainfo)osszefinfocollectionar[i]);
            ar.Add(semainfo);
            rogzitendo = (Tablainfo[])ar.ToArray(typeof(Tablainfo));
            if (!rogzit)
            {
                ar = new ArrayList();
                ar.Add(AktualNode);
                TreeNode[] nodeok = (TreeNode[])ar.ToArray(typeof(TreeNode));
                nodear = Osszesnode(nodeok, nodear);
                if (nodear.Count == tabinfocollection.Count)   // Elemi kod vizsgalat
                {
                    szov = "";
                    nodeok = (TreeNode[])nodear[nodear.Count - 1];
                    if (bev)
                    {
                        foreach (TreeNode egynode in nodeok)
                        {
                            id = egynode.Tag.ToString();
                            ElemikodInfo.DataView.RowFilter = "TERMEKKOD_ID = " + id;
                            MasikElemikodInfo.DataView.RowFilter = "TERMEKKOD_ID = " + id;
                            if (MasikElemikodInfo.DataView.Count != 0)
                            {
                                if (szov != "")
                                    szov += "\n";
                                szov = ElemikodInfo.DataView[0].Row["SZOVEG"].ToString() + " már szerepel az alábbi költségkód(ok) hivatkozásánál:";
                                szov += "\n" + MasikElemikodInfo.DataView[0].Row["SZOVEG"].ToString();
                            }
                        }
                        ElemikodInfo.DataView.RowFilter = "";
                        MasikElemikodInfo.DataView.RowFilter = "";
                        if (szov != "")
                        {
                            szov += "\nElöbb a költségkódok közül kell törölni!";
                            FakPlusz.MessageBox.Show(szov);
                            return;
                        }
                    }
                    else                       // koltsegkod itt esetleg csak a felosztas miatt
                    {
                        foreach (TreeNode egynode in nodeok)
                        {
                            id = egynode.Tag.ToString();
                            Csopinfo.DataView.RowFilter = Csopinfo.IdentityColumnName + " = " + egynode.Parent.Tag.ToString();
                            bool semae = Csopinfo.DataView[0].Row["SEMAE"].ToString() == "I";
                            if (!semae)
                            {
                                CsopKodInfo.DataView.RowFilter = "SORSZAM2 = " + id;
                                if (CsopKodInfo.DataView.Count > 1 && AktualSzint != 3 && !semae)
                                {
                                    if (szov == "")
                                        szov += "Az alábbi költségkód(ok) sémában szerepelnek:";
                                    ElemikodInfo.DataView.RowFilter = ElemikodInfo.IdentityColumnName + "= " + id;
                                    szov += "\n" + ElemikodInfo.DataView[0].Row["SZOVEG"].ToString();
                                }
                            }
                            Csopinfo.DataView.RowFilter = "";
                            CsopKodInfo.DataView.RowFilter = "";    
                            Kukucs.koltscsopkod.DataView.RowFilter = "";
                            ElemikodInfo.DataView.RowFilter = "";
                        }
                        if (szov != "")
                        {
                            szov += "\nCsak egyenként törölhetö";
                            FakPlusz.MessageBox.Show(szov);
                            return;
                        }
                    }
                    foreach (TreeNode egynode in nodeok)
                    {
                        id = egynode.Tag.ToString();
                        if (Kukucs.ComboHasznalatban(ElemikodInfo, null, "", "", id, true))
                            return;
                    }
                }
                if (FakPlusz.MessageBox.Show("Biztosan törölhetö?", "", FakPlusz.MessageBox.MessageBoxButtons.IgenNem) == FakPlusz.MessageBox.DialogResult.Igen)
                {
                    bool voltmasiknode = false;
                    ArrayList ar1 = null;
                    TreeNode masiktorlendonode = null;
                    id = AktualNode.Tag.ToString();
                    string[] idk = null;
                    if (AktualSzint == 3 && !bev)
                    {
                        int arindex = Kukucs.koltsegkodidk.IndexOf(id);
                        ar1 = (ArrayList)Kukucs.treenodeokarray[arindex];
                        if (ar1.Count > 1)
                        {
                            voltmasiknode = true;
                            for (int i = 0; i < ar1.Count; i++)
                            {
                                TreeNode egynode = (TreeNode)ar1[i];
                                if (egynode == AktualNode && i == 0)
                                    masiktorlendonode=(TreeNode)ar1[1];
                            }
                            ar1.Remove(AktualNode);
                            if (masiktorlendonode != null)
                            {
                                Kukucs.treenodeokarray.RemoveAt(arindex); // ar1.Remove(masiktorlendonode);
                                Kukucs.koltsegkodidk.RemoveAt(arindex);
                            }
                        }
                    }
                    for (int i = 0; i < nodear.Count; i++)
                    {
                        TreeNode[] egynodeok = (TreeNode[])nodear[i];
                        Tablainfo torlinfo = tabinfocollection[i];
                        Tablainfo egyosszefinfo = null;
                        Tablainfo egysemainfo = null;
                        if (osszefinfocollectionar[i] != null)
                        {
                            egyosszefinfo = (Tablainfo)osszefinfocollectionar[i];
                            if (egyosszefinfo.Kodtipus == "Termcsopkod" || egyosszefinfo.Kodtipus == "Koltscsopkod")
                            {
                                egysemainfo = semainfo;

                            }
                        }
                        for (int j = 0; j < egynodeok.Length; j++)
                        {
                            string parentid = "";
                            if (egynodeok[j].Parent != null)
                            {
                                parentid = egynodeok[j].Parent.Tag.ToString();
                                if (torlinfo == ElemikodInfo && Fajta=="K")
                                {
                                    if (voltmasiknode)
                                    {
                                        if (masiktorlendonode == null)
                                            torlinfo = null;
                                        else
                                            parentid = "";
                                    }

                                }
                            }
                            TorlesTablabol(egynodeok[j].Tag.ToString(),parentid, torlinfo, egyosszefinfo, egysemainfo);
                        }
                    }
                    FakUserInterface.Rogzit(rogzitendo);
                    for (int i = 0; i < rogzitendo.Length; i++)
                    {
                        Tablainfo egyinfo = rogzitendo[i];
                        if ("CO".Contains(egyinfo.Adatfajta))
                        {
                            if (egyinfo.DataView.Count == 0 && egyinfo.KellVerzio && egyinfo.VerzioTerkepArray.Count !=0)
                            {
                                egyinfo.VerzioTerkepArray.RemoveAt(egyinfo.VerzioTerkepArray.Count - 1);
                                egyinfo.AktVerzioId = egyinfo.LastVersionId;
                            }
                        }
                    }
//                    if ()
//                    {
                        //foreach (TreeNode egynode in torlendonodeok)
                        //{
                        //    TreeNode parentnode = egynode.Parent;
                        //    if (parentnode != null)
                        //    {
                        //        int i = parentnode.Nodes.IndexOf(egynode);
                        //        if (i != -1)
                        //            parentnode.Nodes.Remove(egynode);
                        //    }
                        //}
 //                   }
                    TreeNode node = AktualNode;
                    int pos = 0;
                    int nextpos = 0;
                    TreeNode kovnode = null;
                    TreeNodeCollection coll = null;
                    if (ParentNode == null)
                        coll = TreeView.Nodes;
                    else
                        coll = ParentNode.Nodes;
                    if (coll.Count == 1)
                    {
                        kovnode = null;
                        if (ParentNode != null)
                            kovnode = ParentNode;
                    }
                    else
                    {
                        pos = coll.IndexOf(AktualNode);
                        if (pos == coll.Count - 1)
                            nextpos = pos - 1;
                        else
                            nextpos = pos + 1;
                        kovnode = coll[nextpos];
                    }
                    if (masiktorlendonode != null)
                    {
                        masiktorlendonode.Parent.Nodes.Remove(masiktorlendonode);
                    }
                    coll.Remove(AktualNode);
                    SetAktualNode(kovnode);
                    if (!bev)
                        FeloszthatoKoltsegidkModosit();
                }
            }
            else
            {
                ArrayList afaidar = new ArrayList(FakUserInterface.GetTartal(AfaInfo, "SORSZAM"));
                bool semae = bev;
                string egychar;
                int sorrend = 0;
                string[] csopkodinfoidk = null;
                string[] sematorlendoidk = null;
                string[] semaidirandok = null;
                string felosztid = "0";
                string csopkodid = "";
                string sematorlendoid = "";
                string semairandoid = "";
                string bevid = "";
                string semasorszam1 = "";
                int aktsorrend = 0;
                if (AktualNode != null)           // modosit
                {
                    id = AktualNode.Tag.ToString();
                    modtabinfo.DataView.RowFilter = modtabinfo.IdentityColumnName + " = " + id;
                    row = modtabinfo.DataView[0].Row;
                    row["KOD"] = KodBox.Text;
                    row["SZOVEG"] = SzovegBox.Text;
                    row["MODOSITOTT_M"] = 1;
                    string oldsemae;
                    if (AktualSzint == 2 && !bev)
                    {
                        oldsemae = row["SEMAE"].ToString();
                        csopkodinfoidk = FakUserInterface.GetTartal(CsopKodInfo, "SORSZAM", "SORSZAM1", id);
                        egychar = FelosztCombo.Text.Substring(0, 1);
                        if (egychar != oldsemae && oldsemae == "I")
                        {
                            if (csopkodinfoidk != null)
                            {
                                sematorlendoidk = FakUserInterface.GetTartal(SemaInfo, "SORSZAM", "SORSZAM1", csopkodinfoidk);
                            }
                        }
                        row["SEMAE"] = egychar;
                        semae = egychar == "I";
                    }
                    if (AktualSzint == 3)
                    {
                        if (AfaCombo.SelectedIndex != -1)
                            row["AFA_ID"] = afaidar[AfaCombo.SelectedIndex];
                        else
                            row["AFA_ID"] = 0;
                        if (!bev)
                        {
                            Csopinfo.DataView.RowFilter = Csopinfo.IdentityColumnName + "= " + ParentNode.Tag.ToString();
                            DataRow row1 = Csopinfo.DataView[0].Row;
                            egychar = row1["SEMAE"].ToString();
                            semae = egychar == "I" && FelosztSzazalCombo.Text != "";
                            Csopinfo.DataView.RowFilter = "";
                            string[] bevidk = FakUserInterface.GetTartal(MasikElemikodInfo, MasikElemikodInfo.IdentityColumnName, "SZOVEG", BevetelKodCombo.Text);
                            if (bevidk != null)
                                row["TERMEKKOD_ID"] = bevidk[0];
                            if (AlkalmazottiCombo.Visible && AlkalmazottiCombo.SelectedIndex!=-1)
                                row["DOLGOZO_ID"] = Kukucs.alkalmidk[AlkalmazottiCombo.SelectedIndex];
                        }
                        else
                            semae = FelosztSzazalCombo.Text != "";
                        //if (semae)
                        //{
                        CsopKodInfo.DataView.RowFilter = "SORSZAM1 = " + ParentNode.Tag.ToString() + " AND SORSZAM2 = " + id;
                        semasorszam1 = CsopKodInfo.DataView[0].Row[CsopKodInfo.IdentityColumnName].ToString();
                        sorrend = Convert.ToInt32(CsopKodInfo.DataView[0].Row["SORREND"].ToString());
                        CsopKodInfo.DataView.RowFilter = "";
                        if (!semae)
                        {
                            sematorlendoidk = FakUserInterface.GetTartal(SemaInfo, "SORSZAM", "SORSZAM1", semasorszam1);
                            if (sematorlendoidk != null)
                            {
                                sematorlendoid = sematorlendoidk[0];
                                SemaInfo.Modositott = true;
                                SemaInfo.DataView.RowFilter = "SORSZAM = " + sematorlendoid;
                                SemaInfo.DataView[0].Row.Delete();

                            }
                        }
                        else
                        {
                            felosztid = FakUserInterface.GetTartal(Kukucs.fszazal, Kukucs.fszazal.IdentityColumnName, "SZOVEG", FelosztSzazalCombo.Text)[0];
                            SemaInfo.Modositott = true;
                            semaidirandok = FakUserInterface.GetTartal(SemaInfo, "SORSZAM", "SORSZAM1", semasorszam1);
                            if (semaidirandok != null)
                            {
                                semairandoid = semaidirandok[0];
                                SemaInfo.DataView.RowFilter = "SORSZAM = " + semairandoid;
                                SemaInfo.DataView[0].Row["SORSZAM2"] = felosztid;
                                SemaInfo.DataView[0].Row["PREV_ID2"] = felosztid;
                            }
                            else
                            {
                                row = SemaInfo.Adattabla.Ujsor(sorrend);
                                {
                                    row["SORSZAM1"] = semasorszam1;
                                    row["PREV_ID1"] = semasorszam1;
                                    row["SORSZAM2"] = felosztid;
                                    row["PREV_ID2"] = felosztid;
                                }
                            }
                        }

                    }
                    //                   }

                    modtabinfo.Modositott = true;
                    modtabinfo.DataView.RowFilter = "";
                    AktualNode.Name = KodBox.Text;
                    AktualNode.Text = SzovegBox.Text + " (" + KodBox.Text + ")";
                    ujnode = AktualNode;
                }
                else         // beszur
                {

                    if (ElemiKoltsegMasolando != null)
                    {
                        SzovegBox.Text = ElemiKoltsegMasolando["SZOVEG"].ToString();
                        KodBox.Text = ElemiKoltsegMasolando["KOD"].ToString();
                    }
                    szov = SzovegBox.Text + " (" + KodBox.Text + ")";
                    ujnode = new TreeNode(szov);
                    ujnode.Name = KodBox.Text;
                    if (ParentNode == null)
                    {
                        if (Ele)
                            TreeView.Nodes.Insert(AktualNodePos, ujnode);
                        else if (Moge)
                            TreeView.Nodes.Insert(AktualNodePos + 1, ujnode);
                        else
                            TreeView.Nodes.Add(ujnode);
                    }
                    else if (ParentNode.Nodes.Count == 0)
                        ParentNode.Nodes.Add(ujnode);
                    else
                    {
                        if (Ele)
                            ParentNode.Nodes.Insert(AktualNodePos, ujnode);
                        else if (Moge)
                            ParentNode.Nodes.Insert(AktualNodePos + 1, ujnode);
                        else
                            ParentNode.Nodes.Add(ujnode);

                    }
                    if (ParentNode != null && !bev && AktualSzint == 3)
                    {
                        id = ParentNode.Tag.ToString();
                        semae = FakUserInterface.GetTartal(Csopinfo, "SEMAE", "KOLTSEGCSOPORT_ID", id)[0] == "I";
                    }
                    aktsorrend = 100;
                    if (ElemiKoltsegMasolando == null)
                    {
                        if (EredetiNode != null && EredetiSzint == AktualSzint)
                            aktsorrend = Sorrendszam(Ele, Moge, modtabinfo, EredetiNode.Tag.ToString(), modtabinfo.IdentityColumnName);
                        else if (modtabinfo.DataView.Count != 0)
                            aktsorrend += Convert.ToInt32(modtabinfo.DataView[modtabinfo.DataView.Count - 1].Row["SORREND"].ToString());
                        row = modtabinfo.Adattabla.Ujsor(aktsorrend);
                        row["KOD"] = KodBox.Text;
                        row["SZOVEG"] = SzovegBox.Text;
                        row["MODOSITOTT_M"] = 1;
                        if (AktualSzint == 2 && !bev)
                        {
                            egychar = FelosztCombo.Text.Substring(0, 1);
                            row["SEMAE"] = egychar;
                            semae = egychar == "I";
                        }
                    }
                    if (AktualSzint == 3)
                    {
                        if (ElemiKoltsegMasolando == null)
                        {
                            if (AfaCombo.SelectedIndex != -1)
                                row["AFA_ID"] = afaidar[AfaCombo.SelectedIndex];
                            else
                                row["AFA_ID"] = afaidar[0];
                        }
                        if (semae)
                        {

                            if (FelosztSzazalCombo.Text != "")
                                felosztid = FakUserInterface.GetTartal(Kukucs.fszazal, Kukucs.fszazal.IdentityColumnName, "SZOVEG", FelosztSzazalCombo.Text)[0];
                            //                           SemaInfo
                        }
                        //else
                        //{
                        //}
                        if (!bev && ElemiKoltsegMasolando == null)
                        {
                            string[] bevidk = FakUserInterface.GetTartal(MasikElemikodInfo, MasikElemikodInfo.IdentityColumnName, "SZOVEG", BevetelKodCombo.Text);
                            if (bevidk != null)
                                row["TERMEKKOD_ID"] = bevidk[0];
                            if (AlkalmazottiCombo.Visible && AlkalmazottiCombo.SelectedIndex != -1)
                                row["DOLGOZO_ID"] = Kukucs.alkalmidk[AlkalmazottiCombo.SelectedIndex];
                            Csopinfo.DataView.RowFilter = Csopinfo.IdentityColumnName + " = " + ParentNode.Tag.ToString();
                            semae = Csopinfo.DataView[0].Row["SEMAE"].ToString() == "I";
                            Csopinfo.DataView.RowFilter = "";
                        }
                    }

                }
                if (modtabinfo.Modositott || ElemiKoltsegMasolando != null)
                {
                    if(modtabinfo.Modositott)
                        FakUserInterface.Rogzit(modtabinfo);
                    id = FakUserInterface.GetTartal(modtabinfo, modtabinfo.IdentityColumnName, "KOD", KodBox.Text)[0];
                    if (AktualNode == null)
                    {
                        ujnode.Tag = id;
                        if (!bev && AktualSzint == 3)
                        {
                            int arindex = Kukucs.koltsegkodidk.IndexOf(id);
                            //                           ArrayList ar;
                            if (arindex == -1)
                            {
                                Kukucs.koltsegkodidk.Add(id);
                                ar = new ArrayList();
                                ar.Add(ujnode);
                                Kukucs.treenodeokarray.Add(ar);
                                //                                FeloszthatoKoltsegidkModosit();
                            }
                            else
                            {
                                ar = (ArrayList)Kukucs.treenodeokarray[arindex];
                                ar.Add(ujnode);
                            }
                        }
                        if (osszefinfo != null)
                        {
                            string parentid = ParentNode.Tag.ToString();
                            if (osszefinfo.DataView.Count == 0)
                                                        aktsorrend = 100;
                            //else
                            //    aktsorrend=Convert.ToInt32(FakUserInterface.GetTartal(modtabinfo,"SORREND",modtabinfo.IdentityColumnName,id)[0]);
                                //                               sorrend += Convert.ToInt32(osszefinfo.DataView[osszefinfo.DataView.Count - 1].Row["SORREND"].ToString());
                                //if (parenttabinfo.DataView.Count != 0)
                                //    sorrend = Convert.ToInt32(parenttabinfo.DataView[0].Row["SORREND"].ToString());
                            row = osszefinfo.Adattabla.Ujsor(aktsorrend);
 //                           parenttabinfo.DataView.RowFilter = "";
                            row["SORSZAM1"] = parentid;
                            row["PREV_ID1"] = parentid;
                            row["SORSZAM2"] = id;
                            row["PREV_ID2"] = id;
                            if (AktualSzint == 3 && AktualNode == null && felosztid != "0")
                            {
                                if (osszefinfo.KellVerzio && osszefinfo.VerzioTerkepArray.Count == 0)
                                    osszefinfo.CreateNewVersion();
                                FakUserInterface.Rogzit(osszefinfo);
                                osszefinfo.DataView.RowFilter ="SORSZAM1 = " + parentid + " AND SORSZAM2 = " + id;
                                DataRow row1 = osszefinfo.DataView[0].Row;
                                osszefinfo.DataView.RowFilter = "";
                                if (SemaInfo.KellVerzio && SemaInfo.VerzioTerkepArray.Count == 0)
                                    SemaInfo.CreateNewVersion();
                                sorrend = 100;
                                if (SemaInfo.DataView.Count != 0)
                                    sorrend += Convert.ToInt32(SemaInfo.DataView[SemaInfo.DataView.Count - 1].Row["SORREND"].ToString());
                                row = SemaInfo.Adattabla.Ujsor(sorrend);
                                row["SORSZAM1"] = row1["SORSZAM"];
                                row["PREV_ID1"] = row1["SORSZAM"];
                                row["SORSZAM2"] = felosztid;
                                row["PREV_ID2"] = felosztid;

                            }
                            else
                            {
                                ar = new ArrayList(rogzitendo);
                                if (ar.IndexOf(osszefinfo) == -1)
                                {
                                    ar.Add(osszefinfo);
                                    rogzitendo = (Tablainfo[])ar.ToArray(typeof(Tablainfo));
                                }
                            }
                        }
                    }
                    for (int i = 0; i < rogzitendo.Length; i++)
                    {
                        Tablainfo egyinfo = rogzitendo[i];
                        if ("CO".Contains(egyinfo.Adatfajta))
                        {
                            if (egyinfo.KellVerzio && egyinfo.VerzioTerkepArray.Count == 0)
                                egyinfo.CreateNewVersion();
                        }
                    }
                    FakUserInterface.Rogzit(rogzitendo);
                    if (!bev)
                        FeloszthatoKoltsegidkModosit();
                    EredetiNode = null;
                    EredetiSzint = -1;
                    SetAktualNode(ujnode);
                }
            }
        }
        private int Sorrendszam(bool ele, bool moge, Tablainfo modtabinfo, string referenciaid,string referenciacolnev)
        {
            int aktsorrend = 100;
            int elozosorrend = 0;
            int kovetkezosorrend = 0;
            DataRow aktsor;
            string id = "";
            bool megvan = false;
            for (int i = 0; i < modtabinfo.DataView.Count; i++)
            {
                aktsor = modtabinfo.DataView[i].Row;
                id = aktsor[referenciacolnev].ToString();
                int sorrend1 = Convert.ToInt32(aktsor["SORREND"].ToString());
                if (id == referenciaid)
                {
                    megvan = true;
                    if (ele)
                    {
                        aktsorrend = elozosorrend + (sorrend1 - elozosorrend) / 2;
                        break;
                    }
                    if (moge)
                    {
                        if (i == modtabinfo.DataView.Count - 1)
                            aktsorrend = sorrend1 + 100;
                        else
                        {
                            aktsor = modtabinfo.DataView[i + 1].Row;
                            kovetkezosorrend = Convert.ToInt32(aktsor["SORREND"].ToString());
                            aktsorrend = sorrend1 + (kovetkezosorrend - sorrend1) / 2;
                        }
                    }
                }
                elozosorrend = sorrend1;
            }
            if (!megvan)
            {
            }
            return aktsorrend;
        }
        private ArrayList Osszesnode(TreeNode[] nodeok, ArrayList nodear)
        {
            ArrayList aktar = nodear;
            ArrayList ar = new ArrayList();
            for (int i = 0; i < nodeok.Length; i++)
                ar.Add(nodeok[i]);
            if (ar.Count != 0)
            {
                TreeNode[] ujnodeok = (TreeNode[])ar.ToArray(typeof(TreeNode));
                aktar.Add(ujnodeok);
                ar = new ArrayList();
                for (int i = 0; i < ujnodeok.Length; i++)
                {
                    TreeNode egynode = ujnodeok[i];
                    for (int j = 0; j < egynode.Nodes.Count; j++)
                        ar.Add(egynode.Nodes[j]);
                }
                if (ar.Count != 0)
                {
                    ujnodeok = (TreeNode[])ar.ToArray(typeof(TreeNode));
                    Osszesnode(ujnodeok, aktar);
                }
            }
            return aktar;
        }
        private void TorlesTablabol(string nodeid,string parentid,Tablainfo nodeinfo, Tablainfo osszefinfo, Tablainfo semainfo)
        {
 //           string id = node.Tag.ToString();
            if (nodeinfo != null)
            {
                nodeinfo.DataView.RowFilter = nodeinfo.IdentityColumnName + " = " + nodeid;
                for (int i = 0; i < nodeinfo.DataView.Count; i++)
                {
                    nodeinfo.DataView[i].Row.Delete();
                    i = -1;
                }
                nodeinfo.DataView.RowFilter = "";
                nodeinfo.Modositott = true;
            }
            if (osszefinfo != null)
            {
                osszefinfo.Modositott = true;
                string[] idk = null;
                if(parentid == "")
                    osszefinfo.DataView.RowFilter = "SORSZAM2 = " + nodeid;
                else
                    osszefinfo.DataView.RowFilter = "SORSZAM1 = "+parentid + " AND SORSZAM2 = " + nodeid;
                if (semainfo != null)
                    idk = FakUserInterface.GetTartal(osszefinfo, osszefinfo.IdentityColumnName);
                for (int i = 0; i < osszefinfo.DataView.Count; i++)
                    osszefinfo.DataView[i].Row.Delete();
                osszefinfo.DataView.RowFilter = "";
                if (semainfo != null && idk!=null)
                {
                    for (int i = 0; i < idk.Length; i++)
                    {
                        semainfo.DataView.RowFilter = "SORSZAM1 =" + idk[i];
                        for (int j = 0; j < semainfo.DataView.Count; j++)
                        {
                            semainfo.Modositott = true;
                            semainfo.DataView[j].Row.Delete();
                            j = -1;
                        }
                    }
                    semainfo.DataView.RowFilter = "";
                }
            }
        }

        public void GroupBox_Enter()
        {
            GroupBox.BackColor = FakUserInterface.AktivControlBackColor;
            MasikBevKiad.GroupBox.BackColor = FakUserInterface.InaktivControlBackColor;
        }
        public void TextBox_Enter(TextBox box)
        {
            if (AktivControl != null)
                AktivControl.BackColor = FakUserInterface.InaktivInputBackColor;
            AktivControl = box;
            box.BackColor = FakUserInterface.AktivInputBackColor;
        }
        public void TextBox_Leave(TextBox box)
        {
            TextBox_Leave(box, true);
        }
        public void TextBox_Leave(TextBox box ,bool kellszin)
        {
            string hibaszov = "";
            if (box.Text == "")
                hibaszov = "Nem lehet üres!";
           else 
               hibaszov = AzonossagVizsgal(box);
            box.BackColor = FakUserInterface.InaktivInputBackColor;
            FakUserInterface.ErrorProvider.SetError(box, hibaszov);
            if (hibaszov != "")
                Hibas = true;
            ToolStripButtonokEnable();
        }
        public string AzonossagVizsgal(TextBox box)
        {
            bool kodbox = box == KodBox;
            if (AktualNode!=null && (kodbox && box.Text == AktualNode.Name || !kodbox && box.Text == Szoveg))
                return "";
            else
            {
                Valtozott = true;
                Tablainfo vizsginfo = null;
                bool bevetel = Fajta == "B";
                switch (AktualSzint)
                {
                    case 0:
                        if (bevetel)
                            vizsginfo = Kukucs.termfocsopinfo;
                        else
                            vizsginfo = Kukucs.koltsfocsopinfo;
                        break;
                    case 1:
                        if (bevetel)
                            vizsginfo = Kukucs.termalcsopinfo;
                        else
                            vizsginfo = Kukucs.koltsalcsopinfo;
                        break;
                    case 2:
                        if (bevetel)
                            vizsginfo = Kukucs.termcsopinfo;
                        else
                            vizsginfo = Kukucs.koltscsopinfo;
                        break;
                    case 3:
                        vizsginfo = ElemikodInfo;
                        break;
                }
                string vizsgelem = "KOD";
                string visszaelem = "SZOVEG";
                string hibaszov = "Van már ilyen kód!\nMegnevezés:";
                if (!kodbox)
                {
                    vizsgelem = "SZOVEG";
                    visszaelem = "KOD";
                    hibaszov = "Van már ilyen megnevezés!\nKód:";
                }
                string[] megf = FakUserInterface.GetTartal(vizsginfo, visszaelem,vizsgelem,box.Text);
                if (megf != null)
                    return hibaszov + megf[0];
            }
            return "";
        }
        public void ComboBox_Enter(ComboBox box)
        {
            if(AktivControl !=null)
                AktivControl.BackColor = FakUserInterface.InaktivInputBackColor;
            AktivControl = box;
            box.BackColor = FakUserInterface.AktivInputBackColor;
        }
        public void ComboBox_Leave(ComboBox box)
        {
            box.BackColor = FakUserInterface.InaktivInputBackColor;
//            //"koltsafacombo"
//            string name = box.Name;
////            bool nemlehetures = 
//            if (AktualNode != null)
//            {
//                if (AlkalmazottiCombo!=null && box.Name == AlkalmazottiCombo.Name)
//                {
//                    if (AktualNode != null && AktualSzint == 3)
//                    {
//                    }

//                }
//            }
        }
        public void ToolStripButtonokEnable()
        {
            string csopid = "";
            bool lehetkodszint = true;
            Uj = AktualNode==null;
            if (Uj)
                Valtozott = true;
            else
            {
                if(Fajta=="K" && AktualSzint>1)
                {
                    switch(AktualSzint)
                    {
                        case 2:
                            csopid = AktualNode.Tag.ToString();
                            break;
                        case 3:
                            csopid = ParentNode.Tag.ToString();
                            break;
                    }
                    bool semae = false;
                    Csopinfo.DataView.RowFilter=Csopinfo.IdentityColumnName+ " = "+csopid;
                    semae = Csopinfo.DataView[0].Row["SEMAE"].ToString()=="I";
                    Csopinfo.DataView.RowFilter = "";
                    if(!semae && termkodszovegek==null || semae && FelosztandoKoltsegCombo.Items.Count ==0)
                        lehetkodszint=false;
                }

            }    
            for (int i = 0; i < rovidnevek.Length; i++)
            {
                ToolStripItem egyitem = buttonok[i];
                egyitem.Visible = false;
                switch (rovidnevek[i])
                {
                    case "uj":
                        if(!Uj)
                        {
                            egyitem.Visible = AktualSzint != 3 && AktualNode.Nodes.Count == 0;
                            if (!lehetkodszint)
                                egyitem.Visible = false;
                            if (egyitem.Visible)
                                egyitem.Text = "Új " + csopszoveg[AktualSzint + 1];
                        }
                        break;
                    case "ele":
                        if (!Uj && lehetkodszint)
                            egyitem.Visible = true;
                        break;
                    case "moge":
                        if (!Uj && lehetkodszint)
                            egyitem.Visible = true;
                        break;
                    case "ok":
                        if (Valtozott || Hibas)
                            egyitem.Visible = true;
                        break;
                    case "torol":
                        if (!Uj)
                        {
//                            if(AktualSzint!=3 || Fajta=="B")
                                egyitem.Visible = true;
                        }
                        break;
                    case "rogzit":
                        if (OkVolt && !Hibas)
                            egyitem.Visible = true;
                        break;
                    case "elolrol":
                        egyitem.Visible = true;
                        break;
                    case "masol":
                        if (Fajta == "B")
                        {
                            if (MasikBevKiad != null && AktualSzint == MasikBevKiad.AktualSzint && AktualSzint != 3 && !Valtozott && !Hibas && MasikBevKiad.Uj)
                                egyitem.Visible = true;
                        }
                        else
                        {
                            Kukucs.masol1.Visible = false;
                            if (AktualSzint == MasikBevKiad.AktualSzint && AktualSzint != 3 && Uj)
                                Kukucs.masol1.Visible = true;
                        }
                        break;
                    //case "feloszthoz":
                    //    if (Fajta == "K")
                    //    {
                    //        if (!Uj && AktualSzint == 3)
                    //        {
                    //            string id = ParentNode.Tag.ToString();
                    //            Csopinfo.DataView.RowFilter = Csopinfo.IdentityColumnName + "= " + id;
                    //            if (Csopinfo.DataView[0].Row["SEMAE"].ToString() == "N")
                    //                egyitem.Visible = true;
                    //            Csopinfo.DataView.RowFilter = "";
                    //        }
                    //    }
                    //    break;
                }
            }
            if (ElemiKoltsegMasolando == null)
            {
                if (Uj)
                {
                    GroupBox.Text = "Új " + csopszoveg[AktualSzint];
                    if ( EredetiNode != null && (Ele || Moge))
                    {
                        GroupBox.Text += " " + EredetiNode.Text + " ";
                        if (Ele)
                            GroupBox.Text += "elé";
                        else
                            GroupBox.Text += "mögé";
                    }
                }
                else
                    GroupBox.Text = AktualNode.Text + " " + csopszoveg[AktualSzint];
            }
            else
            {
                GroupBox.Text = csopszoveg[AktualSzint] + ": " + ElemiKoltsegMasolando["SZOVEG"].ToString() + "(" + ElemiKoltsegMasolando["KOD"].ToString()+")";
            }
            GroupBox.Text += EddigFelosztott;
        }
        
    }
}