using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FakPlusz.Alapcontrolok;
using FakPlusz.Alapfunkciok;
namespace FakPlusz.UserAlapcontrolok
{
    /// <summary>
    /// Megadott konvenciok alapjan letrehozott Usercontrolok kozos feladatainak megoldasa
    /// </summary>
    public partial class ControlAlap : Base
    {
        /// <summary>
        /// Lehetseges Alaptipusok enumja
        /// </summary>
        public enum Alapinfotipus 
        {
            /// <summary>
            /// Tipus: Alap
            /// </summary>
            Alap,
            /// <summary>
            /// Tipus: CsakDataGrid
            /// </summary>
            CsakDataGrid,
            /// <summary>
            /// Tipus: Valtozasok
            /// </summary>
            Valtozasok,
            /// <summary>
            /// Tipus: csak szulo
            /// </summary>
            Szulo, 
            /// <summary>
            /// Tipus: Szulo + gyerek(ek)
            /// </summary>
            SzuloGyerekValtozasok,
            /// <summary>
            /// Tipus: tobb gyerek is van
            /// </summary>
            Tobbgyerek,
            /// <summary>
            /// csak gyerek van
            /// </summary>
            Child };
        /// <summary>
        /// a panel vagy groupbox tipusa
        /// </summary>
        public string[] Alapinfostring = new string[] { "Alap","CsakDataGrid", "Valtozasok", "Szulo", "SzuloGyerekValtozasok", "Tobbgyerek", "Child" };
        /// <summary>
        /// Controptipusok kollekcioja
        /// </summary>
        public ControltipusCollection ControltipusCollection;// = new ControltipusCollection();
        /// <summary>
        /// a groupboxok
        /// </summary>
        public GroupBox[] GroupBoxok;
        /// <summary>
        /// a mezocontrolinformaciok tombje
        /// </summary>
        public MezoControlInfo[] Controlinfok;
        /// <summary>
        /// ha valami valtozas volt
        /// </summary>
        public new bool Changed = false;
        /// <summary>
        /// az aktualis mezocontrol kollekcio
        /// </summary>
        public MezoControlCollection AktContInfo = new MezoControlCollection();
        private int[] savind;
        private bool elso = true;
        /// <summary>
        /// 
        /// </summary>
        public bool ujcontroloktoltboljott = true;
        /// <summary>
        /// objectum letrehozas
        /// </summary>
        public ControlAlap()
        {
            InitializeComponent();
            ControltipusCollection = new ControltipusCollection(this);
        }
        /// <summary>
        /// A hivo parametereinek atvetele
        /// </summary>
        /// <param name="fak">
        /// fakuserinterface
        /// </param>
        /// <param name="hivo">
        /// a hivo UserControl
        /// </param>
        /// <param name="vezerles">
        /// vezerloinformaciok
        /// </param>
        public virtual void ParameterAtvetel(FakUserInterface fak, Base hivo, Vezerloinfo vezerles)
        {
            FakUserInterface = fak;
            Hivo = hivo;
            Cegindex = hivo.Cegindex;
            Vezerles = vezerles;
            KezeloiSzint = Vezerles.KezeloiSzint;
            HozferJog = Vezerles.HozferJog;
            LezartCeg=Hivo.LezartCeg;
            if (HozferJog != HozferJogosultsag.Semmi && LezartCeg)
                HozferJog = HozferJogosultsag.Csakolvas;
        }
        /// <summary>
        /// inicializalas
        /// </summary>
        /// <param name="param">
        /// objectumtomb
        /// ezzel hivja meg az Ujterkepez metodust (leiras ott),
        /// majd tovabbi inicializalasokat vegez
        /// </param>
        public virtual void AlapinfoInit(object[] param)
        {
            AktivPage = Vezerles.AktivPage;
            AktivMenuItem = Vezerles.AktivMenuItem;
            AktivDropDownItem = Vezerles.AktivDropDownItem;
            Ujterkepez(param);
            ArrayList ar = new ArrayList();
            ArrayList tabinfoar = new ArrayList();
            foreach(Controltipus egycont in ControltipusCollection)
            {
                if (egycont.GridGroupBox != null)
                {
                    ar.Add(egycont.GridGroupBox);
                    if (egycont.InputGroupBox == null)
                        tabinfoar.Add(egycont.GridGroupBox);
                }
                if (egycont.InputGroupBox != null)
                {
                    ar.Add(egycont.InputGroupBox);
                    tabinfoar.Add(egycont.InputGroupBox);
                }
            }
            GroupBoxok = (GroupBox[])ar.ToArray(typeof(GroupBox));
            AktContInfo = FakUserInterface.ControlTagokTolt(this, GroupBoxok, AktivPage, AktivMenuItem, AktivDropDownItem);
            Aktualtablainfo = new Tablainfo[ControltipusCollection.Count];
            MezoControlInfok = new MezoControlInfo[ControltipusCollection.Count];
            for (int i = 0; i < tabinfoar.Count; i++)
            {
                GroupBox egygr = (GroupBox)tabinfoar[i];
                MezoControlInfo egycontinfo = AktContInfo[egygr.Name];
                int j = AktContInfo.IndexOf(egycontinfo);
                Tablainfo tabinfo = AktContInfo[j].Tabinfo;
                tabinfo.Hivo = Hivo;
                Base.HozferJogosultsag hozferjog = tabinfo.Azonositok.Jogszintek[(int)KezeloiSzint];
                Controltipus egytip = ControltipusCollection[i];
                egytip.SetTablainfo(ref AktContInfo[j].Tabinfo, hozferjog);// = Controlti(Tablainfo)egygr.Tag;
                egytip.MezoControlInfo = AktContInfo[j];
                MezoControlInfok[i] = AktContInfo[j];
                Aktualtablainfo[i] = tabinfo;
                MezoControlInfok[i].UserControlInfo = UserControlInfo;
            }
            savind = new int[Aktualtablainfo.Length];
            SetEgyediValid();
            UserControlInfo = FakUserInterface.Attach(this, Vezerles, ref Aktualtablainfo, AktivPage, AktivMenuItem, AktivDropDownItem);

        }
        private void Ujterkepez(object[] param)
        {
            foreach(object[] egyparam in param)
            {
                object[] controlparam = (object[])egyparam[1];
                Alapinfotipus tipus = (Alapinfotipus)egyparam[0];
                foreach(Control egycontparam in controlparam)
                    new Controltipus(tipus, egycontparam, this);
            }
        }
        /// <summary>
        /// controltipusinformacio toolStripButton-jainak Enabled az allapotnak megfelelo modon 
        /// </summary>
        /// <param name="egycont">
        /// a kivant controltipusinformacio
        /// </param>
        /// <param name="kellchild">
        /// allitsuk-e a childcontroltipusokra is?
        /// </param>
        public virtual void ButtonokEnableAllit(Controltipus egycont, bool kellchild)
        {
            egycont.ButtonokEnableAllit();
            if (kellchild && egycont.ChildControltipus.Count != 0)
            {
                foreach (Controltipus cont in egycont.ChildControltipus)
                    ButtonokEnableAllit(cont, kellchild);
            }
        }
        /// <summary>
        /// valamelyik (nem a munkaasztalra vonatkozo) Button event-je. A szukseges tevekenyseg vegrehajtasa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Button_Click(object sender, EventArgs e)
        {
            ToolStripButton egybut = (ToolStripButton)sender;
            ToolStrip owner = (ToolStrip)egybut.Owner;
            Controltipus conttip = ControltipusCollection.Find(owner);
            if (conttip != null)
            {
                int i = conttip.ButtonokList.IndexOf(egybut);
                string butname = conttip.ButtonNevek[i];
                switch (butname)
                {
                    case "torol":
                        if (MessageBox.Show(FakUserInterface.GetUzenetSzoveg("Torolheto"), "", MessageBox.MessageBoxButtons.IgenNem) == MessageBox.DialogResult.Igen)
                        {
                            conttip.ChildControltipus.TeljesTorles();
                            conttip.Tablainfo.Adatsortorol(conttip.Tablainfo.ViewSorindex);
                            ControltipusCollection.Rogzit(conttip);
                            Controloktolt(conttip, true, true,true);
                        }
                        break;
                    case "rogzit":
                        bool hiba = Hibavizsg(conttip);
                        if (!hiba)
                        {
                            ControltipusCollection.Rogzit(conttip);
                            Controloktolt(conttip, true, true, true);
                        }
                        break;
                    case "ok":
//                        if (!Hibavizsg(UserControlInfo.LastEnter) && !Hibavizsg(conttip))
                        if (!Hibavizsg(conttip))
                            conttip.Ok();
                        break;
                    default:
                        bool mehet = true;
                        ArrayList ar = this.ControltipusCollection.Tablainfok(conttip);
                        if (ar.Count != 1)
                        {
                            for (int ii = 0; ii < ar.Count; ii++)
                            {
                                Tablainfo info = (Tablainfo)ar[ii];
                                if (!info.Valtozott || info.Adattabla.Rows.Count == 0 && info.LehetUres)
                                {
                                    ar.Remove(info);

                                    if (ar.Count == 0)
                                        break;
                                    ii = -1;
                                }
                            }
                            if (ar.Count != 0)
                                mehet = Elveszithet((Tablainfo[])ar.ToArray(typeof(Tablainfo)));
                        }
                        if (mehet)
                        {
                            conttip.UjVolt = false;
                            bool modositott = conttip.Tablainfo.Modositott;
                            switch (butname)
                            {
                                case "uj":
                                    conttip.UjVolt = true;
                                    string idnev = conttip.Tablainfo.IdentityColumnName;
                                    string filter = idnev + " = " + conttip.Aktid.ToString();
                                    if (conttip.DatumtolColumnIndex != -1)
                                    {
                                        foreach (Controltipus egycont in conttip.ChildControltipus)
                                        {
                                            egycont.Tablainfo.DataView.RowFilter = filter;
                                            egycont.Tablainfo.DataView.RowFilter += " and " + egycont.Tablainfo.IdentityColumnName + " IS NOT NULL";
                                            for (int k = 0; k < egycont.Tablainfo.DataView.Count; k++)
                                            {
                                                egycont.Tablainfo.ViewSorindex = k;
                                                DataRow ujsor = egycont.Tablainfo.Ujsor();
                                                int parentidcol = egycont.Tablainfo.TablaColumns.IndexOf(idnev);
                                                if (parentidcol != -1)
                                                    ujsor[parentidcol] = DBNull.Value;
                                            }
                                            egycont.Tablainfo.DataView.RowFilter = idnev + " IS NULL AND " + egycont.Tablainfo.IdentityColumnName + " IS NULL";
                                            for (int k = 0; k < egycont.Tablainfo.DataView.Count; k++)
                                            {
                                                egycont.Tablainfo.ViewSorindex = k;
                                                egycont.Adatsortolt();
                                            }
                                            egycont.Tablainfo.ViewSorindex = 0;
                                            egycont.Tablainfo.Modositott = true;
                                            egycont.Aktid = -1;
                                        }
                                    }
                                    Beallit(conttip, -1, !conttip.UjVolt || conttip.DatumtolColumnIndex == -1,true);
                                    break;
                                case "elozo":
                                    Beallit(conttip, conttip.Tablainfo.ViewSorindex - 1, true,true);
                                    conttip.Tablainfo.Modositott = modositott;
                                   break;
                                case "kovetkezo":
                                    Beallit(conttip, conttip.Tablainfo.ViewSorindex + 1, true,true);
                                    conttip.Tablainfo.Modositott = modositott;
                                    break;
                                case "elolrol":
                                    Elolrol(conttip);
                                    break;
                            }
                        }
                        break;
                }

            }
            if (conttip.ParentChain.Count != 0)
            {
                foreach (Controltipus cont in conttip.ParentChain)
                    ButtonokEnableAllit(cont, true);
            }
            else
                ButtonokEnableAllit(conttip, true);
            EgyediBeallit(conttip);
        }
        /// <summary>
        /// Scroll-nal az Ok buttont tiltja
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void GridView_Scroll(object sender, ScrollEventArgs e)
        {
            DataGridView gridview = (DataGridView)sender;
            Controltipus conttip = ControltipusCollection.Find(gridview);
            conttip.SetOk(false);
        }
        /// <summary>
        /// uj sort valasztunk
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void GridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView gridview = (DataGridView)sender;
            Controltipus conttip = ControltipusCollection.Find(gridview);
            int viewind = conttip.Tablainfo.ViewSorindex;
            if(conttip.Tablainfo.Changed || conttip.Tablainfo.Modositott)
                Hibavizsg(UserControlInfo.LastEnter);
            conttip.SetOk(true);
            if (viewind != e.RowIndex)
            {

                if (conttip.ChildControltipus.Count != 0)
                {
                    if (!conttip.Tablainfo.Changed || conttip.Elveszithet())
                    {
                        Beallit(conttip, e.RowIndex, true, true);
//                        conttip.Beallit(e.RowIndex, true, true);
                        ButtonokEnableAllit(conttip, true);
                    }
                    else
                        conttip.SetDataGridViewRow(conttip.Tablainfo.ViewSorindex);
                }
                else
                {
                    bool modositott = conttip.Tablainfo.Modositott;
                    Beallit(conttip,e.RowIndex, true, true);
                    conttip.Tablainfo.Modositott = modositott;
                    ButtonokEnableAllit(conttip, true);
                }

            }
            else
                conttip.SetFocus();
        }
        /// <summary>
        /// egy adattabla rogzitese
        /// </summary>
        /// <param name="tabinfo">
        /// az adattablainformacio
        /// </param>
        /// <returns>
        /// true: sikeres rogzites
        /// </returns>
        public virtual bool Rogzit(Tablainfo tabinfo)
        {
            return  Rogzit(new Tablainfo[] { tabinfo });
        }
        /// <summary>
        /// tobb adattabla rogzitese
        /// </summary>
        /// <param name="tabinfok">
        /// az adattablak informacioinak tombje
        /// </param>
        /// <returns>
        /// true: sikeres rogzites
        /// </returns>
        public virtual bool Rogzit(Tablainfo[] tabinfok)
        {
            if (!Hivo.Hivo.RogzitesElott())
                return false;
            bool ret = FakUserInterface.Rogzit(tabinfok);
            Hivo.Hivo.RogzitesUtan();
            return ret;
        }
        /// <summary>
        /// Munkaasztal rogzitese hivja, azaz mindent rogzit
        /// </summary>
        /// <returns>
        /// true: sikeres rogzites
        /// </returns>
        public virtual bool RogzitKozos()
        {
            bool hiba = false;
            Hivo.RogzitesElott();
            hiba = !Rogzit(Aktualtablainfo);
            if (!hiba)
            {
                ValtozasBeallit();
                Ujcontroloktolt();
                Hivo.RogzitesUtan();
            }
            return !hiba;
        }
        /// <summary>
        /// Elhagyhato-e a UserControl. Ha nem volt valtozas, vagy a valtozas elveszitheto, igen
        /// </summary>
        /// <returns>
        /// true: igen
        /// </returns>
        public override bool Elhagyhat()
        {
            if (!ControltipusCollection.Valtozott)
                return true;
            else
                return Elveszithet(Aktualtablainfo);
        }
        private void SetEgyediValid()
        {
            foreach(MezoControlInfo egymezinfo in AktContInfo)
            {
                foreach(MezoTag egytag in egymezinfo.Inputeleminfok)
                {
                    if (egytag.Controltipus != "DataGridView")
                        egytag.Control.Validated += Egyedi_Validated;
                }
            }
        }
        /// <summary>
        /// Tablainformacio(k) aktualis sorat tolti az inputcontrolok-bol
        /// </summary>
        /// <param name="info"></param>
        public void Adatsortolt(Tablainfo[] info)
        {
            foreach(Tablainfo egyinfo in info)
                if(egyinfo.Valtozott) 
                    egyinfo.Adatsortolt();
        }
        /// <summary>
        /// Controlok toltese/ujratoltese
        /// </summary>
        public virtual void Ujcontroloktolt()
        {
            ujcontroloktoltboljott = true;
            HozferJog = Vezerles.HozferJog;
            LezartCeg = Hivo.LezartCeg;
            if (HozferJog != HozferJogosultsag.Semmi && LezartCeg)
                HozferJog = HozferJogosultsag.Csakolvas;
            KezeloiSzint = Vezerles.KezeloiSzint;
            for (int i = 0; i < ControltipusCollection.Count; i++)
            {
                Controltipus egytip = ControltipusCollection[i];
                if (egytip.Parent == null)
                    Controloktolt(egytip, true, true, false);
            }
//            ControltipusCollection.Controloktolt(true,false);
            for (int i = 0; i < ControltipusCollection.Count; i++)
            {
                Controltipus egytip = ControltipusCollection[i];
                ButtonokEnableAllit(egytip, false);
                if (egytip.Tipus == Alapinfotipus.Child)
                {
                    if (egytip.InputGroupBox != null)
                        egytip.InputGroupBox.Enabled = false;
                }
            }
            if(tabstopboljott)
                ValtozasTorol();
            ujcontroloktoltboljott = false;
            tabstopboljott = false;
            SajatJelzesTorol();
        }
        /// <summary>
        /// controltipusinformacio adattabla toltes, ha ez egy childcontroltipus, beallitast is vegez
        /// </summary>
        /// <param name="egycont">
        /// controltipusinformacio
        /// </param>
        /// <param name="force">
        /// mindenkeppen vegrehajtando vagy csak, ha uj node-dal hivtak vagy nem volt valtozas
        /// </param>
        /// <param name="kellchild">
        /// a childcontrolinfokra is?
        /// </param>
        /// <param name="kellfocus">
        /// ??
        /// </param>
        public virtual void Controloktolt(Controltipus egycont, bool force, bool kellchild,bool kellfocus)
        {
            egycont.Controloktolt(force, kellchild,kellfocus);
            if (egycont.Parent != null)
                Beallit(egycont, egycont.Tablainfo.ViewSorindex, kellchild);
        }
        /// <summary>
        /// kivant controltipusinfo kivant dataview index alapjan beallitasokat vegez
        /// </summary>
        /// <param name="egycont">
        /// controltipusinfo
        /// </param>
        /// <param name="viewindex">
        /// dataview index
        /// </param>
        /// <param name="kellchild">
        /// ha van childcontroltipusa, az(ok)at is kell-e allitani
        /// </param>
        public virtual void Beallit(Controltipus egycont, int viewindex, bool kellchild)
        {
            Beallit(egycont, viewindex, kellchild, false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="egycont"></param>
        /// <param name="viewindex"></param>
        /// <param name="kellchild"></param>
        /// <param name="kellfocus"></param>
        public virtual void Beallit(Controltipus egycont, int viewindex, bool kellchild, bool kellfocus)
        {
            egycont.Beallit(viewindex, kellchild, kellfocus);
        }
        /// <summary>
        /// Meghivasra kerul mindig, ha beallitasra van szukseg. Specialis beallitasok eseteben felulirando
        /// </summary>
        /// <param name="egycont"></param>
        public virtual void EgyediBeallit(Controltipus egycont)
        {
        }
        public virtual void GroupBox_Enter(GroupBox box)
        {
        }
        public virtual void GroupBox_Leave(GroupBox box)
        {
        }
        /// <summary>
        /// Hibavizsgalat egy controltipusra
        /// </summary>
        /// <param name="conttip">
        /// inputcontrolinformacio
        /// </param>
        /// <returns>
        /// true: hiba van
        /// </returns>

        public virtual bool Hibavizsg(Controltipus conttip)
        {
            bool hiba = false;
            Tablainfo tabinfo = conttip.Tablainfo;
            if (!tabinfo.LehetUres && tabinfo.Ures || tabinfo.Valtozott)
                hiba = Hibavizsg(conttip.MezoControlInfo);
            if (!hiba)
            {
                hiba = Egyvalid(conttip.MezoControlInfo);
            }
            return hiba;
        }
        /// <summary>
        /// Hibavizsgalat egy mezocontrolinfo-ra
        /// </summary>
        /// <param name="continf"></param>
        /// <returns></returns>
        public bool Hibavizsg(MezoControlInfo continf)
        {
            return continf.Hibavizsg();
        }
        /// <summary>
        /// Vegso validalas rogzites elott
        /// </summary>
        /// <returns></returns>
        public override bool VegeValidalas()
        {
            bool osszeshiba = false;
            bool hiba = Hibavizsg(UserControlInfo.LastEnter);
            for (int i = 0; i < Aktualtablainfo.Length; i++)
            {
                Tablainfo egyinfo = Aktualtablainfo[i];
                if (Hibavizsg(ControltipusCollection[i]))
                    hiba = true;
                else
                {
                    if(egyinfo.Ures && egyinfo.LehetUres)
                    {
                        egyinfo.ModositasiHiba = false;
                        egyinfo.Changed = false;
                        foreach(MezoControlInfo egymezocont in egyinfo.ControlInfok)
                        {
                            foreach(MezoTag egytag in egymezocont.Inputeleminfok)
                                FakUserInterface.ErrorProvider.SetError(egytag.Control, "");
                        }
                    }
                    else if(egyinfo.Ures)
                        osszeshiba = true;
                }
            }
            if (!hiba && !osszeshiba)
            {
                Adatsortolt(Aktualtablainfo);
                VegeTevekenysegek();
                return false;
            }
            return true;
        }
        /// <summary>
        /// Felulirhato, 
        /// akkor hivodik meg, ha a munkaasztal rogzitese utan a hibavizsgalatok nem fedeztek fel hibat
        /// </summary>
        public override void VegeTevekenysegek()
        {
        }
        //public virtual void DataGridView_CellValidated(object sender, DataGridViewCellEventArgs e)
        //{
        //    DataGridView gw = (DataGridView)sender;
        //    if (gw.Enabled)
        //    {
        //        DataGridViewCell cell = gw[e.ColumnIndex, e.RowIndex];
        //        Taggyart egytag = (Taggyart)gw.Columns[e.ColumnIndex].Tag;
        //        Taggyart egytag1;
        //        if (egytag != null)
        //        {
        //            egytag.SelectedCell = cell;
        //            if (cell.Tag == null)
        //            {
        //                egytag1 = Fak.Taggyart(cell, egytag);
        //                cell.Tag = egytag1;
        //            }
        //            else
        //                egytag1 = (Taggyart)cell.Tag;
        //            string hiba = Fak.Hibavizsg(egytag1);
        //        }
        //    }
        //}
        /// <summary>
        /// egyedi validalast vegez a controltipusinformacio inputcontroljain, child(ok) eseten azokon is
        /// </summary>
        /// <param name="egycont">
        /// controlinformacio
        /// </param>
        /// <returns>
        /// true:hiba
        /// </returns>
        public bool Egyvalid(Controltipus egycont)
        {
            bool childhiba = false;
            bool hiba = Egyvalid(egycont.MezoControlInfo);
            childhiba = egycont.ChildControltipus.Egyvalid();
            return hiba || childhiba;
        }
        /// <summary>
        /// egy inputcontrolinformacio inputcontroljain vegez egyedi validalast
        /// </summary>
        /// <param name="egycont">
        /// controltipusinformacio
        /// </param>
        /// <returns>
        /// true:hiba
        /// </returns>
        public bool Egyvalid(MezoControlInfo egycont)
        {
            bool hiba = false;
            foreach(MezoTag egytag in egycont.Inputeleminfok)
            {
                if (EgyediValidalas(egytag))
                    hiba = true;
            }
            return hiba;
        }
        /// <summary>
        /// az osszes tablainformacio adatbazisbeli helyreallitas, ha kerjuk
        /// </summary>
        /// <returns></returns>
        public override bool Elveszithet()
        {
            return Elveszithet(Aktualtablainfo);
        }
        /// <summary>
        /// Modositott tablainformacio modositasanak elveszteserol, azaz adatbazisbeli visszaallitasarol van szo
        /// A kerdes feltevese " A módositásokat elvesziti!\n Rendben ?" standard kerdes lesz. Igen eseten
        /// vegre is hajtja a visszaallitast
        /// </summary>
        /// <param name="tabinfo">
        /// tablainformacio
        /// </param>
        /// <returns>
        /// true: visszaallitas megtortent
        /// </returns>
        public bool Elveszithet(Tablainfo tabinfo)
        {
            return Elveszithet(new Tablainfo[] { tabinfo });
        }
        /// <summary>
        /// ugyanolyan mint az Elveszithet (Tablainfo tabinfo), csak tobb tablainformaciora es felulirhato
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public virtual bool Elveszithet(Tablainfo[] info)
        {
            bool kell=false;
            foreach (Tablainfo infoegy in info)
            {
                if (infoegy.Adattabla.Rows.Count != 0)
                    kell = true;
            }
            if(!kell || base.Elveszithet())
            {
                FakUserInterface.ForceAdattolt(info, true);
                foreach (Tablainfo egyinfo in info)
                    egyinfo.ViewSorindex = egyinfo.DataView.Count - 1;
                return true;
            }
            else
                return false;

        }
        /// <summary>
        /// ures, ha szukseges felulirando
        /// </summary>
        /// <param name="egytag">
        /// vizsgalando inputcontrolinformacio
        /// </param>
        /// <returns>
        /// true: hiba
        /// </returns>
        public virtual bool EgyediValidalas(MezoTag egytag)
        {
            return false;
        }
        /// <summary>
        /// A UserControl aktivizalasa/ujraaktivizalasa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void TabStop_Changed(object sender, EventArgs e)
        {
            if (this.TabStop) 
            {
                if (elso)
                {
                    Paramfajta = Parameterezes.Nincsparameterezes;
                    int i = Vezerles.ControlNevek.IndexOf(this.Name);
                    if (i != -1)
                        Paramfajta = Vezerles.Parameterek[i];
                    elso = false;
                }
                bool valt = ValtozasLekerdez().Count != 0 || SajatJelzesLekerdez();
                if (valt || Parameterez != null && (Parameterez.Valtozas || Parameterez.tabstopboljott))
                {
                    if(Parameterez!=null)
                        Parameterez.tabstopboljott = false;
                    tabstopboljott = true;
                    Ujcontroloktolt();
                }
            }
        }
        /// <summary>
        /// egy inputelem validated eventje. Ha az inputelem tartalma a kozonseges, mar elvegzett hibavizsgalatok
        /// alapjan nem hibas, meghivja az alapjaban ures EgyediValidalas metodust, ha szuksegunk van egyedi, azaz specialis
        /// vizsgalatra, azt felul kell irnunk
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //public override void Egyedi_Validated(object sender, EventArgs e)
        //{
        //    if (!FakUserInterface.EventTilt)
        //    {
        //        Control cont = (Control)sender;
        //        MezoTag tag = (MezoTag)cont.Tag;
        //        if (tag.Hibaszov == "")
        //        {
        //            {
        //                hiba = EgyediValidalas(tag);
        //                FakUserInterface.ErrorProvider.SetError(cont, tag.Hibaszov);
        //            }
        //        }
        //        else
        //            hiba = true;
        //        if (hiba)
        //            tag.Tabinfo.ModositasiHiba = true;
        //        Controltipus egycont = ControltipusCollection.Find((GroupBox)tag.ParentControl);
        //        if (egycont.ButtonNevek == null || egycont.ButtonNevek.Length == 0)
        //        {
        //            if (!ControltipusCollection.Hibas)
        //            {
        //                egycont.Hivo.teljesrogzit.Visible = true;
        //                egycont.Hivo.teljesrogzit.Enabled = true;
        //            }
        //            else
        //            {
        //                egycont.Hivo.teljesrogzit.Visible = false;
        //                egycont.Hivo.teljesrogzit.Enabled = false;
        //            }
        //        }
        //        if (egycont.Parent != null)
        //        {
        //            int i = egycont.ButtonNevekList.IndexOf("ok");
        //            if (i != -1)
        //            {
        //                if (!tag.Tabinfo.Ures)
        //                {
        //                    if (!egycont.Hibas)
        //                    {
        //                        egycont.Buttonok[i].Visible = true;
        //                        egycont.Buttonok[i].Enabled = true;
        //                    }
        //                    else
        //                        egycont.Buttonok[i].Enabled = false;
        //                }
        //            }
        //        }
        //        ButtonokEnableAllit(egycont, true);
        //    }
        //}
        /// <summary>
        /// Munkaasztal elolrol megnyomasa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void teljeselolrol_Click(object sender, EventArgs e)
        {
            TeljesElolrol();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool TeljesElolrol()
        {
            if (ControltipusCollection.Valtozott)
            {
                if (MessageBox.Show(FakUserInterface.GetUzenetSzoveg("Modositasvesztes"), "", MessageBox.MessageBoxButtons.IgenNem) == MessageBox.DialogResult.Igen)
                {
                    Elolrol();
                    return false;
                }
                else
                    return true;
            }
            else
            {
                Elolrol();
                return false;
            }
        }
        /// <summary>
        /// elolrol
        /// </summary>
        public override void Elolrol()
        {
            base.Elolrol();
            foreach (Controltipus cont in ControltipusCollection)
            {
                cont.UjVolt = false;
                cont.OkVolt = false;
            }
            if (!SajatJelzesLekerdez())
            {
                if (ValtozasLekerdez().Count == 0)
                    SajatJelzesBeallit();
                Ujcontroloktolt();
            }
        }
        /// <summary>
        /// Munkaasztal rogzit meggnyomasa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void teljesrogzit_Click(object sender, EventArgs e)
        {
            if (!VegeValidalas())
                RogzitKozos();
        }
        /// <summary>
        /// controltipusinformacio es a childok tablainformacioinak adatbazisbeli visszaallitasa, ezekre a Controloktolt hivasa is
        /// </summary>
        /// <param name="conttip">
        /// kivant controltipusinformacio
        /// </param>
        public virtual void Elolrol(Controltipus conttip)
        {
            this.Visible = false;
            ControltipusCollection.Elolrol(conttip);
            Controloktolt(conttip, false, true,true);
            this.Visible = true;
        }
    }
    /// <summary>
    /// Controltipusok kollekcioja
    /// </summary>
    public class ControltipusCollection : ArrayList
    {
        private ArrayList tabindexek = new ArrayList();
        private ArrayList GroupBoxokNevei = new ArrayList();
        private ArrayList PanelekNevei = new ArrayList();
        private ArrayList GridViewk = new ArrayList();
        private ArrayList ToolStripek = new ArrayList();
        /// <summary>
        /// a hivo
        /// </summary>
        public ControlAlap Hivo;
        /// <summary>
        /// objectum letrehozasa
        /// </summary>
        public ControltipusCollection(ControlAlap hivo)
        {
            Hivo = hivo;
        }
        /// <summary>
        /// kereses controltipus alapjan
        /// </summary>
        /// <param name="conttip">
        /// a keresett controltipus
        /// </param>
        /// <returns>
        /// Controltipus vagy null
        /// </returns>
        public Controltipus this[Controltipus conttip]
        {
            get 
            {
                int i = this.IndexOf(conttip);
                if(i==-1)
                    return null;
                else
                    return this[i];
            }
        }
        /// <summary>
        /// kereses index alapjan
        /// </summary>
        /// <param name="index">
        /// a keresett index
        /// </param>
        /// <returns>
        /// controltipus vagy null
        /// </returns>
        public new Controltipus this[int index]
        {
            get
            {
                return (Controltipus)base[index];
            }
            set
            {
                base[index] = value;
            }
        }
        /// <summary>
        /// Uj Controltipus hozzaadasa, a befuzes TabIndex alapjan
        /// </summary>
        /// <param name="tabindex">
        /// a tabindex
        /// </param>
        /// <param name="conttip">
        /// a Controltipus
        /// </param>
        /// <returns>
        /// a befuzes indexe
        /// </returns>
        public int Add(int tabindex, Controltipus conttip)
        {
            bool megvan = false;
            int i = 0;
            for (i = 0; i < tabindexek.Count; i++)
            {
                int egyindex = Convert.ToInt32(tabindexek[i].ToString());
                if (egyindex > tabindex)
                {
                    megvan = true;
                    this.Insert(i, conttip);
                    tabindexek.Insert(i, tabindex);
                    if (conttip.InputGroupBox == null)
                        GroupBoxokNevei.Insert(i, conttip.GridGroupBox.Name);
                    else
                        GroupBoxokNevei.Insert(i, conttip.InputGroupBox.Name);
                    if (conttip.Panel != null)
                        PanelekNevei.Insert(i, conttip.Panel.Name);
                    else
                        PanelekNevei.Insert(i, "");
                    GridViewk.Insert(i, conttip.DataGridView);
                    ToolStripek.Insert(i, conttip.ToolStrip);
                    break;
                }
            }
            if (!megvan)
            {
                tabindexek.Add(tabindex);
                if (conttip.InputGroupBox == null)
                    GroupBoxokNevei.Add(conttip.GridGroupBox.Name);
                else
                    GroupBoxokNevei.Add(conttip.InputGroupBox.Name);
                if (conttip.Panel != null)
                    PanelekNevei.Add(conttip.Panel.Name);
                else
                    PanelekNevei.Add("");

                GridViewk.Add(conttip.DataGridView);
                ToolStripek.Add(conttip.ToolStrip);
                i = this.Add(conttip);
            }
            return i;
        }
        /// <summary>
        /// kereses panelnev alapjan
        /// </summary>
        /// <param name="panelnev">
        /// a panel neve
        /// </param>
        /// <returns>
        /// Controltipus vagy null
        /// </returns>
        public Controltipus Find(string panelnev)
        {
            int i = PanelekNevei.IndexOf(panelnev);
            if (i != -1)
                return this[i];
            else
                return null;
        }
        /// <summary>
        /// kereses gridview alapjan
        /// </summary>
        /// <param name="gridview">
        /// a gridview
        /// </param>
        /// <returns>
        /// Controltipus vagy null
        /// </returns>
        public Controltipus Find(DataGridView gridview)
        {
            int i = GridViewk.IndexOf(gridview);
            if (i != -1)
                return this[i];
            else
                return null;
        }
        /// <summary>
        /// kereses ToolStrip alapjan
        /// </summary>
        /// <param name="toolstrip">
        /// a ToolStrip
        /// </param>
        /// <returns>
        /// Controltipus vagy null
        /// </returns>
        public Controltipus Find(ToolStrip toolstrip)
        {
            int i = ToolStripek.IndexOf(toolstrip);
            if (i != -1)
                return this[i];
            else
                return null;
        }
        /// <summary>
        /// kereses groupbox alapjan
        /// </summary>
        /// <param name="groupbox">
        /// a groupbox
        /// </param>
        /// <returns>
        /// Controltipus vagy null
        /// </returns>
        public Controltipus Find(GroupBox groupbox)
        {
            int i = GroupBoxokNevei.IndexOf(groupbox.Name);
            if (i != -1)
                return this[i];
            else
                return null;
        }
        /// <summary>
        /// Controltipus(ok) toltese
        /// </summary>
        /// <param name="kellchild">
        /// true: gyerekeket is
        /// </param>
        /// <param name="kellfocus">
        /// ??
        /// </param>
        public void Controloktolt(bool kellchild,bool kellfocus)
        {
            Controloktolt(false, kellchild,kellfocus);
        }
        /// <summary>
        /// Controltipus(ok) toltese
        /// </summary>
        /// <param name="force">
        /// true: mindenkeppen tolt
        /// false: csak, ha kell
        /// </param>
        /// <param name="kellchild">
        /// true: gyerekeket is
        /// </param>
        /// <param name="kellfocus">
        /// ???
        /// </param>
        public void Controloktolt(bool force, bool kellchild,bool kellfocus)
        {
            foreach (Controltipus egycont in this)
                Hivo.Controloktolt(egycont, force, kellchild,kellfocus);
        }
        /// <summary>
        /// Buttonok visible es/vagy enabled allitasa, gyerekeknek is
        /// </summary>
        public void ButtonokEnableAllit()
        {
            foreach (Controltipus egycont in this)
                Hivo.ButtonokEnableAllit(egycont, true);
        }
        /// <summary>
        /// Volt valtozas?
        /// </summary>
        private bool valtozott = false;
        /// <summary>
        /// 
        /// </summary>
        public bool Valtozott
        {
            get 
            {
                valtozott = false;
                if(Uj && !LehetUres && Ures)
                    return valtozott;
                foreach (Controltipus egycont in this)
                {
                    if(egycont.Valtozott)
                        valtozott = true;
                }
                return valtozott;
            }
        }
        /// <summary>
        /// Volt- e hiba?
        /// </summary>
        /// <returns>
        /// true: igen
        /// </returns>
        public bool Hibavizsg()
        {
            bool hiba = false;
            bool childhiba = false;
            foreach (Controltipus egycont in this)
            {
                if (egycont.Hivo.Hibavizsg(egycont.MezoControlInfo))
                    hiba = true;
                if (egycont.ChildControltipus.Hibavizsg())
                    childhiba = true;
            }
            return hiba || childhiba;
        }
        /// <summary>
        /// Van validalasi hiba?
        /// </summary>
        /// <returns>
        /// true: igen
        /// </returns>
        public bool Egyvalid()
        {
            bool hiba = false;
            bool childhiba = false;
            foreach (Controltipus egycont in this)
            {
                hiba = egycont.Hivo.Egyvalid(egycont.MezoControlInfo);
                childhiba = egycont.ChildControltipus.Egyvalid();
            }
            return hiba || childhiba;
        }
        /// <summary>
        /// Controltipus adattablainak tartalmat visszaalitja az eredetire, tolt
        /// </summary>
        /// <param name="conttip">
        /// Conttroltipus
        /// </param>
        public void Elolrol(Controltipus conttip)
        {
            ArrayList ar = Tablainfok(conttip);
            conttip.FakUserInterface.ForceAdattolt((Tablainfo[])ar.ToArray(typeof(Tablainfo)), true);
            Hivo.Controloktolt(conttip, true, true, true);
        }
        /// <summary>
        /// Aktualis adatsor a mezokbe
        /// </summary>
        public void Adatsortolt()
        {
            foreach (Controltipus egycont in this)
            {
                if (egycont.Parent == null)
                    egycont.Adatsortolt();
            }
        }
        /// <summary>
        /// Torli az adattablat, gyerekeket is
        /// </summary>
        public void TeljesTorles()
        {
            foreach (Controltipus egycont in this)
            {
                egycont.ChildControltipus.TeljesTorles();
                egycont.Tablainfo.TeljesTorles();
            }
        }
        /// <summary>
        /// Controltipus es gyerekei adattablainak rogzitese, rogzites utani eljaras
        /// </summary>
        /// <param name="egycont">
        /// Controltipus
        /// </param>
        /// <returns>
        /// sikeres rogzites
        /// </returns>
        public bool Rogzit(Controltipus egycont)
        {
            Hivo.RogzitesElott();
            ArrayList ar = Tablainfok(egycont);
            bool ret = egycont.FakUserInterface.Rogzit((Tablainfo[])ar.ToArray(typeof(Tablainfo)));
//            egycont.Valtozott = false;
            Hivo.RogzitesUtan();
            return ret;
        }
        /// <summary>
        /// Controltipus es gyerekei tablainformacioinak osszevadaszasa
        /// </summary>
        /// <param name="egycont">
        /// a controltipus
        /// </param>
        /// <returns>
        /// tablainformaciok listaja
        /// </returns>
        public ArrayList Tablainfok(Controltipus egycont)
        {
            ArrayList ar = new ArrayList();
            if(!egycont.Tablainfo.NemKell)
                ar.Add(egycont.Tablainfo);
            egycont.ChildControltipus.Tablainfok(ar);
            return ar;
        }
        private ArrayList Tablainfok(ArrayList ar)
        {
            foreach (Controltipus egycont in this)
            {
                if(!egycont.Tablainfo.NemKell)
                    ar.Add(egycont.Tablainfo);
                egycont.ChildControltipus.Tablainfok(ar);
            }
            return ar;
        }
        /// <summary>
        /// Van a Controltipusok kozott olyan, mely uj soron all?
        /// </summary>
        /// <returns>
        /// true: igen
        /// </returns>
        public bool Uj
        {
            get
            {
                foreach (Controltipus egycont in this)
                {
                    if (egycont.Uj)
                        return true;
                }
                return false;
            }
        }
        /// <summary>
        /// Van a gyerek Controltipusok es azok gyerekei kozott olyan, mely uj soron all?
        /// </summary>
        /// <returns></returns>
        public bool Ures
        {
            get
            {
                foreach (Controltipus egycont in this)
                {
                    if (egycont.Ures)
                        return true;
                    if (egycont.ChildControltipus.Ures)
                        return true;
                }
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool Hibas
        {
            get
            {
                foreach (Controltipus egycont in this)
                {
                    if (egycont.Hibas)
                    {
                        return true;
                    }
                    if (egycont.ChildControltipus.Hibas)
                        return true;
                }
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool LehetUres
        {
            get
            {
                foreach (Controltipus egycont in this)
                {
                    if (!egycont.Tablainfo.LehetUres)
                        return false;
                }
                return true;
            }
        }
    }
    /// <summary>
    /// Egy panel vagy egy groupbox informacioi
    /// </summary>
    public class Controltipus
    {
        /// <summary>
        /// a panel vagy null
        /// </summary>
        public Panel Panel = null;
        /// <summary>
        /// fakuserinterface
        /// </summary>
        public FakUserInterface FakUserInterface;
        /// <summary>
        /// a control TabIndexe
        /// </summary>
        public int TabIndex;
        /// <summary>
        /// tipusa
        /// </summary>
        public ControlAlap.Alapinfotipus Tipus;
        /// <summary>
        /// a gyerekek controltipus kollekcioja
        /// </summary>
        public ControltipusCollection ChildControltipus;// = new ControltipusCollection();
        /// <summary>
        /// a szulo controltipusa vagy null
        /// </summary>
        public Controltipus Parent = null;
        /// <summary>
        /// a szulolanc
        /// </summary>
        public ControltipusCollection ParentChain;// = new ControltipusCollection();
        /// <summary>
        /// az adattablainformacio
        /// </summary>
        public Tablainfo Tablainfo = null;
        /// <summary>
        /// a panelen beluli gridgroupbox vagy null
        /// </summary>
        public GroupBox GridGroupBox = null;
        /// <summary>
        /// inputgroupbox vagy null
        /// </summary>
        public GroupBox InputGroupBox = null;
        /// <summary>
        /// a toolstrip
        /// </summary>
        public ToolStrip ToolStrip = null;
        /// <summary>
        /// gridview vagy null
        /// </summary>
        public DataGridView DataGridView = null;
        /// <summary>
        /// buttonok listaja
        /// </summary>
        public ArrayList ButtonokList = new ArrayList();
        /// <summary>
        /// buttonnevek listaja
        /// </summary>
        public ArrayList ButtonNevekList = new ArrayList();
        /// <summary>
        /// buttonok tombje
        /// </summary>
        public ToolStripButton[] Buttonok
        {
            get
            {
                return (ToolStripButton[])ButtonokList.ToArray(typeof(ToolStripButton));
            }
        }
        /// <summary>
        /// buttonnevek tombje
        /// </summary>
        public string[] ButtonNevek
        {
            get
            {
                return (string[])ButtonNevekList.ToArray(typeof(string));
            }
        }
        /// <summary>
        /// A hivo
        /// </summary>
        public ControlAlap Hivo;
        /// <summary>
        /// Input mezok informacioi
        /// </summary>
        public MezoControlInfo MezoControlInfo = null;
        /// <summary>
        /// hozzaferesi jogosultsag
        /// </summary>
        public Base.HozferJogosultsag HozferJog;
        /// <summary>
        /// DATUMTOL nevu column indexe
        /// </summary>
        public int DatumtolColumnIndex;
        /// <summary>
        /// Aktualis identity
        /// </summary>
        public long Aktid;
        /// <summary>
        /// Ok-t nyomtak?
        /// </summary>
        public bool OkVolt = false;
        /// <summary>
        /// Uj-at nyomtak?
        /// </summary>
        public bool UjVolt = false;
        private bool uj = false;
        /// <summary>
        /// Uj a sor?
        /// </summary>
        public bool Uj
        {
            get
            {
                uj = false;
                    if (Aktid == -1 && !Tablainfo.NemKell && (!Tablainfo.LehetUres || Tablainfo.LehetUres && !Tablainfo.Ures))
                        uj = true;
                return uj;
            }
        }
        private bool ures = false;
        /// <summary>
        /// Ures a tabla
        /// </summary>
        public bool Ures
        {
            get
            {
                ures = false;
                if (Tablainfo.NemKell || Tablainfo.LehetUres && Tablainfo.Ures)
                    ures = true;
                else if (Tablainfo.ViewSorindex != -1)
                    ures = false;
                else if (!OkVolt)
                    ures = true;
                return ures;
            }
        }
        private bool valtozott = false;
        /// <summary>
        /// valtozott-e az adattabla vagy valamelyik gyereke
        /// </summary>
        public bool Valtozott
        {
            get
            {
                valtozott = HozferJog == Base.HozferJogosultsag.Irolvas && !Tablainfo.NemKell && (Tablainfo.ModositasiHiba || Tablainfo.Changed || Tablainfo.Modositott || UjVolt || Uj);
                if (!valtozott && ChildControltipus.Count != 0)
                    valtozott = ChildControltipus.Valtozott;
                return valtozott;
            }
        }
        private bool hibas = false;
        /// <summary>
        /// Hibas?
        /// </summary>
        public bool Hibas
        {
            get
            {
                hibas=!Tablainfo.NemKell && Tablainfo.ModositasiHiba;
                if (!hibas)
                {
                    foreach (Controltipus egycont in ChildControltipus)
                    {
                        if (egycont.Hibas)
                            hibas = true;
                    }
                }
                return hibas;
            }
        }
        private bool parentvaltozott = false;
        /// <summary>
        /// Valtozott a szulo?
        /// </summary>
        public bool ParentValtozott
        {
            get
            {
                parentvaltozott = false;
                if(Parent!=null && ParentChain.Valtozott && !Tablainfo.NemKell)
                    parentvaltozott = true;
                return parentvaltozott;
            }
        }
        private string TipusString;
        /// <summary>
        /// User beallithat szurot, ha akar
        /// </summary>
        public string UserFilter = "";
       /// <summary>
       /// objectum letrehozasa
       /// </summary>
       /// <param name="tipus">
       /// objectum tipusa
       /// </param>
       /// <param name="cont">
       /// a panel vagy groupbox
       /// </param>
       /// <param name="hivo">
       /// hivo UserControl
       /// </param>
        public Controltipus(ControlAlap.Alapinfotipus tipus, Control cont, ControlAlap hivo)
        {
            ChildControltipus=new ControltipusCollection(hivo);
            ParentChain = new ControltipusCollection(hivo);
            Tipus = tipus;
            Hivo = hivo;
            TipusString = Hivo.Alapinfostring[(int)tipus];
            FakUserInterface = hivo.FakUserInterface;
            Controltipus egygyerek;
            Panel panel;
            switch (tipus)
            {
                case ControlAlap.Alapinfotipus.Alap:
                    Panel = (Panel)cont;
                    TabIndex = Panel.TabIndex;
                    PanelControlokErtekel(Panel);
                    Hivo.ControltipusCollection.Add(TabIndex, this);
                    break;
                case ControlAlap.Alapinfotipus.CsakDataGrid:
                    Panel = (Panel)cont;
                    TabIndex = Panel.TabIndex;
                    PanelControlokErtekel(Panel);
                    Hivo.ControltipusCollection.Add(TabIndex, this);
                    break;
                case ControlAlap.Alapinfotipus.Valtozasok:
                    Panel = (Panel)cont;
                    TabIndex = Panel.TabIndex;
                    PanelControlokErtekel(Panel);
                    Hivo.ControltipusCollection.Add(TabIndex, this);
                    break;
                case ControlAlap.Alapinfotipus.Szulo:
                    panel = (Panel)cont;
                    Panel = (Panel)panel.Controls[0];
                    TabIndex = Panel.TabIndex;
                    PanelControlokErtekel(Panel);
                    Hivo.ControltipusCollection.Add(TabIndex, this);
                    panel = (Panel)panel.Controls[1];
                    egygyerek = new Controltipus(ControlAlap.Alapinfotipus.Child, panel, Hivo);
                    egygyerek.Parent = this;
//                    ChildControltipus.Add(egygyerek.TabIndex, egygyerek);
                    break;
                case ControlAlap.Alapinfotipus.SzuloGyerekValtozasok:
                    panel = (Panel)cont;
                    Panel = (Panel)panel.Controls[0];
                    TabIndex = Panel.TabIndex;
                    PanelControlokErtekel(Panel);
                    Hivo.ControltipusCollection.Add(TabIndex, this);
                    if (panel.Controls.Count > 1)
                    {
                        try
                        {
                            panel = (Panel)panel.Controls[1];
                            foreach (Panel egypanel in panel.Controls)
                                new Controltipus(ControlAlap.Alapinfotipus.Child, egypanel, Hivo);
                        }
                        catch
                        {
                        }
                    }
                    break;
                case ControlAlap.Alapinfotipus.Tobbgyerek:
                    panel = (Panel)cont;
                    Panel = (Panel)panel.Controls[0];
                    TabIndex = Panel.TabIndex;
                    PanelControlokErtekel(Panel);
                    Hivo.ControltipusCollection.Add(TabIndex, this);
                    foreach (Panel egypanel in panel.Controls)
                    {
                        egygyerek = new Controltipus(ControlAlap.Alapinfotipus.Child, egypanel, Hivo);
                        egygyerek.Parent = this;
//                        ChildControltipus.Add(egygyerek.TabIndex, egygyerek);
                    }
                    break;
                case ControlAlap.Alapinfotipus.Child:
                    Panel = (Panel)cont;
                    TabIndex = Panel.TabIndex;
                    PanelControlokErtekel(Panel);
                    Hivo.ControltipusCollection.Add(TabIndex, this);
                    break;
            }
        }
        /// <summary>
        /// Tablainformacio es ahhoz valo hozzaferesi jogosultsag beallitasa
        /// ha a jogosultsag semmi, a panel vagy a groupbox lathatatlan
        /// </summary>
        /// <param name="tabinfo">
        /// a tablainformacio
        /// </param>
        /// <param name="hozferjog">
        /// hozzaferesi jogosultsag
        /// </param>
        public void SetTablainfo(ref Tablainfo tabinfo, Base.HozferJogosultsag hozferjog)
        {
            Tablainfo = tabinfo;
            HozferJog = hozferjog;
            if (Panel != null)
                Panel.Visible = !Tablainfo.NemKell;
            else if(InputGroupBox!=null)
                InputGroupBox.Visible = !Tablainfo.NemKell;
            if (HozferJog == Base.HozferJogosultsag.Semmi)
            {
                if (Panel != null)
                    Panel.Visible = false;
                else if(InputGroupBox!=null)
                    InputGroupBox.Visible = false;
            }
            else if (Hivo.LezartCeg)
                HozferJog = Base.HozferJogosultsag.Csakolvas;
            Tablainfo.HozferJog = hozferjog;
            DatumtolColumnIndex = Tablainfo.DatumtolColumnIndex;
        }
        private void PanelControlokErtekel(Panel panel)
        {
            panel.Enter += Panel_Enter;
            panel.Leave += Panel_Leave;
            Control egycont;
            for (int i = 0; i < panel.Controls.Count; i++)
            {
                egycont = panel.Controls[i];
                if (egycont.Name.Contains("Strip"))
                    ButtonokAllit(egycont);
                else if (egycont.Name.Contains("groupBox"))
                {
                    GroupBox groupBox = (GroupBox)egycont;
                    if (groupBox.Controls.Count != 0)
                    {
                        try
                        {
                            DataGridView = (DataGridView)groupBox.Controls[0];
                            DataGridView.Scroll += Hivo.GridView_Scroll;
                            DataGridView.CellMouseClick += Hivo.GridView_CellMouseClick;
                            GridGroupBox = groupBox;
                        }
                        catch
                        {
                            InputGroupBox = groupBox;
                            InputGroupBox.Enter += GroupBox_Enter;
                            InputGroupBox.Leave += GroupBox_Leave;
                        }
                    }
                }
            }
            string parentpanelnev = "";
            if (panel.Tag != null)
                parentpanelnev = panel.Tag.ToString();
            if (parentpanelnev != "")
            {
                Controltipus parent = Hivo.ControltipusCollection.Find(parentpanelnev);
                if (parent != null)
                {
                    this.Parent = parent;
                    this.ParentChain.AddRange(parent.ParentChain);
                    this.ParentChain.Add(parent);
                    parent.ChildControltipus.Add(this);
                }
            }
        }
        private void ButtonokAllit(Control strip)
        {
            ToolStrip = (ToolStrip)strip;
            string butname;
            foreach( ToolStripButton egybut in ToolStrip.Items)
            {
                butname = egybut.Name;
                if (butname.Contains("uj"))
                    butname = "uj";
                else if (butname.Contains("elozo"))
                    butname = "elozo";
                else if (butname.Contains("kovetkezo"))
                    butname = "kovetkezo";
                else if (butname.Contains("rogzit"))
                    butname = "rogzit";
                else if (butname.Contains("torol"))
                    butname = "torol";
                else if (butname.Contains("ok"))
                    butname = "ok";
                else if (butname.Contains("elolrol"))
                    butname = "elolrol";
                egybut.Click += Hivo.Button_Click;
                ButtonokList.Add(egybut);
                ButtonNevekList.Add(butname);
            }
        }
        /// <summary>
        /// ok-enabled allitasa
        /// </summary>
        /// <param name="ertek">
        /// true: enabled
        /// </param>
        public void SetOk(bool ertek)
        {
            int i = ButtonNevekList.IndexOf("ok");
            if(i!=-1)
              Buttonok[i].Enabled = ertek;
        }
        /// <summary>
        /// Az osszes button visible ill enabled ertekeinek beallitasa
        /// </summary>
        public void ButtonokEnableAllit()
        {
            if (Tablainfo.NemKell)
            {
                if (InputGroupBox != null)
                    InputGroupBox.Enabled = false;
            }
            if (Hivo.HozferJog == Base.HozferJogosultsag.Irolvas)
            {
                if (Hivo.ControltipusCollection.Valtozott)
                    Hivo.teljesrogzit.Visible = true;
                else
                    Hivo.teljesrogzit.Visible = false;
            }
            else
                Hivo.teljesrogzit.Visible = false;
            Hivo.teljesrogzit.Enabled = true;
//            if (Hivo.ControltipusCollection.Count > 1 && Hivo.ControltipusCollection.Uj)
                if (Hivo.ControltipusCollection.Count > 1 && Parent!=null && Parent.Uj)
                    Hivo.teljesrogzit.Enabled = false;
            else
                Hivo.teljesrogzit.Enabled = Tablainfo.ViewSorindex != -1 && !Tablainfo.ModositasiHiba;
            if (Buttonok != null)
            {
                int viewsorindex = Tablainfo.ViewSorindex;
                bool valtozott = Valtozott;
                for (int i = 0; i < Buttonok.Length; i++)
                {
                    ToolStripButton egybut = Buttonok[i];
                    string butnev = ButtonNevek[i];
                    egybut.Enabled = false;
                    switch (butnev)
                    {
                        case "uj":
                            if (HozferJog != Base.HozferJogosultsag.Irolvas || Ures || InputGroupBox != null && !InputGroupBox.Enabled || ChildControltipus.Count != 0 && DatumtolColumnIndex != -1 && Tablainfo.ViewSorindex != Tablainfo.DataView.Count - 1)
                            {
     //                           egybut.Visible = false;
                            }
                            else
                            {
                                //                               if (Uj || valtozott && !ParentChain.Uj) 
                                if (viewsorindex == -1 || valtozott && !ParentChain.Uj && ChildControltipus.Uj&&!ChildControltipus.LehetUres)
                                    //                               egybut.Visible = false;
                                    egybut.Enabled = false;
                                else
                                {
    //                                egybut.Visible = true;
                                    egybut.Enabled = true;
                                }
                            }
                            break;
                        case "elozo":
                            if (viewsorindex > 0 && (!ChildControltipus.Valtozott|| ChildControltipus.Ures && ChildControltipus.LehetUres) && (!Uj || Parent!=null))
                                egybut.Enabled = true;
                            break;
                        case "kovetkezo":
                            if ((!Uj || Parent != null)&& (!ChildControltipus.Valtozott || ChildControltipus.Ures && ChildControltipus.LehetUres) && viewsorindex != -1 && viewsorindex != Tablainfo.DataView.Count - 1)
                                egybut.Enabled = true;
                            break;
                        case "torol":
                            if (HozferJog == Base.HozferJogosultsag.Irolvas)
                            {
                                if (DatumtolColumnIndex != -1)
                                {
                                    if (Tablainfo.DataView.Count > 1 && viewsorindex == Tablainfo.DataView.Count - 1)
                                        egybut.Enabled = true;
                                }
                                else if (InputGroupBox != null && !InputGroupBox.Enabled)
                                {
                                }
                                else if (Tablainfo.DataView.Count != 0 && viewsorindex != -1)
                                    egybut.Enabled = true;
                            }
                            break;
                        case "rogzit":
                            if (HozferJog != Base.HozferJogosultsag.Irolvas)
                                egybut.Visible = false;
                            else
                            {
                                if (Tablainfo.NemKell)
                                    egybut.Visible = false;
                                else
                                {
                                    if (Parent == null)// && ChildControltipus.Count == 0)
                                    {
                                        if (ChildControltipus.Count == 0)
                                        {
                                            if (!Valtozott || !OkVolt)
                                                egybut.Visible = false;
                                            else
                                            {
                                                egybut.Visible = true;
                                                egybut.Enabled = true;
                                            }
                                        }
                                        else
                                        {
                                            if(Tablainfo.ViewSorindex==-1)
                                            {
                                                egybut.Visible=false;
                                                break;
                                            }
                                            egybut.Visible=false;
                                            bool ok = true;
                                            bool valt = false;
                                            foreach(Controltipus egycont in ChildControltipus)
                                            {
                                                Tablainfo egyinfo = egycont.Tablainfo;
                                                if(egyinfo.ViewSorindex==-1 && !egyinfo.LehetUres)
                                                    ok =false;
                                                else if(egycont.Valtozott)
                                                    valt=true;
                                            }
                                            if (ok)
                                                ok = valt;
                                            else
                                            {
                                                egybut.Visible=false;
                                                egybut.Enabled=false;
                                                break;
                                            }
                                            if (ok || Valtozott && OkVolt)
                                            {
                                                egybut.Visible = true;
                                                egybut.Enabled = true;
                                            }
                                        }
                                   }

                                    else if (UjVolt || ParentChain.Uj || ChildControltipus.Ures || !Valtozott && !ChildControltipus.Valtozott || Hibas || !Valtozott && !ParentValtozott)
                                        egybut.Visible = false;
                                    else if(OkVolt)
                                    {
                                        egybut.Visible = true;
                                        egybut.Enabled = true;
                                    }
                                }
                            }
                            break;
                        case "elolrol":
                            if (Valtozott ) 
                                egybut.Enabled = true;
                            break;
                        case "ok":
                            egybut.Visible = true;
                            egybut.Enabled = false;
                            if (HozferJog != Base.HozferJogosultsag.Irolvas)//(DatumtolColumnIndex!=-1 || Parent!=null && Parent.DatumtolColumnIndex!=-1)
                                egybut.Visible = false;
                            else if (OkVolt)
                            {
                                if (Tablainfo.Valtozott && Parent != null)
                                {
                                    if (Parent.InputGroupBox != null)
                                        Parent.InputGroupBox.Enabled = false;
                                }
                                //                                egybut.Enabled = false;
 //                               OkVolt = false;
                            }
                            else if (Parent == null)
                            {
                                if(Valtozott)
                                    egybut.Enabled = true;
                                else if(ChildControltipus.Count!=0)
                                {
                                    if(!Tablainfo.LehetUres&&(Tablainfo.DatumigColumnIndex == -1 || Tablainfo.DatumigColumnIndex!=-1 && Tablainfo.ViewSorindex==Tablainfo.DataView.Count-1))
                                        egybut.Enabled=true;
                                }
                            }
                            else if(Valtozott && !Tablainfo.Ures || ChildControltipus.Valtozott && !ChildControltipus.Ures) //!Tablainfo.Ures) // && Parent == null && Tablainfo.ViewSorindex == Tablainfo.DataView.Count - 1 && Tablainfo.DatumigColumnIndex != -1)
                                egybut.Enabled = true;
                            //else if (!Tablainfo.NemKell || !Tablainfo.Ures)
                            //{
                            //    if (InputGroupBox != null)
                            //    {
                            //        if (ChildControltipus.Count == 0 && Parent == null && Valtozott)
                            //            egybut.Enabled = true;
                            //        else if (ChildControltipus.Count == 0)
                            //        {
                            //            if (Valtozott) // || Parent != null && )
                            //                egybut.Enabled = true;
                            //        }
                            //        else if (ChildControltipus.Count != 0)
                            //            egybut.Enabled = true;
                            //    }
                            //}
                            break;
                        default:
                            egybut.Enabled = true;
                            break;
                    }
                }
                //if (Hivo.HozferJog != Base.HozferJogosultsag.Irolvas)
                //{
                //    for (int i = 0; i < Buttonok.Length; i++)
                //    {
                //        ToolStripButton egybut = Buttonok[i];
                //        string butnev = ButtonNevek[i];
                //        switch (butnev)
                //        {
                //            case "uj":
                //                egybut.Enabled = false;
                //                break;
                //            case "torol":
                //                egybut.Enabled = false;
                //                break;
                //            case "rogzit":
                //                egybut.Enabled = false;
                //                break;
                //        }
                //    }
                //}
                //if (InputGroupBox != null && InputGroupBox.Enabled)
                //{
                //    InputGroupBox.Focus();
                //}
                OkVolt = false;
//                SetFocus();
            }
        }
        /// <summary>
        /// Adatsor toltes, ha a tabla tartalma valtozott vagy uj sorrol lenne szo es a tabla nem ures vagy
        /// nem lehet ures
        /// </summary>
        public void Adatsortolt()
        {
            if(Valtozott && (!Ures || ChildControltipus !=null && !ChildControltipus.Ures))
            {
                string savfilt = Tablainfo.DataView.RowFilter;
                SetRowFilter();
                Tablainfo.Adatsortolt();
                Tablainfo.DataView.RowFilter = savfilt;
            }
        }
        /// <summary>
        /// ok button megnyomasa utan, ha nem volt hiba
        /// </summary>
        public void Ok()
        {
            OkVolt = true;
            Adatsortolt();
            UjVolt = false;
            if (Valtozott)
                Hivo.EgyediBeallit(this);
            if (Parent != null && Parent.InputGroupBox != null && !Valtozott)
            {
                Parent.InputGroupBox.Enabled = true;
                if (InputGroupBox != null)
                    InputGroupBox.Enabled = false;
            }
 //           Panel.Enabled = false;
            bool voltfocus = false;
            if (ChildControltipus.Count != 0)
            {
                foreach (Controltipus egycont in ChildControltipus)
                {
                    if (Valtozott)
                        Hivo.Controloktolt(egycont, true, false,true);
                    else
                    {
                        egycont.Panel.Enabled = true;
                        if (egycont.InputGroupBox != null)
                        {
                            egycont.InputGroupBox.Enabled = true;
                            if (!voltfocus)
                            {
                                voltfocus = true;
                                egycont.SetFocus();
                            }

                            //                       egycont.InputGroupBox.Focus();
                            //                   }
                        }
                    }
                }
            }
            if (DataGridView != null)
                SetDataGridViewRow(Tablainfo.ViewSorindex);
        }
        private void Panel_Enter(object sender, EventArgs e)
        {
            {
                Panel panel = (Panel)sender;
                panel.BackColor = FakUserInterface.AktivControlBackColor;
            }
        }
        private void Panel_Leave(object sender, EventArgs e)
        {
            {
                Panel panel = (Panel)sender;
                panel.BackColor = FakUserInterface.InaktivControlBackColor;
            }
        }
        private void GroupBox_Enter(object sender, EventArgs e)
        {
            {
                GroupBox gr = (GroupBox)sender;
                gr.BackColor = FakUserInterface.AktivControlBackColor;
                Hivo.GroupBox_Enter(gr);
            }
        }
        private void GroupBox_Leave(object sender, EventArgs e)
        {
            {
                GroupBox gr = (GroupBox)sender;
                gr.BackColor = FakUserInterface.InaktivControlBackColor;
                Hivo.GroupBox_Leave(gr);
            }
        }
        /// <summary>
        /// Probal focusalni
        /// </summary>
        public void SetFocus()
        {
            bool saveevent = FakUserInterface.EventTilt;
            FakUserInterface.EventTilt = true;
            if (InputGroupBox != null && InputGroupBox.Enabled)
            {
                {
                    InputGroupBox.Focus();
                    for (int i = 0; i < MezoControlInfo.Inputeleminfok.Length; i++)
                    {
                        MezoTag egytag = MezoControlInfo.Inputeleminfok[i];
                        if (egytag.Control.Enabled)
                        {
                            egytag.Control.Focus();
                            break;
                        }
                    }
                }
            }
            else if (ChildControltipus.Count != 0)
            {
                bool vanfocus = false;
                for (int i = 0; i < ChildControltipus.Count; i++)
                {
                    Controltipus egycont = ChildControltipus[i];
                    if (InputGroupBox != null && InputGroupBox.Enabled)
                    {
                        {
                            InputGroupBox.Focus();
                            for (int j = 0; j < MezoControlInfo.Inputeleminfok.Length; j++)
                            {
                                MezoTag egytag = MezoControlInfo.Inputeleminfok[j];
                                if (egytag.Control.Enabled)
                                {
                                    egytag.Control.Focus();
                                    vanfocus = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (vanfocus)
                        break;
                }
            }
            FakUserInterface.EventTilt = saveevent;
        }
        /// <summary>
        /// Mezok tartalmanak toltese, ha kell , gyerekeket ne toltse
        /// </summary>
        /// <returns>
        /// true: ha valtozas volt
        /// </returns>
        public bool Controloktolt()
        {
            return Controloktolt(true, false,false);
        }
        /// <summary>
        /// Mezok tartalmanak toltese
        /// </summary>
        /// <param name="force">
        /// true: mindenkeppen toltse
        /// false: csak, ha kell
        /// </param>
        /// <param name="kellchild">
        /// true: gyerekeket is
        /// false: gyerekeket nem
        /// </param>
        /// <param name="kellfocus">
        /// ???
        /// </param>
        /// <returns>
        /// true: ha valtozas volt
        /// </returns>
        public bool Controloktolt(bool force, bool kellchild,bool kellfocus)
        {
            bool changed = false;
            Base.HozferJogosultsag hozferjog = Tablainfo.Azonositok.Jogszintek[(int)Hivo.KezeloiSzint];
            Controltipus egytip = Hivo.ControltipusCollection[this];
            if(Hivo.HozferJog==Base.HozferJogosultsag.Csakolvas && hozferjog==Base.HozferJogosultsag.Irolvas)
            {
                hozferjog = Hivo.HozferJog;
            }
            egytip.SetTablainfo(ref Tablainfo, hozferjog);
            Tablainfo.AktualControlInfo = MezoControlInfo;
            ArrayList valtoztak = Hivo.ValtozasLekerdez();
            bool adatvaltozas = valtoztak.IndexOf("Rogzitett") != -1;
            bool nodevaltozas = valtoztak.IndexOf("NodeValtozas") != -1;
            bool cegvaltozas = valtoztak.IndexOf("CegValtozas") != -1;
            bool datumvaltozas = valtoztak.IndexOf("Datumvaltozas") != -1;
            if (adatvaltozas && !Hivo.tabstopboljott)
                Tablainfo.Valtozott = false;
            else
                changed = Valtozott;
            if (nodevaltozas || cegvaltozas || datumvaltozas)
            {
                Tablainfo.DataView.RowFilter = "";
                if (Tablainfo.DataView.Count == 0)
                    Tablainfo.ViewSorindex = -1;
                else if (DatumtolColumnIndex == -1)
                    Tablainfo.ViewSorindex = 0;
                else
                    Tablainfo.ViewSorindex = Tablainfo.DataView.Count - 1;
            }
            SetRowFilter();
            if (!changed || adatvaltozas || nodevaltozas || cegvaltozas || datumvaltozas || force) // || Hivo.Modositott)
            {
                if (Panel != null)
                    Panel.Enabled = true;
                if (InputGroupBox != null)
                    InputGroupBox.Enabled = true;
                if (Tablainfo.DataView.Count == 0) //|| !TipusString.Contains("Szulo"))
                    Tablainfo.ViewSorindex = -1;
                else if (!Uj)
                {
                    if (Tablainfo.ViewSorindex == -1)
                    {
                        int viewind;
                        if (DatumtolColumnIndex == -1)
                            viewind = 0;
                        else
                            viewind = Tablainfo.DataView.Count - 1;
                        if (viewind != Tablainfo.ViewSorindex)
                            Tablainfo.ViewSorindex = viewind;
                        else
                            Tablainfo.ViewSorindex = Tablainfo.ViewSorindex;
                    }
                }
                else if(!Tablainfo.LehetUres)
                    Tablainfo.Modositott = true;
                //if (Tablainfo.ViewSorindex == -1 && !Hivo.ujcontroloktoltboljott)
                //{
                //    //if (!Tablainfo.LehetUres ) 
                //    //    Tablainfo.Changed = true;
                //}
                UjVolt = false;
                Hivo.EgyediBeallit(this);
                if (kellchild && ChildControltipus.Count != 0)
                {
                    ChildControltipus.Controloktolt(force, kellchild,kellfocus);
                    foreach (Controltipus egycont in ChildControltipus)
                        egycont.Panel.Enabled = false;
                }
            }
            else
            {
                if (Tablainfo.ViewSorindex > Tablainfo.DataView.Count - 1)
                    Tablainfo.ViewSorindex = Tablainfo.DataView.Count - 1;
                else
                    Tablainfo.ViewSorindex = Tablainfo.ViewSorindex;
                Hivo.EgyediBeallit(this);
            }
            if (DataGridView != null)
                SetDataGridViewRow(Tablainfo.ViewSorindex);
            if(kellfocus)
                SetFocus();
            return changed;
        }
        private void SetRowFilter()
        {
            string aktid;
            string parentidnev;
            string datumresz = "";
            if (!Hivo.tabstopboljott && !Hivo.ujcontroloktoltboljott)
            {
                if (Uj && Tablainfo.Valtozott) 
                {
                    Aktid = -1;
                    return;
                }
            }
            string savfilt = Tablainfo.DataView.RowFilter;
            int rowindex = Tablainfo.ViewSorindex;
            Tablainfo.DataView.RowFilter = "";
            if(Parent==null)
                Tablainfo.DataView.RowFilter=UserFilter;
            if (Parent != null)
            {
                Tablainfo parenttabinfo = Parent.Tablainfo;
                parentidnev = parenttabinfo.IdentityColumnName;
                Parent.Aktid = Parent.Tablainfo.AktIdentity;
                aktid = Parent.Aktid.ToString();
                if (Parent.DatumtolColumnIndex != -1 && DatumtolColumnIndex != -1)
                {
                    string parenttol = parenttabinfo.TablaColumns["DATUMTOL"].Tartalom;
                    string parentig = parenttabinfo.TablaColumns["DATUMIG"].Tartalom;
                    if (rowindex == -1)
                    {
                        MezoTag egytag = MezoControlInfo.InputelemArray["DATUMTOL"];
                        if (egytag != null)
                            egytag.SetValue(parenttol);
                        else
                            Tablainfo.TablaColumns["DATUMTOL"].Tartalom = parenttol;
                        egytag = MezoControlInfo.InputelemArray["DATUMIG"];
                        if (egytag != null)
                            egytag.SetValue(parentig);
                        else
                            Tablainfo.TablaColumns["DATUMIG"].Tartalom = parentig;
                    }
                    if (parenttol == "")
                        datumresz = ") AND DATUMTOL IS NULL";
                    else
                        datumresz = ") AND (DATUMTOL IS NULL OR DATUMTOL = '" + parenttol + "')";
                }
                string filter = "";
                if (datumresz != "")
                    filter = "(";
                filter += parentidnev + " = " + aktid + " OR " + parentidnev + " IS NULL OR " + parentidnev + " = 0";
                if (datumresz != "")
                    filter += datumresz;
                Tablainfo.DataView.RowFilter = filter;
            }
            if (Tablainfo.DataView.RowFilter != savfilt)
            {
                if (Tablainfo.ViewSorindex == -1 && Tablainfo.DataView.Count != 0)
                {
                    if (Tablainfo.DatumtolColumnIndex == 0)
                        Tablainfo.ViewSorindex = 0;
                    else
                        Tablainfo.ViewSorindex = Tablainfo.DataView.Count - 1;
                }
                else if (Tablainfo.ViewSorindex < Tablainfo.DataView.Count)
                    Tablainfo.ViewSorindex = Tablainfo.ViewSorindex;
                else
                {
                    if (Tablainfo.DatumtolColumnIndex == 0)
                        Tablainfo.ViewSorindex = 0;
                    else
                        Tablainfo.ViewSorindex = Tablainfo.DataView.Count - 1;
                }
            }
        
            Aktid = Tablainfo.AktIdentity;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowindex"></param>
        /// <param name="kellchild"></param>
        public void Beallit(int rowindex, bool kellchild)
        {
            Beallit(rowindex, kellchild, false);
        }
        /// <summary>
        /// Beallitja az adattabla view kivant sorat
        /// </summary>
        /// <param name="rowindex">
        /// a view kivant sorindexe
        /// </param>
        /// <param name="kellchild">
        /// true: az osszes gyereket is
        /// </param>
        /// ???
        /// <param name="kellfocus">
        /// </param>
        public void Beallit(int rowindex, bool kellchild ,bool kellfocus)
        {
            bool eventtilt = FakUserInterface.EventTilt;
            FakUserInterface.EventTilt = true;
            //if (InputGroupBox != null)
            //{
            //    InputGroupBox.Enabled = true;
            //    int datumtolind = DatumtolColumnIndex;
            //    if (rowindex != -1 && datumtolind != -1 && MezoControlInfo.InputelemArray["DATUMTOL"] != null && rowindex != Tablainfo.DataView.Count - 1)
            //        InputGroupBox.Enabled = false;
            //    else if (Parent != null && Parent.DatumtolColumnIndex != -1)
            //    {
            //        Tablainfo tabinfo = Parent.Tablainfo;
            //        if (tabinfo.ViewSorindex != -1 && tabinfo.ViewSorindex != tabinfo.DataView.Count - 1)
            //            InputGroupBox.Enabled = false;
            //    }
            //    else
            //        InputGroupBox.Enabled = true;
            //}
            if (rowindex == -1 && !Tablainfo.LehetUres)
                Tablainfo.Changed = true;
            SetRowFilter();
            if (rowindex != -1 && Tablainfo.DataView.Count - 1 < rowindex)
                rowindex = Tablainfo.DataView.Count - 1;
            Tablainfo.ViewSorindex = rowindex;
            if (InputGroupBox != null)
            {
                InputGroupBox.Enabled = true;
                int datumtolind = DatumtolColumnIndex;
                if (rowindex != -1 && datumtolind != -1 && MezoControlInfo.InputelemArray["DATUMTOL"] != null && rowindex != Tablainfo.DataView.Count - 1)
                    InputGroupBox.Enabled = false;
                else if (Parent != null && Parent.DatumtolColumnIndex != -1)
                {
                    Tablainfo tabinfo = Parent.Tablainfo;
                    if (tabinfo.ViewSorindex != -1 && tabinfo.ViewSorindex != tabinfo.DataView.Count - 1)
                        InputGroupBox.Enabled = false;
                }
                else
                    InputGroupBox.Enabled = true;
            }
            Aktid = Tablainfo.AktIdentity;
            if (UjVolt)
                Tablainfo.Tartalmaktolt();
            if (DataGridView != null)
                SetDataGridViewRow(rowindex);
            if (kellchild && ChildControltipus.Count != 0)
            {
                Hivo.EgyediBeallit(this);
                for (int i = 0; i < ChildControltipus.Count; i++)
                {
                    int rowind = 0;
                    if (ChildControltipus[i].DatumtolColumnIndex != -1)
                        rowind = ChildControltipus[i].Tablainfo.DataView.Count - 1;
                    Hivo.Beallit(ChildControltipus[i], rowind, kellchild, false);
                }
            }
            else
                Hivo.EgyediBeallit(this);
            FakUserInterface.EventTilt = eventtilt;
            if (Parent != null && InputGroupBox != null && Parent.UjVolt)
                InputGroupBox.Enabled = false;
            if(kellfocus)
                SetFocus();
        }
        /// <summary>
        /// elveszitheto-e az adattabla valtozas
        /// </summary>
        /// <returns>
        /// igen, ha nem valtozott, vagy beleegyezunk
        /// </returns>
        public bool Elveszithet()
        {
            return Hivo.Elveszithet(Tablainfo);
        }
        /// <summary>
        /// elolrol
        /// </summary>
        public void Elolrol()
        {
            Hivo.Elolrol(this);
        }
        /// <summary>
        /// datagridview adott soranak select es visible beallitasa 
        /// </summary>
        /// <param name="rowindex">
        /// a sor
        /// </param>
        public void SetDataGridViewRow(int rowindex)
        {
            if (DataGridView.Rows.Count > 1)
            {
                int selectedindex = -1;
                if (DataGridView.SelectedRows.Count != 0)
                    selectedindex = DataGridView.SelectedRows[0].Index;
 //                   DataGridView.SelectedRows[0].Selected = false;
                if (rowindex != -1)
                {
                    if (selectedindex != rowindex)
                    {
                        if(DataGridView.SelectedRows.Count!=0)
                            DataGridView.SelectedRows[0].Selected = false;
                        DataGridView.Rows[rowindex].Selected = true;
                        selectedindex = rowindex;
                        Hivo.SetAktRowVisible(DataGridView, Tablainfo);
                    }
                    //int rowcount = DataGridView.DisplayedRowCount(false);
                    //if (rowcount < rowindex)
                    //{
                    //    DataGridView.FirstDisplayedScrollingRowIndex = rowindex;
                    //}
                    ////int firstdisprowindex = DataGridView.FirstDisplayedScrollingRowIndex;
                    //if (firstdisprowindex != -1 && Tablainfo.ViewSorindex != -1)
                    //{
                    //    if (firstdisprowindex > Tablainfo.ViewSorindex)
                    //        DataGridView.FirstDisplayedScrollingRowIndex = Tablainfo.ViewSorindex;
                    //    else
                    //    {
                    //        int rowcount = DataGridView.DisplayedRowCount(false);
                    //        int teljesrowcount = DataGridView.DisplayedRowCount(true);
                    //        int lastvisible = rowcount + firstdisprowindex;
                    //        int teljeslastvisible = teljesrowcount + firstdisprowindex;
                    //        if (lastvisible == Tablainfo.ViewSorindex)
                    //        {
                    //            if (DataGridView.FirstDisplayedScrollingRowIndex < DataGridView.Rows.Count - 1)
                    //                DataGridView.FirstDisplayedScrollingRowIndex++;
                    //        }
                    //        if (teljeslastvisible == Tablainfo.ViewSorindex)
                    //        {
                    //            if (DataGridView.FirstDisplayedScrollingRowIndex < DataGridView.Rows.Count - 2)
                    //                DataGridView.FirstDisplayedScrollingRowIndex = DataGridView.FirstDisplayedScrollingRowIndex + 2;
                    //        }
                    //    }
                    //}
                }
            }
        }
    }
}
