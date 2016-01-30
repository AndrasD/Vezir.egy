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
//using Vezir.ENYEM.Main;

namespace Vezir
{
    public static class UserParamTabla
    {
        public static bool Elso = true;
        public static Bejelentkezo Bejelentkezo = null;
        public static FakUserInterface FakUserInterface = null;
        public static MainControl MainControl;
        public static VezerloControl VezerloControl;
        public static bool LezartCeg = false;
        public static bool Infotoltkell = true;
        public static bool Infotoltvolt = false;
        public static Ceginformaciok[] AktivCegInformaciok = null;
        public static Ceginformaciok[] LezartCegInformaciok = null;
        public static Ceginformaciok[] AktualCegInformaciok = null;
        public static Ceginformaciok AktualCegInformacio = null;
        public static string[] CegNevek = null;
        public static int AktivCegIndex = 0;
        public static string KezeloId = "";
        public static Base.KezSzint AktualKezeloiSzint = Base.KezSzint.Minden;
        public static Base.HozferJogosultsag CegSzarmazekosJogosultsag = Base.HozferJogosultsag.Semmi;
        public static Base.HozferJogosultsag CegTermeszetesJogosultsag = Base.HozferJogosultsag.Semmi;
        public static Base.HozferJogosultsag AktualTermeszetesJogosultsag = Base.HozferJogosultsag.Semmi;
        public static Base.HozferJogosultsag UserHozferJogosultsag = Base.HozferJogosultsag.Semmi;
        private static DateTime indulodatum = DateTime.MinValue;
        public static string InduloDatumString = "";
        public static string[] Allapotszamitasok = null;
        public static ArrayList UserAllapotNevek = new ArrayList();
        public static string[] UserAllapotIdk = null;
        public static string[] VizsgalandoTablanevek = null;
        public static string[] KozosAllapotszamitasok = null;
        public static ArrayList KozosAllapotNevek = new ArrayList();
        public static string[] KozosVizsgalandoTablanevek = null;
        public static ArrayList KozosAllapotSzovegek = new ArrayList();
        public static bool EgyetlenCegkezelo = false;
        public static bool NincsCegkezelo = false;
        public static bool MenuStripLathato = false;
        public static bool HibasBevSzamla = false;
        public static bool HibasKoltsSzamla = false;
        public static int Kifizetetlenbevszladb = 0;
        public static int Kifizetetlenkoltsszladb = 0;
        public static string[] AdatbeviteliTablaNevek = null;
        public static string[] UserContNevIdk = null;
        public static ArrayList LetezoUserControlNevek = null;
        public static Base.HozferJogosultsag[] UserContHozferjog;
        public static DateTime InduloDatum
        {
            get { return indulodatum; }
            set
            {
                indulodatum = value;
                InduloDatumString = FakUserInterface.DatumToString(value);
                if (Szladat != null)
                {
                    if (Szladat.Value.CompareTo(value) < 0)
                        Szladat.Value = value;
                    else if (Szladat != null)
                        Szladat.MinDate = value;
                }
            }
        }
        private static DateTime aktualisdatum = DateTime.MinValue;
        public static string AktualisDatumString = "";
        public static DateTime AktualisDatum
        {
            get { return aktualisdatum; }
            set
            {
                bool nagyobblett = false;
                bool kisebblett = false;
                aktualisdatum = value;
                AktualisDatumString = FakUserInterface.DatumToString(value);
                aktualisdatumig = aktualisdatum.AddMonths(1).AddDays(-1);

                nagyobblett = aktualisdatum.CompareTo(Szladat.Value) > 0;
                kisebblett = aktualisdatum.CompareTo(Szladat.Value) < 0;
                if (nagyobblett)
                {
                    Szladat.MaxDate = aktualisdatumig;
                    Szladat.Value = value;
                }
                if (kisebblett)
                {
                    Szladat.Value = value;
                    Szladat.MaxDate = aktualisdatumig;
                }
                if (nagyobblett || kisebblett)
                {
                    szamladatumtol = aktualisdatum;
                    szamladatumig = aktualisdatumig;
                    MainControl.label3.Text = "Adatbeviteli hónap: (" + InduloDatumString.Substring(0, 7) + "-" + AktualisDatumString.Substring(0, 7) + ")";
                    MainControl.ValtozasBeallit("Datumvaltozas");
                    Infotoltkell = true;
                }
            }
        }
        private static DateTime aktualisdatumig = DateTime.MaxValue;
        public static string aktualisdatumigstring = "";
        public static DateTime AktualisDatumig
        {
            get { return aktualisdatumig; }
        }
        private static string datumtolstring;
        public static string DatumtolString
        {
            get { return datumtolstring; }
        }
        private static DateTime datumtol = DateTimePicker.MinimumDateTime;
        public static DateTime Datumtol
        {
            get { return datumtol; }
            set
            {
                datumtol = value;
                datumtolstring = FakUserInterface.DatumToString(value);
            }
        }
        private static string datumigstring;
        public static string DatumigString
        {
            get { return datumigstring; }
        }
        private static DateTime datumig = DateTimePicker.MaximumDateTime;
        public static DateTime Datumig
        {
            get { return datumig; }
            set
            {
                datumig = value;
                datumigstring = FakUserInterface.DatumToString(value);
            }
        }
        private static DateTime szamladatumtol;
        public static DateTime SzamlaDatumtol
        {
            get { return szamladatumtol; }
            set
            {
                szamladatumtol = value;
                szamladatumtolstring = FakUserInterface.DatumToString(szamladatumtol);
                szamladatumig = szamladatumtol.AddMonths(1).AddDays(-1);
                szamladatumigstring = FakUserInterface.DatumToString(szamladatumig);
            }
        }
        private static string szamladatumtolstring;
        public static string SzamlaDatumtolString
        {
            get { return szamladatumtolstring; }
        }

        private static DateTime szamladatumig;
        public static DateTime SzamlaDatumig
        {
            get
            {
                return szamladatumig;
            }
        }
        private static string szamladatumigstring;
        public static string SzamlaDatumigString
        {
            get { return szamladatumigstring; }
        }

        public static bool FormvezTreeViewShowNodeToolTip = false;
        public static string UserParamok = "";
        public static string AktualVezerloControlNev = "";
        public static string IgaziVezerloControlNev = "";
        public static int AktualPageindex = 0;
        public static string AktualControlNev = "";
        public static Base.Parameterezes Paramfajta = Base.Parameterezes.Nincsparameterezes;
        public static int ValasztekIndex = 0;
        public static int AktualMenuItemIndex = 0;
        public static int AktualDropItemIndex = 1;
        public static string[] ListaParamok = null;
        public static int RadioButtonIndex = 0;
        public static string[] OsszetettKozepsoParamok = null;
        public static string[] OsszetettAlsoParamok = null;
        public static string OsszetettKozepsoNev = "";
        public static string[] SzurtIdk = null;
        public static TablainfoTag TablainfoTag = null;
        public static string Szint = "";
        public static bool FormvezParam = false;
        private static UsRutinok Usrutinok = null;
        public static bool[] Allapotok = null;
        public static TablainfoCollection TermCegPluszCegalattiTablainfok = new TablainfoCollection();
        public static Tablainfo Cegevinfo = null;
        public static Tablainfo Ceghonapinfo = null;
        public static string Ceghonap_Id = null;
        public static Tablainfo Cegszerzodesinfo = null;
        public static ArrayList OsszesEv = new ArrayList();
        public static ArrayList NemLezartEvek = new ArrayList();
        public static ArrayList LezartEvek = new ArrayList();
        public static ArrayList TermTablaNevek = new ArrayList();
        public static ArrayList TermTablanevIdk = new ArrayList();
        public static bool EgyFolyoszamla = false;
        public static bool EgyPenztar = false;
        public static bool VanDolgozo = false;
        public static bool KellZaras = false;
        public static DateTimePicker Szladat = null;
        public static Tablainfo KezSzintek = null;
        public static Tablainfo UserContKezszint = null;
        public static int AktualisCegverzioId = 0;
        public static DateTime[] AktualisCegIntervallum = null;
        public static bool Lehetpartner = true;
        public static void Open(Bejelentkezo bejel, MainControl cont)
        {
            MainControl = cont;
            Szladat = MainControl.szladat;
            if (Usrutinok == null)
                Usrutinok = new UsRutinok();
            Usrutinok.Open(bejel);
        }
        public static void Close()
        {
            UserParamok = "";
        }
        public static void SetAktualCeginformacio(bool lezart, int cegindex)
        {
            Usrutinok.SetAktualCeginformacio(lezart, cegindex);
        }
        public static void LezartNemLezarEv()
        {
            Usrutinok.LezartNemLezartEv();
        }
        public static Base.HozferJogosultsag SetAktualTermJogosultsag(string ev)
        {
            return Usrutinok.SetAktualTermJogosultsag(ev);
        }

        public static void UserParamokFrissit()
        {
            Usrutinok.UserParamokFrissit();
        }
        public static void SetKozosAllapotok()
        {
            Usrutinok.SetKozosAllapotok();
        }
        public static void Infotolt()
        {
            Usrutinok.Infotolt();
        }
        public static void Infotolt(string id)
        {
            Usrutinok.Infotolt(id);
        }
        private class UsRutinok
        {
            public void SetKozosAllapotok()
            {
                KozosAllapotSzovegek.Clear();
                MainControl.teszt.Visible = true;
                Lehetpartner = true;
                for (int i = 0; i < KozosAllapotNevek.Count; i++)
                {
                    string allapotnev = KozosAllapotNevek[i].ToString();
                    string tablanev = KozosVizsgalandoTablanevek[i];
                    string szamitas = KozosAllapotszamitasok[i];
                    Tablainfo info;
                    switch (szamitas)
                    {
                        case "VANKEZELO":
                            info = FakUserInterface.GetByAzontip("SZUTKEZELOK");
                            if (info.DataView.Count == 0)
                                KozosAllapotSzovegek.Add("Nincs " + allapotnev);
                            break;
                        case "VANRENDSZERGAZDA":
                            info = FakUserInterface.GetByAzontip("SZUTRENDSZERGAZDA");
                            if (info.DataView.Count == 0)
                                KozosAllapotSzovegek.Add("Nincs " + allapotnev);
                            else if (KozosAllapotSzovegek.Count == 0)
                            {
                                info = FakUserInterface.GetByAzontip("SZUOKezeloAlkalm");
                                if (info.DataView.Count == 0)
                                    KozosAllapotSzovegek.Add(" Az alkalmazáshoz nincs kijelölt kezelö");
                                else
                                {
                                    if (info.DataView.Count == 1)
                                        EgyetlenCegkezelo = true;
                                    else
                                        EgyetlenCegkezelo = false;
                                }
                            }
                            break;
                        case "BANKOK":
                            info = FakUserInterface.GetBySzintPluszTablanev("U", szamitas);
                            if (info.DataView.Count == 0)
                                KozosAllapotSzovegek.Add("Nincs " + allapotnev);
                            break;

                        case "Termfocsop":
                            info = FakUserInterface.GetKodtab("C", szamitas);
                            if (info.DataView.Count == 0)
                            {
                                KozosAllapotSzovegek.Add("Nincs " + allapotnev);
                                Lehetpartner = false;
                            }
                            break;
                        case "Termalcsop":
                            info = FakUserInterface.GetKodtab("C", szamitas);
                            if (info.DataView.Count == 0)
                            {
                                KozosAllapotSzovegek.Add("Nincs " + allapotnev);
                                Lehetpartner = false;
                            }
                            break;
                        case "Termcsop":
                            info = FakUserInterface.GetKodtab("C", szamitas);
                            if (info.DataView.Count == 0)
                            {
                                KozosAllapotSzovegek.Add("Nincs " + allapotnev);
                                Lehetpartner = false;
                            }
                            break;
                        case "TERMEKKOD":
                            info = FakUserInterface.GetBySzintPluszTablanev("C", szamitas);
                            if (info.DataView.Count == 0)
                            {
                                KozosAllapotSzovegek.Add("Nincs " + allapotnev);
                                 Lehetpartner = false;
                            }
                            break;

                        case "Koltsfocsop":
                            info = FakUserInterface.GetKodtab("C", szamitas);
                            if (info.DataView.Count == 0)
                            {
                                KozosAllapotSzovegek.Add("Nincs " + allapotnev);
                                Lehetpartner = false;
                            }
                            break;
                        case "Koltsalcsop":
                            info = FakUserInterface.GetKodtab("C", szamitas);
                            if (info.DataView.Count == 0)
                            {
                                KozosAllapotSzovegek.Add("Nincs " + allapotnev);
                                Lehetpartner = false;
                            }
                           break;
                        case "KOLTSEGCSOPORT":
                            info = FakUserInterface.GetBySzintPluszTablanev("C", szamitas);
                            if (info.DataView.Count == 0)
                            {
                                KozosAllapotSzovegek.Add("Nincs " + allapotnev);
                                Lehetpartner = false;
                            }
                            break;
                        case "KOLTSEGKOD":
                            info = FakUserInterface.GetBySzintPluszTablanev("C", szamitas);
                            if (info.DataView.Count == 0)
                            {
                                KozosAllapotSzovegek.Add("Nincs " + allapotnev);
                                 Lehetpartner = false;
                            }
                           break;
                        case "Afa":
                            info = FakUserInterface.GetKodtab("C", szamitas);
                            if (info.DataView.Count == 0)
                            {
                                KozosAllapotSzovegek.Add("Nincs " + allapotnev);
                                MainControl.teszt.Visible = false;
                            }
                            break;
                        case "FOLYOSZAMLAK":
                            info = FakUserInterface.GetBySzintPluszTablanev("C", szamitas);
                            if (info.DataView.Count == 0)
                                KozosAllapotSzovegek.Add("Nincs " + allapotnev);
                            else
                            {
                                EgyFolyoszamla = info.DataView.Count == 1;
                                VezerloControl.folyoszlafolyoszla.Enabled = !EgyFolyoszamla;
                            }
                            break;
                        case "Penztarak":
                            info = FakUserInterface.GetKodtab("C", szamitas);
                            if (info.DataView.Count == 0)
                                KozosAllapotSzovegek.Add("Nincs " + allapotnev);
                            else
                            {
                                EgyPenztar = info.DataView.Count == 1;
                                VezerloControl.penztarpenztar.Enabled = !EgyPenztar;
                            }
                            break;
                        case "Fszazal":
                            info = FakUserInterface.GetKodtab("C", szamitas);
                            if (info.DataView.Count == 0)
                            {
                                KozosAllapotSzovegek.Add("Nincs " + allapotnev);
                                MainControl.teszt.Visible = false;
                            }
                            break;
                        case "PARTNER":
                            info = FakUserInterface.GetByAzontip("SZCTPARTNER");
                            Tablainfo vezirpartnerinfo = FakUserInterface.GetByAzontip("SZCTVEZIRPARTNER");
                            //if (CegSzarmazekosJogosultsag == Base.HozferJogosultsag.Irolvas)
                            //{
                            //    if (vezirpartnerinfo.DataView.Count == 0)
                            //        vezirpartnerinfo.Azonositok.Jogszintek[(int)AktualKezeloiSzint] = Base.HozferJogosultsag.Csakolvas;
                            //    else
                            //        vezirpartnerinfo.Azonositok.Jogszintek[(int)AktualKezeloiSzint] = Base.HozferJogosultsag.Irolvas;

                            //}
                            if (info.DataView.Count == 0)
                                KozosAllapotSzovegek.Add("Nincs " + allapotnev);
                            else
                            {
                                string szov = "Nincs vevöpartner";
                                vezirpartnerinfo.DataView.RowFilter = "BEVPARTNER = 'I'";
                                if (vezirpartnerinfo.DataView.Count == 0)
                                    KozosAllapotSzovegek.Add(szov);
                                szov = "Nincs szállitópartner";
                                vezirpartnerinfo.DataView.RowFilter = "KOLTSPARTNER = 'I'";
                                if (vezirpartnerinfo.DataView.Count == 0)
                                    KozosAllapotSzovegek.Add(szov);
                                vezirpartnerinfo.DataView.RowFilter = "";
                            }
                            break;
                        case "DOLGOZOK":
                            info = FakUserInterface.GetBySzintPluszTablanev("C", szamitas);
                            VanDolgozo = info.DataView.Count != 0;
                            break;
                    }
                }
                Tablainfo tabinfo = FakUserInterface.GetByAzontip("SZCOTARTAL");
                for (int i = 0; i < tabinfo.Adattabla.Rows.Count; i++)
                {
                    DataRow dt = tabinfo.Adattabla.Rows[i];
                    string kodtip = dt["KODTIPUS"].ToString();
                    Tablainfo egyinfo = FakUserInterface.GetOsszef("C", kodtip);
                    if (egyinfo.Adattabla.Rows.Count == 0)
                        KozosAllapotSzovegek.Add("Nincs a " + dt["SZOVEG"].ToString() + " kitöltve!");
                }
                if (KozosAllapotSzovegek.Count == 0)
                {
                    MainControl.hianyzok.Visible = false;
                    MainControl.eredm.Enabled = true;
                }
                else
                {
                    MainControl.hianyzok.Visible = true;
                    MainControl.eredm.Enabled = false;
                }
                MenuStripLathato = KozosAllapotSzovegek.Count == 0 && CegTermeszetesJogosultsag != Base.HozferJogosultsag.Semmi;
                VezerloControl.MenuStrip.Visible = MenuStripLathato;
                if (MenuStripLathato)
                {
                    SetEgyediAllapotok();
                    VezerloControl.MenuBeallitasok();
                }
            }
            private void SetEgyediAllapotok()
            {
                for (int i = 0; i < Allapotok.Length; i++)
                {
                    string tablanev = UserParamTabla.VizsgalandoTablanevek[i];
                    string szamitas = UserParamTabla.Allapotszamitasok[i];
                    string allapotid = UserParamTabla.UserAllapotIdk[i];
                    bool van = false;
                    DataTable dt = new DataTable();
                    switch (szamitas)
                    {

                        case "TOBBFOLYO":
                            if (!EgyFolyoszamla)
                                van = true;
                            break;
                        case "TOBBPENZTAR":
                            if (!EgyPenztar)
                                van = true;
                            break;
                        case "BEVKIEGYENL":
                            dt.Rows.Clear();
                            Sqlinterface.Select(dt, FakUserInterface.AktualCegconn, "BEVSZLA", " where FIZETVE ='N' AND MARADEK = 0", "", false);
                            Kifizetetlenbevszladb = dt.Rows.Count;
                            van = dt.Rows.Count != 0;
                            break;
                        case "KOLTSKIEGYENL":
                            dt.Rows.Clear();
                            Sqlinterface.Select(dt, FakUserInterface.AktualCegconn, "KOLTSSZLA", " where FIZETVE ='N' AND MARADEK = 0", "", false);
                            Kifizetetlenkoltsszladb = dt.Rows.Count;
                            van = dt.Rows.Count != 0;
                            break;
                        case "EVZAR":
                            if (NemLezartEvek.Count == 0)
                                break;
                            if (NemLezartEvek[0].ToString() == OsszesEv[OsszesEv.Count - 1].ToString())
                                break;
                            string tol = NemLezartEvek[0].ToString() + ".01.01";
                            string ig = NemLezartEvek[0].ToString() + ".12.31";
                            string sel = " where FIZETVE ='N' AND SZLA_DATUM>='" + tol + "' AND SZLA_DATUM <= '" + ig + "'";
                            dt.Rows.Clear();
                            Sqlinterface.Select(dt, FakUserInterface.AktualCegconn, "BEVSZLA", sel, "", true);
                            van = dt.Rows.Count == 0;
                            if (!van)
                                break;
                            Sqlinterface.Select(dt, FakUserInterface.AktualCegconn, "KOLTSSZLA", sel, "", true);
                            van = dt.Rows.Count == 0;
                            break;
                        case "EVNYIT":
                            van = LezartEvek.Count != 0;
                            break;
                        case "HONYITZAR":
                            van = UserParamTabla.AktualTermeszetesJogosultsag == Base.HozferJogosultsag.Irolvas;
                            break;
                        case "BEVSZLA_NEZ":
                            dt.Rows.Clear();
                            Sqlinterface.Select(dt, FakUserInterface.AktualCegconn, "BEVSZLA", " where MARADEK <> 0", "", true);
                            van = dt.Rows.Count == 0;
                            HibasBevSzamla = !van;
                            break;
                        case "KOLTSSZLA_NEZ":
                            dt.Rows.Clear();
                            Sqlinterface.Select(dt, FakUserInterface.AktualCegconn, "KOLTSSZLA", " where MARADEK <> 0", "", true);
                            van = dt.Rows.Count == 0;
                            HibasKoltsSzamla = !van;
                            break;
                        case "NINCSHIB_BEVSZLA":
                            van = HibasBevSzamla;
                            break;
                        case "NINCSHIB_KOLTSSZLA":
                            van = HibasKoltsSzamla;
                            break;
                        case "VANADAT":
                            foreach (string tabnev in AdatbeviteliTablaNevek)
                            {
                                dt.Rows.Clear();
                                Sqlinterface.Select(dt, FakUserInterface.AktualCegconn, tabnev, "", "", true);
                                if (dt.Rows.Count != 0)
                                {
                                    van = true;
                                    break;
                                }
                            }
                            break;
                        case "VANALKALMAZOTT":
                            dt.Rows.Clear();
                            Sqlinterface.Select(dt, FakUserInterface.AktualCegconn, "DOLGOZOK", "", "", true);
                            van = dt.Rows.Count != 0;
                            break;
                        case "VANMOZGAS":
                            van = false;
                            Tablainfo info = FakUserInterface.GetOsszef("R","TablanevAlkallapot");
                            info.DataView.RowFilter="SORSZAM2 = "+allapotid;
                            for(int j=0;j<info.DataView.Count;j++)
                            {
                                string tablaid=info.DataView[j].Row["SORSZAM1"].ToString();
                                string nev = TermTablaNevek[TermTablanevIdk.IndexOf(tablaid)].ToString();
                                dt.Rows.Clear();
                                Sqlinterface.Select(dt, FakUserInterface.AktualCegconn, nev, "", "", true);
                                van = dt.Rows.Count != 0;
                                if (van)
                                    break;
                            }
                            dt.Rows.Clear();
                            break;
                    }
                    Allapotok[i] = van;
                }
                MainControl.comboBox2.Enabled = !HibasBevSzamla && !HibasKoltsSzamla;
            }

            //private void Kezeloszereprendberak(Tablainfo szerepinfo)
            //{
            //    Tablainfo kezalkalm = FakUserInterface.GetOsszef("U", "KezeloAlkalm");
            //    string[] idk = FakUserInterface.GetTartal(kezalkalm, "SORSZAM1");
            //    if (idk.Length > 1)
            //    {
            //        string savfilt = szerepinfo.DataView.RowFilter;
            //        for (int i = 0; i < idk.Length; i++)
            //        {
            //            szerepinfo.DataView.RowFilter = "KEZELO_ID = " + idk[i];
            //            bool van = szerepinfo.DataView.Count != 0;
            //            szerepinfo.DataView.RowFilter = "";
            //            if (!van)
            //            {
            //                DataRow row = szerepinfo.Ujsor();
            //                row["CEG_ID"] = FakUserInterface.AktualCegid;
            //                row["ALKALMAZAS_ID"] = FakUserInterface.AlkalmazasId;
            //                row["KEZELO_ID"] = idk[i];
            //                row["SZEREPKOD"] = "10";
            //            }
            //            szerepinfo.DataView.RowFilter = savfilt;
            //            if (szerepinfo.Modositott)
            //                FakUserInterface.Rogzit(szerepinfo);
            //        }
            //    }
            //}
            public void Open(Bejelentkezo bejel)
            {
                Bejelentkezo = bejel;
                KezeloId = bejel.Kezeloid;
                FakUserInterface = bejel.FakUserInterface;
                Cegszerzodesinfo = FakUserInterface.GetByAzontip("SZCTCEGSZERZODES");
                Tablainfo tablanevek = FakUserInterface.GetByAzontip("SZRTTABLANEVEK");
                TermTablaNevek = new ArrayList(FakUserInterface.GetTartal(tablanevek, "SZOVEG"));
                TermTablanevIdk = new ArrayList(FakUserInterface.GetTartal(tablanevek, "ID"));
                tablanevek.DataView.RowFilter = "SZULOSZINT<>''";
                AdatbeviteliTablaNevek = FakUserInterface.GetTartal(tablanevek, "SZOVEG");
                tablanevek.DataView.RowFilter = "";
                Tablainfo userallapotok = FakUserInterface.GetByAzontip("SZRTUSERALLAPOTOK");
                Allapotok = new bool[userallapotok.DataView.Count];
                string[] userallapotnevek = FakUserInterface.GetTartal(userallapotok, "SZOVEG");
                UserAllapotNevek = new ArrayList(userallapotnevek);
                Allapotszamitasok = FakUserInterface.GetTartal(userallapotok, "SZAMITAS");
                UserAllapotIdk = FakUserInterface.GetTartal(userallapotok, "ID");
                VizsgalandoTablanevek = FakUserInterface.GetTartal(userallapotok, "TABLANEV");
                Tablainfo kozosallapotok = FakUserInterface.GetByAzontip("SZRTKOZOSUSERALLAPOTOK");
                userallapotnevek = FakUserInterface.GetTartal(kozosallapotok, "SZOVEG");
                KozosAllapotNevek = new ArrayList(userallapotnevek);
                KozosAllapotszamitasok = FakUserInterface.GetTartal(kozosallapotok, "SZAMITAS");
                KozosVizsgalandoTablanevek = FakUserInterface.GetTartal(kozosallapotok, "TABLANEV");
                AktivCegInformaciok = Bejelentkezo.AktivCegInformaciok;
                LezartCegInformaciok = Bejelentkezo.LezartCegInformaciok;
                LezartCeg = Bejelentkezo.LezartCeg;
                if (LezartCeg)
                    AktualCegInformaciok = LezartCegInformaciok;
                else
                    AktualCegInformaciok = AktivCegInformaciok;
                ArrayList ar = new ArrayList(AktualCegInformaciok);
                FakUserInterface.AktualCegconn = "";
                Tablainfo usercontn = FakUserInterface.GetBySzintPluszTablanev("R", "USERCONTROLNEVEK");
                UserContNevIdk = FakUserInterface.GetTartal(usercontn, "ID", "SZOVEG", MainControl.letezousercontrolnevek);
                LetezoUserControlNevek = new ArrayList(MainControl.letezousercontrolnevek);
                UserContKezszint = FakUserInterface.GetOsszef("R", "UserContKezszint");
                UserContHozferjog = new Base.HozferJogosultsag[UserContNevIdk.Length];
                KezSzintek = FakUserInterface.GetKodtab("R", "Kezszint");
                SetAktualCeginformacio(LezartCeg, ar.IndexOf(bejel.AktualCegInformacio));
            }
            public void UserParamokFrissit()
            {
                DataRow row = AktualCegInformacio.UserLogsor;
                UserParamok = "";
                AktualVezerloControlNev = "";
                IgaziVezerloControlNev = "";
                Paramfajta = Base.Parameterezes.Nincsparameterezes;
                if (row != null)
                {
                    AktualVezerloControlNev = row["USERCONTNEV"].ToString();
                    IgaziVezerloControlNev = AktualVezerloControlNev;
                    Paramfajta = (Base.Parameterezes)Convert.ToInt16(row["PARAMETERTIPUS"].ToString());
                    UserParamok = row["USERPARAMOK"].ToString();
                }
                if (UserParamok != "")
                {
                    char[] vesszo = new char[] { Convert.ToChar(",") };
                    string[] split = UserParamok.Split(vesszo);
                    AktualControlNev = split[0];
                    if (AktualVezerloControlNev == "Formvezerles")
                    {
                        FormvezParam = true;
                        string azontip = split[3];
                        Szint = azontip.Substring(2, 1);
                        if (AktualControlNev != "")
                        {
                            TablainfoTag = FakUserInterface.GetByAzontip(azontip).Azonositok.Tablatag;
                            AktualMenuItemIndex = Convert.ToInt16(split[4]);
                            AktualDropItemIndex = Convert.ToInt16(split[5]);
                        }
                    }
                    else
                    {
                        AktualMenuItemIndex = Convert.ToInt32(split[1]);
                        AktualDropItemIndex = Convert.ToInt32(split[2]);
                        int nyito = UserParamok.IndexOf("{");
                        int zaro = UserParamok.IndexOf("}");
                        string osztando = UserParamok.Substring(nyito + 1, zaro - nyito - 1);
                        split = osztando.Split(vesszo);
                        if (Paramfajta == Base.Parameterezes.Nincsparameterezes)
                        {
                            SzamlaDatumtol = Convert.ToDateTime(split[0]);
                            return;
                        }
                        string paramstring = Paramfajta.ToString();
                        if (Paramfajta == Base.Parameterezes.Listaparamok)
                        {
                            return;
                        }
                        if (paramstring.Contains("Datum"))
                        {
                            Datumtol = Convert.ToDateTime(split[0]);
                            Datumig = Convert.ToDateTime(split[1]);
                            if (paramstring.Contains("valasztek"))
                            {
                                ValasztekIndex = Convert.ToInt32(split[2]);
                                return;
                            }
                        }
                        nyito = UserParamok.IndexOf("/");
                        if (nyito != -1)
                        {
                            zaro = UserParamok.IndexOf("/", nyito + 1);
                            if (zaro != -1)
                            {
                                osztando = UserParamok.Substring(nyito + 1, zaro - nyito - 1);
                                split = osztando.Split(vesszo);
                                SzurtIdk = new string[split.Length];
                                for (int i = 0; i < split.Length; i++)
                                    SzurtIdk[i] = split[i];
                            }
                        }
                        if (paramstring.Contains("Osszetett"))
                        {
                            nyito = UserParamok.IndexOf("[");
                            if (nyito != -1)
                            {
                                zaro = UserParamok.IndexOf("]");
                                if (zaro != -1)
                                {
                                    osztando = UserParamok.Substring(nyito + 1, zaro - nyito - 1);
                                    split = osztando.Split(vesszo);
                                    RadioButtonIndex = Convert.ToInt16(split[0]);
                                    if (split.Length != 1)
                                    {
                                        OsszetettKozepsoParamok = new string[split.Length - 1];
                                        for (int i = 1; i <= OsszetettKozepsoParamok.Length; i++)
                                            OsszetettKozepsoParamok[i - 1] = split[i];
                                    }
                                }
                            }
                        }
                    }
                }
            }
            public void Infotolt(string ceghonapid)
            {
                if (Infotoltkell)
                {
                    Ceghonapinfo.Adattabla.Select();
                    Ceghonapinfo.Tartalmaktolt();
                    Ceghonapinfo.DataView.RowFilter = "CEGHONAP_ID = " + ceghonapid;
                    Ceghonapinfo.ViewSorindex = 0;
                    Ceghonapinfo.DataView.RowFilter = "";
                    FakUserInterface.OsszesAdattoltByParent("CEGSZLAHONAPOK");
                    Infotoltkell = false;
                }
            }
            public void Infotolt()
            {
                if (Infotoltkell)
                {
                    Infotoltkell = false;
                    if (CegTermeszetesJogosultsag != Base.HozferJogosultsag.Semmi)
                    {
                        Ceghonapinfo.Adattabla.Select();
                        Ceghonapinfo.Tartalmaktolt();
                        Ceghonapinfo.DataView.RowFilter = "SZLA_DATUM >='" + SzamlaDatumtolString + "' AND SZLA_DATUM<='" + SzamlaDatumigString + "'";
                        Ceghonapinfo.ViewSorindex = Ceghonapinfo.DataView.Count - 1;
                        Ceghonap_Id = Ceghonapinfo.AktIdentity.ToString();
                        if (Ceghonap_Id == "-1")
                        {
                        }
                        else
                            FakUserInterface.OsszesAdattoltByParent("CEGSZLAHONAPOK");
                    }
                }
            }
            public Base.HozferJogosultsag SetAktualTermJogosultsag(string ev)
            {
                if (CegTermeszetesJogosultsag == AktualTermeszetesJogosultsag)
                    return CegTermeszetesJogosultsag;
                if (CegTermeszetesJogosultsag == Base.HozferJogosultsag.Semmi)
                    AktualTermeszetesJogosultsag = CegTermeszetesJogosultsag;
                else if (LezartEvek.IndexOf(ev) != -1)
                    AktualTermeszetesJogosultsag = Base.HozferJogosultsag.Csakolvas;
                else
                    AktualTermeszetesJogosultsag = CegTermeszetesJogosultsag;
                MainControl.ValtozasBeallit("Datumvaltozas");
                if (VezerloControl != null)
                {
                    for (int i = 0; i < VezerloControl.Vezerles.ChildVezerloinfoCollection.Count; i++)
                    {
                        Vezerloinfo info = VezerloControl.Vezerles.ChildVezerloinfoCollection[i];
                        if (!info.Name.Contains("Formvez"))
                            info.HozferJog = AktualTermeszetesJogosultsag;
                    }
                }

                return AktualTermeszetesJogosultsag;
            }
            public void SetAktualCeginformacio(bool lezart, int cegindex)
            {
                LezartCeg = lezart;
                if (LezartCeg)
                    AktualCegInformaciok = LezartCegInformaciok;
                else
                    AktualCegInformaciok = AktivCegInformaciok;
                AktualCegInformacio = AktualCegInformaciok[cegindex];
                AktivCegIndex = cegindex;
                ArrayList cegnevek = new ArrayList();
                foreach (Ceginformaciok egyinf in AktualCegInformaciok)
                    cegnevek.Add(egyinf.CegNev);
                CegNevek = (string[])cegnevek.ToArray(typeof(string));
                InduloDatum = AktualCegInformacio.InduloDatum;
                AktualisDatum = AktualCegInformacio.AktualisDatum;
                SzamlaDatumtol = AktualisDatum;
                Datumtol = AktualisDatum;
                Datumig = AktualisDatumig;
                FakUserInterface.Cegadatok(AktualCegInformacio.CegConnection, CegNevek[cegindex], new DateTime[] { AktualisDatum, AktualisDatumig });
                AktualisCegverzioId = FakUserInterface.VerzioInfok["C"].AktVerzioId;
                AktualisCegIntervallum = FakUserInterface.VerzioInfok["C"].AktIntervallum;
                AktualKezeloiSzint = AktualCegInformacio.KezeloiSzint;
                TermCegPluszCegalattiTablainfok = FakUserInterface.GetCegPluszCegalattiTermTablaInfok();
                Cegevinfo = TermCegPluszCegalattiTablainfok["CEGEV"];
                Ceghonapinfo = TermCegPluszCegalattiTablainfok["CEGSZLAHONAPOK"];
                Cegevinfo.Adattabla.Select();
                Ceghonapinfo.Adattabla.Select();
                if (Cegevinfo.Adattabla.Rows.Count == 0)
                {
                    DataRow row = Cegevinfo.Ujsor();
                    row["EV"] = indulodatum.Year.ToString();
                    row["LEZART"] = "N";
                    row["KELLZARAS"] = "N";
                    row["MODOSITOTT_M"] = "1";
                    FakUserInterface.Rogzit(Cegevinfo);
                    row = Ceghonapinfo.Ujsor();
                    row["SZLA_DATUM"] = aktualisdatum;
                    row["CEGEV_ID"] = Cegevinfo.Adattabla.Rows[0]["CEGEV_ID"];
                    row["EVHONAP"] = indulodatum.Year.ToString() + ".01";
                    row["MODOSITOTT_M"] = "1";
                    FakUserInterface.Rogzit(Ceghonapinfo);
                }
                UserParamokFrissit();
                LezartNemLezartEv();
                Ceghonapinfo = TermCegPluszCegalattiTablainfok["CEGSZLAHONAPOK"];
                Ceghonapinfo.SelectString = "";
                Ceghonapinfo.Adattabla.Select();
                Ceghonapinfo.ViewSorindex = 0;
                Ceghonap_Id = Ceghonapinfo.AktIdentity.ToString();
                Elso = false;
                UserHozferJogosultsag = AktualCegInformacio.UserJogosultsag;
                CegSzarmazekosJogosultsag = AktualCegInformacio.CegSzarmazekosJogosultsag;
                CegTermeszetesJogosultsag = AktualCegInformacio.CegTermeszetesJogosultsag;
                //               AktualTermeszetesJogosultsag = CegTermeszetesJogosultsag;
                AktualTermeszetesJogosultsag = SetAktualTermJogosultsag(SzamlaDatumtol.Year.ToString());
                string aktkezszint = Convert.ToInt32(AktualKezeloiSzint).ToString();
                string[] kezszintidk = FakUserInterface.GetTartal(KezSzintek, "SORSZAM", "KOD", aktkezszint);
                string[] uscontidk = FakUserInterface.GetTartal(UserContKezszint, "SORSZAM1", "SORSZAM2", kezszintidk[0]);
                ArrayList ar1 = new ArrayList(UserContNevIdk);
                for (int i = 0; i < UserContHozferjog.Length; i++)
                    UserContHozferjog[i] = Base.HozferJogosultsag.Semmi;
                if (uscontidk != null)
                {
                    for (int i = 0; i < uscontidk.Length; i++)
                    {
                        int pos = ar1.IndexOf(uscontidk[i]);
                        if (pos != -1)
                            UserContHozferjog[pos] = Base.HozferJogosultsag.Irolvas;

                    }
                }
            }
            public void LezartNemLezartEv()
            {
                OsszesEv.Clear();
                NemLezartEvek.Clear();
                LezartEvek.Clear();
                KellZaras = false;
                for (int i = 0; i < Cegevinfo.DataView.Count; i++)
                {
                    DataRow dr = Cegevinfo.DataView[i].Row;
                    OsszesEv.Add(dr["EV"]);
                    if (dr["LEZART"].ToString() == "I")
                        LezartEvek.Add(dr["EV"]);
                    else
                    {
                        NemLezartEvek.Add(dr["EV"]);
                        if (dr["KELLZARAS"].ToString() == "I")
                            KellZaras = true;
                    }
                }
            }
        }
    }
}
