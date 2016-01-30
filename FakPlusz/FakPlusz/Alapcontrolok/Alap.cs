using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FakPlusz;
using FakPlusz.Alapfunkciok;
using FakPlusz.Formok;
using FakPlusz.SzerkesztettListak;
namespace FakPlusz.Alapcontrolok
{
    /// <summary>
    /// A CsakGrid illetve Gridpluszinput UserControlok kozos alapja
    /// </summary>
    public partial class Alap : Base
    {
        /// <summary>
        /// Ha ceg, vagy cegalatti szintu termeszetes adattablak tartalomjegyzekevel dolgozunk, a termeszetes
        /// tablak mezoneveinek tartalomjegyzeke, egyebkent null
        /// </summary>
        public Tablainfo tartalinfo = null;
        /// <summary>
        /// Uj verzio-e
        /// </summary>
        public bool UjVerzio = false;
        /// <summary>
        /// ide menthetem az aktualis viewsor indexet, ha kell
        /// </summary>
        public int savviewindex;
        /// <summary>
        /// a tablainformacio DataView-ja
        /// </summary>
        public DataView DataView;
        /// <summary>
        /// az inputtabla
        /// </summary>
        public AdatTabla Inputtabla;
        /// <summary>
        /// a tablainformacio inputoszlopinformacioinak gyujtemenye
        /// </summary>
        public ColCollection InputColumns;
        /// <summary>
        /// a tablainformacio osszes oszlopinformaciojanak gyujtemenye
        /// </summary>
        public ColCollection TablaColumns;
        /// <summary>
        /// a kiegeszito oszlopok gyujtemenye (combo-oszlopoknal)
        /// </summary>
        public ColCollection KiegColumns;
        /// <summary>
        /// az adattabla oszlopai
        /// </summary>
        public DataColumnCollection AdatColumnTomb;
        /// <summary>
        /// Ha a tablaban van SZOVEG nevu oszlop, annak az indexe
        /// Ha az oszlop Combo, a kiegeszito oszlopanak az indexe
        /// Egyebkent a tabla identity indexe
        /// </summary>
        public int szovegcol = -1;
        /// <summary>
        /// ha uj sort szurunk be, true
        /// </summary>
        public bool Beszur = false;
        /// <summary>
        /// Ha egy tabla sorai koze beszurhatunk (ele/moge) szuksegunk van egy, a sorrendet tartalmazo oszlopra
        /// Ha a tablanak van "SORREND" mezeje (pl kodtablak), annak az oszlopinformacioja, egyebkent egy 
        /// generalt (tarolasra nem kerulo) oszlop oszlopinformacioja
        /// </summary>
        public Cols SorrendColumn = null;
        /// <summary>
        /// a sorrend oszlop indexe
        /// </summary>
        public int sorrendcolindex = -1;
        /// <summary>
        /// a beszurando sor sorrendje
        /// </summary>
        public int Beszursorrend = 0;
        /// <summary>
        /// az inputgrid tetejen megjelenitendo szoveg
        /// </summary>
        public string Azonositoszoveg;
        /// <summary>
        /// Az aktualis combo-jellegu inputcella vagy null
        /// </summary>
        public DataGridViewCell combocell = null;
        /// <summary>
        /// Lathato-e a combo
        /// </summary>
        public bool combovisible = false;
//        /// <summary>
//        /// az aktualis textbox-jellegu inputcella vagy null
//        /// </summary>
//        public DataGridViewCell textboxcell = null;
//        /// <summary>
//        /// lathato-e a textboxcella
//        /// </summary>
////        public bool textboxvisible = false;
        /// <summary>
        /// Ha Rendszer, User vagy Ceg uj verziojat allitjuk elo, az elozo verzio ervenyessegenek kezdete +1
        /// </summary>
        public DateTime oldversiondate ;
//        public DateTime versiondate;
        /// <summary>
        /// regi verzioszam +1
        /// </summary>
        public int oldversionnumber;
        /// <summary>
        /// Ha aktualisan Rendszer, User vagy Ceg verziojaval foglalkozunk, true
        /// </summary>
        public bool verzioe = false;
        /// <summary>
        /// objectum letrehozasa
        /// </summary>
        public Alap()
        {
            InitializeComponent();
            MezoControlInfok=new MezoControlInfo[1];
        }
        /// <summary>
        /// A hivoparameterek atvetele
        /// </summary>
        /// <param name="vezerles">
        /// A vezerles informacioi
        /// </param>
        /// <param name="leiroe">
        /// true: leirotablarol van szo
        /// </param>
        public virtual void ParameterAtvetel(Vezerloinfo vezerles,bool leiroe)
        {
            Vezerles = vezerles;
            Hivo = vezerles.Hivo;
            FakUserInterface = vezerles.Fak;
            KezeloiSzint = vezerles.KezeloiSzint;
            HozferJog = Vezerles.HozferJog;
            Leiroe = leiroe;
            AktivMenuItem = vezerles.AktivMenuItem;
            AktivDropDownItem = vezerles.AktivDropDownItem;
            AktivPage = vezerles.AktivPage;
        }
        /// <summary>
        /// Kozos inicializalas, felulirhato
        /// Vegrehajtodik,  vagy a BASE tablarol van szo 
        /// </summary>
        public override void AltalanosInit()
        {
            bool valt = UjTag;
            if (!valt)
            {
                valt = !Tabinfo.KellVerzio && ValtozasLekerdezExcept(new string[] { "Verziovaltozas" }).Count != 0 ||
                    Tabinfo.KellVerzio && ValtozasLekerdez().Count != 0;
            }
            if (!valt)
            {
                if (MezoControlInfok[0] != null)
                {
                    MezoControlInfok[0].UserControlInfo = UserControlInfo;
                    UserControlInfo.AktivPage = AktivPage;
                }
            }
            else
            {
                if (!UjTag)
                    savviewindex = Tabinfo.ViewSorindex;
                //else
                //{
                //    Tabinfo.ViewSorindex
 //               }
                bool verzvaltas = ValtozasLekerdez("Verziovaltozas").Count != 0;
                ValtozasTorol();
                if (verzvaltas)
                {
                    foreach (Control page in AktivPage.Parent.Controls)
                    {
                        if (page != AktivPage)
                        {
                            if (page.Controls.Count != 0)
                            {
                                Base cont = (Base)page.Controls[0];
                                cont.ValtozasTorol("Verziovaltozas");
                            }
                        }
                    }
                }
                verzioe = !Leiroe && TablainfoTag.Tablainfo.Tablanev.Contains("VERSION");
                combocell = null;
                combovisible = false;
                torolalap.Visible = false;
                eleszur.Visible = false;
                mogeszur.Visible = false;
                rogzit.Visible = false;
                string azon = TablainfoTag.Azonositok.Azon;
                if (azon == "LEIR")
                    Leiroe = false;
                else if (this.Name.Contains("Leiro"))
                    Leiroe = true;
                if (Leiroe)
                    Tabinfo = TablainfoTag.Tablainfo.LeiroTablainfo;
                else
                    Tabinfo = TablainfoTag.Tablainfo;
                if (Tabinfo.Tablanev == "LEIRO" && azon == "SZRM")
                {
                    Leiroe = true;
                    Tabinfo = TablainfoTag.Tablainfo;
                }
                if (Tabinfo.InputColumns.Count == 0 || !AktivDropDownItem.Enabled)
                    this.Visible = false;
                else
                    if (this.Parameterez == null)
                        this.Visible = true;
                Tabinfo.Hivo = Hivo;
                if (Tabinfo.Tablanev == "TARTAL" && KezeloiSzint != KezSzint.Fejleszto && "LI".Contains(Tabinfo.Adatfajta))
                {
                    switch (KezeloiSzint)
                    {
                        case KezSzint.Kezelo:
                            HozferJog = HozferJogosultsag.Semmi;
                            break;
                        case KezSzint.Vezeto:
                            HozferJog = HozferJogosultsag.Semmi;
                            break;
                        default:
                            HozferJog = HozferJogosultsag.Irolvas;
                            break;
                    }
                }
                else
                    HozferJog = Hivo.HozferJog;
                if (Leiroe)
                {
                    if (azon.Substring(0, 2) == "SZ" || TablainfoTag.Tablainfo.Tablanev == "BASE")
                    {
                        if (azon == "SZRM")
                            HozferJog = HozferJogosultsag.Csakolvas;
                    }
                    szovegcol = Tabinfo.Adattabla.Columns.IndexOf(Tabinfo.SzovegColName);
                }
                else
                {
                    if (Tabinfo.Tablanev == "TARTAL" && azon == "SZRM")
                        HozferJog = HozferJogosultsag.Csakolvas;
                    szovegcol = Tabinfo.Adattabla.Columns.IndexOf(Tabinfo.SzovegColName);
                }
                Tabinfo.SetAktHozferJog(KezeloiSzint, this.Name);
                if (Tabinfo.Tablanev == "CEGKEZELOKIOSZT")
                    FakUserInterface.Kezeloszereprendberak(Tabinfo);
                SorrendColumn = Tabinfo.SorrendColumn;
                AktivPage.Text = AktivDropDownItem.Text;
                bool valtozasnaplouserlog = Tabinfo.Tablanev == "VALTOZASNAPLO" || Tabinfo.Tablanev == "USERLOG";
                if (LezartCeg)
                {
                    if (HozferJog != HozferJogosultsag.Semmi)
                    {
                        if(Tabinfo.Azonositok.Jogszintek[Convert.ToInt32(KezSzint.Rendszergazda)] == HozferJogosultsag.Irolvas &&
                           (KezeloiSzint==KezSzint.Minden || SzovegesKezeloiSzint[Convert.ToInt32(KezeloiSzint)].Contains("Rendszergazda")))
                            HozferJog = HozferJogosultsag.Irolvas;
                        else
                        {
                            HozferJog = HozferJogosultsag.Csakolvas;
                            Tabinfo.HozferJog = HozferJogosultsag.Csakolvas;
                        }
                    }
                }
                if (Tabinfo.HozferJog == HozferJogosultsag.Irolvas && Tabinfo.Szint == "C")
                    HozferJog = HozferJogosultsag.Irolvas;
                if (HozferJog == HozferJogosultsag.Irolvas && Tabinfo.HozferJog == HozferJogosultsag.Irolvas)
                {
                    if (!valtozasnaplouserlog)
                    {
                        AktivPage.Text += " karbantartása";
                        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                    }
                    else
                    {
                        HozferJog = Tabinfo.HozferJog;
                        AktivPage.Text += " áttekintése";
                        //if (valtozasnaplouserlog)
                        //    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
                        //else
                        //    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    }
                }
                else
                {
                    AktivPage.Text += " áttekintése";
                    //if (valtozasnaplouserlog)
                    //    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
                    //else
                    //    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                }
                if (HozferJog != HozferJogosultsag.Irolvas)
                    toolStrip1.Visible = false;
                else
                    toolStrip1.Visible = true;
                DataView = Tabinfo.DataView;
                Inputtabla = Tabinfo.Inputtabla;
                InputColumns = Tabinfo.InputColumns;
                TablaColumns = Tabinfo.TablaColumns;
                KiegColumns = Tabinfo.KiegColumns;
                Tabinfo.AktualControlInfo = FakUserInterface.ControlTagokTolt(this, panel2, ref Tabinfo, AktivPage, AktivMenuItem, AktivDropDownItem);
                MezoControlInfok[0] = Tabinfo.AktualControlInfo;
                UserControlInfo = FakUserInterface.Attach(this, Vezerles, ref Tabinfo, AktivPage, AktivMenuItem, AktivDropDownItem);
                dataGridView1.Dock = DockStyle.Fill;
                MezoControlInfok[0].UserControlInfo = UserControlInfo;
                Tabinfo.Modositott = false;
                if (!this.Name.Contains("Naptar"))
                    Tabinfo.Tartalmaktolt(true);
                if (!UjTag)
                    Tabinfo.ViewSorindex = savviewindex;
                else
                    Tabinfo.ViewSorindex = Tabinfo.ViewSorindex;
                VerziobuttonokAllit();
                if (!Beszurhat && !verzioe)
                    sorrendcolindex = -1;
                else
                    sorrendcolindex = Tabinfo.Adattabla.Columns.IndexOf(Tabinfo.SorrendColumn.ColumnName);
                UjTag = false;
            }
        }
        /// <summary>
        /// ToolStripButtonok allitasa
        /// </summary>
        public override void VerziobuttonokAllit()
        {
            if (!Tervezoe)
                Hivo.Hivo.VerziobuttonokAllit();
            Beszurhat = Tabinfo.Beszurhat;
            Modosithat = Tabinfo.Modosithat;
            Torolhet = Tabinfo.Torolhet;
            if (verzioe)
            {
                Beszurhat = false;
                Torolhet = false;
            }
            bool valtoztathat = (Beszurhat || Modosithat || Torolhet) && HozferJog==HozferJogosultsag.Irolvas;
            
            toolStrip1.Visible = true;
            rogzit.Visible = false;
            torolalap.Visible = false;
            uj.Visible = false;
            uj.Enabled = !Tabinfo.Valtozott;
            elozoverzio.Visible = false;
            kovetkezoverzio.Visible = false;
            eleszur.Visible = false;
            mogeszur.Visible = false;
            elolrolalap.Visible = false;
            teljestorles.Visible = false;
            if (!valtoztathat)
                toolStrip1.Visible = false;
            else
            {
                if (Beszurhat || Modosithat)
                    rogzit.Visible = true;
                if (Beszurhat)
                {
                    eleszur.Visible = true;
                    mogeszur.Visible = true;
                }
            }
            if (verzioe)
            {
                if (Tabinfo.HozferJog == HozferJogosultsag.Irolvas)
                {
                    elolrolalap.Visible = true;
                    if (Tabinfo.DataView[Tabinfo.DataView.Count - 1].Row["LEZART"].ToString() == "I" && Tabinfo.ViewSorindex != -1)
                    {
                        uj.Visible = true;
                        UjVerzio = true;
                        rogzit.Visible = false;
                    }
                    else
                    {
                        uj.Visible = false;
                        UjVerzio = false;
                        rogzit.Visible = true;
                        if (Tabinfo.Azonositok.Verzioinfok.VersionArray.Length > 1)
                            teljestorles.Visible = true;
                    }
                }
                return;
            }
            else if (Tabinfo.DataView.Count == 0 && !Beszurhat && !Torolhet && Tabinfo.HozferJog==HozferJogosultsag.Irolvas)
            {
                bool kellbeszur = true;
                if (Tabinfo.Tablanev == "VALTOZASNAPLO" || Tabinfo.Tablanev =="USERLOG" || Tabinfo.Tablanev == "KIAJANL" || Tabinfo.Tablanev == "NAPTARAK" || Tabinfo.Tablanev == "OSSZEF" || Tabinfo.Tablanev == "KODTAB")
                    kellbeszur = false;
                else if (HozferJog != HozferJogosultsag.Irolvas) //FakUserInterface.Alkalmazas == "TERVEZO")
                    //         {
                    //            if( Tabinfo.Szint=="U" || Tabinfo.Tablanev=="CEGSZERZODES" || Tabinfo.Tablanev=="CEGKEZELOKIOSZT")
                    kellbeszur = false;
                else if (!AktivDropDownItem.Enabled)
                    kellbeszur = false;

                if (kellbeszur)
                {
                    Tabinfo.Ujsor();
                    FakUserInterface.Rogzit(Tabinfo);
                    Tabinfo.ViewSorindex = 0;
                }
            }
            if (Tabinfo.Tablanev == "VALTOZASNAPLO" || Tabinfo.Tablanev == "USERLOG")
            {
                UjVerzio = false;
                torolalap.Visible = true;
                return;
            }
            if (Tabinfo.KellVerzio && Tabinfo.VerzioTerkepArray.Count > 1 && HozferJog != HozferJogosultsag.Semmi)
            {
                toolStrip1.Visible = true;
                if (Tabinfo.AktVerzioId != Tabinfo.FirstVersionId)
                    elozoverzio.Visible = true;
                if (Tabinfo.AktVerzioId != Tabinfo.LastVersionId)
                    kovetkezoverzio.Visible = true;
            }
            if (Tabinfo.HozferJog == HozferJogosultsag.Irolvas)
            {
                UjVerzio = false;
                elolrolalap.Visible = true;
                rogzit.Visible = false;
                rogzit.Enabled = Tabinfo.Valtozott;
                if (!Tabinfo.KellVerzio || !Tabinfo.LezartVersion)
                {
                    if (Modosithat || Beszurhat || Torolhet)
                    {
                        rogzit.Visible = true;
                        if (Tabinfo.Valtozott)
                            rogzit.Enabled = false;
                        else
                            rogzit.Enabled = true;
                        if (Beszurhat)
                        {
                            eleszur.Visible = true;
                            mogeszur.Visible = true;
                        }
                        else
                        {
                            eleszur.Visible = false;
                            mogeszur.Visible = false;
                        }
                        if (Torolhet)
                        {
                            if (Tabinfo.AktIdentity != -1)
                                torolalap.Visible = true;
                            else
                                torolalap.Visible = false;
                        }
                        else
                            torolalap.Visible = false;
                    }
                    if (!Tabinfo.KellVerzio)
                        return;
                }
                if (Tabinfo.LezartVersion || Tabinfo.VerzioTerkepArray.Count == 0)
                {
                    rogzit.Visible = false;
                    if (Tabinfo.VerzioTerkepArray.Count == 0 || Tabinfo.LastVersionId < Tabinfo.Azonositok.Verzioinfok.LastVersionId)
                    {
                        uj.Visible = true;
                        UjVerzio = true;
                    }
                }
                else if (Tabinfo.VerzioTerkepArray.Count > 1)
                    teljestorles.Visible = true;
                else
                    rogzit.Visible = true;
            }
        }
        /// <summary>
        /// Uj sor a gridview-ban kivalasztott sor ele
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void eleszur_Click(object sender, EventArgs e)
        {
            if (Tabinfo.ViewSorindex != -1)
            {
                int ezenall = Convert.ToInt32(Tabinfo.AktualViewRow[sorrendcolindex].ToString());
                Azonositoszoveg = "Uj sor ";
                if (szovegcol != -1)
                    Azonositoszoveg += Tabinfo.AktualViewRow[szovegcol].ToString() + " elé";
                //            Azonositoszoveg = "Uj sor " + Tabinfo.AktualViewRow[szovegcol].ToString() + " elé"; ;
                Beszursorrend = ezenall / 2;
                int elotte = -1;
                if (Tabinfo.ViewSorindex > 0)
                {
                    elotte = Convert.ToInt32(Tabinfo.DataView[Tabinfo.ViewSorindex - 1].Row[sorrendcolindex].ToString());
                    Beszursorrend = elotte + (ezenall - elotte) / 2;
                }
                Tabinfo.ViewSorindex = -1;
            }
        }
        /// <summary>
        /// Uj sor a gridview-ban kivalasztott sor moge
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void mogeszur_Click(object sender, EventArgs e)
        {
            if (Tabinfo.ViewSorindex != -1)
            {
                int ezenall = Convert.ToInt32(Tabinfo.AktualViewRow[sorrendcolindex].ToString());
                int mogotte;
                Azonositoszoveg = "Uj sor ";
                if (szovegcol != -1)
                    Azonositoszoveg += Tabinfo.AktualViewRow[szovegcol].ToString() + " mögé";
 //               Azonositoszoveg = "Uj sor " + Tabinfo.AktualViewRow[szovegcol].ToString() + " mögé";
                ezenall = Convert.ToInt32(Tabinfo.AktualViewRow[sorrendcolindex].ToString());
                if (Tabinfo.ViewSorindex == DataView.Count - 1)
                    Beszursorrend = ezenall + 100;
                else
                {
                    mogotte = Convert.ToInt32(DataView[Tabinfo.ViewSorindex + 1].Row[sorrendcolindex].ToString());
                    Beszursorrend = (ezenall + mogotte) / 2;
                }
                Tabinfo.ViewSorindex = -1;
            }
        }
        /// <summary>
        /// Rogzites event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void rogzit_Click(object sender, EventArgs e)
        {
                Rogzit();
        }
        /// <summary>
        /// Rogzites 
        /// </summary>
        public virtual void Rogzit()
        {
            savviewindex = Tabinfo.ViewSorindex;
            if (!Leiroe && Tabinfo.Tablanev == "TARTAL" && FakUserInterface.Adatszintek.Contains(Tabinfo.Szint))
            {
                tartalinfo = FakUserInterface.GetByAzontip("SZRMTARTAL");
                if (tartalinfo.AktVerzioId != tartalinfo.LastVersionId)
                {
                    tartalinfo.AktVerzioId = tartalinfo.LastVersionId;
                    tartalinfo.Adattabla.Select();
                }
                string funkcio;
                if (tartalinfo.LezartVersion)
                    tartalinfo.CreateNewVersion();
                for (int i = 0; i < Tabinfo.DataView.Count; i++)
                {
                    funkcio = "MODIFY";
                    DataRow dr = Tabinfo.DataView[i].Row;
                    string tablanev = dr["TABLANEV"].ToString();
                    string kodtipus = "SZRM" + Tabinfo.Szint + tablanev;
                    DataRow ujsor = tartalinfo.Find("KODTIPUS", Tabinfo.Szint + tablanev);
                    string owner = dr["OWNER"].ToString();
                    string userek = dr["USEREK"].ToString();
                    if (ujsor == null)
                    {
                        ujsor = tartalinfo.Ujsor();
                        funkcio = "ADD";
                    }
                    string savsort = tartalinfo.DataView.Sort;
                    tartalinfo.DataView.Sort = "KODTIPUS";
                    int viewind = tartalinfo.ViewSorindex;
                    bool modositott = false;
                    if (funkcio == "ADD")
                    {
                        ujsor[tartalinfo.Kodtipuscol] = Tabinfo.Szint + tablanev;
                        ujsor[tartalinfo.Azontipcol] = kodtipus;
                        ujsor[tartalinfo.Szovegcol] = dr["SZOVEG"].ToString() + " tábla mezöi";
                        ujsor[tartalinfo.Azontip1col] = Tabinfo.Szint + tablanev;
                        ujsor["OWNER"] = owner;
                        ujsor["USEREK"] = userek;
                        ujsor["SORREND"] = dr["SORREND"].ToString();
                        tartalinfo.ViewSorindex = tartalinfo.DataView.Find((object)(Tabinfo.Szint + tablanev));
                        modositott = true;
                    }
                    else
                    {
                        tartalinfo.ViewSorindex = tartalinfo.DataView.Find((object)(Tabinfo.Szint + tablanev));
                        if (ujsor[tartalinfo.Kodtipuscol].ToString() != Tabinfo.Szint + tablanev)
                        {
                            ujsor[tartalinfo.Kodtipuscol] = Tabinfo.Szint + tablanev;
                            ujsor[tartalinfo.Azontip1col] = Tabinfo.Szint + tablanev;
                            modositott = true;
                        }
                        if (ujsor[tartalinfo.Azontipcol].ToString() != kodtipus)
                        {
                            ujsor[tartalinfo.Azontipcol] = kodtipus;
                            modositott = true;
                        }
                        if (ujsor[tartalinfo.Szovegcol].ToString() != dr["SZOVEG"].ToString() + " tábla mezöi")
                        {
                            ujsor[tartalinfo.Szovegcol] = dr["SZOVEG"].ToString() + " tábla mezöi";
                            modositott = true;
                        }
                        if (ujsor["OWNER"].ToString() != owner)
                        {
                            ujsor["OWNER"] = owner;
                            modositott = true;
                        }
                        if (ujsor["USEREK"].ToString() != userek)
                        {
                            ujsor["USEREK"] = userek;
                            modositott = true;
                        }
                        if (ujsor["SORREND"].ToString() != dr["SORREND"].ToString())
                        {
                            ujsor["SORREND"] = dr["SORREND"];
                            modositott = true;
                        }
                    }
                    if (modositott)
                    {
                        tartalinfo.Modositott = true;
                        FakUserInterface.ValtoztatasFunkcio = funkcio;
                        tartalinfo.ValtozasNaplozas(ujsor);
                        tartalinfo.DataView.Sort = savsort;
                        tartalinfo.ViewSorindex = viewind;
                    }
                }
                FakUserInterface.Rogzit(Tabinfo);
            }
            else
                FakUserInterface.Rogzit(new Tablainfo[] { Tabinfo });
            rogzit.Enabled = false;
            if (DataView.Count == 0)
                Tabinfo.ViewSorindex = -1;
            else if (savviewindex < DataView.Count)
                Tabinfo.ViewSorindex = savviewindex;
            else
                Tabinfo.ViewSorindex = 0;
            SetSelectedRow(Tabinfo.ViewSorindex);
            if (verzioe)
            {
                Verzioinfok verinf = FakUserInterface.VerzioInfok[Tabinfo.Szint];
                string conn = verinf.AktualConnection;
                ValtozasBeallit("Verziovaltozas");
                FakUserInterface.Versiontolt(Tabinfo.Szint, conn);
                VerziobuttonokAllit();
            }
            if (Tabinfo.Tablanev == "LEIRO")
            {
                UjTag = true;
                this.AltalanosInit();
            }
            if (KarbantartoPage != null && KarbantartoPage.Controls.Count!=0)
            {
                try
                {
                    Alap karbantarto = (Alap)KarbantartoPage.Controls[0];
                    if (!karbantarto.Modositott)
                    {
                        karbantarto.UjTag = true;
                        karbantarto.AltalanosInit();
                    }
                }
                catch
                {
                    Listatervalap karbantarto = (Listatervalap)KarbantartoPage.Controls[0];
                    if (!karbantarto.Modositott)
                    {
                        karbantarto.UjTag = true;
                        karbantarto.AltalanosInit();
                    }
                }
            }
            RogzitesUtan();
        }
        /// <summary>
        /// lassuk az elozo verziot
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void elozoverzio_Click(object sender, EventArgs e)
        {
            Tabinfo.ElozoVerzio();
            ValtozasBeallit("Verziovaltozas");
            this.AltalanosInit();
        }
        /// <summary>
        /// Lassuk a kovetkezo verziot
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void kovetkezoverzio_Click(object sender, EventArgs e)
        {
            Tabinfo.KovetkezoVerzio();
            ValtozasBeallit("Verziovaltozas");
            this.AltalanosInit();
        }
        /// <summary>
        /// Uj verzio eloallitasa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void uj_Click(object sender, EventArgs e)
        {
            if (Tabinfo.ViewSorindex != -1 && Tabinfo.Valtozott)
            {
                if (!Elveszithet())
                    return;
            }
            if (verzioe && Tabinfo.ViewSorindex != -1)
            {
                oldversiondate = (Convert.ToDateTime(Tabinfo.AktualViewRow["DATUMTOL"].ToString()).AddDays(1));
                oldversionnumber = Convert.ToInt32(Tabinfo.AktualViewRow["VERZIO_ID"].ToString()) + 1;
                int ezenall = Convert.ToInt32(Tabinfo.AktualViewRow[sorrendcolindex].ToString());
                Azonositoszoveg = "Uj verzió ";
                Beszursorrend = ezenall + 100;
                Tabinfo.ViewSorindex = -1;
                VerziobuttonokAllit();
            }
            else
            {
                Tabinfo.CreateNewVersion();
                if (Tabinfo.ViewSorindex != -1)
                    Tabinfo.ViewSorindex = 0;
                ValtozasBeallit("Verziovaltozas");
                this.AltalanosInit();
            }
        }
        /// <summary>
        /// Utolso verzio torlese
        /// Ha a tabla az RVERSION,UVERSION vagy CVERSION, minden tablaban torol
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void teljestorles_Click(object sender, EventArgs e)
        {

            if (verzioe)
            {
                string szov = "";
                int id = Convert.ToInt32(Tabinfo.DataView[Tabinfo.DataView.Count - 1].Row["VERZIO_ID"].ToString());
                TablainfoCollection torlendok = FakUserInterface.Tablainfok.GetBySzintPluszVerzioid("C", id);
                foreach (Tablainfo info in torlendok)
                {
                    do
                    {
                        if (info.AktVerzioId < info.LastVersionId && info.AktVerzioId < id)
                            info.KovetkezoVerzio();
                        else
                            break;

                    } while (true);
                }
                TermCegPluszCegalattiTabinfok = FakUserInterface.GetCegPluszCegalattiTermTablaInfok();
                DataTable dt = new DataTable();
                foreach(Tablainfo info in TermCegPluszCegalattiTabinfok)
                {
                    bool elso=true;
                    for(int i=0;i<info.ComboColumns.Count;i++)
                    {
                        Cols egycol = info.ComboColumns[i];
                        string azontip = egycol.Comboazontip;
                        Tablainfo egyinfo = torlendok.GetByAzontip(egycol.Comboazontip);
                        if (egyinfo != null)
                        {
                            string filebanev = egyinfo.Azonositok.Combofileba;
                            for (int j = 0; j < egyinfo.DataView.Count; j++)
                            {
                                DataRow row = egyinfo.DataView[j].Row;
                                if (row[egyinfo.IdentityColumnName].ToString() == row[egyinfo.PrevIdcol].ToString())
                                {
                                    string tartal = row[filebanev].ToString();
                                    dt.Rows.Clear();
                                    dt = Sqlinterface.Select(dt, FakUserInterface.AktualCegconn, info.Tablanev, " where " + egycol.ColumnName + " = " + tartal, "", true);
                                    if (dt.Rows.Count != 0)
                                    {
                                        if (elso)
                                        {
                                            if (szov != "")
                                                szov += "\n";
                                            szov += info.TablaTag.Node.Text + " már hivatkozik erre a verzióra";
                                            elso = false;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        
                    }
                }
                if (szov != "")
                {
                    MessageBox.Show(szov);
                    return;
                }
                else if (MessageBox.Show("Biztosan törölhetö a teljes verzió?", "", MessageBox.MessageBoxButtons.IgenNem) == MessageBox.DialogResult.Igen)
                {
                    FakUserInterface.DeleteVersionAll(Tabinfo);
                    UjTag = true;
                    ValtozasBeallit("Verziovaltozas");
                    this.AltalanosInit();
                    Hivo.RogzitesUtan();
                    return;
                }
            }
            else if (Tabinfo.Tablanev != "TARTAL")
            {
                string azontip = Tabinfo.Azontip;
                Tablainfo[] infok = FakUserInterface.Tablainfok.GetByAzontipPluszVerzioid(Tabinfo.Azontip, Tabinfo.LastVersionId);
                if (infok != null)
                {
                    string szov = "";
                    foreach (Tablainfo egyinfo in infok)
                    {
                        string szint = egyinfo.Szint;
                        switch (szint)
                        {
                            case "R":
                                szint = "Rendszerszint: ";
                                break;
                            case "U":
                                szint = "Userszint: ";
                                break;
                            default:
                                szint = "Cégszint: ";
                                break;
                        }
                        szov += szint + egyinfo.Szoveg + "\n";
                    }
                    MessageBox.Show("Elöbb az alábbiak verzióit kell törölni:\n" + szov);
                    return;
                }
            }
            Tabinfo.DeleteLastVersion();
            ValtozasBeallit("Verziovaltozas");
            this.AltalanosInit();
        }
        /// <summary>
        /// Allitsuk vissza az adatbazisbeli allapotot
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void elolrolalap_Click(object sender, EventArgs e)
        {
            if (!Leiroe && Tabinfo.Tablanev == "TARTAL" && FakUserInterface.Adatszintek.Contains(Tabinfo.Szint))
            {
                tartalinfo = FakUserInterface.GetByAzontip("SZRMTARTAL");
                FakUserInterface.ForceAdattolt(new Tablainfo[] { Tabinfo, tartalinfo });
            }
            else
            {
                string savfilt = Tabinfo.DataView.RowFilter;
                FakUserInterface.ForceAdattolt(Tabinfo);
                Tabinfo.DataView.RowFilter = savfilt;
            }
            Modositott = false;
            rogzit.Enabled = false;
            if (DataView.Count != 0)
            {
                if(dataGridView1.SelectedRows.Count!=0)
                    dataGridView1.SelectedRows[0].Selected = false;
                dataGridView1.Rows[0].Selected = true;
                dataGridView1.FirstDisplayedScrollingRowIndex = 0;
                Tabinfo.ViewSorindex = 0;
            }
            else
                Tabinfo.ViewSorindex = -1;
            if (!Leiroe && Tabinfo.Tablanev == "TARTAL")
                VerziobuttonokAllit();
        }
        /// <summary>
        /// A modositani/megnezni kivant sor kivalasztasakor hajtodik vegre Beallitja tablainformacio
        /// erintett adatait.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                Tabinfo.ViewSorindex = e.RowIndex;
                if (!Leiroe )
                    VerziobuttonokAllit();
            }
        }
        /// <summary>
        /// Az aktualis sor torlese
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void torolalap_Click(object sender, EventArgs e)
        {
            bool kell = true;
            if (Tabinfo.Tablanev == "TARTAL" || Tabinfo.Tablanev == "BASE")
                kell = MessageBox.Show(FakUserInterface.GetUzenetSzoveg("Torolheto"), "", MessageBox.MessageBoxButtons.IgenNem) == MessageBox.DialogResult.Igen;
            if (kell)
            {
                rogzit.Enabled = true;
                Tabinfo.Adatsortorol(Tabinfo.ViewSorindex);
                if (Tabinfo.Tablanev == "TARTAL" || Tabinfo.Tablanev == "BASE" || Tabinfo.Tablanev == "CEGEK")
                    Rogzit();
            }
        }
 //       }
        /// <summary>
        /// Beallitja a GridView valasztott soranak gridviewselect-jet
        /// </summary>
        /// <param name="viewind">
        /// a sor indexe
        /// </param>
        public void SetSelectedRow(int viewind)
        {
            if (!FakUserInterface.EventTilt)
            {
                if (viewind != -1 && dataGridView1.Rows.Count>viewind)
                {
                    if (dataGridView1.SelectedRows.Count != 0)
                        dataGridView1.SelectedRows[0].Selected = false;
                    dataGridView1.Rows[viewind].Selected = true;
                    SetAktRowVisible(dataGridView1,Tabinfo);
                }
            }
        }
        /// <summary>
        /// ha az inputview egy inputcellajaba a bevitel elott tenni akarunk valamit, ezt kell felulirni
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {

        }
        /// <summary>
        /// ha az inputview egy inputcellajaba tortent bevitel vegen tenni akarunk valamit, ezt kell felulirni
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }
        /// <summary>
        /// Valtozasvizsgalat
        /// </summary>
        /// <returns>
        /// true: Ha valtoztattunk, akar hibasan is
        /// </returns>
        public override bool Changed()
        {
            if (Aktualtablainfo[0].Changed || Aktualtablainfo[0].Modositott || Aktualtablainfo[0].ModositasiHiba)
                return true;
            else
                return false;
        }
        /// <summary>
        /// a usercontrolt a vezerlo usercontrol ugy aktivizalja, hogy a TabStop property-jet false-ra, utana true-ra allitja
        /// Amikor true, az AltalanosInit hivodik meg
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void TabStop_Changed(object sender, EventArgs e)
        {
            if(this.TabStop)
                AltalanosInit();
        }
        /// <summary>
        /// egy cella tartalmanak altalanos(minden cellara vonatkozo) hibavizsgalata. Ha kell ilyen, felul kell irni
        /// </summary>
        /// <param name="dcell">
        /// az erintett cella
        /// </param>
        /// <returns>
        /// Hibas: hibaszoveg, nem hibas: ""
        /// </returns>
        public virtual string Hibavizsg(DataGridViewCell dcell)
        {
            return "";
        }
        /// <summary>
        /// numerikus/nemnumerikus cellak Style property-jenek allitasa
        /// </summary>
        public virtual void Tempcellini()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void dataGridView1_VisibleChanged(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Alap_Load(object sender, EventArgs e)
        {
        }

        public virtual void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
        }

        public virtual void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}
