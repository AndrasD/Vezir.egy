using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FakPlusz;
using FakPlusz.Alapcontrolok;
using FakPlusz.Alapfunkciok;
using FakPlusz.UserAlapcontrolok;

namespace FakPlusz.SzerkesztettListak
{
    /// <summary>
    /// Szerkesztett listak es statisztikak kozos alapja
    /// </summary>
    public partial class SzerkesztettAlap : Base
    {
        /// <summary>
        /// 
        /// </summary>
        public TreeNode alaptreenode = null;
        /// <summary>
        /// 
        /// </summary>
        public TreeNode kelltreenode = null;
        /// <summary>
        /// 
        /// </summary>
        public ArrayList gridviewk = new ArrayList();
        /// <summary>
        /// 
        /// </summary>
        public TreeView kellenetreeview = new TreeView();
        /// <summary>
        /// 
        /// </summary>
        public bool listae;
        /// <summary>
        /// 
        /// </summary>
        public Altlistazoalap altlistazo = null;
        /// <summary>
        /// 
        /// </summary>
        public ArrayList savcont = new ArrayList();
        /// <summary>
        /// 
        /// </summary>
        public int pageindex = 0;
        /// <summary>
        /// 
        /// </summary>
        public int parametersorindexfeltelsoelembol = -1;
        /// <summary>
        /// 
        /// </summary>
        public int[] aktivsorindex = new int[] { -1, -1, -1, -1, -1, -1 };
        /// <summary>
        /// 
        /// </summary>
        public string elsoelemstring = "";
        /// <summary>
        /// 
        /// </summary>
        public string masodikelemstring = "";
        /// <summary>
        /// 
        /// </summary>
        public string nyitozarojelstring = "";
        /// <summary>
        /// 
        /// </summary>
        public string relaciostring = "";
        /// <summary>
        /// 
        /// </summary>
        public string esvagystring = "";
        /// <summary>
        /// 
        /// </summary>
        public string zarozarojelstring = "";
        /// <summary>
        /// 
        /// </summary>
        public string Kodtipus = "";
        /// <summary>
        /// 
        /// </summary>
        public string Azontip = "";
        /// <summary>
        /// 
        /// </summary>
        public string osszeslistaelem = "";
        /// <summary>
        /// 
        /// </summary>
        public string feltetel = "";
        /// <summary>
        /// 
        /// </summary>
        public string sorfeltetel = "";
        /// <summary>
        /// 
        /// </summary>
        public string oszlopfeltetel = "";
        /// <summary>
        /// 
        /// </summary>
        public string listakodtipus = "";
        /// <summary>
        /// 
        /// </summary>
        public string listaazontip = "";
        /// <summary>
        /// 
        /// </summary>
        public int userselectcount = 0;
        /// <summary>
        /// 
        /// </summary>
        public bool teljeshonap = false;
        /// <summary>
        /// 
        /// </summary>
        public bool csakegyhonap = false;
        /// <summary>
        /// 
        /// </summary>
        public ArrayList toolstripek = new ArrayList();
        /// <summary>
        /// 
        /// </summary>
        public AdatTabla listaparametertabla = new AdatTabla("PARAMETER");
        /// <summary>
        /// 
        /// </summary>
        public AdatTabla statisztikaparametertabla = new AdatTabla("PARAMETER");
        /// <summary>
        /// 
        /// </summary>
        public AdatTabla parametertabla = new AdatTabla("PARAMETER");
        /// <summary>
        /// 
        /// </summary>
        public AdatTabla felteteltabla = new AdatTabla("FELTETEL");
        /// <summary>
        /// 
        /// </summary>
        public AdatTabla sorfelteteltabla = new AdatTabla("SORFELTETEL");
        /// <summary>
        /// 
        /// </summary>
        public AdatTabla oszlopfelteteltabla = new AdatTabla("OSZLOPFELTETEL");
        /// <summary>
        /// 
        /// </summary>
        public ArrayList osszestabla = new ArrayList();
        /// <summary>
        /// 
        /// </summary>
        public DataView feltetelview = new DataView();
        /// <summary>
        /// 
        /// </summary>
        public DataView feltetelsview = new DataView();
        /// <summary>
        /// 
        /// </summary>
        public DataView felteteloview = new DataView();
        /// <summary>
        /// 
        /// </summary>
        public ArrayList osszesview = new ArrayList();
        /// <summary>
        /// 
        /// </summary>
        public ArrayList listaparamcombok = new ArrayList();
        /// <summary>
        /// 
        /// </summary>
        public ArrayList statisztikaparamcombok = new ArrayList();
        /// <summary>
        /// 
        /// </summary>
        public ArrayList feltcombok = new ArrayList();
        /// <summary>
        /// 
        /// </summary>
        public ArrayList sorcombok = new ArrayList();
        /// <summary>
        /// 
        /// </summary>
        public ArrayList oszlopcombok = new ArrayList();
        /// <summary>
        /// 
        /// </summary>
        public ArrayList osszescombo = new ArrayList();
        /// <summary>
        /// 
        /// </summary>
        public char[] nyil = new char[] { Convert.ToChar("-"), Convert.ToChar(">") };
        /// <summary>
        /// 
        /// </summary>
        public char[] pontosvesszo = new char[] { Convert.ToChar(";") };
        /// <summary>
        /// 
        /// </summary>
        public char[] zarozarojel = new char[] { Convert.ToChar("]") };
        /// <summary>
        /// 
        /// </summary>
        public char[] vesszo = new char[] { Convert.ToChar(",") };
        /// <summary>
        /// 
        /// </summary>
        public char[] newline = new char[] { Convert.ToChar(10) };
        /// <summary>
        /// 
        /// </summary>
        public string[] split;
        /// <summary>
        /// 
        /// </summary>
        public ArrayList listaparamcombooszlopok = new ArrayList();
        /// <summary>
        /// 
        /// </summary>
        public ArrayList statisztikaparamcombooszlopok = new ArrayList();
        /// <summary>
        /// 
        /// </summary>
        public ArrayList combooszlopok = new ArrayList();
        /// <summary>
        /// 
        /// </summary>
        public string[] nemlathatooszlopok = new string[]{"AZONTIP","MEZONEV","SORRENDBE","OSSZEGBE","ATLAGBA","TETELSORBA","MATRIXPONTBA","FELTETELBE",
            "SORFELTETELBE","OSZLOPFELTETELBE","COMBOELEMEK"};
        /// <summary>
        /// 
        /// </summary>
        public string[] listatobbioszlop = new string[] { "MEZONEVE","OSZLOPSORSZAM", "SORRENDSORSZAM", "KELLOSSZEGZES", "OSSZEGZENDO", "ATLAGOLANDO", "CSAKOSSZEGSORBA" ,
                                                  "VANFELTBEN" };
        /// <summary>
        /// 
        /// </summary>
        public string[] listatobbioszlopnev = new string[] { "Mezö neve", "Oszlop sorszáma", "Sorrend szintje", "Szintváltáskor kell összegzés?", "Összegzendö?", "Átlagolandó?", "Csak szintváltáskor irandó?",
                                                  "Feltételben szerepel?"};
        /// <summary>
        /// 
        /// </summary>
        public string[] statisztikatobbioszlop = new string[] { "MEZONEVE","MATRIXSORSZAM", "SORRENDSORSZAM", "ATLAGOLANDO",
                                                  "VANFELTBEN","VANSORFELTBEN","VANOSZLFELTBEN" };
        /// <summary>
        /// 
        /// </summary>
        public string[] statisztikatobbioszlopnev = new string[] { "Mezö neve", "Mátrixpont sorszáma", "Sorrend szintje", "Átlagolandó?",
                                                  "Feltételben szerepel?","Sordefinicióban szerepel?","Oszlopdefinicióban szerepel?"};
        /// <summary>
        /// 
        /// </summary>
        public string[] nullainitoszlopok = new string[] { "OSZLOPSORSZAM", "SORRENDSORSZAM", "MATRIXSORSZAM" };
        /// <summary>
        /// 
        /// </summary>
        public string[] neminitoszlopok = new string[] { "KELLOSSZEGZES", "OSSZEGZENDO", "ATLAGOLANDO", "CSAKOSSZEGSORBA", "VANFELTBEN", "VANSORFELTBEN", "VANOSZLFELTBEN" };
        /// <summary>
        /// 
        /// </summary>
        public string[] listaszurofeltetel = new string[] { "TETELSORBA='I'", "SORRENDBE='I'", "OSSZEGBE='I'", "ATLAGBA='I'", "FELTETELBE='I'" };
        /// <summary>
        /// 
        /// </summary>
        public string[] statisztikaszurofeltetel = new string[] { "SORRENDBE='I'", "OSSZEGBE='I'", "ATLAGBA='I'", "MATRIXPONTBA='I'", "FELTETELBE='I'", "SORFELTETELBE='I'", "OSZLOPFELTETELBE='I'" };
        /// <summary>
        /// 
        /// </summary>
        public string[] szurofeltetel;
        /// <summary>
        /// 
        /// </summary>
        public string[] listafilter = new string[] { "SORRENDBE='I'", " OSSZEGBE='I' OR ATLAGBA='I'", "", "", "", "TETELSORBA='I' OR SORRENDBE = 'I'" };
        /// <summary>
        /// 
        /// </summary>
        public string[] statisztikafilter = new string[] { "MATRIXPONTBA='I'", "SORRENDBE='I'", "", "ATLAGBA='I'" };
        /// <summary>
        /// 
        /// </summary>
        public string[] paramfilter;
        /// <summary>
        /// Objectum letrehozasa
        /// </summary>
        public SzerkesztettAlap()
        {
            InitializeComponent();
            osszesview.Add(parameterview);
            osszesview.Add(feltetelview);
            osszesview.Add(feltetelsview);
            osszesview.Add(felteteloview);

            for (int i = 0;i<nemlathatooszlopok.Length;i++)
            {
                listaparametertabla.Columns.Add(new DataColumn(nemlathatooszlopok[i]));
                statisztikaparametertabla.Columns.Add(new DataColumn(nemlathatooszlopok[i]));
            }
            for (int i = 0;i<listatobbioszlop.Length;i++)
            {
                string colnev = listatobbioszlop[i];
                DataColumn col = new DataColumn(colnev);
                col.Caption=listatobbioszlopnev[i];
                if(colnev == "OSZLOPSORSZAM" ||colnev=="SORRENDSORSZAM")
                    col.DataType=Type.GetType("System.Int32");
                listaparametertabla.Columns.Add(col);
            }
             for (int i = 0;i<statisztikatobbioszlop.Length;i++)
            {
                string colnev = statisztikatobbioszlop[i];
                DataColumn col = new DataColumn(colnev);
                col.Caption=statisztikatobbioszlopnev[i];
                if(colnev == "MATRIXSORSZAM" ||colnev=="SORRENDSORSZAM")
                    col.DataType=Type.GetType("System.Int32");
                statisztikaparametertabla.Columns.Add(col);
            }
            listaparamcombooszlopok.AddRange(new string[] { "SORRENDSORSZAM", "KELLOSSZEGZES", "OSSZEGZENDO", "ATLAGOLANDO", "CSAKOSSZEGSORBA", "OSZLOPSORSZAM" });
            statisztikaparamcombooszlopok.AddRange(new string[] {"SORRENDSORSZAM", "ATLAGOLANDO","MATRIXSORSZAM" });
            string[] felteteladatnevek = new string[] {"AZONTIP","MEZONEV","NYITOZAROJEL","ELSOELEM","RELACIO","MASODIKELEM",
                "ZAROZAROJEL","ESVAGY"};
            string[] felteteladatszovegek = new[] { "", "", "Nyitó zárjel", "Adat neve ", "Reláció", "Tartalom ", "Záró zárjel", "És/Vagy" };
            AdatTabla[] tablak = new AdatTabla[] { felteteltabla, sorfelteteltabla, oszlopfelteteltabla };
            for (int i = 0; i < felteteladatnevek.Length; i++)
            {
                string adatnev = felteteladatnevek[i];
                string szov = felteteladatszovegek[i];
                for (int j = 0; j < 3; j++)
                {
                    DataColumn col = new DataColumn(adatnev);
                    col.Caption = szov;
                    tablak[j].Columns.Add(col);
                }
            }
            //felteteltabla.Columns.AddRange(
            //    new DataColumn[] {new DataColumn("AZONTIP"),
            //    new DataColumn("MEZONEV"),new DataColumn("NYITOZAROJEL"),
            //    new DataColumn("ELSOELEM"),new DataColumn("RELACIO"),new DataColumn("MASODIKELEM"),
            //    new DataColumn("ZAROZAROJEL"),new DataColumn("ESVAGY")});
            //sorfelteteltabla.Columns.AddRange(new DataColumn[] {new DataColumn("AZONTIP"),
            //    new DataColumn("MEZONEV"),new DataColumn("NYITOZAROJEL"),
            //    new DataColumn("ELSOELEM"),new DataColumn("RELACIO"),new DataColumn("MASODIKELEM"),
            //    new DataColumn("ZAROZAROJEL"),new DataColumn("ESVAGY")});
            //oszlopfelteteltabla.Columns.AddRange(new DataColumn[] {new DataColumn("AZONTIP"),
            //    new DataColumn("MEZONEV"),new DataColumn("NYITOZAROJEL"),
            //    new DataColumn("ELSOELEM"),new DataColumn("RELACIO"),new DataColumn("MASODIKELEM"),
            //    new DataColumn("ZAROZAROJEL"),new DataColumn("ESVAGY")});

            //feltetelview.Table = felteteltabla;
            //feltetelsview.Table = sorfelteteltabla;
            //felteteloview.Table = oszlopfelteteltabla;

        }
        /// <summary>
        /// Aktivizalas, ujraaktivizalas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void TabStop_Changed(object sender, EventArgs e)
        {
            if (this.TabStop)
                AltalanosInit();
        }
        /// <summary>
        /// Inicializalas
        /// </summary>
        public new virtual void AltalanosInit()
        {
            pageindex = 0;
            parametertabla.Rows.Clear();
            felteteltabla.Rows.Clear();
            sorfelteteltabla.Rows.Clear();
            oszlopfelteteltabla.Rows.Clear();
            osszescombo.Clear();
            userselectcount = 0;
        }
        /// <summary>
        /// elemzes
        /// </summary>
        /// <param name="treeview"></param>
        /// <param name="elemek"></param>
        /// <param name="hasontreeview"></param>
        public virtual void Elemez(TreeView treeview, string elemek, TreeView hasontreeview)
        {
            ArrayList ar = new ArrayList();
            string azontip = "";
            string mezonev = "";
            TreeNode fonode = null;
            Tablainfo egyinfo = null;
            DataTable table = parameterview.Table;
            table.Rows.Clear();
            string filt = parameterview.RowFilter;
            string sort = parameterview.Sort;
            parameterview.RowFilter = "";
            parameterview.Sort = "";
            string nev = "";
            treeview.Nodes.Clear();
            if (elemek != "")
            {
                split = elemek.Split(zarozarojel);
                for (int i = 0; i < split.Length; i++)
                {
                    string[] split1 = split[i].Split(pontosvesszo);
                    if (split1[0] != azontip)
                    {
                        azontip = split1[0];
                        egyinfo = FakUserInterface.GetByAzontip(azontip).LeiroTablainfo;
                        nev = egyinfo.TablaTag.Azonositok.Szoveg;
                        fonode = new TreeNode(nev);
                        fonode.Tag = azontip;
                        treeview.Nodes.Add(fonode);
                    }
                    mezonev = split1[1];
                    string filt1 = egyinfo.DataView.RowFilter;
                    egyinfo.DataView.RowFilter = "ADATNEV='" + mezonev + "'";
                    if (egyinfo.DataView.Count != 0)
                    {
                        string egynev = nev + "->" + egyinfo.DataView[0].Row["SORSZOV"].ToString();
                        string tag = azontip + "->" + egyinfo.DataView[0]["ADATNEV"].ToString();
                        if (KellAmezo(azontip, tag, egyinfo, hasontreeview))
                        {
                            TreeNode node = new TreeNode(egynev);
                            node.Tag = tag;
                            fonode.Nodes.Add(node);
                            DataRow dr = table.NewRow();
                            for (int k = 0; k < table.Columns.Count; k++)
                            {
                                string colnev = table.Columns[k].ColumnName;
                                dr[colnev] = split1[k];
                            }
                            table.Rows.Add(dr);
                        }
                    }
                    egyinfo.DataView.RowFilter = filt1;
                }
            }
            for (int i = 0; i < felteteltabla.Rows.Count; i++)
            {
                DataRow dr = felteteltabla.Rows[i];
                azontip = dr["AZONTIP"].ToString();
                mezonev = dr["MEZONEV"].ToString();
                string sorszov = "";
                parameterview.RowFilter = "AZONTIP = '" + azontip + "' AND MEZONEV = '" + mezonev + "'";
                if (parameterview.Count == 0)
                {
                    egyinfo = FakUserInterface.GetByAzontip(azontip).LeiroTablainfo;
                    nev = egyinfo.TablaTag.Azonositok.Szoveg;
                    string savfilt = egyinfo.DataView.RowFilter;
                    egyinfo.DataView.RowFilter = "ADATNEV = '" + mezonev + "'";
                    DataRow dr1 = parametertabla.NewRow();
                    dr1["AZONTIP"] = azontip;
                    dr1["MEZONEV"] = mezonev;
                    sorszov = nev + "->" + egyinfo.DataView[0].Row["SORSZOV"];
                    dr1["MEZONEVE"] = sorszov;
                    dr1["FELTETELBE"] = "I";
                    dr1["VANFELTBEN"] = "Igen";
                    parametertabla.Rows.Add(dr1);
                    egyinfo.DataView.RowFilter = savfilt;
                }
                else
                {
                    sorszov = parameterview[0].Row["MEZONEVE"].ToString();
                    parameterview[0].Row["VANFELTBEN"] = "Igen";
                }
                dr["ELSOELEM"] = sorszov;
            }
            parameterview.RowFilter = filt;
            parameterview.Sort = sort;
        }
        private bool KellAmezo(string azontip, string tag, Tablainfo egyinfo, TreeView hasontreeview)
        {
            if (hasontreeview == null)
                return true;
            for (int j = 0; j < hasontreeview.Nodes.Count; j++)
            {
                TreeNode hasonfonode = hasontreeview.Nodes[j];
                if (azontip == hasonfonode.Tag.ToString())
                {
                    for (int l = 0; l < hasonfonode.Nodes.Count; l++)
                    {
                        if (tag == hasonfonode.Nodes[l].Tag.ToString())
                            return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Feltetelelemzes
        /// </summary>
        /// <param name="adattabla"></param>
        /// <param name="feltetel"></param>
        public virtual void Feltelemez(DataTable adattabla, string feltetel)
        {
            string[] splitsorok;
            string[] splitmezok;
            splitsorok = feltetel.Split(zarozarojel);
            bool voltmar = false;
            if (adattabla.TableName == "FELTETEL")
            {
                userselectcount = adattabla.Rows.Count;
                voltmar = userselectcount != 0;
            }
            for (int i = 0; i < splitsorok.Length; i++)
            {
                DataRow dr = adattabla.NewRow();
                splitmezok = splitsorok[i].Split(pontosvesszo);
                for (int j = 0; j < splitmezok.Length; j++)
                {
                    dr[j] = splitmezok[j];
                }
                if (voltmar)
                {
                    if (i == 0)
                    {
                        switch (adattabla.Rows.Count)
                        {
                            case 1:
                                adattabla.Rows[0]["ESVAGY"] = "ÉS";
                                break;
                            default:
                                adattabla.Rows[0]["NYITOZAROJEL"] = "(";
                                adattabla.Rows[adattabla.Rows.Count - 1]["ZAROZAROJEL"] = ")";
                                adattabla.Rows[adattabla.Rows.Count - 1]["ESVAGY"] = "ÉS";
                                break;
                        }
                        dr["NYITOZAROJEL"] = "(";
                        adattabla.Rows.Add(dr);
                    }
                    else if (voltmar && i == splitsorok.Length - 1)
                    {
                        if (dr["ZAROZAROJEL"].ToString() != ")")
                        {
                            dr["ZAROZAROJEL"] = ")";
                            adattabla.Rows.Add(dr);
                        }
                        else
                        {
                            adattabla.Rows.Add(dr);
                            dr = adattabla.NewRow();
                            dr["ZAROZAROJEL"] = ")";
                            adattabla.Rows.Add(dr);
                        }
                    }
                    else
                        adattabla.Rows.Add(dr);
                }
                else
                    adattabla.Rows.Add(dr);
            }
        }
    }
}
