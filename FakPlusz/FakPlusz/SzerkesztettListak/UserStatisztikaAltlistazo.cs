using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FakPlusz;
using FakPlusz.Alapfunkciok;
using FakPlusz.Alapcontrolok;
using FakPlusz.UserAlapcontrolok;

namespace FakPlusz.SzerkesztettListak
{
    public partial class UserStatisztikaAltlistazo : Altlistazoalap
    {
        public UserStatisztikaAltlistazo(FakUserInterface fak, Base hivo, Vezerloinfo vezerles)
        {
            InitializeComponent();
        }
    }
}
