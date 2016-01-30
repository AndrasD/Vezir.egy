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

namespace Vezir
{
    public partial class Ujhonap : ControlAlap
    {
        private VezerloControl VezerloControl;
        private Tablainfo ceghoinfo = UserParamTabla.Ceghonapinfo;
        private Tablainfo cegszerzinfo = UserParamTabla.Cegszerzodesinfo;
        private Tablainfo cegevinfo = UserParamTabla.Cegevinfo;
        private int regiev;
        private int ujev;
        private DateTime regimaxho;
        private DateTime ujmaxho;
        private string regievho;
        private string ujevho;
        private string ceghoid;
        string[] nevek = new string[] {"BEVSZLA","KOLTSSZLA","BANKBOLBANKBA","BANKBOLPENZTARBA","BANKIMOZGAS","PENZTARBOLBANKBA",
                "PENZTARBOLPENZTARBA","PENZTARMOZGAS"};
        public Ujhonap(FakUserInterface fak, VezerloControl hivo, Vezerloinfo aktivvezerles)
        {
            InitializeComponent();
            ParameterAtvetel(fak, hivo, aktivvezerles);
            VezerloControl = hivo;
            AlapinfoInit(new object[] { new object[] { Alapinfotipus.Alap, new Panel[] { panel1, panel2} } });
            teljesrogzit.Text = "Új hónap rögzitése";
            teljeselolrol.Text = "Hónap törlés ";
        }
        public override void Ujcontroloktolt()
        {
            ValtozasTorol();
            regimaxho = UserParamTabla.AktualisDatum;
            regiev = regimaxho.Year;
            regievho = FakUserInterface.DatumToString(regimaxho).Substring(0, 7);
            ujmaxho = regimaxho.AddMonths(1);
            ujevho = FakUserInterface.DatumToString(ujmaxho).Substring(0, 7);
            textBox1.Text = ujevho;
            panel2.Visible = UserParamTabla.LezartEvek.IndexOf(regiev.ToString()) == -1;
            teljeselolrol.Visible = UserParamTabla.LezartEvek.IndexOf(regiev.ToString()) == -1; // && regimaxho != UserParamTabla.InduloDatum;
            textBox2.Text = regievho;
            ujev = ujmaxho.Year;
            teljesrogzit.Enabled = HozferJog == Base.HozferJogosultsag.Irolvas && !UserParamTabla.HibasBevSzamla && !UserParamTabla.HibasKoltsSzamla;
            teljeselolrol.Enabled = HozferJog == Base.HozferJogosultsag.Irolvas && UserParamTabla.LezartEvek.IndexOf(regiev.ToString()) == -1 && (regimaxho != UserParamTabla.InduloDatum || VanAdat() != 0);
            //if (teljeselolrol.Enabled)
            //{
            //    DataTable dt = new DataTable();
            //    dt = Sqlinterface.Select(dt,FakUserInterface.AktualCegconn,"CEGSZLAHONAPOK"," where EVHONAP = '"+regievho +"'","",false);
            //    string  ceghoid = dt.Rows[0]["CEGHONAP_ID"].ToString();
            //    dt.Rows.Clear();
            //    dt = Sqlinterface.Select(dt, FakUserInterface.AktualCegconn, "BEVSZLA", "where FIZETVE='N' AND CEGHONAP_ID = " + ceghoid, "", true);
            //    teljeselolrol.Enabled = dt.Rows.Count == 0;
            //}
        }
        private int VanAdat()
        {
            int darab = 0;
            string conn = FakUserInterface.AktualCegconn;
            DataTable dt = new DataTable();
            dt = Sqlinterface.Select(dt, conn, "CEGSZLAHONAPOK", "where EVHONAP = '" + regievho + "'", "", false);
            ceghoid = dt.Rows[0]["CEGHONAP_ID"].ToString();
            string sel = "where CEGHONAP_ID = " + ceghoid;
            for (int i = 0; i < nevek.Length; i++)
            {
                dt.Rows.Clear();
                dt.TableName = nevek[i];
                Sqlinterface.Select(dt, conn, nevek[i], sel, "", false);
                darab += dt.Rows.Count;
            }
            return darab;
        }
        public override void ButtonokEnableAllit(Controltipus egycont, bool kellchild)
        {
        }
        public override void teljesrogzit_Click(object sender, EventArgs e)
        {
            DataRow dr = cegszerzinfo.DataView[0].Row;
            if (regiev != ujev)
            {
                cegevinfo.ViewSorindex = -1;
                DataRow ujsor = cegevinfo.Ujsor();
                ujsor["EV"] = ujev;
                ujsor["LEZART"] = "N";
                ujsor["KELLZARAS"] = "I";
                FakUserInterface.UpdateTransaction(new Tablainfo[] { cegevinfo });
            }
            dr["AKTUALISDATUM"] = ujmaxho;
            dr["MODOSITOTT_M"] = 1;
            cegszerzinfo.Modositott = true;
            FakUserInterface.ValtoztatasFunkcio = "MODIFY";
            cegszerzinfo.ValtozasNaplozas(dr);
            ceghoinfo.ViewSorindex = -1;
            dr = ceghoinfo.Ujsor();
            dr["SZLA_DATUM"] = ujmaxho;
            dr["EVHONAP"] = FakUserInterface.DatumToString(ujmaxho).Substring(0, 7);
            dr["CEGEV_ID"] = UserParamTabla.Cegevinfo.Adattabla.Rows[0]["CEGEV_ID"];
            ceghoinfo.ValtozasNaplozas(dr);
            FakUserInterface.UpdateTransaction(new Tablainfo[] { cegszerzinfo, ceghoinfo });
            UserParamTabla.AktualCegInformacio.AktualisDatum = ujmaxho;
            UserParamTabla.SzamlaDatumtol = ujmaxho;
            VezerloControl.WriteLoginfo();
            FakUserInterface.OpenProgress();
            ArrayList ar = new ArrayList(UserParamTabla.AktualCegInformaciok);
            int i = ar.IndexOf(UserParamTabla.AktualCegInformacio);
            UserParamTabla.SetAktualCeginformacio(UserParamTabla.LezartCeg,i);
            UserParamTabla.UserParamokFrissit();
            FakUserInterface.CloseProgress();
            UserParamTabla.SetKozosAllapotok();
            ValtozasBeallit();
            UserParamTabla.Infotoltkell = true;
            Ujcontroloktolt();
        }
        public override void teljeselolrol_Click(object sender, EventArgs e)
        {
            int darab = 0;
            string conn = FakUserInterface.AktualCegconn;
            DataTable dt = new DataTable();
            string[] torlendonevek = UserParamTabla.AdatbeviteliTablaNevek;// new string[] {"BEVSZLA_TETEL","KOLTSSZLA_TETEL","BEVSZLA","KOLTSSZLA","BANKBOLBANKBA","BANKBOLPENZTARBA","BANKIMOZGAS","PENZTARBOLBANKBA",
            //    "PENZTARBOLPENZTARBA","PENZTARMOZGAS"};
            darab = VanAdat();
            string sel = "where CEGHONAP_ID = " + ceghoid;
            if (darab == 0 || darab > 0 && FakPlusz.MessageBox.Show("Elveszithetünk " + darab.ToString() + " adatot?", "", FakPlusz.MessageBox.MessageBoxButtons.IgenNem) == FakPlusz.MessageBox.DialogResult.Igen)
            {
                foreach (string torlendo in torlendonevek)
                {
                    dt.Rows.Clear();
                    dt.TableName = torlendo;
                    Sqlinterface.SpecCommand(dt, conn, torlendo, "DELETE from " + torlendo + " " + sel, "");
                }
                ujmaxho = regimaxho;
                ujev = ujmaxho.Year;
                if (UserParamTabla.AktualisDatum != UserParamTabla.InduloDatum)
                {
                    regiev = regimaxho.Year;
                    regimaxho = regimaxho.AddMonths(-1);
                    if (ceghoinfo.Rows.Count > 1 || ujev != regiev && UserParamTabla.OsszesEv.Count > 1)
                    {
                        ceghoinfo.Adattabla.Rows[0].Delete();
                        ceghoinfo.Modositott = true;
                    }
                    UserParamTabla.AktualisDatum = regimaxho;
                    cegszerzinfo.Adattabla.Rows[0]["AKTUALISDATUM"] = UserParamTabla.AktualisDatum;
                    cegszerzinfo.Adattabla.Rows[0]["MODOSITOTT_M"] = 1;
                    cegszerzinfo.Modositott = true;
                }
                else
                {
                    sel = " where BEVKIADEV = '"+ujev + "'";
                    torlendonevek = new string[] { "AFAEGYENLEG", "BEVETELKIADAS" };
                    foreach (string torlendo in torlendonevek)
                    {
                        dt.Rows.Clear();
                        dt.TableName = torlendo;
                        Sqlinterface.SpecCommand(dt, conn, torlendo, "DELETE from " + torlendo + " " + sel, "");

                    }

                }
                if(ujev!=regiev && UserParamTabla.OsszesEv.Count > 1)
                {
                    cegevinfo.DataView.RowFilter="EV = "+ujev.ToString();
                    cegevinfo.DataView[0].Row.Delete();
                    cegevinfo.Modositott=true;
                    cegevinfo.DataView.RowFilter="";
                }
                FakUserInterface.Rogzit(new Tablainfo[] { cegevinfo, cegszerzinfo, ceghoinfo });
                UserParamTabla.AktualCegInformacio.AktualisDatum = regimaxho;
                UserParamTabla.SzamlaDatumtol = regimaxho;
                VezerloControl.WriteLoginfo();
                FakUserInterface.OpenProgress();
                ArrayList ar = new ArrayList(UserParamTabla.AktualCegInformaciok);
                int j = ar.IndexOf(UserParamTabla.AktualCegInformacio);
                UserParamTabla.SetAktualCeginformacio(UserParamTabla.LezartCeg, j);
                UserParamTabla.UserParamokFrissit();
                FakUserInterface.CloseProgress();
                ValtozasBeallit();
                UserParamTabla.Infotoltkell = true;
                UserParamTabla.SetKozosAllapotok();
                Ujcontroloktolt();
            }

        }
    }
}
