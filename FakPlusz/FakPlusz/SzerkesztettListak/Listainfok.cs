using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FakPlusz.Alapcontrolok;
using FakPlusz.Alapfunkciok;
using FakPlusz.UserAlapcontrolok;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Windows.Forms;

namespace FakPlusz.SzerkesztettListak
{
    /// <summary>
    /// Listainformaciok szerkesztett listak-/statisztikakhoz
    /// </summary>
    public class ListaInfok
    {
        /// <summary>
        /// a hivo
        /// </summary>
        public Altlistazoalap Hivo;
        /// <summary>
        /// true: lista
        /// false:statisztika
        /// </summary>
        public bool Listae;
        /// <summary>
        /// az elso tablainformacio
        /// </summary>
        public Tablainfo ElsoTabinfo;
        /// <summary>
        /// tablainfok kollekcioja
        /// </summary>
        public TablainfoCollection Tablainfok = new TablainfoCollection();
        /// <summary>
        /// oszlopok informacioja
        /// </summary>
        public Oszlopinfok Oszlopinfok = new Oszlopinfok();
        /// <summary>
        /// sorrendinformaciok
        /// </summary>
        public Sorrendinfok Sorrendinfok = new Sorrendinfok();
        /// <summary>
        /// matrixpontok infoja
        /// </summary>
        public Matrixpontinfok Matrixpontinfok = new Matrixpontinfok();
        /// <summary>
        /// feltetelek infoja
        /// </summary>
        public Feltetelinfok Feltetelinfok = new Feltetelinfok();
        /// <summary>
        /// sorfeltetelek infoja
        /// </summary>
        public Sorfeltetelinfok Sorfeltetelinfok = new Sorfeltetelinfok();
        /// <summary>
        /// oszlopfeltetelek infoja
        /// </summary>
        public Oszlopfeltetelinfok Oszlopfeltetelinfok = new Oszlopfeltetelinfok();
        /// <summary>
        /// osszesbeallitando id
        /// </summary>
        public ArrayList OsszesBeallitandoId = new ArrayList();
        /// <summary>
        /// osszes beallitott idertek
        /// </summary>
        public ArrayList OsszesBeallitottIdErtek = new ArrayList();
        /// <summary>
        /// a feltetelinfok kollekcioja
        /// </summary>
        public FeltetelinfoCollection FeltColl = null;
        private FakUserInterface FakUserInterface;
        private Tablainfo tabinfo = null;
        private Tablainfo Tabinfo;
        private DataView view;
        private DataRow dr;
        private DataTable table;
        private string sort;
        private string azontip = "";
        private string egyazontip = "";
        private string mezonev;
        private bool vanfeltben = false;
        private bool vansorfeltben = false;
        private bool vanoszlfeltben = false;
        private DataRow TablainfoRow;
        private string sorrendsorszam;
        private bool kellosszegzes;
        private bool osszegzendo;
        private bool atlagolando;
        private bool csakosszegsorba;
        private string oszlopsorszam;
        private string matrixsorszam;
        /// <summary>
        /// 
        /// </summary>
        public bool Parameterhiba = false;
        /// <summary>
        /// objectum letrehozasa
        /// </summary>
        /// <param name="hivo">
        /// hivo
        /// </param>
        /// <param name="osszesview">
        /// viewk listaja
        /// </param>
        public ListaInfok(Altlistazoalap hivo, ArrayList osszesview)
        {
            Hivo = hivo;
            Tabinfo = Hivo.Tabinfo;
            Listae = hivo.listae;
            FakUserInterface = hivo.FakUserInterface;
            TablainfoRow = Tabinfo.DataView[0].Row;
            int ig;
            if (Listae)
                ig = 2;
            else
                ig = 4;
            for (int l = 0; l < ig; l++)
            {
                view = (DataView)osszesview[0];   // parameterview
                table = view.Table;
                sort = view.Sort;
                view.Sort = "AZONTIP,MEZONEV";
                if (l == 0)
                {
                    azontip = "";
                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        string colnev = table.Columns[i].ColumnName;
                        if (colnev.Contains("SORSZAM"))
                            view.Sort = colnev + ",AZONTIP,MEZONEV";
                        else
                            view.Sort = "AZONTIP,MEZONEV";
                        azontip = "";
                        for (int j = 0; j < view.Count; j++)
                        {
                            dr = view[j].Row;
                            egyazontip = dr["AZONTIP"].ToString();
                            if (egyazontip != azontip)
                                tabinfo = FakUserInterface.GetByAzontip(egyazontip);
                            MezoinfoOsszeallit(dr);
                            if (vanfeltben || vansorfeltben || vanoszlfeltben || sorrendsorszam != "0" || kellosszegzes ||
                                osszegzendo || atlagolando || csakosszegsorba || oszlopsorszam != "0" || matrixsorszam != "0")
                            {
                                if (Tablainfok.IndexOf(tabinfo) == -1)
                                {
                                    Tablainfok.Add(tabinfo);
                                    Clear(tabinfo);
                                }
                                Mezoinfo mezoinfo = new Mezoinfo(tabinfo, mezonev, sorrendsorszam, kellosszegzes, osszegzendo,
                                    atlagolando, csakosszegsorba, oszlopsorszam, matrixsorszam);
                               switch (colnev)
                                {
                                    case "OSZLOPSORSZAM":
                                        if (oszlopsorszam != "0")
                                            Oszlopinfok.Mezoinfok.Add(tabinfo, mezoinfo);
                                        break;
                                    case "SORRENDSORSZAM":
                                        if (sorrendsorszam != "0")
                                        {
                                            Sorrendinfok.Mezoinfok.Add(tabinfo, mezoinfo);
                                            string ssort = tabinfo.Sort;
                                            if (ssort != "")
                                                ssort += ",";
                                            ssort += mezoinfo.ColumnInfo.ColumnName;
                                            tabinfo.Sort = ssort;
                                        }
                                        break;
                                    case "MATRIXSORSZAM":
                                        if (matrixsorszam != "0")
                                        {
                                            Matrixpontinfok.Mezoinfok.Add(tabinfo, mezoinfo);
                                        }
                                        break;
                                }
                            }
                            azontip = egyazontip;
                        }
                    }

                }
                else
                {
                    azontip = "";
                    string filt = view.RowFilter;
                    view.RowFilter = "VANFELTBEN = 'Igen'";
                    if (l != 1)
                        view.RowFilter += " OR VANSORFELTBEN = 'Igen' OR VANOSZLFELTBEN = 'Igen '";
                    for (int i = 0; i < view.Count; i++)
                    {
                        dr = view[i].Row;
                        egyazontip = dr["AZONTIP"].ToString();
                        if (egyazontip != azontip)
                            tabinfo = FakUserInterface.GetByAzontip(egyazontip);
                        azontip = egyazontip;
                        MezoinfoOsszeallit(dr);
                        Tablainfok.Add(tabinfo);
                        Mezoinfo mezoinfo = new Mezoinfo(tabinfo, mezonev, sorrendsorszam, kellosszegzes, osszegzendo,
                            atlagolando, csakosszegsorba, oszlopsorszam, matrixsorszam);
                        switch (l)
                        {
                            case 1:
                                if(dr["VANFELTBEN"].ToString()=="Igen")
                                    Feltetelinfok.Mezoinfok.Add(tabinfo, mezoinfo);
                                break;
                            case 2:
                                if(dr["VANSORFELTBEN"].ToString()=="Igen")
                                    Sorfeltetelinfok.Mezoinfok.Add(tabinfo, mezoinfo);
                                break;
                            case 3:
                                if(dr["VANOSZLFELTBEN"].ToString()=="Igen")
                                    Oszlopfeltetelinfok.Mezoinfok.Add(tabinfo, mezoinfo);
                                break;
                        }
                    }
                    view.RowFilter = filt;
                    DataView feltview = (DataView)osszesview[l];
                    switch (l)
                    {
                        case 1:
                            FeltColl = Feltetelinfok.FeltetelinfoCollection;
                            break;
                        case 2:
                            FeltColl = Sorfeltetelinfok.FeltetelinfoCollection;
                            break;
                        case 3:
                            FeltColl = Oszlopfeltetelinfok.FeltetelinfoCollection;
                            break;
                    }
                    for (int i = 0; i < feltview.Count; i++)
                        FeltColl.Add(new Feltetelinfo(this, FakUserInterface, feltview, i));
                }
                view.Sort = sort;
            }
            if (Matrixpontinfok.Mezoinfok.Count == 0)
            {
                Oszlopinfok.OszlopokOsszeAllit();
                Oszlopinfok.Osszegzesek(Sorrendinfok);
            }
            else
            {
                Oszlopfeltetelinfok.OszlopokOsszeAllit();
                Matrixpontinfok.OszlopokOsszeAllit();
            }
            Parameterhiba = false;
            Tablainfo gyoker = null;
            if (Tablainfok.Count == 0)
            {
                Parameterhiba = true;
                MessageBox.Show("Nincs még paraméterezés!");
                return;
            }
            for (int ii = 0; ii < Tablainfok.Count; ii++)
            {
                if (gyoker == null)
                {
                    gyoker = Tablainfok[ii].FirstTermParentTabinfo;
                    if (gyoker == null)
                        gyoker = Tablainfok[ii];
                }
                else
                {
                    Tablainfo ujgyoker = Tablainfok[ii].FirstTermParentTabinfo;
                    if(ujgyoker==null)
                        ujgyoker=Tablainfok[ii];
                    if (gyoker != ujgyoker)
                    {
                        string hibaszov = "";
                        for (int j = 0; j < Tablainfok.Count; j++)
                        {
                            if (gyoker != Tablainfok[j])
                            {
                                if (gyoker.TermChildTabinfo.IndexOf(Tablainfok[j]) == -1)
                                {
                                    if (hibaszov != "")
                                        hibaszov += ",\n";
                                    hibaszov += Tablainfok[j].TablaTag.Azonositok.Szoveg;
                                }
                            }
                        }
                        hibaszov += "\n Nem elérhetö " + gyoker.TablaTag.Azonositok.Szoveg + "-ból\nParaméterezze újra!";
                        MessageBox.Show(hibaszov);
                        Parameterhiba = true;
                        return;
                    }
                }

            }
            TablainfoCollection teljeschain = new TablainfoCollection();
            TablainfoCollection chain;
            int maxchain = 1;

            foreach (Tablainfo egyinfo1 in gyoker.TermChildTabinfo)
            {
                int ii = egyinfo1.TermChildTabinfo.Count;
                if (ii > maxchain)
                    maxchain = ii;
            }
            for(int i=0;i<gyoker.TermChildTabinfo.Count;i++)
                teljeschain.Add(gyoker.TermChildTabinfo[i]);
            int chaincount = teljeschain.Count;
            for (int i = 0; i < gyoker.TermChildTabinfo.Count; i++)
            {
                chain = gyoker.TermChildTabinfo[i].TermChildTabinfo;
                for (int j = 0; j < chain.Count; j++)
                    teljeschain.Add(chain[j]);
            }
            int teljescount = teljeschain.Count;
            if (chaincount < teljeschain.Count)
            {
                chain = teljeschain;
                do
                {
                    int ij = 0;
                    do
                    {
                        for (int i = 0; i < chain.Count; i++)
                        {
                            TablainfoCollection egychain = chain[i].TermChildTabinfo;
                            for (int j = 0; j < egychain.Count; j++)
                                teljeschain.Add(egychain[j]);
                            chaincount = teljeschain.Count;
                        }
                        ij++;
                        if (chain.Count > ij)
                            chain = chain[ij].TermChildTabinfo;
                        else if(chaincount!=teljeschain.Count)
                        {
                            ij = 0;
                            chain = teljeschain[chaincount - 1].TermChildTabinfo;
                        }
                        else
                            break;
                    } while (true);
                } while (chaincount != teljeschain.Count);

            }
            TablainfoCollection newtabinfok = new TablainfoCollection();
            string elsotabinfoid = "";
            ArrayList tabsorrend = new ArrayList();
            int maxindex=-1;
            object[] egyobj;
            ArrayList indexar = new ArrayList();
            for (int i = 0; i < Tablainfok.Count; i++)
            {
                egyobj = new object[2];
                egyobj[1] = Tablainfok[i];
                int j = -1;
                if (gyoker != Tablainfok[i])
                {
                    j = teljeschain.IndexOf(Tablainfok[i]);
                }
                egyobj[0] = j;
                indexar.Add(j);
                if (maxindex < j)
                    maxindex = j;
                tabsorrend.Add(egyobj);
            }
            int egyindex = -1;
            do
            {
                for (int i = 0; i < tabsorrend.Count; i++)
                {
                    egyobj = (object[])tabsorrend[i];
                    if (egyobj[0].ToString() == egyindex.ToString())
                    {
                        newtabinfok.Add((Tablainfo)egyobj[1]);
                        tabsorrend.RemoveAt(i);
                    }
                }
                egyindex++;
            } while (tabsorrend.Count > 0);
            if (newtabinfok[0].TermParentTabinfo != null)
                elsotabinfoid = newtabinfok[0].TermParentTabinfo.IdentityColumnName;
            else
                elsotabinfoid = newtabinfok[0].IdentityColumnName;
            ElsoTabinfo = newtabinfok[0];      
            if (ElsoTabinfo.TermParentTabinfo != null)
                elsotabinfoid = ElsoTabinfo.TermParentTabinfo.IdentityColumnName;
            else
                elsotabinfoid = ElsoTabinfo.IdentityColumnName;
            Tablainfok = newtabinfok;
            for (int i = 0; i < Tablainfok.Count; i++)
                Tablainfok[i].ElsoTabinfo = ElsoTabinfo;
            Feltetelinfok.Rendezes(this);
            if (!Listae)
            {
                Oszlopfeltetelinfok.Rendezes(this);
                Sorfeltetelinfok.Rendezes(this);
                Matrixpontinfok.Osszegzesek(this);
            }
            TablainfoCollection parentchain = ElsoTabinfo.TermParentTabinfoChain;
            TablainfoCollection childchain = ElsoTabinfo.TermChildTabinfo;
            OsszesBeallitandoId = ElsoTabinfo.BeallitandoIdkArray("", ElsoTabinfo,Tablainfok, OsszesBeallitandoId, OsszesBeallitottIdErtek);
            foreach (Tablainfo info in Tablainfok)
            {
                if (info.TablaColumns.IndexOf("DATUMTOL") != -1)
                    info.DatumString = Hivo.Datumtoligfeltetel;

                else if (info.TablaColumns.IndexOf("KEZDETE") != -1)
                    info.DatumString = Hivo.Kezdetefeltetel;
                if (info != ElsoTabinfo)
                {
                    info.BeallitandoIdkArray(elsotabinfoid, ElsoTabinfo,Tablainfok, OsszesBeallitandoId, OsszesBeallitottIdErtek);
                }
            }
            if (Feltetelinfok.Mezoinfok.Count != 0)
                Feltetelinfok.SortOsszeallit();
            if (Oszlopfeltetelinfok.Mezoinfok.Count != 0)
                Oszlopfeltetelinfok.SortOsszeallit();
            if (Sorfeltetelinfok.Mezoinfok.Count != 0)
                Sorfeltetelinfok.SortOsszeallit();
        }
        /// <summary>
        /// tablainfo altalanos listazas valtozoit kezdoertekre
        /// </summary>
        /// <param name="info">
        /// tablainfo
        /// </param>
        public void Clear(Tablainfo info)
        {
            info.ListaInfok = this;
            info.Tablainfok = Tablainfok;
            info.TablainfoSelect = "";
            info.TablainfoOszlopSelect = "";
            info.TablainfoSorSelect = "";
            info.SelectElemek = new ArrayList();
            info.OszlopSelectElemek = new ArrayList();
            info.SorSelectElemek = new ArrayList();
            info.RowFilterek = new ArrayList();
            info.OszlopRowFilterek = new ArrayList();
            info.SorRowFilterek = new ArrayList();
            info.Feltetelek = new FeltetelinfoCollection();
            info.OszlopFeltetelek = new FeltetelinfoCollection();
            info.SorFeltetelek = new FeltetelinfoCollection();
            info.RowFilterIndex = -1;
            info.OszlopRowFilterIndex = -1;
            info.SorRowFilterIndex = -1;
            info.Sort = "";
            info.OszlopSort = "";
            info.SorSort = "";
            info.OsszesBeallitandoId.Clear();
            info.OsszesBeallitandoOszlopId.Clear();
            info.OsszesBeallitandoSorId.Clear();
            info.OsszesBeallitottIdErtek.Clear();
            info.OsszesBeallitottOszlopIdErtek.Clear();
            info.OsszesBeallitottSorIdErtek.Clear();
            info.ElozoBeallitottIdertekek.Clear();
            info.ElozoBeallitottOszlopIdertekek.Clear();
            info.ElozoBeallitottSorIdertekek.Clear();
            info.SzuksegesIdk.Clear();
            info.SzuksegesOszlopIdk.Clear();
            info.SzuksegesSorIdk.Clear();
            info.SzuksegesIndexek.Clear();
            info.SzuksegesOszlopIndexek.Clear();
            info.SzuksegesSorIndexek.Clear();
            info.BeallitandoIdk.Clear();
            info.BeallitandoOszlopIdk.Clear();
            info.BeallitandoSorIdk.Clear();
            info.BeallitottIdertekek.Clear();
            info.BeallitottOszlopIdertekek.Clear();
            info.BeallitottSorIdertekek.Clear();
            info.BeallitottIndexek.Clear();
            info.BeallitottOszlopIndexek.Clear();
            info.BeallitottSorIndexek.Clear();
            info.sorrendazonositok.Clear();
            info.sorrendtartalomoszlopnevek.Clear();
            info.sorrendtartalmak.Clear();
            info.csakosszegsorba.Clear();
            info.sorrendmezoinfok.Clear();

            info.oszlopmezonevek.Clear();
            info.oszloptartalomnevek.Clear();
            info.oszlopszelessegek.Clear();
            info.oszlopnumericek.Clear();
            info.oszlopmezoinfok.Clear();

            info.DatumString = "";
            //info.OszlopDatumString = "";
            //info.SorDatumString = "";
            for (int i = 0; i < info.SpecDatumNevekArray.Count; i++)
                info.SpecDatumNevSzerepel[i] = false;
        }
        private void MezoinfoOsszeallit(DataRow dr)
        {
            DataTable table = dr.Table;
            vanfeltben = dr["VANFELTBEN"].ToString() == "Igen";
            vansorfeltben = false;
            if (table.Columns.IndexOf("VANSORFELTBEN") != -1)
                vansorfeltben = dr["VANSORFELTBEN"].ToString() == "Igen";
            vanoszlfeltben = false;
            if (table.Columns.IndexOf("VANOSZLFELTBEN") != -1)
                vanoszlfeltben = dr["VANOSZLFELTBEN"].ToString() == "Igen";
            mezonev = dr["MEZONEV"].ToString();
            sorrendsorszam = dr["SORRENDSORSZAM"].ToString();
            if(!Listae)
            {
                oszlopsorszam = "0";
                matrixsorszam = dr["MATRIXSORSZAM"].ToString();
                kellosszegzes = sorrendsorszam != "0";
                osszegzendo=matrixsorszam != "0";
                csakosszegsorba = true;
            }
            else
            {
                matrixsorszam = "0";
                oszlopsorszam = dr["OSZLOPSORSZAM"].ToString();
                kellosszegzes = dr["KELLOSSZEGZES"].ToString() == "Igen";
                osszegzendo = dr["OSSZEGZENDO"].ToString() == "Igen";
                csakosszegsorba = dr["CSAKOSSZEGSORBA"].ToString() == "Igen";
            }
            atlagolando = dr["ATLAGOLANDO"].ToString() == "Igen";
        }
    }
    /// <summary>
    /// Oszlopinformaciok
    /// </summary>
    public class Oszlopinfok
    {
        /// <summary>
        /// mezoinfok kollekcioja
        /// </summary>
        public MezoinfoCollection Mezoinfok = new MezoinfoCollection();
        /// <summary>
        /// fejsorok tombje
        /// </summary>
        public string[] Fejsorok;
        /// <summary>
        /// osszegek listaja
        /// </summary>
        public ArrayList Osszegek = new ArrayList();
        /// <summary>
        /// 
        /// </summary>
        public ArrayList AtlagOsszegek = new ArrayList();
        /// <summary>
        /// 
        /// </summary>
        public ArrayList AtlagDarabszam = new ArrayList();
        /// <summary>
        /// osszegzendo mezoinfok kollekcioja
        /// </summary>
        public MezoinfoCollection Osszegzendok = new MezoinfoCollection();
        /// <summary>
        /// 
        /// </summary>
        public MezoinfoCollection Atlagolandok = new MezoinfoCollection();
        /// <summary>
        /// Fejsorok ertekadasa
        /// </summary>
        public virtual void OszlopokOsszeAllit()
        {
            Fejsorok = new string[Mezoinfok.Count];
            for (int i = 0; i < Mezoinfok.Count; i++)
                Fejsorok[i] = Mezoinfok[i].FejSzoveg;
        }
        /// <summary>
        /// Osszegzendok ertekadasa, Osszegek nullazasa
        /// </summary>
        /// <param name="sorrendinfok"></param>
        public void Osszegzesek(Sorrendinfok sorrendinfok)
        {
            int osszegzendok = 0;
            int atlagolandok = 0;
            for (int i = 0; i < Mezoinfok.Count; i++)
            {
                Mezoinfo info = Mezoinfok[i];
                if (info.Osszegzendo)
                {
                    osszegzendok++;
                    Osszegzendok.Add(info);
                }
                if (info.Atlagolando)
                {
                    atlagolandok++;
                    Atlagolandok.Add(info);
                }
            }
            for (int i = 0; i <= sorrendinfok.Mezoinfok.Count; i++)
            {
                if (osszegzendok != 0)
                    Osszegek.Add(new Decimal[osszegzendok]);
                if (atlagolandok != 0)
                {
                    AtlagOsszegek.Add(new Decimal[atlagolandok]);
                    AtlagDarabszam.Add(new int[atlagolandok]);
                }
            }

        }
        /// <summary>
        /// mezoinfo aktualis tartalmanak feladasa Osszegek-be
        /// </summary>
        /// <param name="info">
        /// mezoinfo
        /// </param>
        public virtual void Osszegfeladasok(Mezoinfo info)
        {
            if (info.Osszegzendo)
                Osszegfeladasok(info, true);
            if (info.Atlagolando)
                Osszegfeladasok(info, false);
        }
        private void Osszegfeladasok(Mezoinfo info, bool osszege)
        {
            Tablainfo tabinfo = info.ColumnInfo.Tablainfo;
            tabinfo.GetOszlopTartalom(info);
            MezoinfoCollection osszegzendok = Osszegzendok;
            if (!osszege)
                osszegzendok = Atlagolandok;
            for (int i = 0; i < osszegzendok.Count; i++)
            {
                if (info == osszegzendok[i])
                {
                    Decimal feladando = Convert.ToDecimal(info.AktualisTartalom);
                    string feladstring = info.AktualisTartalom;
                    if (feladstring != "")
                    {
                        string[] split;
                        split = info.AktualisTartalom.Split(new char[] { Convert.ToChar(",") });
                        if (split.Length == 2 && split[1].Length > 2)
                        {
                            split[1] = split[1].Substring(0, 2);
                            feladstring = split[0] + "," + split[1];
                            feladando = Convert.ToDecimal(feladstring);
                        }
                    }

                    //Decimal feladando = egesz + tized;
                    if (osszege)
                    {
                        foreach (Decimal[] osszegek in Osszegek)
                            osszegek[i] = osszegek[i] + feladando;
                    }
                    else
                    {
                        foreach (int[] darabok in AtlagDarabszam)
                            darabok[i]++;
                        foreach (Decimal[] atlosszegek in AtlagOsszegek)
                            atlosszegek[i] = atlosszegek[i] + feladando;
                    }
                }

            }
            if (info.Atlagolando)
            {
            }
        }
        /// <summary>
        /// adott szintu osszegsor osszeallitasa, dt-ben uj sorba
        /// </summary>
        /// <param name="szint">
        /// szint
        /// </param>
        /// <param name="dt">
        /// adattabla
        /// </param>
        public virtual void OsszegsorOsszeallit(int szint, DataTable dt)
        {
            Decimal[] szintosszegek=null;
            Decimal[] atlagszintosszegek=null;
            int[] atlagdarabok = null;
            if (Osszegek.Count != 0 )  
                szintosszegek = (Decimal[])Osszegek[szint];
            if (AtlagOsszegek.Count != 0)
            {
                atlagszintosszegek = (Decimal[])AtlagOsszegek[szint];
                atlagdarabok = (int[])AtlagDarabszam[szint];
            }
            if(szintosszegek!=null || atlagszintosszegek!=null)
            {
                DataRow dr = dt.NewRow();
                for (int i = 0; i < dt.Columns.Count; i++)
                    dr[i] = "";
                int mindb = -1;
                if (Osszegzendok.Count != 0)
                    for (int i = 0; i < Osszegzendok.Count; i++)
                        OsszegsorOsszeallit(Osszegzendok[i], ref szintosszegek[i], ref mindb, dr);
                if (AtlagOsszegek.Count != 0)
                    for (int i = 0; i < Atlagolandok.Count; i++)
                        OsszegsorOsszeallit(Atlagolandok[i], ref atlagszintosszegek[i], ref atlagdarabok[i], dr);
                dt.Rows.Add(dr);
            }
        }
        private void OsszegsorOsszeallit(Mezoinfo info, ref Decimal szintosszeg, ref int darabszam, DataRow dr)
        {

            int oszl = Mezoinfok.IndexOf(info);
            bool atlagolando = info.Atlagolando;
            if (atlagolando)
            {
                if (darabszam == 0 || darabszam == -1)
                    szintosszeg = 0;
                else
                    szintosszeg = szintosszeg / darabszam;
            }
            string s = szintosszeg.ToString();
            string[] split = s.Split(new char[] { Convert.ToChar(",") });
            if (split.Length == 2 && split[1].Length > 2)
                s = split[0] + "," + split[1].Substring(0, 2);
            dr[oszl] = s;
            szintosszeg = 0;
            if (darabszam != -1)
                darabszam = 0;
        }
    }

    

    
    /// <summary>
    /// Sorrendinformaciok
    /// </summary>
    public class Sorrendinfok
    {
        /// <summary>
        /// mezoinformaciok kollekcioja
        /// </summary>
        public MezoinfoCollection Mezoinfok = new MezoinfoCollection();
    }
    /// <summary>
    /// Matrixpontok informacioja
    /// </summary>
    public class Matrixpontinfok : Oszlopinfok
    {
        /// <summary>
        /// listainformaciok
        /// </summary>
        public ListaInfok ListaInfok;
        /// <summary>
        /// az osszes tablainfo
        /// </summary>
        public TablainfoCollection OsszesTablainfo;
        /// <summary>
        /// feltetelinfok
        /// </summary>
        public Feltetelinfok Feltetelinfok;
        /// <summary>
        /// oszlopfeltetelek
        /// </summary>
        public Oszlopfeltetelinfok Oszlopfeltetelek;
        /// <summary>
        /// sorfeltetelek
        /// </summary>
        public Sorfeltetelinfok Sorfeltetelek;
        /// <summary>
        /// sorrendinformaciok
        /// </summary>
        public Sorrendinfok Sorrendinfok;
        /// <summary>
        /// sorrend szovegei
        /// </summary>
        public string[] Sorrendszovegek;
        /// <summary>
        /// sorrendmezok szama
        /// </summary>
        public int SorrendmezokSzama = 0;
        /// <summary>
        /// sorok szama
        /// </summary>
        public int SorokSzama = 0;
        /// <summary>
        /// oszlopok szama
        /// </summary>
        public int OszlopokSzama = 0;
        /// <summary>
        /// sordefinicio tablainfoi
        /// </summary>
        public ArrayList SorTabinfok = new ArrayList();
        /// <summary>
        /// kell a sorrekord ? listaja
        /// </summary>
        public ArrayList SorRekordKell = new ArrayList();
        /// <summary>
        /// kell rowfilter? listaja
        /// </summary>
        public ArrayList SorRowFilterKell = new ArrayList();
        /// <summary>
        /// oszlopdefiniciok tablainfoi
        /// </summary>
        public ArrayList OszlopTabinfok = new ArrayList();
//        public ArrayList Osszegek = new ArrayList();
        /// <summary>
        /// Osszegzendok es Osszegek osszeallitasa osszegfeladashoz
        /// </summary>
        /// <param name="listainfok">
        /// listainformaciok
        /// </param>
        public void Osszegzesek(ListaInfok listainfok)
        {
            ListaInfok = listainfok;
            OsszesTablainfo = listainfok.Tablainfok;
            int matrixpontdb = Mezoinfok.Count;
            Sorrendinfok = listainfok.Sorrendinfok;
            Feltetelinfok = listainfok.Feltetelinfok;
            SorrendmezokSzama = Sorrendinfok.Mezoinfok.Count;
            Sorrendszovegek = new string[SorrendmezokSzama];
            Oszlopfeltetelek = listainfok.Oszlopfeltetelinfok;
            OszlopokSzama = Oszlopfeltetelek.FeltetelinfoCollection.OszlopokSzama;
            OszlopTabinfok = Oszlopfeltetelek.FeltetelinfoCollection.OszlopTablainfok;
            Sorfeltetelek = listainfok.Sorfeltetelinfok;
            SorokSzama = Sorfeltetelek.FeltetelinfoCollection.SorokSzama;
            SorTabinfok = Sorfeltetelek.FeltetelinfoCollection.SorTablainfok;
            SorRekordKell = Sorfeltetelek.FeltetelinfoCollection.SorRekordKell;
            SorRowFilterKell = Sorfeltetelek.FeltetelinfoCollection.SorRowFilterKell;
            Tabinfoktolt();
            //int osszegzendok = 0;
            //int atlagolandok = 0;
            for (int i = 0; i <= SorrendmezokSzama; i++)
            {
                ArrayList sorarray = new ArrayList();
                for (int j = 0; j <= SorokSzama; j++)
                {
                    ArrayList oszlarray = new ArrayList();
                    for (int k = 0; k <= OszlopokSzama; k++)
                    {
                        ArrayList matrixarray = new ArrayList();
                        for (int l = 0; l < matrixpontdb; l++)
                            matrixarray.Add(new Decimal[] { 0, 0 });
                        oszlarray.Add(matrixarray);
                    }
                    sorarray.Add(oszlarray);
                }
                Osszegek.Add(sorarray);
            }

            //for (int i = 0; i < matrixpontdb; i++)
            //{
            //    Mezoinfo info = Mezoinfok[i];
            //    info.Osszegek = new ArrayList();
            //    for (int j = 0; j <= SorrendmezokSzama; j++)
            //    {
            //        ArrayList sorrendarray = new ArrayList();
            //        info.Osszegek.Add(sorrendarray);
            //        for (int k = 0; k <= SorokSzama; k++)
            //        {
            //            ArrayList sorarray = new ArrayList();
            //            sorrendarray.Add(sorarray);
            //            for (int l = 0; l <= OszlopokSzama; l++)
            //            {
            //                ArrayList oszloparray = new ArrayList();
            //                sorarray.Add(oszloparray);
            //                oszloparray.Add(new Decimal[] { 0, 0 });
            //            }
            //        }
            //    }
            //}
            for (int i = 0; i < 1 + (OszlopokSzama + 1) * matrixpontdb; i++)
            {
                if (i == 0)
                    ListaInfok.Hivo.report.Section3.ReportObjects[i].ObjectFormat.HorizontalAlignment = Alignment.LeftAlign;
                else
                    ListaInfok.Hivo.report.Section3.ReportObjects[i].ObjectFormat.HorizontalAlignment = Alignment.RightAlign;
            }
            for (int i = 1; i < ListaInfok.Hivo.report.Section2.ReportObjects.Count; i++)
                ListaInfok.Hivo.report.Section2.ReportObjects[i].ObjectFormat.HorizontalAlignment = Alignment.RightAlign;
        }
        /// <summary>
        /// adattablak TablainfoSelect,RowFilterek,SorSelectElemek,SorRowFilterek,OszlopRowFilterek,OszlopSelectElemek osszeallitasa
        /// </summary>
        public void Tabinfoktolt()
        {
            for (int i = 0; i < OsszesTablainfo.Count; i++)
            {
                Tablainfo egytabinfo = OsszesTablainfo[i];
                ArrayList rowfilterek = egytabinfo.RowFilterek;
                string feltselectstring = egytabinfo.TablainfoSelect;
                ArrayList sorrowfilterek = egytabinfo.SorRowFilterek;
                ArrayList sorfeltselectek = egytabinfo.SorSelectElemek;
                ArrayList oszloprowfilterek = egytabinfo.OszlopRowFilterek;
                ArrayList oszlopfeltselectek = egytabinfo.OszlopSelectElemek;
                bool feltselectnemures = feltselectstring != "" && (sorfeltselectek.Count != 0 || oszlopfeltselectek.Count != 0);
                if (sorfeltselectek.Count != 0)
                {
                    if (feltselectnemures)
                        feltselectstring += " AND (";
                    else
                    {
                        feltselectnemures = true;
                        feltselectstring += "(";
                    }
                    for (int j = 0; j < sorfeltselectek.Count; j++)
                    {
                        string filt = "";
                        ArrayList egysorsel = (ArrayList)sorfeltselectek[j];
                        for (int k = 0; k < egysorsel.Count; k++)
                        {
                            Feltetelinfo egyfelt = (Feltetelinfo)egysorsel[k];
                            string egyfeltsel = ((Feltetelinfo)egysorsel[k]).FeltetelSorSelect;
                            string esvagy = "";
                            if (k < egysorsel.Count - 1)
                            {
                                esvagy = egyfelt.EsVagy;
                                if (esvagy == "" || esvagy == "VAGY")
                                    esvagy = " OR ";
                                else
                                    esvagy = " AND ";
                            }

                            filt += egyfeltsel + esvagy;
                            if (!feltselectstring.EndsWith("("))
                                feltselectstring += " OR ";
                            feltselectstring += egyfeltsel;
                        }
                        sorrowfilterek.Add(filt);
                    }
                }
                if (oszlopfeltselectek.Count != 0)
                {

                    if (sorfeltselectek.Count == 0)
                    {
                        if (feltselectnemures)
                            feltselectstring += " AND (";
                        else
                        {
                            feltselectnemures = true;
                            feltselectstring += "(";
                        }
                    }
                    for (int j = 0; j < oszlopfeltselectek.Count; j++)
                    {
                        string filt = "";
                        ArrayList egysorsel = (ArrayList)oszlopfeltselectek[j];
                        for (int k = 0; k < egysorsel.Count; k++)
                        {
                            Feltetelinfo egyfelt = (Feltetelinfo)egysorsel[k];
                            string egyfeltsel = ((Feltetelinfo)egysorsel[k]).FeltetelSorSelect;
                            string esvagy = "";
                            if (k < egysorsel.Count - 1)
                            {
                                esvagy = egyfelt.EsVagy;
                                if (esvagy == "" || esvagy == "VAGY")
                                    esvagy = " OR ";
                                else
                                    esvagy = " AND ";
                            }

                            filt += egyfeltsel + esvagy;
                            if (!feltselectstring.EndsWith("("))
                                feltselectstring += " OR ";
                            feltselectstring += egyfeltsel;
                        }
                        oszloprowfilterek.Add(filt);
                    }
                }
                if (feltselectnemures)
                    feltselectstring += ")";
                egytabinfo.TablainfoSelect = feltselectstring;
            }
        }
        /// <summary>
        /// sor-,oszlopindex alapjan osszegfeladasok az osszes szintre
        /// </summary>
        /// <param name="sorindex">
        /// sorindex
        /// </param>
        /// <param name="oszlopindex">
        /// oszlopindex
        /// </param>
        public void Osszegfeladasok(int sorindex, int oszlopindex)
        {
            ArrayList szintosszegek;
            ArrayList sorarray;
            ArrayList oszloparray;
            Decimal[] osszegek;
            Decimal[] feladandok = new Decimal[Mezoinfok.Count];
            int[] darabszamok = new int[Mezoinfok.Count];
            bool atlagolando = false;
            for (int i = 0; i < feladandok.Length; i++)
            {
                Mezoinfo info = Mezoinfok[i];
                atlagolando = info.Atlagolando;
                Tablainfo tabinfo = info.ColumnInfo.Tablainfo;
                tabinfo.GetOszlopTartalom(info);
                if (atlagolando)
                    darabszamok[i]++;
                string feladstring = info.AktualisTartalom;
                if (feladstring != "")
                {
                    string[] split;
                    split = info.AktualisTartalom.Split(new char[] { Convert.ToChar(",") });
                    if (split.Length == 2 && split[1].Length > 2)
                    {
                        split[1] = split[1].Substring(0, 2);
                        feladstring = split[0] + "," + split[1];
                    }
                }
                Decimal feladando = Convert.ToDecimal(feladstring);
                feladandok[i] = feladandok[i] + feladando;
            }
            for (int i = 0; i < SorrendmezokSzama; i++)
            {
                Sorrendszovegek[i] = Sorrendinfok.Mezoinfok[i].ColumnInfo.Tablainfo.AktualViewRow[Sorrendinfok.Mezoinfok[i].TartalomOszlopNev].ToString();
                if (Sorrendszovegek[i] == "")
                    Sorrendszovegek[i] = "Üres az azonositó";
            }

            //for (int i = 0; i < Mezoinfok.Count; i++)
            //{
            //    ArrayList Osszegek = Mezoinfok[i].Osszegek;

            for (int j = 0; j <= SorrendmezokSzama; j++)
            {
                szintosszegek = (ArrayList)Osszegek[j];
                sorarray = (ArrayList)szintosszegek[sorindex];
                oszloparray = (ArrayList)sorarray[oszlopindex];
                for (int i = 0; i < Mezoinfok.Count; i++)
                {
                    osszegek = (Decimal[])oszloparray[i];
                    osszegek[0] += feladandok[i];
                    osszegek[1] += darabszamok[i];
                }
            }
        }
        
        /// <summary>
        /// adott szint alapjan adattabla uj osszegsoranak osszeallitasa
        /// </summary>
        /// <param name="szint">
        /// kivant szint
        /// </param>
        /// <param name="dt">
        /// adattabla
        /// </param>
        public override void OsszegsorOsszeallit(int szint, DataTable dt)
        {
            DataRow dr;
            ArrayList szintosszegek = (ArrayList)Osszegek[szint];
            for (int i = 0; i <= SorokSzama ; i++)
            {
                dr = dt.NewRow();
                if (i < SorokSzama)
                    dr[0] = Sorfeltetelek.Fejsorok[i];
                else
                {
                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                    dr[0] = "Összesen";
                }
                ArrayList sorarray = (ArrayList)szintosszegek[i];
                Decimal kiirando;
                Decimal db;
                int oszlopindex = 0;
                for (int k = 0; k <= OszlopokSzama; k++)
                {
 //                   oszlopindex++;
                    ArrayList oszloparray = (ArrayList)sorarray[k];
                    for(int l=0;l<Mezoinfok.Count;l++)
                    {
 //                       oszlopindex++;
                        Decimal[] osszegek = (Decimal[])oszloparray[l];
                        kiirando = osszegek[0];
                        db = osszegek[1];
                        Mezoinfo info=Mezoinfok[l];
                        if (info.Atlagolando)
                        {
                            if (db == 0)
                                kiirando = 0;
                            else
                                kiirando = kiirando / db;
                        }
                        string s = kiirando.ToString();
                        int vesszoindex = s.IndexOf(",");
                        if (vesszoindex != -1)
                        {
                            string egesz = s.Substring(0, vesszoindex);
                            string tized = s.Substring(vesszoindex + 1);
                            if (tized.Length > 2)
                            {
                                tized = tized.Substring(0, 2);
                                s = egesz + "," + tized;
                            }
                        }
                        oszlopindex++;
                        dr[oszlopindex] = s;
                        osszegek[0] = 0;
                        osszegek[1] = 0;
                    }
                }
                dt.Rows.Add(dr);
            }
        }
    }
            //ArrayList sorarray = (ArrayList)Osszegek[szint];
            //ArrayList atlagdbarray = (ArrayList)AtlagDarabszam[szint];
            //for (int i = 0; i < sorarray.Count; i++)
            //{
            //    dr = dt.NewRow();
            //    ArrayList oszloparray = (ArrayList)sorarray[i];

            //    int oszlopindex = 1;
            //    foreach(Decimal[] osszegek in oszloparray)
            //    {
            //        for (int k = 0; k < osszegek.Length; k++)
            //        {
            //            string s = osszegek[k].ToString();
            //            osszegek[k] = 0;
            //            //string egeszresz = "";
            //            //string tized = "";
            //            //switch (tizedesek[k])
            //            //{
            //            //    case 1:
            //            //        if (s.Length == 1)
            //            //        {
            //            //            egeszresz = "0";
            //            tized = s;
            //        }
            //        else
            //        {
            //            egeszresz = s.Substring(0, s.Length - 1);
            //            tized = s.Substring(s.Length - 1);
            //        }
            //        break;
            //    default:
            //        if (s.Length < 2)
            //        {
            //            egeszresz = "0";
            //            if (s.Length == 1)
            //                tized = s + "0";
            //            else
            //                tized = s;
            //        }
            //        else
            //        {
            //            egeszresz = s.Substring(0, s.Length - 2);
            //            tized = s.Substring(s.Length - 2);
            //        }
            //        break;
            //}
            //s = egeszresz + "," + tized;
  //      }
 //   }
//                dt.Rows.Add(dr);
//                dt.Rows.Add(dt.NewRow());

 //       }
 //   }
    /// <summary>
    /// Feltetelinformaciok
    /// </summary>
    public class Feltetelinfok
    {
        /// <summary>
        /// Feltetelinformaciok kollekcioja
        /// </summary>
        public FeltetelinfoCollection FeltetelinfoCollection = new FeltetelinfoCollection();
        /// <summary>
        /// mezoinformaciok kollekcioja
        /// </summary>
        public MezoinfoCollection Mezoinfok = new MezoinfoCollection();
        /// <summary>
        /// A feltetelinfo kollekcio tipusa szerint a tablainformaciok
        /// Sort/OszlopSort/SorSort osszeallitasa
        /// </summary>
        public void SortOsszeallit()
        {
            string tip = FeltetelinfoCollection.tipus;
            for (int i = 0; i < Mezoinfok.Count; i++)
            {
                Tablainfo tabinfo = Mezoinfok[i].ColumnInfo.Tablainfo;
                for (int j = 0; j < FeltetelinfoCollection.Tablainfok.Count; j++)
                {
                    if (tabinfo == FeltetelinfoCollection.Tablainfok[j])
                    {
                        string sortelem=Mezoinfok.TabinfoSortok[i].ToString();
                        string egysort="";
                        switch (tip)
                        {
                            case "feltetel":
                                egysort = tabinfo.Sort;
                                if (egysort != "")
                                {
                                    if (!egysort.Contains(sortelem))
                                        egysort += "," + sortelem;
                                }
                                else
                                    egysort = sortelem;
                                tabinfo.Sort = egysort;
                                break;
                            case "oszlop":
                                egysort = tabinfo.OszlopSort;
                                if (egysort != "")
                                {
                                    if (!egysort.Contains(sortelem))
                                        egysort += "," + sortelem;
                                }
                                else
                                    egysort = sortelem;
                                tabinfo.OszlopSort = egysort;
                                break;
                            case "sor":
                                egysort = tabinfo.SorSort;
                                if (egysort != "")
                                {
                                    if (!egysort.Contains(sortelem))
                                        egysort += "," + sortelem;
                                }
                                else
                                    egysort = sortelem;
                                tabinfo.SorSort = egysort;
                                break;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// a feltetelinfok kollekciojanak Tabinfoktolt() metodusat hivja
        /// </summary>
        public virtual void Tabinfoktolt()
        {
            FeltetelinfoCollection.Tabinfoktolt();
        }
        /// <summary>
        /// a feltetelinfok kollekciojanak Rendezes(listainformacio) metodusat hivja
        /// </summary>
        /// <param name="listainfo">
        /// a listainformacio
        /// </param>
        public virtual void Rendezes(ListaInfok listainfo)
        {
            FeltetelinfoCollection.Rendezes(listainfo);
        }
    }
    /// <summary>
    /// Sorfeltetelinformaciok
    /// </summary>
    public class Sorfeltetelinfok : Oszlopfeltetelinfok
    {
        /// <summary>
        /// objectum eloallitas, tipus = "sor"
        /// </summary>
        public Sorfeltetelinfok()
        {
            base.FeltetelinfoCollection.tipus = "sor";
        }
        /// <summary>
        /// Fejsorok tombjenek osszeallitasa
        /// </summary>
        public override void OszlopokOsszeAllit()
        {
            int count = base.FeltetelinfoCollection.Reszelemek.Count;
            Fejsorok = new string[count];
            for (int i = 0; i < count; i++)
            {
                ArrayList ar = (ArrayList)base.FeltetelinfoCollection.Reszelemek[i];
                string fejsor = "";
                for (int j = 0; j < ar.Count; j++)
                {
                    Feltetelinfo info = (Feltetelinfo)ar[j];
                    fejsor += info.ColumnInfo.Caption + " " + info.Relacio+" " + info.Ertek + " " + info.EsVagy;
                    if (info.EsVagy != "")
                        fejsor += "\n";
                }
                Fejsorok[i] = fejsor;
            }
        }
        /// <summary>
        /// a feltetelinfok kollekciojanak FeltRendezes(listainfo) metodusat hivja, utana OszlopokOsszeAllit()
        /// </summary>
        /// <param name="listainfo">
        /// listainfo
        /// </param>
        public override void Rendezes(ListaInfok listainfo)
        {
            base.FeltetelinfoCollection.FeltRendezes(listainfo);
            OszlopokOsszeAllit();
        }
    }
    /// <summary>
    /// Oszlopfeltetelek
    /// </summary>
    public class Oszlopfeltetelinfok:Feltetelinfok
    {
        /// <summary>
        /// Fejsorok tombje
        /// </summary>
        public string[] Fejsorok;
        /// <summary>
        /// objectum eloallitasa, tipus = "oszlop"
        /// </summary>
        public Oszlopfeltetelinfok()
        {
            base.FeltetelinfoCollection.tipus = "oszlop";
        }
        /// <summary>
        /// Fejsorok tomb eloallitasa
        /// </summary>
        public virtual void OszlopokOsszeAllit()
        {
            int count = base.FeltetelinfoCollection.Count;
            Fejsorok = new string[count];
            for (int i = 0; i < count; i++)
            {
                Feltetelinfo info = base.FeltetelinfoCollection[i];
                Fejsorok[i] = info.Ertek;
                if(info.Relacio!="=")
                    Fejsorok[i] = info.ColumnInfo.Caption + " " + info.Relacio + " " + info.Ertek;
            }
        }
        /// <summary>
        /// feltetelinfo kollekcio FeltRendezes(listainfo) metodusat hivja
        /// </summary>
        /// <param name="listainfo">
        /// listainfo
        /// </param>
        public override void Rendezes(ListaInfok listainfo)
        {
            base.FeltetelinfoCollection.FeltRendezes(listainfo);
        }
    }
    /// <summary>
    /// mezoinformaciok kollekcioja
    /// </summary>
    public class MezoinfoCollection : ArrayList
    {
        /// <summary>
        /// tablainfok kollekcioja
        /// </summary>
        public TablainfoCollection Tablainfok = new TablainfoCollection();
        /// <summary>
        /// tablainfo sortok listaj
        /// </summary>
        public ArrayList TabinfoSortok = new ArrayList();
        /// <summary>
        /// mezoinformacio mezejenek informacio kollekcioja
        /// </summary>
        public ColCollection Colok = new ColCollection();
        private ArrayList mezonevek = new ArrayList();
        /// <summary>
        /// uj mezoinfo hozzaadas a kollekciohoz
        /// </summary>
        /// <param name="tabinfo">
        /// tablainformacio
        /// </param>
        /// <param name="mezoinfo">
        /// a mezoinformacio
        /// </param>
        public void Add(Tablainfo tabinfo, Mezoinfo mezoinfo)
        {
            Tablainfok.Add(tabinfo);
            string mezonev = mezoinfo.ColumnInfo.ColumnName;
            int i = Colok.IndexOf(mezonev);
            if (i == -1)
            {
                Colok.Add(mezoinfo.ColumnInfo);
                mezonevek.Add(mezonev);
                tabinfo.SorrendInfok(mezoinfo, this);
                tabinfo.OszlopInfok(mezoinfo, this);
                i = base.Add(mezoinfo);
                string sortelem = mezoinfo.ColumnInfo.ColumnName;
                TabinfoSortok.Add(sortelem);
            }
        }
        /// <summary>
        /// kereses index alapjan
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public new Mezoinfo this[int i]
        {
            get { return (Mezoinfo)base[i]; }
        }
        /// <summary>
        /// kereses mezonev alapjan
        /// </summary>
        /// <param name="mezonev"></param>
        /// <returns></returns>
        public Mezoinfo this[string mezonev]
        {
            get
            {
                int i = mezonevek.IndexOf(mezonev);
                if (i == -1)
                    return null;
                else
                    return (Mezoinfo)this[i];
            }
        }
    }
    /// <summary>
    /// mezoinformacio a feladat parameterezese alapjan
    /// </summary>
    public class Mezoinfo
    {
        /// <summary>
        /// </summary>
        public string SorrendSorszam = "0";
        /// <summary>
        /// </summary>
        public bool KellOsszegzes = false;
        /// <summary>
        /// </summary>
        public bool Osszegzendo = false;
        /// <summary>
        /// </summary>
        public bool Atlagolando = false;
        /// <summary>
        /// </summary>
        public bool CsakOsszegsorba = false;
        /// <summary>
        /// </summary>
        public string OszlopSorszam = "0";
        /// <summary>
        /// </summary>
        public string MatrixSorszam = "0";
        /// <summary>
        /// </summary>
        public Cols ColumnInfo = null;
        /// <summary>
        /// </summary>
        public int OszlopSzelesseg = 0;
        /// <summary>
        /// </summary>
        public string FejSzoveg = "";
        /// <summary>
        /// </summary>
        public string TartalomOszlopNev = "";
        /// <summary>
        /// </summary>
        public string AktualisTartalom = "";
        /// <summary>
        /// </summary>
        public string ElozoSorrendTartalom = "";
        /// <summary>
        /// 
        /// </summary>
        public ArrayList Osszegek = new ArrayList();
        //public ArrayList AtlagDarabszamok = new ArrayList();
        /// <summary>
        /// objectum letrehozasa
        /// </summary>
        /// <param name="tabinfo">
        /// a tablainformacio
        /// </param>
        /// <param name="mezonev">
        /// a mezo neve
        /// </param>
        /// <param name="sorrendsorszam">
        /// sorrend sorszama
        /// </param>
        /// <param name="kellosszegzes">
        /// kell osszegzes?
        /// </param>
        /// <param name="osszegzendo">
        /// osszegzendo a mezo tartalma?
        /// </param>
        /// <param name="atlagolando">
        /// atlagolando a mezotartalom?
        /// </param>
        /// <param name="csakosszegsorba">
        /// csak osszegsorban irando?
        /// </param>
        /// <param name="oszlopsorszam">
        /// oszlop sorszama
        /// </param>
        /// <param name="matrixsorszam">
        /// matrixelem sorszama
        /// </param>
        public Mezoinfo(Tablainfo tabinfo, string mezonev, string sorrendsorszam, bool kellosszegzes, bool osszegzendo, bool atlagolando,
            bool csakosszegsorba, string oszlopsorszam, string matrixsorszam)
        {
            ColumnInfo = tabinfo.TablaColumns[mezonev];
            if (ColumnInfo.Comboe)
                TartalomOszlopNev = mezonev + "_K";
            else
                TartalomOszlopNev = mezonev;
            SorrendSorszam = sorrendsorszam;
            KellOsszegzes = kellosszegzes;
            Osszegzendo = osszegzendo;
            Atlagolando = atlagolando;
            CsakOsszegsorba = csakosszegsorba;
            OszlopSorszam = oszlopsorszam;
            MatrixSorszam = matrixsorszam;
            OszlopSzelesseg = ColumnInfo.Caption.Length;
            if (OszlopSzelesseg < ColumnInfo.InputMaxLength)
                OszlopSzelesseg = ColumnInfo.InputMaxLength;
            FejSzoveg = ColumnInfo.Caption;
            if (atlagolando)
                FejSzoveg += " átlag";
            int i=tabinfo.SpecDatumNevekArray.IndexOf(mezonev);
            if (i != -1)
                tabinfo.SpecDatumNevSzerepel[i] = true;
        }
    }
    /// <summary>
    /// feltetelinformaciok kollekcioja
    /// </summary>
    public class FeltetelinfoCollection : ArrayList
    {
        /// <summary>
        /// tipus: feltetel/sor/oszlop
        /// </summary>
        public string tipus = "feltetel";
        /// <summary>
        /// listainformaciok
        /// </summary>
        public ListaInfok ListaInfok;
        /// <summary>
        /// tablainformaciok
        /// </summary>
        public TablainfoCollection Tablainfok = new TablainfoCollection();
        private ArrayList nyitozarojelek = new ArrayList();
        private ArrayList zarozarojelek = new ArrayList();
        private ArrayList zarojelpoziciok = new ArrayList();
        private ArrayList espoziciok = new ArrayList();
        private ArrayList vagypoziciok = new ArrayList();
        private ArrayList urespoziciok = new ArrayList();
        /// <summary>
        /// feltetel reszelemeinek listaja
        /// </summary>
        public ArrayList Reszelemek = new ArrayList();
        private Tablainfo tabinfo;
        private int kezdopoz = 0;
        private int ujkezdopoz = 0;
        private int vegpoz = 0;
        private int vagypoz = -1;
        private int nextvagypoz = -1;
        private int espoz = -1;
        private int nextespoz = -1;
        private int urespoz = -1;
        private int nexturespoz = -1;
        private int secondnyito = -1;
        private int nyitozpoz = -1;
        private int zarozpoz = -1;
        /// <summary>
        /// sorok szama
        /// </summary>
        public int SorokSzama = 0;
        /// <summary>
        /// oszlopok szama
        /// </summary>
        public int OszlopokSzama = 0;
        /// <summary>
        /// sor tablainformaciok listaja
        /// </summary>
        public ArrayList SorTablainfok = new ArrayList();
        /// <summary>
        /// sor rowfilterkell listaja
        /// </summary>
        public ArrayList SorRowFilterKell = new ArrayList();
        /// <summary>
        /// sor rekordkell litaja
        /// </summary>
        public ArrayList SorRekordKell = new ArrayList();
        /// <summary>
        /// oszlop tablainformaciok listaja
        /// </summary>
        public ArrayList OszlopTablainfok = new ArrayList();
        ArrayList egyresz = new ArrayList();
        /// <summary>
        /// uj feltetelinfo hozzaadas
        /// </summary>
        /// <param name="info">
        /// feltetelinfo
        /// </param>
        public void Add(Feltetelinfo info)
        {
            Add(info, false);
        }
        /// <summary>
        /// kereses index alapjan
        /// </summary>
        /// <param name="ind">
        /// index
        /// </param>
        /// <returns>
        /// a feltetelinfo
        /// </returns>
        public new Feltetelinfo this[int ind]
        {
            get { return (Feltetelinfo)base[ind]; }
        }
        /// <summary>
        /// feltetelinfo hozzaadas, teljes vagy csak az ArrayList Add-ja
        /// </summary>
        /// <param name="info">
        /// a feltetelinfo
        /// </param>
        /// <param name="csakbase">
        /// true: csak az ArrayList Add
        /// false: teljes
        /// </param>
        public void Add(Feltetelinfo info, bool csakbase)
        {
            object[] zarojelek = null;
            if (csakbase)
                base.Add(info);
            else
            {
                Feltetelinfo elozoinfo = null;
                Tablainfo elozotabinfo = null;
                if (this.Count != 0)
                {
                    elozoinfo = this[this.Count - 1];
                    elozotabinfo = elozoinfo.Tabinfo;
                }
                info.ElozoFeltetelInfo = elozoinfo;
                if (elozoinfo != null)
                    info.ElozoFeltetelInfo.KovetkezoFeltetelInfo = info;
                Tablainfo tabinfo = info.Tabinfo;
                Tablainfok.Add(info.Tabinfo);
                int i = base.Add(info);
                if (info.NyitoZarojel != "")
                {
                    nyitozarojelek.Add(i);
                    zarojelek = new object[2];
                    zarojelek[0] = i;
                    zarojelpoziciok.Add(zarojelek);
                }
                if (info.ZaroZarojel != "")
                {
                    zarozarojelek.Add(i);
                    int minusz = 0;
                    bool kellmeg = true;
                    do
                    {
                        minusz++;
                        zarojelek = (object[])zarojelpoziciok[zarojelpoziciok.Count - minusz];
                        if (zarojelek[1] == null)
                        {
                            zarojelek[1] = i;
                            zarojelpoziciok[zarojelpoziciok.Count - minusz] = zarojelek;
                            kellmeg = false;
                        }
                    } while (kellmeg);
                }
                if (info.EsVagy != "")
                {
                    if (info.EsVagy == "VAGY")
                        vagypoziciok.Add(i);
                    else
                        espoziciok.Add(i);
                }
                else
                    urespoziciok.Add(i);
                info.Feltetelinfok = this;
                if (tabinfo != null)
                {
                    switch (tipus)
                    {
                        case "feltetel":
                            tabinfo.Feltetelek.Add(info, true);
                            break;
                        case "sor":
                            tabinfo.SorFeltetelek.Add(info, true);
                            break;
                        case "oszlop":
                            tabinfo.OszlopFeltetelek.Add(info, true);
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// Feltetelek rendezese, Tabinfoktolt() hivas
        /// </summary>
        /// <param name="Listainfo">
        /// a listainformacio
        /// </param>
        public void Rendezes(ListaInfok Listainfo)
        {
            ListaInfok = Listainfo;
            FeltRendezes(Listainfo);
            Tabinfoktolt();
        }
        /// <summary>
        /// feltetelek rendezese
        /// </summary>
        /// <param name="ListaInfo">
        /// listainformacio
        /// </param>
        public void FeltRendezes(ListaInfok ListaInfo)
        {
            ArrayList sorrekordkell = SorRekordKell;
            ArrayList sorfilterkell = SorRowFilterKell;
            ArrayList egyresz = new ArrayList();
            if (tipus == "feltetel" && urespoziciok.Count > 1)
                urespoziciok.RemoveAt(0);
            if (this.Count != 0)
            {
                do
                {
                    ujkezdopoz = kezdopoz;
                    egyresz = new ArrayList();
                    bool ujra = true;
                    do
                    {
                        espoz = -1;
                        nextespoz = -1;
                        vagypoz = -1;
                        nextvagypoz = -1;
                        secondnyito = -1;
                        nyitozpoz = -1;
                        zarozpoz = -1;
                        urespoz = -1;
                        nexturespoz = -1;
                        for (int i = 0; i < urespoziciok.Count; i++)
                        {
                            int egyurespoz = Convert.ToInt16(urespoziciok[i].ToString());
                            if (egyurespoz >= ujkezdopoz)
                            {
                                if (urespoz == -1)
                                    urespoz = egyurespoz;
                                else
                                {
                                    nexturespoz = egyurespoz;
                                    break;
                                }
                            }
                        }
                        for (int i = 0; i < nyitozarojelek.Count; i++)
                        {
                            int nyito = Convert.ToInt16(nyitozarojelek[i].ToString());
                            if (nyito >= ujkezdopoz)
                            {
                                if (nyitozpoz == -1)
                                {
                                    nyitozpoz = nyito;
                                    for (int j = 0; j < zarojelpoziciok.Count; j++)
                                    {
                                        object[] egyzaro = (object[])zarojelpoziciok[j];
                                        nyito = Convert.ToInt16(egyzaro[0].ToString());
                                        if (nyito == nyitozpoz)
                                        {
                                            zarozpoz = Convert.ToInt16(egyzaro[1].ToString());
                                            break;
                                        }
                                    }
                                }
                                else if (nyito > nyitozpoz)
                                {
                                    secondnyito = nyito;
                                    break;
                                }
                            }
                        }
                        for (int i = 0; i < vagypoziciok.Count; i++)
                        {
                            int egyvagypoz = Convert.ToInt16(vagypoziciok[i].ToString());
                            if (egyvagypoz >= ujkezdopoz)
                            {
                                if (vagypoz == -1)
                                    vagypoz = egyvagypoz;
                                else if (egyvagypoz > vagypoz)
                                {
                                    nextvagypoz = egyvagypoz;
                                    break;
                                }
                            }
                        }
                        for (int i = 0; i < espoziciok.Count; i++)
                        {
                            int egyespoz = Convert.ToInt16(espoziciok[i].ToString());
                            if (egyespoz >= ujkezdopoz)
                            {
                                if (espoz == -1 && egyespoz < vagypoz)
                                    espoz = egyespoz;
                                else if (egyespoz > vagypoz && nextespoz == -1)
                                {
                                    nextespoz = egyespoz;
                                    break;
                                }
                            }
                        }
                        if (vagypoziciok.Count == 0 || tipus == "sor")
                        {
                            vegpoz = urespoz;
                            ujra = false;
                        }
                        if (ujra)
                        {
                            if (nyitozarojelek.Count == 0 || ujkezdopoz > Convert.ToInt16(nyitozarojelek[nyitozarojelek.Count - 1].ToString()))
                            {
                                if (this[ujkezdopoz].KovetkezoFeltetelInfo != null && this[ujkezdopoz].KovetkezoFeltetelInfo.Tabinfo == this[ujkezdopoz].Tabinfo)
                                {
                                    ujkezdopoz++;
                                }
                                else
                                {
                                    vegpoz = ujkezdopoz;
                                    ujra = false;
                                }
                            }
                            else
                            {
                                if (nyitozpoz > espoz && vagypoz > espoz && nyitozpoz != -1 && vagypoz >= nyitozpoz && (secondnyito > zarozpoz || secondnyito == -1))
                                {
                                    ujkezdopoz = zarozpoz;
                                    if (nextespoz == ujkezdopoz)
                                        ujkezdopoz++;
                                    if (ujkezdopoz >= this.Count - 1)
                                    {
                                        ujra = false;
                                        vegpoz = this.Count - 1;
                                    }
                                }
                                else if (vagypoz != -1 && vagypoz <= nyitozpoz)
                                {
                                    if (this[vagypoz + 1].Tabinfo != this[vagypoz].Tabinfo)
                                    {
                                        ujra = false;
                                        vegpoz = vagypoz;
                                    }
                                    else
                                        ujkezdopoz++;
                                }
                                else if (nextvagypoz != -1 && nextvagypoz < zarozpoz)
                                {
                                    if (secondnyito == nextvagypoz)
                                        vegpoz = nextvagypoz - 1;
                                    else
                                        vegpoz = nextvagypoz;
                                    ujra = false;
                                }
                            }
                        }
                    } while (ujra);
                    object[] selinf = new object[Tablainfok.Count];
                    for (int i = 0; i < selinf.Length; i++)
                        selinf[i] = new ArrayList();
                    bool[] kellfilter = new bool[vegpoz - kezdopoz + 1];
                    bool[] kellrekord = new bool[kellfilter.Length];
                    for (int i = kezdopoz; i <= vegpoz; i++)
                    {
                        Feltetelinfo egyfelt = this[i];
                        tabinfo = egyfelt.Tabinfo;
                        int tabindex;
                        if (tabinfo != null)
                            tabindex = Tablainfok.IndexOf(tabinfo);
                        else
                            tabindex = Tablainfok.Count - 1;
                        ArrayList ar = (ArrayList)selinf[tabindex];
                        ar.Add(egyfelt);
                        if (tipus == "sor")
                        {
                            int filtindex = i - kezdopoz;
                            Feltetelinfo elozofelt = null;
                            Feltetelinfo kovfelt = null;
                            if (i != kezdopoz)
                                elozofelt = egyfelt.ElozoFeltetelInfo;
                            if (i != vegpoz)
                                kovfelt = egyfelt.KovetkezoFeltetelInfo;
                            if (elozofelt != null && elozofelt.Tabinfo == tabinfo && elozofelt.EsVagy == "VAGY")
                            {
                                kellfilter[filtindex] = kellfilter[filtindex - 1];
                                kellrekord[filtindex] = kellrekord[filtindex - 1];
                            }
                            else if (vagypoz == i)
                            {
                                if (nyitozpoz == i)
                                {
                                    kellfilter[filtindex] = true;
                                    kellrekord[filtindex] = false;
                                }
                                else
                                {
                                    if (kovfelt.Tabinfo == tabinfo)
                                    {
                                        kellfilter[filtindex] = true;
                                        kellrekord[filtindex] = false;
                                        ar.Add(kovfelt);
                                        i++;
                                    }
                                }
                            }
                            else // essel kezdodik
                            {
                                kellfilter[filtindex] = true;
                                kellrekord[filtindex] = true;
                            }
                        }
                        egyresz.Add(egyfelt);
                    }
                    if (tipus == "sor")
                    {
                        SorRowFilterKell.Add(kellfilter);
                        SorRekordKell.Add(kellrekord);
                    }
                    for (int i = 0; i < Tablainfok.Count; i++)
                    {
                        tabinfo = Tablainfok[i];
                        switch (tipus)
                        {
                            case "feltetel":
                                tabinfo.SelectElemek.Add(selinf[i]);
                                break;
                            case "oszlop":
                                tabinfo.OszlopSelectElemek.Add(selinf[i]);
                                break;
                            case "sor":
                                tabinfo.SorSelectElemek.Add(selinf[i]);
                                break;
                        }
                    }
                    Reszelemek.Add(egyresz);
                    kezdopoz = vegpoz + 1;
                } while (kezdopoz < this.Count && this[kezdopoz].Tabinfo != null);
                ArrayList tabar = null;
                int db = -1;
                if (tipus == "sor")
                {
                    SorokSzama = Reszelemek.Count;
                    db = SorokSzama;
                    tabar=SorTablainfok;
                }
                if (tipus == "oszlop")
                {
                    OszlopokSzama = Reszelemek.Count;
                    db = OszlopokSzama;
                    tabar = OszlopTablainfok;
                }
                if (tabar != null)
                {
                    for (int i = 0; i < db; i++)
                    {
                        ArrayList egyar = new ArrayList();
                        egyresz = (ArrayList)Reszelemek[i];
                        for (int j = 0; j < egyresz.Count; j++)
                        {
                            Feltetelinfo egyfelt = (Feltetelinfo)egyresz[j];
                            egyar.Add(egyfelt.Tabinfo);
                        }
                        tabar.Add((Tablainfo[])egyar.ToArray(typeof(Tablainfo)));

                    }
                }

            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void Tabinfoktolt()
        {
            bool[] kellselect = new bool[Tablainfok.Count];
            for (int i = 0; i < kellselect.Length; i++)
                kellselect[i] = true;
            for (int i = 0; i < Tablainfok.Count; i++)
            {
                tabinfo = Tablainfok[i];
                for (int j = 0; j < tabinfo.SelectElemek.Count; j++)
                {
                    ArrayList ar = (ArrayList)tabinfo.SelectElemek[j];
                    if (ar.Count == 0)
                        kellselect[i] = false;
                }
            }
            ArrayList filtar = new ArrayList();
            string egyfilt = "";
            for (int i = 0; i < Tablainfok.Count; i++)
            {
                tabinfo = Tablainfok[i];
                FeltetelinfoCollection feltetelek = tabinfo.Feltetelek;
                int lastuser = -1;
                for (int j = 0; j < feltetelek.Count; j++)
                {
                    if (!feltetelek[j].UserFeltetel)
                        break;
                    lastuser = j;
                }
                bool voltnyito = false;
                for (int j = 0; j < tabinfo.SelectElemek.Count; j++)
                {
                    ArrayList ar = (ArrayList)tabinfo.SelectElemek[j];
                    filtar = tabinfo.RowFilterek;
                    filtar.Add("");
                    egyfilt = "";
                    for (int k = 0; k < ar.Count; k++)
                    {
                        Feltetelinfo egyfelt = (Feltetelinfo)ar[k];
                        string egysel = egyfelt.FeltetelSorSelect;
                        string esvagy = egyfelt.EsVagy;
                        if (k < ar.Count - 1)
                        {
                            if (esvagy == "VAGY")
                                esvagy = " OR ";
                            else
                                esvagy = " AND ";
                        }
                        else
                            esvagy = "";
                        egysel += esvagy;
                        if (kellselect[i] || tabinfo == ListaInfok.ElsoTabinfo)
                        {
                            string selstring = tabinfo.TablainfoSelect;
                            if (k == 0 && selstring != "")
                            {
                                selstring += " OR ";
                            }
                            selstring += egysel;
                            if (!voltnyito && selstring != "" && lastuser == k && k != ar.Count - 1)
                            {
                                selstring += "(";
                                voltnyito = true;
                            }

                            tabinfo.TablainfoSelect = selstring;
                        }
                        egyfilt += egysel;
                        filtar[filtar.Count - 1] = egyfilt;
                    }
                }
                if (voltnyito)
                    tabinfo.TablainfoSelect += ")";
                    
            }
        }
    }
    /// <summary>
    /// Feltetelinformaciok egy feltetelsor parameterezes alapjan
    /// </summary>
    public class Feltetelinfo
    {
        /// <summary>
        /// tablainformacio
        /// </summary>
        public Tablainfo Tabinfo;
        /// <summary>
        /// column informacio
        /// </summary>
        public Cols ColumnInfo = null;
        /// <summary>
        /// az elozo feltetelinfo vagy null
        /// </summary>
        public Feltetelinfo ElozoFeltetelInfo = null;
        /// <summary>
        /// a kovetkezo feltetelinfo vagy null
        /// </summary>
        public Feltetelinfo KovetkezoFeltetelInfo = null;
        /// <summary>
        /// az elozo zarojeles feltinfo vagy null
        /// </summary>
        public ArrayList ElozoZarojelesFeltetelInfok = new ArrayList();
        /// <summary>
        /// a kovetkezo zarojeles feltinfo vagy null
        /// </summary>
        public ArrayList KovetkezoZarojelesFeltetelInfok = new ArrayList();
        /// <summary>
        /// nyitozarojel
        /// </summary>
        public string NyitoZarojel = "";
        /// <summary>
        /// relacio
        /// </summary>
        public string Relacio = "";
        /// <summary>
        /// ertek
        /// </summary>
        public string Ertek = "";
        /// <summary>
        /// zarozarojel
        /// </summary>
        public string ZaroZarojel = "";
        /// <summary>
        /// es/vagy vagy ures
        /// </summary>
        public string EsVagy = "";
        /// <summary>
        /// Sorselect
        /// </summary>
        public string FeltetelSorSelect = "";
        /// <summary>
        /// sorlista
        /// </summary>
        public string FeltetelSorLista = "";
        /// <summary>
        /// a listainfok
        /// </summary>
        public ListaInfok ListaInfok;
        /// <summary>
        /// Userfeltetel?
        /// </summary>
        public bool UserFeltetel = false;
        /// <summary>
        /// az osszes feltetelinfo
        /// </summary>
        public FeltetelinfoCollection Feltetelinfok = null;
        /// <summary>
        /// objectum letrehozasa
        /// </summary>
        /// <param name="listainfok">
        /// listainfok
        /// </param>
        /// <param name="fakuserinterface">
        /// fakuserinterface
        /// </param>
        /// <param name="view">
        /// a feltetelmeghatarozas view-ja
        /// </param>
        /// <param name="viewindex">
        /// sorindex a view-ban
        /// </param>
        public Feltetelinfo(ListaInfok listainfok, FakUserInterface fakuserinterface, DataView view, int viewindex)
        {
            ListaInfok = listainfok;
            Feltetelinfok = ListaInfok.FeltColl;
            DataRow dr = view[viewindex].Row;
            Tablainfo tabinfo = fakuserinterface.GetByAzontip(dr["AZONTIP"].ToString());
            Tabinfo = tabinfo;
            Cols mezocol = null;
            bool csakuressor = dr["MEZONEV"].ToString() == "";
            ZaroZarojel = dr["ZAROZAROJEL"].ToString();
            if (csakuressor)
                FeltetelSorLista = ZaroZarojel;
            else
            {
                mezocol = tabinfo.TablaColumns[dr["MEZONEV"].ToString()];
                ColumnInfo = mezocol;
                NyitoZarojel = dr["NYITOZAROJEL"].ToString();
                Relacio = dr["RELACIO"].ToString();
                Ertek = dr["MASODIKELEM"].ToString();
                EsVagy = dr["ESVAGY"].ToString();
                FeltetelSorLista = NyitoZarojel;
                bool stringe = mezocol.DataType.ToString() == "System.String";
                bool datume = mezocol.DataType.ToString() == "System.DateTime";
                if (mezocol.Comboe)
                {
                    FeltetelSorSelect = mezocol.ColumnName + Relacio;
                    if (stringe)
                        FeltetelSorSelect += "'";
                    FeltetelSorSelect += mezocol.Combo_Info.GetComboAktfileba(Ertek);
                    if (stringe)
                        FeltetelSorSelect += "'";
                }
                else if (stringe)
                {
                    if (Relacio == "=")
                    {
                        FeltetelSorSelect = mezocol.ColumnName + "like '%"+ Ertek + "%'";
                    }
                    else
                        FeltetelSorSelect = "substring(" + mezocol.ColumnName + ",1," + Ertek.Length + ")" + Relacio + "'" + Ertek + "'";
                }
                else if (datume)
                    FeltetelSorSelect = mezocol.ColumnName + Relacio + "'" + Ertek + "'";
                else
                    FeltetelSorSelect = mezocol.ColumnName + Relacio + Ertek;
                FeltetelSorLista += dr["ELSOELEM"].ToString();
 //               FeltetelSorSelect += Relacio;
                FeltetelSorLista += Relacio;
                //if (!mezocol.Comboe)
                //{
                //    if (mezocol.DataType.ToString() == "System.String" || mezocol.DataType.ToString() == "System.DateTime")
                //        FeltetelSorSelect += "'" + Ertek + "'";
                //    else
                //        FeltetelSorSelect += Ertek;
                //}
                //else
                //    FeltetelSorSelect += mezocol.Combo_Info.GetComboAktfileba(Ertek);
                if (viewindex < ListaInfok.Hivo.userselectcount && view.Table.TableName=="FELTETEL")
                    UserFeltetel = true;
                FeltetelSorLista += dr["MASODIKELEM"].ToString();
                FeltetelSorLista += ZaroZarojel;
            }
        }
    }
}
