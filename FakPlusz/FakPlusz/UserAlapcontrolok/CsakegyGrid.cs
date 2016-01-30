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
    /// 
    /// </summary>
    public partial class CsakegyGrid : ControlAlap
    {
        /// <summary>
        /// 
        /// </summary>
        public CsakegyGrid()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        public void CsakegyGridInit()
        {
            CsakegyGridInit(null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sajatgroupboxok"></param>
        public void CsakegyGridInit(GroupBox[] sajatgroupboxok)
        {
            object[] alap = null;
            if (sajatgroupboxok != null)
                alap = new object[] { Alapinfotipus.Alap, sajatgroupboxok };
            object[] szulogy = new object[] { Alapinfotipus.CsakDataGrid, new Panel[] { panel1 } };
            if (alap == null)
                AlapinfoInit(new object[] { szulogy });
            else
                AlapinfoInit(new object[] { alap, szulogy });
            groupBox112.Enabled = false;
            this.Controls.Remove(toolStripfo);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="egycont"></param>
        /// <param name="kellchild"></param>
        public override void ButtonokEnableAllit(Controltipus egycont, bool kellchild)
        {
            base.ButtonokEnableAllit(egycont, kellchild);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }

    }
}
