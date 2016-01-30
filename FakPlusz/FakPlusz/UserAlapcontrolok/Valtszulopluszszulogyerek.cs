using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FakPlusz.Alapfunkciok;
namespace FakPlusz.UserAlapcontrolok
{
    /// <summary>
    /// Valtozasok, szulo+szulogyerek tipusu UserControl
    /// </summary>
    public partial class Valtszulopluszszulogyerek : ControlAlap
    {
        /// <summary>
        /// objectum letrehozasa
        /// </summary>
        public Valtszulopluszszulogyerek()
        {
            InitializeComponent();
        }
        /// <summary>
        /// inicializalas, ha nincs valtozastipus es csak egy szulopanel van
        /// </summary>
        /// <param name="szulopanel">
        /// szulopanel
        /// </param>
        public void ValtszulopluszSzuloInit(Panel szulopanel)
        {
            ValtszulopluszSzuloInit(null, szulopanel);
        }
        /// <summary>
        /// inicializalas, ha csak valtozaspanelek vannak
        /// </summary>
        /// <param name="valtpanelek">
        /// valtozaspanelek tombje
        /// </param>
        public void ValtszulopluszSzuloInit(Panel[] valtpanelek)
        {
            ValtszulopluszSzuloInit(valtpanelek, null);
        }
        /// <summary>
        /// inicializalas, ha valtozaspanelek es egy szulopanel van
        /// </summary>
        /// <param name="valtpanelek">
        /// valtozaspanelek tombje
        /// </param>
        /// <param name="szulopanel">
        /// szulopanel
        /// </param>
        public void ValtszulopluszSzuloInit(Panel[] valtpanelek, Panel szulopanel)
        {
            ValtszulopluszSzuloInit(valtpanelek, new Panel[] {szulopanel}, null);
        }
        /// <summary>
        /// kozos inicializalas
        /// </summary>
        /// <param name="valtpanelek">
        /// valtozaspanelek tombje
        /// </param>
        /// <param name="szulopanelek">
        /// szulopanelek tombje
        /// </param>
        /// <param name="sajatpanelek">
        /// alap panelek tombje vagy null
        /// </param>
        public void ValtszulopluszSzuloInit(Panel[] valtpanelek, Panel[] szulopanelek, Panel[] sajatpanelek)
        {
            object[] alap = null;
            if (sajatpanelek != null)
                alap = new object[] { Alapinfotipus.Alap, sajatpanelek };
            object[] valt = null;
            object[] szulogy=null;
            Panel[] panelek = new Panel[] { panel1, panel2, panel4, panel5 ,panel11};
            ArrayList eredetiar = new ArrayList(panelek);
            ArrayList ar = new ArrayList(valtpanelek);
            bool[] van = new bool[5];
            for (int i = 0; i < van.Length; i++)
            {
                Panel eredeti = (Panel)eredetiar[i];
                if (ar.IndexOf(eredeti) != -1)
                    van[i] = true;
            }
            for (int i = 0; i < van.Length; i++)
            {
                if (!van[i])
                    this.Controls.Remove((Control)eredetiar[i]);
            }
            if (valtpanelek != null)
            {
                valt = new object[] { Alapinfotipus.Valtozasok, valtpanelek };
            }
            if(szulopanelek!=null)
                szulogy = new object[] { Alapinfotipus.SzuloGyerekValtozasok, szulopanelek };
            if (alap == null)
                AlapinfoInit(new object[] { valt, szulogy });
            else
                AlapinfoInit(new object[] { alap, valt, szulogy });
        }
    }
}
