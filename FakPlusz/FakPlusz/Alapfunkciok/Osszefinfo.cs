using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Globalization;
using System.Data;
using System.Threading;
using FakPlusz;
namespace FakPlusz.Alapfunkciok
{
    /// <summary>
    /// Az osszefuggesinformaciok tombje
    /// </summary>
    public class OsszefinfoCollection:ArrayList
    {
        private bool csakbase = false;
        /// <summary>
        /// a kereseshez a teljes azonositok tombje
        /// </summary>
        private ArrayList azontipek = new ArrayList();
        /// <summary>
        /// a kereseshez az 1.elem azonositok tombje
        /// </summary>
        private ArrayList azontip1ek = new ArrayList();
        /// <summary>
        /// a kereseshez a 2.elem azonositok tombje
        /// </summary>
        private ArrayList azontip2ok = new ArrayList();
        /// <summary>
        /// Ures objectum letrehozasa
        /// </summary>
        public OsszefinfoCollection()
        {
        }
        /// <summary>
        /// index megallapitas tablainformacio alapjan
        /// </summary>
        /// <param name="info">
        /// a kivant tablainfo
        /// </param>
        /// <returns>
        /// a kivant index vagy -1
        /// </returns>
        public int IndexOf(Tablainfo info)
        {
            return azontipek.IndexOf(info);
        }
        /// <summary>
        /// kereses index alapjan
        /// </summary>
        /// <param name="index">
        /// kivant index
        /// </param>
        /// <returns>
        /// Osszefinformacio vagy null
        /// </returns>
        public new Osszefinfo this[int index]
        {
            get
            {
                if (index < 0 || index > this.Count - 1)
                    return null;
                else
                    return (Osszefinfo)base[index];
            }
            set { base[index] = value; }
        }
        /// <summary>
        /// kereses teljes azonosito alapjan
        /// </summary>
        /// <param name="azontip">
        /// az azonosito
        /// </param>
        /// <returns>
        /// osszefuggesek tombje a megadott azonosito alapjan vagy ures osszefuggestomb
        /// </returns>
        public OsszefinfoCollection this[string azontip]
        {
            get
            {
                OsszefinfoCollection coll = new OsszefinfoCollection();
                csakbase=true;
                for (int i = 0; i < azontip1ek.Count; i++)
                {
                    string tip1 = azontip1ek[i].ToString();
                    string tip2 = azontip2ok[i].ToString();
                    if (azontip == tip1 || azontip == tip2)
                        coll.Add(this[i]);
                }
                csakbase = false;
                return coll;
            }
        }
        /// <summary>
        /// kereses teljes azonosito, 1.azonosito es 2. azonosito alapjan
        /// </summary>
        /// <param name="azontip">
        /// </param>
        /// <param name="azontip1"></param>
        /// <param name="azontip2"></param>
        /// <returns>
        /// a megtalalt osszefuggesinformacio vagy null
        /// </returns>
        public Osszefinfo this[string azontip, string azontip1,string azontip2]
        {
            get
            {
                int i = azontipek.IndexOf(azontip);
                if (i == -1)
                    return null;
                else if (azontip1ek[i].ToString() == azontip1 && azontip2ok[i].ToString() == azontip2)
                    return (Osszefinfo)base[i];
                else
                    return null;
            }
        }
        /// <summary>
        /// Uj osszefinformacio hozzaadas
        /// </summary>
        /// <param name="value">
        /// Uj osszefinformacio
        /// </param>
        /// <returns>
        /// Uj informacio indexe
        /// </returns>
        public override int Add(object value)
        {
            if (csakbase)
                return base.Add(value);
            Osszefinfo osszefinfo = (Osszefinfo)value;
            Tablainfo tabinfo = osszefinfo.tabinfo;
            int i = azontipek.IndexOf(tabinfo.Azontip);
            bool j = false;
            bool k = false;
            if (i != -1)
            {
                j = azontip1ek[i].ToString() == tabinfo.Azontip1;
                k = azontip2ok[i].ToString() == tabinfo.Azontip2;
            }
            if (i == -1 || !j || !k)
            {
                i = base.Add(value);
                azontipek.Add(((Osszefinfo)value).tabinfo.Azontip);
                azontip1ek.Add(((Osszefinfo)value).azontip1);
                azontip2ok.Add(((Osszefinfo)value).azontip2);
            }
            return i;
        }
        /// <summary>
        /// osszefuggesinformacio torlese
        /// </summary>
        /// <param name="obj"></param>
        public override void Remove(object obj)
        {
            if (obj != null)
            {
                Osszefinfo osszefinfo = (Osszefinfo)obj;
                Tablainfo tabinfo = osszefinfo.tabinfo;
                int i = azontipek.IndexOf(tabinfo.Azontip);
                bool j = false;
                bool k = false;
                if (i != -1)
                {
                    j = azontip1ek[i].ToString() == tabinfo.Azontip1;
                    k = azontip2ok[i].ToString() == tabinfo.Azontip2;
                }
                if (i != -1 && j && k)
                {
                    base.RemoveAt(i);
                    azontipek.RemoveAt(i);
                    azontip1ek.RemoveAt(i);
                    azontip2ok.RemoveAt(i);
                }
            }
        }
    }
    /// <summary>
    /// Osszefuggesjellegu tablainformaciok osszefuggesinformacioi (Osszefugges,csoport,szukitett kodtabla)
    /// </summary>
    public class Osszefinfo
    {
        private TablainfoTag tag;
        /// <summary>
        /// A TablainfoTag
        /// </summary>
        public TablainfoTag AktualTag
        {
            set { tag = value; }
        }
        /// <summary>
        /// Eredeti tablainformacio
        /// </summary>
        public Tablainfo tabinfo;
        /// <summary>
        /// eredeti verzioid
        /// </summary>
        private string eredetiaktverid = "";
        /// <summary>
        /// elso elem tablainformacioja
        /// </summary>
        public Tablainfo tabinfo1;
        /// <summary>
        /// masodik elem tablainformacioja
        /// </summary>
        public Tablainfo tabinfo2;
        /// <summary>
        /// elso elem teljes azonositoja
        /// </summary>
        public string azontip1;
        /// <summary>
        /// masodik elem telkes azonositoja
        /// </summary>
        public string azontip2;
        private string keresoszint1 = "";
        private string keresotablanev1 ="";
        private string keresoszint2 = "";
        private string keresotablanev2 = "";
        /// <summary>
        /// elso elem aktualis verzioid
        /// </summary>
        public string aktverid1 = "";
        private string eredetiaktverid1 = "";
        /// <summary>
        /// masodik elem aktualis verzioid
        /// </summary>
        public string aktverid2 = "";
        private string eredetiaktverid2 = "";
        /// <summary>
        /// elso elem KOD indexe
        /// </summary>
        public int kod1col = -1;
        /// <summary>
        /// masodik elem KOD indexe
        /// </summary>
        public int kod2col = -1;
        public int maxszovlength = -1;
        /// <summary>
        /// eredeti tablainfo SZOVEG indexe
        /// </summary>
        public int szovcol = -1;
        /// <summary>
        /// elso elem SZOVEG indexe
        /// </summary>
        public int szoveg1col;
        /// <summary>
        /// masodik elem SZOVEG indexe
        /// </summary>
        public int szoveg2col;
        /// <summary>
        /// eredeti tablainfo identity mezo indexe
        /// </summary>
        public int identcol;
        /// <summary>
        /// elso elem identity mezo indexe
        /// </summary>
        public int ident1col;
        /// <summary>
        /// elso elem identity oszlop neve
        /// </summary>
        public string ident1colname;
        /// <summary>
        /// masodik elem identity mezo indexe
        /// </summary>
        public int ident2col;
        /// <summary>
        /// masodik elem identity oszlop neve
        /// </summary>
        public string ident2colname;
        /// <summary>
        /// elso elem PREV_ID indexe
        /// </summary>
        public int previd1col;
        /// <summary>
        /// masodik elem PREV_ID indexe
        /// </summary>
        public int previd2col;
        /// <summary>
        /// elso elem SORSZAM indexe
        /// </summary>
        public int sorszam1col;
        /// <summary>
        /// masodik elem SORSZAM indexe
        /// </summary>
        public int sorszam2col;
        /// <summary>
        /// 
        /// </summary>
        public int alkalmid1col;
        /// <summary>
        /// 
        /// </summary>
        public int alkalmid2col;
        /// <summary>
        /// forditott TablainfoTag?
        /// </summary>
        public bool forditott = false;
        /// <summary>
        /// Inputtabla
        /// </summary>
        public AdatTabla Inputtabla = new AdatTabla("INPTABLE");
        /// <summary>
        /// eredeti DataView
        /// </summary>
        public DataView DataView = null;
        /// <summary>
        /// elso elem DataView
        /// </summary>
        public DataView DataView1 = null;
        /// <summary>
        /// masodik elem DataView
        /// </summary>
        public DataView DataView2 = null;
        private DataGridView datagridview1;
        private DataGridView datagridview2;
        /// <summary>
        /// elso elem adattabla VALASZT mezo indexe
        /// </summary>
        public int valasztind = -1;
        /// <summary>
        /// Tarolando Combo infok
        /// </summary>
        public ArrayList ComboFileInfo = null;
        /// <summary>
        /// Megjelenitendo Combo infok
        /// </summary>
        public ArrayList ComboSzovInfo = null;
        /// <summary>
        /// DataGridView a megjeleniteshez, ha nincs set,null, set eseten allitja a szukseges property-ket
        /// </summary>
        public DataGridView DataGridView1
        {
            get { return datagridview1; }
            set
            {
                DataGridView gw = value;
                datagridview1 = gw;
                Cols egycol;
                string adatfajta = tabinfo.Adatfajta;
                forditott = tag.Forditott;
                Tablainfo info;
                if (forditott)
                    info = tabinfo2;
                else
                    info = tabinfo1;
                AdatTabla adattabla = info.Adattabla;
                AktualDataView1 = new DataView(adattabla);
                AktualDataView1.Sort = info.DataView.Sort;
                AktualDataView2 = new DataView(Adattabla2);
                AktualDataView2.Sort = tabinfo2.DataView.Sort;
                int kodcol = info.Kodcol;
                int szovegcol = info.Szovegcol;
                if (info.Tablanev == "LEIRO")
                    szovegcol = info.Adattabla.Columns.IndexOf("SORSZOV");
                gw.AutoGenerateColumns = false;
                gw.ColumnHeadersVisible = true;
                gw.Columns.Clear();
                gw.ReadOnly = true;
                if ("CS".Contains(adatfajta ))
                {
                    gw.ReadOnly = false;
                    gw.SelectionMode = DataGridViewSelectionMode.CellSelect;
                }
                //if (kodcol != -1)
                //{
                //    egycol = info.TablaColumns[kodcol];
                //    gw.Columns.Add(info.Ujtextcolumn(egycol, true));
                //}
                if (szovegcol != -1)
                {
                    if (info.Adatfajta=="" || !"CO".Contains(info.Adatfajta))
                    {
                        egycol = info.TablaColumns[szovegcol];
                        gw.Columns.Add(info.Ujtextcolumn(egycol, true));
                    }
                    else
                    {
                        gw.Columns.Add(info.Ujtextcolumn("SZOVEG","Megnevezés",true));
                        gw.Columns[0].Width = maxszovlength * 9;
                    }
                }
                else
                {
                    string propname = info.TablaColumns[info.Azonositocol].ColumnName;
                    gw.Columns.Add(info.Ujtextcolumn(propname, "Megnevezés", true));
                }
                gw.Columns[0].MinimumWidth = gw.Columns[0].HeaderText.Length * 9;
                if (adatfajta == "C" && !forditott)
                {
                    valasztind = tabinfo1.Adattabla.Columns.IndexOf("VALASZT");
                    if (valasztind != -1)
                        tabinfo1.Adattabla.Columns.RemoveAt(valasztind);
                    DataColumn col = new DataColumn("VALASZT", Type.GetType("System.String"));
                    tabinfo1.Adattabla.Columns.Add(col);
                    valasztind = tabinfo1.Adattabla.Columns.IndexOf(col);
                    DataGridViewComboBoxColumn ccol = new DataGridViewComboBoxColumn();
                    ccol.DataPropertyName = "VALASZT";
                    ccol.HeaderText = tabinfo2.Szoveg;
                    ccol.MinimumWidth = ccol.HeaderText.Length * 9;
                    if (tabinfo2.Tablanev == "KEZELOK" && Fak.Alkalmazas!="TERVEZO")
                    {
                        Osszefinfo kezalkalm = Fak.GetOsszef("U", "KezeloAlkalm").Osszefinfo;
                        string[] idk = kezalkalm.GetSzurtOsszefIdk(new object[] { "", Fak.AlkalmazasId });
                        if (idk != null)
                        {
                            tabinfo2.DataView.RowFilter = "";
                            string filt = "";
                            for (int i = 0; i < idk.Length; i++)
                            {
                                if (filt != "")
                                    filt += " OR ";
                                filt += "KEZELO_ID = " + idk[i];
                            }
                            tabinfo2.DataView.RowFilter = filt;
                        }

                    }

                    Comboinfok cinfo = Fak.ComboInfok.ComboinfoKeres(tabinfo2.Azontip);
                    ccol.Width = cinfo.Maxhossz * 9;
                    ccol.MinimumWidth = cinfo.Minhossz * 9;
                    ComboFileInfo = new ArrayList();
                    ComboFileInfo.Add("0");
                    ComboSzovInfo=new ArrayList();
                    ComboSzovInfo.Add("");
                    string id = "";
                    for(int i=0;i<cinfo.ComboId.Count;i++)
                    {
                        for (int j = 0; j < tabinfo2.DataView.Count; j++)
                        {
                            if (tabinfo2.Tablanev == "KEZELOK")
                                id = tabinfo2.DataView[j].Row["KEZELO_ID"].ToString();
                            else
                                id = cinfo.ComboId[i].ToString();
                            if (id == cinfo.ComboId[i].ToString())
                            {
                                ComboFileInfo.Add(cinfo.ComboId[i]);
                                ComboSzovInfo.Add(cinfo.ComboInfo[i]);
                                break;
                            }
                        }
                    }
                    if (tabinfo2.Tablanev == "KEZELOK")
                        tabinfo2.DataView.RowFilter = "";
                    ccol.DataSource = ComboSzovInfo;
                    gw.Columns.Add(ccol);
                }
                else if (adatfajta == "S")
                {
                    valasztind = tabinfo1.Adattabla.Columns.IndexOf("Kell/Nem kell");
                    if (valasztind != -1)
                        tabinfo1.Adattabla.Columns.RemoveAt(valasztind);
                    DataColumn col = new DataColumn("Kell/Nem kell", Type.GetType("System.Int32"));
                    tabinfo1.Adattabla.Columns.Add(col);
                    valasztind = tabinfo1.Adattabla.Columns.IndexOf(col);
                    DataGridViewCheckBoxColumn box2 = new DataGridViewCheckBoxColumn();
                    box2.DataPropertyName = "Kell/Nem kell";
                    box2.Name = "Kell/Nem kell";
                    box2.ReadOnly = false;
                    box2.FalseValue = 0;
                    box2.TrueValue = 1;
                    gw.Columns.Add(box2);
                }
                gw.DataSource = AktualDataView1;
            }
        
        }
        /// <summary>
        /// DataGridView az adatbevitelhez, ha nincs set,null, set eseten eloallitja a szukseges property-ket
        /// </summary>
        public DataGridView DataGridView2
        {
            get { return datagridview2; }
            set
            {
                Cols egycol;
                DataGridView gw = value;
                datagridview2 = gw;
                string adatfajta = tabinfo.Adatfajta;
                forditott = tag.Forditott;
                Tablainfo info;
                if (forditott)
                    info = tabinfo1;
                else
                    info = tabinfo2;
                AdatTabla adattabla = info.Adattabla;
                AktualDataView2 = new DataView(adattabla);
                AktualDataView2.Sort = info.DataView.Sort;
                gw.ColumnHeadersVisible = true;
                gw.AutoGenerateColumns = false;
                gw.Columns.Clear();
                if (adatfajta == "O" || adatfajta == "C" && forditott)
                {
                    Inputtabla.Columns.Clear();
                    Inputtabla.Columns.Add("SZOVEG", Type.GetType("System.String"));
                    Inputtabla.Columns.Add("Kell/Nem kell", Type.GetType("System.Int64"));
                    Inputtabla.Columns.Add("Ident", Type.GetType("System.Int64"));
                    Inputtabla.Columns.Add("PREV_ID", Type.GetType("System.Int64"));
                    string propname = "Megnevezés";
                    int szovegcol = info.Szovegcol;
                    if (szovegcol != -1)
                    {
                        if (info.Adatfajta=="" || !"CO".Contains(info.Adatfajta))
                        {
                            egycol = info.TablaColumns[szovegcol];
                            gw.Columns.Add(info.Ujtextcolumn(egycol, true));
                        }
                        else
                        {
                            gw.Columns.Add(info.Ujtextcolumn("SZOVEG","Megnevezés",true));
                        }
                    }
                    else
                    {
                        propname = info.TablaColumns[info.Azonositocol].ColumnName;
                        Inputtabla.Columns[0].ColumnName = propname;
                        gw.Columns.Add(info.Ujtextcolumn(propname, "Megnevezés", true));
                    }
                    if (adatfajta != "C")
                    {
                        DataGridViewCheckBoxColumn box2 = new DataGridViewCheckBoxColumn();
                        box2.DataPropertyName = "Kell/Nem kell";
                        box2.Name = "Kell/Nem kell";
                        box2.ReadOnly = false;
                        box2.FalseValue = 0;
                        box2.TrueValue = 1;
                        gw.Columns.Add(box2);
                    }
                    gw.Dock = DockStyle.Fill;
                    gw.DataSource = Inputtabla;
                }
            }
        }
        /// <summary>
        /// eredeti adattabla
        /// </summary>
        public AdatTabla Adattabla;
        /// <summary>
        /// elso elem adattabla
        /// </summary>
        public AdatTabla Adattabla1;
        /// <summary>
        /// masodik elem adattabla
        /// </summary>
        public AdatTabla Adattabla2;
        /// <summary>
        /// ha az elso elem maga is osszefugges jellegu annak az aosszefugges informacioja, egyebkent null
        /// </summary>
        public Osszefinfo Osszefinfo1 = null;
        /// <summary>
        /// ha az masodik elem maga is osszefugges jellegu annak az aosszefugges informacioja, egyebkent null
        /// </summary>
        public Osszefinfo Osszefinfo2 = null;
        /// <summary>
        /// megjelenitendo dataview
        /// </summary>
        public DataView AktualDataView1 = new DataView();
        /// <summary>
        /// adatbevitel dataview
        /// </summary>
        public DataView AktualDataView2 = new DataView();
        /// <summary>
        /// true, ha elso eset;
        /// </summary>
        public FakUserInterface Fak;
        /// <summary>
        /// kell tolteni?
        /// </summary>
        public bool TolteniKell = true;
        /// <summary>
        /// kell inicializalni?
        /// </summary>
        public bool InitKell = true;
        /// <summary>
        /// objectum letrehozasa
        /// </summary>
        /// <param name="tablainfo">
        /// tablainformacio
        /// </param>
        public Osszefinfo(Tablainfo tablainfo)
        {
            tabinfo = tablainfo;
            Fak = tablainfo.Fak;
            OsszefinfoInit();
            Fak.Osszefuggesek.Add(this);
        }
        /// <summary>
        /// inicializalas
        /// </summary>
        public void OsszefinfoInit()
        {
            if (!InitKell)
                return;
            InitKell = false;
            TolteniKell=true;
            string[] modositottak = Fak.GetTartal(tabinfo, "SORSZAM", "MODOSITOTT_M", "1");
            if(modositottak!=null)
                Fak.ForceAdattolt(tabinfo,true);
            string aktverid = tabinfo.AktVerzioId.ToString();
            eredetiaktverid = aktverid;
            string oldverid = aktverid;
            Adattabla = tabinfo.Adattabla;
            szovcol = Adattabla.Columns.IndexOf("SZOVEG");
            identcol = tabinfo.IdentityColumnIndex;
            if (szovcol == -1)
            {
                Adattabla.Columns.Add(new DataColumn("SZOVEG"));
                szovcol = Adattabla.Columns.IndexOf("SZOVEG");
            }
            DataView = Adattabla.DataView;
            forditott = tabinfo.TablaTag.Forditott;
            azontip1 = tabinfo.Azontip1;
            azontip2 = tabinfo.Azontip2;
            if (azontip1.Contains("SZRM"))
            {
                keresoszint1 = azontip1.Substring(4, 1);
                keresotablanev1 = azontip1.Substring(5);
            }
            if (azontip2.Contains("SZRM"))
            {
                keresoszint2 = azontip2.Substring(4, 1);
                keresotablanev2 = azontip2.Substring(5);
            }
            if (keresoszint1 == "")
                tabinfo1 = Fak.Tablainfok.GetByAzontip(azontip1);
            else
                tabinfo1 = Fak.Tablainfok.GetBySzintPluszTablanev(keresoszint1, keresotablanev1).LeiroTablainfo;
            if (tabinfo1 != null)
            {
                Adattabla1 = tabinfo1.Adattabla;
                DataView1 = Adattabla1.DataView;
                eredetiaktverid1 = tabinfo1.AktVerzioId.ToString();
                aktverid1 = eredetiaktverid1;
                if (keresoszint2 == "")
                    tabinfo2 = Fak.Tablainfok.GetByAzontip(azontip2);
                else
                {
                    tabinfo2 = Fak.Tablainfok.GetBySzintPluszTablanev(keresoszint2, keresotablanev2);
                    if (tabinfo2 != null)
                        tabinfo2 = tabinfo2.LeiroTablainfo;
                }
                if (tabinfo2 != null)
                {
                    Adattabla2 = tabinfo2.Adattabla;
                    DataView2 = Adattabla2.DataView;
                    eredetiaktverid2 = tabinfo2.AktVerzioId.ToString();
                    aktverid2 = eredetiaktverid2;
                }
                if (DataView.Count != 0)
                {
                    string col1n = "VERZIO_ID1";
                    string col2n = "VERZIO_ID2";
                    aktverid1 = DataView[0].Row[col1n].ToString();
                    if (tabinfo.Adatfajta != "S" && tabinfo2!=null)
                        if (tabinfo2.KellVerzio)
                            aktverid2 = DataView[0].Row[col2n].ToString();
                }
            }
        }
        /// <summary>
        /// Toltes
        /// </summary>
        /// <returns>
        /// true: hiba
        /// </returns>
        public bool Osszefinfotolt()
        {
            if (!TolteniKell)
                return true;
            TolteniKell = false;
            if (!Newversion() || tabinfo1==null)
                return false;
            if (tabinfo.KellVerzio)
                NewVersionKieg();
            else
                NewVersionKieg(Fak.VerzioInfok[tabinfo.Szint].LastVersionId);
            if (tabinfo1.DataView.Count == 0 || tabinfo.Adatfajta != "S" && tabinfo2.DataView.Count == 0)
                return false;
            maxszovlength = 0;
            ident1col = tabinfo1.IdentityColumnIndex;
            ident1colname = tabinfo1.IdentityColumnName;
            ident2col = tabinfo2.IdentityColumnIndex;
            ident2colname = tabinfo2.IdentityColumnName;
            sorszam1col = tabinfo.Sorszam1col;
            sorszam2col = tabinfo.Sorszam2col;
            previd1col = tabinfo1.PrevIdcol;
            previd2col = tabinfo2.PrevIdcol;
            alkalmid1col = tabinfo1.TablaColumns.IndexOf("ALKALMAZAS_ID");
            alkalmid2col = tabinfo2.TablaColumns.IndexOf("ALKALMAZAS_ID");
            string adatfajta1 = tabinfo1.Azon.Substring(3, 1);
            string adatfajta2 = tabinfo2.Azon.Substring(3, 1);
            DataColumn col = new DataColumn("SZOVEG", System.Type.GetType("System.String"));
            if (!"CO".Contains(adatfajta1) && !"CO".Contains(adatfajta2))
            {
                kod1col = tabinfo1.Kodcol;
                szoveg1col = tabinfo1.Szovegcol;
                if (szoveg1col == -1)
                    szoveg1col = tabinfo1.Azonositocol;
                kod2col = tabinfo2.Kodcol;
                szoveg2col = tabinfo2.Szovegcol;
                if (szoveg2col == -1)
                {
                    if (tabinfo2.Tablanev == "LEIRO")
                        szoveg2col = tabinfo2.Adattabla.Columns.IndexOf("SORSZOV");
                    else
                        szoveg2col = tabinfo2.Azonositocol;
                }
            }
            else if ("CO".Contains(adatfajta1))
            {
                if (tabinfo1.Szovegcol == -1)
                    Adattabla1.Columns.Add(col);
                szoveg1col = Adattabla1.Columns.IndexOf("SZOVEG");
                kod2col = tabinfo2.Kodcol;
                szoveg2col = tabinfo2.Szovegcol;
                if (szoveg2col == -1)
                    szoveg2col = tabinfo2.Azonositocol;
                Osszefinfo1 = Fak.Osszefuggesek[azontip1, tabinfo1.Azontip1, tabinfo1.Azontip2];

                if (Osszefinfo1 == null)
                    Osszefinfo1 = new Osszefinfo(tabinfo1);
                else
                {
                    Osszefinfo1.InitKell = true;
                    Osszefinfo1.OsszefinfoInit();
                    Osszefinfo1.Osszefinfotolt();
                }

                maxszovlength = Osszefinfo1.szovtolt(maxszovlength);
                maxszovlength = szovtolt(maxszovlength);
            }
            if ("CO".Contains(adatfajta2))
            {
                if (tabinfo2.Szovegcol == -1)
                    Adattabla2.Columns.Add(col);
                szoveg2col = Adattabla2.Columns.IndexOf("SZOVEG");
                kod1col = tabinfo1.Kodcol;
                szoveg1col = tabinfo1.Szovegcol;
                if (szoveg1col == -1)
                    szoveg1col = tabinfo1.Azonositocol;
                Osszefinfo2 = Fak.Osszefuggesek[azontip2, tabinfo2.Azontip1, tabinfo2.Azontip2];
                if (Osszefinfo2 == null)
                    Osszefinfo2 = new Osszefinfo(tabinfo2);
                else
                {
                    Osszefinfo2.InitKell = true;
                    Osszefinfo2.OsszefinfoInit();
                    Osszefinfo2.Osszefinfotolt();
                }
                maxszovlength =  Osszefinfo2.szovtolt(maxszovlength);
                maxszovlength = szovtolt(maxszovlength);
            }
            DataRow dr;
            for (int i = 0; i < DataView.Count; i++)
            {
                dr = DataView[i].Row;

                if (tabinfo.Adatfajta != "S")
                {
                    string id1 = dr["SORSZAM1"].ToString();
                    string id2 = dr["SORSZAM2"].ToString();
                    if (tabinfo1.Find(tabinfo1.IdentityColumnName, id1) == null || tabinfo2.Find(tabinfo2.IdentityColumnName, id2) == null)
                    {
                        tabinfo.Adatsortorol(i);
                        i = -1;
                    }

                }
                else
                {
                    string id1 = dr["RSORSZAM"].ToString();
                    if (id1 != "" && id1 != "0")
                    {
                        DataRow drr = tabinfo1.Find(tabinfo1.IdentityColumnName, id1);
                        if (Fak.Alkalmazas =="Tervezo" && drr[tabinfo1.Szovegcol].ToString().CompareTo(dr[szovcol].ToString()) != 0)
                        {
                            tabinfo.Modositott = true;
                            tabinfo.ViewSorindex = i;
                            dr[szovcol] = drr[tabinfo1.Szovegcol];
                            Fak.ValtoztatasFunkcio = "MODIFY";
                            tabinfo.ValtozasNaplozas(dr);
                        }
                    }
                }
            }
            if (tabinfo.Modositott)
                Fak.UpdateTransaction(new Tablainfo[] { tabinfo });
            //}
            return true;
        }
        private void Select(Tablainfo tabinfo, string aktverid)
        {
            string sel = tabinfo.SelectString;
            if (tabinfo.KellVerzio)
            {
                if (sel == "")
                    sel = " where ";
                else
                    sel += " and ";
                tabinfo.Adattabla.LastSel = sel + "VERZIO_ID='" + aktverid + "'";
                Sqlinterface.Select((DataTable)tabinfo.Adattabla,tabinfo.Adattabla.Connection, tabinfo.Tablanev, tabinfo.Adattabla.LastSel, tabinfo.OrderString, false);
                tabinfo.AktVerzioId = Convert.ToInt32(aktverid);
            }
        }
        /// <summary>
        /// Masolatot keszit a megadott AdatTabla-rol
        /// </summary>
        /// <param name="origtable">
        /// a masolando AdatTabla
        /// </param>
        /// <returns>
        /// a masolat
        /// </returns>
        private AdatTabla AdattablaCopy(AdatTabla origtable)
        {
            AdatTabla dt = new AdatTabla(origtable.TableName);
            foreach (DataColumn oldcol in origtable.Columns)
            {
                DataColumn egycol = new DataColumn();
                egycol.ColumnName = oldcol.ColumnName;
                egycol.Caption = oldcol.Caption;
                egycol.ColumnMapping = oldcol.ColumnMapping;
                egycol.DataType = oldcol.DataType;
                egycol.MaxLength = oldcol.MaxLength;
                dt.Columns.Add(egycol);
            }
            for (int i = 0; i < origtable.Rows.Count; i++)
            {
                DataRow egyrow = dt.NewRow();
                DataRow oldrow = origtable.Rows[i];
                for (int j = 0; j < origtable.Columns.Count; j++)
                    egyrow[j] = oldrow[j];
                dt.Rows.Add(egyrow);
            }
            return dt;
        }
        /// <summary>
        /// osszefugges uj verziojanak eloallitasa
        /// </summary>
        public bool Newversion()
        {
            if (Fak.Alkalmazas=="TERVEZO" && tabinfo.KellVerzio && tabinfo.LezartVersion && (tabinfo.VerzioTerkepArray.Count == 0 || tabinfo.Verzioinfok.LastVerzioLezart() && tabinfo.DataView.Count == 0))
            {
                string szoveg = "";
                switch (tabinfo.Szint)
                {
                    case "R":
                        szoveg = "Rendszerszinten";
                        break;
                    case "U":
                        szoveg = "Userszinten";
                        break;
                    case "C":
                        szoveg = "Alkalmazás szinten";
                        break;
                }
                FakPlusz.MessageBox.Show(szoveg + " új verziót kell gyártani!");
                TolteniKell = true;
                return false;
            }
            //if (tabinfo.KellVerzio)
            //{
            //    tabinfo.NewVersionCreated = true;
            //    if (DataView.Count != 0)
            //        oldverid1 = DataView[0].Row["VERZIO_ID1"].ToString();
            //    string oldverid2 = "0";
            //    if (DataView.Count != 0 && tabinfo.VerzioId2col != -1)
            //        oldverid2 = DataView[0].Row["VERZIO_ID2"].ToString();
            //    tabinfo1.AktVerzioId = tabinfo1.LastVersionId;
            //    tabinfo2.AktVerzioId = tabinfo2.LastVersionId;
            //    string maxverid1 = tabinfo1.LastVersionId.ToString();
            //    string maxverid2 = tabinfo2.LastVersionId.ToString();
            //    string sorszamnev1 = "SORSZAM1";
            //    if (adatfajta == "S")
            //    {
            //        sorszamnev1 = "RSORSZAM";
            //        if (DataView.Count == 0)
            //        {
            //            if (oldverid1 != maxverid1)
            //                Select(tabinfo1, maxverid1);
            //            DataRow drr = null;
            //            for (int i = 0; i < DataView1.Count; i++)
            //            {
            //                dr = DataView1[i].Row;
            //                drr = tabinfo.Ujsor();
            //                drr["RSORSZAM"] = dr["SORSZAM"];
            //                drr["PREV_ID1"] = dr["PREV_ID"];
            //                drr["VERZIO_ID1"] = dr["VERZIO_ID"];
            //                drr["KOD"] = dr["KOD"];
            //                drr["SZOVEG"] = dr["SZOVEG"];
            //            }
            //            string savsort = DataView.Sort;
            //            DataView.Sort = "";
            //            int sorrend = 100;
            //            for (int i = 0; i < DataView.Count; i++)
            //            {
            //                drr = DataView[i].Row;
            //                drr["SORREND"] = sorrend;
            //                sorrend = sorrend + 100;
            //            }
            //            DataView.Sort = savsort;
            //            if(DataView1.Count!=0)
            //                tabinfo.Modositott = true;
            //        }
            //    }
            //    if (oldverid1 != maxverid1 || oldverid2 != maxverid2)
            //    {
            //        if (oldverid1 != maxverid1)
            //        {
            //            Select(tabinfo1, maxverid1);
            //            for (int i = 0; i < DataView.Count; i++)
            //            {
            //                dr = DataView[i].Row;
            //                string sorszam = dr[sorszamnev1].ToString();
            //                bool talalt = false;
            //                for (int j = 0; j < DataView1.Count; j++)
            //                {
            //                    dr1 = DataView1[j].Row;
            //                    string previd = dr1["PREV_ID"].ToString();
            //                    if (sorszam == previd)
            //                    {
            //                        talalt = true;
            //                        dr[sorszamnev1] = dr1[tabinfo1.IdentityColumnIndex];
            //                        dr["VERZIO_ID1"] = maxverid1;
            //                        tabinfo.Modositott = true;
            //                        break;
            //                    }
            //                }
            //                if (!talalt)
            //                {
            //                    Adattabla.Adatsortorol(i);
            //                    i = -1;
            //                }
            //            }
            //        }

            //        if (adatfajta != "S")
            //        {
            //            if (oldverid2 != maxverid2)
            //            {
            //                Select(tabinfo2, maxverid2);
            //                for (int i = 0; i < DataView.Count; i++)
            //                {
            //                    dr = DataView[i].Row;
            //                    string sorszam = dr["SORSZAM2"].ToString();
            //                    bool talalt = false;
            //                    for (int j = 0; j < DataView2.Count; j++)
            //                    {
            //                        dr1 = DataView2[j].Row;
            //                        string previd = dr1["PREV_ID"].ToString();
            //                        if (sorszam == previd)
            //                        {
            //                            talalt = true;
            //                            dr["SORSZAM2"] = dr1[tabinfo2.IdentityColumnIndex];
            //                            dr["VERZIO_ID2"] = maxverid2;
            //                            tabinfo.Modositott = true;
            //                            break;
            //                        }
            //                    }
            //                    if (!talalt)
            //                    {
            //                        Adattabla.Adatsortorol(i);
            //                        i = -1;
            //                    }

            //                }
            //            }
            //        }
            //    }
            //    if (tabinfo.Modositott)
            //    {
            //        Fak.UpdateTransaction(new Tablainfo[] { tabinfo });
            //        Verzioinfok verinf = tabinfo.Azonositok.Verzioinfok;
            //        int verz = verinf.VersionArray[verinf.VersionArray.Length - 1];
            //        tabinfo.VerzioTerkepArray.Add(verz);
            //    }
            //    else
            //    {
            //        tabinfo.NewVersionCreated = false;
            //        if (DataView1.Count == 0 || adatfajta != "S" && DataView2.Count == 0)
            //            return false;
            //    }
            //}
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool NewVersionKieg()
        {
            return NewVersionKieg(0);
        }
        public bool NewVersionKieg(int verz)
        {
            if (!tabinfo1.KellVerzio && !tabinfo2.KellVerzio)
                return false;
            bool kelltolt1 = false;
            bool kelltolt2 = false;
            int aktverid = tabinfo.AktVerzioId;
            if (verz != 0)
                aktverid = verz;
            if (tabinfo1.KellVerzio)
            {
                if (tabinfo1.AktVerzioId != aktverid)
                {
                    kelltolt1 = true;
                    if (tabinfo1.LastVersionId < aktverid)
                        tabinfo1.AktVerzioId = tabinfo1.LastVersionId;
                    else
                        tabinfo1.AktVerzioId = aktverid;
                    Select(tabinfo1, tabinfo1.AktVerzioId.ToString());
                }
            }
            if (tabinfo2.KellVerzio)
            {
                kelltolt2 = true;
                if (tabinfo2.AktVerzioId != aktverid)
                {
                    if (tabinfo2.LastVersionId < aktverid)
                        tabinfo2.AktVerzioId = tabinfo1.LastVersionId;
                    else
                        tabinfo2.AktVerzioId = aktverid;
                    Select(tabinfo2, tabinfo2.AktVerzioId.ToString());
                }
            }
                if (!kelltolt1 && !kelltolt2)
                    return false;
                else
                    return true;
            //DataRow dr;
            //DataRow dr1;
            //string adatfajta = tabinfo.Adatfajta;
            //string oldverid1 = "0";
            //string sorszamnev1 = "SORSZAM1";
            //if (adatfajta == "S")
            //    sorszamnev1 = "RSORSZAM";
                //{
                //    sorszamnev1 = "RSORSZAM";
                //    if (DataView.Count == 0)
                //    {
                //        if (oldverid1 != maxverid1)
                //            Select(tabinfo1, maxverid1);
                //        DataRow drr = null;
                //        for (int i = 0; i < DataView1.Count; i++)
                //        {
                //            dr = DataView1[i].Row;
                //            drr = tabinfo.Ujsor();
                //            drr["RSORSZAM"] = dr["SORSZAM"];
                //            drr["PREV_ID1"] = dr["PREV_ID"];
                //            drr["VERZIO_ID1"] = dr["VERZIO_ID"];
                //            drr["KOD"] = dr["KOD"];
                //            drr["SZOVEG"] = dr["SZOVEG"];
                //        }
                //        string savsort = DataView.Sort;
                //        DataView.Sort = "";
                //        int sorrend = 100;
                //        for (int i = 0; i < DataView.Count; i++)
                //        {
                //            drr = DataView[i].Row;
                //            drr["SORREND"] = sorrend;
                //            sorrend = sorrend + 100;
                //        }
                //        DataView.Sort = savsort;
                //        if(DataView1.Count!=0)
                //            tabinfo.Modositott = true;
                //    }
                //}
                //if (oldverid1 != maxverid1 || oldverid2 != maxverid2)
                //{
                //    if (oldverid1 != maxverid1)
                //    {
                //        Select(tabinfo1, maxverid1);
                //        for (int i = 0; i < DataView.Count; i++)
                //        {
                //            dr = DataView[i].Row;
                //            string sorszam = dr[sorszamnev1].ToString();
                //            bool talalt = false;
                //            for (int j = 0; j < DataView1.Count; j++)
                //            {
                //                dr1 = DataView1[j].Row;
                //                string previd = dr1["PREV_ID"].ToString();
                //            if (sorszam == previd)
                //            {
                //                talalt = true;
                //                dr[sorszamnev1] = dr1[tabinfo1.IdentityColumnIndex];
                //                dr["VERZIO_ID1"] = maxverid1;
                //                tabinfo.Modositott = true;
                //                break;
                //            }
                //        }
                //        if (!talalt)
                //        {
                //            Adattabla.Adatsortorol(i);
                //            i = -1;
                //        }
                //    }
                //}

                //if (adatfajta != "S")
                //{
                //if (oldverid2 != maxverid2)
                //{
                //    Select(tabinfo2, maxverid2);
                //    for (int i = 0; i < DataView.Count; i++)
                //    {
                //        dr = DataView[i].Row;
                //        string sorszam = dr["SORSZAM2"].ToString();
                //        bool talalt = false;
                //        for (int j = 0; j < DataView2.Count; j++)
                //        {
                //            dr1 = DataView2[j].Row;
                //            string previd = dr1["PREV_ID"].ToString();
                //            if (sorszam == previd)
                //            {
                //                talalt = true;
                //                dr["SORSZAM2"] = dr1[tabinfo2.IdentityColumnIndex];
                //                        dr["VERZIO_ID2"] = maxverid2;
                //                        tabinfo.Modositott = true;
                //                        break;
                //                    }
                //                }
                //                if (!talalt)
                //                {
                //                    Adattabla.Adatsortorol(i);
                //                    i = -1;
                //                }

                //            }
                //        }
                //    }
                //}
                //    if (tabinfo.Modositott)
                //    {
                //        Fak.UpdateTransaction(new Tablainfo[] { tabinfo });
                //        Verzioinfok verinf = tabinfo.Azonositok.Verzioinfok;
                //        int verz = verinf.VersionArray[verinf.VersionArray.Length - 1];
                //        tabinfo.VerzioTerkepArray.Add(verz);
                //    }
                //    else
                //    {
                //        if (DataView1.Count == 0 || adatfajta != "S" && DataView2.Count == 0)
                //            return false;
                //    }
                //}
        }
        /// <summary>
        /// Osszefugges szurese parameterek alapjan
        /// </summary>
        /// <param name="idparamok">
        /// az objectum ketelemu : elso elem szures, masodik elem szures
        /// ha valamelyik elem osszetett( azaz tovabbi osszefugges), annak szurese tovabbi ketelemu objectum
        /// egy elemi szures vagy a kivant szuroertek, vagy ""
        /// az ures azt jelenti, hogy erre nem kivanunk szurni
        /// </param>
        public string[] GetSzurtOsszef(object[] idparamok)
        {
            return GetSzurtOsszef(idparamok, "");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idparamok"></param>
        /// <param name="kivantnev"></param>
        /// <returns></returns>
        public string[] GetSzurtOsszef(object[] idparamok, string kivantnev)
        {
            OsszefinfoInit();
            DataView.RowFilter = "";
            DataView1.RowFilter = "";
            DataView2.RowFilter = "";
            string[] osszef1idk = null;
            string[] osszef2idk = null;
            if (Osszefinfo1 != null)
            {
                osszef1idk = Osszefinfo1.GetSzurtOsszef((object[])idparamok[0]);
                if (osszef1idk == null)
                    return null;
                return GetSzurtOsszefIdk(new object[] { osszef1idk, idparamok[1] });
            }
            else if (Osszefinfo2 != null)
            {
                osszef2idk = Osszefinfo2.GetSzurtOsszef((object[])idparamok[1]);
                if (osszef2idk == null)
                    return null;
                return GetSzurtOsszefIdk(new object[] { osszef2idk, idparamok[0] });
            }
            else
                return GetSzurtOsszefIdk(idparamok,kivantnev);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idparamok"></param>
        /// <returns></returns>
        public string[] GetSzurtOsszefIdk(object[] idparamok)
        {
            return GetSzurtOsszefIdk(idparamok, "");
        }
        /// <summary>
        /// parametereknek megfelelo OSSZEF idk keresese
        /// </summary>
        /// <param name="idparamok">
        /// parameterek
        /// </param>
        /// <param name="kivantnev">
        /// </param>
        /// <returns>
        /// megfelelo idk tombje
        /// </returns>
        public string[] GetSzurtOsszefIdk(object[] idparamok,string kivantnev)
        {
            ArrayList ar = new ArrayList();
            DataRow dr;
            object[] elsoelem = null;
            object[] masodikelem = null;
            string[] param1ek = null;
            string[] param2ok = null;
            try
            {
                elsoelem = (object[])idparamok[0];
                param1ek = (string[])idparamok[0];
            }
            catch
            {
                param1ek = new string[] { idparamok[0].ToString() };
            }
            try
            {
                masodikelem = (object[])idparamok[1];
                param2ok = (string[])idparamok[1];
            }
            catch
            {
                try
                {
                    param2ok = new string[] { idparamok[1].ToString() };
                }
                catch
                {
                }
            }
            if (tabinfo.Adatfajta == "S")
            {
                for (int i = 0; i < DataView.Count; i++)
                {
                    dr = DataView[i].Row;
                    string egypar = dr["RSORSZAM"].ToString();
                    foreach(string egyparam1 in param1ek)
                    {
                        if(egyparam1 == "" || egyparam1==egypar)
                        {
                            foreach(string egyparam2 in param2ok)
                            {
                                if(egypar==egyparam2 && ar.IndexOf(egypar)== -1)
                                {
                                    ar.Add(egypar);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                foreach(string param1 in param1ek)
                {
                    foreach(string param2 in param2ok)
                    {
                        string visszanev = kivantnev;
                        if (kivantnev == "")
                        {
                            if (param1 != "" && param2 != "")
                                visszanev = tabinfo.IdentityColumnName;
                            else if (param2 == "")
                                visszanev = "SORSZAM2";
                            else
                                visszanev = "SORSZAM1";
                        }
                        for (int i = 0; i < DataView.Count; i++)
                        {
                            dr = DataView[i].Row;
                            if (param1 == "" && dr["SORSZAM2"].ToString() == param2 ||
                                param2 == "" && dr["SORSZAM1"].ToString() == param1 ||
                                param1 != "" && param2 != "" && dr["SORSZAM1"].ToString() == param1 && dr["SORSZAM2"].ToString() == param2)
                            {
                                string adat = dr[visszanev].ToString();
                                if (ar.IndexOf(adat) == -1)
                                    ar.Add(dr[visszanev].ToString());
                            }
                        }
                    }
                }
            }
            if (ar.Count == 0)
                return null;
            return (string[])ar.ToArray(typeof(string));
        }
        /// <summary>
        /// OSSZEF dataview filterezes parameterek alapjan
        /// </summary>
        /// <param name="idparamok">
        /// parameterek
        /// </param>
        /// <returns>
        /// filterezett view
        /// </returns>
        public DataView GetSzurtOsszefView(object[] idparamok)
        {
            OsszefinfoInit();
            DataView.RowFilter = "";
            DataView1.RowFilter = "";
            DataView2.RowFilter = "";
            string[] osszef1idk = null;
            string[] osszef2idk = null;
            if (Osszefinfo1 != null)
            {
                osszef1idk = Osszefinfo1.GetSzurtOsszef((object[])idparamok[0]);
                if (osszef1idk == null)
                    return null;
                return GetSzurtOsszefViewIdk(new object[] { osszef1idk[0], idparamok[1] });
            }
            else if (Osszefinfo2 != null)
            {
                osszef2idk = Osszefinfo2.GetSzurtOsszef((object[])idparamok[1]);
                if (osszef2idk == null)
                    return null;
                return GetSzurtOsszefViewIdk(new object[] { osszef2idk[0], idparamok[0] });
            }
            else
                return GetSzurtOsszefViewIdk(idparamok);

        }
        private DataView GetSzurtOsszefViewIdk(object[] idparamok)
        {
            DataView visszaview = new DataView();
            string filter = "";
            DataRow dr;
            if (tabinfo.Adatfajta == "S")
            {
                visszaview.Table = DataView.Table;
                string[] param1ek = (string[])idparamok[0];
                string[] param2ok = (string[])idparamok[1];
                for (int i = 0; i < DataView.Count; i++)
                {
                    dr = DataView[i].Row;
                    string egypar = dr["RSORSZAM"].ToString();
                    foreach(string param1 in param1ek)
                    {
                        if(param1==egypar)
                        {
                            foreach(string param2 in param2ok)
                            {
                                if(egypar==param2)
                                {
                                    if (filter != "")
                                        filter += " OR ";
                                    filter += "RSORSZAM='" + egypar + "'";
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                string param1 = idparamok[0].ToString();
                string param2 = idparamok[1].ToString();
                string visszanev;
                string visszaid;
                if (param1 != "" && param2 != "")
                {
                    visszanev = tabinfo.IdentityColumnName;
                    visszaid = tabinfo.IdentityColumnName;
                    visszaview = DataView;
                    visszaview.Table = DataView.Table;
                }
                else if (param2 == "")
                {
                    visszanev = "SORSZAM2";
                    visszaid = tabinfo2.IdentityColumnName;
                    visszaview.Table = DataView2.Table;
                }
                else
                {
                    visszanev = "SORSZAM1";
                    visszaid = tabinfo1.IdentityColumnName;
                    visszaview.Table = DataView1.Table;
                }
                for (int i = 0; i < DataView.Count; i++)
                {
                    dr = DataView[i].Row;
                    if (param1 == "" && dr["SORSZAM2"].ToString() == param2 ||
                        param2 == "" && dr["SORSZAM1"].ToString() == param1 ||
                        param1 != "" && param2 != "" && dr["SORSZAM1"].ToString() == param1 && dr["SORSZAM2"].ToString() == param2)
                    {
                        if (filter != "")
                            filter += " OR ";
                        filter += visszaid + "='" + dr[visszanev].ToString() + "'";
                    }
                }
            }
            if (filter == "")
                return null;
            else
            {
                visszaview.RowFilter = filter;
                return visszaview;
            }
        }
        private ArrayList GetOsszefElemIdk()
        {
            ArrayList ar = new ArrayList();
            DataRow dr;
            for (int i = 0; i < DataView.Count; i++)
            {
                dr = DataView[i].Row;
                ar.Add(new object[] { dr["SORSZAM1"].ToString(), dr["SORSZAM2"].ToString() });
            }
            return ar;
        }
        private int  szovtolt(int maxlen)
        {
            int szovlen = maxlen;
            int egyszovlen = 0;
            for (int i = 0; i < Adattabla.Rows.Count; i++)
            {
                DataRow dr1 = Adattabla.Rows[i];
                dr1[szovcol] = "";
                string hason1 = dr1[sorszam1col].ToString().Trim();
                string hason2 = dr1[sorszam2col].ToString().Trim();
                dr1 = toltes(dr1, szovcol, ident1col, hason1, tabinfo1);
                egyszovlen = dr1[szovcol].ToString().Length;
                if (egyszovlen > szovlen)
                    szovlen = egyszovlen;
                dr1 = toltes(dr1, szovcol, ident2col, hason2, tabinfo2);
                egyszovlen = dr1[szovcol].ToString().Length;
                if (egyszovlen > szovlen)
                    szovlen = egyszovlen;
            }
            return szovlen;
        }
        private DataRow toltes(DataRow dr1, int szovcol, int idcol, string hason, Tablainfo egytabinfo)
        {
            AdatTabla adattabla = egytabinfo.Adattabla;
            int egyidcol = egytabinfo.IdentityColumnIndex;
            int egyszovcol = adattabla.Columns.IndexOf("SZOVEG");
            string szov;
            foreach(DataRow dr2 in adattabla.Rows)
            {
                if (dr2[egyidcol].ToString().Trim() == hason)
                {
                    szov = dr1[szovcol].ToString().Trim();
                    string hozzaad = dr2[egyszovcol].ToString().Trim();
//                    if(!szov.Contains("/"))
 //                   if (szov == "" || szov != hozzaad)
 //                   {
                    if (szov != "")
                        szov += "/";
                    szov += dr2[egyszovcol].ToString().Trim();
                    dr1[szovcol] = szov;
 //                   }
                    break;
                }
            }
            return dr1;
        }
    }
}
