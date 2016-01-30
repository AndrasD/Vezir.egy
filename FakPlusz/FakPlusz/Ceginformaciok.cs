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
    public class Ceginformaciok
    {
        public bool LezartCeg = false;
        public bool LezartEv = false;
        public bool KellEvzaras = false;
        public Bejelentkezo Bejel;
        public DateTime InduloDatum;
        public DateTime AktualisDatum;
        public DateTime LezarasDatuma;
        public string CegId;
        public int Cegindex;
        public string VezetoId;
        public Base.KezSzint KezeloiSzint;
        public Base.HozferJogosultsag UserJogosultsag = Base.HozferJogosultsag.Csakolvas;
        public Base.HozferJogosultsag CegSzarmazekosJogosultsag = Base.HozferJogosultsag.Csakolvas;
        public Base.HozferJogosultsag CegTermeszetesJogosultsag = Base.HozferJogosultsag.Csakolvas;
        public Base.HozferJogosultsag RendszerJogosultsag = Base.HozferJogosultsag.Csakolvas;
        public string CegNev;
        public string CegConnection;
        public DataRow UserLogsor;
        public Ceginformaciok()
        {
        }
        public virtual void Jogosultsagok()
        {
            if (Bejel.NincsKezelo)
            {
                UserJogosultsag = Base.HozferJogosultsag.Irolvas;
                CegSzarmazekosJogosultsag = Base.HozferJogosultsag.Irolvas;
                CegTermeszetesJogosultsag = Base.HozferJogosultsag.Irolvas;
                return;
            }
            switch (KezeloiSzint)
            {
                case Base.KezSzint.Fejleszto:
                    UserJogosultsag = Base.HozferJogosultsag.Irolvas;
                    CegSzarmazekosJogosultsag = Base.HozferJogosultsag.Irolvas;
                    CegTermeszetesJogosultsag = Base.HozferJogosultsag.Irolvas;
                    break;
                case Base.KezSzint.Minden:
                    UserJogosultsag = Base.HozferJogosultsag.Irolvas;
                    if (!LezartCeg)
                    {
                        CegTermeszetesJogosultsag = Base.HozferJogosultsag.Irolvas;
                        CegSzarmazekosJogosultsag = Base.HozferJogosultsag.Irolvas;
                    }
                    else
                    {
                        CegTermeszetesJogosultsag = Base.HozferJogosultsag.Csakolvas;
                        CegSzarmazekosJogosultsag = Base.HozferJogosultsag.Csakolvas;
                    }
                    break;
                case Base.KezSzint.Rendszergazda:
                    UserJogosultsag = Base.HozferJogosultsag.Irolvas;
                    if (!LezartCeg)
                        CegSzarmazekosJogosultsag = Base.HozferJogosultsag.Irolvas;
                    else
                        CegSzarmazekosJogosultsag = Base.HozferJogosultsag.Csakolvas;
                    CegTermeszetesJogosultsag = Base.HozferJogosultsag.Semmi;
                    break;
                case Base.KezSzint.Rendszergazdapluszkiemelt:
                    UserJogosultsag = Base.HozferJogosultsag.Irolvas;
                    if (!LezartCeg)
                    {
                        CegTermeszetesJogosultsag = Base.HozferJogosultsag.Irolvas;
                        CegSzarmazekosJogosultsag = Base.HozferJogosultsag.Irolvas;
                    }
                    else
                    {
                        CegTermeszetesJogosultsag = Base.HozferJogosultsag.Csakolvas;
                        CegSzarmazekosJogosultsag = Base.HozferJogosultsag.Csakolvas;
                    }
                    break;
                case Base.KezSzint.Rendszergazdapluszkezelo:
                    UserJogosultsag = Base.HozferJogosultsag.Irolvas;
                    if (!LezartCeg)
                    {
                        CegTermeszetesJogosultsag = Base.HozferJogosultsag.Irolvas;
                        CegSzarmazekosJogosultsag = Base.HozferJogosultsag.Irolvas;
                    }
                    else
                    {
                        CegTermeszetesJogosultsag = Base.HozferJogosultsag.Csakolvas;
                        CegSzarmazekosJogosultsag = Base.HozferJogosultsag.Csakolvas;
                    }
                    break;
                case Base.KezSzint.Rendszergazdapluszkiemeltpluszkezelo:
                    UserJogosultsag = Base.HozferJogosultsag.Irolvas;
                    if (!LezartCeg)
                    {
                        CegTermeszetesJogosultsag = Base.HozferJogosultsag.Irolvas;
                        CegSzarmazekosJogosultsag = Base.HozferJogosultsag.Irolvas;
                    }
                    else
                    {
                        CegTermeszetesJogosultsag = Base.HozferJogosultsag.Csakolvas;
                        CegSzarmazekosJogosultsag = Base.HozferJogosultsag.Csakolvas;
                    }
                    break;

                case Base.KezSzint.Rendszergazdapluszvezeto:
                    UserJogosultsag = Base.HozferJogosultsag.Irolvas;
                    CegSzarmazekosJogosultsag = Base.HozferJogosultsag.Csakolvas;
                    CegTermeszetesJogosultsag = Base.HozferJogosultsag.Csakolvas;
                    break;
                case Base.KezSzint.Kiemeltkezelo:
                    UserJogosultsag = Base.HozferJogosultsag.Semmi;
                    CegSzarmazekosJogosultsag = Base.HozferJogosultsag.Irolvas;
                    if (!LezartCeg)
                        CegTermeszetesJogosultsag = Base.HozferJogosultsag.Irolvas;
                    else
                        CegTermeszetesJogosultsag = Base.HozferJogosultsag.Csakolvas;
                    break;
                case Base.KezSzint.Kezelo:
                    UserJogosultsag = Base.HozferJogosultsag.Semmi;
                    CegSzarmazekosJogosultsag = Base.HozferJogosultsag.Semmi;
                    if (!LezartCeg)
                        CegTermeszetesJogosultsag = Base.HozferJogosultsag.Irolvas;
                    else
                        CegTermeszetesJogosultsag = Base.HozferJogosultsag.Csakolvas;
                    break;
                case Base.KezSzint.Kiemeltkezelopluszkezelo:
                    UserJogosultsag = Base.HozferJogosultsag.Semmi;
                    CegSzarmazekosJogosultsag = Base.HozferJogosultsag.Irolvas;
                    if (!LezartCeg)
                        CegTermeszetesJogosultsag = Base.HozferJogosultsag.Irolvas;
                    else
                        CegTermeszetesJogosultsag = Base.HozferJogosultsag.Csakolvas;
                    break;
                case Base.KezSzint.Vezeto:
                    UserJogosultsag = Base.HozferJogosultsag.Semmi;
                    CegSzarmazekosJogosultsag = Base.HozferJogosultsag.Csakolvas;
                    CegTermeszetesJogosultsag = Base.HozferJogosultsag.Csakolvas;
                    break;

            }
        }
    }
}
