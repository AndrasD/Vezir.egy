using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FakPlusz.Alapcontrolok;
using FakPlusz.Alapfunkciok;
namespace FakPlusz.Formok
{
    /// <summary>
    /// Minden leirotablainformacio karbantartasat szolgalo UserControl
    /// </summary>
    public partial class Leirokarb : Gridpluszinput
    {
        /// <summary>
        /// objectum letrehozasa
        /// </summary>
        /// <param name="vezerles">
        /// vezerloinformacio
        /// </param>
        public Leirokarb(Vezerloinfo vezerles)
        {
            InitializeComponent();
            ParameterAtvetel(vezerles, true);
        }
    }
}
