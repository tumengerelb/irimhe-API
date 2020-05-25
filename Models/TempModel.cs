using System;
using irimhe.Maindb;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using Npgsql;
using System.Collections.Generic;
using System.Linq;

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
            public string year { get; set; }
            public string month { get; set; }
            public string num_of_month { get; set; }
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
            properties.month = temps[count].month;
            properties.year = temps[count].year;
            properties.sindex = temps[count].sindex;
            properties.num_of_month = temps[count].num_of_month;
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

        public tenday DateTimeTotenDay(DateTime dates)
        {
            tenday ten = new tenday();

            ten.year = dates.ToString("yyyy");
            ten.month = dates.ToString("MM");

            string day = dates.ToString("dd");
            int dayas = Int32.Parse(day);
            ten.num_of_month = num_of_calc(dayas).ToString();

            return ten;
        }
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
    }
}
