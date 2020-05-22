using System;
using irimhe.Maindb;
using Newtonsoft.Json;
using Npgsql;
using System.Data;

namespace irimhe.Models
{
    public class StationModel
    {
        public string Sindex { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }

        public string GetStation()
        {
            ConnDB conn = new ConnDB();
        
            string sql = "select * from station2";

            NpgsqlCommand cmd = conn.RunCmdPG(sql);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);

            DataSet data = new DataSet();

            da.Fill(data);
            conn.ClosePG();

            string JsonString = string.Empty;
            JsonString = JsonConvert.SerializeObject(data.Tables[0]);

            return JsonString;
        }
        
    }
    
}
