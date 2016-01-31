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

namespace Tervezo
{
    public partial class TervControl : MainControlAlap
    {
        public TervControl()
        {
            InitializeComponent();
        }
        public override void EgyediInditas()
        {
            base.EgyediInditas();
            Vezerloinfo parentvez = Vezerles;
            Base control = new Formvezerles(FakUserInterface, panel2, panel1, parentvez, ref KezeloiSzint, ref HozferJog);
        }
    }
}
