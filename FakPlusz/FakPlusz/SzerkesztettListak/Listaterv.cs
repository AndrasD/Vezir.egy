using System.Text;
using System.Windows.Forms;
using FakPlusz.Alapcontrolok;
using FakPlusz.Alapfunkciok;
using FakPlusz.UserAlapcontrolok;

namespace FakPlusz.SzerkesztettListak
{
    /// <summary>
    /// Szerkesztett listak tervezesenek UserControlja. Listatervalap-tol orokol
    /// </summary>
    public partial class Listaterv : Listatervalap
    {
        /// <summary>
        /// objectum letrehozasa
        /// </summary>
        /// <param name="vezerles">
        /// vezerloinformacio
        /// </param>
        public Listaterv(Vezerloinfo vezerles)
        {
            InitializeComponent();
            listae = true;
            Vezerles = vezerles;
            FakUserInterface = vezerles.Fak;
            KezeloiSzint = vezerles.KezeloiSzint;
            HozferJog = Vezerles.HozferJog;
            AktivMenuItem = vezerles.AktivMenuItem;
            AktivDropDownItem = vezerles.AktivDropDownItem;
            AktivPage = vezerles.AktivPage;
        }
        /// <summary>
        /// Aktivizalas,ujraaktivizalas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void TabStop_Changed(object sender, System.EventArgs e)
        {
            base.TabStop_Changed(sender, e);
        }
        /// <summary>
        /// Fo buttonok click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void fobuttonok_Click(object sender, System.EventArgs e)
        {
                base.fobuttonok_Click(sender, e);
        }

    }
}
