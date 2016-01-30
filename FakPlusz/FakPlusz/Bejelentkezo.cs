#define text
using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FakPlusz;
using FakPlusz.Alapfunkciok;
using FakPlusz.Alapcontrolok;
using FakPlusz.UserAlapcontrolok;
namespace FakPlusz
{
    public class Bejelentkezo
    {
        private bool elso = true;
        public FakUserInterface FakUserInterface = null;
        public string Nev = "Dallos Andras";
        public string Alkalmazas = "";
        public string alkid = "";
        public bool HibasAlkalmazas = false;
        public bool NincsKezelo = false;
        public string Kezeloid = "-1";
        public string Rgazdaid = "-1";
        public string RgazdaNev = "";
        public string RgazdaJelszo = "";
        private string vezetoid = "";
        private string adatkezeloid = "";
        private string kiemeltkezeloid = "";
        public Ceginformaciok[] AktivCegInformaciok = null;
        public Ceginformaciok[] AktualCegInformaciok = null;
        public Ceginformaciok AktualCegInformacio;
        public Base.KezSzint AktivKezeloiSzint;
        public string[] CegNevek = null;
        public string[] CegIdk = null;
        public Ceginformaciok[] LezartCegInformaciok = null;
        public string[] LezartCegNevek = null;
        public string[] LezartCegIdk = null;
        public string Rendszerconn = "";
        public string Userconn = "";
        public string Adatbazisfajta;
//        public DataTable nyilvkodok = new DataTable();
 //       public DataTable lezartnyilvkodok = new DataTable();
        public DataTable userverzio = new DataTable();
        public string[] connectionstringek = null;
        public string jelszo;
        public string ujjelszo;
        public Ceginformaciok AktualCeginformacio = null;
        public string UserContnev;
        public int Parametertipus = 0;
        public string UserParamok = "";
        public int AktualCegindex;
        public bool LezartCeg = false;
        public bool LezartEv = false;
        private ArrayList cegconok = new ArrayList();
        private ArrayList ujcege = new ArrayList();
        private DataTable cegtabla = new DataTable("CEGEK");
        private Tablainfo cegszerzinfo;
        private Tablainfo cegkezszerepinfo;
        private Tablainfo kezeloinfo;
        private Tablainfo rendszergazdainfo;
//        private Tablainfo cegevinfo;
//        private Tablainfo ceghonapokinfo;
        private Tablainfo kezalkalminfo;
        public ArrayList KezeloIdkArray = new ArrayList();
        public bool KezalkalmUpdateKell = false;
        public bool CsakEgyKezelo = false;
        public BejelformAlap Bejelform; // = new BejelformAlap();
        public MainAlap MainAlap = null;
        public bool Vanujceg = false;
        public DateTime UjcegIndulodatum = DateTime.MinValue;
        public DateTime vegedatum = DateTime.MaxValue;
        public Bejelentkezo()
        {
        }
        public Bejelentkezo(MainAlap main)
        {
            MainAlap = main;
        }
        public virtual void BejelentkezoAlapInit(string[] connstringek, string alkalmazas)
        {
            Bejelconstr(connstringek, "Sql", alkalmazas);
        }
        public virtual void BejelentkezoAlapInit(string[] connstringek, string adatbazisfajta, string alkalmazas)
        {
            Bejelconstr(connstringek, adatbazisfajta, alkalmazas);
        }
        private void Bejelconstr(string[] connstringek, string adatbazisfajta, string alkalmazas)
        {
            Rendszerconn = connstringek[0];
            Userconn = connstringek[1];
            connectionstringek = connstringek;
            for (int i = 2; i < connectionstringek.Length; i++)
            {
                cegconok.Add(connectionstringek[i]);
                ujcege.Add(false);
            }
            Adatbazisfajta = adatbazisfajta;
            Alkalmazas = alkalmazas;
            DataTable dt = new DataTable();
            Sqlinterface.RendszerUserConn(Rendszerconn, Userconn);
            Sqlinterface.Select(dt, Rendszerconn, "KODTAB", " where KODTIPUS ='Alkalm' and SZOVEG ='" + Alkalmazas + "'", "", false);
            if (dt.Rows.Count == 0)
                HibasAlkalmazas = true;
            else
            {
                string wherefelt = "" ;
                alkid = dt.Rows[0]["SORSZAM"].ToString();
                for (int i = 0; i < cegconok.Count; i++)
                {
                    if(wherefelt=="")
                        wherefelt+= " where ";
                    else
                        wherefelt+= " or ";
                    wherefelt+="CEGCONNECTION = '"+cegconok[i]+"'";
                }
                Sqlinterface.Select(cegtabla, Userconn, "CEGEK", wherefelt, "", false);
                if (cegtabla.Rows.Count != cegconok.Count)
                {
                    Vanujceg = true;
                    for (int i = 0; i < cegconok.Count; i++)
                        ujcege[i] = true;
                }
                else
                {
                    Sqlinterface.SetCegConnectionok((string[])cegconok.ToArray(typeof(string)));
                    string egycon;
                    string id;
                    for (int i = 0; i < cegtabla.Rows.Count; i++)
                    {
                        egycon = cegtabla.Rows[i]["CEGCONNECTION"].ToString();
                        dt.Rows.Clear();
                        dt.Columns.Clear();
                        Sqlinterface.Cegconn(egycon);
                        Sqlinterface.Select(dt, egycon, "CEGSZERZODES", " where ALKALMAZAS_ID = " + alkid, "", true);
                        if(dt.Rows.Count==0)
                        {
                            ujcege[i] = true;
                            Vanujceg = true;
                        }
                    }
                }
                dt.Rows.Clear();
                dt.Columns.Clear();
                Sqlinterface.Select(dt, Userconn, "KEZELOK", "", "", true);
                if (dt.Rows.Count == 0)
                    NincsKezelo = true;
                else
                {
                    DataTable rgtable = new DataTable();
                    Sqlinterface.Select(rgtable, Userconn, "RENDSZERGAZDA", "", "", false);
                    if (rgtable.Rows.Count != 0)
                    {
                        Rgazdaid = rgtable.Rows[0]["RENDSZERGAZDA_ID"].ToString();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow rrow = dt.Rows[i];
                            RgazdaNev = rrow["SZOVEG"].ToString();
                            RgazdaJelszo = rrow["JELSZO"].ToString();
                        }
                    }
                    dt.Rows.Clear();
                    dt.Columns.Clear();
                    Sqlinterface.Select(dt, Userconn, "OSSZEF", " where KODTIPUS = 'KezeloAlkalm' and SORSZAM2 = " + alkid, "", false);
                    for (int i = 0; i < dt.Rows.Count; i++)
                        KezeloIdkArray.Add(dt.Rows[i]["SORSZAM1"].ToString());
                    if (KezeloIdkArray.Count == 0)
                    {

                        if(Rgazdaid!="-1")
                        {
                            KezeloIdkArray.Add(Rgazdaid);
                            KezalkalmUpdateKell = true;
                        }
                    }
                }
            }
        }

        //public bool Show(BejelformAlap bejelform)
        //{
        //    if (bejelform != null)
        //    {
        //        Bejelform = bejelform;
        //        return Show(true, "");
        //    }
        //    else
        //        return true;
        //}
        public bool Show(bool kellshow, Bitmap bitmap, ImageLayout layout)
        {
            return Show(kellshow,bitmap ,layout , "",null);
        }
        public bool Show(bool kellshow, Bitmap bitmap, ImageLayout layout, string bejelszov)
        {
            return Show(kellshow, bitmap, layout, bejelszov, null);
        }
        public bool Show(bool kellshow,Bitmap bitmap, ImageLayout layout,string bejelszov,Icon icon)
        {
            if (HibasAlkalmazas)
            {
                FakPlusz.MessageBox.Show("Nincs " + Alkalmazas + " nevü alkalmazás!");
                return true;
            }
            bool ok = true;
            if(bejelszov=="")
                bejelszov = Alkalmazas + " bejelentkezés";
            Bejelform = new BejelformAlap(this, bitmap, layout, bejelszov,icon);
            if (kellshow)
            {

                ok = Bejelform.ShowDialog() == DialogResult.OK;
            }
            if (!ok)
                return true;
            else
                return false;
        }
        public bool Bejeltolt()
        {
            int k;
            DataRow rgazdarow;
            FakUserInterface.ProgressRefresh();
            kezeloinfo = FakUserInterface.GetBySzintPluszTablanev("U", "KEZELOK");
            kezalkalminfo = FakUserInterface.GetOsszef("U", "KezeloAlkalm");
            rendszergazdainfo = FakUserInterface.GetBySzintPluszTablanev("U", "RENDSZERGAZDA");
            if (KezalkalmUpdateKell)
            {
                DataRow row = kezalkalminfo.Ujsor();
                row["PREV_ID2"] = alkid;
                row["SORSZAM2"] = alkid;
                row["PREV_ID1"] = Rgazdaid;
                row["SORSZAM1"] = Rgazdaid;
                FakUserInterface.Rogzit(kezalkalminfo);
                KezalkalmUpdateKell = false;
            }
            else if (Rgazdaid=="-1")
            {
                DataRow row = kezeloinfo.Ujsor();
                row["SZOVEG"] = Nev;
                row["JELSZO"] = jelszo;
                FakUserInterface.Rogzit(kezeloinfo);
                Kezeloid = kezeloinfo.DataView[0].Row["KEZELO_ID"].ToString();
                row = kezalkalminfo.Ujsor();
                row["PREV_ID2"] = alkid;
                row["SORSZAM2"] = alkid;
                row["PREV_ID1"] = Kezeloid;
                row["SORSZAM1"] = Kezeloid;
                FakUserInterface.Rogzit(kezalkalminfo);
                rgazdarow = rendszergazdainfo.Ujsor();
                rgazdarow["RENDSZERGAZDA_ID"] = Kezeloid;
                FakUserInterface.Rogzit(rendszergazdainfo);
            }
            rgazdarow = rendszergazdainfo.DataView[0].Row;
            Rgazdaid = rgazdarow["RENDSZERGAZDA_ID"].ToString();
            if (Kezeloid != "-1")
            {
                DataRow row = kezeloinfo.Find("KEZELO_ID", Kezeloid);
                string oldjelszo = row["JELSZO"].ToString();
                string newjelszo = "";
                if (oldjelszo == "")
                    newjelszo = jelszo;
                else if (oldjelszo != jelszo)
                    newjelszo = ujjelszo;
                if (newjelszo != "")
                {
                    row["JELSZO"] = newjelszo;
                    kezeloinfo.Modositott = true;
                    FakUserInterface.Rogzit(kezeloinfo);
                }
            }
            CsakEgyKezelo = KezeloIdkArray.Count < 2;
            Tablainfo ceginfo = FakUserInterface.Tablainfok.GetBySzintPluszTablanev("U", "CEGEK");
            DataView cegDataView = new DataView(ceginfo.Adattabla);
            cegDataView.Sort = "CEG_ID";
            string cegid;
            ArrayList ceginformacioarray = new ArrayList();
            ArrayList lezartceginformacioarray = new ArrayList();
            ArrayList aktceginformacioarray = new ArrayList();
            DataRow dr;
            DateTime aktualisdatum = DateTime.MinValue;
            DateTime indulodatum = DateTime.MinValue;
            string vegedatumstring = "";
            bool lezartceg = false;
            bool lezartev = false;
            cegszerzinfo = FakUserInterface.GetBySzintPluszTablanev("C", "CEGSZERZODES");
            cegkezszerepinfo = FakUserInterface.GetBySzintPluszTablanev("C", "CEGKEZELOKIOSZT");
            Tablainfo cegverzio = FakUserInterface.GetBySzintPluszTablanev("C", "CVERSION");
            Tablainfo[] infok = new Tablainfo[] { cegszerzinfo, cegkezszerepinfo,cegverzio };
            for (int i = 0; i < cegDataView.Count; i++)
            {
                FakUserInterface.ProgressRefresh();
                dr = cegDataView[i].Row;
                cegid = dr["CEG_ID"].ToString();
                string conn = dr["CEGCONNECTION"].ToString();
                if(cegconok.IndexOf(conn)!=-1)
                {
                    FakUserInterface.AktualCegid = Convert.ToInt64(cegid);
                    FakUserInterface.AktualCegconn = conn;
                    Sqlinterface.Cegconn(conn);
                    for (int j = 0; j < infok.Length; j++)
                    {
                        FakUserInterface.ProgressRefresh();
                        infok[j].Adattabla.Connection = conn;
                        infok[j].Adattabla.Select();
                    }
                    cegszerzinfo.DataView.RowFilter = "CEG_ID = " + cegid + " AND " + "ALKALMAZAS_ID = " + alkid;
                    DataRow row;
                    indulodatum = UjcegIndulodatum;
                    aktualisdatum = indulodatum;
                    if (cegszerzinfo.DataView.Count == 0)
                    {
                        FakUserInterface.ProgressRefresh();
                        row = cegszerzinfo.Ujsor();
                        row["ALKALMAZAS_ID"] = alkid;
                        row["INDULODATUM"] = indulodatum;
                        row["AKTUALISDATUM"] = indulodatum;
                        row["MODOSITOTT_M"] = "1";
                        aktualisdatum = indulodatum;
                    }
                    else
                    {
                        row = cegszerzinfo.DataView[0].Row;
                        indulodatum = Convert.ToDateTime(row["INDULODATUM"].ToString());
                        aktualisdatum = Convert.ToDateTime(row["AKTUALISDATUM"].ToString());
                        vegedatumstring = row["VEGEDATUM"].ToString();
                    
                    }
                    cegkezszerepinfo.DataView.RowFilter = "CEG_ID = " + cegid + " AND ALKALMAZAS_ID = " + alkid + " AND KEZELO_ID = " + Kezeloid;
                    if (cegkezszerepinfo.DataView.Count == 0)
                    {
                        FakUserInterface.ProgressRefresh();
                        row = cegkezszerepinfo.Ujsor();
                        row["ALKALMAZAS_ID"] = alkid;
                        row["KEZELO_ID"] = Kezeloid;
                        row["SZEREPKOD"] = "10"; // semmi
                    }
                    row = cegverzio.DataView[0].Row;
                    DateTime datumtol = Convert.ToDateTime(row["DATUMTOL"].ToString());
                    if (indulodatum.CompareTo(datumtol) != 0)
                    {
                        row["DATUMTOL"] = indulodatum;
                        row["MODOSITOTT_M"] = 1;
                        cegverzio.Modositott = true;
                    }
                    cegkezszerepinfo.DataView.RowFilter = "";
                    FakUserInterface.ProgressRefresh();
                    FakUserInterface.Rogzit(infok);
                }
            }
            for (int i = 0; i < cegDataView.Count; i++)
            {
                FakUserInterface.ProgressRefresh();
                dr = cegDataView[i].Row;
                cegid = dr["CEG_ID"].ToString();
                string conn = dr["CEGCONNECTION"].ToString();
                if (cegconok.IndexOf(conn) != -1)
                {
                    Sqlinterface.Cegconn(conn);
                    FakUserInterface.AktualCegid = Convert.ToInt64(cegid);
                    FakUserInterface.AktualCegconn = conn;
                    for (int j = 0; j < infok.Length; j++)
                    {
                        FakUserInterface.ProgressRefresh();
                        infok[j].Adattabla.Connection = conn;
                        infok[j].Adattabla.Select();
                    }
                    cegszerzinfo.DataView.RowFilter = "CEG_ID = " + cegid + " AND " + "ALKALMAZAS_ID = " + alkid;
                    DataRow szerzrow = cegszerzinfo.DataView[0].Row;
                    cegkezszerepinfo.DataView.RowFilter = "CEG_ID = " + cegid + " AND ALKALMAZAS_ID = " + alkid + " AND KEZELO_ID = " + Kezeloid;
                    DataRow kezrow = cegkezszerepinfo.DataView[0].Row;
                    cegkezszerepinfo.DataView.RowFilter = "";
                    int szerep = Convert.ToInt16(kezrow["SZEREPKOD"].ToString());
                    bool vanhozfer = CsakEgyKezelo || Kezeloid == Rgazdaid || szerep != (int)Base.KezSzint.Semmi;
                    if (Kezeloid == Rgazdaid) // && szerep == (int)Base.KezSzint.Semmi)
                        CsakEgyKezelo = false;
                    if (vanhozfer)
                    {
                        adatkezeloid = "-1";
                        if (szerep == (int)Base.KezSzint.Kezelo || szerep == (int)Base.KezSzint.Kiemeltkezelopluszkezelo)
                            adatkezeloid = Kezeloid;
                        vezetoid = "-1";
                        if (szerep == (int)Base.KezSzint.Vezeto)
                            vezetoid = Kezeloid;
                        kiemeltkezeloid = "-1";
                        if (szerep == (int)Base.KezSzint.Kiemeltkezelo || szerep == (int)Base.KezSzint.Kiemeltkezelopluszkezelo)
                            kiemeltkezeloid = Kezeloid;
                        indulodatum = Convert.ToDateTime(szerzrow["INDULODATUM"].ToString());
                        aktualisdatum = Convert.ToDateTime(szerzrow["AKTUALISDATUM"].ToString());
                        vegedatumstring = szerzrow["VEGEDATUM"].ToString();
                        if (vegedatumstring != "")
                            vegedatum = Convert.ToDateTime(vegedatumstring);
                        else
                            vegedatum = DateTime.MaxValue;
                        lezartceg = vegedatum.CompareTo(DateTime.Today) < 0;
                        k = cegDataView.Find(cegid);
                        if (!lezartceg)
                            aktceginformacioarray = ceginformacioarray;
                        else
                            aktceginformacioarray = lezartceginformacioarray;
                        if (FakUserInterface.BajVan)
                            return true;
                        FakUserInterface.ProgressRefresh();
                        Ceginformaciok ujceginfo = Ujceginformacio(ceginfo.Adattabla.Rows[k], k, cegid, Kezeloid, Rgazdaid, indulodatum, aktualisdatum, lezartceg, lezartev);
                        bool ok = true;
                        if (MainAlap != null)
                            ok = MainAlap.AlkalmazasKiegTolt(ujceginfo);
                        if (ok)
                            aktceginformacioarray.Add(ujceginfo);
                    }
                }
            }
            AktivCegInformaciok = null;
            LezartCegInformaciok = null;
//            string[] userparamok = null;
            if (ceginformacioarray.Count == 0 && lezartceginformacioarray.Count == 0)
            {
                FakUserInterface.CloseProgress();
                FakPlusz.MessageBox.Show("  Nincs hozzáférése!\nSzóljon a rendszergazdának!");
                return true;
            }
            DataTable userlogtable;
            Ceginformaciok ceginf;
            if (ceginformacioarray.Count != 0)
            {
                FakUserInterface.ProgressRefresh();
                CegNevek = new string[ceginformacioarray.Count];
                CegIdk = new string[ceginformacioarray.Count];
                for (int i = 0; i < ceginformacioarray.Count; i++)
                {
                    ceginf = (Ceginformaciok)ceginformacioarray[i];
                    CegNevek[i] = ceginf.CegNev;
                    CegIdk[i] = ceginf.CegId;
                    userlogtable = new DataTable("USERLOG");
                    Sqlinterface.Select(userlogtable, FakUserInterface.Userconn, "USERLOG", " where ALKALMAZAS_ID=" + alkid + " AND CEG_ID= " + CegIdk[i] + " AND KEZELO_ID = " + Kezeloid, " order by LAST_MOD DESC", true);
                    if (userlogtable.Rows.Count == 0)
                        ceginf.UserLogsor = null;
                    else
                        ceginf.UserLogsor = userlogtable.Rows[0];
                }
                AktivCegInformaciok = (Ceginformaciok[])ceginformacioarray.ToArray(typeof(Ceginformaciok));
                AktualCegInformaciok = AktivCegInformaciok;
                AktualCegInformacio = AktivCegInformaciok[0];
                LezartCeg = false;
            }
            if (lezartceginformacioarray.Count != 0)
            {
                FakUserInterface.ProgressRefresh();
                LezartCegNevek = new string[lezartceginformacioarray.Count];
                LezartCegIdk = new string[lezartceginformacioarray.Count];
                for (int i = 0; i < lezartceginformacioarray.Count; i++)
                {
                    ceginf = (Ceginformaciok)lezartceginformacioarray[i];
                    LezartCegNevek[i] = ceginf.CegNev;
                    LezartCegIdk[i] = ceginf.CegId;
                    userlogtable = new DataTable("USERLOG");
                    Sqlinterface.Select(userlogtable, FakUserInterface.Userconn, "USERLOG", " where ALKALMAZAS_ID=" + alkid + " AND CEG_ID= " + LezartCegIdk[i] + " AND KEZELO_ID = " + Kezeloid, " order by LAST_MOD DESC", true);
                    if (userlogtable.Rows.Count == 0)
                        ceginf.UserLogsor = null;
                    else
                        ceginf.UserLogsor = userlogtable.Rows[0];
                }
                LezartCegInformaciok = (Ceginformaciok[])lezartceginformacioarray.ToArray(typeof(Ceginformaciok));
                if (AktivCegInformaciok == null)
                {
                    AktualCegInformaciok = LezartCegInformaciok;
                    AktualCegInformacio = LezartCegInformaciok[0];
                    LezartCeg = true;
                }
            }
            if (elso)
            {
                FakUserInterface.ProgressRefresh();
                elso = false;
                bool megvan = false;
                int pos = -1;
                Tablainfo logtabinfo = FakUserInterface.GetBySzintPluszTablanev("U", "USERLOG");
                logtabinfo.Adattabla.Select();
                logtabinfo.DataView.RowFilter = "KEZELO_ID=" + Kezeloid + " AND ALKALMAZAS_ID =" + alkid;
                if (logtabinfo.DataView.Count != 0)
                {
                    DataRow row = logtabinfo.DataView[0].Row;
                    cegid = row["CEG_ID"].ToString();
                    UserContnev = row["USERCONTNEV"].ToString();
                    Parametertipus = Convert.ToInt16(row["PARAMETERTIPUS"].ToString());
                    UserParamok = row["USERPARAMOK"].ToString();
                    if (AktivCegInformaciok != null)
                    {
                        pos = (new ArrayList(CegIdk)).IndexOf(cegid);
                        if (pos != -1)
                        {
                            megvan = true;
                            AktualCegInformaciok = AktivCegInformaciok;
                            AktualCegInformacio = AktualCegInformaciok[pos];
                            LezartCeg = false;
                        }
                    }
                    if (!megvan && LezartCegInformaciok != null)
                    {
                        pos = (new ArrayList(LezartCegIdk)).IndexOf(cegid);
                        if (pos != -1)
                        {
                            megvan = true;
                            AktualCegInformaciok = LezartCegInformaciok;
                            AktualCegInformacio = LezartCegInformaciok[pos];
                            LezartCeg = true;
                        }
                    }
                }
                else
                {
                    cegid = AktualCegInformacio.CegId;
                    UserContnev = "";
                }
                logtabinfo.Adattabla.Rows.Clear();

                //for (int i = 0; i < CegInformaciok.Length; i++)
                //{
                //    FakUserInterface.ProgressRefresh();
                //    if (CegInformaciok[i].CegId == cegid)
                //    {
                //        megvan = true;
                //        AktualCeginformacio = CegInformaciok[i];
                //        AktualCegindex = AktualCeginformacio.Cegindex;
                //        //                        AktualComboIndex = i;
                //        AktivCegInformacio = AktualCeginformacio;
                //        AktivCegInformaciok = CegInformaciok;
                //        LezartCeg = false;
                //        break;

                //    }
                //}
                //if (!megvan)
                //{
                //    if (LezartCegInformaciok != null)
                //    {
                //        for (int i = 0; i < LezartCegInformaciok.Length; i++)
                //        {
                //            FakUserInterface.ProgressRefresh();
                //            if (LezartCegInformaciok[i].CegId == cegid)
                //            {
                //                AktualCeginformacio = LezartCegInformaciok[i];
                //                AktualCegindex = AktualCeginformacio.Cegindex;
                //                //                                AktualComboIndex = i;
                //                AktivCegInformacio = AktualCeginformacio;
                //                AktivCegInformaciok = LezartCegInformaciok;
                //                LezartCeg = true;
                //                megvan = true;
                //                break;
                //            }
                //        }
                //    }
                //}
                if (!megvan)
                {
                    UserParamok = "";
                    Parametertipus = 0;
//                    AktualCeginformacio = AktivCegInformacio;
                    LezartCeg = AktualCegInformacio.LezartCeg;
                }
                logtabinfo.DataView.RowFilter = "";
            }
            //if(ceginfo.Adattabla.Rows.Count>1)
            //    FakUserInterface.Cegadatok(0);
            return false;
        }
        private Ceginformaciok Ujceginformacio(DataRow dr, int cegindex, string cegid, string kezeloid, string rgazdaid, DateTime indulodat, DateTime aktdat, bool lezartceg, bool lezartev)
        {
            Ceginformaciok egyinf = new Ceginformaciok();
            egyinf.Bejel = this;
            egyinf.LezartCeg = lezartceg;
            egyinf.LezartEv = lezartev;
            egyinf.InduloDatum = indulodat;
            egyinf.AktualisDatum = aktdat;
            egyinf.VezetoId = vezetoid;
            egyinf.LezarasDatuma = vegedatum;
            egyinf.CegConnection = dr["CEGCONNECTION"].ToString();
            egyinf.CegNev = dr["SZOVEG"].ToString();
            egyinf.CegId = cegid;
            egyinf.Cegindex = cegindex;
            if (kezeloid == "-1")

                egyinf.KezeloiSzint = Base.KezSzint.Fejleszto;
            else
            {
                Tablainfo tabinfo = FakUserInterface.GetBySzintPluszTablanev("U", "KEZELOK");
                if (CsakEgyKezelo)
                    egyinf.KezeloiSzint = Base.KezSzint.Minden;
                else
                {
                    bool rgazda = rgazdaid == kezeloid;
                    bool vezeto = vezetoid == kezeloid;
                    bool kezelo = adatkezeloid == kezeloid;
                    bool kiemeltkezelo = kiemeltkezeloid == kezeloid;
                    if (kiemeltkezelo)
                    {
                        if (rgazda)
                        {
                            egyinf.KezeloiSzint = Base.KezSzint.Rendszergazdapluszkiemelt;
                            if (kezelo)
                                egyinf.KezeloiSzint = Base.KezSzint.Rendszergazdapluszkiemeltpluszkezelo;
                        }
                        else
                        {
                            egyinf.KezeloiSzint = Base.KezSzint.Kiemeltkezelo;
                            if (kezelo)
                                egyinf.KezeloiSzint = Base.KezSzint.Kiemeltkezelopluszkezelo;
                        }
                    }

                    else if (kezelo)
                    {
                        if (rgazda)
                            egyinf.KezeloiSzint = Base.KezSzint.Rendszergazdapluszkezelo;
                        else
                            egyinf.KezeloiSzint = Base.KezSzint.Kezelo;
                    }
                    else if (vezeto)
                    {
                        if (rgazda)
                            egyinf.KezeloiSzint = Base.KezSzint.Rendszergazdapluszvezeto;
                        else
                            egyinf.KezeloiSzint = Base.KezSzint.Vezeto;
                    }
                    else
                        egyinf.KezeloiSzint = Base.KezSzint.Rendszergazda;
                }
            }
            egyinf.Jogosultsagok();
            return egyinf;
        }
    }
}
