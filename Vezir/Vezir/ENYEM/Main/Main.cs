using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;
using FakPlusz.Alapfunkciok;
using FakPlusz;
using FakPlusz.Alapcontrolok;
using FakPlusz.UserAlapcontrolok;
namespace Vezir
{
    public partial class Main : MainAlap
    {
        Version ver = new Version(1, 0, 0);
        public Main()
        {
            InitializeComponent();
            Alkalmazas = "VEZIR";
            MainControlNev = "MainControl";
            this.Text = this.Text + " " + ver.ToString();
        }
        public override bool AlkalmazasBejelentkezes()
        {
            Bejelentkezo = new Bejelentkezo();
            Bejelentkezo.BejelentkezoAlapInit(connstringek, Alkalmazas);
            return Bejelentkezo.Show(true,global::Vezir.Properties.Resources._1350376768_user8, System.Windows.Forms.ImageLayout.Zoom,"",this.Icon);
        }
        public override bool AlkalmazasMainControlIndit()
        {
            Bejelentkezo.FakUserInterface = FakUserInterface;
            FakUserInterface.OpenProgress();
            close = Bejelentkezo.Bejeltolt();
            if (close)
                return close;
            else
            {
                FakUserInterface.SetProgressText("");
                FakUserInterface.KezeloId = Convert.ToInt32(Bejelentkezo.Kezeloid);
                FakUserInterface.EventTilt = true;
                if (Bejelentkezo.AktivCegInformaciok != null)
                {
                    foreach (Ceginformaciok ceginfo in Bejelentkezo.AktivCegInformaciok)
                    {
                        if (!ceginfo.LezartCeg && ceginfo.KezeloiSzint.ToString().Contains("ezeto"))
                        {
                            ceginfo.UserJogosultsag = Base.HozferJogosultsag.Irolvas;
                            ceginfo.CegTermeszetesJogosultsag = Base.HozferJogosultsag.Irolvas;
                            ceginfo.CegSzarmazekosJogosultsag = Base.HozferJogosultsag.Irolvas;
                        }
                    }
                }
                if (Bejelentkezo.LezartCegInformaciok != null)
                {
                    foreach (Ceginformaciok ceginfo in Bejelentkezo.LezartCegInformaciok)
                    {
                        if (!ceginfo.LezartCeg && ceginfo.KezeloiSzint.ToString().Contains("ezeto"))
                        {
                            ceginfo.UserJogosultsag = Base.HozferJogosultsag.Irolvas;
                            ceginfo.CegTermeszetesJogosultsag = Base.HozferJogosultsag.Irolvas;
                            ceginfo.CegSzarmazekosJogosultsag = Base.HozferJogosultsag.Irolvas;
                        }
                    }
                }
                MainControl  = new MainControl();
                MainControl.Bejelentkezo = Bejelentkezo;
                MainControl.MainControlAlapInit(FakUserInterface, panel1, this,MainControl.UserControlNevek);
                FakUserInterface.EventTilt = false;
                FakUserInterface.CloseProgress();
                AktivControl.Dock = DockStyle.Fill;
                Base cont = AktivControl.AktivControl.AktivControl;
                panel1.Visible = true;
                if (cont != null)
                {
                    cont.Focus();
                }
                if (panel1.Controls.Count == 0)
                    return true;
            }
            return false;
        }
        public override void Main_Load(object sender, EventArgs e)
        {
            base.Main_Load(sender, e);
        }
        public override  void MainForm_Closeing(object sender, FormClosingEventArgs e)
        {
            if (!close)
            {
                if (UserParamTabla.HibasBevSzamla || UserParamTabla.HibasKoltsSzamla)
                {
                    string szov = "";
                    if (UserParamTabla.HibasBevSzamla)
                        szov = "Befejezetlen bevételi számlák vannak ";
                    if (UserParamTabla.HibasKoltsSzamla)
                    {
                        if (szov != "")
                            szov += "\n";
                        szov += "Befejezetlen költségszámlák vannak ";
                    }
                    FakPlusz.MessageBox.Show(szov);
                    e.Cancel = true;
                    return;
                }
                Base cont = AktivControl.AktivControl.AktivControl;
                if (cont != null)
                {
                    if (cont.Name == "Formvezerles" && cont.AktivControl!=null)
                    {
                        try
                        {
                            Alap control = (Alap)cont.AktivControl;
                            if(cont.AktivControl.Userabortkerdes(control.Tabinfo))
                            {
                                e.Cancel=true;
                                return;
                            }
                        }
                        catch
                        {
                            if(cont.AktivControl.Userabortkerdes())
                            {
                                e.Cancel=true;
                                return;
                            }
                        }
                    }
                    if (cont.Name != "Formvezerles" && cont.Userabortkerdes())
                    {
                        e.Cancel = true;
                        return;
                    }
                }
                base.MainForm_Closeing(sender, e);
                if (e.Cancel)
                    ((MainControl)MainControl).SetFocus();
            }
        }
    }
}
