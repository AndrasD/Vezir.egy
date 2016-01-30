using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Text;
using FakPlusz.Alapfunkciok;
namespace UserSzamitasok
{
    /// <summary>
    /// Alkalmazasfuggo szamitasi eljarasok.
    /// Szukseg szerint bovitheto
    /// </summary>
    public static class UserSzamitasok
    {
        static FakUserInterface FakUserInterface = null;
        /// <summary>
        /// 
        /// </summary>
        public static bool KellSzamitasDatum = false;
        static string Datumtolstring = "";
        static string Datumigstring = "";
        static DateTime datumtol = DateTime.Today;
        static DateTime Datumtol
        {
            get { return datumtol; }
            set
            {
                datumtol = value;
                if (FakUserInterface != null)
                    Datumtolstring = FakUserInterface.DatumToString(value);
                else
                    Datumtolstring = "";
            }
        }
        static DateTime datumig = DateTime.Today;
        static DateTime Datumig
        {
            get { return datumig; }
            set
            {
                datumig = value;
                if (FakUserInterface != null)
                    Datumigstring = FakUserInterface.DatumToString(value);
                else
                    Datumigstring = "";

            }
        }
 //       static DataRow Adatsor = null;
 //       static string Adatnev = "";
        static Szamitasok UserSzam = new Szamitasok();
        /// <summary>
        /// Inicializalas
        /// </summary>
        /// <param name="fakuserinterface"></param>
        public static void Init(FakUserInterface fakuserinterface)
        {
            FakUserInterface = fakuserinterface;
        }
        /// <summary>
        /// DAtumhatarok beallitasa
        /// </summary>
        /// <param name="tol"></param>
        /// <param name="ig"></param>
        public static void DatumHatarok(DateTime tol, DateTime ig)
        {
            Datumtol = tol;
            Datumig = ig;
            KellSzamitasDatum = true;
        }
        /// <summary>
        /// Szamitas elvegzese
        /// </summary>
        /// <param name="row">
        /// aktualis adatsor
        /// </param>
        /// <param name="paramadatnevek">
        /// parameterek columnnevei
        /// </param>
        /// <param name="eredmenyadatnev">
        /// eredmeny columneve
        /// </param>
        /// <param name="szamitasnev">
        /// szamitas neve
        /// </param>
        public static void Szamit(DataRow row, string[] paramadatnevek, string eredmenyadatnev, string szamitasnev)
        {
            UserSzam.Szamit(row, paramadatnevek, eredmenyadatnev, szamitasnev);
        }
        /// <summary>
        /// Szamitasok
        /// </summary>
        public class Szamitasok
        {
            string[] split;
            char[] pont = new char[] { Convert.ToChar(".") };
            char[] vesszo = new char[] { Convert.ToChar(",") };
            /// <summary>
            /// szamitas elvegzese
            /// </summary>
            /// <param name="row">
            /// a tabla aktualis sora
            /// </param>
            /// <param name="paramadatnevek">
            /// parameterek neve
            /// </param>
            /// <param name="eredmenyadatnev">
            /// eredmeny neve
            /// </param>
            /// <param name="szamitasnev">
            /// a szamitas neve
            /// </param>
            public void Szamit(DataRow row, string[] paramadatnevek, string eredmenyadatnev, string szamitasnev)
            {
                if (!KellSzamitasDatum)
                {
                    Datumtol = DateTime.Today;
                    Datumig = DateTime.Today;
                }
                switch (szamitasnev)
                {
                    case "KOR":
                        if (row != null)
                        {
                            string szuladatnev = paramadatnevek[0];
                            DateTime szulido = Convert.ToDateTime(row[szuladatnev].ToString());
                            string eredmeny = FakUserInterface.DatumokKulonbsegeEvHoNap(szulido, Datumtol);
                            split = eredmeny.Split(pont);
                            int egesz = Convert.ToInt16(split[0]);
                            string egeszresz = "0,";
                            if (egesz != 0)
                                egeszresz = egesz.ToString() + ",";
                            decimal tized = Convert.ToDecimal(split[1]) / 12 * 10;
                            string tizedresz = tized.ToString();
                            split = tizedresz.Split(vesszo);
                            tizedresz = split[0];
                            //if (split.Length == 2)
                            //{
                            //    tizedresz = split[0];
                            //    if (tizedresz.Length > 2)
                            //        tizedresz = tizedresz.Substring(0, 2);
                            //}
                            //else
                            //    tizedresz = "0";
                            //if (tizedresz.Length == 1)
                            //    tizedresz = "0" + tizedresz;
                            row[eredmenyadatnev] = Convert.ToDecimal(egeszresz + tizedresz);
                        }
                        break;
                    case "LETSZAM":
                        if (row != null)
                        {
                            TimeSpan teljeshossz = Datumig - Datumtol.AddDays(-1);
                            string tolstring = row[paramadatnevek[0]].ToString();
                            string igstring = row[paramadatnevek[1]].ToString();
                            DateTime tol;
                            DateTime ig;
                            if (tolstring.CompareTo(Datumigstring) > 0 || igstring != "" && igstring.CompareTo(Datumtolstring) < 0)
                            {
                                row[eredmenyadatnev] = 0;
                                break;
                            }
                            if (tolstring == "" || tolstring.CompareTo(Datumtolstring) < 0)
                                tol = Datumtol;
                            else
                                tol = Convert.ToDateTime(tolstring);
                            if (igstring == "" || igstring.CompareTo(Datumigstring) > 0)
                                ig = Datumig;
                            else
                                ig = Convert.ToDateTime(igstring);
                            TimeSpan hossz = ig - tol.AddDays(-1);
                            string hosszstring = hossz.ToString();
                            Decimal hosszdec = Convert.ToDecimal(hosszstring.Split(pont)[0]);
                            string teljeshosszstring = teljeshossz.ToString();
                            Decimal teljeshosszdec = Convert.ToDecimal(teljeshosszstring.Split(pont)[0]);
                            row[eredmenyadatnev] = hosszdec / teljeshosszdec;
                        }
                        break;

                }
            }
        }
    }
}
