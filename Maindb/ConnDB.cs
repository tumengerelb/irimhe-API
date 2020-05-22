using System;
using System.Data.SqlClient;
using Npgsql;

namespace irimhe.Maindb
{
    public class ConnDB
    {
        private SqlConnection conn;
        private NpgsqlConnection connPG;

        public string GetConnectionString()
        {
            string conn_string = "Server=192.168.2.191;Database=agro;User Id=agro;Password=agro";

            return conn_string;
        }
        
        public SqlConnection DBConn()
        {   
            string connectionString = GetConnectionString();
            conn = new SqlConnection(connectionString);
            conn.Open();

            return conn;
        }

        public SqlCommand RunCmd(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, DBConn());
            return cmd;
        }
        public void Close()
        {
            conn.Close();
        }
        public string GetConnectionStringPG()
        {
            string conn_string = "Server=localhost;Database=postgres;User Id=postgres;password=Tumeepower123";

            return conn_string;
        }
        public NpgsqlConnection DBConnPG()
        {
            string connectionString = GetConnectionStringPG();
            connPG = new NpgsqlConnection(connectionString);
            
            connPG.Open();
            return connPG;
        }
        public NpgsqlCommand RunCmdPG(string sql)
        {
            NpgsqlCommand cmd = new NpgsqlCommand(sql, DBConnPG());
            return cmd;
        }
        public void ClosePG()
        {
            connPG.Close();
        }
    }
}
