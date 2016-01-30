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
    public partial class KukucsEredm : Base
    {
        Panel Panel;
        Control[] controlok;
        ToolStripMenuItem Teszt;
        private Tablainfo koltsfocsopinfo;
        private Tablainfo koltsalcsopinfo;
        private Tablainfo koltscsopinfo;
        private Tablainfo koltsegkodok;
        private Tablainfo termfocsopinfo;
        private Tablainfo termalcsopinfo;
        private Tablainfo termcsopinfo;
        private Tablainfo termekkodok;
        private Tablainfo koltsfocsopalcsop;
        private Tablainfo koltsalcsopcsop;
        private Tablainfo koltscsopkod;
        private Tablainfo termfocsopalcsop;
        private Tablainfo termalcsopcsop;
        private Tablainfo termcsopkod;
        private Tablainfo bevsemainfo;
        private Tablainfo koltssemainfo;
        private Tablainfo fszazal;
        private string[] fszazalszovegek;
        private string[] fszazalkodok;
        private string[] fszazalidk;
        public KukucsEredm(FakUserInterface fak, Panel panel, ToolStripMenuItem teszt)
        {
            InitializeComponent();
            FakUserInterface = fak;
            Panel = panel;
            Teszt = teszt;
            this.Dock = DockStyle.Fill;
        }
        public void ShowKukucs()
        {
            controlok = new Control[Panel.Controls.Count];
            for (int i = 0; i < Panel.Controls.Count; i++)
                controlok[i] = Panel.Controls[i];
            Panel.Controls.Clear();
            treeView1.Nodes.Clear();
            treeView2.Nodes.Clear();
            treeView3.Nodes.Clear();
            string[] termfocsopidk;
            string[] termfocsopszovegek;
            string[] termfocsopkodok;
            string[] termalcsopidk;
            string[] termalcsopszovegek;
            string[] termalcsopkodok;
            string[] termcsopidk;
            string[] termcsopszovegek;
            string[] termcsopkodok;
            string[] termkodidk;
            string[] termkodszovegek;
            string[] termkodkodok;
            string[] koltsfocsopidk;
            string[] koltsfocsopszovegek;
            string[] koltsfocsopkodok;
            string[] koltsalcsopidk;
            string[] koltsalcsopszovegek;
            string[] koltsalcsopkodok;
            string[] koltscsopidk;
            string[] koltscsopszovegek;
            string[] koltscsopkodok;
            string[] koltskodidk;
            string[] koltskodszovegek;
            string[] koltskodkodok;
            //            TreeNode eredmenytermfonode;
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
            fszazal = FakUserInterface.GetKodtab("C", "Fszazal");
            fszazalszovegek = FakUserInterface.GetTartal(fszazal, "SZOVEG");
            fszazalidk = FakUserInterface.GetTartal(fszazal, fszazal.IdentityColumnName);
            fszazalkodok = FakUserInterface.GetTartal(fszazal, "KOD");
            termfocsopidk = FakUserInterface.GetTartal(termfocsopinfo, "SORSZAM");
            termfocsopszovegek = FakUserInterface.GetTartal(termfocsopinfo, "SZOVEG");
            termfocsopkodok = FakUserInterface.GetTartal(termfocsopinfo, "KOD");
            koltsfocsopidk = FakUserInterface.GetTartal(koltsfocsopinfo, "SORSZAM");
            koltsfocsopszovegek = FakUserInterface.GetTartal(koltsfocsopinfo, "SZOVEG");
            koltsfocsopkodok = FakUserInterface.GetTartal(koltsfocsopinfo, "KOD");
            for (int i = 0; i < termfocsopidk.Length; i++)
            {
                string egytermfocsopid = termfocsopidk[i];
                string egytermfocsopszov = termfocsopszovegek[i];
                string egytermfocsopkod = termfocsopkodok[i];
 //               TreeNode focsopnode = new TreeNode(egytermfocsopszov + " (" + egytermfocsopkod + ")");
                TreeNode eredmenytermfonode = new TreeNode(egytermfocsopszov + " (" + egytermfocsopkod + ")");
 //               treeView1.Nodes.Add(focsopnode);
                treeView3.Nodes.Add(eredmenytermfonode);
                termalcsopidk = FakUserInterface.GetTartal(termfocsopalcsop, "SORSZAM2", "SORSZAM1", egytermfocsopid);
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
//                        focsopnode.Nodes.Add(alcsopnode);
                        termcsopidk = FakUserInterface.GetTartal(termalcsopcsop, "SORSZAM2", "SORSZAM1", egytermalcsopid);
                        if (termcsopidk != null && termcsopidk.Length != 0)
                        {
                            termcsopszovegek = FakUserInterface.GetTartal(termcsopinfo, "SZOVEG", "SORSZAM", termcsopidk);
                            termcsopkodok = FakUserInterface.GetTartal(termcsopinfo, "KOD", "SORSZAM", termcsopidk);
                            for (int k = 0; k < termcsopidk.Length; k++)
                            {
                                string[] bevsemainfoidk = null;
                                string egytermcsopid = termcsopidk[k];
                                string egytermcsopszov = termcsopszovegek[k];
                                string egytermcsopkod = termcsopkodok[k];
                                TreeNode eredmenycsopnode = new TreeNode(egytermcsopszov + " (" + egytermcsopkod + ")");
                                string[] termcsopkodidk = FakUserInterface.GetTartal(termcsopkod, "SORSZAM", "SORSZAM1", egytermcsopid);
                                termkodidk = FakUserInterface.GetTartal(termcsopkod, "SORSZAM2", "SORSZAM1", egytermcsopid);
                                if(termcsopkodidk!=null)
                                    bevsemainfoidk = FakUserInterface.GetTartal(bevsemainfo, "SORSZAM2", "SORSZAM1", termcsopkodidk);
                                eredmenytermfonode.Nodes.Add(eredmenycsopnode);
                                if (termkodidk != null && termkodidk.Length != 0)
                                {
                                    termkodszovegek = FakUserInterface.GetTartal(termekkodok, "SZOVEG", "TERMEKKOD_ID", termkodidk);
                                    termkodkodok = FakUserInterface.GetTartal(termekkodok, "KOD", "TERMEKKOD_ID", termkodidk);
                                    if (bevsemainfoidk != null)
                                    {
                                        TreeNode csopnode = new TreeNode();
                                        treeView1.Nodes.Add(csopnode);
                                        int osszazal = 0;
                                        ArrayList ar = new ArrayList(fszazalidk);
                                        for (int l = 0; l < bevsemainfoidk.Length; l++)
                                        {
                                            int pos = ar.IndexOf(bevsemainfoidk[l]);
                                            osszazal += Convert.ToInt32(fszazalkodok[pos]);
                                            TreeNode kodnode = new TreeNode(termkodszovegek[l]+ " ("+termkodkodok[l]+") - "+fszazalszovegek[pos]);
                                            csopnode.Nodes.Add(kodnode);
                                        }
                                        csopnode.Text = egytermcsopszov + " (" + egytermcsopkod + ") - " + osszazal.ToString() + "%";
//                                        TreeNode csopnode = new TreeNode(egytermcsopszov);
//                                        treeView1.Nodes.Add(csopnode);
                                    }
                                    for (int l = 0; l < termkodidk.Length; l++)
                                    {
                                        string egykodid = termkodidk[l];
                                        string egyszov = termkodszovegek[l];
                                        string egykod = termkodkodok[l];
                                        TreeNode eredmkodnode = new TreeNode(egyszov + " (" + egykod + ")");
                                        eredmenycsopnode.Nodes.Add(eredmkodnode);
                                        //if(bevsemainfoidk!=null)
                                        //{
                                        //    ArrayList ar = new ArrayList(fszazalidk);
                                        //    int pos = ar.IndexOf(bevsemainfoidk[l]);
                                        //    TreeNode kodnode = new TreeNode(egyszov + " (" + fszazalszovegek[pos] + ")");
                                        //    csopnode.Nodes.Add(kodnode);
                                        //}
                                        koltskodszovegek = FakUserInterface.GetTartal(koltsegkodok, "SZOVEG", "TERMEKKOD_ID", egykodid);
                                        koltskodkodok = FakUserInterface.GetTartal(koltsegkodok, "KOD", "TERMEKKOD_ID", egykodid);
                                        if (koltskodszovegek != null && koltskodszovegek.Length != 0)
                                        {
                                            for (int m = 0; m < koltskodszovegek.Length; m++)
                                            {
                                                TreeNode kodnode1 = new TreeNode(koltskodszovegek[m] + " (" + koltskodkodok[m] + ")");
                                                eredmkodnode.Nodes.Add(kodnode1);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < koltsfocsopidk.Length; i++)
            {
                string egykoltsfocsopid = koltsfocsopidk[i];
                string egykoltsfocsopszov = koltsfocsopszovegek[i];
                string egykoltsfocsopkod = koltsfocsopkodok[i];
//                TreeNode focsopnode = new TreeNode(egykoltsfocsopszov + " (" + egykoltsfocsopkod + ")");
//                treeView2.Nodes.Add(focsopnode);
                koltsalcsopidk = FakUserInterface.GetTartal(koltsfocsopalcsop, "SORSZAM2", "SORSZAM1", egykoltsfocsopid);
                if (koltsalcsopidk != null && koltsalcsopidk.Length != 0)
                {
                    koltsalcsopszovegek = FakUserInterface.GetTartal(koltsalcsopinfo, "SZOVEG", "SORSZAM", koltsalcsopidk);
                    koltsalcsopkodok = FakUserInterface.GetTartal(koltsalcsopinfo, "KOD", "SORSZAM", koltsalcsopidk);
                    for (int j = 0; j < koltsalcsopidk.Length; j++)
                    {
                        string egykoltsalcsopid = koltsalcsopidk[j];
                        string egykoltsalcsopszov = koltsalcsopszovegek[j];
                        string egykoltsalcsopkod = koltsalcsopkodok[j];
//                        TreeNode alcsopnode = new TreeNode(egykoltsalcsopszov + "(" + egykoltsalcsopkod + ")");
//                        focsopnode.Nodes.Add(alcsopnode);
                        koltscsopidk = FakUserInterface.GetTartal(koltsalcsopcsop, "SORSZAM2", "SORSZAM1", egykoltsalcsopid);
                        if (koltscsopidk != null && koltscsopidk.Length != 0)
                        {
                            koltscsopszovegek = FakUserInterface.GetTartal(koltscsopinfo, "SZOVEG", "KOLTSEGCSOPORT_ID", koltscsopidk);
                            koltscsopkodok = FakUserInterface.GetTartal(koltscsopinfo, "KOD", "KOLTSEGCSOPORT_ID", koltscsopidk);
                            for (int k = 0; k < koltscsopidk.Length; k++)
                            {
                                string egykoltscsopid = koltscsopidk[k];
                                string egykoltscsopszov = koltscsopszovegek[k];
                                string egykoltscsopkod = koltscsopkodok[k];
                                string[] semainfoidk = null;
                                string[] koltscsopkodidk = FakUserInterface.GetTartal(koltscsopkod, "SORSZAM", "SORSZAM1", egykoltscsopid);
                                if (koltscsopkodidk != null)
                                {
                                    semainfoidk = FakUserInterface.GetTartal(koltssemainfo, "SORSZAM2", "SORSZAM1", koltscsopkodidk);
                                if (semainfoidk != null)
                                {
                                    TreeNode csopnode = new TreeNode(); //(egykoltscsopszov ); //+ " (" + egykoltscsopkod + ")");
                                    treeView2.Nodes.Add(csopnode);
                                    koltskodidk = FakUserInterface.GetTartal(koltscsopkod, "SORSZAM2", "SORSZAM1", egykoltscsopid);
                                    if (koltskodidk != null && koltskodidk.Length != 0)
                                    {
                                        koltskodszovegek = FakUserInterface.GetTartal(koltsegkodok, "SZOVEG", "KOLTSEGKOD_ID", koltskodidk);
                                        koltskodkodok = FakUserInterface.GetTartal(koltsegkodok, "KOD", "KOLTSEGKOD_ID", koltskodidk);
                                        int osszazal = 0;
                                        ArrayList ar = new ArrayList(fszazalidk);
                                        for (int l = 0; l < koltskodidk.Length; l++)
                                        {
                                            int pos = ar.IndexOf(semainfoidk[l]);
                                            TreeNode kodnode = new TreeNode(koltskodszovegek[l]+" ("+koltskodkodok[l]+ ") - "+fszazalszovegek[pos]);
                                            csopnode.Nodes.Add(kodnode);
                                            osszazal+=Convert.ToInt32(fszazalkodok[pos]);
                                        }
                                        csopnode.Text=egykoltscsopszov+" ("+egykoltscsopkod+") - "+osszazal.ToString()+"%";
                                    }
                                }
                                }
                            }
                        }
                    }
                }
            }
            treeView1.ExpandAll();
            treeView2.ExpandAll();
            treeView3.ExpandAll();
            Panel.Controls.Add(this);
        }
        public override void ok_Click(object sender, EventArgs e)
        {
            Panel.Controls.Clear();
            for (int i = 0; i < controlok.Length; i++)
                Panel.Controls.Add(controlok[i]);
            Teszt.Visible = true;
        }
    }
}
