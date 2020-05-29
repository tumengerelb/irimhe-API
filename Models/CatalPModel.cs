using System;
using irimhe.Maindb;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Dynamic;
using Microsoft.SqlServer.Server;

namespace irimhe.Models
{
    public class CatalPModel
    {
        public class catalpclass
        {
            public int id { get; set; }
            public int aimagid { get; set; }
            public string aimagname { get; set; }
            public string sumname { get; set; }
            public int index { get; set; }
            public string sname { get; set; }
            public float lat { get; set; }
            public float lon { get; set; }
            public float rasterheight { get; set; }
            public decimal jan_01 { get; set; }
            public decimal jan_02 { get; set; }
            public decimal jan_03 { get; set; }
            public decimal feb_01 { get; set; }
            public decimal feb_02 { get; set; }
            public decimal feb_03 { get; set; }
            public decimal mar_01 { get; set; }
            public decimal mar_02 { get; set; }
            public decimal mar_03 { get; set; }
            public decimal apr_01 { get; set; }
            public decimal apr_02 { get; set; }
            public decimal apr_03 { get; set; }
            public decimal may_01 { get; set; }
            public decimal may_02 { get; set; }
            public decimal may_03 { get; set; }
            public decimal jun_01 { get; set; }
            public decimal jun_02 { get; set; }
            public decimal jun_03 { get; set; }
            public decimal jul_01 { get; set; }
            public decimal jul_02 { get; set; }
            public decimal jul_03 { get; set; }
            public decimal aug_01 { get; set; }
            public decimal aug_02 { get; set; }
            public decimal aug_03 { get; set; }
            public decimal sep_01 { get; set; }
            public decimal sep_02 { get; set; }
            public decimal sep_03 { get; set; }
            public decimal oct_01 { get; set; }
            public decimal oct_02 { get; set; }
            public decimal oct_03 { get; set; }
            public decimal nov_01 { get; set; }
            public decimal nov_02 { get; set; }
            public decimal nov_03 { get; set; }
            public decimal dec_01 { get; set; }
            public decimal dec_02 { get; set; }
            public decimal dec_03 { get; set; }
        }

        public string only_perc(string station)
        {
            ConnDB conn = new ConnDB();
            string sql = "";

            if (string.IsNullOrWhiteSpace(station))
            {
                sql = "select * from agro_catal_p";
            }
            else
            {
                int sta = Int32.Parse(station);
                sql = "select * from agro_catal_p index =" + sta + "";
            }

            NpgsqlCommand cmd = conn.RunCmdPG(sql);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);

            DataSet data = new DataSet();

            da.Fill(data);
            conn.ClosePG();

            string JsonString = string.Empty;
            JsonString = JsonConvert.SerializeObject(catalt(data.Tables[0]));

            return JsonString;

        }
        public List<catalpclass> catalt(DataTable data)
        {
            catalpclass temp = new catalpclass();

            var convertedList = (from rw in data.AsEnumerable()
                                 select new catalpclass()
                                 {
                                     id = Int32.Parse(Convert.ToString(rw["id"])),
                                     aimagid = Int32.Parse(Convert.ToString(rw["aimagid"])),
                                     aimagname = Convert.ToString(rw["aimagname"]),
                                     sumname = Convert.ToString(rw["sumname"]),
                                     index = Int32.Parse(Convert.ToString(rw["index"])),
                                     sname = Convert.ToString(rw["sname"]),
                                     lat = strtofloat(Convert.ToString(rw["lat"])),
                                     lon = strtofloat(Convert.ToString(rw["lon"])),
                                     rasterheight = strtofloat(Convert.ToString(rw["rasterheight"])),
                                     jan_01 = strtoint(Convert.ToString(rw["jan-01"])),
                                     jan_02 = strtoint(Convert.ToString(rw["jan-02"])),
                                     jan_03 = strtoint(Convert.ToString(rw["jan-03"])),
                                     feb_01 = strtoint(Convert.ToString(rw["feb-01"])),
                                     feb_02 = strtoint(Convert.ToString(rw["feb-02"])),
                                     feb_03 = strtoint(Convert.ToString(rw["feb-03"])),
                                     mar_01 = strtoint(Convert.ToString(rw["mar-01"])),
                                     mar_02 = strtoint(Convert.ToString(rw["mar-02"])),
                                     mar_03 = strtoint(Convert.ToString(rw["mar-03"])),
                                     apr_01 = strtoint(Convert.ToString(rw["apr-01"])),
                                     apr_02 = strtoint(Convert.ToString(rw["apr-02"])),
                                     apr_03 = strtoint(Convert.ToString(rw["apr-03"])),
                                     may_01 = strtoint(Convert.ToString(rw["may-01"])),
                                     may_02 = strtoint(Convert.ToString(rw["may-02"])),
                                     may_03 = strtoint(Convert.ToString(rw["may-03"])),
                                     jun_01 = strtoint(Convert.ToString(rw["jun-01"])),
                                     jun_02 = strtoint(Convert.ToString(rw["jun-02"])),
                                     jun_03 = strtoint(Convert.ToString(rw["jun-03"])),
                                     jul_01 = strtoint(Convert.ToString(rw["jul-01"])),
                                     jul_02 = strtoint(Convert.ToString(rw["jul-02"])),
                                     jul_03 = strtoint(Convert.ToString(rw["jul-03"])),
                                     aug_01 = strtoint(Convert.ToString(rw["aug-01"])),
                                     aug_02 = strtoint(Convert.ToString(rw["aug-02"])),
                                     aug_03 = strtoint(Convert.ToString(rw["aug-03"])),
                                     sep_01 = strtoint(Convert.ToString(rw["sep-01"])),
                                     sep_02 = strtoint(Convert.ToString(rw["sep-02"])),
                                     sep_03 = strtoint(Convert.ToString(rw["sep-03"])),
                                     oct_01 = strtoint(Convert.ToString(rw["oct-01"])),
                                     oct_02 = strtoint(Convert.ToString(rw["oct-02"])),
                                     oct_03 = strtoint(Convert.ToString(rw["oct-03"])),
                                     nov_01 = strtoint(Convert.ToString(rw["nov-01"])),
                                     nov_02 = strtoint(Convert.ToString(rw["nov-02"])),
                                     nov_03 = strtoint(Convert.ToString(rw["nov-03"])),
                                     dec_01 = strtoint(Convert.ToString(rw["dec-01"])),
                                     dec_02 = strtoint(Convert.ToString(rw["dec-02"])),
                                     dec_03 = strtoint(Convert.ToString(rw["dec-03"]))

                                 }).ToList();

            return convertedList;
        }
        public decimal strtoint(string s)
        {
            decimal ret = 0;
            if (string.IsNullOrWhiteSpace(s))
            {
                ret = 0;
            }
            else
            {
                ret = (decimal.Parse(s, System.Globalization.NumberStyles.AllowParentheses |
                System.Globalization.NumberStyles.AllowLeadingWhite |
                System.Globalization.NumberStyles.AllowTrailingWhite |
                System.Globalization.NumberStyles.AllowThousands |
                System.Globalization.NumberStyles.AllowDecimalPoint |
                System.Globalization.NumberStyles.AllowLeadingSign));
            }
            return ret;
        }
        public float strtofloat(string s)
        {
            return float.Parse(s);
        }
        public string GetCatalT()
        {
            ConnDB conn = new ConnDB();

            string sql = "select * from agro_catal_t";

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

            string sql = "select * from agro_catal_t where index = " + station + "";

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
