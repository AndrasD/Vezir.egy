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

namespace Tervezo
{
    public partial class Main : MainAlap
    {
        public Base Formvezerles;
        public Main()
        {
            InitializeComponent();
            Alkalmazas = "TERVEZO";
            MainControlNev = "TervControl";
        }
        public override void Main_Load(object sender, EventArgs e)
        {
            base.Main_Load(sender, e);
        }
        public override bool AlkalmazasMainControlIndit()
        {
            FakUserInterface.OpenProgress("Tervezö inicializálás");
            FakUserInterface.SetProgressText("");
            FakUserInterface.ProgressRefresh();
            MainControl  = new TervControl();
            MainControl.MainControlAlapInit(FakUserInterface, panel1, this);
            FakUserInterface.CloseProgress();
            return false;
        }
        public override bool AlkalmazasBejelentkezes()
        {
            return false;
        }
        public override void MainForm_Closeing(object sender, FormClosingEventArgs e)
        {
            base.MainForm_Closeing(sender, e);
        }
    }
}
