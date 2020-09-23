using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using irimhe.Models;
using Microsoft.AspNetCore.Cors;

namespace irimhe.Controllers
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SnowController:Controller
    {
        [HttpGet]
        public string Index(string beginDateTime,string endDateTime)
        {
            /*
             * SnowModel snow = new SnowModel();
            return snow.PullDataPG();
            */
            SnowModel snow = new SnowModel();
            string ret = "";

            var begindate = snow.DateTimeTenDay(beginDateTime);
            var enddate = snow.DateTimeTenDay(endDateTime);
            string height_of_snow = null;
            string density_of_snow = null;
            ret = snow.only_pull_snow(height_of_snow, density_of_snow, begindate, enddate);

            return ret;

        }
        [HttpPost]
        public string irimhe_snow(DateTime dateTime,string station)
        {
            SnowModel snow = new SnowModel();
            string ret = "";

            if (string.IsNullOrEmpty(station))
            {
                var ten = snow.DateTimeTotenDay(dateTime);
                ret = snow.PullDataPGDB_station(ten);
            }
            else
            {
                var ten = snow.DateTimeTotenDay(dateTime);
                ret = snow.PullData_station(station,ten);
            }

            return ret;
        }
        //
        [HttpGet]
        public string snow(string height_of_snow,string density_of_snow,string beginDateTime,string endDateTime)
        {
            SnowModel snow = new SnowModel();
            string ret = "";

            var begindate = snow.DateTimeTenDay(beginDateTime);
            var enddate = snow.DateTimeTenDay(endDateTime);

            ret = snow.only_pull_snow(height_of_snow,density_of_snow,begindate,enddate);

            return ret;
        }
        [HttpPost]
        public string irimhe_snow_date(DateTime beginDateTime, DateTime endDateTime)
        {
            //
            SnowModel snow = new SnowModel();
            string ret = "";

            var begindate = snow.DateTimeTotenDay(beginDateTime);
            var enddate = snow.DateTimeTotenDay(endDateTime);

            ret = snow.PullData_begin_end(begindate, enddate);

            return ret;
        }
        [HttpPost]
        public string multi_station([System.Web.Http.FromBody]string[] sindex)
        {
            string ret = "";

            SnowModel snow = new SnowModel();

            if (sindex.Length != 0)
            {
                ret = snow.multi_station(sindex);
            }
            else
            {
                ret = "error";
            }

            return ret;
        }

    }
}
