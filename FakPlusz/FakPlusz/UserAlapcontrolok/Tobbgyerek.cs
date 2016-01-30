using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FakPlusz.Alapfunkciok;

namespace FakPlusz.UserAlapcontrolok
{
    /// <summary>
    /// Tobb gyerek van
    /// </summary>
    public partial class Tobbgyerek : ControlAlap
    {
        /// <summary>
        /// objectum letrehozasa
        /// </summary>
        public Tobbgyerek()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Inicializalas
        /// </summary>
        /// <param name="gyerekpanelek">
        /// gyerekpanelek tombje
        /// </param>
        public void TobbgyerekInit(Panel[] gyerekpanelek)
        {
            object[] tobbgy = new object[] { Alapinfotipus.Tobbgyerek, new Panel[] { panel1 } };
            switch (gyerekpanelek.Length)
            {
                case 2:
                    panel1.Controls.Remove(panel14);
                    panel1.Controls.Remove(panel15);
                    break;
                case 3:
                    panel1.Controls.Remove(panel15);
                    break;
            }
            AlapinfoInit(new object[] { tobbgy });
        }
    }
}
