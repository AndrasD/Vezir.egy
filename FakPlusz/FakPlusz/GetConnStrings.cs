using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FakPlusz
{
    /// <summary>
    /// Megadott filenamebol atveszi a definialt connectionstring-eket (, parameter alapjan az SQL VAGY MYSQL adatbazis
    /// fajtat es ha megadom, filename alapjan a atveszi a Backup path-t)
    /// </summary>
    public static class GetConnStrings
    {
        /// <summary>
        /// annak a filenak a neve, ahol elhelyezte a connection-stringeket
        /// </summary>
        private static string Filename = "";
        /// <summary>
        /// Egy fileban definialt ConnectionStringek ellenorzese, atvetele, az SqlInterface inicializalasa
        /// az adott adatbazisfajta szerint
        /// </summary>
        /// <param name="filename">
        /// A file Debug eseten alapertelmezesben a bin\Debug-ban, Release eseten a bin\Release-ben talalhato.
        /// Ezesetben a filename csak a file neve, ellenkezo esetben teljes utvonalat kell megadni.
        /// A ConnectionStringek megadasanak formaja:
        /// [rendszer ConnString],[user ConnString],[1.ceg ConnString](,...tovabbi cegconnectionstrigek)
        /// Ha ilyen file nem letezik, egy ures file-t hoz letre.
        /// Ha nincs meg a kotelezo minimalis szamu ConnectionString ,vagy formailag hibas, vagy valamelyik
        /// nem megnyithato, hibauzenetet ad
        /// <param name="backuppathfilenev">
        /// Backup path-t tartalmazo file neve, az SqlInterface inicialaizalasahoz
        /// </param>
        /// </param>
        /// <param name="adatbazisfajta">
        /// Sql vagy MySql, ha egyik sem, Sql-t feltetelez
        /// </param>
        /// <returns>
        /// Hiba eseten null / egyebkent a ConnectionString-ek tombje
        /// </returns>
        //public static string[] GetConnectionStrings(string filename,string backuppathfilenev,string adatbazisfajta)
        public static string[] GetConnectionStrings(string[] connstringek)
        {
            if (connstringek.Length < 3)
            {
                MessageBox.Show("Kevés a definiált ConnectionString!");
                return null;
            }
            if (Sqlnyitasok(connstringek))
                return null;
            return connstringek;
        }
        public static string[] GetConnectionStrings(string filename)
        {
            Filename = filename;
            if (!File.Exists(filename))
            {
                using (StreamWriter sw = File.CreateText(filename))
                sw.Close();
            }
            string[] connstringek = GetSplit(filename);
            if (connstringek.Length < 3)
            {
                MessageBox.Show("Kevés a Connection.txt-ben definiált ConnectionString, vagy hibás a forma!");
                return null;
            }
            //string mentespath = "";
            //if (backuppathfilenev != "")
            //{
            //    if (!File.Exists(backuppathfilenev))
            //    {
            //        using (StreamWriter sw = File.CreateText(backuppathfilenev))
            //            sw.Close();
            //    }
            //    string[] reszek = GetSplit(backuppathfilenev);
            //    backuppathfilenev = reszek[0];
            //    if (reszek.Length >1)
            //        mentespath=reszek[1];
            //    if (!Directory.Exists(backuppathfilenev))
            //    {
            //Directory.CreateDirectory(backuppathfilenev);
            //    }
            //    if(mentespath!="")
            //    {
            //        if (!Directory.Exists(mentespath))
            //        {
            //            Directory.CreateDirectory(mentespath);
            //        }
            //    }
            //}
            //Sqlinterface.SqlInterFace(adatbazisfajta,backuppathfilenev,mentespath);
            if (Sqlnyitasok(connstringek))
                return null;
            return connstringek;
        }
        private static bool Sqlnyitasok(string[] connstringek)
        {
            bool hibas = false;
            Sqlinterface.SqlInterFace(); 
            bool[] okek = new bool[connstringek.Length];
            for (int i = 0; i < connstringek.Length; i++)
                okek[i] = Sqlinterface.TryOpen(connstringek[i]);
            string hibaszov = "";
            for (int i = 0; i < okek.Length; i++)
            {
                if (!okek[i])
                    hibaszov += connstringek[i] + "\n";
            }
            if (hibaszov != "")
            {
                MessageBox.Show(hibaszov, "Hibás ConnectionStringek:");
                hibas = true;
            }
            return hibas;
        }
        /// <summary>
        /// adatbazisfajta="Sql" -vel hivja a GetConnectionStrings(filenev,backuppathfilenev,adatbazisfajta)-t
        /// </summary>
        /// <param name="filename">
        /// a file neve
        /// </param>
        /// <param name="backuppathfilenev">
        /// backup path
        /// </param>
        /// <returns>
        /// connectionstringek tombje
        /// </returns>
        //public static string[] GetConnectionStrings(string filename, string backuppathfilenev)
        //{
        //    return GetConnectionStrings(filename, backuppathfilenev,"Sql");
        //}

        ///// <summary>
        ///// backuppathfilenev="",adatbazisfajta = "Sql"-lel hivja a GetConnectionStrings(filenev,backuppathfilenev,adatbazisfajta)-t
        ///// </summary>
        ///// <param name="filename"></param>
        ///// <returns></returns>
        //public static string[] GetConnectionStrings(string filename)
        //{
        //    return GetConnectionStrings(filename,"", "Sql");
        //}
        /// <summary>
        /// belso hasznalatra, a GetConnectionStrings hivja
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private static string[] GetSplit(string filename)
        {
            string text = "";
            string[] split;
            char[] vesszo = new char[] { Convert.ToChar(",") };
            text = File.ReadAllText(filename, Encoding.Default);
            text = text.Replace("\r\n", "");
            split = text.Split(vesszo);
            for (int i = 0; i < split.Length; i++)
            {
                split[i] = split[i].Replace("[", "");
                split[i] = split[i].Replace("]", "");
            }
            return split;
        }
    }
}
