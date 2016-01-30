using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FakPlusz;
using FakPlusz.Alapcontrolok;
using FakPlusz.Alapfunkciok;
using FakPlusz.UserAlapcontrolok;

namespace FakPlusz.SzerkesztettListak
{
    /// <summary>
    /// Altalanos listazo UserControl
    /// </summary>
    public partial class Altlistazo : Altlistazoalap
    {
        public Altlistazo(FakUserInterface fak, Base hivo, Vezerloinfo vezerles)
        {
            InitializeComponent();
            FakUserInterface = fak;
            Hivo = hivo;
            Vezerles = vezerles;
            Valtozaskezeles = vezerles.Valtozaskezeles;
            HozferJog = vezerles.HozferJog;
            KezeloiSzint = vezerles.KezeloiSzint;
        }
    }
}
