using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FakPlusz.Alapfunkciok;
using FakPlusz;
using FakPlusz.Alapcontrolok;
using FakPlusz.UserAlapcontrolok;
using FakPlusz.Formok;
using FakPlusz.VezerloFormok;
using FakPlusz.SzerkesztettListak;

namespace Vezir
{
    public partial class Szerklist : Base
    {
        private VezerloControl VezerloControl;
        private Listaterv listaterv = null;
        private string[] valaszt;
        private string[] azontipek;
        private Tablainfo[] tabinfok;
        private TablainfoCollection infocoll;
        public Szerklist(FakUserInterface fak, VezerloControl hivo, Vezerloinfo aktivvezerles)
        {
            InitializeComponent();
            FakUserInterface = fak;
            VezerloControl = hivo;
            Vezerles = aktivvezerles;
            ValasztekInfo = FakUserInterface.GetBySzintPluszTablanev("C", "LISTA");
            infocoll = UserParamTabla.TermCegPluszCegalattiTablainfok;
            listaterv = new Listaterv(aktivvezerles);
            listaterv.Dock = DockStyle.Fill;
            listaterv.Hivo = this;
            listaterv.TablainfoTag = TablainfoTag;
            listapage.Controls.Add(listaterv);

        }
        public override void TabStop_Changed(object sender, EventArgs e)
        {
            if (this.TabStop)
            {
                bool valtozas = ValtozasLekerdez().Count != 0;
                if (valtozas)
                {
                    if (UserParamTabla.UserParamok != "" && UserParamTabla.AktualControlNev == this.Name )
                        ValasztekIndex = UserParamTabla.ValasztekIndex;
                    else
                    {
                        ValasztekIndex = 0;
                        UserParamTabla.ValasztekIndex = 0;
                    }
                    Valasztekallitas(ValasztekIndex);
                    UserParamTabla.Infotoltkell = true;
                }
            }
        }
        private void Valasztekallitas(int ind)
        {
            foreach (Tablainfo info in UserParamTabla.TermCegPluszCegalattiTablainfok)
                info.AktualControlInfo = null;
            Valasztek.Items.Clear();
            DataTable dt = new DataTable("TARTAL");
            Sqlinterface.Select(dt, FakUserInterface.Rendszerconn, "TARTAL", " where AZON = 'SZCL' AND OWNER = "+FakUserInterface.AlkalmazasId, "", false);
            valaszt = new string[dt.Rows.Count];
            azontipek = new string[dt.Rows.Count];
            tabinfok = new Tablainfo[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                valaszt[i] = dr["SZOVEG"].ToString();
                azontipek[i]= dr["AZONTIP"].ToString();
                tabinfok[i] = FakUserInterface.GetByAzontip(azontipek[i]);
            }
            Valasztek.Items.Clear();
            Valasztek.Items.AddRange(valaszt);
            if (valaszt.Length <= ind)
                ValasztekIndex = 0;
            else
                ValasztekIndex=ind;
             UserParamTabla.ValasztekIndex = ind;
            ValtozasTorol();
            Tabinfo = tabinfok[ValasztekIndex];
            listaterv.TablainfoTag = Tabinfo.TablaTag;
            Valasztek.Text = valaszt[ValasztekIndex];
            listaterv.UjTag = true;
            listaterv.TabStop = false;
            listaterv.TabStop = true;
        }
        public override void RogzitesUtan()
        {
            Valasztekallitas(ValasztekIndex);
        }
        private void Valasztek_SelectedIndexChanged(object sender, EventArgs e)
        {
                bool valt = ValasztekIndex != Valasztek.SelectedIndex;
                if (valt)
                {
                    if (Valasztek.SelectedIndex == -1)
                        Valasztek.SelectedIndex = 0;
                    ValasztekIndex = Valasztek.SelectedIndex;
                    UserParamTabla.ValasztekIndex = ValasztekIndex;
                    Tabinfo = tabinfok[ValasztekIndex];
                    listaterv.TablainfoTag = Tabinfo.TablaTag;
                    listaterv.UjTag = true;
                    listaterv.TabStop = false;
                    listaterv.TabStop = true;
            }
        }
    }
}
