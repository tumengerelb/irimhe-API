using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using irimhe.Maindb;
using Newtonsoft.Json;
using Npgsql;

namespace irimhe.Models
{
    public class SnowModel
    {
        /*
         * temp model class for retrieving the data from db
         */
        public string sindex { get; set; }
        public string year { get; set; }
        public string month { get; set; }
        public string num_of_month { get; set; }
        public string Height_of_Snow { get; set; }
        public string Density_of_Snow { get; set; }
        public string Field_of_Snow { get; set; }
        public string Update_Time { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
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
            public string sindex { get; set; }
            //public string year { get; set; }
            //public string month { get; set; }
            //public string num_of_month { get; set; }
            public string date { get; set; }
            public string Height_of_Snow { get; set; }
            public string Density_of_Snow { get; set; }
            public string Field_of_Snow { get; set; }
            public string Update_Time { get; set; }
            public string lat { get; set; }
            public string lon { get; set; }
            //public SnowModel snowmodel { get; set; }
        }
        public class Geometry
        {
            public string type { get; set; }
            public float[][] coordinates { get; set; }
        }
        public string PullDataPG()
        {
            ConnDB conn = new ConnDB();
            string sql = "select t_800_83.sindex,t_800_83.year,t_800_83.month,t_800_83.num_of_month,t_800_83.height_of_snow,t_800_83.density_of_snow,t_800_83.field_of_snow,station2.lat,station2.lon " +
                "from t_800_83 inner join station2 on t_800_83.sindex = station2.sindex";
            NpgsqlCommand cmd = conn.RunCmdPG(sql);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);

            DataSet data = new DataSet();

            da.Fill(data);
            conn.ClosePG();

            string JsonString = string.Empty;
            JsonString = JsonConvert.SerializeObject(ConstructModel(DataTableToList(data.Tables[0])));


            return JsonString;
        }
        /*Generating db table to json*/
        public Rootobject ConstructModel(List<SnowModel> snow)
        {
            Rootobject rootobject = new Rootobject();
            rootobject.type = "Feature";

            rootobject.features = gen_feature(snow.Count, snow);

            return rootobject;
        }

        public List<Feature> gen_feature(int count, List<SnowModel> snow)
        {

            List<Feature> list_feat = new List<Feature>();

            for (int i = 0; i < count; i++)
            {
                list_feat.Add(new Feature
                {
                    type = "Point",
                    properties = gen_properties(i, snow),
                    geometry = gen_geometry(i, snow)
                });
            }


            return list_feat;
        }
        public Properties gen_properties(int count, List<SnowModel> temps)
        {
            //SnowModel temp = new SnowModel();
            Properties prop = new Properties();
            prop.date = temps[count].num_of_month + "/" + temps[count].month + "/" + temps[count].year;
            prop.sindex = temps[count].sindex;
            prop.Density_of_Snow = temps[count].Density_of_Snow;
            prop.Field_of_Snow = temps[count].Field_of_Snow;
            prop.Height_of_Snow = temps[count].Height_of_Snow;
            prop.lat = temps[count].lat;
            prop.lon = temps[count].lon;
            //Properties prop = new Properties();

            //prop.snowmodel = temp;

            return prop;
        }
        /*
         creating data to lat lon Geojson
             */
        public Geometry gen_geometry(int count, List<SnowModel> snow)
        {
            Geometry geometry = new Geometry();

            geometry.type = "Point";


            float lat = float.Parse(snow[count].lat);
            float lon = float.Parse(snow[count].lon);

            geometry.coordinates = new float[][]
            {
                new float[] {lat},
                new float[] {lon}
            };
            return geometry;
        }
        public List<SnowModel> DataTableToList(DataTable table)
        {
            //table to List object
            SnowModel temp = new SnowModel();

            var convertedList = (from rw in table.AsEnumerable()
                                 select new SnowModel()
                                 {
                                     sindex = Convert.ToString(rw["sindex"]),
                                     year = Convert.ToString(rw["year"]),
                                     month = Convert.ToString(rw["month"]),
                                     num_of_month = Convert.ToString(rw["num_of_month"]),
                                     lat = Convert.ToString(rw["lat"]),
                                     lon = Convert.ToString(rw["lon"]),
                                     Height_of_Snow = Convert.ToString(rw["height_of_snow"]),
                                     Density_of_Snow = Convert.ToString(rw["density_of_snow"]),
                                     Field_of_Snow = Convert.ToString(rw["field_of_snow"])

                                 }).ToList();

            return convertedList;
        }
        public class tenday
        {
            public string year { get; set; }
            public string month { get; set; }
            public string num_of_month { get; set; }
        }
        public string retDate(string s)
        {
            string ret = "";

            if( s =="01" )
            {
                ret = "01";
            }
            if (s == "1")
            {
                ret = "01";
            }
            if (s == "11")
            {
                ret = "2";
            }
            if (s == "21")
            {
                ret = "3";
            }
            return ret;
        }
        public tenday DateTimeTenDay(string dates)
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
        public string get_height_of_snow()
        {
            ConnDB conn = new ConnDB();

            string sql = "select max(height_of_snow) from t_800_83";

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
        public string get_density_of_snow()
        {
            ConnDB conn = new ConnDB();

            string sql = "select max(density_of_snow) from t_800_83";

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
        public string only_pull_snow(string height_of_snow,string density_of_snow,tenday startdate,tenday enddate)
        {
            ConnDB conn = new ConnDB();

            string sql,snow_h_max,snow_d_max = "";

            if(string.IsNullOrWhiteSpace(height_of_snow))
            {
                snow_h_max = get_height_of_snow();
            }
            else
            {
                snow_h_max = height_of_snow;
            }
            if(string.IsNullOrWhiteSpace(density_of_snow))
            {
                snow_d_max = get_density_of_snow();
            }
            else
            {
                snow_d_max = density_of_snow;
            }
            sql = "select t_800_83.sindex,t_800_83.year,t_800_83.month,t_800_83.num_of_month,t_800_83.height_of_snow,t_800_83.density_of_snow,t_800_83.field_of_snow,station2.lat,station2.lon from t_800_83 inner join station2 on t_800_83.sindex = station2.sindex " +
                        " where t_800_83.year between " + startdate.year + " and " + enddate.year + " and t_800_83.month between " + startdate.month + " and " + enddate.month + " " +
                        "and t_800_83.num_of_month between " + startdate.num_of_month + " and " + enddate.num_of_month + " and height_of_snow <= " + snow_h_max + " and density_of_snow <=" + snow_d_max + "";
            
            NpgsqlCommand cmd = conn.RunCmdPG(sql);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);

            DataSet data = new DataSet();

            da.Fill(data);
            conn.ClosePG();

            string JsonString = string.Empty;
            JsonString = JsonConvert.SerializeObject(SnowTableToList(data.Tables[0]));

            return JsonString;
        }
        public string chekdate(string s)
        {
            string ret = "";
            if (s == "1")
            {
                ret = "1";
            }
            if (s == "2")
            {
                ret = "11";
            }
            if (s == "3")
            {
                ret = "21";
            }
            return ret;
        }
        public List<snowclass> SnowTableToList(DataTable table)
        {
            //table to List object
            snowclass temp = new snowclass();

            var convertedList = (from rw in table.AsEnumerable()
                                 select new snowclass()
                                 {
                                     sindex = Convert.ToInt32(rw["sindex"]),
                                     date = Convert.ToString(rw["year"]) + "-" + Convert.ToString(rw["month"]) + "-" + chekdate(Convert.ToString(rw["num_of_month"])),
                                     lat = strtofloat(Convert.ToString(rw["lat"])),
                                     lon = strtofloat(Convert.ToString(rw["lon"])),
                                     height_of_snow = Convert.ToInt32(rw["height_of_snow"]),
                                     density_of_snow = strtofloat(Convert.ToString(rw["density_of_snow"])),
                                     field_of_snow = Convert.ToInt32(rw["field_of_snow"])
                                 }).ToList();

            return convertedList;
        }
        public decimal? strtoint(string s)
        {
            decimal? ret = 0;
            if (string.IsNullOrWhiteSpace(s))
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
        public class snowclass
        {
            public int sindex { get; set; }
            public string date { get; set; }
            public float lat { get; set; }
            public float lon { get; set; }
            public int height_of_snow { get; set; }
            public float density_of_snow { get; set; }
            public int field_of_snow { get; set; }
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

        public string PullDataPGDB_station(tenday ten)
        {
            ConnDB conn = new ConnDB();
            int year = Int32.Parse(ten.year);
            int month = Int32.Parse(ten.month);
            int num_of_month = Int32.Parse(ten.num_of_month);

            string sql = "select t_800_83.sindex,t_800_83.year,t_800_83.month,t_800_83.num_of_month,t_800_83.height_of_snow,t_800_83.density_of_snow,t_800_83.field_of_snow,station2.lat,station2.lon " +
                "from t_800_83 inner join station2 on t_800_83.sindex = station2.sindex where t_800_83.year = " + year + " and t_800_83.month = " + month + " and t_800_83.num_of_month = " + num_of_month + "";

            NpgsqlCommand cmd = conn.RunCmdPG(sql);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);

            DataSet data = new DataSet();

            da.Fill(data);
            conn.ClosePG();

            string JsonString = string.Empty;
            JsonString = JsonConvert.SerializeObject(ConstructModel(DataTableToList(data.Tables[0])));


            return JsonString;

        }

        public string PullData_station(string station,tenday ten)
        {
            ConnDB conn = new ConnDB();
            int year = Int32.Parse(ten.year);
            int month = Int32.Parse(ten.month);
            int num_of_month = Int32.Parse(ten.num_of_month);
            string sql = "select t_800_83.sindex,t_800_83.year,t_800_83.month,t_800_83.num_of_month,t_800_83.height_of_snow,t_800_83.density_of_snow,t_800_83.field_of_snow,station2.lat,station2.lon " +
                "from t_800_83 inner join station2 on t_800_83.sindex = station2.sindex where t_800_83.sindex=" + station + " and t_800_83.year="+year+" and t_800_83.month="+month+" and t_800_83.num_of_month="+num_of_month+"";

            NpgsqlCommand cmd = conn.RunCmdPG(sql);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);

            DataSet data = new DataSet();

            da.Fill(data);
            conn.ClosePG();

            string JsonString = string.Empty;
            JsonString = JsonConvert.SerializeObject(ConstructModel(DataTableToList(data.Tables[0])));

            return JsonString;
        }
        public string PullData_begin_end(tenday begindate, tenday enddate)
        {
            ConnDB conn = new ConnDB();

            string sql = "select t_800_83.fid,t_800_83.sindex,t_800_83.year,t_800_83.month,t_800_83.num_of_month,t_800_83.WW_Max,"
                + "t_800_83.TTT_Aver,t_800_83.TTT_Max ,t_800_83.Num_of_Tmax ,t_800_83.TTT_Min,t_800_80.Sum_of_RRR ,t_800_80.TxTxTxAver,"
                + "t_800_83.txtxtx_max,t_800_83.num_of_tmin,t_800_83.Num_of_RRR ,t_800_83.Num_of_Tx_Max,t_800_83.TxTxTx_Min,station2.lat,station2.lon " +
                "from t_800_83 inner join station2 on t_800_83.sindex = station2.sindex where t_800_83.year = " + begindate.year + " and t_800_83.month = " + begindate.month + " and t_800_83.num_of_month =" + begindate.num_of_month + " or t_800_83.year = " + enddate.year + " and t_800_83.month =" + enddate.month + " "
                + " and t_800_83.num_of_month =" + enddate.num_of_month + " ";

            NpgsqlCommand cmd = conn.RunCmdPG(sql);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);

            DataSet data = new DataSet();

            da.Fill(data);
            conn.ClosePG();

            string JsonString = string.Empty;
            JsonString = JsonConvert.SerializeObject(ConstructModel(DataTableToList(data.Tables[0])));

            return JsonString;
        }

        public string multi_station(string[] sindex)
        {
            ConnDB conn = new ConnDB();
            
            DataTable bgtable = new DataTable();
            DataSet data = new DataSet();

            for (int i = 0; i < sindex.Length; i++)
            {
                string sql = "select t_800_83.fid,t_800_83.sindex,t_800_83.year,t_800_83.month,t_800_83.num_of_month,t_800_83.WW_Max,"
                + "t_800_83.TTT_Aver,t_800_83.TTT_Max ,t_800_83.Num_of_Tmax ,t_800_83.TTT_Min,t_800_83.Sum_of_RRR ,t_800_83.TxTxTxAver,"
                + "t_800_83.txtxtx_max,t_800_83.num_of_tmin,t_800_83.Num_of_RRR ,t_800_83.Num_of_Tx_Max,t_800_83.TxTxTx_Min,station2.lat,station2.lon " +
                "from t_800_83 inner join station2 on t_800_83.sindex = " + sindex[i] + "";

                NpgsqlCommand cmd = conn.RunCmdPG(sql);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);

                da.Fill(data);               
            }
            conn.ClosePG();

            for (int i = 0; i < data.Tables.Count; i++)
            {
                bgtable.Merge(data.Tables[i]);
            }

            string JsonString = "";
            JsonString = JsonConvert.SerializeObject(ConstructModel(DataTableToList(bgtable)));

            return JsonString;
        }

        public class garig
        {
            public string date { get; set; }
        }
        public string udur()
        {
            ConnDB conn = new ConnDB();

            DataSet data = new DataSet();
            string sql = "select distinct t_800_83.month , t_800_83.year, t_800_83.num_of_month from t_800_83 order by t_800_83.year,t_800_83.month,t_800_83.num_of_month ASC";
            NpgsqlCommand cmd = conn.RunCmdPG(sql);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);

            da.Fill(data);
            conn.ClosePG();

            string JsonString = string.Empty;
            JsonString = JsonConvert.SerializeObject(GaragTableToList(data.Tables[0]));

            return JsonString;

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
    }
}
