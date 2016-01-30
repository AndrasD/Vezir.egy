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
    /// Valtozasok + Szulogyerek tipusu UserControl
    /// </summary>
    public partial class Valtpluszszulogyerek : ControlAlap
    {
        /// <summary>
        /// dataview1: szulo
        /// </summary>
 //       public DataView dataView1;
        /// <summary>
        /// dataview2:gyerek
        /// </summary>
//        public DataView dataView2;
        /// <summary>
        /// objectum letrehozasa
        /// </summary>
        public Valtpluszszulogyerek()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Inicializalas
        /// </summary>
        /// <param name="valtpanelek">
        /// valtozaspanelek
        /// </param>
        /// <param name="szulopanelek">
        /// szulopanelek
        /// </param>
        public void ValtpluszSzuloInit(Panel[] valtpanelek, Panel[] szulopanelek)
        {
            ValtpluszSzuloInit(valtpanelek, szulopanelek, null);
        }
        /// <summary>
        /// Inicializalas esetleges Alap tipusu groupboxokkal
        /// </summary>
        /// <param name="valtpanelek">
        /// valtozaspanelek
        /// </param>
        /// <param name="szulopanelek">
        /// szulopanelek
        /// </param>
        /// <param name="sajatpanelek">
        /// sajatpanelek tombje vagy null
        /// </param>
        public void ValtpluszSzuloInit(Panel[] valtpanelek, Panel[] szulopanelek, Panel[] sajatpanelek)
        {
            object[] alap = null;
            if (sajatpanelek != null)
                alap = new object[] { Alapinfotipus.Alap, sajatpanelek };
            switch (valtpanelek.Length)
            {
                case 1:
                    this.Controls.Remove(panel2);
                    this.Controls.Remove(panel3);
                    this.Controls.Remove(panel4);
                    break;
                case 2:
                    this.Controls.Remove(panel3);
                    this.Controls.Remove(panel4);
                    break;
                case 3:
                    this.Controls.Remove(panel4);
                    break;
            }
            object[] valt = new object[] { Alapinfotipus.Valtozasok, valtpanelek };
            if (szulopanelek.Length == 1)
                this.Controls.Remove(panel6);
            object[] szulogy = new object[] { Alapinfotipus.Szulo, szulopanelek };
            if (alap == null)
                AlapinfoInit(new object[] { valt, szulogy });
            else
                AlapinfoInit(new object[] { alap, valt, szulogy });
        }
    }
}
