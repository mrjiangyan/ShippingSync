
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
//using MySql.Data.MySqlClient;
using System.Data.SqlClient;

namespace Utilities.Dal
{
    /// <summary>
    /// Connection工厂用于实例化对应的IDbConnection对象，传递给Dapper。
    /// </summary>
    public class ConnectionFactory
    {
        private const string ConnectionKey = "Database";
        private static readonly string connectionName = ConfigurationManager.AppSettings["ConnectionName"];
        private static string connString = ConfigurationManager.ConnectionStrings[ConnectionKey].ConnectionString;

        public static IDbConnection CreateConnection()
        {
            IDbConnection conn = null;
            switch (connectionName)
            {
                case "SQLServer":
                    conn = new SqlConnection(connString);
                    break;
                //case "MySQL":
                //    conn = new MySqlConnection(connString);
                //    break;
                default:
                    conn = new SqlConnection(connString);
                    break;
            }
            conn.Open();
            return conn;
        }

        public static bool TryConnection(string connectionString,out string message)
        {
            IDbConnection conn = null;
           
            try
            {
                switch (connectionName)
                {
                    case "SQLServer":
                        conn = new SqlConnection(connectionString);
                        break;
                    //case "MySQL":
                    //    conn = new MySqlConnection(connString);
                    //    break;
                    default:
                        conn = new SqlConnection(connectionString);
                        break;
                }
                conn.Open();
                conn.Close();
                conn.Dispose();
                message = null;
                return true;
            }
            catch (Exception err)
            {
                message = err.Message;
            }
            return false;
        }

        public static string ConnectionString
        {
            get
            {
                return connString;
            }
            set
            {
                connString = value;
                Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                //添加
                cfa.ConnectionStrings.ConnectionStrings[ConnectionKey].ConnectionString=value;
                //修改
                cfa.Save();
               
            }
        }
    }
}
