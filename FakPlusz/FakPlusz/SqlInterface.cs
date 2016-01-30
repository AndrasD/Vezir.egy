using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
//using MySql.Data;
//using MySql.Data.MySqlClient;
using Microsoft.SqlServer;
//using Microsoft.SqlServer.Management.Smo;
//using Microsoft.SqlServer.Management.Common;
namespace FakPlusz
{
    /// <summary>
    /// Sql vagy MySql parancsok vegrahajtasa
    /// </summary>
    public static class Sqlinterface
    {
        /// <summary>
        /// az adatbazis tipusa: "Sql" vagy "MySql"
        /// </summary>
        private static string adatbazistipus = "";
        /// <summary>
        /// tipus="Sql" eseten a letrehozott Sqlrutinok
        /// </summary>
        private static Sqlrutinok sqlrutinok = null;
        /// <summary>
        /// tipus="MySql" eseten a letrehozott MySqlrutinok
        /// </summary>
        //private static MySqlrutinok mysqlrutinok = null;
        /// <summary>
        /// Ha a tip nem "MySql" letrehozza Sqlrutinok objectumot, egyebkent a MySqlrutinok objektumot
        /// Ha a backuppath ures, bevasalja "C:\\Program Files\\Microsoft SQL Server\\MSSQL.1\\MSSQL\\Backup"-ot,
        /// egyebkent atveszi
        /// </summary>
        /// <param name="tip">
        /// adatbazistipus
        /// </param>
        /// <param name="backuppath">
        /// </param>
        /// <param name="mentespath">
        /// teljes path Backup-hoz
        /// </param>
        //public static void SqlInterFace( string tip,string backuppath,string mentespath)
        public static void SqlInterFace()
        {
            adatbazistipus = "Sql";
            sqlrutinok = new Sqlrutinok();
            //if (backuppath == "")
            //    backuppath = "C:\\Program Files\\Microsoft SQL Server\\MSSQL.1\\MSSQL\\Backup";
            //if (tip != "MySql")
            //    sqlrutinok = new Sqlrutinok(backuppath,mentespath);
            //else
            //    mysqlrutinok = new MySqlrutinok();
        }
        /// <summary>
        /// backuppath = ""-el hivja az SqlInterFace(tip,backuppath)-t
        /// </summary>
        /// <param name="tip">
        /// adatbazistipus
        /// </param>
        //public static void SqlInterFace(string tip)
        //{
        //    SqlInterFace(tip, "","");
        //}
        /// <summary>
        /// rendszer-, userconnection kijelolese
        /// </summary>
        /// <param name="rendszerconn">
        /// Rendszeradatbazis Connection String
        /// </param>
        /// <param name="userconn">
        /// Useradatbazis Connection String
        /// </param>
        /// <returns>
        /// true: rendben, meg tudta nyitni
        /// </returns>
        public static bool RendszerUserConn(string rendszerconn,string userconn)
        {
            //if(adatbazistipus=="Sql")
            //{
                if (!sqlrutinok.TryOpen(rendszerconn))
                    return false;
                else if(rendszerconn!=userconn)
                {
                    if(!sqlrutinok.TryOpen(userconn))
                        return false;
                }
                sqlrutinok.RendszerUserConn(rendszerconn,userconn);
                return true;
            //}
            //else
            //{
            //    if (!mysqlrutinok.TryOpen(rendszerconn))
            //        return false;
            //    else if (rendszerconn != userconn)
            //    {
            //        if (!mysqlrutinok.TryOpen(userconn))
            //            return false;
            //    }
            //    mysqlrutinok.RendszerUserConn(rendszerconn, userconn);
            //    return true;
            //}
        }
        /// <summary>
        /// Az osszes ceg connectionstringjenek atvetele
        /// </summary>
        /// <param name="cegconnstringek">
        /// a cegconnectionok
        /// </param>
        public static void SetCegConnectionok(string[] cegconnstringek)
        {
            if (adatbazistipus == "Sql")
                sqlrutinok.SetCegConnectionok(cegconnstringek);
        }
        /// <summary>
        /// Adatbazis Backup
        /// </summary>
        /// <param name="connstring">
        /// Az adatbazis connectionstring-je
        /// </param>
        //public static void Mentes(string connstring)
        //{
        //    if (adatbazistipus == "Sql")
        //        sqlrutinok.Mentes(connstring);
        //}
        /// <summary>
        /// Adatbazis utolso Backup-janak datuma
        /// </summary>
        /// <param name="connstring">
        /// Az adatbazis connectionstring-je
        /// </param>
        /// <returns></returns>
        //public static DateTime GetLastBackupDate(string connstring)
        //{
        //    if (adatbazistipus == "Sql")
        //        return sqlrutinok.GetLastBackupDate(connstring);
        //    return DateTime.MinValue;
        //}
        /// <summary>
        /// megprobalja megnyitni a connection-t
        /// </summary>
        /// <param name="connstring">
        /// az ellenorizni kivan ConnectionString
        /// </param>
        /// <returns>
        /// true:rendben
        /// </returns>
        public static bool TryOpen(string connstring)
        {
            //if (adatbazistipus == "Sql")
                return sqlrutinok.TryOpen(connstring);
            //else
            //    return mysqlrutinok.TryOpen(connstring);
        }
        /// <summary>
        /// Ellenorzi az alkalmazas adatbazis letet
        /// </summary>
        /// <param name="connstring">
        /// Alkalmazas adatbazis Connection String
        /// </param>
        /// <returns>
        /// true: az adatbazis letezik
        ///</returns>
        public static bool Cegconn(string connstring)
        {
            //if (adatbazistipus == "Sql")
                return (sqlrutinok.Cegconn(connstring));
            //else
            //    return (mysqlrutinok.Cegconn(connstring));
        }
        /// <summary>
        /// Parameterek alapjan Select kiadasa
        /// </summary>
        /// <param name="dt">
        /// DataTable
        /// </param>
        /// <param name="connstring">
        /// Connection String
        /// </param>
        /// <param name="tablanev">
        /// a tabla neve
        /// </param>
        /// <param name="selwhere">
        /// Select WHERE resze: WHERE ... formaban , lehet ures:""
        /// </param>
        /// <param name="selord">
        /// Select ORDER BY resze: ORDER BY... formaban, lehet ures:""
        /// </param>
        /// <param name="top">
        /// true: csak az elso sorat hozza be
        /// </param>
        /// <returns>
        /// Data Table
        /// </returns>
        public static DataTable Select(DataTable dt, string connstring, string tablanev, string selwhere, string selord, bool top)
        {
            dt.Rows.Clear();
            //if (adatbazistipus == "Sql")
                return (sqlrutinok.Select(dt, connstring, tablanev, selwhere, selord, top));
            //else
            //    return (mysqlrutinok.Select(dt, connstring, tablanev, selwhere, selord, top));
        }
        public static DataTable SpecCommand(DataTable dt, string connstring, string tablanev, string selwhere, string selord)
        {
            //if (adatbazistipus == "Sql")
                return (sqlrutinok.SpecCommand(dt, connstring, tablanev, selwhere, selord));
            //else
            //    return new DataTable();
        }
        public static DataTable StoredProcedureCommand(DataTable dt, string connstring, string procnev)
        {
            return sqlrutinok.StoredProcedureCommand(dt, connstring, procnev);
        }
        /// <summary>
        /// SchemaTable beolvasasa
        /// </summary>
        /// <param name="dt">
        /// DataTable
        /// </param>
        /// <param name="connstring">
        /// Connection String
        /// </param>
        /// <param name="tablanev">
        /// Tabla neve
        /// </param>
        /// <returns>
        /// DataTable
        /// </returns>
        public static DataTable GetSchemaTable(DataTable dt, string connstring, string tablanev)
        {
            //if (adatbazistipus == "Sql")
                return (sqlrutinok.GetSchemaTable(dt, connstring, tablanev));
            //else
            //    return (mysqlrutinok.GetSchemaTable(dt, connstring, tablanev));
        }
        /// <summary>
        /// Connection megnyitasa
        /// </summary>
        /// <param name="connstring">
        /// Connection String
        /// </param>
        public static void ConnOpen(string connstring)
        {
            //if (adatbazistipus == "Sql")
                sqlrutinok.ConnOpen(connstring);
            //else
            //    mysqlrutinok.ConnOpen(connstring);
        }
        /// <summary>
        /// Connection lezaras
        /// </summary>
        /// <param name="connstring">
        /// Connection String
        /// </param>
        public static void ConnClose(string connstring)
        {
            //if (adatbazistipus == "Sql")
                sqlrutinok.ConnClose(connstring);
            //else
            //    mysqlrutinok.ConnClose(connstring);
        }
        /// <summary>
        /// Tranzakcio inditasa
        /// </summary>
        /// <param name="connstring">
        /// Connection String
        /// </param>
        public static void BeginTransaction(string connstring)
        {
            //if (adatbazistipus == "Sql")
                sqlrutinok.BeginTransaction(connstring);
            //else
            //    mysqlrutinok.BeginTransaction(connstring);
        }
        /// <summary>
        /// Szarmazekos tablak Update-je
        /// </summary>
        /// <param name="connstring">
        /// Connection String
        /// </param>
        /// <param name="dt">
        /// DataTable
        /// </param>
        /// <param name="sel">
        /// Osszeallitott Select
        /// </param>
        /// <returns>
        /// true: sikeres Update
        /// </returns>
        public static bool CommandBuilderUpd(string connstring, DataTable dt, string sel)
        {
            //if (adatbazistipus == "Sql")
                return (sqlrutinok.CommandBuilderUpd(connstring, sel, dt));
            //else
            //    return (mysqlrutinok.CommandBuilderUpd(connstring, sel, dt));
        }
        /// <summary>
        /// Termeszetes tablak Update-je
        /// </summary>
        /// <param name="connstring">
        /// Connection String
        /// </param>
        /// <param name="tablanev">
        /// A tabla neve
        /// </param>
        /// <param name="ident">
        /// A tabla Identity Column neve
        /// </param>
        /// <param name="dt">
        /// DataTable
        /// </param>
        /// <returns>
        /// true: sikeres Update
        /// </returns>
        public static bool CommandBuilderUpd(string connstring, string tablanev, string ident, DataTable dt)
        {
            string sel = "select * from " + tablanev + " where " + ident + "=(select max(" + ident + ") from " + tablanev + ")";
            //if (adatbazistipus == "Sql")
                return (sqlrutinok.CommandBuilderUpd(connstring, sel, dt));
            //else
            //    return (mysqlrutinok.CommandBuilderUpd(connstring, sel, dt));
        }
        /// <summary>
        /// Fill kiadasa 
        /// Ha elotte utoljara Select-et hivtunk, ezt megismetli,
        /// ha sikeres CommandBuilderUpd-et, az ott megadott Select-et hajtja vegre
        /// </summary>
        /// <param name="dt">
        /// DataTable
        /// </param>
        /// <returns>
        /// true: sikeres Fill
        /// </returns>
        public static DataTable Fill(DataTable dt)
        {
            //if (adatbazistipus == "Sql")
                return sqlrutinok.Fill(dt);
            //else
            //    return mysqlrutinok.Fill(dt);
        }
        /// <summary>
        /// Tranzakcio befejezese
        /// </summary>
        /// <returns>
        /// true: sikeres volt a tranzakcio
        /// </returns>
        public static bool CommitTransaction()
        {
            //if (adatbazistipus == "Sql")
                return sqlrutinok.CommitTransaction();
            //else
            //    return mysqlrutinok.CommitTransaction();
        }
        /// <summary>
        /// adatbazistipus="Sql" eseten az SqlInterface-ben leirt memberek az itteni azonos nevu membereket hivjak
        /// </summary>
        private class Sqlrutinok
        {
//            private Server Server = null;
            private SqlConnection rendszerconn = null;
            private string rendszerconnstring = "";
            private string rendszeradatbazisnev = "";
            private SqlConnection userconn = null;
            private string userconnstring = "";
            private string useradatbazisnev = "";
            private SqlConnection[] cegconnectionok = null;
            private SqlConnection cegconn = null;
            private string[] cegconnstringek = null;
            private string cegconnstring = "";
            private string[] cegadatbazisnevek = null;
            private SqlDataAdapter DataAdapter;
            private SqlCommand comm = new SqlCommand();
            private SqlTransaction tx = null;
            private bool egyconn = false;
            private SqlDataAdapter lastadapter = null;
            private string servernev = "";
//            private string backupadatnev = "";
//            private string mentesdevice = "";
            public Sqlrutinok()
            {
                //backupadatnev = backuppath;
                //mentesdevice = mentespath;
            }
            public void SetCegConnectionok(string[] connstringek)
            {
                cegconnstringek = connstringek;
                ArrayList ar = new ArrayList();
                ArrayList ar1 = new ArrayList();
                for (int i = 0; i < cegconnstringek.Length; i++)
                {
                    SqlConnection conn = new SqlConnection(cegconnstringek[i]);
                    ar.Add(conn);
                    ar1.Add(conn.Database);
                }
                cegconnectionok = (SqlConnection[])ar.ToArray(typeof(SqlConnection));
                cegadatbazisnevek = (string[])ar1.ToArray(typeof(string));
            }
            //public DateTime GetLastBackupDate(string connstring)
            //{
            //    string adatbazisnev = "";

            //    if (connstring == rendszerconnstring)
            //        adatbazisnev = rendszeradatbazisnev;
            //    else if (connstring == userconnstring)
            //        adatbazisnev = useradatbazisnev;
            //    else
            //    {
            //        for (int i = 0; i < cegconnectionok.Length; i++)
            //        {
            //            if (cegconnstringek[i] == connstring)
            //            {
            //                adatbazisnev = cegadatbazisnevek[i];
            //                break;
            //            }
            //        }
            //    }
            //    string[] files = Directory.GetFiles(backupadatnev);
            //    DateTime lastbackup = DateTime.MinValue;
            //    for (int i = 0; i < files.Length; i++)
            //    {
            //        if (files[i].Contains("\\" + adatbazisnev + "."))
            //        {
            //            lastbackup = File.GetLastWriteTime(files[i]);
            //            break;
            //        }
            //    }
            //    return lastbackup;
            //}
            //public void Mentes(string connstring)
            //{
            //    string adatbazisnev="";
            //    if (connstring == rendszerconnstring)
            //        adatbazisnev=rendszeradatbazisnev;
            //    else if (connstring == userconnstring)
            //        adatbazisnev=useradatbazisnev;
            //    else
            //    {
            //        for (int i = 0; i < cegconnectionok.Length; i++)
            //        {
            //            if(cegconnstringek[i]==connstring)
            //            {
            //                adatbazisnev=cegadatbazisnevek[i];
            //                break;
            //            }
            //        }
            //    }
            //    Backup backup = new Backup();
            //    string desc = backup.BackupSetDescription;
            //    backup.BackupSetName = adatbazisnev + ".bak";
            //    backup.Database = adatbazisnev;
            //    BackupDeviceItem dev = new BackupDeviceItem(backup.BackupSetName, DeviceType.File);
            //    backup.Devices.Add(dev);
            //    backup.SqlBackup(Server);
            //    if (mentesdevice != "")
            //    {
            //        File.Copy(backupadatnev + "\\" + backup.BackupSetName, mentesdevice + "\\" + backup.BackupSetName, true);
            //    }
            //}
            public void RendszerUserConn(string connstring, string userstring)
            {
                rendszerconn = new SqlConnection(connstring);
                servernev = rendszerconn.DataSource;
//                Server = new Server(servernev);
                rendszeradatbazisnev = rendszerconn.Database;
                DataAdapter = new SqlDataAdapter("", rendszerconn);
                rendszerconnstring = connstring;
                if (userstring != "" & userstring != connstring)
                {
                    userconnstring = userstring;
                    userconn = new SqlConnection(userconnstring);
                    useradatbazisnev = userconn.Database;
                }
                else
                {
                    egyconn = true;
                    userconnstring = connstring;
                    userconn = rendszerconn;
                    useradatbazisnev = rendszeradatbazisnev;
                }
            }
            public bool TryOpen(string connstring)
            {
                SqlConnection conn = new SqlConnection(connstring);
                try
                {
                    conn.Open();
                    conn.Close();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            public bool Cegconn(string connstring)
            {
                if (connstring == rendszerconnstring)
                {
                    egyconn = true;
                    return true;
                }
                else
                {
                    if (TryOpen(connstring))
                    {
                        cegconn = new SqlConnection(connstring);
                        cegconnstring = connstring;
                        egyconn = false;
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Hibás connectionstring:" + connstring);
                        return false;
                    }
                }
            }
            public bool ConnOpen(string constring)
            {
                if (constring == rendszerconnstring || egyconn)
                {
                    try
                    {
                        rendszerconn.Open();
                        return true;
                    }
                    catch
                    {
                        MessageBox.Show("Hibás connectionstring:" + constring);
                        return false;
                    }
                }
                else if (constring == userconnstring)
                {
                    try
                    {
                        userconn.Open();
                        return true;
                    }
                    catch
                    {
                        MessageBox.Show("Hibás connectionstring:" + constring);
                        return false;
                    }
                }
                else
                {
                    try
                    {
                        cegconn.Open();
                        return true;
                    }
                    catch
                    {
                        MessageBox.Show("Hibás connectionstring:" + constring);
                        return false;
                    }
                }
            }
            public void ConnClose(string constring)
            {
                if (egyconn || constring == rendszerconnstring)
                    rendszerconn.Close();
                else if (constring == userconnstring)
                    userconn.Close();
                else
                    cegconn.Close();
            }
            public DataTable SpecCommand(DataTable dt, string constring, string tablanev, string selwhere, string selord)
            {
                if(constring== rendszerconnstring || egyconn)
                    DataAdapter.SelectCommand.Connection = rendszerconn;
                else if (constring == userconnstring)
                    DataAdapter.SelectCommand.Connection = userconn;
                else
                    DataAdapter.SelectCommand.Connection = cegconn;
                DataAdapter.SelectCommand.CommandText = selwhere ;
                try
                {
                    DataAdapter.Fill(dt);
                }
                catch
                {
                }
                return dt;
            }
            public DataTable StoredProcedureCommand(DataTable dt, string constring, string procnev)
            {
                if (constring == rendszerconnstring || egyconn)
                    DataAdapter.SelectCommand.Connection = rendszerconn;
                else if (constring == userconnstring)
                    DataAdapter.SelectCommand.Connection = userconn;
                else
                    DataAdapter.SelectCommand.Connection = cegconn;
                DataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataAdapter.SelectCommand.CommandText = procnev;
                try
                {
                    DataAdapter.Fill(dt);
                }
                catch
                {
                }
                DataAdapter.SelectCommand.CommandType = CommandType.Text;
                return dt;
            }

            public DataTable Select(DataTable dt, string constring, string tablanev, string selwhere, string selord, bool top)
            {
                string sel = "select ";
                if (top)
                    sel += "top 1 ";
                sel += "* from " + tablanev + " ";
                if (selwhere != "")
                    sel += selwhere;
                if (selord != "")
                    sel += selord;
                if (constring == rendszerconnstring || egyconn)
                    DataAdapter.SelectCommand.Connection = rendszerconn;
                else if (constring == userconnstring)
                    DataAdapter.SelectCommand.Connection = userconn;
                else
                    DataAdapter.SelectCommand.Connection = cegconn;
                DataAdapter.SelectCommand.CommandText = sel;
                try
                {
                    DataAdapter.Fill(dt);
                }
                catch (SqlException ex)
                {
                    if (ex.Number != 208)             // nincs ilyen nevu tabla
                    {
                        string hiba = "Select hiba:"+sel+"\n"+ ex.Message ;
                        MessageBox.Show(hiba);
                    }
                    return null;
                }
                return dt;
            }
            public DataTable GetSchemaTable(DataTable dt, string connstring, string tablanev)
            {
                string sel = "select top 1 * from " + tablanev;
                SqlCommand comm = new SqlCommand();

                if (connstring == rendszerconnstring || egyconn)
                    comm.Connection = rendszerconn;
                else if (connstring == userconnstring)
                    comm.Connection = userconn;
                else
                    comm.Connection = cegconn;
                comm.CommandText = sel;
                comm.Connection.Open();
                SqlDataReader dreader = null;
                try
                {
                    dreader = comm.ExecuteReader();
                    dt = dreader.GetSchemaTable();
                    comm.Connection.Close();
                    return dt;
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 208)
                    {
                        string hiba = tablanev + " nevü tábla nincs a " + comm.Connection.Database + " adatbázisban!";
                        MessageBox.Show(hiba);
                    }
                    else

                    {
                        string hiba = "Select hiba:" + sel + "\n" + ex.Message;
                        MessageBox.Show(hiba);
                    }
                    return null;
                }

            }
            public DataTable Fill(DataTable dt)
            {
                if (lastadapter == null)
                    DataAdapter.Fill(dt);
                else
                    lastadapter.Fill(dt);
                return dt;
            }
            public void BeginTransaction(string connstring)
            {
                if (connstring == rendszerconnstring || egyconn)
                    tx = rendszerconn.BeginTransaction();
                else if (connstring == userconnstring)
                    tx = userconn.BeginTransaction();
                else
                    tx = cegconn.BeginTransaction();
            }
            public bool CommandBuilderUpd(string connstring, string sel, DataTable dt)
            {
                SqlConnection conn;
                if (connstring == rendszerconnstring || egyconn)
                    conn = rendszerconn;
                else if (connstring == userconnstring)
                    conn = userconn;
                else
                    conn = cegconn;
                SqlDataAdapter da = new SqlDataAdapter(sel, conn);
                lastadapter = da;
                da.SelectCommand.Transaction = tx;
                SqlCommandBuilder cb = new SqlCommandBuilder(da);
                string text = "Update hiba:\n" + "Táblanév:" + dt.TableName + "\n" + "Select:" + sel + "\n";
                try
                {
                    cb.DataAdapter.Update(dt);
                    return true;
                }
                catch (DBConcurrencyException ex)
                {
                    MessageBox.Show(text + ex.Message);
                    if (tx != null)
                    {
                        tx.Rollback();
                        if (rendszerconn.State == ConnectionState.Open)
                            rendszerconn.Close();
                        else if (userconn.State == ConnectionState.Open)
                            userconn.Close();
                        else if (cegconn.State == ConnectionState.Open)
                            cegconn.Close();
                        tx = null;
                        lastadapter = null;
                    }
                    return false;
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(text + ex.Message);
                    if (tx != null)
                    {
                        tx.Rollback();
                        if (rendszerconn.State == ConnectionState.Open)
                            rendszerconn.Close();
                        else if (userconn.State == ConnectionState.Open)
                            userconn.Close();
                        else if (cegconn.State == ConnectionState.Open)
                            cegconn.Close();
                        tx = null;
                        lastadapter = null;
                    }
                    return false;
                }
                catch (SqlException ex)
                {
                    for (int i = 0; i < ex.Errors.Count; i++)
                    {
                        text += "\n" + ex.Errors[i].Message;
                    }
                    MessageBox.Show(text);
                    if (tx != null)
                    {
                        tx.Rollback();
                        if (rendszerconn.State == ConnectionState.Open)
                            rendszerconn.Close();
                        else if (userconn.State == ConnectionState.Open)
                            userconn.Close();
                        else if (cegconn.State == ConnectionState.Open)
                            cegconn.Close();
                        tx = null;
                        lastadapter = null;
                    }
                    return false;
                }
            }
            public bool CommitTransaction()
            {
                if (tx != null)
                {
                    try
                    {
                        tx.Commit();
                        if (rendszerconn.State == ConnectionState.Open)
                            rendszerconn.Close();
                        else if (userconn.State == ConnectionState.Open)
                            userconn.Close();
                        else if (cegconn.State == ConnectionState.Open)
                            cegconn.Close();
                        lastadapter = null;
                        tx = null;
                        return true;

                    }
                    catch (Exception ex)
                    {
                        if (tx != null)
                        {
                            string hiba = "Tranzakció hiba:" + ex.Message + "\n";
                            MessageBox.Show("Tranzakció hiba:" + ex.Message + "\n");
                            try
                            {
                                tx.Rollback();
                            }
                            catch (InvalidOperationException ex1)
                            {
                                MessageBox.Show(hiba + "Rollback hiba:" + ex1.Message);
                            }
                            if (rendszerconn.State == ConnectionState.Open)
                                rendszerconn.Close();
                            else if (userconn.State == ConnectionState.Open)
                                userconn.Close();
                            else if (cegconn.State == ConnectionState.Open)
                                cegconn.Close();
                            tx = null;
                            lastadapter = null;
                        }
                        return false;
                    }
                }
                return false;
            }

        }
        /// <summary>
        /// adatbazistipus="MySql" eseten az SqlInterface-ben leirt memberek az itteni azonos nevu membereket hivjak
        /// </summary>
        //private class MySqlrutinok
        //{
        //    private MySqlConnection rendszerconn = null;
        //    private string rendszerconnstring = "";
        //    private MySqlConnection userconn = null;
        //    private string userconnstring = "";
        //    private MySqlConnection cegconn = null;
        //    private string cegconnstring = "";
        //    private MySqlDataAdapter DataAdapter;
        //    private MySqlCommand comm = new MySqlCommand();
        //    private MySqlTransaction tx;
        //    private bool egyconn = false;
        //    private MySqlDataAdapter lastadapter = null;
        //    public MySqlrutinok()
        //    {
        //    }
        //    public void RendszerUserConn(string connstring, string userstring)
        //    {
        //        rendszerconn = new MySqlConnection(connstring);
        //        rendszerconnstring = connstring;
        //        DataAdapter = new MySqlDataAdapter("", rendszerconn);
        //        if (userstring != "" & userstring != connstring)
        //        {
        //            userconnstring = userstring;
        //            userconn = new MySqlConnection(userconnstring);
        //        }
        //        else
        //            egyconn = true;
        //        DataAdapter.FillError += new FillErrorEventHandler(DataAdapter_FillError);

        //    }
        //    public void DataAdapter_FillError(object sender, FillErrorEventArgs e)
        //    {
        //    }
        //    public void SqlrutinokConn(string connstring, string userstring)
        //    {
        //        rendszerconn = new MySqlConnection(connstring);
        //        DataAdapter = new MySqlDataAdapter("", rendszerconn);
        //        rendszerconnstring = connstring;
        //        if (userstring != "" & userstring != connstring)
        //        {
        //            userconnstring = userstring;
        //            userconn = new MySqlConnection(userconnstring);
        //        }
        //        else
        //            egyconn = true;
        //    }
        //    public bool TryOpen(string connstring)
        //    {
        //        MySqlConnection conn = new MySqlConnection(connstring);
        //        try
        //        {
        //            conn.Open();
        //            conn.Close();
        //            return true;
        //        }
        //        catch
        //        {
        //            return false;
        //        }
        //    }
        //    public bool Cegconn(string connstring)
        //    {
        //        if (connstring == rendszerconnstring)
        //        {
        //            egyconn = true;
        //            return true;
        //        }
        //        else
        //        {
        //            if (TryOpen(connstring))
        //            {
        //                cegconn = new MySqlConnection(connstring);
        //                cegconnstring = connstring;
        //                egyconn = false;
        //                return true;
        //            }
        //            else
        //            {
        //                MessageBox.Show("Hibás connectionstring:" + connstring);
        //                return false;
        //            }
        //        }
        //    }
        //    public bool ConnOpen(string constring)
        //    {
        //        if (constring == rendszerconnstring || egyconn)
        //        {
        //            try
        //            {
        //                rendszerconn.Open();
        //                return true;
        //            }
        //            catch
        //            {
        //                MessageBox.Show("Hibás connectionstring:" + constring);
        //                return false;
        //            }
        //        }
        //        else if (constring == userconnstring)
        //        {
        //            try
        //            {
        //                userconn.Open();
        //                return true;
        //            }
        //            catch
        //            {
        //                MessageBox.Show("Hibás connectionstring:" + constring);
        //                return false;
        //           }
        //        }
        //        else
        //        {
        //            try
        //            {
        //                cegconn.Open();
        //                return true;
        //            }
        //            catch
        //            {
        //                MessageBox.Show("Hibás connectionstring:" + constring);
        //                return false;
        //            }
        //        }
        //    }
        //    public void ConnClose(string constring)
        //    {
        //        if (constring == rendszerconnstring || egyconn)
        //            rendszerconn.Close();
        //        else if (constring == userconnstring)
        //            userconn.Close();
        //        else
        //            cegconn.Close();
        //    }

        //    public DataTable Select(DataTable dt, string constring, string tablanev, string selwhere, string selord, bool top)
        //    {
        //        string sel = "select * from " + tablanev + " ";
        //        if (selwhere != "")
        //            sel += selwhere;
        //        if (selord != "")
        //            sel += selord;
        //        if (top)
        //            sel += " limit 1";
        //        if (constring == rendszerconnstring || egyconn)
        //            DataAdapter.SelectCommand.Connection = rendszerconn;
        //        else if (constring == userconnstring)
        //            DataAdapter.SelectCommand.Connection = userconn;
        //        else
        //            DataAdapter.SelectCommand.Connection = cegconn;
        //        DataAdapter.SelectCommand.CommandText = sel;
        //        try
        //        {
        //            DataAdapter.Fill(dt);
        //        }
        //        catch
        //        {
        //            return null;
        //        }
        //        return dt;
        //    }
        //    public DataTable GetSchemaTable(DataTable dt, string connstring, string tablanev)
        //    {
        //        string sel = "select * from " + tablanev + " limit 1";
        //        MySqlCommand comm = new MySqlCommand();

        //        if (connstring == rendszerconnstring || egyconn)
        //            comm.Connection = rendszerconn;
        //        else if (connstring == userconnstring)
        //            comm.Connection = userconn;
        //        else
        //            comm.Connection = cegconn;
        //        comm.CommandText = sel;
        //        comm.Connection.Open();
        //        MySqlDataReader dreader = comm.ExecuteReader();
        //        dt = dreader.GetSchemaTable();
        //        comm.Connection.Close();
        //        return dt;
        //    }
        //    public DataTable Fill(DataTable dt)
        //    {
        //        if (lastadapter == null)
        //            DataAdapter.Fill(dt);
        //        else
        //            lastadapter.Fill(dt);
        //        return dt;
        //    }

        //    public void BeginTransaction(string connstring)
        //    {
        //        if (connstring == rendszerconnstring || egyconn)
        //            tx = rendszerconn.BeginTransaction();
        //        else if (connstring == userconnstring)
        //            tx = userconn.BeginTransaction();
        //        else
        //            tx = cegconn.BeginTransaction();
        //    }
        //    public bool CommandBuilderUpd(string connstring, string sel, DataTable dt)
        //    {
        //        MySqlConnection conn;
        //        if (connstring == rendszerconnstring || egyconn)
        //            conn = rendszerconn;
        //        else if (connstring == userconnstring)
        //            conn = userconn;
        //        else
        //            conn = cegconn;
        //        MySqlDataAdapter da = new MySqlDataAdapter(sel, conn);
        //        lastadapter = da;
        //        da.SelectCommand.Transaction = tx;
        //        MySqlCommandBuilder cb = new MySqlCommandBuilder(da);
        //        try
        //        {
        //            cb.DataAdapter.Update(dt);
        //            return true;
        //        }
        //        catch (ArgumentException ex)
        //        {
        //            string par = ex.ParamName;
        //            if (tx != null)
        //            {
        //                tx.Rollback();
        //                tx.Connection.Close();
        //                lastadapter = null;
        //                tx = null;
        //            }
        //            return false;
        //        }
        //        catch (MySqlException ex)
        //        {
        //            SqlEx(ex);
        //            if (tx != null)
        //            {
        //                tx.Rollback();
        //                tx.Connection.Close();
        //                lastadapter = null;
        //                tx = null;
        //            }
        //            return false;
        //        }
        //    }

        //    private void SqlEx(MySqlException ex)
        //    {
        //        string text = "Adattábla Update hiba:";
        //        MessageBox.Show(text + ex.Message);
        //    }
        //    public bool CommitTransaction()
        //    {
        //        if (tx != null)
        //        {
        //            try
        //            {
        //                tx.Commit();
        //                tx.Connection.Close();
        //                lastadapter = null;
        //                tx = null;
        //                return true;

        //            }
        //            catch (Exception ex)
        //            {
        //                if (tx != null)
        //                {
        //                    string hiba = "Tranzakció hiba:" + ex.Message + "\n";
        //                    MessageBox.Show("Tranzakció hiba:" + ex.Message + "\n");
        //                    try
        //                    {
        //                        tx.Rollback();
        //                    }
        //                    catch (InvalidOperationException ex1)
        //                    {
        //                        MessageBox.Show(hiba + "Rollback hiba:" + ex1.Message);
        //                    }
        //                    tx.Connection.Close();
        //                    tx = null;
        //                    lastadapter = null;
        //                }
        //                return false;
        //            }
        //        }
        //        return false;
        //    }
        //}
    }
}
