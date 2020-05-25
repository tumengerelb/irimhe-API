﻿using System;
using irimhe.Maindb;
using Newtonsoft.Json;
using Npgsql;
using System.Data;
namespace irimhe.Models
{
    public class CatalTModel
    {
        public string GetCatalT()
        {
            ConnDB conn = new ConnDB();

            string sql = "select * from agro_catal_p";

            NpgsqlCommand cmd = conn.RunCmdPG(sql);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);

            DataSet data = new DataSet();

            da.Fill(data);
            conn.ClosePG();

            string JsonString = string.Empty;
            JsonString = JsonConvert.SerializeObject(data.Tables[0]);

            return JsonString;
        }
        public string GetCatal_station(int station)
        {
            ConnDB conn = new ConnDB();

            string sql = "select * from agro_catal_p where index" + station + "";

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