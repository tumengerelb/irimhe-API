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
    public class SnowDirectionModel
    {
        public string sid { get; set; }
        public int sindex { get; set; }
        public string year { get; set; }
        public string month { get; set; }
        public string num_of_month { get; set; }
        public string height_snow_west { get; set; }
        public string density_snow_west { get; set; }
        public string height_snow_north { get; set; }
        public string density_snow_north { get; set; }
        public string height_snow_east { get; set; }
        public string height_snow_south { get; set; }
        public string field_of_west { get; set; }
        public string field_of_north { get; set; }
        public string field_of_east { get; set; }
        public string field_of_south { get; set; }
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
            //public string sid { get; set; }
            public int sindex { get; set; }
            public string year { get; set; }
            public string month { get; set; }
            public string num_of_month { get; set; }
            public string height_snow_west { get; set; }
            public string density_snow_west { get; set; }
            public string height_snow_north { get; set; }
            public string density_snow_north { get; set; }
            public string height_snow_east { get; set; }
            public string height_snow_south { get; set; }
            public string field_of_west { get; set; }
            public string field_of_north { get; set; }
            public string field_of_east { get; set; }
            public string field_of_south { get; set; }
            public string lat { get; set; }
            public string lon { get; set; }
        }
        public class Geometry
        {
            public string type { get; set; }
            public float[][] coordinates { get; set; }
        }
        private DataTable dataTable = new DataTable();

        public string PullData()
        {
            ConnDB conn = new ConnDB();

            string sql = "select * from t_800_93";

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
            string sql = "SELECT t_800_93.id, t_800_93.sindex, t_800_93.year, t_800_93.month, t_800_93.num_of_month, t_800_93.height_snow_west, t_800_93.density_snow_west, t_800_93.height_snow_north, t_800_93.density_snow_north, t_800_93.height_snow_east, t_800_93.density_snow_east, t_800_93.height_snow_south, t_800_93.density_snow_south, t_800_93.field_of_west, t_800_93.field_of_north, t_800_93.field_of_east, t_800_93.field_of_south, t_800_93.update_time " +
                "station2.lat,station2.lon" +
                "from t_800_93 inner join station2 on t_800_93.sindex = station2.sindex";
            NpgsqlCommand cmd = conn.RunCmdPG(sql);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);

            DataSet data = new DataSet();

            da.Fill(data);
            conn.ClosePG();

            string JsonString = string.Empty;
            JsonString = JsonConvert.SerializeObject(ConstructModel(DataTableToList(data.Tables[0])));


            return JsonString;

        }
        public Rootobject ConstructModel(List<SnowDirectionModel> temp)
        {
            Rootobject rootobject = new Rootobject();
            rootobject.type = "Feature";

            rootobject.features = gen_feature(temp.Count, temp);

            return rootobject;
        }
        public List<Feature> gen_feature(int count, List<SnowDirectionModel> temp)
        {

            List<Feature> list_feat = new List<Feature>();

            for (int i = 0; i < count; i++)
            {
                list_feat.Add(new Feature
                {
                    type = "Point",
                    properties = gen_properties(i, temp),
                    geometry = gen_geometry(i, temp)
                });
            }


            return list_feat;
        }
        public Properties gen_properties(int count, List<SnowDirectionModel> temps)
        {
            //SnowDirectionModel temp = new SnowDirectionModel();
            Properties prop = new Properties();
            //prop.sid = temps[count].sid;
            prop.month = temps[count].month;
            prop.year = temps[count].year;
            prop.sindex = temps[count].sindex;
            prop.density_snow_north = temps[count].density_snow_north;
            prop.density_snow_west = temps[count].density_snow_west;
            prop.height_snow_east = temps[count].height_snow_east;
            prop.height_snow_north = temps[count].height_snow_north;
            prop.height_snow_west = temps[count].height_snow_west;
            prop.field_of_east = temps[count].field_of_east;
            prop.field_of_north = temps[count].field_of_north;
            prop.field_of_south = temps[count].field_of_south;
            prop.field_of_west = temps[count].field_of_west;
            //prop.num_of_month = temps[count].num_of_month;
            prop.lat = temps[count].lat;
            prop.lon = temps[count].lon;

            return prop;
        }
        public Geometry gen_geometry(int count, List<SnowDirectionModel> temps)
        {
            Geometry geometry = new Geometry();
            geometry.type = "Point";


            float lat = float.Parse(temps[count].lat);
            float lon = float.Parse(temps[count].lon);

            geometry.coordinates = new float[][]
            {
                new float[] {lat},
                new float[] {lon}
            };
            //geometry.coordinates = "[" + temps[count].lat + "," + temps[count].lon + "]";
            return geometry;
        }
        public List<SnowDirectionModel> DataTableToList(DataTable table)
        {
            //SELECT id, sindex, "year", "month", num_of_month, height_snow_west, density_snow_west, 
            //height_snow_north, density_snow_north, height_snow_east, density_snow_east, height_snow_south, 
            //density_snow_south, field_of_west, field_of_north, field_of_east, field_of_south, update_time
            //FROM t_800_93;
            //table to List object
            SnowDirectionModel temp = new SnowDirectionModel();

            var convertedList = (from rw in table.AsEnumerable()
                                 select new SnowDirectionModel()
                                 {
                                     sid = Convert.ToString(rw["id"]),
                                     sindex = Convert.ToInt32(rw["sindex"]),
                                     year = Convert.ToString(rw["year"]),
                                     month = Convert.ToString(rw["month"]),
                                     num_of_month = Convert.ToString(rw["num_of_month"]),
                                     height_snow_west = Convert.ToString(rw["height_snow_west"]),
                                     density_snow_west = Convert.ToString(rw["density_snow_west"]),
                                     height_snow_north = Convert.ToString(rw["height_snow_north"]),
                                     density_snow_north = Convert.ToString(rw["density_snow_north"]),
                                     height_snow_east = Convert.ToString(rw["height_snow_east"]),
                                     height_snow_south = Convert.ToString(rw["height_snow_south"]),
                                     field_of_east = Convert.ToString(rw["field_of_east"]),
                                     field_of_north = Convert.ToString(rw["field_of_north"]),
                                     field_of_south = Convert.ToString(rw["field_of_south"]),
                                     field_of_west = Convert.ToString(rw["field_of_west"]),
                                     lat = Convert.ToString(rw["lat"]),
                                     lon = Convert.ToString(rw["lon"])
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

            string sql = "SELECT t_800_93.id, t_800_93.sindex, t_800_93.year, t_800_93.month, t_800_93.num_of_month, t_800_93.height_snow_west, t_800_93.density_snow_west, t_800_93.height_snow_north, t_800_93.density_snow_north, t_800_93.height_snow_east, t_800_93.density_snow_east, t_800_93.height_snow_south, t_800_93.density_snow_south, t_800_93.field_of_west, t_800_93.field_of_north, t_800_93.field_of_east, t_800_93.field_of_south, t_800_93.update_time " +
                ",station2.lat,station2.lon " +
                "from t_800_93 inner join station2 on t_800_93.sindex = station2.sindex where t_800_93.year = " + year + " and t_800_93.month = " + month + " and t_800_93.num_of_month = " + num_of_month + "";

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

            string sql = "SELECT t_800_93.id, t_800_93.sindex, t_800_93.year, t_800_93.month, t_800_93.num_of_month, t_800_93.height_snow_west, t_800_93.density_snow_west, t_800_93.height_snow_north, t_800_93.density_snow_north, t_800_93.height_snow_east, t_800_93.density_snow_east, t_800_93.height_snow_south, t_800_93.density_snow_south, t_800_93.field_of_west, t_800_93.field_of_north, t_800_93.field_of_east, t_800_93.field_of_south, t_800_93.update_time " +
                ",station2.lat,station2.lon" +
                " from t_800_93 inner join station2 on t_800_93.sindex = station2.sindex where t_800_93.year = " + year + " and t_800_93.month = " + month + " and t_800_93.num_of_month = " + num_of_month + " and t_800_93.sindex =" + station + "";

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

            string sql = "SELECT t_800_93.id, t_800_93.sindex, t_800_93.year, t_800_93.month, t_800_93.num_of_month, t_800_93.height_snow_west, t_800_93.density_snow_west, t_800_93.height_snow_north, t_800_93.density_snow_north, t_800_93.height_snow_east, t_800_93.density_snow_east, t_800_93.height_snow_south, t_800_93.density_snow_south, t_800_93.field_of_west, t_800_93.field_of_north, t_800_93.field_of_east, t_800_93.field_of_south, t_800_93.update_time " +
                ",station2.lat,station2.lon" +
                " from t_800_93 inner join station2 on t_800_93.sindex = station2.sindex where t_800_93.station = " + station + "";

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
