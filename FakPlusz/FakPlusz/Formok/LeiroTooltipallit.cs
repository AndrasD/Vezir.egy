using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FakPlusz.Alapfunkciok;
using FakPlusz.Alapcontrolok;

namespace FakPlusz.Formok
{
    /// <summary>
    /// 
    /// </summary>
    public partial class LeiroTooltipallit : Tooltipallitkozos
    {
        /// <summary>
        /// objectum letrehozasa
        /// </summary>
        /// <param name="vezerles">
        /// vezerloinformacio
        /// </param>
        public LeiroTooltipallit(Vezerloinfo vezerles)
        {
            InitializeComponent();
            ParameterAtvetel(vezerles, true);
        }
    }
}
