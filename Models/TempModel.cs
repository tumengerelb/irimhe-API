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
    public class TempModel
    {

        public int sindex { get; set; }
        public string year { get; set; }
        public string month { get; set; }
        public string num_of_month { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
        public string fid { get; set; }
        public string www_max { get; set; }
        public string ttt_aver { get; set; }
        public string ttt_max { get; set; }
        public string num_of_tmax { get; set; }
        public string ttt_min { get; set; }
        public string num_of_tmin { get; set; }
        public string sum_of_rrr { get; set; }
        public string txtxtx_aver { get; set; }
        public string num_of_rrr { get; set; }
        public string txtxtx_max { get; set; }
        public string num_of_tx_max { get; set; }
        public string txtxtx_min { get; set; }
        public class Rootobject
        {
            public string type { get; set; }
            public List<Feature> features { get; set; }
        }
        public class Feature
        {
            public string type { get; set; }
            public Properties properties { get; set; }
            public Geometry geometry { get; set; }
        }
        public class Properties
        {
            public int sindex { get; set; }
            public string date { get; set; }
            public string lat { get; set; }
            public string lon { get; set; }
            //public string fid { get; set; }
            public string www_max { get; set; }
            public string ttt_aver { get; set; }
            public string ttt_max { get; set; }
            //public string num_of_tmax { get; set; }
            public string ttt_min { get; set; }
            //public string num_of_tmin { get; set; }
            public string sum_of_rrr { get; set; }
            public string txtxtx_aver { get; set; }
            public string num_of_rrr { get; set; }
            public string txtxtx_max { get; set; }
            public string num_of_tx_max { get; set; }
            public string txtxtx_min { get; set; }
        }
        public class Geometry
        {
            public string type { get; set; }
            public float[,] coordinates { get; set; }
        }

        private DataTable dataTable = new DataTable();

        public string PullData()
        {
            ConnDB conn = new ConnDB();

            string sql = "select * from t_800_80";

            SqlCommand cmd = conn.RunCmd(sql);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(dataTable);

            conn.Close();

            return DataTableToJson(dataTable);
        }
        public string DataTableToJson(DataTable table)
        {
            string JsonString = string.Empty;
            JsonString = JsonConvert.SerializeObject(table);
            return JsonString;
        }
        public string PullDataPG()
        {
            ConnDB conn = new ConnDB();
            /*string sql = "select t_800_80.fid,t_800_80.sindex,t_800_80.year,t_800_80.month,t_800_80.num_of_month,t_800_80.WW_Max,t_800_80.TTT_Aver,t_800_80.TTT_Max,t_800_80.Num_of_TMax," +
                "t_800_80.TTT_Min,t_800_80.Sum_of_RRR,t_800_80.TxTxTxAver,t_800_80.Num_of_RRR,t_800_80.Num_of_Tx_Max,t_800_80.TxTxTx_Min" +
                " ,station2.lat,station2.lon from t_800_80 inner join station2 on t_800_80.sindex = station2.sindex";*/

            string sql = "select t_800_80.fid,t_800_80.sindex,t_800_80.year,t_800_80.month,t_800_80.num_of_month,t_800_80.WW_Max,"
                + "t_800_80.TTT_Aver,t_800_80.TTT_Max ,t_800_80.Num_of_Tmax ,t_800_80.TTT_Min,t_800_80.Sum_of_RRR ,t_800_80.TxTxTxAver,"
                + "t_800_80.txtxtx_max,t_800_80.num_of_tmin,t_800_80.Num_of_RRR ,t_800_80.Num_of_Tx_Max,t_800_80.TxTxTx_Min,station2.lat,station2.lon " +
                "from t_800_80 inner join station2 on t_800_80.sindex = station2.sindex";
            NpgsqlCommand cmd = conn.RunCmdPG(sql);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);

            DataSet data = new DataSet();

            da.Fill(data);
            conn.ClosePG();

            string JsonString = string.Empty;
            JsonString = JsonConvert.SerializeObject(ConstructModel(DataTableToList(data.Tables[0])));


            return JsonString;

        }
        public Rootobject ConstructModel(List<TempModel> temp)
        {
            Rootobject rootobject = new Rootobject();
            rootobject.type = "Feature";

            rootobject.features = gen_feature(temp.Count, temp);

            return rootobject;
        }

        public List<Feature> gen_feature(int count, List<TempModel> temp)
        {

            List<Feature> list_feat = new List<Feature>();
            Properties properties = new Properties();
            for (int i = 0; i < count; i++)
            {
                list_feat.Add(new Feature
                {
                    type = "Point",
                    //properties.sindex = temp[count].sindex
                    properties = gen_properties(i, temp),
                    geometry = gen_geometry(i, temp)
                });
            }


            return list_feat;
        }
        public Properties gen_properties(int count, List<TempModel> temps)
        {
            //TempModel temp = new TempModel();
            Properties properties = new Properties();
            //properties.month = temps[count].month;
            //properties.year = temps[count].year;
            properties.sindex = temps[count].sindex;
            //properties.num_of_month = temps[count].num_of_month;

            properties.date = temps[count].num_of_month + "/" + temps[count].month + "/" + temps[count].year;
            properties.lat = temps[count].lat;
            properties.lon = temps[count].lon;
            //properties.fid = temps[count].fid;
            properties.www_max = temps[count].www_max;
            properties.ttt_aver = temps[count].ttt_aver;
            properties.ttt_max = temps[count].ttt_max;
            //properties.num_of_tmax = temps[count].fid;
            properties.ttt_min = temps[count].ttt_min;
            //properties.num_of_tmin = temps[count].fid;
            properties.sum_of_rrr = temps[count].sum_of_rrr;
            properties.txtxtx_aver = temps[count].txtxtx_aver;
            properties.num_of_rrr = temps[count].num_of_rrr;
            properties.txtxtx_max = temps[count].txtxtx_max;
            properties.num_of_tx_max = temps[count].num_of_tx_max;
            properties.txtxtx_min = temps[count].txtxtx_min;

            return properties;
        }
        public class tumee
        {
            public double[][] geom { get; set; }
        }
        public Geometry gen_geometry(int count, List<TempModel> temps)
        {
            Geometry geometry = new Geometry();
            geometry.type = "Point";


            float lat = float.Parse(temps[count].lat);
            float lon = float.Parse(temps[count].lon);

            geometry.coordinates = new float[,] { { lat }, { lon } };
            /*{
                new float[] {lat},
                new float[] {lon}
            };*/

            //geometry.coordinates = [float.Parse(lat)] + [float.Parse(lon)];
            //geometry.coordinates=(lat = "[" + temps[count].lat + "," + temps[count].lon + "]");
            return geometry;
        }
        public List<TempModel> DataTableToList(DataTable table)
        {
            //table to List object
            TempModel temp = new TempModel();

            var convertedList = (from rw in table.AsEnumerable()
                                 select new TempModel()
                                 {
                                     sindex = Convert.ToInt32(rw["sindex"]),
                                     year = Convert.ToString(rw["year"]),
                                     month = Convert.ToString(rw["month"]),
                                     num_of_month = Convert.ToString(rw["num_of_month"]),
                                     lat = Convert.ToString(rw["lat"]),
                                     lon = Convert.ToString(rw["lon"]),
                                     fid = Convert.ToString(rw["fid"]),
                                     www_max = Convert.ToString(rw["ww_max"]),
                                     ttt_aver = Convert.ToString(rw["ttt_aver"]),
                                     ttt_max = Convert.ToString(rw["ttt_max"]),
                                     num_of_tmax = Convert.ToString(rw["num_of_tmax"]),
                                     ttt_min = Convert.ToString(rw["ttt_min"]),
                                     num_of_tmin = Convert.ToString(rw["num_of_tmin"]),
                                     sum_of_rrr = Convert.ToString(rw["sum_of_rrr"]),
                                     txtxtx_aver = Convert.ToString(rw["txtxtxaver"]),
                                     num_of_rrr = Convert.ToString(rw["num_of_rrr"]),
                                     txtxtx_max = Convert.ToString(rw["txtxtx_max"]),
                                     num_of_tx_max = Convert.ToString(rw["num_of_tx_max"]),
                                     txtxtx_min = Convert.ToString(rw["txtxtx_min"])
                                 }).ToList();

            return convertedList;
        }
        public class tenday
        {
            public string year { get; set; }
            public string month { get; set; }
            public string num_of_month { get; set; }
        }
        public int num_of_calc(int calc)
        {
            int ret = 0;
            if (calc <= 10 && calc >= 1)
            {
                ret = 1;
            }
            if (calc <= 20 && calc >= 11)
            {
                ret = 2;
            }
            if (calc <= 30 && calc >= 21)
            {
                ret = 3;
            }
            if (calc == 31)
            {
                ret = 3;
            }

            return ret;
        }
        /*
         * reading datetime to and pushing it a model class
         */
        public string retDate(string s)
        {
            string ret = "";
            if(s =="01")
            {
                ret = "1";
            }
            if (s =="1")
            {
                ret = "1";
            }
            if(s =="11")
            {
                ret = "2";
            }
            if(s =="21")
            {
                ret = "3";
            }
                return ret;
        }
        public tenday DateTimeTotenDay(string dates)
        {
            string[] s = dates.Split('-');
            tenday ten = new tenday();

            ten.year = s[0];
            ten.month = s[1];

            string day = retDate(s[2]);
            
            //int dayas = Int32.Parse(day);
            //ten.num_of_month = num_of_calc(dayas).ToString();

            ten.num_of_month = day;

            return ten;
        }
        /*
         * Pull every from t_800_80
         */
        public string PullDataPGDB(tenday ten)
        {

            ConnDB conn = new ConnDB();
            int year = Int32.Parse(ten.year);
            int month = Int32.Parse(ten.month);
            int num_of_month = Int32.Parse(ten.num_of_month);

            string sql = "select t_800_80.fid,t_800_80.sindex,t_800_80.year,t_800_80.month,t_800_80.num_of_month,t_800_80.WW_Max,"
                + "t_800_80.TTT_Aver,t_800_80.TTT_Max ,t_800_80.Num_of_Tmax ,t_800_80.TTT_Min,t_800_80.Sum_of_RRR ,t_800_80.TxTxTxAver,"
                + "t_800_80.txtxtx_max,t_800_80.num_of_tmin,t_800_80.Num_of_RRR ,t_800_80.Num_of_Tx_Max,t_800_80.TxTxTx_Min,station2.lat,station2.lon " +
                "from t_800_80 inner join station2 on t_800_80.sindex = station2.sindex where t_800_80.year = " + year + " and t_800_80.month = " + month + " and t_800_80.num_of_month = " + num_of_month + "";

            NpgsqlCommand cmd = conn.RunCmdPG(sql);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);

            DataSet data = new DataSet();

            da.Fill(data);
            conn.ClosePG();

            string JsonString = string.Empty;
            JsonString = JsonConvert.SerializeObject(ConstructModel(DataTableToList(data.Tables[0])));


            return JsonString;

        }
        /*
         Pull's data with station and date time range
             */

        public string PullDataPGDB_station(tenday ten, string station)
        {
            ConnDB conn = new ConnDB();
            int year = Int32.Parse(ten.year);
            int month = Int32.Parse(ten.month);
            int num_of_month = Int32.Parse(ten.num_of_month);

            string sql = "select t_800_80.fid,t_800_80.sindex,t_800_80.year,t_800_80.month,t_800_80.num_of_month,t_800_80.WW_Max,"
                + "t_800_80.TTT_Aver,t_800_80.TTT_Max ,t_800_80.Num_of_Tmax ,t_800_80.TTT_Min,t_800_80.Sum_of_RRR ,t_800_80.TxTxTxAver,"
                + "t_800_80.txtxtx_max,t_800_80.num_of_tmin,t_800_80.Num_of_RRR ,t_800_80.Num_of_Tx_Max,t_800_80.TxTxTx_Min,station2.lat,station2.lon " +
                "from t_800_80 inner join station2 on t_800_80.sindex = station2.sindex where t_800_80.year = " + year + " and t_800_80.month = " + month + " and t_800_80.num_of_month = " + num_of_month + " and t_800_80.sindex =" + station + "";

            NpgsqlCommand cmd = conn.RunCmdPG(sql);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);

            DataSet data = new DataSet();

            da.Fill(data);
            conn.ClosePG();

            string JsonString = string.Empty;
            JsonString = JsonConvert.SerializeObject(ConstructModel(DataTableToList(data.Tables[0])));


            return JsonString;

        }
        /*
         Pull's database only station index
             */
        public string PullData_station(string station)
        {
            ConnDB conn = new ConnDB();

            string sql = "select t_800_80.fid,t_800_80.sindex,t_800_80.year,t_800_80.month,t_800_80.num_of_month,t_800_80.WW_Max,"
                + "t_800_80.TTT_Aver,t_800_80.TTT_Max ,t_800_80.Num_of_Tmax ,t_800_80.TTT_Min,t_800_80.Sum_of_RRR ,t_800_80.TxTxTxAver,"
                + "t_800_80.txtxtx_max,t_800_80.num_of_tmin,t_800_80.Num_of_RRR ,t_800_80.Num_of_Tx_Max,t_800_80.TxTxTx_Min,station2.lat,station2.lon " +
                "from t_800_80 inner join station2 on t_800_80.sindex = station2.sindex where t_800_80.station = " + station + "";

            NpgsqlCommand cmd = conn.RunCmdPG(sql);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);

            DataSet data = new DataSet();

            da.Fill(data);
            conn.ClosePG();

            string JsonString = string.Empty;
            JsonString = JsonConvert.SerializeObject(ConstructModel(DataTableToList(data.Tables[0])));

            return JsonString;
        }
        /*
         Pull's data from database wiht date ranges begindate,enddate
             */
        /*Get's data from db min max*/
        public string get_max_temp()
        {
            ConnDB conn = new ConnDB();

            string sql = "select max(ttt_max) from t_800_80";

            NpgsqlCommand cmd = conn.RunCmdPG(sql);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            string t_max = "";

            while (reader.Read())
            {
                t_max = Convert.ToString(reader["max"]);
            }
            conn.ClosePG();
            return t_max;
        }
        public string get_min_temp()
        {
            ConnDB conn = new ConnDB();

            string sql = "select min(ttt_min) from t_800_80";

            NpgsqlCommand cmd = conn.RunCmdPG(sql);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            string t_min = "";

            while (reader.Read())
            {
                t_min = Convert.ToString(reader["min"]);
            }
            conn.ClosePG();
            return t_min;
        }
        public class soilclass
        {
            public int sindex { get; set; }
            public string date { get; set; }
            public float lat { get; set; }
            public float lon { get; set; }
            public decimal? txtxtxaver { get; set; }
            public decimal? txtxtx_max { get; set; }
            public decimal? txtxtx_min { get; set; }
            public decimal? num_of_tx_max { get; set; }
        }
        public string get_soil_min()
        {
            ConnDB conn = new ConnDB();

            string sql = "select min(txtxtx_min) from t_800_80";

            NpgsqlCommand cmd = conn.RunCmdPG(sql);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            string t_min = "";

            while (reader.Read())
            {
                t_min = Convert.ToString(reader["min"]);
            }
            conn.ClosePG();
            return t_min;
        }
        public string get_soil_max()
        {
            ConnDB conn = new ConnDB();

            string sql = "select max(txtxtx_min) from t_800_80";

            NpgsqlCommand cmd = conn.RunCmdPG(sql);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            string t_min = "";

            while (reader.Read())
            {
                t_min = Convert.ToString(reader["max"]);
            }
            conn.ClosePG();
            return t_min;
        }
        public string only_pull_soil(string tx_aver,string tx_max,string tx_min,tenday startdate,tenday enddate)
        {
            ConnDB conn = new ConnDB();
            string sql = "";
            
            if(string.IsNullOrWhiteSpace(tx_min))
            {
                tx_max = get_soil_max();
            }
            if(string.IsNullOrWhiteSpace(tx_min))
            {
                tx_min = get_soil_min();
            }
            if (string.IsNullOrWhiteSpace(tx_aver))
            {
                sql = "select t_800_80.sindex,t_800_80.year,t_800_80.month,t_800_80.num_of_month,t_800_80.txtxtxaver,t_800_80.txtxtx_max,t_800_80.txtxtx_min,t_800_80.num_of_tx_max,station2.lat,station2.lon from t_800_80 inner join station2 on t_800_80.sindex = station2.sindex " +
                        " where t_800_80.year between " + startdate.year + " and " + enddate.year + " and t_800_80.month between " + startdate.month + " and " + enddate.month + " " +
                        "and t_800_80.num_of_month between " + startdate.num_of_month + " and " + enddate.num_of_month + " and ttt_min >= " + tx_min + " and ttt_max <=" + tx_max + "";
            }
            else
            {
                sql = "select t_800_80.sindex,t_800_80.year,t_800_80.month,t_800_80.num_of_month,t_800_80.txtxtxaver,t_800_80.txtxtx_max,t_800_80.txtxtx_min,t_800_80.num_of_tx_max,station2.lat,station2.lon from t_800_80 inner join station2 on t_800_80.sindex = station2.sindex " +
                    " where t_800_80.year between " + startdate.year + " and " + enddate.year + " and t_800_80.month between " + startdate.month + " and " + enddate.month + " " +
                    "and t_800_80.num_of_month between " + startdate.num_of_month + " and " + enddate.num_of_month + " and ttt_min >= " + tx_max + " and ttt_max <=" + tx_max + " and txtxtx_aver ="+tx_aver+"";
            }

            NpgsqlCommand cmd = conn.RunCmdPG(sql);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);

            DataSet data = new DataSet();

            da.Fill(data);
            conn.ClosePG();

            string JsonString = string.Empty;
            JsonString = JsonConvert.SerializeObject(SoilTableToList(data.Tables[0]));

            return JsonString;
            
        }
        public List<soilclass> SoilTableToList(DataTable table)
        {
            //table to List object
            soilclass temp = new soilclass();

            var convertedList = (from rw in table.AsEnumerable()
                                 select new soilclass()
                                 {
                                     sindex = Convert.ToInt32(rw["sindex"]),
                                     date = Convert.ToString(rw["year"]) + "-" + Convert.ToString(rw["month"]) + "-" + chekdate(Convert.ToString(rw["num_of_month"])),
                                     lat = strtofloat(Convert.ToString(rw["lat"])),
                                     lon = strtofloat(Convert.ToString(rw["lon"])),
                                     txtxtxaver = strtoint(Convert.ToString(rw["txtxtxaver"])),
                                     txtxtx_max = strtoint(Convert.ToString(rw["txtxtx_max"])),
                                     txtxtx_min = strtoint(Convert.ToString(rw["txtxtx_min"])),
                                     num_of_tx_max = strtoint(Convert.ToString(rw["num_of_tx_max"])),                                     
                                 }).ToList();

            return convertedList;
        }
        public class windclass
        {
            public int sindex { get; set; }
            public string date { get; set; }
            public float lat { get; set; }
            public float lon { get; set; }
            public decimal? wind_max { get; set; }            
        }
        public string get_wind_max()
        {
            ConnDB conn = new ConnDB();

            string sql = "select max(ww_max) from t_800_80";

            NpgsqlCommand cmd = conn.RunCmdPG(sql);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            string t_min = "";

            while (reader.Read())
            {
                t_min = Convert.ToString(reader["max"]);
            }
            conn.ClosePG();
            return t_min;
        }
        public string only_pull_wind(string wind_max, tenday startdate,tenday enddate)
        {
            ConnDB conn = new ConnDB();
            string sql = "";

            
            if (string.IsNullOrWhiteSpace(wind_max))
            {
                wind_max = get_wind_max();
                sql = "select t_800_80.sindex,t_800_80.year,t_800_80.month,t_800_80.num_of_month,t_800_80.ww_max,station2.lat,station2.lon from t_800_80 inner join station2 on t_800_80.sindex = station2.sindex " +
                        " where t_800_80.year between " + startdate.year + " and " + enddate.year + " and t_800_80.month between " + startdate.month + " and " + enddate.month + " " +
                        "and t_800_80.num_of_month between " + startdate.num_of_month + " and " + enddate.num_of_month + " and ww_max <= " + wind_max + " ";
            }
            else
            {
                sql = "select t_800_80.sindex,t_800_80.year,t_800_80.month,t_800_80.num_of_month,t_800_80.ww_max,station2.lat,station2.lon from t_800_80 inner join station2 on t_800_80.sindex = station2.sindex " +
                    " where t_800_80.year between " + startdate.year + " and " + enddate.year + " and t_800_80.month between " + startdate.month + " and " + enddate.month + " " +
                    "and t_800_80.num_of_month between " + startdate.num_of_month + " and " + enddate.num_of_month + " and ww_max <= " + wind_max + " ";
            }

            NpgsqlCommand cmd = conn.RunCmdPG(sql);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);

            DataSet data = new DataSet();

            da.Fill(data);
            conn.ClosePG();

            string JsonString = string.Empty;
            JsonString = JsonConvert.SerializeObject(WindTableToList(data.Tables[0]));

            return JsonString;
        }
        public List<windclass> WindTableToList(DataTable table)
        {
            //table to List object
            windclass temp = new windclass();

            var convertedList = (from rw in table.AsEnumerable()
                                 select new windclass()
                                 {
                                     sindex = Convert.ToInt32(rw["sindex"]),
                                     date = Convert.ToString(rw["year"]) + "-" + Convert.ToString(rw["month"]) + "-" + chekdate(Convert.ToString(rw["num_of_month"])),
                                     lat = strtofloat(Convert.ToString(rw["lat"])),
                                     lon = strtofloat(Convert.ToString(rw["lon"])),
                                     wind_max = strtoint(Convert.ToString(rw["ww_max"]))
                                     
                                 }).ToList();

            return convertedList;
        }

        public class precipitationclass
        {
            public int sindex { get; set; }
            public string date { get; set; }
            public float lat { get; set; }
            public float lon { get; set; }
            public decimal? sum_rrr { get; set; }
        }
        public string get_perc_max()
        {
            ConnDB conn = new ConnDB();

            string sql = "select max(sum_of_rrr) from t_800_80";

            NpgsqlCommand cmd = conn.RunCmdPG(sql);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            string t_min = "";

            while (reader.Read())
            {
                t_min = Convert.ToString(reader["max"]);
            }
            conn.ClosePG();
            return t_min;
        }
        public string only_pull_percipitation(string p_max,tenday startdate,tenday enddate)
        {
            ConnDB conn = new ConnDB();
            string sql = "";


            if (string.IsNullOrWhiteSpace(p_max))
            {
                p_max = get_perc_max();
                sql = "select t_800_80.sindex,t_800_80.year,t_800_80.month,t_800_80.num_of_month,t_800_80.sum_of_rrr,station2.lat,station2.lon from t_800_80 inner join station2 on t_800_80.sindex = station2.sindex " +
                        " where t_800_80.year between " + startdate.year + " and " + enddate.year + " and t_800_80.month between " + startdate.month + " and " + enddate.month + " " +
                        "and t_800_80.num_of_month between " + startdate.num_of_month + " and " + enddate.num_of_month + " and sum_of_rrr <= " + p_max + " ";
            }
            else
            {
                sql = "select t_800_80.sindex,t_800_80.year,t_800_80.month,t_800_80.num_of_month,t_800_80.sum_of_rrr,station2.lat,station2.lon from t_800_80 inner join station2 on t_800_80.sindex = station2.sindex " +
                    " where t_800_80.year between " + startdate.year + " and " + enddate.year + " and t_800_80.month between " + startdate.month + " and " + enddate.month + " " +
                    "and t_800_80.num_of_month between " + startdate.num_of_month + " and " + enddate.num_of_month + " and sum_of_rrr <= " + p_max + " ";
            }

            NpgsqlCommand cmd = conn.RunCmdPG(sql);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);

            DataSet data = new DataSet();

            da.Fill(data);
            conn.ClosePG();

            string JsonString = string.Empty;
            JsonString = JsonConvert.SerializeObject(PercTableToList(data.Tables[0]));

            return JsonString;
        }
        public List<precipitationclass> PercTableToList(DataTable table)
        {
            //table to List object
            precipitationclass temp = new precipitationclass();

            var convertedList = (from rw in table.AsEnumerable()
                                 select new precipitationclass()
                                 {
                                     sindex = Convert.ToInt32(rw["sindex"]),
                                     date = Convert.ToString(rw["year"]) + "-" + Convert.ToString(rw["month"]) + "-" + chekdate(Convert.ToString(rw["num_of_month"])),
                                     lat = strtofloat(Convert.ToString(rw["lat"])),
                                     lon = strtofloat(Convert.ToString(rw["lon"])),
                                     sum_rrr = strtoint(Convert.ToString(rw["sum_of_rrr"]))

                                 }).ToList();

            return convertedList;
        }
        public string chekdate(string s)
        {
            string ret = "";

            if( s == "01" )
            {
                ret = "01";
            }
            if(s =="1")
            {
                ret = "01";
            }
            if(s =="2")
            {
                ret = "11";
            }
            if(s =="3")
            {
                ret = "21";
            }
            return ret;
        }
        public string only_pull_temp(string ttt_aver,string ttt_max,string ttt_min,tenday startdate,tenday enddate)
        {
            ConnDB conn = new ConnDB();
            string tmax,tmin,sql = "";
            /*
            if(string.IsNullOrWhiteSpace(ttt_aver) || string.IsNullOrWhiteSpace(ttt_max) || string.IsNullOrWhiteSpace(ttt_min) )
            {

            }*/
            if(string.IsNullOrWhiteSpace(ttt_max))
            {
                tmax = get_max_temp();
            }
            else
            {
                tmax = ttt_max;
            }
            if(string.IsNullOrWhiteSpace(ttt_min))
            {
                tmin = get_min_temp();
            }
            else
            {
                tmin = ttt_min;
            }
            if(string.IsNullOrWhiteSpace(ttt_aver))
            {
                sql = "select t_800_80.sindex,t_800_80.year,t_800_80.month,t_800_80.num_of_month,t_800_80.ttt_aver,t_800_80.ttt_max,t_800_80.ttt_min,t_800_80.num_of_tmin,t_800_80.num_of_tmax,station2.lat,station2.lon from t_800_80 inner join station2 on t_800_80.sindex = station2.sindex " +
                    " where t_800_80.year between " + startdate.year + " and " + enddate.year + " and t_800_80.month between " + startdate.month + " and " + enddate.month + " " +
                    "and t_800_80.num_of_month between " + startdate.num_of_month + " and " + enddate.num_of_month + " and ttt_min >= "+tmin+" and ttt_max <="+tmax+"";
            }
            else
            {
                sql = "select t_800_80.sindex,t_800_80.year,t_800_80.month,t_800_80.num_of_month,t_800_80.ttt_aver,t_800_80.ttt_max,t_800_80.ttt_min,t_800_80.num_of_tmin,t_800_80.num_of_tmax,station2.lat,station2.lon from t_800_80 inner join station2 on t_800_80.sindex = station2.sindex " +
                    " where t_800_80.year between " + startdate.year + " and " + enddate.year + " and t_800_80.month between " + startdate.month + " and " + enddate.month + " " +
                    "and t_800_80.num_of_month between " + startdate.num_of_month + " and " + enddate.num_of_month + " and ttt_min >= " + tmin + " and ttt_max <=" + tmax + " and ttt_aver >="+ttt_aver+"";

            }

            /*            string sql = "select t_800_80.sindex,t_800_80.year,t_800_80.month,t_800_80.num_of_month,t_800_80.ttt_aver,t_800_80.ttt_max,t_800_80.ttt_min,t_800_80.num_of_tmin,t_800_80.num_of_tmax,station2.lat,station2.lon from t_800_80 inner join station2 on t_800_80.sindex = station2.sindex " +
                                " where t_800_80.year between "+startdate.year+" and "+enddate.year+" and t_800_80.month between "+startdate.month+" and "+enddate.month+" " +
                                "and t_800_80.num_of_month between "+startdate.num_of_month+" and "+enddate.num_of_month+" ";
              */
            NpgsqlCommand cmd = conn.RunCmdPG(sql);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);

            DataSet data = new DataSet();

            da.Fill(data);
            conn.ClosePG();

            string JsonString = string.Empty;
            JsonString = JsonConvert.SerializeObject(TempTableToList(data.Tables[0]));

            return JsonString;

        }
        public class tempclass
        {
            public int sindex { get; set; }            
            public string date { get; set; }
            public float lat { get; set; }
            public float lon { get; set; }
            public decimal? ttt_aver { get; set;}
            public decimal? ttt_min { get; set; }
            public decimal? ttt_max { get; set; }
            public decimal? num_of_tmax { get; set; }
            public decimal? num_of_tmin { get; set; }
            
        }
       
        public List<tempclass> TempTableToList(DataTable table)
        {
            //table to List object
            tempclass temp = new tempclass();

            var convertedList = (from rw in table.AsEnumerable()
                                 select new tempclass()
                                 {
                                     sindex = Convert.ToInt32(rw["sindex"]),                                     
                                     date = Convert.ToString(rw["year"])+"-"+ Convert.ToString(rw["month"])+"-"+ chekdate(Convert.ToString(rw["num_of_month"])),
                                     lat = strtofloat(Convert.ToString(rw["lat"])),
                                     lon = strtofloat(Convert.ToString(rw["lon"])),
                                     ttt_aver = strtoint(Convert.ToString(rw["ttt_aver"])),
                                     ttt_max = strtoint(Convert.ToString(rw["ttt_max"])),                                     
                                     ttt_min = strtoint(Convert.ToString(rw["ttt_min"])),
                                     num_of_tmin = strtoint(Convert.ToString(rw["num_of_tmin"])),
                                     num_of_tmax = strtoint(Convert.ToString(rw["num_of_tmax"])),
                                 }).ToList();

            return convertedList;
        }
        public decimal? strtoint(string s)
        {
            decimal? ret = 0;
            if(string.IsNullOrWhiteSpace(s))
            {
                ret = null;
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

        public string checknull(string s)
        {
            string ret = "";

            if(string.IsNullOrWhiteSpace(s))
            {
                ret = null;
            }
            else
            {
                ret = s;
            }
            return ret;
        }
        public string PullData_begin_end(tenday begindate,tenday enddate)
        {
            ConnDB conn = new ConnDB();

            string sql = "select t_800_80.fid,t_800_80.sindex,t_800_80.year,t_800_80.month,t_800_80.num_of_month,t_800_80.WW_Max,"
                + "t_800_80.TTT_Aver,t_800_80.TTT_Max ,t_800_80.Num_of_Tmax ,t_800_80.TTT_Min,t_800_80.Sum_of_RRR ,t_800_80.TxTxTxAver,"
                + "t_800_80.txtxtx_max,t_800_80.num_of_tmin,t_800_80.Num_of_RRR ,t_800_80.Num_of_Tx_Max,t_800_80.TxTxTx_Min,station2.lat,station2.lon " +
                "from t_800_80 inner join station2 on t_800_80.sindex = station2.sindex where t_800_80.year = "+begindate.year+ " and t_800_80.month = "+begindate.month +" and t_800_80.num_of_month ="+begindate.num_of_month+" or t_800_80.year = "+enddate.year+" and t_800_80.month =" +enddate.month+ " " 
                +" and t_800_80.num_of_month ="+enddate.num_of_month+" ";

            NpgsqlCommand cmd = conn.RunCmdPG(sql);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);

            DataSet data = new DataSet();

            da.Fill(data);
            conn.ClosePG();

            string JsonString = string.Empty;
            JsonString = JsonConvert.SerializeObject(ConstructModel(DataTableToList(data.Tables[0])));

            return JsonString;
        }
        /*
         2020-09-23
         */
        public class garig
        {
           public string date { get; set; }           
        }
        public string udur()
        {
            ConnDB conn = new ConnDB();

            DataSet data = new DataSet();
            string sql = "select distinct t_800_80.month , t_800_80.year, t_800_80.num_of_month from t_800_80 order by t_800_80.year,t_800_80.month,t_800_80.num_of_month ASC";
            NpgsqlCommand cmd = conn.RunCmdPG(sql);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);

            da.Fill(data);
            conn.ClosePG();

            string JsonString = string.Empty;
            JsonString = JsonConvert.SerializeObject(GaragTableToList(data.Tables[0]));

            return JsonString;

            return "OK";
        }
        public List<garig> GaragTableToList(DataTable table)
        {
            //table to List object
            garig temp = new garig();

            var convertedList = (from rw in table.AsEnumerable()
                                 select new garig()
                                 {   
                                     date = Convert.ToString(rw["year"]) + "-" + Convert.ToString(rw["month"]) + "-" + chekdate(Convert.ToString(rw["num_of_month"])),                                     

                                 }).ToList();

            return convertedList;
        }
        /*
         2020-09-23
         */
        public string multi_station(string[] sindex)
        {
            ConnDB conn = new ConnDB();
            
            DataTable bgtable = new DataTable();
            DataSet data = new DataSet();
            for (int i = 0; i < sindex.Length; i++)
            {
                string sql = "select t_800_80.fid,t_800_80.sindex,t_800_80.year,t_800_80.month,t_800_80.num_of_month,t_800_80.WW_Max,"
                + "t_800_80.TTT_Aver,t_800_80.TTT_Max ,t_800_80.Num_of_Tmax ,t_800_80.TTT_Min,t_800_80.Sum_of_RRR ,t_800_80.TxTxTxAver,"
                + "t_800_80.txtxtx_max,t_800_80.num_of_tmin,t_800_80.Num_of_RRR ,t_800_80.Num_of_Tx_Max,t_800_80.TxTxTx_Min,station2.lat,station2.lon " +
                "from t_800_80 inner join station2 on t_800_80.sindex = " + sindex[i] + "";

                NpgsqlCommand cmd = conn.RunCmdPG(sql);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);

                da.Fill(data);
                
            }

            conn.ClosePG();

            for(int i=0;i< data.Tables.Count;i++)
            {
                bgtable.Merge(data.Tables[i]);
            }
            
            string JsonString = "";
            JsonString = JsonConvert.SerializeObject(ConstructModel(DataTableToList(bgtable)));
                        
            return JsonString;
        }
    }
}
