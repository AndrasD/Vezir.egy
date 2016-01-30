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
    /// Szulogyerekvaltozasok tipusu UserControl
    /// </summary>
    public partial class Szulogyerekvaltozasok : ControlAlap
    {
        /// <summary>
        /// objectum letrehozasa
        /// </summary>
        public Szulogyerekvaltozasok()
        {
            InitializeComponent();
        }
        /// <summary>
        /// inicializalas
        /// </summary>
        public void SzuloGyerekInit()
        {
            SzuloGyerekInit(null);
        }
        /// <summary>
        /// inicializalas esetleg alap tipusu groupboxokkal
        /// </summary>
        /// <param name="sajatgroupboxok">
        /// groupboxok tombje vagy null
        /// </param>
        public void SzuloGyerekInit(GroupBox[] sajatgroupboxok)
        {
            object[] alap = null;
            if (sajatgroupboxok != null)
                alap = new object[] { Alapinfotipus.Alap, sajatgroupboxok };
            object[] szulogy = new object[] { Alapinfotipus.SzuloGyerekValtozasok, new Panel[] { panel1} };
            if (alap == null)
                AlapinfoInit(new object[] { szulogy });
            else
                AlapinfoInit(new object[] { alap, szulogy });

        }
    }
}
