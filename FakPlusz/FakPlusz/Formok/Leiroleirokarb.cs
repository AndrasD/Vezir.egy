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
    /// LEIRO tabla karbantartasa
    /// </summary>
    public partial class Leiroleirokarb : Gridpluszinput
    {
        /// <summary>
        /// objectum letrehozasa
        /// </summary>
        /// <param name="vezerles">
        /// vezerloinformacio
        /// </param>
        public Leiroleirokarb(Vezerloinfo vezerles)
        {
            InitializeComponent();
            ParameterAtvetel(vezerles, true);
        }
    }
}
