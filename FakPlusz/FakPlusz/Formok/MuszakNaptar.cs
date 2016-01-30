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
using FakPlusz.VezerloFormok;

namespace FakPlusz.Formok
{
    /// <summary>
    /// Muszakok naptara, ha van ilyen
    /// </summary>
    public partial class MuszakNaptar : Naptar
    {
        private TervTreeView terv;
        /// <summary>
        /// Letrehozas
        /// </summary>
        public MuszakNaptar()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Parameterek atvetele
        /// </summary>
        /// <param name="vezerles"></param>
        /// <param name="leiroe"></param>
        public override void ParameterAtvetel(Vezerloinfo vezerles, bool leiroe)
        {
            base.ParameterAtvetel(vezerles, leiroe);
            Formvezerles formhivo = (Formvezerles)Hivo;
            terv = formhivo.terv;
            Datumtol = terv.Datumtol;
            ValasztekIndex = terv.ValasztekIndex;
            ValasztekTablaNev = "KODTAB";
            ValasztekIdNev = "SORSZAM";
            AlapTablaNev = "NAPTARAK";
            AlapTablaSelectString = "";
            AlapIdNev = "KOD_ID";

//            Parameterez.ValasztekIndex = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        public override void ValasztekParameterekInit()
        {
            ValasztekInfo = FakUserInterface.GetKodtab("C", "Muszak");
            ValasztekItemek = FakUserInterface.GetTartal(ValasztekInfo, "SZOVEG", "SORSZAM", "");
            ValasztekIdk = FakUserInterface.GetTartal(ValasztekInfo, "SORSZAM", "SZOVEG", ValasztekItemek);
            if (ValasztekIdk.Length < ValasztekIndex || ValasztekIndex == -1)
                ValasztekIndex = 0;

        }
    }
}
