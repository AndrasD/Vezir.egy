using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FakPlusz.Alapfunkciok;
namespace FakPlusz.Alapcontrolok
{
    /// <summary>
    /// olyan usercontrol, ahol csak DataGridView van, nincs inputgridview
    /// </summary>
    public partial class Csakgrid : Alap
    {
        /// <summary>
        /// objectum eloallitasa
        /// </summary>
        public Csakgrid()
        {
            InitializeComponent();
        }
    }
}
