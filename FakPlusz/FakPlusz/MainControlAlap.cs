using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using FakPlusz.Alapfunkciok;
using FakPlusz;
using FakPlusz.Alapcontrolok;
using FakPlusz.Formok;
using FakPlusz.VezerloFormok;

namespace FakPlusz
{
    public partial class MainControlAlap : Base
    {
        public MainAlap MainForm;
        //        public Vezerloinfo Vezerles;
        public Panel MainPanel;
        public VezerloinfoCollection VezerloinfoCollection = new VezerloinfoCollection();
        public Bejelentkezo Bejelentkezo;
        //        private Ceginformaciok[] AktivCeginformaciok = null;
        public TreeView EgyTreeView;
        public ArrayList CegPluszCegszintuTablanevek = new ArrayList();
        public Ceginformaciok AktualCeginformacio;
        public Formvezerles FormVezerles;
        public ArrayList UserControlNevek = null;
        public MainControlAlap()
        {
            InitializeComponent();
        }
        public virtual void MainControlAlapInit(FakUserInterface fakuserinterface, Panel mainpanel, MainAlap maincont)
        {
            MainControlAlapInit(fakuserinterface, mainpanel, maincont, null);
        }
        public virtual void MainControlAlapInit(FakUserInterface fakuserinterface, Panel mainpanel, MainAlap maincont, ArrayList usercontnevek)
        {
            MainForm = maincont;
            this.TabStop = false;
            FakUserInterface = fakuserinterface;
            MainPanel = mainpanel;
            UserControlNevek = usercontnevek;
            this.Dock = DockStyle.Fill;
            MainPanel.Controls.Add(this);
            KezeloiSzint = Base.KezSzint.Fejleszto;
            HozferJog = Base.HozferJogosultsag.Irolvas;
            Valtozaskezeles.ValtozaskezelesInit(FakUserInterface, MainForm.Alkalmazas);
            MainForm.AktivControl = this;
            EgyediInditas();
            MainPanel.Visible = true;
        }
        public virtual void EgyediInditas()
        {
            Vezerles = new Vezerloinfo(FakUserInterface, MainForm.MainControlNev, null, ref KezeloiSzint, ref HozferJog, UserControlNevek);
 //           Vezerloinfo parentvez = Vezerles;
            Vezerles.Control = this;
        }
    }
}