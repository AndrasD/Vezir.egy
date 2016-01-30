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
    public partial class CegAllapotok : Base
    {
        Panel Panel;
        Control[] controlok;
        MainAlap Main;
        ToolStripMenuItem Cegall;
        public CegAllapotok(FakUserInterface fak, Panel panel, ToolStripMenuItem cegall, MainAlap main)
        {
            InitializeComponent();
            FakUserInterface = fak;
            Panel = panel;
            Main = main;
            Cegall = cegall;
            this.Dock = DockStyle.Fill;
        }
        public void ShowCegall()
        {
            textBox1.TextAlign = HorizontalAlignment.Left;
            controlok = new Control[Panel.Controls.Count];
            for (int i = 0; i < Panel.Controls.Count; i++)
                controlok[i] = Panel.Controls[i];
            Panel.Controls.Clear();
            rendben.Text = "Rendben                     " + UserParamTabla.AktualCegInformacio.CegNev + " aktuális állapota";
            string szov = "\r\n\r\n";
            szov += "Indulás dátuma " + UserParamTabla.InduloDatumString + "        " +
                    "Aktuális dátum " + UserParamTabla.AktualisDatumString + "        ";
            if (UserParamTabla.AktualCegInformacio.LezarasDatuma.CompareTo(DateTime.MaxValue) != 0)
                szov += "Lezárás dátuma " + UserParamTabla.AktualCegInformacio.LezarasDatuma.ToShortDateString();
            if (UserParamTabla.LezartCeg)
                szov += "               A cég lezárt";
            else
                szov += "               A cég aktiv";
            szov += "\r\n\r\n";
            string ev = UserParamTabla.SzamlaDatumtol.Year.ToString();
            string lez = "A " + ev + ".év nincs lezárva";
            if (UserParamTabla.LezartEvek.IndexOf(UserParamTabla.SzamlaDatumtol.Year.ToString()) != -1)
                lez = "A " + ev + ".év le van zárva";
            szov += lez + "\r\n\r\n";
            KezSzint kezszint = UserParamTabla.AktualCegInformacio.KezeloiSzint;
            string kez = SzovegesKezeloiSzint[(int)kezszint];
            szov += "Kezelöi szerep                     " + kez;
            szov += "\r\n\r\n";
            HozferJogosultsag hozf;
            string userjog = HozferStringek[Convert.ToInt32(UserParamTabla.UserHozferJogosultsag)];
            if (UserParamTabla.UserHozferJogosultsag == HozferJogosultsag.Irolvas)
            {
                hozf = FakUserInterface.GetBySzintPluszTablanev("U", "KEZELOK").Azonositok.Jogszintek[(int)kezszint];
                userjog = HozferStringek[(int)hozf];
            }
            string szarmjog = HozferStringek[(int)UserParamTabla.CegSzarmazekosJogosultsag];
            string termjog = HozferStringek[(int)UserParamTabla.AktualTermeszetesJogosultsag];
            if (UserParamTabla.KozosAllapotSzovegek.Count != 0 && termjog != "Semmi")
                termjog = "Semmi, lásd Mi hiányzik?";
            if (kez == "Rendszergazda")
                szarmjog = "Semmi";
            else
            {
                hozf = FakUserInterface.GetKodtab("C","Termfocsop").Azonositok.Jogszintek[(int)UserParamTabla.AktualCegInformacio.KezeloiSzint];
                if (LezartCeg && hozf == HozferJogosultsag.Irolvas)
                    hozf = HozferJogosultsag.Csakolvas;
                szarmjog = HozferStringek[(int)hozf];
            }
            szov += "Hozzáférési jog                    Param.adatok: " + userjog + "       Törzsadatok: " + szarmjog + "       Többi adat: " + termjog + "\r\n\r\n";
            if (UserParamTabla.KozosAllapotSzovegek.Count == 0)
                szov += "Kiegyenlitetlen számlák        Bevételi:     " + UserParamTabla.Kifizetetlenbevszladb.ToString() + "                    Költség:     " + UserParamTabla.Kifizetetlenkoltsszladb + "\r\n\r\n";
            if (UserParamTabla.HibasBevSzamla)
                szov += "Van hibás bevételi számla \r\n\r\n";
            if (UserParamTabla.HibasKoltsSzamla)
                szov += "Van hibás költségszámla \r\n\r\n";
            if(!UserParamTabla.HibasBevSzamla && !UserParamTabla.HibasKoltsSzamla)
                szov += "Nincs hibás számla\r\n\r\n";
            textBox1.Text = szov;
            textBox1.SelectionStart = 0;
            textBox1.SelectionLength = 0;
            textBox1.SelectedText = "";
            Panel.Controls.Add(this);
        }
        private void rendben_Click(object sender, EventArgs e)
        {
            Panel.Controls.Clear();
            for (int i = 0; i < controlok.Length; i++)
                Panel.Controls.Add(controlok[i]);
  //          Cegall.Visible = false;
            Cegall.Visible = true;
        }
    }
}
