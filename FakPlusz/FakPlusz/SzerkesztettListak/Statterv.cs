using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FakPlusz.Alapcontrolok;
using FakPlusz.Alapfunkciok;
using FakPlusz.UserAlapcontrolok;

namespace FakPlusz.SzerkesztettListak
{
    /// <summary>
    /// Szerkesztett statisztikak tervezesenek alapja. Listatervalaptol orokol
    /// </summary>
    public partial class Statterv : Listatervalap
    {
        /// <summary>
        /// objectum letrehozasa
        /// </summary>
        /// <param name="vezerles">
        /// vezerloinfo
        /// </param>
        public Statterv(Vezerloinfo vezerles)
        {
            InitializeComponent();
            listae = false;
            Vezerles = vezerles;
            FakUserInterface = vezerles.Fak;
            KezeloiSzint = vezerles.KezeloiSzint;
            HozferJog = Vezerles.HozferJog;
            AktivMenuItem = vezerles.AktivMenuItem;
            AktivDropDownItem = vezerles.AktivDropDownItem;
            AktivPage = vezerles.AktivPage;
        }
    }
}
