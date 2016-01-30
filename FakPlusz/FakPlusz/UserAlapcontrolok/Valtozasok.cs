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
    /// Valtozasok tipusu paneleket es esetleg Alap tipusu groupbox(oka)t tartalmazo UserControl
    /// </summary>
    public partial class Valtozasok : ControlAlap
    {
        /// <summary>
        /// objectum letrehozasa
        /// </summary>
        public Valtozasok()
        {
            InitializeComponent();
        }
        /// <summary>
        /// inicializalas, ha csak Valtozasok tipusu paneleket tartalmaz
        /// </summary>
        /// <param name="valtpanelek">
        /// a panelek tombje
        /// </param>
        public virtual void ValtozasokInit(Panel[] valtpanelek)
        {
            ValtozasokInit(valtpanelek, null);
        }
        /// <summary>
        /// inicializalas, ha Alap tipusunk is van
        /// </summary>
        /// <param name="valtpanelek">
        /// a panelek tombje
        /// </param>
        /// <param name="sajatpanelek">
        /// Alap tipusu panelek tombje vagy null
        /// </param>
        public virtual void ValtozasokInit(Panel[] valtpanelek, Panel[] sajatpanelek)
        {
            object[] alap = null;
            if (sajatpanelek != null)
                alap = new object[] { Alapinfotipus.Alap, sajatpanelek };
            object[] valt = new object[] { Alapinfotipus.Valtozasok, valtpanelek };
            Panel[] panelek = new Panel[]{panel1,panel2,panel3,panel4,panel5};
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
            //switch (valtpanelek.Length)
            //{
            //    case 1:
            //        this.Controls.Remove(panel2);
            //        this.Controls.Remove(panel3);
            //        this.Controls.Remove(panel4);
            //        this.Controls.Remove(panel5);
            //        break;
            //    case 2:
            //        this.Controls.Remove(panel3);
            //        this.Controls.Remove(panel4);
            //        this.Controls.Remove(panel5);
            //        break;
            //    case 3:
            //        this.Controls.Remove(panel4);
            //        this.Controls.Remove(panel5);
            //        break;
            //    case 4:
            //        this.Controls.Remove(panel5);
            //        break;
            //}
            if (alap == null)
                AlapinfoInit(new object[] { valt });
            else
                AlapinfoInit(new object[] { alap, valt });
        }
    }
}
